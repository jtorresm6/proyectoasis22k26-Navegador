using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_Componentes.Repositorios;
using CapaModelo_Seguridad.Entidades;
using CapaModelo_Seguridad.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta el cierre de un período en una sola transacción (si un paso falla,
 *  no se guarda nada):
 *   1. Bloquea el período y confirma que sigue abierto.
 *   2. Guarda la foto de saldos por proveedor y la antigüedad de saldos.
 *   3. Traslada los saldos de los proveedores al nuevo mes.
 *   4. Bloquea los documentos del período cerrado.
 *   5. Cierra el período, abre el siguiente y registra la bitácora de Seguridad.
 * ===================================================================
*/

namespace CapaModelo_CierreMovimientos.Repositorios
{
    public class ClsComandosCierreMovimientos : ClsRepositorioMaestroCompras
    {
        private const string _AccionBitacora = "Cierre de período";
        private const string _TablaBitacora = "tblPeriodoCompras";

        // Movimientos posteriores al cierre (ya registrados) que forman el mes en curso del proveedor.
        private const string _MovimientosPosteriores =
            "(SELECT COALESCE(SUM(ABS(x.montoTotalCuentaPorPagar) * x.tipoCambioCuentaPorPagar), 0) FROM tblCuentaPorPagar x " +
            "INNER JOIN tblConceptoProveedor c ON c.idConceptoProveedor = x.idConceptoProveedor " +
            "WHERE x.idProveedor = p.idProveedor AND x.estadoCuentaPorPagar <> 'Anulada' " +
            "AND c.naturalezaConceptoProveedor = '{0}' AND x.fechaEmisionCuentaPorPagar > ?)";

