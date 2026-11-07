/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de tblDepartamento (aplicación 9002).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteDepartamentos : Form
    {
        private ClsModeloDepartamento _Departamento = new ClsModeloDepartamento();

        public FrmReporteDepartamentos()
        {
            InitializeComponent();
        }

        private void FrmReporteDepartamentos_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsDepartamento", _Departamento.RrhhMetObtenerParaReporte());
                RrhhRvDepartamentos.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpDepartamento.rdlc";
                RrhhRvDepartamentos.LocalReport.DataSources.Clear();
                RrhhRvDepartamentos.LocalReport.DataSources.Add(Fuente);
                RrhhRvDepartamentos.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
