using CapaModelo_GeneracionPagos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Repositorio del proceso Generación de Pagos (8012). Consulta las
 *  obligaciones por pagar y ejecuta, cada una dentro de su propia
 *  transacción ODBC, los pasos del proceso: preparar la orden,
 *  registrar la aprobación, emitir el cheque o enviar la
 *  transferencia y registrar el movimiento bancario con su póliza.
 * ================================================================
 */

namespace CapaModelo_GeneracionPagos.Repositorio
{
    public class ClsRepositorioGeneracionPagos : ClsSentencias
    {
        private const string FormatoFecha = "yyyy-MM-dd";
        private const string FormatoFechaHora = "yyyy-MM-dd HH:mm:ss";
        private const string MonedaBase = "GTQ";

        private const string EstadoCompraAprobada = "APROBADA";
        private const string EstadoPlanillaRecibida = "RECIBIDA";
        private const string EstadoSolicitudTransporte = "SOLICITADO";
        private const string EstadoSolicitudPagada = "PAGADO";
        private const string EstadoServicioCancelado = "Cancelado";

        private const string PatronCheque = "%cheque%";
        private const string PatronTransferencia = "%transfer%";
        private const int LargoMaximoMedio = 30;

        private const string EstadosVigentes = "'" + ClsValoresPago.EstadoPendiente + "','" + ClsValoresPago.EstadoAutorizada + "','" + ClsValoresPago.EstadoEmitida + "'";
        private const string EstadosConPagadas = EstadosVigentes + ",'" + ClsValoresPago.EstadoPagada + "'";

        private const string PrefijoOrden = "OP-";
        private const string PrefijoMovimiento = "MB-";

        private const string TablaOrden = "tblOrdenPago";
        private const string TablaMovimiento = "tblMovimientoBancario";

        private const string AccionPreparar = "PREPARAR_ORDEN_PAGO";
        private const string AccionAutorizar = "AUTORIZAR_ORDEN_PAGO";
        private const string AccionRechazar = "RECHAZAR_ORDEN_PAGO";
        private const string AccionEmitirCheque = "EMITIR_CHEQUE";
        private const string AccionEnviarTransferencia = "ENVIAR_TRANSFERENCIA";
        private const string AccionRegistrarMovimiento = "REGISTRAR_MOVIMIENTO";

        private const string SelectObligaciones =
            "SELECT t.TipoOrigen, t.Origen, t.IdOrigen, t.Documento, t.Beneficiario, t.Vencimiento, t.Moneda, t.IdMoneda, t.Pendiente FROM (" +
            "SELECT '" + ClsValoresPago.OrigenCompra + "' AS TipoOrigen, 'Compras y CxP' AS Origen, oc.Pk_OrdenCompraRecibida AS IdOrigen, " +
            "oc.referenciaExternaOrdenCompraRecibida AS Documento, oc.proveedorOrdenCompraRecibida AS Beneficiario, " +
            "oc.fechaVencimientoOrdenCompraRecibida AS Vencimiento, m.codigoMoneda AS Moneda, m.idMoneda AS IdMoneda, " +
            "oc.importeOrdenCompraRecibida - IFNULL((SELECT SUM(o.importeOrdenPago) FROM tblOrdenPago o " +
            "WHERE o.Fk_OrdenCompraRecibida_Codigo = oc.Pk_OrdenCompraRecibida AND o.estadoOrdenPago IN (" + EstadosConPagadas + ")), 0) AS Pendiente " +
            "FROM tblOrdenCompraRecibida oc INNER JOIN tblMoneda m ON m.idMoneda = oc.Fk_Moneda_Codigo " +
            "WHERE oc.estadoOrdenCompraRecibida = '" + EstadoCompraAprobada + "' " +
            "UNION ALL " +
            "SELECT '" + ClsValoresPago.OrigenPlanilla + "', 'Recursos Humanos', dp.idDetallePlanillaRecibida, " +
            "CONCAT(pr.referenciaExternaPlanillaRecibida, ' / ', dp.referenciaEmpleadoDetallePlanillaRecibida), " +
            "dp.beneficiarioDetallePlanillaRecibida, pr.finPeriodoPlanillaRecibida, m.codigoMoneda, m.idMoneda, " +
            "dp.importeNetoDetallePlanillaRecibida - IFNULL((SELECT SUM(o.importeOrdenPago) FROM tblOrdenPago o " +
            "WHERE o.Fk_DetallePlanillaRecibida_Codigo = dp.idDetallePlanillaRecibida AND o.estadoOrdenPago IN (" + EstadosConPagadas + ")), 0) " +
            "FROM tblDetallePlanillaRecibida dp INNER JOIN tblPlanillaRecibida pr ON pr.Pk_PlanillaRecibida = dp.Fk_PlanillaRecibida_Codigo " +
            "INNER JOIN tblMoneda m ON m.idMoneda = pr.Fk_Moneda_Codigo WHERE pr.estadoPlanillaRecibida = '" + EstadoPlanillaRecibida + "' " +
            "UNION ALL " +
            "SELECT '" + ClsValoresPago.OrigenLogistica + "', 'Logistica', s.idSolicitudPagoTransporte, s.codigoSolicitudPagoTransporte, e.nombreEmpresaTransportista, " +
            "t.fechaEstimadaEntregaServicioTransporteExterno, m.codigoMoneda, m.idMoneda, " +
            "c.costoTotalCostoTransporte - IFNULL((SELECT SUM(o.importeOrdenPago) FROM tblOrdenPago o " +
            "WHERE o.Fk_SolicitudPagoTransporte_Codigo = s.idSolicitudPagoTransporte AND o.estadoOrdenPago IN (" + EstadosConPagadas + ")), 0) " +
            "FROM tblSolicitudPagoTransporte s " +
            "INNER JOIN tblEstadoSolicitudPago es ON es.idEstadoSolicitudPago = s.Fk_Estado_Solicitud_Pago_Codigo " +
            "INNER JOIN tblServicioTransporteExterno t ON t.idServicioTransporteExterno = s.Fk_Servicio_Transporte_Externo_Codigo " +
            "INNER JOIN tblEmpresaTransportista e ON e.idEmpresaTransportista = t.Fk_Empresa_Transportista_Codigo " +
            "INNER JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo = t.idServicioTransporteExterno " +
            "INNER JOIN tblMoneda m ON m.codigoMoneda = c.monedaCostoTransporte " +
            "WHERE es.codigoEstadoSolicitudPago = '" + EstadoSolicitudTransporte + "' AND t.estadoServicioTransporteExterno <> '" + EstadoServicioCancelado + "'" +
            ") t WHERE t.Pendiente > 0";

        private const string FiltroObligacionOrigen = " AND t.TipoOrigen = ?";
        private const string FiltroObligacionMoneda = " AND t.Moneda = ?";
        private const string FiltroObligacionTexto = " AND (t.Documento LIKE ? OR t.Beneficiario LIKE ?)";
        private const string OrdenObligaciones = " ORDER BY t.Vencimiento, t.Documento";

