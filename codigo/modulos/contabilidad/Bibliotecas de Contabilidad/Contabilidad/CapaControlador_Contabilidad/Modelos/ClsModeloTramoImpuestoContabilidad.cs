/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Tramo de un impuesto que se entrega a los demás módulos.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsModeloTramoImpuestoContabilidad
    {
        public decimal MontoDesde { get; set; }
        public decimal? MontoHasta { get; set; }
        public decimal Tasa { get; set; }
        public decimal ImporteFijo { get; set; }
    }
}
