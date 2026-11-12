namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmReporteTelemetriaTemperatura
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
            this.LogisticaRvReporteTelemetriaTemperatura = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // LogisticaRvReporteTelemetriaTemperatura
            // 
            this.LogisticaRvReporteTelemetriaTemperatura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogisticaRvReporteTelemetriaTemperatura.Location = new System.Drawing.Point(0, 0);
            this.LogisticaRvReporteTelemetriaTemperatura.Name = "LogisticaRvReporteTelemetriaTemperatura";
            this.LogisticaRvReporteTelemetriaTemperatura.ServerReport.BearerToken = null;
            this.LogisticaRvReporteTelemetriaTemperatura.Size = new System.Drawing.Size(800, 450);
            this.LogisticaRvReporteTelemetriaTemperatura.TabIndex = 0;
            // 
            // FrmReporteTelemetriaTemperatura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LogisticaRvReporteTelemetriaTemperatura);
            this.Name = "FrmReporteTelemetriaTemperatura";
            this.Text = "FrmReporteTelemetriaTemperatura";
            this.Load += new System.EventHandler(this.FrmReporteTelemetriaTemperatura_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer LogisticaRvReporteTelemetriaTemperatura;
    }
}