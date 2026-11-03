using System;
using System.Data;
using CapaModelo_Pedidos;

namespace CapaControlador_Pedidos
{
    public sealed class ClsPedidoControlador
    {
        private readonly IPedidoRepositorio _Repositorio;

        public ClsPedidoControlador() : this(new ClsPedidoRepositorio()) { }
        public ClsPedidoControlador(IPedidoRepositorio Repositorio)
        {
            _Repositorio = Repositorio ?? throw new ArgumentNullException(nameof(Repositorio));
        }

        public DataTable VentasFuncCatalogo(string Nombre) { return _Repositorio.VentasFuncCatalogo(Nombre); }
        public DataTable VentasFuncListas(int Cliente, DateTime Fecha) { return _Repositorio.VentasFuncListas(Cliente, Fecha); }
        public DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha)
        {
            return Cliente > 0 ? _Repositorio.VentasFuncCotizaciones(Cliente, Fecha) : new DataTable();
        }
        public DataTable VentasFuncProductos(int Lista) { return _Repositorio.VentasFuncProductos(Lista); }
        public string VentasFuncMoneda(int Id) { return _Repositorio.VentasFuncMoneda(Id); }
        public DataTable VentasFuncListar() { return _Repositorio.VentasFuncListar(); }
        public DataTable VentasFuncPedido(int Id) { return _Repositorio.VentasFuncPedido(Id); }

        public static DataTable VentasFuncLineas()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("ProductoId", typeof(int));
            Tabla.Columns.Add("Codigo", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("Cantidad", typeof(decimal));
            Tabla.Columns.Add("Precio", typeof(decimal));
            Tabla.Columns.Add("Descuento", typeof(decimal));
            Tabla.Columns.Add("Total", typeof(decimal));
            return Tabla;
        }

        public static decimal VentasFuncTotal(decimal Cantidad, decimal Precio, decimal Descuento)
        {
            return Math.Round(Cantidad * Precio - Descuento, 4, MidpointRounding.AwayFromZero);
        }

        public static void VentasMetLinea(DataTable Lineas, DataRow Producto)
        {
            decimal Cantidad = Convert.ToDecimal(Producto["Cantidad"]);
            decimal Precio = Convert.ToDecimal(Producto["Precio"]);
            decimal Descuento = Convert.ToDecimal(Producto["Descuento"]);
            Lineas.Rows.Add(Producto["ProductoId"], Producto["Codigo"], Producto["Descripcion"],
                Cantidad, Precio, Descuento, VentasFuncTotal(Cantidad, Precio, Descuento));
        }

        public int VentasFuncGuardar(DataRow Pedido, DataTable Lineas)
        {
            ClsReglasPedidos.VentasMetValidar(Pedido, Lineas);
            int Id = Convert.ToInt32(Pedido["Id"]);
            if (Id == 0 && Pedido["CotizacionId"] != DBNull.Value &&
                !_Repositorio.VentasFuncCotVigente(Convert.ToInt32(Pedido["CotizacionId"]),
                    Convert.ToInt32(Pedido["ClienteId"]), Convert.ToDateTime(Pedido["Fecha"])))
                throw new ArgumentException("La cotización no está vigente o no pertenece al cliente.");
            return _Repositorio.VentasFuncGuardar(Pedido, Lineas);
        }

        public void VentasMetEstado(int Id, string Estado)
        {
            if (Id <= 0 || (Estado != "CONFIRMADO" && Estado != "ANULADO"))
                throw new ArgumentException("Estado de pedido inválido.");
            _Repositorio.VentasMetEstado(Id, Estado);
        }
    }
}
