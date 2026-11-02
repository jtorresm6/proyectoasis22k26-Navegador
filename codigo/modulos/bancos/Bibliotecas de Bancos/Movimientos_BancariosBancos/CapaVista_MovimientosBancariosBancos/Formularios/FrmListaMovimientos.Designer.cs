namespace CapaVista_MovimientosBancarios.Formularios
{
    partial class FrmListaMovimientos
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
            this.BancosDgvMovimientos = new System.Windows.Forms.DataGridView();
            this.BancosPnlListaMov = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).BeginInit();
            this.BancosPnlListaMov.SuspendLayout();
            this.SuspendLayout();
            // 
            // BancosDgvMovimientos
            // 
            this.BancosDgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BancosDgvMovimientos.Location = new System.Drawing.Point(40, 151);
            this.BancosDgvMovimientos.Name = "BancosDgvMovimientos";
            this.BancosDgvMovimientos.Size = new System.Drawing.Size(653, 427);
            this.BancosDgvMovimientos.TabIndex = 0;
            // 
            // BancosPnlListaMov
            // 
            this.BancosPnlListaMov.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlListaMov.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_contenedor1;
            this.BancosPnlListaMov.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlListaMov.Controls.Add(this.BancosDgvMovimientos);
            this.BancosPnlListaMov.Location = new System.Drawing.Point(0, 0);
            this.BancosPnlListaMov.Name = "BancosPnlListaMov";
            this.BancosPnlListaMov.Size = new System.Drawing.Size(737, 608);
            this.BancosPnlListaMov.TabIndex = 2;
            // 
            // FrmListaMovimientos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 608);
            this.Controls.Add(this.BancosPnlListaMov);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmListaMovimientos";
            this.Text = "Lista de Movimientos";
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).EndInit();
            this.BancosPnlListaMov.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView BancosDgvMovimientos;
        private System.Windows.Forms.Panel BancosPnlListaMov;
    }
}