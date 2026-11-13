/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Consultar y guardar pedidos en la base de datos.
 */
using System;
using System.Data;
using System.Globalization;

namespace CapaModelo_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Guarda pedidos y cambia su estado en la base de datos.
    public partial class ClsPedidoRepositorio
    {
        public virtual int VentasFuncGuardar(DataRow Pedido, DataTable Lineas)
        {
            using (var Datos = new ClsConexion())
            {
                Datos.VentasMetIniciar();
                bool DesdeCotizacion = Convert.ToInt32(Pedido["Id"]) == 0 && Pedido["CotizacionId"] != DBNull.Value;
                if (DesdeCotizacion)
                    VentasMetBloquearCotizacion(Datos, Pedido);
                int Id = VentasFuncEncabezado(Datos, Pedido);
                if (DesdeCotizacion)
                {
                    int Copiadas = Datos.VentasFuncEjecutar(@"INSERT INTO tblPedidoDetalle
                        (idPedidoEncabezado, OrdenPedido, idProducto, cantidadPedidoDetalle,
                         precioUnitarioPedidoDetalle, descuentoMontoPedidoDetalle)
                        SELECT ?, numeroLineaCotizacionDetalle, idProducto, cantidadCotizacionDetalle,
                               precioUnitarioCotizacionDetalle, descuentoMontoCotizacionDetalle
                        FROM tblCotizacionDetalle WHERE idCotizacionEncabezado = ?",
                        Id, Pedido["CotizacionId"]);
                    if (Copiadas == 0) throw new InvalidOperationException("La cotización no tiene productos.");
                }
                else
                    VentasMetLineas(Datos, Id, Lineas);
                Datos.VentasMetConfirmar();
                return Id;
            }
        }

        private static void VentasMetBloquearCotizacion(ClsConexion Datos, DataRow Pedido)
        {
            object Cotizacion = Pedido["CotizacionId"];
            if (Datos.VentasFuncEscalar(@"SELECT idCotizacionEncabezado FROM tblCotizacionEncabezado
                WHERE idCotizacionEncabezado = ? AND idCliente = ? AND idVendedor = ? AND idMoneda = ?
                  AND estadoCotizacionEncabezado IN ('EMITIDA','ACEPTADA')
                  AND fechaEmisionCotizacionEncabezado <= ? AND vigenteHastaCotizacionEncabezado >= ?
                FOR UPDATE", Cotizacion, Pedido["ClienteId"], Pedido["VendedorId"], Pedido["MonedaId"],
                Pedido["Fecha"], Pedido["Fecha"]) == null)
                throw new InvalidOperationException("La cotización dejó de estar disponible para este pedido.");

            if (Convert.ToInt32(Datos.VentasFuncEscalar(@"SELECT
                EXISTS(SELECT 1 FROM tblPedidoEncabezado WHERE idCotizacionEncabezado = ?)
                OR EXISTS(SELECT 1 FROM tblFacturaEncabezado WHERE idCotizacionEncabezado = ?)",
                Cotizacion, Cotizacion)) != 0)
                throw new InvalidOperationException("La cotización ya se usó en un pedido o factura.");
        }

        private static int VentasFuncEncabezado(ClsConexion Datos, DataRow Pedido)
        {
            int Id = Convert.ToInt32(Pedido["Id"]);
            if (Id > 0)
            {
                string EstadoActual = Convert.ToString(Datos.VentasFuncEscalar(@"SELECT estadoPedidoEncabezado
                    FROM tblPedidoEncabezado WHERE idPedidoEncabezado = ? FOR UPDATE", Id));
                if (EstadoActual != Convert.ToString(Pedido["Estado"]) ||
                    (EstadoActual != "PENDIENTE" && EstadoActual != "CONFIRMADO"))
                    throw new InvalidOperationException("El pedido ya no admite cambios.");
                Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado
                    SET estadoPedidoEncabezado = ?, idVendedor = ?, fechaPedidoEncabezado = ?,
                        fechaEntregaPedidoEncabezado = ?, fechaVencimientoPedidoEncabezado = ?
                    WHERE idPedidoEncabezado = ?",
                    Pedido["NuevoEstado"], Pedido["VendedorId"], Pedido["Fecha"], Pedido["Entrega"],
                    Pedido["Vencimiento"], Id);
                Datos.VentasFuncEjecutar("DELETE FROM tblPedidoDetalle WHERE idPedidoEncabezado = ?", Id);
                return Id;
            }

            Datos.VentasFuncEjecutar(@"INSERT INTO tblPedidoEncabezado
                (referenciaPedidoEncabezado, idCotizacionEncabezado,
                 idCliente, idVendedor, idMoneda,
                 fechaPedidoEncabezado, fechaEntregaPedidoEncabezado,
                 fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'PENDIENTE')",
                "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20),
                Pedido["CotizacionId"], Pedido["ClienteId"], Pedido["VendedorId"],
                Pedido["MonedaId"], Pedido["Fecha"], Pedido["Entrega"], Pedido["Vencimiento"]);
            Id = Datos.VentasFuncObtenerId();
            Pedido["Numero"] = "PED-" + Id.ToString("D8", CultureInfo.InvariantCulture);
            Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado
                SET referenciaPedidoEncabezado = ? WHERE idPedidoEncabezado = ?", Pedido["Numero"], Id);
            return Id;
        }

        private static void VentasMetLineas(ClsConexion Datos, int Id, DataTable Lineas)
        {
            int Orden = 0;
            foreach (DataRow Linea in Lineas.Rows)
                Datos.VentasFuncEjecutar(@"INSERT INTO tblPedidoDetalle
                    (idPedidoEncabezado, OrdenPedido, idProducto,
                     cantidadPedidoDetalle, precioUnitarioPedidoDetalle, descuentoMontoPedidoDetalle)
                    VALUES (?, ?, ?, ?, ?, ?)",
                    Id, ++Orden, Linea["ProductoId"],
                    Linea["Cantidad"], Linea["Precio"], Linea["Descuento"]);
        }

        public virtual void VentasMetEstado(int Id, string Estado)
        {
            using (var Datos = new ClsConexion())
                if (Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado SET estadoPedidoEncabezado=?
                    WHERE idPedidoEncabezado=? AND estadoPedidoEncabezado IN ('PENDIENTE','CONFIRMADO')", Estado, Id) != 1)
                    throw new InvalidOperationException("El pedido ya no admite cambios de estado.");
        }

    }
}
