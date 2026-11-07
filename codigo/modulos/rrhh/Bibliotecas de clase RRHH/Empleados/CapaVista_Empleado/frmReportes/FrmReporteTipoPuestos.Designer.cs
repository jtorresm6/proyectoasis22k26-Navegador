namespace CapaVista_Empleado.frmReportes
{
    partial class FrmReporteTipoPuestos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteTipoPuestos));
            this.RrhhRvTipoPuestos = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvTipoPuestos
            // 
            this.RrhhRvTipoPuestos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvTipoPuestos.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvTipoPuestos.Name = "RrhhRvTipoPuestos";
            this.RrhhRvTipoPuestos.ServerReport.BearerToken = null;
            this.RrhhRvTipoPuestos.Size = new System.Drawing.Size(1186, 507);
            this.RrhhRvTipoPuestos.TabIndex = 0;
            // 
            // FrmReporteTipoPuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 507);
            this.Controls.Add(this.RrhhRvTipoPuestos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteTipoPuestos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9003 – ReporteTipoPuestos";
            this.Load += new System.EventHandler(this.FrmReporteTipoPuestos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvTipoPuestos;
    }
}
