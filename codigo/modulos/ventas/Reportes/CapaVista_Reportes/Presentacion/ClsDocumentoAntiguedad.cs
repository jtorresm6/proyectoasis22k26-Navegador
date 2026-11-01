// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Reportes;

namespace CapaVista_Reportes
{
    public static class ClsDocumentoAntiguedad
    {
        public static void VentasMetAbrir(Form Padre)
        {
            try
            {
                Padre.UseWaitCursor = true;
                DateTime Corte = DateTime.Today;
                DataTable Datos = new ClsAntiguedadControlador().VentasFuncConsultar(Corte, 0, "Todos");
                using (var Visor = new FrmVistaAntiguedad(Datos, Corte, "Todos los clientes", "Todos"))
                {
                    Padre.UseWaitCursor = false;
                    Visor.ShowDialog(Padre);
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Padre, Error.Message, "Ventas - Antigüedad de saldos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Padre.UseWaitCursor = false;
            }
        }

        internal static void VentasMetConfigurar(LocalReport Reporte, DataTable Datos, DateTime Corte, string Cliente, string Estado)
        {
            Reporte.ReportEmbeddedResource = "CapaVista_Reportes.Reportes.RpReporteAntiguedad.rdlc";
            Reporte.DataSources.Clear();
            Reporte.DataSources.Add(new ReportDataSource("DsAntiguedad", Datos));
            Reporte.SetParameters(new[] { new ReportParameter("FechaCorte", Corte.ToString("dd/MM/yyyy")), new ReportParameter("Cliente", Cliente), new ReportParameter("Estado", Estado) });
            Reporte.DisplayName = "Ventas_Antiguedad_" + Corte.ToString("yyyyMMdd");
        }

    }
}
