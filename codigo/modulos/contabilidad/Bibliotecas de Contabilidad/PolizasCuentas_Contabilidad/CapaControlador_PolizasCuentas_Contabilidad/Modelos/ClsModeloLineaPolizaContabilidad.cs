// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Una línea del detalle de una póliza (poliza_detalle): cuenta, debe
 *  y haber. Los importes son decimal, nunca double.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsModeloLineaPolizaContabilidad
    {
        public int IdCuenta { get; set; }
        public string Cuenta { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
    }
}
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
