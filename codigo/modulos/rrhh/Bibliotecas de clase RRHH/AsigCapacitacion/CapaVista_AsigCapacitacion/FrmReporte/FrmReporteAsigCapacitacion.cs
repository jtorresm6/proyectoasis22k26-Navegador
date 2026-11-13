using CapaControlador_AsigCapacitacion.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_AsigCapacitacion.FrmReporte
{
    public partial class FrmReporteAsigCapacitacion : Form
    {
        private ClsModeloAsigCapacitacion AsigCapacitacion = new ClsModeloAsigCapacitacion();
        public FrmReporteAsigCapacitacion()
        {
            InitializeComponent();
        }

        private void FrmReporteAsigCapacitacion_Load(object sender, EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource("DsAsigCapacitacion", AsigCapacitacion.RrhhMetObtenerTodos());
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_AsigCapacitacion.Reporte.RpReporteAsigCapacitacion.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
