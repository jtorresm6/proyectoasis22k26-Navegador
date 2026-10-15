/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón
 * Carné: 0901-23-1998
 * Fecha: 14/10/2026
 * Propósito: prototipo no funcional de pólizas locales de Embutidos de Calidad,
 * basado en cuenta, poliza_encabezado y poliza_detalle de presupuesto_db.
 */
namespace CapaVista_Contabilidad
{
    partial class FrmPolizasLocales
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(Disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.ContabilidadPnlEncabezado = new Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblTitulo = new System.Windows.Forms.Label();
            this.ContabilidadLblFecha = new System.Windows.Forms.Label();
            this.ContabilidadLblPeriodo = new System.Windows.Forms.Label();
            this.ContabilidadLblEstado = new System.Windows.Forms.Label();
            this.ContabilidadLblOrigen = new System.Windows.Forms.Label();
            this.ContabilidadDtpFecha = new System.Windows.Forms.DateTimePicker();
            this.ContabilidadTxtPeriodo = new System.Windows.Forms.TextBox();
            this.ContabilidadCboEstado = new System.Windows.Forms.ComboBox();
            this.ContabilidadCboOrigen = new System.Windows.Forms.ComboBox();
            this.ContabilidadLblDescripcion = new System.Windows.Forms.Label();
            this.ContabilidadTxtDescripcion = new System.Windows.Forms.TextBox();
            this.ContabilidadPnlDetalle = new Componentes.ClsPanelRedondeadoContabilidad();
            this.ContabilidadLblAgregarCuenta = new System.Windows.Forms.Label();
            this.ContabilidadCboCuenta = new System.Windows.Forms.ComboBox();
            this.ContabilidadBtnAgregar = new Componentes.ClsBotonContabilidad();
            this.ContabilidadDgvDetalle = new Componentes.ClsTablaContabilidad();
            this.ContabilidadColCuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColDebe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColHaber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContabilidadColQuitar = new System.Windows.Forms.DataGridViewImageColumn();
            this.ContabilidadLblCuadre = new System.Windows.Forms.Label();
            this.ContabilidadLblTotalDebe = new System.Windows.Forms.Label();
            this.ContabilidadLblTotalHaber = new System.Windows.Forms.Label();
            this.ContabilidadLblEmpresa = new System.Windows.Forms.Label();
            this.ContabilidadBtnNueva = new Componentes.ClsBotonContabilidad();
            this.ContabilidadBtnLimpiarDetalle = new Componentes.ClsBotonContabilidad();
            this.ContabilidadBtnGuardar = new Componentes.ClsBotonContabilidad();
            this.ContabilidadBtnAplicar = new Componentes.ClsBotonContabilidad();
            System.Windows.Forms.DataGridViewCellStyle ContabilidadEstiloDebe = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle ContabilidadEstiloHaber = new System.Windows.Forms.DataGridViewCellStyle();
            ContabilidadEstiloDebe.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            ContabilidadEstiloDebe.Format = "N2";
            ContabilidadEstiloDebe.NullValue = "0.00";
            ContabilidadEstiloHaber.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            ContabilidadEstiloHaber.Format = "N2";
            ContabilidadEstiloHaber.NullValue = "0.00";
            this.ContabilidadPnlEncabezado.SuspendLayout();
            this.ContabilidadPnlDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvDetalle)).BeginInit();
            this.SuspendLayout();
            // ContabilidadPnlEncabezado
            this.ContabilidadPnlEncabezado.Name = "ContabilidadPnlEncabezado";
            this.ContabilidadPnlEncabezado.Location = new System.Drawing.Point(24, 24);
            this.ContabilidadPnlEncabezado.Size = new System.Drawing.Size(1132, 640);
            this.ContabilidadPnlEncabezado.TabIndex = 0;
            this.ContabilidadPnlEncabezado.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlEncabezado.ColorRelleno = System.Drawing.Color.White;
            this.ContabilidadPnlEncabezado.ColorBorde = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlEncabezado.Radio = 18;
            // ContabilidadLblTitulo
            this.ContabilidadLblTitulo.Name = "ContabilidadLblTitulo";
            this.ContabilidadLblTitulo.Location = new System.Drawing.Point(24, 18);
            this.ContabilidadLblTitulo.Size = new System.Drawing.Size(1084, 36);
            this.ContabilidadLblTitulo.TabIndex = 0;
            this.ContabilidadLblTitulo.Text = "Póliza Encabezado";
            this.ContabilidadLblTitulo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTitulo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.ContabilidadLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblTitulo);
            // ContabilidadLblFecha
            this.ContabilidadLblFecha.Name = "ContabilidadLblFecha";
            this.ContabilidadLblFecha.Location = new System.Drawing.Point(24, 72);
            this.ContabilidadLblFecha.Size = new System.Drawing.Size(220, 22);
            this.ContabilidadLblFecha.TabIndex = 0;
            this.ContabilidadLblFecha.Text = "Fecha";
            this.ContabilidadLblFecha.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblFecha.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblFecha.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblFecha);
            // ContabilidadLblPeriodo
            this.ContabilidadLblPeriodo.Name = "ContabilidadLblPeriodo";
            this.ContabilidadLblPeriodo.Location = new System.Drawing.Point(264, 72);
            this.ContabilidadLblPeriodo.Size = new System.Drawing.Size(220, 22);
            this.ContabilidadLblPeriodo.TabIndex = 0;
            this.ContabilidadLblPeriodo.Text = "Período contable";
            this.ContabilidadLblPeriodo.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblPeriodo.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblPeriodo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblPeriodo);
            // ContabilidadLblEstado
            this.ContabilidadLblEstado.Name = "ContabilidadLblEstado";
            this.ContabilidadLblEstado.Location = new System.Drawing.Point(504, 72);
            this.ContabilidadLblEstado.Size = new System.Drawing.Size(270, 22);
            this.ContabilidadLblEstado.TabIndex = 0;
            this.ContabilidadLblEstado.Text = "Estado";
            this.ContabilidadLblEstado.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblEstado.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblEstado.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblEstado);
            // ContabilidadLblOrigen
            this.ContabilidadLblOrigen.Name = "ContabilidadLblOrigen";
            this.ContabilidadLblOrigen.Location = new System.Drawing.Point(794, 72);
            this.ContabilidadLblOrigen.Size = new System.Drawing.Size(314, 22);
            this.ContabilidadLblOrigen.TabIndex = 0;
            this.ContabilidadLblOrigen.Text = "Origen";
            this.ContabilidadLblOrigen.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblOrigen.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblOrigen.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblOrigen);
            // ContabilidadDtpFecha
            this.ContabilidadDtpFecha.Name = "ContabilidadDtpFecha";
            this.ContabilidadDtpFecha.Location = new System.Drawing.Point(24, 100);
            this.ContabilidadDtpFecha.Size = new System.Drawing.Size(220, 28);
            this.ContabilidadDtpFecha.TabIndex = 0;
            this.ContabilidadDtpFecha.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadDtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ContabilidadDtpFecha.CustomFormat = "dd/MM/yyyy";
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadDtpFecha);
            // ContabilidadTxtPeriodo
            this.ContabilidadTxtPeriodo.Name = "ContabilidadTxtPeriodo";
            this.ContabilidadTxtPeriodo.Location = new System.Drawing.Point(264, 100);
            this.ContabilidadTxtPeriodo.Size = new System.Drawing.Size(220, 28);
            this.ContabilidadTxtPeriodo.TabIndex = 1;
            this.ContabilidadTxtPeriodo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadTxtPeriodo.ReadOnly = true;
            this.ContabilidadTxtPeriodo.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.ContabilidadTxtPeriodo.TabStop = false;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadTxtPeriodo);
            // ContabilidadCboEstado
            this.ContabilidadCboEstado.Name = "ContabilidadCboEstado";
            this.ContabilidadCboEstado.Location = new System.Drawing.Point(504, 100);
            this.ContabilidadCboEstado.Size = new System.Drawing.Size(270, 28);
            this.ContabilidadCboEstado.TabIndex = 2;
            this.ContabilidadCboEstado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboEstado.FormattingEnabled = true;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadCboEstado);
            // ContabilidadCboOrigen
            this.ContabilidadCboOrigen.Name = "ContabilidadCboOrigen";
            this.ContabilidadCboOrigen.Location = new System.Drawing.Point(794, 100);
            this.ContabilidadCboOrigen.Size = new System.Drawing.Size(314, 28);
            this.ContabilidadCboOrigen.TabIndex = 3;
            this.ContabilidadCboOrigen.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ContabilidadCboOrigen.FormattingEnabled = true;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadCboOrigen);
            // ContabilidadLblDescripcion
            this.ContabilidadLblDescripcion.Name = "ContabilidadLblDescripcion";
            this.ContabilidadLblDescripcion.Location = new System.Drawing.Point(24, 144);
            this.ContabilidadLblDescripcion.Size = new System.Drawing.Size(1084, 22);
            this.ContabilidadLblDescripcion.TabIndex = 0;
            this.ContabilidadLblDescripcion.Text = "Descripción";
            this.ContabilidadLblDescripcion.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblDescripcion.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblDescripcion.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblDescripcion);
            // ContabilidadTxtDescripcion
            this.ContabilidadTxtDescripcion.Name = "ContabilidadTxtDescripcion";
            this.ContabilidadTxtDescripcion.Location = new System.Drawing.Point(24, 172);
            this.ContabilidadTxtDescripcion.Size = new System.Drawing.Size(1084, 28);
            this.ContabilidadTxtDescripcion.TabIndex = 4;
            this.ContabilidadTxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadTxtDescripcion.MaxLength = 255;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadTxtDescripcion);
            // ContabilidadPnlDetalle
            this.ContabilidadPnlDetalle.Name = "ContabilidadPnlDetalle";
            this.ContabilidadPnlDetalle.Location = new System.Drawing.Point(24, 220);
            this.ContabilidadPnlDetalle.Size = new System.Drawing.Size(1084, 336);
            this.ContabilidadPnlDetalle.TabIndex = 0;
            this.ContabilidadPnlDetalle.BackColor = System.Drawing.Color.Transparent;
            this.ContabilidadPnlDetalle.ColorRelleno = System.Drawing.Color.White;
            this.ContabilidadPnlDetalle.ColorBorde = System.Drawing.Color.FromArgb(212, 222, 228);
            this.ContabilidadPnlDetalle.Radio = 18;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadPnlDetalle);
            // ContabilidadLblAgregarCuenta
            this.ContabilidadLblAgregarCuenta.Name = "ContabilidadLblAgregarCuenta";
            this.ContabilidadLblAgregarCuenta.Location = new System.Drawing.Point(20, 16);
            this.ContabilidadLblAgregarCuenta.Size = new System.Drawing.Size(500, 22);
            this.ContabilidadLblAgregarCuenta.TabIndex = 0;
            this.ContabilidadLblAgregarCuenta.Text = "Agregar cuenta...";
            this.ContabilidadLblAgregarCuenta.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblAgregarCuenta.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblAgregarCuenta.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadLblAgregarCuenta);
            // ContabilidadCboCuenta
            this.ContabilidadCboCuenta.Name = "ContabilidadCboCuenta";
            this.ContabilidadCboCuenta.Location = new System.Drawing.Point(20, 44);
            this.ContabilidadCboCuenta.Size = new System.Drawing.Size(500, 28);
            this.ContabilidadCboCuenta.TabIndex = 0;
            this.ContabilidadCboCuenta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadCboCuenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.ContabilidadCboCuenta.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ContabilidadCboCuenta.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ContabilidadCboCuenta.FormattingEnabled = true;
            this.ContabilidadCboCuenta.DropDownWidth = 500;
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadCboCuenta);
            // ContabilidadBtnAgregar
            this.ContabilidadBtnAgregar.Name = "ContabilidadBtnAgregar";
            this.ContabilidadBtnAgregar.Location = new System.Drawing.Point(536, 38);
            this.ContabilidadBtnAgregar.Size = new System.Drawing.Size(136, 42);
            this.ContabilidadBtnAgregar.TabIndex = 1;
            this.ContabilidadBtnAgregar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnAgregar.Text = "Agregar";
            this.ContabilidadBtnAgregar.Estilo = Componentes.EstiloBotonContabilidad.Dorado;
            this.ContabilidadBtnAgregar.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_mas;
            this.ContabilidadBtnAgregar.Radio = 10;
            this.ContabilidadBtnAgregar.UseVisualStyleBackColor = false;
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadBtnAgregar);
            // ContabilidadDgvDetalle
            this.ContabilidadDgvDetalle.Name = "ContabilidadDgvDetalle";
            this.ContabilidadDgvDetalle.Location = new System.Drawing.Point(20, 94);
            this.ContabilidadDgvDetalle.Size = new System.Drawing.Size(1044, 182);
            this.ContabilidadDgvDetalle.TabIndex = 2;
            this.ContabilidadDgvDetalle.AutoGenerateColumns = false;
            this.ContabilidadDgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ContabilidadDgvDetalle.AllowUserToOrderColumns = false;
            this.ContabilidadDgvDetalle.AllowUserToResizeColumns = false;
            this.ContabilidadDgvDetalle.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.ContabilidadDgvDetalle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadDgvDetalle);
            // ContabilidadColCuenta
            this.ContabilidadColCuenta.Name = "ContabilidadColCuenta";
            this.ContabilidadColCuenta.HeaderText = "Cuenta";
            this.ContabilidadColCuenta.DataPropertyName = "Cuenta";
            this.ContabilidadColCuenta.ReadOnly = true;
            this.ContabilidadColCuenta.FillWeight = 520;
            this.ContabilidadColCuenta.MinimumWidth = 300;
            this.ContabilidadColCuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // ContabilidadColDebe
            this.ContabilidadColDebe.Name = "ContabilidadColDebe";
            this.ContabilidadColDebe.HeaderText = "Debe";
            this.ContabilidadColDebe.DataPropertyName = "Debe";
            this.ContabilidadColDebe.ReadOnly = false;
            this.ContabilidadColDebe.FillWeight = 240;
            this.ContabilidadColDebe.MinimumWidth = 200;
            this.ContabilidadColDebe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContabilidadColDebe.ValueType = typeof(decimal);
            this.ContabilidadColDebe.DefaultCellStyle = ContabilidadEstiloDebe;
            // ContabilidadColHaber
            this.ContabilidadColHaber.Name = "ContabilidadColHaber";
            this.ContabilidadColHaber.HeaderText = "Haber";
            this.ContabilidadColHaber.DataPropertyName = "Haber";
            this.ContabilidadColHaber.ReadOnly = false;
            this.ContabilidadColHaber.FillWeight = 240;
            this.ContabilidadColHaber.MinimumWidth = 200;
            this.ContabilidadColHaber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContabilidadColHaber.ValueType = typeof(decimal);
            this.ContabilidadColHaber.DefaultCellStyle = ContabilidadEstiloHaber;
            // ContabilidadColQuitar
            this.ContabilidadColQuitar.Name = "ContabilidadColQuitar";
            this.ContabilidadColQuitar.HeaderText = "";
            this.ContabilidadColQuitar.ToolTipText = "Quitar cuenta";
            this.ContabilidadColQuitar.Image = global::CapaVista_Contabilidad.Properties.Resources.ico_quitar_oscuro;
            this.ContabilidadColQuitar.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ContabilidadColQuitar.ReadOnly = true;
            this.ContabilidadColQuitar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ContabilidadColQuitar.Width = 44;
            this.ContabilidadColQuitar.MinimumWidth = 44;
            this.ContabilidadColQuitar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContabilidadDgvDetalle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.ContabilidadColCuenta, this.ContabilidadColDebe, this.ContabilidadColHaber, this.ContabilidadColQuitar });
            // ContabilidadLblCuadre
            this.ContabilidadLblCuadre.Name = "ContabilidadLblCuadre";
            this.ContabilidadLblCuadre.Location = new System.Drawing.Point(20, 290);
            this.ContabilidadLblCuadre.Size = new System.Drawing.Size(500, 28);
            this.ContabilidadLblCuadre.TabIndex = 0;
            this.ContabilidadLblCuadre.Text = "Descuadrada · diferencia Q 0.00";
            this.ContabilidadLblCuadre.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblCuadre.ForeColor = System.Drawing.Color.FromArgb(197, 143, 62);
            this.ContabilidadLblCuadre.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadLblCuadre);
            // ContabilidadLblTotalDebe
            this.ContabilidadLblTotalDebe.Name = "ContabilidadLblTotalDebe";
            this.ContabilidadLblTotalDebe.Location = new System.Drawing.Point(540, 290);
            this.ContabilidadLblTotalDebe.Size = new System.Drawing.Size(240, 28);
            this.ContabilidadLblTotalDebe.TabIndex = 0;
            this.ContabilidadLblTotalDebe.Text = "Total debe: Q 0.00";
            this.ContabilidadLblTotalDebe.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTotalDebe.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTotalDebe.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblTotalDebe.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadLblTotalDebe);
            // ContabilidadLblTotalHaber
            this.ContabilidadLblTotalHaber.Name = "ContabilidadLblTotalHaber";
            this.ContabilidadLblTotalHaber.Location = new System.Drawing.Point(780, 290);
            this.ContabilidadLblTotalHaber.Size = new System.Drawing.Size(240, 28);
            this.ContabilidadLblTotalHaber.TabIndex = 0;
            this.ContabilidadLblTotalHaber.Text = "Total haber: Q 0.00";
            this.ContabilidadLblTotalHaber.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblTotalHaber.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.ContabilidadLblTotalHaber.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadLblTotalHaber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ContabilidadPnlDetalle.Controls.Add(this.ContabilidadLblTotalHaber);
            // ContabilidadLblEmpresa
            this.ContabilidadLblEmpresa.Name = "ContabilidadLblEmpresa";
            this.ContabilidadLblEmpresa.Location = new System.Drawing.Point(24, 576);
            this.ContabilidadLblEmpresa.Size = new System.Drawing.Size(310, 46);
            this.ContabilidadLblEmpresa.TabIndex = 0;
            this.ContabilidadLblEmpresa.Text = "Embutidos de Calidad";
            this.ContabilidadLblEmpresa.BackColor = System.Drawing.Color.White;
            this.ContabilidadLblEmpresa.ForeColor = System.Drawing.Color.FromArgb(70, 104, 125);
            this.ContabilidadLblEmpresa.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadLblEmpresa);
            // ContabilidadBtnNueva
            this.ContabilidadBtnNueva.Name = "ContabilidadBtnNueva";
            this.ContabilidadBtnNueva.Location = new System.Drawing.Point(466, 576);
            this.ContabilidadBtnNueva.Size = new System.Drawing.Size(120, 42);
            this.ContabilidadBtnNueva.TabIndex = 5;
            this.ContabilidadBtnNueva.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnNueva.Text = "Nueva";
            this.ContabilidadBtnNueva.Estilo = Componentes.EstiloBotonContabilidad.Secundario;
            this.ContabilidadBtnNueva.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_nuevo_oscuro;
            this.ContabilidadBtnNueva.Radio = 10;
            this.ContabilidadBtnNueva.UseVisualStyleBackColor = false;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadBtnNueva);
            // ContabilidadBtnLimpiarDetalle
            this.ContabilidadBtnLimpiarDetalle.Name = "ContabilidadBtnLimpiarDetalle";
            this.ContabilidadBtnLimpiarDetalle.Location = new System.Drawing.Point(598, 576);
            this.ContabilidadBtnLimpiarDetalle.Size = new System.Drawing.Size(170, 42);
            this.ContabilidadBtnLimpiarDetalle.TabIndex = 6;
            this.ContabilidadBtnLimpiarDetalle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnLimpiarDetalle.Text = "Limpiar detalle";
            this.ContabilidadBtnLimpiarDetalle.Estilo = Componentes.EstiloBotonContabilidad.Secundario;
            this.ContabilidadBtnLimpiarDetalle.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_limpiar_oscuro;
            this.ContabilidadBtnLimpiarDetalle.Radio = 10;
            this.ContabilidadBtnLimpiarDetalle.UseVisualStyleBackColor = false;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadBtnLimpiarDetalle);
            // ContabilidadBtnGuardar
            this.ContabilidadBtnGuardar.Name = "ContabilidadBtnGuardar";
            this.ContabilidadBtnGuardar.Location = new System.Drawing.Point(780, 576);
            this.ContabilidadBtnGuardar.Size = new System.Drawing.Size(190, 42);
            this.ContabilidadBtnGuardar.TabIndex = 7;
            this.ContabilidadBtnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnGuardar.Text = "Guardar borrador";
            this.ContabilidadBtnGuardar.Estilo = Componentes.EstiloBotonContabilidad.Acento;
            this.ContabilidadBtnGuardar.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_guardar;
            this.ContabilidadBtnGuardar.Radio = 10;
            this.ContabilidadBtnGuardar.UseVisualStyleBackColor = false;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadBtnGuardar);
            // ContabilidadBtnAplicar
            this.ContabilidadBtnAplicar.Name = "ContabilidadBtnAplicar";
            this.ContabilidadBtnAplicar.Location = new System.Drawing.Point(982, 576);
            this.ContabilidadBtnAplicar.Size = new System.Drawing.Size(126, 42);
            this.ContabilidadBtnAplicar.TabIndex = 8;
            this.ContabilidadBtnAplicar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ContabilidadBtnAplicar.Text = "Aplicar";
            this.ContabilidadBtnAplicar.Estilo = Componentes.EstiloBotonContabilidad.Principal;
            this.ContabilidadBtnAplicar.Icono = global::CapaVista_Contabilidad.Properties.Resources.ico_aplicar;
            this.ContabilidadBtnAplicar.Radio = 10;
            this.ContabilidadBtnAplicar.UseVisualStyleBackColor = false;
            this.ContabilidadPnlEncabezado.Controls.Add(this.ContabilidadBtnAplicar);
            // FrmPolizasLocales
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(246, 247, 248);
            this.ClientSize = new System.Drawing.Size(1180, 688);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(35, 50, 61);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmPolizasLocales";
            this.Text = "Pólizas locales";
            this.Controls.Add(this.ContabilidadPnlEncabezado);
            this.ContabilidadPnlEncabezado.ResumeLayout(false);
            this.ContabilidadPnlEncabezado.PerformLayout();
            this.ContabilidadPnlDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ContabilidadDgvDetalle)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlEncabezado;
        private System.Windows.Forms.Label ContabilidadLblTitulo;
        private System.Windows.Forms.Label ContabilidadLblFecha;
        private System.Windows.Forms.Label ContabilidadLblPeriodo;
        private System.Windows.Forms.Label ContabilidadLblEstado;
        private System.Windows.Forms.Label ContabilidadLblOrigen;
        private System.Windows.Forms.DateTimePicker ContabilidadDtpFecha;
        private System.Windows.Forms.TextBox ContabilidadTxtPeriodo;
        private System.Windows.Forms.ComboBox ContabilidadCboEstado;
        private System.Windows.Forms.ComboBox ContabilidadCboOrigen;
        private System.Windows.Forms.Label ContabilidadLblDescripcion;
        private System.Windows.Forms.TextBox ContabilidadTxtDescripcion;
        private Componentes.ClsPanelRedondeadoContabilidad ContabilidadPnlDetalle;
        private System.Windows.Forms.Label ContabilidadLblAgregarCuenta;
        private System.Windows.Forms.ComboBox ContabilidadCboCuenta;
        private Componentes.ClsBotonContabilidad ContabilidadBtnAgregar;
        private Componentes.ClsTablaContabilidad ContabilidadDgvDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColCuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColDebe;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContabilidadColHaber;
        private System.Windows.Forms.DataGridViewImageColumn ContabilidadColQuitar;
        private System.Windows.Forms.Label ContabilidadLblCuadre;
        private System.Windows.Forms.Label ContabilidadLblTotalDebe;
        private System.Windows.Forms.Label ContabilidadLblTotalHaber;
        private System.Windows.Forms.Label ContabilidadLblEmpresa;
        private Componentes.ClsBotonContabilidad ContabilidadBtnNueva;
        private Componentes.ClsBotonContabilidad ContabilidadBtnLimpiarDetalle;
        private Componentes.ClsBotonContabilidad ContabilidadBtnGuardar;
        private Componentes.ClsBotonContabilidad ContabilidadBtnAplicar;
    }
}
