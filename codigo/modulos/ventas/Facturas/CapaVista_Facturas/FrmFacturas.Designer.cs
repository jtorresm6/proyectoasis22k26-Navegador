/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Matthew, Jose, Natali
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Encargado: Matthew Andreé Juárez Xoy
 * Fecha de actualización: 30/10/2026
 * Propósito: Interfaz gráfica final de facturas con empresa fija, IVA y botón único de guardado.
 */
namespace CapaVista_Facturas
{
    partial class FrmFacturas
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
            this.VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this.components);
            this.VentasBtnAgregarItem = new System.Windows.Forms.Button();
            this.VentasBtnEmitirFactura = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasLblCodigoFormulario = new System.Windows.Forms.Label();
            this.VentasGbxOrigenDocumento = new System.Windows.Forms.GroupBox();
            this.VentasBtnCargarDocumento = new System.Windows.Forms.Button();
            this.VentasCboDocumentoOrigen = new System.Windows.Forms.ComboBox();
            this.VentasLblDocOrigen = new System.Windows.Forms.Label();
            this.VentasCboTipoOrigen = new System.Windows.Forms.ComboBox();
            this.VentasLblTipoOrigen = new System.Windows.Forms.Label();
            this.VentasGbxCabecera = new System.Windows.Forms.GroupBox();
            this.VentasCboCondicionPago = new System.Windows.Forms.ComboBox();
            this.VentasLblCondicionPago = new System.Windows.Forms.Label();
            this.VentasCboMoneda = new System.Windows.Forms.ComboBox();
            this.VentasLblMoneda = new System.Windows.Forms.Label();
            this.VentasCboTipoDte = new System.Windows.Forms.ComboBox();
            this.VentasLblTipoDte = new System.Windows.Forms.Label();
            this.VentasDtpFechaEmision = new System.Windows.Forms.DateTimePicker();
            this.VentasLblFechaEmision = new System.Windows.Forms.Label();
            this.VentasTxtEstablecimiento = new System.Windows.Forms.TextBox();
            this.VentasLblEstablecimiento = new System.Windows.Forms.Label();
            this.VentasGbxIdentificacion = new System.Windows.Forms.GroupBox();
            this.VentasTxtDireccionComprador = new System.Windows.Forms.TextBox();
            this.VentasLblDireccionComprador = new System.Windows.Forms.Label();
            this.VentasTxtNombreComprador = new System.Windows.Forms.TextBox();
            this.VentasLblNombreComprador = new System.Windows.Forms.Label();
            this.VentasTxtNitReceptor = new System.Windows.Forms.TextBox();
            this.VentasLblNitReceptor = new System.Windows.Forms.Label();
            this.VentasRdoCodigoCliente = new System.Windows.Forms.RadioButton();
            this.VentasRdoNit = new System.Windows.Forms.RadioButton();
            this.VentasLblItems = new System.Windows.Forms.Label();
            this.VentasLblCodigoProducto = new System.Windows.Forms.Label();
            this.VentasCboCodigoProducto = new System.Windows.Forms.ComboBox();
            this.VentasLblNombreProducto = new System.Windows.Forms.Label();
            this.VentasCboNombreProducto = new System.Windows.Forms.ComboBox();
            this.VentasLblCantidad = new System.Windows.Forms.Label();
            this.VentasTxtCantidad = new System.Windows.Forms.TextBox();
            this.VentasLblPrecioUnitario = new System.Windows.Forms.Label();
            this.VentasTxtPrecioUnitario = new System.Windows.Forms.TextBox();
            this.VentasLblDescuentos = new System.Windows.Forms.Label();
            this.VentasTxtDescuentos = new System.Windows.Forms.TextBox();
            this.VentasLblSubtotalLinea = new System.Windows.Forms.Label();
            this.VentasTxtSubtotalLinea = new System.Windows.Forms.TextBox();
            this.VentasLblIvaLinea = new System.Windows.Forms.Label();
            this.VentasTxtIvaLinea = new System.Windows.Forms.TextBox();
            this.VentasLblTotalLinea = new System.Windows.Forms.Label();
            this.VentasTxtTotal = new System.Windows.Forms.TextBox();
            this.VentasDgvItems = new System.Windows.Forms.DataGridView();
            this.VentasPnlResumen = new System.Windows.Forms.Panel();
            this.VentasLblSubtotalResumen = new System.Windows.Forms.Label();
            this.VentasLblImpuestosResumen = new System.Windows.Forms.Label();
            this.VentasLblTotalGeneralResumen = new System.Windows.Forms.Label();
            this.VentasPnlEncabezado.SuspendLayout();
            this.VentasGbxOrigenDocumento.SuspendLayout();
            this.VentasGbxCabecera.SuspendLayout();
            this.VentasGbxIdentificacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvItems)).BeginInit();
            this.VentasPnlResumen.SuspendLayout();
            this.SuspendLayout();
            // 
            // VentasBtnAgregarItem
            // 
            this.VentasBtnAgregarItem.AccessibleDescription = "Agregar ítem";
            this.VentasBtnAgregarItem.AccessibleName = "Agregar ítem";
            this.VentasBtnAgregarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnAgregarItem.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.BotonOriginalIngresarVentas;
            this.VentasBtnAgregarItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAgregarItem.FlatAppearance.BorderSize = 0;
            this.VentasBtnAgregarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAgregarItem.Location = new System.Drawing.Point(1195, 378);
            this.VentasBtnAgregarItem.Name = "VentasBtnAgregarItem";
            this.VentasBtnAgregarItem.Size = new System.Drawing.Size(52, 52);
            this.VentasBtnAgregarItem.TabIndex = 18;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnAgregarItem, "Agregar ítem a la factura");
            this.VentasBtnAgregarItem.UseVisualStyleBackColor = false;
            this.VentasBtnAgregarItem.Click += new System.EventHandler(this.VentasBtnAgregarItem_Click);
            // 
            // VentasBtnEmitirFactura
            // 
            this.VentasBtnEmitirFactura.AccessibleDescription = "Guardar y emitir factura";
            this.VentasBtnEmitirFactura.AccessibleName = "Guardar y emitir factura";
            this.VentasBtnEmitirFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnEmitirFactura.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.GuardarVentas;
            this.VentasBtnEmitirFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnEmitirFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnEmitirFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnEmitirFactura.Location = new System.Drawing.Point(20, 640);
            this.VentasBtnEmitirFactura.Name = "VentasBtnEmitirFactura";
            this.VentasBtnEmitirFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnEmitirFactura.TabIndex = 21;
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnEmitirFactura, "Guardar Factura y Descontar Stock");
            this.VentasBtnEmitirFactura.UseVisualStyleBackColor = false;
            this.VentasBtnEmitirFactura.Click += new System.EventHandler(this.VentasBtnEmitirFactura_Click);
            // 
            // VentasPnlEncabezado
            // 
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblCodigoFormulario);
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1280, 70);
            this.VentasPnlEncabezado.TabIndex = 0;
            // 
            // VentasLblTitulo
            // 
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 17F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(20, 10);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(172, 40);
            this.VentasLblTitulo.TabIndex = 0;
            this.VentasLblTitulo.Text = "Facturas";
            // 
            // VentasLblSubtitulo
            // 
            this.VentasLblSubtitulo.AutoSize = true;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.Gainsboro;
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(23, 44);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(418, 22);
            this.VentasLblSubtitulo.TabIndex = 1;
            this.VentasLblSubtitulo.Text = "Emisión de Documento Tributario Electrónico (DTE)";
            // 
            // VentasLblCodigoFormulario
            // 
            this.VentasLblCodigoFormulario.BackColor = System.Drawing.Color.Goldenrod;
            this.VentasLblCodigoFormulario.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.VentasLblCodigoFormulario.ForeColor = System.Drawing.Color.Black;
            this.VentasLblCodigoFormulario.Location = new System.Drawing.Point(1135, 18);
            this.VentasLblCodigoFormulario.Name = "VentasLblCodigoFormulario";
            this.VentasLblCodigoFormulario.Size = new System.Drawing.Size(115, 34);
            this.VentasLblCodigoFormulario.TabIndex = 2;
            this.VentasLblCodigoFormulario.Text = "7007";
            this.VentasLblCodigoFormulario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VentasGbxOrigenDocumento
            // 
            this.VentasGbxOrigenDocumento.Controls.Add(this.VentasBtnCargarDocumento);
            this.VentasGbxOrigenDocumento.Controls.Add(this.VentasCboDocumentoOrigen);
            this.VentasGbxOrigenDocumento.Controls.Add(this.VentasLblDocOrigen);
            this.VentasGbxOrigenDocumento.Controls.Add(this.VentasCboTipoOrigen);
            this.VentasGbxOrigenDocumento.Controls.Add(this.VentasLblTipoOrigen);
            this.VentasGbxOrigenDocumento.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.VentasGbxOrigenDocumento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasGbxOrigenDocumento.Location = new System.Drawing.Point(20, 80);
            this.VentasGbxOrigenDocumento.Name = "VentasGbxOrigenDocumento";
            this.VentasGbxOrigenDocumento.Size = new System.Drawing.Size(1230, 65);
            this.VentasGbxOrigenDocumento.TabIndex = 3;
            this.VentasGbxOrigenDocumento.TabStop = false;
            this.VentasGbxOrigenDocumento.Text = " Origen del Documento (Cotización / Pedido) ";
            // 
            // VentasBtnCargarDocumento
            // 
            this.VentasBtnCargarDocumento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnCargarDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCargarDocumento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.VentasBtnCargarDocumento.ForeColor = System.Drawing.Color.White;
            this.VentasBtnCargarDocumento.Location = new System.Drawing.Point(735, 23);
            this.VentasBtnCargarDocumento.Name = "VentasBtnCargarDocumento";
            this.VentasBtnCargarDocumento.Size = new System.Drawing.Size(130, 32);
            this.VentasBtnCargarDocumento.TabIndex = 6;
            this.VentasBtnCargarDocumento.Text = "Cargar Datos";
            this.VentasBtnCargarDocumento.UseVisualStyleBackColor = false;
            // 
            // VentasCboDocumentoOrigen
            // 
            this.VentasCboDocumentoOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboDocumentoOrigen.FormattingEnabled = true;
            this.VentasCboDocumentoOrigen.Location = new System.Drawing.Point(455, 25);
            this.VentasCboDocumentoOrigen.Name = "VentasCboDocumentoOrigen";
            this.VentasCboDocumentoOrigen.Size = new System.Drawing.Size(260, 33);
            this.VentasCboDocumentoOrigen.TabIndex = 5;
            // 
            // VentasLblDocOrigen
            // 
            this.VentasLblDocOrigen.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDocOrigen.Location = new System.Drawing.Point(300, 28);
            this.VentasLblDocOrigen.Name = "VentasLblDocOrigen";
            this.VentasLblDocOrigen.Size = new System.Drawing.Size(150, 22);
            this.VentasLblDocOrigen.TabIndex = 7;
            this.VentasLblDocOrigen.Text = "Documento Activo";
            // 
            // VentasCboTipoOrigen
            // 
            this.VentasCboTipoOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboTipoOrigen.FormattingEnabled = true;
            this.VentasCboTipoOrigen.Items.AddRange(new object[] {
            "Manual",
            "Cotización",
            "Pedido"});
            this.VentasCboTipoOrigen.Location = new System.Drawing.Point(120, 25);
            this.VentasCboTipoOrigen.Name = "VentasCboTipoOrigen";
            this.VentasCboTipoOrigen.Size = new System.Drawing.Size(155, 33);
            this.VentasCboTipoOrigen.TabIndex = 4;
            // 
            // VentasLblTipoOrigen
            // 
            this.VentasLblTipoOrigen.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblTipoOrigen.Location = new System.Drawing.Point(20, 28);
            this.VentasLblTipoOrigen.Name = "VentasLblTipoOrigen";
            this.VentasLblTipoOrigen.Size = new System.Drawing.Size(95, 22);
            this.VentasLblTipoOrigen.TabIndex = 8;
            this.VentasLblTipoOrigen.Text = "Tipo Origen";
            // 
            // VentasGbxCabecera
            // 
            this.VentasGbxCabecera.Controls.Add(this.VentasCboCondicionPago);
            this.VentasGbxCabecera.Controls.Add(this.VentasLblCondicionPago);
            this.VentasGbxCabecera.Controls.Add(this.VentasCboMoneda);
            this.VentasGbxCabecera.Controls.Add(this.VentasLblMoneda);
            this.VentasGbxCabecera.Controls.Add(this.VentasCboTipoDte);
            this.VentasGbxCabecera.Controls.Add(this.VentasLblTipoDte);
            this.VentasGbxCabecera.Controls.Add(this.VentasDtpFechaEmision);
            this.VentasGbxCabecera.Controls.Add(this.VentasLblFechaEmision);
            this.VentasGbxCabecera.Controls.Add(this.VentasTxtEstablecimiento);
            this.VentasGbxCabecera.Controls.Add(this.VentasLblEstablecimiento);
            this.VentasGbxCabecera.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.VentasGbxCabecera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasGbxCabecera.Location = new System.Drawing.Point(20, 150);
            this.VentasGbxCabecera.Name = "VentasGbxCabecera";
            this.VentasGbxCabecera.Size = new System.Drawing.Size(1230, 75);
            this.VentasGbxCabecera.TabIndex = 7;
            this.VentasGbxCabecera.TabStop = false;
            this.VentasGbxCabecera.Text = " Parámetros de Emisión y Pago ";
            // 
            // VentasCboCondicionPago
            // 
            this.VentasCboCondicionPago.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboCondicionPago.FormattingEnabled = true;
            this.VentasCboCondicionPago.Items.AddRange(new object[] {
            "CONTADO",
            "CREDITO"});
            this.VentasCboCondicionPago.Location = new System.Drawing.Point(1090, 30);
            this.VentasCboCondicionPago.Name = "VentasCboCondicionPago";
            this.VentasCboCondicionPago.Size = new System.Drawing.Size(125, 33);
            this.VentasCboCondicionPago.TabIndex = 0;
            // 
            // VentasLblCondicionPago
            // 
            this.VentasLblCondicionPago.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCondicionPago.Location = new System.Drawing.Point(1035, 33);
            this.VentasLblCondicionPago.Name = "VentasLblCondicionPago";
            this.VentasLblCondicionPago.Size = new System.Drawing.Size(50, 22);
            this.VentasLblCondicionPago.TabIndex = 1;
            this.VentasLblCondicionPago.Text = "Pago";
            // 
            // VentasCboMoneda
            // 
            this.VentasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboMoneda.FormattingEnabled = true;
            this.VentasCboMoneda.Items.AddRange(new object[] {
            "GTQ-Quetzal",
            "USD-Dólar estadounidense"});
            this.VentasCboMoneda.Location = new System.Drawing.Point(870, 30);
            this.VentasCboMoneda.Name = "VentasCboMoneda";
            this.VentasCboMoneda.Size = new System.Drawing.Size(150, 33);
            this.VentasCboMoneda.TabIndex = 2;
            // 
            // VentasLblMoneda
            // 
            this.VentasLblMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblMoneda.Location = new System.Drawing.Point(805, 33);
            this.VentasLblMoneda.Name = "VentasLblMoneda";
            this.VentasLblMoneda.Size = new System.Drawing.Size(60, 22);
            this.VentasLblMoneda.TabIndex = 3;
            this.VentasLblMoneda.Text = "Moneda";
            // 
            // VentasCboTipoDte
            // 
            this.VentasCboTipoDte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboTipoDte.FormattingEnabled = true;
            this.VentasCboTipoDte.Items.AddRange(new object[] {
            "Factura"});
            this.VentasCboTipoDte.Location = new System.Drawing.Point(660, 30);
            this.VentasCboTipoDte.Name = "VentasCboTipoDte";
            this.VentasCboTipoDte.Size = new System.Drawing.Size(130, 33);
            this.VentasCboTipoDte.TabIndex = 4;
            // 
            // VentasLblTipoDte
            // 
            this.VentasLblTipoDte.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblTipoDte.Location = new System.Drawing.Point(585, 33);
            this.VentasLblTipoDte.Name = "VentasLblTipoDte";
            this.VentasLblTipoDte.Size = new System.Drawing.Size(70, 22);
            this.VentasLblTipoDte.TabIndex = 5;
            this.VentasLblTipoDte.Text = "Tipo DTE";
            // 
            // VentasDtpFechaEmision
            // 
            this.VentasDtpFechaEmision.CustomFormat = "dd/MM/yyyy HH:mm";
            this.VentasDtpFechaEmision.Enabled = false;
            this.VentasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaEmision.Location = new System.Drawing.Point(420, 30);
            this.VentasDtpFechaEmision.Name = "VentasDtpFechaEmision";
            this.VentasDtpFechaEmision.Size = new System.Drawing.Size(150, 31);
            this.VentasDtpFechaEmision.TabIndex = 6;
            // 
            // VentasLblFechaEmision
            // 
            this.VentasLblFechaEmision.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblFechaEmision.Location = new System.Drawing.Point(330, 33);
            this.VentasLblFechaEmision.Name = "VentasLblFechaEmision";
            this.VentasLblFechaEmision.Size = new System.Drawing.Size(85, 22);
            this.VentasLblFechaEmision.TabIndex = 7;
            this.VentasLblFechaEmision.Text = "Fecha/Hora";
            // 
            // VentasTxtEstablecimiento
            // 
            this.VentasTxtEstablecimiento.Enabled = false;
            this.VentasTxtEstablecimiento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.VentasTxtEstablecimiento.Location = new System.Drawing.Point(130, 30);
            this.VentasTxtEstablecimiento.Name = "VentasTxtEstablecimiento";
            this.VentasTxtEstablecimiento.Size = new System.Drawing.Size(185, 31);
            this.VentasTxtEstablecimiento.TabIndex = 8;
            this.VentasTxtEstablecimiento.Text = "Embutidos de Calidad, S.A.";
            // 
            // VentasLblEstablecimiento
            // 
            this.VentasLblEstablecimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblEstablecimiento.Location = new System.Drawing.Point(15, 33);
            this.VentasLblEstablecimiento.Name = "VentasLblEstablecimiento";
            this.VentasLblEstablecimiento.Size = new System.Drawing.Size(110, 22);
            this.VentasLblEstablecimiento.TabIndex = 9;
            this.VentasLblEstablecimiento.Text = "Establecimiento";
            // 
            // VentasGbxIdentificacion
            // 
            this.VentasGbxIdentificacion.Controls.Add(this.VentasTxtDireccionComprador);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasLblDireccionComprador);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasTxtNombreComprador);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasLblNombreComprador);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasTxtNitReceptor);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasLblNitReceptor);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasRdoCodigoCliente);
            this.VentasGbxIdentificacion.Controls.Add(this.VentasRdoNit);
            this.VentasGbxIdentificacion.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.VentasGbxIdentificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasGbxIdentificacion.Location = new System.Drawing.Point(20, 230);
            this.VentasGbxIdentificacion.Name = "VentasGbxIdentificacion";
            this.VentasGbxIdentificacion.Size = new System.Drawing.Size(1230, 100);
            this.VentasGbxIdentificacion.TabIndex = 8;
            this.VentasGbxIdentificacion.TabStop = false;
            this.VentasGbxIdentificacion.Text = " Datos del Receptor / Comprador ";
            // 
            // VentasTxtDireccionComprador
            // 
            this.VentasTxtDireccionComprador.Enabled = false;
            this.VentasTxtDireccionComprador.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtDireccionComprador.Location = new System.Drawing.Point(120, 62);
            this.VentasTxtDireccionComprador.Name = "VentasTxtDireccionComprador";
            this.VentasTxtDireccionComprador.ReadOnly = true;
            this.VentasTxtDireccionComprador.Size = new System.Drawing.Size(1095, 31);
            this.VentasTxtDireccionComprador.TabIndex = 0;
            // 
            // VentasLblDireccionComprador
            // 
            this.VentasLblDireccionComprador.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDireccionComprador.Location = new System.Drawing.Point(20, 65);
            this.VentasLblDireccionComprador.Name = "VentasLblDireccionComprador";
            this.VentasLblDireccionComprador.Size = new System.Drawing.Size(95, 22);
            this.VentasLblDireccionComprador.TabIndex = 1;
            this.VentasLblDireccionComprador.Text = "Dirección";
            // 
            // VentasTxtNombreComprador
            // 
            this.VentasTxtNombreComprador.BackColor = System.Drawing.Color.WhiteSmoke;
            this.VentasTxtNombreComprador.Enabled = false;
            this.VentasTxtNombreComprador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.VentasTxtNombreComprador.Location = new System.Drawing.Point(640, 27);
            this.VentasTxtNombreComprador.Name = "VentasTxtNombreComprador";
            this.VentasTxtNombreComprador.ReadOnly = true;
            this.VentasTxtNombreComprador.Size = new System.Drawing.Size(575, 31);
            this.VentasTxtNombreComprador.TabIndex = 2;
            // 
            // VentasLblNombreComprador
            // 
            this.VentasLblNombreComprador.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblNombreComprador.Location = new System.Drawing.Point(500, 30);
            this.VentasLblNombreComprador.Name = "VentasLblNombreComprador";
            this.VentasLblNombreComprador.Size = new System.Drawing.Size(135, 22);
            this.VentasLblNombreComprador.TabIndex = 3;
            this.VentasLblNombreComprador.Text = "Nombre Receptor:";
            // 
            // VentasTxtNitReceptor
            // 
            this.VentasTxtNitReceptor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtNitReceptor.Location = new System.Drawing.Point(325, 27);
            this.VentasTxtNitReceptor.Name = "VentasTxtNitReceptor";
            this.VentasTxtNitReceptor.Size = new System.Drawing.Size(160, 31);
            this.VentasTxtNitReceptor.TabIndex = 4;
            this.VentasTxtNitReceptor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VentasTxtNitReceptor_KeyPress);
            // 
            // VentasLblNitReceptor
            // 
            this.VentasLblNitReceptor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblNitReceptor.Location = new System.Drawing.Point(225, 30);
            this.VentasLblNitReceptor.Name = "VentasLblNitReceptor";
            this.VentasLblNitReceptor.Size = new System.Drawing.Size(95, 22);
            this.VentasLblNitReceptor.TabIndex = 5;
            this.VentasLblNitReceptor.Text = "NIT / Código";
            // 
            // VentasRdoCodigoCliente
            // 
            this.VentasRdoCodigoCliente.AutoSize = true;
            this.VentasRdoCodigoCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasRdoCodigoCliente.Location = new System.Drawing.Point(90, 28);
            this.VentasRdoCodigoCliente.Name = "VentasRdoCodigoCliente";
            this.VentasRdoCodigoCliente.Size = new System.Drawing.Size(130, 26);
            this.VentasRdoCodigoCliente.TabIndex = 6;
            this.VentasRdoCodigoCliente.Text = "Cód. Cliente";
            // 
            // VentasRdoNit
            // 
            this.VentasRdoNit.AutoSize = true;
            this.VentasRdoNit.Checked = true;
            this.VentasRdoNit.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasRdoNit.Location = new System.Drawing.Point(20, 28);
            this.VentasRdoNit.Name = "VentasRdoNit";
            this.VentasRdoNit.Size = new System.Drawing.Size(65, 26);
            this.VentasRdoNit.TabIndex = 7;
            this.VentasRdoNit.TabStop = true;
            this.VentasRdoNit.Text = "NIT";
            // 
            // VentasLblItems
            // 
            this.VentasLblItems.AutoSize = true;
            this.VentasLblItems.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.VentasLblItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblItems.Location = new System.Drawing.Point(20, 338);
            this.VentasLblItems.Name = "VentasLblItems";
            this.VentasLblItems.Size = new System.Drawing.Size(284, 31);
            this.VentasLblItems.TabIndex = 9;
            this.VentasLblItems.Text = "Detalle de productos";
            // 
            // VentasLblCodigoProducto
            // 
            this.VentasLblCodigoProducto.AutoSize = true;
            this.VentasLblCodigoProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCodigoProducto.Location = new System.Drawing.Point(20, 375);
            this.VentasLblCodigoProducto.Name = "VentasLblCodigoProducto";
            this.VentasLblCodigoProducto.Size = new System.Drawing.Size(112, 22);
            this.VentasLblCodigoProducto.TabIndex = 29;
            this.VentasLblCodigoProducto.Text = "Código Prod.";
            // 
            // VentasCboCodigoProducto
            // 
            this.VentasCboCodigoProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboCodigoProducto.FormattingEnabled = true;
            this.VentasCboCodigoProducto.Location = new System.Drawing.Point(20, 398);
            this.VentasCboCodigoProducto.Name = "VentasCboCodigoProducto";
            this.VentasCboCodigoProducto.Size = new System.Drawing.Size(120, 33);
            this.VentasCboCodigoProducto.TabIndex = 10;
            // 
            // VentasLblNombreProducto
            // 
            this.VentasLblNombreProducto.AutoSize = true;
            this.VentasLblNombreProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblNombreProducto.Location = new System.Drawing.Point(150, 375);
            this.VentasLblNombreProducto.Name = "VentasLblNombreProducto";
            this.VentasLblNombreProducto.Size = new System.Drawing.Size(180, 22);
            this.VentasLblNombreProducto.TabIndex = 28;
            this.VentasLblNombreProducto.Text = "Nombre / Descripción";
            // 
            // VentasCboNombreProducto
            // 
            this.VentasCboNombreProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasCboNombreProducto.FormattingEnabled = true;
            this.VentasCboNombreProducto.Location = new System.Drawing.Point(150, 398);
            this.VentasCboNombreProducto.Name = "VentasCboNombreProducto";
            this.VentasCboNombreProducto.Size = new System.Drawing.Size(300, 33);
            this.VentasCboNombreProducto.TabIndex = 11;
            // 
            // VentasLblCantidad
            // 
            this.VentasLblCantidad.AutoSize = true;
            this.VentasLblCantidad.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblCantidad.Location = new System.Drawing.Point(460, 375);
            this.VentasLblCantidad.Name = "VentasLblCantidad";
            this.VentasLblCantidad.Size = new System.Drawing.Size(79, 22);
            this.VentasLblCantidad.TabIndex = 27;
            this.VentasLblCantidad.Text = "Cantidad";
            // 
            // VentasTxtCantidad
            // 
            this.VentasTxtCantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtCantidad.Location = new System.Drawing.Point(460, 398);
            this.VentasTxtCantidad.Name = "VentasTxtCantidad";
            this.VentasTxtCantidad.Size = new System.Drawing.Size(85, 31);
            this.VentasTxtCantidad.TabIndex = 12;
            this.VentasTxtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VentasTxtCantidad_KeyPress);
            // 
            // VentasLblPrecioUnitario
            // 
            this.VentasLblPrecioUnitario.AutoSize = true;
            this.VentasLblPrecioUnitario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblPrecioUnitario.Location = new System.Drawing.Point(555, 375);
            this.VentasLblPrecioUnitario.Name = "VentasLblPrecioUnitario";
            this.VentasLblPrecioUnitario.Size = new System.Drawing.Size(100, 22);
            this.VentasLblPrecioUnitario.TabIndex = 26;
            this.VentasLblPrecioUnitario.Text = "Precio Unit.";
            // 
            // VentasTxtPrecioUnitario
            // 
            this.VentasTxtPrecioUnitario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtPrecioUnitario.Location = new System.Drawing.Point(555, 398);
            this.VentasTxtPrecioUnitario.Name = "VentasTxtPrecioUnitario";
            this.VentasTxtPrecioUnitario.Size = new System.Drawing.Size(110, 31);
            this.VentasTxtPrecioUnitario.TabIndex = 13;
            // 
            // VentasLblDescuentos
            // 
            this.VentasLblDescuentos.AutoSize = true;
            this.VentasLblDescuentos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDescuentos.Location = new System.Drawing.Point(675, 375);
            this.VentasLblDescuentos.Name = "VentasLblDescuentos";
            this.VentasLblDescuentos.Size = new System.Drawing.Size(92, 22);
            this.VentasLblDescuentos.TabIndex = 25;
            this.VentasLblDescuentos.Text = "Descuento";
            // 
            // VentasTxtDescuentos
            // 
            this.VentasTxtDescuentos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtDescuentos.Location = new System.Drawing.Point(675, 398);
            this.VentasTxtDescuentos.Name = "VentasTxtDescuentos";
            this.VentasTxtDescuentos.Size = new System.Drawing.Size(100, 31);
            this.VentasTxtDescuentos.TabIndex = 14;
            // 
            // VentasLblSubtotalLinea
            // 
            this.VentasLblSubtotalLinea.AutoSize = true;
            this.VentasLblSubtotalLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtotalLinea.Location = new System.Drawing.Point(785, 375);
            this.VentasLblSubtotalLinea.Name = "VentasLblSubtotalLinea";
            this.VentasLblSubtotalLinea.Size = new System.Drawing.Size(75, 22);
            this.VentasLblSubtotalLinea.TabIndex = 24;
            this.VentasLblSubtotalLinea.Text = "Subtotal";
            // 
            // VentasTxtSubtotalLinea
            // 
            this.VentasTxtSubtotalLinea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtSubtotalLinea.Location = new System.Drawing.Point(785, 398);
            this.VentasTxtSubtotalLinea.Name = "VentasTxtSubtotalLinea";
            this.VentasTxtSubtotalLinea.Size = new System.Drawing.Size(105, 31);
            this.VentasTxtSubtotalLinea.TabIndex = 15;
            // 
            // VentasLblIvaLinea
            // 
            this.VentasLblIvaLinea.AutoSize = true;
            this.VentasLblIvaLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblIvaLinea.Location = new System.Drawing.Point(900, 375);
            this.VentasLblIvaLinea.Name = "VentasLblIvaLinea";
            this.VentasLblIvaLinea.Size = new System.Drawing.Size(72, 22);
            this.VentasLblIvaLinea.TabIndex = 23;
            this.VentasLblIvaLinea.Text = "IVA (Q)";
            // 
            // VentasTxtIvaLinea
            // 
            this.VentasTxtIvaLinea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtIvaLinea.Location = new System.Drawing.Point(900, 398);
            this.VentasTxtIvaLinea.Name = "VentasTxtIvaLinea";
            this.VentasTxtIvaLinea.Size = new System.Drawing.Size(95, 31);
            this.VentasTxtIvaLinea.TabIndex = 16;
            // 
            // VentasLblTotalLinea
            // 
            this.VentasLblTotalLinea.AutoSize = true;
            this.VentasLblTotalLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblTotalLinea.Location = new System.Drawing.Point(1005, 375);
            this.VentasLblTotalLinea.Name = "VentasLblTotalLinea";
            this.VentasLblTotalLinea.Size = new System.Drawing.Size(50, 22);
            this.VentasLblTotalLinea.TabIndex = 22;
            this.VentasLblTotalLinea.Text = "Total";
            // 
            // VentasTxtTotal
            // 
            this.VentasTxtTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasTxtTotal.Location = new System.Drawing.Point(1005, 398);
            this.VentasTxtTotal.Name = "VentasTxtTotal";
            this.VentasTxtTotal.Size = new System.Drawing.Size(115, 31);
            this.VentasTxtTotal.TabIndex = 17;
            // 
            // VentasDgvItems
            // 
            this.VentasDgvItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.VentasDgvItems.Location = new System.Drawing.Point(20, 440);
            this.VentasDgvItems.Name = "VentasDgvItems";
            this.VentasDgvItems.RowHeadersWidth = 51;
            this.VentasDgvItems.RowTemplate.Height = 26;
            this.VentasDgvItems.Size = new System.Drawing.Size(1230, 175);
            this.VentasDgvItems.TabIndex = 19;
            // 
            // VentasPnlResumen
            // 
            this.VentasPnlResumen.BackColor = System.Drawing.Color.White;
            this.VentasPnlResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VentasPnlResumen.Controls.Add(this.VentasLblSubtotalResumen);
            this.VentasPnlResumen.Controls.Add(this.VentasLblImpuestosResumen);
            this.VentasPnlResumen.Controls.Add(this.VentasLblTotalGeneralResumen);
            this.VentasPnlResumen.Location = new System.Drawing.Point(900, 625);
            this.VentasPnlResumen.Name = "VentasPnlResumen";
            this.VentasPnlResumen.Size = new System.Drawing.Size(350, 110);
            this.VentasPnlResumen.TabIndex = 20;
            // 
            // VentasLblSubtotalResumen
            // 
            this.VentasLblSubtotalResumen.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.VentasLblSubtotalResumen.Location = new System.Drawing.Point(15, 10);
            this.VentasLblSubtotalResumen.Name = "VentasLblSubtotalResumen";
            this.VentasLblSubtotalResumen.Size = new System.Drawing.Size(320, 22);
            this.VentasLblSubtotalResumen.TabIndex = 0;
            this.VentasLblSubtotalResumen.Text = "Subtotal:                              Q 0.00";
            // 
            // VentasLblImpuestosResumen
            // 
            this.VentasLblImpuestosResumen.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.VentasLblImpuestosResumen.Location = new System.Drawing.Point(15, 38);
            this.VentasLblImpuestosResumen.Name = "VentasLblImpuestosResumen";
            this.VentasLblImpuestosResumen.Size = new System.Drawing.Size(320, 22);
            this.VentasLblImpuestosResumen.TabIndex = 1;
            this.VentasLblImpuestosResumen.Text = "Impuestos (IVA):                Q 0.00";
            // 
            // VentasLblTotalGeneralResumen
            // 
            this.VentasLblTotalGeneralResumen.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.VentasLblTotalGeneralResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTotalGeneralResumen.Location = new System.Drawing.Point(15, 70);
            this.VentasLblTotalGeneralResumen.Name = "VentasLblTotalGeneralResumen";
            this.VentasLblTotalGeneralResumen.Size = new System.Drawing.Size(320, 28);
            this.VentasLblTotalGeneralResumen.TabIndex = 2;
            this.VentasLblTotalGeneralResumen.Text = "TOTAL DE LA FACTURA";
            // 
            // FrmFacturas
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1275, 755);
            this.Controls.Add(this.VentasBtnEmitirFactura);
            this.Controls.Add(this.VentasPnlResumen);
            this.Controls.Add(this.VentasDgvItems);
            this.Controls.Add(this.VentasBtnAgregarItem);
            this.Controls.Add(this.VentasTxtTotal);
            this.Controls.Add(this.VentasLblTotalLinea);
            this.Controls.Add(this.VentasTxtIvaLinea);
            this.Controls.Add(this.VentasLblIvaLinea);
            this.Controls.Add(this.VentasTxtSubtotalLinea);
            this.Controls.Add(this.VentasLblSubtotalLinea);
            this.Controls.Add(this.VentasTxtDescuentos);
            this.Controls.Add(this.VentasLblDescuentos);
            this.Controls.Add(this.VentasTxtPrecioUnitario);
            this.Controls.Add(this.VentasLblPrecioUnitario);
            this.Controls.Add(this.VentasTxtCantidad);
            this.Controls.Add(this.VentasLblCantidad);
            this.Controls.Add(this.VentasCboNombreProducto);
            this.Controls.Add(this.VentasLblNombreProducto);
            this.Controls.Add(this.VentasCboCodigoProducto);
            this.Controls.Add(this.VentasLblCodigoProducto);
            this.Controls.Add(this.VentasLblItems);
            this.Controls.Add(this.VentasGbxIdentificacion);
            this.Controls.Add(this.VentasGbxCabecera);
            this.Controls.Add(this.VentasGbxOrigenDocumento);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Facturas.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmFacturas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7007 – Facturas";
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            this.VentasGbxOrigenDocumento.ResumeLayout(false);
            this.VentasGbxCabecera.ResumeLayout(false);
            this.VentasGbxCabecera.PerformLayout();
            this.VentasGbxIdentificacion.ResumeLayout(false);
            this.VentasGbxIdentificacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvItems)).EndInit();
            this.VentasPnlResumen.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblCodigoFormulario;

        private System.Windows.Forms.GroupBox VentasGbxOrigenDocumento;
        private System.Windows.Forms.Label VentasLblTipoOrigen;
        private System.Windows.Forms.ComboBox VentasCboTipoOrigen;
        private System.Windows.Forms.Label VentasLblDocOrigen;
        private System.Windows.Forms.ComboBox VentasCboDocumentoOrigen;
        private System.Windows.Forms.Button VentasBtnCargarDocumento;

        private System.Windows.Forms.GroupBox VentasGbxCabecera;
        private System.Windows.Forms.Label VentasLblEstablecimiento;
        private System.Windows.Forms.TextBox VentasTxtEstablecimiento;
        private System.Windows.Forms.Label VentasLblFechaEmision;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaEmision;
        private System.Windows.Forms.Label VentasLblTipoDte;
        private System.Windows.Forms.ComboBox VentasCboTipoDte;
        private System.Windows.Forms.Label VentasLblMoneda;
        private System.Windows.Forms.ComboBox VentasCboMoneda;
        private System.Windows.Forms.Label VentasLblCondicionPago;
        private System.Windows.Forms.ComboBox VentasCboCondicionPago;

        private System.Windows.Forms.GroupBox VentasGbxIdentificacion;
        private System.Windows.Forms.RadioButton VentasRdoNit;
        private System.Windows.Forms.RadioButton VentasRdoCodigoCliente;
        private System.Windows.Forms.Label VentasLblNitReceptor;
        private System.Windows.Forms.TextBox VentasTxtNitReceptor;
        private System.Windows.Forms.Label VentasLblNombreComprador;
        private System.Windows.Forms.TextBox VentasTxtNombreComprador;
        private System.Windows.Forms.Label VentasLblDireccionComprador;
        private System.Windows.Forms.TextBox VentasTxtDireccionComprador;

        private System.Windows.Forms.Label VentasLblItems;
        private System.Windows.Forms.Label VentasLblCodigoProducto;
        private System.Windows.Forms.ComboBox VentasCboCodigoProducto;
        private System.Windows.Forms.Label VentasLblNombreProducto;
        private System.Windows.Forms.ComboBox VentasCboNombreProducto;
        private System.Windows.Forms.Label VentasLblCantidad;
        private System.Windows.Forms.TextBox VentasTxtCantidad;
        private System.Windows.Forms.Label VentasLblPrecioUnitario;
        private System.Windows.Forms.TextBox VentasTxtPrecioUnitario;
        private System.Windows.Forms.Label VentasLblDescuentos;
        private System.Windows.Forms.TextBox VentasTxtDescuentos;
        private System.Windows.Forms.Label VentasLblSubtotalLinea;
        private System.Windows.Forms.TextBox VentasTxtSubtotalLinea;
        private System.Windows.Forms.Label VentasLblIvaLinea;
        private System.Windows.Forms.TextBox VentasTxtIvaLinea;
        private System.Windows.Forms.Label VentasLblTotalLinea;
        private System.Windows.Forms.TextBox VentasTxtTotal;

        private System.Windows.Forms.Button VentasBtnAgregarItem;
        private System.Windows.Forms.DataGridView VentasDgvItems;

        private System.Windows.Forms.Panel VentasPnlResumen;
        private System.Windows.Forms.Label VentasLblSubtotalResumen;
        private System.Windows.Forms.Label VentasLblImpuestosResumen;
        private System.Windows.Forms.Label VentasLblTotalGeneralResumen;

        private System.Windows.Forms.Button VentasBtnEmitirFactura;
        private System.Windows.Forms.ToolTip VentasTtpAccionesOriginales;
    }
}