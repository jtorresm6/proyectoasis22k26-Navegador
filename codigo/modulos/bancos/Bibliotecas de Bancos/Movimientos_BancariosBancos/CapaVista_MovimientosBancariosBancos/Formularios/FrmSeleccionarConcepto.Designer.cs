namespace CapaVista_MovimientosBancarios.Formularios
{
    partial class FrmSeleccionarConcepto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSeleccionarConcepto));
            this.BancosDgbConceptoBuscar = new System.Windows.Forms.DataGridView();
            this.BancoTxtConceptoBuscar = new System.Windows.Forms.TextBox();
            this.BancosLblConceptoFiltrar = new System.Windows.Forms.Label();
            this.BancosPblConcepto = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgbConceptoBuscar)).BeginInit();
            this.SuspendLayout();
            // 
            // BancosDgbConceptoBuscar
            // 
            this.BancosDgbConceptoBuscar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BancosDgbConceptoBuscar.Location = new System.Drawing.Point(9, 67);
            this.BancosDgbConceptoBuscar.Name = "BancosDgbConceptoBuscar";
            this.BancosDgbConceptoBuscar.RowHeadersWidth = 51;
            this.BancosDgbConceptoBuscar.Size = new System.Drawing.Size(778, 355);
            this.BancosDgbConceptoBuscar.TabIndex = 0;
            this.BancosDgbConceptoBuscar.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BancosDgbConceptoBuscar_CellDoubleClick);
            // 
            // BancoTxtConceptoBuscar
            // 
            this.BancoTxtConceptoBuscar.Location = new System.Drawing.Point(305, 22);
            this.BancoTxtConceptoBuscar.Name = "BancoTxtConceptoBuscar";
            this.BancoTxtConceptoBuscar.Size = new System.Drawing.Size(482, 20);
            this.BancoTxtConceptoBuscar.TabIndex = 1;
            this.BancoTxtConceptoBuscar.TextChanged += new System.EventHandler(this.BancoTxtConceptoBuscar_TextChanged);
            // 
            // BancosLblConceptoFiltrar
            // 
            this.BancosLblConceptoFiltrar.AutoSize = true;
            this.BancosLblConceptoFiltrar.BackColor = System.Drawing.Color.Transparent;
            this.BancosLblConceptoFiltrar.Font = new System.Drawing.Font("Lucida Fax", 10F, System.Drawing.FontStyle.Bold);
            this.BancosLblConceptoFiltrar.Location = new System.Drawing.Point(12, 22);
            this.BancosLblConceptoFiltrar.Name = "BancosLblConceptoFiltrar";
            this.BancosLblConceptoFiltrar.Size = new System.Drawing.Size(225, 16);
            this.BancosLblConceptoFiltrar.TabIndex = 2;
            this.BancosLblConceptoFiltrar.Text = "Ingrese un Concepto a Filtrar";
            // 
            // BancosPblConcepto
            // 
            this.BancosPblConcepto.BackColor = System.Drawing.Color.Transparent;
            this.BancosPblConcepto.BackgroundImage = global::CapaVista_MovimientosBancarios.Properties.Resources.movbac_contenedor;
            this.BancosPblConcepto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BancosPblConcepto.Location = new System.Drawing.Point(0, 0);
            this.BancosPblConcepto.Name = "BancosPblConcepto";
            this.BancosPblConcepto.Size = new System.Drawing.Size(799, 449);
            this.BancosPblConcepto.TabIndex = 3;
            // 
            // FrmSeleccionarConcepto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BancosLblConceptoFiltrar);
            this.Controls.Add(this.BancoTxtConceptoBuscar);
            this.Controls.Add(this.BancosDgbConceptoBuscar);
            this.Controls.Add(this.BancosPblConcepto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSeleccionarConcepto";
            this.Text = "8006.1-ConceptoBancario";
            ((System.ComponentModel.ISupportInitialize)(this.BancosDgbConceptoBuscar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView BancosDgbConceptoBuscar;
        private System.Windows.Forms.TextBox BancoTxtConceptoBuscar;
        private System.Windows.Forms.Label BancosLblConceptoFiltrar;
        private System.Windows.Forms.Panel BancosPblConcepto;
    }
}