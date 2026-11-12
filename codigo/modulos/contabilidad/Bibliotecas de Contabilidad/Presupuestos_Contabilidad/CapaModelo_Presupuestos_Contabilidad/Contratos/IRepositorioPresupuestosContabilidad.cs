/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de presupuestos: el mantenimiento
 *  genérico más la lista de módulos.
 * ===================================================================
*/

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
using System;
using System.Collections.Generic;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
namespace CapaModelo_Contabilidad.Contratos
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public interface IRepositorioPresupuestosContabilidad : IRepositorioGenericoContabilidad<ClsPresupuestoContabilidad>
    {
        string[] ContabilidadFuncObtenerModulos();

        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
        Dictionary<string, decimal> ContabilidadFuncObtenerEjecutado(DateTime Desde, DateTime Hasta);
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 28/10/2026
    }
}
