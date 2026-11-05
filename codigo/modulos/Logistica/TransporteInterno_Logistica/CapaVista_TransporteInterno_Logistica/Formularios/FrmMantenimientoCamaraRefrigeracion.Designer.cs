namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmMantenimientoCamaraRefrigeracion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMantenimientoCamaraRefrigeracion));
            this.ogisticaLblDescripcionCamarasRefrigeracion = new System.Windows.Forms.Label();
            this.LogisticaLblTituloCamarasRefrigeracion = new System.Windows.Forms.Label();
            this.LogisticaNavCamarasRefrigeracion = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // ogisticaLblDescripcionCamarasRefrigeracion
            // 
            this.ogisticaLblDescripcionCamarasRefrigeracion.BackColor = System.Drawing.Color.Transparent;
            this.ogisticaLblDescripcionCamarasRefrigeracion.Font = new System.Drawing.Font("Tahoma", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogisticaLblDescripcionCamarasRefrigeracion.Location = new System.Drawing.Point(40, 57);
            this.ogisticaLblDescripcionCamarasRefrigeracion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ogisticaLblDescripcionCamarasRefrigeracion.Name = "ogisticaLblDescripcionCamarasRefrigeracion";
            this.ogisticaLblDescripcionCamarasRefrigeracion.Size = new System.Drawing.Size(969, 57);
            this.ogisticaLblDescripcionCamarasRefrigeracion.TabIndex = 8;
            this.ogisticaLblDescripcionCamarasRefrigeracion.Text = "Administra los equipos de refrigeración asociados a los vehículos, permitiendo re" +
    "gistrar sus características, rangos de temperatura y condiciones operativas para" +
    " garantizar la cadena de frío.";
            // 
            // LogisticaLblTituloCamarasRefrigeracion
            // 
            this.LogisticaLblTituloCamarasRefrigeracion.AutoSize = true;
            this.LogisticaLblTituloCamarasRefrigeracion.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblTituloCamarasRefrigeracion.Font = new System.Drawing.Font("Lucida Fax", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblTituloCamarasRefrigeracion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(138)))), ((int)(((byte)(132)))));
            this.LogisticaLblTituloCamarasRefrigeracion.Location = new System.Drawing.Point(37, 13);
            this.LogisticaLblTituloCamarasRefrigeracion.Name = "LogisticaLblTituloCamarasRefrigeracion";
            this.LogisticaLblTituloCamarasRefrigeracion.Size = new System.Drawing.Size(384, 32);
            this.LogisticaLblTituloCamarasRefrigeracion.TabIndex = 7;
            this.LogisticaLblTituloCamarasRefrigeracion.Text = "Cámaras de Refrigeración";
            // 
            // LogisticaNavCamarasRefrigeracion
            // 
            this.LogisticaNavCamarasRefrigeracion.Location = new System.Drawing.Point(9, 107);
            this.LogisticaNavCamarasRefrigeracion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LogisticaNavCamarasRefrigeracion.Name = "LogisticaNavCamarasRefrigeracion";
            this.LogisticaNavCamarasRefrigeracion.Size = new System.Drawing.Size(1078, 90);
            this.LogisticaNavCamarasRefrigeracion.TabIndex = 9;
            // 
            // FrmMantenimientoCamaraRefrigeracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(246)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1144, 741);
            this.Controls.Add(this.LogisticaNavCamarasRefrigeracion);
            this.Controls.Add(this.ogisticaLblDescripcionCamarasRefrigeracion);
            this.Controls.Add(this.LogisticaLblTituloCamarasRefrigeracion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoCamaraRefrigeracion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5028-Cámaras de Refrigeración";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ogisticaLblDescripcionCamarasRefrigeracion;
        private System.Windows.Forms.Label LogisticaLblTituloCamarasRefrigeracion;
        private CapaVista_Navegador.Navegador LogisticaNavCamarasRefrigeracion;
    }
}