namespace CapaVista_ComisionVendedores
{
    partial class FrmMdiComisionVendedores
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl VentasTbcPestanas;
        private System.Windows.Forms.TabPage VentasTbpCalculo;
        private System.Windows.Forms.TabPage VentasTbpAsignarFactura;
        private System.Windows.Forms.TabPage VentasTbpMantenimiento;
        private System.Windows.Forms.Label VentasLblFechaInicio;
        private System.Windows.Forms.Label VentasLblFechaFin;
        private System.Windows.Forms.Label VentasLblMetodoCalculo;
        private System.Windows.Forms.Label VentasLblAlcance;
        private System.Windows.Forms.Label VentasLblPorcentajeNuevo;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaInicio;
        private System.Windows.Forms.DateTimePicker VentasDtpFechaFin;
        private System.Windows.Forms.ComboBox VentasCboMetodo;
        private System.Windows.Forms.Button VentasBtnCalcular;
        private System.Windows.Forms.Button VentasBtnGuardar;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmPorcentajeActual;
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
        private System.Windows.Forms.Button VentasBtnActualizarCatalogos;
        private System.Windows.Forms.Button VentasBtnCalcularFactura;
        private System.Windows.Forms.DataGridView VentasDgvDetalleFactura;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmDetalleElemento;
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
        private System.Windows.Forms.Label VentasLblObligatorioMetodo;
        private System.Windows.Forms.Label VentasLblObligatorioFactura;
        private System.Windows.Forms.Label VentasLblObligatorioVendedor;
        private System.Windows.Forms.Label VentasLblObligatorioMetodoFactura;
        private System.Windows.Forms.Label VentasLblObligatorioAlcance;
        private System.Windows.Forms.Label VentasLblObligatorioPorcentaje;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMdiComisionVendedores));
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.VentasLblMetodoCalculo = new System.Windows.Forms.Label();
            this.VentasLblEstado = new System.Windows.Forms.Label();
            this.VentasBtnGuardar = new System.Windows.Forms.Button();
            this.VentasBtnCalcular = new System.Windows.Forms.Button();
            this.VentasCboMetodo = new System.Windows.Forms.ComboBox();
            this.VentasDtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.VentasDtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.VentasTbpAsignarFactura = new System.Windows.Forms.TabPage();
            this.VentasDgvDetalleFactura = new System.Windows.Forms.DataGridView();
            this.VentasClmDetalleProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmDetalleElemento = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.VentasBtnActualizarCatalogos = new System.Windows.Forms.Button();
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
            this.VentasClmPorcentajeActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasBtnAnteriorPorcentajes = new System.Windows.Forms.Button();
            this.VentasBtnSiguientePorcentajes = new System.Windows.Forms.Button();
            this.VentasLblPaginaPorcentajes = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaInicio = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaFin = new System.Windows.Forms.Label();
            this.VentasLblObligatorioMetodo = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFactura = new System.Windows.Forms.Label();
            this.VentasLblObligatorioVendedor = new System.Windows.Forms.Label();
            this.VentasLblObligatorioMetodoFactura = new System.Windows.Forms.Label();
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
            // ComisionVendedoresTbcPestanas
            // 
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpCalculo);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpAsignarFactura);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpMantenimiento);
            this.VentasTbcPestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasTbcPestanas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasTbcPestanas.Location = new System.Drawing.Point(0, 82);
            this.VentasTbcPestanas.Name = "VentasTbcPestanas";
            this.VentasTbcPestanas.SelectedIndex = 0;
            this.VentasTbcPestanas.Size = new System.Drawing.Size(1000, 478);
            this.VentasTbcPestanas.TabIndex = 0;
            // 
            // ComisionVendedoresTabCalculo
            // 
            this.VentasTbpCalculo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpCalculo.Controls.Add(this.VentasDgvResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnAnteriorResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnSiguienteResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblPaginaResultados);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblFechaInicio);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblMetodoCalculo);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblEstado);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblObligatorioFechaInicio);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblObligatorioFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblObligatorioMetodo);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnGuardar);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnCalcular);
            this.VentasTbpCalculo.Controls.Add(this.VentasCboMetodo);
            this.VentasTbpCalculo.Controls.Add(this.VentasDtpFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasDtpFechaInicio);
            this.VentasTbpCalculo.Location = new System.Drawing.Point(4, 32);
            this.VentasTbpCalculo.Name = "VentasTbpCalculo";
            this.VentasTbpCalculo.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpCalculo.Size = new System.Drawing.Size(992, 442);
            this.VentasTbpCalculo.TabIndex = 0;
            this.VentasTbpCalculo.Text = "Calcular comisión";
            // 
            // ComisionVendedoresDgvResultados
            // 
            this.VentasDgvResultados.AllowUserToAddRows = false;
            DataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            DataGridViewCellStyle25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvResultados.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle25;
            this.VentasDgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle26.Font = new System.Drawing.Font("Segoe UI", 10F);
            DataGridViewCellStyle26.ForeColor = System.Drawing.Color.White;
            DataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvResultados.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle26;
            this.VentasDgvResultados.ColumnHeadersHeight = 29;
            this.VentasDgvResultados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmVendedor,
            this.VentasClmElemento,
            this.VentasClmPorcentaje,
            this.VentasClmBase,
            this.VentasClmMonto});
            DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle30.BackColor = System.Drawing.Color.White;
            DataGridViewCellStyle30.Font = new System.Drawing.Font("Segoe UI", 10F);
            DataGridViewCellStyle30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            DataGridViewCellStyle30.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvResultados.DefaultCellStyle = DataGridViewCellStyle30;
            this.VentasDgvResultados.EnableHeadersVisualStyles = false;
            this.VentasDgvResultados.Location = new System.Drawing.Point(24, 104);
            this.VentasDgvResultados.Name = "VentasDgvResultados";
            this.VentasDgvResultados.ReadOnly = true;
            this.VentasDgvResultados.RowHeadersVisible = false;
            this.VentasDgvResultados.RowHeadersWidth = 51;
            this.VentasDgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvResultados.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvResultados.Size = new System.Drawing.Size(930, 295);
            this.VentasDgvResultados.TabIndex = 0;
            this.VentasDgvResultados.MultiSelect = false;
            this.VentasDgvResultados.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmVendedor
            // 
            this.VentasClmVendedor.DataPropertyName = "NombreVendedor";
            this.VentasClmVendedor.HeaderText = "Vendedor";
            this.VentasClmVendedor.MinimumWidth = 6;
            this.VentasClmVendedor.Name = "VentasClmVendedor";
            this.VentasClmVendedor.ReadOnly = true;
            // 
            // ComisionVendedoresClmElemento
            // 
            this.VentasClmElemento.DataPropertyName = "ElementoComision";
            this.VentasClmElemento.HeaderText = "Elemento";
            this.VentasClmElemento.MinimumWidth = 6;
            this.VentasClmElemento.Name = "VentasClmElemento";
            this.VentasClmElemento.ReadOnly = true;
            // 
            // ComisionVendedoresClmPorcentaje
            // 
            this.VentasClmPorcentaje.DataPropertyName = "Porcentaje";
            DataGridViewCellStyle27.Format = "0.00\\ %";
            DataGridViewCellStyle27.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmPorcentaje.DefaultCellStyle = DataGridViewCellStyle27;
            this.VentasClmPorcentaje.HeaderText = "% comisión";
            this.VentasClmPorcentaje.MinimumWidth = 6;
            this.VentasClmPorcentaje.Name = "VentasClmPorcentaje";
            this.VentasClmPorcentaje.ReadOnly = true;
            // 
            // ComisionVendedoresClmBase
            // 
            this.VentasClmBase.DataPropertyName = "BaseVentas";
            DataGridViewCellStyle28.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle28.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmBase.DefaultCellStyle = DataGridViewCellStyle28;
            this.VentasClmBase.HeaderText = "Base ventas";
            this.VentasClmBase.MinimumWidth = 6;
            this.VentasClmBase.Name = "VentasClmBase";
            this.VentasClmBase.ReadOnly = true;
            // 
            // ComisionVendedoresClmMonto
            // 
            this.VentasClmMonto.DataPropertyName = "MontoComision";
            DataGridViewCellStyle29.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle29.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmMonto.DefaultCellStyle = DataGridViewCellStyle29;
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
            this.VentasBtnAnteriorResultados.Location = new System.Drawing.Point(24, 410);
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
            this.VentasBtnSiguienteResultados.Location = new System.Drawing.Point(854, 410);
            this.VentasBtnSiguienteResultados.Name = "VentasBtnSiguienteResultados";
            this.VentasBtnSiguienteResultados.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguienteResultados.TabIndex = 11;
            this.VentasBtnSiguienteResultados.Text = "Siguiente";
            this.VentasBtnSiguienteResultados.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteResultados.Click += new System.EventHandler(this.VentasMetCambiarPaginaResultados);
            this.VentasLblPaginaResultados.AutoSize = true;
            this.VentasLblPaginaResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaResultados.Location = new System.Drawing.Point(360, 414);
            this.VentasLblPaginaResultados.Name = "VentasLblPaginaResultados";
            this.VentasLblPaginaResultados.Size = new System.Drawing.Size(190, 20);
            this.VentasLblPaginaResultados.TabIndex = 12;
            this.VentasLblPaginaResultados.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblFechaInicio
            // 
            this.VentasLblFechaInicio.AutoSize = true;
            this.VentasLblFechaInicio.Location = new System.Drawing.Point(24, 8);
            this.VentasLblFechaInicio.Name = "VentasLblFechaInicio";
            this.VentasLblFechaInicio.Size = new System.Drawing.Size(102, 23);
            this.VentasLblFechaInicio.TabIndex = 1;
            this.VentasLblFechaInicio.Text = "Fecha inicial";
            // 
            // ComisionVendedoresLblFechaFin
            // 
            this.VentasLblFechaFin.AutoSize = true;
            this.VentasLblFechaFin.Location = new System.Drawing.Point(200, 8);
            this.VentasLblFechaFin.Name = "VentasLblFechaFin";
            this.VentasLblFechaFin.Size = new System.Drawing.Size(91, 23);
            this.VentasLblFechaFin.TabIndex = 2;
            this.VentasLblFechaFin.Text = "Fecha final";
            // 
            // ComisionVendedoresLblMetodoCalculo
            // 
            this.VentasLblMetodoCalculo.AutoSize = true;
            this.VentasLblMetodoCalculo.Location = new System.Drawing.Point(376, 8);
            this.VentasLblMetodoCalculo.Name = "VentasLblMetodoCalculo";
            this.VentasLblMetodoCalculo.Size = new System.Drawing.Size(152, 23);
            this.VentasLblMetodoCalculo.TabIndex = 3;
            this.VentasLblMetodoCalculo.Text = "Método de cálculo";
            // 
            // ComisionVendedoresLblEstado
            // 
            this.VentasLblEstado.AutoSize = true;
            this.VentasLblEstado.Location = new System.Drawing.Point(24, 70);
            this.VentasLblEstado.Name = "VentasLblEstado";
            this.VentasLblEstado.Size = new System.Drawing.Size(257, 23);
            this.VentasLblEstado.TabIndex = 4;
            this.VentasLblEstado.Text = "Periodo · Método · Base VENTAS";
            // 
            // ComisionVendedoresBtnGuardar
            // 
            this.VentasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.VentasBtnGuardar.Enabled = false;
            this.VentasBtnGuardar.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.VentasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnGuardar.ForeColor = System.Drawing.SystemColors.GrayText;
            this.VentasBtnGuardar.Location = new System.Drawing.Point(718, 24);
            this.VentasBtnGuardar.Name = "VentasBtnGuardar";
            this.VentasBtnGuardar.Size = new System.Drawing.Size(160, 30);
            this.VentasBtnGuardar.TabIndex = 5;
            this.VentasBtnGuardar.Text = "Guardar resultado";
            this.VentasBtnGuardar.UseVisualStyleBackColor = false;
            this.VentasBtnGuardar.Click += new System.EventHandler(this.VentasMetGuardarClick);
            // 
            // ComisionVendedoresBtnCalcular
            // 
            this.VentasBtnCalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcular.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcular.ForeColor = System.Drawing.Color.White;
            this.VentasBtnCalcular.Location = new System.Drawing.Point(572, 24);
            this.VentasBtnCalcular.Name = "VentasBtnCalcular";
            this.VentasBtnCalcular.Size = new System.Drawing.Size(130, 30);
            this.VentasBtnCalcular.TabIndex = 6;
            this.VentasBtnCalcular.Text = "Calcular";
            this.VentasBtnCalcular.UseVisualStyleBackColor = false;
            this.VentasBtnCalcular.Click += new System.EventHandler(this.VentasMetCalcularClick);
            // 
            // ComisionVendedoresCboMetodo
            // 
            this.VentasCboMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboMetodo.Items.AddRange(new object[] {
            "Vendedor",
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.VentasCboMetodo.Location = new System.Drawing.Point(376, 26);
            this.VentasCboMetodo.Name = "VentasCboMetodo";
            this.VentasCboMetodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboMetodo.SelectedIndex = 0;
            this.VentasCboMetodo.Size = new System.Drawing.Size(180, 31);
            this.VentasCboMetodo.TabIndex = 7;
            // 
            // ComisionVendedoresDtpFechaFin
            // 
            this.VentasDtpFechaFin.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaFin.Location = new System.Drawing.Point(200, 26);
            this.VentasDtpFechaFin.Name = "VentasDtpFechaFin";
            this.VentasDtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpFechaFin.Size = new System.Drawing.Size(160, 30);
            this.VentasDtpFechaFin.TabIndex = 8;
            // 
            // ComisionVendedoresDtpFechaInicio
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
            this.VentasLblObligatorioMetodo.AutoSize = true;
            this.VentasLblObligatorioMetodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioMetodo.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioMetodo.Location = new System.Drawing.Point(529, 8);
            this.VentasLblObligatorioMetodo.Name = "VentasLblObligatorioMetodo";
            this.VentasLblObligatorioMetodo.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioMetodo.TabIndex = 15;
            this.VentasLblObligatorioMetodo.Text = "*";
            this.VentasLblObligatorioMetodo.TabStop = false;
            // 
            // ComisionVendedoresTabAsignarFactura
            // 
            this.VentasTbpAsignarFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasDgvDetalleFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnAnteriorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnSiguienteFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblPaginaFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblEstadoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioVendedor);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnCalcularFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasBtnActualizarCatalogos);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasCboFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblFactura);
            this.VentasTbpAsignarFactura.Location = new System.Drawing.Point(4, 32);
            this.VentasTbpAsignarFactura.Name = "VentasTbpAsignarFactura";
            this.VentasTbpAsignarFactura.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpAsignarFactura.Size = new System.Drawing.Size(992, 442);
            this.VentasTbpAsignarFactura.TabIndex = 1;
            this.VentasTbpAsignarFactura.Text = "Asignar vendedor a factura";
            // 
            // ComisionVendedoresDgvDetalleFactura
            // 
            this.VentasDgvDetalleFactura.AllowUserToAddRows = false;
            DataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvDetalleFactura.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle16;
            this.VentasDgvDetalleFactura.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            DataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            DataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvDetalleFactura.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle17;
            this.VentasDgvDetalleFactura.ColumnHeadersHeight = 29;
            this.VentasDgvDetalleFactura.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmDetalleProducto,
            this.VentasClmDetalleElemento,
            this.VentasClmDetalleCantidad,
            this.VentasClmDetalleBase,
            this.VentasClmDetallePorcentaje,
            this.VentasClmDetalleComision,
            this.VentasClmDetalleCobrado,
            this.VentasClmDetalleAbono,
            this.VentasClmDetalleDisponible,
            this.VentasClmDetallePendiente});
            DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle24.BackColor = System.Drawing.Color.White;
            DataGridViewCellStyle24.Font = new System.Drawing.Font("Segoe UI", 10F);
            DataGridViewCellStyle24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            DataGridViewCellStyle24.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvDetalleFactura.DefaultCellStyle = DataGridViewCellStyle24;
            this.VentasDgvDetalleFactura.EnableHeadersVisualStyles = false;
            this.VentasDgvDetalleFactura.Location = new System.Drawing.Point(24, 164);
            this.VentasDgvDetalleFactura.Name = "VentasDgvDetalleFactura";
            this.VentasDgvDetalleFactura.ReadOnly = true;
            this.VentasDgvDetalleFactura.RowHeadersVisible = false;
            this.VentasDgvDetalleFactura.RowHeadersWidth = 51;
            this.VentasDgvDetalleFactura.RowTemplate.Height = 22;
            this.VentasDgvDetalleFactura.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvDetalleFactura.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvDetalleFactura.Size = new System.Drawing.Size(930, 234);
            this.VentasDgvDetalleFactura.TabIndex = 0;
            this.VentasDgvDetalleFactura.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmDetalleProducto
            // 
            this.VentasClmDetalleProducto.DataPropertyName = "NombreProducto";
            this.VentasClmDetalleProducto.HeaderText = "Producto";
            this.VentasClmDetalleProducto.MinimumWidth = 6;
            this.VentasClmDetalleProducto.Name = "VentasClmDetalleProducto";
            this.VentasClmDetalleProducto.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleElemento
            // 
            this.VentasClmDetalleElemento.DataPropertyName = "ElementoComision";
            this.VentasClmDetalleElemento.HeaderText = "Criterio";
            this.VentasClmDetalleElemento.MinimumWidth = 6;
            this.VentasClmDetalleElemento.Name = "VentasClmDetalleElemento";
            this.VentasClmDetalleElemento.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleCantidad
            // 
            this.VentasClmDetalleCantidad.DataPropertyName = "Cantidad";
            DataGridViewCellStyle36.Format = "N2";
            DataGridViewCellStyle36.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleCantidad.DefaultCellStyle = DataGridViewCellStyle36;
            this.VentasClmDetalleCantidad.HeaderText = "Cantidad";
            this.VentasClmDetalleCantidad.MinimumWidth = 6;
            this.VentasClmDetalleCantidad.Name = "VentasClmDetalleCantidad";
            this.VentasClmDetalleCantidad.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleBase
            // 
            this.VentasClmDetalleBase.DataPropertyName = "BaseVenta";
            DataGridViewCellStyle18.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle18.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleBase.DefaultCellStyle = DataGridViewCellStyle18;
            this.VentasClmDetalleBase.HeaderText = "Venta";
            this.VentasClmDetalleBase.MinimumWidth = 6;
            this.VentasClmDetalleBase.Name = "VentasClmDetalleBase";
            this.VentasClmDetalleBase.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetallePorcentaje
            // 
            this.VentasClmDetallePorcentaje.DataPropertyName = "Porcentaje";
            DataGridViewCellStyle35.Format = "0.00\\ %";
            DataGridViewCellStyle35.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetallePorcentaje.DefaultCellStyle = DataGridViewCellStyle35;
            this.VentasClmDetallePorcentaje.HeaderText = "%";
            this.VentasClmDetallePorcentaje.MinimumWidth = 6;
            this.VentasClmDetallePorcentaje.Name = "VentasClmDetallePorcentaje";
            this.VentasClmDetallePorcentaje.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleComision
            // 
            this.VentasClmDetalleComision.DataPropertyName = "ComisionTotal";
            DataGridViewCellStyle19.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle19.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleComision.DefaultCellStyle = DataGridViewCellStyle19;
            this.VentasClmDetalleComision.HeaderText = "Comisión total";
            this.VentasClmDetalleComision.MinimumWidth = 6;
            this.VentasClmDetalleComision.Name = "VentasClmDetalleComision";
            this.VentasClmDetalleComision.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleCobrado
            // 
            this.VentasClmDetalleCobrado.DataPropertyName = "VentaCobrado";
            DataGridViewCellStyle20.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle20.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleCobrado.DefaultCellStyle = DataGridViewCellStyle20;
            this.VentasClmDetalleCobrado.HeaderText = "Venta cobrada";
            this.VentasClmDetalleCobrado.MinimumWidth = 6;
            this.VentasClmDetalleCobrado.Name = "VentasClmDetalleCobrado";
            this.VentasClmDetalleCobrado.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleAbono
            // 
            this.VentasClmDetalleAbono.DataPropertyName = "ComisionUltimoAbono";
            DataGridViewCellStyle21.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle21.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleAbono.DefaultCellStyle = DataGridViewCellStyle21;
            this.VentasClmDetalleAbono.HeaderText = "Comisión último abono";
            this.VentasClmDetalleAbono.MinimumWidth = 6;
            this.VentasClmDetalleAbono.Name = "VentasClmDetalleAbono";
            this.VentasClmDetalleAbono.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleDisponible
            // 
            this.VentasClmDetalleDisponible.DataPropertyName = "ComisionDisponible";
            DataGridViewCellStyle22.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle22.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetalleDisponible.DefaultCellStyle = DataGridViewCellStyle22;
            this.VentasClmDetalleDisponible.HeaderText = "Disponible acumulado";
            this.VentasClmDetalleDisponible.MinimumWidth = 6;
            this.VentasClmDetalleDisponible.Name = "VentasClmDetalleDisponible";
            this.VentasClmDetalleDisponible.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetallePendiente
            // 
            this.VentasClmDetallePendiente.DataPropertyName = "ComisionPendiente";
            DataGridViewCellStyle23.Format = "\'Q\' #,##0.00";
            DataGridViewCellStyle23.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmDetallePendiente.DefaultCellStyle = DataGridViewCellStyle23;
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
            this.VentasBtnAnteriorFactura.Location = new System.Drawing.Point(24, 410);
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
            this.VentasBtnSiguienteFactura.Location = new System.Drawing.Point(854, 410);
            this.VentasBtnSiguienteFactura.Name = "VentasBtnSiguienteFactura";
            this.VentasBtnSiguienteFactura.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguienteFactura.TabIndex = 11;
            this.VentasBtnSiguienteFactura.Text = "Siguiente";
            this.VentasBtnSiguienteFactura.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteFactura.Click += new System.EventHandler(this.VentasMetCambiarPaginaFactura);
            this.VentasLblPaginaFactura.AutoSize = true;
            this.VentasLblPaginaFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaFactura.Location = new System.Drawing.Point(360, 414);
            this.VentasLblPaginaFactura.Name = "VentasLblPaginaFactura";
            this.VentasLblPaginaFactura.Size = new System.Drawing.Size(190, 20);
            this.VentasLblPaginaFactura.TabIndex = 12;
            this.VentasLblPaginaFactura.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblEstadoFactura
            // 
            this.VentasLblEstadoFactura.AutoSize = true;
            this.VentasLblEstadoFactura.Location = new System.Drawing.Point(40, 132);
            this.VentasLblEstadoFactura.Name = "VentasLblEstadoFactura";
            this.VentasLblEstadoFactura.Size = new System.Drawing.Size(568, 23);
            this.VentasLblEstadoFactura.TabIndex = 1;
            this.VentasLblEstadoFactura.Text = "La comisión disponible se prorratea según los pagos recibidos del cliente.";
            // 
            // ComisionVendedoresBtnCalcularFactura
            // 
            this.VentasBtnCalcularFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcularFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcularFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcularFactura.ForeColor = System.Drawing.Color.White;
            this.VentasBtnCalcularFactura.Location = new System.Drawing.Point(190, 85);
            this.VentasBtnCalcularFactura.Name = "VentasBtnCalcularFactura";
            this.VentasBtnCalcularFactura.Size = new System.Drawing.Size(180, 30);
            this.VentasBtnCalcularFactura.TabIndex = 2;
            this.VentasBtnCalcularFactura.Text = "Calcular comisión";
            this.VentasBtnCalcularFactura.UseVisualStyleBackColor = false;
            this.VentasBtnCalcularFactura.Click += new System.EventHandler(this.VentasMetCalcularFacturaClick);
            // 
            // ComisionVendedoresBtnActualizarCatalogos
            // 
            this.VentasBtnActualizarCatalogos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasBtnActualizarCatalogos.FlatAppearance.BorderSize = 0;
            this.VentasBtnActualizarCatalogos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnActualizarCatalogos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnActualizarCatalogos.Location = new System.Drawing.Point(24, 85);
            this.VentasBtnActualizarCatalogos.Name = "VentasBtnActualizarCatalogos";
            this.VentasBtnActualizarCatalogos.Size = new System.Drawing.Size(150, 30);
            this.VentasBtnActualizarCatalogos.TabIndex = 3;
            this.VentasBtnActualizarCatalogos.Text = "Cargar ventas";
            this.VentasBtnActualizarCatalogos.UseVisualStyleBackColor = false;
            this.VentasBtnActualizarCatalogos.Click += new System.EventHandler(this.VentasMetCargarCatalogosClick);
            // 
            // ComisionVendedoresCboMetodoFactura
            // 
            this.VentasCboMetodoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboMetodoFactura.Items.AddRange(new object[] {
            "Vendedor",
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
            // ComisionVendedoresCboVendedorFactura
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
            // ComisionVendedoresCboFactura
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
            // ComisionVendedoresLblMetodoFactura
            // 
            this.VentasLblMetodoFactura.AutoSize = true;
            this.VentasLblMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblMetodoFactura.Location = new System.Drawing.Point(646, 22);
            this.VentasLblMetodoFactura.Name = "VentasLblMetodoFactura";
            this.VentasLblMetodoFactura.Size = new System.Drawing.Size(102, 20);
            this.VentasLblMetodoFactura.TabIndex = 7;
            this.VentasLblMetodoFactura.Text = "Comisión por";
            // 
            // ComisionVendedoresLblVendedorFactura
            // 
            this.VentasLblVendedorFactura.AutoSize = true;
            this.VentasLblVendedorFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblVendedorFactura.Location = new System.Drawing.Point(426, 16);
            this.VentasLblVendedorFactura.Name = "VentasLblVendedorFactura";
            this.VentasLblVendedorFactura.Size = new System.Drawing.Size(143, 20);
            this.VentasLblVendedorFactura.TabIndex = 8;
            this.VentasLblVendedorFactura.Text = "Vendedor asignado";
            // 
            // ComisionVendedoresLblFactura
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
            this.VentasLblObligatorioMetodoFactura.AutoSize = true;
            this.VentasLblObligatorioMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioMetodoFactura.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioMetodoFactura.Location = new System.Drawing.Point(749, 22);
            this.VentasLblObligatorioMetodoFactura.Name = "VentasLblObligatorioMetodoFactura";
            this.VentasLblObligatorioMetodoFactura.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioMetodoFactura.TabIndex = 15;
            this.VentasLblObligatorioMetodoFactura.Text = "*";
            this.VentasLblObligatorioMetodoFactura.TabStop = false;
            // 
            // ComisionVendedoresTabMantenimiento
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
            this.VentasTbpMantenimiento.Size = new System.Drawing.Size(992, 442);
            this.VentasTbpMantenimiento.TabIndex = 2;
            this.VentasTbpMantenimiento.Text = "Mantenimiento de porcentajes";
            // 
            // ComisionVendedoresDgvPorcentajes
            // 
            this.VentasDgvPorcentajes.AllowUserToAddRows = false;
            this.VentasDgvPorcentajes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewCellStyle33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvPorcentajes.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle33;
            DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DataGridViewCellStyle31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle31.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            DataGridViewCellStyle31.ForeColor = System.Drawing.Color.White;
            DataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.VentasDgvPorcentajes.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle31;
            this.VentasDgvPorcentajes.ColumnHeadersHeight = 29;
            this.VentasDgvPorcentajes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmProducto,
            this.VentasClmMarca,
            this.VentasClmLinea,
            this.VentasClmCategoria,
            this.VentasClmPorcentajeActual});
            this.VentasDgvPorcentajes.Location = new System.Drawing.Point(24, 104);
            this.VentasDgvPorcentajes.Name = "VentasDgvPorcentajes";
            DataGridViewCellStyle32.BackColor = System.Drawing.Color.White;
            DataGridViewCellStyle32.Font = new System.Drawing.Font("Segoe UI", 9F);
            DataGridViewCellStyle32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            DataGridViewCellStyle32.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            DataGridViewCellStyle32.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasDgvPorcentajes.DefaultCellStyle = DataGridViewCellStyle32;
            this.VentasDgvPorcentajes.EnableHeadersVisualStyles = false;
            this.VentasDgvPorcentajes.ReadOnly = true;
            this.VentasDgvPorcentajes.RowHeadersVisible = false;
            this.VentasDgvPorcentajes.RowHeadersWidth = 51;
            this.VentasDgvPorcentajes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.VentasDgvPorcentajes.Size = new System.Drawing.Size(930, 295);
            this.VentasDgvPorcentajes.TabIndex = 0;
            this.VentasDgvPorcentajes.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmProducto
            // 
            this.VentasClmProducto.DataPropertyName = "NombreProducto";
            this.VentasClmProducto.HeaderText = "Producto";
            this.VentasClmProducto.MinimumWidth = 6;
            this.VentasClmProducto.Name = "VentasClmProducto";
            this.VentasClmProducto.ReadOnly = true;
            // 
            // ComisionVendedoresClmMarca
            // 
            this.VentasClmMarca.DataPropertyName = "Marca";
            this.VentasClmMarca.HeaderText = "Marca";
            this.VentasClmMarca.MinimumWidth = 6;
            this.VentasClmMarca.Name = "VentasClmMarca";
            this.VentasClmMarca.ReadOnly = true;
            // 
            // ComisionVendedoresClmLinea
            // 
            this.VentasClmLinea.DataPropertyName = "Linea";
            this.VentasClmLinea.HeaderText = "Línea";
            this.VentasClmLinea.MinimumWidth = 6;
            this.VentasClmLinea.Name = "VentasClmLinea";
            this.VentasClmLinea.ReadOnly = true;
            // 
            // ComisionVendedoresClmCategoria
            // 
            this.VentasClmCategoria.DataPropertyName = "Categoria";
            this.VentasClmCategoria.HeaderText = "Categoría";
            this.VentasClmCategoria.MinimumWidth = 6;
            this.VentasClmCategoria.Name = "VentasClmCategoria";
            this.VentasClmCategoria.ReadOnly = true;
            // 
            // ComisionVendedoresClmPorcentajeActual
            // 
            this.VentasClmPorcentajeActual.DataPropertyName = "Porcentaje";
            DataGridViewCellStyle34.Format = "0.00\\ %";
            DataGridViewCellStyle34.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.VentasClmPorcentajeActual.DefaultCellStyle = DataGridViewCellStyle34;
            this.VentasClmPorcentajeActual.HeaderText = "% actual";
            this.VentasClmPorcentajeActual.MinimumWidth = 6;
            this.VentasClmPorcentajeActual.Name = "VentasClmPorcentajeActual";
            this.VentasClmPorcentajeActual.ReadOnly = true;
            //
            // Paginación de mantenimiento
            //
            this.VentasBtnAnteriorPorcentajes.Enabled = false;
            this.VentasBtnAnteriorPorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorPorcentajes.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorPorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.VentasBtnAnteriorPorcentajes.Location = new System.Drawing.Point(24, 410);
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
            this.VentasBtnSiguientePorcentajes.Location = new System.Drawing.Point(854, 410);
            this.VentasBtnSiguientePorcentajes.Name = "VentasBtnSiguientePorcentajes";
            this.VentasBtnSiguientePorcentajes.Size = new System.Drawing.Size(100, 28);
            this.VentasBtnSiguientePorcentajes.TabIndex = 9;
            this.VentasBtnSiguientePorcentajes.Text = "Siguiente";
            this.VentasBtnSiguientePorcentajes.UseVisualStyleBackColor = false;
            this.VentasBtnSiguientePorcentajes.Click += new System.EventHandler(this.VentasMetCambiarPaginaPorcentajes);
            this.VentasLblPaginaPorcentajes.AutoSize = true;
            this.VentasLblPaginaPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaPorcentajes.Location = new System.Drawing.Point(360, 414);
            this.VentasLblPaginaPorcentajes.Name = "VentasLblPaginaPorcentajes";
            this.VentasLblPaginaPorcentajes.Size = new System.Drawing.Size(190, 20);
            this.VentasLblPaginaPorcentajes.TabIndex = 10;
            this.VentasLblPaginaPorcentajes.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblAlcance
            // 
            this.VentasLblAlcance.AutoSize = true;
            this.VentasLblAlcance.Location = new System.Drawing.Point(24, 7);
            this.VentasLblAlcance.Name = "VentasLblAlcance";
            this.VentasLblAlcance.Size = new System.Drawing.Size(179, 23);
            this.VentasLblAlcance.TabIndex = 1;
            this.VentasLblAlcance.Text = "Aplicar porcentaje por";
            // 
            // ComisionVendedoresLblPorcentajeNuevo
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
            // ComisionVendedoresLblMantenimientoEstado
            // 
            this.VentasLblMantenimientoEstado.AutoSize = true;
            this.VentasLblMantenimientoEstado.Location = new System.Drawing.Point(24, 70);
            this.VentasLblMantenimientoEstado.Name = "VentasLblMantenimientoEstado";
            this.VentasLblMantenimientoEstado.Size = new System.Drawing.Size(684, 23);
            this.VentasLblMantenimientoEstado.TabIndex = 3;
            this.VentasLblMantenimientoEstado.Text = "Busque los elementos, seleccione los que desea actualizar e ingrese el nuevo porc" +
    "entaje.";
            // 
            // ComisionVendedoresBtnActualizar
            // 
            this.VentasBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnActualizar.FlatAppearance.BorderSize = 0;
            this.VentasBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnActualizar.ForeColor = System.Drawing.Color.White;
            this.VentasBtnActualizar.Location = new System.Drawing.Point(478, 23);
            this.VentasBtnActualizar.Name = "VentasBtnActualizar";
            this.VentasBtnActualizar.Size = new System.Drawing.Size(190, 30);
            this.VentasBtnActualizar.TabIndex = 4;
            this.VentasBtnActualizar.Text = "Actualizar porcentaje";
            this.VentasBtnActualizar.UseVisualStyleBackColor = false;
            this.VentasBtnActualizar.Click += new System.EventHandler(this.VentasMetActualizarClick);
            // 
            // ComisionVendedoresBtnBuscar
            // 
            this.VentasBtnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnBuscar.FlatAppearance.BorderSize = 0;
            this.VentasBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnBuscar.ForeColor = System.Drawing.Color.White;
            this.VentasBtnBuscar.Location = new System.Drawing.Point(348, 23);
            this.VentasBtnBuscar.Name = "VentasBtnBuscar";
            this.VentasBtnBuscar.Size = new System.Drawing.Size(110, 30);
            this.VentasBtnBuscar.TabIndex = 5;
            this.VentasBtnBuscar.Text = "Buscar";
            this.VentasBtnBuscar.UseVisualStyleBackColor = false;
            this.VentasBtnBuscar.Click += new System.EventHandler(this.VentasMetBuscarClick);
            // 
            // ComisionVendedoresTxtPorcentaje
            // 
            this.VentasTxtPorcentaje.Location = new System.Drawing.Point(220, 25);
            this.VentasTxtPorcentaje.Name = "VentasTxtPorcentaje";
            this.VentasTxtPorcentaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            this.VentasTxtPorcentaje.TabIndex = 6;
            // 
            // ComisionVendedoresCboAlcance
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
            // ComisionVendedoresPnlEncabezado
            // 
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1000, 82);
            this.VentasPnlEncabezado.TabIndex = 1;
            // 
            // ComisionVendedoresLblTitulo
            // 
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(28, 13);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(412, 46);
            this.VentasLblTitulo.TabIndex = 0;
            this.VentasLblTitulo.Text = "Comisión de vendedores";
            // 
            // ComisionVendedoresLblSubtitulo
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
            // FrmMdiComisionVendedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 560);
            this.Controls.Add(this.VentasTbcPestanas);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMdiComisionVendedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "7013 – ComisionVendedores";
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
