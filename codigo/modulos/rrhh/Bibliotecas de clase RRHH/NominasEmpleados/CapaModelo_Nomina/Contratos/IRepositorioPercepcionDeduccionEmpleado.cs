/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define el contrato de acceso a datos de las asignaciones de
 *  conceptos. Incluye las operaciones básicas y los conteos por
 *  empleado o departamento para comprobar si una asignación
 *  ya existe, excluyendo el registro que se esté modificando.
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
    public interface IRepositorioPercepcionDeduccionEmpleado : IRepositorioGenerico<ClsPercepcionDeduccionEmpleado>
    {
        int RrhhMetContarPorEmpleado(int IdPercepcionDeduccion, int IdEmpleado, int IdExcluir);

        // Cuenta asignaciones repetidas de la misma percepcion/deduccion al mismo departamento
        int RrhhMetContarPorDepartamento(int IdPercepcionDeduccion, int IdDepartamento, int IdExcluir);
    }
}
