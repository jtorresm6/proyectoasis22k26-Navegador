using CapaControlador_Seguridad.Objetos_de_valor;
using CapaModelo_EvaluacionDesempeño.Entidades;
using CapaModelo_EvaluacionDesempeño.Repositorios;
using System.Collections.Generic;
using System.Linq;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9404 - Ver Evaluaciones. Entrega a la vista las
 *  evaluaciones del usuario en sesión con la nota de cada aspecto.
 * ===================================================================
*/

namespace CapaControlador_EvaluacionDesempeño.Controladores
{
    public class ClsModeloVerEvaluaciones
    {
        private readonly ClsRepositorioVerEvaluaciones _RepositorioVerEvaluaciones = new ClsRepositorioVerEvaluaciones();

        public List<ClsResultadoEvaluacion> RrhhMetObtenerMisEvaluaciones()
        {
            return _RepositorioVerEvaluaciones
                .RrhhMetObtenerEvaluacionesUsuario(ClsSesionSeguridad.IdUsuario)
                .ToList();
        }
    }
}
