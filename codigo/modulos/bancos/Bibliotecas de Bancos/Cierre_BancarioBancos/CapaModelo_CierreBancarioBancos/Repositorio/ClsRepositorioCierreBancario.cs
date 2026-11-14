/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 -- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Consulta, valida y actualiza las conciliaciones bancarias
 *  relacionadas con el proceso de cierre.
 * ================================================================
 */
using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_CierreBancario.Repositorio
{
    public class ClsRepositorioCierreBancario : ClsConexion, IClsRepositorioCierreBancario
    {
        private const string SqlConsultarResumen =
            "SELECT c.idConciliacion AS IdConciliacion, " +
            "c.fechaCorteConciliacion AS FechaCorte, " +
            "cb.nombreCuentaBancaria AS Cuenta, " +
            "cb.numeroCuentaBancaria AS NumeroCuenta, " +
            "m.nombreMoneda AS Moneda, " +
            "c.estadoConciliacion AS Estado, " +
            "c.saldoLibroConciliacion AS SaldoLibro, " +
            "c.saldoBancoConciliacion AS SaldoBanco, " +
            "c.diferenciaConciliacion AS Diferencia " +
            "FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb ON c.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria " +
            "INNER JOIN tblMoneda m ON cb.Fk_Moneda_Codigo = m.idMoneda " +
            "WHERE c.fechaCorteConciliacion >= ? AND c.fechaCorteConciliacion < ? " +
            "ORDER BY c.fechaCorteConciliacion, cb.nombreCuentaBancaria";

        private const string SqlContarCerradas =
            "SELECT COUNT(*) FROM tblConciliacion " +
            "WHERE fechaCorteConciliacion >= ? AND fechaCorteConciliacion < ? " +
            "AND estadoConciliacion = 'CERRADA'";

        private const string SqlContarNoConciliadas =
            "SELECT COUNT(*) FROM tblConciliacion " +
            "WHERE fechaCorteConciliacion >= ? AND fechaCorteConciliacion < ? " +
            "AND estadoConciliacion <> 'CONCILIADA'";

        private const string SqlContarSaldosNoVerificados =
            "SELECT COUNT(*) FROM tblConciliacion " +
            "WHERE fechaCorteConciliacion >= ? AND fechaCorteConciliacion < ? " +
            "AND (estadoConciliacion = 'CON_DIFERENCIAS' " +
            "OR saldoLibroConciliacion IS NULL OR saldoBancoConciliacion IS NULL " +
            "OR diferenciaConciliacion IS NULL OR diferenciaConciliacion <> 0)";

        private const string SqlConsultarHistorico =
            "SELECT MIN(fechaCorteConciliacion) AS FechaInicial, " +
            "MAX(fechaCorteConciliacion) AS FechaFinal, " +
            "COUNT(*) AS Conciliaciones, " +
            "fechaCierreConciliacion AS FechaCierre " +
            "FROM tblConciliacion " +
            "WHERE estadoConciliacion = 'CERRADA' AND fechaCierreConciliacion IS NOT NULL " +
            "GROUP BY fechaCierreConciliacion " +
            "ORDER BY fechaCierreConciliacion DESC";

        private const string MonedaBase = "GTQ";

        private const string SqlColumnasSaldoCierre =
            "cb.idCuentaBancaria AS IdCuenta, " +
            "cb.nombreCuentaBancaria AS Cuenta, " +
            "m.codigoMoneda AS CodigoMoneda, " +
            "m.simboloMoneda AS SimboloMoneda, " +
            "c.saldoBancoConciliacion AS Saldo, " +
            "CASE WHEN m.codigoMoneda = '" + MonedaBase + "' THEN 1 ELSE (" +
            "SELECT t.valorTipoCambio FROM tblTipoCambio t " +
            "INNER JOIN tblMoneda md ON md.idMoneda = t.Fk_Moneda_Destino_Codigo " +
            "WHERE t.Fk_Moneda_Origen_Codigo = m.idMoneda AND md.codigoMoneda = '" + MonedaBase + "' " +
            "AND t.fechaTipoCambio <= c.fechaCorteConciliacion " +
            "ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC LIMIT 1) END AS TipoCambio ";

        private const string SqlUnionSaldoCierre =
            "FROM tblConciliacion c " +
            "INNER JOIN tblCuentaBancaria cb ON c.Fk_CuentaBancaria_Codigo = cb.idCuentaBancaria " +
            "INNER JOIN tblMoneda m ON cb.Fk_Moneda_Codigo = m.idMoneda ";

        private const string SqlSaldosCierrePeriodo =
            "SELECT " + SqlColumnasSaldoCierre + SqlUnionSaldoCierre +
            "WHERE c.fechaCorteConciliacion >= ? AND c.fechaCorteConciliacion < ? " +
            "AND NOT EXISTS (SELECT 1 FROM tblConciliacion c2 " +
            "WHERE c2.Fk_CuentaBancaria_Codigo = c.Fk_CuentaBancaria_Codigo " +
            "AND c2.fechaCorteConciliacion >= ? AND c2.fechaCorteConciliacion < ? " +
            "AND (c2.fechaCorteConciliacion > c.fechaCorteConciliacion " +
            "OR (c2.fechaCorteConciliacion = c.fechaCorteConciliacion AND c2.idConciliacion > c.idConciliacion)))";

        private const string SqlSaldosCierresHistoricos =
            "SELECT c.fechaCierreConciliacion AS FechaCierre, " + SqlColumnasSaldoCierre + SqlUnionSaldoCierre +
            "WHERE c.estadoConciliacion = 'CERRADA' AND c.fechaCierreConciliacion IS NOT NULL " +
            "AND NOT EXISTS (SELECT 1 FROM tblConciliacion c2 " +
            "WHERE c2.Fk_CuentaBancaria_Codigo = c.Fk_CuentaBancaria_Codigo " +
            "AND c2.estadoConciliacion = 'CERRADA' " +
            "AND c2.fechaCierreConciliacion = c.fechaCierreConciliacion " +
            "AND (c2.fechaCorteConciliacion > c.fechaCorteConciliacion " +
            "OR (c2.fechaCorteConciliacion = c.fechaCorteConciliacion AND c2.idConciliacion > c.idConciliacion)))";

        public DataTable BancosMetConsultarResumenCuentas(DateTime FechaInicial, DateTime FechaFinal)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlConsultarResumen, Conexion))
            {
                Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                Conexion.Open();
                DataTable Resultado = new DataTable();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    Resultado.Load(Lector);
                return Resultado;
            }
        }

        public bool BancosMetValidarPeriodoSinCierre(DateTime FechaInicial, DateTime FechaFinal)
        {
            return BancosMetContar(SqlContarCerradas, FechaInicial, FechaFinal) == 0;
        }

        public bool BancosMetValidarConciliacionesCompletas(DateTime FechaInicial, DateTime FechaFinal)
        {
            return BancosMetContar(SqlContarNoConciliadas, FechaInicial, FechaFinal) == 0;
        }

        public bool BancosMetValidarSaldosVerificados(DateTime FechaInicial, DateTime FechaFinal)
        {
            return BancosMetContar(SqlContarSaldosNoVerificados, FechaInicial, FechaFinal) == 0;
        }

        private int BancosMetContar(string Sql, DateTime FechaInicial, DateTime FechaFinal)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
            {
                Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                Conexion.Open();
                return Convert.ToInt32(Comando.ExecuteScalar());
            }
        }

        public string BancosMetCerrarPeriodo(DateTime FechaInicial, DateTime FechaFinal)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            {
                Conexion.Open();
                using (OdbcTransaction Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int Cantidad = BancosMetContarBloqueados(Conexion, Transaccion, FechaInicial, FechaFinal);
                        if (Cantidad == 0)
                            return BancosMetRevertir(Transaccion, "No hay conciliaciones en el período seleccionado.");

                        if (BancosMetContarTransaccional(Conexion, Transaccion, SqlContarCerradas, FechaInicial, FechaFinal) > 0)
                            return BancosMetRevertir(Transaccion, "El período ya contiene conciliaciones cerradas.");

                        if (BancosMetContarTransaccional(Conexion, Transaccion, SqlContarNoConciliadas, FechaInicial, FechaFinal) > 0)
                            return BancosMetRevertir(Transaccion, "Todas las conciliaciones deben estar en estado CONCILIADA.");

                        if (BancosMetContarTransaccional(Conexion, Transaccion, SqlContarSaldosNoVerificados, FechaInicial, FechaFinal) > 0)
                            return BancosMetRevertir(Transaccion, "Hay saldos pendientes o diferencias sin resolver.");

                        const string SqlCerrar =
                            "UPDATE tblConciliacion SET estadoConciliacion = 'CERRADA', " +
                            "fechaCierreConciliacion = CURRENT_TIMESTAMP " +
                            "WHERE fechaCorteConciliacion >= ? AND fechaCorteConciliacion < ? " +
                            "AND estadoConciliacion = 'CONCILIADA'";

                        using (OdbcCommand Comando = new OdbcCommand(SqlCerrar, Conexion, Transaccion))
                        {
                            Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                            Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                            if (Comando.ExecuteNonQuery() != Cantidad)
                                return BancosMetRevertir(Transaccion, "Los datos cambiaron durante el cierre. Consulte y valide el período nuevamente.");
                        }

                        Transaccion.Commit();
                        return string.Empty;
                    }
                    catch (OdbcException)
                    {
                        if (Transaccion.Connection != null) Transaccion.Rollback();
                        return "Error de base de datos al cerrar el período.";
                    }
                    catch (Exception)
                    {
                        if (Transaccion.Connection != null) Transaccion.Rollback();
                        return "Error inesperado al cerrar el período.";
                    }
                }
            }
        }

        private int BancosMetContarBloqueados(OdbcConnection Conexion, OdbcTransaction Transaccion, DateTime FechaInicial, DateTime FechaFinal)
        {
            const string SqlBloquear =
                "SELECT idConciliacion FROM tblConciliacion " +
                "WHERE fechaCorteConciliacion >= ? AND fechaCorteConciliacion < ? FOR UPDATE";
            using (OdbcCommand Comando = new OdbcCommand(SqlBloquear, Conexion, Transaccion))
            {
                Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                int Cantidad = 0;
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    while (Lector.Read()) Cantidad++;
                return Cantidad;
            }
        }

        private int BancosMetContarTransaccional(OdbcConnection Conexion, OdbcTransaction Transaccion, string Sql, DateTime FechaInicial, DateTime FechaFinal)
        {
            using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion, Transaccion))
            {
                Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                return Convert.ToInt32(Comando.ExecuteScalar());
            }
        }

        private string BancosMetRevertir(OdbcTransaction Transaccion, string Mensaje)
        {
            Transaccion.Rollback();
            return Mensaje;
        }

        public DataTable BancosMetConsultarHistorico()
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlConsultarHistorico, Conexion))
            {
                Conexion.Open();
                DataTable Resultado = new DataTable();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    Resultado.Load(Lector);
                return Resultado;
            }
        }

        public DataTable BancosMetConsultarSaldosCierrePeriodo(DateTime FechaInicial, DateTime FechaFinal)
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlSaldosCierrePeriodo, Conexion))
            {
                Comando.Parameters.Add("FechaInicial", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinal", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                Comando.Parameters.Add("FechaInicialUltima", OdbcType.Date).Value = FechaInicial.Date;
                Comando.Parameters.Add("FechaFinalUltima", OdbcType.Date).Value = FechaFinal.Date.AddDays(1);
                Conexion.Open();
                DataTable Resultado = new DataTable();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    Resultado.Load(Lector);
                return Resultado;
            }
        }

        public DataTable BancosMetConsultarSaldosCierresHistoricos()
        {
            using (OdbcConnection Conexion = BancosMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(SqlSaldosCierresHistoricos, Conexion))
            {
                Conexion.Open();
                DataTable Resultado = new DataTable();
                using (OdbcDataReader Lector = Comando.ExecuteReader())
                    Resultado.Load(Lector);
                return Resultado;
            }
        }
    }
}