        private const string SelectCuentas =
            "SELECT c.idCuentaBancaria, CONCAT(b.nombreBanco, ' ', c.numeroCuentaBancaria) AS Cuenta, " +
            "d.saldoDisponibleDisponibilidadDiaria - IFNULL((SELECT SUM(o.importeOrdenPago) FROM tblOrdenPago o " +
            "WHERE o.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND o.estadoOrdenPago IN (" + EstadosVigentes + ")), 0) AS Disponible " +
            "FROM tblCuentaBancaria c INNER JOIN tblBanco b ON b.idBanco = c.Fk_Banco_Codigo " +
            "INNER JOIN tblDisponibilidadDiaria d ON d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.fechaDisponibilidadDiaria = CURDATE() " +
            "WHERE c.is_active = 1 AND c.Fk_Moneda_Codigo = ? ORDER BY b.nombreBanco, c.numeroCuentaBancaria";

        private const string SelectTipoCambio =
            "SELECT t.idTipoCambio, t.valorTipoCambio FROM tblTipoCambio t " +
            "INNER JOIN tblMoneda mo ON mo.idMoneda = t.Fk_Moneda_Origen_Codigo " +
            "INNER JOIN tblMoneda md ON md.idMoneda = t.Fk_Moneda_Destino_Codigo " +
            "WHERE mo.codigoMoneda = ? AND md.codigoMoneda = '" + MonedaBase + "' AND t.fechaTipoCambio <= CURDATE() " +
            "ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC LIMIT 1";

        private const string SelectOrdenes =
            "SELECT o.idOrdenPago, o.referenciaOrdenPago, o.beneficiarioOrdenPago, m.codigoMoneda, o.importeOrdenPago, o.estadoOrdenPago " +
            "FROM tblOrdenPago o INNER JOIN tblMoneda m ON m.idMoneda = o.Fk_Moneda_Codigo ORDER BY o.idOrdenPago DESC LIMIT 200";

        private const string SelectDatosOrden =
            "SELECT o.idOrdenPago, o.referenciaOrdenPago, o.beneficiarioOrdenPago, o.importeOrdenPago, o.medioOrdenPago, o.estadoOrdenPago, " +
            "o.Fk_CuentaBancaria_Codigo, CONCAT(b.nombreBanco, ' ', c.numeroCuentaBancaria, ' (', m.codigoMoneda, ')') AS Cuenta, " +
            "o.Fk_Moneda_Codigo, m.codigoMoneda " +
            "FROM tblOrdenPago o INNER JOIN tblCuentaBancaria c ON c.idCuentaBancaria = o.Fk_CuentaBancaria_Codigo " +
            "INNER JOIN tblBanco b ON b.idBanco = c.Fk_Banco_Codigo INNER JOIN tblMoneda m ON m.idMoneda = o.Fk_Moneda_Codigo " +
            "WHERE o.idOrdenPago = ?";

        private const string SelectPolitica =
            "SELECT Pk_PoliticaAprobacion, cantidadAprobacionesPoliticaAprobacion FROM tblPoliticaAprobacion " +
            "WHERE tipoOperacionPoliticaAprobacion = 'ORDEN_PAGO' AND is_active = 1 AND Fk_Moneda_Codigo = ? " +
            "AND ? BETWEEN montoMinimoPoliticaAprobacion AND montoMaximoPoliticaAprobacion " +
            "AND fechaInicioPoliticaAprobacion <= CURDATE() AND (fechaFinPoliticaAprobacion IS NULL OR fechaFinPoliticaAprobacion >= CURDATE()) " +
            "ORDER BY montoMinimoPoliticaAprobacion DESC LIMIT 1";

        private const string SelectAprobaciones =
            "SELECT identificadorAprobadorAprobacion, decisionAprobacion, created_at FROM tblAprobacion " +
            "WHERE Fk_OrdenPago_Codigo = ? ORDER BY idAprobacion";

        private const string SelectMonedas = "SELECT idMoneda, codigoMoneda FROM tblMoneda WHERE is_active = 1 ORDER BY codigoMoneda";

        private const string SelectTiposPago = "SELECT idMetodoPago, nombreMetodoPago FROM tblMetodoPago WHERE is_active = 1 ORDER BY nombreMetodoPago";

        private const string SelectTipoPagoDeMedio =
            "SELECT idMetodoPago FROM tblMetodoPago WHERE is_active = 1 AND nombreMetodoPago LIKE ? ORDER BY idMetodoPago LIMIT 1";

        // Otros medios (efectivo, depósito, etc.): se buscan por el nombre exacto con que se guardó la orden
        private const string SelectTipoPagoPorNombre =
            "SELECT idMetodoPago FROM tblMetodoPago WHERE is_active = 1 AND LEFT(nombreMetodoPago, 30) = ? ORDER BY idMetodoPago LIMIT 1";

        // Solo los usuarios activos con el rol de Tesorero pueden autorizar o rechazar órdenes de pago
        private const string RolAutorizador = "Tesorero";

        private const string SelectAutorizadores =
            "SELECT DISTINCT u.nombreUsuario FROM tblusuario u " +
            "INNER JOIN tblusuariorol ur ON ur.idUsuario = u.idUsuario " +
            "INNER JOIN tblrol r ON r.idRol = ur.idRol " +
            "WHERE u.is_active = 1 AND r.is_active = 1 AND r.nombreRol = ? ORDER BY u.nombreUsuario";

        private const string SelectEsAutorizador =
            "SELECT COUNT(*) FROM tblusuario u " +
            "INNER JOIN tblusuariorol ur ON ur.idUsuario = u.idUsuario " +
            "INNER JOIN tblrol r ON r.idRol = ur.idRol " +
            "WHERE u.nombreUsuario = ? AND u.is_active = 1 AND r.is_active = 1 AND r.nombreRol = ?";

        private const string SelectChequera =
            "SELECT idChequera, serieChequera, siguienteNumeroChequera FROM tblChequera " +
            "WHERE Fk_CuentaBancaria_Codigo = ? AND is_active = 1 AND siguienteNumeroChequera <= numeroFinalChequera " +
            "ORDER BY idChequera LIMIT 1";

        private const string SelectMovimiento =
            "SELECT m.idMovimientoBancario, m.referenciaMovimientoBancario, m.importeMovimientoBancario, m.tipoCambioAplicadoMovimientoBancario, " +
            "m.importeGTQMovimientoBancario, p.idPolizaBancaria, p.estadoPolizaBancaria FROM tblMovimientoBancario m " +
            "LEFT JOIN tblPolizaBancaria p ON p.Fk_MovimientoBancario_Codigo = m.idMovimientoBancario WHERE m.Fk_OrdenPago_Codigo = ?";

        private const string SelectPoliza =
            "SELECT d.codigoCuentaContableDetallePolizaBancaria, IFNULL(c.nombreCuenta, '') AS nombreCuenta, " +
            "d.debeDetallePolizaBancaria, d.haberDetallePolizaBancaria " +
            "FROM tblDetallePolizaBancaria d LEFT JOIN tblCuenta c ON c.codigoCuenta = d.codigoCuentaContableDetallePolizaBancaria " +
            "WHERE d.Fk_PolizaBancaria_Codigo = ? ORDER BY d.idDetallePolizaBancaria";

