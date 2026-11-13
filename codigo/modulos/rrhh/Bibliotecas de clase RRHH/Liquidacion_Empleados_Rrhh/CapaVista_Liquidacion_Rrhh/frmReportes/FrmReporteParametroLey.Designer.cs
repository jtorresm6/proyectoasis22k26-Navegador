namespace CapaVista_Liquidacion_Rrhh.frmReportes
{
    partial class FrmReporteParametroLey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteParametroLey));
            this.RrhhRdvParametroLey = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRdvParametroLey
            // 
            this.RrhhRdvParametroLey.Location = new System.Drawing.Point(12, 5);
            this.RrhhRdvParametroLey.Name = "RrhhRdvParametroLey";
            this.RrhhRdvParametroLey.ServerReport.BearerToken = null;
            this.RrhhRdvParametroLey.Size = new System.Drawing.Size(776, 440);
            this.RrhhRdvParametroLey.TabIndex = 1;
            this.RrhhRdvParametroLey.Load += new System.EventHandler(this.RrhhRdvParametroLey_Load);
            // 
            // FrmReporteParametroLey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RrhhRdvParametroLey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReporteParametroLey";
            this.Text = "FrmReporteParametroLey";
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRdvParametroLey;
    }
}