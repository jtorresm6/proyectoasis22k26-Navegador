using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_CierreBancario.Repositorio
{
    public class ClsRepositorioCierreBancario : ClsSentencias
    {
        private const string SqlConsultarResumen =
            "SELECT " +
            "  c.Pk_Conciliacion, " +
            "  c.fechaCorteConciliacion AS FechaCorte, " +
            "  cb.nombreCuentaBancaria AS Cuenta, " +
            "  cb.numeroCuentaBancaria AS NumeroCuenta, " +
            "  c.estadoConciliacion AS Estado, " +
            "  c.saldoLibroConciliacion AS SaldoLibro, " +
            "  c.saldoBancoConciliacion AS SaldoBanco, " +
            "  c.diferenciaConciliacion AS Diferencia " +
            "FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb " +
            "  ON c.Fk_CuentaBancaria_Codigo = cb.Pk_CuentaBancaria " +
            "WHERE c.fechaCorteConciliacion BETWEEN ? AND ? " +
            "  AND cb.is_active = 1 " +
            "ORDER BY c.fechaCorteConciliacion, cb.nombreCuentaBancaria";

        private const string SqlContarCerradas =
            "SELECT COUNT(*) FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb " +
            "  ON c.Fk_CuentaBancaria_Codigo = cb.Pk_CuentaBancaria " +
            "WHERE c.fechaCorteConciliacion BETWEEN ? AND ? " +
            "  AND c.estadoConciliacion = 'CERRADA' " +
            "  AND cb.is_active = 1";

        private const string SqlContarPendientes =
            "SELECT COUNT(*) FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb " +
            "  ON c.Fk_CuentaBancaria_Codigo = cb.Pk_CuentaBancaria " +
            "WHERE c.fechaCorteConciliacion BETWEEN ? AND ? " +
            "  AND c.estadoConciliacion IN ('ABIERTA', 'PROCESANDO') " +
            "  AND cb.is_active = 1";

        private const string SqlContarConDiferencias =
            "SELECT COUNT(*) FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb " +
            "  ON c.Fk_CuentaBancaria_Codigo = cb.Pk_CuentaBancaria " +
            "WHERE c.fechaCorteConciliacion BETWEEN ? AND ? " +
            "  AND c.estadoConciliacion = 'CON_DIFERENCIAS' " +
            "  AND cb.is_active = 1";

        private const string SqlCerrarPeriodo =
            "UPDATE tblConciliacion SET " +
            "  estadoConciliacion = 'CERRADA', " +
            "  fechaCierreConciliacion = NOW(), " +
            "  identificadorResponsableConciliacion = ? " +
            "WHERE fechaCorteConciliacion BETWEEN ? AND ? " +
            "  AND estadoConciliacion IN ('CONCILIADA', 'CON_DIFERENCIAS')";

        private const string SqlConsultarHistorico =
            "SELECT " +
            "  c.fechaCorteConciliacion AS FechaCorte, " +
            "  cb.nombreCuentaBancaria AS Cuenta, " +
            "  cb.numeroCuentaBancaria AS NumeroCuenta, " +
            "  c.saldoLibroConciliacion AS SaldoLibro, " +
            "  c.saldoBancoConciliacion AS SaldoBanco, " +
            "  c.diferenciaConciliacion AS Diferencia, " +
            "  c.identificadorResponsableConciliacion AS Responsable, " +
            "  c.fechaCierreConciliacion AS FechaCierre " +
            "FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb " +
            "  ON c.Fk_CuentaBancaria_Codigo = cb.Pk_CuentaBancaria " +
            "WHERE c.estadoConciliacion = 'CERRADA' " +
            "ORDER BY c.fechaCierreConciliacion DESC, c.fechaCorteConciliacion";

        public DataTable BancosMetConsultarResumenCuentas(DateTime FechaInicial, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicial", OdbcType.Date) { Value = FechaInicial.Date },
                new OdbcParameter("FechaFinal", OdbcType.Date) { Value = FechaFinal.Date }
            };
            return BancosMetEjecucionConsulta(SqlConsultarResumen, CommandType.Text, Parametros);
        }

        public bool BancosMetValidarPeriodoSinCierre(DateTime FechaInicial, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicial", OdbcType.Date) { Value = FechaInicial.Date },
                new OdbcParameter("FechaFinal", OdbcType.Date) { Value = FechaFinal.Date }
            };
            var Resultado = BancosMetEjecucionConsulta(SqlContarCerradas, CommandType.Text, Parametros);
            return Convert.ToInt32(Resultado.Rows[0][0]) == 0;
        }

        public bool BancosMetValidarConciliacionesCompletas(DateTime FechaInicial, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicial", OdbcType.Date) { Value = FechaInicial.Date },
                new OdbcParameter("FechaFinal", OdbcType.Date) { Value = FechaFinal.Date }
            };
            var Resultado = BancosMetEjecucionConsulta(SqlContarPendientes, CommandType.Text, Parametros);
            return Convert.ToInt32(Resultado.Rows[0][0]) == 0;
        }

        public bool BancosMetValidarSaldosVerificados(DateTime FechaInicial, DateTime FechaFinal)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("FechaInicial", OdbcType.Date) { Value = FechaInicial.Date },
                new OdbcParameter("FechaFinal", OdbcType.Date) { Value = FechaFinal.Date }
            };
            var Resultado = BancosMetEjecucionConsulta(SqlContarConDiferencias, CommandType.Text, Parametros);
            return Convert.ToInt32(Resultado.Rows[0][0]) == 0;
        }

        public string BancosMetCerrarPeriodo(DateTime FechaInicial, DateTime FechaFinal, string Responsable)
        {
            try
            {
                var Parametros = new List<OdbcParameter>
                {
                    new OdbcParameter("Responsable", OdbcType.VarChar, 100) { Value = Responsable },
                    new OdbcParameter("FechaInicial", OdbcType.Date) { Value = FechaInicial.Date },
                    new OdbcParameter("FechaFinal", OdbcType.Date) { Value = FechaFinal.Date }
                };
                int FilasAfectadas = BancosMetEjecucionNonQuery(SqlCerrarPeriodo, Parametros, CommandType.Text);
                if (FilasAfectadas == 0)
                    return "No se encontraron conciliaciones en el período para cerrar.";
                return string.Empty;
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

        public DataTable BancosMetConsultarHistorico()
        {
            return BancosMetEjecucionConsulta(SqlConsultarHistorico, CommandType.Text);
        }
    }
}
