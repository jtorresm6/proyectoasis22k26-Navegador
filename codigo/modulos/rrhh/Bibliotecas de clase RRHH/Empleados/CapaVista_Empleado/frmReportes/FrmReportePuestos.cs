/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de tblPuesto (aplicación 9004).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReportePuestos : Form
    {
        private ClsModeloPuesto _Puesto = new ClsModeloPuesto();

        public FrmReportePuestos()
        {
            InitializeComponent();
        }

        private void FrmReportePuestos_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsPuesto", _Puesto.RrhhMetObtenerParaReporte());
                RrhhRvPuestos.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpPuesto.rdlc";
                RrhhRvPuestos.LocalReport.DataSources.Clear();
                RrhhRvPuestos.LocalReport.DataSources.Add(Fuente);
                RrhhRvPuestos.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