        private const string SelectConfiguracionPoliza =
            "SELECT cp.codigoCuentaContableBancoConfiguracionPoliza, IFNULL(cb.nombreCuenta, '') AS nombreCuentaBanco, " +
            "cp.codigoCuentaContableContrapartidaConfiguracionPoliza, IFNULL(cc.nombreCuenta, '') AS nombreCuentaContrapartida, cp.Fk_TipoTransaccion_Codigo " +
            "FROM tblConfiguracionPoliza cp INNER JOIN tblTipoTransaccion tt ON tt.idTipoTransaccion = cp.Fk_TipoTransaccion_Codigo " +
            "LEFT JOIN tblCuenta cb ON cb.codigoCuenta = cp.codigoCuentaContableBancoConfiguracionPoliza " +
            "LEFT JOIN tblCuenta cc ON cc.codigoCuenta = cp.codigoCuentaContableContrapartidaConfiguracionPoliza " +
            "WHERE cp.Fk_CuentaBancaria_Codigo = ? AND cp.is_active = 1 AND tt.is_active = 1 AND tt.naturalezaTipoTransaccion IN ('EGRESO','AMBOS') " +
            "ORDER BY (tt.naturalezaTipoTransaccion = 'EGRESO') DESC, cp.idConfiguracionPoliza LIMIT 1";

        private const string SelectCompraBloqueada =
            "SELECT importeOrdenCompraRecibida, estadoOrdenCompraRecibida, Fk_Moneda_Codigo, proveedorOrdenCompraRecibida " +
            "FROM tblOrdenCompraRecibida WHERE Pk_OrdenCompraRecibida = ? FOR UPDATE";

        private const string SelectPlanillaBloqueada =
            "SELECT d.importeNetoDetallePlanillaRecibida, p.estadoPlanillaRecibida, p.Fk_Moneda_Codigo, d.beneficiarioDetallePlanillaRecibida " +
            "FROM tblDetallePlanillaRecibida d INNER JOIN tblPlanillaRecibida p ON p.Pk_PlanillaRecibida = d.Fk_PlanillaRecibida_Codigo " +
            "WHERE d.idDetallePlanillaRecibida = ? FOR UPDATE";

        private const string SelectPagadoCompra =
            "SELECT IFNULL(SUM(importeOrdenPago), 0) FROM tblOrdenPago WHERE Fk_OrdenCompraRecibida_Codigo = ? AND estadoOrdenPago IN (" + EstadosConPagadas + ")";

        private const string SelectSolicitudBloqueada =
            "SELECT c.costoTotalCostoTransporte, m.idMoneda, e.nombreEmpresaTransportista, es.codigoEstadoSolicitudPago, t.estadoServicioTransporteExterno " +
            "FROM tblSolicitudPagoTransporte s " +
            "INNER JOIN tblEstadoSolicitudPago es ON es.idEstadoSolicitudPago = s.Fk_Estado_Solicitud_Pago_Codigo " +
            "INNER JOIN tblServicioTransporteExterno t ON t.idServicioTransporteExterno = s.Fk_Servicio_Transporte_Externo_Codigo " +
            "INNER JOIN tblEmpresaTransportista e ON e.idEmpresaTransportista = t.Fk_Empresa_Transportista_Codigo " +
            "INNER JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo = t.idServicioTransporteExterno " +
            "INNER JOIN tblMoneda m ON m.codigoMoneda = c.monedaCostoTransporte " +
            "WHERE s.idSolicitudPagoTransporte = ? FOR UPDATE OF s";

        private const string SelectPagadoTransporte =
            "SELECT IFNULL(SUM(importeOrdenPago), 0) FROM tblOrdenPago WHERE Fk_SolicitudPagoTransporte_Codigo = ? AND estadoOrdenPago IN (" + EstadosConPagadas + ")";

        private const string SelectCobradoTransporte =
            "SELECT IFNULL(SUM(importeOrdenPago), 0) FROM tblOrdenPago WHERE Fk_SolicitudPagoTransporte_Codigo = ? AND estadoOrdenPago = '" + ClsValoresPago.EstadoPagada + "'";

        private const string SelectCostoSolicitud =
            "SELECT c.costoTotalCostoTransporte FROM tblSolicitudPagoTransporte s " +
            "INNER JOIN tblCostoTransporte c ON c.Fk_Servicio_Transporte_Externo_Codigo = s.Fk_Servicio_Transporte_Externo_Codigo " +
            "WHERE s.idSolicitudPagoTransporte = ?";

        private const string SelectIdEstadoSolicitud = "SELECT idEstadoSolicitudPago FROM tblEstadoSolicitudPago WHERE codigoEstadoSolicitudPago = ?";

        private const string SelectPagadoPlanilla =
            "SELECT IFNULL(SUM(importeOrdenPago), 0) FROM tblOrdenPago WHERE Fk_DetallePlanillaRecibida_Codigo = ? AND estadoOrdenPago IN (" + EstadosConPagadas + ")";

        private const string SelectCuentaBloqueada =
            "SELECT c.Fk_Moneda_Codigo, d.saldoDisponibleDisponibilidadDiaria FROM tblCuentaBancaria c " +
            "LEFT JOIN tblDisponibilidadDiaria d ON d.Fk_CuentaBancaria_Codigo = c.idCuentaBancaria AND d.fechaDisponibilidadDiaria = CURDATE() " +
            "WHERE c.idCuentaBancaria = ? AND c.is_active = 1 FOR UPDATE";

        private const string SelectComprometidoCuenta =
            "SELECT IFNULL(SUM(importeOrdenPago), 0) FROM tblOrdenPago WHERE Fk_CuentaBancaria_Codigo = ? AND estadoOrdenPago IN (" + EstadosVigentes + ")";

        private const string SelectOrdenBloqueada =
            "SELECT referenciaOrdenPago, beneficiarioOrdenPago, importeOrdenPago, medioOrdenPago, estadoOrdenPago, " +
            "Fk_CuentaBancaria_Codigo, Fk_Moneda_Codigo FROM tblOrdenPago WHERE idOrdenPago = ? FOR UPDATE";

        private const string SelectOrdenConDocumento =
            "SELECT o.referenciaOrdenPago, o.beneficiarioOrdenPago, o.importeOrdenPago, o.medioOrdenPago, o.estadoOrdenPago, o.Fk_CuentaBancaria_Codigo, " +
            "o.Fk_SolicitudPagoTransporte_Codigo, m.codigoMoneda, " +
            "COALESCE(oc.referenciaExternaOrdenCompraRecibida, pr.referenciaExternaPlanillaRecibida, sp.codigoSolicitudPagoTransporte) AS Documento " +
            "FROM tblOrdenPago o INNER JOIN tblMoneda m ON m.idMoneda = o.Fk_Moneda_Codigo " +
            "LEFT JOIN tblOrdenCompraRecibida oc ON oc.Pk_OrdenCompraRecibida = o.Fk_OrdenCompraRecibida_Codigo " +
            "LEFT JOIN tblDetallePlanillaRecibida dp ON dp.idDetallePlanillaRecibida = o.Fk_DetallePlanillaRecibida_Codigo " +
            "LEFT JOIN tblPlanillaRecibida pr ON pr.Pk_PlanillaRecibida = dp.Fk_PlanillaRecibida_Codigo " +
            "LEFT JOIN tblSolicitudPagoTransporte sp ON sp.idSolicitudPagoTransporte = o.Fk_SolicitudPagoTransporte_Codigo " +
            "WHERE o.idOrdenPago = ? FOR UPDATE OF o";

        private const string SelectChequeraBloqueada =
            "SELECT idChequera, serieChequera, siguienteNumeroChequera FROM tblChequera " +
            "WHERE Fk_CuentaBancaria_Codigo = ? AND is_active = 1 AND siguienteNumeroChequera <= numeroFinalChequera ORDER BY idChequera LIMIT 1 FOR UPDATE";

