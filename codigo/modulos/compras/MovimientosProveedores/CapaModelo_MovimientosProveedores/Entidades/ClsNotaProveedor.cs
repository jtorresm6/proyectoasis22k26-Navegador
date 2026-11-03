using System;

namespace CapaModelo_MovimientosProveedores.Entidades
{
    public sealed class ClsNotaProveedor
    {
        public int IdProveedor { get; set; }
        public int IdConceptoProveedor { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IdMoneda { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal Monto { get; set; }

    }
}
