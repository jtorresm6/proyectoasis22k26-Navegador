using CapaControlador_CierreNominas;
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
    public partial class FrmReporteCierreNominas : Form
    {
        private ClsModeloCierreNomina AsignacionConceptos = new ClsModeloCierreNomina();
        public FrmReporteCierreNominas()
        {
            InitializeComponent();
        }

        private void FrmReporteCierreNominas_Load(object sender, EventArgs e)
        {

            ReportDataSource reportDataSource = new ReportDataSource("DsFilasCierreNominas", AsignacionConceptos.RrhhMetObtenerTodos());
            RpReporteCierreNominas.LocalReport.ReportEmbeddedResource = "CapaVista_CierreNominas.Reportes.RpReporteCierreNominas.rdlc";
            RpReporteCierreNominas.LocalReport.DataSources.Clear();
            RpReporteCierreNominas.LocalReport.DataSources.Add(reportDataSource);
            this.RpReporteCierreNominas.RefreshReport();

     
        }
    }
}
