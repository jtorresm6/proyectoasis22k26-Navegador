using System;
using System.Collections.Generic;
using CapaModelo_TipoCambio.Entidades;

namespace CapaModelo_TipoCambio.Repositorio
{
    public class ClsRepositorioTipoCambio
    {
        // ── Catálogo de tasas de referencia actuales (demo según prototipo) ──
        private static readonly List<ClsTipoCambio> _tasasActuales =
            new List<ClsTipoCambio>
            {
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.753200m,
                    TasaTexto     = "7.753200",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "EUR",
                    NombreMoneda  = "Euro",
                    MonedaOrigen  = "EUR",
                    MonedaDestino = "GTQ",
                    Tasa          = 8.421500m,
                    TasaTexto     = "8.421500",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "JPY",
                    NombreMoneda  = "Yen japonés",
                    MonedaOrigen  = "JPY",
                    MonedaDestino = "GTQ",
                    Tasa          = 0.051000m,
                    TasaTexto     = "0.051000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "CHF",
                    NombreMoneda  = "Franco suizo",
                    MonedaOrigen  = "CHF",
                    MonedaDestino = "GTQ",
                    Tasa          = 8.960000m,
                    TasaTexto     = "8.960000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "CAD",
                    NombreMoneda  = "Dólar canadiense",
                    MonedaOrigen  = "CAD",
                    MonedaDestino = "GTQ",
                    Tasa          = 5.678000m,
                    TasaTexto     = "5.678000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "GBP",
                    NombreMoneda  = "Libra esterlina",
                    MonedaOrigen  = "GBP",
                    MonedaDestino = "GTQ",
                    Tasa          = 10.120000m,
                    TasaTexto     = "10.120000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "MXN",
                    NombreMoneda  = "Peso mexicano",
                    MonedaOrigen  = "MXN",
                    MonedaDestino = "GTQ",
                    Tasa          = 0.421500m,
                    TasaTexto     = "0.421500",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "CNY",
                    NombreMoneda  = "Yuan chino",
                    MonedaOrigen  = "CNY",
                    MonedaDestino = "GTQ",
                    Tasa          = 1.088000m,
                    TasaTexto     = "1.088000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "VEF",
                    NombreMoneda  = "Bolívar fuerte",
                    MonedaOrigen  = "VEF",
                    MonedaDestino = "GTQ",
                    Tasa          = 0m,
                    TasaTexto     = "—",
                    Fuente        = "Demostración",
                    Estado        = "Sin tasa"
                }
            };

        // ── Historial demo (09/10/2026 al 13/10/2026) ──
        private static readonly List<ClsTipoCambio> _historico =
            new List<ClsTipoCambio>
            {
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 13),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.753200m,
                    TasaTexto     = "7.753200",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 12),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.751000m,
                    TasaTexto     = "7.751000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 11),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.749500m,
                    TasaTexto     = "7.749500",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 10),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.748000m,
                    TasaTexto     = "7.748000",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                },
                new ClsTipoCambio
                {
                    FechaTasa     = new DateTime(2026, 10, 9),
                    CodigoMoneda  = "USD",
                    NombreMoneda  = "Dólar estadounidense",
                    MonedaOrigen  = "USD",
                    MonedaDestino = "GTQ",
                    Tasa          = 7.747200m,
                    TasaTexto     = "7.747200",
                    Fuente        = "Demostración",
                    Estado        = "Disponible"
                }
            };

        public List<ClsTipoCambio> BancosMetObtenerTasasActuales(bool incluirSinTasa = true)
        {
            if (incluirSinTasa)
                return new List<ClsTipoCambio>(_tasasActuales);

            return _tasasActuales.FindAll(t => t.Estado != "Sin tasa");
        }

        public ClsTipoCambio BancosMetObtenerTasaActual(string monedaOrigen, string monedaDestino)
        {
            if (monedaOrigen == "EUR" && monedaDestino == "USD")
            {
                return new ClsTipoCambio
                {
                    FechaTasa = new DateTime(2026, 10, 13),
                    CodigoMoneda = "EUR",
                    NombreMoneda = "Euro",
                    MonedaOrigen = "EUR",
                    MonedaDestino = "USD",
                    Tasa = 1.086200m,
                    TasaTexto = "1.086200",
                    Fuente = "Demostración",
                    Estado = "Disponible"
                };
            }

            var encontrada = _tasasActuales.Find(
                t => (t.MonedaOrigen == monedaOrigen && t.MonedaDestino == monedaDestino) ||
                     (t.CodigoMoneda == monedaOrigen && monedaDestino == "GTQ"));

            return encontrada;
        }

        public List<ClsTipoCambio> BancosMetObtenerHistorico(
            DateTime desde,
            DateTime hasta,
            string monedaOrigen,
            string monedaDestino)
        {
            return _historico.FindAll(t =>
                t.FechaTasa >= desde &&
                t.FechaTasa <= hasta &&
                (string.IsNullOrEmpty(monedaOrigen)  || monedaOrigen  == "Todas" || t.MonedaOrigen  == monedaOrigen) &&
                (string.IsNullOrEmpty(monedaDestino) || monedaDestino == "Todas" || t.MonedaDestino == monedaDestino));
        }

        public (decimal Minima, decimal Maxima, decimal Variacion) BancosMetObtenerResumenHistorico()
        {
            decimal min = 7.747200m;
            decimal max = 7.753200m;
            decimal var = max - min;
            return (min, max, var);
        }
    }
}
