using System.Linq;
using CapaModelo_Navegador;

namespace CapaVista_Navegador
{
    // Decide si una Columna y dibuja correspondiente si es fecha, checkbox o campo numerico
    // Diego Alejandro Cheng Peña 0901-22-8091 
    // Fecha actual : 14/09/2026
    public static class ClsTipoColumna
    {
        //Ve si el dato es tipo fecha, dibujar un DateTimePicker
        public static bool NavegadorFuncEsFecha(ClsColumnaInfo Col)
        {
            string TipoNet = (Col.TipoNet ?? "").ToLowerInvariant();

            if (TipoNet == "datetime" || TipoNet == "date" || TipoNet == "timespan")
                return true;

            string TipoDato = (Col.TipoDato ?? "").ToLowerInvariant();
            return TipoDato.Contains("date") || TipoDato.Contains("time") || TipoDato.Contains("timestamp");
        }

        //Si ve que es booleano, dibujar un checkbox para solo seleccionar las opciones posibles
        public static bool NavegadorFuncEsBooleano(ClsColumnaInfo Col)
        {
            string TipoNet = (Col.TipoNet ?? "").ToLowerInvariant();

            if (TipoNet == "boolean")
                return true;

            string ColumnaTexto = (Col.TipoColumnaTexto ?? "").ToLowerInvariant().Replace(" ", "");

            if (ColumnaTexto.Contains("tinyint(1)") || ColumnaTexto == "bool" || ColumnaTexto == "boolean")
                return true;

            string TipoDato = (Col.TipoDato ?? "").ToLowerInvariant();

            if (TipoDato == "bit" || TipoDato == "boolean" || TipoDato == "bool")
                return true;

            if ((TipoDato == "tinyint" || TipoDato.Contains("tinyint")) && Col.TamanoColumna == 1)
                return true;

            return false;
        }

        //Si ve que es numerico, dibujar un campo numerico y que no acepte letras
        public static bool NavegadorFuncEsNumerico(ClsColumnaInfo Col)
        {
            string TipoNet = (Col.TipoNet ?? "").ToLowerInvariant();
            string[] netNumericos = { "int16", "int32", "int64", "byte", "sbyte", "decimal", "double", "single" };

            if (netNumericos.Contains(TipoNet))
                return true;

            string TipoDato = (Col.TipoDato ?? "").ToLowerInvariant();
            //una vez ya verificado crea casos sobre el tipo de dato
            switch (TipoDato)
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
// Decide si una Columna se dibuja como fecha, checkbox o campo numerico
// Diego Alejandro Cheng Peña 0901-22-8091 
// Fecha actual : 14/09/2026