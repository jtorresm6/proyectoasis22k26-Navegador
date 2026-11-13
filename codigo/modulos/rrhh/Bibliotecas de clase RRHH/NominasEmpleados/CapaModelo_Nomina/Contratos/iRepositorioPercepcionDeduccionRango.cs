/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define el contrato de acceso a datos de los rangos salariales
 *  de percepciones y deducciones. Incluye las operaciones básicas
 *  y el conteo de rangos con el mismo concepto y límite inicial,
 *  permitiendo excluir el registro que se esté modificando.
 * ==================================================================
 */

using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface iRepositorioPercepcionDeduccionRango : IRepositorioGenerico<ClsPercepcionDeduccionRango>
    {
        int RrhhMetContarPorDesde(int IdPercepcionDeduccion, decimal SalarioDesde, int IdExcluir);
    }
}
