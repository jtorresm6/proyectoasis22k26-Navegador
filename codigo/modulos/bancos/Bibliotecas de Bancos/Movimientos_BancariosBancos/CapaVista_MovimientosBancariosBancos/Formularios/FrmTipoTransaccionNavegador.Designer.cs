namespace CapaVista_MovimientosBancarios.Formularios
{
    partial class FrmTipoTransaccionNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTipoTransaccionNavegador));
            this.BancosNavTipoTransaccion = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // BancosNavTipoTransaccion
            // 
            this.BancosNavTipoTransaccion.Location = new System.Drawing.Point(11, 11);
            this.BancosNavTipoTransaccion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosNavTipoTransaccion.Name = "BancosNavTipoTransaccion";
            this.BancosNavTipoTransaccion.Size = new System.Drawing.Size(1078, 90);
            this.BancosNavTipoTransaccion.TabIndex = 0;
            // 
            // FrmTipoTransaccionNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1089, 609);
            this.Controls.Add(this.BancosNavTipoTransaccion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmTipoTransaccionNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8004-MantenimientoTipoTransaccion";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador BancosNavTipoTransaccion;
    }
}