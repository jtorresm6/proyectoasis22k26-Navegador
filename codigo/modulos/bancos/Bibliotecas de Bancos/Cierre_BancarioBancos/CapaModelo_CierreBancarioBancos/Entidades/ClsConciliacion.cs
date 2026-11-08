using System;

namespace CapaModelo_CierreBancario.Entidades
{
    public class ClsConciliacion
    {
        public long   PkConciliacion       { get; set; }
        public DateTime FechaCorte         { get; set; }
        public string EstadoConciliacion   { get; set; }
        public string MetodoConciliacion   { get; set; }
        public decimal SaldoLibro          { get; set; }
        public decimal SaldoBanco          { get; set; }
        public decimal Diferencia          { get; set; }
        public string Responsable          { get; set; }
        public DateTime? FechaCierre       { get; set; }
        public string NombreCuenta         { get; set; }
        public string NumeroCuenta         { get; set; }
    }
}
