namespace CapaVista_Empleado.frmReportes
{
    partial class FrmReporteDepartamentos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteDepartamentos));
            this.RrhhRvDepartamentos = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvDepartamentos
            // 
            this.RrhhRvDepartamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvDepartamentos.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvDepartamentos.Name = "RrhhRvDepartamentos";
            this.RrhhRvDepartamentos.ServerReport.BearerToken = null;
            this.RrhhRvDepartamentos.Size = new System.Drawing.Size(1186, 507);
            this.RrhhRvDepartamentos.TabIndex = 0;
            // 
            // FrmReporteDepartamentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 507);
            this.Controls.Add(this.RrhhRvDepartamentos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteDepartamentos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9002 – ReporteDepartamentos";
            this.Load += new System.EventHandler(this.FrmReporteDepartamentos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvDepartamentos;
    }
}
