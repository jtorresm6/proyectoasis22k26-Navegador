using CapaModelo_EvaluacionDesempeño.Contratos;
using CapaModelo_EvaluacionDesempeño.Entidades;
using System;
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
 *  Ventana 9403 - Plan de Mejora. Guarda el plan (tblPlanMejora) con
 *  la evaluación del colaborador a la que pertenece
 *  (tblPlanMejora.idEvaluacionDesempeno). Una evaluación tiene como
 *  máximo un plan.
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Repositorios
{
    public class ClsRepositorioPlanMejora : ClsSentencias, IRepositorioPlanMejora
    {
        private const string _SelectAll = @"
            SELECT p.idPlanMejora,
                   p.nombrePlanMejora,
                   p.descripcionPlanMejora,
                   p.idEvaluacionDesempeno,
                   e.idEmpleado,
                   CONCAT(em.nombresEmpleado, ' ', em.apellidosEmpleado) AS nombreEmpleado,
                   DATE_FORMAT(e.fechaEvaluacionDesempeno, '%d/%m/%Y') AS fechaEvaluacion
            FROM tblPlanMejora p
            LEFT JOIN tblEvaluacionDesempeno e ON e.idEvaluacionDesempeno = p.idEvaluacionDesempeno
            LEFT JOIN tblEmpleado em ON em.idEmpleado = e.idEmpleado
            WHERE p.is_active = 1
            ORDER BY p.idPlanMejora";

        private const string _Insert = "INSERT INTO tblPlanMejora (nombrePlanMejora, descripcionPlanMejora, idEvaluacionDesempeno) VALUES (?, ?, ?)";
        private const string _Update = "UPDATE tblPlanMejora SET nombrePlanMejora = ?, descripcionPlanMejora = ?, idEvaluacionDesempeno = ? WHERE idPlanMejora = ?";
        private const string _Delete = "DELETE FROM tblPlanMejora WHERE idPlanMejora = ?";

        public int RrhhMetAgregar(ClsPlanMejora Entidad)
        {
            RrhhMetEjecucionNonQuery(_Insert, Entidad.NombrePlanMejora, Entidad.DescripcionPlanMejora, Entidad.IdEvaluacionDesempeno);
            return Convert.ToInt32(RrhhMetEjecucionConsulta(
                "SELECT idPlanMejora FROM tblPlanMejora WHERE nombrePlanMejora = ?", Entidad.NombrePlanMejora).Rows[0][0]);
        }

        public int RrhhMetEditar(ClsPlanMejora Entidad)
        {
            return RrhhMetEjecucionNonQuery(_Update,
                Entidad.NombrePlanMejora, Entidad.DescripcionPlanMejora, Entidad.IdEvaluacionDesempeno, Entidad.IdPlanMejora);
        }

        public int RrhhMetRemover(ClsPlanMejora Entidad)
        {
            return RrhhMetEjecucionNonQuery(_Delete, Entidad.IdPlanMejora);
        }

        public IEnumerable<ClsPlanMejora> RrhhMetObtenerTodos()
        {
            var ListaPlanes = new List<ClsPlanMejora>();
            foreach (DataRow Fila in RrhhMetEjecucionConsulta(_SelectAll).Rows)
            {
                ListaPlanes.Add(new ClsPlanMejora
                {
                    IdPlanMejora = Convert.ToInt32(Fila["idPlanMejora"]),
                    NombrePlanMejora = Fila["nombrePlanMejora"].ToString(),
                    DescripcionPlanMejora = Fila["descripcionPlanMejora"].ToString(),
                    IdEvaluacionDesempeno = Fila["idEvaluacionDesempeno"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["idEvaluacionDesempeno"]),
                    IdEmpleado = Fila["idEmpleado"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["idEmpleado"]),
                    NombreEmpleado = Fila["nombreEmpleado"] == DBNull.Value ? "Sin asignar" : Fila["nombreEmpleado"].ToString(),
                    FechaEvaluacion = Fila["fechaEvaluacion"] == DBNull.Value ? "—" : Fila["fechaEvaluacion"].ToString()
                });
            }
            return ListaPlanes;
        }

        public DataTable RrhhMetObtenerEmpleados()
        {
            return RrhhMetEjecucionConsulta(
                @"SELECT idEmpleado,
                         CONCAT(nombresEmpleado, ' ', apellidosEmpleado) AS nombreEmpleado
                  FROM tblEmpleado
                  WHERE is_active = 1
                  ORDER BY nombresEmpleado, apellidosEmpleado");
        }

        // Evaluaciones del empleado que no tienen plan o cuyo plan es este mismo.
        public DataTable RrhhMetObtenerEvaluaciones(int IdEmpleado, int IdPlanMejora)
        {
            return RrhhMetEjecucionConsulta(
                @"SELECT e.idEvaluacionDesempeno,
                         CONCAT(DATE_FORMAT(e.fechaEvaluacionDesempeno, '%d/%m/%Y'),
                                ' - Puntaje ', e.puntajeEvaluacionDesempeno) AS descripcionEvaluacion
                  FROM tblEvaluacionDesempeno e
                  WHERE e.idEmpleado = ?
                    AND e.is_active = 1
                    AND NOT EXISTS (SELECT 1 FROM tblPlanMejora p
                                    WHERE p.idEvaluacionDesempeno = e.idEvaluacionDesempeno
                                      AND p.idPlanMejora <> ?)
                  ORDER BY e.fechaEvaluacionDesempeno DESC",
                IdEmpleado, IdPlanMejora);
        }
    }
}
