namespace CapaVista_MovimientosProveedores
{
    partial class FrmMovimientosProveedores
    {
        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMovimientosProveedores));
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasDgvMovimientos = new CapaVista_Componentes.Componentes.ComprasUsrDataGridView();
            this.ComprasPnlProveedor = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasLblCorreo = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblSeparadorCorreo = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblSeparadorTelefono = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblTelefono = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblTituloTelefono = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblNit = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblTituloNit = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblNombreProveedor = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasPnlFiltros = new CapaVista_Componentes.Componentes.ClsPanelCompras();
            this.ComprasCboProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasUsrFechas = new CapaVista_Componentes.Componentes.ComprasUsrRangoFechas();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasCboTipoMovimiento = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasBtnConsultar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblTipoDeMovimiento = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasLblProveedor = new CapaVista_Componentes.Componentes.ClsEtiquetaCompras();
            this.ComprasColFecha = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColTipoMovimiento = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDocumento = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDescripcion = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColDebe = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColHaber = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColSaldo = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasColEstado = new System.Windows.Forms.DataGridViewColumn();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasPnlProveedor.SuspendLayout();
            this.ComprasPnlFiltros.SuspendLayout();
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
            this.ComprasTlpFormulario.Size = new System.Drawing.Size(1279, 873);
            this.ComprasTlpFormulario.TabIndex = 0;
            // 
            // ComprasUsrEncabezadoFormulario
            // 
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6001";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Consulta los movimientos de cada proveedor y filtra sus documentos por fecha y es" +
    "tado.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1259, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Movimientos de proveedores";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasDgvMovimientos);
            this.ComprasPnlContenido.Controls.Add(this.ComprasPnlProveedor);
            this.ComprasPnlContenido.Controls.Add(this.ComprasPnlFiltros);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 723);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasDgvMovimientos
            // 
            this.ComprasDgvMovimientos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasDgvMovimientos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDgvMovimientos.Location = new System.Drawing.Point(28, 400);
            this.ComprasDgvMovimientos.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasDgvMovimientos.Name = "ComprasDgvMovimientos";
            this.ComprasDgvMovimientos.Size = new System.Drawing.Size(1222, 275);
            this.ComprasDgvMovimientos.TabIndex = 4;
            // 
            // ComprasPnlProveedor
            // 
            this.ComprasPnlProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblCorreo);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblSeparadorCorreo);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblSeparadorTelefono);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblTelefono);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblTituloTelefono);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblNit);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblTituloNit);
            this.ComprasPnlProveedor.Controls.Add(this.ComprasLblNombreProveedor);
            this.ComprasPnlProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlProveedor.Location = new System.Drawing.Point(28, 263);
            this.ComprasPnlProveedor.Name = "ComprasPnlProveedor";
            this.ComprasPnlProveedor.Size = new System.Drawing.Size(1222, 107);
            this.ComprasPnlProveedor.TabIndex = 3;
            // 
            // ComprasLblCorreo
            // 
            this.ComprasLblCorreo.AutoSize = true;
            this.ComprasLblCorreo.Location = new System.Drawing.Point(382, 50);
            this.ComprasLblCorreo.Name = "ComprasLblCorreo";
            this.ComprasLblCorreo.Size = new System.Drawing.Size(227, 20);
            this.ComprasLblCorreo.TabIndex = 7;
            this.ComprasLblCorreo.Text = "toledoalimentos@dipcmi.com.gt";
            // 
            // ComprasLblSeparadorCorreo
            // 
            this.ComprasLblSeparadorCorreo.AutoSize = true;
            this.ComprasLblSeparadorCorreo.Location = new System.Drawing.Point(363, 50);
            this.ComprasLblSeparadorCorreo.Name = "ComprasLblSeparadorCorreo";
            this.ComprasLblSeparadorCorreo.Size = new System.Drawing.Size(13, 20);
            this.ComprasLblSeparadorCorreo.TabIndex = 6;
            this.ComprasLblSeparadorCorreo.Text = "|";
            // 
            // ComprasLblSeparadorTelefono
            // 
            this.ComprasLblSeparadorTelefono.AutoSize = true;
            this.ComprasLblSeparadorTelefono.Location = new System.Drawing.Point(200, 50);
            this.ComprasLblSeparadorTelefono.Name = "ComprasLblSeparadorTelefono";
            this.ComprasLblSeparadorTelefono.Size = new System.Drawing.Size(13, 20);
            this.ComprasLblSeparadorTelefono.TabIndex = 5;
            this.ComprasLblSeparadorTelefono.Text = "|";
            // 
            // ComprasLblTelefono
            // 
            this.ComprasLblTelefono.AutoSize = true;
            this.ComprasLblTelefono.Location = new System.Drawing.Point(285, 50);
            this.ComprasLblTelefono.Name = "ComprasLblTelefono";
            this.ComprasLblTelefono.Size = new System.Drawing.Size(79, 20);
            this.ComprasLblTelefono.TabIndex = 4;
            this.ComprasLblTelefono.Text = "2477-5580";
            // 
            // ComprasLblTituloTelefono
            // 
            this.ComprasLblTituloTelefono.AutoSize = true;
            this.ComprasLblTituloTelefono.Location = new System.Drawing.Point(209, 50);
            this.ComprasLblTituloTelefono.Name = "ComprasLblTituloTelefono";
            this.ComprasLblTituloTelefono.Size = new System.Drawing.Size(70, 20);
            this.ComprasLblTituloTelefono.TabIndex = 3;
            this.ComprasLblTituloTelefono.Text = "Telefono:";
            // 
            // ComprasLblNit
            // 
            this.ComprasLblNit.AutoSize = true;
            this.ComprasLblNit.Location = new System.Drawing.Point(131, 50);
            this.ComprasLblNit.Name = "ComprasLblNit";
            this.ComprasLblNit.Size = new System.Drawing.Size(63, 20);
            this.ComprasLblNit.TabIndex = 2;
            this.ComprasLblNit.Text = "90493-7";
            // 
            // ComprasLblTituloNit
            // 
            this.ComprasLblTituloNit.AutoSize = true;
            this.ComprasLblTituloNit.Location = new System.Drawing.Point(96, 50);
            this.ComprasLblTituloNit.Name = "ComprasLblTituloNit";
            this.ComprasLblTituloNit.Size = new System.Drawing.Size(32, 20);
            this.ComprasLblTituloNit.TabIndex = 1;
            this.ComprasLblTituloNit.Text = "Nit:";
            // 
            // ComprasLblNombreProveedor
            // 
            this.ComprasLblNombreProveedor.AutoSize = true;
            this.ComprasLblNombreProveedor.Location = new System.Drawing.Point(96, 13);
            this.ComprasLblNombreProveedor.Name = "ComprasLblNombreProveedor";
            this.ComprasLblNombreProveedor.Size = new System.Drawing.Size(197, 20);
            this.ComprasLblNombreProveedor.TabIndex = 0;
            this.ComprasLblNombreProveedor.Text = "Empaquetadora Toledo, S.A.";
            // 
            // ComprasPnlFiltros
            // 
            this.ComprasPnlFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasPnlFiltros.Controls.Add(this.ComprasCboProveedor);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasUsrFechas);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasCboEstado);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasCboTipoMovimiento);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasBtnConsultar);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasLblEstado);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasLblTipoDeMovimiento);
            this.ComprasPnlFiltros.Controls.Add(this.ComprasLblProveedor);
            this.ComprasPnlFiltros.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasPnlFiltros.Location = new System.Drawing.Point(28, 45);
            this.ComprasPnlFiltros.Name = "ComprasPnlFiltros";
            this.ComprasPnlFiltros.Size = new System.Drawing.Size(1222, 191);
            this.ComprasPnlFiltros.TabIndex = 2;
            // 
            // ComprasCboProveedor
            // 
            this.ComprasCboProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProveedor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProveedor.Location = new System.Drawing.Point(23, 61);
            this.ComprasCboProveedor.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboProveedor.MaxLength = 0;
            this.ComprasCboProveedor.Name = "ComprasCboProveedor";
            this.ComprasCboProveedor.Size = new System.Drawing.Size(292, 66);
            this.ComprasCboProveedor.TabIndex = 16;
            this.ComprasCboProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasUsrFechas
            // 
            this.ComprasUsrFechas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasUsrFechas.FechaFin = new System.DateTime(2026, 10, 24, 0, 0, 0, 0);
            this.ComprasUsrFechas.FechaInicio = new System.DateTime(2026, 10, 1, 0, 0, 0, 0);
            this.ComprasUsrFechas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrFechas.Location = new System.Drawing.Point(448, 111);
            this.ComprasUsrFechas.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrFechas.MinimumSize = new System.Drawing.Size(400, 80);
            this.ComprasUsrFechas.Name = "ComprasUsrFechas";
            this.ComprasUsrFechas.Size = new System.Drawing.Size(400, 80);
            this.ComprasUsrFechas.TabIndex = 15;
            // 
            // ComprasCboEstado
            // 
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(717, 61);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.MostrarEtiquetaError = false;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(211, 39);
            this.ComprasCboEstado.TabIndex = 13;
            // 
            // ComprasCboTipoMovimiento
            // 
            this.ComprasCboTipoMovimiento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboTipoMovimiento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboTipoMovimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboTipoMovimiento.Location = new System.Drawing.Point(448, 61);
            this.ComprasCboTipoMovimiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboTipoMovimiento.MaxLength = 0;
            this.ComprasCboTipoMovimiento.MostrarEtiquetaError = false;
            this.ComprasCboTipoMovimiento.Name = "ComprasCboTipoMovimiento";
            this.ComprasCboTipoMovimiento.Size = new System.Drawing.Size(203, 39);
            this.ComprasCboTipoMovimiento.TabIndex = 12;
            // 
            // ComprasBtnConsultar
            // 
            this.ComprasBtnConsultar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnConsultar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnConsultar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnConsultar.BackgroundImage")));
            this.ComprasBtnConsultar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnConsultar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnConsultar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnConsultar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnConsultar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnConsultar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnConsultar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnConsultar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnConsultar.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnConsultar.ImagenDeshabilitado")));
            this.ComprasBtnConsultar.Location = new System.Drawing.Point(1025, 61);
            this.ComprasBtnConsultar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnConsultar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnConsultar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnConsultar.Name = "ComprasBtnConsultar";
            this.ComprasBtnConsultar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnConsultar.TabIndex = 10;
            this.ComprasBtnConsultar.UseVisualStyleBackColor = false;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.AutoSize = true;
            this.ComprasLblEstado.Location = new System.Drawing.Point(713, 28);
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(54, 20);
            this.ComprasLblEstado.TabIndex = 4;
            this.ComprasLblEstado.Text = "Estado";
            // 
            // ComprasLblTipoDeMovimiento
            // 
            this.ComprasLblTipoDeMovimiento.AutoSize = true;
            this.ComprasLblTipoDeMovimiento.Location = new System.Drawing.Point(448, 28);
            this.ComprasLblTipoDeMovimiento.Name = "ComprasLblTipoDeMovimiento";
            this.ComprasLblTipoDeMovimiento.Size = new System.Drawing.Size(144, 20);
            this.ComprasLblTipoDeMovimiento.TabIndex = 2;
            this.ComprasLblTipoDeMovimiento.Text = "Tipo de movimiento";
            // 
            // ComprasLblProveedor
            // 
            this.ComprasLblProveedor.AutoSize = true;
            this.ComprasLblProveedor.Location = new System.Drawing.Point(19, 22);
            this.ComprasLblProveedor.Name = "ComprasLblProveedor";
            this.ComprasLblProveedor.Size = new System.Drawing.Size(77, 20);
            this.ComprasLblProveedor.TabIndex = 1;
            this.ComprasLblProveedor.Text = "Proveedor";
            // 
            // ComprasColFecha
            // 
            this.ComprasColFecha.MinimumWidth = 6;
            this.ComprasColFecha.Name = "ComprasColFecha";
            this.ComprasColFecha.ReadOnly = true;
            this.ComprasColFecha.Width = 125;
            // 
            // ComprasColTipoMovimiento
            // 
            this.ComprasColTipoMovimiento.MinimumWidth = 6;
            this.ComprasColTipoMovimiento.Name = "ComprasColTipoMovimiento";
            this.ComprasColTipoMovimiento.ReadOnly = true;
            this.ComprasColTipoMovimiento.Width = 125;
            // 
            // ComprasColDocumento
            // 
            this.ComprasColDocumento.MinimumWidth = 6;
            this.ComprasColDocumento.Name = "ComprasColDocumento";
            this.ComprasColDocumento.ReadOnly = true;
            this.ComprasColDocumento.Width = 125;
            // 
            // ComprasColDescripcion
            // 
            this.ComprasColDescripcion.MinimumWidth = 6;
            this.ComprasColDescripcion.Name = "ComprasColDescripcion";
            this.ComprasColDescripcion.ReadOnly = true;
            this.ComprasColDescripcion.Width = 125;
            // 
            // ComprasColDebe
            // 
            this.ComprasColDebe.MinimumWidth = 6;
            this.ComprasColDebe.Name = "ComprasColDebe";
            this.ComprasColDebe.ReadOnly = true;
            this.ComprasColDebe.Width = 125;
            // 
            // ComprasColHaber
            // 
            this.ComprasColHaber.MinimumWidth = 6;
            this.ComprasColHaber.Name = "ComprasColHaber";
            this.ComprasColHaber.ReadOnly = true;
            this.ComprasColHaber.Width = 125;
            // 
            // ComprasColSaldo
            // 
            this.ComprasColSaldo.MinimumWidth = 6;
            this.ComprasColSaldo.Name = "ComprasColSaldo";
            this.ComprasColSaldo.ReadOnly = true;
            this.ComprasColSaldo.Width = 125;
            // 
            // ComprasColEstado
            // 
            this.ComprasColEstado.MinimumWidth = 6;
            this.ComprasColEstado.Name = "ComprasColEstado";
            this.ComprasColEstado.ReadOnly = true;
            this.ComprasColEstado.Width = 125;
            // 
            // ComprasPnlDesplazamiento
            // 
            this.ComprasPnlDesplazamiento.AutoScroll = true;
            this.ComprasPnlDesplazamiento.Controls.Add(this.ComprasTlpFormulario);
            this.ComprasPnlDesplazamiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlDesplazamiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasPnlDesplazamiento.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlDesplazamiento.Name = "ComprasPnlDesplazamiento";
            this.ComprasPnlDesplazamiento.Size = new System.Drawing.Size(1300, 700);
            this.ComprasPnlDesplazamiento.TabIndex = 0;
            // 
            // FrmMovimientosProveedores
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.Name = "FrmMovimientosProveedores";
            this.Text = "6001 – MovimientosProveedores";
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasPnlProveedor.ResumeLayout(false);
            this.ComprasPnlProveedor.PerformLayout();
            this.ComprasPnlFiltros.ResumeLayout(false);
            this.ComprasPnlFiltros.PerformLayout();
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblProveedor;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlFiltros;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnConsultar;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblTipoDeMovimiento;
        private CapaVista_Componentes.Componentes.ClsPanelCompras ComprasPnlProveedor;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblCorreo;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblSeparadorCorreo;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblSeparadorTelefono;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblTelefono;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblTituloTelefono;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblNit;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblTituloNit;
        private CapaVista_Componentes.Componentes.ClsEtiquetaCompras ComprasLblNombreProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrDataGridView ComprasDgvMovimientos;
        private System.Windows.Forms.DataGridViewColumn ComprasColFecha;
        private System.Windows.Forms.DataGridViewColumn ComprasColTipoMovimiento;
        private System.Windows.Forms.DataGridViewColumn ComprasColDocumento;
        private System.Windows.Forms.DataGridViewColumn ComprasColDescripcion;
        private System.Windows.Forms.DataGridViewColumn ComprasColDebe;
        private System.Windows.Forms.DataGridViewColumn ComprasColHaber;
        private System.Windows.Forms.DataGridViewColumn ComprasColSaldo;
        private System.Windows.Forms.DataGridViewColumn ComprasColEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboTipoMovimiento;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboProveedor;
        private CapaVista_Componentes.Componentes.ComprasUsrRangoFechas ComprasUsrFechas;
    }
}
