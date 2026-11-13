namespace CapaVista_Liquidacion_Rrhh.frmReportes
{
    partial class FrmReporteBajaEmpleado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReporteBajaEmpleado));
            this.RrhhRdvBajaEmpleado = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // RrhhRdvBajaEmpleado
            // 
            this.RrhhRdvBajaEmpleado.Location = new System.Drawing.Point(12, 5);
            this.RrhhRdvBajaEmpleado.Name = "RrhhRdvBajaEmpleado";
            this.RrhhRdvBajaEmpleado.ServerReport.BearerToken = null;
            this.RrhhRdvBajaEmpleado.Size = new System.Drawing.Size(776, 440);
            this.RrhhRdvBajaEmpleado.TabIndex = 2;
            // 
            // FrmReporteBajaEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RrhhRdvBajaEmpleado);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReporteBajaEmpleado";
            this.Text = "FrmReporteBajaEmpleado";
            this.Load += new System.EventHandler(this.FrmReporteBajaEmpleado_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer RrhhRdvBajaEmpleado;
    }
}