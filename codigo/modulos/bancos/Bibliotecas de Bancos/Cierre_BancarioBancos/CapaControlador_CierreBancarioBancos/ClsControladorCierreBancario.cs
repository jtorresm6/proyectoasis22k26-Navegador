using System;
using System.Data;
using System.Data.Odbc;
using CapaModelo_CierreBancario.Repositorio;

namespace CapaControlador_CierreBancario
{
    public class ClsControladorCierreBancario
    {
        private ClsRepositorioCierreBancario _Repositorio;

        public ClsControladorCierreBancario()
        {
            _Repositorio = new ClsRepositorioCierreBancario();
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
            string FechaFinal,
            string Responsable)
        {
            if (string.IsNullOrWhiteSpace(Responsable))
                return "Debe indicar el responsable del cierre.";

            if (!DateTime.TryParseExact(FechaInicial, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaIni))
                return "Fecha inicial no es válida.";

            if (!DateTime.TryParseExact(FechaFinal, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime FechaFin))
                return "Fecha final no es válida.";

            return _Repositorio.BancosMetCerrarPeriodo(FechaIni, FechaFin, Responsable.Trim());
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
