namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsComisionVendedorResultado
    {
        public int IdVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public string ElementoComision { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal BaseVentas { get; set; }
        public decimal MontoComision { get; set; }
    }
}
