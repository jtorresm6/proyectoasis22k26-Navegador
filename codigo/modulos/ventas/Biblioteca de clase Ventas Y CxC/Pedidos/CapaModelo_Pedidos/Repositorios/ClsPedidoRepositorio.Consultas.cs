using System;
using System.Data;

namespace CapaModelo_Pedidos
{
    public sealed partial class ClsPedidoRepositorio
    {
        public DataTable VentasFuncCatalogo(string Nombre)
        {
            string Sql;
            switch (Nombre)
            {
                case "Clientes":
                    Sql = @"SELECT Pk_Cliente Id,
                        CONCAT(codigoCliente, ' - ', nombreCliente) Nombre,
                        COALESCE(nitCliente, dpiCliente, identificacionExtranjeraCliente, 'CF') numeroIdentificacion,
                        direccionCliente, nombreContactoCliente contactoCliente
                        FROM tblCliente WHERE activoCliente=1 ORDER BY nombreCliente";
                    break;
                case "Vendedores":
                    Sql = @"SELECT v.Pk_Vendedor Id,
                        CONCAT(v.codigoVendedor, ' - ', e.nombresEmpleado, ' ', e.apellidosEmpleado) Nombre
                        FROM tblVendedor v JOIN tblEmpleado e ON e.idEmpleado=v.Fk_Empleado_Codigo
                        WHERE v.activoVendedor=1 ORDER BY v.codigoVendedor";
                    break;
                default:
                    throw new ArgumentException("Catálogo no reconocido.");
            }
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(Sql);
        }

        public DataTable VentasFuncListas(int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT l.Pk_ListaPrecioEncabezado Id,
                        l.nombreListaPrecioEncabezado Nombre, l.Fk_Moneda_Codigo MonedaId
                    FROM tblClienteListaPrecio c
                    JOIN tblListaPrecioEncabezado l
                        ON l.Pk_ListaPrecioEncabezado = c.Fk_ListaPrecioEncabezado_Codigo
                    WHERE c.Fk_Cliente_Codigo = ? AND l.activoListaPrecioEncabezado = 1
                        AND l.vigenteDesdeListaPrecioEncabezado <= ?
                        AND (l.vigenteHastaListaPrecioEncabezado IS NULL
                            OR l.vigenteHastaListaPrecioEncabezado >= ?)
                    ORDER BY l.nombreListaPrecioEncabezado", Cliente, Fecha.Date, Fecha.Date);
        }

