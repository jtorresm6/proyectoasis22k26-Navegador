using System;

namespace CapaModelo_DisponibilidadDiariaBancos.Entidades
{
    public class ClsConflictoDisponibilidadException : InvalidOperationException
    {
        public ClsConflictoDisponibilidadException(string mensaje) : base(mensaje) { }
    }

    public class ClsDisponibilidad
    {
        public long CuentaId { get; set; }
        public long BancoId { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoCuenta { get; set; }  // "Monetaria", "Ahorro", etc.
        public string Moneda { get; set; }      // "GTQ", "USD", "EUR"
        public DateTime FechaCorte { get; set; }

        // Importes en la moneda declarada
        public decimal SaldoInicial { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal Comprometido { get; set; }
        public decimal Restringido { get; set; }
        public decimal? TipoCambioAplicado { get; set; }
        public decimal? DisponibleGtq { get; set; }
        public string Fuente { get; set; }
        public DateTime FechaGeneracion { get; set; }
        // Version abierta por el usuario; cambia en cada guardado, incluso en el mismo segundo.
        public long VersionRegistro { get; set; }
        public int? UsuarioRegistroId { get; set; }
        public string UsuarioRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }

      
        // Disponible = SaldoInicial + Ingresos − Egresos − Comprometido − Restringido.
        // Los comprometidos NO están incluidos en los egresos; son fondos separados.
      
        public decimal Disponible =>
            SaldoInicial + Ingresos - Egresos - Comprometido - Restringido;
    }
}
