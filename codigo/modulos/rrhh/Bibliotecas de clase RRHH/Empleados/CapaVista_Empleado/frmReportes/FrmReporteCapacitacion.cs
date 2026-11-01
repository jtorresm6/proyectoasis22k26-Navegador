using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteCapacitacion : Form
    {
        private ClsModeloCapacitacion Capacitacion = new ClsModeloCapacitacion();

        public FrmReporteCapacitacion()
        {
            InitializeComponent();
        }

        private void FrmReporteCapacitacion_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsCapacitacion", Capacitacion.CapacitacionMetObtenerParaReporte());
                reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpCapacitacion.rdlc";
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