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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
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

    public class ClsTotalCierre
    {
        public decimal TotalQuetzales { get; set; }
        public int Cuentas { get; set; }
        public string DetalleMonedas { get; set; } = string.Empty;
        public string MonedasSinTipoCambio { get; set; } = string.Empty;

        public bool TipoCambioCompleto
        {
            get { return MonedasSinTipoCambio.Length == 0; }
        }

        public string TotalTexto
        {
            get
            {
                string Texto = ClsCalculoTotalCierre.BancosFuncFormatearMonto("Q", TotalQuetzales);
                return TipoCambioCompleto ? Texto : Texto + " (parcial)";
            }
        }
    }

    public static class ClsCalculoTotalCierre
    {
        private const string MonedaBase = "GTQ";

        public static string BancosFuncFormatearMonto(string Simbolo, decimal Monto)
        {
            return Simbolo + " " + Monto.ToString("N2", CultureInfo.InvariantCulture);
        }

        public static ClsTotalCierre BancosFuncCalcular(IEnumerable<DataRow> Saldos)
        {
            SortedDictionary<string, decimal> TotalesPorMoneda = new SortedDictionary<string, decimal>(StringComparer.Ordinal);
            Dictionary<string, string> Simbolos = new Dictionary<string, string>();
            SortedSet<string> SinTipoCambio = new SortedSet<string>(StringComparer.Ordinal);
            decimal TotalBase = 0m;
            int Cuentas = 0;

            foreach (DataRow Fila in Saldos ?? Enumerable.Empty<DataRow>())
            {
                string Codigo = Convert.ToString(Fila["CodigoMoneda"]);
                string Simbolo = Convert.ToString(Fila["SimboloMoneda"]);
                decimal Saldo = Fila["Saldo"] == DBNull.Value ? 0m : Convert.ToDecimal(Fila["Saldo"]);
                decimal TipoCambio = Fila["TipoCambio"] == DBNull.Value ? 0m : Convert.ToDecimal(Fila["TipoCambio"]);

                Cuentas++;
                TotalesPorMoneda[Codigo] = (TotalesPorMoneda.ContainsKey(Codigo) ? TotalesPorMoneda[Codigo] : 0m) + Saldo;
                Simbolos[Codigo] = string.IsNullOrWhiteSpace(Simbolo) ? Codigo : Simbolo;

                if (TipoCambio > 0m)
                    TotalBase += Math.Round(Saldo * TipoCambio, 2, MidpointRounding.AwayFromZero);
                else
                    SinTipoCambio.Add(Codigo);
            }

            List<string> Partes = new List<string>();
            foreach (KeyValuePair<string, decimal> Moneda in TotalesPorMoneda)
                Partes.Add(BancosFuncFormatearMonto(Simbolos[Moneda.Key], Moneda.Value) + " (" + Moneda.Key + ")");

            ClsTotalCierre Total = new ClsTotalCierre();
            Total.TotalQuetzales = TotalBase;
            Total.Cuentas = Cuentas;
            Total.MonedasSinTipoCambio = string.Join(", ", SinTipoCambio);
            Total.DetalleMonedas = string.Join("  |  ", Partes);
            if (!Total.TipoCambioCompleto)
                Total.DetalleMonedas += "  |  Sin tipo de cambio: " + Total.MonedasSinTipoCambio;
            return Total;
        }

        public static void BancosMetAgregarTotales(DataTable Historico, DataTable Saldos)
        {
            if (!Historico.Columns.Contains("Cuentas")) Historico.Columns.Add("Cuentas", typeof(int));
            if (!Historico.Columns.Contains("TotalCierre")) Historico.Columns.Add("TotalCierre", typeof(decimal));
            if (!Historico.Columns.Contains("DetalleMonedas")) Historico.Columns.Add("DetalleMonedas", typeof(string));

            Dictionary<DateTime, List<DataRow>> PorCierre = new Dictionary<DateTime, List<DataRow>>();
            if (Saldos != null && Saldos.Columns.Contains("FechaCierre"))
            {
                foreach (DataRow Fila in Saldos.Rows)
                {
                    DateTime FechaCierre = Convert.ToDateTime(Fila["FechaCierre"]);
                    if (!PorCierre.ContainsKey(FechaCierre)) PorCierre[FechaCierre] = new List<DataRow>();
                    PorCierre[FechaCierre].Add(Fila);
                }
            }

            foreach (DataRow Fila in Historico.Rows)
            {
                List<DataRow> Grupo;
                DateTime FechaCierre = Convert.ToDateTime(Fila["FechaCierre"]);
                PorCierre.TryGetValue(FechaCierre, out Grupo);
                ClsTotalCierre Total = BancosFuncCalcular(Grupo);
                Fila["Cuentas"] = Total.Cuentas;
                Fila["TotalCierre"] = Total.TotalQuetzales;
                Fila["DetalleMonedas"] = Total.DetalleMonedas;
            }
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

        public string BancosMetConsultarTotalPeriodo(string FechaInicial, string FechaFinal, out ClsTotalCierre Total)
        {
            Total = null;
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
                DataTable Saldos = _Repositorio.BancosMetConsultarSaldosCierrePeriodo(FechaIni, FechaFin);
                Total = ClsCalculoTotalCierre.BancosFuncCalcular(
                    Saldos == null ? null : Saldos.Rows.Cast<DataRow>());
                return string.Empty;
            }
            catch (OdbcException)
            {
                return "Error de base de datos al calcular el total del cierre.";
            }
            catch (Exception)
            {
                return "Error inesperado al calcular el total del cierre.";
            }
        }

        public string BancosMetConsultarHistorico(out DataTable Resultado)
        {
            Resultado = null;
            try
            {
                DataTable Historico = _Repositorio.BancosMetConsultarHistorico();
                DataTable Saldos = _Repositorio.BancosMetConsultarSaldosCierresHistoricos();
                ClsCalculoTotalCierre.BancosMetAgregarTotales(Historico, Saldos);
                Resultado = Historico;
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
