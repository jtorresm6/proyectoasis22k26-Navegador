namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsProductoComision
    {
        public int IdProducto { get; set; }
        public int IdMarca { get; set; }
        public int IdLinea { get; set; }
        public int IdCategoria { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
        public decimal PorcentajeProducto { get; set; }
    }
}
