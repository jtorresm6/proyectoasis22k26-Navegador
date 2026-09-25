namespace CapaVista_Seguridad
{
    partial class FrmEmpleadosNavegador
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
            this.navegador2 = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // navegador2
            // 
            this.navegador2.Location = new System.Drawing.Point(12, 12);
            this.navegador2.Name = "navegador2";
            this.navegador2.Size = new System.Drawing.Size(1438, 111);
            this.navegador2.TabIndex = 1;
            // 
            // FrmEmpleadosNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 450);
            this.Controls.Add(this.navegador2);
            this.Name = "FrmEmpleadosNavegador";
            this.Text = "2001 - Empleados Navegador";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador navegador2;
    }
}