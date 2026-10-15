/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Una línea del detalle de una póliza (poliza_detalle): cuenta, debe
 *  y haber. Los importes son decimal, nunca double.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsLineaPolizaContabilidad
    {
        public int IdCuenta { get; set; }
        public string Cuenta { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
    }
}
