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
            this.ComprasPnlBotones = new System.Windows.Forms.Panel();
            this.ComprasBtnEliminar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnEditar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnCancelar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpFiltros = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasCboProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblDesde = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblHasta = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTxtDesde = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTxtHasta = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasPnlEncabezado = new System.Windows.Forms.Panel();
            this.ComprasBtnNuevoPago = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasGbxRegistrarPago.SuspendLayout();
            this.ComprasTlpRegistro.SuspendLayout();
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
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1338, 1055);
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
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1338, 905);
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
            this.ComprasGbxRegistrarPago.Size = new System.Drawing.Size(1306, 320);
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
            this.ComprasTlpRegistro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpRegistro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpRegistro.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpRegistro.Name = "ComprasTlpRegistro";
            this.ComprasTlpRegistro.RowCount = 4;
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.ComprasTlpRegistro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.ComprasTlpRegistro.Size = new System.Drawing.Size(1300, 167);
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
            this.ComprasLblFactura.Text = "Factura por pagar";
            this.ComprasLblFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFactura.Texto = "Factura por pagar";
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
            this.ComprasLblMonto.Text = "Monto";
            this.ComprasLblMonto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblMonto.Texto = "Monto";
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
            this.ComprasTxtMonto.Name = "ComprasTxtMonto";
            this.ComprasTxtMonto.SelectedText = "";
            this.ComprasTxtMonto.SelectionLength = 0;
            this.ComprasTxtMonto.SelectionStart = 0;
            this.ComprasTxtMonto.Size = new System.Drawing.Size(433, 72);
            this.ComprasTxtMonto.TabIndex = 9;
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
            // ComprasPnlBotones
            // 
            this.ComprasPnlBotones.BackColor = System.Drawing.Color.Transparent;
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnEliminar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnEditar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnCancelar);
            this.ComprasPnlBotones.Controls.Add(this.ComprasBtnGuardar);
            this.ComprasPnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ComprasPnlBotones.Location = new System.Drawing.Point(3, 191);
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
            this.ComprasBtnEliminar.Location = new System.Drawing.Point(859, 16);
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
            this.ComprasBtnEditar.Location = new System.Drawing.Point(970, 16);
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
            this.ComprasBtnCancelar.Location = new System.Drawing.Point(1081, 16);
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
            this.ComprasTlpFiltros.ColumnCount = 4;
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.ComprasTlpFiltros.Controls.Add(this.ComprasCboProveedor, 0, 1);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblProveedor, 0, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblEstado, 1, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblDesde, 2, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasLblHasta, 3, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasCboEstado, 1, 1);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTxtDesde, 2, 1);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTxtHasta, 3, 1);
            this.ComprasTlpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTlpFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFiltros.Location = new System.Drawing.Point(16, 106);
            this.ComprasTlpFiltros.Name = "ComprasTlpFiltros";
            this.ComprasTlpFiltros.RowCount = 2;
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.ComprasTlpFiltros.Size = new System.Drawing.Size(1306, 98);
            this.ComprasTlpFiltros.TabIndex = 2;
            // 
            // ComprasCboProveedor
            // 
            this.ComprasCboProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProveedor.Location = new System.Drawing.Point(0, 26);
            this.ComprasCboProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProveedor.MaxLength = 0;
            this.ComprasCboProveedor.MostrarEtiquetaError = false;
            this.ComprasCboProveedor.Name = "ComprasCboProveedor";
            this.ComprasCboProveedor.Size = new System.Drawing.Size(326, 72);
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
            this.ComprasLblProveedor.Size = new System.Drawing.Size(320, 20);
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
            this.ComprasLblEstado.Location = new System.Drawing.Point(329, 3);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(320, 20);
            this.ComprasLblEstado.TabIndex = 1;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 9F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.Texto = "Estado";
            // 
            // ComprasLblDesde
            // 
            this.ComprasLblDesde.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDesde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDesde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDesde.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDesde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDesde.Location = new System.Drawing.Point(655, 3);
            this.ComprasLblDesde.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDesde.MostrarAsterisco = false;
            this.ComprasLblDesde.Name = "ComprasLblDesde";
            this.ComprasLblDesde.Size = new System.Drawing.Size(320, 20);
            this.ComprasLblDesde.TabIndex = 2;
            this.ComprasLblDesde.TabStop = false;
            this.ComprasLblDesde.TamanoTipografia = 9F;
            this.ComprasLblDesde.Text = "Desde";
            this.ComprasLblDesde.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDesde.Texto = "Desde";
            // 
            // ComprasLblHasta
            // 
            this.ComprasLblHasta.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblHasta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblHasta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblHasta.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblHasta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblHasta.Location = new System.Drawing.Point(981, 3);
            this.ComprasLblHasta.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblHasta.MostrarAsterisco = false;
            this.ComprasLblHasta.Name = "ComprasLblHasta";
            this.ComprasLblHasta.Size = new System.Drawing.Size(322, 20);
            this.ComprasLblHasta.TabIndex = 3;
            this.ComprasLblHasta.TabStop = false;
            this.ComprasLblHasta.TamanoTipografia = 9F;
            this.ComprasLblHasta.Text = "Hasta";
            this.ComprasLblHasta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblHasta.Texto = "Hasta";
            // 
            // ComprasCboEstado
            // 
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(326, 26);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(326, 72);
            this.ComprasCboEstado.TabIndex = 5;
            // 
            // ComprasTxtDesde
            // 
            this.ComprasTxtDesde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDesde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDesde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDesde.Location = new System.Drawing.Point(652, 26);
            this.ComprasTxtDesde.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtDesde.MaxLength = 2147483647;
            this.ComprasTxtDesde.Name = "ComprasTxtDesde";
            this.ComprasTxtDesde.SelectedText = "";
            this.ComprasTxtDesde.SelectionLength = 0;
            this.ComprasTxtDesde.SelectionStart = 0;
            this.ComprasTxtDesde.Size = new System.Drawing.Size(326, 72);
            this.ComprasTxtDesde.TabIndex = 6;
            // 
            // ComprasTxtHasta
            // 
            this.ComprasTxtHasta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtHasta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtHasta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtHasta.Location = new System.Drawing.Point(978, 26);
            this.ComprasTxtHasta.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtHasta.MaxLength = 2147483647;
            this.ComprasTxtHasta.Name = "ComprasTxtHasta";
            this.ComprasTxtHasta.SelectedText = "";
            this.ComprasTxtHasta.SelectionLength = 0;
            this.ComprasTxtHasta.SelectionStart = 0;
            this.ComprasTxtHasta.Size = new System.Drawing.Size(328, 72);
            this.ComprasTxtHasta.TabIndex = 7;
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
            this.ComprasBtnNuevoPago.Location = new System.Drawing.Point(1220, 4);
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
            this.ComprasPnlBotones.ResumeLayout(false);
            this.ComprasTlpFiltros.ResumeLayout(false);
            this.ComprasPnlEncabezado.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel ComprasPnlEncabezado;
        private ComprasUsrLabel ComprasLblTitulo;
        private ClsButtonCompras ComprasBtnNuevoPago;

        private ClsTableLayoutPanelCompras ComprasTlpFiltros;
        private ComprasUsrLabel ComprasLblProveedor, ComprasLblEstado, ComprasLblDesde, ComprasLblHasta;
        private ComprasUsrComboBox ComprasCboProveedor, ComprasCboEstado;
        private ComprasUsrTextBox ComprasTxtDesde, ComprasTxtHasta;

        private ComprasUsrDataGridView ComprasDgvPagos;

        private ClsGroupBoxCompras ComprasGbxRegistrarPago;
        private ClsTableLayoutPanelCompras ComprasTlpRegistro;
        private ComprasUsrLabel ComprasLblProveedorPago, ComprasLblFactura, ComprasLblFechaPago, ComprasLblMonto, ComprasLblMetodo, ComprasLblReferencia;
        private ComprasUsrComboBox ComprasCboProveedorPago, ComprasCboFactura, ComprasCboMetodo;
        private ComprasUsrTextBox ComprasTxtFechaPago, ComprasTxtMonto, ComprasTxtReferencia;
        private Panel ComprasPnlBotones;
        private ClsButtonCompras ComprasBtnEliminar, ComprasBtnEditar, ComprasBtnCancelar, ComprasBtnGuardar;
    }
}