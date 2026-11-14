namespace CapaVista_AutorizacionOrdenes.Formularios
{
    partial class FrmTipoCuentaBancariaNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTipoCuentaBancariaNavegador));
            this.BancosNavTipoCuenta = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // BancosNavTipoCuenta
            // 
            this.BancosNavTipoCuenta.Location = new System.Drawing.Point(12, 12);
            this.BancosNavTipoCuenta.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BancosNavTipoCuenta.Name = "BancosNavTipoCuenta";
            this.BancosNavTipoCuenta.Size = new System.Drawing.Size(1437, 108);
            this.BancosNavTipoCuenta.TabIndex = 0;
            // 
            // FrmTipoCuentaBancariaNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1447, 894);
            this.Controls.Add(this.BancosNavTipoCuenta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "FrmTipoCuentaBancariaNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8003–MantenimientoTipoCuentaBancaria";
            this.Load += new System.EventHandler(this.FrmTipoCuentaBancariaNavegador_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador BancosNavTipoCuenta;
    }
}