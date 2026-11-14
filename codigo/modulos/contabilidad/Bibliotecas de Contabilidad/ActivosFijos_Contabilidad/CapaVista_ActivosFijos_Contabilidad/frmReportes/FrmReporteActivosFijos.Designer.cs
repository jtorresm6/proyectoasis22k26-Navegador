/* Área: Contabilidad. Autor: Dylan Hernández. Carné: 0901-23-519.
 * Fecha: 11/11/2026. Propósito: presentación de los filtros y el visor del reporte de activos fijos. */
namespace CapaVista_Contabilidad
{
    partial class FrmReporteActivosFijos
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
            this.ContabilidadLblTitulo = new System.Windows.Forms.Label();
            this.ContabilidadPnlFiltros = new Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblCorte = new System.Windows.Forms.Label();
            this.ContabilidadDtpCorte = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadLblModulo = new System.Windows.Forms.Label();
            this.ContabilidadCboModulo = new System.Windows.Forms.ComboBox();
            this.ContabilidadLblTipo = new System.Windows.Forms.Label();
            this.ContabilidadCboTipo = new System.Windows.Forms.ComboBox();
            this.ContabilidadLblAgrupar = new System.Windows.Forms.Label();
            this.ContabilidadCboAgrupar = new System.Windows.Forms.ComboBox();
            this.ContabilidadBtnGenerar = new Componentes.ClsBotonContabilidad();
            this.ContabilidadRpvReporte = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ContabilidadPnlFiltros.SuspendLayout();
            this.SuspendLayout();
            this.ContabilidadLblTitulo.Name = "ContabilidadLblTitulo";
            this.ContabilidadLblTitulo.Text = "Reporte de activos fijos";
            this.ContabilidadLblTitulo.Location = new System.Drawing.Point(24, 16);
            this.ContabilidadLblTitulo.Size = new System.Drawing.Size(1132, 38);
            this.ContabilidadLblTitulo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 20F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.ContabilidadPnlFiltros.Name = "ContabilidadPnlFiltros";
            this.ContabilidadPnlFiltros.Location = new System.Drawing.Point(24, 62);
            this.ContabilidadPnlFiltros.Size = new System.Drawing.Size(1132, 92);
            this.ContabilidadPnlFiltros.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlFiltros.ColorRelleno = System.Drawing.Color.White;
            this.ContabilidadPnlFiltros.ColorBorde = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlFiltros.Radio = 18;
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblCorte);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadDtpCorte);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblModulo);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadCboModulo);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblTipo);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadCboTipo);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblAgrupar);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadCboAgrupar);
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadBtnGenerar);
            this.ContabilidadLblCorte.Name = "ContabilidadLblCorte";
            this.ContabilidadLblCorte.Text = "Fecha de corte";
            this.ContabilidadLblCorte.Location = new System.Drawing.Point(24, 14);
            this.ContabilidadLblCorte.Size = new System.Drawing.Size(170, 24);
            this.ContabilidadLblCorte.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblCorte.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblCorte.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadDtpCorte.Name = "ContabilidadDtpCorte";
            this.ContabilidadDtpCorte.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpCorte.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadDtpCorte.Location = new System.Drawing.Point(24, 40);
            this.ContabilidadDtpCorte.Size = new System.Drawing.Size(170, 30);
            this.ContabilidadDtpCorte.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadDtpCorte.TabIndex = 0;
            this.ContabilidadLblModulo.Name = "ContabilidadLblModulo";
            this.ContabilidadLblModulo.Text = "Módulo";
            this.ContabilidadLblModulo.Location = new System.Drawing.Point(218, 14);
            this.ContabilidadLblModulo.Size = new System.Drawing.Size(220, 24);
            this.ContabilidadLblModulo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblModulo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblModulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadCboModulo.Name = "ContabilidadCboModulo";
            this.ContabilidadCboModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboModulo.Location = new System.Drawing.Point(218, 40);
            this.ContabilidadCboModulo.Size = new System.Drawing.Size(220, 30);
            this.ContabilidadCboModulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboModulo.TabIndex = 1;
            this.ContabilidadLblTipo.Name = "ContabilidadLblTipo";
            this.ContabilidadLblTipo.Text = "Tipo de activo";
            this.ContabilidadLblTipo.Location = new System.Drawing.Point(462, 14);
            this.ContabilidadLblTipo.Size = new System.Drawing.Size(290, 24);
            this.ContabilidadLblTipo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTipo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblTipo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadCboTipo.Name = "ContabilidadCboTipo";
            this.ContabilidadCboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboTipo.Location = new System.Drawing.Point(462, 40);
            this.ContabilidadCboTipo.Size = new System.Drawing.Size(290, 30);
            this.ContabilidadCboTipo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboTipo.TabIndex = 2;
            this.ContabilidadLblAgrupar.Name = "ContabilidadLblAgrupar";
            this.ContabilidadLblAgrupar.Text = "Agrupar por";
            this.ContabilidadLblAgrupar.Location = new System.Drawing.Point(776, 14);
            this.ContabilidadLblAgrupar.Size = new System.Drawing.Size(150, 24);
            this.ContabilidadLblAgrupar.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblAgrupar.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblAgrupar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadCboAgrupar.Name = "ContabilidadCboAgrupar";
            this.ContabilidadCboAgrupar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboAgrupar.Location = new System.Drawing.Point(776, 40);
            this.ContabilidadCboAgrupar.Size = new System.Drawing.Size(150, 30);
            this.ContabilidadCboAgrupar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboAgrupar.TabIndex = 3;
            this.ContabilidadBtnGenerar.Name = "ContabilidadBtnGenerar";
            this.ContabilidadBtnGenerar.Text = "Generar reporte";
            this.ContabilidadBtnGenerar.Location = new System.Drawing.Point(950, 30);
            this.ContabilidadBtnGenerar.Size = new System.Drawing.Size(162, 42);
            this.ContabilidadBtnGenerar.Estilo = Componentes.EstiloBotonContabilidad.Principal;
            this.ContabilidadBtnGenerar.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_aplicar;
            this.ContabilidadBtnGenerar.TabIndex = 4;
            this.ContabilidadRpvReporte.Name = "ContabilidadRpvReporte";
            this.ContabilidadRpvReporte.Location = new System.Drawing.Point(24, 168);
            this.ContabilidadRpvReporte.Size = new System.Drawing.Size(1132, 532);
            this.ContabilidadRpvReporte.ServerReport.BearerToken = null;
            this.ContabilidadRpvReporte.TabIndex = 5;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.ClientSize = new System.Drawing.Size(1180, 720);
            this.Controls.Add(this.ContabilidadLblTitulo);
            this.Controls.Add(this.ContabilidadPnlFiltros);
            this.Controls.Add(this.ContabilidadRpvReporte);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmReporteActivosFijos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10003 - ReporteActivosFijos";
            this.ContabilidadPnlFiltros.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label ContabilidadLblTitulo;
        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlFiltros;
        private System.Windows.Forms.Label ContabilidadLblCorte;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpCorte;
        private System.Windows.Forms.Label ContabilidadLblModulo;
        private System.Windows.Forms.ComboBox ContabilidadCboModulo;
        private System.Windows.Forms.Label ContabilidadLblTipo;
        private System.Windows.Forms.ComboBox ContabilidadCboTipo;
        private System.Windows.Forms.Label ContabilidadLblAgrupar;
        private System.Windows.Forms.ComboBox ContabilidadCboAgrupar;
        private Componentes.ClsBotonContabilidad ContabilidadBtnGenerar;
        private Microsoft.Reporting.WinForms.ReportViewer ContabilidadRpvReporte;
    }
}
