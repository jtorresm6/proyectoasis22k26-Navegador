using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_PagosProveedores
{
    partial class FrmPagosProveedores
    {
        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null)) _Componentes.Dispose();
            base.Dispose(LiberarRecursos);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPagosProveedores));
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasDgvPagos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasGbxRegistrarPago = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpRegistro = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblProveedorPago = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblFactura = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblFechaPago = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboProveedorPago = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasCboFactura = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTxtFechaPago = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblMonto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblMetodo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblReferencia = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtMonto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasCboMetodo = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTxtReferencia = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblMoneda = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboMoneda = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblTotalAplicado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasPnlDocumentos = new System.Windows.Forms.Panel();
            this.ComprasBtnQuitarDocumento = new System.Windows.Forms.Button();
            this.ComprasBtnAgregarDocumento = new System.Windows.Forms.Button();
            this.ComprasDgvDetalle = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasPnlBotones = new System.Windows.Forms.Panel();
            this.ComprasBtnEliminar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnEditar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnCancelar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpFiltros = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasCboProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.comprasUsrRangoFechas1 = new CapaVista_Componentes.Componentes.ComprasUsrRangoFechas();
            this.ComprasPnlEncabezado = new System.Windows.Forms.Panel();
            this.ComprasBtnNuevoPago = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasGbxRegistrarPago.SuspendLayout();
            this.ComprasTlpRegistro.SuspendLayout();
            this.ComprasPnlDocumentos.SuspendLayout();
            this.ComprasPnlBotones.SuspendLayout();
            this.ComprasTlpFiltros.SuspendLayout();
            this.ComprasPnlEncabezado.SuspendLayout();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTlpFormulario
            // 
            this.ComprasTlpFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFormulario.ColumnCount = 1;
            this.ComprasTlpFormulario.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.Controls.Add(this.ComprasUsrEncabezadoFormulario, 0, 0);
            this.ComprasTlpFormulario.Controls.Add(this.ComprasPnlContenido, 0, 1);
            this.ComprasTlpFormulario.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTlpFormulario.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpFormulario.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFormulario.Name = "ComprasTlpFormulario";
            this.ComprasTlpFormulario.RowCount = 2;
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFormulario.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1338, 1349);
            this.ComprasTlpFormulario.TabIndex = 0;
            // 
            // ComprasUsrEncabezadoFormulario
            // 
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6004";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Consulta, registra y administra los pagos a proveedores, sus fechas y métodos de " +
    "pago.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1318, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Pagos a proveedores";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasDgvPagos);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxRegistrarPago);
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpFiltros);
            this.ComprasPnlContenido.Controls.Add(this.ComprasPnlEncabezado);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Padding = new System.Windows.Forms.Padding(16);
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1338, 1199);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasDgvPagos
            // 
            this.ComprasDgvPagos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvPagos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvPagos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvPagos.Location = new System.Drawing.Point(16, 204);
            this.ComprasDgvPagos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvPagos.Name = "ComprasDgvPagos";
            this.ComprasDgvPagos.Size = new System.Drawing.Size(1306, 365);
            this.ComprasDgvPagos.TabIndex = 0;
            // 
            // ComprasGbxRegistrarPago
            // 
            this.ComprasGbxRegistrarPago.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxRegistrarPago.Controls.Add(this.ComprasTlpRegistro);
            this.ComprasGbxRegistrarPago.Controls.Add(this.ComprasPnlBotones);
            this.ComprasGbxRegistrarPago.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ComprasGbxRegistrarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxRegistrarPago.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxRegistrarPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxRegistrarPago.Location = new System.Drawing.Point(16, 569);
            this.ComprasGbxRegistrarPago.Name = "ComprasGbxRegistrarPago";
            this.ComprasGbxRegistrarPago.Size = new System.Drawing.Size(1306, 614);
            this.ComprasGbxRegistrarPago.TabIndex = 1;
            this.ComprasGbxRegistrarPago.TabStop = false;
            this.ComprasGbxRegistrarPago.Text = "Registrar pago";
            // 
            // ComprasTlpRegistro
            // 
            this.ComprasTlpRegistro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpRegistro.ColumnCount = 3;
            this.ComprasTlpRegistro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.ComprasTlpRegistro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.ComprasTlpRegistro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblProveedorPago, 0, 0);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblFactura, 1, 0);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblFechaPago, 2, 0);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasCboProveedorPago, 0, 1);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasCboFactura, 1, 1);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasTxtFechaPago, 2, 1);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblMonto, 0, 2);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblMetodo, 1, 2);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblReferencia, 2, 2);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasTxtMonto, 0, 3);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasCboMetodo, 1, 3);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasTxtReferencia, 2, 3);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblMoneda, 0, 4);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblTotal, 1, 4);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasCboMoneda, 0, 5);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasLblTotalAplicado, 1, 5);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasPnlDocumentos, 2, 5);
            this.ComprasTlpRegistro.Controls.Add(this.ComprasDgvDetalle, 0, 6);
            this.ComprasTlpRegistro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpRegistro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpRegistro.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpRegistro.Name = "ComprasTlpRegistro";
            this.ComprasTlpRegistro.RowCount = 7;
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 157F));
            this.ComprasTlpRegistro.Size = new System.Drawing.Size(1300, 461);
            this.ComprasTlpRegistro.TabIndex = 0;
            // 
            // ComprasLblProveedorPago
            // 
            this.ComprasLblProveedorPago.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedorPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedorPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblProveedorPago.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblProveedorPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProveedorPago.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblProveedorPago.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblProveedorPago.MostrarAsterisco = false;
            this.ComprasLblProveedorPago.Name = "ComprasLblProveedorPago";
            this.ComprasLblProveedorPago.Size = new System.Drawing.Size(427, 20);
            this.ComprasLblProveedorPago.TabIndex = 0;
            this.ComprasLblProveedorPago.TabStop = false;
            this.ComprasLblProveedorPago.TamanoTipografia = 9F;
            this.ComprasLblProveedorPago.Text = "Proveedor";
            this.ComprasLblProveedorPago.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedorPago.Texto = "Proveedor";
            // 
            // ComprasLblFactura
            // 
            this.ComprasLblFactura.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFactura.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFactura.Location = new System.Drawing.Point(436, 3);
            this.ComprasLblFactura.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFactura.MostrarAsterisco = false;
            this.ComprasLblFactura.Name = "ComprasLblFactura";
            this.ComprasLblFactura.Size = new System.Drawing.Size(427, 20);
            this.ComprasLblFactura.TabIndex = 1;
            this.ComprasLblFactura.TabStop = false;
            this.ComprasLblFactura.TamanoTipografia = 9F;
            this.ComprasLblFactura.Text = "Documento por pagar";
            this.ComprasLblFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFactura.Texto = "Documento por pagar";
            // 
            // ComprasLblFechaPago
            // 
            this.ComprasLblFechaPago.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFechaPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaPago.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaPago.Location = new System.Drawing.Point(869, 3);
            this.ComprasLblFechaPago.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaPago.MostrarAsterisco = false;
            this.ComprasLblFechaPago.Name = "ComprasLblFechaPago";
            this.ComprasLblFechaPago.Size = new System.Drawing.Size(428, 20);
            this.ComprasLblFechaPago.TabIndex = 2;
            this.ComprasLblFechaPago.TabStop = false;
            this.ComprasLblFechaPago.TamanoTipografia = 9F;
            this.ComprasLblFechaPago.Text = "Fecha de pago";
            this.ComprasLblFechaPago.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFechaPago.Texto = "Fecha de pago";
            // 
            // ComprasCboProveedorPago
            // 
            this.ComprasCboProveedorPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProveedorPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboProveedorPago.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProveedorPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProveedorPago.Location = new System.Drawing.Point(0, 26);
            this.ComprasCboProveedorPago.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProveedorPago.MaxLength = 0;
            this.ComprasCboProveedorPago.Name = "ComprasCboProveedorPago";
            this.ComprasCboProveedorPago.Size = new System.Drawing.Size(433, 75);
            this.ComprasCboProveedorPago.TabIndex = 3;
            // 
            // ComprasCboFactura
            // 
            this.ComprasCboFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboFactura.Location = new System.Drawing.Point(433, 26);
            this.ComprasCboFactura.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboFactura.MaxLength = 0;
            this.ComprasCboFactura.Name = "ComprasCboFactura";
            this.ComprasCboFactura.Size = new System.Drawing.Size(433, 75);
            this.ComprasCboFactura.TabIndex = 4;
            // 
            // ComprasTxtFechaPago
            // 
            this.ComprasTxtFechaPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtFechaPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtFechaPago.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtFechaPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtFechaPago.Location = new System.Drawing.Point(866, 26);
            this.ComprasTxtFechaPago.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtFechaPago.MaxLength = 2147483647;
            this.ComprasTxtFechaPago.Name = "ComprasTxtFechaPago";
            this.ComprasTxtFechaPago.SelectedText = "";
            this.ComprasTxtFechaPago.SelectionLength = 0;
            this.ComprasTxtFechaPago.SelectionStart = 0;
            this.ComprasTxtFechaPago.Size = new System.Drawing.Size(434, 75);
            this.ComprasTxtFechaPago.TabIndex = 5;
            // 
            // ComprasLblMonto
            // 
            this.ComprasLblMonto.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMonto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMonto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMonto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblMonto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblMonto.Location = new System.Drawing.Point(3, 104);
            this.ComprasLblMonto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblMonto.MostrarAsterisco = false;
            this.ComprasLblMonto.Name = "ComprasLblMonto";
            this.ComprasLblMonto.Size = new System.Drawing.Size(427, 17);
            this.ComprasLblMonto.TabIndex = 6;
            this.ComprasLblMonto.TabStop = false;
            this.ComprasLblMonto.TamanoTipografia = 9F;
            this.ComprasLblMonto.Text = "Monto a aplicar al documento";
            this.ComprasLblMonto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMonto.Texto = "Monto a aplicar al documento";
            // 
            // ComprasLblMetodo
            // 
            this.ComprasLblMetodo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMetodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMetodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMetodo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblMetodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblMetodo.Location = new System.Drawing.Point(436, 104);
            this.ComprasLblMetodo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblMetodo.MostrarAsterisco = false;
            this.ComprasLblMetodo.Name = "ComprasLblMetodo";
            this.ComprasLblMetodo.Size = new System.Drawing.Size(427, 17);
            this.ComprasLblMetodo.TabIndex = 7;
            this.ComprasLblMetodo.TabStop = false;
            this.ComprasLblMetodo.TamanoTipografia = 9F;
            this.ComprasLblMetodo.Text = "Método";
            this.ComprasLblMetodo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMetodo.Texto = "Método";
            // 
            // ComprasLblReferencia
            // 
            this.ComprasLblReferencia.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblReferencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblReferencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblReferencia.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblReferencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblReferencia.Location = new System.Drawing.Point(869, 104);
            this.ComprasLblReferencia.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblReferencia.MostrarAsterisco = false;
            this.ComprasLblReferencia.Name = "ComprasLblReferencia";
            this.ComprasLblReferencia.Size = new System.Drawing.Size(428, 17);
            this.ComprasLblReferencia.TabIndex = 8;
            this.ComprasLblReferencia.TabStop = false;
            this.ComprasLblReferencia.TamanoTipografia = 9F;
            this.ComprasLblReferencia.Text = "Referencia";
            this.ComprasLblReferencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblReferencia.Texto = "Referencia";
            // 
            // ComprasTxtMonto
            // 
            this.ComprasTxtMonto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtMonto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtMonto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtMonto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtMonto.Location = new System.Drawing.Point(0, 124);
            this.ComprasTxtMonto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtMonto.MaxLength = 2147483647;
            this.ComprasTxtMonto.ModoMoneda = true;
            this.ComprasTxtMonto.Name = "ComprasTxtMonto";
            this.ComprasTxtMonto.SelectedText = "";
            this.ComprasTxtMonto.SelectionLength = 0;
            this.ComprasTxtMonto.SelectionStart = 0;
            this.ComprasTxtMonto.Size = new System.Drawing.Size(433, 72);
            this.ComprasTxtMonto.TabIndex = 9;
            this.ComprasTxtMonto.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasCboMetodo
            // 
            this.ComprasCboMetodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboMetodo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboMetodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboMetodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboMetodo.Location = new System.Drawing.Point(433, 124);
            this.ComprasCboMetodo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboMetodo.MaxLength = 0;
            this.ComprasCboMetodo.Name = "ComprasCboMetodo";
            this.ComprasCboMetodo.Size = new System.Drawing.Size(433, 72);
            this.ComprasCboMetodo.TabIndex = 10;
            // 
            // ComprasTxtReferencia
            // 
            this.ComprasTxtReferencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtReferencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtReferencia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtReferencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtReferencia.Location = new System.Drawing.Point(866, 124);
            this.ComprasTxtReferencia.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtReferencia.MaxLength = 2147483647;
            this.ComprasTxtReferencia.Name = "ComprasTxtReferencia";
            this.ComprasTxtReferencia.SelectedText = "";
            this.ComprasTxtReferencia.SelectionLength = 0;
            this.ComprasTxtReferencia.SelectionStart = 0;
            this.ComprasTxtReferencia.Size = new System.Drawing.Size(434, 72);
            this.ComprasTxtReferencia.TabIndex = 11;
            // 
            // ComprasLblMoneda
            // 
            this.ComprasLblMoneda.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblMoneda.Location = new System.Drawing.Point(3, 199);
            this.ComprasLblMoneda.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblMoneda.MostrarAsterisco = false;
            this.ComprasLblMoneda.Name = "ComprasLblMoneda";
            this.ComprasLblMoneda.Size = new System.Drawing.Size(427, 17);
            this.ComprasLblMoneda.TabIndex = 12;
            this.ComprasLblMoneda.TabStop = false;
            this.ComprasLblMoneda.TamanoTipografia = 9F;
            this.ComprasLblMoneda.Text = "Moneda del pago";
            this.ComprasLblMoneda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMoneda.Texto = "Moneda del pago";
            // 
            // ComprasLblTotal
            // 
            this.ComprasLblTotal.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotal.Location = new System.Drawing.Point(436, 199);
            this.ComprasLblTotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotal.MostrarAsterisco = false;
            this.ComprasLblTotal.Name = "ComprasLblTotal";
            this.ComprasLblTotal.Size = new System.Drawing.Size(427, 17);
            this.ComprasLblTotal.TabIndex = 13;
            this.ComprasLblTotal.TabStop = false;
            this.ComprasLblTotal.TamanoTipografia = 9F;
            this.ComprasLblTotal.Text = "Total del pago";
            this.ComprasLblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTotal.Texto = "Total del pago";
            // 
            // ComprasCboMoneda
            // 
            this.ComprasCboMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboMoneda.Location = new System.Drawing.Point(0, 219);
            this.ComprasCboMoneda.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboMoneda.MaxLength = 0;
            this.ComprasCboMoneda.Name = "ComprasCboMoneda";
            this.ComprasCboMoneda.Size = new System.Drawing.Size(433, 85);
            this.ComprasCboMoneda.TabIndex = 14;
            // 
            // ComprasLblTotalAplicado
            // 
            this.ComprasLblTotalAplicado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTotalAplicado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotalAplicado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTotalAplicado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTotalAplicado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotalAplicado.Location = new System.Drawing.Point(436, 222);
            this.ComprasLblTotalAplicado.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotalAplicado.MostrarAsterisco = false;
            this.ComprasLblTotalAplicado.Name = "ComprasLblTotalAplicado";
            this.ComprasLblTotalAplicado.Size = new System.Drawing.Size(427, 22);
            this.ComprasLblTotalAplicado.TabIndex = 15;
            this.ComprasLblTotalAplicado.TabStop = false;
            this.ComprasLblTotalAplicado.TamanoTipografia = 9F;
            this.ComprasLblTotalAplicado.Text = "0.00";
            this.ComprasLblTotalAplicado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTotalAplicado.Texto = "0.00";
            // 
            // ComprasPnlDocumentos
            // 
            this.ComprasPnlDocumentos.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPnlDocumentos.Controls.Add(this.ComprasBtnQuitarDocumento);
            this.ComprasPnlDocumentos.Controls.Add(this.ComprasBtnAgregarDocumento);
            this.ComprasPnlDocumentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDocumentos.Location = new System.Drawing.Point(866, 219);
            this.ComprasPnlDocumentos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDocumentos.Name = "ComprasPnlDocumentos";
            this.ComprasPnlDocumentos.Size = new System.Drawing.Size(434, 85);
            this.ComprasPnlDocumentos.TabIndex = 16;
            // 
            // ComprasBtnQuitarDocumento
            // 
            this.ComprasBtnQuitarDocumento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnQuitarDocumento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnQuitarDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnQuitarDocumento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasBtnQuitarDocumento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnQuitarDocumento.Image = ((System.Drawing.Image)(resources.GetObject("ComprasBtnQuitarDocumento.Image")));
            this.ComprasBtnQuitarDocumento.Location = new System.Drawing.Point(326, 3);
            this.ComprasBtnQuitarDocumento.Name = "ComprasBtnQuitarDocumento";
            this.ComprasBtnQuitarDocumento.Size = new System.Drawing.Size(82, 82);
            this.ComprasBtnQuitarDocumento.TabIndex = 1;
            this.ComprasBtnQuitarDocumento.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnAgregarDocumento
            // 
            this.ComprasBtnAgregarDocumento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnAgregarDocumento.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnAgregarDocumento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnAgregarDocumento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasBtnAgregarDocumento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnAgregarDocumento.Image = ((System.Drawing.Image)(resources.GetObject("ComprasBtnAgregarDocumento.Image")));
            this.ComprasBtnAgregarDocumento.Location = new System.Drawing.Point(230, 3);
            this.ComprasBtnAgregarDocumento.Name = "ComprasBtnAgregarDocumento";
            this.ComprasBtnAgregarDocumento.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnAgregarDocumento.TabIndex = 0;
            this.ComprasBtnAgregarDocumento.UseVisualStyleBackColor = false;
            this.ComprasBtnAgregarDocumento.Click += new System.EventHandler(this.ComprasBtnAgregarDocumento_Click);
            // 
            // ComprasDgvDetalle
            // 
            this.ComprasDgvDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpRegistro.SetColumnSpan(this.ComprasDgvDetalle, 3);
            this.ComprasDgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvDetalle.Location = new System.Drawing.Point(0, 304);
            this.ComprasDgvDetalle.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvDetalle.Name = "ComprasDgvDetalle";
            this.ComprasDgvDetalle.Size = new System.Drawing.Size(1300, 157);
            this.ComprasDgvDetalle.TabIndex = 17;
            // 
            // ComprasPnlBotones
            // 
            this.ComprasPnlBotones.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnEliminar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnEditar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnCancelar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnGuardar);
            this.ComprasPnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ComprasPnlBotones.Location = new System.Drawing.Point(3, 485);
            this.ComprasPnlBotones.Name = "ComprasPnlBotones";
            this.ComprasPnlBotones.Size = new System.Drawing.Size(1300, 126);
            this.ComprasPnlBotones.TabIndex = 1;
            // 
            // ComprasBtnEliminar
            // 
            this.ComprasBtnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasBtnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEliminar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEliminar.BackgroundImage")));
            this.ComprasBtnEliminar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEliminar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEliminar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminar.Location = new System.Drawing.Point(908, 16);
            this.ComprasBtnEliminar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnEliminar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminar.Name = "ComprasBtnEliminar";
            this.ComprasBtnEliminar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminar.TabIndex = 3;
            this.ComprasBtnEliminar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnEditar
            // 
            this.ComprasBtnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasBtnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEditar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEditar.BackgroundImage")));
            this.ComprasBtnEditar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEditar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEditar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEditar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEditar.Location = new System.Drawing.Point(1002, 16);
            this.ComprasBtnEditar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnEditar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEditar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEditar.Name = "ComprasBtnEditar";
            this.ComprasBtnEditar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEditar.TabIndex = 2;
            this.ComprasBtnEditar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnCancelar
            // 
            this.ComprasBtnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasBtnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnCancelar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnCancelar.BackgroundImage")));
            this.ComprasBtnCancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnCancelar.Location = new System.Drawing.Point(1096, 16);
            this.ComprasBtnCancelar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnCancelar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnCancelar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnCancelar.Name = "ComprasBtnCancelar";
            this.ComprasBtnCancelar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnCancelar.TabIndex = 0;
            this.ComprasBtnCancelar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnGuardar
            // 
            this.ComprasBtnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnGuardar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnGuardar.BackgroundImage")));
            this.ComprasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(1192, 16);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnGuardar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.TabIndex = 1;
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            // 
            // ComprasTlpFiltros
            // 
            this.ComprasTlpFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFiltros.ColumnCount = 5;
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.59265F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.75651F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.6125574F));
            this.ComprasTlpFiltros.Controls.Add(this.ComprasCboProveedor, 0, 1);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblProveedor, 0, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblEstado, 1, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasCboEstado, 1, 1);
            this.ComprasTlpFiltros.Controls.Add(this.comprasUsrRangoFechas1, 3, 1);
            this.ComprasTlpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTlpFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFiltros.Location = new System.Drawing.Point(16, 106);
            this.ComprasTlpFiltros.Name = "ComprasTlpFiltros";
            this.ComprasTlpFiltros.RowCount = 3;
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpFiltros.Size = new System.Drawing.Size(1306, 98);
            this.ComprasTlpFiltros.TabIndex = 2;
            // 
            // ComprasCboProveedor
            // 
            this.ComprasCboProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProveedor.Location = new System.Drawing.Point(0, 23);
            this.ComprasCboProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProveedor.MaxLength = 0;
            this.ComprasCboProveedor.MostrarEtiquetaError = false;
            this.ComprasCboProveedor.Name = "ComprasCboProveedor";
            this.ComprasCboProveedor.Size = new System.Drawing.Size(301, 75);
            this.ComprasCboProveedor.TabIndex = 12;
            this.ComprasCboProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasLblProveedor
            // 
            this.ComprasLblProveedor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProveedor.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblProveedor.MostrarAsterisco = false;
            this.ComprasLblProveedor.Name = "ComprasLblProveedor";
            this.ComprasLblProveedor.Size = new System.Drawing.Size(295, 17);
            this.ComprasLblProveedor.TabIndex = 0;
            this.ComprasLblProveedor.TabStop = false;
            this.ComprasLblProveedor.TamanoTipografia = 9F;
            this.ComprasLblProveedor.Text = "Proveedor";
            this.ComprasLblProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.Texto = "Proveedor";
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(304, 3);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(254, 17);
            this.ComprasLblEstado.TabIndex = 1;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 9F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.Texto = "Estado";
            // 
            // ComprasCboEstado
            // 
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(301, 23);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(260, 75);
            this.ComprasCboEstado.TabIndex = 5;
            this.ComprasCboEstado.SelectedIndexChanged += new System.EventHandler(this.ComprasCboEstado_SelectedIndexChanged);
            // 
            // comprasUsrRangoFechas1
            // 
            this.comprasUsrRangoFechas1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.comprasUsrRangoFechas1.FechaFin = new System.DateTime(2026, 11, 7, 0, 0, 0, 0);
            this.comprasUsrRangoFechas1.FechaInicio = new System.DateTime(2026, 11, 1, 0, 0, 0, 0);
            this.comprasUsrRangoFechas1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.comprasUsrRangoFechas1.Location = new System.Drawing.Point(661, 23);
            this.comprasUsrRangoFechas1.Margin = new System.Windows.Forms.Padding(0);
            this.comprasUsrRangoFechas1.MinimumSize = new System.Drawing.Size(400, 80);
            this.comprasUsrRangoFechas1.Name = "comprasUsrRangoFechas1";
            this.comprasUsrRangoFechas1.Size = new System.Drawing.Size(636, 80);
            this.comprasUsrRangoFechas1.TabIndex = 1;
            // 
            // ComprasPnlEncabezado
            // 
            this.ComprasPnlEncabezado.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPnlEncabezado.Controls.Add(this.ComprasBtnNuevoPago);
            this.ComprasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlEncabezado.Location = new System.Drawing.Point(16, 16);
            this.ComprasPnlEncabezado.Name = "ComprasPnlEncabezado";
            this.ComprasPnlEncabezado.Size = new System.Drawing.Size(1306, 90);
            this.ComprasPnlEncabezado.TabIndex = 3;
            // 
            // ComprasBtnNuevoPago
            // 
            this.ComprasBtnNuevoPago.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasBtnNuevoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnNuevoPago.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNuevoPago.BackgroundImage")));
            this.ComprasBtnNuevoPago.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnNuevoPago.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnNuevoPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnNuevoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnNuevoPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNuevoPago.Location = new System.Drawing.Point(1217, 7);
            this.ComprasBtnNuevoPago.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnNuevoPago.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevoPago.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevoPago.Name = "ComprasBtnNuevoPago";
            this.ComprasBtnNuevoPago.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevoPago.TabIndex = 2;
            this.ComprasBtnNuevoPago.UseVisualStyleBackColor = false;
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasTlpFormulario);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1359, 700);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // ComprasLblTitulo
            // 
            this.ComprasLblTitulo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ComprasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTitulo.Location = new System.Drawing.Point(3, 28);
            this.ComprasLblTitulo.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasLblTitulo.MaximumSize = new System.Drawing.Size(1375, 29);
            this.ComprasLblTitulo.MostrarAsterisco = false;
            this.ComprasLblTitulo.Name = "ComprasLblTitulo";
            this.ComprasLblTitulo.Size = new System.Drawing.Size(503, 29);
            this.ComprasLblTitulo.TabIndex = 0;
            this.ComprasLblTitulo.TabStop = false;
            this.ComprasLblTitulo.Text = "Pagos a Proveedores";
            this.ComprasLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblTitulo.Texto = "Pagos a Proveedores";
            this.ComprasLblTitulo.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            // 
            // FrmPagosProveedores
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1359, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmPagosProveedores";
            this.Text = "6004 – PagosProveedores";
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasGbxRegistrarPago.ResumeLayout(false);
            this.ComprasTlpRegistro.ResumeLayout(false);
            this.ComprasPnlDocumentos.ResumeLayout(false);
            this.ComprasPnlBotones.ResumeLayout(false);
            this.ComprasTlpFiltros.ResumeLayout(false);
            this.ComprasPnlEncabezado.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel ComprasPnlEncabezado;
        private ComprasUsrLabel ComprasLblTitulo;

        private ComprasUsrDataGridView ComprasDgvPagos;

        private ClsGroupBoxCompras ComprasGbxRegistrarPago;
        private ClsTableLayoutPanelCompras ComprasTlpRegistro;
        private ComprasUsrLabel ComprasLblProveedorPago, ComprasLblFactura, ComprasLblFechaPago, ComprasLblMonto, ComprasLblMetodo, ComprasLblReferencia;
        private ComprasUsrComboBox ComprasCboProveedorPago, ComprasCboFactura, ComprasCboMetodo;
        private ComprasUsrTextBox ComprasTxtFechaPago, ComprasTxtMonto, ComprasTxtReferencia;
        private Panel ComprasPnlBotones;
        private ClsButtonCompras ComprasBtnEliminar, ComprasBtnEditar, ComprasBtnCancelar, ComprasBtnGuardar;

        // Moneda, total y detalle de documentos del pago
        private ComprasUsrLabel ComprasLblMoneda, ComprasLblTotal, ComprasLblTotalAplicado;
        private ComprasUsrComboBox ComprasCboMoneda;
        private Panel ComprasPnlDocumentos;
        private Button ComprasBtnAgregarDocumento, ComprasBtnQuitarDocumento;
        private ComprasUsrDataGridView ComprasDgvDetalle;
        private ComprasUsrRangoFechas comprasUsrRangoFechas1;
        private ClsTableLayoutPanelCompras ComprasTlpFiltros;
        private ComprasUsrComboBox ComprasCboProveedor;
        private ComprasUsrLabel ComprasLblProveedor;
        private ComprasUsrLabel ComprasLblEstado;
        private ComprasUsrComboBox ComprasCboEstado;
        private ClsButtonCompras ComprasBtnNuevoPago;
    }
}