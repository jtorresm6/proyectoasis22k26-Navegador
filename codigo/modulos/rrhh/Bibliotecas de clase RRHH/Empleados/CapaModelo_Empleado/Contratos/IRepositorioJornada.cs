/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de tblJornada.
 * ===================================================================
*/

using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Contratos
{
    public interface IRepositorioJornada
    {
        IEnumerable<ClsJornada> RrhhMetObtenerTodos();
    }
}
