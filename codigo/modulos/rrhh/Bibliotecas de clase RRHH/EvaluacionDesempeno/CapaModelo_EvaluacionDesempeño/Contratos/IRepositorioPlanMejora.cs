using CapaModelo_EvaluacionDesempeño.Entidades;
using System.Collections.Generic;
using System.Data;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de la ventana 9403 - Plan de Mejora.
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Contratos
{
    public interface IRepositorioPlanMejora
    {
        int RrhhMetAgregar(ClsPlanMejora Entidad);
        int RrhhMetEditar(ClsPlanMejora Entidad);
        int RrhhMetRemover(ClsPlanMejora Entidad);
        IEnumerable<ClsPlanMejora> RrhhMetObtenerTodos();
        DataTable RrhhMetObtenerEmpleados();
        DataTable RrhhMetObtenerEvaluaciones(int IdEmpleado, int IdPlanMejora);
    }
}
