/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de una línea de la póliza (tblPolizaDetalle). Los importes
 *  son decimal, nunca double.
 * Reglas especificas:
 *  Entidad creada por Oskar Cermeño; se ajustó a EST-01 y se agregaron
 *  la línea y la descripción del detalle.
 * ===================================================================
*/

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Entidades
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public class ClsPolizaDetalleContabilidad
    {
        public int IdPolizaDetalle { get; set; }
        public int IdPoliza { get; set; }
        public int IdCuenta { get; set; }
        public int Linea { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string DescripcionDetalle { get; set; }
        public string Codigo { get; set; }
        public string Cuenta { get; set; }
    }
}
