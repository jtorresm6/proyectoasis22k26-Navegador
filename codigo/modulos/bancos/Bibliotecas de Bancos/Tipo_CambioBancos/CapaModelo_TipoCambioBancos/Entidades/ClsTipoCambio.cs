using System;

namespace CapaModelo_TipoCambio.Entidades
{
    public class ClsTipoCambio
    {
        public DateTime FechaTasa { get; set; }
        public string CodigoMoneda { get; set; }
        public string NombreMoneda { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public decimal Tasa { get; set; }
        public string TasaTexto { get; set; }
        public string Fuente { get; set; }
        public string Estado { get; set; }
        public string Metodo { get; set; }
    }
}
