using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CapaModelo_TipoCambio.Entidades
{
    // Correspondencias de identificadores y unidades; no contiene cotizaciones.
    public static class ClsCatalogoMonedasBanguat
    {
        private static readonly IReadOnlyDictionary<int, string> Codigos =
            new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
            {
                { 1, "GTQ" }, { 2, "USD" }, { 3, "JPY" }, { 4, "BEF" },
                { 5, "CHF" }, { 6, "FRF" }, { 7, "CAD" }, { 8, "ITL" },
                { 9, "GBP" }, { 11, "DEM" }, { 12, "ESP" }, { 13, "ATS" },
                { 14, "NLG" }, { 15, "SEK" }, { 16, "CRC" }, { 17, "SVC" },
                { 18, "MXN" }, { 19, "HNL" }, { 21, "NIO" }, { 22, "VEB" },
                { 23, "DKK" }, { 24, "EUR" }, { 25, "NOK" }, { 26, "XDR" },
                { 27, "PTE" }, { 28, "BID" }, { 29, "ARS" }, { 30, "BRL" },
                { 31, "KRW" }, { 32, "HKD" }, { 33, "TWD" }, { 34, "CNY" },
                { 35, "PKR" }, { 36, "INR" }, { 37, "VEF" }, { 38, "COP" },
                { 39, "DOP" }, { 40, "MYR" }, { 41, "VES" }, { 42, "PLN" }
            });
        private static readonly HashSet<string> Historicas = new HashSet<string>(StringComparer.Ordinal)
            { "BEF", "FRF", "ITL", "DEM", "ESP", "ATS", "NLG", "VEB", "PTE", "VEF" };

        public static IReadOnlyDictionary<int, string> BancosCodigosPorId { get { return Codigos; } }

        public static long? BancosMetIdDeCodigo(string codigo)
        {
            codigo = (codigo ?? "").Trim().ToUpperInvariant();
            foreach (var entrada in Codigos)
                if (entrada.Value == codigo) return entrada.Key;
            return null;
        }

        public static string BancosMetClase(string codigo)
        {
            codigo = (codigo ?? "").Trim().ToUpperInvariant();
            if (codigo == "XDR" || codigo == "BID") return "Unidad de cuenta";
            return Historicas.Contains(codigo) ? "Histórica" : "Moneda";
        }
    }
}