        public DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT c.Pk_CotizacionEncabezado CotizacionId,
                        c.numeroCotizacionEncabezado Nombre, c.Fk_Vendedor_Codigo VendedorId,
                        c.Fk_Moneda_Codigo MonedaId,
                        d.Fk_Producto_Codigo ProductoId, p.skuProducto Codigo,
                        p.nombreProducto Descripcion,
                        d.cantidadCotizacionDetalle Cantidad, d.precioUnitarioCotizacionDetalle Precio,
                        d.descuentoMontoCotizacionDetalle Descuento
                    FROM tblCotizacionEncabezado c
                    JOIN tblCotizacionDetalle d
                        ON d.Fk_CotizacionEncabezado_Codigo = c.Pk_CotizacionEncabezado
                    JOIN tblProducto p ON p.Pk_Producto = d.Fk_Producto_Codigo
                    WHERE c.Fk_Cliente_Codigo = ?
                        AND c.estadoCotizacionEncabezado IN ('EMITIDA', 'ACEPTADA')
                        AND c.fechaEmisionCotizacionEncabezado <= ?
                        AND c.vigenteHastaCotizacionEncabezado >= ?
                        AND NOT EXISTS (SELECT 1 FROM tblPedidoEncabezado pe
                            WHERE pe.Fk_CotizacionEncabezado_Codigo = c.Pk_CotizacionEncabezado)
                        AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado fe
                            WHERE fe.Fk_CotizacionEncabezado_Codigo = c.Pk_CotizacionEncabezado)
                    ORDER BY c.numeroCotizacionEncabezado, d.numeroLineaCotizacionDetalle",
                    Cliente, Fecha.Date, Fecha.Date);
        }

        public bool VentasFuncCotVigente(int Cotizacion, int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Convert.ToInt32(Datos.VentasFuncEscalar(@"SELECT COUNT(*)
                    FROM tblCotizacionEncabezado c
                    WHERE c.Pk_CotizacionEncabezado = ? AND c.Fk_Cliente_Codigo = ?
                        AND c.estadoCotizacionEncabezado IN ('EMITIDA', 'ACEPTADA')
                        AND c.fechaEmisionCotizacionEncabezado <= ?
                        AND c.vigenteHastaCotizacionEncabezado >= ?
                        AND NOT EXISTS (SELECT 1 FROM tblPedidoEncabezado p
                            WHERE p.Fk_CotizacionEncabezado_Codigo = c.Pk_CotizacionEncabezado)
                        AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado f
                            WHERE f.Fk_CotizacionEncabezado_Codigo = c.Pk_CotizacionEncabezado)",
                    Cotizacion, Cliente, Fecha.Date, Fecha.Date)) == 1;
        }

        public DataTable VentasFuncProductos(int Lista)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT pv.Pk_ProductoVenta ProductoId,
                        CONCAT(p.skuProducto, ' - ', p.nombreProducto) Nombre,
                        p.skuProducto Codigo, p.nombreProducto Descripcion,
                        d.precioUnitarioListaPrecioDetalle Precio
                    FROM tblProductoVenta pv
                    JOIN tblProducto p ON p.Pk_Producto = pv.Pk_ProductoVenta
                    JOIN tblListaPrecioDetalle d ON d.Fk_Producto_Codigo = pv.Pk_ProductoVenta
                    WHERE pv.activoProductoVenta = 1 AND d.activoListaPrecioDetalle = 1
                        AND d.Fk_ListaPrecioEncabezado_Codigo = ?
                    ORDER BY p.nombreProducto", Lista);
        }

        public string VentasFuncMoneda(int Id)
        {
            using (var Datos = new ClsConexion())
                return Convert.ToString(Datos.VentasFuncEscalar(
                    "SELECT codigoMoneda FROM tblMoneda WHERE Pk_Moneda = ?", Id)) ?? "";
        }

        public DataTable VentasFuncListar()
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT p.Pk_PedidoEncabezado Id,
                        p.referenciaPedidoEncabezado Numero, p.estadoPedidoEncabezado Estado,
                        p.fechaPedidoEncabezado Fecha, c.nombreCliente Cliente
                    FROM tblPedidoEncabezado p
                    JOIN tblCliente c ON c.Pk_Cliente = p.Fk_Cliente_Codigo
                    ORDER BY p.Pk_PedidoEncabezado DESC");
        }

        public DataTable VentasFuncPedido(int Id)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT p.Pk_PedidoEncabezado Id,
                        p.referenciaPedidoEncabezado Numero,
                        p.Fk_CotizacionEncabezado_Codigo CotizacionId,
                        p.Fk_Cliente_Codigo ClienteId, p.Fk_Vendedor_Codigo VendedorId,
                        p.Fk_Moneda_Codigo MonedaId, p.fechaPedidoEncabezado Fecha,
                        p.fechaEntregaPedidoEncabezado Entrega,
                        p.fechaVencimientoPedidoEncabezado Vencimiento,
                        p.estadoPedidoEncabezado Estado,
                        c.numeroCotizacionEncabezado CotizacionNumero,
                        d.Fk_Producto_Codigo ProductoId, pr.skuProducto Codigo,
                        pr.nombreProducto Descripcion, d.cantidadPedidoDetalle Cantidad,
                        d.precioUnitarioPedidoDetalle Precio, d.descuentoMontoPedidoDetalle Descuento
                    FROM tblPedidoEncabezado p
                    LEFT JOIN tblCotizacionEncabezado c
                        ON c.Pk_CotizacionEncabezado = p.Fk_CotizacionEncabezado_Codigo
                    LEFT JOIN tblPedidoDetalle d
                        ON d.Fk_PedidoEncabezado_Codigo = p.Pk_PedidoEncabezado
                    LEFT JOIN tblProducto pr ON pr.Pk_Producto = d.Fk_Producto_Codigo
                    WHERE p.Pk_PedidoEncabezado = ?
                    ORDER BY d.OrdenPedido", Id);
        }

    }
}
