using System;
using System.Data;
using System.Globalization;

namespace CapaModelo_Pedidos
{
    public interface IPedidoRepositorio
    {
        DataTable VentasFuncCatalogo(string Nombre);
        DataTable VentasFuncListas(int Cliente, DateTime Fecha);
        DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha);
        bool VentasFuncCotVigente(int Cotizacion, int Cliente, DateTime Fecha);
        DataTable VentasFuncProductos(int Lista);
        string VentasFuncMoneda(int Id);
        DataTable VentasFuncListar();
        DataTable VentasFuncPedido(int Id);
        int VentasFuncGuardar(DataRow Pedido, DataTable Lineas);
        void VentasMetEstado(int Id, string Estado);
    }

    public sealed partial class ClsPedidoRepositorio : IPedidoRepositorio
    {
        public int VentasFuncGuardar(DataRow Pedido, DataTable Lineas)
        {
            using (var Datos = new ClsConexion())
            {
                Datos.VentasMetIniciar();
                int Id = VentasFuncEncabezado(Datos, Pedido);
                VentasMetLineas(Datos, Id, Lineas);
                Datos.VentasMetConfirmar();
                return Id;
            }
        }

        private static int VentasFuncEncabezado(ClsConexion Datos, DataRow Pedido)
        {
            int Id = Convert.ToInt32(Pedido["Id"]);
            if (Id > 0)
            {
                int Cambios = Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado
                    SET Fk_Vendedor_Codigo = ?, fechaPedidoEncabezado = ?,
                        fechaEntregaPedidoEncabezado = ?, fechaVencimientoPedidoEncabezado = ?
                    WHERE Pk_PedidoEncabezado = ? AND estadoPedidoEncabezado = ?",
                    Pedido["VendedorId"], Pedido["Fecha"], Pedido["Entrega"],
                    Pedido["Vencimiento"], Id, Pedido["Estado"]);
                if (Cambios != 1) throw new InvalidOperationException("El pedido ya no admite cambios.");
                Datos.VentasFuncEjecutar("DELETE FROM tblPedidoDetalle WHERE Fk_PedidoEncabezado_Codigo = ?", Id);
                return Id;
            }

            Datos.VentasFuncEjecutar(@"INSERT INTO tblPedidoEncabezado
                (referenciaPedidoEncabezado, Fk_CotizacionEncabezado_Codigo,
                 Fk_Cliente_Codigo, Fk_Vendedor_Codigo, Fk_Moneda_Codigo,
                 fechaPedidoEncabezado, fechaEntregaPedidoEncabezado,
                 fechaVencimientoPedidoEncabezado, estadoPedidoEncabezado)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'PENDIENTE')",
                "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20),
                Pedido["CotizacionId"], Pedido["ClienteId"], Pedido["VendedorId"],
                Pedido["MonedaId"], Pedido["Fecha"], Pedido["Entrega"], Pedido["Vencimiento"]);
            Id = Datos.VentasFuncObtenerId();
            Pedido["Numero"] = "PED-" + Id.ToString("D8", CultureInfo.InvariantCulture);
            Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado
                SET referenciaPedidoEncabezado = ? WHERE Pk_PedidoEncabezado = ?", Pedido["Numero"], Id);
            return Id;
        }

        private static void VentasMetLineas(ClsConexion Datos, int Id, DataTable Lineas)
        {
            int Orden = 0;
            foreach (DataRow Linea in Lineas.Rows)
                Datos.VentasFuncEjecutar(@"INSERT INTO tblPedidoDetalle
                    (Fk_PedidoEncabezado_Codigo, OrdenPedido, Fk_Producto_Codigo,
                     cantidadPedidoDetalle, precioUnitarioPedidoDetalle, descuentoMontoPedidoDetalle)
                    VALUES (?, ?, ?, ?, ?, ?)",
                    Id, ++Orden, Linea["ProductoId"],
                    Linea["Cantidad"], Linea["Precio"], Linea["Descuento"]);
        }

        public void VentasMetEstado(int Id, string Estado)
        {
            using (var Datos = new ClsConexion())
                if (Datos.VentasFuncEjecutar(@"UPDATE tblPedidoEncabezado SET estadoPedidoEncabezado=?
                    WHERE Pk_PedidoEncabezado=? AND estadoPedidoEncabezado IN ('PENDIENTE','CONFIRMADO')", Estado, Id) != 1)
                    throw new InvalidOperationException("El pedido ya no admite cambios de estado.");
        }
    }
}
