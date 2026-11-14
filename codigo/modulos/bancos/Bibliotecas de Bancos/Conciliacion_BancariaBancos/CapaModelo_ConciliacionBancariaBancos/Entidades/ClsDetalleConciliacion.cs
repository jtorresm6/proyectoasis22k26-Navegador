/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Representa una fila de la conciliación: una pareja entre el libro
 *  y el banco, o una partida que sigue pendiente.
 * ================================================================
 */
using System;

namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public class ClsDetalleConciliacion
    {
        public long IdDetalle { get; set; }
        public string Resultado { get; set; }
        public string Metodo { get; set; }
        public decimal Diferencia { get; set; }
        public string Motivo { get; set; }
        public ClsMovimientoLibro Movimiento { get; set; }
        public ClsLineaExtracto Linea { get; set; }

        public bool EsParejaManual
        {
            get
            {
                return Metodo == ClsValoresConciliacion.MetodoManual && Movimiento != null && Linea != null;
            }
        }
    }
}
