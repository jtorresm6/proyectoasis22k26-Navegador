// Jose Javier Torres Martinez | 0901-23-1091 | 23/10/2026
using System.Data;
using System;
using VentasDatos;

namespace CapaModelo_Cotizaciones
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
            const string Consulta = @"SELECT p.Pk_ProductoVenta Id,CONCAT(pr.skuProducto,' - ',pr.nombreProducto) Nombre,pr.nombreProducto Descripcion,p.tipoItemProductoVenta Tipo,p.tasaImpuestoProductoVenta*100 Tasa,d.precioUnitarioListaPrecioDetalle Precio
    FROM tblProductoVenta p
    JOIN tblProducto pr ON pr.Pk_Producto=p.Pk_ProductoVenta
    JOIN tblListaPrecioDetalle d ON d.Fk_Producto_Codigo=p.Pk_ProductoVenta
    WHERE d.Fk_ListaPrecioEncabezado_Codigo=?
      AND d.activoListaPrecioDetalle=1
      AND p.activoProductoVenta=1
    ORDER BY pr.nombreProducto";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Listar: consultas parametrizadas.
        public DataTable VentasFuncListarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.Pk_CotizacionEncabezado Id,c.numeroCotizacionEncabezado Numero,cl.nombreCliente Cliente,m.codigoMoneda Moneda,c.fechaEmisionCotizacionEncabezado Fecha,c.vigenteHastaCotizacionEncabezado Vigencia,COALESCE((SELECT SUM(d.totalLineaCotizacionDetalle) FROM tblCotizacionDetalle d WHERE d.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado),0) Total,CASE WHEN c.estadoCotizacionEncabezado IN ('BORRADOR','EMITIDA','ACEPTADA')
      AND c.vigenteHastaCotizacionEncabezado<CURRENT_DATE() THEN 'VENCIDA' ELSE c.estadoCotizacionEncabezado END Estado
    FROM tblCotizacionEncabezado c
    JOIN tblCliente cl ON cl.Pk_Cliente=c.Fk_Cliente_Codigo
    JOIN tblMoneda m ON m.Pk_Moneda=c.Fk_Moneda_Codigo
    WHERE (?='' OR c.numeroCotizacionEncabezado LIKE CONCAT('%',?,'%') OR cl.nombreCliente LIKE CONCAT('%',?,'%'))
    ORDER BY c.Pk_CotizacionEncabezado DESC";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Lista: consultas parametrizadas.
        public DataTable VentasFuncListaConsultarClientelistaprecios(params object[] Parametros)
        {
            const string Consulta = @"SELECT l.Pk_ListaPrecioEncabezado idListaPrecio,l.nombreListaPrecioEncabezado nombreListaPrecio
    FROM tblClienteListaPrecio c
    JOIN tblListaPrecioEncabezado l ON l.Pk_ListaPrecioEncabezado=c.Fk_ListaPrecioEncabezado_Codigo
    WHERE c.Fk_Cliente_Codigo=?
      AND l.Fk_Moneda_Codigo=?
      AND l.activoListaPrecioEncabezado=1
      AND l.vigenteDesdeListaPrecioEncabezado<=?
      AND (l.vigenteHastaListaPrecioEncabezado IS NULL OR l.vigenteHastaListaPrecioEncabezado>=?)";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        // Generales: consultas parametrizadas.
        public DataTable VentasFuncGeneralesConsultarConsultarCotizacion(params object[] Parametros)
        {
            const string Consulta = @"SELECT c.Pk_CotizacionEncabezado idCotizacion,c.numeroCotizacionEncabezado numeroCotizacion,c.Fk_Cliente_Codigo idCliente,c.Fk_Vendedor_Codigo idVendedor,c.Fk_Moneda_Codigo idMoneda,c.fechaEmisionCotizacionEncabezado fechaEmision,c.vigenteHastaCotizacionEncabezado vigenteHasta,c.estadoCotizacionEncabezado estadoCotizacion,1 tipoCambio,l.nombreListaPrecioEncabezado nombreListaPrecio,c.Fk_ListaPrecioEncabezado_Codigo idListaPrecio,p.Pk_PedidoEncabezado idPedido,f.Pk_FacturaEncabezado idFactura,CASE WHEN c.estadoCotizacionEncabezado IN ('EMITIDA','ACEPTADA','BORRADOR')
      AND c.vigenteHastaCotizacionEncabezado<CURRENT_DATE() THEN 'VENCIDA' ELSE c.estadoCotizacionEncabezado END EstadoVisual
    FROM tblCotizacionEncabezado c
    JOIN tblListaPrecioEncabezado l ON l.Pk_ListaPrecioEncabezado=c.Fk_ListaPrecioEncabezado_Codigo
    LEFT JOIN tblPedidoEncabezado p ON p.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado
    LEFT JOIN tblFacturaEncabezado f ON f.Fk_CotizacionEncabezado_Codigo=c.Pk_CotizacionEncabezado
    WHERE c.numeroCotizacionEncabezado=?";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }

        public DataTable VentasFuncGeneralesConsultarConsultarCotizaciondetalle(params object[] Parametros)
        {
            const string Consulta = @"SELECT d.Fk_Producto_Codigo idProducto,d.descripcionItemCotizacionDetalle descripcionItem,p.tipoItemProductoVenta tipoItem,d.cantidadCotizacionDetalle cantidad,d.precioUnitarioCotizacionDetalle precioUnitario,d.descuentoMontoCotizacionDetalle descuentoMonto,d.tasaImpuestoCotizacionDetalle*100 porcentajeImpuesto,d.impuestoMontoCotizacionDetalle impuestoMonto,d.totalLineaCotizacionDetalle totalLinea,pr.skuProducto codigoProducto
    FROM tblCotizacionDetalle d
    JOIN tblProductoVenta p ON p.Pk_ProductoVenta=d.Fk_Producto_Codigo
    JOIN tblProducto pr ON pr.Pk_Producto=d.Fk_Producto_Codigo
    WHERE d.Fk_CotizacionEncabezado_Codigo=?
    ORDER BY d.numeroLineaCotizacionDetalle";
            return _Datos.VentasFuncTabla(Consulta, Parametros);
        }
    }
}
