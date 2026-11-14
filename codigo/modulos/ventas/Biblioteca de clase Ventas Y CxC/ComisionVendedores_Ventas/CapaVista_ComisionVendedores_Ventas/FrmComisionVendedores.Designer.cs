namespace ComisionVendedoresVista
{
    partial class FrmComisionVendedores
    {
        private System.ComponentModel.IContainer _Components = null;
        private System.Windows.Forms.TabControl VentasTbcPestanas;
        private System.Windows.Forms.TabPage VentasTbpCalculo;
        private System.Windows.Forms.TabPage VentasTbpAsignarFactura;
        private System.Windows.Forms.TabPage VentasTbpMantenimiento;
        private System.Windows.Forms.Label VentasLblFechaInicio;
        private System.Windows.Forms.Label VentasLblFechaFin;
        private System.Windows.Forms.Label VentasLblAlcance;
        private System.Windows.Forms.Label VentasLblPorcentajeNuevo;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaInicio;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaFin;
        private System.Windows.Forms.Button VentasBtnCalcular;
        private System.Windows.Forms.Button VentasBtnAyuda;
        private System.Windows.Forms.DataGridView VentasDgvResultados;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVendedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmElemento;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmMonto;
        private System.Windows.Forms.Button VentasBtnAnteriorResultados;
        private System.Windows.Forms.Button VentasBtnSiguienteResultados;
        private System.Windows.Forms.Label VentasLblPaginaResultados;
        private System.Windows.Forms.Label VentasLblEstado;
        private System.Windows.Forms.ComboBox VentasCboAlcance;
        private System.Windows.Forms.TextBox VentasTxtPorcentaje;
        private System.Windows.Forms.Button VentasBtnBuscar;
        private System.Windows.Forms.Button VentasBtnActualizar;
        private System.Windows.Forms.DataGridView VentasDgvPorcentajes;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentajeMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentajeLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentajeCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentajeProducto;
        private System.Windows.Forms.Label VentasLblMantenimientoEstado;
        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblFactura;
        private System.Windows.Forms.Label VentasLblVendedorFactura;
        private System.Windows.Forms.Label VentasLblMetodoFactura;
        private System.Windows.Forms.Label VentasLblEstadoFactura;
        private System.Windows.Forms.ComboBox VentasCboFactura;
        private System.Windows.Forms.ComboBox VentasCboVendedorFactura;
        private System.Windows.Forms.ComboBox VentasCboMetodoFactura;
        private System.Windows.Forms.Button VentasBtnCalcularFactura;
        private System.Windows.Forms.Button VentasBtnAsignarVendedorFactura;
        private System.Windows.Forms.Button VentasBtnRegistrarAbonoFactura;
        private System.Windows.Forms.DataGridView VentasDgvDetalleFactura;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleElemento;
        private System.Windows.Forms.DataGridViewComboBoxColumn VentasClmMetodoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetallePorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleComision;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleCobrado;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleAbono;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleDisponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetallePendiente;
        private System.Windows.Forms.Button VentasBtnAnteriorFactura;
        private System.Windows.Forms.Button VentasBtnSiguienteFactura;
        private System.Windows.Forms.Label VentasLblPaginaFactura;
        private System.Windows.Forms.Button VentasBtnAnteriorPorcentajes;
        private System.Windows.Forms.Button VentasBtnSiguientePorcentajes;
        private System.Windows.Forms.Label VentasLblPaginaPorcentajes;
        private System.Windows.Forms.Label VentasLblObligatorioFechaInicio;
        private System.Windows.Forms.Label VentasLblObligatorioFechaFin;
        private System.Windows.Forms.Label VentasLblObligatorioFactura;
        private System.Windows.Forms.Label VentasLblObligatorioVendedor;
        private System.Windows.Forms.Label VentasLblObligatorioAlcance;
        private System.Windows.Forms.Label VentasLblObligatorioPorcentaje;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Components != null) _Components.Dispose();
            if (Disposing && _ErrorProveedor != null) _ErrorProveedor.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager RecursosFormulario = new System.ComponentModel.ComponentResourceManager(typeof(FrmComisionVendedores));
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle EstiloCelda36 = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasTbcPestanas = new System.Windows.Forms.TabControl();
            this.VentasTbpCalculo = new System.Windows.Forms.TabPage();
            this.VentasDgvResultados = new System.Windows.Forms.DataGridView();
            this.VentasClmVendedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmElemento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmMonto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasBtnAnteriorResultados = new System.Windows.Forms.Button();
            this.VentasBtnSiguienteResultados = new System.Windows.Forms.Button();
            this.VentasLblPaginaResultados = new System.Windows.Forms.Label();
            this.VentasLblFechaInicio = new System.Windows.Forms.Label();
            this.VentasLblFechaFin = new System.Windows.Forms.Label();
            this.VentasLblEstado = new System.Windows.Forms.Label();
            this.VentasBtnAyuda = new System.Windows.Forms.Button();
            this.VentasBtnCalcular = new System.Windows.Forms.Button();
            this.VentasDtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.VentasDtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.VentasTbpAsignarFactura = new System.Windows.Forms.TabPage();
            this.VentasDgvDetalleFactura = new System.Windows.Forms.DataGridView();
            this.VentasClmDetalleProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleElemento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmMetodoProducto = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VentasClmDetalleCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetallePorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleComision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleCobrado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleAbono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleDisponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetallePendiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasBtnAnteriorFactura = new System.Windows.Forms.Button();
            this.VentasBtnSiguienteFactura = new System.Windows.Forms.Button();
            this.VentasLblPaginaFactura = new System.Windows.Forms.Label();
            this.VentasLblEstadoFactura = new System.Windows.Forms.Label();
            this.VentasBtnCalcularFactura = new System.Windows.Forms.Button();
            this.VentasBtnAsignarVendedorFactura = new System.Windows.Forms.Button();
            this.VentasBtnRegistrarAbonoFactura = new System.Windows.Forms.Button();
            this.VentasCboMetodoFactura = new System.Windows.Forms.ComboBox();
            this.VentasCboVendedorFactura = new System.Windows.Forms.ComboBox();
            this.VentasCboFactura = new System.Windows.Forms.ComboBox();
            this.VentasLblMetodoFactura = new System.Windows.Forms.Label();
            this.VentasLblVendedorFactura = new System.Windows.Forms.Label();
            this.VentasLblFactura = new System.Windows.Forms.Label();
            this.VentasTbpMantenimiento = new System.Windows.Forms.TabPage();
            this.VentasDgvPorcentajes = new System.Windows.Forms.DataGridView();
            this.VentasClmProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentajeMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentajeLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentajeCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmPorcentajeProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasBtnAnteriorPorcentajes = new System.Windows.Forms.Button();
            this.VentasBtnSiguientePorcentajes = new System.Windows.Forms.Button();
            this.VentasLblPaginaPorcentajes = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaInicio = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaFin = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFactura = new System.Windows.Forms.Label();
            this.VentasLblObligatorioVendedor = new System.Windows.Forms.Label();
            this.VentasLblObligatorioAlcance = new System.Windows.Forms.Label();
            this.VentasLblObligatorioPorcentaje = new System.Windows.Forms.Label();
            this.VentasLblAlcance = new System.Windows.Forms.Label();
            this.VentasLblPorcentajeNuevo = new System.Windows.Forms.Label();
            this.VentasLblMantenimientoEstado = new System.Windows.Forms.Label();
            this.VentasBtnActualizar = new System.Windows.Forms.Button();
            this.VentasBtnBuscar = new System.Windows.Forms.Button();
            this.VentasTxtPorcentaje = new System.Windows.Forms.TextBox();
            this.VentasCboAlcance = new System.Windows.Forms.ComboBox();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasTbcPestanas.SuspendLayout();
            this.VentasTbpCalculo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvResultados)).BeginInit();
            this.VentasTbpAsignarFactura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalleFactura)).BeginInit();
            this.VentasTbpMantenimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvPorcentajes)).BeginInit();
            this.VentasPnlEncabezado.SuspendLayout();
            this.SuspendLayout();
            //
            // VentasTbcPestanas
            //
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpCalculo);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpAsignarFactura);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpMantenimiento);
            this.VentasTbcPestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasTbcPestanas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasTbcPestanas.Location = new System.Drawing.Point(0, 82);
            this.VentasTbcPestanas.Name = "VentasTbcPestanas";
            this.VentasTbcPestanas.SelectedIndex = 0;
            this.VentasTbcPestanas.Size = new System.Drawing.Size(1280, 618);
            this.VentasTbcPestanas.TabIndex = 0;
            //
            // VentasTbpCalculo
            //
            this.VentasTbpCalculo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpCalculo.Controls.Add(this.VentasDgvResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnAnteriorResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnSiguienteResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblPaginaResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblFechaInicio);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblEstado);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblObligatorioFechaInicio);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblObligatorioFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnAyuda);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnCalcular);
            this.VentasTbpCalculo.Controls.Add(this.VentasDtpFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasDtpFechaInicio);
            this.VentasTbpCalculo.Location = new System.Drawing.Point(4, 32);
            this.VentasTbpCalculo.Name = "VentasTbpCalculo";
            this.VentasTbpCalculo.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpCalculo.Size = new System.Drawing.Size(1272, 582);
            this.VentasTbpCalculo.TabIndex = 0;
            this.VentasTbpCalculo.Text = "Calcular comisión";
            //
            // VentasDgvResultados
            //
            this.VentasDgvResultados.AllowUserToAddRows = false;
            EstiloCelda25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            EstiloCelda25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvResultados.AlternatingRowsDefaultCellStyle = EstiloCelda25;
            this.VentasDgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda26.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda26.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            EstiloCelda26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvResultados.ColumnHeadersDefaultCellStyle = EstiloCelda26;
            this.VentasDgvResultados.ColumnHeadersHeight = 29;
            this.VentasDgvResultados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmVendedor,
            this.VentasClmElemento,
            this.VentasClmPorcentaje,
            this.VentasClmBase,
            this.VentasClmMonto});
            EstiloCelda30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda30.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda30.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda30.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda30.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvResultados.DefaultCellStyle = EstiloCelda30;
            this.VentasDgvResultados.EnableHeadersVisualStyles = false;
            this.VentasDgvResultados.Location = new System.Drawing.Point(24, 104);
            this.VentasDgvResultados.Name = "VentasDgvResultados";
            this.VentasDgvResultados.ReadOnly = true;
            this.VentasDgvResultados.RowHeadersVisible = false;
            this.VentasDgvResultados.RowHeadersWidth = 51;
            this.VentasDgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvResultados.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvResultados.Size = new System.Drawing.Size(1210, 435);
            this.VentasDgvResultados.TabIndex = 0;
            this.VentasDgvResultados.MultiSelect = false;
            this.VentasDgvResultados.AutoGenerateColumns = false;
            //
            // VentasClmVendedor
            //
            this.VentasClmVendedor.DataPropertyName = "NombreVendedor";
            this.VentasClmVendedor.HeaderText = "Vendedor";
            this.VentasClmVendedor.MinimumWidth = 6;
            this.VentasClmVendedor.Name = "VentasClmVendedor";
            this.VentasClmVendedor.ReadOnly = true;
            //
            // VentasClmElemento
            //
            this.VentasClmElemento.DataPropertyName = "ElementoComision";
            this.VentasClmElemento.HeaderText = "Factura / producto";
            this.VentasClmElemento.MinimumWidth = 6;
            this.VentasClmElemento.Name = "VentasClmElemento";
            this.VentasClmElemento.ReadOnly = true;
            //
            // VentasClmPorcentaje
            //
            this.VentasClmPorcentaje.DataPropertyName = "EtiquetaMetodoPorcentaje";
            EstiloCelda27.Format = "0.00\\ \\%";
            EstiloCelda27.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmPorcentaje.DefaultCellStyle = EstiloCelda27;
            this.VentasClmPorcentaje.HeaderText = "Método / %";
            this.VentasClmPorcentaje.MinimumWidth = 6;
            this.VentasClmPorcentaje.Name = "VentasClmPorcentaje";
            this.VentasClmPorcentaje.ReadOnly = true;
            //
            // VentasClmBase
            //
            this.VentasClmBase.DataPropertyName = "BaseVentas";
            EstiloCelda28.Format = "#,##0.00";
            EstiloCelda28.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmBase.DefaultCellStyle = EstiloCelda28;
            this.VentasClmBase.HeaderText = "Base cobrada";
            this.VentasClmBase.MinimumWidth = 6;
            this.VentasClmBase.Name = "VentasClmBase";
            this.VentasClmBase.ReadOnly = true;
            //
            // VentasClmMonto
            //
            this.VentasClmMonto.DataPropertyName = "MontoComision";
            EstiloCelda29.Format = "#,##0.00";
            EstiloCelda29.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmMonto.DefaultCellStyle = EstiloCelda29;
            this.VentasClmMonto.HeaderText = "Comisión";
            this.VentasClmMonto.MinimumWidth = 6;
            this.VentasClmMonto.Name = "VentasClmMonto";
            this.VentasClmMonto.ReadOnly = true;
            //
            // Paginación de resultados
            //
            this.VentasBtnAnteriorResultados.Enabled = false;
            this.VentasBtnAnteriorResultados.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorResultados.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.VentasBtnAnteriorResultados.Location = new System.Drawing.Point(24, 550);
            this.VentasBtnAnteriorResultados.Name = "VentasBtnAnteriorResultados";
            this.VentasBtnAnteriorResultados.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnAnteriorResultados.TabIndex = 10;
            this.VentasBtnAnteriorResultados.Text = "Anterior";
            this.VentasBtnAnteriorResultados.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorResultados.Click += new System.EventHandler(this.VentasMetCambiarPaginaResultados);
            this.VentasBtnSiguienteResultados.Enabled = false;
            this.VentasBtnSiguienteResultados.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguienteResultados.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguienteResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguienteResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.VentasBtnSiguienteResultados.Location = new System.Drawing.Point(1134, 550);
            this.VentasBtnSiguienteResultados.Name = "VentasBtnSiguienteResultados";
            this.VentasBtnSiguienteResultados.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguienteResultados.TabIndex = 11;
            this.VentasBtnSiguienteResultados.Text = "Siguiente";
            this.VentasBtnSiguienteResultados.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteResultados.Click += new System.EventHandler(this.VentasMetCambiarPaginaResultados);
            this.VentasLblPaginaResultados.AutoSize = false;
            this.VentasLblPaginaResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaResultados.Location = new System.Drawing.Point(486, 554);
            this.VentasLblPaginaResultados.Name = "VentasLblPaginaResultados";
            this.VentasLblPaginaResultados.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaResultados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasLblPaginaResultados.TabIndex = 12;
            this.VentasLblPaginaResultados.Text = "Página 0 de 0 · 0 registros";
            //
            // VentasLblFechaInicio
            //
            this.VentasLblFechaInicio.AutoSize = true;
            this.VentasLblFechaInicio.Location = new System.Drawing.Point(24, 8);
            this.VentasLblFechaInicio.Name = "VentasLblFechaInicio";
            this.VentasLblFechaInicio.Size = new System.Drawing.Size(102, 23);
            this.VentasLblFechaInicio.TabIndex = 1;
            this.VentasLblFechaInicio.Text = "Fecha inicial";
            //
            // VentasLblFechaFin
            //
            this.VentasLblFechaFin.AutoSize = true;
            this.VentasLblFechaFin.Location = new System.Drawing.Point(200, 8);
            this.VentasLblFechaFin.Name = "VentasLblFechaFin";
            this.VentasLblFechaFin.Size = new System.Drawing.Size(91, 23);
            this.VentasLblFechaFin.TabIndex = 2;
            this.VentasLblFechaFin.Text = "Fecha final";
            //
            // VentasLblEstado
            //
            this.VentasLblEstado.AutoSize = true;
            this.VentasLblEstado.Location = new System.Drawing.Point(24, 70);
            this.VentasLblEstado.Name = "VentasLblEstado";
            this.VentasLblEstado.Size = new System.Drawing.Size(1210, 28);
            this.VentasLblEstado.TabIndex = 4;
            this.VentasLblEstado.Text = "Periodo · Método · Base VENTAS";
            //
            this.VentasBtnAyuda.BackColor = System.Drawing.Color.FromArgb(228, 212, 234);
            this.VentasBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAyuda.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAyuda.Location = new System.Drawing.Point(718, 24);
            this.VentasBtnAyuda.Name = "VentasBtnAyuda";
            this.VentasBtnAyuda.Size = new System.Drawing.Size(90, 30);
            this.VentasBtnAyuda.TabIndex = 5;
            this.VentasBtnAyuda.Text = "Ayuda";
            this.VentasBtnAyuda.UseVisualStyleBackColor = false;
            //
            // VentasBtnCalcular
            //
            this.VentasBtnCalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcular.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcular.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnCalcular.Location = new System.Drawing.Point(572, 24);
            this.VentasBtnCalcular.Name = "VentasBtnCalcular";
            this.VentasBtnCalcular.Size = new System.Drawing.Size(130, 30);
            this.VentasBtnCalcular.TabIndex = 4;
            this.VentasBtnCalcular.Text = "Calcular";
            this.VentasBtnCalcular.UseVisualStyleBackColor = false;
            this.VentasBtnCalcular.Click += new System.EventHandler(this.VentasMetCalcularClick);
            //
            // VentasDtpFechaFin
            //
            this.VentasDtpFechaFin.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaFin.Location = new System.Drawing.Point(200, 26);
            this.VentasDtpFechaFin.Name = "VentasDtpFechaFin";
            this.VentasDtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpFechaFin.Size = new System.Drawing.Size(160, 30);
            this.VentasDtpFechaFin.TabIndex = 8;
            //
            // VentasDtpFechaInicio
            //
            this.VentasDtpFechaInicio.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaInicio.Location = new System.Drawing.Point(24, 26);
            this.VentasDtpFechaInicio.Name = "VentasDtpFechaInicio";
            this.VentasDtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpFechaInicio.Size = new System.Drawing.Size(160, 30);
            this.VentasDtpFechaInicio.TabIndex = 9;
            this.VentasLblObligatorioFechaInicio.AutoSize = true;
            this.VentasLblObligatorioFechaInicio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFechaInicio.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFechaInicio.Location = new System.Drawing.Point(120, 8);
            this.VentasLblObligatorioFechaInicio.Name = "VentasLblObligatorioFechaInicio";
            this.VentasLblObligatorioFechaInicio.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioFechaInicio.TabIndex = 13;
            this.VentasLblObligatorioFechaInicio.Text = "*";
            this.VentasLblObligatorioFechaInicio.TabStop = false;
            this.VentasLblObligatorioFechaFin.AutoSize = true;
            this.VentasLblObligatorioFechaFin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFechaFin.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFechaFin.Location = new System.Drawing.Point(284, 8);
            this.VentasLblObligatorioFechaFin.Name = "VentasLblObligatorioFechaFin";
            this.VentasLblObligatorioFechaFin.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioFechaFin.TabIndex = 14;
            this.VentasLblObligatorioFechaFin.Text = "*";
            this.VentasLblObligatorioFechaFin.TabStop = false;
            //
            // VentasTbpAsignarFactura
            //
            this.VentasTbpAsignarFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasDgvDetalleFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnAnteriorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnSiguienteFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblPaginaFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblEstadoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnCalcularFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnAsignarVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnRegistrarAbonoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioVendedor);
            this.VentasTbpAsignarFactura.Location = new System.Drawing.Point(4, 32);
            this.VentasTbpAsignarFactura.Name = "VentasTbpAsignarFactura";
            this.VentasTbpAsignarFactura.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpAsignarFactura.Size = new System.Drawing.Size(1272, 582);
            this.VentasTbpAsignarFactura.TabIndex = 1;
            this.VentasTbpAsignarFactura.Text = "Asignar vendedor a factura";
            //
            // VentasDgvDetalleFactura
            //
            this.VentasDgvDetalleFactura.AllowUserToAddRows = false;
            EstiloCelda16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvDetalleFactura.AlternatingRowsDefaultCellStyle = EstiloCelda16;
            this.VentasDgvDetalleFactura.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda17.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            EstiloCelda17.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            EstiloCelda17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvDetalleFactura.ColumnHeadersDefaultCellStyle = EstiloCelda17;
            this.VentasDgvDetalleFactura.ColumnHeadersHeight = 29;
            this.VentasDgvDetalleFactura.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmDetalleProducto,
            this.VentasClmDetalleElemento,
            this.VentasClmMetodoProducto,
            this.VentasClmDetalleCantidad,
            this.VentasClmDetalleBase,
            this.VentasClmDetallePorcentaje,
            this.VentasClmDetalleComision,
            this.VentasClmDetalleCobrado,
            this.VentasClmDetalleAbono,
            this.VentasClmDetalleDisponible,
            this.VentasClmDetallePendiente});
            this.VentasDgvDetalleFactura.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VentasMetFormatearMetodoProducto);
            EstiloCelda24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda24.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda24.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda24.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda24.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvDetalleFactura.DefaultCellStyle = EstiloCelda24;
            this.VentasDgvDetalleFactura.EnableHeadersVisualStyles = false;
            this.VentasDgvDetalleFactura.Location = new System.Drawing.Point(24, 170);
            this.VentasDgvDetalleFactura.Name = "VentasDgvDetalleFactura";
            this.VentasDgvDetalleFactura.ReadOnly = false;
            this.VentasDgvDetalleFactura.RowHeadersVisible = false;
            this.VentasDgvDetalleFactura.RowHeadersWidth = 51;
            this.VentasDgvDetalleFactura.RowTemplate.Height = 22;
            this.VentasDgvDetalleFactura.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvDetalleFactura.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvDetalleFactura.Size = new System.Drawing.Size(1210, 368);
            this.VentasDgvDetalleFactura.TabIndex = 0;
            this.VentasDgvDetalleFactura.AutoGenerateColumns = false;
            //
            // VentasClmDetalleProducto
            //
            this.VentasClmDetalleProducto.DataPropertyName = "NombreProducto";
            this.VentasClmDetalleProducto.HeaderText = "Producto";
            this.VentasClmDetalleProducto.MinimumWidth = 6;
            this.VentasClmDetalleProducto.Name = "VentasClmDetalleProducto";
            this.VentasClmDetalleProducto.ReadOnly = true;
            //
            // VentasClmDetalleElemento
            //
            this.VentasClmDetalleElemento.DataPropertyName = "ElementoComision";
            this.VentasClmDetalleElemento.HeaderText = "Criterio";
            this.VentasClmDetalleElemento.MinimumWidth = 6;
            this.VentasClmDetalleElemento.Name = "VentasClmDetalleElemento";
            this.VentasClmDetalleElemento.ReadOnly = true;
            this.VentasClmMetodoProducto.DataPropertyName = "MetodoComision";
            this.VentasClmMetodoProducto.HeaderText = "Comisión por";
            this.VentasClmMetodoProducto.MinimumWidth = 95;
            this.VentasClmMetodoProducto.Name = "VentasClmMetodoProducto";
            this.VentasClmMetodoProducto.Items.AddRange(new object[] { "Linea", "Marca", "Categoria", "Producto" });
            this.VentasClmMetodoProducto.ReadOnly = false;
            //
            // VentasClmDetalleCantidad
            //
            this.VentasClmDetalleCantidad.DataPropertyName = "Cantidad";
            EstiloCelda36.Format = "N2";
            EstiloCelda36.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleCantidad.DefaultCellStyle = EstiloCelda36;
            this.VentasClmDetalleCantidad.HeaderText = "Cantidad";
            this.VentasClmDetalleCantidad.MinimumWidth = 6;
            this.VentasClmDetalleCantidad.Name = "VentasClmDetalleCantidad";
            this.VentasClmDetalleCantidad.ReadOnly = true;
            //
            // VentasClmDetalleBase
            //
            this.VentasClmDetalleBase.DataPropertyName = "BaseVenta";
            EstiloCelda18.Format = "#,##0.00";
            EstiloCelda18.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleBase.DefaultCellStyle = EstiloCelda18;
            this.VentasClmDetalleBase.HeaderText = "Venta";
            this.VentasClmDetalleBase.MinimumWidth = 6;
            this.VentasClmDetalleBase.Name = "VentasClmDetalleBase";
            this.VentasClmDetalleBase.ReadOnly = true;
            //
            // VentasClmDetallePorcentaje
            //
            this.VentasClmDetallePorcentaje.DataPropertyName = "Porcentaje";
            EstiloCelda35.Format = "0.00\\%";
            EstiloCelda35.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetallePorcentaje.DefaultCellStyle = EstiloCelda35;
            this.VentasClmDetallePorcentaje.HeaderText = "%";
            this.VentasClmDetallePorcentaje.MinimumWidth = 6;
            this.VentasClmDetallePorcentaje.Name = "VentasClmDetallePorcentaje";
            this.VentasClmDetallePorcentaje.ReadOnly = true;
            //
            // VentasClmDetalleComision
            //
            this.VentasClmDetalleComision.DataPropertyName = "ComisionTotal";
            EstiloCelda19.Format = "#,##0.00";
            EstiloCelda19.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleComision.DefaultCellStyle = EstiloCelda19;
            this.VentasClmDetalleComision.HeaderText = "Comisión total";
            this.VentasClmDetalleComision.MinimumWidth = 6;
            this.VentasClmDetalleComision.Name = "VentasClmDetalleComision";
            this.VentasClmDetalleComision.ReadOnly = true;
            //
            // VentasClmDetalleCobrado
            //
            this.VentasClmDetalleCobrado.DataPropertyName = "VentaCobrado";
            EstiloCelda20.Format = "#,##0.00";
            EstiloCelda20.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleCobrado.DefaultCellStyle = EstiloCelda20;
            this.VentasClmDetalleCobrado.HeaderText = "Venta cobrada";
            this.VentasClmDetalleCobrado.MinimumWidth = 6;
            this.VentasClmDetalleCobrado.Name = "VentasClmDetalleCobrado";
            this.VentasClmDetalleCobrado.ReadOnly = true;
            //
            // VentasClmDetalleAbono
            //
            this.VentasClmDetalleAbono.DataPropertyName = "ComisionUltimoAbono";
            EstiloCelda21.Format = "#,##0.00";
            EstiloCelda21.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleAbono.DefaultCellStyle = EstiloCelda21;
            this.VentasClmDetalleAbono.HeaderText = "Comisión último abono";
            this.VentasClmDetalleAbono.MinimumWidth = 6;
            this.VentasClmDetalleAbono.Name = "VentasClmDetalleAbono";
            this.VentasClmDetalleAbono.ReadOnly = true;
            //
            // VentasClmDetalleDisponible
            //
            this.VentasClmDetalleDisponible.DataPropertyName = "ComisionDisponible";
            EstiloCelda22.Format = "#,##0.00";
            EstiloCelda22.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleDisponible.DefaultCellStyle = EstiloCelda22;
            this.VentasClmDetalleDisponible.HeaderText = "Disponible acumulado";
            this.VentasClmDetalleDisponible.MinimumWidth = 6;
            this.VentasClmDetalleDisponible.Name = "VentasClmDetalleDisponible";
            this.VentasClmDetalleDisponible.ReadOnly = true;
            //
            // VentasClmDetallePendiente
            //
            this.VentasClmDetallePendiente.DataPropertyName = "ComisionPendiente";
            EstiloCelda23.Format = "#,##0.00";
            EstiloCelda23.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetallePendiente.DefaultCellStyle = EstiloCelda23;
            this.VentasClmDetallePendiente.HeaderText = "Pendiente";
            this.VentasClmDetallePendiente.MinimumWidth = 6;
            this.VentasClmDetallePendiente.Name = "VentasClmDetallePendiente";
            this.VentasClmDetallePendiente.ReadOnly = true;
            //
            // Paginación de detalle de factura
            //
            this.VentasBtnAnteriorFactura.Enabled = false;
            this.VentasBtnAnteriorFactura.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.VentasBtnAnteriorFactura.Location = new System.Drawing.Point(24, 550);
            this.VentasBtnAnteriorFactura.Name = "VentasBtnAnteriorFactura";
            this.VentasBtnAnteriorFactura.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnAnteriorFactura.TabIndex = 10;
            this.VentasBtnAnteriorFactura.Text = "Anterior";
            this.VentasBtnAnteriorFactura.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorFactura.Click += new System.EventHandler(this.VentasMetCambiarPaginaFactura);
            this.VentasBtnSiguienteFactura.Enabled = false;
            this.VentasBtnSiguienteFactura.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguienteFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguienteFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguienteFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.VentasBtnSiguienteFactura.Location = new System.Drawing.Point(1134, 550);
            this.VentasBtnSiguienteFactura.Name = "VentasBtnSiguienteFactura";
            this.VentasBtnSiguienteFactura.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguienteFactura.TabIndex = 11;
            this.VentasBtnSiguienteFactura.Text = "Siguiente";
            this.VentasBtnSiguienteFactura.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteFactura.Click += new System.EventHandler(this.VentasMetCambiarPaginaFactura);
            this.VentasLblPaginaFactura.AutoSize = false;
            this.VentasLblPaginaFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaFactura.Location = new System.Drawing.Point(486, 554);
            this.VentasLblPaginaFactura.Name = "VentasLblPaginaFactura";
            this.VentasLblPaginaFactura.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaFactura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasLblPaginaFactura.TabIndex = 12;
            this.VentasLblPaginaFactura.Text = "Página 0 de 0 · 0 registros";
            //
            // VentasLblEstadoFactura
            //
            this.VentasLblEstadoFactura.AutoSize = false;
            this.VentasLblEstadoFactura.Location = new System.Drawing.Point(24, 126);
            this.VentasLblEstadoFactura.Name = "VentasLblEstadoFactura";
            this.VentasLblEstadoFactura.Size = new System.Drawing.Size(1210, 40);
            this.VentasLblEstadoFactura.TabIndex = 1;
            this.VentasLblEstadoFactura.Text = "Elija el método para cada producto; use la paginación para revisar toda la factura.";
            //
            // VentasBtnCalcularFactura
            //
            this.VentasBtnCalcularFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcularFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcularFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcularFactura.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnCalcularFactura.Location = new System.Drawing.Point(24, 85);
            this.VentasBtnCalcularFactura.Name = "VentasBtnCalcularFactura";
            this.VentasBtnCalcularFactura.Size = new System.Drawing.Size(180, 30);
            this.VentasBtnCalcularFactura.TabIndex = 2;
            this.VentasBtnCalcularFactura.Text = "Calcular comisión";
            this.VentasBtnCalcularFactura.UseVisualStyleBackColor = false;
            this.VentasBtnCalcularFactura.Click += new System.EventHandler(this.VentasMetCalcularFacturaClick);
            //
            // VentasBtnAsignarVendedorFactura
            //
            this.VentasBtnAsignarVendedorFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.VentasBtnAsignarVendedorFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnAsignarVendedorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAsignarVendedorFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnAsignarVendedorFactura.Location = new System.Drawing.Point(214, 85);
            this.VentasBtnAsignarVendedorFactura.Name = "VentasBtnAsignarVendedorFactura";
            this.VentasBtnAsignarVendedorFactura.Size = new System.Drawing.Size(180, 30);
            this.VentasBtnAsignarVendedorFactura.TabIndex = 14;
            this.VentasBtnAsignarVendedorFactura.Text = "Asignar vendedor";
            this.VentasBtnAsignarVendedorFactura.UseVisualStyleBackColor = false;
            this.VentasBtnAsignarVendedorFactura.Click += new System.EventHandler(this.VentasMetAsignarVendedorFacturaClick);
            //
            // VentasBtnRegistrarAbonoFactura
            //
            this.VentasBtnRegistrarAbonoFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnRegistrarAbonoFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnRegistrarAbonoFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnRegistrarAbonoFactura.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnRegistrarAbonoFactura.Location = new System.Drawing.Point(404, 85);
            this.VentasBtnRegistrarAbonoFactura.Name = "VentasBtnRegistrarAbonoFactura";
            this.VentasBtnRegistrarAbonoFactura.Size = new System.Drawing.Size(190, 30);
            this.VentasBtnRegistrarAbonoFactura.TabIndex = 15;
            this.VentasBtnRegistrarAbonoFactura.Text = "Realizar pago";
            this.VentasBtnRegistrarAbonoFactura.UseVisualStyleBackColor = false;
            this.VentasBtnRegistrarAbonoFactura.Click += new System.EventHandler(this.VentasMetRegistrarAbonoFacturaClick);
            //
            //
            // VentasCboMetodoFactura
            //
            this.VentasCboMetodoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboMetodoFactura.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.VentasCboMetodoFactura.Location = new System.Drawing.Point(650, 45);
            this.VentasCboMetodoFactura.Name = "VentasCboMetodoFactura";
            this.VentasCboMetodoFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboMetodoFactura.SelectedIndex = 0;
            this.VentasCboMetodoFactura.Size = new System.Drawing.Size(160, 31);
            this.VentasCboMetodoFactura.TabIndex = 4;
            //
            // VentasCboVendedorFactura
            //
            this.VentasCboVendedorFactura.DisplayMember = "NombreVendedor";
            this.VentasCboVendedorFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboVendedorFactura.Location = new System.Drawing.Point(430, 45);
            this.VentasCboVendedorFactura.Name = "VentasCboVendedorFactura";
            this.VentasCboVendedorFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboVendedorFactura.Size = new System.Drawing.Size(200, 31);
            this.VentasCboVendedorFactura.TabIndex = 5;
            this.VentasCboVendedorFactura.ValueMember = "IdVendedor";
            //
            // VentasCboFactura
            //
            this.VentasCboFactura.DisplayMember = "Etiqueta";
            this.VentasCboFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboFactura.Location = new System.Drawing.Point(24, 45);
            this.VentasCboFactura.Name = "VentasCboFactura";
            this.VentasCboFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboFactura.Size = new System.Drawing.Size(390, 31);
            this.VentasCboFactura.TabIndex = 6;
            this.VentasCboFactura.ValueMember = "IdFactura";
            //
            // VentasLblMetodoFactura
            //
            this.VentasLblMetodoFactura.AutoSize = true;
            this.VentasLblMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblMetodoFactura.Location = new System.Drawing.Point(646, 22);
            this.VentasLblMetodoFactura.Name = "VentasLblMetodoFactura";
            this.VentasLblMetodoFactura.Size = new System.Drawing.Size(102, 20);
            this.VentasLblMetodoFactura.TabIndex = 7;
            this.VentasLblMetodoFactura.Text = "Método predeterminado";
            //
            // VentasLblVendedorFactura
            //
            this.VentasLblVendedorFactura.AutoSize = true;
            this.VentasLblVendedorFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblVendedorFactura.Location = new System.Drawing.Point(426, 16);
            this.VentasLblVendedorFactura.Name = "VentasLblVendedorFactura";
            this.VentasLblVendedorFactura.Size = new System.Drawing.Size(143, 20);
            this.VentasLblVendedorFactura.TabIndex = 8;
            this.VentasLblVendedorFactura.Text = "Vendedor asignado";
            //
            // VentasLblFactura
            //
            this.VentasLblFactura.AutoSize = true;
            this.VentasLblFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblFactura.Location = new System.Drawing.Point(28, 22);
            this.VentasLblFactura.Name = "VentasLblFactura";
            this.VentasLblFactura.Size = new System.Drawing.Size(114, 20);
            this.VentasLblFactura.TabIndex = 9;
            this.VentasLblFactura.Text = "Venta / factura";
            this.VentasLblObligatorioFactura.AutoSize = true;
            this.VentasLblObligatorioFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFactura.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFactura.Location = new System.Drawing.Point(141, 22);
            this.VentasLblObligatorioFactura.Name = "VentasLblObligatorioFactura";
            this.VentasLblObligatorioFactura.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioFactura.TabIndex = 13;
            this.VentasLblObligatorioFactura.Text = "*";
            this.VentasLblObligatorioFactura.TabStop = false;
            this.VentasLblObligatorioVendedor.AutoSize = true;
            this.VentasLblObligatorioVendedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioVendedor.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioVendedor.Location = new System.Drawing.Point(570, 16);
            this.VentasLblObligatorioVendedor.Name = "VentasLblObligatorioVendedor";
            this.VentasLblObligatorioVendedor.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioVendedor.TabIndex = 14;
            this.VentasLblObligatorioVendedor.Text = "*";
            this.VentasLblObligatorioVendedor.TabStop = false;
            //
            // VentasTbpMantenimiento
            //
            this.VentasTbpMantenimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpMantenimiento.Controls.Add(this.VentasDgvPorcentajes);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasBtnAnteriorPorcentajes);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasBtnSiguientePorcentajes);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblPaginaPorcentajes);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblAlcance);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblPorcentajeNuevo);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblMantenimientoEstado);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblObligatorioAlcance);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblObligatorioPorcentaje);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasBtnActualizar);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasBtnBuscar);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasTxtPorcentaje);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasCboAlcance);
            this.VentasTbpMantenimiento.Location = new System.Drawing.Point(4, 32);
            this.VentasTbpMantenimiento.Name = "VentasTbpMantenimiento";
            this.VentasTbpMantenimiento.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpMantenimiento.Size = new System.Drawing.Size(1272, 582);
            this.VentasTbpMantenimiento.TabIndex = 2;
            this.VentasTbpMantenimiento.Text = "Mantenimiento de porcentajes";
            //
            // VentasDgvPorcentajes
            //
            this.VentasDgvPorcentajes.AllowUserToAddRows = false;
            this.VentasDgvPorcentajes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvPorcentajes.AlternatingRowsDefaultCellStyle = EstiloCelda33;
            EstiloCelda31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda31.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            EstiloCelda31.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.VentasDgvPorcentajes.ColumnHeadersDefaultCellStyle = EstiloCelda31;
            this.VentasDgvPorcentajes.ColumnHeadersHeight = 29;
            this.VentasDgvPorcentajes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmProducto,
            this.VentasClmMarca,
            this.VentasClmLinea,
            this.VentasClmCategoria,
            this.VentasClmPorcentajeMarca,
            this.VentasClmPorcentajeLinea,
            this.VentasClmPorcentajeCategoria,
            this.VentasClmPorcentajeProducto});
            this.VentasDgvPorcentajes.Location = new System.Drawing.Point(24, 104);
            this.VentasDgvPorcentajes.Name = "VentasDgvPorcentajes";
            EstiloCelda32.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            EstiloCelda32.Font = new System.Drawing.Font("Segoe UI", 9F);
            EstiloCelda32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda32.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda32.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasDgvPorcentajes.DefaultCellStyle = EstiloCelda32;
            this.VentasDgvPorcentajes.EnableHeadersVisualStyles = false;
            this.VentasDgvPorcentajes.ReadOnly = true;
            this.VentasDgvPorcentajes.RowHeadersVisible = false;
            this.VentasDgvPorcentajes.RowHeadersWidth = 51;
            this.VentasDgvPorcentajes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvPorcentajes.MultiSelect = false;
            this.VentasDgvPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvPorcentajes.Size = new System.Drawing.Size(1210, 435);
            this.VentasDgvPorcentajes.TabIndex = 0;
            this.VentasDgvPorcentajes.AutoGenerateColumns = false;
            //
            // VentasClmProducto
            //
            this.VentasClmProducto.DataPropertyName = "NombreProducto";
            this.VentasClmProducto.HeaderText = "Producto";
            this.VentasClmProducto.MinimumWidth = 6;
            this.VentasClmProducto.Name = "VentasClmProducto";
            this.VentasClmProducto.ReadOnly = true;
            //
            // VentasClmMarca
            //
            this.VentasClmMarca.DataPropertyName = "Marca";
            this.VentasClmMarca.HeaderText = "Marca";
            this.VentasClmMarca.MinimumWidth = 6;
            this.VentasClmMarca.Name = "VentasClmMarca";
            this.VentasClmMarca.ReadOnly = true;
            //
            // VentasClmLinea
            //
            this.VentasClmLinea.DataPropertyName = "Linea";
            this.VentasClmLinea.HeaderText = "Línea";
            this.VentasClmLinea.MinimumWidth = 6;
            this.VentasClmLinea.Name = "VentasClmLinea";
            this.VentasClmLinea.ReadOnly = true;
            //
            // VentasClmCategoria
            //
            this.VentasClmCategoria.DataPropertyName = "Categoria";
            this.VentasClmCategoria.HeaderText = "Categoría";
            this.VentasClmCategoria.MinimumWidth = 6;
            this.VentasClmCategoria.Name = "VentasClmCategoria";
            this.VentasClmCategoria.ReadOnly = true;
            //
            // VentasClmPorcentajeMarca
            //
            EstiloCelda34.Format = "0.00\\ \\%";
            EstiloCelda34.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmPorcentajeMarca.DataPropertyName = "PorcentajeMarca";
            this.VentasClmPorcentajeMarca.DefaultCellStyle = EstiloCelda34;
            this.VentasClmPorcentajeMarca.HeaderText = "% Marca";
            this.VentasClmPorcentajeMarca.MinimumWidth = 6;
            this.VentasClmPorcentajeMarca.Name = "VentasClmPorcentajeMarca";
            this.VentasClmPorcentajeMarca.ReadOnly = true;
            this.VentasClmPorcentajeLinea.DataPropertyName = "PorcentajeLinea";
            this.VentasClmPorcentajeLinea.DefaultCellStyle = EstiloCelda34;
            this.VentasClmPorcentajeLinea.HeaderText = "% Línea";
            this.VentasClmPorcentajeLinea.MinimumWidth = 6;
            this.VentasClmPorcentajeLinea.Name = "VentasClmPorcentajeLinea";
            this.VentasClmPorcentajeLinea.ReadOnly = true;
            this.VentasClmPorcentajeCategoria.DataPropertyName = "PorcentajeCategoria";
            this.VentasClmPorcentajeCategoria.DefaultCellStyle = EstiloCelda34;
            this.VentasClmPorcentajeCategoria.HeaderText = "% Categoría";
            this.VentasClmPorcentajeCategoria.MinimumWidth = 6;
            this.VentasClmPorcentajeCategoria.Name = "VentasClmPorcentajeCategoria";
            this.VentasClmPorcentajeCategoria.ReadOnly = true;
            this.VentasClmPorcentajeProducto.DataPropertyName = "PorcentajeProducto";
            this.VentasClmPorcentajeProducto.DefaultCellStyle = EstiloCelda34;
            this.VentasClmPorcentajeProducto.HeaderText = "% Producto";
            this.VentasClmPorcentajeProducto.MinimumWidth = 6;
            this.VentasClmPorcentajeProducto.Name = "VentasClmPorcentajeProducto";
            this.VentasClmPorcentajeProducto.ReadOnly = true;
            //
            // Paginación de mantenimiento
            //
            this.VentasBtnAnteriorPorcentajes.Enabled = false;
            this.VentasBtnAnteriorPorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorPorcentajes.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorPorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.VentasBtnAnteriorPorcentajes.Location = new System.Drawing.Point(24, 550);
            this.VentasBtnAnteriorPorcentajes.Name = "VentasBtnAnteriorPorcentajes";
            this.VentasBtnAnteriorPorcentajes.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnAnteriorPorcentajes.TabIndex = 8;
            this.VentasBtnAnteriorPorcentajes.Text = "Anterior";
            this.VentasBtnAnteriorPorcentajes.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorPorcentajes.Click += new System.EventHandler(this.VentasMetCambiarPaginaPorcentajes);
            this.VentasBtnSiguientePorcentajes.Enabled = false;
            this.VentasBtnSiguientePorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguientePorcentajes.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguientePorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguientePorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.VentasBtnSiguientePorcentajes.Location = new System.Drawing.Point(1134, 550);
            this.VentasBtnSiguientePorcentajes.Name = "VentasBtnSiguientePorcentajes";
            this.VentasBtnSiguientePorcentajes.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguientePorcentajes.TabIndex = 9;
            this.VentasBtnSiguientePorcentajes.Text = "Siguiente";
            this.VentasBtnSiguientePorcentajes.UseVisualStyleBackColor = false;
            this.VentasBtnSiguientePorcentajes.Click += new System.EventHandler(this.VentasMetCambiarPaginaPorcentajes);
            this.VentasLblPaginaPorcentajes.AutoSize = false;
            this.VentasLblPaginaPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaPorcentajes.Location = new System.Drawing.Point(486, 554);
            this.VentasLblPaginaPorcentajes.Name = "VentasLblPaginaPorcentajes";
            this.VentasLblPaginaPorcentajes.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaPorcentajes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasLblPaginaPorcentajes.TabIndex = 10;
            this.VentasLblPaginaPorcentajes.Text = "Página 0 de 0 · 0 registros";
            //
            // VentasLblAlcance
            //
            this.VentasLblAlcance.AutoSize = true;
            this.VentasLblAlcance.Location = new System.Drawing.Point(24, 7);
            this.VentasLblAlcance.Name = "VentasLblAlcance";
            this.VentasLblAlcance.Size = new System.Drawing.Size(179, 23);
            this.VentasLblAlcance.TabIndex = 1;
            this.VentasLblAlcance.Text = "Aplicar porcentaje por";
            //
            // VentasLblPorcentajeNuevo
            //
            this.VentasLblPorcentajeNuevo.AutoSize = true;
            this.VentasLblPorcentajeNuevo.Location = new System.Drawing.Point(220, 7);
            this.VentasLblPorcentajeNuevo.Name = "VentasLblPorcentajeNuevo";
            this.VentasLblPorcentajeNuevo.Size = new System.Drawing.Size(142, 23);
            this.VentasLblPorcentajeNuevo.TabIndex = 2;
            this.VentasLblPorcentajeNuevo.Text = "Porcentaje nuevo";
            this.VentasLblObligatorioAlcance.AutoSize = true;
            this.VentasLblObligatorioAlcance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioAlcance.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioAlcance.Location = new System.Drawing.Point(201, 7);
            this.VentasLblObligatorioAlcance.Name = "VentasLblObligatorioAlcance";
            this.VentasLblObligatorioAlcance.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioAlcance.TabIndex = 11;
            this.VentasLblObligatorioAlcance.Text = "*";
            this.VentasLblObligatorioAlcance.TabStop = false;
            this.VentasLblObligatorioPorcentaje.AutoSize = true;
            this.VentasLblObligatorioPorcentaje.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioPorcentaje.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioPorcentaje.Location = new System.Drawing.Point(363, 7);
            this.VentasLblObligatorioPorcentaje.Name = "VentasLblObligatorioPorcentaje";
            this.VentasLblObligatorioPorcentaje.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioPorcentaje.TabIndex = 12;
            this.VentasLblObligatorioPorcentaje.Text = "*";
            this.VentasLblObligatorioPorcentaje.TabStop = false;
            //
            // VentasLblMantenimientoEstado
            //
            this.VentasLblMantenimientoEstado.AutoSize = true;
            this.VentasLblMantenimientoEstado.Location = new System.Drawing.Point(24, 70);
            this.VentasLblMantenimientoEstado.Name = "VentasLblMantenimientoEstado";
            this.VentasLblMantenimientoEstado.Size = new System.Drawing.Size(1210, 28);
            this.VentasLblMantenimientoEstado.TabIndex = 3;
            this.VentasLblMantenimientoEstado.Text = "La tabla muestra las 4 tasas por producto. Seleccione producto(s), elija alcance y nuevo %; Marca, Línea o Categoría actualiza la clasificación compartida.";
            //
            // VentasBtnActualizar
            //
            this.VentasBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnActualizar.FlatAppearance.BorderSize = 0;
            this.VentasBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnActualizar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnActualizar.Location = new System.Drawing.Point(478, 23);
            this.VentasBtnActualizar.Name = "VentasBtnActualizar";
            this.VentasBtnActualizar.Size = new System.Drawing.Size(190, 30);
            this.VentasBtnActualizar.TabIndex = 4;
            this.VentasBtnActualizar.Text = "Actualizar porcentaje";
            this.VentasBtnActualizar.UseVisualStyleBackColor = false;
            this.VentasBtnActualizar.Click += new System.EventHandler(this.VentasMetActualizarClick);
            //
            // VentasBtnBuscar
            //
            this.VentasBtnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnBuscar.FlatAppearance.BorderSize = 0;
            this.VentasBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnBuscar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnBuscar.Location = new System.Drawing.Point(348, 23);
            this.VentasBtnBuscar.Name = "VentasBtnBuscar";
            this.VentasBtnBuscar.Size = new System.Drawing.Size(110, 30);
            this.VentasBtnBuscar.TabIndex = 5;
            this.VentasBtnBuscar.Text = "Buscar";
            this.VentasBtnBuscar.UseVisualStyleBackColor = false;
            this.VentasBtnBuscar.Click += new System.EventHandler(this.VentasMetBuscarClick);
            //
            // VentasTxtPorcentaje
            //
            this.VentasTxtPorcentaje.Location = new System.Drawing.Point(220, 25);
            this.VentasTxtPorcentaje.Name = "VentasTxtPorcentaje";
            this.VentasTxtPorcentaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            this.VentasTxtPorcentaje.TabIndex = 6;
            //
            // VentasCboAlcance
            //
            this.VentasCboAlcance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboAlcance.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.VentasCboAlcance.Location = new System.Drawing.Point(24, 25);
            this.VentasCboAlcance.Name = "VentasCboAlcance";
            this.VentasCboAlcance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboAlcance.SelectedIndex = 0;
            this.VentasCboAlcance.Size = new System.Drawing.Size(160, 31);
            this.VentasCboAlcance.TabIndex = 7;
            //
            // VentasPnlEncabezado
            //
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1280, 82);
            this.VentasPnlEncabezado.TabIndex = 1;
            //
            // VentasLblTitulo
            //
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasLblTitulo.Location = new System.Drawing.Point(28, 13);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(412, 46);
            this.VentasLblTitulo.TabIndex = 0;
            this.VentasLblTitulo.Text = "Comisión de vendedores";
            //
            // VentasLblSubtitulo
            //
            this.VentasLblSubtitulo.AutoSize = true;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(32, 55);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(431, 23);
            this.VentasLblSubtitulo.TabIndex = 1;
            this.VentasLblSubtitulo.Text = "Cálculo de comisiones y mantenimiento de porcentajes";
            //
            // FrmComisionVendedores
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.VentasTbcPestanas);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.Icon = ((System.Drawing.Icon)(RecursosFormulario.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FrmComisionVendedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7008 – ComisionVendedores";
            this.VentasTbcPestanas.ResumeLayout(false);
            this.VentasTbpCalculo.ResumeLayout(false);
            this.VentasTbpCalculo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvResultados)).EndInit();
            this.VentasTbpAsignarFactura.ResumeLayout(false);
            this.VentasTbpAsignarFactura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvDetalleFactura)).EndInit();
            this.VentasTbpMantenimiento.ResumeLayout(false);
            this.VentasTbpMantenimiento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvPorcentajes)).EndInit();
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
