namespace CapaVista_Empleado.frmReportes
{
    partial class FrmReportePuestos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportePuestos));
            this.RrhhRvPuestos = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvPuestos
            // 
            this.RrhhRvPuestos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvPuestos.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvPuestos.Name = "RrhhRvPuestos";
            this.RrhhRvPuestos.ServerReport.BearerToken = null;
            this.RrhhRvPuestos.Size = new System.Drawing.Size(1186, 507);
            this.RrhhRvPuestos.TabIndex = 0;
            // 
            // FrmReportePuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 507);
            this.Controls.Add(this.RrhhRvPuestos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReportePuestos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9004 – ReportePuestos";
            this.Load += new System.EventHandler(this.FrmReportePuestos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvPuestos;
    }
}
