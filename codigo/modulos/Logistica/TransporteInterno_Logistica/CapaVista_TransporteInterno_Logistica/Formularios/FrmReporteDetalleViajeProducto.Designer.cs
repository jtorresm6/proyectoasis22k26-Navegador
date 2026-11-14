namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmReporteDetalleViajeProducto
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
            this.LogisticaRvReporteDetalleViajeProducto = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // LogisticaRvReporteDetalleViajeProducto
            // 
            this.LogisticaRvReporteDetalleViajeProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogisticaRvReporteDetalleViajeProducto.Location = new System.Drawing.Point(0, 0);
            this.LogisticaRvReporteDetalleViajeProducto.Name = "LogisticaRvReporteDetalleViajeProducto";
            this.LogisticaRvReporteDetalleViajeProducto.ServerReport.BearerToken = null;
            this.LogisticaRvReporteDetalleViajeProducto.Size = new System.Drawing.Size(800, 450);
            this.LogisticaRvReporteDetalleViajeProducto.TabIndex = 0;
            // 
            // FrmReporteDetalleViajeProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LogisticaRvReporteDetalleViajeProducto);
            this.Name = "FrmReporteDetalleViajeProducto";
            this.Text = "FrmReporteDetalleViajeProducto";
            this.Load += new System.EventHandler(this.FrmReporteDetalleViajeProducto_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer LogisticaRvReporteDetalleViajeProducto;
    }
}