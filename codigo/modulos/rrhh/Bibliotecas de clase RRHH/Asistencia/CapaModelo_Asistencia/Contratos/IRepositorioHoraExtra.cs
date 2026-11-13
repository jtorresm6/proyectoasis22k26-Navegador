using CapaModelo_Asistencia.Entidades;
using System.Collections.Generic;
using System.Data;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de horas extra.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Contratos
{
    public interface IRepositorioHoraExtra
    {
        int RrhhMetAgregar(ClsHoraExtra Entidad);
        int RrhhMetEditar(ClsHoraExtra Entidad);
        int RrhhMetRemover(ClsHoraExtra Entidad);
        IEnumerable<ClsHoraExtra> RrhhMetObtenerTodos();
        IEnumerable<ClsAsistenciaHoraExtra> RrhhMetObtenerAsistenciasConExtra();
        DataTable RrhhMetObtenerConceptos();
        ClsHoraExtra RrhhMetBuscarPorId(int IdHoraExtra);
    }
}
