namespace CapaControlador_ListadoPrecios
{
    /// <summary>Datos transferidos del controlador a la vista del Listado de Precios.</summary>
    public class ClsPrecioCliente
    {
        public string TipoCliente { get; set; }
        public string TipoProducto { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}
