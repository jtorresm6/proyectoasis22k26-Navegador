namespace CapaVista_Herramientas.Controles
{
    partial class UsrHistorialTipoCambio
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
            this.ComprasLblOrigen = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasCboOrigen = new CapaVista_Componentes.Componentes.UsrComboBoxCompras();
            this.ComprasLblDestino = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasCboDestino = new CapaVista_Componentes.Componentes.UsrComboBoxCompras();
            this.ComprasRngFechas = new CapaVista_Componentes.Componentes.UsrRangoFechasCompras();
            this.ComprasBtnConsultar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasGrfHistorial = new CapaVista_Herramientas.Controles.ClsGraficaLineaCompras();
            this.ComprasTlpResumen = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblPar = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMinimo = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMaximo = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblPromedio = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblVariacion = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpFiltro.SuspendLayout();
            this.ComprasTlpResumen.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasTlpPrincipal
            //
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpFiltro, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGrfHistorial, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpResumen, 0, 2);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.ComprasTlpPrincipal.RowCount = 3;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpPrincipal.TabIndex = 0;
            //
            // ComprasTlpFiltro
            //
            this.ComprasTlpFiltro.ColumnCount = 4;
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.ComprasTlpFiltro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltro.Controls.Add(this.ComprasLblOrigen, 0, 0);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasLblDestino, 1, 0);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasCboOrigen, 0, 1);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasCboDestino, 1, 1);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasRngFechas, 2, 0);
            this.ComprasTlpFiltro.Controls.Add(this.ComprasBtnConsultar, 3, 1);
            this.ComprasTlpFiltro.SetRowSpan(this.ComprasRngFechas, 2);
            this.ComprasTlpFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFiltro.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFiltro.Name = "ComprasTlpFiltro";
            this.ComprasTlpFiltro.RowCount = 2;
            this.ComprasTlpFiltro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpFiltro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltro.TabIndex = 0;
            //
            // ComprasLblOrigen
            //
            this.ComprasLblOrigen.AlineacionTexto = System.Drawing.ContentAlignment.BottomLeft;
            this.ComprasLblOrigen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblOrigen.Name = "ComprasLblOrigen";
            this.ComprasLblOrigen.TabIndex = 0;
            this.ComprasLblOrigen.Texto = "Moneda de origen";
            //
            // ComprasCboOrigen
            //
            this.ComprasCboOrigen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboOrigen.MostrarEtiquetaError = false;
            this.ComprasCboOrigen.Name = "ComprasCboOrigen";
            this.ComprasCboOrigen.Size = new System.Drawing.Size(260, 36);
            this.ComprasCboOrigen.TabIndex = 1;
            //
            // ComprasLblDestino
            //
            this.ComprasLblDestino.AlineacionTexto = System.Drawing.ContentAlignment.BottomLeft;
            this.ComprasLblDestino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDestino.Name = "ComprasLblDestino";
            this.ComprasLblDestino.TabIndex = 2;
            this.ComprasLblDestino.Texto = "Moneda de destino";
            //
            // ComprasCboDestino
            //
            this.ComprasCboDestino.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboDestino.MostrarEtiquetaError = false;
            this.ComprasCboDestino.Name = "ComprasCboDestino";
            this.ComprasCboDestino.Size = new System.Drawing.Size(260, 36);
            this.ComprasCboDestino.TabIndex = 3;
            //
            // ComprasRngFechas
            //
            this.ComprasRngFechas.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasRngFechas.Name = "ComprasRngFechas";
            this.ComprasRngFechas.Size = new System.Drawing.Size(440, 80);
            this.ComprasRngFechas.TabIndex = 4;
            //
            // ComprasBtnConsultar
            //
            this.ComprasBtnConsultar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnConsultar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.ComprasBtnConsultar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnConsultar.Name = "ComprasBtnConsultar";
            this.ComprasBtnConsultar.Size = new System.Drawing.Size(150, 40);
            this.ComprasBtnConsultar.TabIndex = 5;
            this.ComprasBtnConsultar.Text = "Consultar";
            //
            // ComprasGrfHistorial
            //
            this.ComprasGrfHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGrfHistorial.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.ComprasGrfHistorial.Name = "ComprasGrfHistorial";
            this.ComprasGrfHistorial.TabIndex = 6;
            //
            // ComprasTlpResumen
            //
            this.ComprasTlpResumen.ColumnCount = 5;
            this.ComprasTlpResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.ComprasTlpResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.ComprasTlpResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.ComprasTlpResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.ComprasTlpResumen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.ComprasTlpResumen.Controls.Add(this.ComprasLblPar, 0, 0);
            this.ComprasTlpResumen.Controls.Add(this.ComprasLblMinimo, 1, 0);
            this.ComprasTlpResumen.Controls.Add(this.ComprasLblMaximo, 2, 0);
            this.ComprasTlpResumen.Controls.Add(this.ComprasLblPromedio, 3, 0);
            this.ComprasTlpResumen.Controls.Add(this.ComprasLblVariacion, 4, 0);
            this.ComprasTlpResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpResumen.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpResumen.Name = "ComprasTlpResumen";
            this.ComprasTlpResumen.RowCount = 1;
            this.ComprasTlpResumen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResumen.TabIndex = 7;
            //
            // ComprasLblPar
            //
            this.ComprasLblPar.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblPar.MostrarAsterisco = false;
            this.ComprasLblPar.Name = "ComprasLblPar";
            this.ComprasLblPar.TabIndex = 0;
            this.ComprasLblPar.Texto = "—";
            //
            // ComprasLblMinimo
            //
            this.ComprasLblMinimo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMinimo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMinimo.MostrarAsterisco = false;
            this.ComprasLblMinimo.Name = "ComprasLblMinimo";
            this.ComprasLblMinimo.TabIndex = 1;
            this.ComprasLblMinimo.Texto = "Mínimo: —";
            //
            // ComprasLblMaximo
            //
            this.ComprasLblMaximo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMaximo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMaximo.MostrarAsterisco = false;
            this.ComprasLblMaximo.Name = "ComprasLblMaximo";
            this.ComprasLblMaximo.TabIndex = 2;
            this.ComprasLblMaximo.Texto = "Máximo: —";
            //
            // ComprasLblPromedio
            //
            this.ComprasLblPromedio.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPromedio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblPromedio.MostrarAsterisco = false;
            this.ComprasLblPromedio.Name = "ComprasLblPromedio";
            this.ComprasLblPromedio.TabIndex = 3;
            this.ComprasLblPromedio.Texto = "Promedio: —";
            //
            // ComprasLblVariacion
            //
            this.ComprasLblVariacion.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblVariacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblVariacion.MostrarAsterisco = false;
            this.ComprasLblVariacion.Name = "ComprasLblVariacion";
            this.ComprasLblVariacion.TabIndex = 4;
            this.ComprasLblVariacion.Texto = "Variación: —";
            //
            // UsrHistorialTipoCambio
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Name = "UsrHistorialTipoCambio";
            this.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpResumen.ResumeLayout(false);
            this.ComprasTlpFiltro.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFiltro;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblOrigen;
        private CapaVista_Componentes.Componentes.UsrComboBoxCompras ComprasCboOrigen;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblDestino;
        private CapaVista_Componentes.Componentes.UsrComboBoxCompras ComprasCboDestino;
        private CapaVista_Componentes.Componentes.UsrRangoFechasCompras ComprasRngFechas;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnConsultar;
        private CapaVista_Herramientas.Controles.ClsGraficaLineaCompras ComprasGrfHistorial;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpResumen;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblPar;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMinimo;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMaximo;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblPromedio;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblVariacion;
    }
}
