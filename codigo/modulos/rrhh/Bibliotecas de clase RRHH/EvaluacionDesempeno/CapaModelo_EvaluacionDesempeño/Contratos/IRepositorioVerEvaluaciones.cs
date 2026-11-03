using CapaModelo_EvaluacionDesempeño.Entidades;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Contrato del repositorio de la ventana 9404 - Ver Evaluaciones.
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Contratos
{
    public interface IRepositorioVerEvaluaciones
    {
        IEnumerable<ClsResultadoEvaluacion> RrhhMetObtenerEvaluacionesUsuario(int IdUsuario);
    }
}
