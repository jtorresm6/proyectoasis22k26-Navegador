using System;
using System.Data;
using VentasDatos;

namespace CapaModelo_Facturas_Ventas
{
    public sealed partial class ClsFacturaOrigenRepositorio
    {
        // dbSistemaEmbutidos v1.6 usa ids descriptivos (idCotizacionEncabezado,
        // idPedidoEncabezado, idFacturaEncabezado) y relaciones con esos mismos nombres.
        private const string _Cotizaciones = @"SELECT q.idCotizacionEncabezado Id,
            CONCAT(q.numeroCotizacionEncabezado, ' - ', c.nombreCliente) Nombre,
            q.idCotizacionEncabezado CotizacionId, NULL PedidoId, q.idCliente ClienteId,
            q.idVendedor VendedorId, q.idMoneda MonedaId, m.codigoMoneda Moneda,
            COALESCE(c.nitCliente, c.dpiCliente, 'CF') Identificacion, c.nombreCliente Cliente,
            COALESCE(NULLIF(c.direccionCliente,''), 'CIUDAD') Direccion, c.diasCreditoCliente DiasCredito
            FROM tblCotizacionEncabezado q
            JOIN tblCliente c ON c.idCliente=q.idCliente AND c.activoCliente=1
            JOIN tblMoneda m ON m.idMoneda=q.idMoneda
            WHERE (?=0 OR q.idCotizacionEncabezado=?)
              AND q.estadoCotizacionEncabezado IN ('EMITIDA','ACEPTADA')
              AND CURRENT_DATE BETWEEN q.fechaEmisionCotizacionEncabezado AND q.vigenteHastaCotizacionEncabezado
              AND NOT EXISTS (SELECT 1 FROM tblPedidoEncabezado p WHERE p.idCotizacionEncabezado=q.idCotizacionEncabezado)
              AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado f WHERE f.idCotizacionEncabezado=q.idCotizacionEncabezado)
              AND EXISTS (SELECT 1 FROM tblCotizacionDetalle d WHERE d.idCotizacionEncabezado=q.idCotizacionEncabezado)
            ORDER BY q.numeroCotizacionEncabezado";

