namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsComisionFacturaDetalle
    {
        public string NombreProducto { get; set; }
        public string ElementoComision { get; set; }
        public decimal Cantidad { get; set; }
        public decimal BaseVenta { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal ComisionTotal { get; set; }
        public decimal VentaCobrado { get; set; }
        public decimal ComisionUltimoAbono { get; set; }
        public decimal ComisionDisponible { get; set; }
        public decimal ComisionPendiente { get; set; }
    }
}
