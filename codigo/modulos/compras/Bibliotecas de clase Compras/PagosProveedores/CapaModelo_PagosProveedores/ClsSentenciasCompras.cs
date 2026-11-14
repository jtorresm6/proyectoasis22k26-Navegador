/*
 * ==================================================================
 * Área : Compras
 * Propósito :
 *  Sentencias SQL del módulo de pagos a proveedores.
 *  Usan parámetros posicionales (?) para ODBC: el orden de los
 *  parámetros en el código debe coincidir con el orden de los "?".
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores
{
    public static class ClsSentenciasCompras
    {
        // ---------- Encabezado ----------
        private const string PagoColumnas =
            @"p.idPagoProveedor, p.idProveedor, pr.nombreProveedor, p.fechaPagoProveedor,
              p.montoPagoProveedor, p.idMetodoPago, mp.nombreMetodoPago, p.idMoneda,
              p.idTipoCambio, p.tipoCambioPagoProveedor, p.referenciaPagoProveedor, p.estadoPagoProveedor";

        private const string PagoDesde =
            @"FROM tblEncabezadoPagoProveedor p
              INNER JOIN tblProveedor pr ON pr.idProveedor = p.idProveedor
              INNER JOIN tblMetodoPago mp ON mp.idMetodoPago = p.idMetodoPago";

        // El WHERE se agrega desde el repositorio según los filtros recibidos
        public const string PagoConsultarBase = "SELECT " + PagoColumnas + " " + PagoDesde + " WHERE 1 = 1";
        public const string PagoConsultarFiltroProveedor = " AND p.idProveedor = ?";
        public const string PagoConsultarFiltroEstado = " AND p.estadoPagoProveedor = ?";
        public const string PagoConsultarFiltroDesde = " AND p.fechaPagoProveedor >= ?";
        public const string PagoConsultarFiltroHasta = " AND p.fechaPagoProveedor <= ?";
        public const string PagoConsultarOrden = " ORDER BY p.idPagoProveedor DESC";

        // Params: idPago
        public const string PagoObtenerPorId =
            "SELECT " + PagoColumnas + " " + PagoDesde + " WHERE p.idPagoProveedor = ?";

        // Params: idProveedor, fecha, monto, idMetodoPago, idMoneda, idTipoCambio, tipoCambio, referencia, estado
        public const string PagoInsertarEncabezado =
            @"INSERT INTO tblEncabezadoPagoProveedor
                (idProveedor, fechaPagoProveedor, montoPagoProveedor, idMetodoPago, idMoneda,
                 idTipoCambio, tipoCambioPagoProveedor, referenciaPagoProveedor, estadoPagoProveedor)
              VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

        public const string UltimoIdInsertado = "SELECT LAST_INSERT_ID()";

        // Params: (los 9 del encabezado) + idPago
        public const string PagoActualizarEncabezado =
            @"UPDATE tblEncabezadoPagoProveedor
                 SET idProveedor = ?,
                     fechaPagoProveedor = ?,
                     montoPagoProveedor = ?,
                     idMetodoPago = ?,
                     idMoneda = ?,
                     idTipoCambio = ?,
                     tipoCambioPagoProveedor = ?,
                     referenciaPagoProveedor = ?,
                     estadoPagoProveedor = ?
               WHERE idPagoProveedor = ?";

        // Params: idPago
        public const string PagoBloquearEncabezado =
            @"SELECT idProveedor, estadoPagoProveedor, idMoneda, tipoCambioPagoProveedor
                FROM tblEncabezadoPagoProveedor
               WHERE idPagoProveedor = ?
                 FOR UPDATE";

        public const string PagoEliminarEncabezado =
            "DELETE FROM tblEncabezadoPagoProveedor WHERE idPagoProveedor = ?";

        // ---------- Detalle ----------
        // Params: idPago, serie, numero, idConcepto, montoAplicado
        public const string PagoInsertarDetalle =
            @"INSERT INTO tblDetallePagoProveedor
                (idPagoProveedor, serieDocumentoCuentaPorPagar, numeroDocumentoCuentaPorPagar,
                 idConceptoProveedor, montoAplicadoDetallePago)
              VALUES (?, ?, ?, ?, ?)";

        // Params: idPago
        public const string PagoObtenerDetalles =
            @"SELECT d.serieDocumentoCuentaPorPagar, d.numeroDocumentoCuentaPorPagar,
                     d.idConceptoProveedor, c.nombreConceptoProveedor, d.montoAplicadoDetallePago
                FROM tblDetallePagoProveedor d
               INNER JOIN tblConceptoProveedor c ON c.idConceptoProveedor = d.idConceptoProveedor
               WHERE d.idPagoProveedor = ?
               ORDER BY d.serieDocumentoCuentaPorPagar, d.numeroDocumentoCuentaPorPagar";

        public const string PagoEliminarDetalles =
            "DELETE FROM tblDetallePagoProveedor WHERE idPagoProveedor = ?";

        // ---------- Cuenta por pagar ----------
        // Params: serie, numero, idConcepto, idProveedor
        public const string PagoBloquearCuenta =
            @"SELECT saldoActualCuentaPorPagar, montoTotalCuentaPorPagar, estadoCuentaPorPagar,
                     idMoneda, tipoCambioCuentaPorPagar
                FROM tblCuentaPorPagar
               WHERE serieDocumentoCuentaPorPagar = ?
                 AND numeroDocumentoCuentaPorPagar = ?
                 AND idConceptoProveedor = ?
                 AND idProveedor = ?
                 FOR UPDATE";

        // Params: saldo, estado, serie, numero, idConcepto
        public const string PagoActualizarSaldoCuenta =
            @"UPDATE tblCuentaPorPagar
                 SET saldoActualCuentaPorPagar = ?,
                     estadoCuentaPorPagar = ?
               WHERE serieDocumentoCuentaPorPagar = ?
                 AND numeroDocumentoCuentaPorPagar = ?
                 AND idConceptoProveedor = ?";

        // Params: idProveedor
        // tblMoneda (Bancos): la llave se renombró a idMoneda (V1.1)
        public const string PagoCuentasPendientes =
            @"SELECT c.serieDocumentoCuentaPorPagar AS serie,
                     c.numeroDocumentoCuentaPorPagar AS numero,
                     c.idConceptoProveedor AS idConcepto,
                     k.nombreConceptoProveedor AS concepto,
                     c.idMoneda AS idMoneda,
                     m.codigoMoneda AS codigoMoneda,
                     c.saldoActualCuentaPorPagar AS saldo,
                     CONCAT(c.serieDocumentoCuentaPorPagar, '-', c.numeroDocumentoCuentaPorPagar,
                            ' (', k.nombreConceptoProveedor, ') - ', m.simboloMoneda, ' ',
                            FORMAT(c.saldoActualCuentaPorPagar, 2)) AS descripcion
                FROM tblCuentaPorPagar c
               INNER JOIN tblConceptoProveedor k ON k.idConceptoProveedor = c.idConceptoProveedor
               INNER JOIN tblMoneda m ON m.idMoneda = c.idMoneda
               WHERE c.idProveedor = ?
                 AND k.naturalezaConceptoProveedor = 'Cargo'
                 AND c.saldoActualCuentaPorPagar > 0
                 AND c.estadoCuentaPorPagar IN ('Pendiente', 'Parcial')
               ORDER BY c.fechaVencimientoCuentaPorPagar";

        // ---------- Solicitud de pago ----------
        // Params: idPago
        public const string PagoEstadoSolicitud =
            @"SELECT estadoSolicitudPago FROM tblSolicitudPago
               WHERE moduloOrigenSolicitudPago = 'Compras' AND idOrigenSolicitudPago = ?
               LIMIT 1";

        // Params: idPago, beneficiario, monto, metodo
        public const string PagoInsertarSolicitud =
            @"INSERT INTO tblSolicitudPago
                (moduloOrigenSolicitudPago, idOrigenSolicitudPago, beneficiarioSolicitudPago,
                 montoSolicitudPago, metodoSolicitudPago)
              VALUES ('Compras', ?, ?, ?, ?)";

        // Params: monto, metodo, estado, idPago
        public const string PagoActualizarSolicitud =
            @"UPDATE tblSolicitudPago
                 SET montoSolicitudPago = ?,
                     metodoSolicitudPago = ?,
                     estadoSolicitudPago = ?
               WHERE moduloOrigenSolicitudPago = 'Compras' AND idOrigenSolicitudPago = ?";

        // Params: idPago
        public const string PagoEliminarSolicitud =
            @"DELETE FROM tblSolicitudPago
               WHERE moduloOrigenSolicitudPago = 'Compras' AND idOrigenSolicitudPago = ?";

        // ---------- Catálogos ----------
        public const string PagoProveedores =
            @"SELECT idProveedor AS idProveedor, nombreProveedor AS nombre
                FROM tblProveedor WHERE estadoProveedor = 1 ORDER BY nombreProveedor";

        public const string PagoMetodos =
            @"SELECT idMetodoPago AS idMetodoPago, nombreMetodoPago AS nombre
                FROM tblMetodoPago WHERE is_active = 1 ORDER BY nombreMetodoPago";

        // tblMoneda (Bancos): llave idMoneda (antes Pk_Moneda) y bandera is_active
        public const string PagoMonedas =
            @"SELECT idMoneda AS idMoneda, codigoMoneda AS codigo, nombreMoneda AS nombre
                FROM tblMoneda WHERE is_active = 1 ORDER BY codigoMoneda";

        // Params: idProveedor
        public const string PagoNombreProveedor =
            "SELECT nombreProveedor FROM tblProveedor WHERE idProveedor = ?";

        // Params: idMetodoPago
        public const string PagoNombreMetodo =
            "SELECT nombreMetodoPago FROM tblMetodoPago WHERE idMetodoPago = ?";

        // Params: codigo
        public const string PagoIdMonedaPorCodigo =
            "SELECT idMoneda FROM tblMoneda WHERE codigoMoneda = ?";

        // Params: idMonedaOrigen, idMonedaDestino, fecha
        // Las columnas Fk_Moneda_*_Codigo guardan el ID numérico de la moneda (no el código de 3 letras).
        // tblTipoCambio (Bancos): llave idTipoCambio (antes Pk_TipoCambio)
        public const string PagoTipoCambioVigente =
            @"SELECT idTipoCambio, valorTipoCambio
                FROM tblTipoCambio
               WHERE Fk_Moneda_Origen_Codigo = ?
                 AND Fk_Moneda_Destino_Codigo = ?
                 AND fechaTipoCambio <= ?
               ORDER BY fechaTipoCambio DESC, versionTipoCambio DESC
               LIMIT 1";
    }
}