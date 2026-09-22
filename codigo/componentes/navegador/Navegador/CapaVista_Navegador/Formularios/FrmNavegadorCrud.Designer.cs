// Inicio cambio - Gabriel André Guillén Pocón - 0901-23-1998
namespace CapaVista_Navegador
{
    partial class FrmNavegadorCrud
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.navegador1 = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            //
            // navegador1
            //
            this.navegador1.Location = new System.Drawing.Point(0, 0);
            this.navegador1.Name = "navegador1";
            this.navegador1.Size = new System.Drawing.Size(1438, 111);
            this.navegador1.TabIndex = 0;
            //
            // FrmNavegadorCrud
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(233)))), ((int)(((byte)(217)))));
            this.ClientSize = new System.Drawing.Size(1480, 653);
            this.Controls.Add(this.navegador1);
            this.Name = "FrmNavegadorCrud";
            this.Text = "1001 – Crud";
            this.ResumeLayout(false);

        }

        #endregion

        protected Navegador navegador1;
    }
}
// Fin cambio - Gabriel André Guillén Pocón - 0901-23-1998
