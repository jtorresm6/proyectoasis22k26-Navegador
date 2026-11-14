/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Representa una línea del estado de cuenta del banco que
 *  participa en la conciliación de una cuenta.
 * ================================================================
 */
using System;

namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public class ClsLineaExtracto
    {
        public long IdLinea { get; set; }
        public string Identificador { get; set; }
        public string Referencia { get; set; }
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
        public int Sentido { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime FechaValor { get; set; }
    }
}
