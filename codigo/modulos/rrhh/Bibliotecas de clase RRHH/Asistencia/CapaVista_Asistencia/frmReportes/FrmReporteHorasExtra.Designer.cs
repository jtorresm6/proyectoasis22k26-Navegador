/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana del reporte de horas extra.
 * ===================================================================
*/

namespace CapaVista_Asistencia.frmReportes
{
    partial class FrmReporteHorasExtra
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

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.RrhhRvHorasExtra = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvHorasExtra
            // 
            this.RrhhRvHorasExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvHorasExtra.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvHorasExtra.Name = "RrhhRvHorasExtra";
            this.RrhhRvHorasExtra.ServerReport.BearerToken = null;
            this.RrhhRvHorasExtra.Size = new System.Drawing.Size(1186, 600);
            this.RrhhRvHorasExtra.TabIndex = 0;
            // 
            // FrmReporteHorasExtra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 600);
            this.Controls.Add(this.RrhhRvHorasExtra);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Asistencia.Properties.Resources.icono_rrhh;
            this.MaximizeBox = false;
            this.Name = "FrmReporteHorasExtra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9102 – Reporte de Horas Extra";
            this.Load += new System.EventHandler(this.FrmReporteHorasExtra_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvHorasExtra;
    }
}
