using System;

namespace CapaModelo_TipoCambio.Entidades
{
    public class ClsResultadoConversion
    {
        public decimal Resultado { get; set; }
        public decimal Tasa { get; set; }
        public DateTime Fecha { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public string Fuente { get; set; }
        public string Metodo { get; set; }
    }
}
