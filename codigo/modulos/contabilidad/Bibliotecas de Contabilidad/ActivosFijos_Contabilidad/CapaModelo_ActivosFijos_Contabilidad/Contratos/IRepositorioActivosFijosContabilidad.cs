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
    }
}
