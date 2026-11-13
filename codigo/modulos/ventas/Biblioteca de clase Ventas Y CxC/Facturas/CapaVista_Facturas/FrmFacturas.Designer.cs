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
        private System.ComponentModel.IContainer _Components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (_Components != null))
            {
                _Components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._Components = new System.ComponentModel.Container();
            this._VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this._Components);
            this._VentasBtnAgregarItem = new System.Windows.Forms.Button();
            this._VentasBtnEmitirFactura = new System.Windows.Forms.Button();
            this._VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this._VentasLblTitulo = new System.Windows.Forms.Label();
            this._VentasLblSubtitulo = new System.Windows.Forms.Label();
            this._VentasLblCodigoFormulario = new System.Windows.Forms.Label();
            this._VentasGbxOrigenDocumento = new System.Windows.Forms.GroupBox();
            this._VentasBtnCargarDocumento = new System.Windows.Forms.Button();
            this._VentasCboDocumentoOrigen = new System.Windows.Forms.ComboBox();
            this._VentasLblDocOrigen = new System.Windows.Forms.Label();
            this._VentasCboTipoOrigen = new System.Windows.Forms.ComboBox();
            this._VentasLblTipoOrigen = new System.Windows.Forms.Label();
            this._VentasGbxCabecera = new System.Windows.Forms.GroupBox();
            this._VentasCboCondicionPago = new System.Windows.Forms.ComboBox();
            this._VentasLblCondicionPago = new System.Windows.Forms.Label();
            this._VentasCboMoneda = new System.Windows.Forms.ComboBox();
            this._VentasLblMoneda = new System.Windows.Forms.Label();
            this._VentasCboTipoDte = new System.Windows.Forms.ComboBox();
            this._VentasLblTipoDte = new System.Windows.Forms.Label();
            this._VentasDtpFechaEmision = new System.Windows.Forms.DateTimePicker();
            this._VentasLblFechaEmision = new System.Windows.Forms.Label();
            this._VentasTxtEstablecimiento = new System.Windows.Forms.TextBox();
            this._VentasLblEstablecimiento = new System.Windows.Forms.Label();
            this._VentasGbxIdentificacion = new System.Windows.Forms.GroupBox();
            this._VentasTxtDireccionComprador = new System.Windows.Forms.TextBox();
            this._VentasLblDireccionComprador = new System.Windows.Forms.Label();
            this._VentasTxtNombreComprador = new System.Windows.Forms.TextBox();
            this._VentasLblNombreComprador = new System.Windows.Forms.Label();
            this._VentasTxtNitReceptor = new System.Windows.Forms.TextBox();
            this._VentasLblNitReceptor = new System.Windows.Forms.Label();
            this._VentasRdoCodigoCliente = new System.Windows.Forms.RadioButton();
            this._VentasRdoNit = new System.Windows.Forms.RadioButton();
            this._VentasLblItems = new System.Windows.Forms.Label();
            this._VentasLblCodigoProducto = new System.Windows.Forms.Label();
            this._VentasCboCodigoProducto = new System.Windows.Forms.ComboBox();
            this._VentasLblNombreProducto = new System.Windows.Forms.Label();
            this._VentasCboNombreProducto = new System.Windows.Forms.ComboBox();
            this._VentasLblCantidad = new System.Windows.Forms.Label();
            this._VentasTxtCantidad = new System.Windows.Forms.TextBox();
            this._VentasLblPrecioUnitario = new System.Windows.Forms.Label();
            this._VentasTxtPrecioUnitario = new System.Windows.Forms.TextBox();
            this._VentasLblDescuentos = new System.Windows.Forms.Label();
            this._VentasTxtDescuentos = new System.Windows.Forms.TextBox();
            this._VentasLblSubtotalLinea = new System.Windows.Forms.Label();
            this._VentasTxtSubtotalLinea = new System.Windows.Forms.TextBox();
            this._VentasLblIvaLinea = new System.Windows.Forms.Label();
            this._VentasTxtIvaLinea = new System.Windows.Forms.TextBox();
            this._VentasLblTotalLinea = new System.Windows.Forms.Label();
            this._VentasTxtTotal = new System.Windows.Forms.TextBox();
            this._VentasDgvItems = new System.Windows.Forms.DataGridView();
            this._VentasPnlResumen = new System.Windows.Forms.Panel();
            this._VentasLblSubtotalResumen = new System.Windows.Forms.Label();
            this._VentasLblImpuestosResumen = new System.Windows.Forms.Label();
            this._VentasLblTotalGeneralResumen = new System.Windows.Forms.Label();
            this._VentasPnlEncabezado.SuspendLayout();
            this._VentasGbxOrigenDocumento.SuspendLayout();
            this._VentasGbxCabecera.SuspendLayout();
            this._VentasGbxIdentificacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._VentasDgvItems)).BeginInit();
            this._VentasPnlResumen.SuspendLayout();
            this.SuspendLayout();
            // 
            // VentasBtnAgregarItem
            // 
            this._VentasBtnAgregarItem.AccessibleDescription = "Agregar ítem";
            this._VentasBtnAgregarItem.AccessibleName = "Agregar ítem";
            this._VentasBtnAgregarItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this._VentasBtnAgregarItem.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.BotonOriginalIngresarVentas;
            this._VentasBtnAgregarItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._VentasBtnAgregarItem.FlatAppearance.BorderSize = 0;
            this._VentasBtnAgregarItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._VentasBtnAgregarItem.Location = new System.Drawing.Point(1195, 378);
            this._VentasBtnAgregarItem.Name = "VentasBtnAgregarItem";
            this._VentasBtnAgregarItem.Size = new System.Drawing.Size(52, 52);
            this._VentasBtnAgregarItem.TabIndex = 18;
            this._VentasTtpAccionesOriginales.SetToolTip(this._VentasBtnAgregarItem, "Agregar ítem a la factura");
            this._VentasBtnAgregarItem.UseVisualStyleBackColor = false;
            this._VentasBtnAgregarItem.Click += new System.EventHandler(this.VentasMetBtnAgregarItemClick);
            // 
            // VentasBtnEmitirFactura
            // 
            this._VentasBtnEmitirFactura.AccessibleDescription = "Guardar y emitir factura";
            this._VentasBtnEmitirFactura.AccessibleName = "Guardar y emitir factura";
            this._VentasBtnEmitirFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this._VentasBtnEmitirFactura.BackgroundImage = global::CapaVista_Facturas.Properties.Resources.GuardarVentas;
            this._VentasBtnEmitirFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this._VentasBtnEmitirFactura.FlatAppearance.BorderSize = 0;
            this._VentasBtnEmitirFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._VentasBtnEmitirFactura.Location = new System.Drawing.Point(20, 640);
            this._VentasBtnEmitirFactura.Name = "VentasBtnEmitirFactura";
            this._VentasBtnEmitirFactura.Size = new System.Drawing.Size(64, 64);
            this._VentasBtnEmitirFactura.TabIndex = 21;
            this._VentasTtpAccionesOriginales.SetToolTip(this._VentasBtnEmitirFactura, "Guardar Factura y Descontar Stock");
            this._VentasBtnEmitirFactura.UseVisualStyleBackColor = false;
            this._VentasBtnEmitirFactura.Click += new System.EventHandler(this.VentasMetBtnEmitirFacturaClick);
            // 
            // VentasPnlEncabezado
            // 
            this._VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasPnlEncabezado.Controls.Add(this._VentasLblTitulo);
            this._VentasPnlEncabezado.Controls.Add(this._VentasLblSubtitulo);
            this._VentasPnlEncabezado.Controls.Add(this._VentasLblCodigoFormulario);
            this._VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this._VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this._VentasPnlEncabezado.Size = new System.Drawing.Size(1280, 70);
            this._VentasPnlEncabezado.TabIndex = 0;
            // 
            // VentasLblTitulo
            // 
            this._VentasLblTitulo.AutoSize = true;
            this._VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 17F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this._VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this._VentasLblTitulo.Location = new System.Drawing.Point(20, 10);
            this._VentasLblTitulo.Name = "VentasLblTitulo";
            this._VentasLblTitulo.Size = new System.Drawing.Size(172, 40);
            this._VentasLblTitulo.TabIndex = 0;
            this._VentasLblTitulo.Text = "Facturas";
            // 
            // VentasLblSubtitulo
            // 
            this._VentasLblSubtitulo.AutoSize = true;
            this._VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblSubtitulo.ForeColor = System.Drawing.Color.Gainsboro;
            this._VentasLblSubtitulo.Location = new System.Drawing.Point(23, 44);
            this._VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this._VentasLblSubtitulo.Size = new System.Drawing.Size(418, 22);
            this._VentasLblSubtitulo.TabIndex = 1;
            this._VentasLblSubtitulo.Text = "Emisión de Documento Tributario Electrónico (DTE)";
            // 
            // VentasLblCodigoFormulario
            // 
            this._VentasLblCodigoFormulario.BackColor = System.Drawing.Color.Goldenrod;
            this._VentasLblCodigoFormulario.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this._VentasLblCodigoFormulario.ForeColor = System.Drawing.Color.Black;
            this._VentasLblCodigoFormulario.Location = new System.Drawing.Point(1135, 18);
            this._VentasLblCodigoFormulario.Name = "VentasLblCodigoFormulario";
            this._VentasLblCodigoFormulario.Size = new System.Drawing.Size(115, 34);
            this._VentasLblCodigoFormulario.TabIndex = 2;
            this._VentasLblCodigoFormulario.Text = "7007";
            this._VentasLblCodigoFormulario.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VentasGbxOrigenDocumento
            // 
            this._VentasGbxOrigenDocumento.Controls.Add(this._VentasBtnCargarDocumento);
            this._VentasGbxOrigenDocumento.Controls.Add(this._VentasCboDocumentoOrigen);
            this._VentasGbxOrigenDocumento.Controls.Add(this._VentasLblDocOrigen);
            this._VentasGbxOrigenDocumento.Controls.Add(this._VentasCboTipoOrigen);
            this._VentasGbxOrigenDocumento.Controls.Add(this._VentasLblTipoOrigen);
            this._VentasGbxOrigenDocumento.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this._VentasGbxOrigenDocumento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasGbxOrigenDocumento.Location = new System.Drawing.Point(20, 80);
            this._VentasGbxOrigenDocumento.Name = "VentasGbxOrigenDocumento";
            this._VentasGbxOrigenDocumento.Size = new System.Drawing.Size(1230, 65);
            this._VentasGbxOrigenDocumento.TabIndex = 3;
            this._VentasGbxOrigenDocumento.TabStop = false;
            this._VentasGbxOrigenDocumento.Text = " Origen del Documento (Cotización / Pedido) ";
            // 
            // VentasBtnCargarDocumento
            // 
            this._VentasBtnCargarDocumento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this._VentasBtnCargarDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._VentasBtnCargarDocumento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._VentasBtnCargarDocumento.ForeColor = System.Drawing.Color.White;
            this._VentasBtnCargarDocumento.Location = new System.Drawing.Point(735, 23);
            this._VentasBtnCargarDocumento.Name = "VentasBtnCargarDocumento";
            this._VentasBtnCargarDocumento.Size = new System.Drawing.Size(130, 32);
            this._VentasBtnCargarDocumento.TabIndex = 6;
            this._VentasBtnCargarDocumento.Text = "Cargar Datos";
            this._VentasBtnCargarDocumento.UseVisualStyleBackColor = false;
            // 
            // VentasCboDocumentoOrigen
            // 
            this._VentasCboDocumentoOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboDocumentoOrigen.FormattingEnabled = true;
            this._VentasCboDocumentoOrigen.Location = new System.Drawing.Point(455, 25);
            this._VentasCboDocumentoOrigen.Name = "VentasCboDocumentoOrigen";
            this._VentasCboDocumentoOrigen.Size = new System.Drawing.Size(260, 33);
            this._VentasCboDocumentoOrigen.TabIndex = 5;
            // 
            // VentasLblDocOrigen
            // 
            this._VentasLblDocOrigen.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblDocOrigen.Location = new System.Drawing.Point(300, 28);
            this._VentasLblDocOrigen.Name = "VentasLblDocOrigen";
            this._VentasLblDocOrigen.Size = new System.Drawing.Size(150, 22);
            this._VentasLblDocOrigen.TabIndex = 7;
            this._VentasLblDocOrigen.Text = "Documento Activo";
            // 
            // VentasCboTipoOrigen
            // 
            this._VentasCboTipoOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboTipoOrigen.FormattingEnabled = true;
            this._VentasCboTipoOrigen.Items.AddRange(new object[] {
            "Manual",
            "Cotización",
            "Pedido"});
            this._VentasCboTipoOrigen.Location = new System.Drawing.Point(120, 25);
            this._VentasCboTipoOrigen.Name = "VentasCboTipoOrigen";
            this._VentasCboTipoOrigen.Size = new System.Drawing.Size(155, 33);
            this._VentasCboTipoOrigen.TabIndex = 4;
            // 
            // VentasLblTipoOrigen
            // 
            this._VentasLblTipoOrigen.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblTipoOrigen.Location = new System.Drawing.Point(20, 28);
            this._VentasLblTipoOrigen.Name = "VentasLblTipoOrigen";
            this._VentasLblTipoOrigen.Size = new System.Drawing.Size(95, 22);
            this._VentasLblTipoOrigen.TabIndex = 8;
            this._VentasLblTipoOrigen.Text = "Tipo Origen";
            // 
            // VentasGbxCabecera
            // 
            this._VentasGbxCabecera.Controls.Add(this._VentasCboCondicionPago);
            this._VentasGbxCabecera.Controls.Add(this._VentasLblCondicionPago);
            this._VentasGbxCabecera.Controls.Add(this._VentasCboMoneda);
            this._VentasGbxCabecera.Controls.Add(this._VentasLblMoneda);
            this._VentasGbxCabecera.Controls.Add(this._VentasCboTipoDte);
            this._VentasGbxCabecera.Controls.Add(this._VentasLblTipoDte);
            this._VentasGbxCabecera.Controls.Add(this._VentasDtpFechaEmision);
            this._VentasGbxCabecera.Controls.Add(this._VentasLblFechaEmision);
            this._VentasGbxCabecera.Controls.Add(this._VentasTxtEstablecimiento);
            this._VentasGbxCabecera.Controls.Add(this._VentasLblEstablecimiento);
            this._VentasGbxCabecera.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this._VentasGbxCabecera.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasGbxCabecera.Location = new System.Drawing.Point(20, 150);
            this._VentasGbxCabecera.Name = "VentasGbxCabecera";
            this._VentasGbxCabecera.Size = new System.Drawing.Size(1230, 75);
            this._VentasGbxCabecera.TabIndex = 7;
            this._VentasGbxCabecera.TabStop = false;
            this._VentasGbxCabecera.Text = " Parámetros de Emisión y Pago ";
            // 
            // VentasCboCondicionPago
            // 
            this._VentasCboCondicionPago.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboCondicionPago.FormattingEnabled = true;
            this._VentasCboCondicionPago.Items.AddRange(new object[] {
            "CONTADO",
            "CREDITO"});
            this._VentasCboCondicionPago.Location = new System.Drawing.Point(1090, 30);
            this._VentasCboCondicionPago.Name = "VentasCboCondicionPago";
            this._VentasCboCondicionPago.Size = new System.Drawing.Size(125, 33);
            this._VentasCboCondicionPago.TabIndex = 0;
            // 
            // VentasLblCondicionPago
            // 
            this._VentasLblCondicionPago.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblCondicionPago.Location = new System.Drawing.Point(1035, 33);
            this._VentasLblCondicionPago.Name = "VentasLblCondicionPago";
            this._VentasLblCondicionPago.Size = new System.Drawing.Size(50, 22);
            this._VentasLblCondicionPago.TabIndex = 1;
            this._VentasLblCondicionPago.Text = "Pago";
            // 
            // VentasCboMoneda
            // 
            this._VentasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboMoneda.FormattingEnabled = true;
            this._VentasCboMoneda.Items.AddRange(new object[] {
            "GTQ-Quetzal",
            "USD-Dólar estadounidense"});
            this._VentasCboMoneda.Location = new System.Drawing.Point(870, 30);
            this._VentasCboMoneda.Name = "VentasCboMoneda";
            this._VentasCboMoneda.Size = new System.Drawing.Size(150, 33);
            this._VentasCboMoneda.TabIndex = 2;
            // 
            // VentasLblMoneda
            // 
            this._VentasLblMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblMoneda.Location = new System.Drawing.Point(805, 33);
            this._VentasLblMoneda.Name = "VentasLblMoneda";
            this._VentasLblMoneda.Size = new System.Drawing.Size(60, 22);
            this._VentasLblMoneda.TabIndex = 3;
            this._VentasLblMoneda.Text = "Moneda";
            // 
            // VentasCboTipoDte
            // 
            this._VentasCboTipoDte.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboTipoDte.FormattingEnabled = true;
            this._VentasCboTipoDte.Items.AddRange(new object[] {
            "Factura"});
            this._VentasCboTipoDte.Location = new System.Drawing.Point(660, 30);
            this._VentasCboTipoDte.Name = "VentasCboTipoDte";
            this._VentasCboTipoDte.Size = new System.Drawing.Size(130, 33);
            this._VentasCboTipoDte.TabIndex = 4;
            // 
            // VentasLblTipoDte
            // 
            this._VentasLblTipoDte.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblTipoDte.Location = new System.Drawing.Point(585, 33);
            this._VentasLblTipoDte.Name = "VentasLblTipoDte";
            this._VentasLblTipoDte.Size = new System.Drawing.Size(70, 22);
            this._VentasLblTipoDte.TabIndex = 5;
            this._VentasLblTipoDte.Text = "Tipo DTE";
            // 
            // VentasDtpFechaEmision
            // 
            this._VentasDtpFechaEmision.CustomFormat = "dd/MM/yyyy HH:mm";
            this._VentasDtpFechaEmision.Enabled = false;
            this._VentasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this._VentasDtpFechaEmision.Location = new System.Drawing.Point(420, 30);
            this._VentasDtpFechaEmision.Name = "VentasDtpFechaEmision";
            this._VentasDtpFechaEmision.Size = new System.Drawing.Size(150, 31);
            this._VentasDtpFechaEmision.TabIndex = 6;
            // 
            // VentasLblFechaEmision
            // 
            this._VentasLblFechaEmision.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblFechaEmision.Location = new System.Drawing.Point(330, 33);
            this._VentasLblFechaEmision.Name = "VentasLblFechaEmision";
            this._VentasLblFechaEmision.Size = new System.Drawing.Size(85, 22);
            this._VentasLblFechaEmision.TabIndex = 7;
            this._VentasLblFechaEmision.Text = "Fecha/Hora";
            // 
            // VentasTxtEstablecimiento
            // 
            this._VentasTxtEstablecimiento.Enabled = false;
            this._VentasTxtEstablecimiento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._VentasTxtEstablecimiento.Location = new System.Drawing.Point(130, 30);
            this._VentasTxtEstablecimiento.Name = "VentasTxtEstablecimiento";
            this._VentasTxtEstablecimiento.Size = new System.Drawing.Size(185, 31);
            this._VentasTxtEstablecimiento.TabIndex = 8;
            this._VentasTxtEstablecimiento.Text = "Embutidos de Calidad, S.A.";
            // 
            // VentasLblEstablecimiento
            // 
            this._VentasLblEstablecimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblEstablecimiento.Location = new System.Drawing.Point(15, 33);
            this._VentasLblEstablecimiento.Name = "VentasLblEstablecimiento";
            this._VentasLblEstablecimiento.Size = new System.Drawing.Size(110, 22);
            this._VentasLblEstablecimiento.TabIndex = 9;
            this._VentasLblEstablecimiento.Text = "Establecimiento";
            // 
            // VentasGbxIdentificacion
            // 
            this._VentasGbxIdentificacion.Controls.Add(this._VentasTxtDireccionComprador);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasLblDireccionComprador);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasTxtNombreComprador);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasLblNombreComprador);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasTxtNitReceptor);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasLblNitReceptor);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasRdoCodigoCliente);
            this._VentasGbxIdentificacion.Controls.Add(this._VentasRdoNit);
            this._VentasGbxIdentificacion.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this._VentasGbxIdentificacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasGbxIdentificacion.Location = new System.Drawing.Point(20, 230);
            this._VentasGbxIdentificacion.Name = "VentasGbxIdentificacion";
            this._VentasGbxIdentificacion.Size = new System.Drawing.Size(1230, 100);
            this._VentasGbxIdentificacion.TabIndex = 8;
            this._VentasGbxIdentificacion.TabStop = false;
            this._VentasGbxIdentificacion.Text = " Datos del Receptor / Comprador ";
            // 
            // VentasTxtDireccionComprador
            // 
            this._VentasTxtDireccionComprador.Enabled = false;
            this._VentasTxtDireccionComprador.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtDireccionComprador.Location = new System.Drawing.Point(120, 62);
            this._VentasTxtDireccionComprador.Name = "VentasTxtDireccionComprador";
            this._VentasTxtDireccionComprador.ReadOnly = true;
            this._VentasTxtDireccionComprador.Size = new System.Drawing.Size(1095, 31);
            this._VentasTxtDireccionComprador.TabIndex = 0;
            // 
            // VentasLblDireccionComprador
            // 
            this._VentasLblDireccionComprador.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblDireccionComprador.Location = new System.Drawing.Point(20, 65);
            this._VentasLblDireccionComprador.Name = "VentasLblDireccionComprador";
            this._VentasLblDireccionComprador.Size = new System.Drawing.Size(95, 22);
            this._VentasLblDireccionComprador.TabIndex = 1;
            this._VentasLblDireccionComprador.Text = "Dirección";
            // 
            // VentasTxtNombreComprador
            // 
            this._VentasTxtNombreComprador.BackColor = System.Drawing.Color.WhiteSmoke;
            this._VentasTxtNombreComprador.Enabled = false;
            this._VentasTxtNombreComprador.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._VentasTxtNombreComprador.Location = new System.Drawing.Point(640, 27);
            this._VentasTxtNombreComprador.Name = "VentasTxtNombreComprador";
            this._VentasTxtNombreComprador.ReadOnly = true;
            this._VentasTxtNombreComprador.Size = new System.Drawing.Size(575, 31);
            this._VentasTxtNombreComprador.TabIndex = 2;
            // 
            // VentasLblNombreComprador
            // 
            this._VentasLblNombreComprador.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblNombreComprador.Location = new System.Drawing.Point(500, 30);
            this._VentasLblNombreComprador.Name = "VentasLblNombreComprador";
            this._VentasLblNombreComprador.Size = new System.Drawing.Size(135, 22);
            this._VentasLblNombreComprador.TabIndex = 3;
            this._VentasLblNombreComprador.Text = "Nombre Receptor:";
            // 
            // VentasTxtNitReceptor
            // 
            this._VentasTxtNitReceptor.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtNitReceptor.Location = new System.Drawing.Point(325, 27);
            this._VentasTxtNitReceptor.Name = "VentasTxtNitReceptor";
            this._VentasTxtNitReceptor.Size = new System.Drawing.Size(160, 31);
            this._VentasTxtNitReceptor.TabIndex = 4;
            this._VentasTxtNitReceptor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VentasMetTxtNitReceptorKeyPress);
            // 
            // VentasLblNitReceptor
            // 
            this._VentasLblNitReceptor.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblNitReceptor.Location = new System.Drawing.Point(225, 30);
            this._VentasLblNitReceptor.Name = "VentasLblNitReceptor";
            this._VentasLblNitReceptor.Size = new System.Drawing.Size(95, 22);
            this._VentasLblNitReceptor.TabIndex = 5;
            this._VentasLblNitReceptor.Text = "NIT / Código";
            // 
            // VentasRdoCodigoCliente
            // 
            this._VentasRdoCodigoCliente.AutoSize = true;
            this._VentasRdoCodigoCliente.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasRdoCodigoCliente.Location = new System.Drawing.Point(90, 28);
            this._VentasRdoCodigoCliente.Name = "VentasRdoCodigoCliente";
            this._VentasRdoCodigoCliente.Size = new System.Drawing.Size(130, 26);
            this._VentasRdoCodigoCliente.TabIndex = 6;
            this._VentasRdoCodigoCliente.Text = "Cód. Cliente";
            // 
            // VentasRdoNit
            // 
            this._VentasRdoNit.AutoSize = true;
            this._VentasRdoNit.Checked = true;
            this._VentasRdoNit.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasRdoNit.Location = new System.Drawing.Point(20, 28);
            this._VentasRdoNit.Name = "VentasRdoNit";
            this._VentasRdoNit.Size = new System.Drawing.Size(65, 26);
            this._VentasRdoNit.TabIndex = 7;
            this._VentasRdoNit.TabStop = true;
            this._VentasRdoNit.Text = "NIT";
            // 
            // VentasLblItems
            // 
            this._VentasLblItems.AutoSize = true;
            this._VentasLblItems.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this._VentasLblItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasLblItems.Location = new System.Drawing.Point(20, 338);
            this._VentasLblItems.Name = "VentasLblItems";
            this._VentasLblItems.Size = new System.Drawing.Size(284, 31);
            this._VentasLblItems.TabIndex = 9;
            this._VentasLblItems.Text = "Detalle de productos";
            // 
            // VentasLblCodigoProducto
            // 
            this._VentasLblCodigoProducto.AutoSize = true;
            this._VentasLblCodigoProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblCodigoProducto.Location = new System.Drawing.Point(20, 375);
            this._VentasLblCodigoProducto.Name = "VentasLblCodigoProducto";
            this._VentasLblCodigoProducto.Size = new System.Drawing.Size(112, 22);
            this._VentasLblCodigoProducto.TabIndex = 29;
            this._VentasLblCodigoProducto.Text = "Código Prod.";
            // 
            // VentasCboCodigoProducto
            // 
            this._VentasCboCodigoProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboCodigoProducto.FormattingEnabled = true;
            this._VentasCboCodigoProducto.Location = new System.Drawing.Point(20, 398);
            this._VentasCboCodigoProducto.Name = "VentasCboCodigoProducto";
            this._VentasCboCodigoProducto.Size = new System.Drawing.Size(120, 33);
            this._VentasCboCodigoProducto.TabIndex = 10;
            // 
            // VentasLblNombreProducto
            // 
            this._VentasLblNombreProducto.AutoSize = true;
            this._VentasLblNombreProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblNombreProducto.Location = new System.Drawing.Point(150, 375);
            this._VentasLblNombreProducto.Name = "VentasLblNombreProducto";
            this._VentasLblNombreProducto.Size = new System.Drawing.Size(180, 22);
            this._VentasLblNombreProducto.TabIndex = 28;
            this._VentasLblNombreProducto.Text = "Nombre / Descripción";
            // 
            // VentasCboNombreProducto
            // 
            this._VentasCboNombreProducto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasCboNombreProducto.FormattingEnabled = true;
            this._VentasCboNombreProducto.Location = new System.Drawing.Point(150, 398);
            this._VentasCboNombreProducto.Name = "VentasCboNombreProducto";
            this._VentasCboNombreProducto.Size = new System.Drawing.Size(300, 33);
            this._VentasCboNombreProducto.TabIndex = 11;
            // 
            // VentasLblCantidad
            // 
            this._VentasLblCantidad.AutoSize = true;
            this._VentasLblCantidad.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblCantidad.Location = new System.Drawing.Point(460, 375);
            this._VentasLblCantidad.Name = "VentasLblCantidad";
            this._VentasLblCantidad.Size = new System.Drawing.Size(79, 22);
            this._VentasLblCantidad.TabIndex = 27;
            this._VentasLblCantidad.Text = "Cantidad";
            // 
            // VentasTxtCantidad
            // 
            this._VentasTxtCantidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtCantidad.Location = new System.Drawing.Point(460, 398);
            this._VentasTxtCantidad.Name = "VentasTxtCantidad";
            this._VentasTxtCantidad.Size = new System.Drawing.Size(85, 31);
            this._VentasTxtCantidad.TabIndex = 12;
            this._VentasTxtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.VentasMetTxtCantidadKeyPress);
            // 
            // VentasLblPrecioUnitario
            // 
            this._VentasLblPrecioUnitario.AutoSize = true;
            this._VentasLblPrecioUnitario.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblPrecioUnitario.Location = new System.Drawing.Point(555, 375);
            this._VentasLblPrecioUnitario.Name = "VentasLblPrecioUnitario";
            this._VentasLblPrecioUnitario.Size = new System.Drawing.Size(100, 22);
            this._VentasLblPrecioUnitario.TabIndex = 26;
            this._VentasLblPrecioUnitario.Text = "Precio Unit.";
            // 
            // VentasTxtPrecioUnitario
            // 
            this._VentasTxtPrecioUnitario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtPrecioUnitario.Location = new System.Drawing.Point(555, 398);
            this._VentasTxtPrecioUnitario.Name = "VentasTxtPrecioUnitario";
            this._VentasTxtPrecioUnitario.Size = new System.Drawing.Size(110, 31);
            this._VentasTxtPrecioUnitario.TabIndex = 13;
            // 
            // VentasLblDescuentos
            // 
            this._VentasLblDescuentos.AutoSize = true;
            this._VentasLblDescuentos.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblDescuentos.Location = new System.Drawing.Point(675, 375);
            this._VentasLblDescuentos.Name = "VentasLblDescuentos";
            this._VentasLblDescuentos.Size = new System.Drawing.Size(92, 22);
            this._VentasLblDescuentos.TabIndex = 25;
            this._VentasLblDescuentos.Text = "Descuento";
            // 
            // VentasTxtDescuentos
            // 
            this._VentasTxtDescuentos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtDescuentos.Location = new System.Drawing.Point(675, 398);
            this._VentasTxtDescuentos.Name = "VentasTxtDescuentos";
            this._VentasTxtDescuentos.Size = new System.Drawing.Size(100, 31);
            this._VentasTxtDescuentos.TabIndex = 14;
            // 
            // VentasLblSubtotalLinea
            // 
            this._VentasLblSubtotalLinea.AutoSize = true;
            this._VentasLblSubtotalLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblSubtotalLinea.Location = new System.Drawing.Point(785, 375);
            this._VentasLblSubtotalLinea.Name = "VentasLblSubtotalLinea";
            this._VentasLblSubtotalLinea.Size = new System.Drawing.Size(75, 22);
            this._VentasLblSubtotalLinea.TabIndex = 24;
            this._VentasLblSubtotalLinea.Text = "Subtotal";
            // 
            // VentasTxtSubtotalLinea
            // 
            this._VentasTxtSubtotalLinea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtSubtotalLinea.Location = new System.Drawing.Point(785, 398);
            this._VentasTxtSubtotalLinea.Name = "VentasTxtSubtotalLinea";
            this._VentasTxtSubtotalLinea.Size = new System.Drawing.Size(105, 31);
            this._VentasTxtSubtotalLinea.TabIndex = 15;
            // 
            // VentasLblIvaLinea
            // 
            this._VentasLblIvaLinea.AutoSize = true;
            this._VentasLblIvaLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblIvaLinea.Location = new System.Drawing.Point(900, 375);
            this._VentasLblIvaLinea.Name = "VentasLblIvaLinea";
            this._VentasLblIvaLinea.Size = new System.Drawing.Size(72, 22);
            this._VentasLblIvaLinea.TabIndex = 23;
            this._VentasLblIvaLinea.Text = "IVA (Q)";
            // 
            // VentasTxtIvaLinea
            // 
            this._VentasTxtIvaLinea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtIvaLinea.Location = new System.Drawing.Point(900, 398);
            this._VentasTxtIvaLinea.Name = "VentasTxtIvaLinea";
            this._VentasTxtIvaLinea.Size = new System.Drawing.Size(95, 31);
            this._VentasTxtIvaLinea.TabIndex = 16;
            // 
            // VentasLblTotalLinea
            // 
            this._VentasLblTotalLinea.AutoSize = true;
            this._VentasLblTotalLinea.Font = new System.Drawing.Font("Tahoma", 9F);
            this._VentasLblTotalLinea.Location = new System.Drawing.Point(1005, 375);
            this._VentasLblTotalLinea.Name = "VentasLblTotalLinea";
            this._VentasLblTotalLinea.Size = new System.Drawing.Size(50, 22);
            this._VentasLblTotalLinea.TabIndex = 22;
            this._VentasLblTotalLinea.Text = "Total";
            // 
            // VentasTxtTotal
            // 
            this._VentasTxtTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._VentasTxtTotal.Location = new System.Drawing.Point(1005, 398);
            this._VentasTxtTotal.Name = "VentasTxtTotal";
            this._VentasTxtTotal.Size = new System.Drawing.Size(115, 31);
            this._VentasTxtTotal.TabIndex = 17;
            // 
            // VentasDgvItems
            // 
            this._VentasDgvItems.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this._VentasDgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._VentasDgvItems.Location = new System.Drawing.Point(20, 440);
            this._VentasDgvItems.Name = "VentasDgvItems";
            this._VentasDgvItems.RowHeadersWidth = 51;
            this._VentasDgvItems.RowTemplate.Height = 26;
            this._VentasDgvItems.Size = new System.Drawing.Size(1230, 175);
            this._VentasDgvItems.TabIndex = 19;
            // 
            // VentasPnlResumen
            // 
            this._VentasPnlResumen.BackColor = System.Drawing.Color.White;
            this._VentasPnlResumen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._VentasPnlResumen.Controls.Add(this._VentasLblSubtotalResumen);
            this._VentasPnlResumen.Controls.Add(this._VentasLblImpuestosResumen);
            this._VentasPnlResumen.Controls.Add(this._VentasLblTotalGeneralResumen);
            this._VentasPnlResumen.Location = new System.Drawing.Point(900, 625);
            this._VentasPnlResumen.Name = "VentasPnlResumen";
            this._VentasPnlResumen.Size = new System.Drawing.Size(350, 110);
            this._VentasPnlResumen.TabIndex = 20;
            // 
            // VentasLblSubtotalResumen
            // 
            this._VentasLblSubtotalResumen.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this._VentasLblSubtotalResumen.Location = new System.Drawing.Point(15, 10);
            this._VentasLblSubtotalResumen.Name = "VentasLblSubtotalResumen";
            this._VentasLblSubtotalResumen.Size = new System.Drawing.Size(320, 22);
            this._VentasLblSubtotalResumen.TabIndex = 0;
            this._VentasLblSubtotalResumen.Text = "Subtotal:                              Q 0.00";
            // 
            // VentasLblImpuestosResumen
            // 
            this._VentasLblImpuestosResumen.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this._VentasLblImpuestosResumen.Location = new System.Drawing.Point(15, 38);
            this._VentasLblImpuestosResumen.Name = "VentasLblImpuestosResumen";
            this._VentasLblImpuestosResumen.Size = new System.Drawing.Size(320, 22);
            this._VentasLblImpuestosResumen.TabIndex = 1;
            this._VentasLblImpuestosResumen.Text = "Impuestos (IVA):                Q 0.00";
            // 
            // VentasLblTotalGeneralResumen
            // 
            this._VentasLblTotalGeneralResumen.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this._VentasLblTotalGeneralResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this._VentasLblTotalGeneralResumen.Location = new System.Drawing.Point(15, 70);
            this._VentasLblTotalGeneralResumen.Name = "VentasLblTotalGeneralResumen";
            this._VentasLblTotalGeneralResumen.Size = new System.Drawing.Size(320, 28);
            this._VentasLblTotalGeneralResumen.TabIndex = 2;
            this._VentasLblTotalGeneralResumen.Text = "TOTAL DE LA FACTURA";
            // 
            // FrmFacturas
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1275, 755);
            this.Controls.Add(this._VentasBtnEmitirFactura);
            this.Controls.Add(this._VentasPnlResumen);
            this.Controls.Add(this._VentasDgvItems);
            this.Controls.Add(this._VentasBtnAgregarItem);
            this.Controls.Add(this._VentasTxtTotal);
            this.Controls.Add(this._VentasLblTotalLinea);
            this.Controls.Add(this._VentasTxtIvaLinea);
            this.Controls.Add(this._VentasLblIvaLinea);
            this.Controls.Add(this._VentasTxtSubtotalLinea);
            this.Controls.Add(this._VentasLblSubtotalLinea);
            this.Controls.Add(this._VentasTxtDescuentos);
            this.Controls.Add(this._VentasLblDescuentos);
            this.Controls.Add(this._VentasTxtPrecioUnitario);
            this.Controls.Add(this._VentasLblPrecioUnitario);
            this.Controls.Add(this._VentasTxtCantidad);
            this.Controls.Add(this._VentasLblCantidad);
            this.Controls.Add(this._VentasCboNombreProducto);
            this.Controls.Add(this._VentasLblNombreProducto);
            this.Controls.Add(this._VentasCboCodigoProducto);
            this.Controls.Add(this._VentasLblCodigoProducto);
            this.Controls.Add(this._VentasLblItems);
            this.Controls.Add(this._VentasGbxIdentificacion);
            this.Controls.Add(this._VentasGbxCabecera);
            this.Controls.Add(this._VentasGbxOrigenDocumento);
            this.Controls.Add(this._VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Facturas.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmFacturas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7007 – Facturas";
            this._VentasPnlEncabezado.ResumeLayout(false);
            this._VentasPnlEncabezado.PerformLayout();
            this._VentasGbxOrigenDocumento.ResumeLayout(false);
            this._VentasGbxCabecera.ResumeLayout(false);
            this._VentasGbxCabecera.PerformLayout();
            this._VentasGbxIdentificacion.ResumeLayout(false);
            this._VentasGbxIdentificacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._VentasDgvItems)).EndInit();
            this._VentasPnlResumen.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _VentasPnlEncabezado;
        private System.Windows.Forms.Label _VentasLblTitulo;
        private System.Windows.Forms.Label _VentasLblSubtitulo;
        private System.Windows.Forms.Label _VentasLblCodigoFormulario;

        private System.Windows.Forms.GroupBox _VentasGbxOrigenDocumento;
        private System.Windows.Forms.Label _VentasLblTipoOrigen;
        private System.Windows.Forms.ComboBox _VentasCboTipoOrigen;
        private System.Windows.Forms.Label _VentasLblDocOrigen;
        private System.Windows.Forms.ComboBox _VentasCboDocumentoOrigen;
        private System.Windows.Forms.Button _VentasBtnCargarDocumento;

        private System.Windows.Forms.GroupBox _VentasGbxCabecera;
        private System.Windows.Forms.Label _VentasLblEstablecimiento;
        private System.Windows.Forms.TextBox _VentasTxtEstablecimiento;
        private System.Windows.Forms.Label _VentasLblFechaEmision;
        private System.Windows.Forms.DateTimePicker _VentasDtpFechaEmision;
        private System.Windows.Forms.Label _VentasLblTipoDte;
        private System.Windows.Forms.ComboBox _VentasCboTipoDte;
        private System.Windows.Forms.Label _VentasLblMoneda;
        private System.Windows.Forms.ComboBox _VentasCboMoneda;
        private System.Windows.Forms.Label _VentasLblCondicionPago;
        private System.Windows.Forms.ComboBox _VentasCboCondicionPago;

        private System.Windows.Forms.GroupBox _VentasGbxIdentificacion;
        private System.Windows.Forms.RadioButton _VentasRdoNit;
        private System.Windows.Forms.RadioButton _VentasRdoCodigoCliente;
        private System.Windows.Forms.Label _VentasLblNitReceptor;
        private System.Windows.Forms.TextBox _VentasTxtNitReceptor;
        private System.Windows.Forms.Label _VentasLblNombreComprador;
        private System.Windows.Forms.TextBox _VentasTxtNombreComprador;
        private System.Windows.Forms.Label _VentasLblDireccionComprador;
        private System.Windows.Forms.TextBox _VentasTxtDireccionComprador;

        private System.Windows.Forms.Label _VentasLblItems;
        private System.Windows.Forms.Label _VentasLblCodigoProducto;
        private System.Windows.Forms.ComboBox _VentasCboCodigoProducto;
        private System.Windows.Forms.Label _VentasLblNombreProducto;
        private System.Windows.Forms.ComboBox _VentasCboNombreProducto;
        private System.Windows.Forms.Label _VentasLblCantidad;
        private System.Windows.Forms.TextBox _VentasTxtCantidad;
        private System.Windows.Forms.Label _VentasLblPrecioUnitario;
        private System.Windows.Forms.TextBox _VentasTxtPrecioUnitario;
        private System.Windows.Forms.Label _VentasLblDescuentos;
        private System.Windows.Forms.TextBox _VentasTxtDescuentos;
        private System.Windows.Forms.Label _VentasLblSubtotalLinea;
        private System.Windows.Forms.TextBox _VentasTxtSubtotalLinea;
        private System.Windows.Forms.Label _VentasLblIvaLinea;
        private System.Windows.Forms.TextBox _VentasTxtIvaLinea;
        private System.Windows.Forms.Label _VentasLblTotalLinea;
        private System.Windows.Forms.TextBox _VentasTxtTotal;

        private System.Windows.Forms.Button _VentasBtnAgregarItem;
        private System.Windows.Forms.DataGridView _VentasDgvItems;

        private System.Windows.Forms.Panel _VentasPnlResumen;
        private System.Windows.Forms.Label _VentasLblSubtotalResumen;
        private System.Windows.Forms.Label _VentasLblImpuestosResumen;
        private System.Windows.Forms.Label _VentasLblTotalGeneralResumen;

        private System.Windows.Forms.Button _VentasBtnEmitirFactura;
        private System.Windows.Forms.ToolTip _VentasTtpAccionesOriginales;
    }
}
