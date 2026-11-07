using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteInstructor : Form
    {
        private ClsModeloInstructor Instructor = new ClsModeloInstructor();

        public FrmReporteInstructor()
        {
            InitializeComponent();
        }

        private void FrmReporteInstructor_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsInstructor", Instructor.InstructorMetObtenerParaReporte());
                reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpInstructor.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(Fuente);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}