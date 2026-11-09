/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de pólizas: el mantenimiento genérico
 *  más los catálogos que necesita la póliza.
 * ===================================================================
*/

// Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
using System;
// Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
using System.Collections.Generic;
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaModelo_Contabilidad.Entidades;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaModelo_Contabilidad.Contratos
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    public interface IRepositorioPolizasContabilidad : CapaModelo_Contabilidad.Contratos.IRepositorioGenericoContabilidad<ClsPolizaContabilidad>
    {
        List<ClsCuentaContabilidad> ContabilidadFuncObtenerCuentas();

        string[] ContabilidadFuncObtenerEstados();

        string ContabilidadFuncObtenerNombreModulo(int IdModulo);

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026
        int ContabilidadFuncActualizarSaldos(string Modo, string TipoPeriodo, DateTime Desde, DateTime Hasta, int IdPoliza, string Usuario);

        string ContabilidadFuncObtenerMetodoActualizacion(int IdModulo);
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 28/10/2026

        // Inicio de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
        Dictionary<int, string> ContabilidadFuncObtenerPaises();

        Dictionary<int, string> ContabilidadFuncObtenerMonedasPais();
        // Fin de cambio: Oskar Cermeño - 0901-23-15379 - 29/10/2026
    }
}
