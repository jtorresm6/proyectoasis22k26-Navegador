namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsVentaVendedorBase
    {
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public string ElementoComision { get; set; }
        public decimal PorcentajeVendedor { get; set; }
        public decimal BaseVentas { get; set; }
    }
}
