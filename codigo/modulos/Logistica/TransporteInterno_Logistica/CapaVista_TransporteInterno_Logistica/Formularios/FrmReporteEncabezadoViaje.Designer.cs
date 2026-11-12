namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmReporteEncabezadoViaje
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
            this.LogisticaRvReporteEncabezadoViaje = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // LogisticaRvReporteEncabezadoViaje
            // 
            this.LogisticaRvReporteEncabezadoViaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogisticaRvReporteEncabezadoViaje.Location = new System.Drawing.Point(0, 0);
            this.LogisticaRvReporteEncabezadoViaje.Name = "LogisticaRvReporteEncabezadoViaje";
            this.LogisticaRvReporteEncabezadoViaje.ServerReport.BearerToken = null;
            this.LogisticaRvReporteEncabezadoViaje.Size = new System.Drawing.Size(800, 450);
            this.LogisticaRvReporteEncabezadoViaje.TabIndex = 0;
            // 
            // FrmReporteEncabezadoViaje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LogisticaRvReporteEncabezadoViaje);
            this.Name = "FrmReporteEncabezadoViaje";
            this.Text = "FrmReporteEncabezadoViaje";
            this.Load += new System.EventHandler(this.FrmReporteEncabezadoViaje_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer LogisticaRvReporteEncabezadoViaje;
    }
}