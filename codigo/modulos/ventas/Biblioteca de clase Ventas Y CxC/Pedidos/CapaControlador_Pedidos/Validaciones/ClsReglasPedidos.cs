using System;
using System.Data;

namespace CapaControlador_Pedidos
{
    internal static class ClsReglasPedidos
    {
        // Los importes de tblPedidoDetalle son DECIMAL(12,4).
        private static bool VentasFuncDecimal(decimal Valor)
        {
            return Valor >= 0 && Valor <= 99999999.9999m &&
                Valor * 10000m == decimal.Truncate(Valor * 10000m);
        }

        internal static void VentasMetValidar(DataRow Pedido, DataTable Lineas)
        {
            if (Pedido == null) throw new ArgumentException("Faltan los datos del pedido.");
            if (Convert.ToInt32(Pedido["ClienteId"]) <= 0)
                throw new ArgumentException("Seleccione un cliente.");
            if (Convert.ToInt32(Pedido["VendedorId"]) <= 0)
                throw new ArgumentException("Seleccione un vendedor.");
            if (Convert.ToInt32(Pedido["MonedaId"]) <= 0)
                throw new ArgumentException("Seleccione una lista de precios o cotización con moneda.");

            int Id = Convert.ToInt32(Pedido["Id"]);
            if (Id > 0 && Convert.ToString(Pedido["Estado"]) != "PENDIENTE" &&
                Convert.ToString(Pedido["Estado"]) != "CONFIRMADO")
                throw new InvalidOperationException("El pedido no admite edición.");
            if (Id > 0 && Convert.ToString(Pedido["Numero"]).Length > 30)
                throw new ArgumentException("El número de pedido excede 30 caracteres.");

            DateTime Fecha = Convert.ToDateTime(Pedido["Fecha"]);
            if (Convert.ToDateTime(Pedido["Entrega"]).Date < Fecha.Date ||
                Convert.ToDateTime(Pedido["Vencimiento"]).Date < Fecha.Date)
                throw new ArgumentException("La entrega y el vencimiento deben ser posteriores a la fecha del pedido.");
            if (Lineas == null || Lineas.Rows.Count == 0)
                throw new ArgumentException("Agregue al menos un producto.");

            foreach (DataRow Linea in Lineas.Rows)
            {
                string Descripcion = Convert.ToString(Linea["Descripcion"]);
                if (Convert.ToInt32(Linea["ProductoId"]) <= 0)
                    throw new ArgumentException("Seleccione un producto válido.");
                if (string.IsNullOrWhiteSpace(Descripcion))
                    throw new ArgumentException("Seleccione un producto con descripción.");

                decimal Cantidad = Convert.ToDecimal(Linea["Cantidad"]);
                decimal Precio = Convert.ToDecimal(Linea["Precio"]);
                decimal Descuento = Convert.ToDecimal(Linea["Descuento"]);
                if (!VentasFuncDecimal(Cantidad) || Cantidad == 0 ||
                    !VentasFuncDecimal(Precio) || !VentasFuncDecimal(Descuento) ||
                    Descuento > Cantidad * Precio)
                    throw new ArgumentException("Revise cantidad, precio y descuento (DECIMAL(12,4) en la BD).");
                Linea["Total"] = ClsPedidoControlador.VentasFuncTotal(Cantidad, Precio, Descuento);
            }
        }
    }
}
