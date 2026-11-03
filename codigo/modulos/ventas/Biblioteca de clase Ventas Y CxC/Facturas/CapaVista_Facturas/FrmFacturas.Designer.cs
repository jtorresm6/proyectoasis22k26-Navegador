/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Jose, Natali
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Encargado: Matthew Andreé Juárez Xoy
 * Fecha de actualización: 14/10/2026
 * Propósito: Conservar los controles del prototipo visual de Facturas.
 */
namespace CapaVista_Facturas
{
    partial class FrmFacturas
    {
        private System.ComponentModel.IContainer components = null;

        // Libera los componentes que el equipo incorpore desde el diseñador.
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
            this.VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this.components);
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblEstablecimiento = new System.Windows.Forms.Label();
            this.VentasTxtEstablecimiento = new System.Windows.Forms.TextBox();
            this.VentasLblFechaEmision = new System.Windows.Forms.Label();
            this.VentasDtpFechaEmision = new System.Windows.Forms.DateTimePicker();
            this.VentasLblTipoDte = new System.Windows.Forms.Label();
            this.VentasCboTipoDte = new System.Windows.Forms.ComboBox();
            this.VentasLblMoneda = new System.Windows.Forms.Label();
            this.VentasCboMoneda = new System.Windows.Forms.ComboBox();
            this.VentasRdoNit = new System.Windows.Forms.RadioButton();
            this.VentasGbxIdentificacion = new System.Windows.Forms.GroupBox();
            this.VentasRdoIdExtranjero = new System.Windows.Forms.RadioButton();
            this.VentasRdoCui = new System.Windows.Forms.RadioButton();
            this.VentasLblNitReceptor = new System.Windows.Forms.Label();
            this.VentasLblDireccionComprador = new System.Windows.Forms.Label();
            this.VentasTxtNitReceptor = new System.Windows.Forms.TextBox();
            this.VentasTxtDireccionComprador = new System.Windows.Forms.TextBox();
            this.VentasDgvItems = new System.Windows.Forms.DataGridView();
            this.VentasLblItems = new System.Windows.Forms.Label();
            this.VentasLblBienServicio = new System.Windows.Forms.Label();
            this.VentasLblCantidad = new System.Windows.Forms.Label();
            this.VentasLblDescripcion = new System.Windows.Forms.Label();
            this.VentasLblPrecioUnitario = new System.Windows.Forms.Label();
            this.VentasLblDescuentos = new System.Windows.Forms.Label();
            this.VentasLblTotal = new System.Windows.Forms.Label();
            this.VentasCboBienServicio = new System.Windows.Forms.ComboBox();
            this.VentasTxtCantidad = new System.Windows.Forms.TextBox();
            this.VentasTxtDescripcion = new System.Windows.Forms.TextBox();
            this.VentasTxtPrecioUnitario = new System.Windows.Forms.TextBox();
            this.VentasTxtDescuentos = new System.Windows.Forms.TextBox();
            this.VentasTxtTotal = new System.Windows.Forms.TextBox();
            this.VentasBtnEmitirFactura = new System.Windows.Forms.Button();
            this.VentasBtnAgregarItem = new System.Windows.Forms.Button();
            this.VentasGbxIdentificacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvItems)).BeginInit();
            this.SuspendLayout();
            //
            // VentasLblTitulo
            //
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(118, 28);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(937, 65);
            this.VentasLblTitulo.TabIndex = 0;
            this.VentasLblTitulo.Text = "EMISIÓN DE FACTURA ELECTRÓNICA (DTE)";
            //
            // VentasLblEstablecimiento
            //
            this.VentasLblEstablecimiento.AutoSize = true;
            this.VentasLblEstablecimiento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblEstablecimiento.Location = new System.Drawing.Point(122, 110);
            this.VentasLblEstablecimiento.Name = "VentasLblEstablecimiento";
            this.VentasLblEstablecimiento.Size = new System.Drawing.Size(144, 25);
            this.VentasLblEstablecimiento.TabIndex = 1;
            this.VentasLblEstablecimiento.Text = "Establecimiento";
            //
            // VentasTxtEstablecimiento
            //
            this.VentasTxtEstablecimiento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtEstablecimiento.Location = new System.Drawing.Point(395, 106);
            this.VentasTxtEstablecimiento.Name = "VentasTxtEstablecimiento";
            this.VentasTxtEstablecimiento.Size = new System.Drawing.Size(268, 31);
            this.VentasTxtEstablecimiento.TabIndex = 2;
            //
            // VentasLblFechaEmision
            //
            this.VentasLblFechaEmision.AutoSize = true;
            this.VentasLblFechaEmision.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblFechaEmision.Location = new System.Drawing.Point(122, 166);
            this.VentasLblFechaEmision.Name = "VentasLblFechaEmision";
            this.VentasLblFechaEmision.Size = new System.Drawing.Size(155, 25);
            this.VentasLblFechaEmision.TabIndex = 3;
            this.VentasLblFechaEmision.Text = "Fecha de emisión";
            //
            // VentasDtpFechaEmision
            //
            this.VentasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasDtpFechaEmision.Location = new System.Drawing.Point(395, 162);
            this.VentasDtpFechaEmision.Name = "VentasDtpFechaEmision";
            this.VentasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaEmision.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaEmision.Size = new System.Drawing.Size(268, 31);
            this.VentasDtpFechaEmision.TabIndex = 4;
            //
            // VentasLblTipoDte
            //
            this.VentasLblTipoDte.AutoSize = true;
            this.VentasLblTipoDte.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblTipoDte.Location = new System.Drawing.Point(122, 222);
            this.VentasLblTipoDte.Name = "VentasLblTipoDte";
            this.VentasLblTipoDte.Size = new System.Drawing.Size(111, 25);
            this.VentasLblTipoDte.TabIndex = 5;
            this.VentasLblTipoDte.Text = "Tipo de DTE";
            //
            // VentasCboTipoDte
            //
            this.VentasCboTipoDte.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasCboTipoDte.FormattingEnabled = true;
            this.VentasCboTipoDte.Items.AddRange(new object[] {
            "Factura",
            "Factura Cambiaria",
            "Factura Especial"});
            this.VentasCboTipoDte.Location = new System.Drawing.Point(395, 218);
            this.VentasCboTipoDte.Name = "VentasCboTipoDte";
            this.VentasCboTipoDte.Size = new System.Drawing.Size(268, 33);
            this.VentasCboTipoDte.TabIndex = 6;
            //
            // VentasLblMoneda
            //
            this.VentasLblMoneda.AutoSize = true;
            this.VentasLblMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblMoneda.Location = new System.Drawing.Point(122, 278);
            this.VentasLblMoneda.Name = "VentasLblMoneda";
            this.VentasLblMoneda.Size = new System.Drawing.Size(81, 25);
            this.VentasLblMoneda.TabIndex = 7;
            this.VentasLblMoneda.Text = "Moneda";
            //
            // VentasCboMoneda
            //
            this.VentasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasCboMoneda.FormattingEnabled = true;
            this.VentasCboMoneda.Items.AddRange(new object[] {
            "GTQ-Quetzal",
            "USD-Dólar estadounidense"});
            this.VentasCboMoneda.Location = new System.Drawing.Point(395, 274);
            this.VentasCboMoneda.Name = "VentasCboMoneda";
            this.VentasCboMoneda.Size = new System.Drawing.Size(268, 33);
            this.VentasCboMoneda.TabIndex = 8;
            //
            // VentasRdoNit
            //
            this.VentasRdoNit.AutoSize = true;
            this.VentasRdoNit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasRdoNit.ForeColor = System.Drawing.Color.White;
            this.VentasRdoNit.Location = new System.Drawing.Point(25, 30);
            this.VentasRdoNit.Name = "VentasRdoNit";
            this.VentasRdoNit.Size = new System.Drawing.Size(66, 29);
            this.VentasRdoNit.TabIndex = 9;
            this.VentasRdoNit.TabStop = true;
            this.VentasRdoNit.Text = "NIT";
            this.VentasRdoNit.UseVisualStyleBackColor = true;
            //
            // VentasGbxIdentificacion
            //
            this.VentasGbxIdentificacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasGbxIdentificacion.Controls.Add(this.VentasRdoIdExtranjero);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasRdoCui);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasRdoNit);
            this.VentasGbxIdentificacion.Location = new System.Drawing.Point(709, 320);
            this.VentasGbxIdentificacion.Name = "VentasGbxIdentificacion";
            this.VentasGbxIdentificacion.Size = new System.Drawing.Size(198, 115);
            this.VentasGbxIdentificacion.TabIndex = 10;
            this.VentasGbxIdentificacion.TabStop = false;
            //
            // VentasRdoIdExtranjero
            //
            this.VentasRdoIdExtranjero.AutoSize = true;
            this.VentasRdoIdExtranjero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasRdoIdExtranjero.ForeColor = System.Drawing.Color.White;
            this.VentasRdoIdExtranjero.Location = new System.Drawing.Point(16, 65);
            this.VentasRdoIdExtranjero.Name = "VentasRdoIdExtranjero";
            this.VentasRdoIdExtranjero.Size = new System.Drawing.Size(168, 29);
            this.VentasRdoIdExtranjero.TabIndex = 11;
            this.VentasRdoIdExtranjero.TabStop = true;
            this.VentasRdoIdExtranjero.Text = "ID EXTRANJERO";
            this.VentasRdoIdExtranjero.UseVisualStyleBackColor = true;
            //
            // VentasRdoCui
            //
            this.VentasRdoCui.AutoSize = true;
            this.VentasRdoCui.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasRdoCui.ForeColor = System.Drawing.Color.White;
            this.VentasRdoCui.Location = new System.Drawing.Point(116, 30);
            this.VentasRdoCui.Name = "VentasRdoCui";
            this.VentasRdoCui.Size = new System.Drawing.Size(66, 29);
            this.VentasRdoCui.TabIndex = 10;
            this.VentasRdoCui.TabStop = true;
            this.VentasRdoCui.Text = "CUI";
            this.VentasRdoCui.UseVisualStyleBackColor = true;
            //
            // VentasLblNitReceptor
            //
            this.VentasLblNitReceptor.AutoSize = true;
            this.VentasLblNitReceptor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblNitReceptor.Location = new System.Drawing.Point(122, 350);
            this.VentasLblNitReceptor.Name = "VentasLblNitReceptor";
            this.VentasLblNitReceptor.Size = new System.Drawing.Size(201, 25);
            this.VentasLblNitReceptor.TabIndex = 11;
            this.VentasLblNitReceptor.Text = "NIT del receptor o (CF)";
            //
            // VentasLblDireccionComprador
            //
            this.VentasLblDireccionComprador.AutoSize = true;
            this.VentasLblDireccionComprador.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblDireccionComprador.Location = new System.Drawing.Point(122, 406);
            this.VentasLblDireccionComprador.Name = "VentasLblDireccionComprador";
            this.VentasLblDireccionComprador.Size = new System.Drawing.Size(217, 25);
            this.VentasLblDireccionComprador.TabIndex = 12;
            this.VentasLblDireccionComprador.Text = "Dirección del comprador";
            //
            // VentasTxtNitReceptor
            //
            this.VentasTxtNitReceptor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtNitReceptor.Location = new System.Drawing.Point(395, 346);
            this.VentasTxtNitReceptor.Name = "VentasTxtNitReceptor";
            this.VentasTxtNitReceptor.Size = new System.Drawing.Size(268, 31);
            this.VentasTxtNitReceptor.TabIndex = 13;
            //
            // VentasTxtDireccionComprador
            //
            this.VentasTxtDireccionComprador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtDireccionComprador.Location = new System.Drawing.Point(395, 402);
            this.VentasTxtDireccionComprador.Name = "VentasTxtDireccionComprador";
            this.VentasTxtDireccionComprador.Size = new System.Drawing.Size(268, 31);
            this.VentasTxtDireccionComprador.TabIndex = 14;
            //
            // VentasDgvItems
            //
            this.VentasDgvItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VentasDgvItems.Location = new System.Drawing.Point(127, 590);
            this.VentasDgvItems.Name = "VentasDgvItems";
            this.VentasDgvItems.RowHeadersWidth = 62;
            this.VentasDgvItems.RowTemplate.Height = 28;
            this.VentasDgvItems.Size = new System.Drawing.Size(850, 38);
            this.VentasDgvItems.TabIndex = 15;
            //
            // VentasLblItems
            //
            this.VentasLblItems.AutoSize = true;
            this.VentasLblItems.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblItems.Location = new System.Drawing.Point(122, 460);
            this.VentasLblItems.Name = "VentasLblItems";
            this.VentasLblItems.Size = new System.Drawing.Size(90, 38);
            this.VentasLblItems.TabIndex = 16;
            this.VentasLblItems.Text = "Items";
            //
            // VentasLblBienServicio
            //
            this.VentasLblBienServicio.AutoSize = true;
            this.VentasLblBienServicio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblBienServicio.Location = new System.Drawing.Point(165, 526);
            this.VentasLblBienServicio.Name = "VentasLblBienServicio";
            this.VentasLblBienServicio.Size = new System.Drawing.Size(40, 25);
            this.VentasLblBienServicio.TabIndex = 17;
            this.VentasLblBienServicio.Text = "B/S";
            //
            // VentasLblCantidad
            //
            this.VentasLblCantidad.AutoSize = true;
            this.VentasLblCantidad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblCantidad.Location = new System.Drawing.Point(278, 526);
            this.VentasLblCantidad.Name = "VentasLblCantidad";
            this.VentasLblCantidad.Size = new System.Drawing.Size(86, 25);
            this.VentasLblCantidad.TabIndex = 18;
            this.VentasLblCantidad.Text = "Cantidad";
            //
            // VentasLblDescripcion
            //
            this.VentasLblDescripcion.AutoSize = true;
            this.VentasLblDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblDescripcion.Location = new System.Drawing.Point(471, 526);
            this.VentasLblDescripcion.Name = "VentasLblDescripcion";
            this.VentasLblDescripcion.Size = new System.Drawing.Size(109, 25);
            this.VentasLblDescripcion.TabIndex = 19;
            this.VentasLblDescripcion.Text = "Descripción";
            //
            // VentasLblPrecioUnitario
            //
            this.VentasLblPrecioUnitario.AutoSize = true;
            this.VentasLblPrecioUnitario.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblPrecioUnitario.Location = new System.Drawing.Point(661, 526);
            this.VentasLblPrecioUnitario.Name = "VentasLblPrecioUnitario";
            this.VentasLblPrecioUnitario.Size = new System.Drawing.Size(216, 25);
            this.VentasLblPrecioUnitario.TabIndex = 20;
            this.VentasLblPrecioUnitario.Text = "Precio/Valor unitario (Q)";
            //
            // VentasLblDescuentos
            //
            this.VentasLblDescuentos.AutoSize = true;
            this.VentasLblDescuentos.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblDescuentos.Location = new System.Drawing.Point(915, 526);
            this.VentasLblDescuentos.Name = "VentasLblDescuentos";
            this.VentasLblDescuentos.Size = new System.Drawing.Size(109, 25);
            this.VentasLblDescuentos.TabIndex = 21;
            this.VentasLblDescuentos.Text = "Descuentos";
            //
            // VentasLblTotal
            //
            this.VentasLblTotal.AutoSize = true;
            this.VentasLblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasLblTotal.Location = new System.Drawing.Point(1089, 526);
            this.VentasLblTotal.Name = "VentasLblTotal";
            this.VentasLblTotal.Size = new System.Drawing.Size(52, 25);
            this.VentasLblTotal.TabIndex = 22;
            this.VentasLblTotal.Text = "Total";
            //
            // VentasCboBienServicio
            //
            this.VentasCboBienServicio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasCboBienServicio.FormattingEnabled = true;
            this.VentasCboBienServicio.Items.AddRange(new object[] {
            "Bien",
            "Servicio"});
            this.VentasCboBienServicio.Location = new System.Drawing.Point(137, 560);
            this.VentasCboBienServicio.Name = "VentasCboBienServicio";
            this.VentasCboBienServicio.Size = new System.Drawing.Size(96, 33);
            this.VentasCboBienServicio.TabIndex = 23;
            //
            // VentasTxtCantidad
            //
            this.VentasTxtCantidad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtCantidad.Location = new System.Drawing.Point(264, 560);
            this.VentasTxtCantidad.Name = "VentasTxtCantidad";
            this.VentasTxtCantidad.Size = new System.Drawing.Size(112, 31);
            this.VentasTxtCantidad.TabIndex = 24;
            //
            // VentasTxtDescripcion
            //
            this.VentasTxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtDescripcion.Location = new System.Drawing.Point(406, 560);
            this.VentasTxtDescripcion.Name = "VentasTxtDescripcion";
            this.VentasTxtDescripcion.Size = new System.Drawing.Size(232, 31);
            this.VentasTxtDescripcion.TabIndex = 25;
            //
            // VentasTxtPrecioUnitario
            //
            this.VentasTxtPrecioUnitario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtPrecioUnitario.Location = new System.Drawing.Point(666, 560);
            this.VentasTxtPrecioUnitario.Name = "VentasTxtPrecioUnitario";
            this.VentasTxtPrecioUnitario.Size = new System.Drawing.Size(211, 31);
            this.VentasTxtPrecioUnitario.TabIndex = 26;
            //
            // VentasTxtDescuentos
            //
            this.VentasTxtDescuentos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtDescuentos.Location = new System.Drawing.Point(918, 560);
            this.VentasTxtDescuentos.Name = "VentasTxtDescuentos";
            this.VentasTxtDescuentos.Size = new System.Drawing.Size(106, 31);
            this.VentasTxtDescuentos.TabIndex = 27;
            //
            // VentasTxtTotal
            //
            this.VentasTxtTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VentasTxtTotal.Location = new System.Drawing.Point(1065, 560);
            this.VentasTxtTotal.Name = "VentasTxtTotal";
            this.VentasTxtTotal.Size = new System.Drawing.Size(112, 31);
            this.VentasTxtTotal.TabIndex = 28;
            //
            // VentasBtnEmitirFactura
            //

            this.VentasBtnEmitirFactura.Location = new System.Drawing.Point(500, 634);
            this.VentasBtnEmitirFactura.Name = "VentasBtnEmitirFactura";
            this.VentasBtnEmitirFactura.AccessibleDescription = "Emitir factura";
            this.VentasBtnEmitirFactura.AccessibleName = "Emitir factura";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnEmitirFactura.Text = string.Empty;
            this.VentasBtnEmitirFactura.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnEmitirFactura.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnEmitirFactura.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.BotonOriginalImprimirVentas;
            this.VentasBtnEmitirFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnEmitirFactura.Image = null;
            this.VentasBtnEmitirFactura.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEmitirFactura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEmitirFactura.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnEmitirFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnEmitirFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnEmitirFactura.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEmitirFactura.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEmitirFactura.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnEmitirFactura.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnEmitirFactura.UseVisualStyleBackColor = false;
            this.VentasBtnEmitirFactura.UseCompatibleTextRendering = false;
            this.VentasBtnEmitirFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnEmitirFactura.TabIndex = 30;

            //
            // VentasBtnAgregarItem
            //

            this.VentasBtnAgregarItem.Location = new System.Drawing.Point(999, 590);
            this.VentasBtnAgregarItem.Name = "VentasBtnAgregarItem";
            this.VentasBtnAgregarItem.AccessibleDescription = "Agregar ítem";
            this.VentasBtnAgregarItem.AccessibleName = "Agregar ítem";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnAgregarItem.Text = string.Empty;
            this.VentasBtnAgregarItem.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnAgregarItem.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnAgregarItem.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.BotonOriginalIngresarVentas;
            this.VentasBtnAgregarItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAgregarItem.Image = null;
            this.VentasBtnAgregarItem.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAgregarItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAgregarItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnAgregarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAgregarItem.FlatAppearance.BorderSize = 0;
            this.VentasBtnAgregarItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAgregarItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAgregarItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnAgregarItem.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnAgregarItem.UseVisualStyleBackColor = false;
            this.VentasBtnAgregarItem.UseCompatibleTextRendering = false;
            this.VentasBtnAgregarItem.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAgregarItem.TabIndex = 31;

            //
            // FrmFacturas
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1221, 700);
            this.Controls.Add(this.VentasBtnAgregarItem);
            this.Controls.Add(this.VentasBtnEmitirFactura);
            this.Controls.Add(this.VentasTxtTotal);
            this.Controls.Add(this.VentasTxtDescuentos);
            this.Controls.Add(this.VentasTxtPrecioUnitario);
            this.Controls.Add(this.VentasTxtDescripcion);
            this.Controls.Add(this.VentasTxtCantidad);
            this.Controls.Add(this.VentasCboBienServicio);
            this.Controls.Add(this.VentasLblTotal);
            this.Controls.Add(this.VentasLblDescuentos);
            this.Controls.Add(this.VentasLblPrecioUnitario);
            this.Controls.Add(this.VentasLblDescripcion);
            this.Controls.Add(this.VentasLblCantidad);
            this.Controls.Add(this.VentasLblBienServicio);
            this.Controls.Add(this.VentasLblItems);
            this.Controls.Add(this.VentasDgvItems);
            this.Controls.Add(this.VentasTxtDireccionComprador);
            this.Controls.Add(this.VentasTxtNitReceptor);
            this.Controls.Add(this.VentasLblDireccionComprador);
            this.Controls.Add(this.VentasLblNitReceptor);
            this.Controls.Add(this.VentasGbxIdentificacion);
            this.Controls.Add(this.VentasCboMoneda);
            this.Controls.Add(this.VentasLblMoneda);
            this.Controls.Add(this.VentasCboTipoDte);
            this.Controls.Add(this.VentasLblTipoDte);
            this.Controls.Add(this.VentasDtpFechaEmision);
            this.Controls.Add(this.VentasLblFechaEmision);
            this.Controls.Add(this.VentasTxtEstablecimiento);
            this.Controls.Add(this.VentasLblEstablecimiento);
            this.Controls.Add(this.VentasLblTitulo);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Facturas.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmFacturas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7007 – Facturas";
            this.VentasGbxIdentificacion.ResumeLayout(false);
            this.VentasGbxIdentificacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvItems)).EndInit();
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnEmitirFactura, "Emitir factura");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnAgregarItem, "Agregar ítem");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblEstablecimiento;
        private System.Windows.Forms.TextBox VentasTxtEstablecimiento;
        private System.Windows.Forms.Label VentasLblFechaEmision;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaEmision;
        private System.Windows.Forms.Label VentasLblTipoDte;
        private System.Windows.Forms.ComboBox VentasCboTipoDte;
        private System.Windows.Forms.Label VentasLblMoneda;
        private System.Windows.Forms.ComboBox VentasCboMoneda;
        private System.Windows.Forms.RadioButton VentasRdoNit;
        private System.Windows.Forms.GroupBox VentasGbxIdentificacion;
        private System.Windows.Forms.RadioButton VentasRdoIdExtranjero;
        private System.Windows.Forms.RadioButton VentasRdoCui;
        private System.Windows.Forms.Label VentasLblNitReceptor;
        private System.Windows.Forms.Label VentasLblDireccionComprador;
        private System.Windows.Forms.TextBox VentasTxtNitReceptor;
        private System.Windows.Forms.TextBox VentasTxtDireccionComprador;
        private System.Windows.Forms.DataGridView VentasDgvItems;
        private System.Windows.Forms.Label VentasLblItems;
        private System.Windows.Forms.Label VentasLblBienServicio;
        private System.Windows.Forms.Label VentasLblCantidad;
        private System.Windows.Forms.Label VentasLblDescripcion;
        private System.Windows.Forms.Label VentasLblPrecioUnitario;
        private System.Windows.Forms.Label VentasLblDescuentos;
        private System.Windows.Forms.Label VentasLblTotal;
        private System.Windows.Forms.ComboBox VentasCboBienServicio;
        private System.Windows.Forms.TextBox VentasTxtCantidad;
        private System.Windows.Forms.TextBox VentasTxtDescripcion;
        private System.Windows.Forms.TextBox VentasTxtPrecioUnitario;
        private System.Windows.Forms.TextBox VentasTxtDescuentos;
        private System.Windows.Forms.TextBox VentasTxtTotal;
        private System.Windows.Forms.Button VentasBtnEmitirFactura;
        private System.Windows.Forms.Button VentasBtnAgregarItem;
        private System.Windows.Forms.ToolTip VentasTtpAccionesOriginales;
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 14/10/2026
