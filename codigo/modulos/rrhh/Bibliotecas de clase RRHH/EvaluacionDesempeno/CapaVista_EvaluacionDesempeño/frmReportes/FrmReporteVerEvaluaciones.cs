/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de evaluaciones de desempeño (aplicación 9404)
 *  con las evaluaciones que la ventana Ver Evaluaciones tiene cargadas
 *  del usuario en sesión.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaModelo_EvaluacionDesempeño.Entidades;
using Microsoft.Reporting.WinForms;

namespace CapaVista_EvaluacionDesempeño.frmReportes
{
    public partial class FrmReporteVerEvaluaciones : Form
    {
        private readonly List<ClsResultadoEvaluacion> _Evaluaciones;

        public FrmReporteVerEvaluaciones(List<ClsResultadoEvaluacion> Evaluaciones)
        {
            InitializeComponent();
            _Evaluaciones = Evaluaciones;
        }

        private void FrmReporteVerEvaluaciones_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsVerEvaluaciones", _Evaluaciones);
                RrhhRvVerEvaluaciones.LocalReport.ReportEmbeddedResource = "CapaVista_EvaluacionDesempeño.Reportes.RpVerEvaluaciones.rdlc";
                RrhhRvVerEvaluaciones.LocalReport.DataSources.Clear();
                RrhhRvVerEvaluaciones.LocalReport.DataSources.Add(Fuente);
                RrhhRvVerEvaluaciones.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
