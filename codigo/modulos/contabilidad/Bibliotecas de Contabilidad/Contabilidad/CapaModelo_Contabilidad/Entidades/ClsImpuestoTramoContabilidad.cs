/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Un tramo de tblImpuestoTramo: desde qué monto aplica, hasta cuál,
 *  su tasa y el importe fijo que se suma.
 * ===================================================================
*/

namespace CapaModelo_Contabilidad.Entidades
{
    public class ClsImpuestoTramoContabilidad
    {
        public decimal MontoDesde { get; set; }
        public decimal? MontoHasta { get; set; }
        public decimal Tasa { get; set; }
        public decimal ImporteFijo { get; set; }
    }
}
