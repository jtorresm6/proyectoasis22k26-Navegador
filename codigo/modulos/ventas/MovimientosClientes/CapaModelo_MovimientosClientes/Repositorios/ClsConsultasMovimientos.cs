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
            const string Consulta = @"SELECT p.Pk_ReciboPago idPago,p.numeroReciboPago Recibo,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.montoReciboPago Monto,COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Depositado,p.montoReciboPago-COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Disponible
FROM tblReciboPago p JOIN tblCliente c ON c.Pk_Cliente=p.Fk_Cliente_Codigo JOIN tblMoneda mo ON mo.Pk_Moneda=p.Fk_Moneda_Codigo LEFT JOIN tblDepositoVentaDetalle dd ON dd.Fk_ReciboPago_Codigo=p.Pk_ReciboPago
WHERE p.Fk_Moneda_Codigo=? AND p.estadoReciboPago='CONFIRMADO'
GROUP BY p.Pk_ReciboPago,p.numeroReciboPago,c.nombreCliente,mo.codigoMoneda,p.montoReciboPago HAVING Disponible>0 ORDER BY p.Pk_ReciboPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncPendientesConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.Pk_FacturaEncabezado idFactura,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,mo.codigoMoneda Moneda,SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC) Saldo
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo JOIN tblMovimientoCxC m ON m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado JOIN tblMoneda mo ON mo.Pk_Moneda=f.Fk_Moneda_Codigo
WHERE cv.Fk_Cliente_Codigo=? AND f.Fk_Moneda_Codigo=? GROUP BY f.Pk_FacturaEncabezado,f.serieFacturaEncabezado,f.numeroFacturaEncabezado,f.fechaEmisionFacturaEncabezado,f.fechaVencimientoFacturaEncabezado,mo.codigoMoneda HAVING Saldo>0 ORDER BY f.fechaVencimientoFacturaEncabezado,f.Pk_FacturaEncabezado";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGeneralesConsultarRecibosConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.Pk_ReciboPago idPago,p.numeroReciboPago Recibo,p.fechaReciboPago Fecha,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.montoReciboPago Monto,p.estadoReciboPago Estado,COALESCE(SUM(dd.importeDepositoVentaDetalle),0) Depositado,COALESCE(GROUP_CONCAT(DISTINCT d.codigoDepositoVentaEncabezado ORDER BY d.codigoDepositoVentaEncabezado SEPARATOR ', '),'') Deposito,COALESCE(GROUP_CONCAT(DISTINCT d.Fk_MovimientoBancario_Codigo SEPARATOR ', '),'') MovimientoBancario
FROM tblReciboPago p JOIN tblCliente c ON c.Pk_Cliente=p.Fk_Cliente_Codigo JOIN tblMoneda mo ON mo.Pk_Moneda=p.Fk_Moneda_Codigo LEFT JOIN tblDepositoVentaDetalle dd ON dd.Fk_ReciboPago_Codigo=p.Pk_ReciboPago LEFT JOIN tblDepositoVentaEncabezado d ON d.Pk_DepositoVentaEncabezado=dd.Fk_DepositoVentaEncabezado_Codigo
WHERE (?='' OR p.numeroReciboPago=?) AND (?='' OR EXISTS(SELECT 1 FROM tblDepositoVentaDetalle x JOIN tblDepositoVentaEncabezado y ON y.Pk_DepositoVentaEncabezado=x.Fk_DepositoVentaEncabezado_Codigo WHERE x.Fk_ReciboPago_Codigo=p.Pk_ReciboPago AND y.codigoDepositoVentaEncabezado=?))
GROUP BY p.Pk_ReciboPago,p.numeroReciboPago,p.fechaReciboPago,c.nombreCliente,mo.codigoMoneda,p.montoReciboPago,p.estadoReciboPago ORDER BY p.fechaReciboPago DESC,p.Pk_ReciboPago DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncListarRecibosConsultarPago(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.Pk_ReciboPago Id,p.numeroReciboPago Recibo,c.nombreCliente Cliente,mo.codigoMoneda Moneda,p.fechaReciboPago Fecha,p.montoReciboPago Monto,fp.codigoFormaPagoVenta Forma,p.referenciaReciboPago Referencia,p.estadoReciboPago Estado,COALESCE(p.observacionesReciboPago,'') Descripcion
FROM tblReciboPago p JOIN tblCliente c ON c.Pk_Cliente=p.Fk_Cliente_Codigo JOIN tblMoneda mo ON mo.Pk_Moneda=p.Fk_Moneda_Codigo JOIN tblFormaPagoVenta fp ON fp.Pk_FormaPagoVenta=p.Fk_FormaPagoVenta_Codigo
WHERE (?=0 OR p.Fk_Cliente_Codigo=?) AND (?=0 OR p.Fk_Moneda_Codigo=?) ORDER BY p.fechaReciboPago DESC,p.Pk_ReciboPago DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncListarDocumentosConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.Pk_FacturaEncabezado Id,'FACTURA' Clase,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,f.estadoFacturaEncabezado Estado,COALESCE(f.observacionesFacturaEncabezado,'') Descripcion
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE f.serieFacturaEncabezado='CXC' AND (?=0 OR cv.Fk_Cliente_Codigo=?) AND (?=0 OR f.Fk_Moneda_Codigo=?)
UNION ALL SELECT n.Pk_NotaCxC,'NOTA',n.numeroNotaCxC,n.fechaNotaCxC,n.fechaNotaCxC,n.montoNotaCxC,n.estadoNotaCxC,n.motivoNotaCxC FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=n.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE (?=0 OR cv.Fk_Cliente_Codigo=?) AND (?=0 OR f.Fk_Moneda_Codigo=?) ORDER BY Fecha DESC,Documento";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT f.Pk_FacturaEncabezado Id,'FACTURA' Clase,cv.Fk_Cliente_Codigo idCliente,f.Fk_Moneda_Codigo idMoneda,f.Fk_Sucursal_Codigo idEstablecimiento,f.numeroFacturaEncabezado Numero,0 Tipo,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,COALESCE(f.observacionesFacturaEncabezado,'') Descripcion,f.estadoFacturaEncabezado Estado,'' Referencia
FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE f.Pk_FacturaEncabezado=? AND f.serieFacturaEncabezado='CXC'";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarNota(params object[] Parametros)
        {
            const string Consulta = @"SELECT n.Pk_NotaCxC Id,'NOTA' Clase,cv.Fk_Cliente_Codigo idCliente,f.Fk_Moneda_Codigo idMoneda,f.Fk_Sucursal_Codigo idEstablecimiento,n.numeroNotaCxC Numero,CASE m.tipoMovimientoCxC WHEN 'AJUSTE_POSITIVO' THEN 3 WHEN 'AJUSTE_NEGATIVO' THEN 4 ELSE CASE n.tipoNotaCxC WHEN 'CREDITO' THEN 1 ELSE 2 END END Tipo,n.fechaNotaCxC Fecha,n.fechaNotaCxC Vencimiento,n.montoNotaCxC Importe,n.motivoNotaCxC Descripcion,n.estadoNotaCxC Estado,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Referencia
FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=n.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo LEFT JOIN tblMovimientoCxC m ON m.Fk_NotaCxC_Codigo=n.Pk_NotaCxC WHERE n.Pk_NotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarRecibo(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.Pk_ReciboPago idPago,p.Fk_Cliente_Codigo idCliente,p.Fk_Moneda_Codigo idMoneda,p.numeroReciboPago numeroRecibo,p.fechaReciboPago fechaPago,p.montoReciboPago montoPago,1 tipoCambio,fp.codigoFormaPagoVenta formaPago,p.referenciaReciboPago referenciaPago,p.observacionesReciboPago observaciones,p.estadoReciboPago estadoPago FROM tblReciboPago p JOIN tblFormaPagoVenta fp ON fp.Pk_FormaPagoVenta=p.Fk_FormaPagoVenta_Codigo WHERE p.Pk_ReciboPago=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarAplicacionesRecibo(params object[] Parametros)
        {
            const string Consulta = @"SELECT a.Fk_FacturaEncabezado_Codigo idFactura,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,mo.codigoMoneda Moneda,(SELECT COALESCE(SUM(m.cargoMovimientoCxC-m.abonoMovimientoCxC),0) FROM tblMovimientoCxC m WHERE m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado) Saldo,a.montoAplicadoAplicacionPago Importe,a.estadoAplicacionPago Estado
FROM tblAplicacionPago a JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=a.Fk_FacturaEncabezado_Codigo JOIN tblMoneda mo ON mo.Pk_Moneda=f.Fk_Moneda_Codigo WHERE a.Fk_ReciboPago_Codigo=? ORDER BY a.Pk_AplicacionPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncLecturaConsultarMovimiento(params object[] Parametros)
        {
            const string Consulta = @"SELECT CASE WHEN m.Fk_AplicacionPago_Codigo IS NOT NULL THEN a.Fk_ReciboPago_Codigo WHEN m.Fk_NotaCxC_Codigo IS NOT NULL THEN m.Fk_NotaCxC_Codigo ELSE m.Fk_FacturaEncabezado_Codigo END Id,CASE WHEN m.Fk_AplicacionPago_Codigo IS NOT NULL THEN 'RECIBO' WHEN m.Fk_NotaCxC_Codigo IS NOT NULL THEN 'NOTA' ELSE 'FACTURA' END Clase FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo LEFT JOIN tblAplicacionPago a ON a.Pk_AplicacionPago=m.Fk_AplicacionPago_Codigo WHERE m.Pk_MovimientoCxC=? AND (f.serieFacturaEncabezado='CXC' OR m.Fk_NotaCxC_Codigo IS NOT NULL OR m.Fk_AplicacionPago_Codigo IS NOT NULL)";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncEstadoCuentaConsultarMovimientocxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC<? THEN m.cargoMovimientoCxC-m.abonoMovimientoCxC ELSE 0 END),0) Inicial,COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.cargoMovimientoCxC ELSE 0 END),0) Cargos,COALESCE(SUM(CASE WHEN m.fechaMovimientoCxC>=? THEN m.abonoMovimientoCxC ELSE 0 END),0) Abonos FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE cv.Fk_Cliente_Codigo=? AND f.Fk_Moneda_Codigo=? AND m.fechaMovimientoCxC<=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncEstadoCuentaConsultarMovimientocxc2(params object[] Parametros)
        {
            const string Consulta = @"SELECT m.Pk_MovimientoCxC idMovimiento,m.fechaMovimientoCxC Fecha,CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,m.tipoMovimientoCxC Tipo,m.descripcionMovimientoCxC Descripcion,m.cargoMovimientoCxC Cargo,m.abonoMovimientoCxC Abono,CASE WHEN c.fechaUltimoCierreControlPeriodoCxC IS NOT NULL AND m.fechaMovimientoCxC<=c.fechaUltimoCierreControlPeriodoCxC THEN 'CERRADO' ELSE 'APLICADO' END Estado FROM tblMovimientoCxC m JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=m.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo LEFT JOIN tblControlPeriodoCxC c ON c.Fk_Moneda_Codigo=f.Fk_Moneda_Codigo WHERE cv.Fk_Cliente_Codigo=? AND f.Fk_Moneda_Codigo=? AND m.fechaMovimientoCxC BETWEEN ? AND ? ORDER BY m.fechaMovimientoCxC,m.Pk_MovimientoCxC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDocumentoConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Documento,f.fechaEmisionFacturaEncabezado Fecha,f.fechaVencimientoFacturaEncabezado Vencimiento,cv.nombreClienteFacturaVersion Cliente,COALESCE((SELECT SUM(m.cargoMovimientoCxC) FROM tblMovimientoCxC m WHERE m.Fk_FacturaEncabezado_Codigo=f.Pk_FacturaEncabezado AND m.tipoMovimientoCxC='FACTURA'),0) Importe,f.estadoFacturaEncabezado Estado,f.observacionesFacturaEncabezado Descripcion FROM tblFacturaEncabezado f JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE cv.Fk_Cliente_Codigo=? AND f.Fk_Moneda_Codigo=? AND f.Fk_Sucursal_Codigo=? AND f.serieFacturaEncabezado='CXC' AND f.numeroFacturaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDocumentoConsultarNotacxc(params object[] Parametros)
        {
            const string Consulta = @"SELECT n.numeroNotaCxC Documento,n.fechaNotaCxC Fecha,n.tipoNotaCxC Tipo,cv.nombreClienteFacturaVersion Cliente,n.montoNotaCxC Importe,n.estadoNotaCxC Estado,n.motivoNotaCxC Descripcion FROM tblNotaCxC n JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=n.Fk_FacturaEncabezado_Codigo JOIN tblClienteFacturaVersion cv ON cv.Pk_ClienteFacturaVersion=f.Fk_ClienteFacturaVersion_Codigo WHERE cv.Fk_Cliente_Codigo=? AND f.Fk_Moneda_Codigo=? AND n.numeroNotaCxC=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDepositoConsultarDepositoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT d.Pk_DepositoVentaEncabezado idDeposito,d.codigoDepositoVentaEncabezado codigoDeposito,d.fechaDepositoVentaEncabezado fechaDeposito,d.Fk_Moneda_Codigo idMoneda,'' empresaReferencia,b.nombreBanco bancoReferencia,cb.numeroCuentaBancaria cuentaReferencia,tt.nombreTipoTransaccion conceptoReferencia,d.referenciaDepositoVentaEncabezado referenciaDeposito,d.totalDepositoVentaEncabezado total,d.estadoDepositoVentaEncabezado estadoDeposito FROM tblDepositoVentaEncabezado d JOIN tblCuentaBancaria cb ON cb.Pk_CuentaBancaria=d.Fk_CuentaBancaria_Codigo JOIN tblBanco b ON b.Pk_Banco=cb.Fk_Banco_Codigo JOIN tblTipoTransaccion tt ON tt.Pk_TipoTransaccion=d.Fk_TipoTransaccion_Codigo WHERE d.codigoDepositoVentaEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncDepositoConsultarDepositoventasdetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.numeroReciboPago Recibo,c.nombreCliente Cliente,dd.importeDepositoVentaDetalle Importe FROM tblDepositoVentaDetalle dd JOIN tblReciboPago p ON p.Pk_ReciboPago=dd.Fk_ReciboPago_Codigo JOIN tblCliente c ON c.Pk_Cliente=p.Fk_Cliente_Codigo WHERE dd.Fk_DepositoVentaEncabezado_Codigo=? ORDER BY p.Pk_ReciboPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncAplicacionesConsultarAplicacionpago(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(f.serieFacturaEncabezado,'-',f.numeroFacturaEncabezado) Factura,a.fechaAplicacionPago Fecha,a.montoAplicadoAplicacionPago Importe,a.estadoAplicacionPago Estado FROM tblAplicacionPago a JOIN tblFacturaEncabezado f ON f.Pk_FacturaEncabezado=a.Fk_FacturaEncabezado_Codigo WHERE a.Fk_ReciboPago_Codigo=? ORDER BY a.Pk_AplicacionPago";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
