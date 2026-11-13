/*
 * ================================================================
 * Área   : Bancos
 * Autor  : André De Jesús Gonzalez Camey
 * Carné  : 9959-23-3117
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Define el visor del reporte de partidas enviadas a Contabilidad
 *  y las propiedades de la ventana que lo contiene.
 * ================================================================
 */
namespace CapaVista_EnlaceB.frmReportes
{
    partial class FrmReporteEnlaceContabilidad
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteEnlaceContabilidad));
            this.BancosRvReporte = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // BancosRvReporte
            // 
            this.BancosRvReporte.Location = new System.Drawing.Point(-2, -2);
            this.BancosRvReporte.Name = "BancosRvReporte";
            this.BancosRvReporte.ServerReport.BearerToken = null;
            this.BancosRvReporte.Size = new System.Drawing.Size(1229, 722);
            this.BancosRvReporte.TabIndex = 0;
            // 
            // FrmReporteEnlaceContabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 713);
            this.Controls.Add(this.BancosRvReporte);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteEnlaceContabilidad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8008 – ReporteEnlaceContabilidad";
            this.Load += new System.EventHandler(this.FrmReporteEnlaceContabilidad_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer BancosRvReporte;
    }
}
