// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_Cotizaciones
{
    public sealed class ClsConversionCotizaciones
    {
        private readonly ClsDatosVentas _Datos;

        internal ClsConversionCotizaciones(ClsDatosVentas Datos)
        {
            _Datos = Datos;
        }

        // Convertir: consultas parametrizadas.
        public object VentasFuncConvertirConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT Fk_Moneda_Codigo
    FROM tblCotizacionEncabezado
    WHERE Pk_CotizacionEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncConvertirConsultarCotizacion2(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.Pk_CotizacionEncabezado idCotizacion,c.numeroCotizacionEncabezado numeroCotizacion,c.Fk_Cliente_Codigo idCliente,c.Fk_Vendedor_Codigo idVendedor,c.Fk_Moneda_Codigo idMoneda,c.fechaEmisionCotizacionEncabezado fechaEmision,c.vigenteHastaCotizacionEncabezado vigenteHasta,c.estadoCotizacionEncabezado estadoCotizacion,1 tipoCambio,p.Pk_PedidoEncabezado idPedido,f.Pk_FacturaEncabezado idFactura
    FROM tblCotizacionEncabezado c
    LEFT JOIN tblPedidoEncabezado p ON p.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado
    LEFT JOIN tblFacturaEncabezado f ON f.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado
    WHERE c.Pk_CotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(serieFacturaEncabezado,'-',numeroFacturaEncabezado)
    FROM tblFacturaEncabezado
    WHERE Pk_FacturaEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarPedido(params object[] Parametros)
        {
            const string Consulta = @"SELECT numeroPedidoEncabezado
    FROM tblPedidoEncabezado
    WHERE Pk_PedidoEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarPedido(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblPedidoEncabezado(numeroPedidoEncabezado,Fk_CotizacionEncabezado_Codigo,Fk_Cliente_Codigo,Fk_Vendedor_Codigo,Fk_Moneda_Codigo,fechaPedidoEncabezado,estadoPedidoEncabezado,Fk_Usuario_Registro)
    VALUES (?,?,?,?,?,?,'CONFIRMADO',?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarPedidodetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblPedidoDetalle(Fk_PedidoEncabezado_Codigo,Fk_CotizacionDetalle_Codigo,numeroLineaPedidoDetalle,Fk_Producto_Codigo,descripcionItemPedidoDetalle,cantidadPedidoDetalle,precioUnitarioPedidoDetalle,descuentoMontoPedidoDetalle,tasaImpuestoPedidoDetalle)
    SELECT ?,Pk_CotizacionDetalle,numeroLineaCotizacionDetalle,Fk_Producto_Codigo,descripcionItemCotizacionDetalle,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle,tasaImpuestoCotizacionDetalle
    FROM tblCotizacionDetalle
    WHERE Fk_CotizacionEncabezado_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarEstablecimientoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblSucursal
    WHERE Pk_Sucursal=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncConvertirConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT Pk_Cliente idCliente,diasCreditoCliente diasCredito,tipoIdentificacionCliente tipoIdentificacion,COALESCE(nitCliente,dpiCliente,identificacionExtranjeraCliente,'CF') numeroIdentificacion,nombreCliente,direccionCliente,nitCliente,dpiCliente,identificacionExtranjeraCliente
    FROM tblCliente
    WHERE Pk_Cliente=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncConvertirCrearVersionCliente(int Cliente)
        {
            const string Consulta = @"INSERT INTO tblClienteFacturaVersion(Fk_Cliente_Codigo,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,identificacionExtranjeraClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
    SELECT c.Pk_Cliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.Fk_Cliente_Codigo=c.Pk_Cliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,c.identificacionExtranjeraCliente,c.nombreCliente,c.direccionCliente
    FROM tblCliente c WHERE c.Pk_Cliente=?";
            _Datos.VentasFuncEjecutar(Consulta, Cliente);
            return _Datos.VentasFuncObtenerId();
        }

        public int VentasFuncConvertirInsertarFactura(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaEncabezado(serieFacturaEncabezado,numeroFacturaEncabezado,Fk_Sucursal_Codigo,Fk_Vendedor_Codigo,Fk_Moneda_Codigo,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,tipoDteFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,observacionesFacturaEncabezado,Fk_Usuario_Registro,Fk_ClienteFacturaVersion_Codigo,Fk_CotizacionEncabezado_Codigo)
    VALUES ('COT',?,?,?,?,?,?,'FACT',?,'BORRADOR',?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarFacturadetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaDetalle(Fk_FacturaEncabezado_Codigo,numeroLineaFacturaDetalle,Fk_Producto_Codigo,descripcionItemFacturaDetalle,cantidadFacturaDetalle,precioUnitarioFacturaDetalle,descuentoMontoFacturaDetalle,tasaImpuestoFacturaDetalle)
    SELECT ?,numeroLineaCotizacionDetalle,Fk_Producto_Codigo,descripcionItemCotizacionDetalle,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle,tasaImpuestoCotizacionDetalle
    FROM tblCotizacionDetalle
    WHERE Fk_CotizacionEncabezado_Codigo=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET estadoCotizacionEncabezado='ACEPTADA'
    WHERE Pk_CotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
