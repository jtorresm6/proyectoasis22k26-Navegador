/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Entidades
{
    public class ClsCuentaBancariaTercero
    {
        public long IdCuentaBancariaTercero { get; set; }
        public string NumeroCuentaBancariaTercero { get; set; }
        public string NombreTitularCuentaBancariaTercero { get; set; }
        public string NombreBanco { get; set; }
        public string CodigoMoneda { get; set; }
        public string DescripcionCuenta
        {
            get{ return $"{NombreBanco} - {NumeroCuentaBancariaTercero} ({CodigoMoneda})"; }
        }
    }
}
