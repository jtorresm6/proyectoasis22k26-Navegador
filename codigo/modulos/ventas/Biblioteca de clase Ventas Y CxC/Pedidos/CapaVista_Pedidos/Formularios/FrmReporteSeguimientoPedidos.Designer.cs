namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Define el visor visual del reporte RDLC de pedidos.
    partial class FrmReporteSeguimientoPedidos
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Reporting.WinForms.ReportViewer VentasUsrReportePedidos;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.VentasUsrReportePedidos = new Microsoft.Reporting.WinForms.ReportViewer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteSeguimientoPedidos));
            this.SuspendLayout();
            this.VentasUsrReportePedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasUsrReportePedidos.Location = new System.Drawing.Point(0, 0);
            this.VentasUsrReportePedidos.Name = "VentasUsrReportePedidos";
            this.VentasUsrReportePedidos.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            this.VentasUsrReportePedidos.Size = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.VentasUsrReportePedidos);
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("IconoVentas")));
            this.Name = "FrmReporteSeguimientoPedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7004 – SeguimientoDePedidos";
            this.Shown += new System.EventHandler(this.VentasMetReporteMostrado);
            this.ResumeLayout(false);
        }
    }
}
