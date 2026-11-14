using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_CierreInventario_Logistica.Validaciones
{
    public static class ClsValidacionCierreInventario
    {
        // Calcula el saldo que debería tener el inventario.
        public static long CalcularSaldo(
            long saldoAnterior,
            long cargos,
            long abonos)
        {
            return checked(saldoAnterior + cargos - abonos);
        }

        // Comprueba que el inventario esté cuadrado.
        public static bool ValidarCuadratura(
            long saldoAnterior,
            long cargos,
            long abonos,
            long saldoActual)
        {
            if (saldoAnterior < 0 ||
                cargos < 0 ||
                abonos < 0 ||
                saldoActual < 0)
            {
                return false;
            }

            return CalcularSaldo(
                saldoAnterior,
                cargos,
                abonos) == saldoActual;
        }

        // Comprueba que el mes seleccionado ya terminó.
        public static bool ValidarPeriodoFinalizado(
            int anio,
            int mes,
            DateTime fechaActual)
        {
            if (anio < 1 || anio > 9999 ||
                mes < 1 || mes > 12)
            {
                return false;
            }

            DateTime inicio =
                new DateTime(anio, mes, 1);

            int dias = DateTime.DaysInMonth(anio, mes);

            DateTime fin = inicio.AddDays(dias - 1);

            return fin < fechaActual.Date;
        }
    }
}