        private const string SelectAprobacionesDelAutorizador =
            "SELECT COUNT(*) FROM tblAprobacion WHERE Fk_OrdenPago_Codigo = ? AND identificadorAprobadorAprobacion = ?";

        private const string SelectAprobadas =
            "SELECT COUNT(DISTINCT identificadorAprobadorAprobacion) FROM tblAprobacion WHERE Fk_OrdenPago_Codigo = ? AND decisionAprobacion = '" + ClsValoresPago.DecisionAprobada + "'";

        private const string SelectChequesDeOrden = "SELECT COUNT(*) FROM tblCheque WHERE Fk_OrdenPago_Codigo = ?";
        private const string SelectMovimientosDeOrden = "SELECT COUNT(*) FROM tblMovimientoBancario WHERE Fk_OrdenPago_Codigo = ?";

        private const string InsertOrden =
            "INSERT INTO tblOrdenPago (referenciaOrdenPago, beneficiarioOrdenPago, importeOrdenPago, medioOrdenPago, estadoOrdenPago, " +
            "fechaProgramadaOrdenPago, identificadorSolicitanteOrdenPago, Fk_CuentaBancaria_Codigo, Fk_Moneda_Codigo, Fk_OrdenCompraRecibida_Codigo, Fk_DetallePlanillaRecibida_Codigo, Fk_SolicitudPagoTransporte_Codigo) " +
            "VALUES (?, ?, ?, ?, '" + ClsValoresPago.EstadoPendiente + "', ?, ?, ?, ?, ?, ?, ?)";

        private const string InsertAprobacion =
            "INSERT INTO tblAprobacion (decisionAprobacion, comentarioAprobacion, identificadorAprobadorAprobacion, referenciaEvidencia2FAAprobacion, " +
            "fechaValidacion2FAAprobacion, Fk_OrdenPago_Codigo, Fk_PoliticaAprobacion_Codigo) VALUES (?, ?, ?, ?, ?, ?, ?)";

        private const string InsertCheque =
            "INSERT INTO tblCheque (numeroCheque, beneficiarioCheque, estadoCheque, referenciaEvidencia2FACheque, fechaValidacion2FACheque, " +
            "identificadorGeneradorCheque, Fk_Chequera_Codigo, Fk_OrdenPago_Codigo) VALUES (?, ?, 'GENERADO', ?, ?, ?, ?, ?)";

        private const string InsertMovimiento =
            "INSERT INTO tblMovimientoBancario (conceptoMovimientoBancario, importeMovimientoBancario, importeGTQMovimientoBancario, " +
            "tipoCambioAplicadoMovimientoBancario, sentidoMovimientoBancario, fechaOperacionMovimientoBancario, fechaValorMovimientoBancario, origenMovimientoBancario, " +
            "Fk_CuentaBancaria_Codigo, Fk_TipoTransaccion_Codigo, Fk_TipoCambio_Codigo, Fk_OrdenPago_Codigo, idMetodoPago) " +
            "VALUES (?, ?, ?, ?, -1, CURDATE(), CURDATE(), 'ORDEN_PAGO', ?, ?, ?, ?, ?)";

        private const string InsertPoliza = "INSERT INTO tblPolizaBancaria (fechaPolizaBancaria, estadoPolizaBancaria, Fk_MovimientoBancario_Codigo) VALUES (CURDATE(), 'BORRADOR', ?)";

        private const string InsertDetallePoliza =
            "INSERT INTO tblDetallePolizaBancaria (codigoCuentaContableDetallePolizaBancaria, debeDetallePolizaBancaria, haberDetallePolizaBancaria, Fk_PolizaBancaria_Codigo) VALUES (?, ?, ?, ?)";

        private const string InsertBitacora =
            "INSERT INTO tblbitacora (idUsuario, accionBitacora, tablaBitacora, idRegistroBitacora, detallesBitacora, ipBitacora, fechaHoraBitacora) " +
            "VALUES (?, ?, ?, ?, ?, ?, ?)";

        private const string UpdateReferenciaOrden = "UPDATE tblOrdenPago SET referenciaOrdenPago = ? WHERE idOrdenPago = ?";
        private const string UpdateReferenciaMovimiento = "UPDATE tblMovimientoBancario SET referenciaMovimientoBancario = ? WHERE idMovimientoBancario = ?";
        private const string UpdateSiguienteCheque = "UPDATE tblChequera SET siguienteNumeroChequera = siguienteNumeroChequera + 1 WHERE idChequera = ?";
        private const string UpdateSolicitudPagada =
            "UPDATE tblSolicitudPagoTransporte SET Fk_Estado_Solicitud_Pago_Codigo = ?, referenciaBancariaSolicitudPagoTransporte = ?, " +
            "fechaPagoSolicitudPagoTransporte = CURDATE() WHERE idSolicitudPagoTransporte = ?";
        private const string UpdateEstadoOrden = "UPDATE tblOrdenPago SET estadoOrdenPago = ? WHERE idOrdenPago = ? AND estadoOrdenPago = ?";
        private const string UltimoId = "SELECT LAST_INSERT_ID()";

        private sealed class ClsDatosObligacion
        {
            public decimal Importe { get; set; }
            public long IdMoneda { get; set; }
            public string Beneficiario { get; set; }
            public decimal PagadoPrevio { get; set; }
        }

        private sealed class ClsCambioAplicado
        {
            public long? IdTipoCambio { get; set; }
            public decimal Valor { get; set; }
        }

        public static decimal BancosFuncCalcularEquivalente(decimal Monto, decimal TipoCambio)
        {
            return Math.Round(Monto * TipoCambio, 4);
        }

        public DataTable BancosMetConsultarObligaciones(string TipoOrigen, string CodigoMoneda, string Texto)
        {
            // Solo se agregan los filtros que traen valor: así nunca se envía un parámetro vacío al controlador ODBC
            var Sentencia = new System.Text.StringBuilder(SelectObligaciones);
            var Valores = new List<object>();

            if (!string.IsNullOrEmpty(TipoOrigen))
            {
                Sentencia.Append(FiltroObligacionOrigen);
                Valores.Add(TipoOrigen);
            }
            if (!string.IsNullOrEmpty(CodigoMoneda))
            {
                Sentencia.Append(FiltroObligacionMoneda);
                Valores.Add(CodigoMoneda);
            }
            if (!string.IsNullOrEmpty(Texto))
            {
                string Patron = "%" + Texto + "%";
                Sentencia.Append(FiltroObligacionTexto);
                Valores.Add(Patron);
                Valores.Add(Patron);
            }
            Sentencia.Append(OrdenObligaciones);

            return BancosMetEjecucionConsulta(Sentencia.ToString(), CommandType.Text, BancosFuncParametros(Valores.ToArray()));
        }

        public DataTable BancosMetConsultarCuentas(long IdMoneda)
        {
            return BancosMetEjecucionConsulta(SelectCuentas, CommandType.Text, BancosFuncParametros(IdMoneda));
        }

        public DataTable BancosMetConsultarOrdenes()
        {
            return BancosMetEjecucionConsulta(SelectOrdenes, CommandType.Text, BancosFuncParametros());
        }

        public DataTable BancosMetObtenerDatosOrden(long IdOrden)
        {
            return BancosMetEjecucionConsulta(SelectDatosOrden, CommandType.Text, BancosFuncParametros(IdOrden));
        }

