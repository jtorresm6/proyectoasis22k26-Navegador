/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Oskar Cermeño
 * Carné : 0901-23-15379
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Diseño de la ventana de encabezados de pólizas: filtros, tabla de
 *  encabezados y actualización de saldos en batch.
 * ===================================================================
*/

namespace CapaVista_Contabilidad
{
    partial class FrmPolizas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ContabilidadPnlPrincipal = new CapaVista_Contabilidad.Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblTitulo = new System.Windows.Forms.Label();
            this.ContabilidadLblModo = new System.Windows.Forms.Label();
            this.ContabilidadLblPeriodo = new System.Windows.Forms.Label();
            this.ContabilidadCboPeriodo = new System.Windows.Forms.ComboBox();
            this.ContabilidadLblEstado = new System.Windows.Forms.Label();
            this.ContabilidadCboEstado = new System.Windows.Forms.ComboBox();
            this.ContabilidadChkPendientes = new System.Windows.Forms.CheckBox();
            this.ContabilidadBtnNueva = new CapaVista_Contabilidad.Componentes.ClsBotonContabilidad();
            this.ContabilidadBtnAbrir = new CapaVista_Contabilidad.Componentes.ClsBotonContabilidad();
            this.ContabilidadDgvEncabezados = new CapaVista_Contabilidad.Componentes.ClsTablaContabilidad();
            this.ContabilidadLblTotal = new System.Windows.Forms.Label();
            this.ContabilidadPnlBatch = new CapaVista_Contabilidad.Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblBatch = new System.Windows.Forms.Label();
            this.ContabilidadLblTipoPeriodo = new System.Windows.Forms.Label();
            this.ContabilidadCboTipoPeriodo = new System.Windows.Forms.ComboBox();
            this.ContabilidadLblDesde = new System.Windows.Forms.Label();
            this.ContabilidadDtpDesde = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadLblHasta = new System.Windows.Forms.Label();
            this.ContabilidadDtpHasta = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadBtnActualizarSaldos = new CapaVista_Contabilidad.Componentes.ClsBotonContabilidad();
            this.ContabilidadPnlPrincipal.SuspendLayout();
            this.ContabilidadPnlBatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvEncabezados)).BeginInit();
            this.SuspendLayout();
            this.ContabilidadPnlPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadLblModo);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadLblTitulo);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadLblPeriodo);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadCboPeriodo);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadLblEstado);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadCboEstado);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadChkPendientes);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadBtnNueva);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadBtnAbrir);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadDgvEncabezados);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadLblTotal);
            this.ContabilidadPnlPrincipal.Controls.Add(this.ContabilidadPnlBatch);
            this.ContabilidadPnlPrincipal.Location = new System.Drawing.Point(24, 24);
            this.ContabilidadPnlPrincipal.Name = "ContabilidadPnlPrincipal";
            this.ContabilidadPnlPrincipal.Size = new System.Drawing.Size(1132, 640);
            this.ContabilidadPnlPrincipal.TabIndex = 0;
            this.ContabilidadLblModo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblModo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ContabilidadLblModo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(143)))), ((int)(((byte)(62)))));
            this.ContabilidadLblModo.Location = new System.Drawing.Point(898, 22);
            this.ContabilidadLblModo.Name = "ContabilidadLblModo";
            this.ContabilidadLblModo.Size = new System.Drawing.Size(210, 28);
            this.ContabilidadLblModo.TabIndex = 0;
            this.ContabilidadLblModo.Text = "Actualización: ● BATCH";
            this.ContabilidadLblModo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ContabilidadLblTitulo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ContabilidadLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblTitulo.Location = new System.Drawing.Point(24, 18);
            this.ContabilidadLblTitulo.Name = "ContabilidadLblTitulo";
            this.ContabilidadLblTitulo.Size = new System.Drawing.Size(600, 36);
            this.ContabilidadLblTitulo.TabIndex = 0;
            this.ContabilidadLblTitulo.Text = "Encabezados de pólizas";
            this.ContabilidadLblPeriodo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblPeriodo.Location = new System.Drawing.Point(24, 66);
            this.ContabilidadLblPeriodo.Name = "ContabilidadLblPeriodo";
            this.ContabilidadLblPeriodo.Size = new System.Drawing.Size(160, 22);
            this.ContabilidadLblPeriodo.TabIndex = 0;
            this.ContabilidadLblPeriodo.Text = "Período contable";
            this.ContabilidadCboPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboPeriodo.FormattingEnabled = true;
            this.ContabilidadCboPeriodo.Location = new System.Drawing.Point(24, 92);
            this.ContabilidadCboPeriodo.Name = "ContabilidadCboPeriodo";
            this.ContabilidadCboPeriodo.Size = new System.Drawing.Size(160, 31);
            this.ContabilidadCboPeriodo.TabIndex = 1;
            this.ContabilidadLblEstado.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblEstado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblEstado.Location = new System.Drawing.Point(204, 66);
            this.ContabilidadLblEstado.Name = "ContabilidadLblEstado";
            this.ContabilidadLblEstado.Size = new System.Drawing.Size(180, 22);
            this.ContabilidadLblEstado.TabIndex = 0;
            this.ContabilidadLblEstado.Text = "Estado";
            this.ContabilidadCboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboEstado.FormattingEnabled = true;
            this.ContabilidadCboEstado.Location = new System.Drawing.Point(204, 92);
            this.ContabilidadCboEstado.Name = "ContabilidadCboEstado";
            this.ContabilidadCboEstado.Size = new System.Drawing.Size(180, 31);
            this.ContabilidadCboEstado.TabIndex = 2;
            this.ContabilidadChkPendientes.BackColor = System.Drawing.Color.White;
            this.ContabilidadChkPendientes.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadChkPendientes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadChkPendientes.Location = new System.Drawing.Point(404, 94);
            this.ContabilidadChkPendientes.Name = "ContabilidadChkPendientes";
            this.ContabilidadChkPendientes.Size = new System.Drawing.Size(250, 28);
            this.ContabilidadChkPendientes.TabIndex = 3;
            this.ContabilidadChkPendientes.Text = "Solo pendientes de batch";
            this.ContabilidadChkPendientes.UseVisualStyleBackColor = false;
            this.ContabilidadBtnNueva.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContabilidadBtnNueva.Estilo = CapaVista_Contabilidad.Componentes.EstiloBotonContabilidad.Acento;
            this.ContabilidadBtnNueva.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ContabilidadBtnNueva.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnNueva.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_mas;
            this.ContabilidadBtnNueva.Location = new System.Drawing.Point(812, 86);
            this.ContabilidadBtnNueva.Name = "ContabilidadBtnNueva";
            this.ContabilidadBtnNueva.Size = new System.Drawing.Size(150, 42);
            this.ContabilidadBtnNueva.TabIndex = 4;
            this.ContabilidadBtnNueva.Text = "Nueva póliza";
            this.ContabilidadBtnNueva.UseVisualStyleBackColor = false;
            this.ContabilidadBtnAbrir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContabilidadBtnAbrir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ContabilidadBtnAbrir.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnAbrir.Location = new System.Drawing.Point(974, 86);
            this.ContabilidadBtnAbrir.Name = "ContabilidadBtnAbrir";
            this.ContabilidadBtnAbrir.Size = new System.Drawing.Size(134, 42);
            this.ContabilidadBtnAbrir.TabIndex = 5;
            this.ContabilidadBtnAbrir.Text = "Abrir póliza";
            this.ContabilidadBtnAbrir.UseVisualStyleBackColor = false;
            this.ContabilidadDgvEncabezados.AllowUserToAddRows = false;
            this.ContabilidadDgvEncabezados.AllowUserToDeleteRows = false;
            this.ContabilidadDgvEncabezados.AllowUserToResizeRows = false;
            this.ContabilidadDgvEncabezados.Location = new System.Drawing.Point(24, 142);
            this.ContabilidadDgvEncabezados.MultiSelect = false;
            this.ContabilidadDgvEncabezados.Name = "ContabilidadDgvEncabezados";
            this.ContabilidadDgvEncabezados.ReadOnly = true;
            this.ContabilidadDgvEncabezados.RowHeadersVisible = false;
            this.ContabilidadDgvEncabezados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ContabilidadDgvEncabezados.Size = new System.Drawing.Size(1084, 330);
            this.ContabilidadDgvEncabezados.TabIndex = 6;
            this.ContabilidadLblTotal.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTotal.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(104)))), ((int)(((byte)(125)))));
            this.ContabilidadLblTotal.Location = new System.Drawing.Point(24, 478);
            this.ContabilidadLblTotal.Name = "ContabilidadLblTotal";
            this.ContabilidadLblTotal.Size = new System.Drawing.Size(1084, 22);
            this.ContabilidadLblTotal.TabIndex = 0;
            this.ContabilidadLblTotal.Text = "0 pólizas";
            this.ContabilidadPnlBatch.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadLblBatch);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadLblTipoPeriodo);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadCboTipoPeriodo);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadLblDesde);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadDtpDesde);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadLblHasta);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadDtpHasta);
            this.ContabilidadPnlBatch.Controls.Add(this.ContabilidadBtnActualizarSaldos);
            this.ContabilidadPnlBatch.Location = new System.Drawing.Point(24, 510);
            this.ContabilidadPnlBatch.Name = "ContabilidadPnlBatch";
            this.ContabilidadPnlBatch.Size = new System.Drawing.Size(1084, 110);
            this.ContabilidadPnlBatch.TabIndex = 7;
            this.ContabilidadLblBatch.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblBatch.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ContabilidadLblBatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblBatch.Location = new System.Drawing.Point(20, 12);
            this.ContabilidadLblBatch.Name = "ContabilidadLblBatch";
            this.ContabilidadLblBatch.Size = new System.Drawing.Size(1044, 22);
            this.ContabilidadLblBatch.TabIndex = 0;
            this.ContabilidadLblBatch.Text = "Actualización de saldos (batch): aplica las pólizas validadas del rango";
            this.ContabilidadLblTipoPeriodo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTipoPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblTipoPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblTipoPeriodo.Location = new System.Drawing.Point(20, 42);
            this.ContabilidadLblTipoPeriodo.Name = "ContabilidadLblTipoPeriodo";
            this.ContabilidadLblTipoPeriodo.Size = new System.Drawing.Size(180, 22);
            this.ContabilidadLblTipoPeriodo.TabIndex = 0;
            this.ContabilidadLblTipoPeriodo.Text = "Tipo de período";
            this.ContabilidadCboTipoPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboTipoPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboTipoPeriodo.FormattingEnabled = true;
            this.ContabilidadCboTipoPeriodo.Location = new System.Drawing.Point(20, 66);
            this.ContabilidadCboTipoPeriodo.Name = "ContabilidadCboTipoPeriodo";
            this.ContabilidadCboTipoPeriodo.Size = new System.Drawing.Size(180, 31);
            this.ContabilidadCboTipoPeriodo.TabIndex = 0;
            this.ContabilidadLblDesde.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblDesde.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblDesde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblDesde.Location = new System.Drawing.Point(220, 42);
            this.ContabilidadLblDesde.Name = "ContabilidadLblDesde";
            this.ContabilidadLblDesde.Size = new System.Drawing.Size(180, 22);
            this.ContabilidadLblDesde.TabIndex = 0;
            this.ContabilidadLblDesde.Text = "Desde";
            this.ContabilidadDtpDesde.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadDtpDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadDtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpDesde.Location = new System.Drawing.Point(220, 66);
            this.ContabilidadDtpDesde.Name = "ContabilidadDtpDesde";
            this.ContabilidadDtpDesde.Size = new System.Drawing.Size(180, 30);
            this.ContabilidadDtpDesde.TabIndex = 1;
            this.ContabilidadLblHasta.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblHasta.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblHasta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.ContabilidadLblHasta.Location = new System.Drawing.Point(420, 42);
            this.ContabilidadLblHasta.Name = "ContabilidadLblHasta";
            this.ContabilidadLblHasta.Size = new System.Drawing.Size(180, 22);
            this.ContabilidadLblHasta.TabIndex = 0;
            this.ContabilidadLblHasta.Text = "Hasta";
            this.ContabilidadDtpHasta.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadDtpHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadDtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpHasta.Location = new System.Drawing.Point(420, 66);
            this.ContabilidadDtpHasta.Name = "ContabilidadDtpHasta";
            this.ContabilidadDtpHasta.Size = new System.Drawing.Size(180, 30);
            this.ContabilidadDtpHasta.TabIndex = 2;
            this.ContabilidadBtnActualizarSaldos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContabilidadBtnActualizarSaldos.Estilo = CapaVista_Contabilidad.Componentes.EstiloBotonContabilidad.Dorado;
            this.ContabilidadBtnActualizarSaldos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ContabilidadBtnActualizarSaldos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnActualizarSaldos.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_aplicar;
            this.ContabilidadBtnActualizarSaldos.Location = new System.Drawing.Point(864, 58);
            this.ContabilidadBtnActualizarSaldos.Name = "ContabilidadBtnActualizarSaldos";
            this.ContabilidadBtnActualizarSaldos.Size = new System.Drawing.Size(200, 42);
            this.ContabilidadBtnActualizarSaldos.TabIndex = 3;
            this.ContabilidadBtnActualizarSaldos.Text = "Actualizar saldos";
            this.ContabilidadBtnActualizarSaldos.UseVisualStyleBackColor = false;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.Controls.Add(this.ContabilidadPnlPrincipal);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(50)))), ((int)(((byte)(61)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Contabilidad.Properties.Resources.Icono_Contabilidad;
            this.MaximizeBox = false;
            this.Name = "FrmPolizas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "10001 - Polizas";
            this.ContabilidadPnlPrincipal.ResumeLayout(false);
            this.ContabilidadPnlBatch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvEncabezados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlPrincipal;
        private System.Windows.Forms.Label ContabilidadLblTitulo;
        private System.Windows.Forms.Label ContabilidadLblModo;
        private System.Windows.Forms.Label ContabilidadLblPeriodo;
        private System.Windows.Forms.ComboBox ContabilidadCboPeriodo;
        private System.Windows.Forms.Label ContabilidadLblEstado;
        private System.Windows.Forms.ComboBox ContabilidadCboEstado;
        private System.Windows.Forms.CheckBox ContabilidadChkPendientes;
        private Componentes.ClsBotonContabilidad ContabilidadBtnNueva;
        private Componentes.ClsBotonContabilidad ContabilidadBtnAbrir;
        private Componentes.ClsTablaContabilidad ContabilidadDgvEncabezados;
        private System.Windows.Forms.Label ContabilidadLblTotal;
        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlBatch;
        private System.Windows.Forms.Label ContabilidadLblBatch;
        private System.Windows.Forms.Label ContabilidadLblTipoPeriodo;
        private System.Windows.Forms.ComboBox ContabilidadCboTipoPeriodo;
        private System.Windows.Forms.Label ContabilidadLblDesde;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpDesde;
        private System.Windows.Forms.Label ContabilidadLblHasta;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpHasta;
        private Componentes.ClsBotonContabilidad ContabilidadBtnActualizarSaldos;
    }
}
