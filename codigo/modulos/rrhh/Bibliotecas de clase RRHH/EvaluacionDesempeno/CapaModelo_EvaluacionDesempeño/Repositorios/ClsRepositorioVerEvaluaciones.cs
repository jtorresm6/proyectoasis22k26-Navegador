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
 *  Ventana 9404 - Ver Evaluaciones. Consulta de solo lectura de las
 *  evaluaciones del colaborador que inició sesión, con la nota de
 *  cada aspecto y el plan de mejora asignado.
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Repositorios
{
    public class ClsRepositorioVerEvaluaciones : ClsSentencias, IRepositorioVerEvaluaciones
    {
        // Aspectos evaluados: columna, nombre y peso (los mismos del trigger del puntaje).
        private static readonly Tuple<string, string, decimal>[] _Aspectos =
        {
            Tuple.Create("calidadEvaluacionDesempeno", "Calidad del trabajo", 0.25m),
            Tuple.Create("productividadEvaluacionDesempeno", "Productividad", 0.25m),
            Tuple.Create("trabajoEquipoEvaluacionDesempeno", "Trabajo en equipo", 0.20m),
            Tuple.Create("puntualidadEvaluacionDesempeno", "Puntualidad y asistencia", 0.15m),
            Tuple.Create("iniciativaEvaluacionDesempeno", "Iniciativa", 0.15m)
        };

        private const string _SelectEvaluaciones = @"
            SELECT e.*,
                   CONCAT(em.nombresEmpleado, ' ', em.apellidosEmpleado) AS nombreColaborador,
                   CONCAT(ev.nombresEmpleado, ' ', ev.apellidosEmpleado) AS nombreEvaluador,
                   pu.nombrePuesto,
                   p.nombrePlanMejora,
                   p.descripcionPlanMejora
            FROM tblEvaluacionDesempeno e
            INNER JOIN tblEmpleado em ON em.idEmpleado = e.idEmpleado
            INNER JOIN tblEmpleado ev ON ev.idEmpleado = e.idEvaluador
            LEFT JOIN tblPuestoEmpleado pe ON pe.idEmpleado = e.idEmpleado
                                          AND pe.fechaFinPuestoEmpleado IS NULL AND pe.is_active = 1
            LEFT JOIN tblPuesto pu ON pu.idPuesto = pe.idPuesto
            LEFT JOIN tblPlanMejora p ON p.idEvaluacionDesempeno = e.idEvaluacionDesempeno
            WHERE e.idEmpleado = (SELECT u.idEmpleado FROM tblusuario u WHERE u.idUsuario = ?)
              AND e.is_active = 1
            ORDER BY e.fechaEvaluacionDesempeno DESC, e.idEvaluacionDesempeno DESC";

        public IEnumerable<ClsResultadoEvaluacion> RrhhMetObtenerEvaluacionesUsuario(int IdUsuario)
        {
            var ListaEvaluaciones = new List<ClsResultadoEvaluacion>();
            foreach (DataRow Fila in RrhhMetEjecucionConsulta(_SelectEvaluaciones, IdUsuario).Rows)
            {
                var Evaluacion = new ClsResultadoEvaluacion
                {
                    IdEvaluacionDesempeno = Convert.ToInt32(Fila["idEvaluacionDesempeno"]),
                    FechaEvaluacionDesempeno = Convert.ToDateTime(Fila["fechaEvaluacionDesempeno"]),
                    NombreColaborador = Fila["nombreColaborador"].ToString(),
                    NombrePuesto = Fila["nombrePuesto"] == DBNull.Value ? "Sin puesto asignado" : Fila["nombrePuesto"].ToString(),
                    NombreEvaluador = Fila["nombreEvaluador"].ToString(),
                    PuntajeEvaluacionDesempeno = Fila["puntajeEvaluacionDesempeno"] == DBNull.Value ? 0 : Convert.ToDecimal(Fila["puntajeEvaluacionDesempeno"]),
                    NombrePlanMejora = Fila["nombrePlanMejora"] == DBNull.Value ? null : Fila["nombrePlanMejora"].ToString(),
                    DescripcionPlanMejora = Fila["descripcionPlanMejora"] == DBNull.Value ? null : Fila["descripcionPlanMejora"].ToString()
                };

                foreach (var Aspecto in _Aspectos)
                {
                    Evaluacion.Aspectos.Add(new ClsDetalleEvaluacion
                    {
                        NombreCriterioEvaluacion = Aspecto.Item2,
                        PesoDetalleEvaluacionDesempeno = Aspecto.Item3,
                        NotaDetalleEvaluacionDesempeno = Convert.ToInt32(Fila[Aspecto.Item1])
                    });
                }

                ListaEvaluaciones.Add(Evaluacion);
            }
            return ListaEvaluaciones;
        }
    }
}
