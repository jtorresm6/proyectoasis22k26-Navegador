/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Muestra el reporte de tblTipoPuesto (aplicación 9003).
 * ===================================================================
*/

using System;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Empleado.Modelos_de_Controladores;

namespace CapaVista_Empleado.frmReportes
{
    public partial class FrmReporteTipoPuestos : Form
    {
        private ClsModeloTipoPuesto _TipoPuesto = new ClsModeloTipoPuesto();

        public FrmReporteTipoPuestos()
        {
            InitializeComponent();
        }

        private void FrmReporteTipoPuestos_Load(object sender, EventArgs e)
        {
            try
            {
                ReportDataSource Fuente = new ReportDataSource("DsTipoPuesto", _TipoPuesto.RrhhMetObtenerParaReporte());
                RrhhRvTipoPuestos.LocalReport.ReportEmbeddedResource = "CapaVista_Empleado.Reportes.RpTipoPuesto.rdlc";
                RrhhRvTipoPuestos.LocalReport.DataSources.Clear();
                RrhhRvTipoPuestos.LocalReport.DataSources.Add(Fuente);
                RrhhRvTipoPuestos.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el reporte.\n" + ex.Message, "Reporte",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
