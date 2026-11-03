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
using CapaControlador_Nomina.Modelos_de_controladores;

namespace CapaVista_Nomina.Reportes
{
    public partial class FrmReportePercepcionDeduccion : Form
    {
        private ClsModeloPercepcionDeduccion PercepcionDeduccion = new ClsModeloPercepcionDeduccion();

        public FrmReportePercepcionDeduccion()
        {
            InitializeComponent();
        }

        private void FrmReportePercepcionDeduccion_Load(object sender, EventArgs e)
        {

            ReportDataSource reportDataSource = new ReportDataSource("DsPercepcionDeduccion", PercepcionDeduccion.RrhhMetObtenerTodos());
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Nomina.Reportes.RpReportePercepcionDeduccion.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
