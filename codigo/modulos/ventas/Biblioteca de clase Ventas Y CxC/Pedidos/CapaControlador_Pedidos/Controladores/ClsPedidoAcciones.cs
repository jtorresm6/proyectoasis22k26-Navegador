using System;
using System.Data;

namespace CapaControlador_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Mantiene las líneas y el estado de un pedido en edición.
    public sealed class ClsPedidoAcciones
    {
        private readonly ClsPedidoControlador _Controlador;
        private DataTable _Cotizaciones = new DataTable();

        public int Id { get; }
        public int Cotizacion { get; set; }
        public int Moneda { get; set; }
        public string Estado { get; set; } = "PENDIENTE";
        public DataTable Lineas { get; } = ClsPedidoControlador.VentasFuncLineas();

        public ClsPedidoAcciones(int Id, ClsPedidoControlador Controlador)
        {
            this.Id = Id;
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
        }

        public void VentasMetLinea(DataRow Producto)
        {
            ClsPedidoControlador.VentasMetLinea(Lineas, Producto);
        }

        public DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha)
        {
            _Cotizaciones = Cliente > 0
                ? _Controlador.VentasFuncCotizaciones(Cliente, Fecha) : new DataTable();
            return _Cotizaciones.Rows.Count == 0 ? new DataTable()
                : _Cotizaciones.DefaultView.ToTable(true, "CotizacionId", "Nombre");
        }

        public int VentasFuncCotizacion(int Id)
        {
            if (_Cotizaciones.Rows.Count == 0)
                throw new ArgumentException("Seleccione una cotización vigente.");
            DataRow[] Filas = _Cotizaciones.Select("CotizacionId=" + Id);
            if (Filas.Length == 0) throw new ArgumentException("Seleccione una cotización vigente.");
            Cotizacion = Id;
            Moneda = Convert.ToInt32(Filas[0]["MonedaId"]);
            Lineas.Clear();
            foreach (DataRow Fila in Filas) VentasMetLinea(Fila);
            return Convert.ToInt32(Filas[0]["VendedorId"]);
        }

        public void VentasMetProducto(DataRow Producto, decimal Cantidad, decimal Descuento)
        {
            if (Producto == null) throw new ArgumentException("Seleccione un producto.");
            decimal Precio = Convert.ToDecimal(Producto["Precio"]);
            if (Descuento > Cantidad * Precio)
                throw new ArgumentException("El descuento no puede superar el importe del producto.");
            Lineas.Rows.Add(Producto["ProductoId"], Producto["Codigo"], Producto["Descripcion"],
                Cantidad, Precio, Descuento, ClsPedidoControlador.VentasFuncTotal(Cantidad, Precio, Descuento));
        }

        public decimal[] VentasFuncTotales()
        {
            decimal Subtotal = 0, Total = 0;
            foreach (DataRow Linea in Lineas.Rows)
            {
                Subtotal += Convert.ToDecimal(Linea["Cantidad"]) * Convert.ToDecimal(Linea["Precio"]);
                Total += Convert.ToDecimal(Linea["Total"]);
            }
            return new[] { Subtotal, Total };
        }

        public string VentasFuncGuardar(int Cliente, int Vendedor, bool PorCotizacion,
            DateTime Fecha, DateTime Entrega, DateTime Vencimiento, string Numero, string NuevoEstado = "PENDIENTE")
        {
            if (PorCotizacion && Cotizacion <= 0)
                throw new ArgumentException("Seleccione una cotización.");
            var Tabla = new DataTable();
            foreach (string Campo in new[] { "Id", "CotizacionId", "ClienteId", "VendedorId", "MonedaId" })
                Tabla.Columns.Add(Campo, typeof(int));
            foreach (string Campo in new[] { "Fecha", "Entrega", "Vencimiento" })
                Tabla.Columns.Add(Campo, typeof(DateTime));
            Tabla.Columns.Add("Estado");
            Tabla.Columns.Add("NuevoEstado");
            Tabla.Columns.Add("Numero");
            DataRow Pedido = Tabla.Rows.Add(Id, PorCotizacion ? (object)Cotizacion : DBNull.Value,
                Cliente, Vendedor, Moneda, Fecha.Date, Entrega.Date, Vencimiento.Date, Estado, NuevoEstado, Numero);
            _Controlador.VentasFuncGuardar(Pedido, Lineas);
            return Convert.ToString(Pedido["Numero"]);
        }

        public void VentasMetEstado(string Estado)
        {
            _Controlador.VentasMetEstado(Id, Estado);
        }
    }
}
