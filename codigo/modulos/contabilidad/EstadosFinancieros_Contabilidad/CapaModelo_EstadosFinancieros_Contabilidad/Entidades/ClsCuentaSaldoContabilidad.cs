/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Cuenta del catálogo con su cuenta padre y sus saldos de un rango de
 *  períodos, base de los estados financieros.
 * ===================================================================
*/

namespace CapaModelo_Contabilidad.Entidades
{
    public class ClsCuentaSaldoContabilidad
    {
        public int IdCuenta { get; set; }
        public int? IdCuentaPadre { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Clasificacion { get; set; }
        public decimal SaldoFinal { get; set; }
        public decimal Cargo { get; set; }
        public decimal Abono { get; set; }
    }
}
