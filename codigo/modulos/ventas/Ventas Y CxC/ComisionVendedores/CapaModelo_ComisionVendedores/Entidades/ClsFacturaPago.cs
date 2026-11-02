using System;

namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsFacturaPago
    {
        public int IdFactura { get; set; }
        public int IdCliente { get; set; }
        public long IdMoneda { get; set; }
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
