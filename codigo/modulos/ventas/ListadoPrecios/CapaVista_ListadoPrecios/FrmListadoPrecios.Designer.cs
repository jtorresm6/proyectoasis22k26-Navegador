/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Diego
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Diego Alejandro Cheng Peña
 * Fecha de actualización: 14/10/2026
 * Propósito: Conservar el diseño de Listado de precios con botones estándar de Ventas.
 */
namespace CapaVista_ListadoPrecios
{
    partial class FrmListadoPrecios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this.components);
            this.VentasBtnGuardar = new System.Windows.Forms.Button();
            this.VentasBtnCancelar = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasPbMarca = new System.Windows.Forms.PictureBox();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasCboTipoCliente = new System.Windows.Forms.ComboBox();
            this.VentasLblTipoCliente = new System.Windows.Forms.Label();
            this.VentasLblPorcentajeDescuento = new System.Windows.Forms.Label();
            this.VentasNudPorcentajeDescuento = new System.Windows.Forms.NumericUpDown();
            this.VentasLblSimboloPorcentaje = new System.Windows.Forms.Label();
            this.VentasLblPrecioBase = new System.Windows.Forms.Label();
            this.VentasTxtPrecioBase = new System.Windows.Forms.TextBox();
            this.VentasLblPrecioFinal = new System.Windows.Forms.Label();
            this.VentasTxtPrecioFinal = new System.Windows.Forms.TextBox();
            this.VentasLblListadoClientes = new System.Windows.Forms.Label();
            this.VentasDgvListadoClientes = new System.Windows.Forms.DataGridView();
            this.VentasLblTipoProducto = new System.Windows.Forms.Label();
            this.VentasCboTipoProducto = new System.Windows.Forms.ComboBox();
            this.VentasClmTipoCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmTipoProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPrecioBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPrecioFinal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAcciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasBtnConsultar = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasPbMarca)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudPorcentajeDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvListadoClientes)).BeginInit();
            this.SuspendLayout();
            // 
            // VentasBtnGuardar
            // 
            this.VentasBtnGuardar.AccessibleDescription = "Guardar";
            this.VentasBtnGuardar.AccessibleName = "Guardar";
            this.VentasBtnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnGuardar.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalGuardarVentas;
            this.VentasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VentasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.VentasBtnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnGuardar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnGuardar.Location = new System.Drawing.Point(12, 366);
            this.VentasBtnGuardar.Name = "VentasBtnGuardar";
            this.VentasBtnGuardar.Size = new System.Drawing.Size(63, 64);
            this.VentasBtnGuardar.TabIndex = 17;
            this.VentasBtnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnGuardar, "Guardar");
            this.VentasBtnGuardar.UseVisualStyleBackColor = false;
            // 
            // VentasBtnCancelar
            // 
            this.VentasBtnCancelar.AccessibleDescription = "Cancelar";
            this.VentasBtnCancelar.AccessibleName = "Cancelar";
            this.VentasBtnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnCancelar.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalCancelarVentas;
            this.VentasBtnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VentasBtnCancelar.FlatAppearance.BorderSize = 0;
            this.VentasBtnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnCancelar.Location = new System.Drawing.Point(11, 436);
            this.VentasBtnCancelar.Name = "VentasBtnCancelar";
            this.VentasBtnCancelar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCancelar.TabIndex = 6;
            this.VentasBtnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnCancelar, "Cancelar");
            this.VentasBtnCancelar.UseVisualStyleBackColor = false;
            // 
            // VentasPnlEncabezado
            // 
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasPbMarca);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1138, 78);
            this.VentasPnlEncabezado.TabIndex = 1;
            // 
            // VentasPbMarca
            // 
            this.VentasPbMarca.Image = global::CapaVista_ListadoPrecios.Properties.Resources.IconoVentasImagen;
            this.VentasPbMarca.Location = new System.Drawing.Point(20, 13);
            this.VentasPbMarca.Name = "VentasPbMarca";
            this.VentasPbMarca.Size = new System.Drawing.Size(52, 52);
            this.VentasPbMarca.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.VentasPbMarca.TabIndex = 1;
            this.VentasPbMarca.TabStop = false;
            // 
            // VentasLblTitulo
            // 
            this.VentasLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasLblTitulo.Location = new System.Drawing.Point(87, 13);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(570, 35);
            this.VentasLblTitulo.TabIndex = 2;
            this.VentasLblTitulo.Text = "Listado de precios";
            // 
            // VentasLblSubtitulo
            // 
            this.VentasLblSubtitulo.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(89, 48);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(575, 22);
            this.VentasLblSubtitulo.TabIndex = 3;
            this.VentasLblSubtitulo.Text = "Tipos de cliente, descuentos y precios";
            // 
            // VentasCboTipoCliente
            // 
            this.VentasCboTipoCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboTipoCliente.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboTipoCliente.FormattingEnabled = true;
            this.VentasCboTipoCliente.Items.AddRange(new object[] {
            "Mayorista",
            "Minorista",
            "Público",
            "Canal 1",
            "Canal 2",
            "Canal 3",
            "Otros"});
            this.VentasCboTipoCliente.Location = new System.Drawing.Point(20, 114);
            this.VentasCboTipoCliente.Name = "VentasCboTipoCliente";
            this.VentasCboTipoCliente.Size = new System.Drawing.Size(292, 38);
            this.VentasCboTipoCliente.TabIndex = 2;
            // 
            // VentasLblTipoCliente
            // 
            this.VentasLblTipoCliente.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblTipoCliente.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblTipoCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTipoCliente.Location = new System.Drawing.Point(33, 81);
            this.VentasLblTipoCliente.Name = "VentasLblTipoCliente";
            this.VentasLblTipoCliente.Size = new System.Drawing.Size(152, 30);
            this.VentasLblTipoCliente.TabIndex = 7;
            this.VentasLblTipoCliente.Text = "Tipo de cliente";
            // 
            // VentasLblPorcentajeDescuento
            // 
            this.VentasLblPorcentajeDescuento.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblPorcentajeDescuento.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblPorcentajeDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblPorcentajeDescuento.Location = new System.Drawing.Point(16, 206);
            this.VentasLblPorcentajeDescuento.Name = "VentasLblPorcentajeDescuento";
            this.VentasLblPorcentajeDescuento.Size = new System.Drawing.Size(237, 30);
            this.VentasLblPorcentajeDescuento.TabIndex = 8;
            this.VentasLblPorcentajeDescuento.Text = "Porcentaje de descuento";
            // 
            // VentasNudPorcentajeDescuento
            // 
            this.VentasNudPorcentajeDescuento.DecimalPlaces = 2;
            this.VentasNudPorcentajeDescuento.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasNudPorcentajeDescuento.Location = new System.Drawing.Point(57, 239);
            this.VentasNudPorcentajeDescuento.Name = "VentasNudPorcentajeDescuento";
            this.VentasNudPorcentajeDescuento.Size = new System.Drawing.Size(126, 37);
            this.VentasNudPorcentajeDescuento.TabIndex = 9;
            this.VentasNudPorcentajeDescuento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // VentasLblSimboloPorcentaje
            // 
            this.VentasLblSimboloPorcentaje.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblSimboloPorcentaje.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblSimboloPorcentaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblSimboloPorcentaje.Location = new System.Drawing.Point(189, 245);
            this.VentasLblSimboloPorcentaje.Name = "VentasLblSimboloPorcentaje";
            this.VentasLblSimboloPorcentaje.Size = new System.Drawing.Size(46, 40);
            this.VentasLblSimboloPorcentaje.TabIndex = 10;
            this.VentasLblSimboloPorcentaje.Text = "%";
            this.VentasLblSimboloPorcentaje.Click += new System.EventHandler(this.VentasMetLblSimboloPorcentajeClick);
            // 
            // VentasLblPrecioBase
            // 
            this.VentasLblPrecioBase.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblPrecioBase.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblPrecioBase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblPrecioBase.Location = new System.Drawing.Point(317, 208);
            this.VentasLblPrecioBase.Name = "VentasLblPrecioBase";
            this.VentasLblPrecioBase.Size = new System.Drawing.Size(114, 30);
            this.VentasLblPrecioBase.TabIndex = 11;
            this.VentasLblPrecioBase.Text = "Precio base";
            // 
            // VentasTxtPrecioBase
            // 
            this.VentasTxtPrecioBase.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasTxtPrecioBase.Location = new System.Drawing.Point(321, 239);
            this.VentasTxtPrecioBase.Name = "VentasTxtPrecioBase";
            this.VentasTxtPrecioBase.Size = new System.Drawing.Size(197, 37);
            this.VentasTxtPrecioBase.TabIndex = 12;
            this.VentasTxtPrecioBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // VentasLblPrecioFinal
            // 
            this.VentasLblPrecioFinal.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblPrecioFinal.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblPrecioFinal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblPrecioFinal.Location = new System.Drawing.Point(574, 206);
            this.VentasLblPrecioFinal.Name = "VentasLblPrecioFinal";
            this.VentasLblPrecioFinal.Size = new System.Drawing.Size(114, 30);
            this.VentasLblPrecioFinal.TabIndex = 13;
            this.VentasLblPrecioFinal.Text = "Precio final";
            // 
            // VentasTxtPrecioFinal
            // 
            this.VentasTxtPrecioFinal.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasTxtPrecioFinal.Location = new System.Drawing.Point(578, 238);
            this.VentasTxtPrecioFinal.Name = "VentasTxtPrecioFinal";
            this.VentasTxtPrecioFinal.Size = new System.Drawing.Size(200, 37);
            this.VentasTxtPrecioFinal.TabIndex = 14;
            this.VentasTxtPrecioFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // VentasLblListadoClientes
            // 
            this.VentasLblListadoClientes.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblListadoClientes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.VentasLblListadoClientes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblListadoClientes.Location = new System.Drawing.Point(89, 336);
            this.VentasLblListadoClientes.Name = "VentasLblListadoClientes";
            this.VentasLblListadoClientes.Size = new System.Drawing.Size(422, 30);
            this.VentasLblListadoClientes.TabIndex = 15;
            this.VentasLblListadoClientes.Text = "Listado de precios por tipo de cliente";
            this.VentasLblListadoClientes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VentasDgvListadoClientes
            // 
            this.VentasDgvListadoClientes.AllowUserToAddRows = false;
            this.VentasDgvListadoClientes.AllowUserToDeleteRows = false;
            this.VentasDgvListadoClientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvListadoClientes.BackgroundColor = System.Drawing.Color.White;
            this.VentasDgvListadoClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvListadoClientes.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1;
            this.VentasDgvListadoClientes.ColumnHeadersHeight = 34;
            this.VentasDgvListadoClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.VentasDgvListadoClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmTipoCliente,
            this.VentasClmTipoProducto,
            this.VentasClmPorcentaje,
            this.VentasClmPrecioBase,
            this.VentasClmPrecioFinal,
            this.VentasClmAcciones});
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            DataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvListadoClientes.DefaultCellStyle = DataGridViewCellStyle2;
            this.VentasDgvListadoClientes.EnableHeadersVisualStyles = false;
            this.VentasDgvListadoClientes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDgvListadoClientes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvListadoClientes.Location = new System.Drawing.Point(93, 369);
            this.VentasDgvListadoClientes.MultiSelect = false;
            this.VentasDgvListadoClientes.Name = "VentasDgvListadoClientes";
            this.VentasDgvListadoClientes.ReadOnly = true;
            this.VentasDgvListadoClientes.RowHeadersVisible = false;
            this.VentasDgvListadoClientes.RowHeadersWidth = 62;
            this.VentasDgvListadoClientes.RowTemplate.Height = 28;
            this.VentasDgvListadoClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvListadoClientes.Size = new System.Drawing.Size(1029, 134);
            this.VentasDgvListadoClientes.TabIndex = 16;
            // 
            // VentasLblTipoProducto
            // 
            this.VentasLblTipoProducto.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblTipoProducto.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblTipoProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTipoProducto.Location = new System.Drawing.Point(383, 81);
            this.VentasLblTipoProducto.Name = "VentasLblTipoProducto";
            this.VentasLblTipoProducto.Size = new System.Drawing.Size(174, 30);
            this.VentasLblTipoProducto.TabIndex = 18;
            this.VentasLblTipoProducto.Text = "Tipo de producto";
            // 
            // comboBox1
            // 
            this.VentasCboTipoProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboTipoProducto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboTipoProducto.FormattingEnabled = true;
            this.VentasCboTipoProducto.Items.AddRange(new object[] {
            "Producto A",
            "Producto B",
            "Producto C"});
            this.VentasCboTipoProducto.Location = new System.Drawing.Point(387, 114);
            this.VentasCboTipoProducto.Name = "VentasCboTipoProducto";
            this.VentasCboTipoProducto.Size = new System.Drawing.Size(292, 38);
            this.VentasCboTipoProducto.TabIndex = 19;
            // 
            // VentasClmTipoCliente
            // 
            this.VentasClmTipoCliente.HeaderText = "Cliente";
            this.VentasClmTipoCliente.MinimumWidth = 8;
            this.VentasClmTipoCliente.Name = "VentasClmTipoCliente";
            this.VentasClmTipoCliente.ReadOnly = true;
            // 
            // VentasClmTipoProducto
            // 
            this.VentasClmTipoProducto.HeaderText = "Producto";
            this.VentasClmTipoProducto.MinimumWidth = 8;
            this.VentasClmTipoProducto.Name = "VentasClmTipoProducto";
            this.VentasClmTipoProducto.ReadOnly = true;
            // 
            // VentasClmPorcentaje
            // 
            this.VentasClmPorcentaje.HeaderText = "Descuento (%)";
            this.VentasClmPorcentaje.MinimumWidth = 8;
            this.VentasClmPorcentaje.Name = "VentasClmPorcentaje";
            this.VentasClmPorcentaje.ReadOnly = true;
            // 
            // VentasClmPrecioBase
            // 
            this.VentasClmPrecioBase.HeaderText = "Precio base";
            this.VentasClmPrecioBase.MinimumWidth = 8;
            this.VentasClmPrecioBase.Name = "VentasClmPrecioBase";
            this.VentasClmPrecioBase.ReadOnly = true;
            // 
            // VentasClmPrecioFinal
            // 
            this.VentasClmPrecioFinal.HeaderText = "Precio final";
            this.VentasClmPrecioFinal.MinimumWidth = 8;
            this.VentasClmPrecioFinal.Name = "VentasClmPrecioFinal";
            this.VentasClmPrecioFinal.ReadOnly = true;
            // 
            // VentasClmAcciones
            // 
            this.VentasClmAcciones.HeaderText = "Acciones";
            this.VentasClmAcciones.MinimumWidth = 8;
            this.VentasClmAcciones.Name = "VentasClmAcciones";
            this.VentasClmAcciones.ReadOnly = true;
            // 
            // VentasBtnConsultar
            // 
            this.VentasBtnConsultar.AccessibleDescription = "Consultar listado de precios";
            this.VentasBtnConsultar.AccessibleName = "Consultar listado de precios";
            this.VentasBtnConsultar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnConsultar.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalConsultarVentas;
            this.VentasBtnConsultar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnConsultar.FlatAppearance.BorderSize = 0;
            this.VentasBtnConsultar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnConsultar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnConsultar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnConsultar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnConsultar.Location = new System.Drawing.Point(800, 88);
            this.VentasBtnConsultar.Name = "VentasBtnConsultar";
            this.VentasBtnConsultar.Size = new System.Drawing.Size(58, 64);
            this.VentasBtnConsultar.TabIndex = 20;
            this.VentasBtnConsultar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnConsultar, "Consultar listado de precios");
            this.VentasBtnConsultar.UseVisualStyleBackColor = false;
            // 
            // FrmListadoPrecios
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1112, 512);
            this.Controls.Add(this.VentasBtnConsultar);
            this.Controls.Add(this.VentasCboTipoProducto);
            this.Controls.Add(this.VentasLblTipoProducto);
            this.Controls.Add(this.VentasBtnCancelar);
            this.Controls.Add(this.VentasBtnGuardar);
            this.Controls.Add(this.VentasDgvListadoClientes);
            this.Controls.Add(this.VentasLblListadoClientes);
            this.Controls.Add(this.VentasTxtPrecioFinal);
            this.Controls.Add(this.VentasLblPrecioFinal);
            this.Controls.Add(this.VentasTxtPrecioBase);
            this.Controls.Add(this.VentasLblPrecioBase);
            this.Controls.Add(this.VentasLblSimboloPorcentaje);
            this.Controls.Add(this.VentasNudPorcentajeDescuento);
            this.Controls.Add(this.VentasLblPorcentajeDescuento);
            this.Controls.Add(this.VentasLblTipoCliente);
            this.Controls.Add(this.VentasCboTipoCliente);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_ListadoPrecios.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmListadoPrecios";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7002 – ListadoDePrecios";
            this.VentasPnlEncabezado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VentasPbMarca)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudPorcentajeDescuento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvListadoClientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.PictureBox VentasPbMarca;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.ComboBox VentasCboTipoCliente;
        private System.Windows.Forms.Label VentasLblTipoCliente;
        private System.Windows.Forms.Label VentasLblPorcentajeDescuento;
        private System.Windows.Forms.NumericUpDown VentasNudPorcentajeDescuento;
        private System.Windows.Forms.Label VentasLblSimboloPorcentaje;
        private System.Windows.Forms.Label VentasLblPrecioBase;
        private System.Windows.Forms.TextBox VentasTxtPrecioBase;
        private System.Windows.Forms.Label VentasLblPrecioFinal;
        private System.Windows.Forms.TextBox VentasTxtPrecioFinal;
        private System.Windows.Forms.Label VentasLblListadoClientes;
        private System.Windows.Forms.DataGridView VentasDgvListadoClientes;
        private System.Windows.Forms.Button VentasBtnGuardar;
        private System.Windows.Forms.Button VentasBtnCancelar;
        private System.Windows.Forms.ToolTip VentasTtpAccionesOriginales;
        private System.Windows.Forms.Label VentasLblTipoProducto;
        private System.Windows.Forms.ComboBox VentasCboTipoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmTipoCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmTipoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPrecioBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPrecioFinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAcciones;
        private System.Windows.Forms.Button VentasBtnConsultar;
    }
}
// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 14/10/2026
