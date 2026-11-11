// Jose Javier Torres Martinez | 0901-23-1091 | 22/10/2026
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CapaVista_Reportes
{
    public sealed class FrmVistaAntiguedad : Form
    {
        private readonly ReportViewer _VentasUsrAntiguedad;
        public FrmVistaAntiguedad(DataTable Datos, DateTime Corte, string Cliente, string Estado)
        {
            Name = "FrmVistaAntiguedad";
            Text = "7006 – AntiguedadDeSaldos";
            Icon = Properties.Resources.IconoVentas;
            Size = new Size(1200, 800);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = true;
            StartPosition = FormStartPosition.CenterParent;
            _VentasUsrAntiguedad = new ReportViewer
            {
                Name = "VentasUsrAntiguedad",
                Dock = DockStyle.Fill,
                ProcessingMode = ProcessingMode.Local
            };
            Controls.Add(_VentasUsrAntiguedad);
            ClsDocumentoAntiguedad.VentasMetConfigurar(_VentasUsrAntiguedad.LocalReport, Datos, Corte, Cliente, Estado);
            Shown += (Origen, Evento) =>
            {
                _VentasUsrAntiguedad.SetDisplayMode(DisplayMode.PrintLayout);
                _VentasUsrAntiguedad.ZoomMode = ZoomMode.PageWidth;
                _VentasUsrAntiguedad.RefreshReport();
            };
        }
    }
}