        private const string _Pedidos = @"SELECT p.idPedidoEncabezado Id,
            CONCAT(p.referenciaPedidoEncabezado, ' - ', c.nombreCliente) Nombre,
            p.idCotizacionEncabezado CotizacionId, p.idPedidoEncabezado PedidoId, p.idCliente ClienteId,
            p.idVendedor VendedorId, p.idMoneda MonedaId, m.codigoMoneda Moneda,
            COALESCE(c.nitCliente, c.dpiCliente, 'CF') Identificacion, c.nombreCliente Cliente,
            COALESCE(NULLIF(c.direccionCliente,''), 'CIUDAD') Direccion, c.diasCreditoCliente DiasCredito
            FROM tblPedidoEncabezado p
            JOIN tblCliente c ON c.idCliente=p.idCliente AND c.activoCliente=1
            JOIN tblMoneda m ON m.idMoneda=p.idMoneda
            WHERE (?=0 OR p.idPedidoEncabezado=?)
              AND p.estadoPedidoEncabezado IN ('PENDIENTE','CONFIRMADO','COMPLETADO')
              AND p.fechaPedidoEncabezado <= CURRENT_DATE
              AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado f
                  WHERE f.idPedidoEncabezado=p.idPedidoEncabezado
                     OR (p.idCotizacionEncabezado IS NOT NULL
                         AND f.idCotizacionEncabezado=p.idCotizacionEncabezado))
              AND EXISTS (SELECT 1 FROM tblPedidoDetalle d WHERE d.idPedidoEncabezado=p.idPedidoEncabezado)
            ORDER BY p.referenciaPedidoEncabezado";

        private const string _LineasCotizacion = @"SELECT d.idProducto ProductoId,
            d.descripcionItemCotizacionDetalle Producto, d.cantidadCotizacionDetalle Cantidad,
            d.precioUnitarioCotizacionDetalle Precio, d.descuentoMontoCotizacionDetalle Descuento,
            d.tasaImpuestoCotizacionDetalle Tasa
            FROM tblCotizacionDetalle d
            WHERE d.idCotizacionEncabezado=?
            ORDER BY d.numeroLineaCotizacionDetalle";

        private const string _LineasPedido = @"SELECT d.idProducto ProductoId,
            COALESCE(c.descripcionItemCotizacionDetalle,p.nombreProducto) Producto,
            d.cantidadPedidoDetalle Cantidad, d.precioUnitarioPedidoDetalle Precio,
            d.descuentoMontoPedidoDetalle Descuento, COALESCE(c.tasaImpuestoCotizacionDetalle,pv.tasaImpuestoProductoVenta,0) Tasa
            FROM tblPedidoDetalle d
            JOIN tblPedidoEncabezado h ON h.idPedidoEncabezado=d.idPedidoEncabezado
            JOIN tblProducto p ON p.idProducto=d.idProducto
            LEFT JOIN tblProductoVenta pv ON pv.idProductoVenta=d.idProducto
            LEFT JOIN tblCotizacionDetalle c ON c.idCotizacionEncabezado=h.idCotizacionEncabezado
                AND c.numeroLineaCotizacionDetalle=d.OrdenPedido
                AND c.idProducto=d.idProducto
            WHERE d.idPedidoEncabezado=?
            ORDER BY d.OrdenPedido";

        public DataTable VentasFuncOrigenes(bool Pedido)
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla(Pedido ? _Pedidos : _Cotizaciones, 0, 0);
        }

        public DataTable VentasFuncSucursales()
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla("SELECT idSucursal Id,nombreSucursal Nombre FROM tblSucursal ORDER BY nombreSucursal");
        }

        public DataSet VentasFuncCargar(bool Pedido, int Id)
        {
            using (var Datos = new ClsDatosVentas())
                return VentasFuncDocumento(Datos, Pedido, Id);
        }

        private static DataSet VentasFuncDocumento(ClsDatosVentas Datos, bool Pedido, int Id)
        {
            if (Id <= 0) throw new ArgumentException("Seleccione un documento de origen.");
            DataTable Encabezado = Datos.VentasFuncTabla(Pedido ? _Pedidos : _Cotizaciones, Id, Id);
            if (Encabezado.Rows.Count != 1)
                throw new InvalidOperationException("El documento ya fue utilizado o dejó de estar disponible.");

            DataTable Lineas = Datos.VentasFuncTabla(Pedido ? _LineasPedido : _LineasCotizacion, Id);
            if (Lineas.Rows.Count == 0) throw new InvalidOperationException("El documento no tiene productos.");
            Lineas.Columns.Add("Subtotal", typeof(decimal));
            Lineas.Columns.Add("Impuesto", typeof(decimal));
            Lineas.Columns.Add("Total", typeof(decimal));
            decimal Tasa = VentasFuncTasa(Datos);
            foreach (DataRow Linea in Lineas.Rows)
            {
                Linea["Tasa"] = Tasa;
                var Importes = ClsReglasFactura.VentasFuncCalcular(Convert.ToDecimal(Linea["Cantidad"]), Convert.ToDecimal(Linea["Precio"]), Convert.ToDecimal(Linea["Descuento"]), Tasa);
                Linea["Subtotal"] = Importes[0]; Linea["Impuesto"] = Importes[1]; Linea["Total"] = Importes[2];
            }            var Documento = new DataSet();
            Documento.Tables.Add(Encabezado);
            Documento.Tables.Add(Lineas);
            return Documento;
        }

        private static decimal VentasFuncRedondear(decimal Monto)
        {
            return Math.Round(Monto, 4, MidpointRounding.AwayFromZero);
        }

    }
}
