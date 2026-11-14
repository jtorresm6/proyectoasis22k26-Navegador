// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Declara los controles y eventos del formulario principal.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista_ComisionVendedores_Ventas
{
    partial class FrmComisionesVendedoresVentas
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
        private System.Windows.Forms.Button VentasBtnAnteriorResultados;
        private System.Windows.Forms.Button VentasBtnSiguienteResultados;
        private System.Windows.Forms.Label VentasLblPaginaResultados;
        private System.Windows.Forms.Label VentasLblEstado;
        private System.Windows.Forms.ComboBox VentasCboAlcance;
        private System.Windows.Forms.TextBox VentasTxtPorcentaje;
        private System.Windows.Forms.Button VentasBtnBuscar;
        private System.Windows.Forms.Button VentasBtnActualizar;
        private System.Windows.Forms.DataGridView VentasDgvPorcentajes;
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
            if (Disposing && VentasErpValidacion != null) VentasErpValidacion.Dispose();
            if (Disposing && VentasTtpAcciones != null) VentasTtpAcciones.Dispose();
            base.Dispose(Disposing);
        }

        private TabPage VentasTbpNomina;
        private DataGridView VentasDgvNomina;
        private Label VentasLblNominaEstado;
        private Button VentasBtnAnteriorNomina;
        private Button VentasBtnSiguienteNomina;
        private Button VentasBtnActualizarNomina;
        private Label VentasLblPaginaNomina;
        private ComboBox VentasCboElementoPorcentaje;
        private Label VentasLblElementoPorcentaje;
        private Label VentasLblObligatorioElementoPorcentaje;
        private ToolTip VentasTtpAcciones;
        private Panel VentasPnlFechaInicio;
        private Panel VentasPnlFechaFin;
        private Panel VentasPnlFactura;
        private Panel VentasPnlVendedor;
        private Panel VentasPnlAlcance;
        private Panel VentasPnlPorcentaje;
        private Panel VentasPnlElementoPorcentaje;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmComisionesVendedoresVentas));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasTbpNomina = new System.Windows.Forms.TabPage();
            this.VentasDgvNomina = new System.Windows.Forms.DataGridView();
            this.VentasLblNominaEstado = new System.Windows.Forms.Label();
            this.VentasLblPaginaNomina = new System.Windows.Forms.Label();
            this.VentasBtnActualizarNomina = new System.Windows.Forms.Button();
            this.VentasBtnAnteriorNomina = new System.Windows.Forms.Button();
            this.VentasBtnSiguienteNomina = new System.Windows.Forms.Button();
            this.VentasCboElementoPorcentaje = new System.Windows.Forms.ComboBox();
            this.VentasLblElementoPorcentaje = new System.Windows.Forms.Label();
            this.VentasLblObligatorioElementoPorcentaje = new System.Windows.Forms.Label();
            this.VentasTtpAcciones = new System.Windows.Forms.ToolTip(this.components);
            this.VentasBtnAyuda = new System.Windows.Forms.Button();
            this.VentasBtnCalcular = new System.Windows.Forms.Button();
            this.VentasBtnCalcularFactura = new System.Windows.Forms.Button();
            this.VentasBtnAsignarVendedorFactura = new System.Windows.Forms.Button();
            this.VentasBtnRegistrarAbonoFactura = new System.Windows.Forms.Button();
            this.VentasBtnBuscar = new System.Windows.Forms.Button();
            this.VentasBtnActualizar = new System.Windows.Forms.Button();
            this.VentasBtnAnteriorResultados = new System.Windows.Forms.Button();
            this.VentasBtnSiguienteResultados = new System.Windows.Forms.Button();
            this.VentasBtnAnteriorFactura = new System.Windows.Forms.Button();
            this.VentasBtnSiguienteFactura = new System.Windows.Forms.Button();
            this.VentasBtnAnteriorPorcentajes = new System.Windows.Forms.Button();
            this.VentasBtnSiguientePorcentajes = new System.Windows.Forms.Button();
            this.VentasPnlFechaInicio = new System.Windows.Forms.Panel();
            this.VentasDtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.VentasPnlFechaFin = new System.Windows.Forms.Panel();
            this.VentasDtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.VentasPnlFactura = new System.Windows.Forms.Panel();
            this.VentasCboFactura = new System.Windows.Forms.ComboBox();
            this.VentasPnlVendedor = new System.Windows.Forms.Panel();
            this.VentasCboVendedorFactura = new System.Windows.Forms.ComboBox();
            this.VentasPnlAlcance = new System.Windows.Forms.Panel();
            this.VentasCboAlcance = new System.Windows.Forms.ComboBox();
            this.VentasPnlPorcentaje = new System.Windows.Forms.Panel();
            this.VentasTxtPorcentaje = new System.Windows.Forms.TextBox();
            this.VentasPnlElementoPorcentaje = new System.Windows.Forms.Panel();
            this.VentasTbcPestanas = new System.Windows.Forms.TabControl();
            this.VentasTbpCalculo = new System.Windows.Forms.TabPage();
            this.VentasDgvResultados = new System.Windows.Forms.DataGridView();
            this.VentasLblPaginaResultados = new System.Windows.Forms.Label();
            this.VentasLblFechaInicio = new System.Windows.Forms.Label();
            this.VentasLblFechaFin = new System.Windows.Forms.Label();
            this.VentasLblEstado = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaInicio = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFechaFin = new System.Windows.Forms.Label();
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
            this.VentasLblPaginaFactura = new System.Windows.Forms.Label();
            this.VentasLblEstadoFactura = new System.Windows.Forms.Label();
            this.VentasCboMetodoFactura = new System.Windows.Forms.ComboBox();
            this.VentasLblMetodoFactura = new System.Windows.Forms.Label();
            this.VentasLblVendedorFactura = new System.Windows.Forms.Label();
            this.VentasLblFactura = new System.Windows.Forms.Label();
            this.VentasLblObligatorioFactura = new System.Windows.Forms.Label();
            this.VentasLblObligatorioVendedor = new System.Windows.Forms.Label();
            this.VentasTbpMantenimiento = new System.Windows.Forms.TabPage();
            this.VentasDgvPorcentajes = new System.Windows.Forms.DataGridView();
            this.VentasLblPaginaPorcentajes = new System.Windows.Forms.Label();
            this.VentasLblAlcance = new System.Windows.Forms.Label();
            this.VentasLblPorcentajeNuevo = new System.Windows.Forms.Label();
            this.VentasLblMantenimientoEstado = new System.Windows.Forms.Label();
            this.VentasLblObligatorioAlcance = new System.Windows.Forms.Label();
            this.VentasLblObligatorioPorcentaje = new System.Windows.Forms.Label();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasTbpNomina.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvNomina)).BeginInit();
            this.VentasPnlFechaInicio.SuspendLayout();
            this.VentasPnlFechaFin.SuspendLayout();
            this.VentasPnlFactura.SuspendLayout();
            this.VentasPnlVendedor.SuspendLayout();
            this.VentasPnlAlcance.SuspendLayout();
            this.VentasPnlPorcentaje.SuspendLayout();
            this.VentasPnlElementoPorcentaje.SuspendLayout();
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
            // VentasTbpNomina
            //
            this.VentasTbpNomina.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpNomina.Controls.Add(this.VentasDgvNomina);
            this.VentasTbpNomina.Controls.Add(this.VentasLblNominaEstado);
            this.VentasTbpNomina.Controls.Add(this.VentasLblPaginaNomina);
            this.VentasTbpNomina.Controls.Add(this.VentasBtnActualizarNomina);
            this.VentasTbpNomina.Controls.Add(this.VentasBtnAnteriorNomina);
            this.VentasTbpNomina.Controls.Add(this.VentasBtnSiguienteNomina);
            this.VentasTbpNomina.Location = new System.Drawing.Point(4, 30);
            this.VentasTbpNomina.Name = "VentasTbpNomina";
            this.VentasTbpNomina.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpNomina.Size = new System.Drawing.Size(1272, 584);
            this.VentasTbpNomina.TabIndex = 3;
            this.VentasTbpNomina.Text = "Comisiones emitidas";
            //
            // VentasDgvNomina
            //
            this.VentasDgvNomina.AllowUserToAddRows = false;
            this.VentasDgvNomina.AllowUserToDeleteRows = false;
            this.VentasDgvNomina.AllowUserToResizeRows = false;
            this.VentasDgvNomina.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasDgvNomina.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvNomina.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvNomina.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.VentasDgvNomina.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.VentasDgvNomina.ColumnHeadersHeight = 54;
            this.VentasDgvNomina.EnableHeadersVisualStyles = false;
            this.VentasDgvNomina.Location = new System.Drawing.Point(24, 104);
            this.VentasDgvNomina.MultiSelect = false;
            this.VentasDgvNomina.Name = "VentasDgvNomina";
            this.VentasDgvNomina.ReadOnly = true;
            this.VentasDgvNomina.RowHeadersVisible = false;
            this.VentasDgvNomina.RowHeadersWidth = 51;
            this.VentasDgvNomina.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvNomina.Size = new System.Drawing.Size(1210, 394);
            this.VentasDgvNomina.TabIndex = 0;
            this.VentasDgvNomina.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VentasMetFormatearImporteNomina);
            this.VentasDgvNomina.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.VentasMetOrdenarTabla);
            //
            // VentasLblNominaEstado
            //
            this.VentasLblNominaEstado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblNominaEstado.Location = new System.Drawing.Point(24, 75);
            this.VentasLblNominaEstado.Name = "VentasLblNominaEstado";
            this.VentasLblNominaEstado.Size = new System.Drawing.Size(1080, 27);
            this.VentasLblNominaEstado.TabIndex = 1;
            this.VentasLblNominaEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // VentasLblPaginaNomina
            //
            this.VentasLblPaginaNomina.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaNomina.Location = new System.Drawing.Point(486, 532);
            this.VentasLblPaginaNomina.Name = "VentasLblPaginaNomina";
            this.VentasLblPaginaNomina.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaNomina.TabIndex = 2;
            this.VentasLblPaginaNomina.Text = "Página 0 de 0 · 0 registros";
            this.VentasLblPaginaNomina.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VentasBtnActualizarNomina
            //
            this.VentasBtnActualizarNomina.AccessibleName = "Actualizar";
            this.VentasBtnActualizarNomina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnActualizarNomina.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnActualizarNomina.BackgroundImage")));
            this.VentasBtnActualizarNomina.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnActualizarNomina.FlatAppearance.BorderSize = 0;
            this.VentasBtnActualizarNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnActualizarNomina.Location = new System.Drawing.Point(1166, 8);
            this.VentasBtnActualizarNomina.Name = "VentasBtnActualizarNomina";
            this.VentasBtnActualizarNomina.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnActualizarNomina.TabIndex = 3;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnActualizarNomina, "Actualizar");
            this.VentasBtnActualizarNomina.UseVisualStyleBackColor = false;
            this.VentasBtnActualizarNomina.Click += new System.EventHandler(this.VentasMetCargarNominaClick);
            //
            // VentasBtnAnteriorNomina
            //
            this.VentasBtnAnteriorNomina.AccessibleName = "Anterior";
            this.VentasBtnAnteriorNomina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentasBtnAnteriorNomina.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAnteriorNomina.BackgroundImage")));
            this.VentasBtnAnteriorNomina.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnteriorNomina.Enabled = false;
            this.VentasBtnAnteriorNomina.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorNomina.Location = new System.Drawing.Point(24, 498);
            this.VentasBtnAnteriorNomina.Name = "VentasBtnAnteriorNomina";
            this.VentasBtnAnteriorNomina.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAnteriorNomina.TabIndex = 4;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAnteriorNomina, "Anterior");
            this.VentasBtnAnteriorNomina.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorNomina.Click += new System.EventHandler(this.VentasMetCambiarPaginaNomina);
            //
            // VentasBtnSiguienteNomina
            //
            this.VentasBtnSiguienteNomina.AccessibleName = "Siguiente";
            this.VentasBtnSiguienteNomina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnSiguienteNomina.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnSiguienteNomina.BackgroundImage")));
            this.VentasBtnSiguienteNomina.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSiguienteNomina.Enabled = false;
            this.VentasBtnSiguienteNomina.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguienteNomina.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguienteNomina.Location = new System.Drawing.Point(1170, 498);
            this.VentasBtnSiguienteNomina.Name = "VentasBtnSiguienteNomina";
            this.VentasBtnSiguienteNomina.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnSiguienteNomina.TabIndex = 5;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnSiguienteNomina, "Siguiente");
            this.VentasBtnSiguienteNomina.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteNomina.Click += new System.EventHandler(this.VentasMetCambiarPaginaNomina);
            //
            // VentasCboElementoPorcentaje
            //
            this.VentasCboElementoPorcentaje.AccessibleDescription = "Campo obligatorio";
            this.VentasCboElementoPorcentaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasCboElementoPorcentaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboElementoPorcentaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboElementoPorcentaje.Location = new System.Drawing.Point(2, 2);
            this.VentasCboElementoPorcentaje.Name = "VentasCboElementoPorcentaje";
            this.VentasCboElementoPorcentaje.Size = new System.Drawing.Size(216, 31);
            this.VentasCboElementoPorcentaje.TabIndex = 0;
            //
            // VentasLblElementoPorcentaje
            //
            this.VentasLblElementoPorcentaje.Location = new System.Drawing.Point(190, 7);
            this.VentasLblElementoPorcentaje.Name = "VentasLblElementoPorcentaje";
            this.VentasLblElementoPorcentaje.Size = new System.Drawing.Size(220, 23);
            this.VentasLblElementoPorcentaje.TabIndex = 14;
            this.VentasLblElementoPorcentaje.Text = "Elemento específico";
            //
            // VentasLblObligatorioElementoPorcentaje
            //
            this.VentasLblObligatorioElementoPorcentaje.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioElementoPorcentaje.Location = new System.Drawing.Point(323, 7);
            this.VentasLblObligatorioElementoPorcentaje.Name = "VentasLblObligatorioElementoPorcentaje";
            this.VentasLblObligatorioElementoPorcentaje.Size = new System.Drawing.Size(12, 18);
            this.VentasLblObligatorioElementoPorcentaje.TabIndex = 15;
            this.VentasLblObligatorioElementoPorcentaje.Text = "*";
            //
            // VentasBtnAyuda
            //
            this.VentasBtnAyuda.AccessibleName = "Ayuda";
            this.VentasBtnAyuda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasBtnAyuda.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAyuda.BackgroundImage")));
            this.VentasBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAyuda.FlatAppearance.BorderSize = 0;
            this.VentasBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAyuda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnAyuda.Location = new System.Drawing.Point(664, 5);
            this.VentasBtnAyuda.Name = "VentasBtnAyuda";
            this.VentasBtnAyuda.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAyuda.TabIndex = 5;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAyuda, "Ayuda");
            this.VentasBtnAyuda.UseVisualStyleBackColor = false;
            this.VentasBtnAyuda.Click += new System.EventHandler(this.VentasMetAbrirAyuda);
            //
            // VentasBtnCalcular
            //
            this.VentasBtnCalcular.AccessibleName = "Calcular";
            this.VentasBtnCalcular.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcular.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnCalcular.BackgroundImage")));
            this.VentasBtnCalcular.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCalcular.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcular.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnCalcular.Location = new System.Drawing.Point(572, 5);
            this.VentasBtnCalcular.Name = "VentasBtnCalcular";
            this.VentasBtnCalcular.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCalcular.TabIndex = 4;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnCalcular, "Calcular");
            this.VentasBtnCalcular.UseVisualStyleBackColor = false;
            this.VentasBtnCalcular.Click += new System.EventHandler(this.VentasMetCalcularClick);
            //
            // VentasBtnCalcularFactura
            //
            this.VentasBtnCalcularFactura.AccessibleName = "Calcular comisión";
            this.VentasBtnCalcularFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnCalcularFactura.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnCalcularFactura.BackgroundImage")));
            this.VentasBtnCalcularFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCalcularFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcularFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcularFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnCalcularFactura.Location = new System.Drawing.Point(24, 80);
            this.VentasBtnCalcularFactura.Name = "VentasBtnCalcularFactura";
            this.VentasBtnCalcularFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnCalcularFactura.TabIndex = 2;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnCalcularFactura, "Calcular comisión");
            this.VentasBtnCalcularFactura.UseVisualStyleBackColor = false;
            this.VentasBtnCalcularFactura.Click += new System.EventHandler(this.VentasMetCalcularFacturaClick);
            //
            // VentasBtnAsignarVendedorFactura
            //
            this.VentasBtnAsignarVendedorFactura.AccessibleName = "Asignar vendedor";
            this.VentasBtnAsignarVendedorFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(162)))), ((int)(((byte)(74)))));
            this.VentasBtnAsignarVendedorFactura.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAsignarVendedorFactura.BackgroundImage")));
            this.VentasBtnAsignarVendedorFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAsignarVendedorFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnAsignarVendedorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAsignarVendedorFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnAsignarVendedorFactura.Location = new System.Drawing.Point(116, 80);
            this.VentasBtnAsignarVendedorFactura.Name = "VentasBtnAsignarVendedorFactura";
            this.VentasBtnAsignarVendedorFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAsignarVendedorFactura.TabIndex = 14;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAsignarVendedorFactura, "Asignar vendedor");
            this.VentasBtnAsignarVendedorFactura.UseVisualStyleBackColor = false;
            this.VentasBtnAsignarVendedorFactura.Click += new System.EventHandler(this.VentasMetAsignarVendedorFacturaClick);
            //
            // VentasBtnRegistrarAbonoFactura
            //
            this.VentasBtnRegistrarAbonoFactura.AccessibleName = "Realizar pago";
            this.VentasBtnRegistrarAbonoFactura.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnRegistrarAbonoFactura.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnRegistrarAbonoFactura.BackgroundImage")));
            this.VentasBtnRegistrarAbonoFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnRegistrarAbonoFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnRegistrarAbonoFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnRegistrarAbonoFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnRegistrarAbonoFactura.Location = new System.Drawing.Point(208, 80);
            this.VentasBtnRegistrarAbonoFactura.Name = "VentasBtnRegistrarAbonoFactura";
            this.VentasBtnRegistrarAbonoFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnRegistrarAbonoFactura.TabIndex = 15;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnRegistrarAbonoFactura, "Realizar pago");
            this.VentasBtnRegistrarAbonoFactura.UseVisualStyleBackColor = false;
            this.VentasBtnRegistrarAbonoFactura.Click += new System.EventHandler(this.VentasMetRegistrarAbonoFacturaClick);
            //
            // VentasBtnBuscar
            //
            this.VentasBtnBuscar.AccessibleName = "Buscar";
            this.VentasBtnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasBtnBuscar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnBuscar.BackgroundImage")));
            this.VentasBtnBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnBuscar.FlatAppearance.BorderSize = 0;
            this.VentasBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnBuscar.Location = new System.Drawing.Point(570, 4);
            this.VentasBtnBuscar.Name = "VentasBtnBuscar";
            this.VentasBtnBuscar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnBuscar.TabIndex = 5;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnBuscar, "Buscar");
            this.VentasBtnBuscar.UseVisualStyleBackColor = false;
            this.VentasBtnBuscar.Click += new System.EventHandler(this.VentasMetBuscarClick);
            //
            // VentasBtnActualizar
            //
            this.VentasBtnActualizar.AccessibleName = "Actualizar porcentaje";
            this.VentasBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasBtnActualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnActualizar.BackgroundImage")));
            this.VentasBtnActualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnActualizar.FlatAppearance.BorderSize = 0;
            this.VentasBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnActualizar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasBtnActualizar.Location = new System.Drawing.Point(664, 4);
            this.VentasBtnActualizar.Name = "VentasBtnActualizar";
            this.VentasBtnActualizar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnActualizar.TabIndex = 4;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnActualizar, "Actualizar porcentaje");
            this.VentasBtnActualizar.UseVisualStyleBackColor = false;
            this.VentasBtnActualizar.Click += new System.EventHandler(this.VentasMetActualizarClick);
            //
            // VentasBtnAnteriorResultados
            //
            this.VentasBtnAnteriorResultados.AccessibleName = "Anterior";
            this.VentasBtnAnteriorResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentasBtnAnteriorResultados.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorResultados.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAnteriorResultados.BackgroundImage")));
            this.VentasBtnAnteriorResultados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnteriorResultados.Enabled = false;
            this.VentasBtnAnteriorResultados.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorResultados.Location = new System.Drawing.Point(24, 498);
            this.VentasBtnAnteriorResultados.Name = "VentasBtnAnteriorResultados";
            this.VentasBtnAnteriorResultados.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAnteriorResultados.TabIndex = 10;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAnteriorResultados, "Anterior");
            this.VentasBtnAnteriorResultados.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorResultados.Click += new System.EventHandler(this.VentasMetCambiarPaginaResultados);
            //
            // VentasBtnSiguienteResultados
            //
            this.VentasBtnSiguienteResultados.AccessibleName = "Siguiente";
            this.VentasBtnSiguienteResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnSiguienteResultados.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguienteResultados.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnSiguienteResultados.BackgroundImage")));
            this.VentasBtnSiguienteResultados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSiguienteResultados.Enabled = false;
            this.VentasBtnSiguienteResultados.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguienteResultados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguienteResultados.Location = new System.Drawing.Point(1170, 498);
            this.VentasBtnSiguienteResultados.Name = "VentasBtnSiguienteResultados";
            this.VentasBtnSiguienteResultados.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnSiguienteResultados.TabIndex = 11;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnSiguienteResultados, "Siguiente");
            this.VentasBtnSiguienteResultados.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteResultados.Click += new System.EventHandler(this.VentasMetCambiarPaginaResultados);
            //
            // VentasBtnAnteriorFactura
            //
            this.VentasBtnAnteriorFactura.AccessibleName = "Anterior";
            this.VentasBtnAnteriorFactura.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentasBtnAnteriorFactura.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorFactura.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAnteriorFactura.BackgroundImage")));
            this.VentasBtnAnteriorFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnteriorFactura.Enabled = false;
            this.VentasBtnAnteriorFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorFactura.Location = new System.Drawing.Point(24, 498);
            this.VentasBtnAnteriorFactura.Name = "VentasBtnAnteriorFactura";
            this.VentasBtnAnteriorFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAnteriorFactura.TabIndex = 10;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAnteriorFactura, "Anterior");
            this.VentasBtnAnteriorFactura.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorFactura.Click += new System.EventHandler(this.VentasMetCambiarPaginaFactura);
            //
            // VentasBtnSiguienteFactura
            //
            this.VentasBtnSiguienteFactura.AccessibleName = "Siguiente";
            this.VentasBtnSiguienteFactura.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnSiguienteFactura.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguienteFactura.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnSiguienteFactura.BackgroundImage")));
            this.VentasBtnSiguienteFactura.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSiguienteFactura.Enabled = false;
            this.VentasBtnSiguienteFactura.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguienteFactura.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguienteFactura.Location = new System.Drawing.Point(1170, 498);
            this.VentasBtnSiguienteFactura.Name = "VentasBtnSiguienteFactura";
            this.VentasBtnSiguienteFactura.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnSiguienteFactura.TabIndex = 11;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnSiguienteFactura, "Siguiente");
            this.VentasBtnSiguienteFactura.UseVisualStyleBackColor = false;
            this.VentasBtnSiguienteFactura.Click += new System.EventHandler(this.VentasMetCambiarPaginaFactura);
            //
            // VentasBtnAnteriorPorcentajes
            //
            this.VentasBtnAnteriorPorcentajes.AccessibleName = "Anterior";
            this.VentasBtnAnteriorPorcentajes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VentasBtnAnteriorPorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnAnteriorPorcentajes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnAnteriorPorcentajes.BackgroundImage")));
            this.VentasBtnAnteriorPorcentajes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnteriorPorcentajes.Enabled = false;
            this.VentasBtnAnteriorPorcentajes.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnteriorPorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnteriorPorcentajes.Location = new System.Drawing.Point(24, 498);
            this.VentasBtnAnteriorPorcentajes.Name = "VentasBtnAnteriorPorcentajes";
            this.VentasBtnAnteriorPorcentajes.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnAnteriorPorcentajes.TabIndex = 8;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnAnteriorPorcentajes, "Anterior");
            this.VentasBtnAnteriorPorcentajes.UseVisualStyleBackColor = false;
            this.VentasBtnAnteriorPorcentajes.Click += new System.EventHandler(this.VentasMetCambiarPaginaPorcentajes);
            //
            // VentasBtnSiguientePorcentajes
            //
            this.VentasBtnSiguientePorcentajes.AccessibleName = "Siguiente";
            this.VentasBtnSiguientePorcentajes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasBtnSiguientePorcentajes.BackColor = System.Drawing.Color.LightGray;
            this.VentasBtnSiguientePorcentajes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("VentasBtnSiguientePorcentajes.BackgroundImage")));
            this.VentasBtnSiguientePorcentajes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSiguientePorcentajes.Enabled = false;
            this.VentasBtnSiguientePorcentajes.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguientePorcentajes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguientePorcentajes.Location = new System.Drawing.Point(1170, 498);
            this.VentasBtnSiguientePorcentajes.Name = "VentasBtnSiguientePorcentajes";
            this.VentasBtnSiguientePorcentajes.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnSiguientePorcentajes.TabIndex = 9;
            this.VentasTtpAcciones.SetToolTip(this.VentasBtnSiguientePorcentajes, "Siguiente");
            this.VentasBtnSiguientePorcentajes.UseVisualStyleBackColor = false;
            this.VentasBtnSiguientePorcentajes.Click += new System.EventHandler(this.VentasMetCambiarPaginaPorcentajes);
            //
            // VentasPnlFechaInicio
            //
            this.VentasPnlFechaInicio.Controls.Add(this.VentasDtpFechaInicio);
            this.VentasPnlFechaInicio.Location = new System.Drawing.Point(24, 26);
            this.VentasPnlFechaInicio.Name = "VentasPnlFechaInicio";
            this.VentasPnlFechaInicio.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlFechaInicio.Size = new System.Drawing.Size(160, 30);
            this.VentasPnlFechaInicio.TabIndex = 9;
            //
            // VentasDtpFechaInicio
            //
            this.VentasDtpFechaInicio.AccessibleDescription = "Campo obligatorio";
            this.VentasDtpFechaInicio.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasDtpFechaInicio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaInicio.Location = new System.Drawing.Point(2, 2);
            this.VentasDtpFechaInicio.Name = "VentasDtpFechaInicio";
            this.VentasDtpFechaInicio.Size = new System.Drawing.Size(156, 30);
            this.VentasDtpFechaInicio.TabIndex = 0;
            //
            // VentasPnlFechaFin
            //
            this.VentasPnlFechaFin.Controls.Add(this.VentasDtpFechaFin);
            this.VentasPnlFechaFin.Location = new System.Drawing.Point(200, 26);
            this.VentasPnlFechaFin.Name = "VentasPnlFechaFin";
            this.VentasPnlFechaFin.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlFechaFin.Size = new System.Drawing.Size(160, 30);
            this.VentasPnlFechaFin.TabIndex = 8;
            //
            // VentasDtpFechaFin
            //
            this.VentasDtpFechaFin.AccessibleDescription = "Campo obligatorio";
            this.VentasDtpFechaFin.CustomFormat = "dd/MM/yyyy";
            this.VentasDtpFechaFin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasDtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.VentasDtpFechaFin.Location = new System.Drawing.Point(2, 2);
            this.VentasDtpFechaFin.Name = "VentasDtpFechaFin";
            this.VentasDtpFechaFin.Size = new System.Drawing.Size(156, 30);
            this.VentasDtpFechaFin.TabIndex = 0;
            //
            // VentasPnlFactura
            //
            this.VentasPnlFactura.Controls.Add(this.VentasCboFactura);
            this.VentasPnlFactura.Location = new System.Drawing.Point(24, 45);
            this.VentasPnlFactura.Name = "VentasPnlFactura";
            this.VentasPnlFactura.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlFactura.Size = new System.Drawing.Size(390, 31);
            this.VentasPnlFactura.TabIndex = 6;
            //
            // VentasCboFactura
            //
            this.VentasCboFactura.AccessibleDescription = "Campo obligatorio";
            this.VentasCboFactura.DisplayMember = "Etiqueta";
            this.VentasCboFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasCboFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboFactura.Location = new System.Drawing.Point(2, 2);
            this.VentasCboFactura.Name = "VentasCboFactura";
            this.VentasCboFactura.Size = new System.Drawing.Size(386, 31);
            this.VentasCboFactura.TabIndex = 0;
            this.VentasCboFactura.ValueMember = "IdFactura";
            this.VentasCboFactura.SelectedValueChanged += new System.EventHandler(this.VentasMetCambioSeleccionFactura);
            //
            // VentasPnlVendedor
            //
            this.VentasPnlVendedor.Controls.Add(this.VentasCboVendedorFactura);
            this.VentasPnlVendedor.Location = new System.Drawing.Point(430, 45);
            this.VentasPnlVendedor.Name = "VentasPnlVendedor";
            this.VentasPnlVendedor.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlVendedor.Size = new System.Drawing.Size(200, 31);
            this.VentasPnlVendedor.TabIndex = 5;
            //
            // VentasCboVendedorFactura
            //
            this.VentasCboVendedorFactura.AccessibleDescription = "Campo obligatorio";
            this.VentasCboVendedorFactura.DisplayMember = "NombreVendedor";
            this.VentasCboVendedorFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasCboVendedorFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboVendedorFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboVendedorFactura.Location = new System.Drawing.Point(2, 2);
            this.VentasCboVendedorFactura.Name = "VentasCboVendedorFactura";
            this.VentasCboVendedorFactura.Size = new System.Drawing.Size(196, 31);
            this.VentasCboVendedorFactura.TabIndex = 0;
            this.VentasCboVendedorFactura.ValueMember = "IdVendedor";
            this.VentasCboVendedorFactura.SelectedValueChanged += new System.EventHandler(this.VentasMetCambioSeleccionFactura);
            //
            // VentasPnlAlcance
            //
            this.VentasPnlAlcance.Controls.Add(this.VentasCboAlcance);
            this.VentasPnlAlcance.Location = new System.Drawing.Point(24, 25);
            this.VentasPnlAlcance.Name = "VentasPnlAlcance";
            this.VentasPnlAlcance.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlAlcance.Size = new System.Drawing.Size(150, 31);
            this.VentasPnlAlcance.TabIndex = 7;
            //
            // VentasCboAlcance
            //
            this.VentasCboAlcance.AccessibleDescription = "Campo obligatorio";
            this.VentasCboAlcance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasCboAlcance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboAlcance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboAlcance.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.VentasCboAlcance.Location = new System.Drawing.Point(2, 2);
            this.VentasCboAlcance.Name = "VentasCboAlcance";
            this.VentasCboAlcance.Size = new System.Drawing.Size(146, 31);
            this.VentasCboAlcance.TabIndex = 0;
            this.VentasCboAlcance.SelectedIndexChanged += new System.EventHandler(this.VentasMetCambiarAlcancePorcentaje);
            //
            // VentasPnlPorcentaje
            //
            this.VentasPnlPorcentaje.Controls.Add(this.VentasTxtPorcentaje);
            this.VentasPnlPorcentaje.Location = new System.Drawing.Point(420, 25);
            this.VentasPnlPorcentaje.Name = "VentasPnlPorcentaje";
            this.VentasPnlPorcentaje.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlPorcentaje.Size = new System.Drawing.Size(110, 30);
            this.VentasPnlPorcentaje.TabIndex = 6;
            //
            // VentasTxtPorcentaje
            //
            this.VentasTxtPorcentaje.AccessibleDescription = "Campo obligatorio";
            this.VentasTxtPorcentaje.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasTxtPorcentaje.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtPorcentaje.Location = new System.Drawing.Point(2, 2);
            this.VentasTxtPorcentaje.Name = "VentasTxtPorcentaje";
            this.VentasTxtPorcentaje.Size = new System.Drawing.Size(106, 30);
            this.VentasTxtPorcentaje.TabIndex = 0;
            //
            // VentasPnlElementoPorcentaje
            //
            this.VentasPnlElementoPorcentaje.Controls.Add(this.VentasCboElementoPorcentaje);
            this.VentasPnlElementoPorcentaje.Location = new System.Drawing.Point(190, 25);
            this.VentasPnlElementoPorcentaje.Name = "VentasPnlElementoPorcentaje";
            this.VentasPnlElementoPorcentaje.Padding = new System.Windows.Forms.Padding(2);
            this.VentasPnlElementoPorcentaje.Size = new System.Drawing.Size(220, 31);
            this.VentasPnlElementoPorcentaje.TabIndex = 13;
            //
            // VentasTbcPestanas
            //
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpCalculo);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpAsignarFactura);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpMantenimiento);
            this.VentasTbcPestanas.Controls.Add(this.VentasTbpNomina);
            this.VentasTbcPestanas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasTbcPestanas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasTbcPestanas.Location = new System.Drawing.Point(0, 82);
            this.VentasTbcPestanas.Name = "VentasTbcPestanas";
            this.VentasTbcPestanas.SelectedIndex = 0;
            this.VentasTbcPestanas.Size = new System.Drawing.Size(1280, 618);
            this.VentasTbcPestanas.TabIndex = 0;
            this.VentasTbcPestanas.SelectedIndexChanged += new System.EventHandler(this.VentasMetCambiarPestana);
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
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlFechaFin);
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlFechaInicio);
            this.VentasTbpCalculo.Location = new System.Drawing.Point(4, 30);
            this.VentasTbpCalculo.Name = "VentasTbpCalculo";
            this.VentasTbpCalculo.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpCalculo.Size = new System.Drawing.Size(1272, 584);
            this.VentasTbpCalculo.TabIndex = 0;
            this.VentasTbpCalculo.Text = "Calcular comisión";
            //
            // VentasDgvResultados
            //
            this.VentasDgvResultados.AllowUserToAddRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvResultados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.VentasDgvResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasDgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvResultados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.VentasDgvResultados.ColumnHeadersHeight = 29;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvResultados.DefaultCellStyle = dataGridViewCellStyle4;
            this.VentasDgvResultados.EnableHeadersVisualStyles = false;
            this.VentasDgvResultados.Location = new System.Drawing.Point(24, 130);
            this.VentasDgvResultados.MultiSelect = false;
            this.VentasDgvResultados.Name = "VentasDgvResultados";
            this.VentasDgvResultados.ReadOnly = true;
            this.VentasDgvResultados.RowHeadersVisible = false;
            this.VentasDgvResultados.RowHeadersWidth = 51;
            this.VentasDgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvResultados.Size = new System.Drawing.Size(1210, 368);
            this.VentasDgvResultados.TabIndex = 0;
            this.VentasDgvResultados.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VentasMetFormatearMoneda);
            this.VentasDgvResultados.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.VentasMetOrdenarTabla);
            //
            // VentasLblPaginaResultados
            //
            this.VentasLblPaginaResultados.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaResultados.Location = new System.Drawing.Point(486, 532);
            this.VentasLblPaginaResultados.Name = "VentasLblPaginaResultados";
            this.VentasLblPaginaResultados.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaResultados.TabIndex = 12;
            this.VentasLblPaginaResultados.Text = "Página 0 de 0 · 0 registros";
            this.VentasLblPaginaResultados.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VentasLblFechaInicio
            //
            this.VentasLblFechaInicio.AutoSize = true;
            this.VentasLblFechaInicio.Location = new System.Drawing.Point(24, 8);
            this.VentasLblFechaInicio.Name = "VentasLblFechaInicio";
            this.VentasLblFechaInicio.Size = new System.Drawing.Size(101, 21);
            this.VentasLblFechaInicio.TabIndex = 1;
            this.VentasLblFechaInicio.Text = "Fecha inicial";
            //
            // VentasLblFechaFin
            //
            this.VentasLblFechaFin.AutoSize = true;
            this.VentasLblFechaFin.Location = new System.Drawing.Point(200, 8);
            this.VentasLblFechaFin.Name = "VentasLblFechaFin";
            this.VentasLblFechaFin.Size = new System.Drawing.Size(90, 21);
            this.VentasLblFechaFin.TabIndex = 2;
            this.VentasLblFechaFin.Text = "Fecha final";
            //
            // VentasLblEstado
            //
            this.VentasLblEstado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblEstado.Location = new System.Drawing.Point(24, 100);
            this.VentasLblEstado.Name = "VentasLblEstado";
            this.VentasLblEstado.Size = new System.Drawing.Size(1210, 28);
            this.VentasLblEstado.TabIndex = 4;
            this.VentasLblEstado.Text = "Periodo · Método · Base VENTAS";
            this.VentasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // VentasLblObligatorioFechaInicio
            //
            this.VentasLblObligatorioFechaInicio.AutoSize = true;
            this.VentasLblObligatorioFechaInicio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFechaInicio.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFechaInicio.Location = new System.Drawing.Point(120, 8);
            this.VentasLblObligatorioFechaInicio.Name = "VentasLblObligatorioFechaInicio";
            this.VentasLblObligatorioFechaInicio.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioFechaInicio.TabIndex = 13;
            this.VentasLblObligatorioFechaInicio.Text = "*";
            //
            // VentasLblObligatorioFechaFin
            //
            this.VentasLblObligatorioFechaFin.AutoSize = true;
            this.VentasLblObligatorioFechaFin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFechaFin.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFechaFin.Location = new System.Drawing.Point(284, 8);
            this.VentasLblObligatorioFechaFin.Name = "VentasLblObligatorioFechaFin";
            this.VentasLblObligatorioFechaFin.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioFechaFin.TabIndex = 14;
            this.VentasLblObligatorioFechaFin.Text = "*";
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
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasPnlVendedor);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasPnlFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblMetodoFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblVendedorFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioFactura);
            this.VentasTbpAsignarFactura.Controls.Add(this.VentasLblObligatorioVendedor);
            this.VentasTbpAsignarFactura.Location = new System.Drawing.Point(4, 30);
            this.VentasTbpAsignarFactura.Name = "VentasTbpAsignarFactura";
            this.VentasTbpAsignarFactura.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpAsignarFactura.Size = new System.Drawing.Size(1272, 584);
            this.VentasTbpAsignarFactura.TabIndex = 1;
            this.VentasTbpAsignarFactura.Text = "Asignar vendedor a factura";
            this.VentasTbpAsignarFactura.Enter += new System.EventHandler(this.VentasMetCambiarPestana);
            //
            // VentasDgvDetalleFactura
            //
            this.VentasDgvDetalleFactura.AllowUserToAddRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasDgvDetalleFactura.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.VentasDgvDetalleFactura.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasDgvDetalleFactura.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvDetalleFactura.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.VentasDgvDetalleFactura.ColumnHeadersHeight = 29;
            this.VentasDgvDetalleFactura.AutoGenerateColumns = false;
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
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvDetalleFactura.DefaultCellStyle = dataGridViewCellStyle7;
            this.VentasDgvDetalleFactura.EnableHeadersVisualStyles = false;
            this.VentasDgvDetalleFactura.Location = new System.Drawing.Point(24, 208);
            this.VentasDgvDetalleFactura.Name = "VentasDgvDetalleFactura";
            this.VentasDgvDetalleFactura.RowHeadersVisible = false;
            this.VentasDgvDetalleFactura.RowHeadersWidth = 51;
            this.VentasDgvDetalleFactura.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvDetalleFactura.Size = new System.Drawing.Size(1210, 290);
            this.VentasDgvDetalleFactura.TabIndex = 0;
            // VentasClmDetalleProducto
            this.VentasClmDetalleProducto.DataPropertyName = "NombreProducto";
            this.VentasClmDetalleProducto.HeaderText = "Producto";
            this.VentasClmDetalleProducto.MinimumWidth = 6;
            this.VentasClmDetalleProducto.Name = "VentasClmDetalleProducto";
            this.VentasClmDetalleProducto.ReadOnly = true;
            // VentasClmDetalleElemento
            this.VentasClmDetalleElemento.DataPropertyName = "ElementoComision";
            this.VentasClmDetalleElemento.HeaderText = "Criterio";
            this.VentasClmDetalleElemento.MinimumWidth = 6;
            this.VentasClmDetalleElemento.Name = "VentasClmDetalleElemento";
            this.VentasClmDetalleElemento.ReadOnly = true;
            // VentasClmMetodoProducto
            this.VentasClmMetodoProducto.DataPropertyName = "MetodoComision";
            this.VentasClmMetodoProducto.HeaderText = "Comisión por";
            this.VentasClmMetodoProducto.MinimumWidth = 95;
            this.VentasClmMetodoProducto.Name = "VentasClmMetodoProducto";
            this.VentasClmMetodoProducto.Items.AddRange(new object[] { "Linea", "Marca", "Categoria", "Producto" });
            this.VentasClmMetodoProducto.ReadOnly = false;
            // VentasClmDetalleCantidad
            this.VentasClmDetalleCantidad.DataPropertyName = "Cantidad";
            this.VentasClmDetalleCantidad.HeaderText = "Cantidad";
            this.VentasClmDetalleCantidad.MinimumWidth = 6;
            this.VentasClmDetalleCantidad.Name = "VentasClmDetalleCantidad";
            this.VentasClmDetalleCantidad.ReadOnly = true;
            this.VentasClmDetalleCantidad.DefaultCellStyle.Format = "N2";
            // VentasClmDetalleBase
            this.VentasClmDetalleBase.DataPropertyName = "BaseVenta";
            this.VentasClmDetalleBase.HeaderText = "Venta";
            this.VentasClmDetalleBase.MinimumWidth = 6;
            this.VentasClmDetalleBase.Name = "VentasClmDetalleBase";
            this.VentasClmDetalleBase.ReadOnly = true;
            // VentasClmDetallePorcentaje
            this.VentasClmDetallePorcentaje.DataPropertyName = "Porcentaje";
            this.VentasClmDetallePorcentaje.HeaderText = "%";
            this.VentasClmDetallePorcentaje.MinimumWidth = 6;
            this.VentasClmDetallePorcentaje.Name = "VentasClmDetallePorcentaje";
            this.VentasClmDetallePorcentaje.ReadOnly = true;
            // VentasClmDetalleComision
            this.VentasClmDetalleComision.DataPropertyName = "ComisionTotal";
            this.VentasClmDetalleComision.HeaderText = "Comisión total";
            this.VentasClmDetalleComision.MinimumWidth = 6;
            this.VentasClmDetalleComision.Name = "VentasClmDetalleComision";
            this.VentasClmDetalleComision.ReadOnly = true;
            // VentasClmDetalleCobrado
            this.VentasClmDetalleCobrado.DataPropertyName = "VentaCobrado";
            this.VentasClmDetalleCobrado.HeaderText = "Venta cobrada";
            this.VentasClmDetalleCobrado.MinimumWidth = 6;
            this.VentasClmDetalleCobrado.Name = "VentasClmDetalleCobrado";
            this.VentasClmDetalleCobrado.ReadOnly = true;
            // VentasClmDetalleAbono
            this.VentasClmDetalleAbono.DataPropertyName = "ComisionUltimoAbono";
            this.VentasClmDetalleAbono.HeaderText = "Comisión último abono";
            this.VentasClmDetalleAbono.MinimumWidth = 6;
            this.VentasClmDetalleAbono.Name = "VentasClmDetalleAbono";
            this.VentasClmDetalleAbono.ReadOnly = true;
            // VentasClmDetalleDisponible
            this.VentasClmDetalleDisponible.DataPropertyName = "ComisionDisponible";
            this.VentasClmDetalleDisponible.HeaderText = "Disponible acumulado";
            this.VentasClmDetalleDisponible.MinimumWidth = 6;
            this.VentasClmDetalleDisponible.Name = "VentasClmDetalleDisponible";
            this.VentasClmDetalleDisponible.ReadOnly = true;
            // VentasClmDetallePendiente
            this.VentasClmDetallePendiente.DataPropertyName = "ComisionPendiente";
            this.VentasClmDetallePendiente.HeaderText = "Pendiente";
            this.VentasClmDetallePendiente.MinimumWidth = 6;
            this.VentasClmDetallePendiente.Name = "VentasClmDetallePendiente";
            this.VentasClmDetallePendiente.ReadOnly = true;
            this.VentasDgvDetalleFactura.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VentasMetFormatearMoneda);
            this.VentasDgvDetalleFactura.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.VentasMetFormatearPorcentajeProducto);
            this.VentasDgvDetalleFactura.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.VentasMetMarcarColumnaObligatoria);
            this.VentasDgvDetalleFactura.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.VentasMetCambiarMetodoProducto);
            this.VentasDgvDetalleFactura.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.VentasMetOrdenarTabla);
            this.VentasDgvDetalleFactura.CurrentCellDirtyStateChanged += new System.EventHandler(this.VentasMetConfirmarEdicionCelda);
            //
            // VentasLblPaginaFactura
            //
            this.VentasLblPaginaFactura.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaFactura.Location = new System.Drawing.Point(486, 532);
            this.VentasLblPaginaFactura.Name = "VentasLblPaginaFactura";
            this.VentasLblPaginaFactura.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaFactura.TabIndex = 12;
            this.VentasLblPaginaFactura.Text = "Página 0 de 0 · 0 registros";
            this.VentasLblPaginaFactura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VentasLblEstadoFactura
            //
            this.VentasLblEstadoFactura.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblEstadoFactura.Location = new System.Drawing.Point(24, 174);
            this.VentasLblEstadoFactura.Name = "VentasLblEstadoFactura";
            this.VentasLblEstadoFactura.Size = new System.Drawing.Size(1210, 40);
            this.VentasLblEstadoFactura.TabIndex = 1;
            this.VentasLblEstadoFactura.Text = "Elija el método para cada producto; use la paginación para revisar toda la factur" +
    "a.";
            this.VentasLblEstadoFactura.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // VentasCboMetodoFactura
            //
            this.VentasCboMetodoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VentasCboMetodoFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasCboMetodoFactura.Items.AddRange(new object[] {
            "Marca",
            "Línea",
            "Producto",
            "Categoría"});
            this.VentasCboMetodoFactura.Location = new System.Drawing.Point(650, 45);
            this.VentasCboMetodoFactura.Name = "VentasCboMetodoFactura";
            this.VentasCboMetodoFactura.Size = new System.Drawing.Size(160, 31);
            this.VentasCboMetodoFactura.TabIndex = 4;
            this.VentasCboMetodoFactura.Visible = false;
            //
            // VentasLblMetodoFactura
            //
            this.VentasLblMetodoFactura.AutoSize = true;
            this.VentasLblMetodoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblMetodoFactura.Location = new System.Drawing.Point(646, 22);
            this.VentasLblMetodoFactura.Name = "VentasLblMetodoFactura";
            this.VentasLblMetodoFactura.Size = new System.Drawing.Size(186, 18);
            this.VentasLblMetodoFactura.TabIndex = 7;
            this.VentasLblMetodoFactura.Text = "Método predeterminado";
            this.VentasLblMetodoFactura.Visible = false;
            //
            // VentasLblVendedorFactura
            //
            this.VentasLblVendedorFactura.AutoSize = true;
            this.VentasLblVendedorFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblVendedorFactura.Location = new System.Drawing.Point(426, 16);
            this.VentasLblVendedorFactura.Name = "VentasLblVendedorFactura";
            this.VentasLblVendedorFactura.Size = new System.Drawing.Size(150, 18);
            this.VentasLblVendedorFactura.TabIndex = 8;
            this.VentasLblVendedorFactura.Text = "Vendedor asignado";
            //
            // VentasLblFactura
            //
            this.VentasLblFactura.AutoSize = true;
            this.VentasLblFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblFactura.Location = new System.Drawing.Point(28, 22);
            this.VentasLblFactura.Name = "VentasLblFactura";
            this.VentasLblFactura.Size = new System.Drawing.Size(122, 18);
            this.VentasLblFactura.TabIndex = 9;
            this.VentasLblFactura.Text = "Venta / factura";
            //
            // VentasLblObligatorioFactura
            //
            this.VentasLblObligatorioFactura.AutoSize = true;
            this.VentasLblObligatorioFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioFactura.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioFactura.Location = new System.Drawing.Point(141, 22);
            this.VentasLblObligatorioFactura.Name = "VentasLblObligatorioFactura";
            this.VentasLblObligatorioFactura.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioFactura.TabIndex = 13;
            this.VentasLblObligatorioFactura.Text = "*";
            //
            // VentasLblObligatorioVendedor
            //
            this.VentasLblObligatorioVendedor.AutoSize = true;
            this.VentasLblObligatorioVendedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioVendedor.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioVendedor.Location = new System.Drawing.Point(570, 16);
            this.VentasLblObligatorioVendedor.Name = "VentasLblObligatorioVendedor";
            this.VentasLblObligatorioVendedor.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioVendedor.TabIndex = 14;
            this.VentasLblObligatorioVendedor.Text = "*";
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
            this.VentasTbpMantenimiento.Controls.Add(this.VentasPnlPorcentaje);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasPnlAlcance);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasPnlElementoPorcentaje);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblElementoPorcentaje);
            this.VentasTbpMantenimiento.Controls.Add(this.VentasLblObligatorioElementoPorcentaje);
            this.VentasTbpMantenimiento.Location = new System.Drawing.Point(4, 30);
            this.VentasTbpMantenimiento.Name = "VentasTbpMantenimiento";
            this.VentasTbpMantenimiento.Padding = new System.Windows.Forms.Padding(16);
            this.VentasTbpMantenimiento.Size = new System.Drawing.Size(1272, 584);
            this.VentasTbpMantenimiento.TabIndex = 2;
            this.VentasTbpMantenimiento.Text = "Mantenimiento de porcentajes";
            //
            // VentasDgvPorcentajes
            //
            this.VentasDgvPorcentajes.AllowUserToAddRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasDgvPorcentajes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.VentasDgvPorcentajes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasDgvPorcentajes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.VentasDgvPorcentajes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.VentasDgvPorcentajes.ColumnHeadersHeight = 29;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 10F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.VentasDgvPorcentajes.DefaultCellStyle = dataGridViewCellStyle10;
            this.VentasDgvPorcentajes.EnableHeadersVisualStyles = false;
            this.VentasDgvPorcentajes.Location = new System.Drawing.Point(24, 130);
            this.VentasDgvPorcentajes.MultiSelect = false;
            this.VentasDgvPorcentajes.Name = "VentasDgvPorcentajes";
            this.VentasDgvPorcentajes.ReadOnly = true;
            this.VentasDgvPorcentajes.RowHeadersVisible = false;
            this.VentasDgvPorcentajes.RowHeadersWidth = 51;
            this.VentasDgvPorcentajes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvPorcentajes.Size = new System.Drawing.Size(1210, 368);
            this.VentasDgvPorcentajes.TabIndex = 0;
            this.VentasDgvPorcentajes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.VentasMetOrdenarTabla);
            //
            // VentasLblPaginaPorcentajes
            //
            this.VentasLblPaginaPorcentajes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VentasLblPaginaPorcentajes.Location = new System.Drawing.Point(486, 532);
            this.VentasLblPaginaPorcentajes.Name = "VentasLblPaginaPorcentajes";
            this.VentasLblPaginaPorcentajes.Size = new System.Drawing.Size(300, 20);
            this.VentasLblPaginaPorcentajes.TabIndex = 10;
            this.VentasLblPaginaPorcentajes.Text = "Página 0 de 0 · 0 registros";
            this.VentasLblPaginaPorcentajes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VentasLblAlcance
            //
            this.VentasLblAlcance.AutoSize = true;
            this.VentasLblAlcance.Location = new System.Drawing.Point(24, 7);
            this.VentasLblAlcance.Name = "VentasLblAlcance";
            this.VentasLblAlcance.Size = new System.Drawing.Size(149, 21);
            this.VentasLblAlcance.TabIndex = 1;
            this.VentasLblAlcance.Text = "Tipo de porcentaje";
            //
            // VentasLblPorcentajeNuevo
            //
            this.VentasLblPorcentajeNuevo.Location = new System.Drawing.Point(420, 7);
            this.VentasLblPorcentajeNuevo.Name = "VentasLblPorcentajeNuevo";
            this.VentasLblPorcentajeNuevo.Size = new System.Drawing.Size(150, 23);
            this.VentasLblPorcentajeNuevo.TabIndex = 2;
            this.VentasLblPorcentajeNuevo.Text = "Porcentaje nuevo";
            //
            // VentasLblMantenimientoEstado
            //
            this.VentasLblMantenimientoEstado.Location = new System.Drawing.Point(24, 100);
            this.VentasLblMantenimientoEstado.Name = "VentasLblMantenimientoEstado";
            this.VentasLblMantenimientoEstado.Size = new System.Drawing.Size(1210, 28);
            this.VentasLblMantenimientoEstado.TabIndex = 3;
            this.VentasLblMantenimientoEstado.Text = "La tabla muestra las 4 tasas por producto. Seleccione producto(s), elija alcance " +
    "y nuevo %; Marca, Línea o Categoría actualiza la clasificación compartida.";
            //
            // VentasLblObligatorioAlcance
            //
            this.VentasLblObligatorioAlcance.AutoSize = true;
            this.VentasLblObligatorioAlcance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioAlcance.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioAlcance.Location = new System.Drawing.Point(154, 7);
            this.VentasLblObligatorioAlcance.Name = "VentasLblObligatorioAlcance";
            this.VentasLblObligatorioAlcance.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioAlcance.TabIndex = 11;
            this.VentasLblObligatorioAlcance.Text = "*";
            //
            // VentasLblObligatorioPorcentaje
            //
            this.VentasLblObligatorioPorcentaje.AutoSize = true;
            this.VentasLblObligatorioPorcentaje.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblObligatorioPorcentaje.ForeColor = System.Drawing.Color.Firebrick;
            this.VentasLblObligatorioPorcentaje.Location = new System.Drawing.Point(557, 7);
            this.VentasLblObligatorioPorcentaje.Name = "VentasLblObligatorioPorcentaje";
            this.VentasLblObligatorioPorcentaje.Size = new System.Drawing.Size(18, 18);
            this.VentasLblObligatorioPorcentaje.TabIndex = 12;
            this.VentasLblObligatorioPorcentaje.Text = "*";
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
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Sans Unicode", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasLblTitulo.Location = new System.Drawing.Point(28, 13);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(399, 37);
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
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(417, 21);
            this.VentasLblSubtitulo.TabIndex = 1;
            this.VentasLblSubtitulo.Text = "Cálculo de comisiones y mantenimiento de porcentajes";
            //
            // FrmComisionesVendedoresVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.VentasTbcPestanas);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FrmComisionesVendedoresVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7008 – ComisionVendedores";
            this.Load += new System.EventHandler(this.VentasMetCargarFormulario);
            this.VentasTbpNomina.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvNomina)).EndInit();
            this.VentasPnlFechaInicio.ResumeLayout(false);
            this.VentasPnlFechaFin.ResumeLayout(false);
            this.VentasPnlFactura.ResumeLayout(false);
            this.VentasPnlVendedor.ResumeLayout(false);
            this.VentasPnlAlcance.ResumeLayout(false);
            this.VentasPnlPorcentaje.ResumeLayout(false);
            this.VentasPnlPorcentaje.PerformLayout();
            this.VentasPnlElementoPorcentaje.ResumeLayout(false);
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

        private IContainer components;
    }
}
