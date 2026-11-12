namespace CapaVista_Mantenimientos
{
    partial class FrmMantenimientosVentas
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && components != null) components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.VentasPnlCabecera = new System.Windows.Forms.Panel();
            this.VentasLblTitulo = new System.Windows.Forms.Label();
            this.VentasLblSubtitulo = new System.Windows.Forms.Label();
            this.VentasLblCodigo = new System.Windows.Forms.Label();
            this.VentasUsrNavegador = new CapaVista_Navegador.Navegador();
            this.VentasPnlCabecera.SuspendLayout();
            this.SuspendLayout();

            this.VentasPnlCabecera.BackColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasPnlCabecera.Controls.Add(this.VentasLblTitulo);
            this.VentasPnlCabecera.Controls.Add(this.VentasLblSubtitulo);
            this.VentasPnlCabecera.Controls.Add(this.VentasLblCodigo);
            this.VentasPnlCabecera.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasPnlCabecera.Size = new System.Drawing.Size(1480, 86);
            this.VentasPnlCabecera.Name = "VentasPnlCabecera";

            this.VentasLblTitulo.Font = new System.Drawing.Font("Lucida Fax", 18F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasLblTitulo.SetBounds(24, 16, 1000, 32);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Text = "Mantenimientos";

            this.VentasLblSubtitulo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.VentasLblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.VentasLblSubtitulo.SetBounds(26, 56, 1000, 20);
            this.VentasLblSubtitulo.Name = "VentasLblSubtitulo";
            this.VentasLblSubtitulo.Text = "Catálogos de Ventas y cuentas por cobrar";

            this.VentasLblCodigo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.VentasLblCodigo.BackColor = System.Drawing.Color.FromArgb(201, 162, 74);
            this.VentasLblCodigo.ForeColor = System.Drawing.Color.FromArgb(59, 29, 74);
            this.VentasLblCodigo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.VentasLblCodigo.SetBounds(1314, 27, 140, 28);
            this.VentasLblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VentasLblCodigo.Name = "VentasLblCodigo";
            this.VentasLblCodigo.Text = "7009";

            this.VentasUsrNavegador.Dock = System.Windows.Forms.DockStyle.Top;
            this.VentasUsrNavegador.Height = 111;
            this.VentasUsrNavegador.Name = "VentasUsrNavegador";
            this.VentasUsrNavegador.TabIndex = 0;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(248, 245, 249);
            this.ClientSize = new System.Drawing.Size(1480, 620);
            this.Controls.Add(this.VentasUsrNavegador);
            this.Controls.Add(this.VentasPnlCabecera);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::CapaVista_Mantenimientos.Properties.Resources.IconoVentas;
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientosVentas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "7009 – Mantenimientos";
            this.VentasPnlCabecera.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel VentasPnlCabecera;
        private System.Windows.Forms.Label VentasLblTitulo;
        private System.Windows.Forms.Label VentasLblSubtitulo;
        private System.Windows.Forms.Label VentasLblCodigo;
        private CapaVista_Navegador.Navegador VentasUsrNavegador;
    }
}
