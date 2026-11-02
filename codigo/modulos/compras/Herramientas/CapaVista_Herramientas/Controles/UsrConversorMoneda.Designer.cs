namespace CapaVista_Herramientas.Controles
{
    partial class UsrConversorMoneda
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
            this.ComprasGbxDatos = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpDatos = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblMonto = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasTxtMonto = new CapaVista_Componentes.Componentes.UsrTextBoxCompras();
            this.ComprasLblOrigen = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasCboOrigen = new CapaVista_Componentes.Componentes.UsrComboBoxCompras();
            this.ComprasLblDestino = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasCboDestino = new CapaVista_Componentes.Componentes.UsrComboBoxCompras();
            this.ComprasLblFecha = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasDtpFecha = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasTlpBotones = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIntercambiar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnConvertir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasGbxResultado = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpResultado = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblResultado = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblEquivalencia = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblMetodo = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblFechaTipoCambio = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasLblFuente = new CapaVista_Componentes.Componentes.UsrLabelCompras();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxDatos.SuspendLayout();
            this.ComprasTlpDatos.SuspendLayout();
            this.ComprasTlpBotones.SuspendLayout();
            this.ComprasGbxResultado.SuspendLayout();
            this.ComprasTlpResultado.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasTlpPrincipal
            //
            this.ComprasTlpPrincipal.ColumnCount = 2;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxDatos, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxResultado, 1, 0);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.ComprasTlpPrincipal.RowCount = 1;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpPrincipal.TabIndex = 0;
            //
            // ComprasGbxDatos
            //
            this.ComprasGbxDatos.Controls.Add(this.ComprasTlpDatos);
            this.ComprasGbxDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxDatos.Name = "ComprasGbxDatos";
            this.ComprasGbxDatos.Padding = new System.Windows.Forms.Padding(12);
            this.ComprasGbxDatos.TabIndex = 0;
            this.ComprasGbxDatos.TabStop = false;
            this.ComprasGbxDatos.Text = "Datos de la conversión";
            //
            // ComprasTlpDatos
            //
            this.ComprasTlpDatos.ColumnCount = 2;
            this.ComprasTlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.ComprasTlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatos.Controls.Add(this.ComprasLblMonto, 0, 0);
            this.ComprasTlpDatos.Controls.Add(this.ComprasTxtMonto, 1, 0);
            this.ComprasTlpDatos.Controls.Add(this.ComprasLblOrigen, 0, 1);
            this.ComprasTlpDatos.Controls.Add(this.ComprasCboOrigen, 1, 1);
            this.ComprasTlpDatos.Controls.Add(this.ComprasLblDestino, 0, 2);
            this.ComprasTlpDatos.Controls.Add(this.ComprasCboDestino, 1, 2);
            this.ComprasTlpDatos.Controls.Add(this.ComprasLblFecha, 0, 3);
            this.ComprasTlpDatos.Controls.Add(this.ComprasDtpFecha, 1, 3);
            this.ComprasTlpDatos.Controls.Add(this.ComprasTlpBotones, 0, 4);
            this.ComprasTlpDatos.SetColumnSpan(this.ComprasTlpBotones, 2);
            this.ComprasTlpDatos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDatos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDatos.Name = "ComprasTlpDatos";
            this.ComprasTlpDatos.RowCount = 6;
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.ComprasTlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatos.TabIndex = 0;
            //
            // ComprasLblMonto
            //
            this.ComprasLblMonto.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMonto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblMonto.Name = "ComprasLblMonto";
            this.ComprasLblMonto.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblMonto.TabIndex = 0;
            this.ComprasLblMonto.Texto = "Monto";
            //
            // ComprasTxtMonto
            //
            this.ComprasTxtMonto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtMonto.MaxLength = 15;
            this.ComprasTxtMonto.Name = "ComprasTxtMonto";
            this.ComprasTxtMonto.Size = new System.Drawing.Size(300, 66);
            this.ComprasTxtMonto.TabIndex = 1;
            //
            // ComprasLblOrigen
            //
            this.ComprasLblOrigen.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblOrigen.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblOrigen.Name = "ComprasLblOrigen";
            this.ComprasLblOrigen.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblOrigen.TabIndex = 2;
            this.ComprasLblOrigen.Texto = "Moneda de origen";
            //
            // ComprasCboOrigen
            //
            this.ComprasCboOrigen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboOrigen.MostrarEtiquetaError = false;
            this.ComprasCboOrigen.Name = "ComprasCboOrigen";
            this.ComprasCboOrigen.Size = new System.Drawing.Size(300, 36);
            this.ComprasCboOrigen.TabIndex = 3;
            //
            // ComprasLblDestino
            //
            this.ComprasLblDestino.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDestino.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblDestino.Name = "ComprasLblDestino";
            this.ComprasLblDestino.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblDestino.TabIndex = 4;
            this.ComprasLblDestino.Texto = "Moneda de destino";
            //
            // ComprasCboDestino
            //
            this.ComprasCboDestino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboDestino.MostrarEtiquetaError = false;
            this.ComprasCboDestino.Name = "ComprasCboDestino";
            this.ComprasCboDestino.Size = new System.Drawing.Size(300, 36);
            this.ComprasCboDestino.TabIndex = 5;
            //
            // ComprasLblFecha
            //
            this.ComprasLblFecha.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFecha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblFecha.MostrarAsterisco = false;
            this.ComprasLblFecha.Name = "ComprasLblFecha";
            this.ComprasLblFecha.Size = new System.Drawing.Size(200, 22);
            this.ComprasLblFecha.TabIndex = 6;
            this.ComprasLblFecha.Texto = "Tipo de cambio al";
            //
            // ComprasDtpFecha
            //
            this.ComprasDtpFecha.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasDtpFecha.Name = "ComprasDtpFecha";
            this.ComprasDtpFecha.Size = new System.Drawing.Size(200, 32);
            this.ComprasDtpFecha.TabIndex = 7;
            //
            // ComprasTlpBotones
            //
            this.ComprasTlpBotones.ColumnCount = 2;
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpBotones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnIntercambiar, 0, 0);
            this.ComprasTlpBotones.Controls.Add(this.ComprasBtnConvertir, 1, 0);
            this.ComprasTlpBotones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBotones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpBotones.Name = "ComprasTlpBotones";
            this.ComprasTlpBotones.RowCount = 1;
            this.ComprasTlpBotones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBotones.TabIndex = 8;
            //
            // ComprasBtnIntercambiar
            //
            this.ComprasBtnIntercambiar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnIntercambiar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnIntercambiar.Name = "ComprasBtnIntercambiar";
            this.ComprasBtnIntercambiar.Size = new System.Drawing.Size(200, 44);
            this.ComprasBtnIntercambiar.TabIndex = 0;
            this.ComprasBtnIntercambiar.Text = "Intercambiar monedas";
            //
            // ComprasBtnConvertir
            //
            this.ComprasBtnConvertir.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            this.ComprasBtnConvertir.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnConvertir.Name = "ComprasBtnConvertir";
            this.ComprasBtnConvertir.Size = new System.Drawing.Size(200, 44);
            this.ComprasBtnConvertir.TabIndex = 1;
            this.ComprasBtnConvertir.Text = "Convertir";
            //
            // ComprasGbxResultado
            //
            this.ComprasGbxResultado.Controls.Add(this.ComprasTlpResultado);
            this.ComprasGbxResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxResultado.Name = "ComprasGbxResultado";
            this.ComprasGbxResultado.Padding = new System.Windows.Forms.Padding(12);
            this.ComprasGbxResultado.TabIndex = 1;
            this.ComprasGbxResultado.TabStop = false;
            this.ComprasGbxResultado.Text = "Resultado";
            //
            // ComprasTlpResultado
            //
            this.ComprasTlpResultado.ColumnCount = 1;
            this.ComprasTlpResultado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResultado.Controls.Add(this.ComprasLblResultado, 0, 0);
            this.ComprasTlpResultado.Controls.Add(this.ComprasLblEquivalencia, 0, 1);
            this.ComprasTlpResultado.Controls.Add(this.ComprasLblMetodo, 0, 2);
            this.ComprasTlpResultado.Controls.Add(this.ComprasLblFechaTipoCambio, 0, 3);
            this.ComprasTlpResultado.Controls.Add(this.ComprasLblFuente, 0, 4);
            this.ComprasTlpResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpResultado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpResultado.Name = "ComprasTlpResultado";
            this.ComprasTlpResultado.RowCount = 6;
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpResultado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResultado.TabIndex = 0;
            //
            // ComprasLblResultado
            //
            this.ComprasLblResultado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblResultado.MaximumSize = new System.Drawing.Size(1000, 90);
            this.ComprasLblResultado.MostrarAsterisco = false;
            this.ComprasLblResultado.Name = "ComprasLblResultado";
            this.ComprasLblResultado.TabIndex = 0;
            this.ComprasLblResultado.TamanoTipografia = 28F;
            this.ComprasLblResultado.Texto = "—";
            this.ComprasLblResultado.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            //
            // ComprasLblEquivalencia
            //
            this.ComprasLblEquivalencia.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEquivalencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEquivalencia.MostrarAsterisco = false;
            this.ComprasLblEquivalencia.Name = "ComprasLblEquivalencia";
            this.ComprasLblEquivalencia.TabIndex = 1;
            this.ComprasLblEquivalencia.TamanoTipografia = 12F;
            this.ComprasLblEquivalencia.Texto = "Ingrese un monto y presione Convertir.";
            //
            // ComprasLblMetodo
            //
            this.ComprasLblMetodo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMetodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMetodo.MostrarAsterisco = false;
            this.ComprasLblMetodo.Name = "ComprasLblMetodo";
            this.ComprasLblMetodo.TabIndex = 2;
            this.ComprasLblMetodo.Texto = "Método: —";
            //
            // ComprasLblFechaTipoCambio
            //
            this.ComprasLblFechaTipoCambio.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFechaTipoCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaTipoCambio.MostrarAsterisco = false;
            this.ComprasLblFechaTipoCambio.Name = "ComprasLblFechaTipoCambio";
            this.ComprasLblFechaTipoCambio.TabIndex = 3;
            this.ComprasLblFechaTipoCambio.Texto = "Tipo de cambio del: —";
            //
            // ComprasLblFuente
            //
            this.ComprasLblFuente.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFuente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFuente.MostrarAsterisco = false;
            this.ComprasLblFuente.Name = "ComprasLblFuente";
            this.ComprasLblFuente.TabIndex = 4;
            this.ComprasLblFuente.Texto = "Fuente: —";
            //
            // UsrConversorMoneda
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Name = "UsrConversorMoneda";
            this.Size = new System.Drawing.Size(1280, 480);
            this.ComprasTlpResultado.ResumeLayout(false);
            this.ComprasGbxResultado.ResumeLayout(false);
            this.ComprasTlpBotones.ResumeLayout(false);
            this.ComprasTlpDatos.ResumeLayout(false);
            this.ComprasGbxDatos.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxDatos;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDatos;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMonto;
        private CapaVista_Componentes.Componentes.UsrTextBoxCompras ComprasTxtMonto;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblOrigen;
        private CapaVista_Componentes.Componentes.UsrComboBoxCompras ComprasCboOrigen;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblDestino;
        private CapaVista_Componentes.Componentes.UsrComboBoxCompras ComprasCboDestino;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblFecha;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFecha;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBotones;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIntercambiar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnConvertir;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxResultado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpResultado;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblResultado;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblEquivalencia;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblMetodo;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblFechaTipoCambio;
        private CapaVista_Componentes.Componentes.UsrLabelCompras ComprasLblFuente;
    }
}
