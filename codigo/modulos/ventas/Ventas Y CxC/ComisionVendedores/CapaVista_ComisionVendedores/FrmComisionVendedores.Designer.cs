namespace ComisionVendedoresVista
{
    partial class FrmComisionVendedores
    {
        private System.ComponentModel.IContainer _Components = null;
        private System.Windows.Forms.TabControl ComisionVendedoresTbcPestanas;
        private System.Windows.Forms.TabPage ComisionVendedoresTabCalculo;
        private System.Windows.Forms.TabPage ComisionVendedoresTabAsignarFactura;
        private System.Windows.Forms.TabPage ComisionVendedoresTabMantenimiento;
        private System.Windows.Forms.Label ComisionVendedoresLblFechaInicio;
        private System.Windows.Forms.Label ComisionVendedoresLblFechaFin;
        private System.Windows.Forms.Label ComisionVendedoresLblAlcance;
        private System.Windows.Forms.Label ComisionVendedoresLblPorcentajeNuevo;
        private System.Windows.Forms.DateTimePicker ComisionVendedoresDtpFechaInicio;
        private System.Windows.Forms.DateTimePicker ComisionVendedoresDtpFechaFin;
        private System.Windows.Forms.Button ComisionVendedoresBtnCalcular;
        private System.Windows.Forms.Button ComisionVendedoresBtnGuardar;
        private System.Windows.Forms.Button ComisionVendedoresBtnAyuda;
        private System.Windows.Forms.DataGridView ComisionVendedoresDgvResultados;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmVendedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmElemento;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmPorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmMonto;
        private System.Windows.Forms.Button ComisionVendedoresBtnAnteriorResultados;
        private System.Windows.Forms.Button ComisionVendedoresBtnSiguienteResultados;
        private System.Windows.Forms.Label ComisionVendedoresLblPaginaResultados;
        private System.Windows.Forms.Label ComisionVendedoresLblEstado;
        private System.Windows.Forms.ComboBox ComisionVendedoresCboAlcance;
        private System.Windows.Forms.TextBox ComisionVendedoresTxtPorcentaje;
        private System.Windows.Forms.Button ComisionVendedoresBtnBuscar;
        private System.Windows.Forms.Button ComisionVendedoresBtnActualizar;
        private System.Windows.Forms.DataGridView ComisionVendedoresDgvPorcentajes;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmPorcentajeMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmPorcentajeLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmPorcentajeCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmPorcentajeProducto;
        private System.Windows.Forms.Label ComisionVendedoresLblMantenimientoEstado;
        private System.Windows.Forms.Panel ComisionVendedoresPnlEncabezado;
        private System.Windows.Forms.Label ComisionVendedoresLblTitulo;
        private System.Windows.Forms.Label ComisionVendedoresLblSubtitulo;
        private System.Windows.Forms.Label ComisionVendedoresLblFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblVendedorFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblMetodoFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblEstadoFactura;
        private System.Windows.Forms.ComboBox ComisionVendedoresCboFactura;
        private System.Windows.Forms.ComboBox ComisionVendedoresCboVendedorFactura;
        private System.Windows.Forms.ComboBox ComisionVendedoresCboMetodoFactura;
        private System.Windows.Forms.Button ComisionVendedoresBtnCalcularFactura;
        private System.Windows.Forms.Button ComisionVendedoresBtnAsignarVendedorFactura;
        private System.Windows.Forms.Button ComisionVendedoresBtnRegistrarAbonoFactura;
        private System.Windows.Forms.DataGridView ComisionVendedoresDgvDetalleFactura;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleElemento;
        private System.Windows.Forms.DataGridViewComboBoxColumn ComisionVendedoresClmMetodoProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetallePorcentaje;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleComision;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleCobrado;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleAbono;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetalleDisponible;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComisionVendedoresClmDetallePendiente;
        private System.Windows.Forms.Button ComisionVendedoresBtnAnteriorFactura;
        private System.Windows.Forms.Button ComisionVendedoresBtnSiguienteFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblPaginaFactura;
        private System.Windows.Forms.Button ComisionVendedoresBtnAnteriorPorcentajes;
        private System.Windows.Forms.Button ComisionVendedoresBtnSiguientePorcentajes;
        private System.Windows.Forms.Label ComisionVendedoresLblPaginaPorcentajes;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioFechaInicio;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioFechaFin;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioVendedor;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioMetodoFactura;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioAlcance;
        private System.Windows.Forms.Label ComisionVendedoresLblObligatorioPorcentaje;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Components != null) _Components.Dispose();
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
            this.ComisionVendedoresTbcPestanas = new System.Windows.Forms.TabControl();
            this.ComisionVendedoresTabCalculo = new System.Windows.Forms.TabPage();
            this.ComisionVendedoresDgvResultados = new System.Windows.Forms.DataGridView();
            this.ComisionVendedoresClmVendedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmElemento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmPorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmMonto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresBtnAnteriorResultados = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnSiguienteResultados = new System.Windows.Forms.Button();
            this.ComisionVendedoresLblPaginaResultados = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblFechaInicio = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblFechaFin = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblEstado = new System.Windows.Forms.Label();
            this.ComisionVendedoresBtnGuardar = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnAyuda = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnCalcular = new System.Windows.Forms.Button();
            this.ComisionVendedoresDtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.ComisionVendedoresDtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.ComisionVendedoresTabAsignarFactura = new System.Windows.Forms.TabPage();
            this.ComisionVendedoresDgvDetalleFactura = new System.Windows.Forms.DataGridView();
            this.ComisionVendedoresClmDetalleProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleElemento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmMetodoProducto = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ComisionVendedoresClmDetalleCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetallePorcentaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleComision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleCobrado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleAbono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetalleDisponible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmDetallePendiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresBtnAnteriorFactura = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnSiguienteFactura = new System.Windows.Forms.Button();
            this.ComisionVendedoresLblPaginaFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblEstadoFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresBtnCalcularFactura = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnAsignarVendedorFactura = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnRegistrarAbonoFactura = new System.Windows.Forms.Button();
            this.ComisionVendedoresCboMetodoFactura = new System.Windows.Forms.ComboBox();
            this.ComisionVendedoresCboVendedorFactura = new System.Windows.Forms.ComboBox();
            this.ComisionVendedoresCboFactura = new System.Windows.Forms.ComboBox();
            this.ComisionVendedoresLblMetodoFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblVendedorFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresTabMantenimiento = new System.Windows.Forms.TabPage();
            this.ComisionVendedoresDgvPorcentajes = new System.Windows.Forms.DataGridView();
            this.ComisionVendedoresClmProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmPorcentajeMarca = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmPorcentajeLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmPorcentajeCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresClmPorcentajeProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComisionVendedoresBtnAnteriorPorcentajes = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnSiguientePorcentajes = new System.Windows.Forms.Button();
            this.ComisionVendedoresLblPaginaPorcentajes = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioFechaInicio = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioFechaFin = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioVendedor = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioMetodoFactura = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioAlcance = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblObligatorioPorcentaje = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblAlcance = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblPorcentajeNuevo = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblMantenimientoEstado = new System.Windows.Forms.Label();
            this.ComisionVendedoresBtnActualizar = new System.Windows.Forms.Button();
            this.ComisionVendedoresBtnBuscar = new System.Windows.Forms.Button();
            this.ComisionVendedoresTxtPorcentaje = new System.Windows.Forms.TextBox();
            this.ComisionVendedoresCboAlcance = new System.Windows.Forms.ComboBox();
            this.ComisionVendedoresPnlEncabezado = new System.Windows.Forms.Panel();
            this.ComisionVendedoresLblTitulo = new System.Windows.Forms.Label();
            this.ComisionVendedoresLblSubtitulo = new System.Windows.Forms.Label();
            this.ComisionVendedoresTbcPestanas.SuspendLayout();
            this.ComisionVendedoresTabCalculo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvResultados)).BeginInit();
            this.ComisionVendedoresTabAsignarFactura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvDetalleFactura)).BeginInit();
            this.ComisionVendedoresTabMantenimiento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvPorcentajes)).BeginInit();
            this.ComisionVendedoresPnlEncabezado.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComisionVendedoresTbcPestanas
            // 
            this.ComisionVendedoresTbcPestanas.Controls.Add(this.ComisionVendedoresTabCalculo);
            this.ComisionVendedoresTbcPestanas.Controls.Add(this.ComisionVendedoresTabAsignarFactura);
            this.ComisionVendedoresTbcPestanas.Controls.Add(this.ComisionVendedoresTabMantenimiento);
            this.ComisionVendedoresTbcPestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComisionVendedoresTbcPestanas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComisionVendedoresTbcPestanas.Location = new System.Drawing.Point(0, 82);
            this.ComisionVendedoresTbcPestanas.Name = "ComisionVendedoresTbcPestanas";
            this.ComisionVendedoresTbcPestanas.SelectedIndex = 0;
            this.ComisionVendedoresTbcPestanas.Size = new System.Drawing.Size(1000, 478);
            this.ComisionVendedoresTbcPestanas.TabIndex = 0;
            // 
            // ComisionVendedoresTabCalculo
            // 
            this.ComisionVendedoresTabCalculo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresDgvResultados);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresBtnAnteriorResultados);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresBtnSiguienteResultados);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblPaginaResultados);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblFechaInicio);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblFechaFin);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblEstado);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblObligatorioFechaInicio);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresLblObligatorioFechaFin);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresBtnGuardar);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresBtnAyuda);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresBtnCalcular);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresDtpFechaFin);
            this.ComisionVendedoresTabCalculo.Controls.Add(this.ComisionVendedoresDtpFechaInicio);
            this.ComisionVendedoresTabCalculo.Location = new System.Drawing.Point(4, 32);
            this.ComisionVendedoresTabCalculo.Name = "ComisionVendedoresTabCalculo";
            this.ComisionVendedoresTabCalculo.Padding = new System.Windows.Forms.Padding(16);
            this.ComisionVendedoresTabCalculo.Size = new System.Drawing.Size(992, 442);
            this.ComisionVendedoresTabCalculo.TabIndex = 0;
            this.ComisionVendedoresTabCalculo.Text = "Calcular comisión";
            // 
            // ComisionVendedoresDgvResultados
            // 
            this.ComisionVendedoresDgvResultados.AllowUserToAddRows = false;
            EstiloCelda25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            EstiloCelda25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.ComisionVendedoresDgvResultados.AlternatingRowsDefaultCellStyle = EstiloCelda25;
            this.ComisionVendedoresDgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda26.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda26.ForeColor = System.Drawing.Color.White;
            EstiloCelda26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            EstiloCelda26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ComisionVendedoresDgvResultados.ColumnHeadersDefaultCellStyle = EstiloCelda26;
            this.ComisionVendedoresDgvResultados.ColumnHeadersHeight = 29;
            this.ComisionVendedoresDgvResultados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComisionVendedoresClmVendedor,
            this.ComisionVendedoresClmElemento,
            this.ComisionVendedoresClmPorcentaje,
            this.ComisionVendedoresClmBase,
            this.ComisionVendedoresClmMonto});
            EstiloCelda30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda30.BackColor = System.Drawing.Color.White;
            EstiloCelda30.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda30.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda30.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda30.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda30.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ComisionVendedoresDgvResultados.DefaultCellStyle = EstiloCelda30;
            this.ComisionVendedoresDgvResultados.EnableHeadersVisualStyles = false;
            this.ComisionVendedoresDgvResultados.Location = new System.Drawing.Point(24, 104);
            this.ComisionVendedoresDgvResultados.Name = "ComisionVendedoresDgvResultados";
            this.ComisionVendedoresDgvResultados.ReadOnly = true;
            this.ComisionVendedoresDgvResultados.RowHeadersVisible = false;
            this.ComisionVendedoresDgvResultados.RowHeadersWidth = 51;
            this.ComisionVendedoresDgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ComisionVendedoresDgvResultados.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresDgvResultados.Size = new System.Drawing.Size(930, 295);
            this.ComisionVendedoresDgvResultados.TabIndex = 0;
            this.ComisionVendedoresDgvResultados.MultiSelect = false;
            this.ComisionVendedoresDgvResultados.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmVendedor
            // 
            this.ComisionVendedoresClmVendedor.DataPropertyName = "NombreVendedor";
            this.ComisionVendedoresClmVendedor.HeaderText = "Vendedor";
            this.ComisionVendedoresClmVendedor.MinimumWidth = 6;
            this.ComisionVendedoresClmVendedor.Name = "ComisionVendedoresClmVendedor";
            this.ComisionVendedoresClmVendedor.ReadOnly = true;
            // 
            // ComisionVendedoresClmElemento
            // 
            this.ComisionVendedoresClmElemento.DataPropertyName = "ElementoComision";
            this.ComisionVendedoresClmElemento.HeaderText = "Factura / producto";
            this.ComisionVendedoresClmElemento.MinimumWidth = 6;
            this.ComisionVendedoresClmElemento.Name = "ComisionVendedoresClmElemento";
            this.ComisionVendedoresClmElemento.ReadOnly = true;
            // 
            // ComisionVendedoresClmPorcentaje
            // 
            this.ComisionVendedoresClmPorcentaje.DataPropertyName = "EtiquetaMetodoPorcentaje";
            EstiloCelda27.Format = "0.00\\ \\%";
            EstiloCelda27.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmPorcentaje.DefaultCellStyle = EstiloCelda27;
            this.ComisionVendedoresClmPorcentaje.HeaderText = "Método / %";
            this.ComisionVendedoresClmPorcentaje.MinimumWidth = 6;
            this.ComisionVendedoresClmPorcentaje.Name = "ComisionVendedoresClmPorcentaje";
            this.ComisionVendedoresClmPorcentaje.ReadOnly = true;
            // 
            // ComisionVendedoresClmBase
            // 
            this.ComisionVendedoresClmBase.DataPropertyName = "BaseVentas";
            EstiloCelda28.Format = "#,##0.00";
            EstiloCelda28.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmBase.DefaultCellStyle = EstiloCelda28;
            this.ComisionVendedoresClmBase.HeaderText = "Base cobrada";
            this.ComisionVendedoresClmBase.MinimumWidth = 6;
            this.ComisionVendedoresClmBase.Name = "ComisionVendedoresClmBase";
            this.ComisionVendedoresClmBase.ReadOnly = true;
            // 
            // ComisionVendedoresClmMonto
            // 
            this.ComisionVendedoresClmMonto.DataPropertyName = "MontoComision";
            EstiloCelda29.Format = "#,##0.00";
            EstiloCelda29.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmMonto.DefaultCellStyle = EstiloCelda29;
            this.ComisionVendedoresClmMonto.HeaderText = "Comisión";
            this.ComisionVendedoresClmMonto.MinimumWidth = 6;
            this.ComisionVendedoresClmMonto.Name = "ComisionVendedoresClmMonto";
            this.ComisionVendedoresClmMonto.ReadOnly = true;
            //
            // Paginación de resultados
            //
            this.ComisionVendedoresBtnAnteriorResultados.Enabled = false;
            this.ComisionVendedoresBtnAnteriorResultados.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnAnteriorResultados.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnAnteriorResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnAnteriorResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.ComisionVendedoresBtnAnteriorResultados.Location = new System.Drawing.Point(24, 410);
            this.ComisionVendedoresBtnAnteriorResultados.Name = "ComisionVendedoresBtnAnteriorResultados";
            this.ComisionVendedoresBtnAnteriorResultados.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnAnteriorResultados.TabIndex = 10;
            this.ComisionVendedoresBtnAnteriorResultados.Text = "Anterior";
            this.ComisionVendedoresBtnAnteriorResultados.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnAnteriorResultados.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaResultados);
            this.ComisionVendedoresBtnSiguienteResultados.Enabled = false;
            this.ComisionVendedoresBtnSiguienteResultados.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnSiguienteResultados.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnSiguienteResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnSiguienteResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresBtnSiguienteResultados.Location = new System.Drawing.Point(854, 410);
            this.ComisionVendedoresBtnSiguienteResultados.Name = "ComisionVendedoresBtnSiguienteResultados";
            this.ComisionVendedoresBtnSiguienteResultados.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnSiguienteResultados.TabIndex = 11;
            this.ComisionVendedoresBtnSiguienteResultados.Text = "Siguiente";
            this.ComisionVendedoresBtnSiguienteResultados.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnSiguienteResultados.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaResultados);
            this.ComisionVendedoresLblPaginaResultados.AutoSize = true;
            this.ComisionVendedoresLblPaginaResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComisionVendedoresLblPaginaResultados.Location = new System.Drawing.Point(360, 414);
            this.ComisionVendedoresLblPaginaResultados.Name = "ComisionVendedoresLblPaginaResultados";
            this.ComisionVendedoresLblPaginaResultados.Size = new System.Drawing.Size(190, 20);
            this.ComisionVendedoresLblPaginaResultados.TabIndex = 12;
            this.ComisionVendedoresLblPaginaResultados.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblFechaInicio
            // 
            this.ComisionVendedoresLblFechaInicio.AutoSize = true;
            this.ComisionVendedoresLblFechaInicio.Location = new System.Drawing.Point(24, 8);
            this.ComisionVendedoresLblFechaInicio.Name = "ComisionVendedoresLblFechaInicio";
            this.ComisionVendedoresLblFechaInicio.Size = new System.Drawing.Size(102, 23);
            this.ComisionVendedoresLblFechaInicio.TabIndex = 1;
            this.ComisionVendedoresLblFechaInicio.Text = "Fecha inicial";
            // 
            // ComisionVendedoresLblFechaFin
            // 
            this.ComisionVendedoresLblFechaFin.AutoSize = true;
            this.ComisionVendedoresLblFechaFin.Location = new System.Drawing.Point(200, 8);
            this.ComisionVendedoresLblFechaFin.Name = "ComisionVendedoresLblFechaFin";
            this.ComisionVendedoresLblFechaFin.Size = new System.Drawing.Size(91, 23);
            this.ComisionVendedoresLblFechaFin.TabIndex = 2;
            this.ComisionVendedoresLblFechaFin.Text = "Fecha final";
            // 
            // ComisionVendedoresLblEstado
            // 
            this.ComisionVendedoresLblEstado.AutoSize = true;
            this.ComisionVendedoresLblEstado.Location = new System.Drawing.Point(24, 70);
            this.ComisionVendedoresLblEstado.Name = "ComisionVendedoresLblEstado";
            this.ComisionVendedoresLblEstado.Size = new System.Drawing.Size(257, 23);
            this.ComisionVendedoresLblEstado.TabIndex = 4;
            this.ComisionVendedoresLblEstado.Text = "Periodo · Método · Base VENTAS";
            // 
            // ComisionVendedoresBtnGuardar
            // 
            this.ComisionVendedoresBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnGuardar.Enabled = false;
            this.ComisionVendedoresBtnGuardar.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnGuardar.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnGuardar.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ComisionVendedoresBtnGuardar.Location = new System.Drawing.Point(718, 24);
            this.ComisionVendedoresBtnGuardar.Name = "ComisionVendedoresBtnGuardar";
            this.ComisionVendedoresBtnGuardar.Size = new System.Drawing.Size(160, 30);
            this.ComisionVendedoresBtnGuardar.TabIndex = 5;
            this.ComisionVendedoresBtnGuardar.Text = "Pagar comisiones";
            this.ComisionVendedoresBtnGuardar.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnGuardar.Click += new System.EventHandler(this.ComisionVendedoresMetGuardarClick);
            this.ComisionVendedoresBtnAyuda.BackColor = System.Drawing.Color.FromArgb(228, 212, 234);
            this.ComisionVendedoresBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnAyuda.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.ComisionVendedoresBtnAyuda.Location = new System.Drawing.Point(880, 24);
            this.ComisionVendedoresBtnAyuda.Name = "ComisionVendedoresBtnAyuda";
            this.ComisionVendedoresBtnAyuda.Size = new System.Drawing.Size(90, 30);
            this.ComisionVendedoresBtnAyuda.TabIndex = 6;
            this.ComisionVendedoresBtnAyuda.Text = "Ayuda";
            this.ComisionVendedoresBtnAyuda.UseVisualStyleBackColor = false;
            // 
            // ComisionVendedoresBtnCalcular
            // 
            this.ComisionVendedoresBtnCalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnCalcular.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnCalcular.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresBtnCalcular.Location = new System.Drawing.Point(572, 24);
            this.ComisionVendedoresBtnCalcular.Name = "ComisionVendedoresBtnCalcular";
            this.ComisionVendedoresBtnCalcular.Size = new System.Drawing.Size(130, 30);
            this.ComisionVendedoresBtnCalcular.TabIndex = 6;
            this.ComisionVendedoresBtnCalcular.Text = "Calcular";
            this.ComisionVendedoresBtnCalcular.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnCalcular.Click += new System.EventHandler(this.ComisionVendedoresMetCalcularClick);
            // 
            // ComisionVendedoresDtpFechaFin
            // 
            this.ComisionVendedoresDtpFechaFin.CustomFormat = "dd/MM/yyyy";
            this.ComisionVendedoresDtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComisionVendedoresDtpFechaFin.Location = new System.Drawing.Point(200, 26);
            this.ComisionVendedoresDtpFechaFin.Name = "ComisionVendedoresDtpFechaFin";
            this.ComisionVendedoresDtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresDtpFechaFin.Size = new System.Drawing.Size(160, 30);
            this.ComisionVendedoresDtpFechaFin.TabIndex = 8;
            // 
            // ComisionVendedoresDtpFechaInicio
            // 
            this.ComisionVendedoresDtpFechaInicio.CustomFormat = "dd/MM/yyyy";
            this.ComisionVendedoresDtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComisionVendedoresDtpFechaInicio.Location = new System.Drawing.Point(24, 26);
            this.ComisionVendedoresDtpFechaInicio.Name = "ComisionVendedoresDtpFechaInicio";
            this.ComisionVendedoresDtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresDtpFechaInicio.Size = new System.Drawing.Size(160, 30);
            this.ComisionVendedoresDtpFechaInicio.TabIndex = 9;
            this.ComisionVendedoresLblObligatorioFechaInicio.AutoSize = true;
            this.ComisionVendedoresLblObligatorioFechaInicio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioFechaInicio.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioFechaInicio.Location = new System.Drawing.Point(120, 8);
            this.ComisionVendedoresLblObligatorioFechaInicio.Name = "ComisionVendedoresLblObligatorioFechaInicio";
            this.ComisionVendedoresLblObligatorioFechaInicio.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioFechaInicio.TabIndex = 13;
            this.ComisionVendedoresLblObligatorioFechaInicio.Text = "*";
            this.ComisionVendedoresLblObligatorioFechaInicio.TabStop = false;
            this.ComisionVendedoresLblObligatorioFechaFin.AutoSize = true;
            this.ComisionVendedoresLblObligatorioFechaFin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioFechaFin.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioFechaFin.Location = new System.Drawing.Point(284, 8);
            this.ComisionVendedoresLblObligatorioFechaFin.Name = "ComisionVendedoresLblObligatorioFechaFin";
            this.ComisionVendedoresLblObligatorioFechaFin.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioFechaFin.TabIndex = 14;
            this.ComisionVendedoresLblObligatorioFechaFin.Text = "*";
            this.ComisionVendedoresLblObligatorioFechaFin.TabStop = false;
            // 
            // ComisionVendedoresTabAsignarFactura
            // 
            this.ComisionVendedoresTabAsignarFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresDgvDetalleFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresBtnAnteriorFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresBtnSiguienteFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblPaginaFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblEstadoFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblObligatorioFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblObligatorioVendedor);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblObligatorioMetodoFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresBtnCalcularFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresBtnAsignarVendedorFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresBtnRegistrarAbonoFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresCboMetodoFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresCboVendedorFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresCboFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblMetodoFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblVendedorFactura);
            this.ComisionVendedoresTabAsignarFactura.Controls.Add(this.ComisionVendedoresLblFactura);
            this.ComisionVendedoresTabAsignarFactura.Location = new System.Drawing.Point(4, 32);
            this.ComisionVendedoresTabAsignarFactura.Name = "ComisionVendedoresTabAsignarFactura";
            this.ComisionVendedoresTabAsignarFactura.Padding = new System.Windows.Forms.Padding(16);
            this.ComisionVendedoresTabAsignarFactura.Size = new System.Drawing.Size(992, 442);
            this.ComisionVendedoresTabAsignarFactura.TabIndex = 1;
            this.ComisionVendedoresTabAsignarFactura.Text = "Asignar vendedor a factura";
            // 
            // ComisionVendedoresDgvDetalleFactura
            // 
            this.ComisionVendedoresDgvDetalleFactura.AllowUserToAddRows = false;
            EstiloCelda16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ComisionVendedoresDgvDetalleFactura.AlternatingRowsDefaultCellStyle = EstiloCelda16;
            this.ComisionVendedoresDgvDetalleFactura.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda17.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            EstiloCelda17.ForeColor = System.Drawing.Color.White;
            EstiloCelda17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            EstiloCelda17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ComisionVendedoresDgvDetalleFactura.ColumnHeadersDefaultCellStyle = EstiloCelda17;
            this.ComisionVendedoresDgvDetalleFactura.ColumnHeadersHeight = 29;
            this.ComisionVendedoresDgvDetalleFactura.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComisionVendedoresClmDetalleProducto,
            this.ComisionVendedoresClmDetalleElemento,
            this.ComisionVendedoresClmMetodoProducto,
            this.ComisionVendedoresClmDetalleCantidad,
            this.ComisionVendedoresClmDetalleBase,
            this.ComisionVendedoresClmDetallePorcentaje,
            this.ComisionVendedoresClmDetalleComision,
            this.ComisionVendedoresClmDetalleCobrado,
            this.ComisionVendedoresClmDetalleAbono,
            this.ComisionVendedoresClmDetalleDisponible,
            this.ComisionVendedoresClmDetallePendiente});
            this.ComisionVendedoresDgvDetalleFactura.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ComisionVendedoresMetFormatearMetodoProducto);
            EstiloCelda24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda24.BackColor = System.Drawing.Color.White;
            EstiloCelda24.Font = new System.Drawing.Font("Segoe UI", 10F);
            EstiloCelda24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda24.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda24.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda24.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ComisionVendedoresDgvDetalleFactura.DefaultCellStyle = EstiloCelda24;
            this.ComisionVendedoresDgvDetalleFactura.EnableHeadersVisualStyles = false;
            this.ComisionVendedoresDgvDetalleFactura.Location = new System.Drawing.Point(24, 170);
            this.ComisionVendedoresDgvDetalleFactura.Name = "ComisionVendedoresDgvDetalleFactura";
            this.ComisionVendedoresDgvDetalleFactura.ReadOnly = false;
            this.ComisionVendedoresDgvDetalleFactura.RowHeadersVisible = false;
            this.ComisionVendedoresDgvDetalleFactura.RowHeadersWidth = 51;
            this.ComisionVendedoresDgvDetalleFactura.RowTemplate.Height = 22;
            this.ComisionVendedoresDgvDetalleFactura.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ComisionVendedoresDgvDetalleFactura.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresDgvDetalleFactura.Size = new System.Drawing.Size(930, 228);
            this.ComisionVendedoresDgvDetalleFactura.TabIndex = 0;
            this.ComisionVendedoresDgvDetalleFactura.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmDetalleProducto
            // 
            this.ComisionVendedoresClmDetalleProducto.DataPropertyName = "NombreProducto";
            this.ComisionVendedoresClmDetalleProducto.HeaderText = "Producto";
            this.ComisionVendedoresClmDetalleProducto.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleProducto.Name = "ComisionVendedoresClmDetalleProducto";
            this.ComisionVendedoresClmDetalleProducto.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleElemento
            // 
            this.ComisionVendedoresClmDetalleElemento.DataPropertyName = "ElementoComision";
            this.ComisionVendedoresClmDetalleElemento.HeaderText = "Criterio";
            this.ComisionVendedoresClmDetalleElemento.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleElemento.Name = "ComisionVendedoresClmDetalleElemento";
            this.ComisionVendedoresClmDetalleElemento.ReadOnly = true;
            this.ComisionVendedoresClmMetodoProducto.DataPropertyName = "MetodoComision";
            this.ComisionVendedoresClmMetodoProducto.HeaderText = "Comisión por";
            this.ComisionVendedoresClmMetodoProducto.MinimumWidth = 95;
            this.ComisionVendedoresClmMetodoProducto.Name = "ComisionVendedoresClmMetodoProducto";
            this.ComisionVendedoresClmMetodoProducto.Items.AddRange(new object[] { "Linea", "Marca", "Categoria", "Producto" });
            this.ComisionVendedoresClmMetodoProducto.ReadOnly = false;
            // 
            // ComisionVendedoresClmDetalleCantidad
            // 
            this.ComisionVendedoresClmDetalleCantidad.DataPropertyName = "Cantidad";
            EstiloCelda36.Format = "N2";
            EstiloCelda36.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleCantidad.DefaultCellStyle = EstiloCelda36;
            this.ComisionVendedoresClmDetalleCantidad.HeaderText = "Cantidad";
            this.ComisionVendedoresClmDetalleCantidad.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleCantidad.Name = "ComisionVendedoresClmDetalleCantidad";
            this.ComisionVendedoresClmDetalleCantidad.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleBase
            // 
            this.ComisionVendedoresClmDetalleBase.DataPropertyName = "BaseVenta";
            EstiloCelda18.Format = "#,##0.00";
            EstiloCelda18.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleBase.DefaultCellStyle = EstiloCelda18;
            this.ComisionVendedoresClmDetalleBase.HeaderText = "Venta";
            this.ComisionVendedoresClmDetalleBase.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleBase.Name = "ComisionVendedoresClmDetalleBase";
            this.ComisionVendedoresClmDetalleBase.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetallePorcentaje
            // 
            this.ComisionVendedoresClmDetallePorcentaje.DataPropertyName = "Porcentaje";
            EstiloCelda35.Format = "0.00\\%";
            EstiloCelda35.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetallePorcentaje.DefaultCellStyle = EstiloCelda35;
            this.ComisionVendedoresClmDetallePorcentaje.HeaderText = "%";
            this.ComisionVendedoresClmDetallePorcentaje.MinimumWidth = 6;
            this.ComisionVendedoresClmDetallePorcentaje.Name = "ComisionVendedoresClmDetallePorcentaje";
            this.ComisionVendedoresClmDetallePorcentaje.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleComision
            // 
            this.ComisionVendedoresClmDetalleComision.DataPropertyName = "ComisionTotal";
            EstiloCelda19.Format = "#,##0.00";
            EstiloCelda19.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleComision.DefaultCellStyle = EstiloCelda19;
            this.ComisionVendedoresClmDetalleComision.HeaderText = "Comisión total";
            this.ComisionVendedoresClmDetalleComision.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleComision.Name = "ComisionVendedoresClmDetalleComision";
            this.ComisionVendedoresClmDetalleComision.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleCobrado
            // 
            this.ComisionVendedoresClmDetalleCobrado.DataPropertyName = "VentaCobrado";
            EstiloCelda20.Format = "#,##0.00";
            EstiloCelda20.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleCobrado.DefaultCellStyle = EstiloCelda20;
            this.ComisionVendedoresClmDetalleCobrado.HeaderText = "Venta cobrada";
            this.ComisionVendedoresClmDetalleCobrado.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleCobrado.Name = "ComisionVendedoresClmDetalleCobrado";
            this.ComisionVendedoresClmDetalleCobrado.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleAbono
            // 
            this.ComisionVendedoresClmDetalleAbono.DataPropertyName = "ComisionUltimoAbono";
            EstiloCelda21.Format = "#,##0.00";
            EstiloCelda21.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleAbono.DefaultCellStyle = EstiloCelda21;
            this.ComisionVendedoresClmDetalleAbono.HeaderText = "Comisión último abono";
            this.ComisionVendedoresClmDetalleAbono.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleAbono.Name = "ComisionVendedoresClmDetalleAbono";
            this.ComisionVendedoresClmDetalleAbono.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetalleDisponible
            // 
            this.ComisionVendedoresClmDetalleDisponible.DataPropertyName = "ComisionDisponible";
            EstiloCelda22.Format = "#,##0.00";
            EstiloCelda22.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetalleDisponible.DefaultCellStyle = EstiloCelda22;
            this.ComisionVendedoresClmDetalleDisponible.HeaderText = "Disponible acumulado";
            this.ComisionVendedoresClmDetalleDisponible.MinimumWidth = 6;
            this.ComisionVendedoresClmDetalleDisponible.Name = "ComisionVendedoresClmDetalleDisponible";
            this.ComisionVendedoresClmDetalleDisponible.ReadOnly = true;
            // 
            // ComisionVendedoresClmDetallePendiente
            // 
            this.ComisionVendedoresClmDetallePendiente.DataPropertyName = "ComisionPendiente";
            EstiloCelda23.Format = "#,##0.00";
            EstiloCelda23.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmDetallePendiente.DefaultCellStyle = EstiloCelda23;
            this.ComisionVendedoresClmDetallePendiente.HeaderText = "Pendiente";
            this.ComisionVendedoresClmDetallePendiente.MinimumWidth = 6;
            this.ComisionVendedoresClmDetallePendiente.Name = "ComisionVendedoresClmDetallePendiente";
            this.ComisionVendedoresClmDetallePendiente.ReadOnly = true;
            //
            // Paginación de detalle de factura
            //
            this.ComisionVendedoresBtnAnteriorFactura.Enabled = false;
            this.ComisionVendedoresBtnAnteriorFactura.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnAnteriorFactura.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnAnteriorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnAnteriorFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.ComisionVendedoresBtnAnteriorFactura.Location = new System.Drawing.Point(24, 410);
            this.ComisionVendedoresBtnAnteriorFactura.Name = "ComisionVendedoresBtnAnteriorFactura";
            this.ComisionVendedoresBtnAnteriorFactura.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnAnteriorFactura.TabIndex = 10;
            this.ComisionVendedoresBtnAnteriorFactura.Text = "Anterior";
            this.ComisionVendedoresBtnAnteriorFactura.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnAnteriorFactura.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaFactura);
            this.ComisionVendedoresBtnSiguienteFactura.Enabled = false;
            this.ComisionVendedoresBtnSiguienteFactura.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnSiguienteFactura.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnSiguienteFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnSiguienteFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresBtnSiguienteFactura.Location = new System.Drawing.Point(854, 410);
            this.ComisionVendedoresBtnSiguienteFactura.Name = "ComisionVendedoresBtnSiguienteFactura";
            this.ComisionVendedoresBtnSiguienteFactura.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnSiguienteFactura.TabIndex = 11;
            this.ComisionVendedoresBtnSiguienteFactura.Text = "Siguiente";
            this.ComisionVendedoresBtnSiguienteFactura.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnSiguienteFactura.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaFactura);
            this.ComisionVendedoresLblPaginaFactura.AutoSize = true;
            this.ComisionVendedoresLblPaginaFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComisionVendedoresLblPaginaFactura.Location = new System.Drawing.Point(360, 414);
            this.ComisionVendedoresLblPaginaFactura.Name = "ComisionVendedoresLblPaginaFactura";
            this.ComisionVendedoresLblPaginaFactura.Size = new System.Drawing.Size(190, 20);
            this.ComisionVendedoresLblPaginaFactura.TabIndex = 12;
            this.ComisionVendedoresLblPaginaFactura.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblEstadoFactura
            // 
            this.ComisionVendedoresLblEstadoFactura.AutoSize = false;
            this.ComisionVendedoresLblEstadoFactura.Location = new System.Drawing.Point(24, 126);
            this.ComisionVendedoresLblEstadoFactura.Name = "ComisionVendedoresLblEstadoFactura";
            this.ComisionVendedoresLblEstadoFactura.Size = new System.Drawing.Size(930, 40);
            this.ComisionVendedoresLblEstadoFactura.TabIndex = 1;
            this.ComisionVendedoresLblEstadoFactura.Text = "Elija el método para cada producto; use la paginación para revisar toda la factura.";
            // 
            // ComisionVendedoresBtnCalcularFactura
            // 
            this.ComisionVendedoresBtnCalcularFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnCalcularFactura.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnCalcularFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnCalcularFactura.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresBtnCalcularFactura.Location = new System.Drawing.Point(24, 85);
            this.ComisionVendedoresBtnCalcularFactura.Name = "ComisionVendedoresBtnCalcularFactura";
            this.ComisionVendedoresBtnCalcularFactura.Size = new System.Drawing.Size(180, 30);
            this.ComisionVendedoresBtnCalcularFactura.TabIndex = 2;
            this.ComisionVendedoresBtnCalcularFactura.Text = "Calcular comisión";
            this.ComisionVendedoresBtnCalcularFactura.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnCalcularFactura.Click += new System.EventHandler(this.ComisionVendedoresMetCalcularFacturaClick);
            // 
            // ComisionVendedoresBtnAsignarVendedorFactura
            // 
            this.ComisionVendedoresBtnAsignarVendedorFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnAsignarVendedorFactura.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnAsignarVendedorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnAsignarVendedorFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnAsignarVendedorFactura.Location = new System.Drawing.Point(214, 85);
            this.ComisionVendedoresBtnAsignarVendedorFactura.Name = "ComisionVendedoresBtnAsignarVendedorFactura";
            this.ComisionVendedoresBtnAsignarVendedorFactura.Size = new System.Drawing.Size(180, 30);
            this.ComisionVendedoresBtnAsignarVendedorFactura.TabIndex = 14;
            this.ComisionVendedoresBtnAsignarVendedorFactura.Text = "Asignar vendedor";
            this.ComisionVendedoresBtnAsignarVendedorFactura.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnAsignarVendedorFactura.Click += new System.EventHandler(this.ComisionVendedoresMetAsignarVendedorFacturaClick);
            // 
            // ComisionVendedoresBtnRegistrarAbonoFactura
            // 
            this.ComisionVendedoresBtnRegistrarAbonoFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.ComisionVendedoresBtnRegistrarAbonoFactura.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnRegistrarAbonoFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnRegistrarAbonoFactura.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresBtnRegistrarAbonoFactura.Location = new System.Drawing.Point(404, 85);
            this.ComisionVendedoresBtnRegistrarAbonoFactura.Name = "ComisionVendedoresBtnRegistrarAbonoFactura";
            this.ComisionVendedoresBtnRegistrarAbonoFactura.Size = new System.Drawing.Size(190, 30);
            this.ComisionVendedoresBtnRegistrarAbonoFactura.TabIndex = 15;
            this.ComisionVendedoresBtnRegistrarAbonoFactura.Text = "Registrar abono cliente";
            this.ComisionVendedoresBtnRegistrarAbonoFactura.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnRegistrarAbonoFactura.Click += new System.EventHandler(this.ComisionVendedoresMetRegistrarAbonoFacturaClick);
            //
            // 
            // ComisionVendedoresCboMetodoFactura
            // 
            this.ComisionVendedoresCboMetodoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComisionVendedoresCboMetodoFactura.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.ComisionVendedoresCboMetodoFactura.Location = new System.Drawing.Point(650, 45);
            this.ComisionVendedoresCboMetodoFactura.Name = "ComisionVendedoresCboMetodoFactura";
            this.ComisionVendedoresCboMetodoFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresCboMetodoFactura.SelectedIndex = 0;
            this.ComisionVendedoresCboMetodoFactura.Size = new System.Drawing.Size(160, 31);
            this.ComisionVendedoresCboMetodoFactura.TabIndex = 4;
            // 
            // ComisionVendedoresCboVendedorFactura
            // 
            this.ComisionVendedoresCboVendedorFactura.DisplayMember = "NombreVendedor";
            this.ComisionVendedoresCboVendedorFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComisionVendedoresCboVendedorFactura.Location = new System.Drawing.Point(430, 45);
            this.ComisionVendedoresCboVendedorFactura.Name = "ComisionVendedoresCboVendedorFactura";
            this.ComisionVendedoresCboVendedorFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresCboVendedorFactura.Size = new System.Drawing.Size(200, 31);
            this.ComisionVendedoresCboVendedorFactura.TabIndex = 5;
            this.ComisionVendedoresCboVendedorFactura.ValueMember = "IdVendedor";
            // 
            // ComisionVendedoresCboFactura
            // 
            this.ComisionVendedoresCboFactura.DisplayMember = "Etiqueta";
            this.ComisionVendedoresCboFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComisionVendedoresCboFactura.Location = new System.Drawing.Point(24, 45);
            this.ComisionVendedoresCboFactura.Name = "ComisionVendedoresCboFactura";
            this.ComisionVendedoresCboFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresCboFactura.Size = new System.Drawing.Size(390, 31);
            this.ComisionVendedoresCboFactura.TabIndex = 6;
            this.ComisionVendedoresCboFactura.ValueMember = "IdFactura";
            // 
            // ComisionVendedoresLblMetodoFactura
            // 
            this.ComisionVendedoresLblMetodoFactura.AutoSize = true;
            this.ComisionVendedoresLblMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblMetodoFactura.Location = new System.Drawing.Point(646, 22);
            this.ComisionVendedoresLblMetodoFactura.Name = "ComisionVendedoresLblMetodoFactura";
            this.ComisionVendedoresLblMetodoFactura.Size = new System.Drawing.Size(102, 20);
            this.ComisionVendedoresLblMetodoFactura.TabIndex = 7;
            this.ComisionVendedoresLblMetodoFactura.Text = "Método predeterminado";
            // 
            // ComisionVendedoresLblVendedorFactura
            // 
            this.ComisionVendedoresLblVendedorFactura.AutoSize = true;
            this.ComisionVendedoresLblVendedorFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblVendedorFactura.Location = new System.Drawing.Point(426, 16);
            this.ComisionVendedoresLblVendedorFactura.Name = "ComisionVendedoresLblVendedorFactura";
            this.ComisionVendedoresLblVendedorFactura.Size = new System.Drawing.Size(143, 20);
            this.ComisionVendedoresLblVendedorFactura.TabIndex = 8;
            this.ComisionVendedoresLblVendedorFactura.Text = "Vendedor asignado";
            // 
            // ComisionVendedoresLblFactura
            // 
            this.ComisionVendedoresLblFactura.AutoSize = true;
            this.ComisionVendedoresLblFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblFactura.Location = new System.Drawing.Point(28, 22);
            this.ComisionVendedoresLblFactura.Name = "ComisionVendedoresLblFactura";
            this.ComisionVendedoresLblFactura.Size = new System.Drawing.Size(114, 20);
            this.ComisionVendedoresLblFactura.TabIndex = 9;
            this.ComisionVendedoresLblFactura.Text = "Venta / factura";
            this.ComisionVendedoresLblObligatorioFactura.AutoSize = true;
            this.ComisionVendedoresLblObligatorioFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioFactura.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioFactura.Location = new System.Drawing.Point(141, 22);
            this.ComisionVendedoresLblObligatorioFactura.Name = "ComisionVendedoresLblObligatorioFactura";
            this.ComisionVendedoresLblObligatorioFactura.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioFactura.TabIndex = 13;
            this.ComisionVendedoresLblObligatorioFactura.Text = "*";
            this.ComisionVendedoresLblObligatorioFactura.TabStop = false;
            this.ComisionVendedoresLblObligatorioVendedor.AutoSize = true;
            this.ComisionVendedoresLblObligatorioVendedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioVendedor.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioVendedor.Location = new System.Drawing.Point(570, 16);
            this.ComisionVendedoresLblObligatorioVendedor.Name = "ComisionVendedoresLblObligatorioVendedor";
            this.ComisionVendedoresLblObligatorioVendedor.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioVendedor.TabIndex = 14;
            this.ComisionVendedoresLblObligatorioVendedor.Text = "*";
            this.ComisionVendedoresLblObligatorioVendedor.TabStop = false;
            this.ComisionVendedoresLblObligatorioMetodoFactura.AutoSize = true;
            this.ComisionVendedoresLblObligatorioMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioMetodoFactura.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioMetodoFactura.Location = new System.Drawing.Point(749, 22);
            this.ComisionVendedoresLblObligatorioMetodoFactura.Name = "ComisionVendedoresLblObligatorioMetodoFactura";
            this.ComisionVendedoresLblObligatorioMetodoFactura.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioMetodoFactura.TabIndex = 15;
            this.ComisionVendedoresLblObligatorioMetodoFactura.Text = "*";
            this.ComisionVendedoresLblObligatorioMetodoFactura.TabStop = false;
            // 
            // ComisionVendedoresTabMantenimiento
            // 
            this.ComisionVendedoresTabMantenimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresDgvPorcentajes);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresBtnAnteriorPorcentajes);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresBtnSiguientePorcentajes);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblPaginaPorcentajes);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblAlcance);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblPorcentajeNuevo);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblMantenimientoEstado);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblObligatorioAlcance);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresLblObligatorioPorcentaje);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresBtnActualizar);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresBtnBuscar);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresTxtPorcentaje);
            this.ComisionVendedoresTabMantenimiento.Controls.Add(this.ComisionVendedoresCboAlcance);
            this.ComisionVendedoresTabMantenimiento.Location = new System.Drawing.Point(4, 32);
            this.ComisionVendedoresTabMantenimiento.Name = "ComisionVendedoresTabMantenimiento";
            this.ComisionVendedoresTabMantenimiento.Padding = new System.Windows.Forms.Padding(16);
            this.ComisionVendedoresTabMantenimiento.Size = new System.Drawing.Size(992, 442);
            this.ComisionVendedoresTabMantenimiento.TabIndex = 2;
            this.ComisionVendedoresTabMantenimiento.Text = "Mantenimiento de porcentajes";
            // 
            // ComisionVendedoresDgvPorcentajes
            // 
            this.ComisionVendedoresDgvPorcentajes.AllowUserToAddRows = false;
            this.ComisionVendedoresDgvPorcentajes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            EstiloCelda33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.ComisionVendedoresDgvPorcentajes.AlternatingRowsDefaultCellStyle = EstiloCelda33;
            EstiloCelda31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            EstiloCelda31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda31.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            EstiloCelda31.ForeColor = System.Drawing.Color.White;
            EstiloCelda31.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            EstiloCelda31.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ComisionVendedoresDgvPorcentajes.ColumnHeadersDefaultCellStyle = EstiloCelda31;
            this.ComisionVendedoresDgvPorcentajes.ColumnHeadersHeight = 29;
            this.ComisionVendedoresDgvPorcentajes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ComisionVendedoresClmProducto,
            this.ComisionVendedoresClmMarca,
            this.ComisionVendedoresClmLinea,
            this.ComisionVendedoresClmCategoria,
            this.ComisionVendedoresClmPorcentajeMarca,
            this.ComisionVendedoresClmPorcentajeLinea,
            this.ComisionVendedoresClmPorcentajeCategoria,
            this.ComisionVendedoresClmPorcentajeProducto});
            this.ComisionVendedoresDgvPorcentajes.Location = new System.Drawing.Point(24, 104);
            this.ComisionVendedoresDgvPorcentajes.Name = "ComisionVendedoresDgvPorcentajes";
            EstiloCelda32.BackColor = System.Drawing.Color.White;
            EstiloCelda32.Font = new System.Drawing.Font("Segoe UI", 9F);
            EstiloCelda32.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            EstiloCelda32.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            EstiloCelda32.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresDgvPorcentajes.DefaultCellStyle = EstiloCelda32;
            this.ComisionVendedoresDgvPorcentajes.EnableHeadersVisualStyles = false;
            this.ComisionVendedoresDgvPorcentajes.ReadOnly = true;
            this.ComisionVendedoresDgvPorcentajes.RowHeadersVisible = false;
            this.ComisionVendedoresDgvPorcentajes.RowHeadersWidth = 51;
            this.ComisionVendedoresDgvPorcentajes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ComisionVendedoresDgvPorcentajes.MultiSelect = true;
            this.ComisionVendedoresDgvPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresDgvPorcentajes.Size = new System.Drawing.Size(930, 295);
            this.ComisionVendedoresDgvPorcentajes.TabIndex = 0;
            this.ComisionVendedoresDgvPorcentajes.AutoGenerateColumns = false;
            // 
            // ComisionVendedoresClmProducto
            // 
            this.ComisionVendedoresClmProducto.DataPropertyName = "NombreProducto";
            this.ComisionVendedoresClmProducto.HeaderText = "Producto";
            this.ComisionVendedoresClmProducto.MinimumWidth = 6;
            this.ComisionVendedoresClmProducto.Name = "ComisionVendedoresClmProducto";
            this.ComisionVendedoresClmProducto.ReadOnly = true;
            // 
            // ComisionVendedoresClmMarca
            // 
            this.ComisionVendedoresClmMarca.DataPropertyName = "Marca";
            this.ComisionVendedoresClmMarca.HeaderText = "Marca";
            this.ComisionVendedoresClmMarca.MinimumWidth = 6;
            this.ComisionVendedoresClmMarca.Name = "ComisionVendedoresClmMarca";
            this.ComisionVendedoresClmMarca.ReadOnly = true;
            // 
            // ComisionVendedoresClmLinea
            // 
            this.ComisionVendedoresClmLinea.DataPropertyName = "Linea";
            this.ComisionVendedoresClmLinea.HeaderText = "Línea";
            this.ComisionVendedoresClmLinea.MinimumWidth = 6;
            this.ComisionVendedoresClmLinea.Name = "ComisionVendedoresClmLinea";
            this.ComisionVendedoresClmLinea.ReadOnly = true;
            // 
            // ComisionVendedoresClmCategoria
            // 
            this.ComisionVendedoresClmCategoria.DataPropertyName = "Categoria";
            this.ComisionVendedoresClmCategoria.HeaderText = "Categoría";
            this.ComisionVendedoresClmCategoria.MinimumWidth = 6;
            this.ComisionVendedoresClmCategoria.Name = "ComisionVendedoresClmCategoria";
            this.ComisionVendedoresClmCategoria.ReadOnly = true;
            // 
            // ComisionVendedoresClmPorcentajeMarca
            // 
            EstiloCelda34.Format = "0.00\\ \\%";
            EstiloCelda34.FormatProvider = System.Globalization.CultureInfo.GetCultureInfo("es-GT");
            this.ComisionVendedoresClmPorcentajeMarca.DataPropertyName = "PorcentajeMarca";
            this.ComisionVendedoresClmPorcentajeMarca.DefaultCellStyle = EstiloCelda34;
            this.ComisionVendedoresClmPorcentajeMarca.HeaderText = "% Marca";
            this.ComisionVendedoresClmPorcentajeMarca.MinimumWidth = 6;
            this.ComisionVendedoresClmPorcentajeMarca.Name = "ComisionVendedoresClmPorcentajeMarca";
            this.ComisionVendedoresClmPorcentajeMarca.ReadOnly = true;
            this.ComisionVendedoresClmPorcentajeLinea.DataPropertyName = "PorcentajeLinea";
            this.ComisionVendedoresClmPorcentajeLinea.DefaultCellStyle = EstiloCelda34;
            this.ComisionVendedoresClmPorcentajeLinea.HeaderText = "% Línea";
            this.ComisionVendedoresClmPorcentajeLinea.MinimumWidth = 6;
            this.ComisionVendedoresClmPorcentajeLinea.Name = "ComisionVendedoresClmPorcentajeLinea";
            this.ComisionVendedoresClmPorcentajeLinea.ReadOnly = true;
            this.ComisionVendedoresClmPorcentajeCategoria.DataPropertyName = "PorcentajeCategoria";
            this.ComisionVendedoresClmPorcentajeCategoria.DefaultCellStyle = EstiloCelda34;
            this.ComisionVendedoresClmPorcentajeCategoria.HeaderText = "% Categoría";
            this.ComisionVendedoresClmPorcentajeCategoria.MinimumWidth = 6;
            this.ComisionVendedoresClmPorcentajeCategoria.Name = "ComisionVendedoresClmPorcentajeCategoria";
            this.ComisionVendedoresClmPorcentajeCategoria.ReadOnly = true;
            this.ComisionVendedoresClmPorcentajeProducto.DataPropertyName = "PorcentajeProducto";
            this.ComisionVendedoresClmPorcentajeProducto.DefaultCellStyle = EstiloCelda34;
            this.ComisionVendedoresClmPorcentajeProducto.HeaderText = "% Producto";
            this.ComisionVendedoresClmPorcentajeProducto.MinimumWidth = 6;
            this.ComisionVendedoresClmPorcentajeProducto.Name = "ComisionVendedoresClmPorcentajeProducto";
            this.ComisionVendedoresClmPorcentajeProducto.ReadOnly = true;
            //
            // Paginación de mantenimiento
            //
            this.ComisionVendedoresBtnAnteriorPorcentajes.Enabled = false;
            this.ComisionVendedoresBtnAnteriorPorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnAnteriorPorcentajes.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnAnteriorPorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnAnteriorPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.ComisionVendedoresBtnAnteriorPorcentajes.Location = new System.Drawing.Point(24, 410);
            this.ComisionVendedoresBtnAnteriorPorcentajes.Name = "ComisionVendedoresBtnAnteriorPorcentajes";
            this.ComisionVendedoresBtnAnteriorPorcentajes.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnAnteriorPorcentajes.TabIndex = 8;
            this.ComisionVendedoresBtnAnteriorPorcentajes.Text = "Anterior";
            this.ComisionVendedoresBtnAnteriorPorcentajes.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnAnteriorPorcentajes.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaPorcentajes);
            this.ComisionVendedoresBtnSiguientePorcentajes.Enabled = false;
            this.ComisionVendedoresBtnSiguientePorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.ComisionVendedoresBtnSiguientePorcentajes.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnSiguientePorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnSiguientePorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.ComisionVendedoresBtnSiguientePorcentajes.Location = new System.Drawing.Point(854, 410);
            this.ComisionVendedoresBtnSiguientePorcentajes.Name = "ComisionVendedoresBtnSiguientePorcentajes";
            this.ComisionVendedoresBtnSiguientePorcentajes.Size = new System.Drawing.Size(100, 28);
            this.ComisionVendedoresBtnSiguientePorcentajes.TabIndex = 9;
            this.ComisionVendedoresBtnSiguientePorcentajes.Text = "Siguiente";
            this.ComisionVendedoresBtnSiguientePorcentajes.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnSiguientePorcentajes.Click += new System.EventHandler(this.ComisionVendedoresMetCambiarPaginaPorcentajes);
            this.ComisionVendedoresLblPaginaPorcentajes.AutoSize = true;
            this.ComisionVendedoresLblPaginaPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComisionVendedoresLblPaginaPorcentajes.Location = new System.Drawing.Point(360, 414);
            this.ComisionVendedoresLblPaginaPorcentajes.Name = "ComisionVendedoresLblPaginaPorcentajes";
            this.ComisionVendedoresLblPaginaPorcentajes.Size = new System.Drawing.Size(190, 20);
            this.ComisionVendedoresLblPaginaPorcentajes.TabIndex = 10;
            this.ComisionVendedoresLblPaginaPorcentajes.Text = "Página 0 de 0 · 0 registros";
            // 
            // ComisionVendedoresLblAlcance
            // 
            this.ComisionVendedoresLblAlcance.AutoSize = true;
            this.ComisionVendedoresLblAlcance.Location = new System.Drawing.Point(24, 7);
            this.ComisionVendedoresLblAlcance.Name = "ComisionVendedoresLblAlcance";
            this.ComisionVendedoresLblAlcance.Size = new System.Drawing.Size(179, 23);
            this.ComisionVendedoresLblAlcance.TabIndex = 1;
            this.ComisionVendedoresLblAlcance.Text = "Aplicar porcentaje por";
            // 
            // ComisionVendedoresLblPorcentajeNuevo
            // 
            this.ComisionVendedoresLblPorcentajeNuevo.AutoSize = true;
            this.ComisionVendedoresLblPorcentajeNuevo.Location = new System.Drawing.Point(220, 7);
            this.ComisionVendedoresLblPorcentajeNuevo.Name = "ComisionVendedoresLblPorcentajeNuevo";
            this.ComisionVendedoresLblPorcentajeNuevo.Size = new System.Drawing.Size(142, 23);
            this.ComisionVendedoresLblPorcentajeNuevo.TabIndex = 2;
            this.ComisionVendedoresLblPorcentajeNuevo.Text = "Porcentaje nuevo";
            this.ComisionVendedoresLblObligatorioAlcance.AutoSize = true;
            this.ComisionVendedoresLblObligatorioAlcance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioAlcance.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioAlcance.Location = new System.Drawing.Point(201, 7);
            this.ComisionVendedoresLblObligatorioAlcance.Name = "ComisionVendedoresLblObligatorioAlcance";
            this.ComisionVendedoresLblObligatorioAlcance.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioAlcance.TabIndex = 11;
            this.ComisionVendedoresLblObligatorioAlcance.Text = "*";
            this.ComisionVendedoresLblObligatorioAlcance.TabStop = false;
            this.ComisionVendedoresLblObligatorioPorcentaje.AutoSize = true;
            this.ComisionVendedoresLblObligatorioPorcentaje.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.ComisionVendedoresLblObligatorioPorcentaje.ForeColor = System.Drawing.Color.Firebrick;
            this.ComisionVendedoresLblObligatorioPorcentaje.Location = new System.Drawing.Point(363, 7);
            this.ComisionVendedoresLblObligatorioPorcentaje.Name = "ComisionVendedoresLblObligatorioPorcentaje";
            this.ComisionVendedoresLblObligatorioPorcentaje.Size = new System.Drawing.Size(12, 18);
            this.ComisionVendedoresLblObligatorioPorcentaje.TabIndex = 12;
            this.ComisionVendedoresLblObligatorioPorcentaje.Text = "*";
            this.ComisionVendedoresLblObligatorioPorcentaje.TabStop = false;
            // 
            // ComisionVendedoresLblMantenimientoEstado
            // 
            this.ComisionVendedoresLblMantenimientoEstado.AutoSize = true;
            this.ComisionVendedoresLblMantenimientoEstado.Location = new System.Drawing.Point(24, 70);
            this.ComisionVendedoresLblMantenimientoEstado.Name = "ComisionVendedoresLblMantenimientoEstado";
            this.ComisionVendedoresLblMantenimientoEstado.Size = new System.Drawing.Size(684, 23);
            this.ComisionVendedoresLblMantenimientoEstado.TabIndex = 3;
            this.ComisionVendedoresLblMantenimientoEstado.Text = "La tabla muestra las 4 tasas por producto. Seleccione producto(s), elija alcance y nuevo %; Marca, Línea o Categoría actualiza la clasificación compartida.";
            // 
            // ComisionVendedoresBtnActualizar
            // 
            this.ComisionVendedoresBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.ComisionVendedoresBtnActualizar.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnActualizar.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresBtnActualizar.Location = new System.Drawing.Point(478, 23);
            this.ComisionVendedoresBtnActualizar.Name = "ComisionVendedoresBtnActualizar";
            this.ComisionVendedoresBtnActualizar.Size = new System.Drawing.Size(190, 30);
            this.ComisionVendedoresBtnActualizar.TabIndex = 4;
            this.ComisionVendedoresBtnActualizar.Text = "Actualizar porcentaje";
            this.ComisionVendedoresBtnActualizar.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnActualizar.Click += new System.EventHandler(this.ComisionVendedoresMetActualizarClick);
            // 
            // ComisionVendedoresBtnBuscar
            // 
            this.ComisionVendedoresBtnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresBtnBuscar.FlatAppearance.BorderSize = 0;
            this.ComisionVendedoresBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComisionVendedoresBtnBuscar.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresBtnBuscar.Location = new System.Drawing.Point(348, 23);
            this.ComisionVendedoresBtnBuscar.Name = "ComisionVendedoresBtnBuscar";
            this.ComisionVendedoresBtnBuscar.Size = new System.Drawing.Size(110, 30);
            this.ComisionVendedoresBtnBuscar.TabIndex = 5;
            this.ComisionVendedoresBtnBuscar.Text = "Buscar";
            this.ComisionVendedoresBtnBuscar.UseVisualStyleBackColor = false;
            this.ComisionVendedoresBtnBuscar.Click += new System.EventHandler(this.ComisionVendedoresMetBuscarClick);
            // 
            // ComisionVendedoresTxtPorcentaje
            // 
            this.ComisionVendedoresTxtPorcentaje.Location = new System.Drawing.Point(220, 25);
            this.ComisionVendedoresTxtPorcentaje.Name = "ComisionVendedoresTxtPorcentaje";
            this.ComisionVendedoresTxtPorcentaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresTxtPorcentaje.Size = new System.Drawing.Size(110, 30);
            this.ComisionVendedoresTxtPorcentaje.TabIndex = 6;
            // 
            // ComisionVendedoresCboAlcance
            // 
            this.ComisionVendedoresCboAlcance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComisionVendedoresCboAlcance.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.ComisionVendedoresCboAlcance.Location = new System.Drawing.Point(24, 25);
            this.ComisionVendedoresCboAlcance.Name = "ComisionVendedoresCboAlcance";
            this.ComisionVendedoresCboAlcance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComisionVendedoresCboAlcance.SelectedIndex = 0;
            this.ComisionVendedoresCboAlcance.Size = new System.Drawing.Size(160, 31);
            this.ComisionVendedoresCboAlcance.TabIndex = 7;
            // 
            // ComisionVendedoresPnlEncabezado
            // 
            this.ComisionVendedoresPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.ComisionVendedoresPnlEncabezado.Controls.Add(this.ComisionVendedoresLblTitulo);
            this.ComisionVendedoresPnlEncabezado.Controls.Add(this.ComisionVendedoresLblSubtitulo);
            this.ComisionVendedoresPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComisionVendedoresPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComisionVendedoresPnlEncabezado.Name = "ComisionVendedoresPnlEncabezado";
            this.ComisionVendedoresPnlEncabezado.Size = new System.Drawing.Size(1000, 82);
            this.ComisionVendedoresPnlEncabezado.TabIndex = 1;
            // 
            // ComisionVendedoresLblTitulo
            // 
            this.ComisionVendedoresLblTitulo.AutoSize = true;
            this.ComisionVendedoresLblTitulo.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.ComisionVendedoresLblTitulo.ForeColor = System.Drawing.Color.White;
            this.ComisionVendedoresLblTitulo.Location = new System.Drawing.Point(28, 13);
            this.ComisionVendedoresLblTitulo.Name = "ComisionVendedoresLblTitulo";
            this.ComisionVendedoresLblTitulo.Size = new System.Drawing.Size(412, 46);
            this.ComisionVendedoresLblTitulo.TabIndex = 0;
            this.ComisionVendedoresLblTitulo.Text = "Comisión de vendedores";
            // 
            // ComisionVendedoresLblSubtitulo
            // 
            this.ComisionVendedoresLblSubtitulo.AutoSize = true;
            this.ComisionVendedoresLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComisionVendedoresLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.ComisionVendedoresLblSubtitulo.Location = new System.Drawing.Point(32, 55);
            this.ComisionVendedoresLblSubtitulo.Name = "ComisionVendedoresLblSubtitulo";
            this.ComisionVendedoresLblSubtitulo.Size = new System.Drawing.Size(431, 23);
            this.ComisionVendedoresLblSubtitulo.TabIndex = 1;
            this.ComisionVendedoresLblSubtitulo.Text = "Cálculo de comisiones y mantenimiento de porcentajes";
            // 
            // FrmComisionVendedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1000, 560);
            this.Controls.Add(this.ComisionVendedoresTbcPestanas);
            this.Controls.Add(this.ComisionVendedoresPnlEncabezado);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.Icon = ((System.Drawing.Icon)(RecursosFormulario.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmComisionVendedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7008 – ComisionVendedores";
            this.ComisionVendedoresTbcPestanas.ResumeLayout(false);
            this.ComisionVendedoresTabCalculo.ResumeLayout(false);
            this.ComisionVendedoresTabCalculo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvResultados)).EndInit();
            this.ComisionVendedoresTabAsignarFactura.ResumeLayout(false);
            this.ComisionVendedoresTabAsignarFactura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvDetalleFactura)).EndInit();
            this.ComisionVendedoresTabMantenimiento.ResumeLayout(false);
            this.ComisionVendedoresTabMantenimiento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ComisionVendedoresDgvPorcentajes)).EndInit();
            this.ComisionVendedoresPnlEncabezado.ResumeLayout(false);
            this.ComisionVendedoresPnlEncabezado.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
