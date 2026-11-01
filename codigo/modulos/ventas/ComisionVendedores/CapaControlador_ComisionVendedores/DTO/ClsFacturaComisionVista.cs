using System;

namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsFacturaComisionVista
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal UltimoAbono { get; set; }
        public string Etiqueta { get { return "FAC-" + IdFactura + "  |  " + Fecha.ToString("dd/MM/yyyy") + "  |  " + Cliente + "  |  Q " + Total.ToString("N2"); } }
    }
}
