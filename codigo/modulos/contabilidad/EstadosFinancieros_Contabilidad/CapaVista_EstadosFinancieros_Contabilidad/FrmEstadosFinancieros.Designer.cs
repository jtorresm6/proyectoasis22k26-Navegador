/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: presentación de estado de resultados de ejemplo. */
using System.Windows.Forms;
using CapaVista_Contabilidad.Componentes;
using CapaVista_Contabilidad.Properties;

namespace CapaVista_Contabilidad
{
    partial class FrmEstadosFinancieros
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            if (Disposing && _Resultado != null) _Resultado.Dispose();
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ContabilidadLblTitulo = new System.Windows.Forms.Label();
            this.ContabilidadPnlFiltros = new Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblDesde = new System.Windows.Forms.Label();
            this.ContabilidadDtpDesde = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadLblHasta = new System.Windows.Forms.Label();
            this.ContabilidadDtpHasta = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadBtnConsultar = new Componentes.ClsBotonContabilidad();
            this.ContabilidadLblFiltro = new System.Windows.Forms.Label();
            this.ContabilidadPnlInforme = new Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblPeriodo = new System.Windows.Forms.Label();
            this.ContabilidadDgvResultado = new Componentes.ClsTablaContabilidad();
            this.ContabilidadColRubro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColCuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColImporte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadLblEstado = new System.Windows.Forms.Label();
            this.ContabilidadLblResumen = new System.Windows.Forms.Label();
            System.Windows.Forms.DataGridViewCellStyle EstiloContabilidadColImporte = new System.Windows.Forms.DataGridViewCellStyle();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvResultado)).BeginInit();
            this.ContabilidadPnlFiltros.SuspendLayout();
            this.ContabilidadPnlInforme.SuspendLayout();
            this.SuspendLayout();
            this.ContabilidadLblTitulo.Name = "ContabilidadLblTitulo";
            this.Controls.Add(this.ContabilidadLblTitulo);
            this.ContabilidadLblTitulo.Text = "Estados financieros";
            this.ContabilidadLblTitulo.Location = new System.Drawing.Point(24, 22);
            this.ContabilidadLblTitulo.Size = new System.Drawing.Size(1132, 38);
            this.ContabilidadLblTitulo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 20F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.ContabilidadPnlFiltros.Name = "ContabilidadPnlFiltros";
            this.Controls.Add(this.ContabilidadPnlFiltros);
            this.ContabilidadPnlFiltros.Location = new System.Drawing.Point(24, 76);
            this.ContabilidadPnlFiltros.Size = new System.Drawing.Size(1132, 112);
            this.ContabilidadPnlFiltros.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlFiltros.ColorRelleno = System.Drawing.Color.White;
            this.ContabilidadPnlFiltros.ColorBorde = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlFiltros.Radio = 18;
            this.ContabilidadLblDesde.Name = "ContabilidadLblDesde";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblDesde);
            this.ContabilidadLblDesde.Text = "Desde";
            this.ContabilidadLblDesde.Location = new System.Drawing.Point(24, 20);
            this.ContabilidadLblDesde.Size = new System.Drawing.Size(250, 24);
            this.ContabilidadLblDesde.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblDesde.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblDesde.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadDtpDesde.Name = "ContabilidadDtpDesde";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadDtpDesde);
            this.ContabilidadDtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpDesde.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadDtpDesde.Location = new System.Drawing.Point(24, 46);
            this.ContabilidadDtpDesde.Size = new System.Drawing.Size(250, 30);
            this.ContabilidadDtpDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadLblHasta.Name = "ContabilidadLblHasta";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblHasta);
            this.ContabilidadLblHasta.Text = "Hasta";
            this.ContabilidadLblHasta.Location = new System.Drawing.Point(298, 20);
            this.ContabilidadLblHasta.Size = new System.Drawing.Size(250, 24);
            this.ContabilidadLblHasta.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblHasta.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblHasta.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadDtpHasta.Name = "ContabilidadDtpHasta";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadDtpHasta);
            this.ContabilidadDtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpHasta.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadDtpHasta.Location = new System.Drawing.Point(298, 46);
            this.ContabilidadDtpHasta.Size = new System.Drawing.Size(250, 30);
            this.ContabilidadDtpHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnConsultar.Name = "ContabilidadBtnConsultar";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadBtnConsultar);
            this.ContabilidadBtnConsultar.Text = "Ver resultados";
            this.ContabilidadBtnConsultar.Location = new System.Drawing.Point(574, 38);
            this.ContabilidadBtnConsultar.Size = new System.Drawing.Size(220, 42);
            this.ContabilidadBtnConsultar.Estilo = Componentes.EstiloBotonContabilidad.Acento;
            this.ContabilidadLblFiltro.Name = "ContabilidadLblFiltro";
            this.ContabilidadPnlFiltros.Controls.Add(this.ContabilidadLblFiltro);
            this.ContabilidadLblFiltro.Text = "Solo pólizas validadas y aplicadas";
            this.ContabilidadLblFiltro.Location = new System.Drawing.Point(820, 26);
            this.ContabilidadLblFiltro.Size = new System.Drawing.Size(290, 60);
            this.ContabilidadLblFiltro.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblFiltro.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblFiltro.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlInforme.Name = "ContabilidadPnlInforme";
            this.Controls.Add(this.ContabilidadPnlInforme);
            this.ContabilidadPnlInforme.Location = new System.Drawing.Point(24, 204);
            this.ContabilidadPnlInforme.Size = new System.Drawing.Size(1132, 342);
            this.ContabilidadPnlInforme.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlInforme.ColorRelleno = System.Drawing.Color.White;
            this.ContabilidadPnlInforme.ColorBorde = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlInforme.Radio = 18;
            this.ContabilidadLblPeriodo.Name = "ContabilidadLblPeriodo";
            this.ContabilidadPnlInforme.Controls.Add(this.ContabilidadLblPeriodo);
            this.ContabilidadLblPeriodo.Text = "";
            this.ContabilidadLblPeriodo.Location = new System.Drawing.Point(20, 14);
            this.ContabilidadLblPeriodo.Size = new System.Drawing.Size(1092, 24);
            this.ContabilidadLblPeriodo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadDgvResultado.Name = "ContabilidadDgvResultado";
            this.ContabilidadPnlInforme.Controls.Add(this.ContabilidadDgvResultado);
            this.ContabilidadDgvResultado.Location = new System.Drawing.Point(20, 52);
            this.ContabilidadDgvResultado.Size = new System.Drawing.Size(1092, 180);
            this.ContabilidadDgvResultado.AutoGenerateColumns = false;
            this.ContabilidadDgvResultado.ReadOnly = true;
            this.ContabilidadDgvResultado.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.ContabilidadColRubro.Name = "ContabilidadColRubro";
            this.ContabilidadColRubro.DataPropertyName = "Rubro";
            this.ContabilidadColRubro.HeaderText = "Rubro";
            this.ContabilidadColRubro.MinimumWidth = 90;
            this.ContabilidadColCodigo.Name = "ContabilidadColCodigo";
            this.ContabilidadColCodigo.DataPropertyName = "Codigo";
            this.ContabilidadColCodigo.HeaderText = "Código de cuenta";
            this.ContabilidadColCodigo.MinimumWidth = 90;
            this.ContabilidadColCuenta.Name = "ContabilidadColCuenta";
            this.ContabilidadColCuenta.DataPropertyName = "Cuenta";
            this.ContabilidadColCuenta.HeaderText = "Cuenta";
            this.ContabilidadColCuenta.MinimumWidth = 90;
            this.ContabilidadColImporte.Name = "ContabilidadColImporte";
            this.ContabilidadColImporte.DataPropertyName = "Importe";
            this.ContabilidadColImporte.HeaderText = "Importe (Q)";
            this.ContabilidadColImporte.MinimumWidth = 90;
            EstiloContabilidadColImporte.Format = "N2";
            EstiloContabilidadColImporte.FormatProvider = new System.Globalization.CultureInfo("es-GT");
            EstiloContabilidadColImporte.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ContabilidadColImporte.DefaultCellStyle = EstiloContabilidadColImporte;
            this.ContabilidadDgvResultado.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.ContabilidadColRubro, this.ContabilidadColCodigo, this.ContabilidadColCuenta, this.ContabilidadColImporte });
            this.ContabilidadLblEstado.Name = "ContabilidadLblEstado";
            this.ContabilidadPnlInforme.Controls.Add(this.ContabilidadLblEstado);
            this.ContabilidadLblEstado.Text = "";
            this.ContabilidadLblEstado.Location = new System.Drawing.Point(20, 248);
            this.ContabilidadLblEstado.Size = new System.Drawing.Size(1092, 64);
            this.ContabilidadLblEstado.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblEstado.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblEstado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblResumen.Name = "ContabilidadLblResumen";
            this.Controls.Add(this.ContabilidadLblResumen);
            this.ContabilidadLblResumen.Text = "";
            this.ContabilidadLblResumen.Location = new System.Drawing.Point(26, 558);
            this.ContabilidadLblResumen.Size = new System.Drawing.Size(1130, 24);
            this.ContabilidadLblResumen.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblResumen.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadLblResumen.Font = new System.Drawing.Font("Tahoma", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmEstadosFinancieros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10005 - EstadosFinancieros";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvResultado)).EndInit();
            this.ContabilidadPnlFiltros.ResumeLayout(false);
            this.ContabilidadPnlFiltros.PerformLayout();
            this.ContabilidadPnlInforme.ResumeLayout(false);
            this.ContabilidadPnlInforme.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label ContabilidadLblTitulo;
        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlFiltros;
        private System.Windows.Forms.Label ContabilidadLblDesde;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpDesde;
        private System.Windows.Forms.Label ContabilidadLblHasta;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpHasta;
        private Componentes.ClsBotonContabilidad ContabilidadBtnConsultar;
        private System.Windows.Forms.Label ContabilidadLblFiltro;
        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlInforme;
        private System.Windows.Forms.Label ContabilidadLblPeriodo;
        private Componentes.ClsTablaContabilidad ContabilidadDgvResultado;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColRubro;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColCuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColImporte;
        private System.Windows.Forms.Label ContabilidadLblEstado;
        private System.Windows.Forms.Label ContabilidadLblResumen;
    }
}
