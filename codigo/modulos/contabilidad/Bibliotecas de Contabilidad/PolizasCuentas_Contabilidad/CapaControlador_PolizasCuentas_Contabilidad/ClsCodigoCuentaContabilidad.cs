/* Área: Contabilidad. Autor: Oskar Cermeño. Carné: 0901-23-15379.
 * Fecha: 14/10/2026. Propósito: formato y clase contable de EST-10 V1.1, Parte III. */
using System;
using System.Text.RegularExpressions;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaControlador_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public static class ClsCodigoCuentaContabilidad
    {
        public static bool ContabilidadFuncEsValido(string Codigo)
        {
            return Codigo != null && Regex.IsMatch(Codigo, @"\A[0-7](\.[0-9]{2}){0,2}(\.[0-9]{3})?\z");
        }

        public static string ContabilidadFuncClase(string Codigo)
        {
            if (!ContabilidadFuncEsValido(Codigo)) throw new ArgumentException("El código de cuenta no tiene un formato válido.");
            switch (Codigo[0])
            {
                case '1': return "ACTIVO";
                case '2': return "PASIVO";
                case '3': return "PATRIMONIO";
                case '4': return "INGRESOS";
                case '5': return Codigo.StartsWith("5.01") || Codigo.StartsWith("5.02") ? "COSTOS" : "GASTOS";
                case '6': return "CONTINGENCIA";
                case '7': return "CUENTAS DE ORDEN";
                default: return "RAIZ";
            }
        }
    }
}
