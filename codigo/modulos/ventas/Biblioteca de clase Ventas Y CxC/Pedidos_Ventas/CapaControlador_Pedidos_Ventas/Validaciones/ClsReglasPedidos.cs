/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Definir las reglas de negocio de pedidos.
 */
using System;
using System.Data;

namespace CapaControlador_Pedidos_Ventas
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Valida fechas, estados, cantidades y descuentos antes de guardar.
    public static class ClsReglasPedidos
    {
        public static string VentasFuncErrorCliente(int Cliente) =>
            Cliente <= 0 ? "Seleccione un cliente." : "";

        public static string VentasFuncErrorVendedor(int Vendedor) =>
            Vendedor <= 0 ? "Seleccione un vendedor." : "";

        public static string VentasFuncErrorOrigen(bool PorCotizacion, int Cotizacion, int Moneda)
        {
            if (PorCotizacion) return Cotizacion <= 0 ? "Seleccione una cotización." : "";
            return Moneda <= 0 ? "Seleccione una lista de precios." : "";
        }

        public static string VentasFuncErrorFecha(DateTime Fecha, DateTime Referencia) =>
            Fecha.Date < Referencia.Date ? "No puede ser anterior a la fecha inicial." : "";

        public static string VentasFuncErrorDetalle(int Productos) =>
            Productos == 0 ? "Agregue al menos un producto al pedido." : "";

        public static string VentasFuncErrorProducto(bool Seleccionado) =>
            Seleccionado ? "" : "Seleccione un producto.";

        public static string VentasFuncErrorCantidad(decimal Cantidad) =>
            Cantidad <= 0 || Cantidad != decimal.Truncate(Cantidad) ? "Entero positivo." : "";

        public static string VentasFuncErrorDescuento(decimal Cantidad, decimal? Precio, decimal Descuento) =>
            Precio.HasValue && Descuento > Cantidad * Precio.Value ? "Supera el importe." : "";

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
            string NuevoEstado = Convert.ToString(Pedido["NuevoEstado"]);
            if (Id > 0 && Convert.ToString(Pedido["Estado"]) != "PENDIENTE" &&
                Convert.ToString(Pedido["Estado"]) != "CONFIRMADO")
                throw new InvalidOperationException("El pedido no admite edición.");
            if (NuevoEstado != "PENDIENTE" && NuevoEstado != "CONFIRMADO" &&
                NuevoEstado != "COMPLETADO" && NuevoEstado != "ANULADO")
                throw new ArgumentException("Seleccione un estado válido para el pedido.");
            if (Id == 0 && NuevoEstado != "PENDIENTE")
                throw new ArgumentException("Un pedido nuevo debe iniciar pendiente.");
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
                if (!VentasFuncDecimal(Cantidad) || Cantidad == 0 || Cantidad != decimal.Truncate(Cantidad) ||
                    !VentasFuncDecimal(Precio) || !VentasFuncDecimal(Descuento) ||
                    Descuento > Cantidad * Precio)
                    throw new ArgumentException("La cantidad debe ser un entero positivo; revise precio y descuento.");
                Linea["Total"] = ClsPedidoControlador.VentasFuncTotal(Cantidad, Precio, Descuento);
            }
        }
    }
}
