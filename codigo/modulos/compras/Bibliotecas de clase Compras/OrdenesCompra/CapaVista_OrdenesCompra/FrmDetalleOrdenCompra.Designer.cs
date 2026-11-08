using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Define los controles y la distribución visual de los detalles de una orden.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    partial class FrmDetalleOrdenCompra
    {

        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos) ComprasProcLiberarDetalles();
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDetalleOrdenCompra));
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxEncabezado = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpEncabezado = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxMoneda = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpConsultaMoneda = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEtiquetaMoneda = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtMoneda = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTipoCambio = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxProveedor = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpConsultaProveedor = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboNombreProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtNitProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTelefonoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCorreoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDireccionProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstadoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtEstadoProveedor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxPlazo = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpConsultaPlazo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblCodigoOrden = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCodigoOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEstadoValor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblFechaDeEmision = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasDtpFechaEmision = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasLblFechaDeVencimiento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasDtpFechaVencimiento = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasLblPlazo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtPlazoValor = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDiasPlazo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDiasPlazo = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxFinalizacion = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpFinalizacion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpImportes = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEtiquetaComentario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtComentario = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpComentarioYTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnDescartar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnRegresar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpTotales = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEtiquetaSubtotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtSubtotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaDescuento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescuentoTotal = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaImpuesto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtImpuesto = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEtiquetaTotal = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtTotalDeLaOrden = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasGbxDetalles = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpDetalles = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasPnlArticulos = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasDgvProductos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasUsrEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasTlpDatosOrden = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpProveedor = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblProveedor = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpResponsable = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpMantenimientoProductos = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpDetallesProducto = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasFlpPrecioCantidadDescuento = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnTogglePorcentaje = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTxtDescuento = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblDescuento = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtCantidad = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblCantidad = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblPrecioUnitario = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasFlpProductoSku = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasCboSku = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblSku = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboProducto = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpTotalesArticulo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTotalesDeProducto = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpAccionesArticulo = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasFlpAccionesOrden = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnIngresarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnModificarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnEliminarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnRefrescarProducto = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxEncabezado.SuspendLayout();
            this.ComprasTlpEncabezado.SuspendLayout();
            this.ComprasGbxMoneda.SuspendLayout();
            this.ComprasTlpConsultaMoneda.SuspendLayout();
            this.ComprasGbxProveedor.SuspendLayout();
            this.ComprasTlpConsultaProveedor.SuspendLayout();
            this.ComprasGbxPlazo.SuspendLayout();
            this.ComprasTlpConsultaPlazo.SuspendLayout();
            this.ComprasGbxFinalizacion.SuspendLayout();
            this.ComprasTlpFinalizacion.SuspendLayout();
            this.ComprasTlpImportes.SuspendLayout();
            this.ComprasTlpComentarioYTotales.SuspendLayout();
            this.ComprasTlpTotales.SuspendLayout();
            this.ComprasGbxDetalles.SuspendLayout();
            this.ComprasTlpDetalles.SuspendLayout();
            this.ComprasPnlArticulos.SuspendLayout();
            this.ComprasTlpProveedor.SuspendLayout();
            this.ComprasTlpMantenimientoProductos.SuspendLayout();
            this.ComprasTlpDetallesProducto.SuspendLayout();
            this.ComprasFlpPrecioCantidadDescuento.SuspendLayout();
            this.ComprasFlpProductoSku.SuspendLayout();
            this.ComprasTlpTotalesArticulo.SuspendLayout();
            this.ComprasTlpAccionesArticulo.SuspendLayout();
            this.ComprasFlpAccionesOrden.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasPnlContenido);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 760);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpPrincipal);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.MinimumSize = new System.Drawing.Size(0, 1339);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 1339);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxEncabezado, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxFinalizacion, 0, 3);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxDetalles, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasUsrEncabezado, 0, 0);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 4;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 540F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 392F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1279, 1339);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasGbxEncabezado
            // 
            this.ComprasGbxEncabezado.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasTlpEncabezado);
            this.ComprasGbxEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxEncabezado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxEncabezado.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxEncabezado.Location = new System.Drawing.Point(10, 160);
            this.ComprasGbxEncabezado.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxEncabezado.Name = "ComprasGbxEncabezado";
            this.ComprasGbxEncabezado.Size = new System.Drawing.Size(1259, 520);
            this.ComprasGbxEncabezado.TabIndex = 4;
            this.ComprasGbxEncabezado.TabStop = false;
            this.ComprasGbxEncabezado.Text = "Datos de la orden";
            // 
            // ComprasTlpEncabezado
            // 
            this.ComprasTlpEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpEncabezado.ColumnCount = 1;
            this.ComprasTlpEncabezado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasGbxMoneda, 0, 0);
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasGbxProveedor, 0, 1);
            this.ComprasTlpEncabezado.Controls.Add(this.ComprasGbxPlazo, 0, 2);
            this.ComprasTlpEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpEncabezado.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpEncabezado.Name = "ComprasTlpEncabezado";
            this.ComprasTlpEncabezado.RowCount = 3;
            this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.ComprasTlpEncabezado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.ComprasTlpEncabezado.Size = new System.Drawing.Size(1253, 493);
            this.ComprasTlpEncabezado.TabIndex = 3;
            // 
            // ComprasGbxMoneda
            // 
            this.ComprasGbxMoneda.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxMoneda.Controls.Add(this.ComprasTlpConsultaMoneda);
            this.ComprasGbxMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxMoneda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxMoneda.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxMoneda.Location = new System.Drawing.Point(6, 6);
            this.ComprasGbxMoneda.Margin = new System.Windows.Forms.Padding(6);
            this.ComprasGbxMoneda.Name = "ComprasGbxMoneda";
            this.ComprasGbxMoneda.Size = new System.Drawing.Size(1241, 88);
            this.ComprasGbxMoneda.TabIndex = 0;
            this.ComprasGbxMoneda.TabStop = false;
            this.ComprasGbxMoneda.Text = "1. Moneda";
            // 
            // ComprasTlpConsultaMoneda
            // 
            this.ComprasTlpConsultaMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpConsultaMoneda.ColumnCount = 4;
            this.ComprasTlpConsultaMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaMoneda.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasLblEtiquetaMoneda, 0, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasTxtMoneda, 1, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasLblEtiquetaTipoCambio, 2, 0);
            this.ComprasTlpConsultaMoneda.Controls.Add(this.ComprasTxtTipoCambio, 3, 0);
            this.ComprasTlpConsultaMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpConsultaMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpConsultaMoneda.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpConsultaMoneda.Name = "ComprasTlpConsultaMoneda";
            this.ComprasTlpConsultaMoneda.Padding = new System.Windows.Forms.Padding(8);
            this.ComprasTlpConsultaMoneda.RowCount = 1;
            this.ComprasTlpConsultaMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpConsultaMoneda.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpConsultaMoneda.Size = new System.Drawing.Size(1235, 61);
            this.ComprasTlpConsultaMoneda.TabIndex = 0;
            // 
            // ComprasLblEtiquetaMoneda
            // 
            this.ComprasLblEtiquetaMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaMoneda.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaMoneda.Location = new System.Drawing.Point(12, 14);
            this.ComprasLblEtiquetaMoneda.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEtiquetaMoneda.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaMoneda.MostrarAsterisco = false;
            this.ComprasLblEtiquetaMoneda.Name = "ComprasLblEtiquetaMoneda";
            this.ComprasLblEtiquetaMoneda.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblEtiquetaMoneda.TabIndex = 11;
            this.ComprasLblEtiquetaMoneda.TabStop = false;
            this.ComprasLblEtiquetaMoneda.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaMoneda.Text = "Moneda (ID)";
            this.ComprasLblEtiquetaMoneda.Texto = "Moneda (ID)";
            // 
            // ComprasTxtMoneda
            // 
            this.ComprasTxtMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtMoneda.Enabled = false;
            this.ComprasTxtMoneda.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtMoneda.Location = new System.Drawing.Point(231, 14);
            this.ComprasTxtMoneda.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtMoneda.MaxLength = 2147483647;
            this.ComprasTxtMoneda.MostrarEtiquetaError = false;
            this.ComprasTxtMoneda.Name = "ComprasTxtMoneda";
            this.ComprasTxtMoneda.SelectedText = "";
            this.ComprasTxtMoneda.SelectionLength = 0;
            this.ComprasTxtMoneda.SelectionStart = 0;
            this.ComprasTxtMoneda.Size = new System.Drawing.Size(382, 33);
            this.ComprasTxtMoneda.TabIndex = 12;
            // 
            // ComprasLblEtiquetaTipoCambio
            // 
            this.ComprasLblEtiquetaTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTipoCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaTipoCambio.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTipoCambio.Location = new System.Drawing.Point(621, 14);
            this.ComprasLblEtiquetaTipoCambio.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEtiquetaTipoCambio.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTipoCambio.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTipoCambio.Name = "ComprasLblEtiquetaTipoCambio";
            this.ComprasLblEtiquetaTipoCambio.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblEtiquetaTipoCambio.TabIndex = 3;
            this.ComprasLblEtiquetaTipoCambio.TabStop = false;
            this.ComprasLblEtiquetaTipoCambio.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTipoCambio.Text = "Tipo de cambio";
            this.ComprasLblEtiquetaTipoCambio.Texto = "Tipo de cambio";
            // 
            // ComprasTxtTipoCambio
            // 
            this.ComprasTxtTipoCambio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTipoCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTipoCambio.Enabled = false;
            this.ComprasTxtTipoCambio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTipoCambio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTipoCambio.Location = new System.Drawing.Point(840, 14);
            this.ComprasTxtTipoCambio.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtTipoCambio.MaxLength = 2147483647;
            this.ComprasTxtTipoCambio.MostrarEtiquetaError = false;
            this.ComprasTxtTipoCambio.Name = "ComprasTxtTipoCambio";
            this.ComprasTxtTipoCambio.SelectedText = "";
            this.ComprasTxtTipoCambio.SelectionLength = 0;
            this.ComprasTxtTipoCambio.SelectionStart = 0;
            this.ComprasTxtTipoCambio.Size = new System.Drawing.Size(383, 33);
            this.ComprasTxtTipoCambio.TabIndex = 9;
            // 
            // ComprasGbxProveedor
            // 
            this.ComprasGbxProveedor.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxProveedor.Controls.Add(this.ComprasTlpConsultaProveedor);
            this.ComprasGbxProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxProveedor.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxProveedor.Location = new System.Drawing.Point(6, 106);
            this.ComprasGbxProveedor.Margin = new System.Windows.Forms.Padding(6);
            this.ComprasGbxProveedor.Name = "ComprasGbxProveedor";
            this.ComprasGbxProveedor.Size = new System.Drawing.Size(1241, 188);
            this.ComprasGbxProveedor.TabIndex = 1;
            this.ComprasGbxProveedor.TabStop = false;
            this.ComprasGbxProveedor.Text = "2. Proveedor";
            // 
            // ComprasTlpConsultaProveedor
            // 
            this.ComprasTlpConsultaProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpConsultaProveedor.ColumnCount = 4;
            this.ComprasTlpConsultaProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblNombreProveedor, 0, 0);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasCboNombreProveedor, 1, 0);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblNitProveedor, 2, 0);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasTxtNitProveedor, 3, 0);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblTelefonoProveedor, 0, 1);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasTxtTelefonoProveedor, 1, 1);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblCorreoProveedor, 2, 1);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasTxtCorreoProveedor, 3, 1);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblDireccionProveedor, 0, 2);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasTxtDireccionProveedor, 1, 2);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasLblEstadoProveedor, 2, 2);
            this.ComprasTlpConsultaProveedor.Controls.Add(this.ComprasTxtEstadoProveedor, 3, 2);
            this.ComprasTlpConsultaProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpConsultaProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpConsultaProveedor.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpConsultaProveedor.Name = "ComprasTlpConsultaProveedor";
            this.ComprasTlpConsultaProveedor.Padding = new System.Windows.Forms.Padding(8);
            this.ComprasTlpConsultaProveedor.RowCount = 3;
            this.ComprasTlpConsultaProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaProveedor.Size = new System.Drawing.Size(1235, 161);
            this.ComprasTlpConsultaProveedor.TabIndex = 0;
            // 
            // ComprasLblNombreProveedor
            // 
            this.ComprasLblNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNombreProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblNombreProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNombreProveedor.Location = new System.Drawing.Point(12, 14);
            this.ComprasLblNombreProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblNombreProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNombreProveedor.MostrarAsterisco = false;
            this.ComprasLblNombreProveedor.Name = "ComprasLblNombreProveedor";
            this.ComprasLblNombreProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblNombreProveedor.TabIndex = 1;
            this.ComprasLblNombreProveedor.TabStop = false;
            this.ComprasLblNombreProveedor.TamanoTipografia = 9F;
            this.ComprasLblNombreProveedor.Text = "Nombre";
            this.ComprasLblNombreProveedor.Texto = "Nombre";
            // 
            // ComprasCboNombreProveedor
            // 
            this.ComprasCboNombreProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboNombreProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasCboNombreProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboNombreProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboNombreProveedor.Location = new System.Drawing.Point(231, 14);
            this.ComprasCboNombreProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasCboNombreProveedor.MaxLength = 0;
            this.ComprasCboNombreProveedor.MostrarEtiquetaError = false;
            this.ComprasCboNombreProveedor.Name = "ComprasCboNombreProveedor";
            this.ComprasCboNombreProveedor.Size = new System.Drawing.Size(382, 36);
            this.ComprasCboNombreProveedor.TabIndex = 1;
            this.ComprasCboNombreProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasLblNitProveedor
            // 
            this.ComprasLblNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNitProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblNitProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNitProveedor.Location = new System.Drawing.Point(621, 14);
            this.ComprasLblNitProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblNitProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblNitProveedor.MostrarAsterisco = false;
            this.ComprasLblNitProveedor.Name = "ComprasLblNitProveedor";
            this.ComprasLblNitProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblNitProveedor.TabIndex = 4;
            this.ComprasLblNitProveedor.TabStop = false;
            this.ComprasLblNitProveedor.TamanoTipografia = 9F;
            this.ComprasLblNitProveedor.Text = "NIT";
            this.ComprasLblNitProveedor.Texto = "NIT";
            // 
            // ComprasTxtNitProveedor
            // 
            this.ComprasTxtNitProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtNitProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtNitProveedor.Enabled = false;
            this.ComprasTxtNitProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtNitProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtNitProveedor.Location = new System.Drawing.Point(840, 14);
            this.ComprasTxtNitProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtNitProveedor.MaxLength = 2147483647;
            this.ComprasTxtNitProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtNitProveedor.Name = "ComprasTxtNitProveedor";
            this.ComprasTxtNitProveedor.SelectedText = "";
            this.ComprasTxtNitProveedor.SelectionLength = 0;
            this.ComprasTxtNitProveedor.SelectionStart = 0;
            this.ComprasTxtNitProveedor.Size = new System.Drawing.Size(383, 36);
            this.ComprasTxtNitProveedor.TabIndex = 10;
            // 
            // ComprasLblTelefonoProveedor
            // 
            this.ComprasLblTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTelefonoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTelefonoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTelefonoProveedor.Location = new System.Drawing.Point(12, 62);
            this.ComprasLblTelefonoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblTelefonoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTelefonoProveedor.MostrarAsterisco = false;
            this.ComprasLblTelefonoProveedor.Name = "ComprasLblTelefonoProveedor";
            this.ComprasLblTelefonoProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblTelefonoProveedor.TabIndex = 2;
            this.ComprasLblTelefonoProveedor.TabStop = false;
            this.ComprasLblTelefonoProveedor.TamanoTipografia = 9F;
            this.ComprasLblTelefonoProveedor.Text = "Teléfono";
            this.ComprasLblTelefonoProveedor.Texto = "Teléfono";
            // 
            // ComprasTxtTelefonoProveedor
            // 
            this.ComprasTxtTelefonoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTelefonoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTelefonoProveedor.Enabled = false;
            this.ComprasTxtTelefonoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTelefonoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTelefonoProveedor.Location = new System.Drawing.Point(231, 62);
            this.ComprasTxtTelefonoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtTelefonoProveedor.MaxLength = 2147483647;
            this.ComprasTxtTelefonoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtTelefonoProveedor.Name = "ComprasTxtTelefonoProveedor";
            this.ComprasTxtTelefonoProveedor.SelectedText = "";
            this.ComprasTxtTelefonoProveedor.SelectionLength = 0;
            this.ComprasTxtTelefonoProveedor.SelectionStart = 0;
            this.ComprasTxtTelefonoProveedor.Size = new System.Drawing.Size(382, 36);
            this.ComprasTxtTelefonoProveedor.TabIndex = 8;
            // 
            // ComprasLblCorreoProveedor
            // 
            this.ComprasLblCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCorreoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCorreoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCorreoProveedor.Location = new System.Drawing.Point(621, 62);
            this.ComprasLblCorreoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblCorreoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCorreoProveedor.MostrarAsterisco = false;
            this.ComprasLblCorreoProveedor.Name = "ComprasLblCorreoProveedor";
            this.ComprasLblCorreoProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblCorreoProveedor.TabIndex = 3;
            this.ComprasLblCorreoProveedor.TabStop = false;
            this.ComprasLblCorreoProveedor.TamanoTipografia = 9F;
            this.ComprasLblCorreoProveedor.Text = "Correo";
            this.ComprasLblCorreoProveedor.Texto = "Correo";
            // 
            // ComprasTxtCorreoProveedor
            // 
            this.ComprasTxtCorreoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCorreoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtCorreoProveedor.Enabled = false;
            this.ComprasTxtCorreoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCorreoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCorreoProveedor.Location = new System.Drawing.Point(840, 62);
            this.ComprasTxtCorreoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtCorreoProveedor.MaxLength = 2147483647;
            this.ComprasTxtCorreoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtCorreoProveedor.Name = "ComprasTxtCorreoProveedor";
            this.ComprasTxtCorreoProveedor.SelectedText = "";
            this.ComprasTxtCorreoProveedor.SelectionLength = 0;
            this.ComprasTxtCorreoProveedor.SelectionStart = 0;
            this.ComprasTxtCorreoProveedor.Size = new System.Drawing.Size(383, 36);
            this.ComprasTxtCorreoProveedor.TabIndex = 9;
            // 
            // ComprasLblDireccionProveedor
            // 
            this.ComprasLblDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDireccionProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDireccionProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDireccionProveedor.Location = new System.Drawing.Point(12, 110);
            this.ComprasLblDireccionProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblDireccionProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDireccionProveedor.MostrarAsterisco = false;
            this.ComprasLblDireccionProveedor.Name = "ComprasLblDireccionProveedor";
            this.ComprasLblDireccionProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblDireccionProveedor.TabIndex = 4;
            this.ComprasLblDireccionProveedor.TabStop = false;
            this.ComprasLblDireccionProveedor.TamanoTipografia = 9F;
            this.ComprasLblDireccionProveedor.Text = "Dirección";
            this.ComprasLblDireccionProveedor.Texto = "Dirección";
            // 
            // ComprasTxtDireccionProveedor
            // 
            this.ComprasTxtDireccionProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDireccionProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtDireccionProveedor.Enabled = false;
            this.ComprasTxtDireccionProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDireccionProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDireccionProveedor.Location = new System.Drawing.Point(231, 110);
            this.ComprasTxtDireccionProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtDireccionProveedor.MaxLength = 2147483647;
            this.ComprasTxtDireccionProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtDireccionProveedor.Name = "ComprasTxtDireccionProveedor";
            this.ComprasTxtDireccionProveedor.SelectedText = "";
            this.ComprasTxtDireccionProveedor.SelectionLength = 0;
            this.ComprasTxtDireccionProveedor.SelectionStart = 0;
            this.ComprasTxtDireccionProveedor.Size = new System.Drawing.Size(382, 37);
            this.ComprasTxtDireccionProveedor.TabIndex = 9;
            // 
            // ComprasLblEstadoProveedor
            // 
            this.ComprasLblEstadoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstadoProveedor.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstadoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstadoProveedor.Location = new System.Drawing.Point(621, 110);
            this.ComprasLblEstadoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEstadoProveedor.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstadoProveedor.MostrarAsterisco = false;
            this.ComprasLblEstadoProveedor.Name = "ComprasLblEstadoProveedor";
            this.ComprasLblEstadoProveedor.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblEstadoProveedor.TabIndex = 3;
            this.ComprasLblEstadoProveedor.TabStop = false;
            this.ComprasLblEstadoProveedor.TamanoTipografia = 9F;
            this.ComprasLblEstadoProveedor.Text = "Estado";
            this.ComprasLblEstadoProveedor.Texto = "Estado";
            // 
            // ComprasTxtEstadoProveedor
            // 
            this.ComprasTxtEstadoProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtEstadoProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtEstadoProveedor.Enabled = false;
            this.ComprasTxtEstadoProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtEstadoProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtEstadoProveedor.Location = new System.Drawing.Point(840, 110);
            this.ComprasTxtEstadoProveedor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtEstadoProveedor.MaxLength = 2147483647;
            this.ComprasTxtEstadoProveedor.MostrarEtiquetaError = false;
            this.ComprasTxtEstadoProveedor.Name = "ComprasTxtEstadoProveedor";
            this.ComprasTxtEstadoProveedor.SelectedText = "";
            this.ComprasTxtEstadoProveedor.SelectionLength = 0;
            this.ComprasTxtEstadoProveedor.SelectionStart = 0;
            this.ComprasTxtEstadoProveedor.Size = new System.Drawing.Size(383, 37);
            this.ComprasTxtEstadoProveedor.TabIndex = 10;
            // 
            // ComprasGbxPlazo
            // 
            this.ComprasGbxPlazo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxPlazo.Controls.Add(this.ComprasTlpConsultaPlazo);
            this.ComprasGbxPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxPlazo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxPlazo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxPlazo.Location = new System.Drawing.Point(6, 306);
            this.ComprasGbxPlazo.Margin = new System.Windows.Forms.Padding(6);
            this.ComprasGbxPlazo.Name = "ComprasGbxPlazo";
            this.ComprasGbxPlazo.Size = new System.Drawing.Size(1241, 181);
            this.ComprasGbxPlazo.TabIndex = 2;
            this.ComprasGbxPlazo.TabStop = false;
            this.ComprasGbxPlazo.Text = "3. Orden y plazo";
            // 
            // ComprasTlpConsultaPlazo
            // 
            this.ComprasTlpConsultaPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpConsultaPlazo.ColumnCount = 4;
            this.ComprasTlpConsultaPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.ComprasTlpConsultaPlazo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblCodigoOrden, 0, 0);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasTxtCodigoOrden, 1, 0);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblEstado, 2, 0);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblEstadoValor, 3, 0);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblFechaDeEmision, 0, 1);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasDtpFechaEmision, 1, 1);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblFechaDeVencimiento, 2, 1);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasDtpFechaVencimiento, 3, 1);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblPlazo, 0, 2);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasTxtPlazoValor, 1, 2);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasLblDiasPlazo, 2, 2);
            this.ComprasTlpConsultaPlazo.Controls.Add(this.ComprasTxtDiasPlazo, 3, 2);
            this.ComprasTlpConsultaPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpConsultaPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpConsultaPlazo.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpConsultaPlazo.Name = "ComprasTlpConsultaPlazo";
            this.ComprasTlpConsultaPlazo.Padding = new System.Windows.Forms.Padding(8);
            this.ComprasTlpConsultaPlazo.RowCount = 3;
            this.ComprasTlpConsultaPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaPlazo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.ComprasTlpConsultaPlazo.Size = new System.Drawing.Size(1235, 154);
            this.ComprasTlpConsultaPlazo.TabIndex = 0;
            // 
            // ComprasLblCodigoOrden
            // 
            this.ComprasLblCodigoOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCodigoOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCodigoOrden.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCodigoOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCodigoOrden.Location = new System.Drawing.Point(12, 14);
            this.ComprasLblCodigoOrden.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblCodigoOrden.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCodigoOrden.MostrarAsterisco = false;
            this.ComprasLblCodigoOrden.Name = "ComprasLblCodigoOrden";
            this.ComprasLblCodigoOrden.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblCodigoOrden.TabIndex = 11;
            this.ComprasLblCodigoOrden.TabStop = false;
            this.ComprasLblCodigoOrden.TamanoTipografia = 9F;
            this.ComprasLblCodigoOrden.Text = "Código de orden";
            this.ComprasLblCodigoOrden.Texto = "Código de orden";
            // 
            // ComprasTxtCodigoOrden
            // 
            this.ComprasTxtCodigoOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCodigoOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtCodigoOrden.Enabled = false;
            this.ComprasTxtCodigoOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCodigoOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCodigoOrden.Location = new System.Drawing.Point(231, 14);
            this.ComprasTxtCodigoOrden.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtCodigoOrden.MaxLength = 2147483647;
            this.ComprasTxtCodigoOrden.MostrarEtiquetaError = false;
            this.ComprasTxtCodigoOrden.Name = "ComprasTxtCodigoOrden";
            this.ComprasTxtCodigoOrden.SelectedText = "";
            this.ComprasTxtCodigoOrden.SelectionLength = 0;
            this.ComprasTxtCodigoOrden.SelectionStart = 0;
            this.ComprasTxtCodigoOrden.Size = new System.Drawing.Size(382, 34);
            this.ComprasTxtCodigoOrden.TabIndex = 12;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(621, 14);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblEstado.TabIndex = 13;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 9F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.Texto = "Estado";
            // 
            // ComprasLblEstadoValor
            // 
            this.ComprasLblEstadoValor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstadoValor.ColorTexto = System.Drawing.Color.Red;
            this.ComprasLblEstadoValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstadoValor.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblEstadoValor.ForeColor = System.Drawing.Color.Red;
            this.ComprasLblEstadoValor.Location = new System.Drawing.Point(840, 14);
            this.ComprasLblEstadoValor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEstadoValor.MaximumSize = new System.Drawing.Size(1375, 29);
            this.ComprasLblEstadoValor.MostrarAsterisco = false;
            this.ComprasLblEstadoValor.Name = "ComprasLblEstadoValor";
            this.ComprasLblEstadoValor.Size = new System.Drawing.Size(383, 29);
            this.ComprasLblEstadoValor.TabIndex = 14;
            this.ComprasLblEstadoValor.TabStop = false;
            this.ComprasLblEstadoValor.TamanoTipografia = 12F;
            this.ComprasLblEstadoValor.Text = "PENDIENTE";
            this.ComprasLblEstadoValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstadoValor.Texto = "PENDIENTE";
            // 
            // ComprasLblFechaDeEmision
            // 
            this.ComprasLblFechaDeEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaDeEmision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaDeEmision.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaDeEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaDeEmision.Location = new System.Drawing.Point(12, 60);
            this.ComprasLblFechaDeEmision.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblFechaDeEmision.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaDeEmision.MostrarAsterisco = false;
            this.ComprasLblFechaDeEmision.Name = "ComprasLblFechaDeEmision";
            this.ComprasLblFechaDeEmision.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblFechaDeEmision.TabIndex = 2;
            this.ComprasLblFechaDeEmision.TabStop = false;
            this.ComprasLblFechaDeEmision.TamanoTipografia = 9F;
            this.ComprasLblFechaDeEmision.Text = "Fecha de Emisión";
            this.ComprasLblFechaDeEmision.Texto = "Fecha de Emisión";
            // 
            // ComprasDtpFechaEmision
            // 
            this.ComprasDtpFechaEmision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarFont = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasDtpFechaEmision.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaEmision.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(86)))), ((int)(((byte)(166)))));
            this.ComprasDtpFechaEmision.CustomFormat = "dd/MM/yyyy";
            this.ComprasDtpFechaEmision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ComprasDtpFechaEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaEmision.Location = new System.Drawing.Point(231, 60);
            this.ComprasDtpFechaEmision.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasDtpFechaEmision.Name = "ComprasDtpFechaEmision";
            this.ComprasDtpFechaEmision.Size = new System.Drawing.Size(382, 34);
            this.ComprasDtpFechaEmision.TabIndex = 15;
            this.ComprasDtpFechaEmision.TamanoFuente = 12F;
            // 
            // ComprasLblFechaDeVencimiento
            // 
            this.ComprasLblFechaDeVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFechaDeVencimiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaDeVencimiento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblFechaDeVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFechaDeVencimiento.Location = new System.Drawing.Point(621, 60);
            this.ComprasLblFechaDeVencimiento.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblFechaDeVencimiento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblFechaDeVencimiento.MostrarAsterisco = false;
            this.ComprasLblFechaDeVencimiento.Name = "ComprasLblFechaDeVencimiento";
            this.ComprasLblFechaDeVencimiento.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblFechaDeVencimiento.TabIndex = 3;
            this.ComprasLblFechaDeVencimiento.TabStop = false;
            this.ComprasLblFechaDeVencimiento.TamanoTipografia = 9F;
            this.ComprasLblFechaDeVencimiento.Text = "Fecha de Vencimiento";
            this.ComprasLblFechaDeVencimiento.Texto = "Fecha de Vencimiento";
            // 
            // ComprasDtpFechaVencimiento
            // 
            this.ComprasDtpFechaVencimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarFont = new System.Drawing.Font("Tahoma", 13F);
            this.ComprasDtpFechaVencimiento.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDtpFechaVencimiento.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(86)))), ((int)(((byte)(166)))));
            this.ComprasDtpFechaVencimiento.CustomFormat = "dd/MM/yyyy";
            this.ComprasDtpFechaVencimiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDtpFechaVencimiento.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.ComprasDtpFechaVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaVencimiento.Location = new System.Drawing.Point(840, 60);
            this.ComprasDtpFechaVencimiento.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasDtpFechaVencimiento.Name = "ComprasDtpFechaVencimiento";
            this.ComprasDtpFechaVencimiento.Size = new System.Drawing.Size(383, 34);
            this.ComprasDtpFechaVencimiento.TabIndex = 16;
            this.ComprasDtpFechaVencimiento.TamanoFuente = 12F;
            // 
            // ComprasLblPlazo
            // 
            this.ComprasLblPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblPlazo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPlazo.Location = new System.Drawing.Point(12, 106);
            this.ComprasLblPlazo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblPlazo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPlazo.MostrarAsterisco = false;
            this.ComprasLblPlazo.Name = "ComprasLblPlazo";
            this.ComprasLblPlazo.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblPlazo.TabIndex = 4;
            this.ComprasLblPlazo.TabStop = false;
            this.ComprasLblPlazo.TamanoTipografia = 9F;
            this.ComprasLblPlazo.Text = "Plazo";
            this.ComprasLblPlazo.Texto = "Plazo";
            // 
            // ComprasTxtPlazoValor
            // 
            this.ComprasTxtPlazoValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtPlazoValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtPlazoValor.Enabled = false;
            this.ComprasTxtPlazoValor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtPlazoValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtPlazoValor.Location = new System.Drawing.Point(231, 106);
            this.ComprasTxtPlazoValor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtPlazoValor.MaxLength = 2147483647;
            this.ComprasTxtPlazoValor.MostrarEtiquetaError = false;
            this.ComprasTxtPlazoValor.Name = "ComprasTxtPlazoValor";
            this.ComprasTxtPlazoValor.SelectedText = "";
            this.ComprasTxtPlazoValor.SelectionLength = 0;
            this.ComprasTxtPlazoValor.SelectionStart = 0;
            this.ComprasTxtPlazoValor.Size = new System.Drawing.Size(382, 34);
            this.ComprasTxtPlazoValor.TabIndex = 10;
            // 
            // ComprasLblDiasPlazo
            // 
            this.ComprasLblDiasPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDiasPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblDiasPlazo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDiasPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDiasPlazo.Location = new System.Drawing.Point(621, 106);
            this.ComprasLblDiasPlazo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblDiasPlazo.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDiasPlazo.MostrarAsterisco = false;
            this.ComprasLblDiasPlazo.Name = "ComprasLblDiasPlazo";
            this.ComprasLblDiasPlazo.Size = new System.Drawing.Size(211, 22);
            this.ComprasLblDiasPlazo.TabIndex = 13;
            this.ComprasLblDiasPlazo.TabStop = false;
            this.ComprasLblDiasPlazo.TamanoTipografia = 9F;
            this.ComprasLblDiasPlazo.Text = "Días de plazo";
            this.ComprasLblDiasPlazo.Texto = "Días de plazo";
            // 
            // ComprasTxtDiasPlazo
            // 
            this.ComprasTxtDiasPlazo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDiasPlazo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtDiasPlazo.Enabled = false;
            this.ComprasTxtDiasPlazo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDiasPlazo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDiasPlazo.Location = new System.Drawing.Point(840, 106);
            this.ComprasTxtDiasPlazo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasTxtDiasPlazo.MaxLength = 2147483647;
            this.ComprasTxtDiasPlazo.MostrarEtiquetaError = false;
            this.ComprasTxtDiasPlazo.Name = "ComprasTxtDiasPlazo";
            this.ComprasTxtDiasPlazo.SelectedText = "";
            this.ComprasTxtDiasPlazo.SelectionLength = 0;
            this.ComprasTxtDiasPlazo.SelectionStart = 0;
            this.ComprasTxtDiasPlazo.Size = new System.Drawing.Size(383, 34);
            this.ComprasTxtDiasPlazo.TabIndex = 14;
            // 
            // ComprasGbxFinalizacion
            // 
            this.ComprasGbxFinalizacion.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxFinalizacion.Controls.Add(this.ComprasTlpFinalizacion);
            this.ComprasGbxFinalizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxFinalizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxFinalizacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxFinalizacion.Location = new System.Drawing.Point(10, 1092);
            this.ComprasGbxFinalizacion.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxFinalizacion.Name = "ComprasGbxFinalizacion";
            this.ComprasGbxFinalizacion.Size = new System.Drawing.Size(1259, 237);
            this.ComprasGbxFinalizacion.TabIndex = 5;
            this.ComprasGbxFinalizacion.TabStop = false;
            this.ComprasGbxFinalizacion.Text = "5. Totales y comentario";
            // 
            // ComprasTlpFinalizacion
            // 
            this.ComprasTlpFinalizacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFinalizacion.ColumnCount = 2;
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFinalizacion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpImportes, 1, 0);
            this.ComprasTlpFinalizacion.Controls.Add(this.ComprasTlpTotales, 0, 0);
            this.ComprasTlpFinalizacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFinalizacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFinalizacion.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpFinalizacion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFinalizacion.Name = "ComprasTlpFinalizacion";
            this.ComprasTlpFinalizacion.RowCount = 1;
            this.ComprasTlpFinalizacion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFinalizacion.Size = new System.Drawing.Size(1253, 210);
            this.ComprasTlpFinalizacion.TabIndex = 3;
            // 
            // ComprasTlpImportes
            // 
            this.ComprasTlpImportes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpImportes.ColumnCount = 1;
            this.ComprasTlpImportes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpImportes.Controls.Add(this.ComprasLblEtiquetaComentario, 0, 0);
            this.ComprasTlpImportes.Controls.Add(this.ComprasTxtComentario, 0, 1);
            this.ComprasTlpImportes.Controls.Add(this.ComprasTlpComentarioYTotales, 0, 2);
            this.ComprasTlpImportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpImportes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpImportes.Location = new System.Drawing.Point(634, 0);
            this.ComprasTlpImportes.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ComprasTlpImportes.Name = "ComprasTlpImportes";
            this.ComprasTlpImportes.RowCount = 3;
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpImportes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpImportes.Size = new System.Drawing.Size(611, 210);
            this.ComprasTlpImportes.TabIndex = 6;
            // 
            // ComprasLblEtiquetaComentario
            // 
            this.ComprasLblEtiquetaComentario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblEtiquetaComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaComentario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaComentario.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblEtiquetaComentario.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaComentario.MostrarAsterisco = false;
            this.ComprasLblEtiquetaComentario.Name = "ComprasLblEtiquetaComentario";
            this.ComprasLblEtiquetaComentario.Size = new System.Drawing.Size(89, 18);
            this.ComprasLblEtiquetaComentario.TabIndex = 12;
            this.ComprasLblEtiquetaComentario.TabStop = false;
            this.ComprasLblEtiquetaComentario.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaComentario.Text = "Comentario";
            this.ComprasLblEtiquetaComentario.Texto = "Comentario";
            // 
            // ComprasTxtComentario
            // 
            this.ComprasTxtComentario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtComentario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtComentario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtComentario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtComentario.Location = new System.Drawing.Point(0, 24);
            this.ComprasTxtComentario.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.ComprasTxtComentario.MaxLength = 2147483647;
            this.ComprasTxtComentario.MostrarEtiquetaError = false;
            this.ComprasTxtComentario.Multiline = true;
            this.ComprasTxtComentario.Name = "ComprasTxtComentario";
            this.ComprasTxtComentario.SelectedText = "";
            this.ComprasTxtComentario.SelectionLength = 0;
            this.ComprasTxtComentario.SelectionStart = 0;
            this.ComprasTxtComentario.Size = new System.Drawing.Size(611, 88);
            this.ComprasTxtComentario.TabIndex = 9;
            // 
            // ComprasTlpComentarioYTotales
            // 
            this.ComprasTlpComentarioYTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpComentarioYTotales.ColumnCount = 4;
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpComentarioYTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnDescartar, 2, 0);
            this.ComprasTlpComentarioYTotales.Controls.Add(this.ComprasBtnRegresar, 1, 0);
            this.ComprasTlpComentarioYTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpComentarioYTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpComentarioYTotales.Location = new System.Drawing.Point(0, 120);
            this.ComprasTlpComentarioYTotales.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpComentarioYTotales.Name = "ComprasTlpComentarioYTotales";
            this.ComprasTlpComentarioYTotales.RowCount = 1;
            this.ComprasTlpComentarioYTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpComentarioYTotales.Size = new System.Drawing.Size(611, 90);
            this.ComprasTlpComentarioYTotales.TabIndex = 15;
            // 
            // ComprasBtnDescartar
            // 
            this.ComprasBtnDescartar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnDescartar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnDescartar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnDescartar.BackgroundImage")));
            this.ComprasBtnDescartar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnDescartar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnDescartar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnDescartar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnDescartar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnDescartar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnDescartar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnDescartar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnDescartar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnDescartar.Location = new System.Drawing.Point(310, 5);
            this.ComprasBtnDescartar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnDescartar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.Name = "ComprasBtnDescartar";
            this.ComprasBtnDescartar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnDescartar.TabIndex = 15;
            this.ComprasBtnDescartar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnRegresar
            // 
            this.ComprasBtnRegresar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnRegresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnRegresar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnRegresar.BackgroundImage")));
            this.ComprasBtnRegresar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnRegresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnRegresar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRegresar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnRegresar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnRegresar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnRegresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnRegresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnRegresar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRegresar.Location = new System.Drawing.Point(220, 5);
            this.ComprasBtnRegresar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnRegresar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.Name = "ComprasBtnRegresar";
            this.ComprasBtnRegresar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnRegresar.TabIndex = 16;
            this.ComprasBtnRegresar.UseVisualStyleBackColor = false;
            // 
            // ComprasTlpTotales
            // 
            this.ComprasTlpTotales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotales.ColumnCount = 2;
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.ComprasTlpTotales.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaSubtotal, 0, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtSubtotal, 1, 0);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaDescuento, 0, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtDescuentoTotal, 1, 1);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaImpuesto, 0, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtImpuesto, 1, 2);
            this.ComprasTlpTotales.Controls.Add(this.ComprasLblEtiquetaTotal, 0, 3);
            this.ComprasTlpTotales.Controls.Add(this.ComprasTxtTotalDeLaOrden, 1, 3);
            this.ComprasTlpTotales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpTotales.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotales.Location = new System.Drawing.Point(8, 0);
            this.ComprasTlpTotales.Margin = new System.Windows.Forms.Padding(8, 0, 16, 0);
            this.ComprasTlpTotales.Name = "ComprasTlpTotales";
            this.ComprasTlpTotales.RowCount = 6;
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotales.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotales.Size = new System.Drawing.Size(602, 210);
            this.ComprasTlpTotales.TabIndex = 5;
            // 
            // ComprasLblEtiquetaSubtotal
            // 
            this.ComprasLblEtiquetaSubtotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaSubtotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaSubtotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaSubtotal.Location = new System.Drawing.Point(3, 3);
            this.ComprasLblEtiquetaSubtotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaSubtotal.MostrarAsterisco = false;
            this.ComprasLblEtiquetaSubtotal.Name = "ComprasLblEtiquetaSubtotal";
            this.ComprasLblEtiquetaSubtotal.Size = new System.Drawing.Size(264, 22);
            this.ComprasLblEtiquetaSubtotal.TabIndex = 2;
            this.ComprasLblEtiquetaSubtotal.TabStop = false;
            this.ComprasLblEtiquetaSubtotal.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaSubtotal.Text = "Subtotal";
            this.ComprasLblEtiquetaSubtotal.Texto = "Subtotal";
            // 
            // ComprasTxtSubtotal
            // 
            this.ComprasTxtSubtotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtSubtotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtSubtotal.Enabled = false;
            this.ComprasTxtSubtotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtSubtotal.Location = new System.Drawing.Point(272, 2);
            this.ComprasTxtSubtotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtSubtotal.MaxLength = 2147483647;
            this.ComprasTxtSubtotal.MostrarEtiquetaError = false;
            this.ComprasTxtSubtotal.Name = "ComprasTxtSubtotal";
            this.ComprasTxtSubtotal.SelectedText = "";
            this.ComprasTxtSubtotal.SelectionLength = 0;
            this.ComprasTxtSubtotal.SelectionStart = 0;
            this.ComprasTxtSubtotal.Size = new System.Drawing.Size(328, 36);
            this.ComprasTxtSubtotal.TabIndex = 8;
            // 
            // ComprasLblEtiquetaDescuento
            // 
            this.ComprasLblEtiquetaDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaDescuento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaDescuento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaDescuento.Location = new System.Drawing.Point(3, 43);
            this.ComprasLblEtiquetaDescuento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaDescuento.MostrarAsterisco = false;
            this.ComprasLblEtiquetaDescuento.Name = "ComprasLblEtiquetaDescuento";
            this.ComprasLblEtiquetaDescuento.Size = new System.Drawing.Size(264, 22);
            this.ComprasLblEtiquetaDescuento.TabIndex = 1;
            this.ComprasLblEtiquetaDescuento.TabStop = false;
            this.ComprasLblEtiquetaDescuento.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaDescuento.Text = "Descuento Total";
            this.ComprasLblEtiquetaDescuento.Texto = "Descuento Total";
            // 
            // ComprasTxtDescuentoTotal
            // 
            this.ComprasTxtDescuentoTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuentoTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtDescuentoTotal.Enabled = false;
            this.ComprasTxtDescuentoTotal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuentoTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuentoTotal.Location = new System.Drawing.Point(272, 42);
            this.ComprasTxtDescuentoTotal.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuentoTotal.MaxLength = 2147483647;
            this.ComprasTxtDescuentoTotal.MostrarEtiquetaError = false;
            this.ComprasTxtDescuentoTotal.Name = "ComprasTxtDescuentoTotal";
            this.ComprasTxtDescuentoTotal.SelectedText = "";
            this.ComprasTxtDescuentoTotal.SelectionLength = 0;
            this.ComprasTxtDescuentoTotal.SelectionStart = 0;
            this.ComprasTxtDescuentoTotal.Size = new System.Drawing.Size(328, 36);
            this.ComprasTxtDescuentoTotal.TabIndex = 5;
            // 
            // ComprasLblEtiquetaImpuesto
            // 
            this.ComprasLblEtiquetaImpuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaImpuesto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaImpuesto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaImpuesto.Location = new System.Drawing.Point(3, 83);
            this.ComprasLblEtiquetaImpuesto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaImpuesto.MostrarAsterisco = false;
            this.ComprasLblEtiquetaImpuesto.Name = "ComprasLblEtiquetaImpuesto";
            this.ComprasLblEtiquetaImpuesto.Size = new System.Drawing.Size(264, 22);
            this.ComprasLblEtiquetaImpuesto.TabIndex = 1;
            this.ComprasLblEtiquetaImpuesto.TabStop = false;
            this.ComprasLblEtiquetaImpuesto.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaImpuesto.Text = "Impuesto";
            this.ComprasLblEtiquetaImpuesto.Texto = "Impuesto";
            // 
            // ComprasTxtImpuesto
            // 
            this.ComprasTxtImpuesto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtImpuesto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtImpuesto.Enabled = false;
            this.ComprasTxtImpuesto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtImpuesto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtImpuesto.Location = new System.Drawing.Point(272, 82);
            this.ComprasTxtImpuesto.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtImpuesto.MaxLength = 2147483647;
            this.ComprasTxtImpuesto.MostrarEtiquetaError = false;
            this.ComprasTxtImpuesto.Name = "ComprasTxtImpuesto";
            this.ComprasTxtImpuesto.SelectedText = "";
            this.ComprasTxtImpuesto.SelectionLength = 0;
            this.ComprasTxtImpuesto.SelectionStart = 0;
            this.ComprasTxtImpuesto.Size = new System.Drawing.Size(328, 36);
            this.ComprasTxtImpuesto.TabIndex = 5;
            // 
            // ComprasLblEtiquetaTotal
            // 
            this.ComprasLblEtiquetaTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEtiquetaTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEtiquetaTotal.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEtiquetaTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEtiquetaTotal.Location = new System.Drawing.Point(3, 123);
            this.ComprasLblEtiquetaTotal.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblEtiquetaTotal.MostrarAsterisco = false;
            this.ComprasLblEtiquetaTotal.Name = "ComprasLblEtiquetaTotal";
            this.ComprasLblEtiquetaTotal.Size = new System.Drawing.Size(264, 22);
            this.ComprasLblEtiquetaTotal.TabIndex = 4;
            this.ComprasLblEtiquetaTotal.TabStop = false;
            this.ComprasLblEtiquetaTotal.TamanoTipografia = 9F;
            this.ComprasLblEtiquetaTotal.Text = "Total de la orden";
            this.ComprasLblEtiquetaTotal.Texto = "Total de la orden";
            // 
            // ComprasTxtTotalDeLaOrden
            // 
            this.ComprasTxtTotalDeLaOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtTotalDeLaOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTxtTotalDeLaOrden.Enabled = false;
            this.ComprasTxtTotalDeLaOrden.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtTotalDeLaOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtTotalDeLaOrden.Location = new System.Drawing.Point(272, 122);
            this.ComprasTxtTotalDeLaOrden.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtTotalDeLaOrden.MaxLength = 2147483647;
            this.ComprasTxtTotalDeLaOrden.MostrarEtiquetaError = false;
            this.ComprasTxtTotalDeLaOrden.Name = "ComprasTxtTotalDeLaOrden";
            this.ComprasTxtTotalDeLaOrden.SelectedText = "";
            this.ComprasTxtTotalDeLaOrden.SelectionLength = 0;
            this.ComprasTxtTotalDeLaOrden.SelectionStart = 0;
            this.ComprasTxtTotalDeLaOrden.Size = new System.Drawing.Size(328, 36);
            this.ComprasTxtTotalDeLaOrden.TabIndex = 10;
            // 
            // ComprasGbxDetalles
            // 
            this.ComprasGbxDetalles.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxDetalles.Controls.Add(this.ComprasTlpDetalles);
            this.ComprasGbxDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxDetalles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxDetalles.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxDetalles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxDetalles.Location = new System.Drawing.Point(10, 700);
            this.ComprasGbxDetalles.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasGbxDetalles.Name = "ComprasGbxDetalles";
            this.ComprasGbxDetalles.Size = new System.Drawing.Size(1259, 372);
            this.ComprasGbxDetalles.TabIndex = 6;
            this.ComprasGbxDetalles.TabStop = false;
            this.ComprasGbxDetalles.Text = "4. Productos de la orden";
            // 
            // ComprasTlpDetalles
            // 
            this.ComprasTlpDetalles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDetalles.ColumnCount = 1;
            this.ComprasTlpDetalles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Controls.Add(this.ComprasPnlArticulos, 0, 1);
            this.ComprasTlpDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDetalles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDetalles.Location = new System.Drawing.Point(3, 24);
            this.ComprasTlpDetalles.Name = "ComprasTlpDetalles";
            this.ComprasTlpDetalles.RowCount = 2;
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.ComprasTlpDetalles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDetalles.Size = new System.Drawing.Size(1253, 345);
            this.ComprasTlpDetalles.TabIndex = 0;
            // 
            // ComprasPnlArticulos
            // 
            this.ComprasPnlArticulos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlArticulos.Controls.Add(this.ComprasDgvProductos);
            this.ComprasPnlArticulos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlArticulos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlArticulos.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlArticulos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlArticulos.Name = "ComprasPnlArticulos";
            this.ComprasPnlArticulos.Size = new System.Drawing.Size(1253, 345);
            this.ComprasPnlArticulos.TabIndex = 0;
            // 
            // ComprasDgvProductos
            // 
            this.ComprasDgvProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasDgvProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvProductos.MinimumSize = new System.Drawing.Size(1274, 205);
            this.ComprasDgvProductos.Name = "ComprasDgvProductos";
            this.ComprasDgvProductos.PaginacionHabilitada = false;
            this.ComprasDgvProductos.Size = new System.Drawing.Size(1274, 345);
            this.ComprasDgvProductos.TabIndex = 0;
            // 
            // ComprasUsrEncabezado
            // 
            this.ComprasUsrEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezado.Codigo = "6011";
            this.ComprasUsrEncabezado.Descripcion = "Consulta los datos y productos de la orden de compra. Regresa al listado o anula " +
    "la orden.";
            this.ComprasUsrEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrEncabezado.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.Name = "ComprasUsrEncabezado";
            this.ComprasUsrEncabezado.PaginaInicialAyuda = "DetallesOrdenesCompra.html";
            this.ComprasUsrEncabezado.RutaReporte = "RpReporteDetalleOrdenCompra.rdlc";
            this.ComprasUsrEncabezado.Size = new System.Drawing.Size(1279, 130);
            this.ComprasUsrEncabezado.TabIndex = 7;
            this.ComprasUsrEncabezado.TabStop = false;
            this.ComprasUsrEncabezado.Titulo = "DETALLE DE ORDEN DE COMPRA";
            // 
            // ComprasTlpDatosOrden
            // 
            this.ComprasTlpDatosOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDatosOrden.ColumnCount = 2;
            this.ComprasTlpDatosOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.ComprasTlpDatosOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatosOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpDatosOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDatosOrden.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpDatosOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDatosOrden.Name = "ComprasTlpDatosOrden";
            this.ComprasTlpDatosOrden.RowCount = 6;
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpDatosOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpDatosOrden.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpDatosOrden.TabIndex = 5;
            // 
            // ComprasTlpProveedor
            // 
            this.ComprasTlpProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpProveedor.ColumnCount = 2;
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpProveedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedor.Controls.Add(this.ComprasLblProveedor, 1, 0);
            this.ComprasTlpProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpProveedor.Location = new System.Drawing.Point(420, 0);
            this.ComprasTlpProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpProveedor.Name = "ComprasTlpProveedor";
            this.ComprasTlpProveedor.RowCount = 6;
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpProveedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpProveedor.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpProveedor.TabIndex = 4;
            // 
            // ComprasLblProveedor
            // 
            this.ComprasLblProveedor.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ComprasLblProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProveedor.Font = new System.Drawing.Font("Tahoma", 10.8F);
            this.ComprasLblProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProveedor.Location = new System.Drawing.Point(94, 9);
            this.ComprasLblProveedor.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasLblProveedor.MaximumSize = new System.Drawing.Size(1250, 27);
            this.ComprasLblProveedor.MostrarAsterisco = false;
            this.ComprasLblProveedor.Name = "ComprasLblProveedor";
            this.ComprasLblProveedor.Size = new System.Drawing.Size(302, 22);
            this.ComprasLblProveedor.TabIndex = 7;
            this.ComprasLblProveedor.TabStop = false;
            this.ComprasLblProveedor.TamanoTipografia = 10.8F;
            this.ComprasLblProveedor.Text = "PROVEEDOR";
            this.ComprasLblProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblProveedor.Texto = "PROVEEDOR";
            // 
            // ComprasTlpResponsable
            // 
            this.ComprasTlpResponsable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpResponsable.ColumnCount = 2;
            this.ComprasTlpResponsable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpResponsable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResponsable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpResponsable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpResponsable.Location = new System.Drawing.Point(840, 0);
            this.ComprasTlpResponsable.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpResponsable.Name = "ComprasTlpResponsable";
            this.ComprasTlpResponsable.RowCount = 6;
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpResponsable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpResponsable.Size = new System.Drawing.Size(400, 207);
            this.ComprasTlpResponsable.TabIndex = 3;
            // 
            // ComprasBtnGuardar
            // 
            this.ComprasBtnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ComprasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnGuardar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnGuardar.BackgroundImage")));
            this.ComprasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnGuardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnGuardar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnGuardar.ImagenDeshabilitado")));
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(115, 70);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnGuardar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.TabIndex = 10;
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            this.ComprasBtnGuardar.Visible = false;
            // 
            // ComprasTlpMantenimientoProductos
            // 
            this.ComprasTlpMantenimientoProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpMantenimientoProductos.ColumnCount = 2;
            this.ComprasTlpMantenimientoProductos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.ComprasTlpMantenimientoProductos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.ComprasTlpMantenimientoProductos.Controls.Add(this.ComprasTlpDetallesProducto, 0, 0);
            this.ComprasTlpMantenimientoProductos.Controls.Add(this.ComprasTlpAccionesArticulo, 1, 0);
            this.ComprasTlpMantenimientoProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMantenimientoProductos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpMantenimientoProductos.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpMantenimientoProductos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpMantenimientoProductos.Name = "ComprasTlpMantenimientoProductos";
            this.ComprasTlpMantenimientoProductos.RowCount = 1;
            this.ComprasTlpMantenimientoProductos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMantenimientoProductos.Size = new System.Drawing.Size(1274, 140);
            this.ComprasTlpMantenimientoProductos.TabIndex = 1;
            this.ComprasTlpMantenimientoProductos.Visible = false;
            // 
            // ComprasTlpDetallesProducto
            // 
            this.ComprasTlpDetallesProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasTlpDetallesProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpDetallesProducto.ColumnCount = 1;
            this.ComprasTlpDetallesProducto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasFlpPrecioCantidadDescuento, 0, 2);
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasFlpProductoSku, 0, 1);
            this.ComprasTlpDetallesProducto.Controls.Add(this.ComprasTlpTotalesArticulo, 0, 3);
            this.ComprasTlpDetallesProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpDetallesProducto.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpDetallesProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpDetallesProducto.Name = "ComprasTlpDetallesProducto";
            this.ComprasTlpDetallesProducto.RowCount = 5;
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpDetallesProducto.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpDetallesProducto.Size = new System.Drawing.Size(891, 140);
            this.ComprasTlpDetallesProducto.TabIndex = 7;
            // 
            // ComprasFlpPrecioCantidadDescuento
            // 
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasBtnTogglePorcentaje);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasTxtDescuento);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblDescuento);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasTxtCantidad);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblCantidad);
            this.ComprasFlpPrecioCantidadDescuento.Controls.Add(this.ComprasLblPrecioUnitario);
            this.ComprasFlpPrecioCantidadDescuento.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasFlpPrecioCantidadDescuento.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ComprasFlpPrecioCantidadDescuento.Location = new System.Drawing.Point(96, 50);
            this.ComprasFlpPrecioCantidadDescuento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpPrecioCantidadDescuento.Name = "ComprasFlpPrecioCantidadDescuento";
            this.ComprasFlpPrecioCantidadDescuento.Size = new System.Drawing.Size(795, 40);
            this.ComprasFlpPrecioCantidadDescuento.TabIndex = 7;
            this.ComprasBtnTogglePorcentaje.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnTogglePorcentaje
            // 
            this.ComprasBtnTogglePorcentaje.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnTogglePorcentaje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnTogglePorcentaje.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnTogglePorcentaje.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnTogglePorcentaje.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnTogglePorcentaje.FlatAppearance.BorderSize = 0;
            this.ComprasBtnTogglePorcentaje.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnTogglePorcentaje.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnTogglePorcentaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnTogglePorcentaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnTogglePorcentaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnTogglePorcentaje.Location = new System.Drawing.Point(761, 3);
            this.ComprasBtnTogglePorcentaje.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnTogglePorcentaje.Name = "ComprasBtnTogglePorcentaje";
            this.ComprasBtnTogglePorcentaje.Size = new System.Drawing.Size(34, 34);
            this.ComprasBtnTogglePorcentaje.TabIndex = 7;
            this.ComprasBtnTogglePorcentaje.Text = "%";
            this.ComprasBtnTogglePorcentaje.UseVisualStyleBackColor = false;
            // 
            // ComprasTxtDescuento
            // 
            this.ComprasTxtDescuento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescuento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescuento.Location = new System.Drawing.Point(603, 2);
            this.ComprasTxtDescuento.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtDescuento.MaxLength = 2147483647;
            this.ComprasTxtDescuento.ModoMoneda = true;
            this.ComprasTxtDescuento.MostrarEtiquetaError = false;
            this.ComprasTxtDescuento.Name = "ComprasTxtDescuento";
            this.ComprasTxtDescuento.SelectedText = "";
            this.ComprasTxtDescuento.SelectionLength = 0;
            this.ComprasTxtDescuento.SelectionStart = 0;
            this.ComprasTxtDescuento.Size = new System.Drawing.Size(156, 36);
            this.ComprasTxtDescuento.TabIndex = 6;
            this.ComprasTxtDescuento.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Moneda;
            // 
            // ComprasLblDescuento
            // 
            this.ComprasLblDescuento.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblDescuento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescuento.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblDescuento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescuento.Location = new System.Drawing.Point(511, 11);
            this.ComprasLblDescuento.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblDescuento.MostrarAsterisco = false;
            this.ComprasLblDescuento.Name = "ComprasLblDescuento";
            this.ComprasLblDescuento.Size = new System.Drawing.Size(87, 18);
            this.ComprasLblDescuento.TabIndex = 21;
            this.ComprasLblDescuento.TabStop = false;
            this.ComprasLblDescuento.TamanoTipografia = 9F;
            this.ComprasLblDescuento.Text = "Descuento";
            this.ComprasLblDescuento.Texto = "Descuento";
            // 
            // ComprasTxtCantidad
            // 
            this.ComprasTxtCantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtCantidad.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtCantidad.Location = new System.Drawing.Point(350, 2);
            this.ComprasTxtCantidad.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtCantidad.MaxLength = 2147483647;
            this.ComprasTxtCantidad.MostrarEtiquetaError = false;
            this.ComprasTxtCantidad.Name = "ComprasTxtCantidad";
            this.ComprasTxtCantidad.SelectedText = "";
            this.ComprasTxtCantidad.SelectionLength = 0;
            this.ComprasTxtCantidad.SelectionStart = 0;
            this.ComprasTxtCantidad.Size = new System.Drawing.Size(156, 36);
            this.ComprasTxtCantidad.SoloEnteros = true;
            this.ComprasTxtCantidad.TabIndex = 5;
            this.ComprasTxtCantidad.TipoEntrada = CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada.Enteros;
            // 
            // ComprasLblCantidad
            // 
            this.ComprasLblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblCantidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCantidad.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblCantidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCantidad.Location = new System.Drawing.Point(251, 11);
            this.ComprasLblCantidad.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblCantidad.Name = "ComprasLblCantidad";
            this.ComprasLblCantidad.Size = new System.Drawing.Size(94, 18);
            this.ComprasLblCantidad.TabIndex = 23;
            this.ComprasLblCantidad.TabStop = false;
            this.ComprasLblCantidad.TamanoTipografia = 9F;
            this.ComprasLblCantidad.Text = "Cantidad";
            this.ComprasLblCantidad.Texto = "Cantidad";
            // 
            // ComprasLblPrecioUnitario
            // 
            this.ComprasLblPrecioUnitario.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblPrecioUnitario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPrecioUnitario.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblPrecioUnitario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPrecioUnitario.Location = new System.Drawing.Point(19, 11);
            this.ComprasLblPrecioUnitario.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.ComprasLblPrecioUnitario.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblPrecioUnitario.MostrarAsterisco = false;
            this.ComprasLblPrecioUnitario.Name = "ComprasLblPrecioUnitario";
            this.ComprasLblPrecioUnitario.Size = new System.Drawing.Size(220, 18);
            this.ComprasLblPrecioUnitario.TabIndex = 24;
            this.ComprasLblPrecioUnitario.TabStop = false;
            this.ComprasLblPrecioUnitario.TamanoTipografia = 9F;
            this.ComprasLblPrecioUnitario.Text = "Costo unitario";
            this.ComprasLblPrecioUnitario.Texto = "Costo unitario";
            // 
            // ComprasFlpProductoSku
            // 
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasCboSku);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasLblSku);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasCboProducto);
            this.ComprasFlpProductoSku.Controls.Add(this.ComprasLblProducto);
            this.ComprasFlpProductoSku.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasFlpProductoSku.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ComprasFlpProductoSku.Location = new System.Drawing.Point(27, 10);
            this.ComprasFlpProductoSku.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpProductoSku.Name = "ComprasFlpProductoSku";
            this.ComprasFlpProductoSku.Size = new System.Drawing.Size(864, 40);
            this.ComprasFlpProductoSku.TabIndex = 8;
            // 
            // ComprasCboSku
            // 
            this.ComprasCboSku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboSku.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboSku.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboSku.Location = new System.Drawing.Point(664, 0);
            this.ComprasCboSku.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboSku.MaxLength = 0;
            this.ComprasCboSku.MostrarEtiquetaError = false;
            this.ComprasCboSku.Name = "ComprasCboSku";
            this.ComprasCboSku.Size = new System.Drawing.Size(200, 40);
            this.ComprasCboSku.TabIndex = 4;
            // 
            // ComprasLblSku
            // 
            this.ComprasLblSku.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblSku.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblSku.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblSku.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblSku.Location = new System.Drawing.Point(577, 11);
            this.ComprasLblSku.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblSku.Name = "ComprasLblSku";
            this.ComprasLblSku.Size = new System.Drawing.Size(84, 18);
            this.ComprasLblSku.TabIndex = 23;
            this.ComprasLblSku.TabStop = false;
            this.ComprasLblSku.TamanoTipografia = 9F;
            this.ComprasLblSku.Text = "SKU";
            this.ComprasLblSku.Texto = "SKU";
            // 
            // ComprasCboProducto
            // 
            this.ComprasCboProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProducto.Location = new System.Drawing.Point(154, 0);
            this.ComprasCboProducto.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProducto.MaxLength = 0;
            this.ComprasCboProducto.MostrarEtiquetaError = false;
            this.ComprasCboProducto.Name = "ComprasCboProducto";
            this.ComprasCboProducto.Size = new System.Drawing.Size(420, 40);
            this.ComprasCboProducto.TabIndex = 3;
            // 
            // ComprasLblProducto
            // 
            this.ComprasLblProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblProducto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblProducto.Location = new System.Drawing.Point(27, 11);
            this.ComprasLblProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblProducto.Name = "ComprasLblProducto";
            this.ComprasLblProducto.Size = new System.Drawing.Size(124, 18);
            this.ComprasLblProducto.TabIndex = 25;
            this.ComprasLblProducto.TabStop = false;
            this.ComprasLblProducto.TamanoTipografia = 9F;
            this.ComprasLblProducto.Text = "Producto";
            this.ComprasLblProducto.Texto = "Producto";
            // 
            // ComprasTlpTotalesArticulo
            // 
            this.ComprasTlpTotalesArticulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpTotalesArticulo.ColumnCount = 1;
            this.ComprasTlpTotalesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpTotalesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpTotalesArticulo.Controls.Add(this.ComprasLblTotalesDeProducto, 0, 0);
            this.ComprasTlpTotalesArticulo.Dock = System.Windows.Forms.DockStyle.Right;
            this.ComprasTlpTotalesArticulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpTotalesArticulo.Location = new System.Drawing.Point(51, 90);
            this.ComprasTlpTotalesArticulo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpTotalesArticulo.Name = "ComprasTlpTotalesArticulo";
            this.ComprasTlpTotalesArticulo.RowCount = 1;
            this.ComprasTlpTotalesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpTotalesArticulo.Size = new System.Drawing.Size(840, 40);
            this.ComprasTlpTotalesArticulo.TabIndex = 19;
            // 
            // ComprasLblTotalesDeProducto
            // 
            this.ComprasLblTotalesDeProducto.AlineacionTexto = System.Drawing.ContentAlignment.BottomRight;
            this.ComprasLblTotalesDeProducto.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ComprasLblTotalesDeProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTotalesDeProducto.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblTotalesDeProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTotalesDeProducto.Location = new System.Drawing.Point(48, 9);
            this.ComprasLblTotalesDeProducto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComprasLblTotalesDeProducto.MaximumSize = new System.Drawing.Size(1000, 22);
            this.ComprasLblTotalesDeProducto.MostrarAsterisco = false;
            this.ComprasLblTotalesDeProducto.Name = "ComprasLblTotalesDeProducto";
            this.ComprasLblTotalesDeProducto.Size = new System.Drawing.Size(789, 22);
            this.ComprasLblTotalesDeProducto.TabIndex = 3;
            this.ComprasLblTotalesDeProducto.TabStop = false;
            this.ComprasLblTotalesDeProducto.TamanoTipografia = 10F;
            this.ComprasLblTotalesDeProducto.Text = "";
            this.ComprasLblTotalesDeProducto.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.ComprasLblTotalesDeProducto.Texto = "";
            // 
            // ComprasTlpAccionesArticulo
            // 
            this.ComprasTlpAccionesArticulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpAccionesArticulo.ColumnCount = 1;
            this.ComprasTlpAccionesArticulo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpAccionesArticulo.Controls.Add(this.ComprasFlpAccionesOrden, 0, 1);
            this.ComprasTlpAccionesArticulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpAccionesArticulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpAccionesArticulo.Location = new System.Drawing.Point(894, 3);
            this.ComprasTlpAccionesArticulo.Name = "ComprasTlpAccionesArticulo";
            this.ComprasTlpAccionesArticulo.RowCount = 3;
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpAccionesArticulo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpAccionesArticulo.Size = new System.Drawing.Size(377, 134);
            this.ComprasTlpAccionesArticulo.TabIndex = 6;
            // 
            // ComprasFlpAccionesOrden
            // 
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnIngresarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnModificarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnEliminarProducto);
            this.ComprasFlpAccionesOrden.Controls.Add(this.ComprasBtnRefrescarProducto);
            this.ComprasFlpAccionesOrden.Dock = System.Windows.Forms.DockStyle.Left;
            this.ComprasFlpAccionesOrden.Location = new System.Drawing.Point(0, 27);
            this.ComprasFlpAccionesOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAccionesOrden.Name = "ComprasFlpAccionesOrden";
            this.ComprasFlpAccionesOrden.Size = new System.Drawing.Size(377, 80);
            this.ComprasFlpAccionesOrden.TabIndex = 0;
            // 
            // ComprasBtnIngresarProducto
            // 
            this.ComprasBtnIngresarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnIngresarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnIngresarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIngresarProducto.BackgroundImage")));
            this.ComprasBtnIngresarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnIngresarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIngresarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIngresarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnIngresarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnIngresarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIngresarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIngresarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarProducto.Location = new System.Drawing.Point(10, 0);
            this.ComprasBtnIngresarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnIngresarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.Name = "ComprasBtnIngresarProducto";
            this.ComprasBtnIngresarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarProducto.TabIndex = 8;
            this.ComprasBtnIngresarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnModificarProducto
            // 
            this.ComprasBtnModificarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnModificarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnModificarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnModificarProducto.BackgroundImage")));
            this.ComprasBtnModificarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnModificarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnModificarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnModificarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnModificarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnModificarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnModificarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnModificarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnModificarProducto.Location = new System.Drawing.Point(100, 0);
            this.ComprasBtnModificarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnModificarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.Name = "ComprasBtnModificarProducto";
            this.ComprasBtnModificarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnModificarProducto.TabIndex = 1;
            this.ComprasBtnModificarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnEliminarProducto
            // 
            this.ComprasBtnEliminarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnEliminarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEliminarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEliminarProducto.BackgroundImage")));
            this.ComprasBtnEliminarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEliminarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnEliminarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnEliminarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEliminarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEliminarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEliminarProducto.Location = new System.Drawing.Point(190, 0);
            this.ComprasBtnEliminarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnEliminarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.Name = "ComprasBtnEliminarProducto";
            this.ComprasBtnEliminarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEliminarProducto.TabIndex = 2;
            this.ComprasBtnEliminarProducto.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnRefrescarProducto
            // 
            this.ComprasBtnRefrescarProducto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnRefrescarProducto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnRefrescarProducto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnRefrescarProducto.BackgroundImage")));
            this.ComprasBtnRefrescarProducto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnRefrescarProducto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnRefrescarProducto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRefrescarProducto.FlatAppearance.BorderSize = 0;
            this.ComprasBtnRefrescarProducto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnRefrescarProducto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnRefrescarProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnRefrescarProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnRefrescarProducto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnRefrescarProducto.Location = new System.Drawing.Point(280, 0);
            this.ComprasBtnRefrescarProducto.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ComprasBtnRefrescarProducto.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.Name = "ComprasBtnRefrescarProducto";
            this.ComprasBtnRefrescarProducto.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnRefrescarProducto.TabIndex = 3;
            this.ComprasBtnRefrescarProducto.UseVisualStyleBackColor = false;
            // 
            // FrmDetalleOrdenCompra
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 760);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmDetalleOrdenCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6011 - Detalle de orden de compra";
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasGbxEncabezado.ResumeLayout(false);
            this.ComprasTlpEncabezado.ResumeLayout(false);
            this.ComprasGbxMoneda.ResumeLayout(false);
            this.ComprasTlpConsultaMoneda.ResumeLayout(false);
            this.ComprasGbxProveedor.ResumeLayout(false);
            this.ComprasTlpConsultaProveedor.ResumeLayout(false);
            this.ComprasGbxPlazo.ResumeLayout(false);
            this.ComprasTlpConsultaPlazo.ResumeLayout(false);
            this.ComprasGbxFinalizacion.ResumeLayout(false);
            this.ComprasTlpFinalizacion.ResumeLayout(false);
            this.ComprasTlpImportes.ResumeLayout(false);
            this.ComprasTlpComentarioYTotales.ResumeLayout(false);
            this.ComprasTlpTotales.ResumeLayout(false);
            this.ComprasGbxDetalles.ResumeLayout(false);
            this.ComprasTlpDetalles.ResumeLayout(false);
            this.ComprasPnlArticulos.ResumeLayout(false);
            this.ComprasTlpProveedor.ResumeLayout(false);
            this.ComprasTlpMantenimientoProductos.ResumeLayout(false);
            this.ComprasTlpDetallesProducto.ResumeLayout(false);
            this.ComprasFlpPrecioCantidadDescuento.ResumeLayout(false);
            this.ComprasFlpProductoSku.ResumeLayout(false);
            this.ComprasTlpTotalesArticulo.ResumeLayout(false);
            this.ComprasTlpAccionesArticulo.ResumeLayout(false);
            this.ComprasFlpAccionesOrden.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ClsGroupBoxCompras ComprasGbxMoneda;
        private ClsTableLayoutPanelCompras ComprasTlpConsultaMoneda;
        private ClsGroupBoxCompras ComprasGbxProveedor;
        private ClsTableLayoutPanelCompras ComprasTlpConsultaProveedor;
        private ClsGroupBoxCompras ComprasGbxPlazo;
        private ClsTableLayoutPanelCompras ComprasTlpConsultaPlazo;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxEncabezado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpEncabezado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpResponsable;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDireccionProveedor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNombreProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtNitProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCorreoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTelefonoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtEstadoProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDireccionProveedor;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFinalizacion;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpImportes;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotales;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaMoneda;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTotalDeLaOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtTipoCambio;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtSubtotal;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtImpuesto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaImpuesto;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuentoTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaSubtotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTipoCambio;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaTotal;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEtiquetaComentario;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxDetalles;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDetalles;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlArticulos;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtComentario;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvProductos;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboNombreProveedor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMantenimientoProductos;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpAccionesArticulo;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAccionesOrden;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIngresarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnModificarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnEliminarProducto;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnRefrescarProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDiasPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDiasPlazo;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpComentarioYTotales;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnDescartar;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDatosOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtPlazoValor;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaDeEmision;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFechaDeVencimiento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPlazo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCodigoOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCodigoOrden;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnGuardar;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstadoValor;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpDetallesProducto;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpPrecioCantidadDescuento;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnTogglePorcentaje;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescuento;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtCantidad;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCantidad;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPrecioUnitario;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpProductoSku;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboSku;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblSku;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblProducto;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpTotalesArticulo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTotalesDeProducto;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnRegresar;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFechaEmision;
        private CapaVista_Componentes.Componentes.ClsDatePickerCompras ComprasDtpFechaVencimiento;
    }
}
