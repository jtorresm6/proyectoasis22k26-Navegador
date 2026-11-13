/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Contiene el período y los saldos del extracto bancario
 *  seleccionado para conciliar.
 * ================================================================
 */
using System;

namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public class ClsDatosExtracto
    {
        public long IdExtracto { get; set; }
        public long IdCuenta { get; set; }
        public string Referencia { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
