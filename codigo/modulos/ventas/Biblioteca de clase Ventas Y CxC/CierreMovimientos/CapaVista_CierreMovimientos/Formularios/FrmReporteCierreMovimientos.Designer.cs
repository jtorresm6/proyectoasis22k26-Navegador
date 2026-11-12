/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 10/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Mostrar el reporte estándar del cierre.
 */
namespace CapaVista_CierreMovimientos
{
    internal sealed partial class FrmReporteCierreMovimientos
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Reporting.WinForms.ReportViewer VentasUsrReporteCierre;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.VentasUsrReporteCierre = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            this.VentasUsrReporteCierre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasUsrReporteCierre.Location = new System.Drawing.Point(0, 0);
            this.VentasUsrReporteCierre.Name = "VentasUsrReporteCierre";
            this.VentasUsrReporteCierre.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.VentasUsrReporteCierre.Size = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.VentasUsrReporteCierre);
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.MinimizeBox = true;
            this.Name = "FrmReporteCierreMovimientos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7010 – Reporte de cierre de movimientos";
            this.Shown += new System.EventHandler(this.VentasMetReporteMostrado);
            this.ResumeLayout(false);
        }
    }
}
