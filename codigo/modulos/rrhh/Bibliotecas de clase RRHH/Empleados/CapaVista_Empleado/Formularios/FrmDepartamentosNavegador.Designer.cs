namespace CapaVista_Empleado.Formularios
{
    partial class FrmDepartamentosNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDepartamentosNavegador));
            this.RrhhNavDepartamentos = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // RrhhNavDepartamentos
            // 
            this.RrhhNavDepartamentos.Location = new System.Drawing.Point(12, 92);
            this.RrhhNavDepartamentos.Name = "RrhhNavDepartamentos";
            this.RrhhNavDepartamentos.Size = new System.Drawing.Size(1435, 107);
            this.RrhhNavDepartamentos.TabIndex = 0;
            // 
            // FrmDepartamentosNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 640);
            this.Controls.Add(this.RrhhNavDepartamentos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmDepartamentosNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9002 – Departamentos";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavDepartamentos;
    }
}
