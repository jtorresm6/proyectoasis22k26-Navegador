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
            const string Consulta = @"SELECT idMoneda
    FROM tblCotizacionEncabezado
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncConvertirConsultarCotizacion2(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.idCotizacionEncabezado idCotizacion,c.numeroCotizacionEncabezado numeroCotizacion,c.idCliente idCliente,c.idVendedor idVendedor,c.idMoneda idMoneda,c.fechaEmisionCotizacionEncabezado fechaEmision,c.vigenteHastaCotizacionEncabezado vigenteHasta,c.estadoCotizacionEncabezado estadoCotizacion,1 tipoCambio,p.idPedidoEncabezado idPedido,f.idFacturaEncabezado idFactura
    FROM tblCotizacionEncabezado c
    LEFT JOIN tblPedidoEncabezado p ON p.idCotizacionEncabezado=c.idCotizacionEncabezado
    LEFT JOIN tblFacturaEncabezado f ON f.idCotizacionEncabezado=c.idCotizacionEncabezado
    WHERE c.idCotizacionEncabezado=?
    FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarFactura(params object[] Parametros)
        {
            const string Consulta = @"SELECT CONCAT(serieFacturaEncabezado,'-',numeroFacturaEncabezado)
    FROM tblFacturaEncabezado
    WHERE idFacturaEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarPedido(params object[] Parametros)
        {
            const string Consulta = @"SELECT referenciaPedidoEncabezado
    FROM tblPedidoEncabezado
    WHERE idPedidoEncabezado=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarPedido(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblPedidoEncabezado(referenciaPedidoEncabezado,idCotizacionEncabezado,idCliente,idVendedor,idMoneda,fechaPedidoEncabezado,estadoPedidoEncabezado)
    VALUES (?,?,?,?,?,?,'CONFIRMADO')";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarPedidodetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblPedidoDetalle(idPedidoEncabezado,OrdenPedido,idProducto,cantidadPedidoDetalle,precioUnitarioPedidoDetalle,descuentoMontoPedidoDetalle)
    SELECT ?,numeroLineaCotizacionDetalle,idProducto,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle
    FROM tblCotizacionDetalle
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public object VentasFuncConvertirConsultarEstablecimientoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT COUNT(*)
    FROM tblSucursal
    WHERE idSucursal=?";
            return _Datos.VentasFuncEscalar(Consulta, Parametros);
        }

        public DataTable VentasFuncConvertirConsultarCliente(params object[] Parametros)
        {
            const string Consulta = @"SELECT idCliente idCliente,diasCreditoCliente diasCredito,tipoIdentificacionCliente tipoIdentificacion,COALESCE(nitCliente,dpiCliente,'CF') numeroIdentificacion,nombreCliente,direccionCliente,nitCliente,dpiCliente
    FROM tblCliente
    WHERE idCliente=? FOR UPDATE";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public int VentasFuncConvertirCrearVersionCliente(int Cliente)
        {
            const string Consulta = @"INSERT INTO tblClienteFacturaVersion(idCliente,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,nitClienteFacturaVersion,dpiClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
    SELECT c.idCliente,COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1 FROM tblClienteFacturaVersion v WHERE v.idCliente=c.idCliente),1),c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,c.nombreCliente,c.direccionCliente
    FROM tblCliente c WHERE c.idCliente=?";
            _Datos.VentasFuncEjecutar(Consulta, Cliente);
            return _Datos.VentasFuncObtenerId();
        }

        public int VentasFuncConvertirInsertarFactura(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaEncabezado(uuidDteFacturaEncabezado,serieFacturaEncabezado,numeroFacturaEncabezado,idSucursal,idVendedor,idMoneda,fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,tipoDteFacturaEncabezado,condicionPagoFacturaEncabezado,estadoFacturaEncabezado,observacionesFacturaEncabezado,idUsuarioRegistro,idClienteFacturaVersion,idCotizacionEncabezado)
    VALUES (UUID(),'COT',?,?,?,?,?,?,'FACT',?,'BORRADOR',?,?,?,?)";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirInsertarFacturadetalle(params object[] Parametros)
        {
            const string Consulta = @"INSERT INTO tblFacturaDetalle(idFacturaEncabezadoUuidDte,numeroLineaFacturaDetalle,idProducto,cantidadFacturaDetalle,precioUnitarioFacturaDetalle,descuentoMontoFacturaDetalle,tasaImpuestoFacturaDetalle)
    SELECT (SELECT uuidDteFacturaEncabezado FROM tblFacturaEncabezado WHERE idFacturaEncabezado=?),
           numeroLineaCotizacionDetalle,idProducto,cantidadCotizacionDetalle,precioUnitarioCotizacionDetalle,descuentoMontoCotizacionDetalle,tasaImpuestoCotizacionDetalle
    FROM tblCotizacionDetalle
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }

        public int VentasFuncConvertirActualizarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"UPDATE tblCotizacionEncabezado
    SET estadoCotizacionEncabezado='ACEPTADA'
    WHERE idCotizacionEncabezado=?";
            return _Datos.VentasFuncEjecutar(Consulta, Parametros);
        }
    }
}
