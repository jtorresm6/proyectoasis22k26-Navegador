/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de planes de mejora (aplicación 9403) con todos
 *  los planes registrados.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using CapaControlador_EvaluacionDesempeño.Controladores;
using Microsoft.Reporting.WinForms;

namespace CapaVista_EvaluacionDesempeño.frmReportes
{
    public partial class FrmReportePlanMejora : Form
    {
        private readonly ClsModeloPlanMejora _PlanMejora = new ClsModeloPlanMejora();

        public FrmReportePlanMejora()
        {
            InitializeComponent();
        }

        private void FrmReportePlanMejora_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsPlanMejora", _PlanMejora.RrhhMetObtenerTodos());
                RrhhRvPlanMejora.LocalReport.ReportEmbeddedResource = "CapaVista_EvaluacionDesempeño.Reportes.RpPlanMejora.rdlc";
                RrhhRvPlanMejora.LocalReport.DataSources.Clear();
                RrhhRvPlanMejora.LocalReport.DataSources.Add(Fuente);
                RrhhRvPlanMejora.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
