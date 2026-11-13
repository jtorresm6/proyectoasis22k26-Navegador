/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Contrato de la consulta del reporte de activos fijos.
 * Reglas especificas:
 *  Un módulo o tipo vacío significa sin filtro.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using CapaModelo_Contabilidad.Entidades;

namespace CapaModelo_Contabilidad.Contratos
{
    // Consulta del reporte
    public interface IRepositorioReporteActivosFijosContabilidad
    {
        // Activos al corte
        List<ClsActivoReporteContabilidad> ContabilidadFuncObtenerActivosAlCorte(DateTime Corte, string Modulo, string Tipo);
    }
}
