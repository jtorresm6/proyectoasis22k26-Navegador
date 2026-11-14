/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Fila del reporte de activos fijos: la ficha del activo más el
 *  nombre de su tipo de vida útil.
 * Reglas especificas:
 *  Solo transporta datos leídos de la base; los cálculos de
 *  depreciación los hace el controlador.
 * ===================================================================
*/

namespace CapaModelo_Contabilidad.Entidades
{
    // Activo para el reporte
    public class ClsActivoReporteContabilidad : ClsActivoFijoContabilidad
    {
        // Nombre del tipo
        public string NombreTipo { get; set; }
    }
}
