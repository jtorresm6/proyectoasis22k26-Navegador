namespace CapaVista_EnlaceB.Formularios
{
    partial class FrmOperacionCompletada
    {
        private System.Windows.Forms.Label _BancarioLblMarca;
        private System.Windows.Forms.Label _BancarioLblMensaje;
        private System.Windows.Forms.Button _BancarioBtnAceptar;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOperacionCompletada));
            this._BancarioLblMarca = new System.Windows.Forms.Label();
            this._BancarioLblMensaje = new System.Windows.Forms.Label();
            this._BancarioBtnAceptar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this._BancarioLblMarca.Name = "_BancarioLblMarca";
            this._BancarioLblMarca.Location = new System.Drawing.Point(16, 12);
            this._BancarioLblMarca.Size = new System.Drawing.Size(56, 75);
            this._BancarioLblMarca.TabIndex = 1;
            this._BancarioLblMarca.Text = "✓";
            this._BancarioLblMarca.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblMarca.Font = new System.Drawing.Font("Tahoma", 30F, System.Drawing.FontStyle.Regular);
            this._BancarioLblMarca.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioLblMarca.ForeColor = System.Drawing.Color.ForestGreen;
            this._BancarioLblMarca.Font = new System.Drawing.Font("Segoe UI Symbol", 30F);
            this._BancarioLblMensaje.Name = "_BancarioLblMensaje";
            this._BancarioLblMensaje.Location = new System.Drawing.Point(82, 13);
            this._BancarioLblMensaje.Size = new System.Drawing.Size(326, 82);
            this._BancarioLblMensaje.TabIndex = 2;
            this._BancarioLblMensaje.Text = "";
            this._BancarioLblMensaje.BackColor = System.Drawing.Color.Transparent;
            this._BancarioLblMensaje.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this._BancarioLblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._BancarioBtnAceptar.Name = "_BancarioBtnAceptar";
            this._BancarioBtnAceptar.Location = new System.Drawing.Point(310, 110);
            this._BancarioBtnAceptar.Size = new System.Drawing.Size(100, 30);
            this._BancarioBtnAceptar.TabIndex = 3;
            this._BancarioBtnAceptar.Text = "Aceptar";
            this._BancarioBtnAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._BancarioBtnAceptar.BackColor = System.Drawing.Color.FromArgb(212, 231, 211);
            this.AcceptButton = this._BancarioBtnAceptar;
            this.Controls.Add(this._BancarioLblMarca);
            this.Controls.Add(this._BancarioLblMensaje);
            this.Controls.Add(this._BancarioBtnAceptar);
            this.Name = "FrmOperacionCompletada";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Size = new System.Drawing.Size(430, 155);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.ForeColor = System.Drawing.Color.FromArgb(28, 74, 48);
            this.Text = "8008 – OperacionCompletada";
            this.ClientSize = new System.Drawing.Size(430, 155);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.BackColor = System.Drawing.Color.FromArgb(245, 248, 243);
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

