/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 10/11/2026
 * ================================================================
 * Propósito :
 *  Muestra en un visor el reporte con el historial de cierres
 *  bancarios registrados.
 * ================================================================
 */
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_CierreBancario;
using Microsoft.Reporting.WinForms;

namespace CapaVista_CierreBancario.frmReportes
{
    public partial class FrmReporteCierreBancario : Form
    {
        private readonly ClsControladorCierreBancario _Controlador = new ClsControladorCierreBancario();

        public FrmReporteCierreBancario()
        {
            InitializeComponent();
        }

        private void FrmReporteCierreBancario_Load(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarHistorico(out DataTable Historico);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            ReportDataSource DatosReporte = new ReportDataSource("DsCierreBancario", Historico);
            BancosRvReporte.LocalReport.ReportEmbeddedResource = "CapaVista_CierreBancario.Reportes.RpReporteCierreBancario.rdlc";
            BancosRvReporte.LocalReport.DataSources.Clear();
            BancosRvReporte.LocalReport.DataSources.Add(DatosReporte);
            BancosRvReporte.RefreshReport();
        }
    }
}
