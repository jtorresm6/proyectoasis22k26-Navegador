/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Capturar y administrar pedidos de Ventas.
 */
/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Jose, Natali
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Encargado: Matthew Andreé Juárez Xoy
 * Fecha de actualización: 14/10/2026
 * Propósito: Formulario de Ventas y cuentas por cobrar.
 */
namespace CapaVista_Pedidos_Ventas
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Define los controles visuales del formulario de pedidos.
    public partial class FrmPedidoVentas : System.Windows.Forms.Form
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPedidoVentas));
            this.VentasTtpAcciones = new System.Windows.Forms.ToolTip(this.components);
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasLblCodigo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasPnlContenido = new System.Windows.Forms.Panel();
            this.VentasBtnGuardar = new System.Windows.Forms.Button();
            this.VentasBtnCancelar = new System.Windows.Forms.Button();
            this.VentasBtnNuevoPedido = new System.Windows.Forms.Button();
            this.VentasBtnConsultarPedido = new System.Windows.Forms.Button();
            this.VentasBtnModificarPedido = new System.Windows.Forms.Button();
            this.VentasBtnAnularPedido = new System.Windows.Forms.Button();
            this.VentasBtnAyuda = new System.Windows.Forms.Button();
            this.VentasPnlResumen = new System.Windows.Forms.Panel();
            this.VentasLblTotal = new System.Windows.Forms.Label();
            this.VentasLblSubtotal = new System.Windows.Forms.Label();
            this.VentasLblDescuentoTotal = new System.Windows.Forms.Label();
            this.VentasLblResumenTitulo = new System.Windows.Forms.Label();
            this.VentasLblEstadoPedido = new System.Windows.Forms.Label();
            this.VentasCboEstadoPedido = new System.Windows.Forms.ComboBox();
            this.VentasLblMonedaPedido = new System.Windows.Forms.Label();
            this.VentasTxtMonedaPedido = new System.Windows.Forms.TextBox();
            this.VentasLblListaPrecioPedido = new System.Windows.Forms.Label();
            this.VentasCboListaPrecioPedido = new System.Windows.Forms.ComboBox();
            this.VentasLblIdentificacionCliente = new System.Windows.Forms.Label();
            this.VentasTxtIdentificacionCliente = new System.Windows.Forms.TextBox();
            this.VentasLblContactoCliente = new System.Windows.Forms.Label();
            this.VentasTxtContactoCliente = new System.Windows.Forms.TextBox();
            this.VentasLblDireccionCliente = new System.Windows.Forms.Label();
            this.VentasTxtDireccionCliente = new System.Windows.Forms.TextBox();
            this.VentasDgvDetalle = new System.Windows.Forms.DataGridView();
            this.VentasDgvDetalleColCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColDescuento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasPnlEncabezadoDetalle = new System.Windows.Forms.Panel();
            this.VentasCboProductoPedido = new System.Windows.Forms.ComboBox();
            this.VentasNudCantidadProducto = new System.Windows.Forms.NumericUpDown();
            this.VentasNudDescuentoProducto = new System.Windows.Forms.NumericUpDown();
            this.VentasLblCantidadProducto = new System.Windows.Forms.Label();
            this.VentasLblDescuentoProducto = new System.Windows.Forms.Label();
            this.VentasLblPrecioListaProducto = new System.Windows.Forms.Label();
            this.VentasTxtPrecioListaProducto = new System.Windows.Forms.TextBox();
            this.VentasLblDetalleAyuda = new System.Windows.Forms.Label();
            this.VentasBtnEliminarProducto = new System.Windows.Forms.Button();
            this.VentasBtnIngresarProducto = new System.Windows.Forms.Button();
            this.VentasLblDetalleTitulo = new System.Windows.Forms.Label();
            this.VentasPnlDatosPedido = new System.Windows.Forms.Panel();
            this.VentasCboCotizacionOrigen = new System.Windows.Forms.ComboBox();
            this.VentasLblCotizacionOrigen = new System.Windows.Forms.Label();
            this.VentasRdoPedidoIndependiente = new System.Windows.Forms.RadioButton();
            this.VentasRdoPedidoPorCotizacion = new System.Windows.Forms.RadioButton();
            this.VentasLblOrigenPedido = new System.Windows.Forms.Label();
            this.VentasCboVendedor = new System.Windows.Forms.ComboBox();
            this.VentasLblVendedor = new System.Windows.Forms.Label();
            this.VentasDtpEntrega = new System.Windows.Forms.DateTimePicker();
            this.VentasLblEntrega = new System.Windows.Forms.Label();
            this.VentasDtpVencimiento = new System.Windows.Forms.DateTimePicker();
            this.VentasLblVencimiento = new System.Windows.Forms.Label();
            this.VentasDtpPedido = new System.Windows.Forms.DateTimePicker();
            this.VentasLblFechaPedido = new System.Windows.Forms.Label();
            this.VentasUsrCliente = new CapaVista_Pedidos_Ventas.VentasUsrCliente();
            this.VentasTxtNumeroPedido = new System.Windows.Forms.TextBox();
            this.VentasLblNumeroPedido = new System.Windows.Forms.Label();
            this.VentasLblDatosTitulo = new System.Windows.Forms.Label();
            this.VentasPnlEncabezado.SuspendLayout();
            this.VentasPnlContenido.SuspendLayout();
            this.VentasPnlResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalle)).BeginInit();
            this.VentasPnlEncabezadoDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudCantidadProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudDescuentoProducto)).BeginInit();
            this.VentasPnlDatosPedido.SuspendLayout();
            this.SuspendLayout();
            //
            // VentasPnlEncabezado
            //
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblCodigo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Padding = new System.Windows.Forms.Padding(28, 18, 28, 18);
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1120, 80);
            this.VentasPnlEncabezado.TabIndex = 0;
            //
            // VentasLblCodigo
            //
            this.VentasLblCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.VentasLblCodigo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblCodigo.Location = new System.Drawing.Point(916, 24);
            this.VentasLblCodigo.Name = "VentasLblCodigo";
            this.VentasLblCodigo.Size = new System.Drawing.Size(174, 30);
            this.VentasLblCodigo.TabIndex = 2;
            this.VentasLblCodigo.Text = "7004  ·  PEDIDOS";
            this.VentasLblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VentasLblSubtitulo
            //
            this.VentasLblSubtitulo.AutoSize = true;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(31, 52);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(404, 20);
            this.VentasLblSubtitulo.TabIndex = 1;
            this.VentasLblSubtitulo.Text = "VENTAS Y CUENTAS POR COBRAR  /  GESTIÓN DE PEDIDOS";
            //
            // VentasLblTitulo
            //
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 19F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(27, 12);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(145, 37);
            this.VentasLblTitulo.TabIndex = 0;
            this.VentasLblTitulo.Text = "Pedidos";
            //
            // VentasPnlContenido
            //
            this.VentasPnlContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasPnlContenido.Controls.Add(this.VentasBtnGuardar);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnCancelar);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnNuevoPedido);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnConsultarPedido);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnModificarPedido);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnAnularPedido);
            this.VentasPnlContenido.Controls.Add(this.VentasBtnAyuda);
            this.VentasPnlContenido.Controls.Add(this.VentasPnlResumen);
            this.VentasPnlContenido.Controls.Add(this.VentasDgvDetalle);
            this.VentasPnlContenido.Controls.Add(this.VentasPnlEncabezadoDetalle);
            this.VentasPnlContenido.Controls.Add(this.VentasPnlDatosPedido);
            this.VentasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasPnlContenido.Location = new System.Drawing.Point(0, 80);
            this.VentasPnlContenido.Name = "VentasPnlContenido";
            this.VentasPnlContenido.Padding = new System.Windows.Forms.Padding(26, 20, 26, 18);
            this.VentasPnlContenido.Size = new System.Drawing.Size(1120, 720);
            this.VentasPnlContenido.TabIndex = 1;
            this.VentasBtnNuevoPedido.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalIngresarVentas")));
            this.VentasBtnNuevoPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnNuevoPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnNuevoPedido.FlatAppearance.BorderSize = 0;
            this.VentasBtnNuevoPedido.Location = new System.Drawing.Point(30, 628);
            this.VentasBtnNuevoPedido.Name = "VentasBtnNuevoPedido";
            this.VentasBtnNuevoPedido.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnNuevoPedido.Text = string.Empty;
            this.VentasBtnNuevoPedido.AccessibleName = "Nuevo pedido";
            this.VentasBtnNuevoPedido.Click += new System.EventHandler(this.VentasMetAccion);
            this.VentasBtnConsultarPedido.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalConsultarVentas")));
            this.VentasBtnConsultarPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnConsultarPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnConsultarPedido.FlatAppearance.BorderSize = 0;
            this.VentasBtnConsultarPedido.Location = new System.Drawing.Point(106, 628);
            this.VentasBtnConsultarPedido.Name = "VentasBtnConsultarPedido";
            this.VentasBtnConsultarPedido.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnConsultarPedido.Text = string.Empty;
            this.VentasBtnConsultarPedido.AccessibleName = "Consultar pedidos";
            this.VentasBtnConsultarPedido.Click += new System.EventHandler(this.VentasMetAccion);
            this.VentasBtnModificarPedido.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalModificarVentas")));
            this.VentasBtnModificarPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnModificarPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnModificarPedido.FlatAppearance.BorderSize = 0;
            this.VentasBtnModificarPedido.Location = new System.Drawing.Point(182, 628);
            this.VentasBtnModificarPedido.Name = "VentasBtnModificarPedido";
            this.VentasBtnModificarPedido.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnModificarPedido.Text = string.Empty;
            this.VentasBtnModificarPedido.AccessibleName = "Modificar pedido";
            this.VentasBtnModificarPedido.Click += new System.EventHandler(this.VentasMetAccion);
            this.VentasBtnAnularPedido.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalEliminarVentas")));
            this.VentasBtnAnularPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnularPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnularPedido.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnularPedido.Location = new System.Drawing.Point(258, 628);
            this.VentasBtnAnularPedido.Name = "VentasBtnAnularPedido";
            this.VentasBtnAnularPedido.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAnularPedido.Text = string.Empty;
            this.VentasBtnAnularPedido.AccessibleName = "Anular pedido";
            this.VentasBtnAnularPedido.Click += new System.EventHandler(this.VentasMetAccion);
            this.VentasBtnAyuda.AccessibleName = "Ayuda de pedidos";
            this.VentasBtnAyuda.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnAyuda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalAyudaVentas")));
            this.VentasBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAyuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VentasBtnAyuda.FlatAppearance.BorderSize = 0;
            this.VentasBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAyuda.Location = new System.Drawing.Point(334, 628);
            this.VentasBtnAyuda.Name = "VentasBtnAyuda";
            this.VentasBtnAyuda.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAyuda.Click += new System.EventHandler(this.VentasMetAyudaClick);
            //
            // VentasBtnGuardar
            //
            this.VentasBtnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

            this.VentasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnGuardar.Location = new System.Drawing.Point(950, 628);
            this.VentasBtnGuardar.Name = "VentasBtnGuardar";
            this.VentasBtnGuardar.AccessibleDescription = "Guardar";
            this.VentasBtnGuardar.Text = string.Empty;
            this.VentasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnGuardar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnGuardar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Guardar")));
            this.VentasBtnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.VentasBtnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnGuardar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnGuardar.UseVisualStyleBackColor = false;
            this.VentasBtnGuardar.UseCompatibleTextRendering = false;
            this.VentasBtnGuardar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnGuardar.TabIndex = 0;
            this.VentasBtnGuardar.Click += new System.EventHandler(this.VentasMetAccion);

            this.VentasBtnGuardar.AccessibleName = "Guardar";
            //
            // VentasBtnCancelar
            //
            this.VentasBtnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

            this.VentasBtnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnCancelar.Location = new System.Drawing.Point(1026, 628);
            this.VentasBtnCancelar.Name = "VentasBtnCancelar";
            this.VentasBtnCancelar.AccessibleDescription = "Cancelar";
            this.VentasBtnCancelar.Text = string.Empty;
            this.VentasBtnCancelar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnCancelar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCancelar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Cancelar")));
            this.VentasBtnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCancelar.FlatAppearance.BorderSize = 0;
            this.VentasBtnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnCancelar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnCancelar.UseVisualStyleBackColor = false;
            this.VentasBtnCancelar.UseCompatibleTextRendering = false;
            this.VentasBtnCancelar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCancelar.TabIndex = 1;
            this.VentasBtnCancelar.Click += new System.EventHandler(this.VentasMetAccion);

            this.VentasBtnCancelar.AccessibleName = "Cancelar";
            //
            // VentasPnlResumen
            //
            this.VentasPnlResumen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.VentasPnlResumen.BackColor = System.Drawing.Color.White;
            this.VentasPnlResumen.Controls.Add(this.VentasLblTotal);
            this.VentasPnlResumen.Controls.Add(this.VentasLblSubtotal);
            this.VentasPnlResumen.Controls.Add(this.VentasLblDescuentoTotal);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenTitulo);
            this.VentasPnlResumen.Location = new System.Drawing.Point(575, 606);
            this.VentasPnlResumen.Name = "VentasPnlResumen";
            this.VentasPnlResumen.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.VentasPnlResumen.Size = new System.Drawing.Size(340, 108);
            this.VentasPnlResumen.TabIndex = 5;
            //
            // VentasLblTotal
            //
            this.VentasLblTotal.Font = new System.Drawing.Font("Tahoma", 13F);
            this.VentasLblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTotal.Location = new System.Drawing.Point(16, 66);
            this.VentasLblTotal.Name = "VentasLblTotal";
            this.VentasLblTotal.Size = new System.Drawing.Size(302, 36);
            this.VentasLblTotal.TabIndex = 3;
            this.VentasLblTotal.Text = "TOTAL DEL PEDIDO     Q 0.00";
            this.VentasLblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblSubtotal
            //
            this.VentasLblSubtotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(93)))), ((int)(((byte)(103)))));
            this.VentasLblSubtotal.Location = new System.Drawing.Point(16, 25);
            this.VentasLblSubtotal.Name = "VentasLblSubtotal";
            this.VentasLblSubtotal.Size = new System.Drawing.Size(302, 18);
            this.VentasLblSubtotal.TabIndex = 1;
            this.VentasLblSubtotal.Text = "Subtotal                          Q 0.00";
            this.VentasLblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblDescuentoTotal
            //
            this.VentasLblDescuentoTotal.Location = new System.Drawing.Point(16, 44);
            this.VentasLblDescuentoTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDescuentoTotal.Name = "VentasLblDescuentoTotal";
            this.VentasLblDescuentoTotal.Size = new System.Drawing.Size(302, 18);
            this.VentasLblDescuentoTotal.Text = "Descuentos 0.00";
            this.VentasLblDescuentoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblResumenTitulo
            //
            this.VentasLblResumenTitulo.AutoSize = true;
            this.VentasLblResumenTitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblResumenTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasLblResumenTitulo.Location = new System.Drawing.Point(16, 8);
            this.VentasLblResumenTitulo.Name = "VentasLblResumenTitulo";
            this.VentasLblResumenTitulo.Size = new System.Drawing.Size(154, 18);
            this.VentasLblResumenTitulo.TabIndex = 0;
            this.VentasLblResumenTitulo.Text = "RESUMEN DEL PEDIDO";
            //
            // VentasDgvDetalle
            //
            this.VentasDgvDetalle.AllowUserToAddRows = false;
            this.VentasDgvDetalle.AllowUserToDeleteRows = false;
            this.VentasDgvDetalle.AllowUserToResizeRows = false;
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvDetalle.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1;
            this.VentasDgvDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvDetalle.BackgroundColor = System.Drawing.Color.White;
            this.VentasDgvDetalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VentasDgvDetalle.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.VentasDgvDetalle.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvDetalle.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2;
            this.VentasDgvDetalle.ColumnHeadersHeight = 40;
            this.VentasDgvDetalle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasDgvDetalleColCodigo,
            this.VentasDgvDetalleColProducto,
            this.VentasDgvDetalleColCantidad,
            this.VentasDgvDetalleColPrecio,
            this.VentasDgvDetalleColDescuento,
            this.VentasDgvDetalleColTotal});
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            DataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(51)))), ((int)(((byte)(63)))));
            DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvDetalle.DefaultCellStyle = DataGridViewCellStyle3;
            this.VentasDgvDetalle.EnableHeadersVisualStyles = false;
            this.VentasDgvDetalle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(229)))), ((int)(((byte)(238)))));
            this.VentasDgvDetalle.Location = new System.Drawing.Point(30, 474);
            this.VentasDgvDetalle.MultiSelect = false;
            this.VentasDgvDetalle.Name = "VentasDgvDetalle";
            this.VentasDgvDetalle.ReadOnly = true;
            this.VentasDgvDetalle.RowHeadersVisible = false;
            this.VentasDgvDetalle.RowHeadersWidth = 51;
            this.VentasDgvDetalle.RowTemplate.Height = 38;
            this.VentasDgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvDetalle.Size = new System.Drawing.Size(1060, 120);
            this.VentasDgvDetalle.TabIndex = 2;
            //
            // VentasDgvDetalleColCodigo
            //
            this.VentasDgvDetalleColCodigo.FillWeight = 75F;
            this.VentasDgvDetalleColCodigo.HeaderText = "CÓDIGO";
            this.VentasDgvDetalleColCodigo.MinimumWidth = 6;
            this.VentasDgvDetalleColCodigo.Name = "VentasDgvDetalleColCodigo";
            this.VentasDgvDetalleColCodigo.ReadOnly = false;
            //
            // VentasDgvDetalleColProducto
            //
            this.VentasDgvDetalleColProducto.FillWeight = 210F;
            this.VentasDgvDetalleColProducto.HeaderText = "PRODUCTO";
            this.VentasDgvDetalleColProducto.MinimumWidth = 6;
            this.VentasDgvDetalleColProducto.Name = "VentasDgvDetalleColProducto";
            this.VentasDgvDetalleColProducto.ReadOnly = false;
            //
            //
            //
            // VentasDgvDetalleColCantidad
            //
            this.VentasDgvDetalleColCantidad.FillWeight = 75F;
            this.VentasDgvDetalleColCantidad.HeaderText = "CANTIDAD";
            this.VentasDgvDetalleColCantidad.MinimumWidth = 6;
            this.VentasDgvDetalleColCantidad.Name = "VentasDgvDetalleColCantidad";
            this.VentasDgvDetalleColCantidad.ReadOnly = false;
            //
            // VentasDgvDetalleColPrecio
            //
            this.VentasDgvDetalleColPrecio.FillWeight = 90F;
            this.VentasDgvDetalleColPrecio.HeaderText = "PRECIO UNITARIO";
            this.VentasDgvDetalleColPrecio.MinimumWidth = 6;
            this.VentasDgvDetalleColPrecio.Name = "VentasDgvDetalleColPrecio";
            this.VentasDgvDetalleColPrecio.ReadOnly = false;
            //
            // VentasDgvDetalleColDescuento
            //
            this.VentasDgvDetalleColDescuento.FillWeight = 75F;
            this.VentasDgvDetalleColDescuento.HeaderText = "DESCUENTO";
            this.VentasDgvDetalleColDescuento.MinimumWidth = 6;
            this.VentasDgvDetalleColDescuento.Name = "VentasDgvDetalleColDescuento";
            this.VentasDgvDetalleColDescuento.ReadOnly = false;
            //
            // VentasDgvDetalleColTotal
            //
            this.VentasDgvDetalleColTotal.FillWeight = 90F;
            this.VentasDgvDetalleColTotal.HeaderText = "TOTAL";
            this.VentasDgvDetalleColTotal.MinimumWidth = 6;
            this.VentasDgvDetalleColTotal.Name = "VentasDgvDetalleColTotal";
            this.VentasDgvDetalleColTotal.ReadOnly = false;
            //
            // VentasPnlEncabezadoDetalle
            //
            this.VentasPnlEncabezadoDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasPnlEncabezadoDetalle.BackColor = System.Drawing.Color.White;
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblDetalleAyuda);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasCboProductoPedido);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasNudCantidadProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasNudDescuentoProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblCantidadProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblDescuentoProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblPrecioListaProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasTxtPrecioListaProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasBtnEliminarProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasBtnIngresarProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblDetalleTitulo);
            this.VentasPnlEncabezadoDetalle.Location = new System.Drawing.Point(30, 410);
            this.VentasPnlEncabezadoDetalle.Name = "VentasPnlEncabezadoDetalle";
            this.VentasPnlEncabezadoDetalle.Padding = new System.Windows.Forms.Padding(14, 10, 14, 8);
            this.VentasPnlEncabezadoDetalle.Size = new System.Drawing.Size(1060, 64);
            this.VentasPnlEncabezadoDetalle.TabIndex = 1;
            this.VentasCboProductoPedido.DisplayMember = "Descripcion";
            this.VentasCboProductoPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboProductoPedido.Location = new System.Drawing.Point(255, 18);
            this.VentasCboProductoPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboProductoPedido.Name = "VentasCboProductoPedido";
            this.VentasCboProductoPedido.Size = new System.Drawing.Size(280, 28);
            this.VentasNudCantidadProducto.Location = new System.Drawing.Point(545, 18);
            this.VentasNudCantidadProducto.DecimalPlaces = 0;
            this.VentasNudCantidadProducto.Maximum = 99999999m;
            this.VentasNudCantidadProducto.Minimum = 1m;
            this.VentasNudCantidadProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasNudCantidadProducto.Name = "VentasNudCantidadProducto";
            this.VentasNudCantidadProducto.Size = new System.Drawing.Size(85, 28);
            this.VentasNudCantidadProducto.Value = 1m;
            this.VentasNudDescuentoProducto.DecimalPlaces = 2;
            this.VentasNudDescuentoProducto.Location = new System.Drawing.Point(745, 18);
            this.VentasNudDescuentoProducto.Maximum = 99999999.99m;
            this.VentasNudDescuentoProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasNudDescuentoProducto.Name = "VentasNudDescuentoProducto";
            this.VentasNudDescuentoProducto.Size = new System.Drawing.Size(130, 28);
            this.VentasNudDescuentoProducto.ThousandsSeparator = true;
            this.VentasTtpAcciones.SetToolTip(this.VentasNudDescuentoProducto, "Descuento total de esta línea");
            this.VentasLblCantidadProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCantidadProducto.Location = new System.Drawing.Point(545, 2);
            this.VentasLblCantidadProducto.Name = "VentasLblCantidadProducto";
            this.VentasLblCantidadProducto.Size = new System.Drawing.Size(85, 16);
            this.VentasLblCantidadProducto.Text = "Cantidad";
            this.VentasLblDescuentoProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDescuentoProducto.Location = new System.Drawing.Point(745, 2);
            this.VentasLblDescuentoProducto.Name = "VentasLblDescuentoProducto";
            this.VentasLblDescuentoProducto.Size = new System.Drawing.Size(130, 16);
            this.VentasLblDescuentoProducto.Text = "Descuento";
            this.VentasLblPrecioListaProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblPrecioListaProducto.Location = new System.Drawing.Point(640, 2);
            this.VentasLblPrecioListaProducto.Name = "VentasLblPrecioListaProducto";
            this.VentasLblPrecioListaProducto.Size = new System.Drawing.Size(95, 16);
            this.VentasLblPrecioListaProducto.Text = "Precio de lista";
            this.VentasTxtPrecioListaProducto.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasTxtPrecioListaProducto.Location = new System.Drawing.Point(640, 18);
            this.VentasTxtPrecioListaProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtPrecioListaProducto.Name = "VentasTxtPrecioListaProducto";
            this.VentasTxtPrecioListaProducto.ReadOnly = true;
            this.VentasTxtPrecioListaProducto.Size = new System.Drawing.Size(95, 27);
            //
            // VentasLblDetalleAyuda
            //
            this.VentasLblDetalleAyuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblDetalleAyuda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDetalleAyuda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(119)))), ((int)(((byte)(132)))));
            this.VentasLblDetalleAyuda.Location = new System.Drawing.Point(250, 18);
            this.VentasLblDetalleAyuda.Name = "VentasLblDetalleAyuda";
            this.VentasLblDetalleAyuda.Size = new System.Drawing.Size(630, 28);
            this.VentasLblDetalleAyuda.TabIndex = 1;
            this.VentasLblDetalleAyuda.Text = "Seleccione los productos del pedido";
            this.VentasLblDetalleAyuda.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasBtnEliminarProducto
            //
            this.VentasBtnEliminarProducto.AccessibleName = "Eliminar";

            this.VentasBtnEliminarProducto.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnEliminarProducto.Location = new System.Drawing.Point(976, 0);
            this.VentasBtnEliminarProducto.Name = "VentasBtnEliminarProducto";
            this.VentasBtnEliminarProducto.AccessibleDescription = "Eliminar";
            this.VentasBtnEliminarProducto.Text = string.Empty;
            this.VentasBtnEliminarProducto.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnEliminarProducto.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnEliminarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnEliminarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Eliminar")));
            this.VentasBtnEliminarProducto.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEliminarProducto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEliminarProducto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnEliminarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnEliminarProducto.FlatAppearance.BorderSize = 0;
            this.VentasBtnEliminarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEliminarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEliminarProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnEliminarProducto.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnEliminarProducto.UseVisualStyleBackColor = false;
            this.VentasBtnEliminarProducto.UseCompatibleTextRendering = false;
            this.VentasBtnEliminarProducto.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnEliminarProducto.TabIndex = 3;
            this.VentasBtnEliminarProducto.Click += new System.EventHandler(this.VentasMetAccion);

            //
            // VentasBtnIngresarProducto
            //
            this.VentasBtnIngresarProducto.AccessibleName = "Ingresar";

            this.VentasBtnIngresarProducto.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnIngresarProducto.Location = new System.Drawing.Point(900, 0);
            this.VentasBtnIngresarProducto.Name = "VentasBtnIngresarProducto";
            this.VentasBtnIngresarProducto.AccessibleDescription = "Ingresar";
            this.VentasBtnIngresarProducto.Text = string.Empty;
            this.VentasBtnIngresarProducto.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnIngresarProducto.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnIngresarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnIngresarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Ingresar")));
            this.VentasBtnIngresarProducto.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnIngresarProducto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnIngresarProducto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnIngresarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnIngresarProducto.FlatAppearance.BorderSize = 0;
            this.VentasBtnIngresarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnIngresarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnIngresarProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnIngresarProducto.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnIngresarProducto.UseVisualStyleBackColor = false;
            this.VentasBtnIngresarProducto.UseCompatibleTextRendering = false;
            this.VentasBtnIngresarProducto.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnIngresarProducto.TabIndex = 2;
            this.VentasBtnIngresarProducto.Click += new System.EventHandler(this.VentasMetAccion);

            //
            // VentasLblDetalleTitulo
            //
            this.VentasLblDetalleTitulo.AutoSize = true;
            this.VentasLblDetalleTitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDetalleTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblDetalleTitulo.Location = new System.Drawing.Point(20, 20);
            this.VentasLblDetalleTitulo.Name = "VentasLblDetalleTitulo";
            this.VentasLblDetalleTitulo.Size = new System.Drawing.Size(188, 21);
            this.VentasLblDetalleTitulo.TabIndex = 0;
            this.VentasLblDetalleTitulo.Text = "Detalle de productos";
            //
            // VentasPnlDatosPedido
            //
            this.VentasPnlDatosPedido.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasPnlDatosPedido.BackColor = System.Drawing.Color.White;
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboCotizacionOrigen);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblEstadoPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboEstadoPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblMonedaPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtMonedaPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblListaPrecioPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboListaPrecioPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblIdentificacionCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtIdentificacionCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblContactoCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtContactoCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblDireccionCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtDireccionCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblCotizacionOrigen);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasRdoPedidoIndependiente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasRdoPedidoPorCotizacion);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblOrigenPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboVendedor);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblVendedor);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpEntrega);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblEntrega);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpVencimiento);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblVencimiento);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblFechaPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasUsrCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtNumeroPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblNumeroPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblDatosTitulo);
            this.VentasPnlDatosPedido.Location = new System.Drawing.Point(30, 12);
            this.VentasPnlDatosPedido.Name = "VentasPnlDatosPedido";
            this.VentasPnlDatosPedido.Padding = new System.Windows.Forms.Padding(16, 12, 16, 14);
            this.VentasPnlDatosPedido.Size = new System.Drawing.Size(1060, 390);
            this.VentasPnlDatosPedido.TabIndex = 0;
            //
            // Estado y moneda del pedido
            //
            this.VentasLblEstadoPedido.AutoSize = true;
            this.VentasLblEstadoPedido.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblEstadoPedido.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblEstadoPedido.Location = new System.Drawing.Point(26, 301);
            this.VentasLblEstadoPedido.Name = "VentasLblEstadoPedido";
            this.VentasLblEstadoPedido.Text = "Estado";
            this.VentasCboEstadoPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboEstadoPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboEstadoPedido.Location = new System.Drawing.Point(26, 322);
            this.VentasCboEstadoPedido.Name = "VentasCboEstadoPedido";
            this.VentasCboEstadoPedido.Size = new System.Drawing.Size(310, 30);
            this.VentasCboEstadoPedido.Items.AddRange(new object[] { "PENDIENTE", "CONFIRMADO", "COMPLETADO", "ANULADO" });
            this.VentasCboEstadoPedido.SelectedIndex = 0;
            this.VentasLblMonedaPedido.AutoSize = true;
            this.VentasLblMonedaPedido.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblMonedaPedido.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblMonedaPedido.Location = new System.Drawing.Point(375, 301);
            this.VentasLblMonedaPedido.Name = "VentasLblMonedaPedido";
            this.VentasLblMonedaPedido.Text = "Moneda";
            this.VentasTxtMonedaPedido.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasTxtMonedaPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtMonedaPedido.Location = new System.Drawing.Point(375, 322);
            this.VentasTxtMonedaPedido.Name = "VentasTxtMonedaPedido";
            this.VentasTxtMonedaPedido.ReadOnly = true;
            this.VentasTxtMonedaPedido.Size = new System.Drawing.Size(310, 30);
            this.VentasLblListaPrecioPedido.AutoSize = true;
            this.VentasLblListaPrecioPedido.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblListaPrecioPedido.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblListaPrecioPedido.Location = new System.Drawing.Point(724, 301);
            this.VentasLblListaPrecioPedido.Name = "VentasLblListaPrecioPedido";
            this.VentasLblListaPrecioPedido.Text = "Lista de precios del cliente";
            this.VentasCboListaPrecioPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboListaPrecioPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboListaPrecioPedido.Location = new System.Drawing.Point(724, 322);
            this.VentasCboListaPrecioPedido.Name = "VentasCboListaPrecioPedido";
            this.VentasCboListaPrecioPedido.Size = new System.Drawing.Size(310, 30);
            this.VentasLblIdentificacionCliente.AutoSize = true;
            this.VentasLblIdentificacionCliente.Location = new System.Drawing.Point(540, 103);
            this.VentasLblIdentificacionCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblIdentificacionCliente.Name = "VentasLblIdentificacionCliente";
            this.VentasLblIdentificacionCliente.Text = "Identificación del cliente";
            this.VentasTxtIdentificacionCliente.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasTxtIdentificacionCliente.Location = new System.Drawing.Point(540, 124);
            this.VentasTxtIdentificacionCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtIdentificacionCliente.Name = "VentasTxtIdentificacionCliente";
            this.VentasTxtIdentificacionCliente.ReadOnly = true;
            this.VentasTxtIdentificacionCliente.Size = new System.Drawing.Size(170, 27);
            this.VentasLblContactoCliente.AutoSize = true;
            this.VentasLblContactoCliente.Location = new System.Drawing.Point(724, 103);
            this.VentasLblContactoCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblContactoCliente.Name = "VentasLblContactoCliente";
            this.VentasLblContactoCliente.Text = "Contacto del cliente";
            this.VentasTxtContactoCliente.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasTxtContactoCliente.Location = new System.Drawing.Point(724, 124);
            this.VentasTxtContactoCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtContactoCliente.Name = "VentasTxtContactoCliente";
            this.VentasTxtContactoCliente.ReadOnly = true;
            this.VentasTxtContactoCliente.Size = new System.Drawing.Size(310, 27);
            this.VentasLblDireccionCliente.AutoSize = true;
            this.VentasLblDireccionCliente.Location = new System.Drawing.Point(26, 172);
            this.VentasLblDireccionCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDireccionCliente.Name = "VentasLblDireccionCliente";
            this.VentasLblDireccionCliente.Text = "Dirección";
            this.VentasTxtDireccionCliente.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasTxtDireccionCliente.Location = new System.Drawing.Point(26, 192);
            this.VentasTxtDireccionCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtDireccionCliente.Name = "VentasTxtDireccionCliente";
            this.VentasTxtDireccionCliente.ReadOnly = true;
            this.VentasTxtDireccionCliente.Size = new System.Drawing.Size(500, 30);
            //
            // VentasCboCotizacionOrigen
            //
            this.VentasCboCotizacionOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboCotizacionOrigen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboCotizacionOrigen.FormattingEnabled = true;
            this.VentasCboCotizacionOrigen.Location = new System.Drawing.Point(724, 192);
            this.VentasCboCotizacionOrigen.Name = "VentasCboCotizacionOrigen";
            this.VentasCboCotizacionOrigen.Size = new System.Drawing.Size(310, 30);
            this.VentasCboCotizacionOrigen.DropDownWidth = 310;
            this.VentasCboCotizacionOrigen.TabIndex = 23;
            this.VentasCboCotizacionOrigen.Visible = false;
            //
            // VentasLblCotizacionOrigen
            //
            this.VentasLblCotizacionOrigen.AutoSize = true;
            this.VentasLblCotizacionOrigen.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblCotizacionOrigen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblCotizacionOrigen.Location = new System.Drawing.Point(724, 172);
            this.VentasLblCotizacionOrigen.Name = "VentasLblCotizacionOrigen";
            this.VentasLblCotizacionOrigen.Size = new System.Drawing.Size(190, 18);
            this.VentasLblCotizacionOrigen.TabIndex = 22;
            this.VentasLblCotizacionOrigen.Text = "Cotización de referencia";
            this.VentasLblCotizacionOrigen.Visible = false;
            //
            // VentasRdoPedidoIndependiente
            //
            this.VentasRdoPedidoIndependiente.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasRdoPedidoIndependiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasRdoPedidoIndependiente.Location = new System.Drawing.Point(160, 58);
            this.VentasRdoPedidoIndependiente.Appearance = System.Windows.Forms.Appearance.Button;
            this.VentasRdoPedidoIndependiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasRdoPedidoIndependiente.FlatAppearance.BorderSize = 1;
            this.VentasRdoPedidoIndependiente.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasRdoPedidoIndependiente.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(201, 162, 74);
            this.VentasRdoPedidoIndependiente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasRdoPedidoIndependiente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasRdoPedidoIndependiente.Name = "VentasRdoPedidoIndependiente";
            this.VentasRdoPedidoIndependiente.Size = new System.Drawing.Size(155, 34);
            this.VentasRdoPedidoIndependiente.Checked = true;
            this.VentasRdoPedidoIndependiente.TabIndex = 0;
            this.VentasRdoPedidoIndependiente.Text = "Independiente";
            this.VentasRdoPedidoIndependiente.UseVisualStyleBackColor = false;
            //
            // VentasRdoPedidoPorCotizacion
            //
            this.VentasRdoPedidoPorCotizacion.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasRdoPedidoPorCotizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasRdoPedidoPorCotizacion.Location = new System.Drawing.Point(326, 58);
            this.VentasRdoPedidoPorCotizacion.Appearance = System.Windows.Forms.Appearance.Button;
            this.VentasRdoPedidoPorCotizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasRdoPedidoPorCotizacion.FlatAppearance.BorderSize = 1;
            this.VentasRdoPedidoPorCotizacion.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasRdoPedidoPorCotizacion.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(201, 162, 74);
            this.VentasRdoPedidoPorCotizacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasRdoPedidoPorCotizacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasRdoPedidoPorCotizacion.Name = "VentasRdoPedidoPorCotizacion";
            this.VentasRdoPedidoPorCotizacion.Size = new System.Drawing.Size(168, 34);
            this.VentasRdoPedidoPorCotizacion.TabIndex = 1;
            this.VentasRdoPedidoPorCotizacion.Text = "Por cotización";
            this.VentasRdoPedidoPorCotizacion.UseVisualStyleBackColor = false;
            //
            // VentasLblOrigenPedido
            //
            this.VentasLblOrigenPedido.AutoSize = true;
            this.VentasLblOrigenPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblOrigenPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblOrigenPedido.Location = new System.Drawing.Point(26, 65);
            this.VentasLblOrigenPedido.Name = "VentasLblOrigenPedido";
            this.VentasLblOrigenPedido.Size = new System.Drawing.Size(140, 18);
            this.VentasLblOrigenPedido.TabIndex = 19;
            this.VentasLblOrigenPedido.Text = "Tipo de pedido";
            //
            // VentasCboVendedor
            //
            this.VentasCboVendedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboVendedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboVendedor.FormattingEnabled = true;
            this.VentasCboVendedor.Location = new System.Drawing.Point(540, 192);
            this.VentasCboVendedor.Name = "VentasCboVendedor";
            this.VentasCboVendedor.Size = new System.Drawing.Size(170, 31);
            this.VentasCboVendedor.TabIndex = 4;
            this.VentasCboVendedor.DropDownWidth = 310;
            //
            // VentasLblVendedor
            //
            this.VentasLblVendedor.AutoSize = true;
            this.VentasLblVendedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblVendedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblVendedor.Location = new System.Drawing.Point(540, 172);
            this.VentasLblVendedor.Name = "VentasLblVendedor";
            this.VentasLblVendedor.Size = new System.Drawing.Size(79, 18);
            this.VentasLblVendedor.TabIndex = 10;
            this.VentasLblVendedor.Text = "Vendedor";
            //
            // VentasDtpEntrega
            //
            this.VentasDtpEntrega.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpEntrega.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpEntrega.Location = new System.Drawing.Point(375, 260);
            this.VentasDtpEntrega.Name = "VentasDtpEntrega";
            this.VentasDtpEntrega.Size = new System.Drawing.Size(310, 30);
            this.VentasDtpEntrega.TabIndex = 9;
            //
            // VentasLblEntrega
            //
            this.VentasLblEntrega.AutoSize = true;
            this.VentasLblEntrega.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblEntrega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblEntrega.Location = new System.Drawing.Point(375, 239);
            this.VentasLblEntrega.Name = "VentasLblEntrega";
            this.VentasLblEntrega.Size = new System.Drawing.Size(136, 18);
            this.VentasLblEntrega.TabIndex = 8;
            this.VentasLblEntrega.Text = "Fecha de entrega";
            //
            // VentasDtpVencimiento
            //
            this.VentasDtpVencimiento.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpVencimiento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpVencimiento.Location = new System.Drawing.Point(724, 260);
            this.VentasDtpVencimiento.Name = "VentasDtpVencimiento";
            this.VentasDtpVencimiento.Size = new System.Drawing.Size(310, 30);
            //
            // VentasLblVencimiento
            //
            this.VentasLblVencimiento.AutoSize = true;
            this.VentasLblVencimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblVencimiento.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblVencimiento.Location = new System.Drawing.Point(724, 239);
            this.VentasLblVencimiento.Name = "VentasLblVencimiento";
            this.VentasLblVencimiento.Text = "Vencimiento del pedido";
            //
            // VentasDtpPedido
            //
            this.VentasDtpPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpPedido.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpPedido.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpPedido.Location = new System.Drawing.Point(26, 260);
            this.VentasDtpPedido.Name = "VentasDtpPedido";
            this.VentasDtpPedido.Size = new System.Drawing.Size(310, 30);
            this.VentasDtpPedido.TabIndex = 5;
            //
            // VentasLblFechaPedido
            //
            this.VentasLblFechaPedido.AutoSize = true;
            this.VentasLblFechaPedido.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblFechaPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblFechaPedido.Location = new System.Drawing.Point(26, 239);
            this.VentasLblFechaPedido.Name = "VentasLblFechaPedido";
            this.VentasLblFechaPedido.Size = new System.Drawing.Size(128, 18);
            this.VentasLblFechaPedido.TabIndex = 4;
            this.VentasLblFechaPedido.Text = "Fecha de pedido";
            //
            // VentasUsrCliente
            //
            this.VentasUsrCliente.Location = new System.Drawing.Point(26, 102);
            this.VentasUsrCliente.Name = "VentasUsrCliente";
            this.VentasUsrCliente.Size = new System.Drawing.Size(500, 70);
            this.VentasUsrCliente.TabIndex = 2;
            //
            // VentasTxtNumeroPedido
            //
            this.VentasTxtNumeroPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTxtNumeroPedido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VentasTxtNumeroPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtNumeroPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(96)))), ((int)(((byte)(111)))));
            this.VentasTxtNumeroPedido.Location = new System.Drawing.Point(540, 63);
            this.VentasTxtNumeroPedido.Name = "VentasTxtNumeroPedido";
            this.VentasTxtNumeroPedido.ReadOnly = true;
            this.VentasTxtNumeroPedido.Size = new System.Drawing.Size(494, 31);
            this.VentasTxtNumeroPedido.TabStop = false;
            this.VentasTxtNumeroPedido.TabIndex = 1;
            this.VentasTxtNumeroPedido.Text = "Se asignará al guardar";
            //
            // VentasLblNumeroPedido
            //
            this.VentasLblNumeroPedido.AutoSize = true;
            this.VentasLblNumeroPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblNumeroPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblNumeroPedido.Location = new System.Drawing.Point(540, 42);
            this.VentasLblNumeroPedido.Name = "VentasLblNumeroPedido";
            this.VentasLblNumeroPedido.Size = new System.Drawing.Size(143, 18);
            this.VentasLblNumeroPedido.TabIndex = 0;
            this.VentasLblNumeroPedido.Text = "Referencia del pedido (automática)";
            //
            // VentasLblDatosTitulo
            //
            this.VentasLblDatosTitulo.AutoSize = true;
            this.VentasLblDatosTitulo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblDatosTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblDatosTitulo.Location = new System.Drawing.Point(26, 10);
            this.VentasLblDatosTitulo.Name = "VentasLblDatosTitulo";
            this.VentasLblDatosTitulo.Size = new System.Drawing.Size(171, 23);
            this.VentasLblDatosTitulo.TabIndex = 12;
            this.VentasLblDatosTitulo.Text = "Tipo, referencia y datos del pedido";
            //
            // FrmPedidoVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1120, 800);
            this.Controls.Add(this.VentasPnlContenido);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("IconoVentas")));
            this.MaximizeBox = false;
            this.Name = "FrmPedidoVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7004 – PedidoVentas";
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            this.VentasPnlContenido.ResumeLayout(false);
            this.VentasPnlResumen.ResumeLayout(false);
            this.VentasPnlResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalle)).EndInit();
            this.VentasPnlEncabezadoDetalle.ResumeLayout(false);
            this.VentasPnlEncabezadoDetalle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudCantidadProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudDescuentoProducto)).EndInit();
            this.VentasPnlDatosPedido.ResumeLayout(false);
            this.VentasPnlDatosPedido.PerformLayout();
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnGuardar, "Guardar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnCancelar, "Cancelar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnEliminarProducto, "Eliminar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnIngresarProducto, "Ingresar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnNuevoPedido, "Crear otro pedido");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnConsultarPedido, "Consultar pedidos");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnModificarPedido, "Modificar este pedido");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAnularPedido, "Anular este pedido");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAyuda, "Ayuda de pedidos (F1)");
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblCodigo;
        private System.Windows.Forms.Panel VentasPnlContenido;
        private System.Windows.Forms.Panel VentasPnlDatosPedido;
        private System.Windows.Forms.Label VentasLblDatosTitulo;
        private System.Windows.Forms.Label VentasLblNumeroPedido;
        private System.Windows.Forms.TextBox VentasTxtNumeroPedido;
        private VentasUsrCliente VentasUsrCliente;
        private System.Windows.Forms.Label VentasLblFechaPedido;
        private System.Windows.Forms.DateTimePicker VentasDtpPedido;
        private System.Windows.Forms.Label VentasLblEntrega;
        private System.Windows.Forms.DateTimePicker VentasDtpEntrega;
        private System.Windows.Forms.DateTimePicker VentasDtpVencimiento;
        private System.Windows.Forms.Label VentasLblVencimiento;
        private System.Windows.Forms.Label VentasLblVendedor;
        private System.Windows.Forms.ComboBox VentasCboVendedor;
        private System.Windows.Forms.Panel VentasPnlEncabezadoDetalle;
        private System.Windows.Forms.ComboBox VentasCboProductoPedido;
        private System.Windows.Forms.NumericUpDown VentasNudCantidadProducto;
        private System.Windows.Forms.NumericUpDown VentasNudDescuentoProducto;
        private System.Windows.Forms.Label VentasLblCantidadProducto;
        private System.Windows.Forms.Label VentasLblDescuentoProducto;
        private System.Windows.Forms.Label VentasLblPrecioListaProducto;
        private System.Windows.Forms.TextBox VentasTxtPrecioListaProducto;
        private System.Windows.Forms.Label VentasLblDetalleTitulo;
        private System.Windows.Forms.Label VentasLblDetalleAyuda;
        private System.Windows.Forms.DataGridView VentasDgvDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColDescuento;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColTotal;
        private System.Windows.Forms.Panel VentasPnlResumen;
        private System.Windows.Forms.Label VentasLblResumenTitulo;
        private System.Windows.Forms.Label VentasLblSubtotal;
        private System.Windows.Forms.Label VentasLblDescuentoTotal;
        private System.Windows.Forms.Label VentasLblTotal;
        private System.Windows.Forms.Button VentasBtnGuardar;
        private System.Windows.Forms.Button VentasBtnCancelar;
        private System.Windows.Forms.Button VentasBtnNuevoPedido;
        private System.Windows.Forms.Button VentasBtnConsultarPedido;
        private System.Windows.Forms.Button VentasBtnModificarPedido;
        private System.Windows.Forms.Button VentasBtnAnularPedido;
        private System.Windows.Forms.Button VentasBtnAyuda;
        private System.Windows.Forms.Button VentasBtnIngresarProducto;
        private System.Windows.Forms.Button VentasBtnEliminarProducto;
        private System.Windows.Forms.Label VentasLblOrigenPedido;
        private System.Windows.Forms.RadioButton VentasRdoPedidoPorCotizacion;
        private System.Windows.Forms.RadioButton VentasRdoPedidoIndependiente;
        private System.Windows.Forms.Label VentasLblCotizacionOrigen;
        private System.Windows.Forms.ComboBox VentasCboCotizacionOrigen;
        private System.Windows.Forms.Label VentasLblEstadoPedido;
        private System.Windows.Forms.ComboBox VentasCboEstadoPedido;
        private System.Windows.Forms.Label VentasLblMonedaPedido;
        private System.Windows.Forms.TextBox VentasTxtMonedaPedido;
        private System.Windows.Forms.Label VentasLblListaPrecioPedido;
        private System.Windows.Forms.ComboBox VentasCboListaPrecioPedido;
        private System.Windows.Forms.Label VentasLblIdentificacionCliente;
        private System.Windows.Forms.TextBox VentasTxtIdentificacionCliente;
        private System.Windows.Forms.Label VentasLblContactoCliente;
        private System.Windows.Forms.TextBox VentasTxtContactoCliente;
        private System.Windows.Forms.Label VentasLblDireccionCliente;
        private System.Windows.Forms.TextBox VentasTxtDireccionCliente;

        private System.Windows.Forms.ToolTip VentasTtpAcciones;
    }
}

