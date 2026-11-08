using CapaControlador_Seguridad;
using CapaVista_Componentes.Componentes;
using Microsoft.Reporting.WinForms;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CapaVista_Compras
{
    public class FrmReporteBitacoraCompras : Form
    {
        internal FrmReporteBitacoraCompras(List<ClsModeloBitacora> datos)
        {
            Text = "Reporte de bitácora - Compras";
            BackColor = ClsTemaCompras.FondoGeneral;
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2,
                BackColor = ClsTemaCompras.FondoGeneral };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var encabezado = new ComprasUsrEncabezado { Dock = DockStyle.Fill, Margin = Padding.Empty,
                Titulo = "REPORTE DE BITÁCORA", Codigo = "", Descripcion = "Historial completo de acciones del sistema." };
            FrmBitacoraCompras.ComprasMetEncabezadoSinImagen(encabezado);
            layout.Controls.Add(encabezado, 0, 0);
            var visor = new ReportViewer { Dock = DockStyle.Fill, ZoomMode = ZoomMode.PageWidth,
                BackColor = ClsTemaCompras.FondoGeneral };
            visor.LocalReport.ReportEmbeddedResource = "CapaVista_Compras.Reportes.RpReporteBitacora.rdlc";
            visor.LocalReport.DataSources.Add(new ReportDataSource("RpReporteBitacora", datos));
            layout.Controls.Add(visor, 0, 1);
            Controls.Add(layout);
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            Load += (sender, e) => visor.RefreshReport();
        }
    }
}
