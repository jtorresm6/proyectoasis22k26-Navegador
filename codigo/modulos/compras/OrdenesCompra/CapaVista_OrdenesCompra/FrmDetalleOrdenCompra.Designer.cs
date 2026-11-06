namespace CapaVista_OrdenesCompra
{
    partial class FrmDetalleOrdenCompra
    {

        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDetalleOrdenCompra));
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxEncabezado = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpEncabezado = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpDatosOrden = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEstadoValor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblCodigoOrden = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblFechaDeEmision = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblFechaDeVencimiento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblPlazo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCodigoOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtPlazoValor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasDtpFechaEmision = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasDtpFechaVencimiento = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasTlpProveedor = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTxtNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTlpResponsable = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstadoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtEstadoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxFinalizacion = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpFinalizacion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpImportes = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEtiquetaComentario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtComentario = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpComentarioYTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnDescartar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnRegresar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTxtMoneda = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaMoneda = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTotalDeLaOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtSubtotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaDescuento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEtiquetaSubtotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEtiquetaTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEtiquetaTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescuentoTotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDiasPlazo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDiasPlazo = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaImpuesto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtImpuesto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxDetalles = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpDetalles = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPnlArticulos = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasDgvProductos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasTlpMantenimientoProductos = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpDetallesProducto = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasFlpPrecioCantidadDescuento = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnTogglePorcentaje = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTxtDescuento = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDescuento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCantidad = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblCantidad = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblPrecioUnitario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasFlpProductoSku = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasCboSku = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblSku = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboProducto = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpTotalesArticulo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTotalesDeProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpAccionesArticulo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasFlpAccionesOrden = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnIngresarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnModificarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnEliminarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnRefrescarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasUsrEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxEncabezado.SuspendLayout();
            this.ComprasTlpEncabezado.SuspendLayout();
            this.ComprasTlpDatosOrden.SuspendLayout();
            this.ComprasTlpProveedor.SuspendLayout();
            this.ComprasTlpResponsable.SuspendLayout();
            this.ComprasGbxFinalizacion.SuspendLayout();
            this.ComprasTlpFinalizacion.SuspendLayout();
            this.ComprasTlpImportes.SuspendLayout();
            this.ComprasTlpComentarioYTotales.SuspendLayout();
            this.ComprasTlpTotales.SuspendLayout();
            this.ComprasGbxDetalles.SuspendLayout();
            this.ComprasTlpDetalles.SuspendLayout();
            this.ComprasPnlArticulos.SuspendLayout();
            this.ComprasTlpMantenimientoProductos.SuspendLayout();
            this.ComprasTlpDetallesProducto.SuspendLayout();
            this.ComprasFlpPrecioCantidadDescuento.SuspendLayout();
            this.ComprasFlpProductoSku.SuspendLayout();
            this.ComprasTlpTotalesArticulo.SuspendLayout();
            this.ComprasTlpAccionesArticulo.SuspendLayout();
            this.ComprasFlpAccionesOrden.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasPnlContenido);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 1055);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpPrincipal);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.MinimumSize = new System.Drawing.Size(0, 1055);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1300, 1055);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxEncabezado, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxFinalizacion, 0, 3);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxDetalles, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasUsrEncabezado, 0, 0);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 4;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 254F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 392F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1300, 1055);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasGbxEncabezado
            // 
            this.ComprasGbxEncabezado.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasTlpEncabezado);
            this.ComprasGbxEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxEncabezado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxEncabezado.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxEncabezado.Location = new System.Drawing.Point(10, 160);
            this.ComprasGbxEncabezado.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxEncabezado.Name = "ComprasGbxEncabezado";
            this.ComprasGbxEncabezado.Size = new System.Drawing.Size(1280, 234);
            this.ComprasGbxEncabezado.TabIndex = 4;
            this.ComprasGbxEncabezado.TabStop = false;
            this.ComprasGbxEncabezado.Text = "Encabezado";
            // 
            // ComprasTlpEncabezado
            // 
            this.ComprasTlpEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpEncabezado.ColumnCount = 6;
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasTlpDatosOrden, 0, 0);
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasTlpProveedor, 2, 0);
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasTlpResponsable, 4, 0);
            this.ComprasTlpEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpEncabezado.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpEncabezado.Name = "ComprasTlpEncabezado";
            this.ComprasTlpEncabezado.RowCount = 1;
            this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpEncabezado.Size = new System.Drawing.Size(1274, 207);
            this.ComprasTlpEncabezado.TabIndex = 3;
            // 
            // ComprasTlpDatosOrden
            // 
            this.ComprasTlpDatosOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDatosOrden.ColumnCount = 2;
            this.ComprasTlpDatosOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.ComprasTlpDatosOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblEstadoValor, 1, 0);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblCodigoOrden, 0, 1);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblFechaDeEmision, 0, 2);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblFechaDeVencimiento, 0, 3);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblPlazo, 0, 4);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasTxtCodigoOrden, 1, 1);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasTxtPlazoValor, 1, 4);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasLblEstado, 0, 0);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasDtpFechaEmision, 1, 2);
            this.ComprasTlpDatosOrden.Controls.Add(this.ComprasDtpFechaVencimiento, 1, 3);
            this.ComprasTlpDatosOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDatosOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDatosOrden.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpDatosOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDatosOrden.Name = "ComprasTlpDatosOrden";
            this.ComprasTlpDatosOrden.RowCount = 6;
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpDatosOrden.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpDatosOrden.TabIndex = 5;
            // 
            // ComprasLblEstadoValor
            // 
            this.ComprasLblEstadoValor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEstadoValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoValor.ColorTexto = System.Drawing.Color.Red;
            this.ComprasLblEstadoValor.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblEstadoValor.ForeColor = System.Drawing.Color.Red;
            this.ComprasLblEstadoValor.Location = new System.Drawing.Point(164, 6);
            this.ComprasLblEstadoValor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComprasLblEstadoValor.MaximumSize = new System.Drawing.Size(1375, 29);
            this.ComprasLblEstadoValor.MostrarAsterisco = false;
            this.ComprasLblEstadoValor.Name = "ComprasLblEstadoValor";
            this.ComprasLblEstadoValor.Size = new System.Drawing.Size(232, 27);
            this.ComprasLblEstadoValor.TabIndex = 14;
            this.ComprasLblEstadoValor.TabStop = false;
            this.ComprasLblEstadoValor.TamanoTipografia = 12F;
            this.ComprasLblEstadoValor.Text = "PENDIENTE";
            this.ComprasLblEstadoValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoValor.Texto = "PENDIENTE";
            // 
            // ComprasLblCodigoOrden
            // 
            this.ComprasLblCodigoOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblCodigoOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCodigoOrden.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCodigoOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCodigoOrden.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblCodigoOrden.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCodigoOrden.MostrarAsterisco = false;
            this.ComprasLblCodigoOrden.Name = "ComprasLblCodigoOrden";
            this.ComprasLblCodigoOrden.Size = new System.Drawing.Size(154, 18);
            this.ComprasLblCodigoOrden.TabIndex = 11;
            this.ComprasLblCodigoOrden.TabStop = false;
            this.ComprasLblCodigoOrden.TamanoTipografia = 9F;
            this.ComprasLblCodigoOrden.Text = "Código de orden";
            this.ComprasLblCodigoOrden.Texto = "Código de orden";
            // 
            // ComprasLblFechaDeEmision
            // 
            this.ComprasLblFechaDeEmision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblFechaDeEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaDeEmision.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaDeEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaDeEmision.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblFechaDeEmision.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaDeEmision.MostrarAsterisco = false;
            this.ComprasLblFechaDeEmision.Name = "ComprasLblFechaDeEmision";
            this.ComprasLblFechaDeEmision.Size = new System.Drawing.Size(154, 18);
            this.ComprasLblFechaDeEmision.TabIndex = 2;
            this.ComprasLblFechaDeEmision.TabStop = false;
            this.ComprasLblFechaDeEmision.TamanoTipografia = 9F;
            this.ComprasLblFechaDeEmision.Text = "Fecha de Emisión";
            this.ComprasLblFechaDeEmision.Texto = "Fecha de Emisión";
            // 
            // ComprasLblFechaDeVencimiento
            // 
            this.ComprasLblFechaDeVencimiento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblFechaDeVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaDeVencimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaDeVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaDeVencimiento.Location = new System.Drawing.Point(3, 131);
            this.ComprasLblFechaDeVencimiento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaDeVencimiento.MostrarAsterisco = false;
            this.ComprasLblFechaDeVencimiento.Name = "ComprasLblFechaDeVencimiento";
            this.ComprasLblFechaDeVencimiento.Size = new System.Drawing.Size(154, 18);
            this.ComprasLblFechaDeVencimiento.TabIndex = 3;
            this.ComprasLblFechaDeVencimiento.TabStop = false;
            this.ComprasLblFechaDeVencimiento.TamanoTipografia = 9F;
            this.ComprasLblFechaDeVencimiento.Text = "Fecha de Vencimiento";
            this.ComprasLblFechaDeVencimiento.Texto = "Fecha de Vencimiento";
            // 
            // ComprasLblPlazo
            // 
            this.ComprasLblPlazo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPlazo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPlazo.Location = new System.Drawing.Point(3, 171);
            this.ComprasLblPlazo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPlazo.MostrarAsterisco = false;
            this.ComprasLblPlazo.Name = "ComprasLblPlazo";
            this.ComprasLblPlazo.Size = new System.Drawing.Size(154, 18);
            this.ComprasLblPlazo.TabIndex = 4;
            this.ComprasLblPlazo.TabStop = false;
            this.ComprasLblPlazo.TamanoTipografia = 9F;
            this.ComprasLblPlazo.Text = "Plazo";
            this.ComprasLblPlazo.Texto = "Plazo";
            // 
            // ComprasTxtCodigoOrden
            // 
            this.ComprasTxtCodigoOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCodigoOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCodigoOrden.Enabled = false;
            this.ComprasTxtCodigoOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCodigoOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCodigoOrden.Location = new System.Drawing.Point(162, 42);
            this.ComprasTxtCodigoOrden.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCodigoOrden.MaxLength = 2147483647;
            this.ComprasTxtCodigoOrden.MostrarEtiquetaError = false;
            this.ComprasTxtCodigoOrden.Name = "ComprasTxtCodigoOrden";
            this.ComprasTxtCodigoOrden.SelectedText = "";
            this.ComprasTxtCodigoOrden.SelectionLength = 0;
            this.ComprasTxtCodigoOrden.SelectionStart = 0;
            this.ComprasTxtCodigoOrden.Size = new System.Drawing.Size(236, 36);
            this.ComprasTxtCodigoOrden.TabIndex = 12;
            // 
            // ComprasTxtPlazoValor
            // 
            this.ComprasTxtPlazoValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtPlazoValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtPlazoValor.Enabled = false;
            this.ComprasTxtPlazoValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtPlazoValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtPlazoValor.Location = new System.Drawing.Point(162, 163);
            this.ComprasTxtPlazoValor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtPlazoValor.MaxLength = 2147483647;
            this.ComprasTxtPlazoValor.MostrarEtiquetaError = false;
            this.ComprasTxtPlazoValor.Name = "ComprasTxtPlazoValor";
            this.ComprasTxtPlazoValor.SelectedText = "";
            this.ComprasTxtPlazoValor.SelectionLength = 0;
            this.ComprasTxtPlazoValor.SelectionStart = 0;
            this.ComprasTxtPlazoValor.Size = new System.Drawing.Size(236, 33);
            this.ComprasTxtPlazoValor.TabIndex = 10;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(154, 18);
            this.ComprasLblEstado.TabIndex = 13;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 9F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.Texto = "Estado";
            // 
            // ComprasDtpFechaEmision
            // 
            this.ComprasDtpFechaEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarFont = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasDtpFechaEmision.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(86)))), ((int)(((byte)(166)))));
            this.ComprasDtpFechaEmision.CustomFormat = "dd/MM/yyyy";
            this.ComprasDtpFechaEmision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ComprasDtpFechaEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaEmision.Location = new System.Drawing.Point(164, 86);
            this.ComprasDtpFechaEmision.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasDtpFechaEmision.Name = "ComprasDtpFechaEmision";
            this.ComprasDtpFechaEmision.Size = new System.Drawing.Size(232, 34);
            this.ComprasDtpFechaEmision.TabIndex = 15;
            // 
            // ComprasDtpFechaVencimiento
            // 
            this.ComprasDtpFechaVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarFont = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasDtpFechaVencimiento.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(86)))), ((int)(((byte)(166)))));
            this.ComprasDtpFechaVencimiento.CustomFormat = "dd/MM/yyyy";
            this.ComprasDtpFechaVencimiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDtpFechaVencimiento.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ComprasDtpFechaVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaVencimiento.Location = new System.Drawing.Point(164, 126);
            this.ComprasDtpFechaVencimiento.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasDtpFechaVencimiento.Name = "ComprasDtpFechaVencimiento";
            this.ComprasDtpFechaVencimiento.Size = new System.Drawing.Size(232, 34);
            this.ComprasDtpFechaVencimiento.TabIndex = 16;
            // 
            // ComprasTlpProveedor
            // 
            this.ComprasTlpProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpProveedor.ColumnCount = 2;
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtNitProveedor, 1, 4);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtCorreoProveedor, 1, 3);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasTxtTelefonoProveedor, 1, 2);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblProveedor, 1, 0);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblNombreProveedor, 0, 1);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblTelefonoProveedor, 0, 2);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblCorreoProveedor, 0, 3);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblNitProveedor, 0, 4);
            this.ComprasTlpProveedor.Controls.Add(this.ComprasCboNombreProveedor, 1, 1);
            this.ComprasTlpProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpProveedor.Location = new System.Drawing.Point(420, 0);
            this.ComprasTlpProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpProveedor.Name = "ComprasTlpProveedor";
            this.ComprasTlpProveedor.RowCount = 6;
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedor.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpProveedor.TabIndex = 4;
            // 
            // ComprasTxtNitProveedor
            // 
            this.ComprasTxtNitProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtNitProveedor.Enabled = false;
            this.ComprasTxtNitProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtNitProveedor.Location = new System.Drawing.Point(92, 162);
            this.ComprasTxtNitProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtNitProveedor.MaxLength = 2147483647;
            this.ComprasTxtNitProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtNitProveedor.Name = "ComprasTxtNitProveedor";
            this.ComprasTxtNitProveedor.SelectedText = "";
            this.ComprasTxtNitProveedor.SelectionLength = 0;
            this.ComprasTxtNitProveedor.SelectionStart = 0;
            this.ComprasTxtNitProveedor.Size = new System.Drawing.Size(306, 36);
            this.ComprasTxtNitProveedor.TabIndex = 10;
            // 
            // ComprasTxtCorreoProveedor
            // 
            this.ComprasTxtCorreoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCorreoProveedor.Enabled = false;
            this.ComprasTxtCorreoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCorreoProveedor.Location = new System.Drawing.Point(92, 122);
            this.ComprasTxtCorreoProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCorreoProveedor.MaxLength = 2147483647;
            this.ComprasTxtCorreoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtCorreoProveedor.Name = "ComprasTxtCorreoProveedor";
            this.ComprasTxtCorreoProveedor.SelectedText = "";
            this.ComprasTxtCorreoProveedor.SelectionLength = 0;
            this.ComprasTxtCorreoProveedor.SelectionStart = 0;
            this.ComprasTxtCorreoProveedor.Size = new System.Drawing.Size(306, 36);
            this.ComprasTxtCorreoProveedor.TabIndex = 9;
            // 
            // ComprasTxtTelefonoProveedor
            // 
            this.ComprasTxtTelefonoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTelefonoProveedor.Enabled = false;
            this.ComprasTxtTelefonoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTelefonoProveedor.Location = new System.Drawing.Point(92, 82);
            this.ComprasTxtTelefonoProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTelefonoProveedor.MaxLength = 2147483647;
            this.ComprasTxtTelefonoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtTelefonoProveedor.Name = "ComprasTxtTelefonoProveedor";
            this.ComprasTxtTelefonoProveedor.SelectedText = "";
            this.ComprasTxtTelefonoProveedor.SelectionLength = 0;
            this.ComprasTxtTelefonoProveedor.SelectionStart = 0;
            this.ComprasTxtTelefonoProveedor.Size = new System.Drawing.Size(306, 36);
            this.ComprasTxtTelefonoProveedor.TabIndex = 8;
            // 
            // ComprasLblProveedor
            // 
            this.ComprasLblProveedor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedor.Font = new System.Drawing.Font("Tahoma", 10.8F);
            this.ComprasLblProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProveedor.Location = new System.Drawing.Point(94, 9);
            this.ComprasLblProveedor.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasLblProveedor.MaximumSize = new System.Drawing.Size(1250, 27);
            this.ComprasLblProveedor.MostrarAsterisco = false;
            this.ComprasLblProveedor.Name = "ComprasLblProveedor";
            this.ComprasLblProveedor.Size = new System.Drawing.Size(302, 22);
            this.ComprasLblProveedor.TabIndex = 7;
            this.ComprasLblProveedor.TabStop = false;
            this.ComprasLblProveedor.TamanoTipografia = 10.8F;
            this.ComprasLblProveedor.Text = "PROVEEDOR";
            this.ComprasLblProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.Texto = "PROVEEDOR";
            // 
            // ComprasLblNombreProveedor
            // 
            this.ComprasLblNombreProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNombreProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNombreProveedor.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblNombreProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNombreProveedor.Name = "ComprasLblNombreProveedor";
            this.ComprasLblNombreProveedor.Size = new System.Drawing.Size(84, 18);
            this.ComprasLblNombreProveedor.TabIndex = 1;
            this.ComprasLblNombreProveedor.TabStop = false;
            this.ComprasLblNombreProveedor.TamanoTipografia = 9F;
            this.ComprasLblNombreProveedor.Text = "Nombre";
            this.ComprasLblNombreProveedor.Texto = "Nombre";
            // 
            // ComprasLblTelefonoProveedor
            // 
            this.ComprasLblTelefonoProveedor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTelefonoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTelefonoProveedor.Location = new System.Drawing.Point(13, 91);
            this.ComprasLblTelefonoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTelefonoProveedor.MostrarAsterisco = false;
            this.ComprasLblTelefonoProveedor.Name = "ComprasLblTelefonoProveedor";
            this.ComprasLblTelefonoProveedor.Size = new System.Drawing.Size(74, 18);
            this.ComprasLblTelefonoProveedor.TabIndex = 2;
            this.ComprasLblTelefonoProveedor.TabStop = false;
            this.ComprasLblTelefonoProveedor.TamanoTipografia = 9F;
            this.ComprasLblTelefonoProveedor.Text = "Teléfono";
            this.ComprasLblTelefonoProveedor.Texto = "Teléfono";
            // 
            // ComprasLblCorreoProveedor
            // 
            this.ComprasLblCorreoProveedor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCorreoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCorreoProveedor.Location = new System.Drawing.Point(13, 131);
            this.ComprasLblCorreoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCorreoProveedor.MostrarAsterisco = false;
            this.ComprasLblCorreoProveedor.Name = "ComprasLblCorreoProveedor";
            this.ComprasLblCorreoProveedor.Size = new System.Drawing.Size(74, 18);
            this.ComprasLblCorreoProveedor.TabIndex = 3;
            this.ComprasLblCorreoProveedor.TabStop = false;
            this.ComprasLblCorreoProveedor.TamanoTipografia = 9F;
            this.ComprasLblCorreoProveedor.Text = "Correo";
            this.ComprasLblCorreoProveedor.Texto = "Correo";
            // 
            // ComprasLblNitProveedor
            // 
            this.ComprasLblNitProveedor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNitProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNitProveedor.Location = new System.Drawing.Point(13, 171);
            this.ComprasLblNitProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNitProveedor.MostrarAsterisco = false;
            this.ComprasLblNitProveedor.Name = "ComprasLblNitProveedor";
            this.ComprasLblNitProveedor.Size = new System.Drawing.Size(74, 18);
            this.ComprasLblNitProveedor.TabIndex = 4;
            this.ComprasLblNitProveedor.TabStop = false;
            this.ComprasLblNitProveedor.TamanoTipografia = 9F;
            this.ComprasLblNitProveedor.Text = "NIT";
            this.ComprasLblNitProveedor.Texto = "NIT";
            // 
            // ComprasCboNombreProveedor
            // 
            this.ComprasCboNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboNombreProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboNombreProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboNombreProveedor.Location = new System.Drawing.Point(90, 40);
            this.ComprasCboNombreProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboNombreProveedor.MaxLength = 0;
            this.ComprasCboNombreProveedor.MostrarEtiquetaError = false;
            this.ComprasCboNombreProveedor.Name = "ComprasCboNombreProveedor";
            this.ComprasCboNombreProveedor.Size = new System.Drawing.Size(310, 40);
            this.ComprasCboNombreProveedor.TabIndex = 1;
            this.ComprasCboNombreProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasTlpResponsable
            // 
            this.ComprasTlpResponsable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpResponsable.ColumnCount = 2;
            this.ComprasTlpResponsable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpResponsable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResponsable.Controls.Add(this.ComprasLblDireccionProveedor, 0, 1);
            this.ComprasTlpResponsable.Controls.Add(this.ComprasTxtDireccionProveedor, 1, 1);
            this.ComprasTlpResponsable.Controls.Add(this.ComprasLblEstadoProveedor, 0, 2);
            this.ComprasTlpResponsable.Controls.Add(this.ComprasTxtEstadoProveedor, 1, 2);
            this.ComprasTlpResponsable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpResponsable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpResponsable.Location = new System.Drawing.Point(840, 0);
            this.ComprasTlpResponsable.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpResponsable.Name = "ComprasTlpResponsable";
            this.ComprasTlpResponsable.RowCount = 6;
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResponsable.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpResponsable.TabIndex = 3;
            // 
            // ComprasLblDireccionProveedor
            // 
            this.ComprasLblDireccionProveedor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDireccionProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDireccionProveedor.Location = new System.Drawing.Point(13, 51);
            this.ComprasLblDireccionProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDireccionProveedor.MostrarAsterisco = false;
            this.ComprasLblDireccionProveedor.Name = "ComprasLblDireccionProveedor";
            this.ComprasLblDireccionProveedor.Size = new System.Drawing.Size(74, 18);
            this.ComprasLblDireccionProveedor.TabIndex = 4;
            this.ComprasLblDireccionProveedor.TabStop = false;
            this.ComprasLblDireccionProveedor.TamanoTipografia = 9F;
            this.ComprasLblDireccionProveedor.Text = "Dirección";
            this.ComprasLblDireccionProveedor.Texto = "Dirección";
            // 
            // ComprasTxtDireccionProveedor
            // 
            this.ComprasTxtDireccionProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDireccionProveedor.Enabled = false;
            this.ComprasTxtDireccionProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDireccionProveedor.Location = new System.Drawing.Point(92, 42);
            this.ComprasTxtDireccionProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDireccionProveedor.MaxLength = 2147483647;
            this.ComprasTxtDireccionProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtDireccionProveedor.Name = "ComprasTxtDireccionProveedor";
            this.ComprasTxtDireccionProveedor.SelectedText = "";
            this.ComprasTxtDireccionProveedor.SelectionLength = 0;
            this.ComprasTxtDireccionProveedor.SelectionStart = 0;
            this.ComprasTxtDireccionProveedor.Size = new System.Drawing.Size(306, 36);
            this.ComprasTxtDireccionProveedor.TabIndex = 9;
            // 
            // ComprasLblEstadoProveedor
            // 
            this.ComprasLblEstadoProveedor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblEstadoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstadoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstadoProveedor.Location = new System.Drawing.Point(13, 91);
            this.ComprasLblEstadoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstadoProveedor.MostrarAsterisco = false;
            this.ComprasLblEstadoProveedor.Name = "ComprasLblEstadoProveedor";
            this.ComprasLblEstadoProveedor.Size = new System.Drawing.Size(74, 18);
            this.ComprasLblEstadoProveedor.TabIndex = 3;
            this.ComprasLblEstadoProveedor.TabStop = false;
            this.ComprasLblEstadoProveedor.TamanoTipografia = 9F;
            this.ComprasLblEstadoProveedor.Text = "Estado";
            this.ComprasLblEstadoProveedor.Texto = "Estado";
            // 
            // ComprasTxtEstadoProveedor
            // 
            this.ComprasTxtEstadoProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtEstadoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtEstadoProveedor.Enabled = false;
            this.ComprasTxtEstadoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtEstadoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtEstadoProveedor.Location = new System.Drawing.Point(92, 82);
            this.ComprasTxtEstadoProveedor.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtEstadoProveedor.MaxLength = 2147483647;
            this.ComprasTxtEstadoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtEstadoProveedor.Name = "ComprasTxtEstadoProveedor";
            this.ComprasTxtEstadoProveedor.SelectedText = "";
            this.ComprasTxtEstadoProveedor.SelectionLength = 0;
            this.ComprasTxtEstadoProveedor.SelectionStart = 0;
            this.ComprasTxtEstadoProveedor.Size = new System.Drawing.Size(306, 36);
            this.ComprasTxtEstadoProveedor.TabIndex = 10;
            // 
            // ComprasGbxFinalizacion
            // 
            this.ComprasGbxFinalizacion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxFinalizacion.Controls.Add(this.ComprasTlpFinalizacion);
            this.ComprasGbxFinalizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxFinalizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxFinalizacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxFinalizacion.Location = new System.Drawing.Point(10, 806);
            this.ComprasGbxFinalizacion.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxFinalizacion.Name = "ComprasGbxFinalizacion";
            this.ComprasGbxFinalizacion.Size = new System.Drawing.Size(1280, 239);
            this.ComprasGbxFinalizacion.TabIndex = 5;
            this.ComprasGbxFinalizacion.TabStop = false;
            this.ComprasGbxFinalizacion.Text = "Finalización";
            // 
            // ComprasTlpFinalizacion
            // 
            this.ComprasTlpFinalizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFinalizacion.ColumnCount = 2;
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpImportes, 1, 0);
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpTotales, 0, 0);
            this.ComprasTlpFinalizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFinalizacion.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpFinalizacion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFinalizacion.Name = "ComprasTlpFinalizacion";
            this.ComprasTlpFinalizacion.RowCount = 1;
            this.ComprasTlpFinalizacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFinalizacion.Size = new System.Drawing.Size(1274, 212);
            this.ComprasTlpFinalizacion.TabIndex = 3;
            // 
            // ComprasTlpImportes
            // 
            this.ComprasTlpImportes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpImportes.ColumnCount = 1;
            this.ComprasTlpImportes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpImportes.Controls.Add(this.ComprasLblEtiquetaComentario, 0, 0);
            this.ComprasTlpImportes.Controls.Add(this.ComprasTxtComentario, 0, 1);
            this.ComprasTlpImportes.Controls.Add(this.ComprasTlpComentarioYTotales, 0, 2);
            this.ComprasTlpImportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpImportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpImportes.Location = new System.Drawing.Point(645, 0);
            this.ComprasTlpImportes.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ComprasTlpImportes.Name = "ComprasTlpImportes";
            this.ComprasTlpImportes.RowCount = 3;
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpImportes.Size = new System.Drawing.Size(621, 212);
            this.ComprasTlpImportes.TabIndex = 6;
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
            this.ComprasTxtComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtComentario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtComentario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtComentario.Location = new System.Drawing.Point(0, 24);
            this.ComprasTxtComentario.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.ComprasTxtComentario.MaxLength = 2147483647;
            this.ComprasTxtComentario.MostrarEtiquetaError = false;
            this.ComprasTxtComentario.Multiline = true;
            this.ComprasTxtComentario.Name = "ComprasTxtComentario";
            this.ComprasTxtComentario.SelectedText = "";
            this.ComprasTxtComentario.SelectionLength = 0;
            this.ComprasTxtComentario.SelectionStart = 0;
            this.ComprasTxtComentario.Size = new System.Drawing.Size(621, 90);
            this.ComprasTxtComentario.TabIndex = 9;
            // 
            // ComprasTlpComentarioYTotales
            // 
            this.ComprasTlpComentarioYTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpComentarioYTotales.ColumnCount = 6;
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnDescartar, 3, 0);
            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnGuardar, 5, 0);
            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnRegresar, 1, 0);
            this.ComprasTlpComentarioYTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpComentarioYTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpComentarioYTotales.Location = new System.Drawing.Point(0, 122);
            this.ComprasTlpComentarioYTotales.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpComentarioYTotales.Name = "ComprasTlpComentarioYTotales";
            this.ComprasTlpComentarioYTotales.RowCount = 1;
            this.ComprasTlpComentarioYTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpComentarioYTotales.Size = new System.Drawing.Size(621, 90);
            this.ComprasTlpComentarioYTotales.TabIndex = 15;
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
            this.ComprasBtnDescartar.Location = new System.Drawing.Point(451, 10);
            this.ComprasBtnDescartar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnDescartar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.Name = "ComprasBtnDescartar";
            this.ComprasBtnDescartar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.TabIndex = 15;
            this.ComprasBtnDescartar.UseVisualStyleBackColor = false;
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
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(541, 10);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnGuardar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.TabIndex = 10;
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnRegresar
            // 
            this.ComprasBtnRegresar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComprasBtnRegresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnRegresar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnRegresar.BackgroundImage")));
            this.ComprasBtnRegresar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnRegresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnRegresar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRegresar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnRegresar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnRegresar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnRegresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnRegresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnRegresar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRegresar.Location = new System.Drawing.Point(361, 10);
            this.ComprasBtnRegresar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnRegresar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.Name = "ComprasBtnRegresar";
            this.ComprasBtnRegresar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.TabIndex = 16;
            this.ComprasBtnRegresar.UseVisualStyleBackColor = false;
            // 
            // ComprasTlpTotales
            // 
            this.ComprasTlpTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.ColumnCount = 4;
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtMoneda, 1, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaMoneda, 0, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTotalDeLaOrden, 1, 4);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTipoCambio, 1, 3);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtSubtotal, 1, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaDescuento, 0, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaSubtotal, 0, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaTipoCambio, 0, 3);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaTotal, 0, 4);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtDescuentoTotal, 1, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblDiasPlazo, 2, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtDiasPlazo, 3, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaImpuesto, 2, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtImpuesto, 3, 1);
            this.ComprasTlpTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotales.Location = new System.Drawing.Point(8, 0);
            this.ComprasTlpTotales.Margin = new System.Windows.Forms.Padding(8, 0, 16, 0);
            this.ComprasTlpTotales.Name = "ComprasTlpTotales";
            this.ComprasTlpTotales.RowCount = 6;
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotales.Size = new System.Drawing.Size(613, 212);
            this.ComprasTlpTotales.TabIndex = 5;
            // 
            // ComprasTxtMoneda
            // 
            this.ComprasTxtMoneda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.SetColumnSpan(this.ComprasTxtMoneda, 3);
            this.ComprasTxtMoneda.Enabled = false;
            this.ComprasTxtMoneda.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtMoneda.Location = new System.Drawing.Point(132, 2);
            this.ComprasTxtMoneda.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtMoneda.MaxLength = 2147483647;
            this.ComprasTxtMoneda.MostrarEtiquetaError = false;
            this.ComprasTxtMoneda.Name = "ComprasTxtMoneda";
            this.ComprasTxtMoneda.SelectedText = "";
            this.ComprasTxtMoneda.SelectionLength = 0;
            this.ComprasTxtMoneda.SelectionStart = 0;
            this.ComprasTxtMoneda.Size = new System.Drawing.Size(479, 36);
            this.ComprasTxtMoneda.TabIndex = 12;
            // 
            // ComprasLblEtiquetaMoneda
            // 
            this.ComprasLblEtiquetaMoneda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaMoneda.Location = new System.Drawing.Point(3, 11);
            this.ComprasLblEtiquetaMoneda.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaMoneda.MostrarAsterisco = false;
            this.ComprasLblEtiquetaMoneda.Name = "ComprasLblEtiquetaMoneda";
            this.ComprasLblEtiquetaMoneda.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblEtiquetaMoneda.TabIndex = 11;
            this.ComprasLblEtiquetaMoneda.TabStop = false;
            this.ComprasLblEtiquetaMoneda.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaMoneda.Text = "Moneda (ID)";
            this.ComprasLblEtiquetaMoneda.Texto = "Moneda (ID)";
            // 
            // ComprasTxtTotalDeLaOrden
            // 
            this.ComprasTxtTotalDeLaOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTotalDeLaOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.SetColumnSpan(this.ComprasTxtTotalDeLaOrden, 3);
            this.ComprasTxtTotalDeLaOrden.Enabled = false;
            this.ComprasTxtTotalDeLaOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalDeLaOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalDeLaOrden.Location = new System.Drawing.Point(132, 162);
            this.ComprasTxtTotalDeLaOrden.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTotalDeLaOrden.MaxLength = 2147483647;
            this.ComprasTxtTotalDeLaOrden.MostrarEtiquetaError = false;
            this.ComprasTxtTotalDeLaOrden.Name = "ComprasTxtTotalDeLaOrden";
            this.ComprasTxtTotalDeLaOrden.SelectedText = "";
            this.ComprasTxtTotalDeLaOrden.SelectionLength = 0;
            this.ComprasTxtTotalDeLaOrden.SelectionStart = 0;
            this.ComprasTxtTotalDeLaOrden.Size = new System.Drawing.Size(479, 36);
            this.ComprasTxtTotalDeLaOrden.TabIndex = 10;
            // 
            // ComprasTxtTipoCambio
            // 
            this.ComprasTxtTipoCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.SetColumnSpan(this.ComprasTxtTipoCambio, 3);
            this.ComprasTxtTipoCambio.Enabled = false;
            this.ComprasTxtTipoCambio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTipoCambio.Location = new System.Drawing.Point(132, 122);
            this.ComprasTxtTipoCambio.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTipoCambio.MaxLength = 2147483647;
            this.ComprasTxtTipoCambio.MostrarEtiquetaError = false;
            this.ComprasTxtTipoCambio.Name = "ComprasTxtTipoCambio";
            this.ComprasTxtTipoCambio.SelectedText = "";
            this.ComprasTxtTipoCambio.SelectionLength = 0;
            this.ComprasTxtTipoCambio.SelectionStart = 0;
            this.ComprasTxtTipoCambio.Size = new System.Drawing.Size(479, 36);
            this.ComprasTxtTipoCambio.TabIndex = 9;
            // 
            // ComprasTxtSubtotal
            // 
            this.ComprasTxtSubtotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtSubtotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSubtotal.Enabled = false;
            this.ComprasTxtSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSubtotal.Location = new System.Drawing.Point(132, 42);
            this.ComprasTxtSubtotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtSubtotal.MaxLength = 2147483647;
            this.ComprasTxtSubtotal.MostrarEtiquetaError = false;
            this.ComprasTxtSubtotal.Name = "ComprasTxtSubtotal";
            this.ComprasTxtSubtotal.SelectedText = "";
            this.ComprasTxtSubtotal.SelectionLength = 0;
            this.ComprasTxtSubtotal.SelectionStart = 0;
            this.ComprasTxtSubtotal.Size = new System.Drawing.Size(182, 36);
            this.ComprasTxtSubtotal.TabIndex = 8;
            // 
            // ComprasLblEtiquetaDescuento
            // 
            this.ComprasLblEtiquetaDescuento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaDescuento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaDescuento.Location = new System.Drawing.Point(3, 91);
            this.ComprasLblEtiquetaDescuento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaDescuento.MostrarAsterisco = false;
            this.ComprasLblEtiquetaDescuento.Name = "ComprasLblEtiquetaDescuento";
            this.ComprasLblEtiquetaDescuento.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblEtiquetaDescuento.TabIndex = 1;
            this.ComprasLblEtiquetaDescuento.TabStop = false;
            this.ComprasLblEtiquetaDescuento.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaDescuento.Text = "Descuento Total";
            this.ComprasLblEtiquetaDescuento.Texto = "Descuento Total";
            // 
            // ComprasLblEtiquetaSubtotal
            // 
            this.ComprasLblEtiquetaSubtotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaSubtotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaSubtotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaSubtotal.Location = new System.Drawing.Point(3, 51);
            this.ComprasLblEtiquetaSubtotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaSubtotal.MostrarAsterisco = false;
            this.ComprasLblEtiquetaSubtotal.Name = "ComprasLblEtiquetaSubtotal";
            this.ComprasLblEtiquetaSubtotal.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblEtiquetaSubtotal.TabIndex = 2;
            this.ComprasLblEtiquetaSubtotal.TabStop = false;
            this.ComprasLblEtiquetaSubtotal.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaSubtotal.Text = "Subtotal";
            this.ComprasLblEtiquetaSubtotal.Texto = "Subtotal";
            // 
            // ComprasLblEtiquetaTipoCambio
            // 
            this.ComprasLblEtiquetaTipoCambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTipoCambio.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTipoCambio.Location = new System.Drawing.Point(3, 131);
            this.ComprasLblEtiquetaTipoCambio.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTipoCambio.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTipoCambio.Name = "ComprasLblEtiquetaTipoCambio";
            this.ComprasLblEtiquetaTipoCambio.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblEtiquetaTipoCambio.TabIndex = 3;
            this.ComprasLblEtiquetaTipoCambio.TabStop = false;
            this.ComprasLblEtiquetaTipoCambio.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTipoCambio.Text = "Tipo de cambio";
            this.ComprasLblEtiquetaTipoCambio.Texto = "Tipo de cambio";
            // 
            // ComprasLblEtiquetaTotal
            // 
            this.ComprasLblEtiquetaTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTotal.Location = new System.Drawing.Point(3, 171);
            this.ComprasLblEtiquetaTotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTotal.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTotal.Name = "ComprasLblEtiquetaTotal";
            this.ComprasLblEtiquetaTotal.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblEtiquetaTotal.TabIndex = 4;
            this.ComprasLblEtiquetaTotal.TabStop = false;
            this.ComprasLblEtiquetaTotal.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTotal.Text = "Total de la orden";
            this.ComprasLblEtiquetaTotal.Texto = "Total de la orden";
            // 
            // ComprasTxtDescuentoTotal
            // 
            this.ComprasTxtDescuentoTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuentoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuentoTotal.Enabled = false;
            this.ComprasTxtDescuentoTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuentoTotal.Location = new System.Drawing.Point(132, 82);
            this.ComprasTxtDescuentoTotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoTotal.MaxLength = 2147483647;
            this.ComprasTxtDescuentoTotal.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoTotal.Name = "ComprasTxtDescuentoTotal";
            this.ComprasTxtDescuentoTotal.SelectedText = "";
            this.ComprasTxtDescuentoTotal.SelectionLength = 0;
            this.ComprasTxtDescuentoTotal.SelectionStart = 0;
            this.ComprasTxtDescuentoTotal.Size = new System.Drawing.Size(182, 36);
            this.ComprasTxtDescuentoTotal.TabIndex = 5;
            // 
            // ComprasLblDiasPlazo
            // 
            this.ComprasLblDiasPlazo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblDiasPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDiasPlazo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDiasPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDiasPlazo.Location = new System.Drawing.Point(319, 91);
            this.ComprasLblDiasPlazo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDiasPlazo.MostrarAsterisco = false;
            this.ComprasLblDiasPlazo.Name = "ComprasLblDiasPlazo";
            this.ComprasLblDiasPlazo.Size = new System.Drawing.Size(104, 18);
            this.ComprasLblDiasPlazo.TabIndex = 13;
            this.ComprasLblDiasPlazo.TabStop = false;
            this.ComprasLblDiasPlazo.TamanoTipografia = 9F;
            this.ComprasLblDiasPlazo.Text = "Días de plazo";
            this.ComprasLblDiasPlazo.Texto = "Días de plazo";
            // 
            // ComprasTxtDiasPlazo
            // 
            this.ComprasTxtDiasPlazo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDiasPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDiasPlazo.Enabled = false;
            this.ComprasTxtDiasPlazo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDiasPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDiasPlazo.Location = new System.Drawing.Point(428, 82);
            this.ComprasTxtDiasPlazo.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDiasPlazo.MaxLength = 2147483647;
            this.ComprasTxtDiasPlazo.MostrarEtiquetaError = false;
            this.ComprasTxtDiasPlazo.Name = "ComprasTxtDiasPlazo";
            this.ComprasTxtDiasPlazo.SelectedText = "";
            this.ComprasTxtDiasPlazo.SelectionLength = 0;
            this.ComprasTxtDiasPlazo.SelectionStart = 0;
            this.ComprasTxtDiasPlazo.Size = new System.Drawing.Size(183, 36);
            this.ComprasTxtDiasPlazo.TabIndex = 14;
            // 
            // ComprasLblEtiquetaImpuesto
            // 
            this.ComprasLblEtiquetaImpuesto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEtiquetaImpuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaImpuesto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaImpuesto.Location = new System.Drawing.Point(319, 51);
            this.ComprasLblEtiquetaImpuesto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaImpuesto.MostrarAsterisco = false;
            this.ComprasLblEtiquetaImpuesto.Name = "ComprasLblEtiquetaImpuesto";
            this.ComprasLblEtiquetaImpuesto.Size = new System.Drawing.Size(104, 18);
            this.ComprasLblEtiquetaImpuesto.TabIndex = 1;
            this.ComprasLblEtiquetaImpuesto.TabStop = false;
            this.ComprasLblEtiquetaImpuesto.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaImpuesto.Text = "Impuesto";
            this.ComprasLblEtiquetaImpuesto.Texto = "Impuesto";
            // 
            // ComprasTxtImpuesto
            // 
            this.ComprasTxtImpuesto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtImpuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtImpuesto.Enabled = false;
            this.ComprasTxtImpuesto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtImpuesto.Location = new System.Drawing.Point(428, 42);
            this.ComprasTxtImpuesto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtImpuesto.MaxLength = 2147483647;
            this.ComprasTxtImpuesto.MostrarEtiquetaError = false;
            this.ComprasTxtImpuesto.Name = "ComprasTxtImpuesto";
            this.ComprasTxtImpuesto.SelectedText = "";
            this.ComprasTxtImpuesto.SelectionLength = 0;
            this.ComprasTxtImpuesto.SelectionStart = 0;
            this.ComprasTxtImpuesto.Size = new System.Drawing.Size(183, 36);
            this.ComprasTxtImpuesto.TabIndex = 5;
            // 
            // ComprasGbxDetalles
            // 
            this.ComprasGbxDetalles.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxDetalles.Controls.Add(this.ComprasTlpDetalles);
            this.ComprasGbxDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxDetalles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxDetalles.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxDetalles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxDetalles.Location = new System.Drawing.Point(10, 414);
            this.ComprasGbxDetalles.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxDetalles.Name = "ComprasGbxDetalles";
            this.ComprasGbxDetalles.Size = new System.Drawing.Size(1280, 372);
            this.ComprasGbxDetalles.TabIndex = 6;
            this.ComprasGbxDetalles.TabStop = false;
            this.ComprasGbxDetalles.Text = "Detalles";
            // 
            // ComprasTlpDetalles
            // 
            this.ComprasTlpDetalles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDetalles.ColumnCount = 1;
            this.ComprasTlpDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Controls.Add(this.ComprasPnlArticulos, 0, 1);
            this.ComprasTlpDetalles.Controls.Add(this.ComprasTlpMantenimientoProductos, 0, 0);
            this.ComprasTlpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDetalles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDetalles.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpDetalles.Name = "ComprasTlpDetalles";
            this.ComprasTlpDetalles.RowCount = 2;
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Size = new System.Drawing.Size(1274, 345);
            this.ComprasTlpDetalles.TabIndex = 0;
            // 
            // ComprasPnlArticulos
            // 
            this.ComprasPnlArticulos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlArticulos.Controls.Add(this.ComprasDgvProductos);
            this.ComprasPnlArticulos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlArticulos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlArticulos.Location = new System.Drawing.Point(0, 140);
            this.ComprasPnlArticulos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlArticulos.Name = "ComprasPnlArticulos";
            this.ComprasPnlArticulos.Size = new System.Drawing.Size(1274, 205);
            this.ComprasPnlArticulos.TabIndex = 0;
            // 
            // ComprasDgvProductos
            // 
            this.ComprasDgvProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasDgvProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvProductos.MinimumSize = new System.Drawing.Size(1274, 205);
            this.ComprasDgvProductos.Name = "ComprasDgvProductos";
            this.ComprasDgvProductos.PaginacionHabilitada = false;
            this.ComprasDgvProductos.Size = new System.Drawing.Size(1274, 205);
            this.ComprasDgvProductos.TabIndex = 0;
            // 
            // ComprasTlpMantenimientoProductos
            // 
            this.ComprasTlpMantenimientoProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpMantenimientoProductos.ColumnCount = 2;
            this.ComprasTlpMantenimientoProductos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.ComprasTlpMantenimientoProductos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.ComprasTlpMantenimientoProductos.Controls.Add(this.ComprasTlpDetallesProducto, 0, 0);
            this.ComprasTlpMantenimientoProductos.Controls.Add(this.ComprasTlpAccionesArticulo, 1, 0);
            this.ComprasTlpMantenimientoProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMantenimientoProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpMantenimientoProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpMantenimientoProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpMantenimientoProductos.Name = "ComprasTlpMantenimientoProductos";
            this.ComprasTlpMantenimientoProductos.RowCount = 1;
            this.ComprasTlpMantenimientoProductos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMantenimientoProductos.Size = new System.Drawing.Size(1274, 140);
            this.ComprasTlpMantenimientoProductos.TabIndex = 1;
            // 
            // ComprasTlpDetallesProducto
            // 
            this.ComprasTlpDetallesProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasTlpDetallesProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDetallesProducto.ColumnCount = 1;
            this.ComprasTlpDetallesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasFlpPrecioCantidadDescuento, 0, 2);
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasFlpProductoSku, 0, 1);
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasTlpTotalesArticulo, 0, 3);
            this.ComprasTlpDetallesProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDetallesProducto.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpDetallesProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDetallesProducto.Name = "ComprasTlpDetallesProducto";
            this.ComprasTlpDetallesProducto.RowCount = 5;
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpDetallesProducto.Size = new System.Drawing.Size(891, 140);
            this.ComprasTlpDetallesProducto.TabIndex = 7;
            // 
            // ComprasFlpPrecioCantidadDescuento
            // 
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasBtnTogglePorcentaje);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasTxtDescuento);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblDescuento);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasTxtCantidad);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblCantidad);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblPrecioUnitario);
            this.ComprasFlpPrecioCantidadDescuento.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasFlpPrecioCantidadDescuento.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ComprasFlpPrecioCantidadDescuento.Location = new System.Drawing.Point(96, 50);
            this.ComprasFlpPrecioCantidadDescuento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpPrecioCantidadDescuento.Name = "ComprasFlpPrecioCantidadDescuento";
            this.ComprasFlpPrecioCantidadDescuento.Size = new System.Drawing.Size(795, 40);
            this.ComprasFlpPrecioCantidadDescuento.TabIndex = 7;
            this.ComprasBtnTogglePorcentaje.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnTogglePorcentaje
            // 
            this.ComprasBtnTogglePorcentaje.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnTogglePorcentaje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnTogglePorcentaje.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnTogglePorcentaje.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnTogglePorcentaje.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnTogglePorcentaje.FlatAppearance.BorderSize = 0;
            this.ComprasBtnTogglePorcentaje.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnTogglePorcentaje.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnTogglePorcentaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnTogglePorcentaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnTogglePorcentaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnTogglePorcentaje.Location = new System.Drawing.Point(761, 3);
            this.ComprasBtnTogglePorcentaje.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnTogglePorcentaje.Name = "ComprasBtnTogglePorcentaje";
            this.ComprasBtnTogglePorcentaje.Size = new System.Drawing.Size(34, 34);
            this.ComprasBtnTogglePorcentaje.TabIndex = 7;
            this.ComprasBtnTogglePorcentaje.Text = "%";
            this.ComprasBtnTogglePorcentaje.UseVisualStyleBackColor = false;
            // 
            // ComprasTxtDescuento
            // 
            this.ComprasTxtDescuento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuento.Location = new System.Drawing.Point(603, 2);
            this.ComprasTxtDescuento.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuento.MaxLength = 2147483647;
            this.ComprasTxtDescuento.ModoMoneda = true;
            this.ComprasTxtDescuento.MostrarEtiquetaError = false;
            this.ComprasTxtDescuento.Name = "ComprasTxtDescuento";
            this.ComprasTxtDescuento.SelectedText = "";
            this.ComprasTxtDescuento.SelectionLength = 0;
            this.ComprasTxtDescuento.SelectionStart = 0;
            this.ComprasTxtDescuento.Size = new System.Drawing.Size(156, 36);
            this.ComprasTxtDescuento.TabIndex = 6;
            this.ComprasTxtDescuento.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblDescuento
            // 
            this.ComprasLblDescuento.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescuento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescuento.Location = new System.Drawing.Point(511, 11);
            this.ComprasLblDescuento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDescuento.MostrarAsterisco = false;
            this.ComprasLblDescuento.Name = "ComprasLblDescuento";
            this.ComprasLblDescuento.Size = new System.Drawing.Size(87, 18);
            this.ComprasLblDescuento.TabIndex = 21;
            this.ComprasLblDescuento.TabStop = false;
            this.ComprasLblDescuento.TamanoTipografia = 9F;
            this.ComprasLblDescuento.Text = "Descuento";
            this.ComprasLblDescuento.Texto = "Descuento";
            // 
            // ComprasTxtCantidad
            // 
            this.ComprasTxtCantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCantidad.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCantidad.Location = new System.Drawing.Point(350, 2);
            this.ComprasTxtCantidad.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCantidad.MaxLength = 2147483647;
            this.ComprasTxtCantidad.MostrarEtiquetaError = false;
            this.ComprasTxtCantidad.Name = "ComprasTxtCantidad";
            this.ComprasTxtCantidad.SelectedText = "";
            this.ComprasTxtCantidad.SelectionLength = 0;
            this.ComprasTxtCantidad.SelectionStart = 0;
            this.ComprasTxtCantidad.Size = new System.Drawing.Size(156, 36);
            this.ComprasTxtCantidad.SoloEnteros = true;
            this.ComprasTxtCantidad.TabIndex = 5;
            this.ComprasTxtCantidad.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Enteros;
            // 
            // ComprasLblCantidad
            // 
            this.ComprasLblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCantidad.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCantidad.Location = new System.Drawing.Point(251, 11);
            this.ComprasLblCantidad.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCantidad.Name = "ComprasLblCantidad";
            this.ComprasLblCantidad.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblCantidad.TabIndex = 23;
            this.ComprasLblCantidad.TabStop = false;
            this.ComprasLblCantidad.TamanoTipografia = 9F;
            this.ComprasLblCantidad.Text = "Cantidad";
            this.ComprasLblCantidad.Texto = "Cantidad";
            // 
            // ComprasLblPrecioUnitario
            // 
            this.ComprasLblPrecioUnitario.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblPrecioUnitario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPrecioUnitario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPrecioUnitario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPrecioUnitario.Location = new System.Drawing.Point(19, 11);
            this.ComprasLblPrecioUnitario.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.ComprasLblPrecioUnitario.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPrecioUnitario.MostrarAsterisco = false;
            this.ComprasLblPrecioUnitario.Name = "ComprasLblPrecioUnitario";
            this.ComprasLblPrecioUnitario.Size = new System.Drawing.Size(220, 18);
            this.ComprasLblPrecioUnitario.TabIndex = 24;
            this.ComprasLblPrecioUnitario.TabStop = false;
            this.ComprasLblPrecioUnitario.TamanoTipografia = 9F;
            this.ComprasLblPrecioUnitario.Text = "Costo unitario";
            this.ComprasLblPrecioUnitario.Texto = "Costo unitario";
            // 
            // ComprasFlpProductoSku
            // 
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasCboSku);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasLblSku);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasCboProducto);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasLblProducto);
            this.ComprasFlpProductoSku.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasFlpProductoSku.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ComprasFlpProductoSku.Location = new System.Drawing.Point(27, 10);
            this.ComprasFlpProductoSku.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpProductoSku.Name = "ComprasFlpProductoSku";
            this.ComprasFlpProductoSku.Size = new System.Drawing.Size(864, 40);
            this.ComprasFlpProductoSku.TabIndex = 8;
            // 
            // ComprasCboSku
            // 
            this.ComprasCboSku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboSku.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboSku.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboSku.Location = new System.Drawing.Point(664, 0);
            this.ComprasCboSku.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboSku.MaxLength = 0;
            this.ComprasCboSku.MostrarEtiquetaError = false;
            this.ComprasCboSku.Name = "ComprasCboSku";
            this.ComprasCboSku.Size = new System.Drawing.Size(200, 40);
            this.ComprasCboSku.TabIndex = 4;
            // 
            // ComprasLblSku
            // 
            this.ComprasLblSku.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblSku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSku.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSku.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSku.Location = new System.Drawing.Point(577, 11);
            this.ComprasLblSku.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSku.Name = "ComprasLblSku";
            this.ComprasLblSku.Size = new System.Drawing.Size(84, 18);
            this.ComprasLblSku.TabIndex = 23;
            this.ComprasLblSku.TabStop = false;
            this.ComprasLblSku.TamanoTipografia = 9F;
            this.ComprasLblSku.Text = "SKU";
            this.ComprasLblSku.Texto = "SKU";
            // 
            // ComprasCboProducto
            // 
            this.ComprasCboProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProducto.Location = new System.Drawing.Point(154, 0);
            this.ComprasCboProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProducto.MaxLength = 0;
            this.ComprasCboProducto.MostrarEtiquetaError = false;
            this.ComprasCboProducto.Name = "ComprasCboProducto";
            this.ComprasCboProducto.Size = new System.Drawing.Size(420, 40);
            this.ComprasCboProducto.TabIndex = 3;
            // 
            // ComprasLblProducto
            // 
            this.ComprasLblProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProducto.Location = new System.Drawing.Point(27, 11);
            this.ComprasLblProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblProducto.Name = "ComprasLblProducto";
            this.ComprasLblProducto.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblProducto.TabIndex = 25;
            this.ComprasLblProducto.TabStop = false;
            this.ComprasLblProducto.TamanoTipografia = 9F;
            this.ComprasLblProducto.Text = "Producto";
            this.ComprasLblProducto.Texto = "Producto";
            // 
            // ComprasTlpTotalesArticulo
            // 
            this.ComprasTlpTotalesArticulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotalesArticulo.ColumnCount = 1;
            this.ComprasTlpTotalesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotalesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpTotalesArticulo.Controls.Add(this.ComprasLblTotalesDeProducto, 0, 0);
            this.ComprasTlpTotalesArticulo.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasTlpTotalesArticulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotalesArticulo.Location = new System.Drawing.Point(51, 90);
            this.ComprasTlpTotalesArticulo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpTotalesArticulo.Name = "ComprasTlpTotalesArticulo";
            this.ComprasTlpTotalesArticulo.RowCount = 1;
            this.ComprasTlpTotalesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotalesArticulo.Size = new System.Drawing.Size(840, 40);
            this.ComprasTlpTotalesArticulo.TabIndex = 19;
            // 
            // ComprasLblTotalesDeProducto
            // 
            this.ComprasLblTotalesDeProducto.AlineacionTexto = System.Drawing.ContentAlignment.BottomRight;
            this.ComprasLblTotalesDeProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblTotalesDeProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotalesDeProducto.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblTotalesDeProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotalesDeProducto.Location = new System.Drawing.Point(48, 9);
            this.ComprasLblTotalesDeProducto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblTotalesDeProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotalesDeProducto.MostrarAsterisco = false;
            this.ComprasLblTotalesDeProducto.Name = "ComprasLblTotalesDeProducto";
            this.ComprasLblTotalesDeProducto.Size = new System.Drawing.Size(789, 22);
            this.ComprasLblTotalesDeProducto.TabIndex = 3;
            this.ComprasLblTotalesDeProducto.TabStop = false;
            this.ComprasLblTotalesDeProducto.TamanoTipografia = 10F;
            this.ComprasLblTotalesDeProducto.Text = "";
            this.ComprasLblTotalesDeProducto.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ComprasLblTotalesDeProducto.Texto = "";
            // 
            // ComprasTlpAccionesArticulo
            // 
            this.ComprasTlpAccionesArticulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpAccionesArticulo.ColumnCount = 1;
            this.ComprasTlpAccionesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAccionesArticulo.Controls.Add(this.ComprasFlpAccionesOrden, 0, 1);
            this.ComprasTlpAccionesArticulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAccionesArticulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpAccionesArticulo.Location = new System.Drawing.Point(894, 3);
            this.ComprasTlpAccionesArticulo.Name = "ComprasTlpAccionesArticulo";
            this.ComprasTlpAccionesArticulo.RowCount = 3;
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAccionesArticulo.Size = new System.Drawing.Size(377, 134);
            this.ComprasTlpAccionesArticulo.TabIndex = 6;
            // 
            // ComprasFlpAccionesOrden
            // 
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnIngresarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnModificarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnEliminarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnRefrescarProducto);
            this.ComprasFlpAccionesOrden.Dock = System.Windows.Forms.DockStyle.Left;
            this.ComprasFlpAccionesOrden.Location = new System.Drawing.Point(0, 27);
            this.ComprasFlpAccionesOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAccionesOrden.Name = "ComprasFlpAccionesOrden";
            this.ComprasFlpAccionesOrden.Size = new System.Drawing.Size(377, 80);
            this.ComprasFlpAccionesOrden.TabIndex = 0;
            // 
            // ComprasBtnIngresarProducto
            // 
            this.ComprasBtnIngresarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnIngresarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnIngresarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIngresarProducto.BackgroundImage")));
            this.ComprasBtnIngresarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnIngresarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIngresarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIngresarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnIngresarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnIngresarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIngresarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIngresarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarProducto.Location = new System.Drawing.Point(10, 0);
            this.ComprasBtnIngresarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnIngresarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.Name = "ComprasBtnIngresarProducto";
            this.ComprasBtnIngresarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.TabIndex = 8;
            this.ComprasBtnIngresarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnModificarProducto
            // 
            this.ComprasBtnModificarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnModificarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnModificarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnModificarProducto.BackgroundImage")));
            this.ComprasBtnModificarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnModificarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnModificarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnModificarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnModificarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnModificarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnModificarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnModificarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.Location = new System.Drawing.Point(100, 0);
            this.ComprasBtnModificarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnModificarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.Name = "ComprasBtnModificarProducto";
            this.ComprasBtnModificarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.TabIndex = 1;
            this.ComprasBtnModificarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnEliminarProducto
            // 
            this.ComprasBtnEliminarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnEliminarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEliminarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEliminarProducto.BackgroundImage")));
            this.ComprasBtnEliminarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEliminarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnEliminarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEliminarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEliminarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.Location = new System.Drawing.Point(190, 0);
            this.ComprasBtnEliminarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnEliminarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.Name = "ComprasBtnEliminarProducto";
            this.ComprasBtnEliminarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.TabIndex = 2;
            this.ComprasBtnEliminarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnRefrescarProducto
            // 
            this.ComprasBtnRefrescarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnRefrescarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnRefrescarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnRefrescarProducto.BackgroundImage")));
            this.ComprasBtnRefrescarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnRefrescarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnRefrescarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRefrescarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnRefrescarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnRefrescarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnRefrescarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnRefrescarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnRefrescarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRefrescarProducto.Location = new System.Drawing.Point(280, 0);
            this.ComprasBtnRefrescarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnRefrescarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.Name = "ComprasBtnRefrescarProducto";
            this.ComprasBtnRefrescarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.TabIndex = 3;
            this.ComprasBtnRefrescarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasUsrEncabezado
            // 
            this.ComprasUsrEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezado.Codigo = "6003";
            this.ComprasUsrEncabezado.Descripcion = "Registra órdenes de compra con los datos del proveedor, los artículos solicitados" +
    " y el detalle de importes, descuentos e impuestos.";
            this.ComprasUsrEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrEncabezado.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.Name = "ComprasUsrEncabezado";
            this.ComprasUsrEncabezado.Size = new System.Drawing.Size(1300, 130);
            this.ComprasUsrEncabezado.TabIndex = 7;
            this.ComprasUsrEncabezado.TabStop = false;
            this.ComprasUsrEncabezado.Titulo = "ORDENES DE COMPRA";
            // 
            // FrmDetalleOrdenCompra
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 1055);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmDetalleOrdenCompra";
            this.Text = "6003 – OrdenesCompra";
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasGbxEncabezado.ResumeLayout(false);
            this.ComprasTlpEncabezado.ResumeLayout(false);
            this.ComprasTlpDatosOrden.ResumeLayout(false);
            this.ComprasTlpProveedor.ResumeLayout(false);
            this.ComprasTlpResponsable.ResumeLayout(false);
            this.ComprasGbxFinalizacion.ResumeLayout(false);
            this.ComprasTlpFinalizacion.ResumeLayout(false);
            this.ComprasTlpImportes.ResumeLayout(false);
            this.ComprasTlpComentarioYTotales.ResumeLayout(false);
            this.ComprasTlpTotales.ResumeLayout(false);
            this.ComprasGbxDetalles.ResumeLayout(false);
            this.ComprasTlpDetalles.ResumeLayout(false);
            this.ComprasPnlArticulos.ResumeLayout(false);
            this.ComprasTlpMantenimientoProductos.ResumeLayout(false);
            this.ComprasTlpDetallesProducto.ResumeLayout(false);
            this.ComprasFlpPrecioCantidadDescuento.ResumeLayout(false);
            this.ComprasFlpProductoSku.ResumeLayout(false);
            this.ComprasTlpTotalesArticulo.ResumeLayout(false);
            this.ComprasTlpAccionesArticulo.ResumeLayout(false);
            this.ComprasFlpAccionesOrden.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxEncabezado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpEncabezado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpResponsable;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDireccionProveedor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNombreProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtEstadoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDireccionProveedor;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpImportes;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotales;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalDeLaOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTipoCambio;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSubtotal;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtImpuesto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaImpuesto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaSubtotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTipoCambio;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaComentario;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxDetalles;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDetalles;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlArticulos;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtComentario;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvProductos;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboNombreProveedor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMantenimientoProductos;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpAccionesArticulo;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAccionesOrden;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIngresarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnModificarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnEliminarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnRefrescarProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDiasPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDiasPlazo;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpComentarioYTotales;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnDescartar;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDatosOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtPlazoValor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaDeEmision;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaDeVencimiento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCodigoOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCodigoOrden;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnGuardar;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoValor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDetallesProducto;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpPrecioCantidadDescuento;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnTogglePorcentaje;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCantidad;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCantidad;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPrecioUnitario;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpProductoSku;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboSku;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSku;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblProducto;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotalesArticulo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTotalesDeProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnRegresar;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFechaEmision;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFechaVencimiento;
    }
}
