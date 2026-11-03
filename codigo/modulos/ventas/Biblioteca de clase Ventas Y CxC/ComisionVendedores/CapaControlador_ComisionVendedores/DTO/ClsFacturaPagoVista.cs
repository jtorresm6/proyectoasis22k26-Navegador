using System;

namespace CapaControlador_ComisionVendedores.DTO
{
    public class ClsFacturaPagoVista
    {
        public int IdFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string Etiqueta { get; set; }
        public string SimboloMoneda { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Saldo { get; set; }
        public decimal ComisionTotal { get; set; }
        public decimal ComisionDevengada { get; set; }
    }
}
