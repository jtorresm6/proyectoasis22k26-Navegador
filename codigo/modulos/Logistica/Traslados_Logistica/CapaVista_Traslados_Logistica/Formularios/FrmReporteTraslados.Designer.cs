namespace CapaVista_Traslados_Logistica.Formularios
{
    partial class FrmReporteTraslados
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
            this.LogisticaRvReporteTraslados = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // LogisticaRvReporteTraslados
            // 
            this.LogisticaRvReporteTraslados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogisticaRvReporteTraslados.Location = new System.Drawing.Point(0, 0);
            this.LogisticaRvReporteTraslados.Name = "LogisticaRvReporteTraslados";
            this.LogisticaRvReporteTraslados.ServerReport.BearerToken = null;
            this.LogisticaRvReporteTraslados.Size = new System.Drawing.Size(1471, 973);
            this.LogisticaRvReporteTraslados.TabIndex = 0;
            // 
            // FrmReporteTraslados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1471, 973);
            this.Controls.Add(this.LogisticaRvReporteTraslados);
            this.MaximizeBox = false;
            this.Name = "FrmReporteTraslados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmReporteTraslados";
            this.Load += new System.EventHandler(this.FrmReporteTraslados_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer LogisticaRvReporteTraslados;
    }
}