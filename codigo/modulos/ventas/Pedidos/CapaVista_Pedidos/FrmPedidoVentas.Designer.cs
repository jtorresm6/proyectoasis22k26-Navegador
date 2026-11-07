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
namespace CapaVista_Pedidos
{
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
            this.VentasPnlResumen = new System.Windows.Forms.Panel();
            this.VentasLblTotal = new System.Windows.Forms.Label();
            this.VentasLblImpuesto = new System.Windows.Forms.Label();
            this.VentasLblSubtotal = new System.Windows.Forms.Label();
            this.VentasLblResumenTitulo = new System.Windows.Forms.Label();
            this.VentasDgvDetalle = new System.Windows.Forms.DataGridView();
            this.VentasDgvDetalleColCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColDisponibilidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColDescuento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasDgvDetalleColTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasPnlEncabezadoDetalle = new System.Windows.Forms.Panel();
            this.VentasLblDetalleAyuda = new System.Windows.Forms.Label();
            this.VentasBtnEliminarProducto = new System.Windows.Forms.Button();
            this.VentasBtnIngresarProducto = new System.Windows.Forms.Button();
            this.VentasLblDetalleTitulo = new System.Windows.Forms.Label();
            this.VentasPnlDatosPedido = new System.Windows.Forms.Panel();
            this.VentasCboCotizacionOrigen = new System.Windows.Forms.ComboBox();
            this.VentasLblCotizacionOrigen = new System.Windows.Forms.Label();
            this.VentasTxtReferenciaIndependiente = new System.Windows.Forms.TextBox();
            this.VentasLblReferenciaIndependiente = new System.Windows.Forms.Label();
            this.VentasChkPedidoIndependiente = new System.Windows.Forms.CheckBox();
            this.VentasChkPedidoPorCotizacion = new System.Windows.Forms.CheckBox();
            this.VentasLblOrigenPedido = new System.Windows.Forms.Label();
            this.VentasCboVendedor = new System.Windows.Forms.ComboBox();
            this.VentasLblVendedor = new System.Windows.Forms.Label();
            this.VentasDtpEntrega = new System.Windows.Forms.DateTimePicker();
            this.VentasLblEntrega = new System.Windows.Forms.Label();
            this.VentasDtpVencimiento = new System.Windows.Forms.DateTimePicker();
            this.VentasLblVencimiento = new System.Windows.Forms.Label();
            this.VentasDtpPedido = new System.Windows.Forms.DateTimePicker();
            this.VentasLblFechaPedido = new System.Windows.Forms.Label();
            this.VentasCboCliente = new System.Windows.Forms.ComboBox();
            this.VentasLblCliente = new System.Windows.Forms.Label();
            this.VentasTxtNumeroPedido = new System.Windows.Forms.TextBox();
            this.VentasLblNumeroPedido = new System.Windows.Forms.Label();
            this.VentasLblDatosTitulo = new System.Windows.Forms.Label();
            this.VentasPnlEncabezado.SuspendLayout();
            this.VentasPnlContenido.SuspendLayout();
            this.VentasPnlResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalle)).BeginInit();
            this.VentasPnlEncabezadoDetalle.SuspendLayout();
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
            this.VentasLblCodigo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
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
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 19F, System.Drawing.FontStyle.Bold);
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
            this.VentasPnlContenido.Controls.Add(this.VentasPnlResumen);
            this.VentasPnlContenido.Controls.Add(this.VentasDgvDetalle);
            this.VentasPnlContenido.Controls.Add(this.VentasPnlEncabezadoDetalle);
            this.VentasPnlContenido.Controls.Add(this.VentasPnlDatosPedido);
            this.VentasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasPnlContenido.Location = new System.Drawing.Point(0, 80);
            this.VentasPnlContenido.Name = "VentasPnlContenido";
            this.VentasPnlContenido.Padding = new System.Windows.Forms.Padding(26, 20, 26, 18);
            this.VentasPnlContenido.Size = new System.Drawing.Size(1120, 600);
            this.VentasPnlContenido.TabIndex = 1;
            //
            // VentasBtnGuardar
            //
            this.VentasBtnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

            this.VentasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnGuardar.Location = new System.Drawing.Point(950, 508);
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
            this.VentasBtnGuardar.Click += new System.EventHandler(this.VentasMetBtnGuardarClick);

            this.VentasBtnGuardar.AccessibleName = "Guardar";
            //
            // VentasBtnCancelar
            //
            this.VentasBtnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));

            this.VentasBtnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;

            this.VentasBtnCancelar.Location = new System.Drawing.Point(1026, 508);
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
            this.VentasBtnCancelar.Click += new System.EventHandler(this.VentasMetBtnCancelarClick);

            this.VentasBtnCancelar.AccessibleName = "Cancelar";
            //
            // VentasPnlResumen
            //
            this.VentasPnlResumen.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.VentasPnlResumen.BackColor = System.Drawing.Color.White;
            this.VentasPnlResumen.Controls.Add(this.VentasLblTotal);
            this.VentasPnlResumen.Controls.Add(this.VentasLblImpuesto);
            this.VentasPnlResumen.Controls.Add(this.VentasLblSubtotal);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenTitulo);
            this.VentasPnlResumen.Location = new System.Drawing.Point(575, 486);
            this.VentasPnlResumen.Name = "VentasPnlResumen";
            this.VentasPnlResumen.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.VentasPnlResumen.Size = new System.Drawing.Size(340, 108);
            this.VentasPnlResumen.TabIndex = 5;
            //
            // VentasLblTotal
            //
            this.VentasLblTotal.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.VentasLblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTotal.Location = new System.Drawing.Point(16, 62);
            this.VentasLblTotal.Name = "VentasLblTotal";
            this.VentasLblTotal.Size = new System.Drawing.Size(302, 36);
            this.VentasLblTotal.TabIndex = 3;
            this.VentasLblTotal.Text = "TOTAL DEL PEDIDO     Q 0.00";
            this.VentasLblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblImpuesto
            //
            this.VentasLblImpuesto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasLblImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(93)))), ((int)(((byte)(103)))));
            this.VentasLblImpuesto.Location = new System.Drawing.Point(16, 43);
            this.VentasLblImpuesto.Name = "VentasLblImpuesto";
            this.VentasLblImpuesto.Size = new System.Drawing.Size(302, 18);
            this.VentasLblImpuesto.TabIndex = 2;
            this.VentasLblImpuesto.Text = "Impuestos                         Q 0.00";
            this.VentasLblImpuesto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblSubtotal
            //
            this.VentasLblSubtotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasLblSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(93)))), ((int)(((byte)(103)))));
            this.VentasLblSubtotal.Location = new System.Drawing.Point(16, 25);
            this.VentasLblSubtotal.Name = "VentasLblSubtotal";
            this.VentasLblSubtotal.Size = new System.Drawing.Size(302, 18);
            this.VentasLblSubtotal.TabIndex = 1;
            this.VentasLblSubtotal.Text = "Subtotal                          Q 0.00";
            this.VentasLblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VentasLblResumenTitulo
            //
            this.VentasLblResumenTitulo.AutoSize = true;
            this.VentasLblResumenTitulo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
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
            this.VentasDgvDetalleColDisponibilidad,
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
            this.VentasDgvDetalle.Location = new System.Drawing.Point(30, 354);
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
            // VentasDgvDetalleColDisponibilidad
            //
            this.VentasDgvDetalleColDisponibilidad.FillWeight = 95F;
            this.VentasDgvDetalleColDisponibilidad.HeaderText = "DISPONIBILIDAD";
            this.VentasDgvDetalleColDisponibilidad.Name = "VentasDgvDetalleColDisponibilidad";
            this.VentasDgvDetalleColDisponibilidad.ReadOnly = true;
            this.VentasDgvDetalleColDisponibilidad.ToolTipText = "Pendiente de la fuente de inventario; se muestra en blanco.";
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
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasBtnEliminarProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasBtnIngresarProducto);
            this.VentasPnlEncabezadoDetalle.Controls.Add(this.VentasLblDetalleTitulo);
            this.VentasPnlEncabezadoDetalle.Location = new System.Drawing.Point(30, 290);
            this.VentasPnlEncabezadoDetalle.Name = "VentasPnlEncabezadoDetalle";
            this.VentasPnlEncabezadoDetalle.Padding = new System.Windows.Forms.Padding(14, 10, 14, 8);
            this.VentasPnlEncabezadoDetalle.Size = new System.Drawing.Size(1060, 64);
            this.VentasPnlEncabezadoDetalle.TabIndex = 1;
            //
            // VentasLblDetalleAyuda
            //
            this.VentasLblDetalleAyuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblDetalleAyuda.Font = new System.Drawing.Font("Segoe UI", 8.5F);
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
            this.VentasBtnEliminarProducto.Click += new System.EventHandler(this.VentasMetBtnEliminarProductoClick);

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
            this.VentasBtnIngresarProducto.Click += new System.EventHandler(this.VentasMetBtnIngresarProductoClick);

            //
            // VentasLblDetalleTitulo
            //
            this.VentasLblDetalleTitulo.AutoSize = true;
            this.VentasLblDetalleTitulo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
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
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblCotizacionOrigen);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtReferenciaIndependiente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblReferenciaIndependiente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasChkPedidoIndependiente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasChkPedidoPorCotizacion);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblOrigenPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboVendedor);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblVendedor);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpEntrega);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblEntrega);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpVencimiento);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblVencimiento);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasDtpPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblFechaPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasCboCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblCliente);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasTxtNumeroPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblNumeroPedido);
            this.VentasPnlDatosPedido.Controls.Add(this.VentasLblDatosTitulo);
            this.VentasPnlDatosPedido.Location = new System.Drawing.Point(30, 12);
            this.VentasPnlDatosPedido.Name = "VentasPnlDatosPedido";
            this.VentasPnlDatosPedido.Padding = new System.Windows.Forms.Padding(16, 12, 16, 14);
            this.VentasPnlDatosPedido.Size = new System.Drawing.Size(1060, 270);
            this.VentasPnlDatosPedido.TabIndex = 0;
            //
            // VentasCboCotizacionOrigen
            //
            this.VentasCboCotizacionOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboCotizacionOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboCotizacionOrigen.FormattingEnabled = true;
            this.VentasCboCotizacionOrigen.Items.AddRange(new object[] {
            "Seleccionar cotización",
            "COT-2026-014 · Cliente A",
            "COT-2026-018 · Cliente B"});
            this.VentasCboCotizacionOrigen.Location = new System.Drawing.Point(540, 99);
            this.VentasCboCotizacionOrigen.Name = "VentasCboCotizacionOrigen";
            this.VentasCboCotizacionOrigen.Size = new System.Drawing.Size(494, 30);
            this.VentasCboCotizacionOrigen.TabIndex = 23;
            this.VentasCboCotizacionOrigen.Visible = false;
            //
            // VentasLblCotizacionOrigen
            //
            this.VentasLblCotizacionOrigen.AutoSize = true;
            this.VentasLblCotizacionOrigen.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblCotizacionOrigen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblCotizacionOrigen.Location = new System.Drawing.Point(540, 78);
            this.VentasLblCotizacionOrigen.Name = "VentasLblCotizacionOrigen";
            this.VentasLblCotizacionOrigen.Size = new System.Drawing.Size(190, 18);
            this.VentasLblCotizacionOrigen.TabIndex = 22;
            this.VentasLblCotizacionOrigen.Text = "Cotización de referencia";
            this.VentasLblCotizacionOrigen.Visible = false;
            //
            // VentasTxtReferenciaIndependiente
            //
            this.VentasTxtReferenciaIndependiente.BackColor = System.Drawing.Color.White;
            this.VentasTxtReferenciaIndependiente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VentasTxtReferenciaIndependiente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtReferenciaIndependiente.Location = new System.Drawing.Point(26, 99);
            this.VentasTxtReferenciaIndependiente.Name = "VentasTxtReferenciaIndependiente";
            this.VentasTxtReferenciaIndependiente.Size = new System.Drawing.Size(500, 30);
            this.VentasTxtReferenciaIndependiente.TabIndex = 25;
            this.VentasTxtReferenciaIndependiente.Text = "Ingresar referencia propia";
            //
            // VentasLblReferenciaIndependiente
            //
            this.VentasLblReferenciaIndependiente.AutoSize = true;
            this.VentasLblReferenciaIndependiente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblReferenciaIndependiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblReferenciaIndependiente.Location = new System.Drawing.Point(26, 78);
            this.VentasLblReferenciaIndependiente.Name = "VentasLblReferenciaIndependiente";
            this.VentasLblReferenciaIndependiente.Size = new System.Drawing.Size(200, 18);
            this.VentasLblReferenciaIndependiente.TabIndex = 24;
            this.VentasLblReferenciaIndependiente.Text = "Referencia propia (opcional)";
            //
            // VentasChkPedidoIndependiente
            //
            this.VentasChkPedidoIndependiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasChkPedidoIndependiente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.VentasChkPedidoIndependiente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasChkPedidoIndependiente.Location = new System.Drawing.Point(245, 36);
            this.VentasChkPedidoIndependiente.Appearance = System.Windows.Forms.Appearance.Button;
            this.VentasChkPedidoIndependiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasChkPedidoIndependiente.FlatAppearance.BorderSize = 0;
            this.VentasChkPedidoIndependiente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasChkPedidoIndependiente.Name = "VentasChkPedidoIndependiente";
            this.VentasChkPedidoIndependiente.Size = new System.Drawing.Size(185, 34);
            this.VentasChkPedidoIndependiente.TabIndex = 21;
            this.VentasChkPedidoIndependiente.Text = "Independiente";
            this.VentasChkPedidoIndependiente.UseVisualStyleBackColor = false;
            //
            // VentasChkPedidoPorCotizacion
            //
            this.VentasChkPedidoPorCotizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasChkPedidoPorCotizacion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.VentasChkPedidoPorCotizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasChkPedidoPorCotizacion.Location = new System.Drawing.Point(442, 36);
            this.VentasChkPedidoPorCotizacion.Appearance = System.Windows.Forms.Appearance.Button;
            this.VentasChkPedidoPorCotizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasChkPedidoPorCotizacion.FlatAppearance.BorderSize = 0;
            this.VentasChkPedidoPorCotizacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasChkPedidoPorCotizacion.Name = "VentasChkPedidoPorCotizacion";
            this.VentasChkPedidoPorCotizacion.Size = new System.Drawing.Size(185, 34);
            this.VentasChkPedidoPorCotizacion.TabIndex = 20;
            this.VentasChkPedidoPorCotizacion.Text = "Por cotización";
            this.VentasChkPedidoPorCotizacion.UseVisualStyleBackColor = false;
            //
            // VentasLblOrigenPedido
            //
            this.VentasLblOrigenPedido.AutoSize = true;
            this.VentasLblOrigenPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblOrigenPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblOrigenPedido.Location = new System.Drawing.Point(26, 46);
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
            this.VentasCboVendedor.Items.AddRange(new object[] {
            "Seleccionar vendedor"});
            this.VentasCboVendedor.Location = new System.Drawing.Point(670, 169);
            this.VentasCboVendedor.Name = "VentasCboVendedor";
            this.VentasCboVendedor.Size = new System.Drawing.Size(364, 31);
            this.VentasCboVendedor.TabIndex = 11;
            //
            // VentasLblVendedor
            //
            this.VentasLblVendedor.AutoSize = true;
            this.VentasLblVendedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblVendedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblVendedor.Location = new System.Drawing.Point(670, 148);
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
            this.VentasDtpEntrega.Location = new System.Drawing.Point(375, 229);
            this.VentasDtpEntrega.Name = "VentasDtpEntrega";
            this.VentasDtpEntrega.Size = new System.Drawing.Size(310, 30);
            this.VentasDtpEntrega.TabIndex = 9;
            //
            // VentasLblEntrega
            //
            this.VentasLblEntrega.AutoSize = true;
            this.VentasLblEntrega.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblEntrega.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblEntrega.Location = new System.Drawing.Point(375, 208);
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
            this.VentasDtpVencimiento.Location = new System.Drawing.Point(724, 229);
            this.VentasDtpVencimiento.Name = "VentasDtpVencimiento";
            this.VentasDtpVencimiento.Size = new System.Drawing.Size(310, 30);
            //
            // VentasLblVencimiento
            //
            this.VentasLblVencimiento.AutoSize = true;
            this.VentasLblVencimiento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblVencimiento.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblVencimiento.Location = new System.Drawing.Point(724, 208);
            this.VentasLblVencimiento.Name = "VentasLblVencimiento";
            this.VentasLblVencimiento.Text = "Vencimiento del pedido";
            //
            // VentasDtpPedido
            //
            this.VentasDtpPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpPedido.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpPedido.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpPedido.Location = new System.Drawing.Point(26, 229);
            this.VentasDtpPedido.Name = "VentasDtpPedido";
            this.VentasDtpPedido.Size = new System.Drawing.Size(310, 30);
            this.VentasDtpPedido.TabIndex = 5;
            //
            // VentasLblFechaPedido
            //
            this.VentasLblFechaPedido.AutoSize = true;
            this.VentasLblFechaPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblFechaPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblFechaPedido.Location = new System.Drawing.Point(26, 208);
            this.VentasLblFechaPedido.Name = "VentasLblFechaPedido";
            this.VentasLblFechaPedido.Size = new System.Drawing.Size(128, 18);
            this.VentasLblFechaPedido.TabIndex = 4;
            this.VentasLblFechaPedido.Text = "Fecha de pedido";
            //
            // VentasCboCliente
            //
            this.VentasCboCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboCliente.FormattingEnabled = true;
            this.VentasCboCliente.Items.AddRange(new object[] {
            "Seleccionar cliente"});
            this.VentasCboCliente.Location = new System.Drawing.Point(300, 169);
            this.VentasCboCliente.Name = "VentasCboCliente";
            this.VentasCboCliente.Size = new System.Drawing.Size(340, 31);
            this.VentasCboCliente.TabIndex = 3;
            //
            // VentasLblCliente
            //
            this.VentasLblCliente.AutoSize = true;
            this.VentasLblCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblCliente.Location = new System.Drawing.Point(300, 148);
            this.VentasLblCliente.Name = "VentasLblCliente";
            this.VentasLblCliente.Size = new System.Drawing.Size(61, 18);
            this.VentasLblCliente.TabIndex = 2;
            this.VentasLblCliente.Text = "Cliente";
            //
            // VentasTxtNumeroPedido
            //
            this.VentasTxtNumeroPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTxtNumeroPedido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VentasTxtNumeroPedido.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtNumeroPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(96)))), ((int)(((byte)(111)))));
            this.VentasTxtNumeroPedido.Location = new System.Drawing.Point(26, 169);
            this.VentasTxtNumeroPedido.Name = "VentasTxtNumeroPedido";
            this.VentasTxtNumeroPedido.ReadOnly = true;
            this.VentasTxtNumeroPedido.Size = new System.Drawing.Size(245, 31);
            this.VentasTxtNumeroPedido.TabIndex = 1;
            this.VentasTxtNumeroPedido.Text = "Se asignará al guardar";
            //
            // VentasLblNumeroPedido
            //
            this.VentasLblNumeroPedido.AutoSize = true;
            this.VentasLblNumeroPedido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblNumeroPedido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblNumeroPedido.Location = new System.Drawing.Point(26, 148);
            this.VentasLblNumeroPedido.Name = "VentasLblNumeroPedido";
            this.VentasLblNumeroPedido.Size = new System.Drawing.Size(143, 18);
            this.VentasLblNumeroPedido.TabIndex = 0;
            this.VentasLblNumeroPedido.Text = "Número de pedido";
            //
            // VentasLblDatosTitulo
            //
            this.VentasLblDatosTitulo.AutoSize = true;
            this.VentasLblDatosTitulo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.VentasLblDatosTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblDatosTitulo.Location = new System.Drawing.Point(26, 10);
            this.VentasLblDatosTitulo.Name = "VentasLblDatosTitulo";
            this.VentasLblDatosTitulo.Size = new System.Drawing.Size(171, 23);
            this.VentasLblDatosTitulo.TabIndex = 12;
            this.VentasLblDatosTitulo.Text = "Datos del pedido";
            //
            // FrmPedidoVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1120, 680);
            this.Controls.Add(this.VentasPnlContenido);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("IconoVentas")));
            this.MaximizeBox = false;
            this.Name = "FrmPedidoVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7004 – Pedidos";
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            this.VentasPnlContenido.ResumeLayout(false);
            this.VentasPnlResumen.ResumeLayout(false);
            this.VentasPnlResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalle)).EndInit();
            this.VentasPnlEncabezadoDetalle.ResumeLayout(false);
            this.VentasPnlEncabezadoDetalle.PerformLayout();
            this.VentasPnlDatosPedido.ResumeLayout(false);
            this.VentasPnlDatosPedido.PerformLayout();
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnGuardar, "Guardar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnCancelar, "Cancelar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnEliminarProducto, "Eliminar");
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnIngresarProducto, "Ingresar");
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
        private System.Windows.Forms.Label VentasLblCliente;
        private System.Windows.Forms.ComboBox VentasCboCliente;
        private System.Windows.Forms.Label VentasLblFechaPedido;
        private System.Windows.Forms.DateTimePicker VentasDtpPedido;
        private System.Windows.Forms.Label VentasLblEntrega;
        private System.Windows.Forms.DateTimePicker VentasDtpEntrega;
        private System.Windows.Forms.DateTimePicker VentasDtpVencimiento;
        private System.Windows.Forms.Label VentasLblVencimiento;
        private System.Windows.Forms.Label VentasLblVendedor;
        private System.Windows.Forms.ComboBox VentasCboVendedor;
        private System.Windows.Forms.Panel VentasPnlEncabezadoDetalle;
        private System.Windows.Forms.Label VentasLblDetalleTitulo;
        private System.Windows.Forms.Label VentasLblDetalleAyuda;
        private System.Windows.Forms.DataGridView VentasDgvDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColDisponibilidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColDescuento;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasDgvDetalleColTotal;
        private System.Windows.Forms.Panel VentasPnlResumen;
        private System.Windows.Forms.Label VentasLblResumenTitulo;
        private System.Windows.Forms.Label VentasLblSubtotal;
        private System.Windows.Forms.Label VentasLblImpuesto;
        private System.Windows.Forms.Label VentasLblTotal;
        private System.Windows.Forms.Button VentasBtnGuardar;
        private System.Windows.Forms.Button VentasBtnCancelar;
        private System.Windows.Forms.Button VentasBtnIngresarProducto;
        private System.Windows.Forms.Button VentasBtnEliminarProducto;
        private System.Windows.Forms.Label VentasLblOrigenPedido;
        private System.Windows.Forms.CheckBox VentasChkPedidoPorCotizacion;
        private System.Windows.Forms.CheckBox VentasChkPedidoIndependiente;
        private System.Windows.Forms.Label VentasLblCotizacionOrigen;
        private System.Windows.Forms.ComboBox VentasCboCotizacionOrigen;
        private System.Windows.Forms.Label VentasLblReferenciaIndependiente;
        private System.Windows.Forms.TextBox VentasTxtReferenciaIndependiente;

        private System.Windows.Forms.ToolTip VentasTtpAcciones;
    }
}

