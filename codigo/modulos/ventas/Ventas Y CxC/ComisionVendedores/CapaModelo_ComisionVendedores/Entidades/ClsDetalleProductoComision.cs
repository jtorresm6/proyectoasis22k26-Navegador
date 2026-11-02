namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsDetalleProductoComision
    {
        public int IdFacturaDetalle { get; set; }
        public int IdProducto { get; set; }
        public int IdProductoLogistica { get; set; }
        public int IdClasificacionLogistica { get; set; }
        public decimal MontoComisionLineaGuardado { get; set; }
        public string MetodoGuardado { get; set; }
        public string MetodoSeleccionado { get; set; }
        public int IdVendedorGuardado { get; set; }
        public bool ComisionGuardada { get; set; }
        public decimal MontoComisionTotalGuardado { get; set; }
        public decimal ComisionDevengada { get; set; }
        public decimal UltimaComisionDevengada { get; set; }
        public string NombreProducto { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string ElementoComision { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal BaseComisionable { get; set; }
        public int IdReglaComision { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeProducto { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
    }
}
