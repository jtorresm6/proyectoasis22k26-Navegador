namespace CapaVista_Liquidacion_Rrhh.frmReportes
{
    partial class FrmReporteMotivoBaja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteMotivoBaja));
            this.RrhhRdvMotivoBaja = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRdvMotivoBaja
            // 
            this.RrhhRdvMotivoBaja.Location = new System.Drawing.Point(0, 0);
            this.RrhhRdvMotivoBaja.Name = "RrhhRdvMotivoBaja";
            this.RrhhRdvMotivoBaja.ServerReport.BearerToken = null;
            this.RrhhRdvMotivoBaja.Size = new System.Drawing.Size(776, 440);
            this.RrhhRdvMotivoBaja.TabIndex = 0;
            // 
            // FrmReporteMotivoBaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RrhhRdvMotivoBaja);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteMotivoBaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9702 – ReporteMotivoBaja";
            this.Load += new System.EventHandler(this.FrmReporteMotivoBaja_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRdvMotivoBaja;
    }
}