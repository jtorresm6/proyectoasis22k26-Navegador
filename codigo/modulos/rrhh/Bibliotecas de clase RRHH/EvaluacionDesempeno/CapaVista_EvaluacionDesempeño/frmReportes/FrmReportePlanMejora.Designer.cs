namespace CapaVista_EvaluacionDesempeño.frmReportes
{
    partial class FrmReportePlanMejora
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
            this.RrhhRvPlanMejora = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            //
            // RrhhRvPlanMejora
            //
            this.RrhhRvPlanMejora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvPlanMejora.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvPlanMejora.Name = "RrhhRvPlanMejora";
            this.RrhhRvPlanMejora.ServerReport.BearerToken = null;
            this.RrhhRvPlanMejora.Size = new System.Drawing.Size(1186, 600);
            this.RrhhRvPlanMejora.TabIndex = 0;
            //
            // FrmReportePlanMejora
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 600);
            this.Controls.Add(this.RrhhRvPlanMejora);
            this.Name = "FrmReportePlanMejora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "9403 – Reporte de Planes de Mejora";
            this.Load += new System.EventHandler(this.FrmReportePlanMejora_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvPlanMejora;
    }
}
