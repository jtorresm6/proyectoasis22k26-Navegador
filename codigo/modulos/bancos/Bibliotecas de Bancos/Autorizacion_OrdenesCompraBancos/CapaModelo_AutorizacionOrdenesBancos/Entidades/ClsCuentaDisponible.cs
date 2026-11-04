using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La entidad ClsCuentaDisponible representa una cuenta bancaria con su saldo
 *  disponible del día, su moneda y el tipo de cambio aplicado.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Entidades
{
    public class ClsCuentaDisponible
    {
        public long IdCuenta { get; set; }
        public string NombreBanco { get; set; }
        public string NumeroCuenta { get; set; }
        public long IdMoneda { get; set; }
        public string CodigoMoneda { get; set; }
        public decimal SaldoDisponible { get; set; }
        public long? IdTipoCambio { get; set; }
        public decimal TipoCambio { get; set; }
    }
}
