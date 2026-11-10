/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 28/10/2026. Propósito: presentación del visor del reporte de estados financieros. */
namespace CapaVista_Contabilidad
{
    partial class FrmReporteEstadoFinanciero
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
            this.ContabilidadRpvReporte = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            this.ContabilidadRpvReporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContabilidadRpvReporte.Location = new System.Drawing.Point(0, 0);
            this.ContabilidadRpvReporte.Name = "ContabilidadRpvReporte";
            this.ContabilidadRpvReporte.ServerReport.BearerToken = null;
            this.ContabilidadRpvReporte.Size = new System.Drawing.Size(1100, 720);
            this.ContabilidadRpvReporte.TabIndex = 0;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.Controls.Add(this.ContabilidadRpvReporte);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmReporteEstadoFinanciero";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10005 - ReporteEstadoFinanciero";
            this.ResumeLayout(false);
        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer ContabilidadRpvReporte;
    }
}
