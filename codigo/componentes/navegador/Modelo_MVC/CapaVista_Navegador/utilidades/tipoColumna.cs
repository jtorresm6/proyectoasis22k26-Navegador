using System.Linq;
using CapaEntidades_Navegador;

namespace CapaVista_Navegador
{
    // Decide si una columna se dibuja como fecha, checkbox o campo numerico
    public static class tipoColumna
    {
        public static bool EsFecha(columnaInfo col)
        {
            string net = (col.TipoNet ?? "").ToLowerInvariant();

            if (net == "datetime" || net == "date" || net == "timespan")
                return true;

            string t = (col.TipoDato ?? "").ToLowerInvariant();
            return t.Contains("date") || t.Contains("time") || t.Contains("timestamp");
        }

        // MySQL guarda BOOLEAN como tinyint(1), por eso se revisa el texto real de la columna
        public static bool EsBooleano(columnaInfo col)
        {
            string net = (col.TipoNet ?? "").ToLowerInvariant();

            if (net == "boolean")
                return true;

            string columnaTexto = (col.TipoColumnaTexto ?? "").ToLowerInvariant().Replace(" ", "");

            if (columnaTexto.Contains("tinyint(1)") || columnaTexto == "bool" || columnaTexto == "boolean")
                return true;

            string t = (col.TipoDato ?? "").ToLowerInvariant();

            if (t == "bit" || t == "boolean" || t == "bool")
                return true;

            if ((t == "tinyint" || t.Contains("tinyint")) && col.TamanoColumna == 1)
                return true;

            return false;
        }

        public static bool EsNumerico(columnaInfo col)
        {
            string net = (col.TipoNet ?? "").ToLowerInvariant();
            string[] netNumericos = { "int16", "int32", "int64", "byte", "sbyte", "decimal", "double", "single" };

            if (netNumericos.Contains(net))
                return true;

            string t = (col.TipoDato ?? "").ToLowerInvariant();

            switch (t)
            {
                case "int":
                case "integer":
                case "smallint":
                case "bigint":
                case "tinyint":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                case "real":
                case "counter":
                case "number":
                    return true;

                default:
                    return false;
            }
        }
    }
}