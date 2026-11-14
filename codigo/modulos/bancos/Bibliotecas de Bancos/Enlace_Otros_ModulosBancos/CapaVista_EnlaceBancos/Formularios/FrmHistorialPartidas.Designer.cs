namespace CapaVista_EnlaceB.Formularios
{
    partial class FrmHistorialPartidas
    {
        private System.Windows.Forms.Label _BancarioLblTitulo;
        private System.Windows.Forms.DataGridView _BancarioDgvPartidas;
        private System.Windows.Forms.Button _BancarioBtnAnterior;
        private System.Windows.Forms.Button _BancarioBtnSiguiente;
        private System.Windows.Forms.Label _BancarioLblPagina;
        private System.Windows.Forms.Button _BancarioBtnSeleccionar;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHistorialPartidas));
            this._BancarioLblTitulo = new System.Windows.Forms.Label();
            this._BancarioDgvPartidas = new System.Windows.Forms.DataGridView();
            this._BancarioBtnAnterior = new System.Windows.Forms.Button();
            this._BancarioBtnSiguiente = new System.Windows.Forms.Button();
            this._BancarioLblPagina = new System.Windows.Forms.Label();
            this._BancarioBtnSeleccionar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this._BancarioLblTitulo.Name = "_BancarioLblTitulo";
            this._BancarioLblTitulo.Location = new System.Drawing.Point(20, 5);
            this._BancarioLblTitulo.Size = new System.Drawing.Size(800, 57);
            this._BancarioLblTitulo.TabIndex = 1;
            this._BancarioLblTitulo.Text = "Historial de partidas";
            this._BancarioLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblTitulo.Font = new System.Drawing.Font("Lucida Bright", 22F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this._BancarioLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioDgvPartidas.Name = "_BancarioDgvPartidas";
            this._BancarioDgvPartidas.Location = new System.Drawing.Point(20, 70);
            this._BancarioDgvPartidas.Size = new System.Drawing.Size(1010, 310);
            this._BancarioDgvPartidas.TabIndex = 2;
            this._BancarioDgvPartidas.BackgroundColor = System.Drawing.Color.White;
            this._BancarioDgvPartidas.ReadOnly = true;
            this._BancarioDgvPartidas.AllowUserToAddRows = false;
            this._BancarioDgvPartidas.RowHeadersVisible = false;
            this._BancarioBtnAnterior.Name = "_BancarioBtnAnterior";
            this._BancarioBtnAnterior.Location = new System.Drawing.Point(20, 382);
            this._BancarioBtnAnterior.Size = new System.Drawing.Size(90, 27);
            this._BancarioBtnAnterior.TabIndex = 3;
            this._BancarioBtnAnterior.Text = "Anterior";
            this._BancarioBtnAnterior.BackColor = System.Drawing.Color.FromArgb(212, 231, 211);
            this._BancarioBtnSiguiente.Name = "_BancarioBtnSiguiente";
            this._BancarioBtnSiguiente.Location = new System.Drawing.Point(114, 382);
            this._BancarioBtnSiguiente.Size = new System.Drawing.Size(90, 27);
            this._BancarioBtnSiguiente.TabIndex = 4;
            this._BancarioBtnSiguiente.Text = "Siguiente";
            this._BancarioBtnSiguiente.BackColor = System.Drawing.Color.FromArgb(212, 231, 211);
            this._BancarioLblPagina.Name = "_BancarioLblPagina";
            this._BancarioLblPagina.Location = new System.Drawing.Point(213, 382);
            this._BancarioLblPagina.Size = new System.Drawing.Size(817, 27);
            this._BancarioLblPagina.TabIndex = 5;
            this._BancarioLblPagina.Text = "";
            this._BancarioLblPagina.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblPagina.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
            this._BancarioLblPagina.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioBtnSeleccionar.Name = "_BancarioBtnSeleccionar";
            this._BancarioBtnSeleccionar.Location = new System.Drawing.Point(936, 429);
            this._BancarioBtnSeleccionar.Size = new System.Drawing.Size(90, 90);
            this._BancarioBtnSeleccionar.TabIndex = 6;
            this._BancarioBtnSeleccionar.AccessibleName = "Seleccionar";
            this._BancarioBtnSeleccionar.BackColor = System.Drawing.Color.FromArgb(28, 74, 48);
            this._BancarioBtnSeleccionar.ForeColor = System.Drawing.Color.White;
            this._BancarioBtnSeleccionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._BancarioBtnSeleccionar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(210, 169, 59);
            this._BancarioBtnSeleccionar.FlatAppearance.BorderSize = 2;
            this._BancarioBtnSeleccionar.Text = "Seleccionar";
            this._BancarioBtnSeleccionar.Image = ((System.Drawing.Image)(resources.GetObject("IconoSeleccionar")));
            this._BancarioBtnSeleccionar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this._BancarioBtnSeleccionar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this._BancarioBtnSeleccionar.UseVisualStyleBackColor = false;
            this._BancarioBtnSeleccionar.Click += new System.EventHandler(this.BancarioMetSeleccionar);
            this.Controls.Add(this._BancarioLblTitulo);
            this.Controls.Add(this._BancarioDgvPartidas);
            this.Controls.Add(this._BancarioBtnAnterior);
            this.Controls.Add(this._BancarioBtnSiguiente);
            this.Controls.Add(this._BancarioLblPagina);
            this.Controls.Add(this._BancarioBtnSeleccionar);
            this.Name = "FrmHistorialPartidas";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new System.Drawing.Size(1050, 530);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.ForeColor = System.Drawing.Color.FromArgb(28, 74, 48);
            this.Text = "8008 – HistorialPartidas";
            this.ClientSize = new System.Drawing.Size(1050, 530);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.BackgroundImage = global::CapaVista_EnlaceB.Properties.Resources.FondoEnlaceotrosModulo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.AutoScroll = true;
            this.ResumeLayout(false);
        }
    }
}
