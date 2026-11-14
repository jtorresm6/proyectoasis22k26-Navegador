/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Consultar y guardar pedidos en la base de datos.
 */
using System;
using System.Data;

namespace CapaModelo_Pedidos_Ventas
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Consulta clientes, precios, cotizaciones y reportes de Pedidos.
    public partial class ClsPedidoRepositorio
    {
        public virtual DataTable VentasFuncCatalogo(string Nombre)
        {
            string Sql;
            switch (Nombre)
            {
                case "Clientes":
                    Sql = @"SELECT cl.idCliente Id,
                        CONCAT(codigoCliente, ' - ', nombreCliente) Nombre,
                        COALESCE(nitCliente, dpiCliente, 'CF') numeroIdentificacion,
                        direccionCliente, COALESCE(NULLIF(TRIM(cl.nombreContactoCliente), ''),
                                 NULLIF(TRIM(cl.telefonoCliente), ''),
                                 NULLIF(TRIM(cl.correoCliente), ''), 'Sin contacto registrado') contactoCliente,
                        COALESCE(t.nombreTipoCliente, 'Sin tipo asignado') TipoCliente,
                        t.idTipoCliente TipoClienteId
                        FROM tblCliente cl
                        LEFT JOIN tblClienteListaPrecio clp ON clp.idCliente=cl.idCliente
                        LEFT JOIN tblListaPrecioEncabezado l ON l.idListaPrecioEncabezado=clp.idListaPrecioEncabezado
                        LEFT JOIN tblTipoCliente t ON t.idTipoCliente=l.idTipoCliente
                        WHERE cl.activoCliente=1 ORDER BY cl.nombreCliente";
                    break;
                case "Vendedores":
                    Sql = @"SELECT v.idVendedor Id,
                        CONCAT(v.codigoVendedor, ' - ', e.nombresEmpleado, ' ', e.apellidosEmpleado) Nombre
                        FROM tblVendedor v JOIN tblEmpleado e ON e.idEmpleado=v.idEmpleado
                        WHERE v.activoVendedor=1 ORDER BY v.codigoVendedor";
                    break;
                default:
                    throw new ArgumentException("Catálogo no reconocido.");
            }
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(Sql);
        }

        public virtual DataTable VentasFuncListas(int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT l.idListaPrecioEncabezado Id,
                        l.nombreListaPrecioEncabezado Nombre, l.idMoneda MonedaId,
                        l.idTipoCliente TipoClienteId
                    FROM tblClienteListaPrecio c
                    JOIN tblListaPrecioEncabezado l
                        ON l.idListaPrecioEncabezado = c.idListaPrecioEncabezado
                    JOIN tblTipoCliente t ON t.idTipoCliente = l.idTipoCliente
                    WHERE c.idCliente = ? AND l.activoListaPrecioEncabezado = 1
                        AND t.activoTipoCliente = 1
                        AND l.vigenteDesdeListaPrecioEncabezado <= ?
                        AND (l.vigenteHastaListaPrecioEncabezado IS NULL
                            OR l.vigenteHastaListaPrecioEncabezado >= ?)
                    ORDER BY l.nombreListaPrecioEncabezado", Cliente, Fecha.Date, Fecha.Date);
        }

        public virtual DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT c.idCotizacionEncabezado CotizacionId,
                        c.numeroCotizacionEncabezado Nombre, c.idVendedor VendedorId,
                        c.idMoneda MonedaId,
                        d.idProducto ProductoId, p.skuProducto Codigo,
                        p.nombreProducto Descripcion,
                        d.cantidadCotizacionDetalle Cantidad, d.precioUnitarioCotizacionDetalle Precio,
                        d.descuentoMontoCotizacionDetalle Descuento
                    FROM tblCotizacionEncabezado c
                    JOIN tblCotizacionDetalle d
                        ON d.idCotizacionEncabezado = c.idCotizacionEncabezado
                    JOIN tblProducto p ON p.idProducto = d.idProducto
                    WHERE c.idCliente = ?
                        AND c.estadoCotizacionEncabezado IN ('EMITIDA', 'ACEPTADA')
                        AND c.fechaEmisionCotizacionEncabezado <= ?
                        AND c.vigenteHastaCotizacionEncabezado >= ?
                        AND NOT EXISTS (SELECT 1 FROM tblPedidoEncabezado pe
                            WHERE pe.idCotizacionEncabezado = c.idCotizacionEncabezado)
                        AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado fe
                            WHERE fe.idCotizacionEncabezado = c.idCotizacionEncabezado)
                    ORDER BY c.numeroCotizacionEncabezado, d.numeroLineaCotizacionDetalle",
                    Cliente, Fecha.Date, Fecha.Date);
        }

        public virtual bool VentasFuncCotVigente(int Cotizacion, int Cliente, DateTime Fecha)
        {
            using (var Datos = new ClsConexion())
                return Convert.ToInt32(Datos.VentasFuncEscalar(@"SELECT COUNT(*)
                    FROM tblCotizacionEncabezado c
                    WHERE c.idCotizacionEncabezado = ? AND c.idCliente = ?
                        AND c.estadoCotizacionEncabezado IN ('EMITIDA', 'ACEPTADA')
                        AND c.fechaEmisionCotizacionEncabezado <= ?
                        AND c.vigenteHastaCotizacionEncabezado >= ?
                        AND NOT EXISTS (SELECT 1 FROM tblPedidoEncabezado p
                            WHERE p.idCotizacionEncabezado = c.idCotizacionEncabezado)
                        AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado f
                            WHERE f.idCotizacionEncabezado = c.idCotizacionEncabezado)",
                    Cotizacion, Cliente, Fecha.Date, Fecha.Date)) == 1;
        }

        public virtual DataTable VentasFuncProductos(int Lista)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT pv.idProductoVenta ProductoId,
                        CONCAT(p.skuProducto, ' - ', p.nombreProducto) Nombre,
                        p.skuProducto Codigo, p.nombreProducto Descripcion,
                        d.precioUnitarioListaPrecioDetalle Precio
                    FROM tblProductoVenta pv
                    JOIN tblProducto p ON p.idProducto = pv.idProductoVenta
                    JOIN tblListaPrecioDetalle d ON d.idProducto = pv.idProductoVenta
                    WHERE pv.activoProductoVenta = 1 AND d.activoListaPrecioDetalle = 1
                        AND d.idListaPrecioEncabezado = ?
                    ORDER BY p.nombreProducto", Lista);
        }

        public virtual string VentasFuncMoneda(int Id)
        {
            using (var Datos = new ClsConexion())
                return Convert.ToString(Datos.VentasFuncEscalar(
                    "SELECT codigoMoneda FROM tblMoneda WHERE idMoneda = ?", Id)) ?? "";
        }

        public virtual DataTable VentasFuncListar()
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT p.idPedidoEncabezado Id,
                        p.referenciaPedidoEncabezado Numero, p.estadoPedidoEncabezado Estado,
                        p.fechaPedidoEncabezado Fecha, c.nombreCliente Cliente
                    FROM tblPedidoEncabezado p
                    JOIN tblCliente c ON c.idCliente = p.idCliente
                    ORDER BY p.idPedidoEncabezado DESC");
        }

        public virtual DataTable VentasFuncReporte(DateTime Desde, DateTime Hasta, string Estado)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT p.referenciaPedidoEncabezado Numero,
                        p.fechaPedidoEncabezado Fecha, c.nombreCliente Cliente,
                        p.estadoPedidoEncabezado Estado, m.codigoMoneda Moneda,
                        COALESCE(SUM(d.cantidadPedidoDetalle * d.precioUnitarioPedidoDetalle
                            - d.descuentoMontoPedidoDetalle), 0) Total
                    FROM tblPedidoEncabezado p
                    JOIN tblCliente c ON c.idCliente = p.idCliente
                    JOIN tblMoneda m ON m.idMoneda = p.idMoneda
                    LEFT JOIN tblPedidoDetalle d ON d.idPedidoEncabezado = p.idPedidoEncabezado
                    WHERE p.fechaPedidoEncabezado >= ? AND p.fechaPedidoEncabezado <= ?
                        AND (? = 'Todos los estados' OR p.estadoPedidoEncabezado = ?)
                    GROUP BY p.idPedidoEncabezado, p.referenciaPedidoEncabezado,
                        p.fechaPedidoEncabezado, c.nombreCliente, p.estadoPedidoEncabezado,
                        m.codigoMoneda
                    ORDER BY p.fechaPedidoEncabezado, p.idPedidoEncabezado", Desde.Date, Hasta.Date, Estado, Estado);
        }

        public virtual DataTable VentasFuncPedido(int Id)
        {
            using (var Datos = new ClsConexion())
                return Datos.VentasFuncTabla(@"SELECT p.idPedidoEncabezado Id,
                        p.referenciaPedidoEncabezado Numero,
                        p.idCotizacionEncabezado CotizacionId,
                        p.idCliente ClienteId, p.idVendedor VendedorId,
                        COALESCE(cl.nitCliente, cl.dpiCliente, 'CF') numeroIdentificacion,
                        cl.direccionCliente, COALESCE(NULLIF(TRIM(cl.nombreContactoCliente), ''),
                                 NULLIF(TRIM(cl.telefonoCliente), ''),
                                 NULLIF(TRIM(cl.correoCliente), ''), 'Sin contacto registrado') contactoCliente,
                        p.idMoneda MonedaId, p.fechaPedidoEncabezado Fecha,
                        p.fechaEntregaPedidoEncabezado Entrega,
                        p.fechaVencimientoPedidoEncabezado Vencimiento,
                        p.estadoPedidoEncabezado Estado,
                        c.numeroCotizacionEncabezado CotizacionNumero,
                        d.idProducto ProductoId, pr.skuProducto Codigo,
                        pr.nombreProducto Descripcion, d.cantidadPedidoDetalle Cantidad,
                        d.precioUnitarioPedidoDetalle Precio, d.descuentoMontoPedidoDetalle Descuento
                    FROM tblPedidoEncabezado p
                    JOIN tblCliente cl ON cl.idCliente = p.idCliente
                    LEFT JOIN tblCotizacionEncabezado c
                        ON c.idCotizacionEncabezado = p.idCotizacionEncabezado
                    LEFT JOIN tblPedidoDetalle d
                        ON d.idPedidoEncabezado = p.idPedidoEncabezado
                    LEFT JOIN tblProducto pr ON pr.idProducto = d.idProducto
                    WHERE p.idPedidoEncabezado = ?
                    ORDER BY d.OrdenPedido", Id);
        }

    }
}
