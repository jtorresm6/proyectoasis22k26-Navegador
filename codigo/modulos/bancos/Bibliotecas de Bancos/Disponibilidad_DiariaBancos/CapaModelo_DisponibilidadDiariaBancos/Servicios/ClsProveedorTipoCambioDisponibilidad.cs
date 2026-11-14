using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CapaModelo_DisponibilidadDiariaBancos.Entidades;
using CapaModelo_TipoCambio.Servicios;

namespace CapaModelo_DisponibilidadDiariaBancos.Servicios
{
    public interface IProveedorTipoCambioDisponibilidad
    {
        Task<ClsCotizacionDisponibilidad> BancosMetObtenerTasaAsync(
            string codigoMoneda, DateTime fecha, CancellationToken token);
    }

    // Adapta el servicio compartido de Banguat al corte de disponibilidad.
    // No consulta saldos bancarios ni modifica la fuente documental del saldo.
    public class ClsProveedorTipoCambioDisponibilidad : IProveedorTipoCambioDisponibilidad
    {
        private const decimal TasaMaxima = 99999999.9999m;
        private readonly ClsServicioBanguat _servicio;

        public ClsProveedorTipoCambioDisponibilidad() : this(new ClsServicioBanguat()) { }

        public ClsProveedorTipoCambioDisponibilidad(ClsServicioBanguat servicio)
        {
            _servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        public async Task<ClsCotizacionDisponibilidad> BancosMetObtenerTasaAsync(
            string codigoMoneda, DateTime fecha, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            string codigo = NormalizarMoneda(codigoMoneda);
            fecha = fecha.Date;
            if (fecha > DateTime.Today)
                throw new ArgumentException("No se pueden consultar tipos de cambio futuros.");
            if (fecha < new DateTime(1000, 1, 1))
                throw new ArgumentException("La fecha está fuera del rango de MySQL.");

            if (codigo == "GTQ")
            {
                return new ClsCotizacionDisponibilidad
                {
                    Moneda = codigo, Fecha = fecha,
                    TasaOriginal = 1m, TasaAplicada = 1m,
                    Fuente = "Identidad matemática", Metodo = "IDENTIDAD"
                };
            }

            var catalogo = await _servicio.BancosMetObtenerMonedasAsync(token)
                .ConfigureAwait(false);
            var moneda = catalogo.SingleOrDefault(m =>
                string.Equals(m.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
            if (moneda == null)
                throw new InvalidOperationException(
                    "La moneda " + codigo + " no está disponible en el servicio de Banguat.");

            // Banguat no publica todos los días (fines de semana y feriados).
            // Para un corte contable se conserva la última referencia oficial
            // disponible, nunca una tasa inventada ni una cotización futura.
            DateTime desde = fecha.AddDays(-10);
            if (desde < new DateTime(1000, 1, 1)) desde = new DateTime(1000, 1, 1);
            var tasas = await _servicio.BancosMetObtenerTasasEnGtqAsync(
                desde, fecha, new[] { moneda }, token).ConfigureAwait(false);
            token.ThrowIfCancellationRequested();
            var tasa = tasas == null ? null : tasas
                .Where(t => t != null && t.Tasa > 0m &&
                    !string.Equals(t.Estado, "Sin tasa", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(t => t.FechaTasa)
                .FirstOrDefault();
            if (tasa == null)
                throw new InvalidOperationException(
                    "Banguat no publicó una cotización para " + codigo + " en los diez días anteriores al " +
                    fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ".");
            if (tasa.FechaTasa.Date > fecha ||
                !string.Equals(tasa.CodigoMoneda, codigo, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(tasa.MonedaOrigen, codigo, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(tasa.MonedaDestino, "GTQ", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    "La cotización recibida no corresponde a la fecha y moneda seleccionadas.");

            // El esquema del corte conserva cuatro decimales. El mismo valor
            // visible se aplica al cálculo y al guardado, sin tasas ocultas.
            decimal aplicada = Math.Round(tasa.Tasa, 4, MidpointRounding.AwayFromZero);
            if (aplicada <= 0m || aplicada > TasaMaxima)
                throw new InvalidOperationException(
                    "La cotización de Banguat no se puede representar con la precisión del corte diario.");

            return new ClsCotizacionDisponibilidad
            {
                Moneda = codigo,
                Fecha = tasa.FechaTasa.Date,
                TasaOriginal = tasa.Tasa,
                TasaAplicada = aplicada,
                Fuente = tasa.FechaTasa.Date == fecha ? "Banco de Guatemala" :
                    "Banco de Guatemala - última publicación",
                Metodo = tasa.FechaTasa.Date == fecha ? tasa.Metodo :
                    tasa.Metodo + "; ÚLTIMA PUBLICACIÓN"
            };
        }

        private static string NormalizarMoneda(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Selecciona la moneda de la cuenta.");
            codigo = codigo.Trim().ToUpperInvariant();
            if (codigo.Length != 3 || codigo.Any(c => c < 'A' || c > 'Z'))
                throw new ArgumentException("Selecciona un código de moneda válido.");
            return codigo;
        }
    }
}
