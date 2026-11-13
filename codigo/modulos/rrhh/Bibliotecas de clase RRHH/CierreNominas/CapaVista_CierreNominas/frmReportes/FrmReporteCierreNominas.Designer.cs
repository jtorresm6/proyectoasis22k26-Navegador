using System;

namespace CapaVista_Nomina.Reportes
{
    partial class FrmReporteCierreNominas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteCierreNominas));
            this.RpReporteCierreNominas = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RpReporteCierreNominas
            // 
            this.RpReporteCierreNominas.Location = new System.Drawing.Point(0, 0);
            this.RpReporteCierreNominas.Name = "RpReporteCierreNominas";
            this.RpReporteCierreNominas.ServerReport.BearerToken = null;
            this.RpReporteCierreNominas.Size = new System.Drawing.Size(1405, 602);
            this.RpReporteCierreNominas.TabIndex = 0;
            // 
            // FrmReporteCierreNominas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 555);
            this.Controls.Add(this.RpReporteCierreNominas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteCierreNominas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "9602 - FrmReporteCierreNominas";
            this.Load += new System.EventHandler(this.FrmReporteCierreNominas_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RpReporteCierreNominas;
    }
}