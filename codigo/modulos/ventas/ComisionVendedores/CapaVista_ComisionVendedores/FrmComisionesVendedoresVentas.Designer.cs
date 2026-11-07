/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Roger
 * Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
 * Encargado: Roger Yankhel de Jesús Herrera Alcántara
 * Fecha de actualización: 15/10/2026
 * Propósito: Definir controles visibles desde el diseñador de Windows Forms.
 */
namespace CapaVista_ComisionVendedores
{
    partial class FrmComisionesVendedoresVentas
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel VentasPnlEncabezado;
        private System.Windows.Forms.PictureBox VentasPbMarca;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblPeriodo;
        private System.Windows.Forms.TabControl VentasTbcComisiones;
        private System.Windows.Forms.TabPage VentasTbpCalculo;
        private System.Windows.Forms.TabPage VentasTbpConsulta;
        private System.Windows.Forms.TabPage VentasTbpAutorizacion;
        private System.Windows.Forms.Label VentasLblDesde;
        private System.Windows.Forms.Label VentasLblHasta;
        private System.Windows.Forms.Label VentasLblVendedor;
        private System.Windows.Forms.Label VentasLblRegla;
        private System.Windows.Forms.TextBox VentasTxtDesde;
        private System.Windows.Forms.TextBox VentasTxtHasta;
        private System.Windows.Forms.TextBox VentasTxtVendedor;
        private System.Windows.Forms.TextBox VentasTxtRegla;
        private System.Windows.Forms.Button VentasBtnCalcular;
        private System.Windows.Forms.Panel VentasPnlTarjetaVentas;
        private System.Windows.Forms.Panel VentasPnlTarjetaMonto;
        private System.Windows.Forms.Panel VentasPnlTarjetaEstado;
        private System.Windows.Forms.Label VentasLblTarjetaVentas;
        private System.Windows.Forms.Label VentasLblTarjetaVentasValor;
        private System.Windows.Forms.Label VentasLblTarjetaVentasDetalle;
        private System.Windows.Forms.Label VentasLblTarjetaMonto;
        private System.Windows.Forms.Label VentasLblTarjetaMontoValor;
        private System.Windows.Forms.Label VentasLblTarjetaMontoDetalle;
        private System.Windows.Forms.Label VentasLblTarjetaEstado;
        private System.Windows.Forms.Label VentasLblTarjetaEstadoValor;
        private System.Windows.Forms.Label VentasLblTarjetaEstadoDetalle;
        private System.Windows.Forms.Label VentasLblVentasElegibles;
        private System.Windows.Forms.Label VentasLblAyudaElegibles;
        private System.Windows.Forms.DataGridView VentasDgvVentas;

        private System.Windows.Forms.Label VentasLblPaginacion;
        private System.Windows.Forms.Button VentasBtnAnterior;
        private System.Windows.Forms.Button VentasBtnSiguiente;
        private System.Windows.Forms.Panel VentasPnlPieCalculo;
        private System.Windows.Forms.Label VentasLblAvisoAutorizacion;
        private System.Windows.Forms.Button VentasBtnSolicitar;
        private System.Windows.Forms.Label VentasLblConsultaVendedor;
        private System.Windows.Forms.Label VentasLblConsultaPeriodo;
        private System.Windows.Forms.Label VentasLblConsultaDocumento;
        private System.Windows.Forms.Label VentasLblConsultaEstado;
        private System.Windows.Forms.TextBox VentasTxtConsultaVendedor;
        private System.Windows.Forms.TextBox VentasTxtConsultaPeriodo;
        private System.Windows.Forms.TextBox VentasTxtConsultaDocumento;
        private System.Windows.Forms.TextBox VentasTxtConsultaEstado;
        private System.Windows.Forms.Button VentasBtnBuscar;
        private System.Windows.Forms.Label VentasLblHistorial;
        private System.Windows.Forms.DataGridView VentasDgvHistorial;

        private System.Windows.Forms.Panel VentasPnlHistorialCantidad;
        private System.Windows.Forms.Panel VentasPnlHistorialMonto;
        private System.Windows.Forms.Panel VentasPnlHistorialSeleccion;
        private System.Windows.Forms.Label VentasLblHistorialCantidad;
        private System.Windows.Forms.Label VentasLblHistorialMonto;
        private System.Windows.Forms.Label VentasLblHistorialSeleccion;
        private System.Windows.Forms.Label VentasLblHistorialCantidadDetalle;
        private System.Windows.Forms.Label VentasLblHistorialMontoDetalle;
        private System.Windows.Forms.Label VentasLblHistorialSeleccionDetalle;
        private System.Windows.Forms.Label VentasLblBandeja;
        private System.Windows.Forms.Label VentasLblAyudaBandeja;
        private System.Windows.Forms.DataGridView VentasDgvAutorizacion;

