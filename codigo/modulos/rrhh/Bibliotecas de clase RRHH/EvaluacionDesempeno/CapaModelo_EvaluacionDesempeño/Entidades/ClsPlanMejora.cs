/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Plan de mejora (tblPlanMejora) y la evaluación a la que
 *  pertenece (tblPlanMejora.idEvaluacionDesempeno).
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Entidades
{
    public class ClsPlanMejora
    {
        public int IdPlanMejora { get; set; }
        public string NombrePlanMejora { get; set; }
        public string DescripcionPlanMejora { get; set; }
        public int? IdEvaluacionDesempeno { get; set; }
        public int? IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string FechaEvaluacion { get; set; }
    }
}
