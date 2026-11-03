namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsComisionFacturaDetalle
    {
        public int IdFacturaDetalle { get; set; }
        public string MetodoComision { get; set; }
        public string SimboloMoneda { get; set; }
        public bool ComisionGuardada { get; set; }
        public string NombreProducto { get; set; }
        public string ElementoComision { get; set; }
        public decimal Cantidad { get; set; }
        public decimal BaseVenta { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeProducto { get; set; }
        public decimal PorcentajeMarca { get; set; }
        public decimal PorcentajeLinea { get; set; }
        public decimal PorcentajeCategoria { get; set; }
        public string EtiquetaComisionPor { get { return MetodoComision + " · " + Porcentaje.ToString("0.00") + "%"; } }
        public decimal ComisionTotal { get; set; }
        public decimal VentaCobrado { get; set; }
        public decimal ComisionUltimoAbono { get; set; }
        public decimal ComisionDisponible { get; set; }
        public decimal ComisionPendiente { get; set; }
    }
}
