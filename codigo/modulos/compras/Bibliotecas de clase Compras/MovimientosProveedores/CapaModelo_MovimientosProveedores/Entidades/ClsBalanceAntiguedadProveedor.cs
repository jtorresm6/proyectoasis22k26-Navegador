using System;

namespace CapaModelo_MovimientosProveedores.Entidades
{
    /// <summary>Una fila del reporte RDLC por proveedor y moneda.</summary>
    public sealed class ClsBalanceAntiguedadProveedor
    {
        public DateTime FechaCorte { get; set; }
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdMoneda { get; set; }
        public string Moneda { get; set; }
        public decimal Vigente { get; set; }
        public decimal De1A30 { get; set; }
        public decimal De31A60 { get; set; }
        public decimal De61A90 { get; set; }
        public decimal MasDe90 { get; set; }
        public decimal Total { get; set; }
    }
}
