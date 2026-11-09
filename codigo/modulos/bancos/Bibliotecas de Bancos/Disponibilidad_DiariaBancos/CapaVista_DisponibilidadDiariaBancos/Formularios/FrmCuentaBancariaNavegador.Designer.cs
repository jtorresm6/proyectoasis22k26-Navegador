namespace CapaVista_DisponibilidadDiaria.Formularios
{
    partial class FrmCuentaBancariaNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCuentaBancariaNavegador));
            this.BancosNavCuentaBancaria = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // BancosNavCuentaBancaria
            // 
            this.BancosNavCuentaBancaria.Location = new System.Drawing.Point(2, 19);
            this.BancosNavCuentaBancaria.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosNavCuentaBancaria.Name = "BancosNavCuentaBancaria";
            this.BancosNavCuentaBancaria.Size = new System.Drawing.Size(1081, 88);
            this.BancosNavCuentaBancaria.TabIndex = 0;
            // 
            // FrmCuentaBancariaNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1077, 609);
            this.Controls.Add(this.BancosNavCuentaBancaria);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "FrmCuentaBancariaNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8005-MantenimientoCuentaBancaria";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador BancosNavCuentaBancaria;
    }
}