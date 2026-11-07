namespace CapaVista_Empleado.frmReportes
{
    partial class FrmReporteJornadas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteJornadas));
            this.RrhhRvJornadas = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvJornadas
            // 
            this.RrhhRvJornadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvJornadas.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvJornadas.Name = "RrhhRvJornadas";
            this.RrhhRvJornadas.ServerReport.BearerToken = null;
            this.RrhhRvJornadas.Size = new System.Drawing.Size(1186, 507);
            this.RrhhRvJornadas.TabIndex = 0;
            // 
            // FrmReporteJornadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 507);
            this.Controls.Add(this.RrhhRvJornadas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteJornadas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9005 – ReporteJornadas";
            this.Load += new System.EventHandler(this.FrmReporteJornadas_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvJornadas;
    }
}
