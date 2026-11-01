namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsDetalleProductoComision
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
