namespace CapaVista_OrdenesCompra
{
    partial class FrmOrdenesCompra
    {
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezado;
        private System.Windows.Forms.Panel ComprasPnlContenido;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos) ComprasProcLiberarCatalogos();
            if (LiberarRecursos && _Componentes != null)
                _Componentes.Dispose();
            base.Dispose(LiberarRecursos);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOrdenesCompra));
            this.ComprasUsrEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnDescartar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpDetalles = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasDgvProductos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasFlpAccionesProductos = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasGbxProducto = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpProducto = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasCboSkuProducto = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblSkuProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblNombreProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboNombreProducto = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblDescripcionProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescripcionProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblPesoProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtPesoProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtCostoUnitarioProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblCostoUnitarioProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblCantidadProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDescuentoProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescuentoProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtDescuentoPorcentajeProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtCantidadProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpAccionesProducto = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnEliminarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnModificarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnInsertarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblTotalProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTotalProducto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxFinalizacion = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpFinalizacion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpComentario = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEtiquetaComentario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtComentario = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTotalOrden = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTotalOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblImpuestoTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtImpuestoTotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtSubtotalNeto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtDescuentoPorcentajeTotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblSubtotalNeto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDescuentoPorcentajeTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescuentoTotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDescuentoTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblSubtotalOrden = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtSubtotalOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxBodega = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpBodega = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasCboSucursal = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblSucursal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblBodega = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboBodega = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblPaisBodega = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtPaisBodega = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxPlazo = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpPlazo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblFechaEmision = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtFechaEmision = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblPlazo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboPlazo = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblFechaVencimiento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtFechaVencimiento = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxMoneda = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpMoneda = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblMoneda = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboMoneda = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTxtTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxProveedor = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpProveedor = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTxtDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasCboNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblSaldoActualProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtSaldoActualProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstadoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEstadoProveedorValor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpDetalles.SuspendLayout();
            this.ComprasGbxProducto.SuspendLayout();
            this.ComprasTlpProducto.SuspendLayout();
            this.ComprasTlpAccionesProducto.SuspendLayout();
            this.ComprasGbxFinalizacion.SuspendLayout();
            this.ComprasTlpFinalizacion.SuspendLayout();
            this.ComprasTlpComentario.SuspendLayout();
            this.ComprasTlpTotales.SuspendLayout();
            this.ComprasGbxBodega.SuspendLayout();
            this.ComprasTlpBodega.SuspendLayout();
            this.ComprasGbxPlazo.SuspendLayout();
            this.ComprasTlpPlazo.SuspendLayout();
            this.ComprasGbxMoneda.SuspendLayout();
            this.ComprasTlpMoneda.SuspendLayout();
            this.ComprasGbxProveedor.SuspendLayout();
            this.ComprasTlpProveedor.SuspendLayout();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasUsrEncabezado
            // 
            this.ComprasUsrEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezado.Codigo = "6003";
            this.ComprasUsrEncabezado.Descripcion = "Registra órdenes de compra con los datos del proveedor y  los artículos solicitad" +
    "os";
            this.ComprasUsrEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasUsrEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrEncabezado.MaximumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.Name = "ComprasUsrEncabezado";
            this.ComprasUsrEncabezado.Size = new System.Drawing.Size(1279, 130);
            this.ComprasUsrEncabezado.TabIndex = 0;
            this.ComprasUsrEncabezado.TabStop = false;
            this.ComprasUsrEncabezado.Titulo = "ORDENES DE COMPRA";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlContenido.Controls.Add(this.ComprasUsrEncabezado);
            this.ComprasPnlContenido.Controls.Add(this.ComprasBtnGuardar);
            this.ComprasPnlContenido.Controls.Add(this.ComprasBtnDescartar);
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpDetalles);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxProducto);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxFinalizacion);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxBodega);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxPlazo);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxMoneda);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxProveedor);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 1830);
            this.ComprasPnlContenido.TabIndex = 1;
            // 
            // ComprasBtnGuardar
            // 
            this.ComprasBtnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComprasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnGuardar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnGuardar.BackgroundImage")));
            this.ComprasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnGuardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnGuardar.ImagenDeshabilitado")));
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(105, 1741);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnGuardar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.TabIndex = 10;
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnDescartar
            // 
            this.ComprasBtnDescartar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComprasBtnDescartar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnDescartar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnDescartar.BackgroundImage")));
            this.ComprasBtnDescartar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnDescartar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnDescartar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnDescartar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnDescartar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnDescartar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnDescartar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnDescartar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnDescartar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnDescartar.Location = new System.Drawing.Point(13, 1741);
            this.ComprasBtnDescartar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnDescartar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.Name = "ComprasBtnDescartar";
            this.ComprasBtnDescartar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.TabIndex = 15;
            this.ComprasBtnDescartar.UseVisualStyleBackColor = false;
            // 
            // ComprasTlpDetalles
            // 
            this.ComprasTlpDetalles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDetalles.ColumnCount = 1;
            this.ComprasTlpDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpDetalles.Controls.Add(this.ComprasDgvProductos, 0, 1);
            this.ComprasTlpDetalles.Controls.Add(this.ComprasFlpAccionesProductos, 0, 0);
            this.ComprasTlpDetalles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDetalles.Location = new System.Drawing.Point(454, 930);
            this.ComprasTlpDetalles.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDetalles.Name = "ComprasTlpDetalles";
            this.ComprasTlpDetalles.RowCount = 2;
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Size = new System.Drawing.Size(773, 497);
            this.ComprasTlpDetalles.TabIndex = 6;
            // 
            // ComprasDgvProductos
            // 
            this.ComprasDgvProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvProductos.Location = new System.Drawing.Point(10, 40);
            this.ComprasDgvProductos.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ComprasDgvProductos.Name = "ComprasDgvProductos";
            this.ComprasDgvProductos.ResaltarFilasAlPasar = true;
            this.ComprasDgvProductos.Size = new System.Drawing.Size(753, 457);
            this.ComprasDgvProductos.TabIndex = 3;
            // 
            // ComprasFlpAccionesProductos
            // 
            this.ComprasFlpAccionesProductos.Dock = System.Windows.Forms.DockStyle.Left;
            this.ComprasFlpAccionesProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasFlpAccionesProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAccionesProductos.Name = "ComprasFlpAccionesProductos";
            this.ComprasFlpAccionesProductos.Size = new System.Drawing.Size(773, 40);
            this.ComprasFlpAccionesProductos.TabIndex = 0;
            // 
            // ComprasGbxProducto
            // 
            this.ComprasGbxProducto.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxProducto.Controls.Add(this.ComprasTlpProducto);
            this.ComprasGbxProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxProducto.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxProducto.Location = new System.Drawing.Point(13, 906);
            this.ComprasGbxProducto.Name = "ComprasGbxProducto";
            this.ComprasGbxProducto.Size = new System.Drawing.Size(420, 565);
            this.ComprasGbxProducto.TabIndex = 4;
            this.ComprasGbxProducto.TabStop = false;
            this.ComprasGbxProducto.Text = "5. Agregar Productos";
            // 
            // ComprasTlpProducto
            // 
            this.ComprasTlpProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpProducto.ColumnCount = 2;
            this.ComprasTlpProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.ComprasTlpProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProducto.Controls.Add(this.ComprasCboSkuProducto, 1, 0);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblSkuProducto, 0, 0);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblNombreProducto, 0, 1);
            this.ComprasTlpProducto.Controls.Add(this.ComprasCboNombreProducto, 1, 1);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblDescripcionProducto, 0, 2);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtDescripcionProducto, 1, 2);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblPesoProducto, 0, 5);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtPesoProducto, 1, 5);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtCostoUnitarioProducto, 1, 6);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblCostoUnitarioProducto, 0, 6);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblCantidadProducto, 0, 9);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblDescuentoProducto, 0, 7);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtDescuentoProducto, 1, 7);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtDescuentoPorcentajeProducto, 1, 8);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtCantidadProducto, 1, 9);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTlpAccionesProducto, 0, 12);
            this.ComprasTlpProducto.Controls.Add(this.ComprasLblTotalProducto, 0, 10);
            this.ComprasTlpProducto.Controls.Add(this.ComprasTxtTotalProducto, 1, 10);
            this.ComprasTlpProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpProducto.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpProducto.Name = "ComprasTlpProducto";
            this.ComprasTlpProducto.RowCount = 14;
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProducto.Size = new System.Drawing.Size(414, 538);
            this.ComprasTlpProducto.TabIndex = 0;
            // 
            // ComprasCboSkuProducto
            // 
            this.ComprasCboSkuProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboSkuProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboSkuProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboSkuProducto.Location = new System.Drawing.Point(125, 0);
            this.ComprasCboSkuProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboSkuProducto.MaxLength = 0;
            this.ComprasCboSkuProducto.MostrarEtiquetaError = false;
            this.ComprasCboSkuProducto.Name = "ComprasCboSkuProducto";
            this.ComprasCboSkuProducto.Size = new System.Drawing.Size(289, 40);
            this.ComprasCboSkuProducto.TabIndex = 24;
            this.ComprasCboSkuProducto.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.ProductosPorSku;
            // 
            // ComprasLblSkuProducto
            // 
            this.ComprasLblSkuProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblSkuProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSkuProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSkuProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSkuProducto.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblSkuProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSkuProducto.Name = "ComprasLblSkuProducto";
            this.ComprasLblSkuProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblSkuProducto.TabIndex = 4;
            this.ComprasLblSkuProducto.TabStop = false;
            this.ComprasLblSkuProducto.TamanoTipografia = 9F;
            this.ComprasLblSkuProducto.Text = "SKU";
            this.ComprasLblSkuProducto.Texto = "SKU";
            // 
            // ComprasLblNombreProducto
            // 
            this.ComprasLblNombreProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblNombreProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNombreProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNombreProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNombreProducto.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblNombreProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNombreProducto.Name = "ComprasLblNombreProducto";
            this.ComprasLblNombreProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblNombreProducto.TabIndex = 5;
            this.ComprasLblNombreProducto.TabStop = false;
            this.ComprasLblNombreProducto.TamanoTipografia = 9F;
            this.ComprasLblNombreProducto.Text = "Producto";
            this.ComprasLblNombreProducto.Texto = "Producto";
            // 
            // ComprasCboNombreProducto
            // 
            this.ComprasCboNombreProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboNombreProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboNombreProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboNombreProducto.Location = new System.Drawing.Point(125, 40);
            this.ComprasCboNombreProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboNombreProducto.MaxLength = 0;
            this.ComprasCboNombreProducto.MostrarEtiquetaError = false;
            this.ComprasCboNombreProducto.Name = "ComprasCboNombreProducto";
            this.ComprasCboNombreProducto.Size = new System.Drawing.Size(289, 40);
            this.ComprasCboNombreProducto.TabIndex = 20;
            this.ComprasCboNombreProducto.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Productos;
            // 
            // ComprasLblDescripcionProducto
            // 
            this.ComprasLblDescripcionProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblDescripcionProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescripcionProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescripcionProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescripcionProducto.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblDescripcionProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDescripcionProducto.MostrarAsterisco = false;
            this.ComprasLblDescripcionProducto.Name = "ComprasLblDescripcionProducto";
            this.ComprasLblDescripcionProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblDescripcionProducto.TabIndex = 22;
            this.ComprasLblDescripcionProducto.TabStop = false;
            this.ComprasLblDescripcionProducto.TamanoTipografia = 9F;
            this.ComprasLblDescripcionProducto.Text = "Descripción";
            this.ComprasLblDescripcionProducto.Texto = "Descripción";
            // 
            // ComprasTxtDescripcionProducto
            // 
            this.ComprasTxtDescripcionProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDescripcionProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescripcionProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescripcionProducto.Enabled = false;
            this.ComprasTxtDescripcionProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescripcionProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescripcionProducto.Location = new System.Drawing.Point(127, 82);
            this.ComprasTxtDescripcionProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescripcionProducto.MaxLength = 255;
            this.ComprasTxtDescripcionProducto.MostrarEtiquetaError = false;
            this.ComprasTxtDescripcionProducto.Multiline = true;
            this.ComprasTxtDescripcionProducto.Name = "ComprasTxtDescripcionProducto";
            this.ComprasTxtDescripcionProducto.ReadOnly = true;
            this.ComprasTlpProducto.SetRowSpan(this.ComprasTxtDescripcionProducto, 3);
            this.ComprasTxtDescripcionProducto.SelectedText = "";
            this.ComprasTxtDescripcionProducto.SelectionLength = 0;
            this.ComprasTxtDescripcionProducto.SelectionStart = 0;
            this.ComprasTxtDescripcionProducto.Size = new System.Drawing.Size(285, 116);
            this.ComprasTxtDescripcionProducto.TabIndex = 23;
            this.ComprasTxtDescripcionProducto.TabStop = false;
            // 
            // ComprasLblPesoProducto
            // 
            this.ComprasLblPesoProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblPesoProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPesoProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPesoProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPesoProducto.Location = new System.Drawing.Point(3, 211);
            this.ComprasLblPesoProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPesoProducto.MostrarAsterisco = false;
            this.ComprasLblPesoProducto.Name = "ComprasLblPesoProducto";
            this.ComprasLblPesoProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblPesoProducto.TabIndex = 25;
            this.ComprasLblPesoProducto.TabStop = false;
            this.ComprasLblPesoProducto.TamanoTipografia = 9F;
            this.ComprasLblPesoProducto.Text = "Peso";
            this.ComprasLblPesoProducto.Texto = "Peso";
            // 
            // ComprasTxtPesoProducto
            // 
            this.ComprasTxtPesoProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtPesoProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtPesoProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtPesoProducto.Enabled = false;
            this.ComprasTxtPesoProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtPesoProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtPesoProducto.Location = new System.Drawing.Point(127, 202);
            this.ComprasTxtPesoProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtPesoProducto.MaxLength = 5;
            this.ComprasTxtPesoProducto.MostrarEtiquetaError = false;
            this.ComprasTxtPesoProducto.Name = "ComprasTxtPesoProducto";
            this.ComprasTxtPesoProducto.ReadOnly = true;
            this.ComprasTxtPesoProducto.SelectedText = "";
            this.ComprasTxtPesoProducto.SelectionLength = 0;
            this.ComprasTxtPesoProducto.SelectionStart = 0;
            this.ComprasTxtPesoProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtPesoProducto.TabIndex = 26;
            this.ComprasTxtPesoProducto.TabStop = false;
            // 
            // ComprasTxtCostoUnitarioProducto
            // 
            this.ComprasTxtCostoUnitarioProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtCostoUnitarioProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCostoUnitarioProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCostoUnitarioProducto.Enabled = false;
            this.ComprasTxtCostoUnitarioProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCostoUnitarioProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCostoUnitarioProducto.Location = new System.Drawing.Point(127, 242);
            this.ComprasTxtCostoUnitarioProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCostoUnitarioProducto.MaxLength = 32;
            this.ComprasTxtCostoUnitarioProducto.ModoMoneda = true;
            this.ComprasTxtCostoUnitarioProducto.MostrarEtiquetaError = false;
            this.ComprasTxtCostoUnitarioProducto.Name = "ComprasTxtCostoUnitarioProducto";
            this.ComprasTxtCostoUnitarioProducto.ReadOnly = true;
            this.ComprasTxtCostoUnitarioProducto.SelectedText = "";
            this.ComprasTxtCostoUnitarioProducto.SelectionLength = 0;
            this.ComprasTxtCostoUnitarioProducto.SelectionStart = 0;
            this.ComprasTxtCostoUnitarioProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtCostoUnitarioProducto.TabIndex = 28;
            this.ComprasTxtCostoUnitarioProducto.TabStop = false;
            this.ComprasTxtCostoUnitarioProducto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblCostoUnitarioProducto
            // 
            this.ComprasLblCostoUnitarioProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblCostoUnitarioProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCostoUnitarioProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCostoUnitarioProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCostoUnitarioProducto.Location = new System.Drawing.Point(3, 251);
            this.ComprasLblCostoUnitarioProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCostoUnitarioProducto.MostrarAsterisco = false;
            this.ComprasLblCostoUnitarioProducto.Name = "ComprasLblCostoUnitarioProducto";
            this.ComprasLblCostoUnitarioProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblCostoUnitarioProducto.TabIndex = 29;
            this.ComprasLblCostoUnitarioProducto.TabStop = false;
            this.ComprasLblCostoUnitarioProducto.TamanoTipografia = 9F;
            this.ComprasLblCostoUnitarioProducto.Text = "Precio Unitario";
            this.ComprasLblCostoUnitarioProducto.Texto = "Precio Unitario";
            // 
            // ComprasLblCantidadProducto
            // 
            this.ComprasLblCantidadProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblCantidadProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCantidadProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCantidadProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCantidadProducto.Location = new System.Drawing.Point(3, 371);
            this.ComprasLblCantidadProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCantidadProducto.Name = "ComprasLblCantidadProducto";
            this.ComprasLblCantidadProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblCantidadProducto.TabIndex = 27;
            this.ComprasLblCantidadProducto.TabStop = false;
            this.ComprasLblCantidadProducto.TamanoTipografia = 9F;
            this.ComprasLblCantidadProducto.Text = "Cantidad";
            this.ComprasLblCantidadProducto.Texto = "Cantidad";
            // 
            // ComprasLblDescuentoProducto
            // 
            this.ComprasLblDescuentoProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblDescuentoProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescuentoProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescuentoProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescuentoProducto.Location = new System.Drawing.Point(3, 291);
            this.ComprasLblDescuentoProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDescuentoProducto.MostrarAsterisco = false;
            this.ComprasLblDescuentoProducto.Name = "ComprasLblDescuentoProducto";
            this.ComprasLblDescuentoProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblDescuentoProducto.TabIndex = 30;
            this.ComprasLblDescuentoProducto.TabStop = false;
            this.ComprasLblDescuentoProducto.TamanoTipografia = 9F;
            this.ComprasLblDescuentoProducto.Text = "Descuento";
            this.ComprasLblDescuentoProducto.Texto = "Descuento";
            // 
            // ComprasTxtDescuentoProducto
            // 
            this.ComprasTxtDescuentoProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDescuentoProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuentoProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuentoProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuentoProducto.Location = new System.Drawing.Point(127, 282);
            this.ComprasTxtDescuentoProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoProducto.MaxLength = 32;
            this.ComprasTxtDescuentoProducto.ModoMoneda = true;
            this.ComprasTxtDescuentoProducto.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoProducto.Name = "ComprasTxtDescuentoProducto";
            this.ComprasTxtDescuentoProducto.SelectedText = "";
            this.ComprasTxtDescuentoProducto.SelectionLength = 0;
            this.ComprasTxtDescuentoProducto.SelectionStart = 0;
            this.ComprasTxtDescuentoProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtDescuentoProducto.TabIndex = 31;
            this.ComprasTxtDescuentoProducto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasTxtDescuentoPorcentajeProducto
            // 
            this.ComprasTxtDescuentoPorcentajeProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDescuentoPorcentajeProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuentoPorcentajeProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuentoPorcentajeProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoPorcentajeProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuentoPorcentajeProducto.Location = new System.Drawing.Point(127, 322);
            this.ComprasTxtDescuentoPorcentajeProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoPorcentajeProducto.MaxLength = 8;
            this.ComprasTxtDescuentoPorcentajeProducto.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoPorcentajeProducto.Name = "ComprasTxtDescuentoPorcentajeProducto";
            this.ComprasTxtDescuentoPorcentajeProducto.SelectedText = "";
            this.ComprasTxtDescuentoPorcentajeProducto.SelectionLength = 0;
            this.ComprasTxtDescuentoPorcentajeProducto.SelectionStart = 0;
            this.ComprasTxtDescuentoPorcentajeProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtDescuentoPorcentajeProducto.TabIndex = 32;
            this.ComprasTxtDescuentoPorcentajeProducto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Porcentaje;
            // 
            // ComprasTxtCantidadProducto
            // 
            this.ComprasTxtCantidadProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtCantidadProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCantidadProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCantidadProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCantidadProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCantidadProducto.Location = new System.Drawing.Point(127, 362);
            this.ComprasTxtCantidadProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCantidadProducto.MaxLength = 8;
            this.ComprasTxtCantidadProducto.MostrarEtiquetaError = false;
            this.ComprasTxtCantidadProducto.Name = "ComprasTxtCantidadProducto";
            this.ComprasTxtCantidadProducto.SelectedText = "";
            this.ComprasTxtCantidadProducto.SelectionLength = 0;
            this.ComprasTxtCantidadProducto.SelectionStart = 0;
            this.ComprasTxtCantidadProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtCantidadProducto.SoloEnteros = true;
            this.ComprasTxtCantidadProducto.TabIndex = 33;
            this.ComprasTxtCantidadProducto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Enteros;
            // 
            // ComprasTlpAccionesProducto
            // 
            this.ComprasTlpAccionesProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpAccionesProducto.ColumnCount = 5;
            this.ComprasTlpProducto.SetColumnSpan(this.ComprasTlpAccionesProducto, 2);
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpAccionesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpAccionesProducto.Controls.Add(this.ComprasBtnEliminarProducto, 4, 0);
            this.ComprasTlpAccionesProducto.Controls.Add(this.ComprasBtnModificarProducto, 3, 0);
            this.ComprasTlpAccionesProducto.Controls.Add(this.ComprasBtnInsertarProducto, 2, 0);
            this.ComprasTlpAccionesProducto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAccionesProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpAccionesProducto.Location = new System.Drawing.Point(0, 450);
            this.ComprasTlpAccionesProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpAccionesProducto.Name = "ComprasTlpAccionesProducto";
            this.ComprasTlpAccionesProducto.RowCount = 1;
            this.ComprasTlpAccionesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAccionesProducto.Size = new System.Drawing.Size(414, 90);
            this.ComprasTlpAccionesProducto.TabIndex = 34;
            // 
            // ComprasBtnEliminarProducto
            // 
            this.ComprasBtnEliminarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnEliminarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEliminarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEliminarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnEliminarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEliminarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEliminarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.Location = new System.Drawing.Point(329, 5);
            this.ComprasBtnEliminarProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnEliminarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.Name = "ComprasBtnEliminarProducto";
            this.ComprasBtnEliminarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.TabIndex = 2;
            this.ComprasBtnEliminarProducto.Text = "Eliminar";
            this.ComprasBtnEliminarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnModificarProducto
            // 
            this.ComprasBtnModificarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnModificarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnModificarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnModificarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnModificarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnModificarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnModificarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnModificarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnModificarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnModificarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.Location = new System.Drawing.Point(239, 5);
            this.ComprasBtnModificarProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnModificarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.Name = "ComprasBtnModificarProducto";
            this.ComprasBtnModificarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.TabIndex = 1;
            this.ComprasBtnModificarProducto.Text = "Modificar";
            this.ComprasBtnModificarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnInsertarProducto
            // 
            this.ComprasBtnInsertarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnInsertarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnInsertarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnInsertarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnInsertarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnInsertarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnInsertarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnInsertarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnInsertarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnInsertarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnInsertarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnInsertarProducto.Location = new System.Drawing.Point(149, 5);
            this.ComprasBtnInsertarProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnInsertarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnInsertarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnInsertarProducto.Name = "ComprasBtnInsertarProducto";
            this.ComprasBtnInsertarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnInsertarProducto.TabIndex = 0;
            this.ComprasBtnInsertarProducto.Text = "Insertar";
            this.ComprasBtnInsertarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasLblTotalProducto
            // 
            this.ComprasLblTotalProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblTotalProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotalProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTotalProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotalProducto.Location = new System.Drawing.Point(3, 411);
            this.ComprasLblTotalProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotalProducto.MostrarAsterisco = false;
            this.ComprasLblTotalProducto.Name = "ComprasLblTotalProducto";
            this.ComprasLblTotalProducto.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblTotalProducto.TabIndex = 35;
            this.ComprasLblTotalProducto.TabStop = false;
            this.ComprasLblTotalProducto.TamanoTipografia = 9F;
            this.ComprasLblTotalProducto.Text = "Total";
            this.ComprasLblTotalProducto.Texto = "Total";
            // 
            // ComprasTxtTotalProducto
            // 
            this.ComprasTxtTotalProducto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtTotalProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTotalProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTotalProducto.Enabled = false;
            this.ComprasTxtTotalProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalProducto.Location = new System.Drawing.Point(127, 402);
            this.ComprasTxtTotalProducto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTotalProducto.MaxLength = 32767;
            this.ComprasTxtTotalProducto.ModoMoneda = true;
            this.ComprasTxtTotalProducto.MostrarEtiquetaError = false;
            this.ComprasTxtTotalProducto.Name = "ComprasTxtTotalProducto";
            this.ComprasTxtTotalProducto.ReadOnly = true;
            this.ComprasTxtTotalProducto.SelectedText = "";
            this.ComprasTxtTotalProducto.SelectionLength = 0;
            this.ComprasTxtTotalProducto.SelectionStart = 0;
            this.ComprasTxtTotalProducto.Size = new System.Drawing.Size(285, 36);
            this.ComprasTxtTotalProducto.TabIndex = 36;
            this.ComprasTxtTotalProducto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasGbxFinalizacion
            // 
            this.ComprasGbxFinalizacion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxFinalizacion.Controls.Add(this.ComprasTlpFinalizacion);
            this.ComprasGbxFinalizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxFinalizacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxFinalizacion.Location = new System.Drawing.Point(10, 1471);
            this.ComprasGbxFinalizacion.Name = "ComprasGbxFinalizacion";
            this.ComprasGbxFinalizacion.Size = new System.Drawing.Size(1244, 271);
            this.ComprasGbxFinalizacion.TabIndex = 5;
            this.ComprasGbxFinalizacion.TabStop = false;
            this.ComprasGbxFinalizacion.Text = "6. Revisión Final y Comentario";
            // 
            // ComprasTlpFinalizacion
            // 
            this.ComprasTlpFinalizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFinalizacion.ColumnCount = 2;
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpComentario, 1, 0);
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpTotales, 0, 0);
            this.ComprasTlpFinalizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFinalizacion.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpFinalizacion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFinalizacion.Name = "ComprasTlpFinalizacion";
            this.ComprasTlpFinalizacion.RowCount = 1;
            this.ComprasTlpFinalizacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFinalizacion.Size = new System.Drawing.Size(1238, 244);
            this.ComprasTlpFinalizacion.TabIndex = 4;
            // 
            // ComprasTlpComentario
            // 
            this.ComprasTlpComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpComentario.ColumnCount = 1;
            this.ComprasTlpComentario.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpComentario.Controls.Add(this.ComprasLblEtiquetaComentario, 0, 0);
            this.ComprasTlpComentario.Controls.Add(this.ComprasTxtComentario, 0, 1);
            this.ComprasTlpComentario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpComentario.Location = new System.Drawing.Point(441, 0);
            this.ComprasTlpComentario.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ComprasTlpComentario.Name = "ComprasTlpComentario";
            this.ComprasTlpComentario.RowCount = 2;
            this.ComprasTlpComentario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpComentario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpComentario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpComentario.Size = new System.Drawing.Size(789, 244);
            this.ComprasTlpComentario.TabIndex = 6;
            // 
            // ComprasLblEtiquetaComentario
            // 
            this.ComprasLblEtiquetaComentario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblEtiquetaComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaComentario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaComentario.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblEtiquetaComentario.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaComentario.MostrarAsterisco = false;
            this.ComprasLblEtiquetaComentario.Name = "ComprasLblEtiquetaComentario";
            this.ComprasLblEtiquetaComentario.Size = new System.Drawing.Size(89, 18);
            this.ComprasLblEtiquetaComentario.TabIndex = 12;
            this.ComprasLblEtiquetaComentario.TabStop = false;
            this.ComprasLblEtiquetaComentario.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaComentario.Text = "Comentario";
            this.ComprasLblEtiquetaComentario.Texto = "Comentario";
            // 
            // ComprasTxtComentario
            // 
            this.ComprasTxtComentario.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtComentario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtComentario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtComentario.Location = new System.Drawing.Point(0, 24);
            this.ComprasTxtComentario.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.ComprasTxtComentario.MaxLength = 500;
            this.ComprasTxtComentario.MostrarEtiquetaError = false;
            this.ComprasTxtComentario.Multiline = true;
            this.ComprasTxtComentario.Name = "ComprasTxtComentario";
            this.ComprasTxtComentario.SelectedText = "";
            this.ComprasTxtComentario.SelectionLength = 0;
            this.ComprasTxtComentario.SelectionStart = 0;
            this.ComprasTxtComentario.Size = new System.Drawing.Size(789, 212);
            this.ComprasTxtComentario.TabIndex = 9;
            // 
            // ComprasTlpTotales
            // 
            this.ComprasTlpTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.ColumnCount = 2;
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblTotalOrden, 0, 5);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTotalOrden, 1, 5);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblImpuestoTotal, 0, 4);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtImpuestoTotal, 1, 4);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtSubtotalNeto, 1, 3);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtDescuentoTotal, 1, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblDescuentoTotal, 0, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblSubtotalOrden, 0, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtSubtotalOrden, 1, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtDescuentoPorcentajeTotal, 1, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblDescuentoPorcentajeTotal, 0, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblSubtotalNeto, 0, 3);
            this.ComprasTxtDescuentoPorcentajeTotal.Name = "ComprasTxtDescuentoPorcentajeTotal";
            this.ComprasTxtDescuentoPorcentajeTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoPorcentajeTotal.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComprasTxtDescuentoPorcentajeTotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoPorcentajeTotal.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoPorcentajeTotal.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtDescuentoPorcentajeTotal.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Porcentaje;
            this.ComprasTxtDescuentoPorcentajeTotal.AlineacionTexto = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDescuentoPorcentajeTotal.TabIndex = 9;
            this.ComprasTxtDescuentoPorcentajeTotal.Enabled = false;
            this.ComprasTxtDescuentoPorcentajeTotal.ReadOnly = true;
            this.ComprasTxtDescuentoPorcentajeTotal.TabStop = false;
            this.ComprasLblSubtotalNeto.Name = "ComprasLblSubtotalNeto";
            this.ComprasLblSubtotalNeto.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblSubtotalNeto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSubtotalNeto.TamanoTipografia = 9F;
            this.ComprasLblSubtotalNeto.MostrarAsterisco = false;
            this.ComprasLblSubtotalNeto.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblSubtotalNeto.TabStop = false;
            this.ComprasLblSubtotalNeto.Text = "Subtotal neto";
            this.ComprasLblSubtotalNeto.Texto = "Subtotal neto";
            this.ComprasLblDescuentoPorcentajeTotal.Name = "ComprasLblDescuentoPorcentajeTotal";
            this.ComprasLblDescuentoPorcentajeTotal.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblDescuentoPorcentajeTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescuentoPorcentajeTotal.TamanoTipografia = 9F;
            this.ComprasLblDescuentoPorcentajeTotal.MostrarAsterisco = false;
            this.ComprasLblDescuentoPorcentajeTotal.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblDescuentoPorcentajeTotal.TabStop = false;
            this.ComprasLblDescuentoPorcentajeTotal.Text = "Descuento (%)";
            this.ComprasLblDescuentoPorcentajeTotal.Texto = "Descuento (%)";
            this.ComprasTlpTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotales.Location = new System.Drawing.Point(8, 0);
            this.ComprasTlpTotales.Margin = new System.Windows.Forms.Padding(8, 0, 16, 0);
            this.ComprasTlpTotales.Name = "ComprasTlpTotales";
            this.ComprasTlpTotales.RowCount = 7;
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotales.Size = new System.Drawing.Size(409, 244);
            this.ComprasTlpTotales.TabIndex = 5;
            // 
            // ComprasLblTotalOrden
            // 
            this.ComprasLblTotalOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblTotalOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotalOrden.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTotalOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotalOrden.Location = new System.Drawing.Point(3, 192);
            this.ComprasLblTotalOrden.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotalOrden.MostrarAsterisco = false;
            this.ComprasLblTotalOrden.Name = "ComprasLblTotalOrden";
            this.ComprasLblTotalOrden.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblTotalOrden.TabIndex = 4;
            this.ComprasLblTotalOrden.TabStop = false;
            this.ComprasLblTotalOrden.TamanoTipografia = 9F;
            this.ComprasLblTotalOrden.Text = "Total de la orden";
            this.ComprasLblTotalOrden.Texto = "Total de la orden";
            // 
            // ComprasTxtTotalOrden
            // 
            this.ComprasTxtTotalOrden.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtTotalOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTotalOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTotalOrden.Enabled = false;
            this.ComprasTxtTotalOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalOrden.Location = new System.Drawing.Point(132, 183);
            this.ComprasTxtTotalOrden.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTotalOrden.MaxLength = 32;
            this.ComprasTxtTotalOrden.ModoMoneda = true;
            this.ComprasTxtTotalOrden.MostrarEtiquetaError = false;
            this.ComprasTxtTotalOrden.Name = "ComprasTxtTotalOrden";
            this.ComprasTxtTotalOrden.ReadOnly = true;
            this.ComprasTxtTotalOrden.SelectedText = "";
            this.ComprasTxtTotalOrden.SelectionLength = 0;
            this.ComprasTxtTotalOrden.SelectionStart = 0;
            this.ComprasTxtTotalOrden.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtTotalOrden.TabIndex = 14;
            this.ComprasTxtTotalOrden.TabStop = false;
            this.ComprasTxtTotalOrden.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblImpuestoTotal
            // 
            this.ComprasLblImpuestoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblImpuestoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblImpuestoTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblImpuestoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblImpuestoTotal.Location = new System.Drawing.Point(3, 152);
            this.ComprasLblImpuestoTotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblImpuestoTotal.MostrarAsterisco = false;
            this.ComprasLblImpuestoTotal.Name = "ComprasLblImpuestoTotal";
            this.ComprasLblImpuestoTotal.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblImpuestoTotal.TabIndex = 3;
            this.ComprasLblImpuestoTotal.TabStop = false;
            this.ComprasLblImpuestoTotal.TamanoTipografia = 9F;
            this.ComprasLblImpuestoTotal.Text = "Impuestos";
            this.ComprasLblImpuestoTotal.Texto = "Impuestos";
            // 
            // ComprasTxtImpuestoTotal
            // 
            this.ComprasTxtImpuestoTotal.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtImpuestoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtImpuestoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtImpuestoTotal.Enabled = false;
            this.ComprasTxtImpuestoTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtImpuestoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtImpuestoTotal.Location = new System.Drawing.Point(132, 143);
            this.ComprasTxtImpuestoTotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtImpuestoTotal.MaxLength = 32;
            this.ComprasTxtImpuestoTotal.ModoMoneda = true;
            this.ComprasTxtImpuestoTotal.MostrarEtiquetaError = false;
            this.ComprasTxtImpuestoTotal.Name = "ComprasTxtImpuestoTotal";
            this.ComprasTxtImpuestoTotal.ReadOnly = true;
            this.ComprasTxtImpuestoTotal.SelectedText = "";
            this.ComprasTxtImpuestoTotal.SelectionLength = 0;
            this.ComprasTxtImpuestoTotal.SelectionStart = 0;
            this.ComprasTxtImpuestoTotal.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtImpuestoTotal.TabIndex = 13;
            this.ComprasTxtImpuestoTotal.TabStop = false;
            this.ComprasTxtImpuestoTotal.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasTxtSubtotalNeto
            // 
            this.ComprasTxtSubtotalNeto.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtSubtotalNeto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtSubtotalNeto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSubtotalNeto.Enabled = false;
            this.ComprasTxtSubtotalNeto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSubtotalNeto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSubtotalNeto.Location = new System.Drawing.Point(132, 103);
            this.ComprasTxtSubtotalNeto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtSubtotalNeto.MaxLength = 32;
            this.ComprasTxtSubtotalNeto.ModoMoneda = true;
            this.ComprasTxtSubtotalNeto.MostrarEtiquetaError = false;
            this.ComprasTxtSubtotalNeto.Name = "ComprasTxtSubtotalNeto";
            this.ComprasTxtSubtotalNeto.ReadOnly = true;
            this.ComprasTxtSubtotalNeto.SelectedText = "";
            this.ComprasTxtSubtotalNeto.SelectionLength = 0;
            this.ComprasTxtSubtotalNeto.SelectionStart = 0;
            this.ComprasTxtSubtotalNeto.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtSubtotalNeto.TabIndex = 5;
            this.ComprasTxtSubtotalNeto.TabStop = false;
            this.ComprasTxtSubtotalNeto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasTxtDescuentoTotal
            // 
            this.ComprasTxtDescuentoTotal.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDescuentoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuentoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuentoTotal.Enabled = false;
            this.ComprasTxtDescuentoTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuentoTotal.Location = new System.Drawing.Point(132, 63);
            this.ComprasTxtDescuentoTotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoTotal.MaxLength = 32;
            this.ComprasTxtDescuentoTotal.ModoMoneda = true;
            this.ComprasTxtDescuentoTotal.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoTotal.Name = "ComprasTxtDescuentoTotal";
            this.ComprasTxtDescuentoTotal.ReadOnly = true;
            this.ComprasTxtDescuentoTotal.SelectedText = "";
            this.ComprasTxtDescuentoTotal.SelectionLength = 0;
            this.ComprasTxtDescuentoTotal.SelectionStart = 0;
            this.ComprasTxtDescuentoTotal.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtDescuentoTotal.TabIndex = 8;
            this.ComprasTxtDescuentoTotal.TabStop = false;
            this.ComprasTxtDescuentoTotal.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblDescuentoTotal
            // 
            this.ComprasLblDescuentoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblDescuentoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescuentoTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescuentoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescuentoTotal.Location = new System.Drawing.Point(3, 72);
            this.ComprasLblDescuentoTotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDescuentoTotal.MostrarAsterisco = false;
            this.ComprasLblDescuentoTotal.Name = "ComprasLblDescuentoTotal";
            this.ComprasLblDescuentoTotal.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblDescuentoTotal.TabIndex = 2;
            this.ComprasLblDescuentoTotal.TabStop = false;
            this.ComprasLblDescuentoTotal.TamanoTipografia = 9F;
            this.ComprasLblDescuentoTotal.Text = "Descuentos";
            this.ComprasLblDescuentoTotal.Texto = "Descuentos";
            // 
            // ComprasLblSubtotalOrden
            // 
            this.ComprasLblSubtotalOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblSubtotalOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSubtotalOrden.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSubtotalOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSubtotalOrden.Location = new System.Drawing.Point(3, 32);
            this.ComprasLblSubtotalOrden.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSubtotalOrden.MostrarAsterisco = false;
            this.ComprasLblSubtotalOrden.Name = "ComprasLblSubtotalOrden";
            this.ComprasLblSubtotalOrden.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblSubtotalOrden.TabIndex = 11;
            this.ComprasLblSubtotalOrden.TabStop = false;
            this.ComprasLblSubtotalOrden.TamanoTipografia = 9F;
            this.ComprasLblSubtotalOrden.Text = "Subtotal";
            this.ComprasLblSubtotalOrden.Texto = "Subtotal";
            // 
            // ComprasTxtSubtotalOrden
            // 
            this.ComprasTxtSubtotalOrden.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtSubtotalOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtSubtotalOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSubtotalOrden.Enabled = false;
            this.ComprasTxtSubtotalOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSubtotalOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSubtotalOrden.Location = new System.Drawing.Point(132, 23);
            this.ComprasTxtSubtotalOrden.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtSubtotalOrden.MaxLength = 32;
            this.ComprasTxtSubtotalOrden.ModoMoneda = true;
            this.ComprasTxtSubtotalOrden.MostrarEtiquetaError = false;
            this.ComprasTxtSubtotalOrden.Name = "ComprasTxtSubtotalOrden";
            this.ComprasTxtSubtotalOrden.ReadOnly = true;
            this.ComprasTxtSubtotalOrden.SelectedText = "";
            this.ComprasTxtSubtotalOrden.SelectionLength = 0;
            this.ComprasTxtSubtotalOrden.SelectionStart = 0;
            this.ComprasTxtSubtotalOrden.Size = new System.Drawing.Size(275, 36);
            this.ComprasTxtSubtotalOrden.TabIndex = 12;
            this.ComprasTxtSubtotalOrden.TabStop = false;
            this.ComprasTxtSubtotalOrden.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasGbxBodega
            // 
            this.ComprasGbxBodega.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxBodega.Controls.Add(this.ComprasTlpBodega);
            this.ComprasGbxBodega.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxBodega.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxBodega.Location = new System.Drawing.Point(14, 734);
            this.ComprasGbxBodega.Name = "ComprasGbxBodega";
            this.ComprasGbxBodega.Size = new System.Drawing.Size(412, 148);
            this.ComprasGbxBodega.TabIndex = 3;
            this.ComprasGbxBodega.TabStop = false;
            this.ComprasGbxBodega.Text = "4. Elegir Sucursal y Bodega";
            // 
            // ComprasTlpBodega
            // 
            this.ComprasTlpBodega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpBodega.ColumnCount = 3;
            this.ComprasTlpBodega.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpBodega.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.ComprasTlpBodega.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpBodega.Controls.Add(this.ComprasCboSucursal, 1, 0);
            this.ComprasTlpBodega.Controls.Add(this.ComprasLblSucursal, 0, 0);
            this.ComprasTlpBodega.Controls.Add(this.ComprasLblBodega, 0, 1);
            this.ComprasTlpBodega.Controls.Add(this.ComprasCboBodega, 1, 1);
            this.ComprasTlpBodega.Controls.Add(this.ComprasLblPaisBodega, 0, 2);
            this.ComprasTlpBodega.Controls.Add(this.ComprasTxtPaisBodega, 1, 2);
            this.ComprasTlpBodega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpBodega.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpBodega.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpBodega.Name = "ComprasTlpBodega";
            this.ComprasTlpBodega.RowCount = 4;
            this.ComprasTlpBodega.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBodega.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBodega.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBodega.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBodega.Size = new System.Drawing.Size(406, 121);
            this.ComprasTlpBodega.TabIndex = 0;
            // 
            // ComprasCboSucursal
            // 
            this.ComprasCboSucursal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboSucursal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboSucursal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboSucursal.Location = new System.Drawing.Point(100, 0);
            this.ComprasCboSucursal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboSucursal.MaxLength = 0;
            this.ComprasCboSucursal.MostrarEtiquetaError = false;
            this.ComprasCboSucursal.Name = "ComprasCboSucursal";
            this.ComprasCboSucursal.Size = new System.Drawing.Size(300, 40);
            this.ComprasCboSucursal.TabIndex = 24;
            this.ComprasCboSucursal.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Sucursales;
            // 
            // ComprasLblSucursal
            // 
            this.ComprasLblSucursal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblSucursal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSucursal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSucursal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSucursal.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblSucursal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSucursal.Name = "ComprasLblSucursal";
            this.ComprasLblSucursal.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblSucursal.TabIndex = 4;
            this.ComprasLblSucursal.TabStop = false;
            this.ComprasLblSucursal.TamanoTipografia = 9F;
            this.ComprasLblSucursal.Text = "Sucursal";
            this.ComprasLblSucursal.Texto = "Sucursal";
            // 
            // ComprasLblBodega
            // 
            this.ComprasLblBodega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblBodega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblBodega.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblBodega.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblBodega.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblBodega.Name = "ComprasLblBodega";
            this.ComprasLblBodega.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblBodega.TabIndex = 5;
            this.ComprasLblBodega.TabStop = false;
            this.ComprasLblBodega.TamanoTipografia = 9F;
            this.ComprasLblBodega.Text = "Bodega";
            this.ComprasLblBodega.Texto = "Bodega";
            // 
            // ComprasCboBodega
            // 
            this.ComprasCboBodega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboBodega.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboBodega.Location = new System.Drawing.Point(100, 40);
            this.ComprasCboBodega.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboBodega.MaxLength = 0;
            this.ComprasCboBodega.MostrarEtiquetaError = false;
            this.ComprasCboBodega.Name = "ComprasCboBodega";
            this.ComprasCboBodega.Size = new System.Drawing.Size(300, 40);
            this.ComprasCboBodega.TabIndex = 20;
            this.ComprasCboBodega.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Bodegas;
            // 
            // ComprasLblPaisBodega
            // 
            this.ComprasLblPaisBodega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblPaisBodega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPaisBodega.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPaisBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPaisBodega.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblPaisBodega.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPaisBodega.MostrarAsterisco = false;
            this.ComprasLblPaisBodega.Name = "ComprasLblPaisBodega";
            this.ComprasLblPaisBodega.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblPaisBodega.TabIndex = 22;
            this.ComprasLblPaisBodega.TabStop = false;
            this.ComprasLblPaisBodega.TamanoTipografia = 9F;
            this.ComprasLblPaisBodega.Text = "Pais";
            this.ComprasLblPaisBodega.Texto = "Pais";
            // 
            // ComprasTxtPaisBodega
            // 
            this.ComprasTxtPaisBodega.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtPaisBodega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtPaisBodega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtPaisBodega.Enabled = false;
            this.ComprasTxtPaisBodega.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtPaisBodega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtPaisBodega.Location = new System.Drawing.Point(102, 82);
            this.ComprasTxtPaisBodega.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtPaisBodega.MaxLength = 100;
            this.ComprasTxtPaisBodega.MostrarEtiquetaError = false;
            this.ComprasTxtPaisBodega.Name = "ComprasTxtPaisBodega";
            this.ComprasTxtPaisBodega.ReadOnly = true;
            this.ComprasTxtPaisBodega.SelectedText = "";
            this.ComprasTxtPaisBodega.SelectionLength = 0;
            this.ComprasTxtPaisBodega.SelectionStart = 0;
            this.ComprasTxtPaisBodega.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtPaisBodega.TabIndex = 23;
            this.ComprasTxtPaisBodega.TabStop = false;
            // 
            // ComprasGbxPlazo
            // 
            this.ComprasGbxPlazo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxPlazo.Controls.Add(this.ComprasTlpPlazo);
            this.ComprasGbxPlazo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxPlazo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxPlazo.Location = new System.Drawing.Point(10, 550);
            this.ComprasGbxPlazo.Name = "ComprasGbxPlazo";
            this.ComprasGbxPlazo.Size = new System.Drawing.Size(409, 151);
            this.ComprasGbxPlazo.TabIndex = 2;
            this.ComprasGbxPlazo.TabStop = false;
            this.ComprasGbxPlazo.Text = "3. Confirmar Plazo";
            // 
            // ComprasTlpPlazo
            // 
            this.ComprasTlpPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPlazo.ColumnCount = 2;
            this.ComprasTlpPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.ComprasTlpPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPlazo.Controls.Add(this.ComprasLblFechaEmision, 0, 0);
            this.ComprasTlpPlazo.Controls.Add(this.ComprasTxtFechaEmision, 1, 0);
            this.ComprasTlpPlazo.Controls.Add(this.ComprasLblPlazo, 0, 1);
            this.ComprasTlpPlazo.Controls.Add(this.ComprasCboPlazo, 1, 1);
            this.ComprasTlpPlazo.Controls.Add(this.ComprasLblFechaVencimiento, 0, 2);
            this.ComprasTlpPlazo.Controls.Add(this.ComprasTxtFechaVencimiento, 1, 2);
            this.ComprasTlpPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPlazo.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpPlazo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPlazo.Name = "ComprasTlpPlazo";
            this.ComprasTlpPlazo.RowCount = 4;
            this.ComprasTlpPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpPlazo.Size = new System.Drawing.Size(403, 124);
            this.ComprasTlpPlazo.TabIndex = 0;
            // 
            // ComprasLblFechaEmision
            // 
            this.ComprasLblFechaEmision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblFechaEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaEmision.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaEmision.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblFechaEmision.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaEmision.MostrarAsterisco = false;
            this.ComprasLblFechaEmision.Name = "ComprasLblFechaEmision";
            this.ComprasLblFechaEmision.Size = new System.Drawing.Size(169, 18);
            this.ComprasLblFechaEmision.TabIndex = 4;
            this.ComprasLblFechaEmision.TabStop = false;
            this.ComprasLblFechaEmision.TamanoTipografia = 9F;
            this.ComprasLblFechaEmision.Text = "Fecha de Emisión";
            this.ComprasLblFechaEmision.Texto = "Fecha de Emisión";
            // 
            // ComprasTxtFechaEmision
            // 
            this.ComprasTxtFechaEmision.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtFechaEmision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtFechaEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtFechaEmision.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtFechaEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtFechaEmision.Location = new System.Drawing.Point(177, 2);
            this.ComprasTxtFechaEmision.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtFechaEmision.MaxLength = 10;
            this.ComprasTxtFechaEmision.MostrarEtiquetaError = false;
            this.ComprasTxtFechaEmision.Name = "ComprasTxtFechaEmision";
            this.ComprasTxtFechaEmision.ReadOnly = true;
            this.ComprasTxtFechaEmision.SelectedText = "";
            this.ComprasTxtFechaEmision.SelectionLength = 0;
            this.ComprasTxtFechaEmision.SelectionStart = 0;
            this.ComprasTxtFechaEmision.Size = new System.Drawing.Size(224, 36);
            this.ComprasTxtFechaEmision.TabIndex = 19;
            this.ComprasTxtFechaEmision.TabStop = false;
            // 
            // ComprasLblPlazo
            // 
            this.ComprasLblPlazo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPlazo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPlazo.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblPlazo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPlazo.Name = "ComprasLblPlazo";
            this.ComprasLblPlazo.Size = new System.Drawing.Size(169, 18);
            this.ComprasLblPlazo.TabIndex = 5;
            this.ComprasLblPlazo.TabStop = false;
            this.ComprasLblPlazo.TamanoTipografia = 9F;
            this.ComprasLblPlazo.Text = "Plazo";
            this.ComprasLblPlazo.Texto = "Plazo";
            // 
            // ComprasCboPlazo
            // 
            this.ComprasCboPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboPlazo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboPlazo.Location = new System.Drawing.Point(175, 40);
            this.ComprasCboPlazo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboPlazo.MaxLength = 0;
            this.ComprasCboPlazo.MostrarEtiquetaError = false;
            this.ComprasCboPlazo.Name = "ComprasCboPlazo";
            this.ComprasCboPlazo.Size = new System.Drawing.Size(228, 40);
            this.ComprasCboPlazo.TabIndex = 20;
            this.ComprasCboPlazo.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Plazos;
            // 
            // ComprasLblFechaVencimiento
            // 
            this.ComprasLblFechaVencimiento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblFechaVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaVencimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaVencimiento.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblFechaVencimiento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaVencimiento.MostrarAsterisco = false;
            this.ComprasLblFechaVencimiento.Name = "ComprasLblFechaVencimiento";
            this.ComprasLblFechaVencimiento.Size = new System.Drawing.Size(169, 18);
            this.ComprasLblFechaVencimiento.TabIndex = 6;
            this.ComprasLblFechaVencimiento.TabStop = false;
            this.ComprasLblFechaVencimiento.TamanoTipografia = 9F;
            this.ComprasLblFechaVencimiento.Text = "Fecha de Vencimiento";
            this.ComprasLblFechaVencimiento.Texto = "Fecha de Vencimiento";
            // 
            // ComprasTxtFechaVencimiento
            // 
            this.ComprasTxtFechaVencimiento.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtFechaVencimiento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtFechaVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtFechaVencimiento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtFechaVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtFechaVencimiento.Location = new System.Drawing.Point(177, 82);
            this.ComprasTxtFechaVencimiento.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtFechaVencimiento.MaxLength = 10;
            this.ComprasTxtFechaVencimiento.MostrarEtiquetaError = false;
            this.ComprasTxtFechaVencimiento.Name = "ComprasTxtFechaVencimiento";
            this.ComprasTxtFechaVencimiento.ReadOnly = true;
            this.ComprasTxtFechaVencimiento.SelectedText = "";
            this.ComprasTxtFechaVencimiento.SelectionLength = 0;
            this.ComprasTxtFechaVencimiento.SelectionStart = 0;
            this.ComprasTxtFechaVencimiento.Size = new System.Drawing.Size(224, 36);
            this.ComprasTxtFechaVencimiento.TabIndex = 21;
            this.ComprasTxtFechaVencimiento.TabStop = false;
            // 
            // ComprasGbxMoneda
            // 
            this.ComprasGbxMoneda.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxMoneda.Controls.Add(this.ComprasTlpMoneda);
            this.ComprasGbxMoneda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxMoneda.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxMoneda.Location = new System.Drawing.Point(10, 160);
            this.ComprasGbxMoneda.Name = "ComprasGbxMoneda";
            this.ComprasGbxMoneda.Size = new System.Drawing.Size(401, 106);
            this.ComprasGbxMoneda.TabIndex = 1;
            this.ComprasGbxMoneda.TabStop = false;
            this.ComprasGbxMoneda.Text = "1. Confirmar Moneda";
            // 
            // ComprasTlpMoneda
            // 
            this.ComprasTlpMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpMoneda.ColumnCount = 2;
            this.ComprasTlpMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.ComprasTlpMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMoneda.Controls.Add(this.ComprasLblMoneda, 0, 0);
            this.ComprasTlpMoneda.Controls.Add(this.ComprasLblTipoCambio, 0, 1);
            this.ComprasTlpMoneda.Controls.Add(this.ComprasCboMoneda, 1, 0);
            this.ComprasTlpMoneda.Controls.Add(this.ComprasTxtTipoCambio, 1, 1);
            this.ComprasTlpMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpMoneda.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpMoneda.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpMoneda.Name = "ComprasTlpMoneda";
            this.ComprasTlpMoneda.RowCount = 3;
            this.ComprasTlpMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpMoneda.Size = new System.Drawing.Size(395, 79);
            this.ComprasTlpMoneda.TabIndex = 0;
            // 
            // ComprasLblMoneda
            // 
            this.ComprasLblMoneda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblMoneda.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblMoneda.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblMoneda.Name = "ComprasLblMoneda";
            this.ComprasLblMoneda.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblMoneda.TabIndex = 7;
            this.ComprasLblMoneda.TabStop = false;
            this.ComprasLblMoneda.TamanoTipografia = 9F;
            this.ComprasLblMoneda.Text = "Moneda";
            this.ComprasLblMoneda.Texto = "Moneda";
            // 
            // ComprasLblTipoCambio
            // 
            this.ComprasLblTipoCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTipoCambio.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTipoCambio.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblTipoCambio.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTipoCambio.MostrarAsterisco = false;
            this.ComprasLblTipoCambio.Name = "ComprasLblTipoCambio";
            this.ComprasLblTipoCambio.Size = new System.Drawing.Size(119, 18);
            this.ComprasLblTipoCambio.TabIndex = 8;
            this.ComprasLblTipoCambio.TabStop = false;
            this.ComprasLblTipoCambio.TamanoTipografia = 9F;
            this.ComprasLblTipoCambio.Text = "Tipo de Cambio";
            this.ComprasLblTipoCambio.Texto = "Tipo de Cambio";
            // 
            // ComprasCboMoneda
            // 
            this.ComprasCboMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboMoneda.Location = new System.Drawing.Point(125, 0);
            this.ComprasCboMoneda.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboMoneda.MaxLength = 0;
            this.ComprasCboMoneda.MostrarEtiquetaError = false;
            this.ComprasCboMoneda.Name = "ComprasCboMoneda";
            this.ComprasCboMoneda.Size = new System.Drawing.Size(270, 40);
            this.ComprasCboMoneda.TabIndex = 22;
            this.ComprasCboMoneda.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Monedas;
            // 
            // ComprasTxtTipoCambio
            // 
            this.ComprasTxtTipoCambio.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtTipoCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTipoCambio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTipoCambio.Location = new System.Drawing.Point(127, 42);
            this.ComprasTxtTipoCambio.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTipoCambio.MaxLength = 13;
            this.ComprasTxtTipoCambio.MostrarEtiquetaError = false;
            this.ComprasTxtTipoCambio.Name = "ComprasTxtTipoCambio";
            this.ComprasTxtTipoCambio.ReadOnly = true;
            this.ComprasTxtTipoCambio.SelectedText = "";
            this.ComprasTxtTipoCambio.SelectionLength = 0;
            this.ComprasTxtTipoCambio.SelectionStart = 0;
            this.ComprasTxtTipoCambio.Size = new System.Drawing.Size(266, 36);
            this.ComprasTxtTipoCambio.TabIndex = 23;
            this.ComprasTxtTipoCambio.TabStop = false;
            // 
            // ComprasGbxProveedor
            // 
            this.ComprasGbxProveedor.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxProveedor.Controls.Add(this.ComprasTlpProveedor);
            this.ComprasGbxProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxProveedor.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxProveedor.Location = new System.Drawing.Point(10, 313);
            this.ComprasGbxProveedor.Name = "ComprasGbxProveedor";
            this.ComprasGbxProveedor.Size = new System.Drawing.Size(888, 191);
            this.ComprasGbxProveedor.TabIndex = 0;
            this.ComprasGbxProveedor.TabStop = false;
            this.ComprasGbxProveedor.Text = "2. Elegir un proveedor";
            // 
            // ComprasTlpProveedor
            // 
            this.ComprasTlpProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpProveedor.ColumnCount = 5;
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtDireccionProveedor, 1, 2);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtNitProveedor, 1, 1);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasCboNombreProveedor, 1, 0);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblNombreProveedor, 0, 0);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblNitProveedor, 0, 1);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblDireccionProveedor, 0, 2);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblTelefonoProveedor, 0, 3);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtTelefonoProveedor, 1, 3);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblCorreoProveedor, 2, 0);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtCorreoProveedor, 3, 0);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblSaldoActualProveedor, 2, 1);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtSaldoActualProveedor, 3, 1);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblEstadoProveedor, 2, 2);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblEstadoProveedorValor, 3, 2);
            this.ComprasTlpProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpProveedor.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpProveedor.Name = "ComprasTlpProveedor";
            this.ComprasTlpProveedor.RowCount = 5;
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.Size = new System.Drawing.Size(882, 164);
            this.ComprasTlpProveedor.TabIndex = 0;
            // 
            // ComprasTxtDireccionProveedor
            // 
            this.ComprasTxtDireccionProveedor.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtDireccionProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDireccionProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDireccionProveedor.Location = new System.Drawing.Point(102, 82);
            this.ComprasTxtDireccionProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDireccionProveedor.MaxLength = 500;
            this.ComprasTxtDireccionProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtDireccionProveedor.Name = "ComprasTxtDireccionProveedor";
            this.ComprasTxtDireccionProveedor.ReadOnly = true;
            this.ComprasTxtDireccionProveedor.SelectedText = "";
            this.ComprasTxtDireccionProveedor.SelectionLength = 0;
            this.ComprasTxtDireccionProveedor.SelectionStart = 0;
            this.ComprasTxtDireccionProveedor.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtDireccionProveedor.TabIndex = 10;
            this.ComprasTxtDireccionProveedor.TabStop = false;
            // 
            // ComprasTxtNitProveedor
            // 
            this.ComprasTxtNitProveedor.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtNitProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtNitProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtNitProveedor.Location = new System.Drawing.Point(102, 42);
            this.ComprasTxtNitProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtNitProveedor.MaxLength = 10;
            this.ComprasTxtNitProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtNitProveedor.Name = "ComprasTxtNitProveedor";
            this.ComprasTxtNitProveedor.ReadOnly = true;
            this.ComprasTxtNitProveedor.SelectedText = "";
            this.ComprasTxtNitProveedor.SelectionLength = 0;
            this.ComprasTxtNitProveedor.SelectionStart = 0;
            this.ComprasTxtNitProveedor.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtNitProveedor.TabIndex = 9;
            this.ComprasTxtNitProveedor.TabStop = false;
            // 
            // ComprasCboNombreProveedor
            // 
            this.ComprasCboNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboNombreProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboNombreProveedor.Location = new System.Drawing.Point(100, 0);
            this.ComprasCboNombreProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboNombreProveedor.MaxLength = 0;
            this.ComprasCboNombreProveedor.MostrarEtiquetaError = false;
            this.ComprasCboNombreProveedor.Name = "ComprasCboNombreProveedor";
            this.ComprasCboNombreProveedor.Size = new System.Drawing.Size(300, 40);
            this.ComprasCboNombreProveedor.TabIndex = 8;
            this.ComprasCboNombreProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasLblNombreProveedor
            // 
            this.ComprasLblNombreProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNombreProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNombreProveedor.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblNombreProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNombreProveedor.Name = "ComprasLblNombreProveedor";
            this.ComprasLblNombreProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblNombreProveedor.TabIndex = 2;
            this.ComprasLblNombreProveedor.TabStop = false;
            this.ComprasLblNombreProveedor.TamanoTipografia = 9F;
            this.ComprasLblNombreProveedor.Text = "Nombre";
            this.ComprasLblNombreProveedor.Texto = "Nombre";
            // 
            // ComprasLblNitProveedor
            // 
            this.ComprasLblNitProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNitProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNitProveedor.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblNitProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNitProveedor.MostrarAsterisco = false;
            this.ComprasLblNitProveedor.Name = "ComprasLblNitProveedor";
            this.ComprasLblNitProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblNitProveedor.TabIndex = 3;
            this.ComprasLblNitProveedor.TabStop = false;
            this.ComprasLblNitProveedor.TamanoTipografia = 9F;
            this.ComprasLblNitProveedor.Text = "NIT";
            this.ComprasLblNitProveedor.Texto = "NIT";
            // 
            // ComprasLblDireccionProveedor
            // 
            this.ComprasLblDireccionProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDireccionProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDireccionProveedor.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblDireccionProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDireccionProveedor.MostrarAsterisco = false;
            this.ComprasLblDireccionProveedor.Name = "ComprasLblDireccionProveedor";
            this.ComprasLblDireccionProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblDireccionProveedor.TabIndex = 4;
            this.ComprasLblDireccionProveedor.TabStop = false;
            this.ComprasLblDireccionProveedor.TamanoTipografia = 9F;
            this.ComprasLblDireccionProveedor.Text = "Dirección";
            this.ComprasLblDireccionProveedor.Texto = "Dirección";
            // 
            // ComprasLblTelefonoProveedor
            // 
            this.ComprasLblTelefonoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTelefonoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTelefonoProveedor.Location = new System.Drawing.Point(3, 131);
            this.ComprasLblTelefonoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTelefonoProveedor.MostrarAsterisco = false;
            this.ComprasLblTelefonoProveedor.Name = "ComprasLblTelefonoProveedor";
            this.ComprasLblTelefonoProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblTelefonoProveedor.TabIndex = 5;
            this.ComprasLblTelefonoProveedor.TabStop = false;
            this.ComprasLblTelefonoProveedor.TamanoTipografia = 9F;
            this.ComprasLblTelefonoProveedor.Text = "Telefono";
            this.ComprasLblTelefonoProveedor.Texto = "Telefono";
            // 
            // ComprasTxtTelefonoProveedor
            // 
            this.ComprasTxtTelefonoProveedor.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtTelefonoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTelefonoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTelefonoProveedor.Location = new System.Drawing.Point(102, 122);
            this.ComprasTxtTelefonoProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTelefonoProveedor.MaxLength = 30;
            this.ComprasTxtTelefonoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtTelefonoProveedor.Name = "ComprasTxtTelefonoProveedor";
            this.ComprasTxtTelefonoProveedor.ReadOnly = true;
            this.ComprasTxtTelefonoProveedor.SelectedText = "";
            this.ComprasTxtTelefonoProveedor.SelectionLength = 0;
            this.ComprasTxtTelefonoProveedor.SelectionStart = 0;
            this.ComprasTxtTelefonoProveedor.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtTelefonoProveedor.TabIndex = 11;
            this.ComprasTxtTelefonoProveedor.TabStop = false;
            // 
            // ComprasLblCorreoProveedor
            // 
            this.ComprasLblCorreoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCorreoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCorreoProveedor.Location = new System.Drawing.Point(403, 11);
            this.ComprasLblCorreoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCorreoProveedor.MostrarAsterisco = false;
            this.ComprasLblCorreoProveedor.Name = "ComprasLblCorreoProveedor";
            this.ComprasLblCorreoProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblCorreoProveedor.TabIndex = 6;
            this.ComprasLblCorreoProveedor.TabStop = false;
            this.ComprasLblCorreoProveedor.TamanoTipografia = 9F;
            this.ComprasLblCorreoProveedor.Text = "E-mail";
            this.ComprasLblCorreoProveedor.Texto = "E-mail";
            // 
            // ComprasTxtCorreoProveedor
            // 
            this.ComprasTxtCorreoProveedor.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtCorreoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCorreoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCorreoProveedor.Location = new System.Drawing.Point(502, 2);
            this.ComprasTxtCorreoProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCorreoProveedor.MaxLength = 150;
            this.ComprasTxtCorreoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtCorreoProveedor.Name = "ComprasTxtCorreoProveedor";
            this.ComprasTxtCorreoProveedor.ReadOnly = true;
            this.ComprasTxtCorreoProveedor.SelectedText = "";
            this.ComprasTxtCorreoProveedor.SelectionLength = 0;
            this.ComprasTxtCorreoProveedor.SelectionStart = 0;
            this.ComprasTxtCorreoProveedor.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtCorreoProveedor.TabIndex = 12;
            this.ComprasTxtCorreoProveedor.TabStop = false;
            // 
            // ComprasLblSaldoActualProveedor
            // 
            this.ComprasLblSaldoActualProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblSaldoActualProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSaldoActualProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSaldoActualProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSaldoActualProveedor.Location = new System.Drawing.Point(403, 51);
            this.ComprasLblSaldoActualProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSaldoActualProveedor.MostrarAsterisco = false;
            this.ComprasLblSaldoActualProveedor.Name = "ComprasLblSaldoActualProveedor";
            this.ComprasLblSaldoActualProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblSaldoActualProveedor.TabIndex = 16;
            this.ComprasLblSaldoActualProveedor.TabStop = false;
            this.ComprasLblSaldoActualProveedor.TamanoTipografia = 9F;
            this.ComprasLblSaldoActualProveedor.Text = "Saldo Actual";
            this.ComprasLblSaldoActualProveedor.Texto = "Saldo Actual";
            // 
            // ComprasTxtSaldoActualProveedor
            // 
            this.ComprasTxtSaldoActualProveedor.AlineacionMoneda = System.Windows.Forms.HorizontalAlignment.Left;
            this.ComprasTxtSaldoActualProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtSaldoActualProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSaldoActualProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSaldoActualProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSaldoActualProveedor.Location = new System.Drawing.Point(502, 42);
            this.ComprasTxtSaldoActualProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtSaldoActualProveedor.MaxLength = 32;
            this.ComprasTxtSaldoActualProveedor.ModoMoneda = true;
            this.ComprasTxtSaldoActualProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtSaldoActualProveedor.Name = "ComprasTxtSaldoActualProveedor";
            this.ComprasTxtSaldoActualProveedor.ReadOnly = true;
            this.ComprasTxtSaldoActualProveedor.SelectedText = "";
            this.ComprasTxtSaldoActualProveedor.SelectionLength = 0;
            this.ComprasTxtSaldoActualProveedor.SelectionStart = 0;
            this.ComprasTxtSaldoActualProveedor.Size = new System.Drawing.Size(296, 36);
            this.ComprasTxtSaldoActualProveedor.TabIndex = 17;
            this.ComprasTxtSaldoActualProveedor.TabStop = false;
            this.ComprasTxtSaldoActualProveedor.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblEstadoProveedor
            // 
            this.ComprasLblEstadoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEstadoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstadoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstadoProveedor.Location = new System.Drawing.Point(403, 111);
            this.ComprasLblEstadoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstadoProveedor.MostrarAsterisco = false;
            this.ComprasLblEstadoProveedor.Name = "ComprasLblEstadoProveedor";
            this.ComprasTlpProveedor.SetRowSpan(this.ComprasLblEstadoProveedor, 2);
            this.ComprasLblEstadoProveedor.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblEstadoProveedor.TabIndex = 7;
            this.ComprasLblEstadoProveedor.TabStop = false;
            this.ComprasLblEstadoProveedor.TamanoTipografia = 9F;
            this.ComprasLblEstadoProveedor.Text = "Estado";
            this.ComprasLblEstadoProveedor.Texto = "Estado";
            // 
            // ComprasLblEstadoProveedorValor
            // 
            this.ComprasLblEstadoProveedorValor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoProveedorValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEstadoProveedorValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoProveedorValor.ColorTexto = System.Drawing.Color.Red;
            this.ComprasLblEstadoProveedorValor.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblEstadoProveedorValor.ForeColor = System.Drawing.Color.Red;
            this.ComprasLblEstadoProveedorValor.Location = new System.Drawing.Point(504, 106);
            this.ComprasLblEstadoProveedorValor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComprasLblEstadoProveedorValor.MaximumSize = new System.Drawing.Size(1375, 29);
            this.ComprasLblEstadoProveedorValor.MostrarAsterisco = false;
            this.ComprasLblEstadoProveedorValor.Name = "ComprasLblEstadoProveedorValor";
            this.ComprasTlpProveedor.SetRowSpan(this.ComprasLblEstadoProveedorValor, 2);
            this.ComprasLblEstadoProveedorValor.Size = new System.Drawing.Size(292, 27);
            this.ComprasLblEstadoProveedorValor.TabIndex = 15;
            this.ComprasLblEstadoProveedorValor.TabStop = false;
            this.ComprasLblEstadoProveedorValor.TamanoTipografia = 12F;
            this.ComprasLblEstadoProveedorValor.Text = "PENDIENTE";
            this.ComprasLblEstadoProveedorValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoProveedorValor.Texto = "PENDIENTE";
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasPnlContenido);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 713);
            this.ComprasPnlDesplazamiento.TabIndex = 1;
            // 
            // FrmOrdenesCompra
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 713);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmOrdenesCompra";
            this.Text = "6003 - OrdenesCompra";
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpDetalles.ResumeLayout(false);
            this.ComprasGbxProducto.ResumeLayout(false);
            this.ComprasTlpProducto.ResumeLayout(false);
            this.ComprasTlpAccionesProducto.ResumeLayout(false);
            this.ComprasGbxFinalizacion.ResumeLayout(false);
            this.ComprasTlpFinalizacion.ResumeLayout(false);
            this.ComprasTlpComentario.ResumeLayout(false);
            this.ComprasTlpTotales.ResumeLayout(false);
            this.ComprasGbxBodega.ResumeLayout(false);
            this.ComprasTlpBodega.ResumeLayout(false);
            this.ComprasGbxPlazo.ResumeLayout(false);
            this.ComprasTlpPlazo.ResumeLayout(false);
            this.ComprasGbxMoneda.ResumeLayout(false);
            this.ComprasTlpMoneda.ResumeLayout(false);
            this.ComprasGbxProveedor.ResumeLayout(false);
            this.ComprasTlpProveedor.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxMoneda;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxProveedor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNombreProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDireccionProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboNombreProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDireccionProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoProveedorValor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTipoCambio;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSaldoActualProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSaldoActualProveedor;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxPlazo;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaEmision;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtFechaEmision;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaVencimiento;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtFechaVencimiento;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTipoCambio;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxBodega;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBodega;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSucursal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblBodega;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboBodega;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPaisBodega;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboSucursal;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtPaisBodega;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpComentario;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaComentario;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtComentario;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnDescartar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnGuardar;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotales;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSubtotalOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSubtotalOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoPorcentajeTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSubtotalNeto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescuentoPorcentajeTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescuentoTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblImpuestoTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTotalOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSubtotalNeto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtImpuestoTotal;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxProducto;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboSkuProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSkuProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNombreProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboNombreProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescripcionProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescripcionProducto;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDetalles;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAccionesProductos;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvProductos;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPesoProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtPesoProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCantidadProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCostoUnitarioProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCostoUnitarioProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescuentoProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoPorcentajeProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCantidadProducto;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpAccionesProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnInsertarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnModificarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnEliminarProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTotalProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalProducto;
    }
}
