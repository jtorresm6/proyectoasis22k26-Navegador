namespace CapaVista_Empleado.Formularios
{
    partial class FrmTipoPuestosNavegador
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTipoPuestosNavegador));
            this.RrhhNavTipoPuestos = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // RrhhNavTipoPuestos
            // 
            this.RrhhNavTipoPuestos.Location = new System.Drawing.Point(2, 52);
            this.RrhhNavTipoPuestos.Name = "RrhhNavTipoPuestos";
            this.RrhhNavTipoPuestos.Size = new System.Drawing.Size(1435, 107);
            this.RrhhNavTipoPuestos.TabIndex = 0;
            // 
            // FrmTipoPuestosNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 640);
            this.Controls.Add(this.RrhhNavTipoPuestos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmTipoPuestosNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9003 – TipoPuestos";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavTipoPuestos;
    }
}
