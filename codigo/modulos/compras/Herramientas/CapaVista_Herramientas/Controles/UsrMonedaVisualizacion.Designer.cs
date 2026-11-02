namespace CapaVista_Herramientas.Controles
{
    partial class UsrMonedaVisualizacion
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
            this.ComprasGbxPreferencia = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpPreferencia = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTituloActual = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblActual = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMoneda = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasCboMoneda = new CapaVista_Componentes.Componentes.UsrComboBoxCompras();
            this.ComprasTlpBotones = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnRestablecer = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnAplicar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblNota = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasGbxVistaPrevia = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpVistaPrevia = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTituloLocal = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMontoLocal = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblTituloConvertido = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMontoConvertido = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblDetalle = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxPreferencia.SuspendLayout();
            this.ComprasTlpPreferencia.SuspendLayout();
            this.ComprasTlpBotones.SuspendLayout();
            this.ComprasGbxVistaPrevia.SuspendLayout();
            this.ComprasTlpVistaPrevia.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasTlpPrincipal
            //
            this.ComprasTlpPrincipal.ColumnCount = 2;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxPreferencia, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxVistaPrevia, 1, 0);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.ComprasTlpPrincipal.RowCount = 1;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpPrincipal.TabIndex = 0;
            //
            // ComprasGbxPreferencia
            //
            this.ComprasGbxPreferencia.Controls.Add(this.ComprasTlpPreferencia);
            this.ComprasGbxPreferencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxPreferencia.Name = "ComprasGbxPreferencia";
            this.ComprasGbxPreferencia.Padding = new System.Windows.Forms.Padding(12);
            this.ComprasGbxPreferencia.TabIndex = 0;
            this.ComprasGbxPreferencia.TabStop = false;
            this.ComprasGbxPreferencia.Text = "Preferencia de la sesión";
            //
            // ComprasTlpPreferencia
            //
            this.ComprasTlpPreferencia.ColumnCount = 2;
            this.ComprasTlpPreferencia.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.ComprasTlpPreferencia.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasLblTituloActual, 0, 0);
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasLblActual, 1, 0);
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasLblMoneda, 0, 1);
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasCboMoneda, 1, 1);
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasTlpBotones, 0, 2);
            this.ComprasTlpPreferencia.Controls.Add(this.ComprasLblNota, 0, 3);
            this.ComprasTlpPreferencia.SetColumnSpan(this.ComprasTlpBotones, 2);
            this.ComprasTlpPreferencia.SetColumnSpan(this.ComprasLblNota, 2);
            this.ComprasTlpPreferencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPreferencia.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPreferencia.Name = "ComprasTlpPreferencia";
            this.ComprasTlpPreferencia.RowCount = 5;
            this.ComprasTlpPreferencia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.ComprasTlpPreferencia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.ComprasTlpPreferencia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.ComprasTlpPreferencia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpPreferencia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPreferencia.TabIndex = 0;
            //
            // ComprasLblTituloActual
            //
            this.ComprasLblTituloActual.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTituloActual.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblTituloActual.MostrarAsterisco = false;
            this.ComprasLblTituloActual.Name = "ComprasLblTituloActual";
            this.ComprasLblTituloActual.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblTituloActual.TabIndex = 0;
            this.ComprasLblTituloActual.Texto = "Moneda actual";
            //
            // ComprasLblActual
            //
            this.ComprasLblActual.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblActual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblActual.MostrarAsterisco = false;
            this.ComprasLblActual.Name = "ComprasLblActual";
            this.ComprasLblActual.TabIndex = 1;
            this.ComprasLblActual.Texto = "—";
            this.ComprasLblActual.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            //
            // ComprasLblMoneda
            //
            this.ComprasLblMoneda.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMoneda.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblMoneda.Name = "ComprasLblMoneda";
            this.ComprasLblMoneda.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblMoneda.TabIndex = 2;
            this.ComprasLblMoneda.Texto = "Mostrar montos en";
            //
            // ComprasCboMoneda
            //
            this.ComprasCboMoneda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboMoneda.MostrarEtiquetaError = false;
            this.ComprasCboMoneda.Name = "ComprasCboMoneda";
            this.ComprasCboMoneda.Size = new System.Drawing.Size(300, 36);
            this.ComprasCboMoneda.TabIndex = 3;
            //
            // ComprasTlpBotones
            //
            this.ComprasTlpBotones.ColumnCount = 2;
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnRestablecer, 0, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnAplicar, 1, 0);
            this.ComprasTlpBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBotones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpBotones.Name = "ComprasTlpBotones";
            this.ComprasTlpBotones.RowCount = 1;
            this.ComprasTlpBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBotones.TabIndex = 4;
            //
            // ComprasBtnRestablecer
            //
            this.ComprasBtnRestablecer.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnRestablecer.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnRestablecer.Name = "ComprasBtnRestablecer";
            this.ComprasBtnRestablecer.Size = new System.Drawing.Size(200, 44);
            this.ComprasBtnRestablecer.TabIndex = 0;
            this.ComprasBtnRestablecer.Text = "Usar moneda local";
            //
            // ComprasBtnAplicar
            //
            this.ComprasBtnAplicar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnAplicar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnAplicar.Name = "ComprasBtnAplicar";
            this.ComprasBtnAplicar.Size = new System.Drawing.Size(200, 44);
            this.ComprasBtnAplicar.TabIndex = 1;
            this.ComprasBtnAplicar.Text = "Aplicar";
            //
            // ComprasLblNota
            //
            this.ComprasLblNota.AlineacionTexto = System.Drawing.ContentAlignment.TopLeft;
            this.ComprasLblNota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblNota.MostrarAsterisco = false;
            this.ComprasLblNota.Name = "ComprasLblNota";
            this.ComprasLblNota.TabIndex = 5;
            this.ComprasLblNota.Texto = "La preferencia dura mientras la sesión esté abierta. Los montos se siguen registrando en la moneda local; solo cambia cómo se muestran.";
            //
            // ComprasGbxVistaPrevia
            //
            this.ComprasGbxVistaPrevia.Controls.Add(this.ComprasTlpVistaPrevia);
            this.ComprasGbxVistaPrevia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxVistaPrevia.Name = "ComprasGbxVistaPrevia";
            this.ComprasGbxVistaPrevia.Padding = new System.Windows.Forms.Padding(12);
            this.ComprasGbxVistaPrevia.TabIndex = 1;
            this.ComprasGbxVistaPrevia.TabStop = false;
            this.ComprasGbxVistaPrevia.Text = "Vista previa (tipo de cambio de hoy)";
            //
            // ComprasTlpVistaPrevia
            //
            this.ComprasTlpVistaPrevia.ColumnCount = 1;
            this.ComprasTlpVistaPrevia.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpVistaPrevia.Controls.Add(this.ComprasLblTituloLocal, 0, 0);
            this.ComprasTlpVistaPrevia.Controls.Add(this.ComprasLblMontoLocal, 0, 1);
            this.ComprasTlpVistaPrevia.Controls.Add(this.ComprasLblTituloConvertido, 0, 2);
            this.ComprasTlpVistaPrevia.Controls.Add(this.ComprasLblMontoConvertido, 0, 3);
            this.ComprasTlpVistaPrevia.Controls.Add(this.ComprasLblDetalle, 0, 4);
            this.ComprasTlpVistaPrevia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpVistaPrevia.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpVistaPrevia.Name = "ComprasTlpVistaPrevia";
            this.ComprasTlpVistaPrevia.RowCount = 6;
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ComprasTlpVistaPrevia.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpVistaPrevia.TabIndex = 0;
            //
            // ComprasLblTituloLocal
            //
            this.ComprasLblTituloLocal.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTituloLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTituloLocal.MostrarAsterisco = false;
            this.ComprasLblTituloLocal.Name = "ComprasLblTituloLocal";
            this.ComprasLblTituloLocal.TabIndex = 0;
            this.ComprasLblTituloLocal.Texto = "Monto registrado";
            //
            // ComprasLblMontoLocal
            //
            this.ComprasLblMontoLocal.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMontoLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMontoLocal.MostrarAsterisco = false;
            this.ComprasLblMontoLocal.Name = "ComprasLblMontoLocal";
            this.ComprasLblMontoLocal.TabIndex = 1;
            this.ComprasLblMontoLocal.TamanoTipografia = 20F;
            this.ComprasLblMontoLocal.Texto = "—";
            this.ComprasLblMontoLocal.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            //
            // ComprasLblTituloConvertido
            //
            this.ComprasLblTituloConvertido.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTituloConvertido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTituloConvertido.MostrarAsterisco = false;
            this.ComprasLblTituloConvertido.Name = "ComprasLblTituloConvertido";
            this.ComprasLblTituloConvertido.TabIndex = 2;
            this.ComprasLblTituloConvertido.Texto = "Se mostraría como";
            //
            // ComprasLblMontoConvertido
            //
            this.ComprasLblMontoConvertido.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMontoConvertido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMontoConvertido.MostrarAsterisco = false;
            this.ComprasLblMontoConvertido.Name = "ComprasLblMontoConvertido";
            this.ComprasLblMontoConvertido.TabIndex = 3;
            this.ComprasLblMontoConvertido.TamanoTipografia = 20F;
            this.ComprasLblMontoConvertido.Texto = "—";
            this.ComprasLblMontoConvertido.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            //
            // ComprasLblDetalle
            //
            this.ComprasLblDetalle.AlineacionTexto = System.Drawing.ContentAlignment.TopLeft;
            this.ComprasLblDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDetalle.MostrarAsterisco = false;
            this.ComprasLblDetalle.Name = "ComprasLblDetalle";
            this.ComprasLblDetalle.TabIndex = 4;
            this.ComprasLblDetalle.Texto = "";
            //
            // UsrMonedaVisualizacion
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Name = "UsrMonedaVisualizacion";
            this.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpVistaPrevia.ResumeLayout(false);
            this.ComprasGbxVistaPrevia.ResumeLayout(false);
            this.ComprasTlpBotones.ResumeLayout(false);
            this.ComprasTlpPreferencia.ResumeLayout(false);
            this.ComprasGbxPreferencia.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxPreferencia;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPreferencia;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblTituloActual;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblActual;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMoneda;
        private CapaVista_Componentes.Componentes.UsrComboBoxCompras ComprasCboMoneda;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBotones;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnRestablecer;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnAplicar;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblNota;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxVistaPrevia;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpVistaPrevia;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblTituloLocal;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMontoLocal;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblTituloConvertido;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMontoConvertido;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblDetalle;
    }
}
