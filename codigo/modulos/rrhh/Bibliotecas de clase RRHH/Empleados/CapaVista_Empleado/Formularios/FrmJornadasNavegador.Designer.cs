namespace CapaVista_Empleado.Formularios
{
    partial class FrmJornadasNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmJornadasNavegador));
            this.RrhhNavJornadas = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // RrhhNavJornadas
            // 
            this.RrhhNavJornadas.Location = new System.Drawing.Point(12, 92);
            this.RrhhNavJornadas.Name = "RrhhNavJornadas";
            this.RrhhNavJornadas.Size = new System.Drawing.Size(1435, 107);
            this.RrhhNavJornadas.TabIndex = 0;
            // 
            // FrmJornadasNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 640);
            this.Controls.Add(this.RrhhNavJornadas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmJornadasNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9005 – Jornadas";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavJornadas;
    }
}
