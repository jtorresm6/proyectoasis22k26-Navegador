namespace CapaVista_MovimientosBancarios.Formularios
{
    partial class FrmSeleccionarBeneficiario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSeleccionarBeneficiario));
            this.BancosPblBeneficiario = new System.Windows.Forms.Panel();
            this.BancoTxtBeneficiarioBuscar = new System.Windows.Forms.TextBox();
            this.BancosLblBeneficiarioFiltrar = new System.Windows.Forms.Label();
            this.BancosDgbBeneficiarioBuscar = new System.Windows.Forms.DataGridView();
            this.BancosPblBeneficiario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgbBeneficiarioBuscar)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosPblBeneficiario
            // 
            this.BancosPblBeneficiario.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_contenedor;
            this.BancosPblBeneficiario.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPblBeneficiario.Controls.Add(this.BancoTxtBeneficiarioBuscar);
            this.BancosPblBeneficiario.Controls.Add(this.BancosLblBeneficiarioFiltrar);
            this.BancosPblBeneficiario.Controls.Add(this.BancosDgbBeneficiarioBuscar);
            this.BancosPblBeneficiario.Location = new System.Drawing.Point(1, 1);
            this.BancosPblBeneficiario.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosPblBeneficiario.Name = "BancosPblBeneficiario";
            this.BancosPblBeneficiario.Size = new System.Drawing.Size(799, 449);
            this.BancosPblBeneficiario.TabIndex = 0;
            // 
            // BancoTxtBeneficiarioBuscar
            // 
            this.BancoTxtBeneficiarioBuscar.Location = new System.Drawing.Point(356, 22);
            this.BancoTxtBeneficiarioBuscar.Name = "BancoTxtBeneficiarioBuscar";
            this.BancoTxtBeneficiarioBuscar.Size = new System.Drawing.Size(431, 20);
            this.BancoTxtBeneficiarioBuscar.TabIndex = 4;
            this.BancoTxtBeneficiarioBuscar.TextChanged += new System.EventHandler(this.BancoTxtBeneficiarioBuscar_TextChanged);
            // 
            // BancosLblBeneficiarioFiltrar
            // 
            this.BancosLblBeneficiarioFiltrar.AutoSize = true;
            this.BancosLblBeneficiarioFiltrar.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblBeneficiarioFiltrar.Font = new System.Drawing.Font("Lucida Fax", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblBeneficiarioFiltrar.Location = new System.Drawing.Point(12, 22);
            this.BancosLblBeneficiarioFiltrar.Name = "BancosLblBeneficiarioFiltrar";
            this.BancosLblBeneficiarioFiltrar.Size = new System.Drawing.Size(343, 16);
            this.BancosLblBeneficiarioFiltrar.TabIndex = 3;
            this.BancosLblBeneficiarioFiltrar.Text = "Ingrese un Beneficiario ( Proveedor ) a Filtrar";
            // 
            // BancosDgbBeneficiarioBuscar
            // 
            this.BancosDgbBeneficiarioBuscar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BancosDgbBeneficiarioBuscar.Location = new System.Drawing.Point(9, 67);
            this.BancosDgbBeneficiarioBuscar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BancosDgbBeneficiarioBuscar.Name = "BancosDgbBeneficiarioBuscar";
            this.BancosDgbBeneficiarioBuscar.RowTemplate.Height = 24;
            this.BancosDgbBeneficiarioBuscar.Size = new System.Drawing.Size(778, 355);
            this.BancosDgbBeneficiarioBuscar.TabIndex = 0;
            this.BancosDgbBeneficiarioBuscar.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BancosDgbBeneficiarioBuscar_CellContentDoubleClick);
            // 
            // FrmSeleccionarBeneficiario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BancosPblBeneficiario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmSeleccionarBeneficiario";
            this.Text = "8006.2-Beneficiario";
            this.Load += new System.EventHandler(this.FrmSeleccionarBeneficiario_Load);
            this.BancosPblBeneficiario.ResumeLayout(false);
            this.BancosPblBeneficiario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgbBeneficiarioBuscar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BancosPblBeneficiario;
        private System.Windows.Forms.DataGridView BancosDgbBeneficiarioBuscar;
        private System.Windows.Forms.Label BancosLblBeneficiarioFiltrar;
        private System.Windows.Forms.TextBox BancoTxtBeneficiarioBuscar;
    }
}