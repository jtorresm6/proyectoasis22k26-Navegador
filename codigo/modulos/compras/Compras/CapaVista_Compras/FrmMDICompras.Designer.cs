namespace CapaVista_Compras
{
    partial class FrmMDICompras
    {
        private System.ComponentModel.IContainer _Componentes = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Componentes != null) _Componentes.Dispose();
            base.Dispose(Disposing);
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMDICompras));
            this.ComprasTlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTlpNavegador = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasPbMarca = new System.Windows.Forms.PictureBox();
            this.ComprasTlpMovimientosProveedores = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoMovimientosProveedores = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblMovimientosProveedores = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPolizas = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoPolizas = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblPolizas = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpProveedores = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoProveedores = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblProveedores = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpOrdenesCompra = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoOrdenesCompra = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblOrdenesCompra = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPagosProveedores = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoPagosProveedores = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblPagosProveedores = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpAprobacionFinanciera = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoAprobacionFinanciera = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblAprobacionFinanciera = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpCierreMovimientos = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoCierreMovimientos = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblCierreMovimientos = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpBitacora = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoBitacora = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblBitacora = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpHerramientas = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoHerramientas = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblHerramientas = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpSalir = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnIconoSalir = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblSalir = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasFlpEncabezado = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnAlternarNavegador = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblUsuario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblRol = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlMain = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasPnlDashboard = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpDashboard = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpHero = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpPresentacion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblBienvenido = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblModulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblParrafo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasBtnAyuda = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPbMascota = new System.Windows.Forms.PictureBox();
            this.ComprasLblResumen = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpKpis = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPnlKpi1 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi1 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi1 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi1 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi1 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlKpi2 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi2 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi2 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi2 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi2 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlKpi3 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi3 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi3 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi3 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi3 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlKpi4 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi4 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi4 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi4 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi4 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlKpi5 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi5 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi5 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi5 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi5 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlKpi6 = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpKpi6 = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPbKpi6 = new System.Windows.Forms.PictureBox();
            this.ComprasLblValorKpi6 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblKpi6 = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlInformacion = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasTlpInformacion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblInformacion = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblIntegracion = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpNavegador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbMarca)).BeginInit();
            this.ComprasTlpMovimientosProveedores.SuspendLayout();
            this.ComprasTlpPolizas.SuspendLayout();
            this.ComprasTlpProveedores.SuspendLayout();
            this.ComprasTlpOrdenesCompra.SuspendLayout();
            this.ComprasTlpPagosProveedores.SuspendLayout();
            this.ComprasTlpAprobacionFinanciera.SuspendLayout();
            this.ComprasTlpCierreMovimientos.SuspendLayout();
            this.ComprasTlpBitacora.SuspendLayout();
            this.ComprasTlpHerramientas.SuspendLayout();
            this.ComprasTlpSalir.SuspendLayout();
            this.ComprasFlpEncabezado.SuspendLayout();
            this.ComprasPnlMain.SuspendLayout();
            this.ComprasPnlDashboard.SuspendLayout();
            this.ComprasTlpDashboard.SuspendLayout();
            this.ComprasTlpHero.SuspendLayout();
            this.ComprasTlpPresentacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbMascota)).BeginInit();
            this.ComprasTlpKpis.SuspendLayout();
            this.ComprasPnlKpi1.SuspendLayout();
            this.ComprasTlpKpi1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi1)).BeginInit();
            this.ComprasPnlKpi2.SuspendLayout();
            this.ComprasTlpKpi2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi2)).BeginInit();
            this.ComprasPnlKpi3.SuspendLayout();
            this.ComprasTlpKpi3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi3)).BeginInit();
            this.ComprasPnlKpi4.SuspendLayout();
            this.ComprasTlpKpi4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi4)).BeginInit();
            this.ComprasPnlKpi5.SuspendLayout();
            this.ComprasTlpKpi5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi5)).BeginInit();
            this.ComprasPnlKpi6.SuspendLayout();
            this.ComprasTlpKpi6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi6)).BeginInit();
            this.ComprasPnlInformacion.SuspendLayout();
            this.ComprasTlpInformacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 2;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 286F));
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpNavegador, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasFlpEncabezado, 1, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasPnlMain, 1, 1);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.MaximumSize = new System.Drawing.Size(1536, 1014);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.ComprasTlpPrincipal.RowCount = 2;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1536, 1014);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasTlpNavegador
            // 
            this.ComprasTlpNavegador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpNavegador.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpNavegador.BackgroundImage")));
            this.ComprasTlpNavegador.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpNavegador.ColumnCount = 1;
            this.ComprasTlpNavegador.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNavegador.Controls.Add(this.ComprasPbMarca, 0, 0);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpMovimientosProveedores, 0, 1);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpPolizas, 0, 2);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpProveedores, 0, 3);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpOrdenesCompra, 0, 4);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpPagosProveedores, 0, 5);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpAprobacionFinanciera, 0, 6);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpCierreMovimientos, 0, 7);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpBitacora, 0, 8);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpHerramientas, 0, 9);
            this.ComprasTlpNavegador.Controls.Add(this.ComprasTlpSalir, 0, 11);
            this.ComprasTlpNavegador.Dock = System.Windows.Forms.DockStyle.Left;
            this.ComprasTlpNavegador.Location = new System.Drawing.Point(14, 0);
            this.ComprasTlpNavegador.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpNavegador.MaximumSize = new System.Drawing.Size(270, 1014);
            this.ComprasTlpNavegador.MinimumSize = new System.Drawing.Size(76, 1014);
            this.ComprasTlpNavegador.Name = "ComprasTlpNavegador";
            this.ComprasTlpNavegador.Padding = new System.Windows.Forms.Padding(6, 8, 6, 37);
            this.ComprasTlpNavegador.RowCount = 13;
            this.ComprasTlpPrincipal.SetRowSpan(this.ComprasTlpNavegador, 2);
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.ComprasTlpNavegador.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNavegador.Size = new System.Drawing.Size(270, 1014);
            this.ComprasTlpNavegador.TabIndex = 0;
            // 
            // ComprasPbMarca
            // 
            this.ComprasPbMarca.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbMarca.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbMarca.BackgroundImage")));
            this.ComprasPbMarca.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbMarca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbMarca.Location = new System.Drawing.Point(70, 8);
            this.ComprasPbMarca.Margin = new System.Windows.Forms.Padding(64, 0, 64, 10);
            this.ComprasPbMarca.Name = "ComprasPbMarca";
            this.ComprasPbMarca.Size = new System.Drawing.Size(130, 90);
            this.ComprasPbMarca.TabIndex = 0;
            this.ComprasPbMarca.TabStop = false;
            // 
            // ComprasTlpMovimientosProveedores
            // 
            this.ComprasTlpMovimientosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpMovimientosProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpMovimientosProveedores.BackgroundImage")));
            this.ComprasTlpMovimientosProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpMovimientosProveedores.ColumnCount = 2;
            this.ComprasTlpMovimientosProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpMovimientosProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMovimientosProveedores.Controls.Add(this.ComprasBtnIconoMovimientosProveedores, 0, 0);
            this.ComprasTlpMovimientosProveedores.Controls.Add(this.ComprasLblMovimientosProveedores, 1, 0);
            this.ComprasTlpMovimientosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpMovimientosProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMovimientosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpMovimientosProveedores.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpMovimientosProveedores.ImagenHover")));
            this.ComprasTlpMovimientosProveedores.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpMovimientosProveedores.ImagenNormal")));
            this.ComprasTlpMovimientosProveedores.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpMovimientosProveedores.ImagenSeleccionado")));
            this.ComprasTlpMovimientosProveedores.Location = new System.Drawing.Point(6, 110);
            this.ComprasTlpMovimientosProveedores.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpMovimientosProveedores.Name = "ComprasTlpMovimientosProveedores";
            this.ComprasTlpMovimientosProveedores.RowCount = 1;
            this.ComprasTlpMovimientosProveedores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMovimientosProveedores.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpMovimientosProveedores.TabIndex = 1;
            this.ComprasTlpMovimientosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirMovimientosProveedores);
            this.ComprasBtnIconoMovimientosProveedores.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoMovimientosProveedores
            // 
            this.ComprasBtnIconoMovimientosProveedores.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnIconoMovimientosProveedores.AplicarColoresEstado = false;
            this.ComprasBtnIconoMovimientosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoMovimientosProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoMovimientosProveedores.BackgroundImage")));
            this.ComprasBtnIconoMovimientosProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoMovimientosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoMovimientosProveedores.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoMovimientosProveedores.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoMovimientosProveedores.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoMovimientosProveedores.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoMovimientosProveedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoMovimientosProveedores.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnIconoMovimientosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoMovimientosProveedores.Location = new System.Drawing.Point(2, 6);
            this.ComprasBtnIconoMovimientosProveedores.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnIconoMovimientosProveedores.Name = "ComprasBtnIconoMovimientosProveedores";
            this.ComprasBtnIconoMovimientosProveedores.Size = new System.Drawing.Size(60, 49);
            this.ComprasBtnIconoMovimientosProveedores.TabIndex = 0;
            this.ComprasBtnIconoMovimientosProveedores.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoMovimientosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirMovimientosProveedores);
            // 
            // ComprasLblMovimientosProveedores
            // 
            this.ComprasLblMovimientosProveedores.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMovimientosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblMovimientosProveedores.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMovimientosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblMovimientosProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMovimientosProveedores.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblMovimientosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMovimientosProveedores.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblMovimientosProveedores.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblMovimientosProveedores.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblMovimientosProveedores.MostrarAsterisco = false;
            this.ComprasLblMovimientosProveedores.Name = "ComprasLblMovimientosProveedores";
            this.ComprasLblMovimientosProveedores.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblMovimientosProveedores.TabIndex = 1;
            this.ComprasLblMovimientosProveedores.TabStop = false;
            this.ComprasLblMovimientosProveedores.TamanoTipografia = 10F;
            this.ComprasLblMovimientosProveedores.Text = "Movimientos de Proveedores";
            this.ComprasLblMovimientosProveedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMovimientosProveedores.Texto = "Movimientos de Proveedores";
            this.ComprasLblMovimientosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirMovimientosProveedores);
            // 
            // ComprasTlpPolizas
            // 
            this.ComprasTlpPolizas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpPolizas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPolizas.BackgroundImage")));
            this.ComprasTlpPolizas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpPolizas.ColumnCount = 2;
            this.ComprasTlpPolizas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpPolizas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPolizas.Controls.Add(this.ComprasBtnIconoPolizas, 0, 0);
            this.ComprasTlpPolizas.Controls.Add(this.ComprasLblPolizas, 1, 0);
            this.ComprasTlpPolizas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpPolizas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPolizas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPolizas.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPolizas.ImagenHover")));
            this.ComprasTlpPolizas.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPolizas.ImagenNormal")));
            this.ComprasTlpPolizas.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPolizas.ImagenSeleccionado")));
            this.ComprasTlpPolizas.Location = new System.Drawing.Point(6, 175);
            this.ComprasTlpPolizas.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpPolizas.Name = "ComprasTlpPolizas";
            this.ComprasTlpPolizas.RowCount = 1;
            this.ComprasTlpPolizas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPolizas.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpPolizas.TabIndex = 2;
            this.ComprasTlpPolizas.Click += new System.EventHandler(this.ComprasMetAbrirPolizas);
            this.ComprasBtnIconoPolizas.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoPolizas
            // 
            this.ComprasBtnIconoPolizas.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoPolizas.AplicarColoresEstado = false;
            this.ComprasBtnIconoPolizas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPolizas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoPolizas.BackgroundImage")));
            this.ComprasBtnIconoPolizas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoPolizas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoPolizas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoPolizas.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoPolizas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPolizas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPolizas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoPolizas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnIconoPolizas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoPolizas.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoPolizas.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoPolizas.Name = "ComprasBtnIconoPolizas";
            this.ComprasBtnIconoPolizas.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoPolizas.TabIndex = 0;
            this.ComprasBtnIconoPolizas.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoPolizas.Click += new System.EventHandler(this.ComprasMetAbrirPolizas);
            // 
            // ComprasLblPolizas
            // 
            this.ComprasLblPolizas.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPolizas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblPolizas.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPolizas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblPolizas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblPolizas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblPolizas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPolizas.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblPolizas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblPolizas.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblPolizas.MostrarAsterisco = false;
            this.ComprasLblPolizas.Name = "ComprasLblPolizas";
            this.ComprasLblPolizas.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblPolizas.TabIndex = 1;
            this.ComprasLblPolizas.TabStop = false;
            this.ComprasLblPolizas.TamanoTipografia = 10F;
            this.ComprasLblPolizas.Text = "Pólizas";
            this.ComprasLblPolizas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPolizas.Texto = "Pólizas";
            this.ComprasLblPolizas.Click += new System.EventHandler(this.ComprasMetAbrirPolizas);
            // 
            // ComprasTlpProveedores
            // 
            this.ComprasTlpProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpProveedores.BackgroundImage")));
            this.ComprasTlpProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpProveedores.ColumnCount = 2;
            this.ComprasTlpProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedores.Controls.Add(this.ComprasBtnIconoProveedores, 0, 0);
            this.ComprasTlpProveedores.Controls.Add(this.ComprasLblProveedores, 1, 0);
            this.ComprasTlpProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpProveedores.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpProveedores.ImagenHover")));
            this.ComprasTlpProveedores.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpProveedores.ImagenNormal")));
            this.ComprasTlpProveedores.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpProveedores.ImagenSeleccionado")));
            this.ComprasTlpProveedores.Location = new System.Drawing.Point(6, 240);
            this.ComprasTlpProveedores.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpProveedores.Name = "ComprasTlpProveedores";
            this.ComprasTlpProveedores.RowCount = 1;
            this.ComprasTlpProveedores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedores.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpProveedores.TabIndex = 3;
            this.ComprasTlpProveedores.Click += new System.EventHandler(this.ComprasMetAbrirProveedores);
            this.ComprasBtnIconoProveedores.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoProveedores
            // 
            this.ComprasBtnIconoProveedores.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoProveedores.AplicarColoresEstado = false;
            this.ComprasBtnIconoProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoProveedores.BackgroundImage")));
            this.ComprasBtnIconoProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoProveedores.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoProveedores.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoProveedores.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoProveedores.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoProveedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoProveedores.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoProveedores.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoProveedores.Name = "ComprasBtnIconoProveedores";
            this.ComprasBtnIconoProveedores.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoProveedores.TabIndex = 0;
            this.ComprasBtnIconoProveedores.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoProveedores.Click += new System.EventHandler(this.ComprasMetAbrirProveedores);
            // 
            // ComprasLblProveedores
            // 
            this.ComprasLblProveedores.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblProveedores.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblProveedores.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedores.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblProveedores.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblProveedores.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblProveedores.MostrarAsterisco = false;
            this.ComprasLblProveedores.Name = "ComprasLblProveedores";
            this.ComprasLblProveedores.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblProveedores.TabIndex = 1;
            this.ComprasLblProveedores.TabStop = false;
            this.ComprasLblProveedores.TamanoTipografia = 10F;
            this.ComprasLblProveedores.Text = "Proveedores";
            this.ComprasLblProveedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedores.Texto = "Proveedores";
            this.ComprasLblProveedores.Click += new System.EventHandler(this.ComprasMetAbrirProveedores);
            // 
            // ComprasTlpOrdenesCompra
            // 
            this.ComprasTlpOrdenesCompra.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpOrdenesCompra.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpOrdenesCompra.BackgroundImage")));
            this.ComprasTlpOrdenesCompra.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpOrdenesCompra.ColumnCount = 2;
            this.ComprasTlpOrdenesCompra.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpOrdenesCompra.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOrdenesCompra.Controls.Add(this.ComprasBtnIconoOrdenesCompra, 0, 0);
            this.ComprasTlpOrdenesCompra.Controls.Add(this.ComprasLblOrdenesCompra, 1, 0);
            this.ComprasTlpOrdenesCompra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpOrdenesCompra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpOrdenesCompra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpOrdenesCompra.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpOrdenesCompra.ImagenHover")));
            this.ComprasTlpOrdenesCompra.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpOrdenesCompra.ImagenNormal")));
            this.ComprasTlpOrdenesCompra.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpOrdenesCompra.ImagenSeleccionado")));
            this.ComprasTlpOrdenesCompra.Location = new System.Drawing.Point(6, 305);
            this.ComprasTlpOrdenesCompra.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpOrdenesCompra.Name = "ComprasTlpOrdenesCompra";
            this.ComprasTlpOrdenesCompra.RowCount = 1;
            this.ComprasTlpOrdenesCompra.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOrdenesCompra.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpOrdenesCompra.TabIndex = 5;
            this.ComprasTlpOrdenesCompra.Click += new System.EventHandler(this.ComprasMetAbrirOrdenesCompra);
            this.ComprasBtnIconoOrdenesCompra.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoOrdenesCompra
            // 
            this.ComprasBtnIconoOrdenesCompra.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoOrdenesCompra.AplicarColoresEstado = false;
            this.ComprasBtnIconoOrdenesCompra.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoOrdenesCompra.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoOrdenesCompra.BackgroundImage")));
            this.ComprasBtnIconoOrdenesCompra.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoOrdenesCompra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoOrdenesCompra.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoOrdenesCompra.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoOrdenesCompra.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoOrdenesCompra.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoOrdenesCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoOrdenesCompra.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoOrdenesCompra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoOrdenesCompra.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoOrdenesCompra.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoOrdenesCompra.Name = "ComprasBtnIconoOrdenesCompra";
            this.ComprasBtnIconoOrdenesCompra.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoOrdenesCompra.TabIndex = 0;
            this.ComprasBtnIconoOrdenesCompra.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoOrdenesCompra.Click += new System.EventHandler(this.ComprasMetAbrirOrdenesCompra);
            // 
            // ComprasLblOrdenesCompra
            // 
            this.ComprasLblOrdenesCompra.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblOrdenesCompra.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblOrdenesCompra.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblOrdenesCompra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblOrdenesCompra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblOrdenesCompra.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblOrdenesCompra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblOrdenesCompra.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblOrdenesCompra.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblOrdenesCompra.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblOrdenesCompra.MostrarAsterisco = false;
            this.ComprasLblOrdenesCompra.Name = "ComprasLblOrdenesCompra";
            this.ComprasLblOrdenesCompra.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblOrdenesCompra.TabIndex = 1;
            this.ComprasLblOrdenesCompra.TabStop = false;
            this.ComprasLblOrdenesCompra.TamanoTipografia = 10F;
            this.ComprasLblOrdenesCompra.Text = "Órdenes de Compra";
            this.ComprasLblOrdenesCompra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblOrdenesCompra.Texto = "Órdenes de Compra";
            this.ComprasLblOrdenesCompra.Click += new System.EventHandler(this.ComprasMetAbrirOrdenesCompra);
            // 
            // ComprasTlpPagosProveedores
            // 
            this.ComprasTlpPagosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpPagosProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPagosProveedores.BackgroundImage")));
            this.ComprasTlpPagosProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpPagosProveedores.ColumnCount = 2;
            this.ComprasTlpPagosProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpPagosProveedores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPagosProveedores.Controls.Add(this.ComprasBtnIconoPagosProveedores, 0, 0);
            this.ComprasTlpPagosProveedores.Controls.Add(this.ComprasLblPagosProveedores, 1, 0);
            this.ComprasTlpPagosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpPagosProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPagosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPagosProveedores.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPagosProveedores.ImagenHover")));
            this.ComprasTlpPagosProveedores.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPagosProveedores.ImagenNormal")));
            this.ComprasTlpPagosProveedores.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpPagosProveedores.ImagenSeleccionado")));
            this.ComprasTlpPagosProveedores.Location = new System.Drawing.Point(6, 370);
            this.ComprasTlpPagosProveedores.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpPagosProveedores.Name = "ComprasTlpPagosProveedores";
            this.ComprasTlpPagosProveedores.RowCount = 1;
            this.ComprasTlpPagosProveedores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPagosProveedores.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpPagosProveedores.TabIndex = 6;
            this.ComprasTlpPagosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirPagosProveedores);
            this.ComprasBtnIconoPagosProveedores.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoPagosProveedores
            // 
            this.ComprasBtnIconoPagosProveedores.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoPagosProveedores.AplicarColoresEstado = false;
            this.ComprasBtnIconoPagosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPagosProveedores.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoPagosProveedores.BackgroundImage")));
            this.ComprasBtnIconoPagosProveedores.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoPagosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoPagosProveedores.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoPagosProveedores.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoPagosProveedores.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPagosProveedores.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoPagosProveedores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoPagosProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoPagosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoPagosProveedores.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoPagosProveedores.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoPagosProveedores.Name = "ComprasBtnIconoPagosProveedores";
            this.ComprasBtnIconoPagosProveedores.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoPagosProveedores.TabIndex = 0;
            this.ComprasBtnIconoPagosProveedores.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoPagosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirPagosProveedores);
            // 
            // ComprasLblPagosProveedores
            // 
            this.ComprasLblPagosProveedores.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPagosProveedores.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblPagosProveedores.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPagosProveedores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblPagosProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblPagosProveedores.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblPagosProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPagosProveedores.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblPagosProveedores.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblPagosProveedores.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblPagosProveedores.MostrarAsterisco = false;
            this.ComprasLblPagosProveedores.Name = "ComprasLblPagosProveedores";
            this.ComprasLblPagosProveedores.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblPagosProveedores.TabIndex = 1;
            this.ComprasLblPagosProveedores.TabStop = false;
            this.ComprasLblPagosProveedores.TamanoTipografia = 10F;
            this.ComprasLblPagosProveedores.Text = "Pagos a Proveedores";
            this.ComprasLblPagosProveedores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPagosProveedores.Texto = "Pagos a Proveedores";
            this.ComprasLblPagosProveedores.Click += new System.EventHandler(this.ComprasMetAbrirPagosProveedores);
            // 
            // ComprasTlpAprobacionFinanciera
            // 
            this.ComprasTlpAprobacionFinanciera.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpAprobacionFinanciera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpAprobacionFinanciera.BackgroundImage")));
            this.ComprasTlpAprobacionFinanciera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpAprobacionFinanciera.ColumnCount = 2;
            this.ComprasTlpAprobacionFinanciera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpAprobacionFinanciera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAprobacionFinanciera.Controls.Add(this.ComprasBtnIconoAprobacionFinanciera, 0, 0);
            this.ComprasTlpAprobacionFinanciera.Controls.Add(this.ComprasLblAprobacionFinanciera, 1, 0);
            this.ComprasTlpAprobacionFinanciera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpAprobacionFinanciera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAprobacionFinanciera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpAprobacionFinanciera.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpAprobacionFinanciera.ImagenHover")));
            this.ComprasTlpAprobacionFinanciera.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpAprobacionFinanciera.ImagenNormal")));
            this.ComprasTlpAprobacionFinanciera.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpAprobacionFinanciera.ImagenSeleccionado")));
            this.ComprasTlpAprobacionFinanciera.Location = new System.Drawing.Point(6, 435);
            this.ComprasTlpAprobacionFinanciera.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpAprobacionFinanciera.Name = "ComprasTlpAprobacionFinanciera";
            this.ComprasTlpAprobacionFinanciera.RowCount = 1;
            this.ComprasTlpAprobacionFinanciera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAprobacionFinanciera.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpAprobacionFinanciera.TabIndex = 7;
            this.ComprasTlpAprobacionFinanciera.Click += new System.EventHandler(this.ComprasMetAbrirAprobacionFinanciera);
            this.ComprasBtnIconoAprobacionFinanciera.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoAprobacionFinanciera
            // 
            this.ComprasBtnIconoAprobacionFinanciera.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoAprobacionFinanciera.AplicarColoresEstado = false;
            this.ComprasBtnIconoAprobacionFinanciera.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoAprobacionFinanciera.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoAprobacionFinanciera.BackgroundImage")));
            this.ComprasBtnIconoAprobacionFinanciera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoAprobacionFinanciera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoAprobacionFinanciera.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoAprobacionFinanciera.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoAprobacionFinanciera.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoAprobacionFinanciera.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoAprobacionFinanciera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoAprobacionFinanciera.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoAprobacionFinanciera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoAprobacionFinanciera.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoAprobacionFinanciera.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoAprobacionFinanciera.Name = "ComprasBtnIconoAprobacionFinanciera";
            this.ComprasBtnIconoAprobacionFinanciera.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoAprobacionFinanciera.TabIndex = 0;
            this.ComprasBtnIconoAprobacionFinanciera.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoAprobacionFinanciera.Click += new System.EventHandler(this.ComprasMetAbrirAprobacionFinanciera);
            // 
            // ComprasLblAprobacionFinanciera
            // 
            this.ComprasLblAprobacionFinanciera.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblAprobacionFinanciera.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblAprobacionFinanciera.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblAprobacionFinanciera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblAprobacionFinanciera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblAprobacionFinanciera.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblAprobacionFinanciera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblAprobacionFinanciera.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblAprobacionFinanciera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblAprobacionFinanciera.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblAprobacionFinanciera.MostrarAsterisco = false;
            this.ComprasLblAprobacionFinanciera.Name = "ComprasLblAprobacionFinanciera";
            this.ComprasLblAprobacionFinanciera.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblAprobacionFinanciera.TabIndex = 1;
            this.ComprasLblAprobacionFinanciera.TabStop = false;
            this.ComprasLblAprobacionFinanciera.TamanoTipografia = 10F;
            this.ComprasLblAprobacionFinanciera.Text = "Aprobación Financiera";
            this.ComprasLblAprobacionFinanciera.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblAprobacionFinanciera.Texto = "Aprobación Financiera";
            this.ComprasLblAprobacionFinanciera.Click += new System.EventHandler(this.ComprasMetAbrirAprobacionFinanciera);
            // 
            // ComprasTlpCierreMovimientos
            // 
            this.ComprasTlpCierreMovimientos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpCierreMovimientos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpCierreMovimientos.BackgroundImage")));
            this.ComprasTlpCierreMovimientos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpCierreMovimientos.ColumnCount = 2;
            this.ComprasTlpCierreMovimientos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpCierreMovimientos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpCierreMovimientos.Controls.Add(this.ComprasBtnIconoCierreMovimientos, 0, 0);
            this.ComprasTlpCierreMovimientos.Controls.Add(this.ComprasLblCierreMovimientos, 1, 0);
            this.ComprasTlpCierreMovimientos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpCierreMovimientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpCierreMovimientos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpCierreMovimientos.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpCierreMovimientos.ImagenHover")));
            this.ComprasTlpCierreMovimientos.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpCierreMovimientos.ImagenNormal")));
            this.ComprasTlpCierreMovimientos.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpCierreMovimientos.ImagenSeleccionado")));
            this.ComprasTlpCierreMovimientos.Location = new System.Drawing.Point(6, 500);
            this.ComprasTlpCierreMovimientos.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpCierreMovimientos.Name = "ComprasTlpCierreMovimientos";
            this.ComprasTlpCierreMovimientos.RowCount = 1;
            this.ComprasTlpCierreMovimientos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpCierreMovimientos.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpCierreMovimientos.TabIndex = 8;
            this.ComprasTlpCierreMovimientos.Click += new System.EventHandler(this.ComprasMetAbrirCierreMovimientos);
            this.ComprasBtnIconoCierreMovimientos.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoCierreMovimientos
            // 
            this.ComprasBtnIconoCierreMovimientos.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoCierreMovimientos.AplicarColoresEstado = false;
            this.ComprasBtnIconoCierreMovimientos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoCierreMovimientos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoCierreMovimientos.BackgroundImage")));
            this.ComprasBtnIconoCierreMovimientos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoCierreMovimientos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoCierreMovimientos.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoCierreMovimientos.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoCierreMovimientos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoCierreMovimientos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoCierreMovimientos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoCierreMovimientos.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoCierreMovimientos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoCierreMovimientos.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoCierreMovimientos.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoCierreMovimientos.Name = "ComprasBtnIconoCierreMovimientos";
            this.ComprasBtnIconoCierreMovimientos.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoCierreMovimientos.TabIndex = 0;
            this.ComprasBtnIconoCierreMovimientos.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoCierreMovimientos.Click += new System.EventHandler(this.ComprasMetAbrirCierreMovimientos);
            // 
            // ComprasLblCierreMovimientos
            // 
            this.ComprasLblCierreMovimientos.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblCierreMovimientos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblCierreMovimientos.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCierreMovimientos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblCierreMovimientos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCierreMovimientos.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblCierreMovimientos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCierreMovimientos.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblCierreMovimientos.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblCierreMovimientos.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblCierreMovimientos.MostrarAsterisco = false;
            this.ComprasLblCierreMovimientos.Name = "ComprasLblCierreMovimientos";
            this.ComprasLblCierreMovimientos.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblCierreMovimientos.TabIndex = 1;
            this.ComprasLblCierreMovimientos.TabStop = false;
            this.ComprasLblCierreMovimientos.TamanoTipografia = 10F;
            this.ComprasLblCierreMovimientos.Text = "Cierre de Movimientos";
            this.ComprasLblCierreMovimientos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblCierreMovimientos.Texto = "Cierre de Movimientos";
            this.ComprasLblCierreMovimientos.Click += new System.EventHandler(this.ComprasMetAbrirCierreMovimientos);
            // 
            // ComprasTlpBitacora
            // 
            this.ComprasTlpBitacora.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpBitacora.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpBitacora.BackgroundImage")));
            this.ComprasTlpBitacora.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpBitacora.ColumnCount = 2;
            this.ComprasTlpBitacora.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpBitacora.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBitacora.Controls.Add(this.ComprasBtnIconoBitacora, 0, 0);
            this.ComprasTlpBitacora.Controls.Add(this.ComprasLblBitacora, 1, 0);
            this.ComprasTlpBitacora.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBitacora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpBitacora.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpBitacora.ImagenHover")));
            this.ComprasTlpBitacora.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpBitacora.ImagenNormal")));
            this.ComprasTlpBitacora.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpBitacora.ImagenSeleccionado")));
            this.ComprasTlpBitacora.Location = new System.Drawing.Point(6, 565);
            this.ComprasTlpBitacora.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpBitacora.Name = "ComprasTlpBitacora";
            this.ComprasTlpBitacora.RowCount = 1;
            this.ComprasTlpBitacora.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBitacora.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpBitacora.TabIndex = 9;
            this.ComprasTlpBitacora.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            this.ComprasBtnIconoBitacora.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoBitacora
            // 
            this.ComprasBtnIconoBitacora.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoBitacora.AplicarColoresEstado = false;
            this.ComprasBtnIconoBitacora.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoBitacora.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoBitacora.BackgroundImage")));
            this.ComprasBtnIconoBitacora.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoBitacora.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoBitacora.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoBitacora.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoBitacora.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoBitacora.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoBitacora.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoBitacora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoBitacora.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoBitacora.ImagenDeshabilitado")));
            this.ComprasBtnIconoBitacora.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoBitacora.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoBitacora.Name = "ComprasBtnIconoBitacora";
            this.ComprasBtnIconoBitacora.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoBitacora.TabIndex = 0;
            this.ComprasBtnIconoBitacora.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoBitacora.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            // 
            // ComprasLblBitacora
            // 
            this.ComprasLblBitacora.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBitacora.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblBitacora.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblBitacora.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblBitacora.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblBitacora.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblBitacora.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblBitacora.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblBitacora.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblBitacora.MostrarAsterisco = false;
            this.ComprasLblBitacora.Name = "ComprasLblBitacora";
            this.ComprasLblBitacora.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblBitacora.TabIndex = 1;
            this.ComprasLblBitacora.TabStop = false;
            this.ComprasLblBitacora.TamanoTipografia = 10F;
            this.ComprasLblBitacora.Text = "Bitácora";
            this.ComprasLblBitacora.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBitacora.Texto = "Bitácora";
            this.ComprasLblBitacora.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            // 
            // ComprasTlpHerramientas
            // 
            this.ComprasTlpHerramientas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpHerramientas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpHerramientas.BackgroundImage")));
            this.ComprasTlpHerramientas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpHerramientas.ColumnCount = 2;
            this.ComprasTlpHerramientas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpHerramientas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpHerramientas.Controls.Add(this.ComprasBtnIconoHerramientas, 0, 0);
            this.ComprasTlpHerramientas.Controls.Add(this.ComprasLblHerramientas, 1, 0);
            this.ComprasTlpHerramientas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpHerramientas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpHerramientas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpHerramientas.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpHerramientas.ImagenHover")));
            this.ComprasTlpHerramientas.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpHerramientas.ImagenNormal")));
            this.ComprasTlpHerramientas.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpHerramientas.ImagenSeleccionado")));
            this.ComprasTlpHerramientas.Location = new System.Drawing.Point(6, 630);
            this.ComprasTlpHerramientas.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpHerramientas.Name = "ComprasTlpHerramientas";
            this.ComprasTlpHerramientas.RowCount = 1;
            this.ComprasTlpHerramientas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpHerramientas.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpHerramientas.TabIndex = 9;
            this.ComprasTlpHerramientas.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            this.ComprasBtnIconoHerramientas.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoHerramientas
            // 
            this.ComprasBtnIconoHerramientas.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoHerramientas.AplicarColoresEstado = false;
            this.ComprasBtnIconoHerramientas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoHerramientas.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoHerramientas.BackgroundImage")));
            this.ComprasBtnIconoHerramientas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoHerramientas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoHerramientas.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoHerramientas.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoHerramientas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoHerramientas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoHerramientas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoHerramientas.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoHerramientas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoHerramientas.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoHerramientas.ImagenDeshabilitado")));
            this.ComprasBtnIconoHerramientas.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoHerramientas.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoHerramientas.Name = "ComprasBtnIconoHerramientas";
            this.ComprasBtnIconoHerramientas.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoHerramientas.TabIndex = 0;
            this.ComprasBtnIconoHerramientas.Text = "?";
            this.ComprasBtnIconoHerramientas.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoHerramientas.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            // 
            // ComprasLblHerramientas
            // 
            this.ComprasLblHerramientas.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblHerramientas.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblHerramientas.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblHerramientas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblHerramientas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblHerramientas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblHerramientas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblHerramientas.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblHerramientas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblHerramientas.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblHerramientas.MostrarAsterisco = false;
            this.ComprasLblHerramientas.Name = "ComprasLblHerramientas";
            this.ComprasLblHerramientas.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblHerramientas.TabIndex = 1;
            this.ComprasLblHerramientas.TabStop = false;
            this.ComprasLblHerramientas.TamanoTipografia = 10F;
            this.ComprasLblHerramientas.Text = "Herramientas";
            this.ComprasLblHerramientas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblHerramientas.Texto = "Herramientas";
            this.ComprasLblHerramientas.Click += new System.EventHandler(this.ComprasMetOpcionPendienteClick);
            // 
            // ComprasTlpSalir
            // 
            this.ComprasTlpSalir.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpSalir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasTlpSalir.BackgroundImage")));
            this.ComprasTlpSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasTlpSalir.ColumnCount = 2;
            this.ComprasTlpSalir.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.ComprasTlpSalir.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpSalir.Controls.Add(this.ComprasBtnIconoSalir, 0, 0);
            this.ComprasTlpSalir.Controls.Add(this.ComprasLblSalir, 1, 0);
            this.ComprasTlpSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasTlpSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpSalir.ImagenHover = ((System.Drawing.Image)(resources.GetObject("ComprasTlpSalir.ImagenHover")));
            this.ComprasTlpSalir.ImagenNormal = ((System.Drawing.Image)(resources.GetObject("ComprasTlpSalir.ImagenNormal")));
            this.ComprasTlpSalir.ImagenSeleccionado = ((System.Drawing.Image)(resources.GetObject("ComprasTlpSalir.ImagenSeleccionado")));
            this.ComprasTlpSalir.Location = new System.Drawing.Point(6, 705);
            this.ComprasTlpSalir.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ComprasTlpSalir.Name = "ComprasTlpSalir";
            this.ComprasTlpSalir.RowCount = 1;
            this.ComprasTlpSalir.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpSalir.Size = new System.Drawing.Size(258, 61);
            this.ComprasTlpSalir.TabIndex = 10;
            this.ComprasTlpSalir.Click += new System.EventHandler(this.ComprasProcSalirClick);
            this.ComprasBtnIconoSalir.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnIconoSalir
            // 
            this.ComprasBtnIconoSalir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasBtnIconoSalir.AplicarColoresEstado = false;
            this.ComprasBtnIconoSalir.BackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoSalir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIconoSalir.BackgroundImage")));
            this.ComprasBtnIconoSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnIconoSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIconoSalir.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ComprasBtnIconoSalir.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIconoSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.ComprasBtnIconoSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIconoSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIconoSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIconoSalir.Location = new System.Drawing.Point(5, 6);
            this.ComprasBtnIconoSalir.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ComprasBtnIconoSalir.Name = "ComprasBtnIconoSalir";
            this.ComprasBtnIconoSalir.Size = new System.Drawing.Size(54, 49);
            this.ComprasBtnIconoSalir.TabIndex = 0;
            this.ComprasBtnIconoSalir.UseVisualStyleBackColor = false;
            this.ComprasBtnIconoSalir.Click += new System.EventHandler(this.ComprasProcSalirClick);
            // 
            // ComprasLblSalir
            // 
            this.ComprasLblSalir.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblSalir.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblSalir.ColorTexto = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasLblSalir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblSalir.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSalir.Location = new System.Drawing.Point(67, 4);
            this.ComprasLblSalir.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblSalir.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblSalir.MostrarAsterisco = false;
            this.ComprasLblSalir.Name = "ComprasLblSalir";
            this.ComprasLblSalir.Size = new System.Drawing.Size(188, 53);
            this.ComprasLblSalir.TabIndex = 1;
            this.ComprasLblSalir.TabStop = false;
            this.ComprasLblSalir.TamanoTipografia = 10F;
            this.ComprasLblSalir.Text = "Salir";
            this.ComprasLblSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblSalir.Texto = "Salir";
            this.ComprasLblSalir.Click += new System.EventHandler(this.ComprasProcSalirClick);
            // 
            // ComprasFlpEncabezado
            // 
            this.ComprasFlpEncabezado.AutoScroll = true;
            this.ComprasFlpEncabezado.Controls.Add(this.ComprasBtnAlternarNavegador);
            this.ComprasFlpEncabezado.Controls.Add(this.ComprasLblUsuario);
            this.ComprasFlpEncabezado.Controls.Add(this.ComprasLblRol);
            this.ComprasFlpEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasFlpEncabezado.Location = new System.Drawing.Point(303, 3);
            this.ComprasFlpEncabezado.Name = "ComprasFlpEncabezado";
            this.ComprasFlpEncabezado.Size = new System.Drawing.Size(1216, 40);
            this.ComprasFlpEncabezado.TabIndex = 1;
            this.ComprasFlpEncabezado.WrapContents = false;
            this.ComprasBtnAlternarNavegador.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnAlternarNavegador
            // 
            this.ComprasBtnAlternarNavegador.AccessibleName = "Contraer menú";
            this.ComprasBtnAlternarNavegador.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnAlternarNavegador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasBtnAlternarNavegador.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAlternarNavegador.BackgroundImage")));
            this.ComprasBtnAlternarNavegador.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasBtnAlternarNavegador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnAlternarNavegador.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAlternarNavegador.FlatAppearance.BorderSize = 0;
            this.ComprasBtnAlternarNavegador.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnAlternarNavegador.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnAlternarNavegador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnAlternarNavegador.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnAlternarNavegador.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAlternarNavegador.Location = new System.Drawing.Point(0, 0);
            this.ComprasBtnAlternarNavegador.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnAlternarNavegador.MaximumSize = new System.Drawing.Size(58, 40);
            this.ComprasBtnAlternarNavegador.MinimumSize = new System.Drawing.Size(58, 40);
            this.ComprasBtnAlternarNavegador.Name = "ComprasBtnAlternarNavegador";
            this.ComprasBtnAlternarNavegador.Size = new System.Drawing.Size(58, 40);
            this.ComprasBtnAlternarNavegador.TabIndex = 0;
            this.ComprasBtnAlternarNavegador.UseVisualStyleBackColor = false;
            this.ComprasBtnAlternarNavegador.Click += new System.EventHandler(this.ComprasProcAlternarMenuClick);
            // 
            // ComprasLblUsuario
            // 
            this.ComprasLblUsuario.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblUsuario.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblUsuario.Location = new System.Drawing.Point(62, 4);
            this.ComprasLblUsuario.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComprasLblUsuario.MaximumSize = new System.Drawing.Size(1375, 0);
            this.ComprasLblUsuario.MostrarAsterisco = false;
            this.ComprasLblUsuario.Name = "ComprasLblUsuario";
            this.ComprasLblUsuario.Size = new System.Drawing.Size(180, 32);
            this.ComprasLblUsuario.TabIndex = 1;
            this.ComprasLblUsuario.TabStop = false;
            this.ComprasLblUsuario.TamanoTipografia = 12F;
            this.ComprasLblUsuario.Text = "Usuario:";
            this.ComprasLblUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblUsuario.Texto = "Usuario:";
            // 
            // ComprasLblRol
            // 
            this.ComprasLblRol.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblRol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblRol.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblRol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblRol.Location = new System.Drawing.Point(250, 4);
            this.ComprasLblRol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComprasLblRol.MaximumSize = new System.Drawing.Size(1375, 0);
            this.ComprasLblRol.MostrarAsterisco = false;
            this.ComprasLblRol.Name = "ComprasLblRol";
            this.ComprasLblRol.Size = new System.Drawing.Size(180, 32);
            this.ComprasLblRol.TabIndex = 2;
            this.ComprasLblRol.TabStop = false;
            this.ComprasLblRol.TamanoTipografia = 12F;
            this.ComprasLblRol.Text = "Rol:";
            this.ComprasLblRol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblRol.Texto = "Rol:";
            // 
            // ComprasPnlMain
            // 
            this.ComprasPnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(247)))), ((int)(((byte)(242)))));
            this.ComprasPnlMain.Controls.Add(this.ComprasPnlDashboard);
            this.ComprasPnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlMain.Location = new System.Drawing.Point(300, 51);
            this.ComprasPnlMain.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ComprasPnlMain.Name = "ComprasPnlMain";
            this.ComprasPnlMain.Size = new System.Drawing.Size(1222, 963);
            this.ComprasPnlMain.TabIndex = 2;
            // 
            // ComprasPnlDashboard
            // 
            this.ComprasPnlDashboard.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPnlDashboard.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPnlDashboard.BackgroundImage")));
            this.ComprasPnlDashboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlDashboard.Controls.Add(this.ComprasTlpDashboard);
            this.ComprasPnlDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlDashboard.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDashboard.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ComprasPnlDashboard.Name = "ComprasPnlDashboard";
            this.ComprasPnlDashboard.Padding = new System.Windows.Forms.Padding(24, 0, 24, 26);
            this.ComprasPnlDashboard.Size = new System.Drawing.Size(1222, 963);
            this.ComprasPnlDashboard.TabIndex = 3;
            // 
            // ComprasTlpDashboard
            // 
            this.ComprasTlpDashboard.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpDashboard.ColumnCount = 1;
            this.ComprasTlpDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDashboard.Controls.Add(this.ComprasTlpHero, 0, 0);
            this.ComprasTlpDashboard.Controls.Add(this.ComprasLblResumen, 0, 1);
            this.ComprasTlpDashboard.Controls.Add(this.ComprasTlpKpis, 0, 2);
            this.ComprasTlpDashboard.Controls.Add(this.ComprasPnlInformacion, 0, 3);
            this.ComprasTlpDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDashboard.Location = new System.Drawing.Point(24, 0);
            this.ComprasTlpDashboard.Name = "ComprasTlpDashboard";
            this.ComprasTlpDashboard.RowCount = 4;
            this.ComprasTlpDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.ComprasTlpDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 176F));
            this.ComprasTlpDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.ComprasTlpDashboard.Size = new System.Drawing.Size(1174, 937);
            this.ComprasTlpDashboard.TabIndex = 0;
            // 
            // ComprasTlpHero
            // 
            this.ComprasTlpHero.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpHero.ColumnCount = 2;
            this.ComprasTlpHero.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.ComprasTlpHero.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpHero.Controls.Add(this.ComprasTlpPresentacion, 0, 0);
            this.ComprasTlpHero.Controls.Add(this.ComprasPbMascota, 1, 0);
            this.ComprasTlpHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpHero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpHero.Location = new System.Drawing.Point(3, 3);
            this.ComprasTlpHero.Name = "ComprasTlpHero";
            this.ComprasTlpHero.RowCount = 1;
            this.ComprasTlpHero.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpHero.Size = new System.Drawing.Size(1168, 638);
            this.ComprasTlpHero.TabIndex = 0;
            // 
            // ComprasTlpPresentacion
            // 
            this.ComprasTlpPresentacion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpPresentacion.ColumnCount = 1;
            this.ComprasTlpPresentacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPresentacion.Controls.Add(this.ComprasLblBienvenido, 0, 0);
            this.ComprasTlpPresentacion.Controls.Add(this.ComprasLblModulo, 0, 1);
            this.ComprasTlpPresentacion.Controls.Add(this.ComprasLblParrafo, 0, 2);
            this.ComprasTlpPresentacion.Controls.Add(this.ComprasBtnAyuda, 0, 3);
            this.ComprasTlpPresentacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPresentacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPresentacion.Location = new System.Drawing.Point(3, 3);
            this.ComprasTlpPresentacion.Name = "ComprasTlpPresentacion";
            this.ComprasTlpPresentacion.RowCount = 4;
            this.ComprasTlpPresentacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.ComprasTlpPresentacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.ComprasTlpPresentacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpPresentacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.ComprasTlpPresentacion.Size = new System.Drawing.Size(746, 632);
            this.ComprasTlpPresentacion.TabIndex = 0;
            // 
            // ComprasLblBienvenido
            // 
            this.ComprasLblBienvenido.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBienvenido.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblBienvenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblBienvenido.Font = new System.Drawing.Font("Tahoma", 40F);
            this.ComprasLblBienvenido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblBienvenido.Location = new System.Drawing.Point(0, 15);
            this.ComprasLblBienvenido.Margin = new System.Windows.Forms.Padding(0, 15, 12, 15);
            this.ComprasLblBienvenido.MaximumSize = new System.Drawing.Size(4625, 0);
            this.ComprasLblBienvenido.MostrarAsterisco = false;
            this.ComprasLblBienvenido.Name = "ComprasLblBienvenido";
            this.ComprasLblBienvenido.Size = new System.Drawing.Size(734, 209);
            this.ComprasLblBienvenido.TabIndex = 0;
            this.ComprasLblBienvenido.TabStop = false;
            this.ComprasLblBienvenido.TamanoTipografia = 40F;
            this.ComprasLblBienvenido.Text = "¡Bienvenido!";
            this.ComprasLblBienvenido.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBienvenido.Texto = "¡Bienvenido!";
            // 
            // ComprasLblModulo
            // 
            this.ComprasLblModulo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblModulo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblModulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblModulo.Font = new System.Drawing.Font("Tahoma", 19F);
            this.ComprasLblModulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblModulo.Location = new System.Drawing.Point(18, 246);
            this.ComprasLblModulo.Margin = new System.Windows.Forms.Padding(18, 7, 6, 7);
            this.ComprasLblModulo.MaximumSize = new System.Drawing.Size(2250, 0);
            this.ComprasLblModulo.MostrarAsterisco = false;
            this.ComprasLblModulo.Name = "ComprasLblModulo";
            this.ComprasLblModulo.Size = new System.Drawing.Size(722, 136);
            this.ComprasLblModulo.TabIndex = 1;
            this.ComprasLblModulo.TabStop = false;
            this.ComprasLblModulo.TamanoTipografia = 19F;
            this.ComprasLblModulo.Text = "Compras y cuentas\npor pagar";
            this.ComprasLblModulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblModulo.Texto = "Compras y cuentas\npor pagar";
            // 
            // ComprasLblParrafo
            // 
            this.ComprasLblParrafo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblParrafo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblParrafo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblParrafo.Font = new System.Drawing.Font("Tahoma", 11.5F);
            this.ComprasLblParrafo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblParrafo.Location = new System.Drawing.Point(24, 393);
            this.ComprasLblParrafo.Margin = new System.Windows.Forms.Padding(24, 4, 3, 4);
            this.ComprasLblParrafo.MaximumSize = new System.Drawing.Size(1250, 0);
            this.ComprasLblParrafo.MostrarAsterisco = false;
            this.ComprasLblParrafo.Name = "ComprasLblParrafo";
            this.ComprasLblParrafo.Size = new System.Drawing.Size(719, 162);
            this.ComprasLblParrafo.TabIndex = 2;
            this.ComprasLblParrafo.TabStop = false;
            this.ComprasLblParrafo.TamanoTipografia = 11.5F;
            this.ComprasLblParrafo.Text = "Gestiona proveedores y órdenes de compra.\nControla pagos y cuentas por pagar en u" +
    "n solo lugar.";
            this.ComprasLblParrafo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblParrafo.Texto = "Gestiona proveedores y órdenes de compra.\nControla pagos y cuentas por pagar en u" +
    "n solo lugar.";
            this.ComprasBtnAyuda.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnAyuda
            // 
            this.ComprasBtnAyuda.AccessibleDescription = "Ayuda";
            this.ComprasBtnAyuda.AccessibleName = "Ayuda";
            this.ComprasBtnAyuda.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasBtnAyuda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(86)))), ((int)(((byte)(166)))));
            this.ComprasBtnAyuda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAyuda.BackgroundImage")));
            this.ComprasBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnAyuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnAyuda.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAyuda.FlatAppearance.BorderSize = 0;
            this.ComprasBtnAyuda.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnAyuda.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnAyuda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ComprasBtnAyuda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasBtnAyuda.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAyuda.ImagenDeshabilitado")));
            this.ComprasBtnAyuda.Location = new System.Drawing.Point(30, 563);
            this.ComprasBtnAyuda.Margin = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.ComprasBtnAyuda.Name = "ComprasBtnAyuda";
            this.ComprasBtnAyuda.Size = new System.Drawing.Size(64, 64);
            this.ComprasBtnAyuda.TabIndex = 3;
            this.ComprasBtnAyuda.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ComprasBtnAyuda.UseVisualStyleBackColor = false;
            // 
            // ComprasPbMascota
            // 
            this.ComprasPbMascota.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbMascota.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbMascota.BackgroundImage")));
            this.ComprasPbMascota.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbMascota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbMascota.Location = new System.Drawing.Point(752, 4);
            this.ComprasPbMascota.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.ComprasPbMascota.Name = "ComprasPbMascota";
            this.ComprasPbMascota.Size = new System.Drawing.Size(416, 630);
            this.ComprasPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbMascota.TabIndex = 1;
            this.ComprasPbMascota.TabStop = false;
            // 
            // ComprasLblResumen
            // 
            this.ComprasLblResumen.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblResumen.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblResumen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblResumen.Font = new System.Drawing.Font("Tahoma", 19F);
            this.ComprasLblResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblResumen.Location = new System.Drawing.Point(6, 651);
            this.ComprasLblResumen.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.ComprasLblResumen.MaximumSize = new System.Drawing.Size(2250, 0);
            this.ComprasLblResumen.MostrarAsterisco = false;
            this.ComprasLblResumen.Name = "ComprasLblResumen";
            this.ComprasLblResumen.Size = new System.Drawing.Size(1162, 31);
            this.ComprasLblResumen.TabIndex = 1;
            this.ComprasLblResumen.TabStop = false;
            this.ComprasLblResumen.TamanoTipografia = 19F;
            this.ComprasLblResumen.Text = "Resumen de Compras";
            this.ComprasLblResumen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblResumen.Texto = "Resumen de Compras";
            // 
            // ComprasTlpKpis
            // 
            this.ComprasTlpKpis.BackColor = System.Drawing.Color.Transparent;
            this.ComprasTlpKpis.ColumnCount = 6;
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi1, 0, 0);
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi2, 1, 0);
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi3, 2, 0);
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi4, 3, 0);
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi5, 4, 0);
            this.ComprasTlpKpis.Controls.Add(this.ComprasPnlKpi6, 5, 0);
            this.ComprasTlpKpis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpis.Location = new System.Drawing.Point(3, 692);
            this.ComprasTlpKpis.Name = "ComprasTlpKpis";
            this.ComprasTlpKpis.RowCount = 1;
            this.ComprasTlpKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpKpis.Size = new System.Drawing.Size(1168, 170);
            this.ComprasTlpKpis.TabIndex = 2;
            // 
            // ComprasPnlKpi1
            // 
            this.ComprasPnlKpi1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi1.Controls.Add(this.ComprasTlpKpi1);
            this.ComprasPnlKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi1.Location = new System.Drawing.Point(3, 0);
            this.ComprasPnlKpi1.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi1.Name = "ComprasPnlKpi1";
            this.ComprasPnlKpi1.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi1.Size = new System.Drawing.Size(185, 162);
            this.ComprasPnlKpi1.TabIndex = 0;
            // 
            // ComprasTlpKpi1
            // 
            this.ComprasTlpKpi1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi1.ColumnCount = 1;
            this.ComprasTlpKpi1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi1.Controls.Add(this.ComprasPbKpi1, 0, 0);
            this.ComprasTlpKpi1.Controls.Add(this.ComprasLblValorKpi1, 0, 1);
            this.ComprasTlpKpi1.Controls.Add(this.ComprasLblKpi1, 0, 2);
            this.ComprasTlpKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi1.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi1.Name = "ComprasTlpKpi1";
            this.ComprasTlpKpi1.RowCount = 3;
            this.ComprasTlpKpi1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi1.Size = new System.Drawing.Size(173, 150);
            this.ComprasTlpKpi1.TabIndex = 0;
            // 
            // ComprasPbKpi1
            // 
            this.ComprasPbKpi1.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi1.BackgroundImage")));
            this.ComprasPbKpi1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi1.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi1.Name = "ComprasPbKpi1";
            this.ComprasPbKpi1.Size = new System.Drawing.Size(167, 76);
            this.ComprasPbKpi1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi1.TabIndex = 0;
            this.ComprasPbKpi1.TabStop = false;
            // 
            // ComprasLblValorKpi1
            // 
            this.ComprasLblValorKpi1.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi1.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi1.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi1.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi1.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi1.MostrarAsterisco = false;
            this.ComprasLblValorKpi1.Name = "ComprasLblValorKpi1";
            this.ComprasLblValorKpi1.Size = new System.Drawing.Size(163, 18);
            this.ComprasLblValorKpi1.TabIndex = 1;
            this.ComprasLblValorKpi1.TabStop = false;
            this.ComprasLblValorKpi1.TamanoTipografia = 16F;
            this.ComprasLblValorKpi1.Text = "—";
            this.ComprasLblValorKpi1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi1.Texto = "—";
            // 
            // ComprasLblKpi1
            // 
            this.ComprasLblKpi1.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi1.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi1.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi1.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi1.MostrarAsterisco = false;
            this.ComprasLblKpi1.Name = "ComprasLblKpi1";
            this.ComprasLblKpi1.Size = new System.Drawing.Size(167, 30);
            this.ComprasLblKpi1.TabIndex = 2;
            this.ComprasLblKpi1.TabStop = false;
            this.ComprasLblKpi1.TamanoTipografia = 10F;
            this.ComprasLblKpi1.Text = "Compras del mes";
            this.ComprasLblKpi1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi1.Texto = "Compras del mes";
            // 
            // ComprasPnlKpi2
            // 
            this.ComprasPnlKpi2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi2.Controls.Add(this.ComprasTlpKpi2);
            this.ComprasPnlKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi2.Location = new System.Drawing.Point(197, 0);
            this.ComprasPnlKpi2.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi2.Name = "ComprasPnlKpi2";
            this.ComprasPnlKpi2.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi2.Size = new System.Drawing.Size(185, 162);
            this.ComprasPnlKpi2.TabIndex = 1;
            // 
            // ComprasTlpKpi2
            // 
            this.ComprasTlpKpi2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi2.ColumnCount = 1;
            this.ComprasTlpKpi2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi2.Controls.Add(this.ComprasPbKpi2, 0, 0);
            this.ComprasTlpKpi2.Controls.Add(this.ComprasLblValorKpi2, 0, 1);
            this.ComprasTlpKpi2.Controls.Add(this.ComprasLblKpi2, 0, 2);
            this.ComprasTlpKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi2.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi2.Name = "ComprasTlpKpi2";
            this.ComprasTlpKpi2.RowCount = 3;
            this.ComprasTlpKpi2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi2.Size = new System.Drawing.Size(173, 150);
            this.ComprasTlpKpi2.TabIndex = 0;
            // 
            // ComprasPbKpi2
            // 
            this.ComprasPbKpi2.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi2.BackgroundImage")));
            this.ComprasPbKpi2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi2.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi2.Name = "ComprasPbKpi2";
            this.ComprasPbKpi2.Size = new System.Drawing.Size(167, 76);
            this.ComprasPbKpi2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi2.TabIndex = 0;
            this.ComprasPbKpi2.TabStop = false;
            // 
            // ComprasLblValorKpi2
            // 
            this.ComprasLblValorKpi2.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi2.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi2.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi2.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi2.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi2.MostrarAsterisco = false;
            this.ComprasLblValorKpi2.Name = "ComprasLblValorKpi2";
            this.ComprasLblValorKpi2.Size = new System.Drawing.Size(163, 18);
            this.ComprasLblValorKpi2.TabIndex = 1;
            this.ComprasLblValorKpi2.TabStop = false;
            this.ComprasLblValorKpi2.TamanoTipografia = 16F;
            this.ComprasLblValorKpi2.Text = "—";
            this.ComprasLblValorKpi2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi2.Texto = "—";
            // 
            // ComprasLblKpi2
            // 
            this.ComprasLblKpi2.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi2.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi2.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi2.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi2.MostrarAsterisco = false;
            this.ComprasLblKpi2.Name = "ComprasLblKpi2";
            this.ComprasLblKpi2.Size = new System.Drawing.Size(167, 30);
            this.ComprasLblKpi2.TabIndex = 2;
            this.ComprasLblKpi2.TabStop = false;
            this.ComprasLblKpi2.TamanoTipografia = 10F;
            this.ComprasLblKpi2.Text = "Órdenes pendientes de aprobación";
            this.ComprasLblKpi2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi2.Texto = "Órdenes pendientes de aprobación";
            // 
            // ComprasPnlKpi3
            // 
            this.ComprasPnlKpi3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi3.Controls.Add(this.ComprasTlpKpi3);
            this.ComprasPnlKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi3.Location = new System.Drawing.Point(391, 0);
            this.ComprasPnlKpi3.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi3.Name = "ComprasPnlKpi3";
            this.ComprasPnlKpi3.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi3.Size = new System.Drawing.Size(185, 162);
            this.ComprasPnlKpi3.TabIndex = 2;
            // 
            // ComprasTlpKpi3
            // 
            this.ComprasTlpKpi3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi3.ColumnCount = 1;
            this.ComprasTlpKpi3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi3.Controls.Add(this.ComprasPbKpi3, 0, 0);
            this.ComprasTlpKpi3.Controls.Add(this.ComprasLblValorKpi3, 0, 1);
            this.ComprasTlpKpi3.Controls.Add(this.ComprasLblKpi3, 0, 2);
            this.ComprasTlpKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi3.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi3.Name = "ComprasTlpKpi3";
            this.ComprasTlpKpi3.RowCount = 3;
            this.ComprasTlpKpi3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi3.Size = new System.Drawing.Size(173, 150);
            this.ComprasTlpKpi3.TabIndex = 0;
            // 
            // ComprasPbKpi3
            // 
            this.ComprasPbKpi3.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi3.BackgroundImage")));
            this.ComprasPbKpi3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi3.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi3.Name = "ComprasPbKpi3";
            this.ComprasPbKpi3.Size = new System.Drawing.Size(167, 76);
            this.ComprasPbKpi3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi3.TabIndex = 0;
            this.ComprasPbKpi3.TabStop = false;
            // 
            // ComprasLblValorKpi3
            // 
            this.ComprasLblValorKpi3.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi3.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi3.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi3.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi3.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi3.MostrarAsterisco = false;
            this.ComprasLblValorKpi3.Name = "ComprasLblValorKpi3";
            this.ComprasLblValorKpi3.Size = new System.Drawing.Size(163, 18);
            this.ComprasLblValorKpi3.TabIndex = 1;
            this.ComprasLblValorKpi3.TabStop = false;
            this.ComprasLblValorKpi3.TamanoTipografia = 16F;
            this.ComprasLblValorKpi3.Text = "—";
            this.ComprasLblValorKpi3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi3.Texto = "—";
            // 
            // ComprasLblKpi3
            // 
            this.ComprasLblKpi3.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi3.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi3.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi3.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi3.MostrarAsterisco = false;
            this.ComprasLblKpi3.Name = "ComprasLblKpi3";
            this.ComprasLblKpi3.Size = new System.Drawing.Size(167, 30);
            this.ComprasLblKpi3.TabIndex = 2;
            this.ComprasLblKpi3.TabStop = false;
            this.ComprasLblKpi3.TamanoTipografia = 10F;
            this.ComprasLblKpi3.Text = "Órdenes pendientes de recepción";
            this.ComprasLblKpi3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi3.Texto = "Órdenes pendientes de recepción";
            // 
            // ComprasPnlKpi4
            // 
            this.ComprasPnlKpi4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi4.Controls.Add(this.ComprasTlpKpi4);
            this.ComprasPnlKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi4.Location = new System.Drawing.Point(585, 0);
            this.ComprasPnlKpi4.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi4.Name = "ComprasPnlKpi4";
            this.ComprasPnlKpi4.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi4.Size = new System.Drawing.Size(185, 162);
            this.ComprasPnlKpi4.TabIndex = 3;
            // 
            // ComprasTlpKpi4
            // 
            this.ComprasTlpKpi4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi4.ColumnCount = 1;
            this.ComprasTlpKpi4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi4.Controls.Add(this.ComprasPbKpi4, 0, 0);
            this.ComprasTlpKpi4.Controls.Add(this.ComprasLblValorKpi4, 0, 1);
            this.ComprasTlpKpi4.Controls.Add(this.ComprasLblKpi4, 0, 2);
            this.ComprasTlpKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi4.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi4.Name = "ComprasTlpKpi4";
            this.ComprasTlpKpi4.RowCount = 3;
            this.ComprasTlpKpi4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi4.Size = new System.Drawing.Size(173, 150);
            this.ComprasTlpKpi4.TabIndex = 0;
            // 
            // ComprasPbKpi4
            // 
            this.ComprasPbKpi4.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi4.BackgroundImage")));
            this.ComprasPbKpi4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi4.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi4.Name = "ComprasPbKpi4";
            this.ComprasPbKpi4.Size = new System.Drawing.Size(167, 76);
            this.ComprasPbKpi4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi4.TabIndex = 0;
            this.ComprasPbKpi4.TabStop = false;
            // 
            // ComprasLblValorKpi4
            // 
            this.ComprasLblValorKpi4.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi4.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi4.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi4.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi4.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi4.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi4.MostrarAsterisco = false;
            this.ComprasLblValorKpi4.Name = "ComprasLblValorKpi4";
            this.ComprasLblValorKpi4.Size = new System.Drawing.Size(163, 18);
            this.ComprasLblValorKpi4.TabIndex = 1;
            this.ComprasLblValorKpi4.TabStop = false;
            this.ComprasLblValorKpi4.TamanoTipografia = 16F;
            this.ComprasLblValorKpi4.Text = "—";
            this.ComprasLblValorKpi4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi4.Texto = "—";
            // 
            // ComprasLblKpi4
            // 
            this.ComprasLblKpi4.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi4.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi4.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi4.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi4.MostrarAsterisco = false;
            this.ComprasLblKpi4.Name = "ComprasLblKpi4";
            this.ComprasLblKpi4.Size = new System.Drawing.Size(167, 30);
            this.ComprasLblKpi4.TabIndex = 2;
            this.ComprasLblKpi4.TabStop = false;
            this.ComprasLblKpi4.TamanoTipografia = 10F;
            this.ComprasLblKpi4.Text = "Saldo por pagar";
            this.ComprasLblKpi4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi4.Texto = "Saldo por pagar";
            // 
            // ComprasPnlKpi5
            // 
            this.ComprasPnlKpi5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi5.Controls.Add(this.ComprasTlpKpi5);
            this.ComprasPnlKpi5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi5.Location = new System.Drawing.Point(779, 0);
            this.ComprasPnlKpi5.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi5.Name = "ComprasPnlKpi5";
            this.ComprasPnlKpi5.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi5.Size = new System.Drawing.Size(185, 162);
            this.ComprasPnlKpi5.TabIndex = 4;
            // 
            // ComprasTlpKpi5
            // 
            this.ComprasTlpKpi5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi5.ColumnCount = 1;
            this.ComprasTlpKpi5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi5.Controls.Add(this.ComprasPbKpi5, 0, 0);
            this.ComprasTlpKpi5.Controls.Add(this.ComprasLblValorKpi5, 0, 1);
            this.ComprasTlpKpi5.Controls.Add(this.ComprasLblKpi5, 0, 2);
            this.ComprasTlpKpi5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi5.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi5.Name = "ComprasTlpKpi5";
            this.ComprasTlpKpi5.RowCount = 3;
            this.ComprasTlpKpi5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi5.Size = new System.Drawing.Size(173, 150);
            this.ComprasTlpKpi5.TabIndex = 0;
            // 
            // ComprasPbKpi5
            // 
            this.ComprasPbKpi5.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi5.BackgroundImage")));
            this.ComprasPbKpi5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi5.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi5.Name = "ComprasPbKpi5";
            this.ComprasPbKpi5.Size = new System.Drawing.Size(167, 76);
            this.ComprasPbKpi5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi5.TabIndex = 0;
            this.ComprasPbKpi5.TabStop = false;
            // 
            // ComprasLblValorKpi5
            // 
            this.ComprasLblValorKpi5.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi5.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi5.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi5.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi5.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi5.MostrarAsterisco = false;
            this.ComprasLblValorKpi5.Name = "ComprasLblValorKpi5";
            this.ComprasLblValorKpi5.Size = new System.Drawing.Size(163, 18);
            this.ComprasLblValorKpi5.TabIndex = 1;
            this.ComprasLblValorKpi5.TabStop = false;
            this.ComprasLblValorKpi5.TamanoTipografia = 16F;
            this.ComprasLblValorKpi5.Text = "—";
            this.ComprasLblValorKpi5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi5.Texto = "—";
            // 
            // ComprasLblKpi5
            // 
            this.ComprasLblKpi5.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi5.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi5.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi5.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi5.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi5.MostrarAsterisco = false;
            this.ComprasLblKpi5.Name = "ComprasLblKpi5";
            this.ComprasLblKpi5.Size = new System.Drawing.Size(167, 30);
            this.ComprasLblKpi5.TabIndex = 2;
            this.ComprasLblKpi5.TabStop = false;
            this.ComprasLblKpi5.TamanoTipografia = 10F;
            this.ComprasLblKpi5.Text = "Saldo vencido";
            this.ComprasLblKpi5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi5.Texto = "Saldo vencido";
            // 
            // ComprasPnlKpi6
            // 
            this.ComprasPnlKpi6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlKpi6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlKpi6.Controls.Add(this.ComprasTlpKpi6);
            this.ComprasPnlKpi6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlKpi6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlKpi6.Location = new System.Drawing.Point(973, 0);
            this.ComprasPnlKpi6.Margin = new System.Windows.Forms.Padding(3, 0, 6, 8);
            this.ComprasPnlKpi6.Name = "ComprasPnlKpi6";
            this.ComprasPnlKpi6.Padding = new System.Windows.Forms.Padding(6);
            this.ComprasPnlKpi6.Size = new System.Drawing.Size(189, 162);
            this.ComprasPnlKpi6.TabIndex = 5;
            // 
            // ComprasTlpKpi6
            // 
            this.ComprasTlpKpi6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpKpi6.ColumnCount = 1;
            this.ComprasTlpKpi6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi6.Controls.Add(this.ComprasPbKpi6, 0, 0);
            this.ComprasTlpKpi6.Controls.Add(this.ComprasLblValorKpi6, 0, 1);
            this.ComprasTlpKpi6.Controls.Add(this.ComprasLblKpi6, 0, 2);
            this.ComprasTlpKpi6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpKpi6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpKpi6.Location = new System.Drawing.Point(6, 6);
            this.ComprasTlpKpi6.Name = "ComprasTlpKpi6";
            this.ComprasTlpKpi6.RowCount = 3;
            this.ComprasTlpKpi6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpKpi6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpKpi6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.ComprasTlpKpi6.Size = new System.Drawing.Size(177, 150);
            this.ComprasTlpKpi6.TabIndex = 0;
            // 
            // ComprasPbKpi6
            // 
            this.ComprasPbKpi6.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPbKpi6.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasPbKpi6.BackgroundImage")));
            this.ComprasPbKpi6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ComprasPbKpi6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPbKpi6.Location = new System.Drawing.Point(3, 3);
            this.ComprasPbKpi6.Name = "ComprasPbKpi6";
            this.ComprasPbKpi6.Size = new System.Drawing.Size(171, 76);
            this.ComprasPbKpi6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ComprasPbKpi6.TabIndex = 0;
            this.ComprasPbKpi6.TabStop = false;
            // 
            // ComprasLblValorKpi6
            // 
            this.ComprasLblValorKpi6.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi6.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblValorKpi6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblValorKpi6.Font = new System.Drawing.Font("Tahoma", 16F);
            this.ComprasLblValorKpi6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblValorKpi6.Location = new System.Drawing.Point(5, 88);
            this.ComprasLblValorKpi6.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.ComprasLblValorKpi6.MaximumSize = new System.Drawing.Size(1875, 0);
            this.ComprasLblValorKpi6.MostrarAsterisco = false;
            this.ComprasLblValorKpi6.Name = "ComprasLblValorKpi6";
            this.ComprasLblValorKpi6.Size = new System.Drawing.Size(167, 18);
            this.ComprasLblValorKpi6.TabIndex = 1;
            this.ComprasLblValorKpi6.TabStop = false;
            this.ComprasLblValorKpi6.TamanoTipografia = 16F;
            this.ComprasLblValorKpi6.Text = "—";
            this.ComprasLblValorKpi6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblValorKpi6.Texto = "—";
            // 
            // ComprasLblKpi6
            // 
            this.ComprasLblKpi6.AlineacionTexto = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi6.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblKpi6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblKpi6.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblKpi6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblKpi6.Location = new System.Drawing.Point(3, 116);
            this.ComprasLblKpi6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblKpi6.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblKpi6.MostrarAsterisco = false;
            this.ComprasLblKpi6.Name = "ComprasLblKpi6";
            this.ComprasLblKpi6.Size = new System.Drawing.Size(171, 30);
            this.ComprasLblKpi6.TabIndex = 2;
            this.ComprasLblKpi6.TabStop = false;
            this.ComprasLblKpi6.TamanoTipografia = 10F;
            this.ComprasLblKpi6.Text = "Pagos próximos a vencer - 7 días";
            this.ComprasLblKpi6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ComprasLblKpi6.Texto = "Pagos próximos a vencer - 7 días";
            // 
            // ComprasPnlInformacion
            // 
            this.ComprasPnlInformacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasPnlInformacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasPnlInformacion.Controls.Add(this.ComprasTlpInformacion);
            this.ComprasPnlInformacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlInformacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlInformacion.Location = new System.Drawing.Point(3, 868);
            this.ComprasPnlInformacion.Name = "ComprasPnlInformacion";
            this.ComprasPnlInformacion.Padding = new System.Windows.Forms.Padding(14, 9, 14, 9);
            this.ComprasPnlInformacion.Size = new System.Drawing.Size(1168, 66);
            this.ComprasPnlInformacion.TabIndex = 3;
            // 
            // ComprasTlpInformacion
            // 
            this.ComprasTlpInformacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasTlpInformacion.ColumnCount = 2;
            this.ComprasTlpInformacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.ComprasTlpInformacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ComprasTlpInformacion.Controls.Add(this.ComprasLblInformacion, 0, 0);
            this.ComprasTlpInformacion.Controls.Add(this.ComprasLblIntegracion, 1, 0);
            this.ComprasTlpInformacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpInformacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpInformacion.Location = new System.Drawing.Point(14, 9);
            this.ComprasTlpInformacion.Name = "ComprasTlpInformacion";
            this.ComprasTlpInformacion.RowCount = 1;
            this.ComprasTlpInformacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpInformacion.Size = new System.Drawing.Size(1140, 48);
            this.ComprasTlpInformacion.TabIndex = 0;
            // 
            // ComprasLblInformacion
            // 
            this.ComprasLblInformacion.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblInformacion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblInformacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblInformacion.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ComprasLblInformacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblInformacion.Location = new System.Drawing.Point(3, 4);
            this.ComprasLblInformacion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblInformacion.MaximumSize = new System.Drawing.Size(1250, 0);
            this.ComprasLblInformacion.MostrarAsterisco = false;
            this.ComprasLblInformacion.Name = "ComprasLblInformacion";
            this.ComprasLblInformacion.Size = new System.Drawing.Size(678, 40);
            this.ComprasLblInformacion.TabIndex = 0;
            this.ComprasLblInformacion.TabStop = false;
            this.ComprasLblInformacion.TamanoTipografia = 11F;
            this.ComprasLblInformacion.Text = "Información de Compras\nProveedores, órdenes de compra y cuentas por pagar.";
            this.ComprasLblInformacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblInformacion.Texto = "Información de Compras\nProveedores, órdenes de compra y cuentas por pagar.";
            // 
            // ComprasLblIntegracion
            // 
            this.ComprasLblIntegracion.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblIntegracion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasLblIntegracion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblIntegracion.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblIntegracion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblIntegracion.Location = new System.Drawing.Point(687, 4);
            this.ComprasLblIntegracion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblIntegracion.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblIntegracion.MostrarAsterisco = false;
            this.ComprasLblIntegracion.Name = "ComprasLblIntegracion";
            this.ComprasLblIntegracion.Size = new System.Drawing.Size(450, 40);
            this.ComprasLblIntegracion.TabIndex = 1;
            this.ComprasLblIntegracion.TabStop = false;
            this.ComprasLblIntegracion.TamanoTipografia = 10F;
            this.ComprasLblIntegracion.Text = "Integración\nIndicadores pendientes de datos del controlador.";
            this.ComprasLblIntegracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblIntegracion.Texto = "Integración\nIndicadores pendientes de datos del controlador.";
            // 
            // FrmMDICompras
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1536, 1014);
            this.Controls.Add(this.ComprasTlpPrincipal);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMDICompras";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6000 – MDICompras";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasTlpNavegador.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbMarca)).EndInit();
            this.ComprasTlpMovimientosProveedores.ResumeLayout(false);
            this.ComprasTlpPolizas.ResumeLayout(false);
            this.ComprasTlpProveedores.ResumeLayout(false);
            this.ComprasTlpOrdenesCompra.ResumeLayout(false);
            this.ComprasTlpPagosProveedores.ResumeLayout(false);
            this.ComprasTlpAprobacionFinanciera.ResumeLayout(false);
            this.ComprasTlpCierreMovimientos.ResumeLayout(false);
            this.ComprasTlpBitacora.ResumeLayout(false);
            this.ComprasTlpHerramientas.ResumeLayout(false);
            this.ComprasTlpSalir.ResumeLayout(false);
            this.ComprasFlpEncabezado.ResumeLayout(false);
            this.ComprasPnlMain.ResumeLayout(false);
            this.ComprasPnlDashboard.ResumeLayout(false);
            this.ComprasTlpDashboard.ResumeLayout(false);
            this.ComprasTlpHero.ResumeLayout(false);
            this.ComprasTlpPresentacion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbMascota)).EndInit();
            this.ComprasTlpKpis.ResumeLayout(false);
            this.ComprasPnlKpi1.ResumeLayout(false);
            this.ComprasTlpKpi1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi1)).EndInit();
            this.ComprasPnlKpi2.ResumeLayout(false);
            this.ComprasTlpKpi2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi2)).EndInit();
            this.ComprasPnlKpi3.ResumeLayout(false);
            this.ComprasTlpKpi3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi3)).EndInit();
            this.ComprasPnlKpi4.ResumeLayout(false);
            this.ComprasTlpKpi4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi4)).EndInit();
            this.ComprasPnlKpi5.ResumeLayout(false);
            this.ComprasTlpKpi5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi5)).EndInit();
            this.ComprasPnlKpi6.ResumeLayout(false);
            this.ComprasTlpKpi6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ComprasPbKpi6)).EndInit();
            this.ComprasPnlInformacion.ResumeLayout(false);
            this.ComprasTlpInformacion.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TableLayoutPanel ComprasTlpPrincipal;
        private System.Windows.Forms.TableLayoutPanel ComprasTlpNavegador;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpEncabezado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnAlternarNavegador;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlMain;
        private System.Windows.Forms.PictureBox ComprasPbMarca;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMovimientosProveedores;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoMovimientosProveedores;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblMovimientosProveedores;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPolizas;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoPolizas;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPolizas;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpProveedores;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoProveedores;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblProveedores;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpOrdenesCompra;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoOrdenesCompra;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblOrdenesCompra;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPagosProveedores;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoPagosProveedores;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPagosProveedores;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpAprobacionFinanciera;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoAprobacionFinanciera;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblAprobacionFinanciera;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpCierreMovimientos;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoCierreMovimientos;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCierreMovimientos;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBitacora;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpHerramientas;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoBitacora;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoHerramientas;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblBitacora;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblHerramientas;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpSalir;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIconoSalir;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSalir;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblUsuario;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblRol;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlDashboard;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDashboard;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpHero;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPresentacion;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblBienvenido;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblModulo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblParrafo;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnAyuda;
        private System.Windows.Forms.PictureBox ComprasPbMascota;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblResumen;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpis;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi1;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi1;
        private System.Windows.Forms.PictureBox ComprasPbKpi1;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi1;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi1;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi2;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi2;
        private System.Windows.Forms.PictureBox ComprasPbKpi2;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi2;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi2;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi3;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi3;
        private System.Windows.Forms.PictureBox ComprasPbKpi3;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi3;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi3;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi4;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi4;
        private System.Windows.Forms.PictureBox ComprasPbKpi4;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi4;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi4;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi5;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi5;
        private System.Windows.Forms.PictureBox ComprasPbKpi5;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi5;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi5;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlKpi6;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpKpi6;
        private System.Windows.Forms.PictureBox ComprasPbKpi6;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblValorKpi6;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblKpi6;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlInformacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpInformacion;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblInformacion;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblIntegracion;
    }
}
