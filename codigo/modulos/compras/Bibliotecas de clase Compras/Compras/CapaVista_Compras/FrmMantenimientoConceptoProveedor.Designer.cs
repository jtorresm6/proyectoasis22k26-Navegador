namespace CapaVista_Compras
{
    partial class FrmMantenimientoConceptoProveedor
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
            this.ComprasUsrMantenimiento = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // ComprasUsrMantenimiento
            // 
            this.ComprasUsrMantenimiento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasUsrMantenimiento.Location = new System.Drawing.Point(0, 0);
            this.ComprasUsrMantenimiento.Name = "ComprasUsrMantenimiento";
            this.ComprasUsrMantenimiento.Size = new System.Drawing.Size(1261, 676);
            this.ComprasUsrMantenimiento.TabIndex = 0;
            // 
            // FrmMantenimientoConceptoProveedor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 676);
            this.Controls.Add(this.ComprasUsrMantenimiento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmMantenimientoConceptoProveedor";
            this.Text = "6014-MantenimientoPlazo";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador ComprasUsrMantenimiento;
    }
}