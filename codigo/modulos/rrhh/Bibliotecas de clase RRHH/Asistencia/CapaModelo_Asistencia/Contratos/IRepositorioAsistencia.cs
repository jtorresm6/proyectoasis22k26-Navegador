using CapaModelo_Asistencia.Entidades;
using System;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de asistencia de empleados.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Contratos
{
    public interface IRepositorioAsistencia
    {
        int RrhhMetAgregar(ClsAsistenciaEmpleado Entidad);
        int RrhhMetEditar(ClsAsistenciaEmpleado Entidad);
        int RrhhMetRemover(ClsAsistenciaEmpleado Entidad);
        IEnumerable<ClsAsistenciaEmpleado> RrhhMetObtenerTodos();
        IEnumerable<ClsEmpleadoAsistencia> RrhhMetObtenerEmpleados();
        IEnumerable<ClsPermisoAsistencia> RrhhMetObtenerPermisosAprobados();
        IEnumerable<ClsCapacitacionAsistencia> RrhhMetObtenerCapacitaciones();
        DateTime? RrhhMetObtenerFechaBaja(int IdEmpleado);
    }
}
