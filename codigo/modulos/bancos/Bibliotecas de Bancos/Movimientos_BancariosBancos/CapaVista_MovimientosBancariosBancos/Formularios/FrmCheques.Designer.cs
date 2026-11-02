namespace CapaVista_MovimientosBancarios.Formularios
{
    partial class FrmCheques
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCheques));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.BancosPnlCheques = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.BancosPnlCheques.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(389, 340);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 20);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "prueba";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // BancosPnlCheques
            // 
            this.BancosPnlCheques.BackColor = System.Drawing.Color.Transparent;
            this.BancosPnlCheques.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_contenedor1;
            this.BancosPnlCheques.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPnlCheques.Controls.Add(this.button1);
            this.BancosPnlCheques.Controls.Add(this.checkBox1);
            this.BancosPnlCheques.Location = new System.Drawing.Point(0, 0);
            this.BancosPnlCheques.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BancosPnlCheques.Name = "BancosPnlCheques";
            this.BancosPnlCheques.Size = new System.Drawing.Size(983, 748);
            this.BancosPnlCheques.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(396, 396);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "prueba";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FrmCheques
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 748);
            this.Controls.Add(this.BancosPnlCheques);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmCheques";
            this.Text = "Generación de Cheques";
            this.BancosPnlCheques.ResumeLayout(false);
            this.BancosPnlCheques.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel BancosPnlCheques;
        private System.Windows.Forms.Button button1;
    }
}