        public DataTable BancosMetConsultarMonedas()
        {
            return BancosMetEjecucionConsulta(SelectMonedas, CommandType.Text, BancosFuncParametros());
        }

        public DataTable BancosMetConsultarTiposPago()
        {
            return BancosMetEjecucionConsulta(SelectTiposPago, CommandType.Text, BancosFuncParametros());
        }

        // Medio que se guarda en la orden: CHEQUE y TRANSFERENCIA conservan su valor fijo; cualquier otro tipo del catálogo
        // (efectivo, depósito, etc.) se guarda con su propio nombre, recortado al largo de la columna medioOrdenPago
        public static string BancosFuncMedioDeTipoPago(string NombreTipoPago)
        {
            string Nombre = (NombreTipoPago ?? "").Trim();
            if (Nombre.Length == 0)
                return null;
            if (Nombre.IndexOf("cheque", StringComparison.OrdinalIgnoreCase) >= 0)
                return ClsValoresPago.MedioCheque;
            if (Nombre.IndexOf("transfer", StringComparison.OrdinalIgnoreCase) >= 0)
                return ClsValoresPago.MedioTransferencia;
            return Nombre.Length > LargoMaximoMedio ? Nombre.Substring(0, LargoMaximoMedio).TrimEnd() : Nombre;
        }

        public DataTable BancosMetObtenerAutorizadores()
        {
            return BancosMetEjecucionConsulta(SelectAutorizadores, CommandType.Text, BancosFuncParametros(RolAutorizador));
        }

        public DataTable BancosMetObtenerPolitica(decimal Importe, long IdMoneda)
        {
            return BancosMetEjecucionConsulta(SelectPolitica, CommandType.Text, BancosFuncParametros(IdMoneda, BancosFuncDecimal(Importe)));
        }

        public DataTable BancosMetConsultarAprobaciones(long IdOrden)
        {
            return BancosMetEjecucionConsulta(SelectAprobaciones, CommandType.Text, BancosFuncParametros(IdOrden));
        }

        public DataTable BancosMetObtenerChequeraDisponible(long IdCuenta)
        {
            return BancosMetEjecucionConsulta(SelectChequera, CommandType.Text, BancosFuncParametros(IdCuenta));
        }

        public DataTable BancosMetObtenerMovimiento(long IdOrden)
        {
            return BancosMetEjecucionConsulta(SelectMovimiento, CommandType.Text, BancosFuncParametros(IdOrden));
        }

        public DataTable BancosMetConsultarPoliza(long IdPoliza)
        {
            return BancosMetEjecucionConsulta(SelectPoliza, CommandType.Text, BancosFuncParametros(IdPoliza));
        }

        public DataTable BancosMetObtenerConfiguracionPoliza(long IdCuenta)
        {
            return BancosMetEjecucionConsulta(SelectConfiguracionPoliza, CommandType.Text, BancosFuncParametros(IdCuenta));
        }

        public DataTable BancosFuncObtenerTipoCambio(string CodigoMoneda)
        {
            return BancosMetEjecucionConsulta(SelectTipoCambio, CommandType.Text, BancosFuncParametros(CodigoMoneda));
        }

        public long BancosMetPrepararOrden(ClsOrdenPago Orden)
        {
            return BancosFuncEjecutarTransaccion((Conexion, Transaccion) =>
            {
                ClsDatosObligacion Obligacion = BancosFuncBloquearObligacion(Orden, Conexion, Transaccion);
                BancosFuncValidarMonto(Orden, Obligacion);
                BancosFuncValidarCuenta(Orden, Conexion, Transaccion);

                long IdOrden = BancosFuncInsertarOrden(Orden, Obligacion.Beneficiario, Conexion, Transaccion);
                string Referencia = PrefijoOrden + IdOrden.ToString("0000");
                BancosMetRegistrarBitacora(Conexion, Transaccion, Orden.IdUsuario, AccionPreparar, TablaOrden, IdOrden,
                    "Orden " + Referencia + " por " + BancosFuncDecimal(Orden.ImporteOrdenPago) + " (" + Orden.MedioOrdenPago + ")", Orden.Ip);
                return IdOrden;
            });
        }

        public string BancosMetRegistrarAprobacion(long IdOrden, string Autorizador, string Decision, string Comentario,
            string Referencia, DateTime FechaValidacion, int? IdUsuario, string Ip)
        {
            return BancosFuncEjecutarTransaccion((Conexion, Transaccion) =>
            {
                DataRow Orden = BancosFuncBloquearOrden(Conexion, Transaccion, IdOrden);
                if (Convert.ToString(Orden["estadoOrdenPago"]) != ClsValoresPago.EstadoPendiente)
                    throw new InvalidOperationException("La orden ya no está pendiente de autorización.");
                if (BancosFuncSumar(SelectEsAutorizador, Conexion, Transaccion, Autorizador, RolAutorizador) == 0)
                    throw new InvalidOperationException("Solo un usuario con el perfil de " + RolAutorizador + " puede autorizar o rechazar órdenes de pago.");

                DataTable Politica = BancosFuncConsultar(SelectPolitica, Conexion, Transaccion,
                    Convert.ToInt64(Orden["Fk_Moneda_Codigo"]), BancosFuncDecimal(Convert.ToDecimal(Orden["importeOrdenPago"])));
                if (Politica.Rows.Count == 0)
                    throw new InvalidOperationException("No hay una política de aprobación vigente para este monto y moneda.");
                if (BancosFuncSumar(SelectAprobacionesDelAutorizador, Conexion, Transaccion, IdOrden, Autorizador) > 0)
                    throw new InvalidOperationException("Ese autorizador ya registró su decisión en esta orden.");

                BancosFuncEjecutar(InsertAprobacion, Conexion, Transaccion, Decision,
                    string.IsNullOrWhiteSpace(Comentario) ? null : Comentario, Autorizador, Referencia,
                    FechaValidacion.ToString(FormatoFechaHora), IdOrden, Convert.ToInt64(Politica.Rows[0]["Pk_PoliticaAprobacion"]));

                int Requeridas = Convert.ToInt32(Politica.Rows[0]["cantidadAprobacionesPoliticaAprobacion"]);
                string NuevoEstado = BancosFuncCalcularNuevoEstado(IdOrden, Decision, Requeridas, Conexion, Transaccion);
                if (NuevoEstado != ClsValoresPago.EstadoPendiente)
                    BancosFuncCambiarEstado(Conexion, Transaccion, IdOrden, ClsValoresPago.EstadoPendiente, NuevoEstado);

                string Accion = Decision == ClsValoresPago.DecisionRechazada ? AccionRechazar : AccionAutorizar;
                BancosMetRegistrarBitacora(Conexion, Transaccion, IdUsuario, Accion, TablaOrden, IdOrden,
                    "Orden " + Convert.ToString(Orden["referenciaOrdenPago"]) + ", autorizador " + Autorizador + ", referencia " + Referencia, Ip);
                return NuevoEstado;
            });
        }

