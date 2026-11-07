using CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores;
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

namespace CapaVista_Liquidacion_Rrhh.frmReportes
{
    public partial class FrmReporteMotivoBaja : Form
    {
        private ClsModeloMotivoBaja MotivoBaja = new ClsModeloMotivoBaja();
        public FrmReporteMotivoBaja()
        {
            InitializeComponent();
        }

        private void FrmReporteMotivoBaja_Load(object sender, EventArgs e)
        {

            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsMotivoBaja", MotivoBaja.MotivoBajaMetObtenerParaReporte());
                reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_Liquidacion_Rrhh.Reportes.RpMotivoBaja.rdlc";
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(Fuente);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.reportViewer1.RefreshReport();
        }

    }
}

