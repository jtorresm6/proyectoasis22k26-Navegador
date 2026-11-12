using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Servicios;

namespace CapaModelo_TipoCambio.Repositorio
{
    // Guarda únicamente respuestas reales en memoria durante esta sesión.
    // Ninguna consulta sustituye una fecha faltante por la de otro día.
    public class ClsRepositorioTipoCambio
    {
        private readonly ClsServicioBanguat _servicio;
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);
        private readonly Dictionary<string, EntradaCache> _cache =
            new Dictionary<string, EntradaCache>(StringComparer.Ordinal);
        private List<ClsMoneda> _catalogo;
        private DateTime _fechaCatalogoUtc;

        public ClsRepositorioTipoCambio() : this(new ClsServicioBanguat()) { }

        public ClsRepositorioTipoCambio(ClsServicioBanguat servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        public async Task BancosMetInvalidarCacheAsync(CancellationToken cancellationToken)
        {
            await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _cache.Clear();
            }
            finally { _mutex.Release(); }
        }

        public async Task<List<ClsMoneda>> BancosMetObtenerMonedasAsync(
            CancellationToken cancellationToken)
        {
            await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var catalogo = await ObtenerCatalogoInternoAsync(cancellationToken)
                    .ConfigureAwait(false);
                return catalogo.Select(CopiarMoneda).ToList();
            }
            finally { _mutex.Release(); }
        }

        public async Task<List<ClsTipoCambio>> BancosMetObtenerTasasActualesAsync(
            DateTime fecha, bool incluirSinTasa, bool forzar,
            CancellationToken cancellationToken)
        {
            ValidarFechas(fecha, fecha);
            var lista = await ObtenerTasasAsync(fecha.Date, fecha.Date, null,
                forzar, cancellationToken).ConfigureAwait(false);
            return lista.Where(t => incluirSinTasa || Disponible(t))
                .OrderBy(t => t.CodigoMoneda, StringComparer.Ordinal).ToList();
        }

        public async Task<List<ClsTipoCambio>> BancosMetObtenerHistoricoAsync(
            DateTime desde, DateTime hasta, string origen, string destino,
            CancellationToken cancellationToken)
        {
            ValidarFechas(desde, hasta);
            origen = NormalizarCodigo(origen);
            destino = NormalizarCodigo(destino);
            var catalogo = await BancosMetObtenerMonedasAsync(cancellationToken)
                .ConfigureAwait(false);
            var monedaOrigen = BuscarMoneda(catalogo, origen);
            BuscarMoneda(catalogo, destino);

            if (origen == destino)
            {
                var identidad = new List<ClsTipoCambio>();
                for (DateTime fecha = desde.Date; fecha <= hasta.Date; fecha = fecha.AddDays(1))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    identidad.Add(CrearIdentidad(fecha, monedaOrigen));
                }
                return identidad;
            }

            var lista = await ObtenerTasasAsync(desde.Date, hasta.Date,
                new[] { origen, destino }, false, cancellationToken).ConfigureAwait(false);
            var indice = lista.ToDictionary(t => Clave(t.FechaTasa, t.CodigoMoneda));
            var resultado = new List<ClsTipoCambio>();
            for (DateTime fecha = desde.Date; fecha <= hasta.Date; fecha = fecha.AddDays(1))
            {
                cancellationToken.ThrowIfCancellationRequested();
                ClsTipoCambio tasaOrigen;
                ClsTipoCambio tasaDestino;
                if (indice.TryGetValue(Clave(fecha, origen), out tasaOrigen) &&
                    indice.TryGetValue(Clave(fecha, destino), out tasaDestino) &&
                    Disponible(tasaOrigen) && Disponible(tasaDestino))
                {
                    resultado.Add(CrearPar(tasaOrigen, tasaDestino));
                }
            }
            return resultado;
        }

        public async Task<ClsResultadoConversion> BancosMetConvertirAsync(
            decimal importe, DateTime fecha, string origen, string destino,
            CancellationToken cancellationToken)
        {
            if (importe < 0)
                throw new ArgumentOutOfRangeException(nameof(importe), "El importe no puede ser negativo.");

            var tasas = await BancosMetObtenerHistoricoAsync(fecha, fecha,
                origen, destino, cancellationToken).ConfigureAwait(false);
            var tasa = tasas.SingleOrDefault();
            if (tasa == null)
                throw new InvalidOperationException(
                    $"No hay cotizaciones para {origen}/{destino} en la fecha exacta {fecha:dd/MM/yyyy}.");

            decimal tasaAplicada = Math.Round(tasa.Tasa, 4, MidpointRounding.AwayFromZero);
            if (tasaAplicada <= 0m)
                throw new InvalidOperationException("La tasa no se puede representar con los cuatro decimales del estándar.");

            return new ClsResultadoConversion
            {
                Resultado = Math.Round(checked(importe * tasaAplicada), 4,
                    MidpointRounding.AwayFromZero),
                Tasa = tasaAplicada,
                Fecha = tasa.FechaTasa,
                MonedaOrigen = tasa.MonedaOrigen,
                MonedaDestino = tasa.MonedaDestino,
                Fuente = tasa.Fuente,
                Metodo = tasa.Metodo
            };
        }

        private async Task<List<ClsMoneda>> ObtenerCatalogoInternoAsync(
            CancellationToken cancellationToken)
        {
            if (_catalogo == null || DateTime.UtcNow - _fechaCatalogoUtc > TimeSpan.FromHours(6))
            {
                var monedas = await _servicio.BancosMetObtenerMonedasAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (monedas == null || monedas.Count == 0 ||
                    monedas.Any(m => m == null || string.IsNullOrWhiteSpace(m.Codigo)) ||
                    monedas.Select(m => NormalizarCodigo(m.Codigo)).Distinct().Count() != monedas.Count)
                    throw new InvalidOperationException("El catálogo de monedas recibido no es válido.");

                if (!monedas.Any(m => NormalizarCodigo(m.Codigo) == "GTQ") ||
                    !monedas.Any(m => NormalizarCodigo(m.Codigo) == "USD"))
                    throw new InvalidOperationException("El catálogo debe contener GTQ y USD.");

                cancellationToken.ThrowIfCancellationRequested();
                _catalogo = monedas.Select(CopiarMoneda).ToList();
                _fechaCatalogoUtc = DateTime.UtcNow;
            }
            return _catalogo;
        }

        private async Task<List<ClsTipoCambio>> ObtenerTasasAsync(
            DateTime desde, DateTime hasta, IEnumerable<string> codigos, bool forzar,
            CancellationToken cancellationToken)
        {
            await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                var catalogo = await ObtenerCatalogoInternoAsync(cancellationToken)
                    .ConfigureAwait(false);
                var monedas = codigos == null ? catalogo : codigos
                    .Select(c => BuscarMoneda(catalogo, NormalizarCodigo(c)))
                    .GroupBy(m => m.Codigo).Select(g => g.First()).ToList();

                DateTime? primerFaltante = null;
                DateTime? ultimoFaltante = null;
                var codigosFaltantes = new HashSet<string>(StringComparer.Ordinal);
                for (DateTime fecha = desde; fecha <= hasta; fecha = fecha.AddDays(1))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var moneda in monedas)
                    {
                        EntradaCache entrada;
                        if (forzar || !_cache.TryGetValue(Clave(fecha, moneda.Codigo), out entrada) ||
                            !Vigente(entrada))
                        {
                            if (!primerFaltante.HasValue) primerFaltante = fecha;
                            ultimoFaltante = fecha;
                            codigosFaltantes.Add(moneda.Codigo);
                        }
                    }
                }

                if (primerFaltante.HasValue)
                {
                    var solicitadas = monedas.Where(m => codigosFaltantes.Contains(m.Codigo)).ToList();
                    var recibidas = await _servicio.BancosMetObtenerTasasEnGtqAsync(
                        primerFaltante.Value, ultimoFaltante.Value, solicitadas, cancellationToken)
                        .ConfigureAwait(false);
                    if (recibidas == null)
                        throw new InvalidOperationException("Banguat no devolvió una respuesta de tasas.");

                    var nuevas = new Dictionary<string, ClsTipoCambio>(StringComparer.Ordinal);
                    foreach (var tasa in recibidas)
                    {
                        if (tasa == null || tasa.FechaTasa.Date < primerFaltante.Value ||
                            tasa.FechaTasa.Date > ultimoFaltante.Value || tasa.Tasa < 0)
                            throw new InvalidOperationException("La respuesta de tasas contiene datos inválidos.");
                        string codigo = NormalizarCodigo(tasa.CodigoMoneda);
                        if (!codigosFaltantes.Contains(codigo)) continue;
                        string clave = Clave(tasa.FechaTasa, codigo);
                        if (nuevas.ContainsKey(clave))
                            throw new InvalidOperationException("Banguat devolvió más de una tasa para la misma moneda y fecha.");
                        var copia = CopiarTasa(tasa);
                        copia.CodigoMoneda = codigo;
                        copia.FechaTasa = copia.FechaTasa.Date;
                        nuevas.Add(clave, copia);
                    }

                    // Un día sin publicación se mantiene como faltante; nunca recibe
                    // la tasa del día anterior ni una cotización escrita en el código.
                    for (DateTime fecha = primerFaltante.Value; fecha <= ultimoFaltante.Value; fecha = fecha.AddDays(1))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        foreach (var moneda in solicitadas)
                        {
                            string clave = Clave(fecha, moneda.Codigo);
                            if (!nuevas.ContainsKey(clave))
                                nuevas.Add(clave, CrearSinTasa(fecha, moneda));
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    DateTime ahora = DateTime.UtcNow;
                    foreach (var nueva in nuevas)
                        _cache[nueva.Key] = new EntradaCache { Tasa = nueva.Value, ConsultadaUtc = ahora };
                }

                var resultado = new List<ClsTipoCambio>();
                for (DateTime fecha = desde; fecha <= hasta; fecha = fecha.AddDays(1))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var moneda in monedas)
                        resultado.Add(CopiarTasa(_cache[Clave(fecha, moneda.Codigo)].Tasa));
                }
                return resultado;
            }
            finally { _mutex.Release(); }
        }

        private static ClsTipoCambio CrearPar(ClsTipoCambio origen, ClsTipoCambio destino)
        {
            decimal valor = origen.Tasa / destino.Tasa;
            string metodo = destino.CodigoMoneda == "GTQ" ? origen.Metodo :
                origen.CodigoMoneda == "GTQ" && destino.CodigoMoneda == "USD" ?
                    (destino.Metodo == "DIRECTA" ? "INVERSA" :
                        "INVERSA (" + destino.Metodo + ")") :
                "CRUZADA (promedio)";
            return new ClsTipoCambio
            {
                FechaTasa = origen.FechaTasa,
                CodigoMoneda = origen.CodigoMoneda,
                NombreMoneda = origen.NombreMoneda,
                MonedaOrigen = origen.CodigoMoneda,
                MonedaDestino = destino.CodigoMoneda,
                Tasa = valor,
                TasaTexto = valor.ToString("F4", CultureInfo.InvariantCulture),
                Fuente = "Banco de Guatemala",
                Estado = "Disponible",
                Metodo = metodo
            };
        }

        private static ClsTipoCambio CrearIdentidad(DateTime fecha, ClsMoneda moneda)
        {
            var tasa = CrearSinTasa(fecha, moneda);
            tasa.MonedaDestino = moneda.Codigo;
            tasa.Tasa = 1m;
            tasa.TasaTexto = "1.0000";
            tasa.Fuente = "Identidad matemática";
            tasa.Estado = "Disponible";
            tasa.Metodo = "IDENTIDAD";
            return tasa;
        }

        private static ClsTipoCambio CrearSinTasa(DateTime fecha, ClsMoneda moneda)
        {
            return new ClsTipoCambio
            {
                FechaTasa = fecha.Date, CodigoMoneda = moneda.Codigo,
                NombreMoneda = moneda.Nombre, MonedaOrigen = moneda.Codigo,
                MonedaDestino = "GTQ", Tasa = 0m, TasaTexto = "—",
                Fuente = "Banco de Guatemala", Estado = "Sin tasa", Metodo = "Sin publicación"
            };
        }

        private static bool Disponible(ClsTipoCambio tasa)
        { return tasa.Tasa > 0 && tasa.Estado != "Sin tasa"; }

        private static bool Vigente(EntradaCache entrada)
        {
            TimeSpan duracion = entrada.Tasa.FechaTasa.Date == DateTime.Today ||
                !Disponible(entrada.Tasa) ? TimeSpan.FromMinutes(5) : TimeSpan.FromHours(24);
            return DateTime.UtcNow - entrada.ConsultadaUtc < duracion;
        }

        private static string Clave(DateTime fecha, string codigo)
        { return fecha.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ":" + NormalizarCodigo(codigo); }

        private static string NormalizarCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Selecciona una moneda.");
            return codigo.Trim().ToUpperInvariant();
        }

        private static ClsMoneda BuscarMoneda(IEnumerable<ClsMoneda> monedas, string codigo)
        {
            var moneda = monedas.SingleOrDefault(m => NormalizarCodigo(m.Codigo) == codigo);
            if (moneda == null)
                throw new ArgumentException("La moneda " + codigo + " no está disponible en el catálogo de Banguat.");
            return moneda;
        }

        private static void ValidarFechas(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException("La fecha inicial debe ser anterior o igual a la final.");
            if (hasta.Date > DateTime.Today)
                throw new ArgumentException("No se pueden consultar cotizaciones de fechas futuras.");
        }

        private static ClsMoneda CopiarMoneda(ClsMoneda moneda)
        {
            return new ClsMoneda { IdBanguat = moneda.IdBanguat,
                Codigo = NormalizarCodigo(moneda.Codigo), Nombre = moneda.Nombre,
                DolaresPorUnidad = moneda.DolaresPorUnidad };
        }

        private static ClsTipoCambio CopiarTasa(ClsTipoCambio tasa)
        {
            return new ClsTipoCambio { FechaTasa = tasa.FechaTasa,
                CodigoMoneda = tasa.CodigoMoneda, NombreMoneda = tasa.NombreMoneda,
                MonedaOrigen = tasa.MonedaOrigen, MonedaDestino = tasa.MonedaDestino,
                Tasa = tasa.Tasa, TasaTexto = tasa.TasaTexto, Fuente = tasa.Fuente,
                Estado = tasa.Estado, Metodo = tasa.Metodo };
        }

        private sealed class EntradaCache
        {
            public ClsTipoCambio Tasa { get; set; }
            public DateTime ConsultadaUtc { get; set; }
        }
    }
}
