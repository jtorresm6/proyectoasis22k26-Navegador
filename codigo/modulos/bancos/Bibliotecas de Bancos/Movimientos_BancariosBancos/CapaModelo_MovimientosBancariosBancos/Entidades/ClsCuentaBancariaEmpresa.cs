/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Entidades
{
    public class ClsCuentaBancariaEmpresa
    {
        public long IdCuentaBancaria { get; set; }
        public string NumeroCuentaBancaria { get; set; }
        public string NombreCuentaBancaria { get; set; }
        public string NombreBanco { get; set; }
        public string CodigoMoneda { get; set; }
        public decimal SaldoInicialCuentaBancaria { get; set; }
        public decimal LimiteSobregiroCuentaBancaria { get; set; }
        public string DescripcionCuenta
        {
            get { return $"{NombreBanco} - {NumeroCuentaBancaria} ({CodigoMoneda})";}
        }
    }
}
