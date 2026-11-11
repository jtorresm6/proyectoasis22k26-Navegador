using System.Data;
using VentasDatos;

namespace CapaModelo_MovimientosClientes
{
    public sealed class ClsConsultasMovimientos
    {
        private readonly ClsDatosVentas _Datos;
        internal ClsConsultasMovimientos(ClsDatosVentas Datos) { _Datos = Datos; }

        public DataTable VentasFuncRecibosDisponiblesConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.idReciboPago idPago,p.numeroReciboPago Recibo,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.montoReciboPago Monto,COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Depositado,p.montoReciboPago-COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Disponible
FROM tblReciboPago p JOIN tblCliente c ON c.idCliente=p.idCliente JOIN tblMoneda mo ON mo.idMoneda=p.idMoneda LEFT JOIN tblDepositoVentaDetalle dd ON dd.idReciboPago=p.idReciboPago
WHERE p.idMoneda=? AND p.estadoReciboPago='CONFIRMADO'
GROUP BY p.idReciboPago,p.numeroReciboPago,c.nombreCliente,mo.codigoMoneda,p.montoReciboPago HAVING Disponible>0 ORDER BY p.idReciboPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncPendientesConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.idFacturaEncabezado idFactura,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,mo.codigoMoneda Moneda,SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Saldo
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion JOIN tblMovimientoCxC m ON m.idFacturaEncabezado=f.idFacturaEncabezado JOIN tblMoneda mo ON mo.idMoneda=f.idMoneda
WHERE cv.idCliente=? AND f.idMoneda=? GROUP BY f.idFacturaEncabezado,f.serieFacturaEncabezado,f.numeroFacturaEncabezado,f.fechaEmisionFacturaEncabezado,f.fechaVencimientoFacturaEncabezado,mo.codigoMoneda HAVING Saldo>0 ORDER BY f.fechaVencimientoFacturaEncabezado,f.idFacturaEncabezado";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGeneralesConsultarRecibosConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.idReciboPago idPago,p.numeroReciboPago Recibo,p.fechaReciboPago Fecha,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.montoReciboPago Monto,p.estadoReciboPago Estado,COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Depositado,COALESCE(GROUP_CONCAT(DISTINCT d.codigoDepositoVentaEncabezado ORDER BY d.codigoDepositoVentaEncabezado SEPARATOR ', '),'') Deposito,COALESCE(GROUP_CONCAT(DISTINCT d.idMovimientoBancario SEPARATOR ', '),'') MovimientoBancario
FROM tblReciboPago p JOIN tblCliente c ON c.idCliente=p.idCliente JOIN tblMoneda mo ON mo.idMoneda=p.idMoneda LEFT JOIN tblDepositoVentaDetalle dd ON dd.idReciboPago=p.idReciboPago LEFT JOIN tblDepositoVentaEncabezado d ON d.idDepositoVentaEncabezado=dd.idDepositoVentaEncabezado
WHERE (?='' OR p.numeroReciboPago=?) AND (?='' OR EXISTS(SELECT 1 FROM tblDepositoVentaDetalle x JOIN tblDepositoVentaEncabezado y ON y.idDepositoVentaEncabezado=x.idDepositoVentaEncabezado WHERE x.idReciboPago=p.idReciboPago AND y.codigoDepositoVentaEncabezado=?))
GROUP BY p.idReciboPago,p.numeroReciboPago,p.fechaReciboPago,c.nombreCliente,mo.codigoMoneda,p.montoReciboPago,p.estadoReciboPago ORDER BY p.fechaReciboPago DESC,p.idReciboPago DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncListarRecibosConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.idReciboPago Id,p.numeroReciboPago Recibo,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.fechaReciboPago Fecha,p.montoReciboPago Monto,fp.codigoFormaPagoVenta Forma,p.referenciaReciboPago Referencia,p.estadoReciboPago Estado,COALESCE(p.observacionesReciboPago,'') Descripcion
FROM tblReciboPago p JOIN tblCliente c ON c.idCliente=p.idCliente JOIN tblMoneda mo ON mo.idMoneda=p.idMoneda JOIN tblFormaPagoVenta fp ON fp.idFormaPagoVenta=p.idFormaPagoVenta
WHERE (?=0 OR p.idCliente=?) AND (?=0 OR p.idMoneda=?) ORDER BY p.fechaReciboPago DESC,p.idReciboPago DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncListarDocumentosConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.idFacturaEncabezado Id,'FACTURA' Clase,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.idFacturaEncabezado=f.idFacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,f.estadoFacturaEncabezado Estado,COALESCE(f.observacionesFacturaEncabezado,'') Descripcion
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE f.serieFacturaEncabezado='CXC' AND (?=0 OR cv.idCliente=?) AND (?=0 OR f.idMoneda=?)
UNION ALL SELECT n.idNotaCxC,'NOTA',n.numeroNotaCxC,n.fechaNotaCxC,n.fechaNotaCxC,n.montoNotaCxC,n.estadoNotaCxC,n.motivoNotaCxC FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=n.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE (?=0 OR cv.idCliente=?) AND (?=0 OR f.idMoneda=?) ORDER BY Fecha DESC,Documento";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.idFacturaEncabezado Id,'FACTURA' Clase,cv.idCliente idCliente,f.idMoneda idMoneda,f.idSucursal idEstablecimiento,f.numeroFacturaEncabezado Numero,0 Tipo,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.idFacturaEncabezado=f.idFacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,COALESCE(f.observacionesFacturaEncabezado,'') Descripcion,f.estadoFacturaEncabezado Estado,'' Referencia
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE f.idFacturaEncabezado=? AND f.serieFacturaEncabezado='CXC'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarNota(params object[] Parametros)
        {
            const string Consulta = @"SELECT n.idNotaCxC Id,'NOTA' Clase,cv.idCliente idCliente,f.idMoneda idMoneda,f.idSucursal idEstablecimiento,n.numeroNotaCxC Numero,CASE m.tipoMovimientoCxC WHEN 'AJUSTE_POSITIVO' THEN 3 WHEN 'AJUSTE_NEGATIVO' THEN 4 ELSE CASE n.tipoNotaCxC WHEN 'CREDITO' THEN 1 ELSE 2 END END Tipo,n.fechaNotaCxC Fecha,n.fechaNotaCxC Vencimiento,n.montoNotaCxC Importe,n.motivoNotaCxC Descripcion,n.estadoNotaCxC Estado,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Referencia
FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=n.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion LEFT JOIN tblMovimientoCxC m ON m.idNotaCxC=n.idNotaCxC WHERE n.idNotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarRecibo(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.idReciboPago idPago,p.idCliente idCliente,p.idMoneda idMoneda,p.numeroReciboPago numeroRecibo,p.fechaReciboPago fechaPago,p.montoReciboPago montoPago,1 tipoCambio,fp.codigoFormaPagoVenta formaPago,p.referenciaReciboPago referenciaPago,p.observacionesReciboPago observaciones,p.estadoReciboPago estadoPago FROM tblReciboPago p JOIN tblFormaPagoVenta fp ON fp.idFormaPagoVenta=p.idFormaPagoVenta WHERE p.idReciboPago=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarAplicacionesRecibo(params object[] Parametros)
        {
            const string Consulta = @"SELECT a.idFacturaEncabezado idFactura,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,mo.codigoMoneda Moneda,(SELECT COALESCE(SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC),0) FROM tblMovimientoCxC m WHERE m.idFacturaEncabezado=f.idFacturaEncabezado) Saldo,a.montoAplicadoAplicacionPago Importe,a.estadoAplicacionPago Estado
FROM tblAplicacionPago a JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=a.idFacturaEncabezado JOIN tblMoneda mo ON mo.idMoneda=f.idMoneda WHERE a.idReciboPago=? ORDER BY a.idAplicacionPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarMovimiento(params object[] Parametros)
        {
            const string Consulta = @"SELECT CASE WHEN m.idAplicacionPago IS NOT NULL THEN a.idReciboPago WHEN m.idNotaCxC IS NOT NULL THEN m.idNotaCxC ELSE m.idFacturaEncabezado END Id,CASE WHEN m.idAplicacionPago IS NOT NULL THEN 'RECIBO' WHEN m.idNotaCxC IS NOT NULL THEN 'NOTA' ELSE 'FACTURA' END Clase FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado LEFT JOIN tblAplicacionPago a ON a.idAplicacionPago=m.idAplicacionPago WHERE m.idMovimientoCxC=? AND (f.serieFacturaEncabezado='CXC' OR m.idNotaCxC IS NOT NULL OR m.idAplicacionPago IS NOT NULL)";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncEstadoCuentaConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC<? THEN m.cargoMovimientoCxC-m.abonoMovimientoCxC ELSE 0 END),0) Inicial,COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END),0) Cargos,COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END),0) Abonos FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE cv.idCliente=? AND f.idMoneda=? AND m.fechaMovimientoCxC<=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncEstadoCuentaConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.idMovimientoCxC idMovimiento,m.fechaMovimientoCxC Fecha,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,m.tipoMovimientoCxC Tipo,m.descripcionMovimientoCxC Descripcion,m.cargoMovimientoCxC Cargo,m.abonoMovimientoCxC Abono,CASE WHEN c.fechaUltimoCierreControlPeriodoCxC IS NOT NULL AND m.fechaMovimientoCxC<=c.fechaUltimoCierreControlPeriodoCxC THEN 'CERRADO' ELSE 'APLICADO' END Estado FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=m.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion LEFT JOIN tblControlPeriodoCxC c ON c.idMoneda=f.idMoneda WHERE cv.idCliente=? AND f.idMoneda=? AND m.fechaMovimientoCxC BETWEEN ? AND ? ORDER BY m.fechaMovimientoCxC,m.idMovimientoCxC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,cv.nombreClienteFacturaVersion Cliente,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.idFacturaEncabezado=f.idFacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,f.estadoFacturaEncabezado Estado,f.observacionesFacturaEncabezado Descripcion FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE cv.idCliente=? AND f.idMoneda=? AND f.idSucursal=? AND f.serieFacturaEncabezado='CXC' AND f.numeroFacturaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT n.numeroNotaCxC Documento,n.fechaNotaCxC Fecha,n.tipoNotaCxC Tipo,cv.nombreClienteFacturaVersion Cliente,n.montoNotaCxC Importe,n.estadoNotaCxC Estado,n.motivoNotaCxC Descripcion FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=n.idFacturaEncabezado JOIN tblClienteFacturaVersion cv ON cv.idClienteFacturaVersion=f.idClienteFacturaVersion WHERE cv.idCliente=? AND f.idMoneda=? AND n.numeroNotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDepositoConsultarDepositoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT d.idDepositoVentaEncabezado idDeposito,d.codigoDepositoVentaEncabezado codigoDeposito,d.fechaDepositoVentaEncabezado fechaDeposito,d.idMoneda idMoneda,
                b.idBanco idBanco,cb.idCuentaBancaria idCuenta,tt.idTipoTransaccion idConcepto,d.idMovimientoBancario idMovimientoBancario,
                '' empresaReferencia,b.nombreBanco bancoReferencia,cb.numeroCuentaBancaria cuentaReferencia,tt.nombreTipoTransaccion conceptoReferencia,d.referenciaDepositoVentaEncabezado referenciaDeposito,d.totalDepositoVentaEncabezado total,d.estadoDepositoVentaEncabezado estadoDeposito
                FROM tblDepositoVentaEncabezado d JOIN tblCuentaBancaria cb ON cb.idCuentaBancaria=d.idCuentaBancaria JOIN tblBanco b ON b.idBanco=cb.Fk_Banco_Codigo JOIN tblTipoTransaccion tt ON tt.idTipoTransaccion=d.idTipoTransaccion WHERE d.codigoDepositoVentaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDepositoConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.numeroReciboPago Recibo,c.nombreCliente Cliente,dd.importeDepositoVentaDetalle Importe FROM tblDepositoVentaDetalle dd JOIN tblReciboPago p ON p.idReciboPago=dd.idReciboPago JOIN tblCliente c ON c.idCliente=p.idCliente WHERE dd.idDepositoVentaEncabezado=? ORDER BY p.idReciboPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAplicacionesConsultarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Factura,a.fechaAplicacionPago Fecha,a.montoAplicadoAplicacionPago Importe,a.estadoAplicacionPago Estado FROM tblAplicacionPago a JOIN tblFacturaEncabezado f ON f.idFacturaEncabezado=a.idFacturaEncabezado WHERE a.idReciboPago=? ORDER BY a.idAplicacionPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
