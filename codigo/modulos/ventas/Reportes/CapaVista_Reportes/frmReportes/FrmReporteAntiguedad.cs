// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Reportes
{
    public sealed class FrmReporteAntiguedad : Form
    {
        private readonly ReportViewer _VentasRpvAntiguedad;
        public FrmReporteAntiguedad(DataTable Datos, DateTime Corte, string Cliente, string Estado)
        {
            Text = "7006 – AntiguedadDeSaldos";
            Icon = Properties.Resources.IconoVentas;
            Size = new Size(1200, 700);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            _VentasRpvAntiguedad = new ReportViewer
            {
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local
            };
            Controls.Add(_VentasRpvAntiguedad);
            ClsDocumentoAntiguedad.VentasMetConfigurar(_VentasRpvAntiguedad.LocalReport, Datos, Corte, Cliente, Estado);
            Shown += (Origen, Evento) =>
            {
                _VentasRpvAntiguedad.SetDisplayMode(DisplayMode.PrintLayout);
                _VentasRpvAntiguedad.ZoomMode = ZoomMode.PageWidth;
                _VentasRpvAntiguedad.RefreshReport();
            };
        }
    }
}
