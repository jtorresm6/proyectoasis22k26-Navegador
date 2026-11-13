/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Representa el encabezado de una conciliación bancaria junto con
 *  los datos de la cuenta y del extracto que se concilian.
 * ================================================================
 */
using System;

namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public class ClsConciliacion
    {
        public long IdConciliacion { get; set; }
        public long IdCuenta { get; set; }
        public long IdExtracto { get; set; }
        public DateTime FechaCorte { get; set; }
        public string Estado { get; set; }
        public string Metodo { get; set; }
        public decimal SaldoLibro { get; set; }
        public decimal SaldoBanco { get; set; }
        public decimal Diferencia { get; set; }
        public string Responsable { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public string Moneda { get; set; }
        public string ReferenciaExtracto { get; set; }
    }
}
