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
            this.ComprasPnlDesplazamiento = new System.Windows.Forms.Panel();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasTlpPrincipal = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasUsrEncabezado = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasGbxFiltros = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasTlpFiltros = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTlpBuscar = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasTxtBuscar = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblBuscar = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasTlpFechas = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasChkFechas = new System.Windows.Forms.CheckBox();
            this.ComprasUsrFechas = new CapaVista_Componentes.Componentes.ComprasUsrRangoFechas();
            this.ComprasFlpAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnActualizar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasDgvEncabezados = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasTlpPie = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasBtnNuevaOrden = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblResultado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasBtnCerrar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasPnlDesplazamiento.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasTlpPrincipal.SuspendLayout();
            this.ComprasGbxFiltros.SuspendLayout();
            this.ComprasTlpFiltros.SuspendLayout();
            this.ComprasTlpBuscar.SuspendLayout();
            this.ComprasTlpFechas.SuspendLayout();
            this.ComprasFlpAcciones.SuspendLayout();
            this.ComprasTlpPie.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasPnlContenido);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 700);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasTlpPrincipal);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlContenido.MinimumSize = new System.Drawing.Size(0, 900);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 900);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasTlpPrincipal
            // 
            this.ComprasTlpPrincipal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPrincipal.ColumnCount = 1;
            this.ComprasTlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasUsrEncabezado, 0, 0);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasGbxFiltros, 0, 1);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasDgvEncabezados, 0, 2);
            this.ComprasTlpPrincipal.Controls.Add(this.ComprasTlpPie, 0, 3);
            this.ComprasTlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPrincipal.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpPrincipal.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPrincipal.Name = "ComprasTlpPrincipal";
            this.ComprasTlpPrincipal.RowCount = 4;
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpPrincipal.Size = new System.Drawing.Size(1279, 900);
            this.ComprasTlpPrincipal.TabIndex = 0;
            // 
            // ComprasUsrEncabezado
            // 
            this.ComprasUsrEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezado.Codigo = "6003";
            this.ComprasUsrEncabezado.Descripcion = "Consulta las compras por pagar y abre una con doble clic para ver sus datos.";
            this.ComprasUsrEncabezado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezado.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezado.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezado.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezado.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezado.Name = "ComprasUsrEncabezado";
            this.ComprasUsrEncabezado.Size = new System.Drawing.Size(1259, 130);
            this.ComprasUsrEncabezado.TabIndex = 0;
            this.ComprasUsrEncabezado.TabStop = false;
            this.ComprasUsrEncabezado.Titulo = "Compras por pagar";
            // 
            // ComprasGbxFiltros
            // 
            this.ComprasGbxFiltros.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxFiltros.Controls.Add(this.ComprasTlpFiltros);
            this.ComprasGbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasGbxFiltros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxFiltros.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxFiltros.Location = new System.Drawing.Point(10, 150);
            this.ComprasGbxFiltros.Margin = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.ComprasGbxFiltros.Name = "ComprasGbxFiltros";
            this.ComprasGbxFiltros.Padding = new System.Windows.Forms.Padding(8);
            this.ComprasGbxFiltros.Size = new System.Drawing.Size(1259, 240);
            this.ComprasGbxFiltros.TabIndex = 1;
            this.ComprasGbxFiltros.TabStop = false;
            this.ComprasGbxFiltros.Text = "Filtros";
            // 
            // ComprasTlpFiltros
            // 
            this.ComprasTlpFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFiltros.ColumnCount = 2;
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTlpBuscar, 0, 0);
            this.ComprasTlpFiltros.Controls.Add(this.ComprasTlpFechas, 1, 0);
            this.ComprasTlpFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFiltros.Location = new System.Drawing.Point(8, 29);
            this.ComprasTlpFiltros.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFiltros.Name = "ComprasTlpFiltros";
            this.ComprasTlpFiltros.RowCount = 1;
            this.ComprasTlpFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFiltros.Size = new System.Drawing.Size(1243, 203);
            this.ComprasTlpFiltros.TabIndex = 0;
            // 
            // ComprasTlpBuscar
            // 
            this.ComprasTlpBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpBuscar.ColumnCount = 1;
            this.ComprasTlpBuscar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBuscar.Controls.Add(this.ComprasTxtBuscar, 0, 3);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasLblEstado, 0, 0);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasLblBuscar, 0, 2);
            this.ComprasTlpBuscar.Controls.Add(this.ComprasCboEstado, 0, 1);
            this.ComprasTlpBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpBuscar.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpBuscar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpBuscar.Name = "ComprasTlpBuscar";
            this.ComprasTlpBuscar.Padding = new System.Windows.Forms.Padding(4, 0, 16, 0);
            this.ComprasTlpBuscar.RowCount = 4;
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ComprasTlpBuscar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpBuscar.Size = new System.Drawing.Size(621, 203);
            this.ComprasTlpBuscar.TabIndex = 0;
            // 
            // ComprasTxtBuscar
            // 
            this.ComprasTxtBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtBuscar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtBuscar.Location = new System.Drawing.Point(6, 92);
            this.ComprasTxtBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasTxtBuscar.MaxLength = 2147483647;
            this.ComprasTxtBuscar.Name = "ComprasTxtBuscar";
            this.ComprasTxtBuscar.SelectedText = "";
            this.ComprasTxtBuscar.SelectionLength = 0;
            this.ComprasTxtBuscar.SelectionStart = 0;
            this.ComprasTxtBuscar.Size = new System.Drawing.Size(597, 66);
            this.ComprasTxtBuscar.TabIndex = 1;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(6, 0);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(597, 25);
            this.ComprasLblEstado.TabIndex = 2;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.TamanoTipografia = 10F;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.Texto = "Estado";
            // 
            // ComprasLblBuscar
            // 
            this.ComprasLblBuscar.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblBuscar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblBuscar.Location = new System.Drawing.Point(6, 65);
            this.ComprasLblBuscar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ComprasLblBuscar.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblBuscar.MostrarAsterisco = false;
            this.ComprasLblBuscar.Name = "ComprasLblBuscar";
            this.ComprasLblBuscar.Size = new System.Drawing.Size(597, 25);
            this.ComprasLblBuscar.TabIndex = 0;
            this.ComprasLblBuscar.TabStop = false;
            this.ComprasLblBuscar.TamanoTipografia = 10F;
            this.ComprasLblBuscar.Text = "Factura, proveedor o NIT";
            this.ComprasLblBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblBuscar.Texto = "Factura, proveedor o NIT";
            // 
            // ComprasCboEstado
            // 
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(6, 27);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(2);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.MostrarEtiquetaError = false;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(597, 36);
            this.ComprasCboEstado.TabIndex = 3;
            // 
            // ComprasTlpFechas
            // 
            this.ComprasTlpFechas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpFechas.ColumnCount = 1;
            this.ComprasTlpFechas.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpFechas.Controls.Add(this.ComprasChkFechas, 0, 0);
            this.ComprasTlpFechas.Controls.Add(this.ComprasUsrFechas, 0, 1);
            this.ComprasTlpFechas.Controls.Add(this.ComprasFlpAcciones, 0, 2);
            this.ComprasTlpFechas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpFechas.Location = new System.Drawing.Point(621, 0);
            this.ComprasTlpFechas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpFechas.Name = "ComprasTlpFechas";
            this.ComprasTlpFechas.RowCount = 3;
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpFechas.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.ComprasTlpFechas.Size = new System.Drawing.Size(622, 203);
            this.ComprasTlpFechas.TabIndex = 1;
            // 
            // ComprasChkFechas
            // 
            this.ComprasChkFechas.AutoSize = true;
            this.ComprasChkFechas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasChkFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasChkFechas.Location = new System.Drawing.Point(3, 3);
            this.ComprasChkFechas.Name = "ComprasChkFechas";
            this.ComprasChkFechas.Size = new System.Drawing.Size(235, 22);
            this.ComprasChkFechas.TabIndex = 0;
            this.ComprasChkFechas.Text = "Filtrar por fecha de emision";
            // 
            // ComprasUsrFechas
            // 
            this.ComprasUsrFechas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasUsrFechas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrFechas.Enabled = false;
            this.ComprasUsrFechas.FechaFin = new System.DateTime(2026, 10, 24, 0, 0, 0, 0);
            this.ComprasUsrFechas.FechaInicio = new System.DateTime(2026, 10, 1, 0, 0, 0, 0);
            this.ComprasUsrFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrFechas.Location = new System.Drawing.Point(0, 28);
            this.ComprasUsrFechas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrFechas.MinimumSize = new System.Drawing.Size(400, 80);
            this.ComprasUsrFechas.Name = "ComprasUsrFechas";
            this.ComprasUsrFechas.Size = new System.Drawing.Size(622, 90);
            this.ComprasUsrFechas.TabIndex = 1;
            // 
            // ComprasFlpAcciones
            // 
            this.ComprasFlpAcciones.Controls.Add(this.ComprasBtnActualizar);
            this.ComprasFlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasFlpAcciones.Location = new System.Drawing.Point(0, 118);
            this.ComprasFlpAcciones.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasFlpAcciones.Name = "ComprasFlpAcciones";
            this.ComprasFlpAcciones.Size = new System.Drawing.Size(622, 85);
            this.ComprasFlpAcciones.TabIndex = 2;
            this.ComprasFlpAcciones.WrapContents = false;
            this.ComprasBtnActualizar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnActualizar
            // 
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
            this.ComprasBtnActualizar.Location = new System.Drawing.Point(4, 4);
            this.ComprasBtnActualizar.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasBtnActualizar.Name = "ComprasBtnActualizar";
            this.ComprasBtnActualizar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnActualizar.TabIndex = 0;
            this.ComprasBtnActualizar.UseVisualStyleBackColor = false;
            // 
            // ComprasDgvEncabezados
            // 
            this.ComprasDgvEncabezados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvEncabezados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasDgvEncabezados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvEncabezados.Location = new System.Drawing.Point(10, 400);
            this.ComprasDgvEncabezados.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ComprasDgvEncabezados.Name = "ComprasDgvEncabezados";
            this.ComprasDgvEncabezados.ResaltarFilasAlPasar = true;
            this.ComprasDgvEncabezados.Size = new System.Drawing.Size(1259, 410);
            this.ComprasDgvEncabezados.TabIndex = 2;
            // 
            // ComprasTlpPie
            // 
            this.ComprasTlpPie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTlpPie.ColumnCount = 3;
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpPie.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.ComprasTlpPie.Controls.Add(this.ComprasBtnNuevaOrden, 1, 0);
            this.ComprasTlpPie.Controls.Add(this.ComprasLblResultado, 0, 0);
            this.ComprasTlpPie.Controls.Add(this.ComprasBtnCerrar, 2, 0);
            this.ComprasTlpPie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpPie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTlpPie.Location = new System.Drawing.Point(0, 810);
            this.ComprasTlpPie.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpPie.Name = "ComprasTlpPie";
            this.ComprasTlpPie.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ComprasTlpPie.RowCount = 1;
            this.ComprasTlpPie.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPie.Size = new System.Drawing.Size(1279, 90);
            this.ComprasTlpPie.TabIndex = 3;
            this.ComprasBtnNuevaOrden.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnNuevaOrden
            // 
            this.ComprasBtnNuevaOrden.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.ComprasBtnNuevaOrden.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNuevaOrden.ImagenDeshabilitado")));
            this.ComprasBtnNuevaOrden.Location = new System.Drawing.Point(1095, 5);
            this.ComprasBtnNuevaOrden.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasBtnNuevaOrden.Name = "ComprasBtnNuevaOrden";
            this.ComprasBtnNuevaOrden.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevaOrden.TabIndex = 2;
            this.ComprasBtnNuevaOrden.UseVisualStyleBackColor = false;
            // 
            // ComprasLblResultado
            // 
            this.ComprasLblResultado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblResultado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblResultado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ComprasLblResultado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblResultado.Location = new System.Drawing.Point(12, 0);
            this.ComprasLblResultado.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ComprasLblResultado.MaximumSize = new System.Drawing.Size(1125, 0);
            this.ComprasLblResultado.MostrarAsterisco = false;
            this.ComprasLblResultado.Name = "ComprasLblResultado";
            this.ComprasLblResultado.Size = new System.Drawing.Size(1075, 90);
            this.ComprasLblResultado.TabIndex = 0;
            this.ComprasLblResultado.TabStop = false;
            this.ComprasLblResultado.TamanoTipografia = 10F;
            this.ComprasLblResultado.Text = "";
            this.ComprasLblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblResultado.Texto = "";
            this.ComprasBtnCerrar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnCerrar
            // 
            this.ComprasBtnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.ComprasBtnCerrar.Location = new System.Drawing.Point(1185, 5);
            this.ComprasBtnCerrar.Margin = new System.Windows.Forms.Padding(4);
            this.ComprasBtnCerrar.Name = "ComprasBtnCerrar";
            this.ComprasBtnCerrar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnCerrar.TabIndex = 1;
            this.ComprasBtnCerrar.UseVisualStyleBackColor = false;
            // 
            // FrmEncabezados
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmEncabezados";
            this.Text = "6012 – EncabezadosComprasPorPagar";
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasTlpPrincipal.ResumeLayout(false);
            this.ComprasGbxFiltros.ResumeLayout(false);
            this.ComprasTlpFiltros.ResumeLayout(false);
            this.ComprasTlpBuscar.ResumeLayout(false);
            this.ComprasTlpFechas.ResumeLayout(false);
            this.ComprasTlpFechas.PerformLayout();
            this.ComprasFlpAcciones.ResumeLayout(false);
            this.ComprasTlpPie.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Panel ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPrincipal;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezado;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxFiltros;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFiltros;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpBuscar;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpFechas;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtBuscar;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboEstado;
        private System.Windows.Forms.CheckBox ComprasChkFechas;
        private CapaVista_Componentes.Componentes.ComprasUsrRangoFechas ComprasUsrFechas;
        private System.Windows.Forms.FlowLayoutPanel ComprasFlpAcciones;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnActualizar;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvEncabezados;
        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpPie;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblResultado;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnCerrar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnNuevaOrden;
    }
}
