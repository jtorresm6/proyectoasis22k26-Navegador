/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Define los controles visuales del formulario de cierre bancario,
 *  incluida la cuadrícula y navegación del historial.
 * ================================================================
 */
namespace CapaVista_CierreBancario.Formularios
{
    partial class FrmCierreBancario
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel BancosPnlEncabezado;
        private System.Windows.Forms.PictureBox BancosPbIconoBanco;
        private System.Windows.Forms.PictureBox BancosPbMascota;
        private System.Windows.Forms.Label BancosLblTitulo;
        private System.Windows.Forms.TabControl BancosTabCierre;
        private System.Windows.Forms.TabPage BancosTabPeriodo;
        private System.Windows.Forms.TabPage BancosTabHistorico;
        private System.Windows.Forms.Panel BancosPnlPeriodoCierre;
        private System.Windows.Forms.Label BancosLblPeriodoCierre;
        private System.Windows.Forms.Label BancosLblTipoCierre;
        private System.Windows.Forms.Label BancosLblFechaInicial;
        private System.Windows.Forms.Label BancosLblFechaFinal;
        private System.Windows.Forms.Label BancosLblEstadoPeriodo;
        private System.Windows.Forms.Label BancosLblAlcance;
        private System.Windows.Forms.ComboBox BancosCboTipoCierre;
        private System.Windows.Forms.TextBox BancosTxtFechaInicial;
        private System.Windows.Forms.TextBox BancosTxFechaFinal;
        private System.Windows.Forms.TextBox BancosTxtEstadoPeriodo;
        private System.Windows.Forms.TextBox BancosTxtAlcance;
        private System.Windows.Forms.Button BancosBtnConsultar;
        private System.Windows.Forms.Panel BancosPnlResumenCuentasCierre;
        private System.Windows.Forms.Label BancosLblResumenCuentas;
        private System.Windows.Forms.DataGridView BancosDgvResumenCuentas;
        private System.Windows.Forms.Panel BancosPnlValidacionesCierre;
        private System.Windows.Forms.Label BancosLblValidaciones;
        private System.Windows.Forms.CheckBox BancosChkPeriodoValido;
        private System.Windows.Forms.CheckBox BancosChkConciliacionesVerificadas;
        private System.Windows.Forms.CheckBox BancosChkSaldosVerificados;
        private System.Windows.Forms.Label BancosTxtResultadoValidacionesCierre;
        private System.Windows.Forms.Label BancosTxtAdvertencia;
        private System.Windows.Forms.Panel BancosPnlBotones;
        private System.Windows.Forms.Button BancosBtnValidarCierre;
        private System.Windows.Forms.Button BancosBtnCerrarPeriodo;
        private System.Windows.Forms.Button BancosBtnVerHistorico;
        private System.Windows.Forms.Button BancosBtnExportar;
        private System.Windows.Forms.Button BancosBtnLimpiar;
        private System.Windows.Forms.Button BancosBtnAyuda;
        private System.Windows.Forms.Button BancosBtnSalir;
        private System.Windows.Forms.Panel BancosPnlHistorico;
        private System.Windows.Forms.Label BancosLblHistorico;
        private System.Windows.Forms.DataGridView BancosDgvHistorico;
        private System.Windows.Forms.Button BancosBtnActualizarHistorico;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCierreBancario));
            this.BancosPnlEncabezado = new System.Windows.Forms.Panel();
            this.BancosLblTitulo = new System.Windows.Forms.Label();
            this.BancosTabCierre = new System.Windows.Forms.TabControl();
            this.BancosTabPeriodo = new System.Windows.Forms.TabPage();
            this.BancosPnlResumenCuentasCierre = new System.Windows.Forms.Panel();
            this.BancosDgvResumenCuentas = new System.Windows.Forms.DataGridView();
            this.BancosLblResumenCuentas = new System.Windows.Forms.Label();
            this.BancosPnlValidacionesCierre = new System.Windows.Forms.Panel();
            this.BancosLblValidaciones = new System.Windows.Forms.Label();
            this.BancosChkPeriodoValido = new System.Windows.Forms.CheckBox();
            this.BancosChkConciliacionesVerificadas = new System.Windows.Forms.CheckBox();
            this.BancosChkSaldosVerificados = new System.Windows.Forms.CheckBox();
            this.BancosTxtResultadoValidacionesCierre = new System.Windows.Forms.Label();
            this.BancosTxtAdvertencia = new System.Windows.Forms.Label();
            this.BancosPnlBotones = new System.Windows.Forms.Panel();
            this.BancosPnlPeriodoCierre = new System.Windows.Forms.Panel();
            this.BancosLblPeriodoCierre = new System.Windows.Forms.Label();
            this.BancosLblTipoCierre = new System.Windows.Forms.Label();
            this.BancosLblFechaInicial = new System.Windows.Forms.Label();
            this.BancosLblFechaFinal = new System.Windows.Forms.Label();
            this.BancosLblEstadoPeriodo = new System.Windows.Forms.Label();
            this.BancosLblAlcance = new System.Windows.Forms.Label();
            this.BancosCboTipoCierre = new System.Windows.Forms.ComboBox();
            this.BancosTxtFechaInicial = new System.Windows.Forms.TextBox();
            this.BancosTxFechaFinal = new System.Windows.Forms.TextBox();
            this.BancosTxtEstadoPeriodo = new System.Windows.Forms.TextBox();
            this.BancosTxtAlcance = new System.Windows.Forms.TextBox();
            this.BancosTabHistorico = new System.Windows.Forms.TabPage();
            this.BancosPnlHistorico = new System.Windows.Forms.Panel();
            this.BancosDgvHistorico = new System.Windows.Forms.DataGridView();
            this.BancosLblHistorico = new System.Windows.Forms.Label();
            this.BancosBtnReporte = new System.Windows.Forms.Button();
            this.BancosBtnValidarCierre = new System.Windows.Forms.Button();
            this.BancosBtnCerrarPeriodo = new System.Windows.Forms.Button();
            this.BancosBtnVerHistorico = new System.Windows.Forms.Button();
            this.BancosBtnExportar = new System.Windows.Forms.Button();
            this.BancosBtnLimpiar = new System.Windows.Forms.Button();
            this.BancosBtnAyuda = new System.Windows.Forms.Button();
            this.BancosBtnSalir = new System.Windows.Forms.Button();
            this.BancosBtnConsultar = new System.Windows.Forms.Button();
            this.BancosBtnSiguiente = new System.Windows.Forms.Button();
            this.BancosBtnAnterior = new System.Windows.Forms.Button();
            this.BancosBtnInicio = new System.Windows.Forms.Button();
            this.BancosBtnFin = new System.Windows.Forms.Button();
            this.BancosBtnActualizarHistorico = new System.Windows.Forms.Button();
            this.BancosPbIconoBanco = new System.Windows.Forms.PictureBox();
            this.BancosPbMascota = new System.Windows.Forms.PictureBox();
            this.BancosPnlEncabezado.SuspendLayout();
            this.BancosTabCierre.SuspendLayout();
            this.BancosTabPeriodo.SuspendLayout();
            this.BancosPnlResumenCuentasCierre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvResumenCuentas)).BeginInit();
            this.BancosPnlValidacionesCierre.SuspendLayout();
            this.BancosPnlBotones.SuspendLayout();
            this.BancosPnlPeriodoCierre.SuspendLayout();
            this.BancosTabHistorico.SuspendLayout();
            this.BancosPnlHistorico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvHistorico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbIconoBanco)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbMascota)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosPnlEncabezado
            // 
            this.BancosPnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosPnlEncabezado.Controls.Add(this.BancosPbIconoBanco);
            this.BancosPnlEncabezado.Controls.Add(this.BancosPbMascota);
            this.BancosPnlEncabezado.Controls.Add(this.BancosLblTitulo);
            this.BancosPnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.BancosPnlEncabezado.Location = new System.Drawing.Point(0, 0);
            this.BancosPnlEncabezado.Name = "BancosPnlEncabezado";
            this.BancosPnlEncabezado.Size = new System.Drawing.Size(1200, 70);
            this.BancosPnlEncabezado.TabIndex = 1;
            // 
            // BancosLblTitulo
            // 
            this.BancosLblTitulo.AutoSize = true;
            this.BancosLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblTitulo.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BancosLblTitulo.ForeColor = System.Drawing.Color.White;
            this.BancosLblTitulo.Location = new System.Drawing.Point(88, 23);
            this.BancosLblTitulo.Name = "BancosLblTitulo";
            this.BancosLblTitulo.Size = new System.Drawing.Size(211, 25);
            this.BancosLblTitulo.TabIndex = 2;
            this.BancosLblTitulo.Text = "CIERRE BANCARIO";
            // 
            // BancosTabCierre
            // 
            this.BancosTabCierre.Controls.Add(this.BancosTabPeriodo);
            this.BancosTabCierre.Controls.Add(this.BancosTabHistorico);
            this.BancosTabCierre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BancosTabCierre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BancosTabCierre.Location = new System.Drawing.Point(0, 70);
            this.BancosTabCierre.Name = "BancosTabCierre";
            this.BancosTabCierre.SelectedIndex = 0;
            this.BancosTabCierre.Size = new System.Drawing.Size(1200, 530);
            this.BancosTabCierre.TabIndex = 0;
            // 
            // BancosTabPeriodo
            // 
            this.BancosTabPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(247)))));
            this.BancosTabPeriodo.Controls.Add(this.BancosPnlResumenCuentasCierre);
            this.BancosTabPeriodo.Controls.Add(this.BancosPnlValidacionesCierre);
            this.BancosTabPeriodo.Controls.Add(this.BancosPnlPeriodoCierre);
            this.BancosTabPeriodo.Location = new System.Drawing.Point(4, 23);
            this.BancosTabPeriodo.Name = "BancosTabPeriodo";
            this.BancosTabPeriodo.Padding = new System.Windows.Forms.Padding(12);
            this.BancosTabPeriodo.Size = new System.Drawing.Size(1192, 503);
            this.BancosTabPeriodo.TabIndex = 0;
            this.BancosTabPeriodo.Text = "Cierre de período";
            // 
            // BancosPnlResumenCuentasCierre
            // 
            this.BancosPnlResumenCuentasCierre.BackColor = System.Drawing.Color.White;
            this.BancosPnlResumenCuentasCierre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BancosPnlResumenCuentasCierre.Controls.Add(this.BancosDgvResumenCuentas);
            this.BancosPnlResumenCuentasCierre.Controls.Add(this.BancosLblResumenCuentas);
            this.BancosPnlResumenCuentasCierre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BancosPnlResumenCuentasCierre.Location = new System.Drawing.Point(12, 127);
            this.BancosPnlResumenCuentasCierre.Name = "BancosPnlResumenCuentasCierre";
            this.BancosPnlResumenCuentasCierre.Padding = new System.Windows.Forms.Padding(10);
            this.BancosPnlResumenCuentasCierre.Size = new System.Drawing.Size(1168, 210);
            this.BancosPnlResumenCuentasCierre.TabIndex = 0;
            // 
            // BancosDgvResumenCuentas
            // 
            this.BancosDgvResumenCuentas.AllowUserToAddRows = false;
            this.BancosDgvResumenCuentas.AllowUserToDeleteRows = false;
            this.BancosDgvResumenCuentas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosDgvResumenCuentas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.BancosDgvResumenCuentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BancosDgvResumenCuentas.BackgroundColor = System.Drawing.Color.White;
            this.BancosDgvResumenCuentas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BancosDgvResumenCuentas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.BancosDgvResumenCuentas.ColumnHeadersHeight = 34;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(232)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BancosDgvResumenCuentas.DefaultCellStyle = dataGridViewCellStyle3;
            this.BancosDgvResumenCuentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BancosDgvResumenCuentas.EnableHeadersVisualStyles = false;
            this.BancosDgvResumenCuentas.Location = new System.Drawing.Point(10, 44);
            this.BancosDgvResumenCuentas.MultiSelect = false;
            this.BancosDgvResumenCuentas.Name = "BancosDgvResumenCuentas";
            this.BancosDgvResumenCuentas.ReadOnly = true;
            this.BancosDgvResumenCuentas.RowHeadersVisible = false;
            this.BancosDgvResumenCuentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BancosDgvResumenCuentas.Size = new System.Drawing.Size(1146, 154);
            this.BancosDgvResumenCuentas.TabIndex = 0;
            // 
            // BancosLblResumenCuentas
            // 
            this.BancosLblResumenCuentas.Dock = System.Windows.Forms.DockStyle.Top;
            this.BancosLblResumenCuentas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblResumenCuentas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblResumenCuentas.Location = new System.Drawing.Point(10, 10);
            this.BancosLblResumenCuentas.Name = "BancosLblResumenCuentas";
            this.BancosLblResumenCuentas.Size = new System.Drawing.Size(1146, 34);
            this.BancosLblResumenCuentas.TabIndex = 1;
            this.BancosLblResumenCuentas.Text = "Conciliaciones incluidas en el cierre";
            this.BancosLblResumenCuentas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BancosPnlValidacionesCierre
            // 
            this.BancosPnlValidacionesCierre.BackColor = System.Drawing.Color.White;
            this.BancosPnlValidacionesCierre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosLblValidaciones);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkPeriodoValido);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkConciliacionesVerificadas);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkSaldosVerificados);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosTxtResultadoValidacionesCierre);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosTxtAdvertencia);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosPnlBotones);
            this.BancosPnlValidacionesCierre.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BancosPnlValidacionesCierre.Location = new System.Drawing.Point(12, 337);
            this.BancosPnlValidacionesCierre.Name = "BancosPnlValidacionesCierre";
            this.BancosPnlValidacionesCierre.Padding = new System.Windows.Forms.Padding(10);
            this.BancosPnlValidacionesCierre.Size = new System.Drawing.Size(1168, 154);
            this.BancosPnlValidacionesCierre.TabIndex = 1;
            // 
            // BancosLblValidaciones
            // 
            this.BancosLblValidaciones.AutoSize = true;
            this.BancosLblValidaciones.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblValidaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblValidaciones.Location = new System.Drawing.Point(12, 8);
            this.BancosLblValidaciones.Name = "BancosLblValidaciones";
            this.BancosLblValidaciones.Size = new System.Drawing.Size(159, 17);
            this.BancosLblValidaciones.TabIndex = 0;
            this.BancosLblValidaciones.Text = "Requisitos para cerrar";
            // 
            // BancosChkPeriodoValido
            // 
            this.BancosChkPeriodoValido.AutoSize = true;
            this.BancosChkPeriodoValido.Enabled = false;
            this.BancosChkPeriodoValido.Location = new System.Drawing.Point(16, 38);
            this.BancosChkPeriodoValido.Name = "BancosChkPeriodoValido";
            this.BancosChkPeriodoValido.Size = new System.Drawing.Size(263, 18);
            this.BancosChkPeriodoValido.TabIndex = 1;
            this.BancosChkPeriodoValido.Text = "No hay un cierre previo para el período";
            // 
            // BancosChkConciliacionesVerificadas
            // 
            this.BancosChkConciliacionesVerificadas.AutoSize = true;
            this.BancosChkConciliacionesVerificadas.Enabled = false;
            this.BancosChkConciliacionesVerificadas.Location = new System.Drawing.Point(16, 65);
            this.BancosChkConciliacionesVerificadas.Name = "BancosChkConciliacionesVerificadas";
            this.BancosChkConciliacionesVerificadas.Size = new System.Drawing.Size(287, 18);
            this.BancosChkConciliacionesVerificadas.TabIndex = 2;
            this.BancosChkConciliacionesVerificadas.Text = "Todas las conciliaciones están completadas";
            // 
            // BancosChkSaldosVerificados
            // 
            this.BancosChkSaldosVerificados.AutoSize = true;
            this.BancosChkSaldosVerificados.Enabled = false;
            this.BancosChkSaldosVerificados.Location = new System.Drawing.Point(16, 92);
            this.BancosChkSaldosVerificados.Name = "BancosChkSaldosVerificados";
            this.BancosChkSaldosVerificados.Size = new System.Drawing.Size(295, 18);
            this.BancosChkSaldosVerificados.TabIndex = 3;
            this.BancosChkSaldosVerificados.Text = "Los saldos están completos y sin diferencias";
            // 
            // BancosTxtResultadoValidacionesCierre
            // 
            this.BancosTxtResultadoValidacionesCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(241)))), ((int)(((byte)(231)))));
            this.BancosTxtResultadoValidacionesCierre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosTxtResultadoValidacionesCierre.Location = new System.Drawing.Point(16, 114);
            this.BancosTxtResultadoValidacionesCierre.Name = "BancosTxtResultadoValidacionesCierre";
            this.BancosTxtResultadoValidacionesCierre.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.BancosTxtResultadoValidacionesCierre.Size = new System.Drawing.Size(300, 28);
            this.BancosTxtResultadoValidacionesCierre.TabIndex = 4;
            this.BancosTxtResultadoValidacionesCierre.Text = "Pendiente de consulta y validación";
            this.BancosTxtResultadoValidacionesCierre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BancosTxtAdvertencia
            // 
            this.BancosTxtAdvertencia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BancosTxtAdvertencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(76)))), ((int)(((byte)(18)))));
            this.BancosTxtAdvertencia.Location = new System.Drawing.Point(330, 102);
            this.BancosTxtAdvertencia.Name = "BancosTxtAdvertencia";
            this.BancosTxtAdvertencia.Size = new System.Drawing.Size(420, 44);
            this.BancosTxtAdvertencia.TabIndex = 5;
            this.BancosTxtAdvertencia.Text = "Al confirmar, las conciliaciones del período pasarán a estado CERRADA.";
            // 
            // BancosPnlBotones
            // 
            this.BancosPnlBotones.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlBotones.Controls.Add(this.BancosBtnReporte);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnValidarCierre);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnCerrarPeriodo);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnVerHistorico);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnExportar);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnLimpiar);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnAyuda);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnSalir);
            this.BancosPnlBotones.Dock = System.Windows.Forms.DockStyle.Right;
            this.BancosPnlBotones.Location = new System.Drawing.Point(806, 10);
            this.BancosPnlBotones.Name = "BancosPnlBotones";
            this.BancosPnlBotones.Size = new System.Drawing.Size(350, 132);
            this.BancosPnlBotones.TabIndex = 6;
            // 
            // BancosPnlPeriodoCierre
            // 
            this.BancosPnlPeriodoCierre.BackColor = System.Drawing.Color.White;
            this.BancosPnlPeriodoCierre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblPeriodoCierre);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblTipoCierre);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblFechaInicial);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblFechaFinal);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblEstadoPeriodo);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblAlcance);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosCboTipoCierre);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtFechaInicial);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxFechaFinal);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtEstadoPeriodo);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtAlcance);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosBtnConsultar);
            this.BancosPnlPeriodoCierre.Dock = System.Windows.Forms.DockStyle.Top;
            this.BancosPnlPeriodoCierre.Location = new System.Drawing.Point(12, 12);
            this.BancosPnlPeriodoCierre.Name = "BancosPnlPeriodoCierre";
            this.BancosPnlPeriodoCierre.Padding = new System.Windows.Forms.Padding(12);
            this.BancosPnlPeriodoCierre.Size = new System.Drawing.Size(1168, 115);
            this.BancosPnlPeriodoCierre.TabIndex = 2;
            // 
            // BancosLblPeriodoCierre
            // 
            this.BancosLblPeriodoCierre.AutoSize = true;
            this.BancosLblPeriodoCierre.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblPeriodoCierre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblPeriodoCierre.Location = new System.Drawing.Point(14, 9);
            this.BancosLblPeriodoCierre.Name = "BancosLblPeriodoCierre";
            this.BancosLblPeriodoCierre.Size = new System.Drawing.Size(270, 17);
            this.BancosLblPeriodoCierre.TabIndex = 0;
            this.BancosLblPeriodoCierre.Text = "Seleccione el período que desea cerrar";
            // 
            // BancosLblTipoCierre
            // 
            this.BancosLblTipoCierre.AutoSize = true;
            this.BancosLblTipoCierre.Location = new System.Drawing.Point(18, 48);
            this.BancosLblTipoCierre.Name = "BancosLblTipoCierre";
            this.BancosLblTipoCierre.Size = new System.Drawing.Size(103, 14);
            this.BancosLblTipoCierre.TabIndex = 1;
            this.BancosLblTipoCierre.Text = "Tipo de período";
            // 
            // BancosLblFechaInicial
            // 
            this.BancosLblFechaInicial.AutoSize = true;
            this.BancosLblFechaInicial.Location = new System.Drawing.Point(345, 48);
            this.BancosLblFechaInicial.Name = "BancosLblFechaInicial";
            this.BancosLblFechaInicial.Size = new System.Drawing.Size(78, 14);
            this.BancosLblFechaInicial.TabIndex = 2;
            this.BancosLblFechaInicial.Text = "Fecha inicial";
            // 
            // BancosLblFechaFinal
            // 
            this.BancosLblFechaFinal.AutoSize = true;
            this.BancosLblFechaFinal.Location = new System.Drawing.Point(610, 48);
            this.BancosLblFechaFinal.Name = "BancosLblFechaFinal";
            this.BancosLblFechaFinal.Size = new System.Drawing.Size(71, 14);
            this.BancosLblFechaFinal.TabIndex = 3;
            this.BancosLblFechaFinal.Text = "Fecha final";
            // 
            // BancosLblEstadoPeriodo
            // 
            this.BancosLblEstadoPeriodo.AutoSize = true;
            this.BancosLblEstadoPeriodo.Location = new System.Drawing.Point(18, 85);
            this.BancosLblEstadoPeriodo.Name = "BancosLblEstadoPeriodo";
            this.BancosLblEstadoPeriodo.Size = new System.Drawing.Size(49, 14);
            this.BancosLblEstadoPeriodo.TabIndex = 4;
            this.BancosLblEstadoPeriodo.Text = "Estado";
            // 
            // BancosLblAlcance
            // 
            this.BancosLblAlcance.AutoSize = true;
            this.BancosLblAlcance.Location = new System.Drawing.Point(385, 85);
            this.BancosLblAlcance.Name = "BancosLblAlcance";
            this.BancosLblAlcance.Size = new System.Drawing.Size(53, 14);
            this.BancosLblAlcance.TabIndex = 5;
            this.BancosLblAlcance.Text = "Alcance";
            // 
            // BancosCboTipoCierre
            // 
            this.BancosCboTipoCierre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BancosCboTipoCierre.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.BancosCboTipoCierre.Location = new System.Drawing.Point(145, 45);
            this.BancosCboTipoCierre.Name = "BancosCboTipoCierre";
            this.BancosCboTipoCierre.Size = new System.Drawing.Size(170, 27);
            this.BancosCboTipoCierre.TabIndex = 6;
            // 
            // BancosTxtFechaInicial
            // 
            this.BancosTxtFechaInicial.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.BancosTxtFechaInicial.Location = new System.Drawing.Point(440, 45);
            this.BancosTxtFechaInicial.Name = "BancosTxtFechaInicial";
            this.BancosTxtFechaInicial.Size = new System.Drawing.Size(145, 26);
            this.BancosTxtFechaInicial.TabIndex = 7;
            // 
            // BancosTxFechaFinal
            // 
            this.BancosTxFechaFinal.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.BancosTxFechaFinal.Location = new System.Drawing.Point(690, 45);
            this.BancosTxFechaFinal.Name = "BancosTxFechaFinal";
            this.BancosTxFechaFinal.Size = new System.Drawing.Size(145, 26);
            this.BancosTxFechaFinal.TabIndex = 8;
            // 
            // BancosTxtEstadoPeriodo
            // 
            this.BancosTxtEstadoPeriodo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosTxtEstadoPeriodo.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.BancosTxtEstadoPeriodo.Location = new System.Drawing.Point(145, 82);
            this.BancosTxtEstadoPeriodo.Name = "BancosTxtEstadoPeriodo";
            this.BancosTxtEstadoPeriodo.ReadOnly = true;
            this.BancosTxtEstadoPeriodo.Size = new System.Drawing.Size(220, 26);
            this.BancosTxtEstadoPeriodo.TabIndex = 9;
            // 
            // BancosTxtAlcance
            // 
            this.BancosTxtAlcance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosTxtAlcance.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.BancosTxtAlcance.Location = new System.Drawing.Point(455, 82);
            this.BancosTxtAlcance.Name = "BancosTxtAlcance";
            this.BancosTxtAlcance.ReadOnly = true;
            this.BancosTxtAlcance.Size = new System.Drawing.Size(380, 26);
            this.BancosTxtAlcance.TabIndex = 10;
            // 
            // BancosTabHistorico
            // 
            this.BancosTabHistorico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(247)))));
            this.BancosTabHistorico.Controls.Add(this.BancosPnlHistorico);
            this.BancosTabHistorico.Location = new System.Drawing.Point(4, 23);
            this.BancosTabHistorico.Name = "BancosTabHistorico";
            this.BancosTabHistorico.Padding = new System.Windows.Forms.Padding(12);
            this.BancosTabHistorico.Size = new System.Drawing.Size(1192, 503);
            this.BancosTabHistorico.TabIndex = 1;
            this.BancosTabHistorico.Text = "Historial de cierres";
            // 
            // BancosPnlHistorico
            // 
            this.BancosPnlHistorico.BackColor = System.Drawing.Color.White;
            this.BancosPnlHistorico.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BancosPnlHistorico.Controls.Add(this.BancosBtnSiguiente);
            this.BancosPnlHistorico.Controls.Add(this.BancosBtnAnterior);
            this.BancosPnlHistorico.Controls.Add(this.BancosBtnInicio);
            this.BancosPnlHistorico.Controls.Add(this.BancosBtnFin);
            this.BancosPnlHistorico.Controls.Add(this.BancosDgvHistorico);
            this.BancosPnlHistorico.Controls.Add(this.BancosBtnActualizarHistorico);
            this.BancosPnlHistorico.Controls.Add(this.BancosLblHistorico);
            this.BancosPnlHistorico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BancosPnlHistorico.Location = new System.Drawing.Point(12, 12);
            this.BancosPnlHistorico.Name = "BancosPnlHistorico";
            this.BancosPnlHistorico.Padding = new System.Windows.Forms.Padding(12);
            this.BancosPnlHistorico.Size = new System.Drawing.Size(1168, 479);
            this.BancosPnlHistorico.TabIndex = 0;
            // 
            // BancosDgvHistorico
            // 
            this.BancosDgvHistorico.AllowUserToAddRows = false;
            this.BancosDgvHistorico.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(240)))));
            this.BancosDgvHistorico.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.BancosDgvHistorico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BancosDgvHistorico.BackgroundColor = System.Drawing.Color.White;
            this.BancosDgvHistorico.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.BancosDgvHistorico.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.BancosDgvHistorico.ColumnHeadersHeight = 32;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(232)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.BancosDgvHistorico.DefaultCellStyle = dataGridViewCellStyle6;
            this.BancosDgvHistorico.EnableHeadersVisualStyles = false;
            this.BancosDgvHistorico.Location = new System.Drawing.Point(12, 70);
            this.BancosDgvHistorico.Name = "BancosDgvHistorico";
            this.BancosDgvHistorico.ReadOnly = true;
            this.BancosDgvHistorico.RowHeadersVisible = false;
            this.BancosDgvHistorico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.BancosDgvHistorico.Size = new System.Drawing.Size(1142, 397);
            this.BancosDgvHistorico.TabIndex = 0;
            // 
            // BancosLblHistorico
            // 
            this.BancosLblHistorico.Dock = System.Windows.Forms.DockStyle.Top;
            this.BancosLblHistorico.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblHistorico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(78)))), ((int)(((byte)(53)))));
            this.BancosLblHistorico.Location = new System.Drawing.Point(12, 12);
            this.BancosLblHistorico.Name = "BancosLblHistorico";
            this.BancosLblHistorico.Size = new System.Drawing.Size(1142, 59);
            this.BancosLblHistorico.TabIndex = 2;
            this.BancosLblHistorico.Text = "Cierres registrados ";
            this.BancosLblHistorico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BancosBtnReporte
            // 
            this.BancosBtnReporte.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_reporteB1;
            this.BancosBtnReporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnReporte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnReporte.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.BancosBtnReporte.Location = new System.Drawing.Point(141, 65);
            this.BancosBtnReporte.Name = "BancosBtnReporte";
            this.BancosBtnReporte.Size = new System.Drawing.Size(64, 56);
            this.BancosBtnReporte.TabIndex = 7;
            this.BancosBtnReporte.UseVisualStyleBackColor = true;
            this.BancosBtnReporte.Click += new System.EventHandler(this.BancosBtnReporte_Click);
            // 
            // BancosBtnValidarCierre
            // 
            this.BancosBtnValidarCierre.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnValidarCierre.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_validarcierreB;
            this.BancosBtnValidarCierre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnValidarCierre.FlatAppearance.BorderSize = 0;
            this.BancosBtnValidarCierre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnValidarCierre.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnValidarCierre.Location = new System.Drawing.Point(0, 3);
            this.BancosBtnValidarCierre.Name = "BancosBtnValidarCierre";
            this.BancosBtnValidarCierre.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnValidarCierre.TabIndex = 0;
            this.BancosBtnValidarCierre.Click += new System.EventHandler(this.BancosBtnValidarCierre_Click);
            // 
            // BancosBtnCerrarPeriodo
            // 
            this.BancosBtnCerrarPeriodo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnCerrarPeriodo.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_cerrarperiodoB;
            this.BancosBtnCerrarPeriodo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnCerrarPeriodo.FlatAppearance.BorderSize = 0;
            this.BancosBtnCerrarPeriodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnCerrarPeriodo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnCerrarPeriodo.Location = new System.Drawing.Point(70, 3);
            this.BancosBtnCerrarPeriodo.Name = "BancosBtnCerrarPeriodo";
            this.BancosBtnCerrarPeriodo.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnCerrarPeriodo.TabIndex = 1;
            this.BancosBtnCerrarPeriodo.Click += new System.EventHandler(this.BancosBtnCerrarPeriodo_Click);
            // 
            // BancosBtnVerHistorico
            // 
            this.BancosBtnVerHistorico.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnVerHistorico.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_verhistoricoB;
            this.BancosBtnVerHistorico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnVerHistorico.FlatAppearance.BorderSize = 0;
            this.BancosBtnVerHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnVerHistorico.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnVerHistorico.Location = new System.Drawing.Point(140, 3);
            this.BancosBtnVerHistorico.Name = "BancosBtnVerHistorico";
            this.BancosBtnVerHistorico.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnVerHistorico.TabIndex = 2;
            this.BancosBtnVerHistorico.Click += new System.EventHandler(this.BancosBtnVerHistorico_Click);
            // 
            // BancosBtnExportar
            // 
            this.BancosBtnExportar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnExportar.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_exportar;
            this.BancosBtnExportar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnExportar.FlatAppearance.BorderSize = 0;
            this.BancosBtnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnExportar.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnExportar.Location = new System.Drawing.Point(210, 3);
            this.BancosBtnExportar.Name = "BancosBtnExportar";
            this.BancosBtnExportar.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnExportar.TabIndex = 3;
            this.BancosBtnExportar.Click += new System.EventHandler(this.BancosBtnExportar_Click);
            // 
            // BancosBtnLimpiar
            // 
            this.BancosBtnLimpiar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnLimpiar.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_limpiarB;
            this.BancosBtnLimpiar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnLimpiar.FlatAppearance.BorderSize = 0;
            this.BancosBtnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnLimpiar.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnLimpiar.Location = new System.Drawing.Point(3, 65);
            this.BancosBtnLimpiar.Name = "BancosBtnLimpiar";
            this.BancosBtnLimpiar.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnLimpiar.TabIndex = 4;
            this.BancosBtnLimpiar.Click += new System.EventHandler(this.BancosBtnLimpiar_Click);
            // 
            // BancosBtnAyuda
            // 
            this.BancosBtnAyuda.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnAyuda.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_ayuda;
            this.BancosBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnAyuda.FlatAppearance.BorderSize = 0;
            this.BancosBtnAyuda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnAyuda.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnAyuda.Location = new System.Drawing.Point(70, 65);
            this.BancosBtnAyuda.Name = "BancosBtnAyuda";
            this.BancosBtnAyuda.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnAyuda.TabIndex = 5;
            this.BancosBtnAyuda.Click += new System.EventHandler(this.BancosBtnAyuda_Click);
            // 
            // BancosBtnSalir
            // 
            this.BancosBtnSalir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BancosBtnSalir.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_salir;
            this.BancosBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnSalir.FlatAppearance.BorderSize = 0;
            this.BancosBtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnSalir.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.BancosBtnSalir.Location = new System.Drawing.Point(211, 65);
            this.BancosBtnSalir.Name = "BancosBtnSalir";
            this.BancosBtnSalir.Size = new System.Drawing.Size(65, 56);
            this.BancosBtnSalir.TabIndex = 6;
            this.BancosBtnSalir.Click += new System.EventHandler(this.BancosBtnSalir_Click);
            // 
            // BancosBtnConsultar
            // 
            this.BancosBtnConsultar.AccessibleName = "Consultar conciliaciones";
            this.BancosBtnConsultar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnConsultar.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_consultarB;
            this.BancosBtnConsultar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BancosBtnConsultar.FlatAppearance.BorderSize = 0;
            this.BancosBtnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnConsultar.Location = new System.Drawing.Point(1978, 36);
            this.BancosBtnConsultar.Name = "BancosBtnConsultar";
            this.BancosBtnConsultar.Size = new System.Drawing.Size(94, 76);
            this.BancosBtnConsultar.TabIndex = 11;
            this.BancosBtnConsultar.Click += new System.EventHandler(this.BancosBtnConsultar_Click);
            // 
            // BancosBtnSiguiente
            // 
            this.BancosBtnSiguiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnSiguiente.BackColor = System.Drawing.Color.Transparent;
            this.BancosBtnSiguiente.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_siguiente;
            this.BancosBtnSiguiente.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnSiguiente.FlatAppearance.BorderSize = 0;
            this.BancosBtnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnSiguiente.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosBtnSiguiente.ForeColor = System.Drawing.Color.White;
            this.BancosBtnSiguiente.Location = new System.Drawing.Point(964, 6);
            this.BancosBtnSiguiente.Name = "BancosBtnSiguiente";
            this.BancosBtnSiguiente.Size = new System.Drawing.Size(60, 52);
            this.BancosBtnSiguiente.TabIndex = 6;
            this.BancosBtnSiguiente.UseVisualStyleBackColor = false;
            // 
            // BancosBtnAnterior
            // 
            this.BancosBtnAnterior.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnAnterior.BackColor = System.Drawing.Color.Transparent;
            this.BancosBtnAnterior.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_anterior;
            this.BancosBtnAnterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnAnterior.FlatAppearance.BorderSize = 0;
            this.BancosBtnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnAnterior.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosBtnAnterior.ForeColor = System.Drawing.Color.White;
            this.BancosBtnAnterior.Location = new System.Drawing.Point(898, 6);
            this.BancosBtnAnterior.Name = "BancosBtnAnterior";
            this.BancosBtnAnterior.Size = new System.Drawing.Size(60, 52);
            this.BancosBtnAnterior.TabIndex = 5;
            this.BancosBtnAnterior.UseVisualStyleBackColor = false;
            // 
            // BancosBtnInicio
            // 
            this.BancosBtnInicio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnInicio.BackColor = System.Drawing.Color.Transparent;
            this.BancosBtnInicio.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_inicio;
            this.BancosBtnInicio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnInicio.FlatAppearance.BorderSize = 0;
            this.BancosBtnInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnInicio.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosBtnInicio.ForeColor = System.Drawing.Color.White;
            this.BancosBtnInicio.Location = new System.Drawing.Point(832, 6);
            this.BancosBtnInicio.Name = "BancosBtnInicio";
            this.BancosBtnInicio.Size = new System.Drawing.Size(60, 52);
            this.BancosBtnInicio.TabIndex = 4;
            this.BancosBtnInicio.UseVisualStyleBackColor = false;
            // 
            // BancosBtnFin
            // 
            this.BancosBtnFin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnFin.BackColor = System.Drawing.Color.Transparent;
            this.BancosBtnFin.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_fin;
            this.BancosBtnFin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnFin.FlatAppearance.BorderSize = 0;
            this.BancosBtnFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnFin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosBtnFin.ForeColor = System.Drawing.Color.White;
            this.BancosBtnFin.Location = new System.Drawing.Point(1030, 6);
            this.BancosBtnFin.Name = "BancosBtnFin";
            this.BancosBtnFin.Size = new System.Drawing.Size(60, 52);
            this.BancosBtnFin.TabIndex = 3;
            this.BancosBtnFin.UseVisualStyleBackColor = false;
            // 
            // BancosBtnActualizarHistorico
            // 
            this.BancosBtnActualizarHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BancosBtnActualizarHistorico.BackColor = System.Drawing.Color.Transparent;
            this.BancosBtnActualizarHistorico.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_actualizar_historial;
            this.BancosBtnActualizarHistorico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnActualizarHistorico.FlatAppearance.BorderSize = 0;
            this.BancosBtnActualizarHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BancosBtnActualizarHistorico.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BancosBtnActualizarHistorico.ForeColor = System.Drawing.Color.White;
            this.BancosBtnActualizarHistorico.Location = new System.Drawing.Point(1096, 6);
            this.BancosBtnActualizarHistorico.Name = "BancosBtnActualizarHistorico";
            this.BancosBtnActualizarHistorico.Size = new System.Drawing.Size(60, 52);
            this.BancosBtnActualizarHistorico.TabIndex = 1;
            this.BancosBtnActualizarHistorico.UseVisualStyleBackColor = false;
            this.BancosBtnActualizarHistorico.Click += new System.EventHandler(this.BancosBtnVerHistorico_Click);
            // 
            // BancosPbIconoBanco
            // 
            this.BancosPbIconoBanco.Image = global::CapaVista_CierreBancario.Properties.Resources.Bancario;
            this.BancosPbIconoBanco.Location = new System.Drawing.Point(18, 9);
            this.BancosPbIconoBanco.Name = "BancosPbIconoBanco";
            this.BancosPbIconoBanco.Size = new System.Drawing.Size(52, 52);
            this.BancosPbIconoBanco.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BancosPbIconoBanco.TabIndex = 0;
            this.BancosPbIconoBanco.TabStop = false;
            // 
            // BancosPbMascota
            // 
            this.BancosPbMascota.Dock = System.Windows.Forms.DockStyle.Right;
            this.BancosPbMascota.Image = global::CapaVista_CierreBancario.Properties.Resources._10;
            this.BancosPbMascota.Location = new System.Drawing.Point(1112, 0);
            this.BancosPbMascota.Name = "BancosPbMascota";
            this.BancosPbMascota.Size = new System.Drawing.Size(88, 70);
            this.BancosPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BancosPbMascota.TabIndex = 1;
            this.BancosPbMascota.TabStop = false;
            // 
            // FrmCierreBancario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1200, 600);
            this.Controls.Add(this.BancosTabCierre);
            this.Controls.Add(this.BancosPnlEncabezado);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmCierreBancario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8013-CierreBancario";
            this.Load += new System.EventHandler(this.FrmCierreBancario_Load);
            this.BancosPnlEncabezado.ResumeLayout(false);
            this.BancosPnlEncabezado.PerformLayout();
            this.BancosTabCierre.ResumeLayout(false);
            this.BancosTabPeriodo.ResumeLayout(false);
            this.BancosPnlResumenCuentasCierre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvResumenCuentas)).EndInit();
            this.BancosPnlValidacionesCierre.ResumeLayout(false);
            this.BancosPnlValidacionesCierre.PerformLayout();
            this.BancosPnlBotones.ResumeLayout(false);
            this.BancosPnlPeriodoCierre.ResumeLayout(false);
            this.BancosPnlPeriodoCierre.PerformLayout();
            this.BancosTabHistorico.ResumeLayout(false);
            this.BancosPnlHistorico.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvHistorico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbIconoBanco)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbMascota)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button BancosBtnSiguiente;
        private System.Windows.Forms.Button BancosBtnAnterior;
        private System.Windows.Forms.Button BancosBtnInicio;
        private System.Windows.Forms.Button BancosBtnFin;
        private System.Windows.Forms.Button BancosBtnReporte;
    }
}
