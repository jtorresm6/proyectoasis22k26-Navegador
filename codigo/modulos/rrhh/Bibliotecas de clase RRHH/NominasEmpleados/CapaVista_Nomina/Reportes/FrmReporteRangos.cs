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
    public partial class FrmReporteRangos : Form
    {
        private ClsModeloPercepcionDeduccionRango Rango = new ClsModeloPercepcionDeduccionRango();
        public FrmReporteRangos()
        {
            InitializeComponent();
        }

        private void FrmReporteRangos_Load(object sender, EventArgs e)
        {

            ReportDataSource reportDataSource = new ReportDataSource("DsRango", Rango.RrhhMetObtenerTodos());
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Nomina.Reportes.RpReporteRangos.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
