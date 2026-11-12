using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CapaModelo_TipoCambio.Entidades;

namespace CapaModelo_TipoCambio.Servicios
{
    // Toda cotizacion se obtiene del servicio. El catalogo contiene solamente
    // identificadores de monedas y reglas de unidades, nunca tasas.
    public class ClsServicioBanguat
    {
        private const string UrlServicio =
            "https://www.banguat.gob.gt/variables/ws/TipoCambio.asmx";
        private static readonly XNamespace Ns =
            "http://www.banguat.gob.gt/variables/ws/";
        private static readonly XNamespace Soap =
            "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly HttpClient ClienteCompartido = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(45)
        };
        private static readonly SemaphoreSlim LimiteSolicitudes =
            new SemaphoreSlim(4, 4);
        private readonly HttpClient _cliente;

        public ClsServicioBanguat() : this(ClienteCompartido) { }

        // Permite probar el transporte con HttpMessageHandler sin usar internet.
        // Quien inyecta el cliente conserva la responsabilidad de disponerlo.
        public ClsServicioBanguat(HttpClient cliente)
        {
            _cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
        }

        public async Task<List<ClsMoneda>> BancosMetObtenerMonedasAsync(
            CancellationToken token = default(CancellationToken))
        {
            XElement resultado = await EnviarAsync(
                "VariablesDisponibles", new XElement[0], token).ConfigureAwait(false);

            // Identificadores publicados por VariablesDisponibles.
            // Incluye monedas actuales, historicas y unidades de cuenta.
            // BID es un codigo interno para la unidad de cuenta del BID (28),
            // no un codigo ISO. Los demas codigos conservan su identidad ISO.
            // Las excepciones de unidad son GBP, EUR y XDR: USD por una unidad,
            // segun la nota de cotizaciones del sitio oficial de Banguat.
            var codigos = ClsCatalogoMonedasBanguat.BancosCodigosPorId;
            var monedas = new List<ClsMoneda>();
            var ids = new HashSet<int>();
            foreach (XElement variable in
                resultado.Element(Ns + "Variables")?.Elements(Ns + "Variable")
                ?? Enumerable.Empty<XElement>())
            {
                int id;
                string codigo;
                if (!int.TryParse((string)variable.Element(Ns + "moneda"),
                    NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
                    throw new InvalidOperationException("El catalogo de Banguat contiene un identificador invalido.");
                if (!codigos.TryGetValue(id, out codigo)) continue;
                string nombre = ((string)variable.Element(Ns + "descripcion") ?? "").Trim();
                if (nombre.Length == 0 || !ids.Add(id))
                    throw new InvalidOperationException("El catalogo de Banguat contiene monedas invalidas o duplicadas.");
                monedas.Add(new ClsMoneda
                {
                    IdBanguat = id,
                    Codigo = codigo,
                    Nombre = nombre,
                    DolaresPorUnidad = codigo == "EUR" || codigo == "GBP" || codigo == "XDR"
                });
            }
            if (!monedas.Any(m => m.Codigo == "USD") ||
                !monedas.Any(m => m.Codigo == "GTQ"))
                throw new InvalidOperationException("Banguat no devolvio el catalogo requerido de USD y GTQ.");
            return monedas.OrderBy(m => m.Codigo).ToList();
        }

        public async Task<List<ClsTipoCambio>> BancosMetObtenerTasasEnGtqAsync(
            DateTime desde, DateTime hasta, IEnumerable<ClsMoneda> monedas,
            CancellationToken token = default(CancellationToken))
        {
            desde = desde.Date;
            hasta = hasta.Date;
            if (desde > hasta)
                throw new ArgumentException("La fecha inicial debe ser menor o igual a la final.");
            if (hasta > DateTime.Today)
                throw new ArgumentException("No se pueden consultar cotizaciones futuras.");
            if (monedas == null) throw new ArgumentNullException(nameof(monedas));
            List<ClsMoneda> solicitadas = monedas.GroupBy(m => m.Codigo)
                .Select(g => g.First()).ToList();
            if (solicitadas.Count == 0) return new List<ClsTipoCambio>();

            // Los cruces siempre utilizan la referencia USD/GTQ de la MISMA fecha.
            var series = new Dictionary<int, Dictionary<DateTime, Cotizacion>>();
            if (solicitadas.Any(m => m.Codigo != "GTQ"))
            {
                series[2] = await ObtenerSerieAsync(2, desde, hasta, token)
                    .ConfigureAwait(false);
                var consultas = solicitadas
                    .Where(m => m.Codigo != "GTQ" && m.Codigo != "USD")
                    .Select(async moneda => new
                    {
                        moneda.IdBanguat,
                        Serie = await ObtenerSerieAsync(moneda.IdBanguat, desde, hasta, token)
                            .ConfigureAwait(false)
                    }).ToArray();
                var resultados = await Task.WhenAll(consultas).ConfigureAwait(false);
                foreach (var resultado in resultados)
                    series[resultado.IdBanguat] = resultado.Serie;
            }

            var tasas = new List<ClsTipoCambio>();
            for (DateTime fecha = desde; fecha <= hasta; fecha = fecha.AddDays(1))
            {
                token.ThrowIfCancellationRequested();
                foreach (ClsMoneda moneda in solicitadas)
                {
                    decimal tasa = 0m;
                    string metodo = "SIN PUBLICACION";
                    if (moneda.Codigo == "GTQ")
                    {
                        tasa = 1m; // Identidad matematica; no es una cotizacion.
                        metodo = "IDENTIDAD";
                    }
                    else
                    {
                        Cotizacion dolar;
                        if (series[2].TryGetValue(fecha, out dolar))
                        {
                            if (moneda.Codigo == "USD")
                            {
                                tasa = dolar.Promedio;
                                metodo = dolar.Compra == dolar.Venta
                                    ? "DIRECTA" : "PROMEDIO HISTORICO";
                            }
                            else
                            {
                                Cotizacion original;
                                if (series[moneda.IdBanguat].TryGetValue(fecha, out original))
                                {
                                    tasa = moneda.DolaresPorUnidad
                                        ? dolar.Promedio * original.Promedio
                                        : dolar.Promedio / original.Promedio;
                                    metodo = "CRUZADA (promedio)";
                                }
                            }
                        }
                    }
                    tasas.Add(new ClsTipoCambio
                    {
                        FechaTasa = fecha,
                        CodigoMoneda = moneda.Codigo,
                        NombreMoneda = moneda.Nombre,
                        MonedaOrigen = moneda.Codigo,
                        MonedaDestino = "GTQ",
                        Tasa = tasa,
                        TasaTexto = tasa > 0m
                            ? tasa.ToString("F4", CultureInfo.InvariantCulture) : "—",
                        Fuente = moneda.Codigo == "GTQ"
                            ? "Identidad matematica" : "Banco de Guatemala",
                        Estado = tasa > 0m ? "Disponible" : "Sin tasa",
                        Metodo = metodo
                    });
                }
                // Evita desbordamiento al incrementar DateTime.MaxValue.
                if (fecha == hasta) break;
            }
            return tasas;
        }

        public async Task<ClsTipoCambio> BancosMetObtenerTipoCambioDiaAsync(
            CancellationToken token = default(CancellationToken))
        {
            List<ClsMoneda> monedas = await BancosMetObtenerMonedasAsync(token)
                .ConfigureAwait(false);
            List<ClsTipoCambio> tasas = await BancosMetObtenerTasasEnGtqAsync(
                DateTime.Today, DateTime.Today, monedas.Where(m => m.Codigo == "USD"), token)
                .ConfigureAwait(false);
            ClsTipoCambio tasa = tasas.FirstOrDefault(t => t.Tasa > 0m);
            if (tasa == null)
                throw new InvalidOperationException("Banguat no ha publicado una referencia USD/GTQ para hoy.");
            return tasa;
        }

        private async Task<Dictionary<DateTime, Cotizacion>> ObtenerSerieAsync(
            int idMoneda, DateTime desde, DateTime hasta, CancellationToken token)
        {
            var serie = new Dictionary<DateTime, Cotizacion>();
            // Evita respuestas enormes al consultar varios anos.
            for (DateTime inicio = desde; inicio <= hasta;)
            {
                DateTime fin = (hasta - inicio).TotalDays > 365
                    ? inicio.AddDays(365) : hasta;
                string operacion = idMoneda == 2
                    ? "TipoCambioRango" : "TipoCambioRangoMoneda";
                var parametros = new List<XElement>
                {
                    new XElement(Ns + "fechainit", inicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new XElement(Ns + "fechafin", fin.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
                };
                if (idMoneda != 2)
                    parametros.Add(new XElement(Ns + "moneda", idMoneda));

                XElement resultado = await EnviarAsync(operacion, parametros, token)
                    .ConfigureAwait(false);
                XElement vars = resultado.Element(Ns + "Vars");
                List<XElement> filas = vars?.Elements(Ns + "Var").ToList()
                    ?? new List<XElement>();
                int total;
                if (!int.TryParse((string)resultado.Element(Ns + "TotalItems"),
                    NumberStyles.Integer, CultureInfo.InvariantCulture, out total)
                    || total < 0 || total != filas.Count)
                    throw new InvalidOperationException("Banguat devolvio una serie incompleta o sin la estructura esperada.");
                foreach (XElement fila in filas)
                {
                    DateTime fecha;
                    int moneda;
                    if (!DateTime.TryParseExact((string)fila.Element(Ns + "fecha"),
                        new[] { "dd/MM/yyyy", "d/M/yyyy" }, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out fecha) ||
                        !int.TryParse((string)fila.Element(Ns + "moneda"), out moneda) ||
                        moneda != idMoneda || fecha < inicio || fecha > fin)
                        throw new InvalidOperationException("Banguat devolvio una fecha o moneda inesperada.");
                    decimal compra;
                    decimal venta;
                    if (!decimal.TryParse((string)fila.Element(Ns + "compra"),
                        NumberStyles.Float, CultureInfo.InvariantCulture, out compra) ||
                        !decimal.TryParse((string)fila.Element(Ns + "venta"),
                        NumberStyles.Float, CultureInfo.InvariantCulture, out venta) ||
                        compra < 0m || venta < 0m)
                        throw new InvalidOperationException("Banguat devolvio una cotizacion invalida.");
                    if (compra == 0m || venta == 0m) continue;
                    Cotizacion existente;
                    if (serie.TryGetValue(fecha, out existente) &&
                        (existente.Compra != compra || existente.Venta != venta))
                        throw new InvalidOperationException("Banguat devolvio cotizaciones contradictorias para la misma fecha.");
                    serie[fecha] = new Cotizacion { Compra = compra, Venta = venta };
                }
                if (fin == hasta) break;
                inicio = fin.AddDays(1);
            }
            return serie;
        }

        private async Task<XElement> EnviarAsync(
            string operacion, IEnumerable<XElement> parametros, CancellationToken token)
        {
            var envelope = new XElement(Soap + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soap", Soap.NamespaceName),
                new XElement(Soap + "Body", new XElement(Ns + operacion, parametros)));
            await LimiteSolicitudes.WaitAsync(token).ConfigureAwait(false);
            try
            {
                using (var peticion = new HttpRequestMessage(HttpMethod.Post, UrlServicio))
                {
                    peticion.Headers.Add("SOAPAction", "\"" + Ns.NamespaceName + operacion + "\"");
                    peticion.Content = new StringContent(
                        envelope.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml");
                    using (HttpResponseMessage respuesta = await _cliente.SendAsync(
                        peticion, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                    {
                        string contenido = await respuesta.Content.ReadAsStringAsync().ConfigureAwait(false);
                        token.ThrowIfCancellationRequested();
                        XDocument documento;
                        try
                        {
                            var opciones = new XmlReaderSettings
                            {
                                DtdProcessing = DtdProcessing.Prohibit,
                                XmlResolver = null,
                                MaxCharactersInDocument = 10000000
                            };
                            using (var texto = new StringReader(contenido))
                            using (XmlReader lector = XmlReader.Create(texto, opciones))
                                documento = XDocument.Load(lector);
                        }
                        catch (XmlException ex)
                        {
                            throw new InvalidOperationException(
                                "Banguat no devolvio XML valido (HTTP " +
                                (int)respuesta.StatusCode + "). Intente consultar de nuevo.", ex);
                        }
                        XElement cuerpo = documento.Root?.Element(Soap + "Body");
                        XElement fallo = cuerpo?.Element(Soap + "Fault");
                        if (fallo != null)
                            throw new InvalidOperationException(
                                "Banguat rechazo la consulta: " +
                                ((string)fallo.Element("faultstring") ?? "Error SOAP."));
                        respuesta.EnsureSuccessStatusCode();
                        XElement resultado = cuerpo?.Element(Ns + operacion + "Response")
                            ?.Element(Ns + operacion + "Result");
                        if (resultado == null)
                            throw new InvalidOperationException("La respuesta de Banguat no contiene el resultado esperado.");
                        return resultado;
                    }
                }
            }
            catch (OperationCanceledException ex) when (!token.IsCancellationRequested)
            {
                throw new TimeoutException(
                    "Banguat no respondio dentro del tiempo de espera. Intente consultar de nuevo.", ex);
            }
            finally
            {
                LimiteSolicitudes.Release();
            }
        }

        private sealed class Cotizacion
        {
            public decimal Compra { get; set; }
            public decimal Venta { get; set; }
            public decimal Promedio { get { return (Compra + Venta) / 2m; } }
        }
    }
}
