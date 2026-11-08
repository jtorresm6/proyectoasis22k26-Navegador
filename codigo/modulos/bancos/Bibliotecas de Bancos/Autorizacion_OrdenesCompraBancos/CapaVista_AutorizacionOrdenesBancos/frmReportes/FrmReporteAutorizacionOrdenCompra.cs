using CapaControlador_AutorizacionOrdenes.Modelos_de_controladores;
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
using static System.Net.Mime.MediaTypeNames;

namespace CapaVista_AutorizacionOrdenes.frmReportes
{
    public partial class FrmReporteAutorizacionOrdenCompra : Form
    {
        private ClsModeloAutorizacionOrdenCompra autorizacionOrdenCompra = new ClsModeloAutorizacionOrdenCompra();
        public FrmReporteAutorizacionOrdenCompra()
        {
            InitializeComponent();
        }

        private void FrmReporteAutorizacionOrdenCompra_Load(object sender, EventArgs e)
        {
            ReportDataSource reportDataSource = new ReportDataSource("DsAutorizacionOrdenCompra", autorizacionOrdenCompra.BancosMetObtenerTodos());
            reportViewer1.LocalReport.ReportEmbeddedResource = "CapaVista_AutorizacionOrdenes.Reportes.RpReporteAutorizacionOrdenCompra.rdlc";
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
