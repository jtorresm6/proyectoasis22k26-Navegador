using System;
using System.Data;
using VentasDatos;

namespace CapaModelo_Facturas
{
    public sealed class ClsFacturaOrigenRepositorio
    {
        private const string Cotizaciones = @"SELECT q.idCotizacionEncabezado Id,
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

        private const string Pedidos = @"SELECT p.idPedidoEncabezado Id,
            CONCAT(p.referenciaPedidoEncabezado, ' - ', c.nombreCliente) Nombre,
            p.idCotizacionEncabezado CotizacionId, p.idPedidoEncabezado PedidoId, p.idCliente ClienteId,
            p.idVendedor VendedorId, p.idMoneda MonedaId, m.codigoMoneda Moneda,
            COALESCE(c.nitCliente, c.dpiCliente, 'CF') Identificacion, c.nombreCliente Cliente,
            COALESCE(NULLIF(c.direccionCliente,''), 'CIUDAD') Direccion, c.diasCreditoCliente DiasCredito
            FROM tblPedidoEncabezado p
            JOIN tblCliente c ON c.idCliente=p.idCliente AND c.activoCliente=1
            JOIN tblMoneda m ON m.idMoneda=p.idMoneda
            WHERE (?=0 OR p.idPedidoEncabezado=?)
              AND p.estadoPedidoEncabezado IN ('CONFIRMADO','COMPLETADO')
              AND p.fechaPedidoEncabezado <= CURRENT_DATE
              AND NOT EXISTS (SELECT 1 FROM tblFacturaEncabezado f
                  WHERE f.idPedidoEncabezado=p.idPedidoEncabezado OR f.idCotizacionEncabezado=p.idCotizacionEncabezado)
              AND EXISTS (SELECT 1 FROM tblPedidoDetalle d WHERE d.idPedidoEncabezado=p.idPedidoEncabezado)
            ORDER BY p.referenciaPedidoEncabezado";

        private const string LineasCotizacion = @"SELECT d.idProducto ProductoId, d.descripcionItemCotizacionDetalle Producto,
            d.cantidadCotizacionDetalle Cantidad, d.precioUnitarioCotizacionDetalle Precio,
            d.descuentoMontoCotizacionDetalle Descuento, d.tasaImpuestoCotizacionDetalle Tasa
            FROM tblCotizacionDetalle d WHERE d.idCotizacionEncabezado=?
            ORDER BY d.numeroLineaCotizacionDetalle";

        private const string LineasPedido = @"SELECT d.idProducto ProductoId,
            COALESCE(c.descripcionItemCotizacionDetalle,p.nombreProducto) Producto,
            d.cantidadPedidoDetalle Cantidad, d.precioUnitarioPedidoDetalle Precio,
            d.descuentoMontoPedidoDetalle Descuento, COALESCE(c.tasaImpuestoCotizacionDetalle,pv.tasaImpuestoProductoVenta,0) Tasa
            FROM tblPedidoDetalle d JOIN tblPedidoEncabezado h ON h.idPedidoEncabezado=d.idPedidoEncabezado
            JOIN tblProducto p ON p.idProducto=d.idProducto
            LEFT JOIN tblProductoVenta pv ON pv.idProductoVenta=d.idProducto
            LEFT JOIN tblCotizacionDetalle c ON c.idCotizacionEncabezado=h.idCotizacionEncabezado
                AND c.numeroLineaCotizacionDetalle=d.OrdenPedido AND c.idProducto=d.idProducto
            WHERE d.idPedidoEncabezado=? ORDER BY d.OrdenPedido";

        public DataTable VentasFuncOrigenes(bool Pedido)
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncTabla(Pedido ? Pedidos : Cotizaciones, 0, 0);
        }

        public DataTable VentasFuncSucursales()
        {
            using (var Datos = new ClsDatosVentas())
                return Datos.VentasFuncCatalogo("Establecimientos");
        }

        public DataSet VentasFuncCargar(bool Pedido, int Id)
        {
            using (var Datos = new ClsDatosVentas())
                return VentasFuncDocumento(Datos, Pedido, Id);
        }

