/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Representa un movimiento bancario registrado en el libro que
 *  participa en la conciliación de una cuenta.
 * ================================================================
 */
using System;

namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public class ClsMovimientoLibro
    {
        public long IdMovimiento { get; set; }
        public string Referencia { get; set; }
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
        public int Sentido { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime FechaValor { get; set; }
    }
}
