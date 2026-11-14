// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_Cotizaciones_Ventas
{
    public sealed class ClsConsultasCotizaciones
    {
        private readonly ClsDatosVentas _Datos;

        internal ClsConsultasCotizaciones(ClsDatosVentas Datos)
        {
            _Datos = Datos;
        }

        // Productos: consultas parametrizadas.
        public DataTable VentasFuncProductosConsultarProductoventas(params object[] Parametros)
        {
            const string Consulta = @"SELECT p.idProductoVenta Id,CONCAT(pr.skuProducto,' - ',pr.nombreProducto) Nombre,pr.nombreProducto Descripcion,p.tipoItemProductoVenta Tipo,p.tasaImpuestoProductoVenta*100 Tasa,d.precioUnitarioListaPrecioDetalle Precio
    FROM tblProductoVenta p
    JOIN tblProducto pr ON pr.idProducto=p.idProductoVenta
    JOIN tblListaPrecioDetalle d ON d.idProducto=p.idProductoVenta
    WHERE d.idListaPrecioEncabezado=?
      AND d.activoListaPrecioDetalle=1
      AND p.activoProductoVenta=1
    ORDER BY pr.nombreProducto";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Listar: consultas parametrizadas.
        public DataTable VentasFuncListarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.idCotizacionEncabezado Id,c.numeroCotizacionEncabezado Numero,cl.nombreCliente Cliente,m.codigoMoneda Moneda,c.fechaEmisionCotizacionEncabezado Fecha,c.vigenteHastaCotizacionEncabezado Vigencia,COALESCE((SELECT SUM(d.totalLineaCotizacionDetalle) FROM tblCotizacionDetalle d WHERE d.idCotizacionEncabezado=c.idCotizacionEncabezado),0) Total,CASE WHEN c.estadoCotizacionEncabezado IN ('BORRADOR','EMITIDA','ACEPTADA')
      AND c.vigenteHastaCotizacionEncabezado<CURRENT_DATE() THEN 'VENCIDA' ELSE c.estadoCotizacionEncabezado END Estado
    FROM tblCotizacionEncabezado c
    JOIN tblCliente cl ON cl.idCliente=c.idCliente
    JOIN tblMoneda m ON m.idMoneda=c.idMoneda
    WHERE (?='' OR c.numeroCotizacionEncabezado LIKE CONCAT('%',?,'%') OR cl.nombreCliente LIKE CONCAT('%',?,'%'))
    ORDER BY c.idCotizacionEncabezado DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Lista: consultas parametrizadas.
        public DataTable VentasFuncListaConsultarClientelistaprecios(params object[] Parametros)
        {
            const string Consulta = @"SELECT l.idListaPrecioEncabezado idListaPrecio,l.nombreListaPrecioEncabezado nombreListaPrecio
    FROM tblClienteListaPrecio c
    JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=c.idListaPrecioEncabezado
    WHERE c.idCliente=?
      AND l.idMoneda=?
      AND l.activoListaPrecioEncabezado=1
      AND l.vigenteDesdeListaPrecioEncabezado<=?
      AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR l.vigenteHastaListaPrecioEncabezado>=?)";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Generales: consultas parametrizadas.
        public DataTable VentasFuncGeneralesConsultarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.idCotizacionEncabezado idCotizacion,c.numeroCotizacionEncabezado numeroCotizacion,c.idCliente idCliente,c.idVendedor idVendedor,c.idMoneda idMoneda,c.fechaEmisionCotizacionEncabezado fechaEmision,c.vigenteHastaCotizacionEncabezado vigenteHasta,c.estadoCotizacionEncabezado estadoCotizacion,1 tipoCambio,l.nombreListaPrecioEncabezado nombreListaPrecio,c.idListaPrecioEncabezado idListaPrecio,p.idPedidoEncabezado idPedido,f.idFacturaEncabezado idFactura,CASE WHEN c.estadoCotizacionEncabezado IN ('EMITIDA','ACEPTADA','BORRADOR')
      AND c.vigenteHastaCotizacionEncabezado<CURRENT_DATE() THEN 'VENCIDA' ELSE c.estadoCotizacionEncabezado END EstadoVisual
    FROM tblCotizacionEncabezado c
    JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=c.idListaPrecioEncabezado
    LEFT JOIN tblPedidoEncabezado p ON p.idCotizacionEncabezado=c.idCotizacionEncabezado
    LEFT JOIN tblFacturaEncabezado f ON f.idCotizacionEncabezado=c.idCotizacionEncabezado
    WHERE c.numeroCotizacionEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGeneralesConsultarConsultarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT d.idProducto idProducto,d.descripcionItemCotizacionDetalle descripcionItem,p.tipoItemProductoVenta tipoItem,d.cantidadCotizacionDetalle cantidad,d.precioUnitarioCotizacionDetalle precioUnitario,d.descuentoMontoCotizacionDetalle descuentoMonto,d.tasaImpuestoCotizacionDetalle*100 porcentajeImpuesto,d.impuestoMontoCotizacionDetalle impuestoMonto,d.totalLineaCotizacionDetalle totalLinea,pr.skuProducto codigoProducto
    FROM tblCotizacionDetalle d
    JOIN tblProductoVenta p ON p.idProductoVenta=d.idProducto
    JOIN tblProducto pr ON pr.idProducto=d.idProducto
    WHERE d.idCotizacionEncabezado=?
    ORDER BY d.numeroLineaCotizacionDetalle";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
