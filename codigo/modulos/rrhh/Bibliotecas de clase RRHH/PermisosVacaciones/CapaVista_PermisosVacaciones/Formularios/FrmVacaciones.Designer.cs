namespace CapaVista_PermisosVacaciones.Formularios
{
    partial class FrmVacaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVacaciones));
            this.RrhhNavVacaciones = new CapaVista_Navegador.Navegador();
            this.RrhhLblAntiguedad = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // RrhhLblAntiguedad
            //
            this.RrhhLblAntiguedad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.RrhhLblAntiguedad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(76)))), ((int)(((byte)(85)))));
            this.RrhhLblAntiguedad.Location = new System.Drawing.Point(12, 12);
            this.RrhhLblAntiguedad.Name = "RrhhLblAntiguedad";
            this.RrhhLblAntiguedad.Size = new System.Drawing.Size(1435, 70);
            this.RrhhLblAntiguedad.TabIndex = 1;
            this.RrhhLblAntiguedad.Text = "Presione Ingresar y elija un empleado para ver su fecha de contratación y si puede optar a vacaciones.";
            // 
            // RrhhNavVacaciones
            // 
            this.RrhhNavVacaciones.Location = new System.Drawing.Point(12, 92);
            this.RrhhNavVacaciones.Name = "RrhhNavVacaciones";
            this.RrhhNavVacaciones.Size = new System.Drawing.Size(1435, 107);
            this.RrhhNavVacaciones.TabIndex = 0;
            // 
            // FrmVacaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 640);
            this.Controls.Add(this.RrhhNavVacaciones);
            this.Controls.Add(this.RrhhLblAntiguedad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmVacaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9203 – Vacaciones";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavVacaciones;
        private System.Windows.Forms.Label RrhhLblAntiguedad;
    }
}