using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using CapaControlador_Facturas;
namespace CapaVista_Facturas
{
    public sealed class FrmReporteFactura : Form
    {
        public FrmReporteFactura(ClsFacturaRegistrada Factura)
        {
            Text="7007 – Vista previa de factura " + Factura.Numero;
            Width=1100; Height=800; StartPosition=FormStartPosition.CenterParent;
            MaximizeBox=false; FormBorderStyle=FormBorderStyle.FixedDialog;
            var VentasViewerFactura=new ReportViewer { Dock=DockStyle.Fill, ProcessingMode=ProcessingMode.Local, ShowPrintButton=true };
            Controls.Add(VentasViewerFactura);
            ClsReporteFactura.VentasMetConfigurar(VentasViewerFactura.LocalReport,Factura);
            VentasViewerFactura.SetDisplayMode(DisplayMode.PrintLayout);
            VentasViewerFactura.ZoomMode=ZoomMode.PageWidth;
            VentasViewerFactura.RefreshReport();
        }
    }
}
