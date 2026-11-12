/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de activos fijos: el mantenimiento
 *  genérico más módulos y maestro de vida útil.
 * ===================================================================
*/

using System.Collections.Generic;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Contratos
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public interface IRepositorioActivosFijosContabilidad : IRepositorioGenericoContabilidad<ClsActivoFijoContabilidad>
    {
        string[] ContabilidadFuncObtenerModulos();

        List<ClsVidaUtilContabilidad> ContabilidadFuncObtenerVidaUtil();

        // Inicio de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
        // Indica si el mes ya tiene póliza de depreciación
        bool ContabilidadFuncPeriodoDepreciado(string Periodo);

        // Graba la póliza del mes y baja el valor en libros
        string ContabilidadFuncRegistrarDepreciacion(string Periodo, System.DateTime Fecha, string Descripcion,
            List<ClsCuotaDepreciacionContabilidad> Cuotas, List<ClsLineaDepreciacionContabilidad> Lineas);
        // Fin de cambio: Gabriel Guillén - 0901-23-1998 - 10/11/2026
    }
}
