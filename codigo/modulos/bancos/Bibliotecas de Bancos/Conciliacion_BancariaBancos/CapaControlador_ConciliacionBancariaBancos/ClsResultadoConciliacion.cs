/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Entrega a la ventana los saldos, totales y la tabla de partidas
 *  que resultan de una conciliación.
 * ================================================================
 */
using System.Data;

namespace CapaControlador_ConciliacionBancaria
{
    public class ClsResultadoConciliacion
    {
        public long IdConciliacion { get; set; }
        public string Estado { get; set; }
        public string EstadoTexto { get; set; }
        public decimal SaldoInicialBanco { get; set; }
        public decimal SaldoBanco { get; set; }
        public decimal SaldoLibro { get; set; }
        public decimal Diferencia { get; set; }
        public decimal CargosConciliados { get; set; }
        public decimal AbonosConciliados { get; set; }
        public decimal CargosPorConciliar { get; set; }
        public decimal AbonosPorConciliar { get; set; }
        public int Coincidencias { get; set; }
        public int Diferencias { get; set; }
        public int Pendientes { get; set; }
        public DataTable Tabla { get; set; }
    }
}
