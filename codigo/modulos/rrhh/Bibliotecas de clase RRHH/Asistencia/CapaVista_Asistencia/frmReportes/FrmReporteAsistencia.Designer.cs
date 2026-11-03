/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana del reporte de asistencias.
 * ===================================================================
*/

namespace CapaVista_Asistencia.frmReportes
{
    partial class FrmReporteAsistencia
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
            this.RrhhRvAsistencia = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRvAsistencia
            // 
            this.RrhhRvAsistencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RrhhRvAsistencia.Location = new System.Drawing.Point(0, 0);
            this.RrhhRvAsistencia.Name = "RrhhRvAsistencia";
            this.RrhhRvAsistencia.ServerReport.BearerToken = null;
            this.RrhhRvAsistencia.Size = new System.Drawing.Size(1186, 600);
            this.RrhhRvAsistencia.TabIndex = 0;
            // 
            // FrmReporteAsistencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 600);
            this.Controls.Add(this.RrhhRvAsistencia);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Asistencia.Properties.Resources.icono_rrhh;
            this.MaximizeBox = false;
            this.Name = "FrmReporteAsistencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9101 – Reporte de Asistencia";
            this.Load += new System.EventHandler(this.FrmReporteAsistencia_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRvAsistencia;
    }
}
