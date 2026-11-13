/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 11/11/2026
 * ================================================================
 * Propósito :
 *  Define el visor del reporte de monedas y las propiedades
 *  de la ventana que lo contiene.
 * ================================================================
 */
namespace CapaVista_GeneracionPagos.frmReportes
{
    partial class FrmReporteMoneda
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteMoneda));
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
            // FrmReporteMoneda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 713);
            this.Controls.Add(this.BancosRvReporte);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmReporteMoneda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8001–FrmReporteMoneda";
            this.Load += new System.EventHandler(this.FrmReporteMoneda_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer BancosRvReporte;
    }
}
