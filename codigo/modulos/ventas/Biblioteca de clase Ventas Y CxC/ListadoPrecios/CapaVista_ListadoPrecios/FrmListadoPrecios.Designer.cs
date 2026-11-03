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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this.components);
            this.VentasBtnGuardarListadoPrecios = new System.Windows.Forms.Button();
            this.VentasBtnCancelarListadoPrecios = new System.Windows.Forms.Button();
            this.VentasBtnConsultarListadoPrecios = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasPbMarca = new System.Windows.Forms.PictureBox();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasCboTipoCliente = new System.Windows.Forms.ComboBox();
            this.VentasLblTipoCliente = new System.Windows.Forms.Label();
            this.VentasLblTipoMod = new System.Windows.Forms.Label();
            this.VentasNudPorcentajeDescuento = new System.Windows.Forms.NumericUpDown();
            this.VentasLblSimboloPorcentaje = new System.Windows.Forms.Label();
            this.VentasLblCategoria = new System.Windows.Forms.Label();
            this.VentasLblValor = new System.Windows.Forms.Label();
            this.VentasLblListadoClientes = new System.Windows.Forms.Label();
            this.VentasDgvListadoClientes = new System.Windows.Forms.DataGridView();
            this.VentasClmTipoCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmProveedores = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmTipoProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPrecioBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPrecioFinal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasLblMarca = new System.Windows.Forms.Label();
            this.VentasCboTipoProducto = new System.Windows.Forms.ComboBox();
            this.VentasLbllinea = new System.Windows.Forms.Label();
            this.VentasCboLinea = new System.Windows.Forms.ComboBox();
            this.VentasCboCategoria = new System.Windows.Forms.ComboBox();
            this.VentasLblProveedores = new System.Windows.Forms.Label();
            this.VentasCboProveedores = new System.Windows.Forms.ComboBox();
            this.VentasLblProducto = new System.Windows.Forms.Label();
            this.VentasCboProducto = new System.Windows.Forms.ComboBox();
            this.VentasTxtLisPre = new System.Windows.Forms.TextBox();
            this.VentasLblAlcance = new System.Windows.Forms.Label();
            this.VentasGbxMod = new System.Windows.Forms.GroupBox();
            this.VentasRbtnAumBase = new System.Windows.Forms.RadioButton();
            this.VentasRbtnAumPor = new System.Windows.Forms.RadioButton();
            this.VentasRbtnDescuento = new System.Windows.Forms.RadioButton();
            this.VentasGbxAlc = new System.Windows.Forms.GroupBox();
            this.VentasRbtnTdsRslt = new System.Windows.Forms.RadioButton();
            this.VentasRbtnPrdSelec = new System.Windows.Forms.RadioButton();
            this.VentasRbtnPrdInd = new System.Windows.Forms.RadioButton();
            this.VentasBtnCotRefrescar = new System.Windows.Forms.Button();
            this.VentasClmAcciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasPnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasPbMarca)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasNudPorcentajeDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvListadoClientes)).BeginInit();
            this.VentasGbxMod.SuspendLayout();
            this.VentasGbxAlc.SuspendLayout();
            this.SuspendLayout();
            // 
            // VentasBtnGuardarListadoPrecios
            // 
            this.VentasBtnGuardarListadoPrecios.AccessibleDescription = "Guardar";
            this.VentasBtnGuardarListadoPrecios.AccessibleName = "Guardar";
            this.VentasBtnGuardarListadoPrecios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnGuardarListadoPrecios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnGuardarListadoPrecios.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalGuardarVentas;
            this.VentasBtnGuardarListadoPrecios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnGuardarListadoPrecios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VentasBtnGuardarListadoPrecios.FlatAppearance.BorderSize = 0;
            this.VentasBtnGuardarListadoPrecios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnGuardarListadoPrecios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnGuardarListadoPrecios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnGuardarListadoPrecios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnGuardarListadoPrecios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnGuardarListadoPrecios.Location = new System.Drawing.Point(3, 416);
            this.VentasBtnGuardarListadoPrecios.Name = "VentasBtnGuardarListadoPrecios";
            this.VentasBtnGuardarListadoPrecios.Size = new System.Drawing.Size(63, 64);
            this.VentasBtnGuardarListadoPrecios.TabIndex = 17;
            this.VentasBtnGuardarListadoPrecios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnGuardarListadoPrecios, "Guardar");
            this.VentasBtnGuardarListadoPrecios.UseVisualStyleBackColor = false;
            // 
            // VentasBtnCancelarListadoPrecios
            // 
            this.VentasBtnCancelarListadoPrecios.AccessibleDescription = "Cancelar";
            this.VentasBtnCancelarListadoPrecios.AccessibleName = "Cancelar";
            this.VentasBtnCancelarListadoPrecios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnCancelarListadoPrecios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnCancelarListadoPrecios.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalCancelarVentas;
            this.VentasBtnCancelarListadoPrecios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCancelarListadoPrecios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VentasBtnCancelarListadoPrecios.FlatAppearance.BorderSize = 0;
            this.VentasBtnCancelarListadoPrecios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCancelarListadoPrecios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCancelarListadoPrecios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCancelarListadoPrecios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnCancelarListadoPrecios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnCancelarListadoPrecios.Location = new System.Drawing.Point(0, 502);
            this.VentasBtnCancelarListadoPrecios.Name = "VentasBtnCancelarListadoPrecios";
            this.VentasBtnCancelarListadoPrecios.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCancelarListadoPrecios.TabIndex = 6;
            this.VentasBtnCancelarListadoPrecios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnCancelarListadoPrecios, "Cancelar");
            this.VentasBtnCancelarListadoPrecios.UseVisualStyleBackColor = false;
            // 
            // VentasBtnConsultarListadoPrecios
            // 
            this.VentasBtnConsultarListadoPrecios.AccessibleDescription = "Consultar listado de precios";
            this.VentasBtnConsultarListadoPrecios.AccessibleName = "Consultar listado de precios";
            this.VentasBtnConsultarListadoPrecios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnConsultarListadoPrecios.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.BotonOriginalConsultarVentas;
            this.VentasBtnConsultarListadoPrecios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnConsultarListadoPrecios.FlatAppearance.BorderSize = 0;
            this.VentasBtnConsultarListadoPrecios.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnConsultarListadoPrecios.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnConsultarListadoPrecios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnConsultarListadoPrecios.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnConsultarListadoPrecios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnConsultarListadoPrecios.Location = new System.Drawing.Point(1122, 88);
            this.VentasBtnConsultarListadoPrecios.Name = "VentasBtnConsultarListadoPrecios";
            this.VentasBtnConsultarListadoPrecios.Size = new System.Drawing.Size(58, 64);
            this.VentasBtnConsultarListadoPrecios.TabIndex = 20;
            this.VentasBtnConsultarListadoPrecios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnConsultarListadoPrecios, "Consultar listado de precios");
            this.VentasBtnConsultarListadoPrecios.UseVisualStyleBackColor = false;
            // 
            // VentasPnlEncabezado
            // 
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasPbMarca);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1197, 78);
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
            this.VentasCboTipoCliente.Location = new System.Drawing.Point(12, 114);
            this.VentasCboTipoCliente.Name = "VentasCboTipoCliente";
            this.VentasCboTipoCliente.Size = new System.Drawing.Size(274, 38);
            this.VentasCboTipoCliente.TabIndex = 2;
            // 
            // VentasLblTipoCliente
            // 
            this.VentasLblTipoCliente.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblTipoCliente.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblTipoCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTipoCliente.Location = new System.Drawing.Point(16, 90);
            this.VentasLblTipoCliente.Name = "VentasLblTipoCliente";
            this.VentasLblTipoCliente.Size = new System.Drawing.Size(152, 30);
            this.VentasLblTipoCliente.TabIndex = 7;
            this.VentasLblTipoCliente.Text = "Tipo de cliente";
            // 
            // VentasLblTipoMod
            // 
            this.VentasLblTipoMod.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblTipoMod.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblTipoMod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTipoMod.Location = new System.Drawing.Point(8, 228);
            this.VentasLblTipoMod.Name = "VentasLblTipoMod";
            this.VentasLblTipoMod.Size = new System.Drawing.Size(237, 30);
            this.VentasLblTipoMod.TabIndex = 8;
            this.VentasLblTipoMod.Text = "Tipo de modificación";
            // 
            // VentasNudPorcentajeDescuento
            // 
            this.VentasNudPorcentajeDescuento.DecimalPlaces = 2;
            this.VentasNudPorcentajeDescuento.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasNudPorcentajeDescuento.Location = new System.Drawing.Point(424, 258);
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
            this.VentasLblSimboloPorcentaje.Location = new System.Drawing.Point(550, 264);
            this.VentasLblSimboloPorcentaje.Name = "VentasLblSimboloPorcentaje";
            this.VentasLblSimboloPorcentaje.Size = new System.Drawing.Size(46, 34);
            this.VentasLblSimboloPorcentaje.TabIndex = 10;
            this.VentasLblSimboloPorcentaje.Text = "%";
            // 
            // VentasLblCategoria
            // 
            this.VentasLblCategoria.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblCategoria.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblCategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblCategoria.Location = new System.Drawing.Point(16, 155);
            this.VentasLblCategoria.Name = "VentasLblCategoria";
            this.VentasLblCategoria.Size = new System.Drawing.Size(114, 30);
            this.VentasLblCategoria.TabIndex = 11;
            this.VentasLblCategoria.Text = "Categoria";
            // 
            // VentasLblValor
            // 
            this.VentasLblValor.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblValor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblValor.Location = new System.Drawing.Point(430, 228);
            this.VentasLblValor.Name = "VentasLblValor";
            this.VentasLblValor.Size = new System.Drawing.Size(114, 30);
            this.VentasLblValor.TabIndex = 13;
            this.VentasLblValor.Text = "Valor";
            // 
            // VentasLblListadoClientes
            // 
            this.VentasLblListadoClientes.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblListadoClientes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.VentasLblListadoClientes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblListadoClientes.Location = new System.Drawing.Point(117, 369);
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvListadoClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.VentasDgvListadoClientes.ColumnHeadersHeight = 34;
            this.VentasDgvListadoClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.VentasDgvListadoClientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmTipoCliente,
            this.VentasClmMarca,
            this.VentasClmLinea,
            this.VentasClmCategoria,
            this.VentasClmProveedores,
            this.VentasClmTipoProducto,
            this.VentasClmPrecioBase,
            this.VentasClmPorcentaje,
            this.VentasClmPrecioFinal,
            this.VentasClmAcciones});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvListadoClientes.DefaultCellStyle = dataGridViewCellStyle2;
            this.VentasDgvListadoClientes.EnableHeadersVisualStyles = false;
            this.VentasDgvListadoClientes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDgvListadoClientes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvListadoClientes.Location = new System.Drawing.Point(79, 402);
            this.VentasDgvListadoClientes.MultiSelect = false;
            this.VentasDgvListadoClientes.Name = "VentasDgvListadoClientes";
            this.VentasDgvListadoClientes.ReadOnly = true;
            this.VentasDgvListadoClientes.RowHeadersVisible = false;
            this.VentasDgvListadoClientes.RowHeadersWidth = 62;
            this.VentasDgvListadoClientes.RowTemplate.Height = 28;
            this.VentasDgvListadoClientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvListadoClientes.Size = new System.Drawing.Size(1118, 226);
            this.VentasDgvListadoClientes.TabIndex = 16;
            // 
            // VentasClmTipoCliente
            // 
            this.VentasClmTipoCliente.HeaderText = "Cliente";
            this.VentasClmTipoCliente.MinimumWidth = 8;
            this.VentasClmTipoCliente.Name = "VentasClmTipoCliente";
            this.VentasClmTipoCliente.ReadOnly = true;
            // 
            // VentasClmMarca
            // 
            this.VentasClmMarca.HeaderText = "Marca";
            this.VentasClmMarca.MinimumWidth = 8;
            this.VentasClmMarca.Name = "VentasClmMarca";
            this.VentasClmMarca.ReadOnly = true;
            // 
            // VentasClmLinea
            // 
            this.VentasClmLinea.HeaderText = "Linea";
            this.VentasClmLinea.MinimumWidth = 8;
            this.VentasClmLinea.Name = "VentasClmLinea";
            this.VentasClmLinea.ReadOnly = true;
            // 
            // VentasClmCategoria
            // 
            this.VentasClmCategoria.HeaderText = "Categoria";
            this.VentasClmCategoria.MinimumWidth = 8;
            this.VentasClmCategoria.Name = "VentasClmCategoria";
            this.VentasClmCategoria.ReadOnly = true;
            // 
            // VentasClmProveedores
            // 
            this.VentasClmProveedores.HeaderText = "Proveedores";
            this.VentasClmProveedores.MinimumWidth = 8;
            this.VentasClmProveedores.Name = "VentasClmProveedores";
            this.VentasClmProveedores.ReadOnly = true;
            // 
            // VentasClmTipoProducto
            // 
            this.VentasClmTipoProducto.HeaderText = "Producto";
            this.VentasClmTipoProducto.MinimumWidth = 8;
            this.VentasClmTipoProducto.Name = "VentasClmTipoProducto";
            this.VentasClmTipoProducto.ReadOnly = true;
            // 
            // VentasClmPrecioBase
            // 
            this.VentasClmPrecioBase.HeaderText = "Precio base";
            this.VentasClmPrecioBase.MinimumWidth = 8;
            this.VentasClmPrecioBase.Name = "VentasClmPrecioBase";
            this.VentasClmPrecioBase.ReadOnly = true;
            // 
            // VentasClmPorcentaje
            // 
            this.VentasClmPorcentaje.HeaderText = "Descuento (%)";
            this.VentasClmPorcentaje.MinimumWidth = 8;
            this.VentasClmPorcentaje.Name = "VentasClmPorcentaje";
            this.VentasClmPorcentaje.ReadOnly = true;
            // 
            // VentasClmPrecioFinal
            // 
            this.VentasClmPrecioFinal.HeaderText = "Precio final";
            this.VentasClmPrecioFinal.MinimumWidth = 8;
            this.VentasClmPrecioFinal.Name = "VentasClmPrecioFinal";
            this.VentasClmPrecioFinal.ReadOnly = true;
            // 
            // VentasLblMarca
            // 
            this.VentasLblMarca.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblMarca.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblMarca.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblMarca.Location = new System.Drawing.Point(392, 90);
            this.VentasLblMarca.Name = "VentasLblMarca";
            this.VentasLblMarca.Size = new System.Drawing.Size(174, 30);
            this.VentasLblMarca.TabIndex = 18;
            this.VentasLblMarca.Text = "Marca";
            // 
            // VentasCboTipoProducto
            // 
            this.VentasCboTipoProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboTipoProducto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboTipoProducto.FormattingEnabled = true;
            this.VentasCboTipoProducto.Location = new System.Drawing.Point(383, 114);
            this.VentasCboTipoProducto.Name = "VentasCboTipoProducto";
            this.VentasCboTipoProducto.Size = new System.Drawing.Size(292, 38);
            this.VentasCboTipoProducto.TabIndex = 19;
            // 
            // VentasLbllinea
            // 
            this.VentasLbllinea.BackColor = System.Drawing.Color.Transparent;
            this.VentasLbllinea.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLbllinea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLbllinea.Location = new System.Drawing.Point(726, 90);
            this.VentasLbllinea.Name = "VentasLbllinea";
            this.VentasLbllinea.Size = new System.Drawing.Size(174, 30);
            this.VentasLbllinea.TabIndex = 21;
            this.VentasLbllinea.Text = "Linea";
            // 
            // VentasCboLinea
            // 
            this.VentasCboLinea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboLinea.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboLinea.FormattingEnabled = true;
            this.VentasCboLinea.Location = new System.Drawing.Point(730, 114);
            this.VentasCboLinea.Name = "VentasCboLinea";
            this.VentasCboLinea.Size = new System.Drawing.Size(292, 38);
            this.VentasCboLinea.TabIndex = 22;
            // 
            // VentasCboCategoria
            // 
            this.VentasCboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboCategoria.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboCategoria.FormattingEnabled = true;
            this.VentasCboCategoria.Location = new System.Drawing.Point(12, 178);
            this.VentasCboCategoria.Name = "VentasCboCategoria";
            this.VentasCboCategoria.Size = new System.Drawing.Size(274, 38);
            this.VentasCboCategoria.TabIndex = 23;
            // 
            // VentasLblProveedores
            // 
            this.VentasLblProveedores.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblProveedores.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblProveedores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblProveedores.Location = new System.Drawing.Point(392, 155);
            this.VentasLblProveedores.Name = "VentasLblProveedores";
            this.VentasLblProveedores.Size = new System.Drawing.Size(174, 30);
            this.VentasLblProveedores.TabIndex = 24;
            this.VentasLblProveedores.Text = "Proveedores";
            // 
            // VentasCboProveedores
            // 
            this.VentasCboProveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboProveedores.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboProveedores.FormattingEnabled = true;
            this.VentasCboProveedores.Location = new System.Drawing.Point(383, 178);
            this.VentasCboProveedores.Name = "VentasCboProveedores";
            this.VentasCboProveedores.Size = new System.Drawing.Size(292, 38);
            this.VentasCboProveedores.TabIndex = 25;
            // 
            // VentasLblProducto
            // 
            this.VentasLblProducto.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblProducto.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblProducto.Location = new System.Drawing.Point(726, 155);
            this.VentasLblProducto.Name = "VentasLblProducto";
            this.VentasLblProducto.Size = new System.Drawing.Size(174, 30);
            this.VentasLblProducto.TabIndex = 26;
            this.VentasLblProducto.Text = "Producto";
            // 
            // VentasCboProducto
            // 
            this.VentasCboProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboProducto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasCboProducto.FormattingEnabled = true;
            this.VentasCboProducto.Location = new System.Drawing.Point(730, 178);
            this.VentasCboProducto.Name = "VentasCboProducto";
            this.VentasCboProducto.Size = new System.Drawing.Size(292, 38);
            this.VentasCboProducto.TabIndex = 27;
            // 
            // VentasTxtLisPre
            // 
            this.VentasTxtLisPre.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.VentasTxtLisPre.Location = new System.Drawing.Point(426, 301);
            this.VentasTxtLisPre.Name = "VentasTxtLisPre";
            this.VentasTxtLisPre.Size = new System.Drawing.Size(124, 37);
            this.VentasTxtLisPre.TabIndex = 29;
            this.VentasTxtLisPre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // VentasLblAlcance
            // 
            this.VentasLblAlcance.BackColor = System.Drawing.Color.Transparent;
            this.VentasLblAlcance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblAlcance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblAlcance.Location = new System.Drawing.Point(726, 228);
            this.VentasLblAlcance.Name = "VentasLblAlcance";
            this.VentasLblAlcance.Size = new System.Drawing.Size(237, 30);
            this.VentasLblAlcance.TabIndex = 30;
            this.VentasLblAlcance.Text = "Alcance de modificación";
            // 
            // VentasGbxMod
            // 
            this.VentasGbxMod.Controls.Add(this.VentasRbtnAumBase);
            this.VentasGbxMod.Controls.Add(this.VentasRbtnAumPor);
            this.VentasGbxMod.Controls.Add(this.VentasRbtnDescuento);
            this.VentasGbxMod.Location = new System.Drawing.Point(3, 250);
            this.VentasGbxMod.Name = "VentasGbxMod";
            this.VentasGbxMod.Size = new System.Drawing.Size(312, 116);
            this.VentasGbxMod.TabIndex = 31;
            this.VentasGbxMod.TabStop = false;
            // 
            // VentasRbtnAumBase
            // 
            this.VentasRbtnAumBase.AutoSize = true;
            this.VentasRbtnAumBase.Location = new System.Drawing.Point(0, 82);
            this.VentasRbtnAumBase.Name = "VentasRbtnAumBase";
            this.VentasRbtnAumBase.Size = new System.Drawing.Size(218, 34);
            this.VentasRbtnAumBase.TabIndex = 5;
            this.VentasRbtnAumBase.TabStop = true;
            this.VentasRbtnAumBase.Text = "Aumento por base";
            this.VentasRbtnAumBase.UseVisualStyleBackColor = true;
            // 
            // VentasRbtnAumPor
            // 
            this.VentasRbtnAumPor.AutoSize = true;
            this.VentasRbtnAumPor.Location = new System.Drawing.Point(0, 51);
            this.VentasRbtnAumPor.Name = "VentasRbtnAumPor";
            this.VentasRbtnAumPor.Size = new System.Drawing.Size(314, 34);
            this.VentasRbtnAumPor.TabIndex = 4;
            this.VentasRbtnAumPor.TabStop = true;
            this.VentasRbtnAumPor.Text = "Aumento por porcentaje (%)";
            this.VentasRbtnAumPor.UseVisualStyleBackColor = true;
            // 
            // VentasRbtnDescuento
            // 
            this.VentasRbtnDescuento.AutoSize = true;
            this.VentasRbtnDescuento.Location = new System.Drawing.Point(0, 21);
            this.VentasRbtnDescuento.Name = "VentasRbtnDescuento";
            this.VentasRbtnDescuento.Size = new System.Drawing.Size(178, 34);
            this.VentasRbtnDescuento.TabIndex = 3;
            this.VentasRbtnDescuento.TabStop = true;
            this.VentasRbtnDescuento.Text = "Descuento (%)";
            this.VentasRbtnDescuento.UseVisualStyleBackColor = true;
            // 
            // VentasGbxAlc
            // 
            this.VentasGbxAlc.Controls.Add(this.VentasRbtnTdsRslt);
            this.VentasGbxAlc.Controls.Add(this.VentasRbtnPrdSelec);
            this.VentasGbxAlc.Controls.Add(this.VentasRbtnPrdInd);
            this.VentasGbxAlc.Location = new System.Drawing.Point(730, 250);
            this.VentasGbxAlc.Name = "VentasGbxAlc";
            this.VentasGbxAlc.Size = new System.Drawing.Size(361, 126);
            this.VentasGbxAlc.TabIndex = 32;
            this.VentasGbxAlc.TabStop = false;
            // 
            // VentasRbtnTdsRslt
            // 
            this.VentasRbtnTdsRslt.AutoSize = true;
            this.VentasRbtnTdsRslt.Location = new System.Drawing.Point(6, 86);
            this.VentasRbtnTdsRslt.Name = "VentasRbtnTdsRslt";
            this.VentasRbtnTdsRslt.Size = new System.Drawing.Size(363, 34);
            this.VentasRbtnTdsRslt.TabIndex = 2;
            this.VentasRbtnTdsRslt.TabStop = true;
            this.VentasRbtnTdsRslt.Text = "Todos los resultado (segun filtros)";
            this.VentasRbtnTdsRslt.UseVisualStyleBackColor = true;
            // 
            // VentasRbtnPrdSelec
            // 
            this.VentasRbtnPrdSelec.AutoSize = true;
            this.VentasRbtnPrdSelec.Location = new System.Drawing.Point(6, 52);
            this.VentasRbtnPrdSelec.Name = "VentasRbtnPrdSelec";
            this.VentasRbtnPrdSelec.Size = new System.Drawing.Size(275, 34);
            this.VentasRbtnPrdSelec.TabIndex = 1;
            this.VentasRbtnPrdSelec.TabStop = true;
            this.VentasRbtnPrdSelec.Text = "Productos seleccionados";
            this.VentasRbtnPrdSelec.UseVisualStyleBackColor = true;
            // 
            // VentasRbtnPrdInd
            // 
            this.VentasRbtnPrdInd.AutoSize = true;
            this.VentasRbtnPrdInd.Location = new System.Drawing.Point(6, 21);
            this.VentasRbtnPrdInd.Name = "VentasRbtnPrdInd";
            this.VentasRbtnPrdInd.Size = new System.Drawing.Size(225, 34);
            this.VentasRbtnPrdInd.TabIndex = 0;
            this.VentasRbtnPrdInd.TabStop = true;
            this.VentasRbtnPrdInd.Text = "Producto Invididual";
            this.VentasRbtnPrdInd.UseVisualStyleBackColor = true;
            // 
            // VentasBtnCotRefrescar
            // 
            this.VentasBtnCotRefrescar.AccessibleName = "Refrescar la cotizacion";
            this.VentasBtnCotRefrescar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnCotRefrescar.BackgroundImage = global::CapaVista_ListadoPrecios.Properties.Resources.RefrescarVentas;
            this.VentasBtnCotRefrescar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCotRefrescar.FlatAppearance.BorderSize = 0;
            this.VentasBtnCotRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCotRefrescar.Location = new System.Drawing.Point(1122, 155);
            this.VentasBtnCotRefrescar.Name = "VentasBtnCotRefrescar";
            this.VentasBtnCotRefrescar.Size = new System.Drawing.Size(58, 61);
            this.VentasBtnCotRefrescar.TabIndex = 58;
            this.VentasBtnCotRefrescar.UseVisualStyleBackColor = false;
            // 
            // VentasClmAcciones
            // 
            this.VentasClmAcciones.HeaderText = "Acciones";
            this.VentasClmAcciones.MinimumWidth = 8;
            this.VentasClmAcciones.Name = "VentasClmAcciones";
            this.VentasClmAcciones.ReadOnly = true;
            // 
            // FrmListadoPrecios
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1192, 626);
            this.Controls.Add(this.VentasBtnCotRefrescar);
            this.Controls.Add(this.VentasGbxAlc);
            this.Controls.Add(this.VentasGbxMod);
            this.Controls.Add(this.VentasLblAlcance);
            this.Controls.Add(this.VentasTxtLisPre);
            this.Controls.Add(this.VentasCboProducto);
            this.Controls.Add(this.VentasLblProducto);
            this.Controls.Add(this.VentasCboProveedores);
            this.Controls.Add(this.VentasLblProveedores);
            this.Controls.Add(this.VentasCboCategoria);
            this.Controls.Add(this.VentasCboLinea);
            this.Controls.Add(this.VentasLbllinea);
            this.Controls.Add(this.VentasBtnConsultarListadoPrecios);
            this.Controls.Add(this.VentasCboTipoProducto);
            this.Controls.Add(this.VentasLblMarca);
            this.Controls.Add(this.VentasBtnCancelarListadoPrecios);
            this.Controls.Add(this.VentasBtnGuardarListadoPrecios);
            this.Controls.Add(this.VentasDgvListadoClientes);
            this.Controls.Add(this.VentasLblListadoClientes);
            this.Controls.Add(this.VentasLblValor);
            this.Controls.Add(this.VentasLblCategoria);
            this.Controls.Add(this.VentasLblSimboloPorcentaje);
            this.Controls.Add(this.VentasNudPorcentajeDescuento);
            this.Controls.Add(this.VentasLblTipoMod);
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
            this.VentasGbxMod.ResumeLayout(false);
            this.VentasGbxMod.PerformLayout();
            this.VentasGbxAlc.ResumeLayout(false);
            this.VentasGbxAlc.PerformLayout();
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
        private System.Windows.Forms.Label VentasLblTipoMod;
        private System.Windows.Forms.NumericUpDown VentasNudPorcentajeDescuento;
        private System.Windows.Forms.Label VentasLblSimboloPorcentaje;
        private System.Windows.Forms.Label VentasLblCategoria;
        private System.Windows.Forms.Label VentasLblValor;
        private System.Windows.Forms.Label VentasLblListadoClientes;
        private System.Windows.Forms.DataGridView VentasDgvListadoClientes;
        private System.Windows.Forms.Button VentasBtnGuardarListadoPrecios;
        private System.Windows.Forms.Button VentasBtnCancelarListadoPrecios;
        private System.Windows.Forms.ToolTip VentasTtpAccionesOriginales;
        private System.Windows.Forms.Label VentasLblMarca;
        private System.Windows.Forms.ComboBox VentasCboTipoProducto;
        private System.Windows.Forms.Label VentasLbllinea;
        private System.Windows.Forms.ComboBox VentasCboLinea;
        private System.Windows.Forms.ComboBox VentasCboCategoria;
        private System.Windows.Forms.Label VentasLblProveedores;
        private System.Windows.Forms.ComboBox VentasCboProveedores;
        private System.Windows.Forms.Label VentasLblProducto;
        private System.Windows.Forms.ComboBox VentasCboProducto;
        private System.Windows.Forms.Button VentasBtnConsultarListadoPrecios;
        private System.Windows.Forms.TextBox VentasTxtLisPre;
        private System.Windows.Forms.Label VentasLblAlcance;
        private System.Windows.Forms.GroupBox VentasGbxMod;
        private System.Windows.Forms.GroupBox VentasGbxAlc;
        private System.Windows.Forms.RadioButton VentasRbtnAumBase;
        private System.Windows.Forms.RadioButton VentasRbtnAumPor;
        private System.Windows.Forms.RadioButton VentasRbtnDescuento;
        private System.Windows.Forms.RadioButton VentasRbtnTdsRslt;
        private System.Windows.Forms.RadioButton VentasRbtnPrdSelec;
        private System.Windows.Forms.RadioButton VentasRbtnPrdInd;
        internal System.Windows.Forms.Button VentasBtnCotRefrescar;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmTipoCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmProveedores;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmTipoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPrecioBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPrecioFinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAcciones;
    }
}
// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 14/10/2026
