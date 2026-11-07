namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmMantenimientoConductor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMantenimientoConductor));
            this.LogisticaNavConductores = new CapaVista_Navegador.Navegador();
            this.LogisticaLblTituloConductores = new System.Windows.Forms.Label();
            this.ogisticaLblDescripcionConductores = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LogisticaNavConductores
            // 
            this.LogisticaNavConductores.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaNavConductores.Location = new System.Drawing.Point(20, 102);
            this.LogisticaNavConductores.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LogisticaNavConductores.Name = "LogisticaNavConductores";
            this.LogisticaNavConductores.Size = new System.Drawing.Size(1078, 90);
            this.LogisticaNavConductores.TabIndex = 0;
            // 
            // LogisticaLblTituloConductores
            // 
            this.LogisticaLblTituloConductores.AutoSize = true;
            this.LogisticaLblTituloConductores.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblTituloConductores.Font = new System.Drawing.Font("Lucida Fax", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblTituloConductores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(138)))), ((int)(((byte)(132)))));
            this.LogisticaLblTituloConductores.Location = new System.Drawing.Point(37, 13);
            this.LogisticaLblTituloConductores.Name = "LogisticaLblTituloConductores";
            this.LogisticaLblTituloConductores.Size = new System.Drawing.Size(198, 32);
            this.LogisticaLblTituloConductores.TabIndex = 3;
            this.LogisticaLblTituloConductores.Text = "Conductores";
            // 
            // ogisticaLblDescripcionConductores
            // 
            this.ogisticaLblDescripcionConductores.BackColor = System.Drawing.Color.Transparent;
            this.ogisticaLblDescripcionConductores.Font = new System.Drawing.Font("Tahoma", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogisticaLblDescripcionConductores.Location = new System.Drawing.Point(40, 57);
            this.ogisticaLblDescripcionConductores.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ogisticaLblDescripcionConductores.Name = "ogisticaLblDescripcionConductores";
            this.ogisticaLblDescripcionConductores.Size = new System.Drawing.Size(969, 57);
            this.ogisticaLblDescripcionConductores.TabIndex = 6;
            this.ogisticaLblDescripcionConductores.Text = "Registra y administra la información de los conductores, incluyendo datos persona" +
    "les, licencias de conducción, fechas de vencimiento y estados de disponibilidad." +
    "";
            // 
            // FrmMantenimientoConductor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(246)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1144, 741);
            this.Controls.Add(this.ogisticaLblDescripcionConductores);
            this.Controls.Add(this.LogisticaLblTituloConductores);
            this.Controls.Add(this.LogisticaNavConductores);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoConductor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5016-Conductores";
            this.Load += new System.EventHandler(this.FrmMantenimientoConductor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CapaVista_Navegador.Navegador LogisticaNavConductores;
        private System.Windows.Forms.Label LogisticaLblTituloConductores;
        private System.Windows.Forms.Label ogisticaLblDescripcionConductores;
    }
}