        public string BancosMetEmitirCheque(long IdOrden, string Generador, string Referencia, DateTime FechaValidacion, int? IdUsuario, string Ip)
        {
            return BancosFuncEjecutarTransaccion((Conexion, Transaccion) =>
            {
                DataRow Orden = BancosFuncBloquearOrden(Conexion, Transaccion, IdOrden);
                if (Convert.ToString(Orden["estadoOrdenPago"]) != ClsValoresPago.EstadoAutorizada)
                    throw new InvalidOperationException("Solo se puede emitir una orden autorizada.");
                if (Convert.ToString(Orden["medioOrdenPago"]) != ClsValoresPago.MedioCheque)
                    throw new InvalidOperationException("La orden no es por cheque; se envía con el botón Enviar.");
                if (BancosFuncSumar(SelectChequesDeOrden, Conexion, Transaccion, IdOrden) > 0)
                    throw new InvalidOperationException("La orden ya tiene un cheque generado.");

                DataTable Chequera = BancosFuncConsultar(SelectChequeraBloqueada, Conexion, Transaccion, Convert.ToInt64(Orden["Fk_CuentaBancaria_Codigo"]));
                if (Chequera.Rows.Count == 0)
                    throw new InvalidOperationException("La cuenta no tiene una chequera activa con cheques disponibles.");

                long IdChequera = Convert.ToInt64(Chequera.Rows[0]["idChequera"]);
                string Serie = Convert.ToString(Chequera.Rows[0]["serieChequera"]);
                long Numero = Convert.ToInt64(Chequera.Rows[0]["siguienteNumeroChequera"]);

                BancosFuncEjecutar(InsertCheque, Conexion, Transaccion, Numero, Convert.ToString(Orden["beneficiarioOrdenPago"]), Referencia,
                    FechaValidacion.ToString(FormatoFechaHora), Generador, IdChequera, IdOrden);
                BancosFuncEjecutar(UpdateSiguienteCheque, Conexion, Transaccion, IdChequera);
                BancosFuncCambiarEstado(Conexion, Transaccion, IdOrden, ClsValoresPago.EstadoAutorizada, ClsValoresPago.EstadoEmitida);

                string NumeroCompleto = Serie + "-" + Numero.ToString("000000");
                BancosMetRegistrarBitacora(Conexion, Transaccion, IdUsuario, AccionEmitirCheque, TablaOrden, IdOrden,
                    "Orden " + Convert.ToString(Orden["referenciaOrdenPago"]) + ", cheque " + NumeroCompleto + ", referencia " + Referencia, Ip);
                return NumeroCompleto;
            });
        }

        public void BancosMetEnviarTransferencia(long IdOrden, string Referencia, int? IdUsuario, string Ip)
        {
            BancosFuncEjecutarTransaccion((Conexion, Transaccion) =>
            {
                DataRow Orden = BancosFuncBloquearOrden(Conexion, Transaccion, IdOrden);
                if (Convert.ToString(Orden["estadoOrdenPago"]) != ClsValoresPago.EstadoAutorizada)
                    throw new InvalidOperationException("Solo se puede enviar una orden autorizada.");
                if (Convert.ToString(Orden["medioOrdenPago"]) == ClsValoresPago.MedioCheque)
                    throw new InvalidOperationException("La orden es por cheque; debe emitirse el cheque.");

                BancosFuncCambiarEstado(Conexion, Transaccion, IdOrden, ClsValoresPago.EstadoAutorizada, ClsValoresPago.EstadoEmitida);
                BancosMetRegistrarBitacora(Conexion, Transaccion, IdUsuario, AccionEnviarTransferencia, TablaOrden, IdOrden,
                    "Orden " + Convert.ToString(Orden["referenciaOrdenPago"]) + ", referencia " + Referencia, Ip);
                return 0;
            });
        }

        public long BancosMetRegistrarMovimiento(long IdOrden, int? IdUsuario, string Ip)
        {
            return BancosFuncEjecutarTransaccion((Conexion, Transaccion) =>
            {
                DataTable Datos = BancosFuncConsultar(SelectOrdenConDocumento, Conexion, Transaccion, IdOrden);
                if (Datos.Rows.Count == 0)
                    throw new InvalidOperationException("La orden de pago no existe.");

                DataRow Orden = Datos.Rows[0];
                if (Convert.ToString(Orden["estadoOrdenPago"]) != ClsValoresPago.EstadoEmitida)
                    throw new InvalidOperationException("Solo se puede registrar el movimiento de una orden emitida.");
                if (BancosFuncSumar(SelectMovimientosDeOrden, Conexion, Transaccion, IdOrden) > 0)
                    throw new InvalidOperationException("La orden ya tiene un movimiento bancario registrado.");

                long IdCuenta = Convert.ToInt64(Orden["Fk_CuentaBancaria_Codigo"]);
                DataTable Configuracion = BancosFuncConsultar(SelectConfiguracionPoliza, Conexion, Transaccion, IdCuenta);
                if (Configuracion.Rows.Count == 0)
                    throw new InvalidOperationException("Falta la configuración de póliza de la cuenta para un tipo de transacción de egreso.");

                long IdTipoPago = BancosFuncObtenerTipoPago(Convert.ToString(Orden["medioOrdenPago"]), Conexion, Transaccion);
                ClsCambioAplicado Cambio = BancosFuncObtenerCambioAplicado(Convert.ToString(Orden["codigoMoneda"]), Conexion, Transaccion);
                decimal Importe = Convert.ToDecimal(Orden["importeOrdenPago"]);
                decimal ImporteGTQ = BancosFuncCalcularEquivalente(Importe, Cambio.Valor);

                long IdMovimiento = BancosFuncInsertarMovimiento(IdOrden, IdTipoPago, Orden, Configuracion.Rows[0], Cambio, ImporteGTQ, Conexion, Transaccion);
                BancosFuncInsertarPoliza(IdMovimiento, Configuracion.Rows[0], ImporteGTQ, Conexion, Transaccion);

                BancosFuncCambiarEstado(Conexion, Transaccion, IdOrden, ClsValoresPago.EstadoEmitida, ClsValoresPago.EstadoPagada);
                if (Orden["Fk_SolicitudPagoTransporte_Codigo"] != DBNull.Value)
                    BancosFuncCerrarSolicitudTransporte(Convert.ToInt64(Orden["Fk_SolicitudPagoTransporte_Codigo"]), IdMovimiento, Conexion, Transaccion);
                BancosMetRegistrarBitacora(Conexion, Transaccion, IdUsuario, AccionRegistrarMovimiento, TablaMovimiento, IdMovimiento,
                    "Orden " + Convert.ToString(Orden["referenciaOrdenPago"]) + ", importe GTQ " + BancosFuncDecimal(ImporteGTQ), Ip);
                return IdMovimiento;
            });
        }

        private ClsDatosObligacion BancosFuncBloquearObligacion(ClsOrdenPago Orden, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            if (Orden.IdOrdenCompraRecibida.HasValue)
                return BancosFuncBloquearCompra(Orden.IdOrdenCompraRecibida.Value, Conexion, Transaccion);
            if (Orden.IdDetallePlanillaRecibida.HasValue)
                return BancosFuncBloquearPlanilla(Orden.IdDetallePlanillaRecibida.Value, Conexion, Transaccion);
            if (Orden.IdSolicitudPagoTransporte.HasValue)
                return BancosFuncBloquearSolicitudTransporte(Orden.IdSolicitudPagoTransporte.Value, Conexion, Transaccion);
            throw new InvalidOperationException("La orden debe tener un origen: compra, planilla o solicitud de transporte.");
        }

