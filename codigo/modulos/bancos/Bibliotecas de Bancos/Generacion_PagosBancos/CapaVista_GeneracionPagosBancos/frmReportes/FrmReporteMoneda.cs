/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 11/11/2026
 * ================================================================
 * Propósito :
 *  Muestra en un visor el reporte con el listado de monedas
 *  registradas en el mantenimiento Moneda.
 * ================================================================
 */
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_GeneracionPagos;
using Microsoft.Reporting.WinForms;

namespace CapaVista_GeneracionPagos.frmReportes
{
    public partial class FrmReporteMoneda : Form
    {
        private readonly ClsControladorGeneracionPagos _Controlador = new ClsControladorGeneracionPagos();

        public FrmReporteMoneda()
        {
            InitializeComponent();
        }

        private void FrmReporteMoneda_Load(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarReporteMonedas(out DataTable Monedas);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            ReportDataSource DatosReporte = new ReportDataSource("DsMoneda", Monedas);
            BancosRvReporte.LocalReport.ReportEmbeddedResource = "CapaVista_GeneracionPagos.Reportes.RpReporteMoneda.rdlc";
            BancosRvReporte.LocalReport.DataSources.Clear();
            BancosRvReporte.LocalReport.DataSources.Add(DatosReporte);
            BancosRvReporte.RefreshReport();
        }
    }
}
