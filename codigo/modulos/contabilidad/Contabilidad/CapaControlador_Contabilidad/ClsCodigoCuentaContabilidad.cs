/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: formato y clase contable de EST-10 V1.1, Parte III. */
using System;
using System.Text.RegularExpressions;

namespace CapaControlador_Contabilidad
{
    public static class ClsCodigoCuentaContabilidad
    {
        public static bool ContabilidadFuncEsValido(string Codigo)
        {
            return Codigo != null && Regex.IsMatch(Codigo, @"\A[1-6]\.[0-9]{2}\.[0-9]{2}\.[0-9]{3}\z");
        }

        public static string ContabilidadFuncClase(string Codigo)
        {
            if (!ContabilidadFuncEsValido(Codigo)) throw new ArgumentException("El código de cuenta debe tener el formato C.GG.CC.SSS.");
            return new[] { "ACTIVO", "PASIVO", "PATRIMONIO", "INGRESOS", "COSTOS", "GASTOS" }[Codigo[0] - '1'];
        }
    }
}
