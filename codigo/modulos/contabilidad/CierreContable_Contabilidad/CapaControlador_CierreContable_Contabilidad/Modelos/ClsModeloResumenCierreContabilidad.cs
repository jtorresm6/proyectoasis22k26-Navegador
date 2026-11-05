// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Donald Osorio
 * Carné : 0901-23-17982
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Resumen del período que la pantalla de cierre muestra al usuario.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public sealed class ClsModeloResumenCierreContabilidad
    {
        public int Aplicadas { get; set; }
        public int Borradores { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
    }
}
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