        private static DataSet VentasFuncDocumento(ClsDatosVentas Datos, bool Pedido, int Id)
        {
            if (Id <= 0) throw new ArgumentException("Seleccione un documento de origen.");
            DataTable Encabezado = Datos.VentasFuncTabla(Pedido ? Pedidos : Cotizaciones, Id, Id);
            if (Encabezado.Rows.Count != 1)
                throw new InvalidOperationException("El documento ya fue utilizado o dejó de estar disponible.");

            DataTable Lineas = Datos.VentasFuncTabla(Pedido ? LineasPedido : LineasCotizacion, Id);
            if (Lineas.Rows.Count == 0) throw new InvalidOperationException("El documento no tiene productos.");
            Lineas.Columns.Add("Subtotal", typeof(decimal));
            Lineas.Columns.Add("Impuesto", typeof(decimal));
            Lineas.Columns.Add("Total", typeof(decimal));
            foreach (DataRow Linea in Lineas.Rows)
            {
                decimal Neto = (decimal)Linea["Cantidad"] * (decimal)Linea["Precio"] - (decimal)Linea["Descuento"];
                decimal Iva = VentasFuncRedondear(Neto * (decimal)Linea["Tasa"]);
                Linea["Subtotal"] = VentasFuncRedondear(Neto);
                Linea["Impuesto"] = Iva;
                Linea["Total"] = VentasFuncRedondear(Neto + Iva);
            }
            var Documento = new DataSet();
            Documento.Tables.Add(Encabezado);
            Documento.Tables.Add(Lineas);
            return Documento;
        }

        private static decimal VentasFuncRedondear(decimal Monto)
        {
            return Math.Round(Monto, 4, MidpointRounding.AwayFromZero);
        }

