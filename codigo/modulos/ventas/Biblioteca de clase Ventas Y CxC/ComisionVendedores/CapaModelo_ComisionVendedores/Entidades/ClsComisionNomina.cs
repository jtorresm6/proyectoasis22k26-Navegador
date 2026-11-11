using System;

namespace CapaModelo_ComisionVendedores.Entidades
{
    /// <summary>Un abono de comisión generado por un pago individual del cliente.</summary>
    public class ClsComisionNomina
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
