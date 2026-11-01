using System;

namespace CapaModelo_TipoCambio.Entidades
{
    public class ClsTipoCambio
    {
        public DateTime FechaTasa { get; set; }
        public string CodigoMoneda { get; set; }     // "USD", "EUR", "GTQ", etc.
        public string NombreMoneda { get; set; }     // "Dólar estadounidense", "Euro", etc.
        public string MonedaOrigen { get; set; }     // "USD", "EUR"
        public string MonedaDestino { get; set; }    // "GTQ"
        public decimal Tasa { get; set; }            // tasa numérica
        public string TasaTexto { get; set; }        // texto formateado o "—"
        public string Fuente { get; set; }           // "Demostración"
        public string Estado { get; set; }           // "Disponible" o "Sin tasa"
    }
}
