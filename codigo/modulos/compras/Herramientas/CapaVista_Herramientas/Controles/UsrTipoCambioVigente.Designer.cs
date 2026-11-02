namespace CapaVista_Herramientas.Controles
{
    partial class UsrTipoCambioVigente
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

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpFiltro = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblFecha = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasDtpFecha = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasBtnConsultar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasDgvTiposCambio = new CapaVista_Componentes.Componentes.UsrDataGridViewCompras();
            this.ComprasLblNota = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpFiltro.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasTlpPrincipal
            //
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpFiltro, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasDgvTiposCambio, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasLblNota, 0, 2);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.ComprasTlpPrincipal.RowCount = 3;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpPrincipal.TabIndex = 0;
            //
            // ComprasTlpFiltro
            //
            this.ComprasTlpFiltro.ColumnCount = 4;
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltro.Controls.Add(this.ComprasLblFecha, 0, 0);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasDtpFecha, 1, 0);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasBtnConsultar, 2, 0);
            this.ComprasTlpFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFiltro.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFiltro.Name = "ComprasTlpFiltro";
            this.ComprasTlpFiltro.RowCount = 1;
            this.ComprasTlpFiltro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltro.TabIndex = 0;
            //
            // ComprasLblFecha
            //
            this.ComprasLblFecha.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFecha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblFecha.MostrarAsterisco = false;
            this.ComprasLblFecha.Name = "ComprasLblFecha";
            this.ComprasLblFecha.Size = new System.Drawing.Size(160, 22);
            this.ComprasLblFecha.TabIndex = 0;
            this.ComprasLblFecha.Texto = "Fecha de consulta";
            //
            // ComprasDtpFecha
            //
            this.ComprasDtpFecha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasDtpFecha.Name = "ComprasDtpFecha";
            this.ComprasDtpFecha.Size = new System.Drawing.Size(200, 32);
            this.ComprasDtpFecha.TabIndex = 1;
            //
            // ComprasBtnConsultar
            //
            this.ComprasBtnConsultar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnConsultar.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasBtnConsultar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnConsultar.Name = "ComprasBtnConsultar";
            this.ComprasBtnConsultar.Size = new System.Drawing.Size(150, 40);
            this.ComprasBtnConsultar.TabIndex = 2;
            this.ComprasBtnConsultar.Text = "Consultar";
            //
            // ComprasDgvTiposCambio
            //
            this.ComprasDgvTiposCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvTiposCambio.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.ComprasDgvTiposCambio.Name = "ComprasDgvTiposCambio";
            this.ComprasDgvTiposCambio.TabIndex = 3;
            //
            // ComprasLblNota
            //
            this.ComprasLblNota.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblNota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblNota.MostrarAsterisco = false;
            this.ComprasLblNota.Name = "ComprasLblNota";
            this.ComprasLblNota.TabIndex = 4;
            this.ComprasLblNota.Texto = "Tipos de cambio registrados por el módulo de Bancos. Consulta de solo lectura.";
            //
            // UsrTipoCambioVigente
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Name = "UsrTipoCambioVigente";
            this.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpFiltro.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFiltro;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblFecha;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFecha;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnConsultar;
        private CapaVista_Componentes.Componentes.UsrDataGridViewCompras ComprasDgvTiposCambio;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblNota;
    }
}
