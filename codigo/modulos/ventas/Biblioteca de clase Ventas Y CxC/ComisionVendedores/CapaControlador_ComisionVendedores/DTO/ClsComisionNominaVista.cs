using System;

namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsComisionNominaVista
    {
        public int IdAbono { get; set; }
        public int IdFactura { get; set; }
        public DateTime FechaAbono { get; set; }
        public string Vendedor { get; set; }
        public string SimboloMoneda { get; set; }
        public decimal TotalComision { get; set; }
        public decimal MontoPagoCliente { get; set; }
        public decimal ComisionDevengadaAbono { get; set; }
        public string EstadoPago { get; set; }
    }
}
