namespace CapaVista_CierreBancario.Formularios
{
    partial class FrmCierreBancario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCierreBancario));
            this.BancosBtnConsultar = new System.Windows.Forms.Button();
            this.BancosPnlPeriodoCierre = new System.Windows.Forms.Panel();
            this.BancosCboTipoCierre = new System.Windows.Forms.ComboBox();
            this.BancosTxFechaFinal = new System.Windows.Forms.TextBox();
            this.BancosTxtAlcance = new System.Windows.Forms.TextBox();
            this.BancosTxtFechaInicial = new System.Windows.Forms.TextBox();
            this.BancosTxtEstadoPeriodo = new System.Windows.Forms.TextBox();
            this.BancosLblFechaFinal = new System.Windows.Forms.Label();
            this.BancosLblAlcance = new System.Windows.Forms.Label();
            this.BancosLblFechaInicial = new System.Windows.Forms.Label();
            this.BancosLblEstadoPeriodo = new System.Windows.Forms.Label();
            this.BancosLblTipoCierre = new System.Windows.Forms.Label();
            this.BancosLblPeriodoCierre = new System.Windows.Forms.Label();
            this.BancosLblTitulo = new System.Windows.Forms.Label();
            this.BancosPnlResumenCuentasCierre = new System.Windows.Forms.Panel();
            this.BancosDgvResumenCuentas = new System.Windows.Forms.DataGridView();
            this.BancosLblResumenCuentas = new System.Windows.Forms.Label();
            this.BancosPnlValidacionesCierre = new System.Windows.Forms.Panel();
            this.BancosTxtAdvertencia = new System.Windows.Forms.Label();
            this.BancosTxtResultadoValidacionesCierre = new System.Windows.Forms.Label();
            this.BancosTxtObservaciones = new System.Windows.Forms.TextBox();
            this.BancosLblObservaciones = new System.Windows.Forms.Label();
            this.BancosChkSaldosVerificados = new System.Windows.Forms.CheckBox();
            this.BancosChkConciliacionesVerificadas = new System.Windows.Forms.CheckBox();
            this.BancosChkPeriodoValido = new System.Windows.Forms.CheckBox();
            this.BancosLblValidaciones = new System.Windows.Forms.Label();
            this.BancosPnlBotones = new System.Windows.Forms.Panel();
            this.BancosBtnSalir = new System.Windows.Forms.Button();
            this.BancosBtnValidarCierre = new System.Windows.Forms.Button();
            this.BancosBtnAyuda = new System.Windows.Forms.Button();
            this.BancosBtnVerHistorico = new System.Windows.Forms.Button();
            this.BancosBtnReporte = new System.Windows.Forms.Button();
            this.BancosBtnCerrarPeriodo = new System.Windows.Forms.Button();
            this.BancosBtnLimpiar = new System.Windows.Forms.Button();
            this.BancosPbIconoBanco = new System.Windows.Forms.PictureBox();
            this.BancosPbMascota = new System.Windows.Forms.PictureBox();
            this.BancosPnlPeriodoCierre.SuspendLayout();
            this.BancosPnlResumenCuentasCierre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvResumenCuentas)).BeginInit();
            this.BancosPnlValidacionesCierre.SuspendLayout();
            this.BancosPnlBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbIconoBanco)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbMascota)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosBtnConsultar
            // 
            this.BancosBtnConsultar.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_consultarB;
            this.BancosBtnConsultar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnConsultar.Location = new System.Drawing.Point(1101, 22);
            this.BancosBtnConsultar.Name = "BancosBtnConsultar";
            this.BancosBtnConsultar.Size = new System.Drawing.Size(77, 75);
            this.BancosBtnConsultar.TabIndex = 1;
            this.BancosBtnConsultar.UseVisualStyleBackColor = true;
            // 
            // BancosPnlPeriodoCierre
            // 
            this.BancosPnlPeriodoCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosCboTipoCierre);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxFechaFinal);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtAlcance);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtFechaInicial);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosTxtEstadoPeriodo);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblFechaFinal);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblAlcance);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosBtnConsultar);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblFechaInicial);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblEstadoPeriodo);
            this.BancosPnlPeriodoCierre.Controls.Add(this.BancosLblTipoCierre);
            this.BancosPnlPeriodoCierre.Location = new System.Drawing.Point(10, 92);
            this.BancosPnlPeriodoCierre.Name = "BancosPnlPeriodoCierre";
            this.BancosPnlPeriodoCierre.Size = new System.Drawing.Size(1181, 105);
            this.BancosPnlPeriodoCierre.TabIndex = 2;
            // 
            // BancosCboTipoCierre
            // 
            this.BancosCboTipoCierre.FormattingEnabled = true;
            this.BancosCboTipoCierre.Location = new System.Drawing.Point(154, 28);
            this.BancosCboTipoCierre.Name = "BancosCboTipoCierre";
            this.BancosCboTipoCierre.Size = new System.Drawing.Size(218, 20);
            this.BancosCboTipoCierre.TabIndex = 15;
            // 
            // BancosTxFechaFinal
            // 
            this.BancosTxFechaFinal.Location = new System.Drawing.Point(537, 69);
            this.BancosTxFechaFinal.Name = "BancosTxFechaFinal";
            this.BancosTxFechaFinal.Size = new System.Drawing.Size(218, 20);
            this.BancosTxFechaFinal.TabIndex = 14;
            // 
            // BancosTxtAlcance
            // 
            this.BancosTxtAlcance.Location = new System.Drawing.Point(863, 48);
            this.BancosTxtAlcance.Name = "BancosTxtAlcance";
            this.BancosTxtAlcance.Size = new System.Drawing.Size(218, 20);
            this.BancosTxtAlcance.TabIndex = 13;
            // 
            // BancosTxtFechaInicial
            // 
            this.BancosTxtFechaInicial.Location = new System.Drawing.Point(537, 27);
            this.BancosTxtFechaInicial.Name = "BancosTxtFechaInicial";
            this.BancosTxtFechaInicial.Size = new System.Drawing.Size(218, 20);
            this.BancosTxtFechaInicial.TabIndex = 12;
            // 
            // BancosTxtEstadoPeriodo
            // 
            this.BancosTxtEstadoPeriodo.Location = new System.Drawing.Point(187, 69);
            this.BancosTxtEstadoPeriodo.Name = "BancosTxtEstadoPeriodo";
            this.BancosTxtEstadoPeriodo.Size = new System.Drawing.Size(185, 20);
            this.BancosTxtEstadoPeriodo.TabIndex = 11;
            // 
            // BancosLblFechaFinal
            // 
            this.BancosLblFechaFinal.AutoSize = true;
            this.BancosLblFechaFinal.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblFechaFinal.Location = new System.Drawing.Point(417, 69);
            this.BancosLblFechaFinal.Name = "BancosLblFechaFinal";
            this.BancosLblFechaFinal.Size = new System.Drawing.Size(80, 18);
            this.BancosLblFechaFinal.TabIndex = 9;
            this.BancosLblFechaFinal.Text = "Fecha Final";
            // 
            // BancosLblAlcance
            // 
            this.BancosLblAlcance.AutoSize = true;
            this.BancosLblAlcance.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblAlcance.Location = new System.Drawing.Point(779, 46);
            this.BancosLblAlcance.Name = "BancosLblAlcance";
            this.BancosLblAlcance.Size = new System.Drawing.Size(57, 18);
            this.BancosLblAlcance.TabIndex = 8;
            this.BancosLblAlcance.Text = "Alcance";
            // 
            // BancosLblFechaInicial
            // 
            this.BancosLblFechaInicial.AutoSize = true;
            this.BancosLblFechaInicial.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblFechaInicial.Location = new System.Drawing.Point(416, 28);
            this.BancosLblFechaInicial.Name = "BancosLblFechaInicial";
            this.BancosLblFechaInicial.Size = new System.Drawing.Size(87, 18);
            this.BancosLblFechaInicial.TabIndex = 7;
            this.BancosLblFechaInicial.Text = "Fecha Inicial";
            // 
            // BancosLblEstadoPeriodo
            // 
            this.BancosLblEstadoPeriodo.AutoSize = true;
            this.BancosLblEstadoPeriodo.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblEstadoPeriodo.Location = new System.Drawing.Point(15, 69);
            this.BancosLblEstadoPeriodo.Name = "BancosLblEstadoPeriodo";
            this.BancosLblEstadoPeriodo.Size = new System.Drawing.Size(127, 18);
            this.BancosLblEstadoPeriodo.TabIndex = 6;
            this.BancosLblEstadoPeriodo.Text = "Estado del periodo";
            // 
            // BancosLblTipoCierre
            // 
            this.BancosLblTipoCierre.AutoSize = true;
            this.BancosLblTipoCierre.Font = new System.Drawing.Font("Tahoma", 11F);
            this.BancosLblTipoCierre.Location = new System.Drawing.Point(15, 30);
            this.BancosLblTipoCierre.Name = "BancosLblTipoCierre";
            this.BancosLblTipoCierre.Size = new System.Drawing.Size(97, 18);
            this.BancosLblTipoCierre.TabIndex = 5;
            this.BancosLblTipoCierre.Text = "Tipo de cierre";
            // 
            // BancosLblPeriodoCierre
            // 
            this.BancosLblPeriodoCierre.AutoSize = true;
            this.BancosLblPeriodoCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(169)))), ((int)(((byte)(59)))));
            this.BancosLblPeriodoCierre.Font = new System.Drawing.Font("Tahoma", 13F);
            this.BancosLblPeriodoCierre.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosLblPeriodoCierre.Location = new System.Drawing.Point(23, 76);
            this.BancosLblPeriodoCierre.Name = "BancosLblPeriodoCierre";
            this.BancosLblPeriodoCierre.Size = new System.Drawing.Size(142, 22);
            this.BancosLblPeriodoCierre.TabIndex = 4;
            this.BancosLblPeriodoCierre.Text = "Período de cierre";
            // 
            // BancosLblTitulo
            // 
            this.BancosLblTitulo.AutoSize = true;
            this.BancosLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.BancosLblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosLblTitulo.Location = new System.Drawing.Point(89, 12);
            this.BancosLblTitulo.Name = "BancosLblTitulo";
            this.BancosLblTitulo.Size = new System.Drawing.Size(237, 32);
            this.BancosLblTitulo.TabIndex = 3;
            this.BancosLblTitulo.Text = "Cierre bancario";
            // 
            // BancosPnlResumenCuentasCierre
            // 
            this.BancosPnlResumenCuentasCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.BancosPnlResumenCuentasCierre.Controls.Add(this.BancosDgvResumenCuentas);
            this.BancosPnlResumenCuentasCierre.Location = new System.Drawing.Point(10, 230);
            this.BancosPnlResumenCuentasCierre.Name = "BancosPnlResumenCuentasCierre";
            this.BancosPnlResumenCuentasCierre.Size = new System.Drawing.Size(1181, 168);
            this.BancosPnlResumenCuentasCierre.TabIndex = 4;
            // 
            // BancosDgvResumenCuentas
            // 
            this.BancosDgvResumenCuentas.BackgroundColor = System.Drawing.Color.White;
            this.BancosDgvResumenCuentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BancosDgvResumenCuentas.Location = new System.Drawing.Point(17, 31);
            this.BancosDgvResumenCuentas.Name = "BancosDgvResumenCuentas";
            this.BancosDgvResumenCuentas.RowHeadersWidth = 51;
            this.BancosDgvResumenCuentas.RowTemplate.Height = 24;
            this.BancosDgvResumenCuentas.Size = new System.Drawing.Size(1147, 124);
            this.BancosDgvResumenCuentas.TabIndex = 11;
            // 
            // BancosLblResumenCuentas
            // 
            this.BancosLblResumenCuentas.AutoSize = true;
            this.BancosLblResumenCuentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(169)))), ((int)(((byte)(59)))));
            this.BancosLblResumenCuentas.Font = new System.Drawing.Font("Tahoma", 13F);
            this.BancosLblResumenCuentas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosLblResumenCuentas.Location = new System.Drawing.Point(21, 215);
            this.BancosLblResumenCuentas.Name = "BancosLblResumenCuentas";
            this.BancosLblResumenCuentas.Size = new System.Drawing.Size(240, 22);
            this.BancosLblResumenCuentas.TabIndex = 10;
            this.BancosLblResumenCuentas.Text = "Resumen de cuentas al cierre";
            // 
            // BancosPnlValidacionesCierre
            // 
            this.BancosPnlValidacionesCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosTxtAdvertencia);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosTxtResultadoValidacionesCierre);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosTxtObservaciones);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosLblObservaciones);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkSaldosVerificados);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkConciliacionesVerificadas);
            this.BancosPnlValidacionesCierre.Controls.Add(this.BancosChkPeriodoValido);
            this.BancosPnlValidacionesCierre.Location = new System.Drawing.Point(12, 433);
            this.BancosPnlValidacionesCierre.Name = "BancosPnlValidacionesCierre";
            this.BancosPnlValidacionesCierre.Size = new System.Drawing.Size(640, 298);
            this.BancosPnlValidacionesCierre.TabIndex = 5;
            // 
            // BancosTxtAdvertencia
            // 
            this.BancosTxtAdvertencia.AutoSize = true;
            this.BancosTxtAdvertencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.BancosTxtAdvertencia.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BancosTxtAdvertencia.Location = new System.Drawing.Point(13, 266);
            this.BancosTxtAdvertencia.Name = "BancosTxtAdvertencia";
            this.BancosTxtAdvertencia.Size = new System.Drawing.Size(354, 17);
            this.BancosTxtAdvertencia.TabIndex = 17;
            this.BancosTxtAdvertencia.Text = "Al cerrar los docuentos del periodo quedaran bloqueados";
            // 
            // BancosTxtResultadoValidacionesCierre
            // 
            this.BancosTxtResultadoValidacionesCierre.AutoSize = true;
            this.BancosTxtResultadoValidacionesCierre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(231)))), ((int)(((byte)(211)))));
            this.BancosTxtResultadoValidacionesCierre.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BancosTxtResultadoValidacionesCierre.Location = new System.Drawing.Point(13, 91);
            this.BancosTxtResultadoValidacionesCierre.Name = "BancosTxtResultadoValidacionesCierre";
            this.BancosTxtResultadoValidacionesCierre.Size = new System.Drawing.Size(176, 17);
            this.BancosTxtResultadoValidacionesCierre.TabIndex = 16;
            this.BancosTxtResultadoValidacionesCierre.Text = "Resultado: Listo para cerrar";
            // 
            // BancosTxtObservaciones
            // 
            this.BancosTxtObservaciones.BackColor = System.Drawing.Color.White;
            this.BancosTxtObservaciones.Location = new System.Drawing.Point(14, 159);
            this.BancosTxtObservaciones.Multiline = true;
            this.BancosTxtObservaciones.Name = "BancosTxtObservaciones";
            this.BancosTxtObservaciones.Size = new System.Drawing.Size(605, 91);
            this.BancosTxtObservaciones.TabIndex = 15;
            // 
            // BancosLblObservaciones
            // 
            this.BancosLblObservaciones.AutoSize = true;
            this.BancosLblObservaciones.Font = new System.Drawing.Font("Tahoma", 13F);
            this.BancosLblObservaciones.Location = new System.Drawing.Point(12, 129);
            this.BancosLblObservaciones.Name = "BancosLblObservaciones";
            this.BancosLblObservaciones.Size = new System.Drawing.Size(123, 22);
            this.BancosLblObservaciones.TabIndex = 10;
            this.BancosLblObservaciones.Text = "Observaciones";
            // 
            // BancosChkSaldosVerificados
            // 
            this.BancosChkSaldosVerificados.AutoSize = true;
            this.BancosChkSaldosVerificados.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BancosChkSaldosVerificados.ForeColor = System.Drawing.Color.Black;
            this.BancosChkSaldosVerificados.Location = new System.Drawing.Point(14, 55);
            this.BancosChkSaldosVerificados.Name = "BancosChkSaldosVerificados";
            this.BancosChkSaldosVerificados.Size = new System.Drawing.Size(223, 21);
            this.BancosChkSaldosVerificados.TabIndex = 14;
            this.BancosChkSaldosVerificados.Text = "Saldos de las cuentas verificadas";
            this.BancosChkSaldosVerificados.UseVisualStyleBackColor = true;
            // 
            // BancosChkConciliacionesVerificadas
            // 
            this.BancosChkConciliacionesVerificadas.AutoSize = true;
            this.BancosChkConciliacionesVerificadas.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BancosChkConciliacionesVerificadas.ForeColor = System.Drawing.Color.Black;
            this.BancosChkConciliacionesVerificadas.Location = new System.Drawing.Point(300, 20);
            this.BancosChkConciliacionesVerificadas.Name = "BancosChkConciliacionesVerificadas";
            this.BancosChkConciliacionesVerificadas.Size = new System.Drawing.Size(177, 21);
            this.BancosChkConciliacionesVerificadas.TabIndex = 13;
            this.BancosChkConciliacionesVerificadas.Text = "Conciliaciones verificadas";
            this.BancosChkConciliacionesVerificadas.UseVisualStyleBackColor = true;
            // 
            // BancosChkPeriodoValido
            // 
            this.BancosChkPeriodoValido.AutoSize = true;
            this.BancosChkPeriodoValido.Font = new System.Drawing.Font("Tahoma", 10F);
            this.BancosChkPeriodoValido.ForeColor = System.Drawing.Color.Black;
            this.BancosChkPeriodoValido.Location = new System.Drawing.Point(16, 20);
            this.BancosChkPeriodoValido.Name = "BancosChkPeriodoValido";
            this.BancosChkPeriodoValido.Size = new System.Drawing.Size(223, 21);
            this.BancosChkPeriodoValido.TabIndex = 12;
            this.BancosChkPeriodoValido.Text = "Período válido y sin cierre previo";
            this.BancosChkPeriodoValido.UseVisualStyleBackColor = true;
            // 
            // BancosLblValidaciones
            // 
            this.BancosLblValidaciones.AutoSize = true;
            this.BancosLblValidaciones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(169)))), ((int)(((byte)(59)))));
            this.BancosLblValidaciones.Font = new System.Drawing.Font("Tahoma", 13F);
            this.BancosLblValidaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.BancosLblValidaciones.Location = new System.Drawing.Point(23, 416);
            this.BancosLblValidaciones.Name = "BancosLblValidaciones";
            this.BancosLblValidaciones.Size = new System.Drawing.Size(183, 22);
            this.BancosLblValidaciones.TabIndex = 11;
            this.BancosLblValidaciones.Text = "Validaciones del cierre";
            // 
            // BancosPnlBotones
            // 
            this.BancosPnlBotones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(243)))));
            this.BancosPnlBotones.Controls.Add(this.BancosBtnSalir);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnValidarCierre);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnAyuda);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnVerHistorico);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnReporte);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnCerrarPeriodo);
            this.BancosPnlBotones.Controls.Add(this.BancosBtnLimpiar);
            this.BancosPnlBotones.Location = new System.Drawing.Point(675, 433);
            this.BancosPnlBotones.Name = "BancosPnlBotones";
            this.BancosPnlBotones.Size = new System.Drawing.Size(298, 298);
            this.BancosPnlBotones.TabIndex = 14;
            // 
            // BancosBtnSalir
            // 
            this.BancosBtnSalir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BancosBtnSalir.BackgroundImage")));
            this.BancosBtnSalir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnSalir.Location = new System.Drawing.Point(105, 203);
            this.BancosBtnSalir.Name = "BancosBtnSalir";
            this.BancosBtnSalir.Size = new System.Drawing.Size(85, 84);
            this.BancosBtnSalir.TabIndex = 0;
            this.BancosBtnSalir.UseVisualStyleBackColor = true;
            // 
            // BancosBtnValidarCierre
            // 
            this.BancosBtnValidarCierre.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_validarcierreB;
            this.BancosBtnValidarCierre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnValidarCierre.Location = new System.Drawing.Point(14, 3);
            this.BancosBtnValidarCierre.Name = "BancosBtnValidarCierre";
            this.BancosBtnValidarCierre.Size = new System.Drawing.Size(85, 77);
            this.BancosBtnValidarCierre.TabIndex = 7;
            this.BancosBtnValidarCierre.UseVisualStyleBackColor = true;
            // 
            // BancosBtnAyuda
            // 
            this.BancosBtnAyuda.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_ayuda;
            this.BancosBtnAyuda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnAyuda.Location = new System.Drawing.Point(196, 104);
            this.BancosBtnAyuda.Name = "BancosBtnAyuda";
            this.BancosBtnAyuda.Size = new System.Drawing.Size(85, 84);
            this.BancosBtnAyuda.TabIndex = 12;
            this.BancosBtnAyuda.UseVisualStyleBackColor = true;
            // 
            // BancosBtnVerHistorico
            // 
            this.BancosBtnVerHistorico.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_verhistoricoB;
            this.BancosBtnVerHistorico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnVerHistorico.Location = new System.Drawing.Point(105, 104);
            this.BancosBtnVerHistorico.Name = "BancosBtnVerHistorico";
            this.BancosBtnVerHistorico.Size = new System.Drawing.Size(85, 84);
            this.BancosBtnVerHistorico.TabIndex = 10;
            this.BancosBtnVerHistorico.UseVisualStyleBackColor = true;
            // 
            // BancosBtnReporte
            // 
            this.BancosBtnReporte.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_reporteB;
            this.BancosBtnReporte.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnReporte.Location = new System.Drawing.Point(196, 3);
            this.BancosBtnReporte.Name = "BancosBtnReporte";
            this.BancosBtnReporte.Size = new System.Drawing.Size(85, 82);
            this.BancosBtnReporte.TabIndex = 13;
            this.BancosBtnReporte.UseVisualStyleBackColor = true;
            // 
            // BancosBtnCerrarPeriodo
            // 
            this.BancosBtnCerrarPeriodo.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_cerrarperiodoB;
            this.BancosBtnCerrarPeriodo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnCerrarPeriodo.Location = new System.Drawing.Point(105, 3);
            this.BancosBtnCerrarPeriodo.Name = "BancosBtnCerrarPeriodo";
            this.BancosBtnCerrarPeriodo.Size = new System.Drawing.Size(85, 82);
            this.BancosBtnCerrarPeriodo.TabIndex = 9;
            this.BancosBtnCerrarPeriodo.UseVisualStyleBackColor = true;
            // 
            // BancosBtnLimpiar
            // 
            this.BancosBtnLimpiar.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.btn_limpiarB;
            this.BancosBtnLimpiar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosBtnLimpiar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BancosBtnLimpiar.Location = new System.Drawing.Point(14, 104);
            this.BancosBtnLimpiar.Name = "BancosBtnLimpiar";
            this.BancosBtnLimpiar.Size = new System.Drawing.Size(85, 84);
            this.BancosBtnLimpiar.TabIndex = 8;
            this.BancosBtnLimpiar.UseVisualStyleBackColor = true;
            // 
            // BancosPbIconoBanco
            // 
            this.BancosPbIconoBanco.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.Bancario;
            this.BancosPbIconoBanco.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbIconoBanco.Location = new System.Drawing.Point(12, 1);
            this.BancosPbIconoBanco.Name = "BancosPbIconoBanco";
            this.BancosPbIconoBanco.Size = new System.Drawing.Size(71, 59);
            this.BancosPbIconoBanco.TabIndex = 11;
            this.BancosPbIconoBanco.TabStop = false;
            // 
            // BancosPbMascota
            // 
            this.BancosPbMascota.BackColor = System.Drawing.Color.Transparent;
            this.BancosPbMascota.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources._10;
            this.BancosPbMascota.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbMascota.Location = new System.Drawing.Point(980, 430);
            this.BancosPbMascota.Name = "BancosPbMascota";
            this.BancosPbMascota.Size = new System.Drawing.Size(194, 290);
            this.BancosPbMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BancosPbMascota.TabIndex = 6;
            this.BancosPbMascota.TabStop = false;
            // 
            // FrmCierreBancario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::CapaVista_CierreBancario.Properties.Resources.FondoAutorizacionModulo;
            this.ClientSize = new System.Drawing.Size(1202, 740);
            this.Controls.Add(this.BancosPnlBotones);
            this.Controls.Add(this.BancosPbIconoBanco);
            this.Controls.Add(this.BancosLblResumenCuentas);
            this.Controls.Add(this.BancosLblValidaciones);
            this.Controls.Add(this.BancosPbMascota);
            this.Controls.Add(this.BancosLblPeriodoCierre);
            this.Controls.Add(this.BancosPnlValidacionesCierre);
            this.Controls.Add(this.BancosPnlResumenCuentasCierre);
            this.Controls.Add(this.BancosLblTitulo);
            this.Controls.Add(this.BancosPnlPeriodoCierre);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmCierreBancario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8013-CierreBancario";
            this.BancosPnlPeriodoCierre.ResumeLayout(false);
            this.BancosPnlPeriodoCierre.PerformLayout();
            this.BancosPnlResumenCuentasCierre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvResumenCuentas)).EndInit();
            this.BancosPnlValidacionesCierre.ResumeLayout(false);
            this.BancosPnlValidacionesCierre.PerformLayout();
            this.BancosPnlBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbIconoBanco)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbMascota)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BancosBtnSalir;
        private System.Windows.Forms.Button BancosBtnConsultar;
        private System.Windows.Forms.Panel BancosPnlPeriodoCierre;
        private System.Windows.Forms.Label BancosLblFechaFinal;
        private System.Windows.Forms.Label BancosLblAlcance;
        private System.Windows.Forms.Label BancosLblFechaInicial;
        private System.Windows.Forms.Label BancosLblEstadoPeriodo;
        private System.Windows.Forms.Label BancosLblTipoCierre;
        private System.Windows.Forms.Label BancosLblPeriodoCierre;
        private System.Windows.Forms.Label BancosLblTitulo;
        private System.Windows.Forms.Panel BancosPnlResumenCuentasCierre;
        private System.Windows.Forms.DataGridView BancosDgvResumenCuentas;
        private System.Windows.Forms.Label BancosLblResumenCuentas;
        private System.Windows.Forms.Panel BancosPnlValidacionesCierre;
        private System.Windows.Forms.TextBox BancosTxtObservaciones;
        private System.Windows.Forms.Label BancosLblObservaciones;
        private System.Windows.Forms.CheckBox BancosChkSaldosVerificados;
        private System.Windows.Forms.CheckBox BancosChkConciliacionesVerificadas;
        private System.Windows.Forms.CheckBox BancosChkPeriodoValido;
        private System.Windows.Forms.Label BancosLblValidaciones;
        private System.Windows.Forms.PictureBox BancosPbMascota;
        private System.Windows.Forms.Button BancosBtnValidarCierre;
        private System.Windows.Forms.Button BancosBtnLimpiar;
        private System.Windows.Forms.Button BancosBtnCerrarPeriodo;
        private System.Windows.Forms.Button BancosBtnVerHistorico;
        private System.Windows.Forms.PictureBox BancosPbIconoBanco;
        private System.Windows.Forms.Label BancosTxtResultadoValidacionesCierre;
        private System.Windows.Forms.TextBox BancosTxFechaFinal;
        private System.Windows.Forms.TextBox BancosTxtAlcance;
        private System.Windows.Forms.TextBox BancosTxtFechaInicial;
        private System.Windows.Forms.TextBox BancosTxtEstadoPeriodo;
        private System.Windows.Forms.Label BancosTxtAdvertencia;
        private System.Windows.Forms.ComboBox BancosCboTipoCierre;
        private System.Windows.Forms.Button BancosBtnAyuda;
        private System.Windows.Forms.Button BancosBtnReporte;
        private System.Windows.Forms.Panel BancosPnlBotones;
    }
}