        private ClsDatosObligacion BancosFuncBloquearCompra(long IdCompra, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            DataTable Compra = BancosFuncConsultar(SelectCompraBloqueada, Conexion, Transaccion, IdCompra);
            if (Compra.Rows.Count == 0)
                throw new InvalidOperationException("La obligación seleccionada ya no existe.");

            DataRow Fila = Compra.Rows[0];
            if (Convert.ToString(Fila["estadoOrdenCompraRecibida"]) != EstadoCompraAprobada)
                throw new InvalidOperationException("La orden de compra no está aprobada para pago.");

            return new ClsDatosObligacion
            {
                Importe = Convert.ToDecimal(Fila["importeOrdenCompraRecibida"]),
                IdMoneda = Convert.ToInt64(Fila["Fk_Moneda_Codigo"]),
                Beneficiario = Convert.ToString(Fila["proveedorOrdenCompraRecibida"]),
                PagadoPrevio = BancosFuncSumar(SelectPagadoCompra, Conexion, Transaccion, IdCompra)
            };
        }

        private ClsDatosObligacion BancosFuncBloquearPlanilla(long IdDetalle, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            DataTable Planilla = BancosFuncConsultar(SelectPlanillaBloqueada, Conexion, Transaccion, IdDetalle);
            if (Planilla.Rows.Count == 0)
                throw new InvalidOperationException("La obligación seleccionada ya no existe.");

            DataRow Fila = Planilla.Rows[0];
            if (Convert.ToString(Fila["estadoPlanillaRecibida"]) != EstadoPlanillaRecibida)
                throw new InvalidOperationException("La planilla ya no está pendiente de pago.");

            return new ClsDatosObligacion
            {
                Importe = Convert.ToDecimal(Fila["importeNetoDetallePlanillaRecibida"]),
                IdMoneda = Convert.ToInt64(Fila["Fk_Moneda_Codigo"]),
                Beneficiario = Convert.ToString(Fila["beneficiarioDetallePlanillaRecibida"]),
                PagadoPrevio = BancosFuncSumar(SelectPagadoPlanilla, Conexion, Transaccion, IdDetalle)
            };
        }

        private ClsDatosObligacion BancosFuncBloquearSolicitudTransporte(long IdSolicitud, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            DataTable Solicitud = BancosFuncConsultar(SelectSolicitudBloqueada, Conexion, Transaccion, IdSolicitud);
            if (Solicitud.Rows.Count == 0)
                throw new InvalidOperationException("La obligación seleccionada ya no existe.");

            DataRow Fila = Solicitud.Rows[0];
            if (Convert.ToString(Fila["codigoEstadoSolicitudPago"]) != EstadoSolicitudTransporte)
                throw new InvalidOperationException("La solicitud de pago de transporte ya no está pendiente.");
            if (Convert.ToString(Fila["estadoServicioTransporteExterno"]) == EstadoServicioCancelado)
                throw new InvalidOperationException("El servicio de transporte fue cancelado.");

            return new ClsDatosObligacion
            {
                Importe = Convert.ToDecimal(Fila["costoTotalCostoTransporte"]),
                IdMoneda = Convert.ToInt64(Fila["idMoneda"]),
                Beneficiario = Convert.ToString(Fila["nombreEmpresaTransportista"]),
                PagadoPrevio = BancosFuncSumar(SelectPagadoTransporte, Conexion, Transaccion, IdSolicitud)
            };
        }

        // Cuando lo pagado alcanza el costo del servicio, la solicitud se devuelve a Logística como pagada,
        // con la referencia del movimiento bancario y la fecha del pago.
        private void BancosFuncCerrarSolicitudTransporte(long IdSolicitud, long IdMovimiento, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            decimal Costo = BancosFuncSumar(SelectCostoSolicitud, Conexion, Transaccion, IdSolicitud);
            decimal Cobrado = BancosFuncSumar(SelectCobradoTransporte, Conexion, Transaccion, IdSolicitud);
            if (Cobrado < Costo)
                return;

            DataTable Estado = BancosFuncConsultar(SelectIdEstadoSolicitud, Conexion, Transaccion, EstadoSolicitudPagada);
            if (Estado.Rows.Count == 0)
                throw new InvalidOperationException("Falta el estado PAGADO en el catálogo de estados de solicitud de pago de Logística.");

            BancosFuncEjecutar(UpdateSolicitudPagada, Conexion, Transaccion, Convert.ToInt64(Estado.Rows[0][0]),
                PrefijoMovimiento + IdMovimiento.ToString("0000"), IdSolicitud);
        }

        private void BancosFuncValidarMonto(ClsOrdenPago Orden, ClsDatosObligacion Obligacion)
        {
            if (Obligacion.IdMoneda != Orden.IdMoneda)
                throw new InvalidOperationException("La moneda de la orden debe ser la de la obligación.");
            if (Orden.ImporteOrdenPago <= 0)
                throw new InvalidOperationException("El monto debe ser mayor que cero.");
            if (Orden.ImporteOrdenPago > Obligacion.Importe - Obligacion.PagadoPrevio)
                throw new InvalidOperationException("El monto supera lo que queda pendiente de la obligación.");
            if (Orden.FechaProgramadaOrdenPago.Date < DateTime.Today)
                throw new InvalidOperationException("La fecha de pago no puede ser anterior a hoy.");
        }

        private void BancosFuncValidarCuenta(ClsOrdenPago Orden, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            DataTable Cuenta = BancosFuncConsultar(SelectCuentaBloqueada, Conexion, Transaccion, Orden.IdCuentaBancaria);
            if (Cuenta.Rows.Count == 0)
                throw new InvalidOperationException("La cuenta bancaria no está activa.");

            DataRow Fila = Cuenta.Rows[0];
            if (Convert.ToInt64(Fila["Fk_Moneda_Codigo"]) != Orden.IdMoneda)
                throw new InvalidOperationException("La cuenta debe ser de la misma moneda de la obligación.");
            if (Fila["saldoDisponibleDisponibilidadDiaria"] == DBNull.Value)
                throw new InvalidOperationException("La cuenta no tiene disponibilidad registrada para hoy.");

            decimal Comprometido = BancosFuncSumar(SelectComprometidoCuenta, Conexion, Transaccion, Orden.IdCuentaBancaria);
            if (Orden.ImporteOrdenPago > Convert.ToDecimal(Fila["saldoDisponibleDisponibilidadDiaria"]) - Comprometido)
                throw new InvalidOperationException("Fondos insuficientes en la cuenta seleccionada.");
        }

        private long BancosFuncInsertarOrden(ClsOrdenPago Orden, string Beneficiario, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            string ReferenciaTemporal = "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20);
            BancosFuncEjecutar(InsertOrden, Conexion, Transaccion, ReferenciaTemporal, Beneficiario, BancosFuncDecimal(Orden.ImporteOrdenPago),
                Orden.MedioOrdenPago, Orden.FechaProgramadaOrdenPago.ToString(FormatoFecha), Orden.IdentificadorSolicitanteOrdenPago,
                Orden.IdCuentaBancaria, Orden.IdMoneda, Orden.IdOrdenCompraRecibida, Orden.IdDetallePlanillaRecibida, Orden.IdSolicitudPagoTransporte);

            long IdOrden = BancosFuncObtenerUltimoId(Conexion, Transaccion);
            BancosFuncEjecutar(UpdateReferenciaOrden, Conexion, Transaccion, PrefijoOrden + IdOrden.ToString("0000"), IdOrden);
            return IdOrden;
        }

