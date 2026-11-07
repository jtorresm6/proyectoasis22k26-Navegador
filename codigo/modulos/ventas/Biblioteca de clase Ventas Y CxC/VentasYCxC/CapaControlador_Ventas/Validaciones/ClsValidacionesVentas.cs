using System;
using System.Globalization;

namespace VentasValidaciones
{
    internal static class ClsValidacionesVentas
    {
        internal const decimal ImporteMaximo = 9999999999999999.99m;
        internal static void VentasMetExigir(bool Condicion, string Mensaje)
        {
            if (!Condicion)
                throw new InvalidOperationException(Mensaje);
        }

        internal static decimal VentasFuncDecimal(object Valor)
        {
            return Valor == null || Valor == DBNull.Value ? 0m : Convert.ToDecimal(Valor);
        }

        internal static decimal VentasFuncRedondear(decimal Valor)
        {
            return Math.Round(Valor, 2, MidpointRounding.AwayFromZero);
        }

        internal static string VentasFuncTexto(string Valor, int Maximo, string Campo, bool Requerido = true)
        {
            Valor = (Valor ?? "").Trim();
            VentasMetExigir(!Requerido || Valor.Length > 0, "Ingrese " + Campo + ".");
            VentasMetExigir(Valor.Length <= Maximo, Campo + " supera " + Maximo + " caracteres.");
            return Valor;
        }

        internal static decimal VentasFuncImporteCelda(object Valor)
        {
            if (Valor == null || Valor == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(Valor)))
                return 0;
            decimal Resultado;
            VentasMetExigir(decimal.TryParse(Convert.ToString(Valor), NumberStyles.Number, CultureInfo.CurrentCulture, out Resultado), "Ingrese un importe numerico valido.");
            return Resultado;
        }
    }
}
