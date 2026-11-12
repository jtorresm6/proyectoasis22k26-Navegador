/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Mostrar el reporte estándar de seguimiento de pedidos.
 */
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Presenta el reporte RDLC filtrado de pedidos.
    internal partial class FrmReporteSeguimientoPedidos : Form
    {
        public FrmReporteSeguimientoPedidos() { InitializeComponent(); }

        internal FrmReporteSeguimientoPedidos(DataTable Pedidos, DateTime Desde, DateTime Hasta, string Estado) : this()
        {
            LocalReport Reporte = VentasUsrReportePedidos.LocalReport;
            Reporte.ReportEmbeddedResource = "CapaVista_Pedidos.Reportes.RpReporteSeguimientoPedidos.rdlc";
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DsPedidos", Pedidos));
            Reporte.SetParameters(new[]
            {
                new ReportParameter("FechaDesde", Desde.ToString("dd/MM/yyyy")),
                new ReportParameter("FechaHasta", Hasta.ToString("dd/MM/yyyy")),
                new ReportParameter("Estado", Estado)
            });
            Reporte.DisplayName = "Ventas_Pedidos_" + Estado.Replace(' ', '_') + "_" +
                Desde.ToString("yyyyMMdd") + "_" + Hasta.ToString("yyyyMMdd");
        }

        private void VentasMetReporteMostrado(object Origen, EventArgs Evento)
        {
            VentasUsrReportePedidos.SetDisplayMode(DisplayMode.PrintLayout);
            VentasUsrReportePedidos.ZoomMode = ZoomMode.PageWidth;
            VentasUsrReportePedidos.RefreshReport();
        }
    }
}
