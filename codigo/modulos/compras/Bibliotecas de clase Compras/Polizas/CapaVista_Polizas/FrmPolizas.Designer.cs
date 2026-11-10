namespace CapaVista_Polizas
{
    partial class FrmPolizas
    {
        private System.Windows.Forms.TableLayoutPanel ComprasTlpFormulario;
        private CapaVista_Componentes.Componentes.ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        private System.ComponentModel.IContainer _Componentes = null;
        private CapaVista_Componentes.Componentes.ClsPanelVerticalCompras ComprasPnlDesplazamiento;
        private System.Windows.Forms.Panel ComprasPnlContenido;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && _Componentes != null) _Componentes.Dispose();
            base.Dispose(LiberarRecursos);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPolizas));
            this.ComprasTlpFormulario = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasPnlContenido = new System.Windows.Forms.Panel();
            this.ComprasGbxDetalle = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasLblDiferencia = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasBtnEnviar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnNuevo = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnIngresarCuenta = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasCboCuenta = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblCuenta = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasGbxEncabezado = new CapaVista_Componentes.Componentes.ClsGroupBoxCompras();
            this.ComprasLblDescripcion = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtDescripcion = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblFecha = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboFecha = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblPeriodo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTxtPeriodo = new CapaVista_Componentes.Componentes.ComprasUsrTextBox();
            this.ComprasLblOrigen = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasLblEstado = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasCboOrigen = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasCboEstado = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasPnlDesplazamiento = new CapaVista_Componentes.Componentes.ClsPanelVerticalCompras();
            this.ComprasLblTitulo = new CapaVista_Componentes.Componentes.ComprasUsrLabel();
            this.ComprasTlpFormulario.SuspendLayout();
            this.ComprasPnlContenido.SuspendLayout();
            this.ComprasGbxDetalle.SuspendLayout();
            this.ComprasGbxEncabezado.SuspendLayout();
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
            this.ComprasUsrEncabezadoFormulario.Codigo = "6002";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Registra las cuentas y los movimientos contables de las compras para preparar sus" +
    " pólizas.";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(10, 10);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(10);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1259, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 0;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Pólizas de compras";
            // 
            // ComprasPnlContenido
            // 
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxDetalle);
            this.ComprasPnlContenido.Controls.Add(this.ComprasGbxEncabezado);
            this.ComprasPnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlContenido.Location = new System.Drawing.Point(0, 150);
            this.ComprasPnlContenido.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasPnlContenido.Name = "ComprasPnlContenido";
            this.ComprasPnlContenido.Size = new System.Drawing.Size(1279, 723);
            this.ComprasPnlContenido.TabIndex = 0;
            // 
            // ComprasGbxDetalle
            // 
            this.ComprasGbxDetalle.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxDetalle.Controls.Add(this.ComprasLblDiferencia);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasBtnEnviar);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasBtnGuardar);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasBtnNuevo);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasBtnIngresarCuenta);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasCboCuenta);
            this.ComprasGbxDetalle.Controls.Add(this.ComprasLblCuenta);
            this.ComprasGbxDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxDetalle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxDetalle.Location = new System.Drawing.Point(14, 336);
            this.ComprasGbxDetalle.Name = "ComprasGbxDetalle";
            this.ComprasGbxDetalle.Size = new System.Drawing.Size(1255, 296);
            this.ComprasGbxDetalle.TabIndex = 9;
            this.ComprasGbxDetalle.TabStop = false;
            // 
            // ComprasLblDiferencia
            // 
            this.ComprasLblDiferencia.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDiferencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDiferencia.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblDiferencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDiferencia.Location = new System.Drawing.Point(10, 245);
            this.ComprasLblDiferencia.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblDiferencia.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblDiferencia.MostrarAsterisco = false;
            this.ComprasLblDiferencia.Name = "ComprasLblDiferencia";
            this.ComprasLblDiferencia.Size = new System.Drawing.Size(368, 32);
            this.ComprasLblDiferencia.TabIndex = 14;
            this.ComprasLblDiferencia.TabStop = false;
            this.ComprasLblDiferencia.Text = "Descuadra- Diferencia Q 00.00";
            this.ComprasLblDiferencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDiferencia.Texto = "Descuadra- Diferencia Q 00.00";
            this.ComprasLblDiferencia.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasBtnEnviar
            // 
            this.ComprasBtnEnviar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnEnviar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEnviar.BackgroundImage")));
            this.ComprasBtnEnviar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnEnviar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnEnviar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEnviar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnEnviar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnEnviar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnEnviar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnEnviar.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnEnviar.ImagenDeshabilitado")));
            this.ComprasBtnEnviar.Location = new System.Drawing.Point(1146, 213);
            this.ComprasBtnEnviar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnEnviar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEnviar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnEnviar.Name = "ComprasBtnEnviar";
            this.ComprasBtnEnviar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnEnviar.TabIndex = 13;
            this.ComprasBtnEnviar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnGuardar
            // 
            this.ComprasBtnGuardar.Anchor = System.Windows.Forms.AnchorStyles.None;
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
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(1045, 213);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnGuardar.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnGuardar.TabIndex = 12;
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnNuevo
            // 
            this.ComprasBtnNuevo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnNuevo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnNuevo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNuevo.BackgroundImage")));
            this.ComprasBtnNuevo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnNuevo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnNuevo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNuevo.FlatAppearance.BorderSize = 0;
            this.ComprasBtnNuevo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnNuevo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnNuevo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnNuevo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNuevo.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnNuevo.ImagenDeshabilitado")));
            this.ComprasBtnNuevo.Location = new System.Drawing.Point(942, 213);
            this.ComprasBtnNuevo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnNuevo.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevo.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevo.Name = "ComprasBtnNuevo";
            this.ComprasBtnNuevo.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnNuevo.TabIndex = 11;
            this.ComprasBtnNuevo.UseVisualStyleBackColor = false;
            // 
            // ComprasBtnIngresarCuenta
            // 
            this.ComprasBtnIngresarCuenta.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnIngresarCuenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnIngresarCuenta.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIngresarCuenta.BackgroundImage")));
            this.ComprasBtnIngresarCuenta.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnIngresarCuenta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnIngresarCuenta.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarCuenta.FlatAppearance.BorderSize = 0;
            this.ComprasBtnIngresarCuenta.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnIngresarCuenta.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnIngresarCuenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnIngresarCuenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasBtnIngresarCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnIngresarCuenta.ImagenDeshabilitado = ((System.Drawing.Image)(resources.GetObject("ComprasBtnIngresarCuenta.ImagenDeshabilitado")));
            this.ComprasBtnIngresarCuenta.Location = new System.Drawing.Point(338, 45);
            this.ComprasBtnIngresarCuenta.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasBtnIngresarCuenta.MaximumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarCuenta.MinimumSize = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarCuenta.Name = "ComprasBtnIngresarCuenta";
            this.ComprasBtnIngresarCuenta.Size = new System.Drawing.Size(80, 80);
            this.ComprasBtnIngresarCuenta.TabIndex = 10;
            this.ComprasBtnIngresarCuenta.UseVisualStyleBackColor = false;
            // 
            // ComprasCboCuenta
            // 
            this.ComprasCboCuenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboCuenta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboCuenta.Location = new System.Drawing.Point(10, 68);
            this.ComprasCboCuenta.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboCuenta.MaxLength = 0;
            this.ComprasCboCuenta.MostrarEtiquetaError = false;
            this.ComprasCboCuenta.Name = "ComprasCboCuenta";
            this.ComprasCboCuenta.Size = new System.Drawing.Size(289, 45);
            this.ComprasCboCuenta.TabIndex = 9;
            // 
            // ComprasLblCuenta
            // 
            this.ComprasLblCuenta.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblCuenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblCuenta.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblCuenta.Location = new System.Drawing.Point(7, 30);
            this.ComprasLblCuenta.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblCuenta.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblCuenta.MostrarAsterisco = false;
            this.ComprasLblCuenta.Name = "ComprasLblCuenta";
            this.ComprasLblCuenta.Size = new System.Drawing.Size(236, 32);
            this.ComprasLblCuenta.TabIndex = 5;
            this.ComprasLblCuenta.TabStop = false;
            this.ComprasLblCuenta.Text = "Ingresar Cuenta";
            this.ComprasLblCuenta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblCuenta.Texto = "Ingresar Cuenta";
            this.ComprasLblCuenta.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasGbxEncabezado
            // 
            this.ComprasGbxEncabezado.BackColor = System.Drawing.Color.Transparent;
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasLblDescripcion);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasTxtDescripcion);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasLblFecha);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasCboFecha);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasLblPeriodo);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasTxtPeriodo);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasLblOrigen);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasLblEstado);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasCboOrigen);
            this.ComprasGbxEncabezado.Controls.Add(this.ComprasCboEstado);
            this.ComprasGbxEncabezado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasGbxEncabezado.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasGbxEncabezado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasGbxEncabezado.Location = new System.Drawing.Point(14, 22);
            this.ComprasGbxEncabezado.Name = "ComprasGbxEncabezado";
            this.ComprasGbxEncabezado.Size = new System.Drawing.Size(1255, 296);
            this.ComprasGbxEncabezado.TabIndex = 1;
            this.ComprasGbxEncabezado.TabStop = false;
            // 
            // ComprasLblDescripcion
            // 
            this.ComprasLblDescripcion.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDescripcion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblDescripcion.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblDescripcion.Location = new System.Drawing.Point(7, 136);
            this.ComprasLblDescripcion.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblDescripcion.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblDescripcion.MostrarAsterisco = false;
            this.ComprasLblDescripcion.Name = "ComprasLblDescripcion";
            this.ComprasLblDescripcion.Size = new System.Drawing.Size(236, 32);
            this.ComprasLblDescripcion.TabIndex = 7;
            this.ComprasLblDescripcion.TabStop = false;
            this.ComprasLblDescripcion.Text = "Descripcion";
            this.ComprasLblDescripcion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblDescripcion.Texto = "Descripcion";
            this.ComprasLblDescripcion.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasTxtDescripcion
            // 
            this.ComprasTxtDescripcion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtDescripcion.Location = new System.Drawing.Point(10, 174);
            this.ComprasTxtDescripcion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtDescripcion.MaxLength = 2147483647;
            this.ComprasTxtDescripcion.MostrarEtiquetaError = false;
            this.ComprasTxtDescripcion.Multiline = true;
            this.ComprasTxtDescripcion.Name = "ComprasTxtDescripcion";
            this.ComprasTxtDescripcion.SelectedText = "";
            this.ComprasTxtDescripcion.SelectionLength = 0;
            this.ComprasTxtDescripcion.SelectionStart = 0;
            this.ComprasTxtDescripcion.Size = new System.Drawing.Size(1216, 85);
            this.ComprasTxtDescripcion.TabIndex = 8;
            // 
            // ComprasLblFecha
            // 
            this.ComprasLblFecha.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFecha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblFecha.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblFecha.Location = new System.Drawing.Point(22, 36);
            this.ComprasLblFecha.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblFecha.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblFecha.MostrarAsterisco = false;
            this.ComprasLblFecha.Name = "ComprasLblFecha";
            this.ComprasLblFecha.Size = new System.Drawing.Size(224, 32);
            this.ComprasLblFecha.TabIndex = 5;
            this.ComprasLblFecha.TabStop = false;
            this.ComprasLblFecha.Text = "Fecha";
            this.ComprasLblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFecha.Texto = "Fecha";
            this.ComprasLblFecha.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasCboFecha
            // 
            this.ComprasCboFecha.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboFecha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboFecha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboFecha.Location = new System.Drawing.Point(22, 73);
            this.ComprasCboFecha.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboFecha.MaxLength = 0;
            this.ComprasCboFecha.MostrarEtiquetaError = false;
            this.ComprasCboFecha.Name = "ComprasCboFecha";
            this.ComprasCboFecha.Size = new System.Drawing.Size(224, 45);
            this.ComprasCboFecha.TabIndex = 3;
            // 
            // ComprasLblPeriodo
            // 
            this.ComprasLblPeriodo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblPeriodo.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblPeriodo.Location = new System.Drawing.Point(260, 36);
            this.ComprasLblPeriodo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblPeriodo.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblPeriodo.MostrarAsterisco = false;
            this.ComprasLblPeriodo.Name = "ComprasLblPeriodo";
            this.ComprasLblPeriodo.Size = new System.Drawing.Size(236, 32);
            this.ComprasLblPeriodo.TabIndex = 5;
            this.ComprasLblPeriodo.TabStop = false;
            this.ComprasLblPeriodo.Text = "Periodo";
            this.ComprasLblPeriodo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblPeriodo.Texto = "Periodo";
            this.ComprasLblPeriodo.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasTxtPeriodo
            // 
            this.ComprasTxtPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasTxtPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasTxtPeriodo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasTxtPeriodo.Location = new System.Drawing.Point(275, 73);
            this.ComprasTxtPeriodo.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTxtPeriodo.MaxLength = 2147483647;
            this.ComprasTxtPeriodo.MostrarEtiquetaError = false;
            this.ComprasTxtPeriodo.Multiline = true;
            this.ComprasTxtPeriodo.Name = "ComprasTxtPeriodo";
            this.ComprasTxtPeriodo.SelectedText = "";
            this.ComprasTxtPeriodo.SelectionLength = 0;
            this.ComprasTxtPeriodo.SelectionStart = 0;
            this.ComprasTxtPeriodo.Size = new System.Drawing.Size(236, 36);
            this.ComprasTxtPeriodo.TabIndex = 6;
            // 
            // ComprasLblOrigen
            // 
            this.ComprasLblOrigen.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblOrigen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblOrigen.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblOrigen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblOrigen.Location = new System.Drawing.Point(850, 30);
            this.ComprasLblOrigen.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblOrigen.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblOrigen.MostrarAsterisco = false;
            this.ComprasLblOrigen.Name = "ComprasLblOrigen";
            this.ComprasLblOrigen.Size = new System.Drawing.Size(338, 32);
            this.ComprasLblOrigen.TabIndex = 5;
            this.ComprasLblOrigen.TabStop = false;
            this.ComprasLblOrigen.Text = "Origen";
            this.ComprasLblOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblOrigen.Texto = "Origen";
            this.ComprasLblOrigen.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblEstado.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.ComprasLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblEstado.Location = new System.Drawing.Point(541, 30);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ComprasLblEstado.MaximumSize = new System.Drawing.Size(1625, 0);
            this.ComprasLblEstado.MostrarAsterisco = false;
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(278, 32);
            this.ComprasLblEstado.TabIndex = 4;
            this.ComprasLblEstado.TabStop = false;
            this.ComprasLblEstado.Text = "Estado";
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblEstado.Texto = "Estado";
            this.ComprasLblEstado.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.Campo;
            // 
            // ComprasCboOrigen
            // 
            this.ComprasCboOrigen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboOrigen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboOrigen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboOrigen.Location = new System.Drawing.Point(876, 74);
            this.ComprasCboOrigen.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboOrigen.MaxLength = 0;
            this.ComprasCboOrigen.MostrarEtiquetaError = false;
            this.ComprasCboOrigen.Name = "ComprasCboOrigen";
            this.ComprasCboOrigen.Size = new System.Drawing.Size(289, 44);
            this.ComprasCboOrigen.TabIndex = 3;
            // 
            // ComprasCboEstado
            // 
            this.ComprasCboEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ComprasCboEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboEstado.Location = new System.Drawing.Point(541, 73);
            this.ComprasCboEstado.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasCboEstado.MaxLength = 0;
            this.ComprasCboEstado.MostrarEtiquetaError = false;
            this.ComprasCboEstado.Name = "ComprasCboEstado";
            this.ComprasCboEstado.Size = new System.Drawing.Size(289, 45);
            this.ComprasCboEstado.TabIndex = 2;
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
            // ComprasLblTitulo
            // 
            this.ComprasLblTitulo.AlineacionTexto = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasLblTitulo.Font = new System.Drawing.Font("Lucida Sans", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.ComprasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasLblTitulo.Location = new System.Drawing.Point(346, 33);
            this.ComprasLblTitulo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.ComprasLblTitulo.MaximumSize = new System.Drawing.Size(2375, 0);
            this.ComprasLblTitulo.MostrarAsterisco = false;
            this.ComprasLblTitulo.Name = "ComprasLblTitulo";
            this.ComprasLblTitulo.Size = new System.Drawing.Size(494, 36);
            this.ComprasLblTitulo.TabIndex = 0;
            this.ComprasLblTitulo.TabStop = false;
            this.ComprasLblTitulo.Text = "Póliza de compras";
            this.ComprasLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ComprasLblTitulo.Texto = "Póliza de compras";
            this.ComprasLblTitulo.Tipografia = CapaVista_Componentes.Componentes.TipografiaCompras.TituloFormulario;
            // 
            // FrmPolizas
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.ComprasPnlDesplazamiento);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmPolizas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6002 – Polizas";
            this.ComprasTlpFormulario.ResumeLayout(false);
            this.ComprasPnlContenido.ResumeLayout(false);
            this.ComprasGbxDetalle.ResumeLayout(false);
            this.ComprasGbxEncabezado.ResumeLayout(false);
            this.ComprasPnlDesplazamiento.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxEncabezado;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblTitulo;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtPeriodo;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblOrigen;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboOrigen;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboEstado;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDescripcion;
        private CapaVista_Componentes.Componentes.ComprasUsrTextBox ComprasTxtDescripcion;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblFecha;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboFecha;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblPeriodo;
        private CapaVista_Componentes.Componentes.ClsGroupBoxCompras ComprasGbxDetalle;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblDiferencia;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnEnviar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnGuardar;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnNuevo;
        private CapaVista_Componentes.Componentes.ClsButtonCompras ComprasBtnIngresarCuenta;
        private CapaVista_Componentes.Componentes.ComprasUsrComboBox ComprasCboCuenta;
        private CapaVista_Componentes.Componentes.ComprasUsrLabel ComprasLblCuenta;
    }
}
