/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Valores de los seis indicadores del MDI. Un valor nulo es un
 *  indicador que no se pudo leer.
 * ===================================================================
*/

namespace CapaControlador_Contabilidad
{
    public class ClsModeloIndicadoresContabilidad
    {
        public int? Polizas { get; set; }
        public int? PresupuestosVigentes { get; set; }
        public int? ActivosFijos { get; set; }
        public int? PeriodosCerrados { get; set; }
        public int? PolizasAplicadas { get; set; }
        public int? Cuentas { get; set; }
    }
}
