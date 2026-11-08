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
            this.ComprasTlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasBtnAyudas = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnReportes = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPbxIcono = new System.Windows.Forms.PictureBox();
            this.ComprasTlpTItuloCodigo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDescripcion = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbxIcono)).BeginInit();
            this.ComprasTlpTItuloCodigo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpPrincipal.ColumnCount = 3;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpAcciones, 2, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasPbxIcono, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpTItuloCodigo, 1, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasLblDescripcion, 1, 1);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
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
            // ComprasTlpAcciones
            // 
            this.ComprasTlpAcciones.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasTlpAcciones.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpAcciones.ColumnCount = 2;
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnAyudas, 0, 0);
            this.ComprasTlpAcciones.Controls.Add(this.ComprasBtnReportes, 1, 0);
            this.ComprasTlpAcciones.Location = new System.Drawing.Point(814, 22);
            this.ComprasTlpAcciones.Margin = new System.Windows.Forms.Padding(6);
            this.ComprasTlpAcciones.Name = "ComprasTlpAcciones";
            this.ComprasTlpAcciones.RowCount = 1;
            this.ComprasTlpPrincipal.SetRowSpan(this.ComprasTlpAcciones, 3);
            this.ComprasTlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAcciones.Size = new System.Drawing.Size(172, 86);
            this.ComprasTlpAcciones.TabIndex = 8;
            this.ComprasBtnAyudas.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnAyudas
            // 
            this.ComprasBtnAyudas.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnAyudas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnAyudas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAyudas.BackgroundImage")));
            this.ComprasBtnAyudas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnAyudas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnAyudas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAyudas.FlatAppearance.BorderSize = 0;
            this.ComprasBtnAyudas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnAyudas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnAyudas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnAyudas.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnAyudas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAyudas.Location = new System.Drawing.Point(3, 3);
            this.ComprasBtnAyudas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnAyudas.Name = "ComprasBtnAyudas";
            this.ComprasBtnAyudas.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnAyudas.TabIndex = 0;
            this.ComprasBtnAyudas.UseVisualStyleBackColor = false;
            this.ComprasBtnAyudas.Click += new System.EventHandler(this.ComprasBtnAyudas_Click);
            this.ComprasBtnReportes.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnReportes
            // 
            this.ComprasBtnReportes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnReportes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnReportes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnReportes.BackgroundImage")));
            this.ComprasBtnReportes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnReportes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnReportes.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnReportes.FlatAppearance.BorderSize = 0;
            this.ComprasBtnReportes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnReportes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnReportes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnReportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnReportes.Location = new System.Drawing.Point(89, 3);
            this.ComprasBtnReportes.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnReportes.Name = "ComprasBtnReportes";
            this.ComprasBtnReportes.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnReportes.TabIndex = 1;
            this.ComprasBtnReportes.UseVisualStyleBackColor = false;
            this.ComprasBtnReportes.Click += new System.EventHandler(this.ComprasBtnReportes_Click);
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
            this.ComprasTlpTItuloCodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTItuloCodigo.Location = new System.Drawing.Point(130, 0);
            this.ComprasTlpTItuloCodigo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpTItuloCodigo.Name = "ComprasTlpTItuloCodigo";
            this.ComprasTlpTItuloCodigo.RowCount = 1;
            this.ComprasTlpTItuloCodigo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTItuloCodigo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpTItuloCodigo.Size = new System.Drawing.Size(670, 75);
            this.ComprasTlpTItuloCodigo.TabIndex = 6;
            // 
            // ComprasLblTitulo
            // 
            this.ComprasLblTitulo.AlineacionTexto = System.Drawing.ContentAlignment.BottomLeft;
            this.ComprasLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblTitulo.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTItuloCodigo.SetColumnSpan(this.ComprasLblTitulo, 2);
            this.ComprasLblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ComprasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTitulo.Location = new System.Drawing.Point(0, 0);
            this.ComprasLblTitulo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.ComprasLblTitulo.MaximumSize = new System.Drawing.Size(2748, 0);
            this.ComprasLblTitulo.MostrarAsterisco = false;
            this.ComprasLblTitulo.Name = "ComprasLblTitulo";
            this.ComprasLblTitulo.Size = new System.Drawing.Size(670, 65);
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
            this.ComprasLblDescripcion.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescripcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDescripcion.Font = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasLblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescripcion.Location = new System.Drawing.Point(137, 75);
            this.ComprasLblDescripcion.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.ComprasLblDescripcion.MaximumSize = new System.Drawing.Size(5500, 0);
            this.ComprasLblDescripcion.MostrarAsterisco = false;
            this.ComprasLblDescripcion.Name = "ComprasLblDescripcion";
            this.ComprasLblDescripcion.Size = new System.Drawing.Size(663, 55);
            this.ComprasLblDescripcion.TabIndex = 2;
            this.ComprasLblDescripcion.TabStop = false;
            this.ComprasLblDescripcion.Text = "Descripcion";
            this.ComprasLblDescripcion.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ComprasLblDescripcion.Texto = "Descripcion";
            // 
            // ComprasUsrEncabezado
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(1500, 130);
            this.MinimumSize = new System.Drawing.Size(0, 130);
            this.Name = "ComprasUsrEncabezado";
            this.Size = new System.Drawing.Size(1000, 130);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasTlpAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbxIcono)).EndInit();
            this.ComprasTlpTItuloCodigo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel ComprasTlpAcciones;
        private ClsButtonCompras ComprasBtnAyudas;
        private ClsButtonCompras ComprasBtnReportes;
        private ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private System.Windows.Forms.PictureBox ComprasPbxIcono;
        private ComprasUsrLabel ComprasLblDescripcion;
        private ClsTableLayoutPanelCompras ComprasTlpTItuloCodigo;
        private ComprasUsrLabel ComprasLblTitulo;
    }
}