        private string BancosFuncCalcularNuevoEstado(long IdOrden, string Decision, int Requeridas, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            if (Decision == ClsValoresPago.DecisionRechazada)
                return ClsValoresPago.EstadoRechazada;

            decimal Aprobadas = BancosFuncSumar(SelectAprobadas, Conexion, Transaccion, IdOrden);
            return Aprobadas >= Requeridas ? ClsValoresPago.EstadoAutorizada : ClsValoresPago.EstadoPendiente;
        }

        private long BancosFuncObtenerTipoPago(string Medio, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            DataTable Tipo;
            if (Medio == ClsValoresPago.MedioCheque)
                Tipo = BancosFuncConsultar(SelectTipoPagoDeMedio, Conexion, Transaccion, PatronCheque);
            else if (Medio == ClsValoresPago.MedioTransferencia)
                Tipo = BancosFuncConsultar(SelectTipoPagoDeMedio, Conexion, Transaccion, PatronTransferencia);
            else
                Tipo = BancosFuncConsultar(SelectTipoPagoPorNombre, Conexion, Transaccion, Medio);
            if (Tipo.Rows.Count == 0)
                throw new InvalidOperationException("No hay un tipo de pago activo para " + Medio.ToLower() + " en el catálogo de tipos de pago.");
            return Convert.ToInt64(Tipo.Rows[0]["idMetodoPago"]);
        }

        private ClsCambioAplicado BancosFuncObtenerCambioAplicado(string CodigoMoneda, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            if (CodigoMoneda == MonedaBase)
                return new ClsCambioAplicado { Valor = 1m };

            DataTable Cambio = BancosFuncConsultar(SelectTipoCambio, Conexion, Transaccion, CodigoMoneda);
            if (Cambio.Rows.Count == 0)
                throw new InvalidOperationException("No hay tipo de cambio vigente para la moneda de la orden.");

            return new ClsCambioAplicado
            {
                IdTipoCambio = Convert.ToInt64(Cambio.Rows[0]["idTipoCambio"]),
                Valor = Convert.ToDecimal(Cambio.Rows[0]["valorTipoCambio"])
            };
        }

        private long BancosFuncInsertarMovimiento(long IdOrden, long IdTipoPago, DataRow Orden, DataRow Configuracion, ClsCambioAplicado Cambio, decimal ImporteGTQ,
            OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            string Concepto = "Pago " + Convert.ToString(Orden["Documento"]) + " - " + Convert.ToString(Orden["beneficiarioOrdenPago"]);
            BancosFuncEjecutar(InsertMovimiento, Conexion, Transaccion, Concepto,
                BancosFuncDecimal(Convert.ToDecimal(Orden["importeOrdenPago"])), BancosFuncDecimal(ImporteGTQ), BancosFuncDecimal(Cambio.Valor),
                Convert.ToInt64(Orden["Fk_CuentaBancaria_Codigo"]), Convert.ToInt64(Configuracion["Fk_TipoTransaccion_Codigo"]),
                Cambio.IdTipoCambio, IdOrden, IdTipoPago);

            long IdMovimiento = BancosFuncObtenerUltimoId(Conexion, Transaccion);
            BancosFuncEjecutar(UpdateReferenciaMovimiento, Conexion, Transaccion, PrefijoMovimiento + IdMovimiento.ToString("0000"), IdMovimiento);
            return IdMovimiento;
        }

        private void BancosFuncInsertarPoliza(long IdMovimiento, DataRow Configuracion, decimal ImporteGTQ, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            BancosFuncEjecutar(InsertPoliza, Conexion, Transaccion, IdMovimiento);
            long IdPoliza = BancosFuncObtenerUltimoId(Conexion, Transaccion);

            BancosFuncEjecutar(InsertDetallePoliza, Conexion, Transaccion,
                Convert.ToString(Configuracion["codigoCuentaContableContrapartidaConfiguracionPoliza"]), BancosFuncDecimal(ImporteGTQ), BancosFuncDecimal(0m), IdPoliza);
            BancosFuncEjecutar(InsertDetallePoliza, Conexion, Transaccion,
                Convert.ToString(Configuracion["codigoCuentaContableBancoConfiguracionPoliza"]), BancosFuncDecimal(0m), BancosFuncDecimal(ImporteGTQ), IdPoliza);
        }

        private T BancosFuncEjecutarTransaccion<T>(Func<OdbcConnection, OdbcTransaction, T> Operacion)
        {
            using (var Conexion = BancosMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        T Resultado = Operacion(Conexion, Transaccion);
                        Transaccion.Commit();
                        return Resultado;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        private static List<OdbcParameter> BancosFuncParametros(params object[] Valores)
        {
            var Parametros = new List<OdbcParameter>();
            for (int Indice = 0; Indice < Valores.Length; Indice++)
                Parametros.Add(new OdbcParameter("p_" + Indice, Valores[Indice] ?? DBNull.Value));
            return Parametros;
        }

        private DataTable BancosFuncConsultar(string Sentencia, OdbcConnection Conexion, OdbcTransaction Transaccion, params object[] Valores)
        {
            return BancosMetEjecucionConsulta(Sentencia, CommandType.Text, BancosFuncParametros(Valores), Conexion, Transaccion);
        }

        private int BancosFuncEjecutar(string Sentencia, OdbcConnection Conexion, OdbcTransaction Transaccion, params object[] Valores)
        {
            return BancosMetEjecucionNonQuery(Sentencia, BancosFuncParametros(Valores), CommandType.Text, Conexion, Transaccion);
        }

        private decimal BancosFuncSumar(string Sentencia, OdbcConnection Conexion, OdbcTransaction Transaccion, params object[] Valores)
        {
            return Convert.ToDecimal(BancosFuncConsultar(Sentencia, Conexion, Transaccion, Valores).Rows[0][0]);
        }

        private long BancosFuncObtenerUltimoId(OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            return Convert.ToInt64(BancosMetEjecucionEscalar(UltimoId, BancosFuncParametros(), CommandType.Text, Conexion, Transaccion));
        }

        private DataRow BancosFuncBloquearOrden(OdbcConnection Conexion, OdbcTransaction Transaccion, long IdOrden)
        {
            DataTable Orden = BancosFuncConsultar(SelectOrdenBloqueada, Conexion, Transaccion, IdOrden);
            if (Orden.Rows.Count == 0)
                throw new InvalidOperationException("La orden de pago no existe.");
            return Orden.Rows[0];
        }

        private void BancosFuncCambiarEstado(OdbcConnection Conexion, OdbcTransaction Transaccion, long IdOrden, string EstadoActual, string EstadoNuevo)
        {
            if (BancosFuncEjecutar(UpdateEstadoOrden, Conexion, Transaccion, EstadoNuevo, IdOrden, EstadoActual) != 1)
                throw new InvalidOperationException("La orden cambió de estado mientras se procesaba. Actualice e intente de nuevo.");
        }

        private void BancosMetRegistrarBitacora(OdbcConnection Conexion, OdbcTransaction Transaccion, int? IdUsuario, string Accion,
            string Tabla, long IdRegistro, string Detalle, string Ip)
        {
            BancosFuncEjecutar(InsertBitacora, Conexion, Transaccion, IdUsuario, Accion, Tabla, (int)IdRegistro, Detalle,
                string.IsNullOrEmpty(Ip) ? "127.0.0.1" : Ip, DateTime.Now.ToString(FormatoFechaHora));
        }

        private string BancosFuncDecimal(decimal Valor)
        {
            return Valor.ToString("0.0000", CultureInfo.InvariantCulture);
        }
    }
}
