

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Define la distribución visual del listado y los filtros de encabezados de compra.
 * ===================================================================
*/

namespace CapaVista_OrdenesCompra
{
    partial class FrmEncabezados
    {
        private System.ComponentModel.IContainer _Components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Components != null) _Components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEncabezados));
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasUsrEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasTlpNuevaOrden = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblNuevaOrden = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasGbxNuevo = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasBtnNuevaOrden = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasGbxFiltros = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpFiltros = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxRefrescar = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasBtnActualizar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpBuscar = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblBuscar = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtBuscar = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasTlpFechas = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasChkFechas = new System.Windows.Forms.CheckBox();
            this.ComprasUsrFechas = new CapaVista_Componentes.Componentes.ComprasUsrRangoFechas();
            this.ComprasGbxOrdenes = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpOrdenes = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblResultado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasDgvEncabezados = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasTlpPie = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasGbxSalir = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasBtnCerrar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasTlpNuevaOrden.SuspendLayout();
            this.ComprasGbxNuevo.SuspendLayout();
            this.ComprasGbxFiltros.SuspendLayout();
            this.ComprasTlpFiltros.SuspendLayout();
            this.ComprasGbxRefrescar.SuspendLayout();
            this.ComprasTlpBuscar.SuspendLayout();
            this.ComprasTlpFechas.SuspendLayout();
            this.ComprasGbxOrdenes.SuspendLayout();
            this.ComprasTlpOrdenes.SuspendLayout();
            this.ComprasTlpPie.SuspendLayout();
            this.ComprasGbxSalir.SuspendLayout();
            this.SuspendLayout();
            //
            // ComprasPnlDesplazamiento
            //
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasPnlContenido);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 700);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            //
            // ComprasPnlContenido
            //
            this.ComprasPnlContenido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpPrincipal);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlContenido.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 960);
            this.ComprasPnlContenido.TabIndex = 0;
            //
            // ComprasTlpPrincipal
            //
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasUsrEncabezado, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpNuevaOrden, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxFiltros, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxOrdenes, 0, 3);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpPie, 0, 4);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 5;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1279, 960);
            this.ComprasTlpPrincipal.TabIndex = 0;
            //
            // ComprasUsrEncabezado
            //
            this.ComprasUsrEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezado.Codigo = "6003";
            this.ComprasUsrEncabezado.Descripcion = "Consulta las ordenes de compra, filtra los resultados y abre una orden con doble " +
    "clic.";
            this.ComprasUsrEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezado.ForeColor = System.Drawing.Color.White;
            this.ComprasUsrEncabezado.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrEncabezado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrEncabezado.MaximumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.Name = "ComprasUsrEncabezado";
            this.ComprasUsrEncabezado.PaginaInicialAyuda = "EncabezadosOrdenesCompra.html";
            this.ComprasUsrEncabezado.RutaReporte = "RpReporteEncabezadosCompras.rdlc";
            this.ComprasUsrEncabezado.Size = new System.Drawing.Size(1279, 130);
            this.ComprasUsrEncabezado.TabIndex = 0;
            this.ComprasUsrEncabezado.TabStop = false;
            this.ComprasUsrEncabezado.Titulo = "ORDENES DE COMPRA";
            //
            // ComprasTlpNuevaOrden
            //
            this.ComprasTlpNuevaOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpNuevaOrden.ColumnCount = 4;
            this.ComprasTlpNuevaOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpNuevaOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 375F));
            this.ComprasTlpNuevaOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpNuevaOrden.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNuevaOrden.Controls.Add(this.ComprasLblNuevaOrden, 1, 0);
            this.ComprasTlpNuevaOrden.Controls.Add(this.ComprasGbxNuevo, 2, 0);
            this.ComprasTlpNuevaOrden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpNuevaOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpNuevaOrden.Location = new System.Drawing.Point(0, 130);
            this.ComprasTlpNuevaOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpNuevaOrden.Name = "ComprasTlpNuevaOrden";
            this.ComprasTlpNuevaOrden.RowCount = 1;
            this.ComprasTlpNuevaOrden.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpNuevaOrden.Size = new System.Drawing.Size(1279, 100);
            this.ComprasTlpNuevaOrden.TabIndex = 0;
            //
            // ComprasLblNuevaOrden
            //
            this.ComprasLblNuevaOrden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblNuevaOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblNuevaOrden.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ComprasLblNuevaOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblNuevaOrden.Location = new System.Drawing.Point(24, 39);
            this.ComprasLblNuevaOrden.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComprasLblNuevaOrden.MaximumSize = new System.Drawing.Size(1375, 0);
            this.ComprasLblNuevaOrden.MostrarAsterisco = false;
            this.ComprasLblNuevaOrden.Name = "ComprasLblNuevaOrden";
            this.ComprasLblNuevaOrden.Size = new System.Drawing.Size(367, 22);
            this.ComprasLblNuevaOrden.TabIndex = 1;
            this.ComprasLblNuevaOrden.TabStop = false;
            this.ComprasLblNuevaOrden.TamanoTipografia = 12F;
            this.ComprasLblNuevaOrden.Text = "Crear una nueva orden de compra →";
            this.ComprasLblNuevaOrden.Texto = "Crear una nueva orden de compra →";
            //
            // ComprasGbxNuevo
            //
            this.ComprasGbxNuevo.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxNuevo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasGbxNuevo.Controls.Add(this.ComprasBtnNuevaOrden);
            this.ComprasGbxNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxNuevo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxNuevo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxNuevo.Location = new System.Drawing.Point(398, 3);
            this.ComprasGbxNuevo.MostrarBorde = false;
            this.ComprasGbxNuevo.Name = "ComprasGbxNuevo";
            this.ComprasGbxNuevo.Size = new System.Drawing.Size(94, 94);
            this.ComprasGbxNuevo.TabIndex = 2;
            this.ComprasGbxNuevo.TabStop = false;
            this.ComprasBtnNuevaOrden.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            //
            // ComprasBtnNuevaOrden
            //
            this.ComprasBtnNuevaOrden.AccessibleName = "Crear nueva orden";
            this.ComprasBtnNuevaOrden.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnNuevaOrden.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnNuevaOrden.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNuevaOrden.BackgroundImage")));
            this.ComprasBtnNuevaOrden.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnNuevaOrden.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnNuevaOrden.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNuevaOrden.FlatAppearance.BorderSize = 0;
            this.ComprasBtnNuevaOrden.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnNuevaOrden.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnNuevaOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnNuevaOrden.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnNuevaOrden.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNuevaOrden.Location = new System.Drawing.Point(8, 7);
            this.ComprasBtnNuevaOrden.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnNuevaOrden.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevaOrden.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevaOrden.Name = "ComprasBtnNuevaOrden";
            this.ComprasBtnNuevaOrden.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevaOrden.TabIndex = 0;
            this.ComprasBtnNuevaOrden.UseVisualStyleBackColor = false;
            //
            // ComprasGbxFiltros
            //
            this.ComprasGbxFiltros.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxFiltros.Controls.Add(this.ComprasTlpFiltros);
            this.ComprasGbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxFiltros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxFiltros.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxFiltros.Location = new System.Drawing.Point(10, 242);
            this.ComprasGbxFiltros.Margin = new System.Windows.Forms.Padding(10, 12, 10, 0);
            this.ComprasGbxFiltros.Name = "ComprasGbxFiltros";
            this.ComprasGbxFiltros.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.ComprasGbxFiltros.Size = new System.Drawing.Size(1259, 168);
            this.ComprasGbxFiltros.TabIndex = 1;
            this.ComprasGbxFiltros.TabStop = false;
            this.ComprasGbxFiltros.Text = "1. Buscar ordenes de compra";
            //
            // ComprasTlpFiltros
            //
            this.ComprasTlpFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFiltros.ColumnCount = 3;
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpFiltros.Controls.Add(this.ComprasGbxRefrescar, 2, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTlpBuscar, 0, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTlpFechas, 1, 0);
            this.ComprasTlpFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFiltros.Location = new System.Drawing.Point(8, 25);
            this.ComprasTlpFiltros.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFiltros.Name = "ComprasTlpFiltros";
            this.ComprasTlpFiltros.RowCount = 1;
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltros.Size = new System.Drawing.Size(1243, 135);
            this.ComprasTlpFiltros.TabIndex = 0;
            //
            // ComprasGbxRefrescar
            //
            this.ComprasGbxRefrescar.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxRefrescar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasGbxRefrescar.Controls.Add(this.ComprasBtnActualizar);
            this.ComprasGbxRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxRefrescar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxRefrescar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxRefrescar.Location = new System.Drawing.Point(1145, 3);
            this.ComprasGbxRefrescar.MostrarBorde = false;
            this.ComprasGbxRefrescar.Name = "ComprasGbxRefrescar";
            this.ComprasGbxRefrescar.Size = new System.Drawing.Size(95, 105);
            this.ComprasGbxRefrescar.TabIndex = 3;
            this.ComprasGbxRefrescar.TabStop = false;
            this.ComprasBtnActualizar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            //
            // ComprasBtnActualizar
            //
            this.ComprasBtnActualizar.AccessibleName = "Actualizar listado";
            this.ComprasBtnActualizar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnActualizar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnActualizar.BackgroundImage")));
            this.ComprasBtnActualizar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnActualizar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnActualizar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnActualizar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnActualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnActualizar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnActualizar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnActualizar.Location = new System.Drawing.Point(8, 11);
            this.ComprasBtnActualizar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnActualizar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnActualizar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnActualizar.Name = "ComprasBtnActualizar";
            this.ComprasBtnActualizar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnActualizar.TabIndex = 0;
            this.ComprasBtnActualizar.UseVisualStyleBackColor = false;
            //
            // ComprasTlpBuscar
            //
            this.ComprasTlpBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpBuscar.ColumnCount = 2;
            this.ComprasTlpBuscar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.ComprasTlpBuscar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBuscar.Controls.Add(this.ComprasLblEstado, 0, 0);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasCboEstado, 1, 0);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasLblBuscar, 0, 1);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasTxtBuscar, 1, 1);
            this.ComprasTlpBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpBuscar.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpBuscar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpBuscar.Name = "ComprasTlpBuscar";
            this.ComprasTlpBuscar.Padding = new System.Windows.Forms.Padding(0, 8, 20, 0);
            this.ComprasTlpBuscar.RowCount = 3;
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBuscar.Size = new System.Drawing.Size(514, 135);
            this.ComprasTlpBuscar.TabIndex = 0;
            //
            // ComprasLblEstado
            //
            this.ComprasLblEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(0, 19);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1000, 0);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(160, 18);
            this.ComprasLblEstado.TabIndex = 0;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 9F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.Texto = "Estado";
            //
            // ComprasCboEstado
            //
            this.ComprasCboEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(162, 10);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.MostrarEtiquetaError = false;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(330, 36);
            this.ComprasCboEstado.TabIndex = 0;
            //
            // ComprasLblBuscar
            //
            this.ComprasLblBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblBuscar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblBuscar.Location = new System.Drawing.Point(0, 48);
            this.ComprasLblBuscar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasLblBuscar.MaximumSize = new System.Drawing.Size(1000, 0);
            this.ComprasLblBuscar.MostrarAsterisco = false;
            this.ComprasLblBuscar.Name = "ComprasLblBuscar";
            this.ComprasLblBuscar.Size = new System.Drawing.Size(160, 42);
            this.ComprasLblBuscar.TabIndex = 1;
            this.ComprasLblBuscar.TabStop = false;
            this.ComprasLblBuscar.TamanoTipografia = 9F;
            this.ComprasLblBuscar.Text = "No. de orden,\r\nproveedor o NIT";
            this.ComprasLblBuscar.Texto = "No. de orden,\r\nproveedor o NIT";
            //
            // ComprasTxtBuscar
            //
            this.ComprasTxtBuscar.AccessibleDescription = "Buscar por numero de orden, proveedor o NIT";
            this.ComprasTxtBuscar.AlturaEtiquetaError = 32;
            this.ComprasTxtBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasTxtBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtBuscar.Location = new System.Drawing.Point(162, 52);
            this.ComprasTxtBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtBuscar.MaxLength = 2147483647;
            this.ComprasTxtBuscar.Name = "ComprasTxtBuscar";
            this.ComprasTxtBuscar.SelectedText = "";
            this.ComprasTxtBuscar.SelectionLength = 0;
            this.ComprasTxtBuscar.SelectionStart = 0;
            this.ComprasTxtBuscar.Size = new System.Drawing.Size(330, 76);
            this.ComprasTxtBuscar.TabIndex = 1;
            //
            // ComprasTlpFechas
            //
            this.ComprasTlpFechas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFechas.ColumnCount = 1;
            this.ComprasTlpFechas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFechas.Controls.Add(this.ComprasChkFechas, 0, 0);
            this.ComprasTlpFechas.Controls.Add(this.ComprasUsrFechas, 0, 1);
            this.ComprasTlpFechas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFechas.Location = new System.Drawing.Point(514, 0);
            this.ComprasTlpFechas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFechas.Name = "ComprasTlpFechas";
            this.ComprasTlpFechas.Padding = new System.Windows.Forms.Padding(4, 0, 8, 0);
            this.ComprasTlpFechas.RowCount = 3;
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFechas.Size = new System.Drawing.Size(628, 135);
            this.ComprasTlpFechas.TabIndex = 1;
            //
            // ComprasChkFechas
            //
            this.ComprasChkFechas.AutoSize = true;
            this.ComprasChkFechas.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasChkFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasChkFechas.Location = new System.Drawing.Point(8, 3);
            this.ComprasChkFechas.Margin = new System.Windows.Forms.Padding(4, 3, 0, 0);
            this.ComprasChkFechas.Name = "ComprasChkFechas";
            this.ComprasChkFechas.Size = new System.Drawing.Size(206, 22);
            this.ComprasChkFechas.TabIndex = 0;
            this.ComprasChkFechas.Text = "Filtrar por fecha de emision";
            //
            // ComprasUsrFechas
            //
            this.ComprasUsrFechas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasUsrFechas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrFechas.Enabled = false;
            this.ComprasUsrFechas.FechaFin = new System.DateTime(2026, 11, 1, 0, 0, 0, 0);
            this.ComprasUsrFechas.FechaInicio = new System.DateTime(2026, 11, 1, 0, 0, 0, 0);
            this.ComprasUsrFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrFechas.Location = new System.Drawing.Point(4, 28);
            this.ComprasUsrFechas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrFechas.MinimumSize = new System.Drawing.Size(400, 80);
            this.ComprasUsrFechas.Name = "ComprasUsrFechas";
            this.ComprasUsrFechas.Size = new System.Drawing.Size(616, 80);
            this.ComprasUsrFechas.TabIndex = 1;
            this.ComprasUsrFechas.TamanoFuente = 10F;
            //
            // ComprasGbxOrdenes
            //
            this.ComprasGbxOrdenes.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxOrdenes.Controls.Add(this.ComprasTlpOrdenes);
            this.ComprasGbxOrdenes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxOrdenes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxOrdenes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxOrdenes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxOrdenes.Location = new System.Drawing.Point(10, 422);
            this.ComprasGbxOrdenes.Margin = new System.Windows.Forms.Padding(10, 12, 10, 0);
            this.ComprasGbxOrdenes.Name = "ComprasGbxOrdenes";
            this.ComprasGbxOrdenes.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.ComprasGbxOrdenes.Size = new System.Drawing.Size(1259, 438);
            this.ComprasGbxOrdenes.TabIndex = 2;
            this.ComprasGbxOrdenes.TabStop = false;
            this.ComprasGbxOrdenes.Text = "2. Revisar y seleccionar una orden";
            //
            // ComprasTlpOrdenes
            //
            this.ComprasTlpOrdenes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpOrdenes.ColumnCount = 1;
            this.ComprasTlpOrdenes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOrdenes.Controls.Add(this.ComprasLblResultado, 0, 0);
            this.ComprasTlpOrdenes.Controls.Add(this.ComprasDgvEncabezados, 0, 1);
            this.ComprasTlpOrdenes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpOrdenes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpOrdenes.Location = new System.Drawing.Point(8, 25);
            this.ComprasTlpOrdenes.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpOrdenes.Name = "ComprasTlpOrdenes";
            this.ComprasTlpOrdenes.RowCount = 2;
            this.ComprasTlpOrdenes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.ComprasTlpOrdenes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOrdenes.Size = new System.Drawing.Size(1243, 405);
            this.ComprasTlpOrdenes.TabIndex = 0;
            //
            // ComprasLblResultado
            //
            this.ComprasLblResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComprasLblResultado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblResultado.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ComprasLblResultado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblResultado.Location = new System.Drawing.Point(0, 5);
            this.ComprasLblResultado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasLblResultado.MaximumSize = new System.Drawing.Size(1000, 0);
            this.ComprasLblResultado.MostrarAsterisco = false;
            this.ComprasLblResultado.Name = "ComprasLblResultado";
            this.ComprasLblResultado.Size = new System.Drawing.Size(1000, 18);
            this.ComprasLblResultado.TabIndex = 0;
            this.ComprasLblResultado.TabStop = false;
            this.ComprasLblResultado.TamanoTipografia = 9F;
            this.ComprasLblResultado.Text = "Selecciona una orden con doble clic para consultar sus datos.";
            this.ComprasLblResultado.Texto = "Selecciona una orden con doble clic para consultar sus datos.";
            //
            // ComprasDgvEncabezados
            //
            this.ComprasDgvEncabezados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvEncabezados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvEncabezados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvEncabezados.Location = new System.Drawing.Point(0, 28);
            this.ComprasDgvEncabezados.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvEncabezados.Name = "ComprasDgvEncabezados";
            this.ComprasDgvEncabezados.RegistrosPorPagina = 10;
            this.ComprasDgvEncabezados.ResaltarFilasAlPasar = true;
            this.ComprasDgvEncabezados.Size = new System.Drawing.Size(1243, 377);
            this.ComprasDgvEncabezados.TabIndex = 2;
            //
            // ComprasTlpPie
            //
            this.ComprasTlpPie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPie.ColumnCount = 2;
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ComprasTlpPie.Controls.Add(this.ComprasGbxSalir, 1, 0);
            this.ComprasTlpPie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPie.Location = new System.Drawing.Point(0, 860);
            this.ComprasTlpPie.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPie.Name = "ComprasTlpPie";
            this.ComprasTlpPie.RowCount = 1;
            this.ComprasTlpPie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.Size = new System.Drawing.Size(1279, 100);
            this.ComprasTlpPie.TabIndex = 3;
            //
            // ComprasGbxSalir
            //
            this.ComprasGbxSalir.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasGbxSalir.Controls.Add(this.ComprasBtnCerrar);
            this.ComprasGbxSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxSalir.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxSalir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxSalir.Location = new System.Drawing.Point(1182, 3);
            this.ComprasGbxSalir.MostrarBorde = false;
            this.ComprasGbxSalir.Name = "ComprasGbxSalir";
            this.ComprasGbxSalir.Size = new System.Drawing.Size(94, 94);
            this.ComprasGbxSalir.TabIndex = 4;
            this.ComprasGbxSalir.TabStop = false;
            this.ComprasBtnCerrar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            //
            // ComprasBtnCerrar
            //
            this.ComprasBtnCerrar.AccessibleName = "Cerrar listado";
            this.ComprasBtnCerrar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnCerrar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnCerrar.BackgroundImage")));
            this.ComprasBtnCerrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnCerrar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnCerrar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnCerrar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasBtnCerrar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnCerrar.Location = new System.Drawing.Point(7, 7);
            this.ComprasBtnCerrar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnCerrar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnCerrar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnCerrar.Name = "ComprasBtnCerrar";
            this.ComprasBtnCerrar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnCerrar.TabIndex = 0;
            this.ComprasBtnCerrar.UseVisualStyleBackColor = false;
            //
            // FrmEncabezados
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEncabezados";
            this.Text = "6003 - Consulta de ordenes de compra";
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasTlpNuevaOrden.ResumeLayout(false);
            this.ComprasGbxNuevo.ResumeLayout(false);
            this.ComprasGbxFiltros.ResumeLayout(false);
            this.ComprasTlpFiltros.ResumeLayout(false);
            this.ComprasGbxRefrescar.ResumeLayout(false);
            this.ComprasTlpBuscar.ResumeLayout(false);
            this.ComprasTlpFechas.ResumeLayout(false);
            this.ComprasTlpFechas.PerformLayout();
            this.ComprasGbxOrdenes.ResumeLayout(false);
            this.ComprasTlpOrdenes.ResumeLayout(false);
            this.ComprasTlpPie.ResumeLayout(false);
            this.ComprasGbxSalir.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezado;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxFiltros;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFiltros;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboEstado;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFechas;
        private System.Windows.Forms.CheckBox ComprasChkFechas;
        private CapaVista_Componentes.Componentes.ComprasUsrRangoFechas ComprasUsrFechas;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpNuevaOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblNuevaOrden;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnActualizar;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxOrdenes;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpOrdenes;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvEncabezados;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPie;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnNuevaOrden;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblResultado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnCerrar;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxNuevo;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxRefrescar;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxSalir;
    }
}
