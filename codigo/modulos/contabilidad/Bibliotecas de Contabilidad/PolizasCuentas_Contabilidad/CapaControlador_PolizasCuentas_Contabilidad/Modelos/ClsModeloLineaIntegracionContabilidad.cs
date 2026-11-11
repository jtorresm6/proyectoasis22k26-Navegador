/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Línea de una póliza enviada por otro módulo: la cuenta va por su
 *  código del catálogo contable, con su monto en el debe o en el haber.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsModeloLineaIntegracionContabilidad
    {
        public string CodigoCuenta { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string Descripcion { get; set; }
    }
}
