namespace CapaVista_Empleado.frmReportes
{
    partial class FrmReporteAsigPuestos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteAsigPuestos));
            this.RrhhRvAsigPuestos = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvAsigPuestos
            // 
            this.RrhhRvAsigPuestos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvAsigPuestos.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvAsigPuestos.Name = "RrhhRvAsigPuestos";
            this.RrhhRvAsigPuestos.ServerReport.BearerToken = null;
            this.RrhhRvAsigPuestos.Size = new System.Drawing.Size(1186, 507);
            this.RrhhRvAsigPuestos.TabIndex = 0;
            // 
            // FrmReporteAsigPuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 507);
            this.Controls.Add(this.RrhhRvAsigPuestos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteAsigPuestos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9006 – ReporteAsigPuestos";
            this.Load += new System.EventHandler(this.FrmReporteAsigPuestos_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvAsigPuestos;
    }
}
