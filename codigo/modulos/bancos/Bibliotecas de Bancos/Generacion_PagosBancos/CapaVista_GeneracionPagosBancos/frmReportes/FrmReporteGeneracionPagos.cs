/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 10/11/2026
 * ================================================================
 * Propósito :
 *  Muestra en un visor el reporte con las órdenes de pago del
 *  proceso Generación de Pagos.
 * ================================================================
 */
using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_GeneracionPagos;
using Microsoft.Reporting.WinForms;

namespace CapaVista_GeneracionPagos.frmReportes
{
    public partial class FrmReporteGeneracionPagos : Form
    {
        private readonly ClsControladorGeneracionPagos _Controlador = new ClsControladorGeneracionPagos();

        public FrmReporteGeneracionPagos()
        {
            InitializeComponent();
        }

        private void FrmReporteGeneracionPagos_Load(object sender, EventArgs e)
        {
            string Error = _Controlador.BancosMetConsultarReporteOrdenes(out DataTable Ordenes);
            if (!string.IsNullOrEmpty(Error))
            {
                MessageBox.Show(Error, "No fue posible generar el reporte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            ReportDataSource DatosReporte = new ReportDataSource("DsGeneracionPagos", Ordenes);
            BancosRvReporte.LocalReport.ReportEmbeddedResource = "CapaVista_GeneracionPagos.Reportes.RpReporteGeneracionPagos.rdlc";
            BancosRvReporte.LocalReport.DataSources.Clear();
            BancosRvReporte.LocalReport.DataSources.Add(DatosReporte);
            BancosRvReporte.RefreshReport();
        }
    }
}
