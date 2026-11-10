/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Resultado de un estado financiero: filas jerárquicas y totales por
 *  rubro para el resumen de la pantalla.
 * ===================================================================
*/

using System.Data;

namespace CapaControlador_Contabilidad
{
    public sealed class ClsModeloEstadoFinancieroContabilidad
    {
        public DataTable Filas { get; set; }
        public decimal Activo { get; set; }
        public decimal Pasivo { get; set; }
        public decimal Capital { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Costos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Resultado { get; set; }
        public decimal Operacion { get; set; }
        public decimal Inversion { get; set; }
        public decimal Financiamiento { get; set; }
        public decimal EfectivoInicial { get; set; }
        public decimal EfectivoFinal { get; set; }
        public bool Cuadra { get; set; }
    }
}
