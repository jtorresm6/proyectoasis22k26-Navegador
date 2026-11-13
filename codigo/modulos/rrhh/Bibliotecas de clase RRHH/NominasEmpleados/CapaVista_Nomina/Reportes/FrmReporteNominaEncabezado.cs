using CapaControlador_Nomina.Modelos_de_controladores;
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

namespace CapaVista_Nomina.Reportes
{
    public partial class FrmReporteNominaEncabezado : Form
    {
        private ClsModeloNominaEncabezado NominaEncabezado = new ClsModeloNominaEncabezado();
        public FrmReporteNominaEncabezado()
        {
            InitializeComponent();
        }

        private void FrmReporteNominaEncabezado_Load(object sender, EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource("DsNominaEncabezado", NominaEncabezado.RrhhMetObtenerTodos());
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Nomina.Reportes.RpReporteNominaEncabezado.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
