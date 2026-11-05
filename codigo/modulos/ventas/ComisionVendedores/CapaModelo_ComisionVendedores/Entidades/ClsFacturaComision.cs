using System;

namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsFacturaComision
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal UltimoAbono { get; set; }
    }
}
