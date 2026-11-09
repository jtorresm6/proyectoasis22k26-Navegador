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
            this.BancosPnlListaMov = new System.Windows.Forms.Panel();
            this.BancosPnlTitulo = new System.Windows.Forms.Panel();
            this.BancosPbButti = new System.Windows.Forms.PictureBox();
            this.BancosPbLogo = new System.Windows.Forms.PictureBox();
            this.SeguridadLbl2Modulo = new System.Windows.Forms.Label();
            this.SeguridadLbl1Modulo = new System.Windows.Forms.Label();
            this.BancosDgvMovimientos = new System.Windows.Forms.DataGridView();
            this.BancosPnlListaMov.SuspendLayout();
            this.BancosPnlTitulo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbButti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosPnlListaMov
            // 
            this.BancosPnlListaMov.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlListaMov.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_contenedor1;
            this.BancosPnlListaMov.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlListaMov.Controls.Add(this.BancosPnlTitulo);
            this.BancosPnlListaMov.Controls.Add(this.BancosDgvMovimientos);
            this.BancosPnlListaMov.Location = new System.Drawing.Point(0, 0);
            this.BancosPnlListaMov.Name = "BancosPnlListaMov";
            this.BancosPnlListaMov.Size = new System.Drawing.Size(737, 608);
            this.BancosPnlListaMov.TabIndex = 2;
            // 
            // BancosPnlTitulo
            // 
            this.BancosPnlTitulo.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_titulo;
            this.BancosPnlTitulo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlTitulo.Controls.Add(this.BancosPbButti);
            this.BancosPnlTitulo.Controls.Add(this.BancosPbLogo);
            this.BancosPnlTitulo.Controls.Add(this.SeguridadLbl2Modulo);
            this.BancosPnlTitulo.Controls.Add(this.SeguridadLbl1Modulo);
            this.BancosPnlTitulo.Location = new System.Drawing.Point(25, 11);
            this.BancosPnlTitulo.Margin = new System.Windows.Forms.Padding(2);
            this.BancosPnlTitulo.Name = "BancosPnlTitulo";
            this.BancosPnlTitulo.Size = new System.Drawing.Size(688, 128);
            this.BancosPnlTitulo.TabIndex = 9;
            // 
            // BancosPbButti
            // 
            this.BancosPbButti.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.butti31;
            this.BancosPbButti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbButti.Location = new System.Drawing.Point(505, 7);
            this.BancosPbButti.Margin = new System.Windows.Forms.Padding(2);
            this.BancosPbButti.Name = "BancosPbButti";
            this.BancosPbButti.Size = new System.Drawing.Size(96, 117);
            this.BancosPbButti.TabIndex = 30;
            this.BancosPbButti.TabStop = false;
            // 
            // BancosPbLogo
            // 
            this.BancosPbLogo.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_movbac;
            this.BancosPbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPbLogo.Location = new System.Drawing.Point(25, 14);
            this.BancosPbLogo.Margin = new System.Windows.Forms.Padding(2);
            this.BancosPbLogo.Name = "BancosPbLogo";
            this.BancosPbLogo.Size = new System.Drawing.Size(96, 99);
            this.BancosPbLogo.TabIndex = 29;
            this.BancosPbLogo.TabStop = false;
            // 
            // SeguridadLbl2Modulo
            // 
            this.SeguridadLbl2Modulo.AutoSize = true;
            this.SeguridadLbl2Modulo.BackColor = System.Drawing.Color.Transparent;
            this.SeguridadLbl2Modulo.Font = new System.Drawing.Font("Tahoma", 10F);
            this.SeguridadLbl2Modulo.ForeColor = System.Drawing.Color.DimGray;
            this.SeguridadLbl2Modulo.Location = new System.Drawing.Point(136, 72);
            this.SeguridadLbl2Modulo.Name = "SeguridadLbl2Modulo";
            this.SeguridadLbl2Modulo.Size = new System.Drawing.Size(347, 17);
            this.SeguridadLbl2Modulo.TabIndex = 28;
            this.SeguridadLbl2Modulo.Text = "Lista de todos los movimientos registrados al momento.";
            // 
            // SeguridadLbl1Modulo
            // 
            this.SeguridadLbl1Modulo.AutoSize = true;
            this.SeguridadLbl1Modulo.BackColor = System.Drawing.Color.Transparent;
            this.SeguridadLbl1Modulo.Font = new System.Drawing.Font("Lucida Bright", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.SeguridadLbl1Modulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(74)))), ((int)(((byte)(48)))));
            this.SeguridadLbl1Modulo.Location = new System.Drawing.Point(135, 41);
            this.SeguridadLbl1Modulo.Name = "SeguridadLbl1Modulo";
            this.SeguridadLbl1Modulo.Size = new System.Drawing.Size(288, 28);
            this.SeguridadLbl1Modulo.TabIndex = 27;
            this.SeguridadLbl1Modulo.Text = "Movimientos Bancarios";
            // 
            // BancosDgvMovimientos
            // 
            this.BancosDgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BancosDgvMovimientos.Location = new System.Drawing.Point(42, 151);
            this.BancosDgvMovimientos.Name = "BancosDgvMovimientos";
            this.BancosDgvMovimientos.Size = new System.Drawing.Size(653, 427);
            this.BancosDgvMovimientos.TabIndex = 0;
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
            this.BancosPnlListaMov.ResumeLayout(false);
            this.BancosPnlTitulo.ResumeLayout(false);
            this.BancosPnlTitulo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbButti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosPbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgvMovimientos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView BancosDgvMovimientos;
        private System.Windows.Forms.Panel BancosPnlListaMov;
        private System.Windows.Forms.Panel BancosPnlTitulo;
        private System.Windows.Forms.PictureBox BancosPbButti;
        private System.Windows.Forms.PictureBox BancosPbLogo;
        private System.Windows.Forms.Label SeguridadLbl2Modulo;
        private System.Windows.Forms.Label SeguridadLbl1Modulo;
    }
}