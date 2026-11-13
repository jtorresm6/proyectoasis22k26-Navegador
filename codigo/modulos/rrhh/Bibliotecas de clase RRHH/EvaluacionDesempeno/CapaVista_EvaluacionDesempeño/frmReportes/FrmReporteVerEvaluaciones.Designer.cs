namespace CapaVista_EvaluacionDesempeño.frmReportes
{
    partial class FrmReporteVerEvaluaciones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RrhhRvVerEvaluaciones = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            //
            // RrhhRvVerEvaluaciones
            //
            this.RrhhRvVerEvaluaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvVerEvaluaciones.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvVerEvaluaciones.Name = "RrhhRvVerEvaluaciones";
            this.RrhhRvVerEvaluaciones.ServerReport.BearerToken = null;
            this.RrhhRvVerEvaluaciones.Size = new System.Drawing.Size(1186, 600);
            this.RrhhRvVerEvaluaciones.TabIndex = 0;
            //
            // FrmReporteVerEvaluaciones
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 600);
            this.Controls.Add(this.RrhhRvVerEvaluaciones);
            this.Name = "FrmReporteVerEvaluaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "9404 – Reporte de Mis Evaluaciones";
            this.Load += new System.EventHandler(this.FrmReporteVerEvaluaciones_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvVerEvaluaciones;
    }
}