        private System.Windows.Forms.Panel VentasPnlResumen;
        private System.Windows.Forms.Label VentasLblResumenTitulo;
        private System.Windows.Forms.Label VentasLblResumenVendedor;
        private System.Windows.Forms.Label VentasLblResumenPeriodo;
        private System.Windows.Forms.Label VentasLblResumenVentas;
        private System.Windows.Forms.Label VentasLblResumenMonto;
        private System.Windows.Forms.Label VentasLblComentario;
        private System.Windows.Forms.TextBox VentasTxtComentario;
        private System.Windows.Forms.Button VentasBtnRechazar;
        private System.Windows.Forms.Button VentasBtnAprobar;
        private System.Windows.Forms.Panel VentasPnlPieAutorizacion;
        private System.Windows.Forms.Label VentasLblAvisoEnvio;
        private System.Windows.Forms.Button VentasBtnEnviar;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null)) components.Dispose();
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.VentasTtpAccionesOriginales = new System.Windows.Forms.ToolTip(this.components);
            System.Windows.Forms.DataGridViewCellStyle VentasEstiloCabecera = new System.Windows.Forms.DataGridViewCellStyle();
            this.VentasPnlEncabezado = new System.Windows.Forms.Panel();
            this.VentasPbMarca = new System.Windows.Forms.PictureBox();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasLblPeriodo = new System.Windows.Forms.Label();
            this.VentasTbcComisiones = new System.Windows.Forms.TabControl();
            this.VentasTbpCalculo = new System.Windows.Forms.TabPage();
            this.VentasLblDesde = new System.Windows.Forms.Label();
            this.VentasTxtDesde = new System.Windows.Forms.TextBox();
            this.VentasLblHasta = new System.Windows.Forms.Label();
            this.VentasTxtHasta = new System.Windows.Forms.TextBox();
            this.VentasLblVendedor = new System.Windows.Forms.Label();
            this.VentasTxtVendedor = new System.Windows.Forms.TextBox();
            this.VentasLblRegla = new System.Windows.Forms.Label();
            this.VentasTxtRegla = new System.Windows.Forms.TextBox();
            this.VentasBtnCalcular = new System.Windows.Forms.Button();
            this.VentasPnlTarjetaVentas = new System.Windows.Forms.Panel();
            this.VentasLblTarjetaVentas = new System.Windows.Forms.Label();
            this.VentasLblTarjetaVentasValor = new System.Windows.Forms.Label();
            this.VentasLblTarjetaVentasDetalle = new System.Windows.Forms.Label();
            this.VentasPnlTarjetaMonto = new System.Windows.Forms.Panel();
            this.VentasLblTarjetaMonto = new System.Windows.Forms.Label();
            this.VentasLblTarjetaMontoValor = new System.Windows.Forms.Label();
            this.VentasLblTarjetaMontoDetalle = new System.Windows.Forms.Label();
            this.VentasPnlTarjetaEstado = new System.Windows.Forms.Panel();
            this.VentasLblTarjetaEstado = new System.Windows.Forms.Label();
            this.VentasLblTarjetaEstadoValor = new System.Windows.Forms.Label();
            this.VentasLblTarjetaEstadoDetalle = new System.Windows.Forms.Label();
            this.VentasLblVentasElegibles = new System.Windows.Forms.Label();
            this.VentasLblAyudaElegibles = new System.Windows.Forms.Label();
            this.VentasDgvVentas = new System.Windows.Forms.DataGridView();
            this.VentasClmVentaFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaDocumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaTasa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaImporte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmVentaEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasLblPaginacion = new System.Windows.Forms.Label();
            this.VentasBtnAnterior = new System.Windows.Forms.Button();
            this.VentasBtnSiguiente = new System.Windows.Forms.Button();
            this.VentasPnlPieCalculo = new System.Windows.Forms.Panel();
            this.VentasLblAvisoAutorizacion = new System.Windows.Forms.Label();
            this.VentasBtnSolicitar = new System.Windows.Forms.Button();
            this.VentasTbpConsulta = new System.Windows.Forms.TabPage();
            this.VentasLblConsultaVendedor = new System.Windows.Forms.Label();
            this.VentasTxtConsultaVendedor = new System.Windows.Forms.TextBox();
            this.VentasLblConsultaPeriodo = new System.Windows.Forms.Label();
            this.VentasTxtConsultaPeriodo = new System.Windows.Forms.TextBox();
            this.VentasLblConsultaDocumento = new System.Windows.Forms.Label();
            this.VentasTxtConsultaDocumento = new System.Windows.Forms.TextBox();
            this.VentasLblConsultaEstado = new System.Windows.Forms.Label();
            this.VentasTxtConsultaEstado = new System.Windows.Forms.TextBox();
            this.VentasBtnBuscar = new System.Windows.Forms.Button();
            this.VentasLblHistorial = new System.Windows.Forms.Label();
            this.VentasDgvHistorial = new System.Windows.Forms.DataGridView();
            this.VentasClmHistorialPeriodo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmHistorialVendedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmHistorialVentas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmHistorialImporte = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmHistorialDocumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmHistorialEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasPnlHistorialCantidad = new System.Windows.Forms.Panel();
            this.VentasLblHistorialCantidad = new System.Windows.Forms.Label();
            this.VentasLblHistorialCantidadDetalle = new System.Windows.Forms.Label();
            this.VentasPnlHistorialMonto = new System.Windows.Forms.Panel();
            this.VentasLblHistorialMonto = new System.Windows.Forms.Label();
            this.VentasLblHistorialMontoDetalle = new System.Windows.Forms.Label();
            this.VentasPnlHistorialSeleccion = new System.Windows.Forms.Panel();
            this.VentasLblHistorialSeleccion = new System.Windows.Forms.Label();
            this.VentasLblHistorialSeleccionDetalle = new System.Windows.Forms.Label();
            this.VentasTbpAutorizacion = new System.Windows.Forms.TabPage();
            this.VentasLblBandeja = new System.Windows.Forms.Label();
            this.VentasLblAyudaBandeja = new System.Windows.Forms.Label();
            this.VentasDgvAutorizacion = new System.Windows.Forms.DataGridView();
            this.VentasClmAutorizacionPeriodo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAutorizacionVendedor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAutorizacionVentas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAutorizacionMonto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAutorizacionSolicitada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasClmAutorizacionEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VentasPnlResumen = new System.Windows.Forms.Panel();
            this.VentasLblResumenTitulo = new System.Windows.Forms.Label();
            this.VentasLblResumenVendedor = new System.Windows.Forms.Label();
            this.VentasLblResumenPeriodo = new System.Windows.Forms.Label();
            this.VentasLblResumenVentas = new System.Windows.Forms.Label();
            this.VentasLblResumenMonto = new System.Windows.Forms.Label();
            this.VentasLblComentario = new System.Windows.Forms.Label();
            this.VentasTxtComentario = new System.Windows.Forms.TextBox();
            this.VentasBtnRechazar = new System.Windows.Forms.Button();
            this.VentasBtnAprobar = new System.Windows.Forms.Button();
            this.VentasPnlPieAutorizacion = new System.Windows.Forms.Panel();
            this.VentasLblAvisoEnvio = new System.Windows.Forms.Label();
            this.VentasBtnEnviar = new System.Windows.Forms.Button();
            this.VentasPnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasPbMarca)).BeginInit();
            this.VentasTbcComisiones.SuspendLayout();
            this.VentasTbpCalculo.SuspendLayout();
            this.VentasPnlTarjetaVentas.SuspendLayout();
            this.VentasPnlTarjetaMonto.SuspendLayout();
            this.VentasPnlTarjetaEstado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvVentas)).BeginInit();
            this.VentasPnlPieCalculo.SuspendLayout();
            this.VentasTbpConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvHistorial)).BeginInit();
            this.VentasPnlHistorialCantidad.SuspendLayout();
            this.VentasPnlHistorialMonto.SuspendLayout();
            this.VentasPnlHistorialSeleccion.SuspendLayout();
            this.VentasTbpAutorizacion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvAutorizacion)).BeginInit();
            this.VentasPnlResumen.SuspendLayout();
            this.VentasPnlPieAutorizacion.SuspendLayout();
            this.SuspendLayout();
            //
            // VentasPnlEncabezado
            //
            this.VentasPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasPnlEncabezado.Controls.Add(this.VentasPbMarca);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblPeriodo);
            this.VentasPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Size = new System.Drawing.Size(1060, 82);
            this.VentasPnlEncabezado.TabIndex = 0;
            //
            // VentasPbMarca
            //
            this.VentasPbMarca.Image = global::CapaVista_ComisionVendedores.Properties.Resources.IconoVentasImagen;
            this.VentasPbMarca.Location = new System.Drawing.Point(20, 14);
            this.VentasPbMarca.Name = "VentasPbMarca";
            this.VentasPbMarca.Size = new System.Drawing.Size(52, 52);
            this.VentasPbMarca.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.VentasPbMarca.TabIndex = 0;
            this.VentasPbMarca.TabStop = false;
            //
            // VentasLblTitulo
            //
            this.VentasLblTitulo.AutoSize = true;
            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.White;
            this.VentasLblTitulo.Location = new System.Drawing.Point(86, 14);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Size = new System.Drawing.Size(467, 33);
            this.VentasLblTitulo.TabIndex = 1;
            this.VentasLblTitulo.Text = "COMISIONES DE VENDEDORES";
            //
            // VentasLblSubtitulo
            //
            this.VentasLblSubtitulo.AutoSize = true;
            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(212)))), ((int)(((byte)(234)))));
            this.VentasLblSubtitulo.Location = new System.Drawing.Point(88, 45);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Size = new System.Drawing.Size(330, 21);
            this.VentasLblSubtitulo.TabIndex = 2;
            this.VentasLblSubtitulo.Text = "Cálculo, consulta y autorización de comisiones";
            //
            // VentasLblPeriodo
            //
            this.VentasLblPeriodo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasLblPeriodo.AutoSize = true;
            this.VentasLblPeriodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblPeriodo.ForeColor = System.Drawing.Color.White;
            this.VentasLblPeriodo.Location = new System.Drawing.Point(827, 31);
            this.VentasLblPeriodo.Name = "VentasLblPeriodo";
            this.VentasLblPeriodo.Size = new System.Drawing.Size(217, 18);
            this.VentasLblPeriodo.TabIndex = 3;
            this.VentasLblPeriodo.Text = "PERIODO  ·  OCTUBRE 2026";
            //
            // VentasTbcComisiones
            //
            this.VentasTbcComisiones.Controls.Add(this.VentasTbpCalculo);
            this.VentasTbcComisiones.Controls.Add(this.VentasTbpConsulta);
            this.VentasTbcComisiones.Controls.Add(this.VentasTbpAutorizacion);
            this.VentasTbcComisiones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VentasTbcComisiones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTbcComisiones.Location = new System.Drawing.Point(0, 82);
            this.VentasTbcComisiones.Name = "VentasTbcComisiones";
            this.VentasTbcComisiones.Padding = new System.Drawing.Point(18, 8);
            this.VentasTbcComisiones.SelectedIndex = 0;
            this.VentasTbcComisiones.Size = new System.Drawing.Size(1060, 578);
            this.VentasTbcComisiones.TabIndex = 1;
            //
            // VentasTbpCalculo
            //
            this.VentasTbpCalculo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpCalculo.Controls.Add(this.VentasLblDesde);
            this.VentasTbpCalculo.Controls.Add(this.VentasTxtDesde);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblHasta);
            this.VentasTbpCalculo.Controls.Add(this.VentasTxtHasta);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblVendedor);
            this.VentasTbpCalculo.Controls.Add(this.VentasTxtVendedor);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblRegla);
            this.VentasTbpCalculo.Controls.Add(this.VentasTxtRegla);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnCalcular);
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlTarjetaVentas);
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlTarjetaMonto);
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlTarjetaEstado);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblVentasElegibles);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblAyudaElegibles);
            this.VentasTbpCalculo.Controls.Add(this.VentasDgvVentas);
            this.VentasTbpCalculo.Controls.Add(this.VentasLblPaginacion);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnAnterior);
            this.VentasTbpCalculo.Controls.Add(this.VentasBtnSiguiente);
            this.VentasTbpCalculo.Controls.Add(this.VentasPnlPieCalculo);
            this.VentasTbpCalculo.Location = new System.Drawing.Point(4, 42);
            this.VentasTbpCalculo.Name = "VentasTbpCalculo";
            this.VentasTbpCalculo.Padding = new System.Windows.Forms.Padding(10);
            this.VentasTbpCalculo.Size = new System.Drawing.Size(1052, 532);
            this.VentasTbpCalculo.TabIndex = 0;
            this.VentasTbpCalculo.Text = "Calcular comisión";
            //
            // VentasLblDesde
            //
            this.VentasLblDesde.AutoSize = true;
            this.VentasLblDesde.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblDesde.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblDesde.Location = new System.Drawing.Point(24, 18);
            this.VentasLblDesde.Name = "VentasLblDesde";
            this.VentasLblDesde.Size = new System.Drawing.Size(111, 18);
            this.VentasLblDesde.TabIndex = 0;
            this.VentasLblDesde.Text = "Periodo · Desde";
            //
            // VentasTxtDesde
            //
            this.VentasTxtDesde.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtDesde.Location = new System.Drawing.Point(24, 42);
            this.VentasTxtDesde.Name = "VentasTxtDesde";
            this.VentasTxtDesde.ReadOnly = true;
            this.VentasTxtDesde.Size = new System.Drawing.Size(145, 30);
            this.VentasTxtDesde.TabIndex = 1;
            this.VentasTxtDesde.Text = "01/10/2026";
            //
            // VentasLblHasta
            //
            this.VentasLblHasta.AutoSize = true;
            this.VentasLblHasta.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblHasta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblHasta.Location = new System.Drawing.Point(180, 18);
            this.VentasLblHasta.Name = "VentasLblHasta";
            this.VentasLblHasta.Size = new System.Drawing.Size(46, 18);
            this.VentasLblHasta.TabIndex = 2;
            this.VentasLblHasta.Text = "Hasta";
            //
            // VentasTxtHasta
            //
            this.VentasTxtHasta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtHasta.Location = new System.Drawing.Point(180, 42);
            this.VentasTxtHasta.Name = "VentasTxtHasta";
            this.VentasTxtHasta.ReadOnly = true;
            this.VentasTxtHasta.Size = new System.Drawing.Size(145, 30);
            this.VentasTxtHasta.TabIndex = 3;
            this.VentasTxtHasta.Text = "31/10/2026";
            //
            // VentasLblVendedor
            //
            this.VentasLblVendedor.AutoSize = true;
            this.VentasLblVendedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblVendedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblVendedor.Location = new System.Drawing.Point(337, 18);
            this.VentasLblVendedor.Name = "VentasLblVendedor";
            this.VentasLblVendedor.Size = new System.Drawing.Size(70, 18);
            this.VentasLblVendedor.TabIndex = 4;
            this.VentasLblVendedor.Text = "Vendedor";
            //
            // VentasTxtVendedor
            //
            this.VentasTxtVendedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtVendedor.Location = new System.Drawing.Point(337, 42);
            this.VentasTxtVendedor.Name = "VentasTxtVendedor";
            this.VentasTxtVendedor.ReadOnly = true;
            this.VentasTxtVendedor.Size = new System.Drawing.Size(220, 30);
            this.VentasTxtVendedor.TabIndex = 5;
            this.VentasTxtVendedor.Text = "Todos los vendedores  ▾";
            //
            // VentasLblRegla
            //
            this.VentasLblRegla.AutoSize = true;
            this.VentasLblRegla.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblRegla.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblRegla.Location = new System.Drawing.Point(569, 18);
            this.VentasLblRegla.Name = "VentasLblRegla";
            this.VentasLblRegla.Size = new System.Drawing.Size(124, 18);
            this.VentasLblRegla.TabIndex = 6;
            this.VentasLblRegla.Text = "Regla de comisión";
            //
            // VentasTxtRegla
            //
            this.VentasTxtRegla.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtRegla.Location = new System.Drawing.Point(569, 42);
            this.VentasTxtRegla.Name = "VentasTxtRegla";
            this.VentasTxtRegla.ReadOnly = true;
            this.VentasTxtRegla.Size = new System.Drawing.Size(220, 30);
            this.VentasTxtRegla.TabIndex = 7;
            this.VentasTxtRegla.Text = "Comisión estándar  ▾";
            //
            // VentasBtnCalcular
            //

            this.VentasBtnCalcular.Location = new System.Drawing.Point(950, 17);
            this.VentasBtnCalcular.Name = "VentasBtnCalcular";
            this.VentasBtnCalcular.AccessibleDescription = "Calcular comisión";
            this.VentasBtnCalcular.AccessibleName = "Calcular comisión";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnCalcular.Text = string.Empty;
            this.VentasBtnCalcular.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnCalcular.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnCalcular.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalConsultarVentas;
            this.VentasBtnCalcular.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnCalcular.Image = null;
            this.VentasBtnCalcular.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnCalcular.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnCalcular.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnCalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnCalcular.FlatAppearance.BorderSize = 0;
            this.VentasBtnCalcular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnCalcular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnCalcular.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnCalcular.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnCalcular.UseVisualStyleBackColor = false;
            this.VentasBtnCalcular.UseCompatibleTextRendering = false;
            this.VentasBtnCalcular.Size = new System.Drawing.Size(58, 64);
            this.VentasBtnCalcular.TabIndex = 8;

            //
            // VentasPnlTarjetaVentas
            //
            this.VentasPnlTarjetaVentas.BackColor = System.Drawing.Color.White;
            this.VentasPnlTarjetaVentas.Controls.Add(this.VentasLblTarjetaVentas);
            this.VentasPnlTarjetaVentas.Controls.Add(this.VentasLblTarjetaVentasValor);
            this.VentasPnlTarjetaVentas.Controls.Add(this.VentasLblTarjetaVentasDetalle);
            this.VentasPnlTarjetaVentas.Location = new System.Drawing.Point(24, 104);
            this.VentasPnlTarjetaVentas.Name = "VentasPnlTarjetaVentas";
            this.VentasPnlTarjetaVentas.Size = new System.Drawing.Size(313, 76);
            this.VentasPnlTarjetaVentas.TabIndex = 9;
            //
            // VentasLblTarjetaVentas
            //
            this.VentasLblTarjetaVentas.AutoSize = true;
            this.VentasLblTarjetaVentas.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaVentas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasLblTarjetaVentas.Location = new System.Drawing.Point(14, 10);
            this.VentasLblTarjetaVentas.Name = "VentasLblTarjetaVentas";
            this.VentasLblTarjetaVentas.Size = new System.Drawing.Size(149, 17);
            this.VentasLblTarjetaVentas.TabIndex = 0;
            this.VentasLblTarjetaVentas.Text = "VENTAS ELEGIBLES";
            //
            // VentasLblTarjetaVentasValor
            //
            this.VentasLblTarjetaVentasValor.AutoSize = true;
            this.VentasLblTarjetaVentasValor.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaVentasValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTarjetaVentasValor.Location = new System.Drawing.Point(14, 29);
            this.VentasLblTarjetaVentasValor.Name = "VentasLblTarjetaVentasValor";
            this.VentasLblTarjetaVentasValor.Size = new System.Drawing.Size(155, 27);
            this.VentasLblTarjetaVentasValor.TabIndex = 1;
            this.VentasLblTarjetaVentasValor.Text = "Q 48,750.00";
            //
            // VentasLblTarjetaVentasDetalle
            //
            this.VentasLblTarjetaVentasDetalle.AutoSize = true;
            this.VentasLblTarjetaVentasDetalle.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.VentasLblTarjetaVentasDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblTarjetaVentasDetalle.Location = new System.Drawing.Point(15, 54);
            this.VentasLblTarjetaVentasDetalle.Name = "VentasLblTarjetaVentasDetalle";
            this.VentasLblTarjetaVentasDetalle.Size = new System.Drawing.Size(201, 20);
            this.VentasLblTarjetaVentasDetalle.TabIndex = 2;
            this.VentasLblTarjetaVentasDetalle.Text = "12 ventas dentro del periodo";
            //
            // VentasPnlTarjetaMonto
            //
            this.VentasPnlTarjetaMonto.BackColor = System.Drawing.Color.White;
            this.VentasPnlTarjetaMonto.Controls.Add(this.VentasLblTarjetaMonto);
            this.VentasPnlTarjetaMonto.Controls.Add(this.VentasLblTarjetaMontoValor);
            this.VentasPnlTarjetaMonto.Controls.Add(this.VentasLblTarjetaMontoDetalle);
            this.VentasPnlTarjetaMonto.Location = new System.Drawing.Point(349, 104);
            this.VentasPnlTarjetaMonto.Name = "VentasPnlTarjetaMonto";
            this.VentasPnlTarjetaMonto.Size = new System.Drawing.Size(313, 76);
            this.VentasPnlTarjetaMonto.TabIndex = 10;
            //
            // VentasLblTarjetaMonto
            //
            this.VentasLblTarjetaMonto.AutoSize = true;
            this.VentasLblTarjetaMonto.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaMonto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasLblTarjetaMonto.Location = new System.Drawing.Point(14, 10);
            this.VentasLblTarjetaMonto.Name = "VentasLblTarjetaMonto";
            this.VentasLblTarjetaMonto.Size = new System.Drawing.Size(166, 17);
            this.VentasLblTarjetaMonto.TabIndex = 0;
            this.VentasLblTarjetaMonto.Text = "COMISIÓN ESTIMADA";
            //
            // VentasLblTarjetaMontoValor
            //
            this.VentasLblTarjetaMontoValor.AutoSize = true;
            this.VentasLblTarjetaMontoValor.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaMontoValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTarjetaMontoValor.Location = new System.Drawing.Point(14, 29);
            this.VentasLblTarjetaMontoValor.Name = "VentasLblTarjetaMontoValor";
            this.VentasLblTarjetaMontoValor.Size = new System.Drawing.Size(140, 27);
            this.VentasLblTarjetaMontoValor.TabIndex = 1;
            this.VentasLblTarjetaMontoValor.Text = "Q 2,437.50";
            //
            // VentasLblTarjetaMontoDetalle
            //
            this.VentasLblTarjetaMontoDetalle.AutoSize = true;
            this.VentasLblTarjetaMontoDetalle.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.VentasLblTarjetaMontoDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblTarjetaMontoDetalle.Location = new System.Drawing.Point(15, 54);
            this.VentasLblTarjetaMontoDetalle.Name = "VentasLblTarjetaMontoDetalle";
            this.VentasLblTarjetaMontoDetalle.Size = new System.Drawing.Size(144, 20);
            this.VentasLblTarjetaMontoDetalle.TabIndex = 2;
            this.VentasLblTarjetaMontoDetalle.Text = "Tasa aplicada: 5.00%";
            //
            // VentasPnlTarjetaEstado
            //
            this.VentasPnlTarjetaEstado.BackColor = System.Drawing.Color.White;
            this.VentasPnlTarjetaEstado.Controls.Add(this.VentasLblTarjetaEstado);
            this.VentasPnlTarjetaEstado.Controls.Add(this.VentasLblTarjetaEstadoValor);
            this.VentasPnlTarjetaEstado.Controls.Add(this.VentasLblTarjetaEstadoDetalle);
            this.VentasPnlTarjetaEstado.Location = new System.Drawing.Point(674, 104);
            this.VentasPnlTarjetaEstado.Name = "VentasPnlTarjetaEstado";
            this.VentasPnlTarjetaEstado.Size = new System.Drawing.Size(348, 76);
            this.VentasPnlTarjetaEstado.TabIndex = 11;
            //
            // VentasLblTarjetaEstado
            //
            this.VentasLblTarjetaEstado.AutoSize = true;
            this.VentasLblTarjetaEstado.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasLblTarjetaEstado.Location = new System.Drawing.Point(14, 10);
            this.VentasLblTarjetaEstado.Name = "VentasLblTarjetaEstado";
            this.VentasLblTarjetaEstado.Size = new System.Drawing.Size(168, 17);
            this.VentasLblTarjetaEstado.TabIndex = 0;
            this.VentasLblTarjetaEstado.Text = "ESTADO DEL PERIODO";
            //
            // VentasLblTarjetaEstadoValor
            //
            this.VentasLblTarjetaEstadoValor.AutoSize = true;
            this.VentasLblTarjetaEstadoValor.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.VentasLblTarjetaEstadoValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblTarjetaEstadoValor.Location = new System.Drawing.Point(14, 29);
            this.VentasLblTarjetaEstadoValor.Name = "VentasLblTarjetaEstadoValor";
            this.VentasLblTarjetaEstadoValor.Size = new System.Drawing.Size(118, 27);
            this.VentasLblTarjetaEstadoValor.TabIndex = 1;
            this.VentasLblTarjetaEstadoValor.Text = "Borrador";
            //
            // VentasLblTarjetaEstadoDetalle
            //
            this.VentasLblTarjetaEstadoDetalle.AutoSize = true;
            this.VentasLblTarjetaEstadoDetalle.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.VentasLblTarjetaEstadoDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblTarjetaEstadoDetalle.Location = new System.Drawing.Point(15, 54);
            this.VentasLblTarjetaEstadoDetalle.Name = "VentasLblTarjetaEstadoDetalle";
            this.VentasLblTarjetaEstadoDetalle.Size = new System.Drawing.Size(231, 20);
            this.VentasLblTarjetaEstadoDetalle.TabIndex = 2;
            this.VentasLblTarjetaEstadoDetalle.Text = "Pendiente de solicitar aprobación";
            //
            // VentasLblVentasElegibles
            //
            this.VentasLblVentasElegibles.AutoSize = true;
            this.VentasLblVentasElegibles.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.VentasLblVentasElegibles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblVentasElegibles.Location = new System.Drawing.Point(24, 194);
            this.VentasLblVentasElegibles.Name = "VentasLblVentasElegibles";
            this.VentasLblVentasElegibles.Size = new System.Drawing.Size(183, 23);
            this.VentasLblVentasElegibles.TabIndex = 12;
            this.VentasLblVentasElegibles.Text = "Ventas elegibles";
            //
            // VentasLblAyudaElegibles
            //
            this.VentasLblAyudaElegibles.AutoSize = true;
            this.VentasLblAyudaElegibles.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblAyudaElegibles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblAyudaElegibles.Location = new System.Drawing.Point(24, 220);
            this.VentasLblAyudaElegibles.Name = "VentasLblAyudaElegibles";
            this.VentasLblAyudaElegibles.Size = new System.Drawing.Size(424, 20);
            this.VentasLblAyudaElegibles.TabIndex = 13;
            this.VentasLblAyudaElegibles.Text = "Las ventas anuladas o fuera de vigencia se excluyen de la base.";
            //
            // VentasDgvVentas
            //
            this.VentasDgvVentas.AllowUserToAddRows = false;
            this.VentasDgvVentas.AllowUserToDeleteRows = false;
            this.VentasDgvVentas.AllowUserToResizeRows = false;
            this.VentasDgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvVentas.BackgroundColor = System.Drawing.Color.White;
            this.VentasDgvVentas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            VentasEstiloCabecera.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            VentasEstiloCabecera.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            VentasEstiloCabecera.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            VentasEstiloCabecera.ForeColor = System.Drawing.Color.White;
            VentasEstiloCabecera.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            VentasEstiloCabecera.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            VentasEstiloCabecera.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.VentasDgvVentas.ColumnHeadersDefaultCellStyle = VentasEstiloCabecera;
            this.VentasDgvVentas.ColumnHeadersHeight = 38;
            this.VentasDgvVentas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmVentaFecha,
            this.VentasClmVentaDocumento,
            this.VentasClmVentaCliente,
            this.VentasClmVentaBase,
            this.VentasClmVentaTasa,
            this.VentasClmVentaImporte,
            this.VentasClmVentaEstado});
            this.VentasDgvVentas.EnableHeadersVisualStyles = false;
            this.VentasDgvVentas.Location = new System.Drawing.Point(34, 243);
            this.VentasDgvVentas.Name = "VentasDgvVentas";
            this.VentasDgvVentas.ReadOnly = true;
            this.VentasDgvVentas.RowHeadersVisible = false;
            this.VentasDgvVentas.RowHeadersWidth = 51;
            this.VentasDgvVentas.RowTemplate.Height = 38;
            this.VentasDgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.VentasDgvVentas.Size = new System.Drawing.Size(971, 131);
            this.VentasDgvVentas.TabIndex = 12;
            //
            // VentasClmVentaFecha
            //
            this.VentasClmVentaFecha.HeaderText = "Fecha";
            this.VentasClmVentaFecha.MinimumWidth = 6;
            this.VentasClmVentaFecha.Name = "VentasClmVentaFecha";
            this.VentasClmVentaFecha.ReadOnly = true;
            //
            // VentasClmVentaDocumento
            //
            this.VentasClmVentaDocumento.HeaderText = "Documento";
            this.VentasClmVentaDocumento.MinimumWidth = 6;
            this.VentasClmVentaDocumento.Name = "VentasClmVentaDocumento";
            this.VentasClmVentaDocumento.ReadOnly = true;
            //
            // VentasClmVentaCliente
            //
            this.VentasClmVentaCliente.HeaderText = "Cliente";
            this.VentasClmVentaCliente.MinimumWidth = 6;
            this.VentasClmVentaCliente.Name = "VentasClmVentaCliente";
            this.VentasClmVentaCliente.ReadOnly = true;
            //
            // VentasClmVentaBase
            //
            this.VentasClmVentaBase.HeaderText = "Base de comisión";
            this.VentasClmVentaBase.MinimumWidth = 6;
            this.VentasClmVentaBase.Name = "VentasClmVentaBase";
            this.VentasClmVentaBase.ReadOnly = true;
            //
            // VentasClmVentaTasa
            //
            this.VentasClmVentaTasa.HeaderText = "Tasa";
            this.VentasClmVentaTasa.MinimumWidth = 6;
            this.VentasClmVentaTasa.Name = "VentasClmVentaTasa";
            this.VentasClmVentaTasa.ReadOnly = true;
            //
            // VentasClmVentaImporte
            //
            this.VentasClmVentaImporte.HeaderText = "Comisión";
            this.VentasClmVentaImporte.MinimumWidth = 6;
            this.VentasClmVentaImporte.Name = "VentasClmVentaImporte";
            this.VentasClmVentaImporte.ReadOnly = true;
            //
            // VentasClmVentaEstado
            //
            this.VentasClmVentaEstado.HeaderText = "Estado";
            this.VentasClmVentaEstado.MinimumWidth = 6;
            this.VentasClmVentaEstado.Name = "VentasClmVentaEstado";
            this.VentasClmVentaEstado.ReadOnly = true;
            //
            // VentasLblPaginacion
            //
            this.VentasLblPaginacion.AutoSize = true;
            this.VentasLblPaginacion.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.VentasLblPaginacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblPaginacion.Location = new System.Drawing.Point(24, 420);
            this.VentasLblPaginacion.Name = "VentasLblPaginacion";
            this.VentasLblPaginacion.Size = new System.Drawing.Size(244, 20);
            this.VentasLblPaginacion.TabIndex = 14;
            this.VentasLblPaginacion.Text = "Mostrando 3 de 12 ventas elegibles";
            //
            // VentasBtnAnterior
            //

            this.VentasBtnAnterior.Location = new System.Drawing.Point(770, 380);
            this.VentasBtnAnterior.Name = "VentasBtnAnterior";
            this.VentasBtnAnterior.AccessibleDescription = "Anterior";
            this.VentasBtnAnterior.AccessibleName = "Anterior";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnAnterior.Text = string.Empty;
            this.VentasBtnAnterior.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnAnterior.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnAnterior.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalAnteriorVentas;
            this.VentasBtnAnterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAnterior.Image = null;
            this.VentasBtnAnterior.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAnterior.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAnterior.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAnterior.FlatAppearance.BorderSize = 0;
            this.VentasBtnAnterior.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAnterior.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAnterior.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnAnterior.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnAnterior.UseVisualStyleBackColor = false;
            this.VentasBtnAnterior.UseCompatibleTextRendering = false;
            this.VentasBtnAnterior.Size = new System.Drawing.Size(61, 64);
            this.VentasBtnAnterior.TabIndex = 15;

            //
            // VentasBtnSiguiente
            //

            this.VentasBtnSiguiente.Location = new System.Drawing.Point(897, 380);
            this.VentasBtnSiguiente.Name = "VentasBtnSiguiente";
            this.VentasBtnSiguiente.AccessibleDescription = "Siguiente";
            this.VentasBtnSiguiente.AccessibleName = "Siguiente";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnSiguiente.Text = string.Empty;
            this.VentasBtnSiguiente.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnSiguiente.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnSiguiente.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalSiguienteVentas;
            this.VentasBtnSiguiente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSiguiente.Image = null;
            this.VentasBtnSiguiente.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnSiguiente.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnSiguiente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSiguiente.FlatAppearance.BorderSize = 0;
            this.VentasBtnSiguiente.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnSiguiente.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnSiguiente.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnSiguiente.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnSiguiente.UseVisualStyleBackColor = false;
            this.VentasBtnSiguiente.UseCompatibleTextRendering = false;
            this.VentasBtnSiguiente.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnSiguiente.TabIndex = 16;

            //
            // VentasPnlPieCalculo
            //
            this.VentasPnlPieCalculo.BackColor = System.Drawing.Color.White;
            this.VentasPnlPieCalculo.Controls.Add(this.VentasLblAvisoAutorizacion);
            this.VentasPnlPieCalculo.Controls.Add(this.VentasBtnSolicitar);
            this.VentasPnlPieCalculo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.VentasPnlPieCalculo.Location = new System.Drawing.Point(10, 462);
            this.VentasPnlPieCalculo.Name = "VentasPnlPieCalculo";
            this.VentasPnlPieCalculo.Size = new System.Drawing.Size(1032, 72);
            this.VentasPnlPieCalculo.TabIndex = 13;
            //
            // VentasLblAvisoAutorizacion
            //
            this.VentasLblAvisoAutorizacion.AutoSize = true;
            this.VentasLblAvisoAutorizacion.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblAvisoAutorizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblAvisoAutorizacion.Location = new System.Drawing.Point(20, 23);
            this.VentasLblAvisoAutorizacion.Name = "VentasLblAvisoAutorizacion";
            this.VentasLblAvisoAutorizacion.Size = new System.Drawing.Size(523, 20);
            this.VentasLblAvisoAutorizacion.TabIndex = 0;
            this.VentasLblAvisoAutorizacion.Text = "La comisión requiere autorización antes de enviarse a Contabilidad / Nómina.";
            //
            // VentasBtnSolicitar
            //

            this.VentasBtnSolicitar.Location = new System.Drawing.Point(952, 4);
            this.VentasBtnSolicitar.Name = "VentasBtnSolicitar";
            this.VentasBtnSolicitar.AccessibleDescription = "Solicitar aprobación";
            this.VentasBtnSolicitar.AccessibleName = "Solicitar aprobación";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnSolicitar.Text = string.Empty;
            this.VentasBtnSolicitar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnSolicitar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnSolicitar.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalGuardarVentas;
            this.VentasBtnSolicitar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnSolicitar.Image = null;
            this.VentasBtnSolicitar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnSolicitar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnSolicitar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnSolicitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnSolicitar.FlatAppearance.BorderSize = 0;
            this.VentasBtnSolicitar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnSolicitar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnSolicitar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnSolicitar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnSolicitar.UseVisualStyleBackColor = false;
            this.VentasBtnSolicitar.UseCompatibleTextRendering = false;
            this.VentasBtnSolicitar.Size = new System.Drawing.Size(63, 64);
            this.VentasBtnSolicitar.TabIndex = 1;

            //
            // VentasTbpConsulta
            //
            this.VentasTbpConsulta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpConsulta.Controls.Add(this.VentasLblConsultaVendedor);
            this.VentasTbpConsulta.Controls.Add(this.VentasTxtConsultaVendedor);
            this.VentasTbpConsulta.Controls.Add(this.VentasLblConsultaPeriodo);
            this.VentasTbpConsulta.Controls.Add(this.VentasTxtConsultaPeriodo);
            this.VentasTbpConsulta.Controls.Add(this.VentasLblConsultaDocumento);
            this.VentasTbpConsulta.Controls.Add(this.VentasTxtConsultaDocumento);
            this.VentasTbpConsulta.Controls.Add(this.VentasLblConsultaEstado);
            this.VentasTbpConsulta.Controls.Add(this.VentasTxtConsultaEstado);
            this.VentasTbpConsulta.Controls.Add(this.VentasBtnBuscar);
            this.VentasTbpConsulta.Controls.Add(this.VentasLblHistorial);
            this.VentasTbpConsulta.Controls.Add(this.VentasDgvHistorial);
            this.VentasTbpConsulta.Controls.Add(this.VentasPnlHistorialCantidad);
            this.VentasTbpConsulta.Controls.Add(this.VentasPnlHistorialMonto);
            this.VentasTbpConsulta.Controls.Add(this.VentasPnlHistorialSeleccion);
            this.VentasTbpConsulta.Location = new System.Drawing.Point(4, 42);
            this.VentasTbpConsulta.Name = "VentasTbpConsulta";
            this.VentasTbpConsulta.Padding = new System.Windows.Forms.Padding(10);
            this.VentasTbpConsulta.Size = new System.Drawing.Size(1052, 532);
            this.VentasTbpConsulta.TabIndex = 1;
            this.VentasTbpConsulta.Text = "Consultar comisiones";
            //
            // VentasLblConsultaVendedor
            //
            this.VentasLblConsultaVendedor.AutoSize = true;
            this.VentasLblConsultaVendedor.Location = new System.Drawing.Point(24, 22);
            this.VentasLblConsultaVendedor.Name = "VentasLblConsultaVendedor";
            this.VentasLblConsultaVendedor.Size = new System.Drawing.Size(84, 23);
            this.VentasLblConsultaVendedor.TabIndex = 0;
            this.VentasLblConsultaVendedor.Text = "Vendedor";
            //
            // VentasTxtConsultaVendedor
            //
            this.VentasTxtConsultaVendedor.Location = new System.Drawing.Point(24, 46);
            this.VentasTxtConsultaVendedor.Name = "VentasTxtConsultaVendedor";
            this.VentasTxtConsultaVendedor.ReadOnly = true;
            this.VentasTxtConsultaVendedor.Size = new System.Drawing.Size(225, 30);
            this.VentasTxtConsultaVendedor.TabIndex = 1;
            this.VentasTxtConsultaVendedor.Text = "Todos los vendedores  ▾";
            //
            // VentasLblConsultaPeriodo
            //
            this.VentasLblConsultaPeriodo.AutoSize = true;
            this.VentasLblConsultaPeriodo.Location = new System.Drawing.Point(266, 22);
            this.VentasLblConsultaPeriodo.Name = "VentasLblConsultaPeriodo";
            this.VentasLblConsultaPeriodo.Size = new System.Drawing.Size(68, 23);
            this.VentasLblConsultaPeriodo.TabIndex = 2;
            this.VentasLblConsultaPeriodo.Text = "Periodo";
            //
            // VentasTxtConsultaPeriodo
            //
            this.VentasTxtConsultaPeriodo.Location = new System.Drawing.Point(266, 46);
            this.VentasTxtConsultaPeriodo.Name = "VentasTxtConsultaPeriodo";
            this.VentasTxtConsultaPeriodo.ReadOnly = true;
            this.VentasTxtConsultaPeriodo.Size = new System.Drawing.Size(240, 30);
            this.VentasTxtConsultaPeriodo.TabIndex = 3;
            this.VentasTxtConsultaPeriodo.Text = "01/10/2026 – 31/10/2026";
            //
            // VentasLblConsultaDocumento
            //
            this.VentasLblConsultaDocumento.AutoSize = true;
            this.VentasLblConsultaDocumento.Location = new System.Drawing.Point(523, 22);
            this.VentasLblConsultaDocumento.Name = "VentasLblConsultaDocumento";
            this.VentasLblConsultaDocumento.Size = new System.Drawing.Size(100, 23);
            this.VentasLblConsultaDocumento.TabIndex = 4;
            this.VentasLblConsultaDocumento.Text = "Documento";
            //
            // VentasTxtConsultaDocumento
            //
            this.VentasTxtConsultaDocumento.Location = new System.Drawing.Point(523, 46);
            this.VentasTxtConsultaDocumento.Name = "VentasTxtConsultaDocumento";
            this.VentasTxtConsultaDocumento.ReadOnly = true;
            this.VentasTxtConsultaDocumento.Size = new System.Drawing.Size(220, 30);
            this.VentasTxtConsultaDocumento.TabIndex = 5;
            this.VentasTxtConsultaDocumento.Text = "Buscar por número";
            //
            // VentasLblConsultaEstado
            //
            this.VentasLblConsultaEstado.AutoSize = true;
            this.VentasLblConsultaEstado.Location = new System.Drawing.Point(760, 22);
            this.VentasLblConsultaEstado.Name = "VentasLblConsultaEstado";
            this.VentasLblConsultaEstado.Size = new System.Drawing.Size(61, 23);
            this.VentasLblConsultaEstado.TabIndex = 6;
            this.VentasLblConsultaEstado.Text = "Estado";
            //
            // VentasTxtConsultaEstado
            //
            this.VentasTxtConsultaEstado.Location = new System.Drawing.Point(760, 46);
            this.VentasTxtConsultaEstado.Name = "VentasTxtConsultaEstado";
            this.VentasTxtConsultaEstado.ReadOnly = true;
            this.VentasTxtConsultaEstado.Size = new System.Drawing.Size(205, 30);
            this.VentasTxtConsultaEstado.TabIndex = 7;
            this.VentasTxtConsultaEstado.Text = "Todos los estados  ▾";
            //
            // VentasBtnBuscar
            //

            this.VentasBtnBuscar.Location = new System.Drawing.Point(950, 87);
            this.VentasBtnBuscar.Name = "VentasBtnBuscar";
            this.VentasBtnBuscar.AccessibleDescription = "Buscar";
            this.VentasBtnBuscar.AccessibleName = "Buscar";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnBuscar.Text = string.Empty;
            this.VentasBtnBuscar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnBuscar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnBuscar.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalConsultarVentas;
            this.VentasBtnBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnBuscar.Image = null;
            this.VentasBtnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnBuscar.FlatAppearance.BorderSize = 0;
            this.VentasBtnBuscar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnBuscar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnBuscar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnBuscar.UseVisualStyleBackColor = false;
            this.VentasBtnBuscar.UseCompatibleTextRendering = false;
            this.VentasBtnBuscar.Size = new System.Drawing.Size(58, 64);
            this.VentasBtnBuscar.TabIndex = 8;

            //
            // VentasLblHistorial
            //
            this.VentasLblHistorial.AutoSize = true;
            this.VentasLblHistorial.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.VentasLblHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblHistorial.Location = new System.Drawing.Point(24, 120);
            this.VentasLblHistorial.Name = "VentasLblHistorial";
            this.VentasLblHistorial.Size = new System.Drawing.Size(276, 25);
            this.VentasLblHistorial.TabIndex = 9;
            this.VentasLblHistorial.Text = "Historial de comisiones";
            //
            // VentasDgvHistorial
            //
            this.VentasDgvHistorial.AllowUserToAddRows = false;
            this.VentasDgvHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvHistorial.BackgroundColor = System.Drawing.Color.White;
            this.VentasDgvHistorial.ColumnHeadersHeight = 29;
            this.VentasDgvHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmHistorialPeriodo,
            this.VentasClmHistorialVendedor,
            this.VentasClmHistorialVentas,
            this.VentasClmHistorialImporte,
            this.VentasClmHistorialDocumento,
            this.VentasClmHistorialEstado});
            this.VentasDgvHistorial.Location = new System.Drawing.Point(24, 158);
            this.VentasDgvHistorial.Name = "VentasDgvHistorial";
            this.VentasDgvHistorial.ReadOnly = true;
            this.VentasDgvHistorial.RowHeadersVisible = false;
            this.VentasDgvHistorial.RowHeadersWidth = 51;
            this.VentasDgvHistorial.Size = new System.Drawing.Size(998, 275);
            this.VentasDgvHistorial.TabIndex = 10;
            //
            // VentasClmHistorialPeriodo
            //
            this.VentasClmHistorialPeriodo.HeaderText = "Periodo";
            this.VentasClmHistorialPeriodo.MinimumWidth = 6;
            this.VentasClmHistorialPeriodo.Name = "VentasClmHistorialPeriodo";
            this.VentasClmHistorialPeriodo.ReadOnly = true;
            //
            // VentasClmHistorialVendedor
            //
            this.VentasClmHistorialVendedor.HeaderText = "Vendedor";
            this.VentasClmHistorialVendedor.MinimumWidth = 6;
            this.VentasClmHistorialVendedor.Name = "VentasClmHistorialVendedor";
            this.VentasClmHistorialVendedor.ReadOnly = true;
            //
            // VentasClmHistorialVentas
            //
            this.VentasClmHistorialVentas.HeaderText = "Ventas";
            this.VentasClmHistorialVentas.MinimumWidth = 6;
            this.VentasClmHistorialVentas.Name = "VentasClmHistorialVentas";
            this.VentasClmHistorialVentas.ReadOnly = true;
            //
            // VentasClmHistorialImporte
            //
            this.VentasClmHistorialImporte.HeaderText = "Comisión";
            this.VentasClmHistorialImporte.MinimumWidth = 6;
            this.VentasClmHistorialImporte.Name = "VentasClmHistorialImporte";
            this.VentasClmHistorialImporte.ReadOnly = true;
            //
            // VentasClmHistorialDocumento
            //
            this.VentasClmHistorialDocumento.HeaderText = "Documento";
            this.VentasClmHistorialDocumento.MinimumWidth = 6;
            this.VentasClmHistorialDocumento.Name = "VentasClmHistorialDocumento";
            this.VentasClmHistorialDocumento.ReadOnly = true;
            //
            // VentasClmHistorialEstado
            //
            this.VentasClmHistorialEstado.HeaderText = "Estado";
            this.VentasClmHistorialEstado.MinimumWidth = 6;
            this.VentasClmHistorialEstado.Name = "VentasClmHistorialEstado";
            this.VentasClmHistorialEstado.ReadOnly = true;
            //
            // VentasPnlHistorialCantidad
            //
            this.VentasPnlHistorialCantidad.BackColor = System.Drawing.Color.White;
            this.VentasPnlHistorialCantidad.Controls.Add(this.VentasLblHistorialCantidad);
            this.VentasPnlHistorialCantidad.Controls.Add(this.VentasLblHistorialCantidadDetalle);
            this.VentasPnlHistorialCantidad.Location = new System.Drawing.Point(24, 456);
            this.VentasPnlHistorialCantidad.Name = "VentasPnlHistorialCantidad";
            this.VentasPnlHistorialCantidad.Size = new System.Drawing.Size(313, 68);
            this.VentasPnlHistorialCantidad.TabIndex = 11;
            //
            // VentasLblHistorialCantidad
            //
            this.VentasLblHistorialCantidad.AutoSize = true;
            this.VentasLblHistorialCantidad.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.VentasLblHistorialCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblHistorialCantidad.Location = new System.Drawing.Point(14, 10);
            this.VentasLblHistorialCantidad.Name = "VentasLblHistorialCantidad";
            this.VentasLblHistorialCantidad.Size = new System.Drawing.Size(160, 25);
            this.VentasLblHistorialCantidad.TabIndex = 0;
            this.VentasLblHistorialCantidad.Text = "4 comisiones";
            //
            // VentasLblHistorialCantidadDetalle
            //
            this.VentasLblHistorialCantidadDetalle.AutoSize = true;
            this.VentasLblHistorialCantidadDetalle.Location = new System.Drawing.Point(15, 40);
            this.VentasLblHistorialCantidadDetalle.Name = "VentasLblHistorialCantidadDetalle";
            this.VentasLblHistorialCantidadDetalle.Size = new System.Drawing.Size(182, 23);
            this.VentasLblHistorialCantidadDetalle.TabIndex = 1;
            this.VentasLblHistorialCantidadDetalle.Text = "Resultados de ejemplo";
            //
            // VentasPnlHistorialMonto
            //
            this.VentasPnlHistorialMonto.BackColor = System.Drawing.Color.White;
            this.VentasPnlHistorialMonto.Controls.Add(this.VentasLblHistorialMonto);
            this.VentasPnlHistorialMonto.Controls.Add(this.VentasLblHistorialMontoDetalle);
            this.VentasPnlHistorialMonto.Location = new System.Drawing.Point(349, 456);
            this.VentasPnlHistorialMonto.Name = "VentasPnlHistorialMonto";
            this.VentasPnlHistorialMonto.Size = new System.Drawing.Size(313, 68);
            this.VentasPnlHistorialMonto.TabIndex = 12;
            //
            // VentasLblHistorialMonto
            //
            this.VentasLblHistorialMonto.AutoSize = true;
            this.VentasLblHistorialMonto.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.VentasLblHistorialMonto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblHistorialMonto.Location = new System.Drawing.Point(14, 10);
            this.VentasLblHistorialMonto.Name = "VentasLblHistorialMonto";
            this.VentasLblHistorialMonto.Size = new System.Drawing.Size(131, 25);
            this.VentasLblHistorialMonto.TabIndex = 0;
            this.VentasLblHistorialMonto.Text = "Q 7,427.50";
            //
            // VentasLblHistorialMontoDetalle
            //
            this.VentasLblHistorialMontoDetalle.AutoSize = true;
            this.VentasLblHistorialMontoDetalle.Location = new System.Drawing.Point(15, 40);
            this.VentasLblHistorialMontoDetalle.Name = "VentasLblHistorialMontoDetalle";
            this.VentasLblHistorialMontoDetalle.Size = new System.Drawing.Size(217, 23);
            this.VentasLblHistorialMontoDetalle.TabIndex = 1;
            this.VentasLblHistorialMontoDetalle.Text = "Total ilustrativo del periodo";
            //
            // VentasPnlHistorialSeleccion
            //
            this.VentasPnlHistorialSeleccion.BackColor = System.Drawing.Color.White;
            this.VentasPnlHistorialSeleccion.Controls.Add(this.VentasLblHistorialSeleccion);
            this.VentasPnlHistorialSeleccion.Controls.Add(this.VentasLblHistorialSeleccionDetalle);
            this.VentasPnlHistorialSeleccion.Location = new System.Drawing.Point(674, 456);
            this.VentasPnlHistorialSeleccion.Name = "VentasPnlHistorialSeleccion";
            this.VentasPnlHistorialSeleccion.Size = new System.Drawing.Size(348, 68);
            this.VentasPnlHistorialSeleccion.TabIndex = 13;
            //
            // VentasLblHistorialSeleccion
            //
            this.VentasLblHistorialSeleccion.AutoSize = true;
            this.VentasLblHistorialSeleccion.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.VentasLblHistorialSeleccion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblHistorialSeleccion.Location = new System.Drawing.Point(14, 10);
            this.VentasLblHistorialSeleccion.Name = "VentasLblHistorialSeleccion";
            this.VentasLblHistorialSeleccion.Size = new System.Drawing.Size(245, 23);
            this.VentasLblHistorialSeleccion.TabIndex = 0;
            this.VentasLblHistorialSeleccion.Text = "Seleccione un registro";
            //
            // VentasLblHistorialSeleccionDetalle
            //
            this.VentasLblHistorialSeleccionDetalle.AutoSize = true;
            this.VentasLblHistorialSeleccionDetalle.Location = new System.Drawing.Point(15, 40);
            this.VentasLblHistorialSeleccionDetalle.Name = "VentasLblHistorialSeleccionDetalle";
            this.VentasLblHistorialSeleccionDetalle.Size = new System.Drawing.Size(190, 23);
            this.VentasLblHistorialSeleccionDetalle.TabIndex = 1;
            this.VentasLblHistorialSeleccionDetalle.Text = "Detalle, ventas y estado";
            //
            // VentasTbpAutorizacion
            //
            this.VentasTbpAutorizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.VentasTbpAutorizacion.Controls.Add(this.VentasLblBandeja);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasLblAyudaBandeja);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasDgvAutorizacion);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasPnlResumen);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasLblComentario);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasTxtComentario);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasBtnRechazar);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasBtnAprobar);
            this.VentasTbpAutorizacion.Controls.Add(this.VentasPnlPieAutorizacion);
            this.VentasTbpAutorizacion.Location = new System.Drawing.Point(4, 42);
            this.VentasTbpAutorizacion.Name = "VentasTbpAutorizacion";
            this.VentasTbpAutorizacion.Padding = new System.Windows.Forms.Padding(10);
            this.VentasTbpAutorizacion.Size = new System.Drawing.Size(1052, 532);
            this.VentasTbpAutorizacion.TabIndex = 2;
            this.VentasTbpAutorizacion.Text = "Autorizar comisión";
            //
            // VentasLblBandeja
            //
            this.VentasLblBandeja.AutoSize = true;
            this.VentasLblBandeja.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.VentasLblBandeja.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.VentasLblBandeja.Location = new System.Drawing.Point(24, 20);
            this.VentasLblBandeja.Name = "VentasLblBandeja";
            this.VentasLblBandeja.Size = new System.Drawing.Size(286, 25);
            this.VentasLblBandeja.TabIndex = 0;
            this.VentasLblBandeja.Text = "Bandeja de autorización";
            //
            // VentasLblAyudaBandeja
            //
            this.VentasLblAyudaBandeja.AutoSize = true;
            this.VentasLblAyudaBandeja.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblAyudaBandeja.Location = new System.Drawing.Point(24, 48);
            this.VentasLblAyudaBandeja.Name = "VentasLblAyudaBandeja";
            this.VentasLblAyudaBandeja.Size = new System.Drawing.Size(462, 23);
            this.VentasLblAyudaBandeja.TabIndex = 1;
            this.VentasLblAyudaBandeja.Text = "Revise el resumen del periodo antes de aprobar o rechazar.";
            //
            // VentasDgvAutorizacion
            //
            this.VentasDgvAutorizacion.AllowUserToAddRows = false;
            this.VentasDgvAutorizacion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.VentasDgvAutorizacion.BackgroundColor = System.Drawing.Color.White;
            this.VentasDgvAutorizacion.ColumnHeadersHeight = 29;
            this.VentasDgvAutorizacion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VentasClmAutorizacionPeriodo,
            this.VentasClmAutorizacionVendedor,
            this.VentasClmAutorizacionVentas,
            this.VentasClmAutorizacionMonto,
            this.VentasClmAutorizacionSolicitada,
            this.VentasClmAutorizacionEstado});
            this.VentasDgvAutorizacion.Location = new System.Drawing.Point(24, 82);
            this.VentasDgvAutorizacion.Name = "VentasDgvAutorizacion";
            this.VentasDgvAutorizacion.ReadOnly = true;
            this.VentasDgvAutorizacion.RowHeadersVisible = false;
            this.VentasDgvAutorizacion.RowHeadersWidth = 51;
            this.VentasDgvAutorizacion.Size = new System.Drawing.Size(998, 170);
            this.VentasDgvAutorizacion.TabIndex = 2;
            //
            // VentasClmAutorizacionPeriodo
            //
            this.VentasClmAutorizacionPeriodo.HeaderText = "Periodo";
            this.VentasClmAutorizacionPeriodo.MinimumWidth = 6;
            this.VentasClmAutorizacionPeriodo.Name = "VentasClmAutorizacionPeriodo";
            this.VentasClmAutorizacionPeriodo.ReadOnly = true;
            //
            // VentasClmAutorizacionVendedor
            //
            this.VentasClmAutorizacionVendedor.HeaderText = "Vendedor";
            this.VentasClmAutorizacionVendedor.MinimumWidth = 6;
            this.VentasClmAutorizacionVendedor.Name = "VentasClmAutorizacionVendedor";
            this.VentasClmAutorizacionVendedor.ReadOnly = true;
            //
            // VentasClmAutorizacionVentas
            //
            this.VentasClmAutorizacionVentas.HeaderText = "Ventas";
            this.VentasClmAutorizacionVentas.MinimumWidth = 6;
            this.VentasClmAutorizacionVentas.Name = "VentasClmAutorizacionVentas";
            this.VentasClmAutorizacionVentas.ReadOnly = true;
            //
            // VentasClmAutorizacionMonto
            //
            this.VentasClmAutorizacionMonto.HeaderText = "Monto";
            this.VentasClmAutorizacionMonto.MinimumWidth = 6;
            this.VentasClmAutorizacionMonto.Name = "VentasClmAutorizacionMonto";
            this.VentasClmAutorizacionMonto.ReadOnly = true;
            //
            // VentasClmAutorizacionSolicitada
            //
            this.VentasClmAutorizacionSolicitada.HeaderText = "Solicitada";
            this.VentasClmAutorizacionSolicitada.MinimumWidth = 6;
            this.VentasClmAutorizacionSolicitada.Name = "VentasClmAutorizacionSolicitada";
            this.VentasClmAutorizacionSolicitada.ReadOnly = true;
            //
            // VentasClmAutorizacionEstado
            //
            this.VentasClmAutorizacionEstado.HeaderText = "Estado";
            this.VentasClmAutorizacionEstado.MinimumWidth = 6;
            this.VentasClmAutorizacionEstado.Name = "VentasClmAutorizacionEstado";
            this.VentasClmAutorizacionEstado.ReadOnly = true;
            //
            // VentasPnlResumen
            //
            this.VentasPnlResumen.BackColor = System.Drawing.Color.White;
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenTitulo);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenVendedor);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenPeriodo);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenVentas);
            this.VentasPnlResumen.Controls.Add(this.VentasLblResumenMonto);
            this.VentasPnlResumen.Location = new System.Drawing.Point(24, 266);
            this.VentasPnlResumen.Name = "VentasPnlResumen";
            this.VentasPnlResumen.Size = new System.Drawing.Size(998, 110);
            this.VentasPnlResumen.TabIndex = 3;
            //
            // VentasLblResumenTitulo
            //
            this.VentasLblResumenTitulo.AutoSize = true;
            this.VentasLblResumenTitulo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.VentasLblResumenTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(63)))), ((int)(((byte)(147)))));
            this.VentasLblResumenTitulo.Location = new System.Drawing.Point(18, 16);
            this.VentasLblResumenTitulo.Name = "VentasLblResumenTitulo";
            this.VentasLblResumenTitulo.Size = new System.Drawing.Size(343, 18);
            this.VentasLblResumenTitulo.TabIndex = 0;
            this.VentasLblResumenTitulo.Text = "RESUMEN DE LA SOLICITUD SELECCIONADA";
            //
            // VentasLblResumenVendedor
            //
            this.VentasLblResumenVendedor.AutoSize = true;
            this.VentasLblResumenVendedor.Location = new System.Drawing.Point(20, 50);
            this.VentasLblResumenVendedor.Name = "VentasLblResumenVendedor";
            this.VentasLblResumenVendedor.Size = new System.Drawing.Size(90, 46);
            this.VentasLblResumenVendedor.TabIndex = 1;
            this.VentasLblResumenVendedor.Text = "Vendedor\nAna López";
            //
            // VentasLblResumenPeriodo
            //
            this.VentasLblResumenPeriodo.AutoSize = true;
            this.VentasLblResumenPeriodo.Location = new System.Drawing.Point(240, 50);
            this.VentasLblResumenPeriodo.Name = "VentasLblResumenPeriodo";
            this.VentasLblResumenPeriodo.Size = new System.Drawing.Size(205, 46);
            this.VentasLblResumenPeriodo.TabIndex = 2;
            this.VentasLblResumenPeriodo.Text = "Periodo\n01/10/2026 al 31/10/2026";
            //
            // VentasLblResumenVentas
            //
            this.VentasLblResumenVentas.AutoSize = true;
            this.VentasLblResumenVentas.Location = new System.Drawing.Point(520, 50);
            this.VentasLblResumenVentas.Name = "VentasLblResumenVentas";
            this.VentasLblResumenVentas.Size = new System.Drawing.Size(132, 46);
            this.VentasLblResumenVentas.TabIndex = 3;
            this.VentasLblResumenVentas.Text = "Ventas elegibles\n12 documentos";
            //
            // VentasLblResumenMonto
            //
            this.VentasLblResumenMonto.AutoSize = true;
            this.VentasLblResumenMonto.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.VentasLblResumenMonto.Location = new System.Drawing.Point(770, 50);
            this.VentasLblResumenMonto.Name = "VentasLblResumenMonto";
            this.VentasLblResumenMonto.Size = new System.Drawing.Size(98, 46);
            this.VentasLblResumenMonto.TabIndex = 4;
            this.VentasLblResumenMonto.Text = "Comisión\nQ 2,437.50";
            //
            // VentasLblComentario
            //
            this.VentasLblComentario.AutoSize = true;
            this.VentasLblComentario.Location = new System.Drawing.Point(24, 388);
            this.VentasLblComentario.Name = "VentasLblComentario";
            this.VentasLblComentario.Size = new System.Drawing.Size(223, 23);
            this.VentasLblComentario.TabIndex = 4;
            this.VentasLblComentario.Text = "Comentario de autorización";
            //
            // VentasTxtComentario
            //
            this.VentasTxtComentario.Location = new System.Drawing.Point(24, 420);
            this.VentasTxtComentario.Name = "VentasTxtComentario";
            this.VentasTxtComentario.ReadOnly = true;
            this.VentasTxtComentario.Size = new System.Drawing.Size(620, 28);
            this.VentasTxtComentario.TabIndex = 5;
            this.VentasTxtComentario.Text = "Agregar comentario (opcional)";
            //
            // VentasBtnRechazar
            //

            this.VentasBtnRechazar.Location = new System.Drawing.Point(854, 382);
            this.VentasBtnRechazar.Name = "VentasBtnRechazar";
            this.VentasBtnRechazar.AccessibleDescription = "Rechazar";
            this.VentasBtnRechazar.AccessibleName = "Rechazar";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnRechazar.Text = string.Empty;
            this.VentasBtnRechazar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnRechazar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnRechazar.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalCancelarVentas;
            this.VentasBtnRechazar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnRechazar.Image = null;
            this.VentasBtnRechazar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnRechazar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnRechazar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnRechazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnRechazar.FlatAppearance.BorderSize = 0;
            this.VentasBtnRechazar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnRechazar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnRechazar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnRechazar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnRechazar.UseVisualStyleBackColor = false;
            this.VentasBtnRechazar.UseCompatibleTextRendering = false;
            this.VentasBtnRechazar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnRechazar.TabIndex = 6;

            //
            // VentasBtnAprobar
            //

            this.VentasBtnAprobar.Location = new System.Drawing.Point(950, 382);
            this.VentasBtnAprobar.Name = "VentasBtnAprobar";
            this.VentasBtnAprobar.AccessibleDescription = "Aprobar comisión";
            this.VentasBtnAprobar.AccessibleName = "Aprobar comisión";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnAprobar.Text = string.Empty;
            this.VentasBtnAprobar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnAprobar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnAprobar.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalGuardarVentas;
            this.VentasBtnAprobar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnAprobar.Image = null;
            this.VentasBtnAprobar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAprobar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnAprobar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnAprobar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnAprobar.FlatAppearance.BorderSize = 0;
            this.VentasBtnAprobar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAprobar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnAprobar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnAprobar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnAprobar.UseVisualStyleBackColor = false;
            this.VentasBtnAprobar.UseCompatibleTextRendering = false;
            this.VentasBtnAprobar.Size = new System.Drawing.Size(63, 64);
            this.VentasBtnAprobar.TabIndex = 7;

            //
            // VentasPnlPieAutorizacion
            //
            this.VentasPnlPieAutorizacion.BackColor = System.Drawing.Color.White;
            this.VentasPnlPieAutorizacion.Controls.Add(this.VentasLblAvisoEnvio);
            this.VentasPnlPieAutorizacion.Controls.Add(this.VentasBtnEnviar);
            this.VentasPnlPieAutorizacion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.VentasPnlPieAutorizacion.Location = new System.Drawing.Point(10, 462);
            this.VentasPnlPieAutorizacion.Name = "VentasPnlPieAutorizacion";
            this.VentasPnlPieAutorizacion.Size = new System.Drawing.Size(1032, 72);
            this.VentasPnlPieAutorizacion.TabIndex = 8;
            //
            // VentasLblAvisoEnvio
            //
            this.VentasLblAvisoEnvio.AutoSize = true;
            this.VentasLblAvisoEnvio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(102)))), ((int)(((byte)(117)))));
            this.VentasLblAvisoEnvio.Location = new System.Drawing.Point(20, 21);
            this.VentasLblAvisoEnvio.Name = "VentasLblAvisoEnvio";
            this.VentasLblAvisoEnvio.Size = new System.Drawing.Size(555, 23);
            this.VentasLblAvisoEnvio.TabIndex = 0;
            this.VentasLblAvisoEnvio.Text = "Al aprobar, el resumen queda listo para envío a Contabilidad / Nómina.";
            //
            // VentasBtnEnviar
            //

            this.VentasBtnEnviar.Location = new System.Drawing.Point(952, 4);
            this.VentasBtnEnviar.Name = "VentasBtnEnviar";
            this.VentasBtnEnviar.AccessibleDescription = "Enviar resumen aprobado";
            this.VentasBtnEnviar.AccessibleName = "Enviar resumen aprobado";
            // Imagen original completa: solo el color vino fue cambiado a morado.
            this.VentasBtnEnviar.Text = string.Empty;
            this.VentasBtnEnviar.BackColor = System.Drawing.Color.FromArgb(123, 63, 147);
            this.VentasBtnEnviar.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasBtnEnviar.BackgroundImage = global::CapaVista_ComisionVendedores.Properties.Resources.BotonOriginalSalirVentas;
            this.VentasBtnEnviar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VentasBtnEnviar.Image = null;
            this.VentasBtnEnviar.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEnviar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasBtnEnviar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.VentasBtnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VentasBtnEnviar.FlatAppearance.BorderSize = 0;
            this.VentasBtnEnviar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEnviar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasBtnEnviar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.VentasBtnEnviar.Padding = new System.Windows.Forms.Padding(0);
            this.VentasBtnEnviar.UseVisualStyleBackColor = false;
            this.VentasBtnEnviar.UseCompatibleTextRendering = false;
            this.VentasBtnEnviar.Size = new System.Drawing.Size(64, 64);
            this.VentasBtnEnviar.TabIndex = 1;

            //
            // FrmComisionesVendedoresVentas
            //
            this.VentasLblConsultaVendedor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblConsultaPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblConsultaDocumento.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblConsultaEstado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasTxtConsultaVendedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtConsultaPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtConsultaDocumento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasTxtConsultaEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasLblHistorialCantidadDetalle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblHistorialMontoDetalle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblHistorialSeleccionDetalle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblAyudaBandeja.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblResumenVendedor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblResumenPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblResumenVentas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasLblComentario.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasTxtComentario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasLblAvisoEnvio.Font = new System.Drawing.Font("Tahoma", 10F);
            this.VentasDgvVentas.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDgvHistorial.ColumnHeadersDefaultCellStyle = VentasEstiloCabecera;
            this.VentasDgvHistorial.EnableHeadersVisualStyles = false;
            this.VentasDgvHistorial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.VentasDgvAutorizacion.ColumnHeadersDefaultCellStyle = VentasEstiloCabecera;
            this.VentasDgvAutorizacion.EnableHeadersVisualStyles = false;
            this.VentasDgvAutorizacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1060, 660);
            this.Controls.Add(this.VentasTbcComisiones);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(29)))), ((int)(((byte)(74)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_ComisionVendedores.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmComisionesVendedoresVentas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7008 – ComisionesVendedores";
            this.VentasPnlEncabezado.ResumeLayout(false);
            this.VentasPnlEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasPbMarca)).EndInit();
            this.VentasTbcComisiones.ResumeLayout(false);
            this.VentasTbpCalculo.ResumeLayout(false);
            this.VentasTbpCalculo.PerformLayout();
            this.VentasPnlTarjetaVentas.ResumeLayout(false);
            this.VentasPnlTarjetaVentas.PerformLayout();
            this.VentasPnlTarjetaMonto.ResumeLayout(false);
            this.VentasPnlTarjetaMonto.PerformLayout();
            this.VentasPnlTarjetaEstado.ResumeLayout(false);
            this.VentasPnlTarjetaEstado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvVentas)).EndInit();
            this.VentasPnlPieCalculo.ResumeLayout(false);
            this.VentasPnlPieCalculo.PerformLayout();
            this.VentasTbpConsulta.ResumeLayout(false);
            this.VentasTbpConsulta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvHistorial)).EndInit();
            this.VentasPnlHistorialCantidad.ResumeLayout(false);
            this.VentasPnlHistorialCantidad.PerformLayout();
            this.VentasPnlHistorialMonto.ResumeLayout(false);
            this.VentasPnlHistorialMonto.PerformLayout();
            this.VentasPnlHistorialSeleccion.ResumeLayout(false);
            this.VentasPnlHistorialSeleccion.PerformLayout();
            this.VentasTbpAutorizacion.ResumeLayout(false);
            this.VentasTbpAutorizacion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VentasDgvAutorizacion)).EndInit();
            this.VentasPnlResumen.ResumeLayout(false);
            this.VentasPnlResumen.PerformLayout();
            this.VentasPnlPieAutorizacion.ResumeLayout(false);
            this.VentasPnlPieAutorizacion.PerformLayout();
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnCalcular, "Calcular comisión");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnAnterior, "Anterior");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnSiguiente, "Siguiente");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnSolicitar, "Solicitar aprobación");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnBuscar, "Buscar");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnRechazar, "Rechazar");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnAprobar, "Aprobar comisión");
            this.VentasTtpAccionesOriginales.SetToolTip(this.VentasBtnEnviar, "Enviar resumen aprobado");
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaDocumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaTasa;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaImporte;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmVentaEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialPeriodo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialVendedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialVentas;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialImporte;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialDocumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmHistorialEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionPeriodo;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionVendedor;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionVentas;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionMonto;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionSolicitada;
        private System.Windows.Forms.DataGridViewTextBoxColumn VentasClmAutorizacionEstado;
        private System.Windows.Forms.ToolTip VentasTtpAccionesOriginales;
    }
}

// FIN DEL CÓDIGO — Equipo asignado al formulario | Fecha: 15/10/2026
