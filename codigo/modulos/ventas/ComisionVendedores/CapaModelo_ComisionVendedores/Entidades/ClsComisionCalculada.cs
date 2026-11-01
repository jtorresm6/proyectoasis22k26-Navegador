using System;

namespace CapaModelo_ComisionVendedores.Entidades
{
    public class ClsComisionCalculada
    {
        public int IdVendedor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Metodo { get; set; }
        public string BaseCalculo { get; set; }
        public decimal Monto { get; set; }
    }
}
