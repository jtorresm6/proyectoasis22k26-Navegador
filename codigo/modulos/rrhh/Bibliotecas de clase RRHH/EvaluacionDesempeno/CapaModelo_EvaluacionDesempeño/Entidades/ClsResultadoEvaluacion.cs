using System;
using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Resultado de una evaluación de desempeño para Ver Evaluaciones:
 *  datos de la evaluación y la calificación de cada aspecto.
 * ===================================================================
*/

namespace CapaModelo_EvaluacionDesempeño.Entidades
{
    public class ClsResultadoEvaluacion
    {
        public int IdEvaluacionDesempeno { get; set; }
        public DateTime FechaEvaluacionDesempeno { get; set; }
        public string NombreColaborador { get; set; }
        public string NombrePuesto { get; set; }
        public string NombreEvaluador { get; set; }
        public decimal PuntajeEvaluacionDesempeno { get; set; }
        public string NombrePlanMejora { get; set; }
        public string DescripcionPlanMejora { get; set; }
        public List<ClsDetalleEvaluacion> Aspectos { get; set; } = new List<ClsDetalleEvaluacion>();

        public string DescripcionEvaluacion => FechaEvaluacionDesempeno.ToString("dd/MM/yyyy") + " - " + NombreEvaluador;

        // Puntaje sobre 5.00 expresado de 0 a 1.
        public decimal PorcentajeEvaluacionDesempeno => PuntajeEvaluacionDesempeno / 5m;

        public string PorcentajeTexto => (PorcentajeEvaluacionDesempeno * 100).ToString("0") + "%";

        public string NombreNivelDesempeno =>
            PorcentajeEvaluacionDesempeno >= 0.80m ? "Excelente" :
            PorcentajeEvaluacionDesempeno >= 0.50m ? "Satisfactorio" : "Insuficiente";
    }

    public class ClsDetalleEvaluacion
    {
        public string NombreCriterioEvaluacion { get; set; }
        public decimal PesoDetalleEvaluacionDesempeno { get; set; }
        public int NotaDetalleEvaluacionDesempeno { get; set; }
        public decimal PonderadoDetalleEvaluacionDesempeno => PesoDetalleEvaluacionDesempeno * NotaDetalleEvaluacionDesempeno;
    }
}
