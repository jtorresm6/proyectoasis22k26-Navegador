/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Define los controles y la distribución de la ventana de
 *  conciliación bancaria.
 * ================================================================
 */
namespace CapaVista_ConciliacionBancaria.Formularios
{
    partial class FrmConciliacion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConciliacion));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BancosPnlFondo = new System.Windows.Forms.Panel();
            this.BancosPnlTitulo = new System.Windows.Forms.Panel();
            this.BancosPbLogo = new System.Windows.Forms.PictureBox();
            this.BancosLblTitulo = new System.Windows.Forms.Label();
            this.BancosLblDescripcion = new System.Windows.Forms.Label();
            this.BancosPbButti = new System.Windows.Forms.PictureBox();
            this.BancosPnlSeleccion = new System.Windows.Forms.Panel();
            this.BancosLblCuenta = new System.Windows.Forms.Label();
            this.BancosCboCuenta = new System.Windows.Forms.ComboBox();
            this.BancosLblExtracto = new System.Windows.Forms.Label();
            this.BancosCboExtracto = new System.Windows.Forms.ComboBox();
            this.BancosLblFechaCorte = new System.Windows.Forms.Label();
            this.BancosDtpFechaCorte = new System.Windows.Forms.DateTimePicker();
            this.BancosLblEstado = new System.Windows.Forms.Label();
            this.BancosTxtEstado = new System.Windows.Forms.TextBox();
            this.BancosLblIndicacion = new System.Windows.Forms.Label();
            this.BancosBtnCorte = new System.Windows.Forms.Button();
            this.BancosBtnMarcar = new System.Windows.Forms.Button();
            this.BancosBtnReporte = new System.Windows.Forms.Button();
            this.BancosBtnAyuda = new System.Windows.Forms.Button();
            this.BancosBtnSalir = new System.Windows.Forms.Button();
            this.BancosLblContadores = new System.Windows.Forms.Label();
            this.BancosPnlInfo = new System.Windows.Forms.Panel();
            this.BancosLblTitSaldos = new System.Windows.Forms.Label();
            this.BancosLblSaldoInicialBanco = new System.Windows.Forms.Label();
            this.BancosTxtSaldoInicialBanco = new System.Windows.Forms.TextBox();
            this.BancosLblSaldoBanco = new System.Windows.Forms.Label();
            this.BancosTxtSaldoBanco = new System.Windows.Forms.TextBox();
            this.BancosLblSaldoLibro = new System.Windows.Forms.Label();
            this.BancosTxtSaldoLibro = new System.Windows.Forms.TextBox();
            this.BancosLblDiferencia = new System.Windows.Forms.Label();
            this.BancosTxtDiferencia = new System.Windows.Forms.TextBox();
            this.BancosLblTitConciliado = new System.Windows.Forms.Label();
            this.BancosLblCargoCon = new System.Windows.Forms.Label();
            this.BancosTxtCargoCon = new System.Windows.Forms.TextBox();
            this.BancosLblAbonoCon = new System.Windows.Forms.Label();
            this.BancosTxtAbonoCon = new System.Windows.Forms.TextBox();
            this.BancosLblSaldoCon = new System.Windows.Forms.Label();
            this.BancosTxtSaldoCon = new System.Windows.Forms.TextBox();
            this.BancosLblTitPorConciliar = new System.Windows.Forms.Label();
            this.BancosLblCargoPorCon = new System.Windows.Forms.Label();
            this.BancosTxtCargoPorCon = new System.Windows.Forms.TextBox();
            this.BancosLblAbonoPorCon = new System.Windows.Forms.Label();
            this.BancosTxtAbonoPorCon = new System.Windows.Forms.TextBox();
            this.BancosLblSaldoPorCon = new System.Windows.Forms.Label();
            this.BancosTxtSaldoPorCon = new System.Windows.Forms.TextBox();
            this.BancosDgvMovimientos = new System.Windows.Forms.DataGridView();
            this.BancosPnlFondo.SuspendLayout();
            this.BancosPnlTitulo.SuspendLayout();
            this.BancosPnlSeleccion.SuspendLayout();
            this.BancosPnlInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbButti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosPnlFondo
            // 
            this.BancosPnlFondo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.BancosPnlFondo.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.conci_contenedor;
            this.BancosPnlFondo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlFondo.Location = new System.Drawing.Point(0, 0);
            this.BancosPnlFondo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPnlFondo.Name = "BancosPnlFondo";
            this.BancosPnlFondo.Size = new System.Drawing.Size(1100, 700);
            this.BancosPnlFondo.TabIndex = 0;
            this.BancosPnlFondo.Controls.Add(this.BancosPnlTitulo);
            this.BancosPnlFondo.Controls.Add(this.BancosPnlSeleccion);
            this.BancosPnlFondo.Controls.Add(this.BancosLblIndicacion);
            this.BancosPnlFondo.Controls.Add(this.BancosBtnCorte);
            this.BancosPnlFondo.Controls.Add(this.BancosBtnMarcar);
            this.BancosPnlFondo.Controls.Add(this.BancosBtnReporte);
            this.BancosPnlFondo.Controls.Add(this.BancosBtnAyuda);
            this.BancosPnlFondo.Controls.Add(this.BancosBtnSalir);
            this.BancosPnlFondo.Controls.Add(this.BancosLblContadores);
            this.BancosPnlFondo.Controls.Add(this.BancosPnlInfo);
            this.BancosPnlFondo.Controls.Add(this.BancosDgvMovimientos);
            // 
            // BancosPnlTitulo
            // 
            this.BancosPnlTitulo.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlTitulo.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.conci_titulo;
            this.BancosPnlTitulo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlTitulo.Location = new System.Drawing.Point(35, 20);
            this.BancosPnlTitulo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPnlTitulo.Name = "BancosPnlTitulo";
            this.BancosPnlTitulo.Size = new System.Drawing.Size(1030, 118);
            this.BancosPnlTitulo.TabIndex = 0;
            this.BancosPnlTitulo.Controls.Add(this.BancosPbLogo);
            this.BancosPnlTitulo.Controls.Add(this.BancosLblTitulo);
            this.BancosPnlTitulo.Controls.Add(this.BancosLblDescripcion);
            this.BancosPnlTitulo.Controls.Add(this.BancosPbButti);
            // 
            // BancosPbLogo
            // 
            this.BancosPbLogo.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.conci_logo;
            this.BancosPbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbLogo.Location = new System.Drawing.Point(39, 15);
            this.BancosPbLogo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPbLogo.Name = "BancosPbLogo";
            this.BancosPbLogo.Size = new System.Drawing.Size(86, 88);
            this.BancosPbLogo.TabIndex = 0;
            this.BancosPbLogo.TabStop = false;
            // 
            // BancosLblTitulo
            // 
            this.BancosLblTitulo.AutoSize = true;
            this.BancosLblTitulo.Font = new System.Drawing.Font("Lucida Bright", 29F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.BancosLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosLblTitulo.Location = new System.Drawing.Point(146, 22);
            this.BancosLblTitulo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BancosLblTitulo.Name = "BancosLblTitulo";
            this.BancosLblTitulo.Size = new System.Drawing.Size(449, 44);
            this.BancosLblTitulo.TabIndex = 1;
            this.BancosLblTitulo.Text = "Conciliación Bancaria";
            // 
            // BancosLblDescripcion
            // 
            this.BancosLblDescripcion.AutoSize = true;
            this.BancosLblDescripcion.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblDescripcion.ForeColor = System.Drawing.Color.DimGray;
            this.BancosLblDescripcion.Location = new System.Drawing.Point(153, 68);
            this.BancosLblDescripcion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BancosLblDescripcion.Name = "BancosLblDescripcion";
            this.BancosLblDescripcion.TabIndex = 2;
            this.BancosLblDescripcion.Text = "Compare los movimientos registrados contra el estado de cuenta del banco.";
            // 
            // BancosPbButti
            // 
            this.BancosPbButti.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.conci_butti1;
            this.BancosPbButti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbButti.Location = new System.Drawing.Point(900, 6);
            this.BancosPbButti.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPbButti.Name = "BancosPbButti";
            this.BancosPbButti.Size = new System.Drawing.Size(91, 106);
            this.BancosPbButti.TabIndex = 3;
            this.BancosPbButti.TabStop = false;
            // 
            // BancosPnlSeleccion
            // 
            this.BancosPnlSeleccion.BackColor = System.Drawing.Color.White;
            this.BancosPnlSeleccion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BancosPnlSeleccion.Location = new System.Drawing.Point(35, 148);
            this.BancosPnlSeleccion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPnlSeleccion.Name = "BancosPnlSeleccion";
            this.BancosPnlSeleccion.Size = new System.Drawing.Size(1030, 62);
            this.BancosPnlSeleccion.TabIndex = 1;
            this.BancosPnlSeleccion.Controls.Add(this.BancosLblCuenta);
            this.BancosPnlSeleccion.Controls.Add(this.BancosCboCuenta);
            this.BancosPnlSeleccion.Controls.Add(this.BancosLblExtracto);
            this.BancosPnlSeleccion.Controls.Add(this.BancosCboExtracto);
            this.BancosPnlSeleccion.Controls.Add(this.BancosLblFechaCorte);
            this.BancosPnlSeleccion.Controls.Add(this.BancosDtpFechaCorte);
            this.BancosPnlSeleccion.Controls.Add(this.BancosLblEstado);
            this.BancosPnlSeleccion.Controls.Add(this.BancosTxtEstado);
            // 
            // BancosLblCuenta
            // 
            this.BancosLblCuenta.AutoSize = true;
            this.BancosLblCuenta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblCuenta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblCuenta.Location = new System.Drawing.Point(12, 8);
            this.BancosLblCuenta.Name = "BancosLblCuenta";
            this.BancosLblCuenta.Text = "Cuenta bancaria";
            // 
            // BancosCboCuenta
            // 
            this.BancosCboCuenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BancosCboCuenta.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosCboCuenta.FormattingEnabled = true;
            this.BancosCboCuenta.Location = new System.Drawing.Point(12, 28);
            this.BancosCboCuenta.Name = "BancosCboCuenta";
            this.BancosCboCuenta.Size = new System.Drawing.Size(330, 22);
            this.BancosCboCuenta.TabIndex = 0;
            // 
            // BancosLblExtracto
            // 
            this.BancosLblExtracto.AutoSize = true;
            this.BancosLblExtracto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblExtracto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblExtracto.Location = new System.Drawing.Point(356, 8);
            this.BancosLblExtracto.Name = "BancosLblExtracto";
            this.BancosLblExtracto.Text = "Estado de cuenta (extracto)";
            // 
            // BancosCboExtracto
            // 
            this.BancosCboExtracto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BancosCboExtracto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosCboExtracto.FormattingEnabled = true;
            this.BancosCboExtracto.Location = new System.Drawing.Point(356, 28);
            this.BancosCboExtracto.Name = "BancosCboExtracto";
            this.BancosCboExtracto.Size = new System.Drawing.Size(260, 22);
            this.BancosCboExtracto.TabIndex = 1;
            // 
            // BancosLblFechaCorte
            // 
            this.BancosLblFechaCorte.AutoSize = true;
            this.BancosLblFechaCorte.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblFechaCorte.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblFechaCorte.Location = new System.Drawing.Point(630, 8);
            this.BancosLblFechaCorte.Name = "BancosLblFechaCorte";
            this.BancosLblFechaCorte.Text = "Fecha de corte";
            // 
            // BancosDtpFechaCorte
            // 
            this.BancosDtpFechaCorte.CustomFormat = "dd/MM/yyyy";
            this.BancosDtpFechaCorte.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosDtpFechaCorte.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BancosDtpFechaCorte.Location = new System.Drawing.Point(630, 28);
            this.BancosDtpFechaCorte.Name = "BancosDtpFechaCorte";
            this.BancosDtpFechaCorte.Size = new System.Drawing.Size(120, 22);
            this.BancosDtpFechaCorte.TabIndex = 2;
            // 
            // BancosLblEstado
            // 
            this.BancosLblEstado.AutoSize = true;
            this.BancosLblEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblEstado.Location = new System.Drawing.Point(764, 8);
            this.BancosLblEstado.Name = "BancosLblEstado";
            this.BancosLblEstado.Text = "Estado de la conciliación";
            // 
            // BancosTxtEstado
            // 
            this.BancosTxtEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosTxtEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosTxtEstado.Location = new System.Drawing.Point(764, 28);
            this.BancosTxtEstado.Name = "BancosTxtEstado";
            this.BancosTxtEstado.ReadOnly = true;
            this.BancosTxtEstado.Size = new System.Drawing.Size(250, 22);
            this.BancosTxtEstado.TabIndex = 3;
            this.BancosTxtEstado.TabStop = false;
            this.BancosTxtEstado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BancosLblIndicacion
            // 
            this.BancosLblIndicacion.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblIndicacion.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosLblIndicacion.ForeColor = System.Drawing.Color.DimGray;
            this.BancosLblIndicacion.Location = new System.Drawing.Point(35, 222);
            this.BancosLblIndicacion.Name = "BancosLblIndicacion";
            this.BancosLblIndicacion.Size = new System.Drawing.Size(290, 62);
            this.BancosLblIndicacion.TabIndex = 2;
            this.BancosLblIndicacion.Text = "Para marcar a mano, seleccione con Ctrl un movimiento del libro y una línea del banco que estén pendientes.";
            // 
            // BancosBtnCorte
            // 
            this.BancosBtnCorte.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.btn_corte_conciliacion;
            this.BancosBtnCorte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnCorte.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BancosBtnCorte.Location = new System.Drawing.Point(342, 216);
            this.BancosBtnCorte.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosBtnCorte.Name = "BancosBtnCorte";
            this.BancosBtnCorte.Size = new System.Drawing.Size(75, 81);
            this.BancosBtnCorte.TabIndex = 3;
            this.BancosBtnCorte.UseVisualStyleBackColor = true;
            // 
            // BancosBtnMarcar
            // 
            this.BancosBtnMarcar.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.btn_marcar_conciliacion;
            this.BancosBtnMarcar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnMarcar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BancosBtnMarcar.Location = new System.Drawing.Point(427, 216);
            this.BancosBtnMarcar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosBtnMarcar.Name = "BancosBtnMarcar";
            this.BancosBtnMarcar.Size = new System.Drawing.Size(75, 81);
            this.BancosBtnMarcar.TabIndex = 4;
            this.BancosBtnMarcar.UseVisualStyleBackColor = true;
            // 
            // BancosBtnReporte
            // 
            this.BancosBtnReporte.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.btn_reporteB;
            this.BancosBtnReporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnReporte.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BancosBtnReporte.Location = new System.Drawing.Point(512, 216);
            this.BancosBtnReporte.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosBtnReporte.Name = "BancosBtnReporte";
            this.BancosBtnReporte.Size = new System.Drawing.Size(75, 81);
            this.BancosBtnReporte.TabIndex = 5;
            this.BancosBtnReporte.UseVisualStyleBackColor = true;
            // 
            // BancosBtnAyuda
            // 
            this.BancosBtnAyuda.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.btn_ayudaB;
            this.BancosBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnAyuda.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BancosBtnAyuda.Location = new System.Drawing.Point(597, 216);
            this.BancosBtnAyuda.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosBtnAyuda.Name = "BancosBtnAyuda";
            this.BancosBtnAyuda.Size = new System.Drawing.Size(75, 81);
            this.BancosBtnAyuda.TabIndex = 6;
            this.BancosBtnAyuda.UseVisualStyleBackColor = true;
            // 
            // BancosBtnSalir
            // 
            this.BancosBtnSalir.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.btn_salirB;
            this.BancosBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BancosBtnSalir.Location = new System.Drawing.Point(682, 216);
            this.BancosBtnSalir.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosBtnSalir.Name = "BancosBtnSalir";
            this.BancosBtnSalir.Size = new System.Drawing.Size(75, 81);
            this.BancosBtnSalir.TabIndex = 7;
            this.BancosBtnSalir.UseVisualStyleBackColor = true;
            // 
            // BancosLblContadores
            // 
            this.BancosLblContadores.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblContadores.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblContadores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblContadores.Location = new System.Drawing.Point(775, 222);
            this.BancosLblContadores.Name = "BancosLblContadores";
            this.BancosLblContadores.Size = new System.Drawing.Size(290, 62);
            this.BancosLblContadores.TabIndex = 8;
            this.BancosLblContadores.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BancosPnlInfo
            // 
            this.BancosPnlInfo.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlInfo.BackgroundImage = global::CapaVista_ConciliacionBancaria.Properties.Resources.conci_titulo;
            this.BancosPnlInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlInfo.Location = new System.Drawing.Point(35, 306);
            this.BancosPnlInfo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPnlInfo.Name = "BancosPnlInfo";
            this.BancosPnlInfo.Size = new System.Drawing.Size(1030, 128);
            this.BancosPnlInfo.TabIndex = 9;
            this.BancosPnlInfo.Controls.Add(this.BancosLblTitSaldos);
            this.BancosPnlInfo.Controls.Add(this.BancosLblSaldoInicialBanco);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtSaldoInicialBanco);
            this.BancosPnlInfo.Controls.Add(this.BancosLblSaldoBanco);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtSaldoBanco);
            this.BancosPnlInfo.Controls.Add(this.BancosLblSaldoLibro);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtSaldoLibro);
            this.BancosPnlInfo.Controls.Add(this.BancosLblDiferencia);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtDiferencia);
            this.BancosPnlInfo.Controls.Add(this.BancosLblTitConciliado);
            this.BancosPnlInfo.Controls.Add(this.BancosLblCargoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtCargoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosLblAbonoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtAbonoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosLblSaldoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtSaldoCon);
            this.BancosPnlInfo.Controls.Add(this.BancosLblTitPorConciliar);
            this.BancosPnlInfo.Controls.Add(this.BancosLblCargoPorCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtCargoPorCon);
            this.BancosPnlInfo.Controls.Add(this.BancosLblAbonoPorCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtAbonoPorCon);
            this.BancosPnlInfo.Controls.Add(this.BancosLblSaldoPorCon);
            this.BancosPnlInfo.Controls.Add(this.BancosTxtSaldoPorCon);
            // 
            // BancosLblTitSaldos
            // 
            this.BancosLblTitSaldos.AutoSize = true;
            this.BancosLblTitSaldos.Font = new System.Drawing.Font("Lucida Fax", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblTitSaldos.Location = new System.Drawing.Point(30, 8);
            this.BancosLblTitSaldos.Name = "BancosLblTitSaldos";
            this.BancosLblTitSaldos.Text = "Saldos al corte";
            // 
            // BancosLblSaldoInicialBanco
            // 
            this.BancosLblSaldoInicialBanco.AutoSize = true;
            this.BancosLblSaldoInicialBanco.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblSaldoInicialBanco.Location = new System.Drawing.Point(30, 37);
            this.BancosLblSaldoInicialBanco.Name = "BancosLblSaldoInicialBanco";
            this.BancosLblSaldoInicialBanco.Text = "Saldo inicial del banco";
            // 
            // BancosTxtSaldoInicialBanco
            // 
            this.BancosTxtSaldoInicialBanco.BackColor = System.Drawing.Color.White;
            this.BancosTxtSaldoInicialBanco.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtSaldoInicialBanco.Location = new System.Drawing.Point(215, 34);
            this.BancosTxtSaldoInicialBanco.Name = "BancosTxtSaldoInicialBanco";
            this.BancosTxtSaldoInicialBanco.ReadOnly = true;
            this.BancosTxtSaldoInicialBanco.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtSaldoInicialBanco.TabStop = false;
            this.BancosTxtSaldoInicialBanco.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblSaldoBanco
            // 
            this.BancosLblSaldoBanco.AutoSize = true;
            this.BancosLblSaldoBanco.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblSaldoBanco.Location = new System.Drawing.Point(30, 60);
            this.BancosLblSaldoBanco.Name = "BancosLblSaldoBanco";
            this.BancosLblSaldoBanco.Text = "Saldo del banco";
            // 
            // BancosTxtSaldoBanco
            // 
            this.BancosTxtSaldoBanco.BackColor = System.Drawing.Color.White;
            this.BancosTxtSaldoBanco.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtSaldoBanco.Location = new System.Drawing.Point(215, 57);
            this.BancosTxtSaldoBanco.Name = "BancosTxtSaldoBanco";
            this.BancosTxtSaldoBanco.ReadOnly = true;
            this.BancosTxtSaldoBanco.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtSaldoBanco.TabStop = false;
            this.BancosTxtSaldoBanco.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblSaldoLibro
            // 
            this.BancosLblSaldoLibro.AutoSize = true;
            this.BancosLblSaldoLibro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblSaldoLibro.Location = new System.Drawing.Point(30, 83);
            this.BancosLblSaldoLibro.Name = "BancosLblSaldoLibro";
            this.BancosLblSaldoLibro.Text = "Saldo del libro";
            // 
            // BancosTxtSaldoLibro
            // 
            this.BancosTxtSaldoLibro.BackColor = System.Drawing.Color.White;
            this.BancosTxtSaldoLibro.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtSaldoLibro.Location = new System.Drawing.Point(215, 80);
            this.BancosTxtSaldoLibro.Name = "BancosTxtSaldoLibro";
            this.BancosTxtSaldoLibro.ReadOnly = true;
            this.BancosTxtSaldoLibro.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtSaldoLibro.TabStop = false;
            this.BancosTxtSaldoLibro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblDiferencia
            // 
            this.BancosLblDiferencia.AutoSize = true;
            this.BancosLblDiferencia.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblDiferencia.Location = new System.Drawing.Point(30, 106);
            this.BancosLblDiferencia.Name = "BancosLblDiferencia";
            this.BancosLblDiferencia.Text = "Diferencia";
            // 
            // BancosTxtDiferencia
            // 
            this.BancosTxtDiferencia.BackColor = System.Drawing.Color.White;
            this.BancosTxtDiferencia.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosTxtDiferencia.Location = new System.Drawing.Point(215, 103);
            this.BancosTxtDiferencia.Name = "BancosTxtDiferencia";
            this.BancosTxtDiferencia.ReadOnly = true;
            this.BancosTxtDiferencia.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtDiferencia.TabStop = false;
            this.BancosTxtDiferencia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblTitConciliado
            // 
            this.BancosLblTitConciliado.AutoSize = true;
            this.BancosLblTitConciliado.Font = new System.Drawing.Font("Lucida Fax", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblTitConciliado.Location = new System.Drawing.Point(440, 8);
            this.BancosLblTitConciliado.Name = "BancosLblTitConciliado";
            this.BancosLblTitConciliado.Text = "Conciliado";
            // 
            // BancosLblCargoCon
            // 
            this.BancosLblCargoCon.AutoSize = true;
            this.BancosLblCargoCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblCargoCon.Location = new System.Drawing.Point(440, 37);
            this.BancosLblCargoCon.Name = "BancosLblCargoCon";
            this.BancosLblCargoCon.Text = "Cargos";
            // 
            // BancosTxtCargoCon
            // 
            this.BancosTxtCargoCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtCargoCon.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtCargoCon.Location = new System.Drawing.Point(520, 34);
            this.BancosTxtCargoCon.Name = "BancosTxtCargoCon";
            this.BancosTxtCargoCon.ReadOnly = true;
            this.BancosTxtCargoCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtCargoCon.TabStop = false;
            this.BancosTxtCargoCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblAbonoCon
            // 
            this.BancosLblAbonoCon.AutoSize = true;
            this.BancosLblAbonoCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblAbonoCon.Location = new System.Drawing.Point(440, 60);
            this.BancosLblAbonoCon.Name = "BancosLblAbonoCon";
            this.BancosLblAbonoCon.Text = "Abonos";
            // 
            // BancosTxtAbonoCon
            // 
            this.BancosTxtAbonoCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtAbonoCon.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtAbonoCon.Location = new System.Drawing.Point(520, 57);
            this.BancosTxtAbonoCon.Name = "BancosTxtAbonoCon";
            this.BancosTxtAbonoCon.ReadOnly = true;
            this.BancosTxtAbonoCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtAbonoCon.TabStop = false;
            this.BancosTxtAbonoCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblSaldoCon
            // 
            this.BancosLblSaldoCon.AutoSize = true;
            this.BancosLblSaldoCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblSaldoCon.Location = new System.Drawing.Point(440, 83);
            this.BancosLblSaldoCon.Name = "BancosLblSaldoCon";
            this.BancosLblSaldoCon.Text = "Saldo";
            // 
            // BancosTxtSaldoCon
            // 
            this.BancosTxtSaldoCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtSaldoCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosTxtSaldoCon.Location = new System.Drawing.Point(520, 80);
            this.BancosTxtSaldoCon.Name = "BancosTxtSaldoCon";
            this.BancosTxtSaldoCon.ReadOnly = true;
            this.BancosTxtSaldoCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtSaldoCon.TabStop = false;
            this.BancosTxtSaldoCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblTitPorConciliar
            // 
            this.BancosLblTitPorConciliar.AutoSize = true;
            this.BancosLblTitPorConciliar.Font = new System.Drawing.Font("Lucida Fax", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblTitPorConciliar.Location = new System.Drawing.Point(740, 8);
            this.BancosLblTitPorConciliar.Name = "BancosLblTitPorConciliar";
            this.BancosLblTitPorConciliar.Text = "Por conciliar";
            // 
            // BancosLblCargoPorCon
            // 
            this.BancosLblCargoPorCon.AutoSize = true;
            this.BancosLblCargoPorCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblCargoPorCon.Location = new System.Drawing.Point(740, 37);
            this.BancosLblCargoPorCon.Name = "BancosLblCargoPorCon";
            this.BancosLblCargoPorCon.Text = "Cargos";
            // 
            // BancosTxtCargoPorCon
            // 
            this.BancosTxtCargoPorCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtCargoPorCon.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtCargoPorCon.Location = new System.Drawing.Point(820, 34);
            this.BancosTxtCargoPorCon.Name = "BancosTxtCargoPorCon";
            this.BancosTxtCargoPorCon.ReadOnly = true;
            this.BancosTxtCargoPorCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtCargoPorCon.TabStop = false;
            this.BancosTxtCargoPorCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblAbonoPorCon
            // 
            this.BancosLblAbonoPorCon.AutoSize = true;
            this.BancosLblAbonoPorCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblAbonoPorCon.Location = new System.Drawing.Point(740, 60);
            this.BancosLblAbonoPorCon.Name = "BancosLblAbonoPorCon";
            this.BancosLblAbonoPorCon.Text = "Abonos";
            // 
            // BancosTxtAbonoPorCon
            // 
            this.BancosTxtAbonoPorCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtAbonoPorCon.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosTxtAbonoPorCon.Location = new System.Drawing.Point(820, 57);
            this.BancosTxtAbonoPorCon.Name = "BancosTxtAbonoPorCon";
            this.BancosTxtAbonoPorCon.ReadOnly = true;
            this.BancosTxtAbonoPorCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtAbonoPorCon.TabStop = false;
            this.BancosTxtAbonoPorCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosLblSaldoPorCon
            // 
            this.BancosLblSaldoPorCon.AutoSize = true;
            this.BancosLblSaldoPorCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosLblSaldoPorCon.Location = new System.Drawing.Point(740, 83);
            this.BancosLblSaldoPorCon.Name = "BancosLblSaldoPorCon";
            this.BancosLblSaldoPorCon.Text = "Saldo";
            // 
            // BancosTxtSaldoPorCon
            // 
            this.BancosTxtSaldoPorCon.BackColor = System.Drawing.Color.White;
            this.BancosTxtSaldoPorCon.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.BancosTxtSaldoPorCon.Location = new System.Drawing.Point(820, 80);
            this.BancosTxtSaldoPorCon.Name = "BancosTxtSaldoPorCon";
            this.BancosTxtSaldoPorCon.ReadOnly = true;
            this.BancosTxtSaldoPorCon.Size = new System.Drawing.Size(135, 21);
            this.BancosTxtSaldoPorCon.TabStop = false;
            this.BancosTxtSaldoPorCon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BancosDgvMovimientos
            // 
            this.BancosDgvMovimientos.AllowUserToAddRows = false;
            this.BancosDgvMovimientos.AllowUserToDeleteRows = false;
            this.BancosDgvMovimientos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosDgvMovimientos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.BancosDgvMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BancosDgvMovimientos.BackgroundColor = System.Drawing.Color.White;
            this.BancosDgvMovimientos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BancosDgvMovimientos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.BancosDgvMovimientos.ColumnHeadersHeight = 32;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(232)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BancosDgvMovimientos.DefaultCellStyle = dataGridViewCellStyle3;
            this.BancosDgvMovimientos.EnableHeadersVisualStyles = false;
            this.BancosDgvMovimientos.Location = new System.Drawing.Point(35, 444);
            this.BancosDgvMovimientos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosDgvMovimientos.Name = "BancosDgvMovimientos";
            this.BancosDgvMovimientos.ReadOnly = true;
            this.BancosDgvMovimientos.RowHeadersVisible = false;
            this.BancosDgvMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BancosDgvMovimientos.Size = new System.Drawing.Size(1030, 236);
            this.BancosDgvMovimientos.TabIndex = 10;
            // 
            // FrmConciliacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.BancosPnlFondo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmConciliacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8007-ConciliaciónBancaria";
            this.Load += new System.EventHandler(this.FrmConciliacion_Load);
            this.BancosPnlInfo.ResumeLayout(false);
            this.BancosPnlInfo.PerformLayout();
            this.BancosPnlSeleccion.ResumeLayout(false);
            this.BancosPnlSeleccion.PerformLayout();
            this.BancosPnlTitulo.ResumeLayout(false);
            this.BancosPnlTitulo.PerformLayout();
            this.BancosPnlFondo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbButti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BancosPnlFondo;
        private System.Windows.Forms.Panel BancosPnlTitulo;
        private System.Windows.Forms.PictureBox BancosPbLogo;
        private System.Windows.Forms.Label BancosLblTitulo;
        private System.Windows.Forms.Label BancosLblDescripcion;
        private System.Windows.Forms.PictureBox BancosPbButti;
        private System.Windows.Forms.Panel BancosPnlSeleccion;
        private System.Windows.Forms.Label BancosLblCuenta;
        private System.Windows.Forms.ComboBox BancosCboCuenta;
        private System.Windows.Forms.Label BancosLblExtracto;
        private System.Windows.Forms.ComboBox BancosCboExtracto;
        private System.Windows.Forms.Label BancosLblFechaCorte;
        private System.Windows.Forms.DateTimePicker BancosDtpFechaCorte;
        private System.Windows.Forms.Label BancosLblEstado;
        private System.Windows.Forms.TextBox BancosTxtEstado;
        private System.Windows.Forms.Label BancosLblIndicacion;
        private System.Windows.Forms.Button BancosBtnCorte;
        private System.Windows.Forms.Button BancosBtnMarcar;
        private System.Windows.Forms.Button BancosBtnReporte;
        private System.Windows.Forms.Button BancosBtnAyuda;
        private System.Windows.Forms.Button BancosBtnSalir;
        private System.Windows.Forms.Label BancosLblContadores;
        private System.Windows.Forms.Panel BancosPnlInfo;
        private System.Windows.Forms.Label BancosLblTitSaldos;
        private System.Windows.Forms.Label BancosLblSaldoInicialBanco;
        private System.Windows.Forms.TextBox BancosTxtSaldoInicialBanco;
        private System.Windows.Forms.Label BancosLblSaldoBanco;
        private System.Windows.Forms.TextBox BancosTxtSaldoBanco;
        private System.Windows.Forms.Label BancosLblSaldoLibro;
        private System.Windows.Forms.TextBox BancosTxtSaldoLibro;
        private System.Windows.Forms.Label BancosLblDiferencia;
        private System.Windows.Forms.TextBox BancosTxtDiferencia;
        private System.Windows.Forms.Label BancosLblTitConciliado;
        private System.Windows.Forms.Label BancosLblCargoCon;
        private System.Windows.Forms.TextBox BancosTxtCargoCon;
        private System.Windows.Forms.Label BancosLblAbonoCon;
        private System.Windows.Forms.TextBox BancosTxtAbonoCon;
        private System.Windows.Forms.Label BancosLblSaldoCon;
        private System.Windows.Forms.TextBox BancosTxtSaldoCon;
        private System.Windows.Forms.Label BancosLblTitPorConciliar;
        private System.Windows.Forms.Label BancosLblCargoPorCon;
        private System.Windows.Forms.TextBox BancosTxtCargoPorCon;
        private System.Windows.Forms.Label BancosLblAbonoPorCon;
        private System.Windows.Forms.TextBox BancosTxtAbonoPorCon;
        private System.Windows.Forms.Label BancosLblSaldoPorCon;
        private System.Windows.Forms.TextBox BancosTxtSaldoPorCon;
        private System.Windows.Forms.DataGridView BancosDgvMovimientos;
    }
}
