using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_MovimientosProveedores
{
    partial class FrmMovimientosProveedores
    {
        private System.ComponentModel.IContainer _Componentes = null;
        private ComprasUsrEncabezado ComprasUsrEncabezadoFormulario;
        private ClsButtonCompras ComprasBtnGuardar, ComprasBtnNueva;
        private ComprasUsrComboBox ComprasCboProveedor, ComprasCboConcepto, ComprasCboMoneda;
        private TextBox ComprasTxtSerie, ComprasTxtNumero;
        private ClsDatePickerCompras ComprasDtpFechaEmision, ComprasDtpFechaVencimiento;
        private Label ComprasLblEfecto, ComprasLblEquivalente;
        private LinkLabel ComprasLblEstado;
        private TableLayoutPanel ComprasTlpPagina, ComprasTlpCampos;
        private FlowLayoutPanel ComprasFlpAcciones;
        private Panel ComprasPnlScroll;

        private Label ComprasLblCampoCboProveedor;
        private Label ComprasLblCampoCboConcepto;
        private Label ComprasLblCampoCboMoneda;
        private Label ComprasLblCampoTxtSerie;
        private Label ComprasLblCampoTxtNumero;
        private Label ComprasLblCampoNumMonto;
        private Label ComprasLblCampoFechaEmision;
        private Label ComprasLblCampoFechaVencimiento;
        private Label ComprasLblCampoNumCambio;
        private Label ComprasLblAyuda;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && _Componentes != null) _Componentes.Dispose();
            base.Dispose(LiberarRecursos);
        }

        private void InitializeComponent()
        {
            this.ComprasTxtSerie = new System.Windows.Forms.TextBox();
            this.ComprasTxtNumero = new System.Windows.Forms.TextBox();
            this.ComprasLblEfecto = new System.Windows.Forms.Label();
            this.ComprasLblEquivalente = new System.Windows.Forms.Label();
            this.ComprasLblEstado = new System.Windows.Forms.LinkLabel();
            this.ComprasTlpPagina = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasFlpAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.ComprasBtnNueva = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnGuardar = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasBtnHistorial = new CapaVista_Componentes.Componentes.ClsButtonCompras();
            this.ComprasTlpCampos = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasTxtMonto = new System.Windows.Forms.TextBox();
            this.ComprasLblCampoCboProveedor = new System.Windows.Forms.Label();
            this.ComprasCboProveedor = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblCampoCboConcepto = new System.Windows.Forms.Label();
            this.ComprasCboConcepto = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblCampoCboMoneda = new System.Windows.Forms.Label();
            this.ComprasCboMoneda = new CapaVista_Componentes.Componentes.ComprasUsrComboBox();
            this.ComprasLblCampoTxtSerie = new System.Windows.Forms.Label();
            this.ComprasLblCampoTxtNumero = new System.Windows.Forms.Label();
            this.ComprasLblCampoNumMonto = new System.Windows.Forms.Label();
            this.ComprasLblCampoFechaEmision = new System.Windows.Forms.Label();
            this.ComprasDtpFechaEmision = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasLblCampoFechaVencimiento = new System.Windows.Forms.Label();
            this.ComprasDtpFechaVencimiento = new CapaVista_Componentes.Componentes.ClsDatePickerCompras();
            this.ComprasLblCampoNumCambio = new System.Windows.Forms.Label();
            this.ComprasTxtTipoCambio = new System.Windows.Forms.TextBox();
            this.ComprasLblAyuda = new System.Windows.Forms.Label();
            this.ComprasPnlScroll = new System.Windows.Forms.Panel();
            this.ComprasUsrEncabezadoFormulario = new CapaVista_Componentes.Componentes.ComprasUsrEncabezado();
            this.ComprasTlpPagina.SuspendLayout();
            this.ComprasFlpAcciones.SuspendLayout();
            this.ComprasTlpCampos.SuspendLayout();
            this.ComprasPnlScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // ComprasTxtSerie
            // 
            this.ComprasTxtSerie.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTxtSerie.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasTxtSerie.Location = new System.Drawing.Point(16, 114);
            this.ComprasTxtSerie.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasTxtSerie.MaxLength = 20;
            this.ComprasTxtSerie.Name = "ComprasTxtSerie";
            this.ComprasTxtSerie.Size = new System.Drawing.Size(469, 32);
            this.ComprasTxtSerie.TabIndex = 7;
            // 
            // ComprasTxtNumero
            // 
            this.ComprasTxtNumero.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTxtNumero.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasTxtNumero.Location = new System.Drawing.Point(501, 114);
            this.ComprasTxtNumero.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasTxtNumero.MaxLength = 30;
            this.ComprasTxtNumero.Name = "ComprasTxtNumero";
            this.ComprasTxtNumero.Size = new System.Drawing.Size(348, 32);
            this.ComprasTxtNumero.TabIndex = 9;
            // 
            // ComprasLblEfecto
            // 
            this.ComprasLblEfecto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEfecto.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ComprasLblEfecto.Location = new System.Drawing.Point(4, 312);
            this.ComprasLblEfecto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasLblEfecto.Name = "ComprasLblEfecto";
            this.ComprasLblEfecto.Size = new System.Drawing.Size(1244, 34);
            this.ComprasLblEfecto.TabIndex = 2;
            this.ComprasLblEfecto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasLblEquivalente
            // 
            this.ComprasLblEquivalente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEquivalente.Location = new System.Drawing.Point(4, 346);
            this.ComprasLblEquivalente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasLblEquivalente.Name = "ComprasLblEquivalente";
            this.ComprasLblEquivalente.Size = new System.Drawing.Size(1244, 30);
            this.ComprasLblEquivalente.TabIndex = 3;
            this.ComprasLblEquivalente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasLblEstado
            // 
            this.ComprasLblEstado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblEstado.Location = new System.Drawing.Point(4, 412);
            this.ComprasLblEstado.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasLblEstado.Name = "ComprasLblEstado";
            this.ComprasLblEstado.Size = new System.Drawing.Size(1244, 36);
            this.ComprasLblEstado.TabIndex = 7;
            this.ComprasLblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasTlpPagina
            // 
            this.ComprasTlpPagina.AutoSize = true;
            this.ComprasTlpPagina.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ComprasTlpPagina.ColumnCount = 1;
            this.ComprasTlpPagina.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpPagina.Controls.Add(this.ComprasFlpAcciones, 0, 0);
            this.ComprasTlpPagina.Controls.Add(this.ComprasTlpCampos, 0, 1);
            this.ComprasTlpPagina.Controls.Add(this.ComprasLblEfecto, 0, 2);
            this.ComprasTlpPagina.Controls.Add(this.ComprasLblEquivalente, 0, 3);
            this.ComprasTlpPagina.Controls.Add(this.ComprasLblAyuda, 0, 4);
            this.ComprasTlpPagina.Controls.Add(this.ComprasLblEstado, 0, 5);
            this.ComprasTlpPagina.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTlpPagina.Location = new System.Drawing.Point(24, 12);
            this.ComprasTlpPagina.Name = "ComprasTlpPagina";
            this.ComprasTlpPagina.RowCount = 6;
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpPagina.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.ComprasTlpPagina.Size = new System.Drawing.Size(1252, 448);
            this.ComprasTlpPagina.TabIndex = 0;
            // 
            // ComprasFlpAcciones
            // 
            this.ComprasFlpAcciones.Controls.Add(this.ComprasBtnNueva);
            this.ComprasFlpAcciones.Controls.Add(this.ComprasBtnGuardar);
            this.ComprasFlpAcciones.Controls.Add(this.ComprasBtnHistorial);
            this.ComprasFlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasFlpAcciones.Location = new System.Drawing.Point(3, 3);
            this.ComprasFlpAcciones.Name = "ComprasFlpAcciones";
            this.ComprasFlpAcciones.Size = new System.Drawing.Size(1246, 46);
            this.ComprasFlpAcciones.TabIndex = 0;
            this.ComprasFlpAcciones.WrapContents = false;
            this.ComprasBtnNueva.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnNueva
            // 
            this.ComprasBtnNueva.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnNueva.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnNueva.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnNueva.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnNueva.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNueva.FlatAppearance.BorderSize = 0;
            this.ComprasBtnNueva.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnNueva.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnNueva.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnNueva.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnNueva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnNueva.Location = new System.Drawing.Point(0, 2);
            this.ComprasBtnNueva.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.ComprasBtnNueva.Name = "ComprasBtnNueva";
            this.ComprasBtnNueva.Size = new System.Drawing.Size(154, 40);
            this.ComprasBtnNueva.TabIndex = 2;
            this.ComprasBtnNueva.Text = "Limpiar campos";
            this.ComprasBtnNueva.UseVisualStyleBackColor = false;
            this.ComprasBtnGuardar.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnGuardar
            // 
            this.ComprasBtnGuardar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnGuardar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnGuardar.FlatAppearance.BorderSize = 0;
            this.ComprasBtnGuardar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnGuardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnGuardar.ForeColor = System.Drawing.Color.White;
            this.ComprasBtnGuardar.Location = new System.Drawing.Point(166, 2);
            this.ComprasBtnGuardar.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.ComprasBtnGuardar.Name = "ComprasBtnGuardar";
            this.ComprasBtnGuardar.Size = new System.Drawing.Size(154, 40);
            this.ComprasBtnGuardar.TabIndex = 3;
            this.ComprasBtnGuardar.Text = "Guardar";
            this.ComprasBtnGuardar.UseVisualStyleBackColor = false;
            this.ComprasBtnHistorial.TipoBoton = CapaVista_Componentes.Componentes.ComprasTipoBoton.Libre;
            // 
            // ComprasBtnHistorial
            // 
            this.ComprasBtnHistorial.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ComprasBtnHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(220)))), ((int)(((byte)(239)))));
            this.ComprasBtnHistorial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ComprasBtnHistorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComprasBtnHistorial.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnHistorial.FlatAppearance.BorderSize = 0;
            this.ComprasBtnHistorial.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(173)))), ((int)(((byte)(213)))));
            this.ComprasBtnHistorial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(224)))));
            this.ComprasBtnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ComprasBtnHistorial.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ComprasBtnHistorial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasBtnHistorial.Location = new System.Drawing.Point(332, 2);
            this.ComprasBtnHistorial.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.ComprasBtnHistorial.Name = "ComprasBtnHistorial";
            this.ComprasBtnHistorial.Size = new System.Drawing.Size(140, 40);
            this.ComprasBtnHistorial.TabIndex = 5;
            this.ComprasBtnHistorial.Text = "Historial";
            this.ComprasBtnHistorial.UseVisualStyleBackColor = false;
            this.ComprasBtnHistorial.Click += new System.EventHandler(this.ComprasMetAbrirHistorial);
            // 
            // ComprasTlpCampos
            // 
            this.ComprasTlpCampos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(247)))), ((int)(((byte)(242)))));
            this.ComprasTlpCampos.ColumnCount = 3;
            this.ComprasTlpCampos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ComprasTlpCampos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.ComprasTlpCampos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.ComprasTlpCampos.Controls.Add(this.ComprasTxtMonto, 2, 3);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoCboProveedor, 0, 0);
            this.ComprasTlpCampos.Controls.Add(this.ComprasCboProveedor, 0, 1);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoCboConcepto, 1, 0);
            this.ComprasTlpCampos.Controls.Add(this.ComprasCboConcepto, 1, 1);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoCboMoneda, 2, 0);
            this.ComprasTlpCampos.Controls.Add(this.ComprasCboMoneda, 2, 1);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoTxtSerie, 0, 2);
            this.ComprasTlpCampos.Controls.Add(this.ComprasTxtSerie, 0, 3);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoTxtNumero, 1, 2);
            this.ComprasTlpCampos.Controls.Add(this.ComprasTxtNumero, 1, 3);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoNumMonto, 2, 2);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoFechaEmision, 0, 4);
            this.ComprasTlpCampos.Controls.Add(this.ComprasDtpFechaEmision, 0, 5);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoFechaVencimiento, 1, 4);
            this.ComprasTlpCampos.Controls.Add(this.ComprasDtpFechaVencimiento, 1, 5);
            this.ComprasTlpCampos.Controls.Add(this.ComprasLblCampoNumCambio, 2, 4);
            this.ComprasTlpCampos.Controls.Add(this.ComprasTxtTipoCambio, 2, 5);
            this.ComprasTlpCampos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpCampos.Location = new System.Drawing.Point(3, 55);
            this.ComprasTlpCampos.Name = "ComprasTlpCampos";
            this.ComprasTlpCampos.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.ComprasTlpCampos.RowCount = 6;
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.ComprasTlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.ComprasTlpCampos.Size = new System.Drawing.Size(1246, 254);
            this.ComprasTlpCampos.TabIndex = 1;
            // 
            // ComprasTxtMonto
            // 
            this.ComprasTxtMonto.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTxtMonto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasTxtMonto.Location = new System.Drawing.Point(865, 114);
            this.ComprasTxtMonto.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasTxtMonto.MaxLength = 30;
            this.ComprasTxtMonto.Name = "ComprasTxtMonto";
            this.ComprasTxtMonto.Size = new System.Drawing.Size(349, 32);
            this.ComprasTxtMonto.TabIndex = 17;
            this.ComprasTxtMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ComprasLblCampoCboProveedor
            // 
            this.ComprasLblCampoCboProveedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoCboProveedor.Location = new System.Drawing.Point(19, 8);
            this.ComprasLblCampoCboProveedor.Name = "ComprasLblCampoCboProveedor";
            this.ComprasLblCampoCboProveedor.Size = new System.Drawing.Size(479, 24);
            this.ComprasLblCampoCboProveedor.TabIndex = 0;
            this.ComprasLblCampoCboProveedor.Text = "Proveedor *";
            this.ComprasLblCampoCboProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasCboProveedor
            // 
            this.ComprasCboProveedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboProveedor.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasCboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.ComprasCboProveedor.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasCboProveedor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboProveedor.Location = new System.Drawing.Point(16, 36);
            this.ComprasCboProveedor.Margin = new System.Windows.Forms.Padding(0, 4, 18, 0);
            this.ComprasCboProveedor.MaxLength = 0;
            this.ComprasCboProveedor.MostrarEtiquetaError = false;
            this.ComprasCboProveedor.Name = "ComprasCboProveedor";
            this.ComprasCboProveedor.PermitirEscritura = true;
            this.ComprasCboProveedor.Size = new System.Drawing.Size(467, 44);
            this.ComprasCboProveedor.TabIndex = 1;
            this.ComprasCboProveedor.TipoDatos = CapaVista_Componentes.Componentes.EnmTipoDatosCompras.Proveedores;
            // 
            // ComprasLblCampoCboConcepto
            // 
            this.ComprasLblCampoCboConcepto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoCboConcepto.Location = new System.Drawing.Point(504, 8);
            this.ComprasLblCampoCboConcepto.Name = "ComprasLblCampoCboConcepto";
            this.ComprasLblCampoCboConcepto.Size = new System.Drawing.Size(358, 24);
            this.ComprasLblCampoCboConcepto.TabIndex = 2;
            this.ComprasLblCampoCboConcepto.Text = "Concepto del documento *";
            this.ComprasLblCampoCboConcepto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasCboConcepto
            //
            this.ComprasCboConcepto.BackColor = System.Drawing.Color.FromArgb(246, 247, 250);
            this.ComprasCboConcepto.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasCboConcepto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComprasCboConcepto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasCboConcepto.ForeColor = System.Drawing.Color.FromArgb(30, 42, 90);
            this.ComprasCboConcepto.Location = new System.Drawing.Point(501, 36);
            this.ComprasCboConcepto.Margin = new System.Windows.Forms.Padding(0, 4, 18, 0);
            this.ComprasCboConcepto.MostrarEtiquetaError = false;
            this.ComprasCboConcepto.Name = "ComprasCboConcepto";
            this.ComprasCboConcepto.Size = new System.Drawing.Size(346, 44);
            this.ComprasCboConcepto.TabIndex = 3;
            //
            // ComprasLblCampoCboMoneda
            // 
            this.ComprasLblCampoCboMoneda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoCboMoneda.Location = new System.Drawing.Point(868, 8);
            this.ComprasLblCampoCboMoneda.Name = "ComprasLblCampoCboMoneda";
            this.ComprasLblCampoCboMoneda.Size = new System.Drawing.Size(359, 24);
            this.ComprasLblCampoCboMoneda.TabIndex = 4;
            this.ComprasLblCampoCboMoneda.Text = "Moneda *";
            this.ComprasLblCampoCboMoneda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasCboMoneda
            // 
            this.ComprasCboMoneda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ComprasCboMoneda.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasCboMoneda.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasCboMoneda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasCboMoneda.Location = new System.Drawing.Point(865, 36);
            this.ComprasCboMoneda.Margin = new System.Windows.Forms.Padding(0, 4, 18, 0);
            this.ComprasCboMoneda.MaxLength = 0;
            this.ComprasCboMoneda.MostrarEtiquetaError = false;
            this.ComprasCboMoneda.Name = "ComprasCboMoneda";
            this.ComprasCboMoneda.Size = new System.Drawing.Size(347, 44);
            this.ComprasCboMoneda.TabIndex = 5;
            // 
            // ComprasLblCampoTxtSerie
            // 
            this.ComprasLblCampoTxtSerie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoTxtSerie.Location = new System.Drawing.Point(19, 86);
            this.ComprasLblCampoTxtSerie.Name = "ComprasLblCampoTxtSerie";
            this.ComprasLblCampoTxtSerie.Size = new System.Drawing.Size(479, 24);
            this.ComprasLblCampoTxtSerie.TabIndex = 6;
            this.ComprasLblCampoTxtSerie.Text = "Serie del documento *";
            this.ComprasLblCampoTxtSerie.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasLblCampoTxtNumero
            // 
            this.ComprasLblCampoTxtNumero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoTxtNumero.Location = new System.Drawing.Point(504, 86);
            this.ComprasLblCampoTxtNumero.Name = "ComprasLblCampoTxtNumero";
            this.ComprasLblCampoTxtNumero.Size = new System.Drawing.Size(358, 24);
            this.ComprasLblCampoTxtNumero.TabIndex = 8;
            this.ComprasLblCampoTxtNumero.Text = "Número del documento *";
            this.ComprasLblCampoTxtNumero.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasLblCampoNumMonto
            // 
            this.ComprasLblCampoNumMonto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoNumMonto.Location = new System.Drawing.Point(868, 86);
            this.ComprasLblCampoNumMonto.Name = "ComprasLblCampoNumMonto";
            this.ComprasLblCampoNumMonto.Size = new System.Drawing.Size(359, 24);
            this.ComprasLblCampoNumMonto.TabIndex = 10;
            this.ComprasLblCampoNumMonto.Text = "Importe del documento *";
            this.ComprasLblCampoNumMonto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasLblCampoFechaEmision
            // 
            this.ComprasLblCampoFechaEmision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoFechaEmision.Location = new System.Drawing.Point(19, 164);
            this.ComprasLblCampoFechaEmision.Name = "ComprasLblCampoFechaEmision";
            this.ComprasLblCampoFechaEmision.Size = new System.Drawing.Size(479, 24);
            this.ComprasLblCampoFechaEmision.TabIndex = 12;
            this.ComprasLblCampoFechaEmision.Text = "Fecha de emisión *";
            this.ComprasLblCampoFechaEmision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ComprasDtpFechaEmision.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasDtpFechaEmision.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasDtpFechaEmision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaEmision.Location = new System.Drawing.Point(16, 192);
            this.ComprasDtpFechaEmision.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasDtpFechaEmision.Name = "ComprasDtpFechaEmision";
            this.ComprasDtpFechaEmision.Size = new System.Drawing.Size(469, 32);
            this.ComprasDtpFechaEmision.TabIndex = 13;
            // 
            // ComprasLblCampoFechaVencimiento
            // 
            this.ComprasLblCampoFechaVencimiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoFechaVencimiento.Location = new System.Drawing.Point(504, 164);
            this.ComprasLblCampoFechaVencimiento.Name = "ComprasLblCampoFechaVencimiento";
            this.ComprasLblCampoFechaVencimiento.Size = new System.Drawing.Size(358, 24);
            this.ComprasLblCampoFechaVencimiento.TabIndex = 14;
            this.ComprasLblCampoFechaVencimiento.Text = "Fecha de vencimiento *";
            this.ComprasLblCampoFechaVencimiento.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.ComprasDtpFechaVencimiento.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasDtpFechaVencimiento.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasDtpFechaVencimiento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasDtpFechaVencimiento.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ComprasDtpFechaVencimiento.Location = new System.Drawing.Point(501, 192);
            this.ComprasDtpFechaVencimiento.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasDtpFechaVencimiento.Name = "ComprasDtpFechaVencimiento";
            this.ComprasDtpFechaVencimiento.Size = new System.Drawing.Size(348, 32);
            this.ComprasDtpFechaVencimiento.TabIndex = 15;
            // 
            // ComprasLblCampoNumCambio
            // 
            this.ComprasLblCampoNumCambio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblCampoNumCambio.Location = new System.Drawing.Point(868, 164);
            this.ComprasLblCampoNumCambio.Name = "ComprasLblCampoNumCambio";
            this.ComprasLblCampoNumCambio.Size = new System.Drawing.Size(359, 24);
            this.ComprasLblCampoNumCambio.TabIndex = 16;
            this.ComprasLblCampoNumCambio.Text = "Tipo de cambio a moneda base *";
            this.ComprasLblCampoNumCambio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasTxtTipoCambio
            // 
            this.ComprasTxtTipoCambio.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasTxtTipoCambio.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.ComprasTxtTipoCambio.Location = new System.Drawing.Point(865, 192);
            this.ComprasTxtTipoCambio.Margin = new System.Windows.Forms.Padding(0, 4, 16, 0);
            this.ComprasTxtTipoCambio.MaxLength = 30;
            this.ComprasTxtTipoCambio.Name = "ComprasTxtTipoCambio";
            this.ComprasTxtTipoCambio.ReadOnly = true;
            this.ComprasTxtTipoCambio.Size = new System.Drawing.Size(349, 32);
            this.ComprasTxtTipoCambio.TabIndex = 18;
            this.ComprasTxtTipoCambio.TabStop = false;
            this.ComprasTxtTipoCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ComprasLblAyuda
            // 
            this.ComprasLblAyuda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblAyuda.Location = new System.Drawing.Point(4, 376);
            this.ComprasLblAyuda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasLblAyuda.Name = "ComprasLblAyuda";
            this.ComprasLblAyuda.Size = new System.Drawing.Size(1244, 36);
            this.ComprasLblAyuda.TabIndex = 4;
            this.ComprasLblAyuda.Text = "* Campos obligatorios. El documento se registra como Pendiente. " +
                "Los abonos se registran por separado de las facturas.";
            this.ComprasLblAyuda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComprasPnlScroll
            // 
            this.ComprasPnlScroll.AutoScroll = true;
            this.ComprasPnlScroll.Controls.Add(this.ComprasTlpPagina);
            this.ComprasPnlScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasPnlScroll.Location = new System.Drawing.Point(0, 130);
            this.ComprasPnlScroll.Name = "ComprasPnlScroll";
            this.ComprasPnlScroll.Padding = new System.Windows.Forms.Padding(24, 12, 24, 16);
            this.ComprasPnlScroll.Size = new System.Drawing.Size(1300, 520);
            this.ComprasPnlScroll.TabIndex = 0;
            // 
            // ComprasUsrEncabezadoFormulario
            // 
            this.ComprasUsrEncabezadoFormulario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.ComprasUsrEncabezadoFormulario.Codigo = "6002";
            this.ComprasUsrEncabezadoFormulario.Descripcion = "Registro de documentos de proveedor en cuentas por pagar";
            this.ComprasUsrEncabezadoFormulario.Dock = System.Windows.Forms.DockStyle.Top;
            this.ComprasUsrEncabezadoFormulario.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrEncabezadoFormulario.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasUsrEncabezadoFormulario.MaximumSize = new System.Drawing.Size(1500, 130);
            this.ComprasUsrEncabezadoFormulario.MinimumSize = new System.Drawing.Size(0, 130);
            this.ComprasUsrEncabezadoFormulario.Name = "ComprasUsrEncabezadoFormulario";
            this.ComprasUsrEncabezadoFormulario.Size = new System.Drawing.Size(1300, 130);
            this.ComprasUsrEncabezadoFormulario.TabIndex = 1;
            this.ComprasUsrEncabezadoFormulario.TabStop = false;
            this.ComprasUsrEncabezadoFormulario.Titulo = "Movimientos de proveedores";
            // 
            // FrmMovimientosProveedores
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1300, 650);
            this.Controls.Add(this.ComprasPnlScroll);
            this.Controls.Add(this.ComprasUsrEncabezadoFormulario);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(42)))), ((int)(((byte)(90)))));
            this.MinimumSize = new System.Drawing.Size(1040, 620);
            this.Name = "FrmMovimientosProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "6002 – Movimientos de proveedores";
            this.ComprasTlpPagina.ResumeLayout(false);
            this.ComprasFlpAcciones.ResumeLayout(false);
            this.ComprasTlpCampos.ResumeLayout(false);
            this.ComprasTlpCampos.PerformLayout();
            this.ComprasPnlScroll.ResumeLayout(false);
            this.ComprasPnlScroll.PerformLayout();
            this.ResumeLayout(false);

        }

        private ClsButtonCompras ComprasBtnHistorial;
        private TextBox ComprasTxtMonto;
        private TextBox ComprasTxtTipoCambio;
    }
}