        public ClsResultadoCierre ComprasFuncRegistrarCierre(ClsDatosCierre Datos)
        {
            if (Datos == null || Datos.Periodo == null) throw new ArgumentNullException(nameof(Datos));
            ClsPeriodoCompras Periodo = Datos.Periodo;

            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                ComprasProcConfirmarPeriodoAbierto(Conexion, Transaccion, Periodo.IdPeriodo);

                foreach (ClsSaldoProveedorCierre Saldo in Datos.Saldos)
                {
                    ComprasFuncEjecutarComando(Conexion, Transaccion,
                        "INSERT INTO tblSaldoProveedorPeriodo (idPeriodoCompras, idProveedor, idMoneda, " +
                        "saldoInicialSaldoProveedorPeriodo, cargosSaldoProveedorPeriodo, abonosSaldoProveedorPeriodo, " +
                        "saldoFinalSaldoProveedorPeriodo, tipoCambioSaldoProveedorPeriodo, saldoFinalLocalSaldoProveedorPeriodo) " +
                        "VALUES (?, ?, ?, ?, ?, ?, ?, 1, ?)",
                        ComprasFuncEntero(Periodo.IdPeriodo), ComprasFuncEntero(Saldo.IdProveedor), ComprasFuncLargo(Datos.IdMonedaLocal),
                        ComprasFuncMonto(Saldo.SaldoInicial), ComprasFuncMonto(Saldo.Cargos), ComprasFuncMonto(Saldo.Abonos),
                        ComprasFuncMonto(Saldo.SaldoFinal), ComprasFuncMonto(Saldo.SaldoFinal));
                }

                foreach (ClsAntiguedadCierre Antiguedad in Datos.Antiguedad)
                {
                    ComprasFuncEjecutarComando(Conexion, Transaccion,
                        "INSERT INTO tblAntiguedadSaldo (idPeriodoCompras, idProveedor, idMoneda, idRangoAntiguedad, " +
                        "montoAntiguedadSaldo, montoLocalAntiguedadSaldo, cantidadDocumentosAntiguedadSaldo) VALUES (?, ?, ?, ?, ?, ?, ?)",
                        ComprasFuncEntero(Periodo.IdPeriodo), ComprasFuncEntero(Antiguedad.IdProveedor), ComprasFuncLargo(Datos.IdMonedaLocal),
                        ComprasFuncEntero(Antiguedad.IdRango), ComprasFuncMonto(Antiguedad.Monto), ComprasFuncMonto(Antiguedad.Monto),
                        ComprasFuncEntero(Antiguedad.CantidadDocumentos));
                }

                ComprasProcTrasladarSaldosProveedor(Conexion, Transaccion, Periodo);
                int Bloqueados = ComprasFuncBloquearDocumentos(Conexion, Transaccion, Periodo.FechaFin);

                var Resultado = new ClsResultadoCierre
                {
                    Periodo = Periodo,
                    Proveedores = Datos.Saldos.Count,
                    TotalCargos = Datos.Saldos.Sum(Saldo => Saldo.Cargos),
                    TotalAbonos = Datos.Saldos.Sum(Saldo => Saldo.Abonos),
                    SaldoFinal = Datos.Saldos.Sum(Saldo => Saldo.SaldoFinal),
                    DocumentosBloqueados = Bloqueados
                };

                ComprasFuncEjecutarComando(Conexion, Transaccion,
                    "UPDATE tblPeriodoCompras SET estadoPeriodoCompras = 'Cerrado', totalCargosPeriodoCompras = ?, " +
                    "totalAbonosPeriodoCompras = ?, fechaCierrePeriodoCompras = NOW(), idUsuarioCierrePeriodoCompras = ? " +
                    "WHERE idPeriodoCompras = ?",
                    ComprasFuncMonto(Resultado.TotalCargos), ComprasFuncMonto(Resultado.TotalAbonos),
                    new OdbcParameter("Usuario", OdbcType.Int) { Value = Datos.IdUsuario.HasValue ? (object)Datos.IdUsuario.Value : DBNull.Value },
                    ComprasFuncEntero(Periodo.IdPeriodo));

                DateTime InicioSiguiente = Periodo.FechaFin.AddDays(1);
                DateTime FinSiguiente = InicioSiguiente.AddMonths(1).AddDays(-1);
                ComprasFuncEjecutarComando(Conexion, Transaccion,
                    "INSERT INTO tblPeriodoCompras (anioPeriodoCompras, mesPeriodoCompras, fechaInicioPeriodoCompras, fechaFinPeriodoCompras) " +
                    "SELECT ?, ?, ?, ? FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM tblPeriodoCompras WHERE anioPeriodoCompras = ? AND mesPeriodoCompras = ?)",
                    ComprasFuncEntero(InicioSiguiente.Year), ComprasFuncEntero(InicioSiguiente.Month),
                    ClsConsultasCierreMovimientos.ComprasFuncFecha("Inicio", InicioSiguiente),
                    ClsConsultasCierreMovimientos.ComprasFuncFecha("Fin", FinSiguiente),
                    ComprasFuncEntero(InicioSiguiente.Year), ComprasFuncEntero(InicioSiguiente.Month));
                Resultado.SiguientePeriodo = new ClsPeriodoCompras { Anio = InicioSiguiente.Year, Mes = InicioSiguiente.Month }.Descripcion;

                new ClsRepositorioBitacora().SeguridadMetAgregar(new ClsBitacora
                {
                    IdUsuario = Datos.IdUsuario,
                    AccionBitacora = _AccionBitacora,
                    TablaBitacora = _TablaBitacora,
                    IdRegistroBitacora = Periodo.IdPeriodo,
                    DetallesBitacora = "Cierre de " + Periodo.Descripcion + ": " + Resultado.Proveedores + " proveedor(es), cargos Q " +
                        Resultado.TotalCargos.ToString("N2") + ", abonos Q " + Resultado.TotalAbonos.ToString("N2") +
                        ", saldo final Q " + Resultado.SaldoFinal.ToString("N2") + ", " + Bloqueados + " documento(s) bloqueado(s).",
                    IpBitacora = string.IsNullOrWhiteSpace(Datos.Ip) ? "No disponible" : Datos.Ip,
                    FechaHoraBitacora = DateTime.Now
                }, Conexion, Transaccion);

                return Resultado;
            }, true);
        }

        private static void ComprasProcConfirmarPeriodoAbierto(OdbcConnection Conexion, OdbcTransaction Transaccion, int IdPeriodo)
        {
            using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion,
                "SELECT estadoPeriodoCompras FROM tblPeriodoCompras WHERE idPeriodoCompras = ? FOR UPDATE", CommandType.Text,
                new List<OdbcParameter> { ComprasFuncEntero(IdPeriodo) }))
            {
                object Estado = Comando.ExecuteScalar();
                if (Estado == null) throw new InvalidOperationException("El período seleccionado no existe.");
                if (!string.Equals(Convert.ToString(Estado), "Abierto", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("El período ya fue cerrado por otro usuario.");
            }
        }

        private static void ComprasProcTrasladarSaldosProveedor(OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsPeriodoCompras Periodo)
        {
            string Saldo = "(SELECT s.{0} FROM tblSaldoProveedorPeriodo s WHERE s.idPeriodoCompras = ? AND s.idProveedor = p.idProveedor)";
            ComprasFuncEjecutarComando(Conexion, Transaccion,
                "UPDATE tblProveedor p SET " +
                "saldoAnteriorProveedor = COALESCE(" + string.Format(Saldo, "saldoFinalLocalSaldoProveedorPeriodo") + ", 0), " +
                "cargosAcumuladosProveedor = cargosAcumuladosProveedor + COALESCE(" + string.Format(Saldo, "cargosSaldoProveedorPeriodo") + ", 0), " +
                "abonosAcumuladosProveedor = abonosAcumuladosProveedor + COALESCE(" + string.Format(Saldo, "abonosSaldoProveedorPeriodo") + ", 0), " +
                "cargosMesProveedor = " + string.Format(_MovimientosPosteriores, "Cargo") + ", " +
                "abonosMesProveedor = " + string.Format(_MovimientosPosteriores, "Abono"),
                ComprasFuncEntero(Periodo.IdPeriodo), ComprasFuncEntero(Periodo.IdPeriodo), ComprasFuncEntero(Periodo.IdPeriodo),
                ClsConsultasCierreMovimientos.ComprasFuncFecha("FinCargos", Periodo.FechaFin),
                ClsConsultasCierreMovimientos.ComprasFuncFecha("FinAbonos", Periodo.FechaFin));

            ComprasFuncEjecutarComando(Conexion, Transaccion,
                "UPDATE tblProveedor SET saldoActualProveedor = saldoAnteriorProveedor + cargosMesProveedor - abonosMesProveedor");
        }

        private static int ComprasFuncBloquearDocumentos(OdbcConnection Conexion, OdbcTransaction Transaccion, DateTime FechaFin)
        {
            string[] Sentencias =
            {
                "UPDATE tblCuentaPorPagar SET bloqueadoCuentaPorPagar = 1 WHERE bloqueadoCuentaPorPagar = 0 AND fechaEmisionCuentaPorPagar <= ?",
                "UPDATE tblEncabezadoCompra SET bloqueadoCompra = 1 WHERE bloqueadoCompra = 0 AND fechaCompra <= ?",
                "UPDATE tblDevolucionCompra SET bloqueadoDevolucionCompra = 1 WHERE bloqueadoDevolucionCompra = 0 AND fechaDevolucionCompra <= ?",
                "UPDATE tblEncabezadoPagoProveedor SET bloqueadoPagoProveedor = 1 WHERE bloqueadoPagoProveedor = 0 " +
                    "AND estadoPagoProveedor <> 'Programado' AND fechaPagoProveedor <= ?",
                // Las órdenes pendientes o autorizadas siguen su trámite en el mes siguiente.
                "UPDATE tblEncabezadoOrdenCompra SET bloqueadoOrdenCompra = 1 WHERE bloqueadoOrdenCompra = 0 " +
                    "AND estadoOrdenCompra IN ('Rechazada','Anulada','Recibida') AND fechaEmisionOrdenCompra <= ?"
            };
            return Sentencias.Sum(Sentencia => ComprasFuncEjecutarComando(Conexion, Transaccion, Sentencia,
                ClsConsultasCierreMovimientos.ComprasFuncFecha("Fin", FechaFin)));
        }

        private static int ComprasFuncEjecutarComando(OdbcConnection Conexion, OdbcTransaction Transaccion, string Sentencia,
            params OdbcParameter[] Parametros)
        {
            using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, Sentencia, CommandType.Text, Parametros.ToList()))
                return Comando.ExecuteNonQuery();
        }

        private static OdbcParameter ComprasFuncEntero(int Valor) => new OdbcParameter { OdbcType = OdbcType.Int, Value = Valor };
        private static OdbcParameter ComprasFuncLargo(long Valor) => new OdbcParameter { OdbcType = OdbcType.BigInt, Value = Valor };
        private static OdbcParameter ComprasFuncMonto(decimal Valor) =>
            new OdbcParameter { OdbcType = OdbcType.Decimal, Precision = 12, Scale = 4, Value = Math.Round(Valor, 4) };
    }
}
