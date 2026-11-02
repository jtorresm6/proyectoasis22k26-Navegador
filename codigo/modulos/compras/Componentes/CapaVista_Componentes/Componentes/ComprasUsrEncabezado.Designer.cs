namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrEncabezado
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer _Components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (_Components != null))
            {
                _Components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComprasUsrEncabezado));
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbxIcono = new System.Windows.Forms.PictureBox();
            this.ComprasTlpTItuloCodigo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDescripcion = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbxIcono)).BeginInit();
            this.ComprasTlpTItuloCodigo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpPrincipal.ColumnCount = 2;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasPbxIcono, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpTItuloCodigo, 1, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasLblDescripcion, 1, 1);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 3;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1000, 130);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasPbxIcono
            // 
            this.ComprasPbxIcono.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbxIcono.BackgroundImage")));
            this.ComprasPbxIcono.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbxIcono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbxIcono.Location = new System.Drawing.Point(15, 15);
            this.ComprasPbxIcono.Margin = new System.Windows.Forms.Padding(15);
            this.ComprasPbxIcono.Name = "ComprasPbxIcono";
            this.ComprasTlpPrincipal.SetRowSpan(this.ComprasPbxIcono, 2);
            this.ComprasPbxIcono.Size = new System.Drawing.Size(100, 100);
            this.ComprasPbxIcono.TabIndex = 0;
            this.ComprasPbxIcono.TabStop = false;
            // 
            // ComprasTlpTItuloCodigo
            // 
            this.ComprasTlpTItuloCodigo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpTItuloCodigo.ColumnCount = 2;
            this.ComprasTlpTItuloCodigo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTItuloCodigo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.ComprasTlpTItuloCodigo.Controls.Add(this.ComprasLblTitulo, 0, 0);
            this.ComprasTlpTItuloCodigo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpTItuloCodigo.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasTlpTItuloCodigo.Location = new System.Drawing.Point(130, 0);
            this.ComprasTlpTItuloCodigo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpTItuloCodigo.Name = "ComprasTlpTItuloCodigo";
            this.ComprasTlpTItuloCodigo.RowCount = 1;
            this.ComprasTlpTItuloCodigo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTItuloCodigo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpTItuloCodigo.Size = new System.Drawing.Size(870, 75);
            this.ComprasTlpTItuloCodigo.TabIndex = 6;
            // 
            // ComprasLblTitulo
            // 
            this.ComprasLblTitulo.AlineacionTexto = System.Drawing.ContentAlignment.BottomLeft;
            this.ComprasLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblTitulo.ColorTexto = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasTlpTItuloCodigo.SetColumnSpan(this.ComprasLblTitulo, 2);
            this.ComprasLblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ComprasLblTitulo.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasLblTitulo.Location = new System.Drawing.Point(0, 0);
            this.ComprasLblTitulo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.ComprasLblTitulo.MaximumSize = new System.Drawing.Size(2748, 0);
            this.ComprasLblTitulo.MostrarAsterisco = false;
            this.ComprasLblTitulo.Name = "ComprasLblTitulo";
            this.ComprasLblTitulo.Size = new System.Drawing.Size(870, 65);
            this.ComprasLblTitulo.TabIndex = 7;
            this.ComprasLblTitulo.TabStop = false;
            this.ComprasLblTitulo.Text = "Titulo";
            this.ComprasLblTitulo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ComprasLblTitulo.Texto = "Titulo";
            this.ComprasLblTitulo.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            // 
            // ComprasLblDescripcion
            // 
            this.ComprasLblDescripcion.AlineacionTexto = System.Drawing.ContentAlignment.TopLeft;
            this.ComprasLblDescripcion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblDescripcion.ColorTexto = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasLblDescripcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDescripcion.Font = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasLblDescripcion.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasLblDescripcion.Location = new System.Drawing.Point(137, 75);
            this.ComprasLblDescripcion.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ComprasLblDescripcion.MaximumSize = new System.Drawing.Size(5500, 0);
            this.ComprasLblDescripcion.MostrarAsterisco = false;
            this.ComprasLblDescripcion.Name = "ComprasLblDescripcion";
            this.ComprasLblDescripcion.Size = new System.Drawing.Size(863, 55);
            this.ComprasLblDescripcion.TabIndex = 2;
            this.ComprasLblDescripcion.TabStop = false;
            this.ComprasLblDescripcion.Text = "Descripcion";
            this.ComprasLblDescripcion.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ComprasLblDescripcion.Texto = "Descripcion";
            // 
            // ComprasUsrEncabezado
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(1500, 130);
            this.MinimumSize = new System.Drawing.Size(0, 130);
            this.Name = "ComprasUsrEncabezado";
            this.Size = new System.Drawing.Size(1000, 130);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbxIcono)).EndInit();
            this.ComprasTlpTItuloCodigo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private System.Windows.Forms.PictureBox ComprasPbxIcono;
        private ComprasUsrLabel ComprasLblDescripcion;
        private ClsTableLayoutPanelCompras ComprasTlpTItuloCodigo;
        private ComprasUsrLabel ComprasLblTitulo;
    }
}
