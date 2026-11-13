/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Agrupa los totales de cargos, abonos y cantidades de partidas
 *  calculados sobre el resultado de una conciliación.
 * ================================================================
 */
namespace CapaControlador_ConciliacionBancaria
{
    public class ClsTotalesConciliacion
    {
        public decimal CargosConciliados { get; set; }
        public decimal AbonosConciliados { get; set; }
        public decimal CargosPorConciliar { get; set; }
        public decimal AbonosPorConciliar { get; set; }
        public int Coincidencias { get; set; }
        public int Diferencias { get; set; }
        public int Pendientes { get; set; }
    }
}
