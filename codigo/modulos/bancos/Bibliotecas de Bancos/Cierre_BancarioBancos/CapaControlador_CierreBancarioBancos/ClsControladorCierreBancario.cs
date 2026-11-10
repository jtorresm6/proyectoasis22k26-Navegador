/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 -- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Valida los períodos de cierre bancario y coordina las consultas,
 *  verificaciones y cierres con el repositorio del módulo.
 * ================================================================
 */
using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_CierreBancario.Repositorio;

namespace CapaControlador_CierreBancario
{
    public static class ClsNavegacionHistorialCierre
    {
        public static int BancosMetInicio(int TotalFilas)
        {
            return TotalFilas <= 0 ? -1 : 0;
        }

        public static int BancosMetAnterior(int IndiceActual, int TotalFilas)
        {
            if (TotalFilas <= 0) return -1;
            if (IndiceActual < 0) return 0;
            return Math.Max(0, Math.Min(IndiceActual, TotalFilas - 1) - 1);
        }

        public static int BancosMetSiguiente(int IndiceActual, int TotalFilas)
        {
            if (TotalFilas <= 0) return -1;
            if (IndiceActual < 0) return 0;
            return Math.Min(TotalFilas - 1, IndiceActual + 1);
        }

        public static int BancosMetFin(int TotalFilas)
        {
            return TotalFilas <= 0 ? -1 : TotalFilas - 1;
        }
    }

    public class ClsControladorCierreBancario
    {
        private readonly IClsRepositorioCierreBancario _Repositorio;

        public ClsControladorCierreBancario()
            : this(new ClsRepositorioCierreBancario())
        {
        }

        public ClsControladorCierreBancario(IClsRepositorioCierreBancario Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public string BancosMetConsultar(string FechaInicial, string FechaFinal, out DataTable Resultado)
        {
            Resultado = null;
            if (!DateTime.TryParseExact(FechaInicial, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaIni))
                return "Fecha inicial no es válida. Use el formato dd/MM/aaaa.";

            if (!DateTime.TryParseExact(FechaFinal, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaFin))
                return "Fecha final no es válida. Use el formato dd/MM/aaaa.";

            if (FechaIni > FechaFin)
                return "La fecha inicial no puede ser mayor que la fecha final.";

            try
            {
                Resultado = _Repositorio.BancosMetConsultarResumenCuentas(FechaIni, FechaFin);
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar el período.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar el período.";
            }
        }

        public string BancosMetValidarCierre(
            string FechaInicial,
            string FechaFinal,
            out bool PeriodoValido,
            out bool ConciliacionesVerificadas,
            out bool SaldosVerificados)
        {
            PeriodoValido = false;
            ConciliacionesVerificadas = false;
            SaldosVerificados = false;

            if (!DateTime.TryParseExact(FechaInicial, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaIni))
                return "Fecha inicial no es válida.";

            if (!DateTime.TryParseExact(FechaFinal, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaFin))
                return "Fecha final no es válida.";

            if (FechaIni > FechaFin)
                return "La fecha inicial no puede ser mayor que la fecha final.";

            try
            {
                PeriodoValido = _Repositorio.BancosMetValidarPeriodoSinCierre(FechaIni, FechaFin);
                ConciliacionesVerificadas = _Repositorio.BancosMetValidarConciliacionesCompletas(FechaIni, FechaFin);
                SaldosVerificados = _Repositorio.BancosMetValidarSaldosVerificados(FechaIni, FechaFin);
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al validar el período.";
            }
            catch (Exception)
            {
                return "Error inesperado al validar el período.";
            }
        }

        public string BancosMetCerrarPeriodo(
            string FechaInicial,
            string FechaFinal)
        {
            if (!DateTime.TryParseExact(FechaInicial, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaIni))
                return "Fecha inicial no es válida.";

            if (!DateTime.TryParseExact(FechaFinal, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaFin))
                return "Fecha final no es válida.";

            if (FechaIni > FechaFin)
                return "La fecha inicial no puede ser mayor que la fecha final.";

            try
            {
                return _Repositorio.BancosMetCerrarPeriodo(FechaIni, FechaFin);
            }
            catch (OdbcException)
            {
                return "Error de base de datos al cerrar el período.";
            }
            catch (Exception)
            {
                return "Error inesperado al cerrar el período.";
            }
        }

        public string BancosMetConsultarHistorico(out DataTable Resultado)
        {
            Resultado = null;
            try
            {
                Resultado = _Repositorio.BancosMetConsultarHistorico();
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al consultar el historial.";
            }
            catch (Exception)
            {
                return "Error inesperado al consultar el historial.";
            }
        }
    }
}
