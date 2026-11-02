using System;

namespace CapaModelo_DisponibilidadDiariaBancos.Entidades
{
    public class ClsDisponibilidad
    {
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

      
        // Disponible = SaldoInicial + Ingresos − Egresos − Comprometido − Restringido.
        // Los comprometidos NO están incluidos en los egresos; son fondos separados.
      
        public decimal Disponible =>
            SaldoInicial + Ingresos - Egresos - Comprometido - Restringido;
    }
}
