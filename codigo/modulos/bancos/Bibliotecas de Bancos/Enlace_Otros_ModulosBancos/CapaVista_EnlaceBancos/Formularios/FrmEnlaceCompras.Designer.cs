namespace CapaVista_EnlaceB.Formularios
{
    partial class FrmEnlaceCompras
    {
        private System.Windows.Forms.Label _BancarioLblTitulo;
        private System.Windows.Forms.PictureBox _BancarioPbBalanza;
        private System.Windows.Forms.Label _BancarioLblNota;
        private global::CapaVista_EnlaceB.Formularios.UsrConsultaModulo _BancarioConsulta;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEnlaceCompras));
            this._BancarioLblTitulo = new System.Windows.Forms.Label();
            this._BancarioPbBalanza = new System.Windows.Forms.PictureBox();
            this._BancarioLblNota = new System.Windows.Forms.Label();
            this._BancarioConsulta = new global::CapaVista_EnlaceB.Formularios.UsrConsultaModulo();
            this.SuspendLayout();
            this._BancarioLblTitulo.Name = "_BancarioLblTitulo";
            this._BancarioLblTitulo.Location = new System.Drawing.Point(22, 16);
            this._BancarioLblTitulo.Size = new System.Drawing.Size(920, 68);
            this._BancarioLblTitulo.TabIndex = 1;
            this._BancarioLblTitulo.Text = "Enlace con Compras";
            this._BancarioLblTitulo.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblTitulo.Font = new System.Drawing.Font("Lucida Bright", 24F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this._BancarioLblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioPbBalanza.Name = "_BancarioPbBalanza";
            this._BancarioPbBalanza.Location = new System.Drawing.Point(1022, 14);
            this._BancarioPbBalanza.Size = new System.Drawing.Size(70, 70);
            this._BancarioPbBalanza.TabIndex = 2;
            this._BancarioPbBalanza.Image = global::CapaVista_EnlaceB.Properties.Resources.BalanzaOro;
            this._BancarioPbBalanza.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._BancarioPbBalanza.BackColor = System.Drawing.Color.Transparent;
            this._BancarioPbBalanza.TabStop = false;
            this._BancarioLblNota.Name = "_BancarioLblNota";
            this._BancarioLblNota.Location = new System.Drawing.Point(22, 84);
            this._BancarioLblNota.Size = new System.Drawing.Size(1076, 35);
            this._BancarioLblNota.TabIndex = 3;
            this._BancarioLblNota.Text = "Fecha de emisión de la orden de compra. La recepción bancaria usa una referencia externa.";
            this._BancarioLblNota.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblNota.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this._BancarioLblNota.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioConsulta.Name = "_BancarioConsulta";
            this._BancarioConsulta.Location = new System.Drawing.Point(20, 122);
            this._BancarioConsulta.Size = new System.Drawing.Size(1080, 550);
            this._BancarioConsulta.TabIndex = 4;
            this.Controls.Add(this._BancarioLblTitulo);
            this.Controls.Add(this._BancarioPbBalanza);
            this.Controls.Add(this._BancarioLblNota);
            this.Controls.Add(this._BancarioConsulta);
            this.Name = "FrmEnlaceCompras";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new System.Drawing.Size(1120, 680);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.ForeColor = System.Drawing.Color.FromArgb(28, 74, 48);
            this.Text = "8008 – EnlaceCompras";
            this.ClientSize = new System.Drawing.Size(1120, 680);
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
