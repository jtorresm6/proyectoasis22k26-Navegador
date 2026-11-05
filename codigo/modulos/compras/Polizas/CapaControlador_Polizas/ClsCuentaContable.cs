using System.Text.RegularExpressions;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Valida el formato del código de cuenta contable utilizado en las pólizas de compras.
 * ===================================================================
*/

namespace CapaControlador_Polizas
{
    public static class ClsCuentaContable
    {
        public static bool ComprasFuncFormatoValido(string CodigoCuenta)
        {
            return CodigoCuenta != null && Regex.IsMatch(CodigoCuenta,
                @"\A[1-6]\.[0-9]{2}\.[0-9]{2}\.[0-9]{3}\z", RegexOptions.CultureInvariant);
        }
    }
}
