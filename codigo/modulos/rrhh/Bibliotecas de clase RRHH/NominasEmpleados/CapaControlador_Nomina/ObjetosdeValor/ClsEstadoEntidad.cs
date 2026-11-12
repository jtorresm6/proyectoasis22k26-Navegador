/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Define la enumeración EstadoEntidad con los valores Added,
 *  Modified y Deleted. Los controladores utilizan estos estados
 *  para determinar si deben agregar, modificar o eliminar
 *  un registro al ejecutar la operación de grabar cambios.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.ObjetosdeValor
{
    public enum EstadoEntidad
    {
        Added,
        Modified,
        Deleted
    }
}