        public int VentasFuncRegistrar(bool Pedido, int Id, int Sucursal, int Usuario, string Condicion, out DataSet Documento)
        {
            using (var Datos = new ClsDatosVentas())
            {
                Datos.VentasMetIniciar();
                if (Convert.ToInt32(Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblUsuario WHERE idUsuario=? AND is_active=1", Usuario)) != 1)
                    throw new InvalidOperationException("Inicie sesión con un usuario activo para registrar la factura.");
                if (Convert.ToInt32(Datos.VentasFuncEscalar("SELECT COUNT(*) FROM tblSucursal WHERE idSucursal=?", Sucursal)) != 1)
                    throw new ArgumentException("Seleccione una sucursal emisora.");

                DateTime Fecha = DateTime.Now;
                DataTable Encabezado = Datos.VentasFuncTabla(Pedido ? Pedidos : Cotizaciones, Id, Id);
                if (Encabezado.Rows.Count != 1) throw new InvalidOperationException("El documento dejó de estar disponible.");
                DataRow Origen = Encabezado.Rows[0];
                object Moneda = Origen["MonedaId"];
                Datos.VentasFuncEjecutar(@"INSERT INTO tblControlPeriodoCxC(idMoneda) VALUES (?)
                    ON DUPLICATE KEY UPDATE idMoneda=VALUES(idMoneda)", Moneda);
                object Cierre = Datos.VentasFuncEscalar(@"SELECT fechaUltimoCierreControlPeriodoCxC FROM tblControlPeriodoCxC
                    WHERE idMoneda=? FOR UPDATE", Moneda);
                if (Cierre != DBNull.Value && Cierre != null && Fecha.Date <= Convert.ToDateTime(Cierre).Date)
                    throw new InvalidOperationException("La fecha de la factura está en un período cerrado.");
                if (Origen["CotizacionId"] != DBNull.Value)
                    Datos.VentasFuncEscalar("SELECT idCotizacionEncabezado FROM tblCotizacionEncabezado WHERE idCotizacionEncabezado=? FOR UPDATE", Origen["CotizacionId"]);
                if (Pedido)
                    Datos.VentasFuncEscalar("SELECT idPedidoEncabezado FROM tblPedidoEncabezado WHERE idPedidoEncabezado=? FOR UPDATE", Id);

                Documento = VentasFuncDocumento(Datos, Pedido, Id);
                Origen = Documento.Tables[0].Rows[0];
                object Cliente = Origen["ClienteId"];
                Datos.VentasFuncEscalar("SELECT idCliente FROM tblCliente WHERE idCliente=? FOR UPDATE", Cliente);
                Datos.VentasFuncEjecutar(@"INSERT INTO tblClienteFacturaVersion
                    (idCliente,numeroVersionClienteFacturaVersion,tipoIdentificacionClienteFacturaVersion,
                     nitClienteFacturaVersion,dpiClienteFacturaVersion,nombreClienteFacturaVersion,direccionClienteFacturaVersion)
                    SELECT c.idCliente, COALESCE((SELECT MAX(v.numeroVersionClienteFacturaVersion)+1
                    FROM tblClienteFacturaVersion v WHERE v.idCliente=c.idCliente),1),
                    c.tipoIdentificacionCliente,c.nitCliente,c.dpiCliente,c.nombreCliente,c.direccionCliente
                    FROM tblCliente c WHERE c.idCliente=?", Cliente);
                int Version = Datos.VentasFuncObtenerId();

                DataTable Lineas = Documento.Tables[1];
                decimal Subtotal = 0, Iva = 0, Total = 0;
                foreach (DataRow Linea in Lineas.Rows)
                {
                    Subtotal += (decimal)Linea["Subtotal"];
                    Iva += (decimal)Linea["Impuesto"];
                    Total += (decimal)Linea["Total"];
                }
                string Uuid = Guid.NewGuid().ToString();
                DateTime Vencimiento = Fecha.Date.AddDays(Condicion == "CREDITO" ? Convert.ToInt32(Origen["DiasCredito"]) : 0);
                string Numero = Datos.VentasFuncNumero("FVE");
                Datos.VentasFuncEjecutar(@"INSERT INTO tblFacturaEncabezado
                    (uuidDteFacturaEncabezado,serieFacturaEncabezado,numeroFacturaEncabezado,idSucursal,
                     idClienteFacturaVersion,idVendedor,idMoneda,idCotizacionEncabezado,idPedidoEncabezado,
                     fechaEmisionFacturaEncabezado,fechaVencimientoFacturaEncabezado,tipoDteFacturaEncabezado,
                     condicionPagoFacturaEncabezado,estadoFacturaEncabezado,subtotalFacturaEncabezado,
                     impuestoIvaFacturaEncabezado,totalFacturaEncabezado,idUsuarioRegistro)
                    VALUES (?,'VENTAS',?,?,?,?,?,?,?,?,?,'INTERNO_VENTAS',?,'EMITIDA',?,?,?,?)",
                    Uuid, Numero, Sucursal, Version, Origen["VendedorId"], Moneda, Origen["CotizacionId"],
                    Origen["PedidoId"], Fecha, Vencimiento, Condicion, Subtotal, Iva, Total, Usuario);
                int Factura = Datos.VentasFuncObtenerId();
                int Orden = 0;
                foreach (DataRow Linea in Lineas.Rows)
                    Datos.VentasFuncEjecutar(@"INSERT INTO tblFacturaDetalle
                        (idFacturaEncabezadoUuidDte,numeroLineaFacturaDetalle,idProducto,cantidadFacturaDetalle,
                         precioUnitarioFacturaDetalle,descuentoMontoFacturaDetalle,tasaImpuestoFacturaDetalle)
                        VALUES (?,?,?,?,?,?,?)", Uuid, ++Orden, Linea["ProductoId"], Linea["Cantidad"],
                        Linea["Precio"], Linea["Descuento"], Linea["Tasa"]);
                if (Total > 0)
                    Datos.VentasFuncEjecutar(@"INSERT INTO tblMovimientoCxC
                        (idFacturaEncabezado,fechaMovimientoCxC,tipoMovimientoCxC,descripcionMovimientoCxC,
                         cargoMovimientoCxC,idUsuarioRegistro)
                        VALUES (?,?,'FACTURA',?,?,?)", Factura, Fecha.Date, "Factura " + Numero, Total, Usuario);
                if (Origen["CotizacionId"] != DBNull.Value)
                    Datos.VentasFuncEjecutar("UPDATE tblCotizacionEncabezado SET estadoCotizacionEncabezado='ACEPTADA' WHERE idCotizacionEncabezado=?", Origen["CotizacionId"]);
                Datos.VentasMetAuditar("FACTURA", Factura, "GUARDAR", Numero, Usuario);
                Datos.VentasMetConfirmar();
                return Factura;
            }
        }
    }
}
