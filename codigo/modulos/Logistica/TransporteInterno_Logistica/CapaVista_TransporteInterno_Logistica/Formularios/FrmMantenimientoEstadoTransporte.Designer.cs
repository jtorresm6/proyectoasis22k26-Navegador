namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmMantenimientoEstadoTransporte
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMantenimientoEstadoTransporte));
            this.LogisticaNavEstados = new CapaVista_Navegador.Navegador();
            this.LogisticaLblTituloEstados = new System.Windows.Forms.Label();
            this.ogisticaLblDescripcionEstados = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LogisticaNavEstados
            // 
            this.LogisticaNavEstados.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaNavEstados.Location = new System.Drawing.Point(20, 94);
            this.LogisticaNavEstados.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LogisticaNavEstados.Name = "LogisticaNavEstados";
            this.LogisticaNavEstados.Size = new System.Drawing.Size(1078, 90);
            this.LogisticaNavEstados.TabIndex = 0;
            // 
            // LogisticaLblTituloEstados
            // 
            this.LogisticaLblTituloEstados.AutoSize = true;
            this.LogisticaLblTituloEstados.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblTituloEstados.Font = new System.Drawing.Font("Lucida Fax", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblTituloEstados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(138)))), ((int)(((byte)(132)))));
            this.LogisticaLblTituloEstados.Location = new System.Drawing.Point(37, 13);
            this.LogisticaLblTituloEstados.Name = "LogisticaLblTituloEstados";
            this.LogisticaLblTituloEstados.Size = new System.Drawing.Size(126, 32);
            this.LogisticaLblTituloEstados.TabIndex = 1;
            this.LogisticaLblTituloEstados.Text = "Estados";
            // 
            // ogisticaLblDescripcionEstados
            // 
            this.ogisticaLblDescripcionEstados.AutoSize = true;
            this.ogisticaLblDescripcionEstados.BackColor = System.Drawing.Color.Transparent;
            this.ogisticaLblDescripcionEstados.Font = new System.Drawing.Font("Tahoma", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogisticaLblDescripcionEstados.Location = new System.Drawing.Point(40, 57);
            this.ogisticaLblDescripcionEstados.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ogisticaLblDescripcionEstados.Name = "ogisticaLblDescripcionEstados";
            this.ogisticaLblDescripcionEstados.Size = new System.Drawing.Size(945, 22);
            this.ogisticaLblDescripcionEstados.TabIndex = 8;
            this.ogisticaLblDescripcionEstados.Text = "Administra los estados utilizados para identificar la disponibilidad y condición " +
    "operativa de los vehículos y conductores.";
            // 
            // FrmMantenimientoEstadoTransporte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(246)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1144, 741);
            this.Controls.Add(this.ogisticaLblDescripcionEstados);
            this.Controls.Add(this.LogisticaLblTituloEstados);
            this.Controls.Add(this.LogisticaNavEstados);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoEstadoTransporte";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5014-Estados";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CapaVista_Navegador.Navegador LogisticaNavEstados;
        private System.Windows.Forms.Label LogisticaLblTituloEstados;
        private System.Windows.Forms.Label ogisticaLblDescripcionEstados;
    }
}