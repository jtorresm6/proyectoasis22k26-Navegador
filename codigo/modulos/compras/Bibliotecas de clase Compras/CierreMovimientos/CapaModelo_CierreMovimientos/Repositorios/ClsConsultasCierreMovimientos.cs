using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_Componentes.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Consultas de solo lectura del cierre de movimientos: períodos, saldos por
 *  proveedor, documentos pendientes, rangos de antigüedad y los indicadores
 *  que alimentan las validaciones previas al cierre.
 *  Los montos se convierten a moneda local con el tipo de cambio de cada documento.
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Repositorios
{
    public class ClsConsultasCierreMovimientos : ClsRepositorioMaestroCompras
    {
        private const string _CamposPeriodo =
            "SELECT idPeriodoCompras, anioPeriodoCompras, mesPeriodoCompras, fechaInicioPeriodoCompras, " +
            "fechaFinPeriodoCompras, estadoPeriodoCompras FROM tblPeriodoCompras ";

        // Monto del documento en moneda local, con signo según la naturaleza del concepto.
        private const string _MontoLocal = "x.montoTotalCuentaPorPagar * x.tipoCambioCuentaPorPagar";

        public List<ClsPeriodoCompras> ComprasFuncObtenerPeriodosAbiertos()
        {
            var Periodos = new List<ClsPeriodoCompras>();
            using (DataTable Datos = ComprasFuncEjecucionConsulta(
                _CamposPeriodo + "WHERE estadoPeriodoCompras = 'Abierto' ORDER BY fechaInicioPeriodoCompras", CommandType.Text))
            {
                foreach (DataRow Fila in Datos.Rows) Periodos.Add(ComprasFuncLeerPeriodo(Fila));
            }
            return Periodos;
        }

        public ClsPeriodoCompras ComprasFuncObtenerPeriodo(int IdPeriodo)
        {
            using (DataTable Datos = ComprasFuncEjecucionConsulta(_CamposPeriodo + "WHERE idPeriodoCompras = ?",
                CommandType.Text, new List<OdbcParameter> { ComprasFuncEntero("IdPeriodo", IdPeriodo) }))
            {
                return Datos.Rows.Count == 0 ? null : ComprasFuncLeerPeriodo(Datos.Rows[0]);
            }
        }

        public DateTime? ComprasFuncObtenerUltimaFechaCerrada()
        {
            object Valor = ComprasFuncEjecucionEscalar(
                "SELECT MAX(fechaFinPeriodoCompras) FROM tblPeriodoCompras WHERE estadoPeriodoCompras = 'Cerrado'",
                null, CommandType.Text);
            return Valor == null || Valor == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Valor);
        }

        public long ComprasFuncObtenerIdMoneda(string Codigo)
        {
            object Valor = ComprasFuncEjecucionEscalar(
                "SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = ?", new List<OdbcParameter>
                {
                    new OdbcParameter("Codigo", OdbcType.Char, 3) { Value = Codigo }
                }, CommandType.Text);
            if (Valor == null || Valor == DBNull.Value)
                throw new InvalidOperationException("La moneda local (" + Codigo + ") no está registrada en tblMoneda.");
            return Convert.ToInt64(Valor);
        }

        public List<ClsSaldoProveedorCierre> ComprasFuncObtenerSaldosProveedor(ClsPeriodoCompras Periodo)
        {
            // Saldo inicial: el saldo guardado en el cierre del mes anterior; si no existe
            // (primer cierre), se calcula con todos los documentos anteriores al período.
            string Consulta =
                "SELECT p.idProveedor, p.nombreProveedor, " +
                "COALESCE((SELECT SUM(s.saldoFinalLocalSaldoProveedorPeriodo) FROM tblSaldoProveedorPeriodo s " +
                "          INNER JOIN tblPeriodoCompras pe ON pe.idPeriodoCompras = s.idPeriodoCompras " +
                "          WHERE s.idProveedor = p.idProveedor AND pe.estadoPeriodoCompras = 'Cerrado' AND pe.fechaFinPeriodoCompras = ?), " +
                "         (SELECT COALESCE(SUM(CASE WHEN c.naturalezaConceptoProveedor = 'Cargo' THEN 1 ELSE -1 END * " + _MontoLocal + "), 0) " +
                "          FROM tblCuentaPorPagar x INNER JOIN tblConceptoProveedor c ON c.idConceptoProveedor = x.idConceptoProveedor " +
                "          WHERE x.idProveedor = p.idProveedor AND x.estadoCuentaPorPagar <> 'Anulada' AND x.fechaEmisionCuentaPorPagar < ?)) AS saldoInicial, " +
                ComprasFuncSumaNaturaleza("Cargo") + " AS cargos, " +
                ComprasFuncSumaNaturaleza("Abono") + " AS abonos " +
                "FROM tblProveedor p ORDER BY p.nombreProveedor";

            var Parametros = new List<OdbcParameter>
            {
                ComprasFuncFecha("FinAnterior", Periodo.FechaInicio.AddDays(-1)),
                ComprasFuncFecha("Inicio", Periodo.FechaInicio),
                ComprasFuncFecha("InicioCargos", Periodo.FechaInicio), ComprasFuncFecha("FinCargos", Periodo.FechaFin),
                ComprasFuncFecha("InicioAbonos", Periodo.FechaInicio), ComprasFuncFecha("FinAbonos", Periodo.FechaFin)
            };

            var Saldos = new List<ClsSaldoProveedorCierre>();
            using (DataTable Datos = ComprasFuncEjecucionConsulta(Consulta, CommandType.Text, Parametros))
            {
                foreach (DataRow Fila in Datos.Rows)
                {
                    var Saldo = new ClsSaldoProveedorCierre
                    {
                        IdProveedor = Convert.ToInt32(Fila["idProveedor"]),
                        NombreProveedor = Convert.ToString(Fila["nombreProveedor"]),
                        SaldoInicial = Math.Round(Convert.ToDecimal(Fila["saldoInicial"]), 2),
                        Cargos = Math.Round(Convert.ToDecimal(Fila["cargos"]), 2),
                        Abonos = Math.Round(Convert.ToDecimal(Fila["abonos"]), 2)
                    };
                    if (Saldo.TieneMovimiento) Saldos.Add(Saldo);
                }
            }
            return Saldos;
        }

        public List<ClsDocumentoPendiente> ComprasFuncObtenerDocumentosPendientes(DateTime FechaCorte)
        {
            string Consulta =
                "SELECT x.idProveedor, x.fechaVencimientoCuentaPorPagar, " +
                "x.saldoActualCuentaPorPagar * x.tipoCambioCuentaPorPagar AS saldoLocal " +
                "FROM tblCuentaPorPagar x INNER JOIN tblConceptoProveedor c ON c.idConceptoProveedor = x.idConceptoProveedor " +
                "WHERE c.naturalezaConceptoProveedor = 'Cargo' AND x.estadoCuentaPorPagar IN ('Pendiente','Parcial') " +
                "AND x.saldoActualCuentaPorPagar > 0 AND x.fechaEmisionCuentaPorPagar <= ?";

            var Documentos = new List<ClsDocumentoPendiente>();
            using (DataTable Datos = ComprasFuncEjecucionConsulta(Consulta, CommandType.Text,
                new List<OdbcParameter> { ComprasFuncFecha("Corte", FechaCorte) }))
            {
                foreach (DataRow Fila in Datos.Rows)
                {
                    Documentos.Add(new ClsDocumentoPendiente
                    {
                        IdProveedor = Convert.ToInt32(Fila["idProveedor"]),
                        FechaVencimiento = Convert.ToDateTime(Fila["fechaVencimientoCuentaPorPagar"]),
                        SaldoLocal = Math.Round(Convert.ToDecimal(Fila["saldoLocal"]), 2)
                    });
                }
            }
            return Documentos;
        }

        public List<ClsRangoAntiguedad> ComprasFuncObtenerRangosAntiguedad()
        {
            var Rangos = new List<ClsRangoAntiguedad>();
            using (DataTable Datos = ComprasFuncEjecucionConsulta(
                "SELECT idRangoAntiguedad, nombreRangoAntiguedad, diasDesdeRangoAntiguedad, diasHastaRangoAntiguedad " +
                "FROM tblRangoAntiguedad WHERE activoRangoAntiguedad = 1 ORDER BY ordenRangoAntiguedad", CommandType.Text))
            {
                foreach (DataRow Fila in Datos.Rows)
                {
                    Rangos.Add(new ClsRangoAntiguedad
                    {
                        IdRango = Convert.ToInt32(Fila["idRangoAntiguedad"]),
                        Nombre = Convert.ToString(Fila["nombreRangoAntiguedad"]),
                        DiasDesde = Fila["diasDesdeRangoAntiguedad"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["diasDesdeRangoAntiguedad"]),
                        DiasHasta = Fila["diasHastaRangoAntiguedad"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["diasHastaRangoAntiguedad"])
                    });
                }
            }
            return Rangos;
        }

        public ClsIndicadoresCierre ComprasFuncObtenerIndicadores(ClsPeriodoCompras Periodo, long IdMonedaLocal)
        {
            var Indicadores = new ClsIndicadoresCierre
            {
                PeriodosAbiertosAnteriores = ComprasFuncContar(
                    "SELECT COUNT(*) FROM tblPeriodoCompras WHERE estadoPeriodoCompras = 'Abierto' AND fechaFinPeriodoCompras < ?",
                    ComprasFuncFecha("Inicio", Periodo.FechaInicio)),
                OrdenesPendientes = ComprasFuncContar(
                    "SELECT COUNT(*) FROM tblEncabezadoOrdenCompra WHERE estadoOrdenCompra = 'Pendiente' AND fechaEmisionOrdenCompra <= ?",
                    ComprasFuncFecha("Fin", Periodo.FechaFin)),
                ComprasSinCuentaPorPagar = ComprasFuncContar(
                    "SELECT COUNT(*) FROM tblEncabezadoCompra ec WHERE ec.estadoCompra = 'Registrada' " +
                    "AND ec.fechaCompra BETWEEN ? AND ? AND NOT EXISTS (SELECT 1 FROM tblCuentaPorPagar x " +
                    "WHERE x.idProveedor = ec.idProveedor AND x.serieDocumentoCuentaPorPagar = ec.serieFacturaCompra " +
                    "AND x.numeroDocumentoCuentaPorPagar = ec.numeroFacturaCompra)",
                    ComprasFuncFecha("Inicio", Periodo.FechaInicio), ComprasFuncFecha("Fin", Periodo.FechaFin)),
                DocumentosMonedaExtranjera = ComprasFuncContar(
                    "SELECT COUNT(*) FROM tblCuentaPorPagar WHERE estadoCuentaPorPagar <> 'Anulada' AND idMoneda <> ? " +
                    "AND fechaEmisionCuentaPorPagar BETWEEN ? AND ?",
                    new OdbcParameter("Moneda", OdbcType.BigInt) { Value = IdMonedaLocal },
                    ComprasFuncFecha("Inicio", Periodo.FechaInicio), ComprasFuncFecha("Fin", Periodo.FechaFin)),
                RangosAntiguedadActivos = ComprasFuncContar(
                    "SELECT COUNT(*) FROM tblRangoAntiguedad WHERE activoRangoAntiguedad = 1")
            };

            using (DataTable Pagos = ComprasFuncEjecucionConsulta(
                "SELECT COUNT(*) AS cantidad, COALESCE(SUM(montoPagoProveedor * tipoCambioPagoProveedor), 0) AS monto " +
                "FROM tblEncabezadoPagoProveedor WHERE estadoPagoProveedor = 'Programado' AND fechaPagoProveedor <= ?",
                CommandType.Text, new List<OdbcParameter> { ComprasFuncFecha("Fin", Periodo.FechaFin) }))
            {
                Indicadores.PagosProgramados = Convert.ToInt32(Pagos.Rows[0]["cantidad"]);
                Indicadores.MontoPagosProgramados = Math.Round(Convert.ToDecimal(Pagos.Rows[0]["monto"]), 2);
            }
            return Indicadores;
        }

        private string ComprasFuncSumaNaturaleza(string Naturaleza)
        {
            return "(SELECT COALESCE(SUM(" + _MontoLocal + "), 0) FROM tblCuentaPorPagar x " +
                   "INNER JOIN tblConceptoProveedor c ON c.idConceptoProveedor = x.idConceptoProveedor " +
                   "WHERE x.idProveedor = p.idProveedor AND x.estadoCuentaPorPagar <> 'Anulada' " +
                   "AND c.naturalezaConceptoProveedor = '" + Naturaleza + "' AND x.fechaEmisionCuentaPorPagar BETWEEN ? AND ?)";
        }

        private int ComprasFuncContar(string Consulta, params OdbcParameter[] Parametros)
        {
            object Valor = ComprasFuncEjecucionEscalar(Consulta, new List<OdbcParameter>(Parametros), CommandType.Text);
            return Valor == null || Valor == DBNull.Value ? 0 : Convert.ToInt32(Valor);
        }

        private static ClsPeriodoCompras ComprasFuncLeerPeriodo(DataRow Fila)
        {
            return new ClsPeriodoCompras
            {
                IdPeriodo = Convert.ToInt32(Fila["idPeriodoCompras"]),
                Anio = Convert.ToInt32(Fila["anioPeriodoCompras"]),
                Mes = Convert.ToInt32(Fila["mesPeriodoCompras"]),
                FechaInicio = Convert.ToDateTime(Fila["fechaInicioPeriodoCompras"]),
                FechaFin = Convert.ToDateTime(Fila["fechaFinPeriodoCompras"]),
                Estado = Convert.ToString(Fila["estadoPeriodoCompras"])
            };
        }

        internal static OdbcParameter ComprasFuncFecha(string Nombre, DateTime Fecha)
        {
            return new OdbcParameter(Nombre, OdbcType.Date) { Value = Fecha.Date };
        }

        internal static OdbcParameter ComprasFuncEntero(string Nombre, int Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Int) { Value = Valor };
        }
    }
}
