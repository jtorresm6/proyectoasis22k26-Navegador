namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    partial class FrmMantenimientoTipoTransporte
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMantenimientoTipoTransporte));
            this.LogisticaNavTipoTransporte = new CapaVista_Navegador.Navegador();
            this.LogisticaLblTituloTipoTransporte = new System.Windows.Forms.Label();
            this.ogisticaLblDescripcionTipoTransporte = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LogisticaNavTipoTransporte
            // 
            this.LogisticaNavTipoTransporte.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaNavTipoTransporte.Location = new System.Drawing.Point(20, 94);
            this.LogisticaNavTipoTransporte.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LogisticaNavTipoTransporte.Name = "LogisticaNavTipoTransporte";
            this.LogisticaNavTipoTransporte.Size = new System.Drawing.Size(1078, 90);
            this.LogisticaNavTipoTransporte.TabIndex = 0;
            // 
            // LogisticaLblTituloTipoTransporte
            // 
            this.LogisticaLblTituloTipoTransporte.AutoSize = true;
            this.LogisticaLblTituloTipoTransporte.BackColor = System.Drawing.Color.Transparent;
            this.LogisticaLblTituloTipoTransporte.Font = new System.Drawing.Font("Lucida Fax", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogisticaLblTituloTipoTransporte.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(138)))), ((int)(((byte)(132)))));
            this.LogisticaLblTituloTipoTransporte.Location = new System.Drawing.Point(37, 13);
            this.LogisticaLblTituloTipoTransporte.Name = "LogisticaLblTituloTipoTransporte";
            this.LogisticaLblTituloTipoTransporte.Size = new System.Drawing.Size(310, 32);
            this.LogisticaLblTituloTipoTransporte.TabIndex = 2;
            this.LogisticaLblTituloTipoTransporte.Text = "Tipos de Transporte";
            // 
            // ogisticaLblDescripcionTipoTransporte
            // 
            this.ogisticaLblDescripcionTipoTransporte.BackColor = System.Drawing.Color.Transparent;
            this.ogisticaLblDescripcionTipoTransporte.Font = new System.Drawing.Font("Tahoma", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ogisticaLblDescripcionTipoTransporte.Location = new System.Drawing.Point(40, 57);
            this.ogisticaLblDescripcionTipoTransporte.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ogisticaLblDescripcionTipoTransporte.Name = "ogisticaLblDescripcionTipoTransporte";
            this.ogisticaLblDescripcionTipoTransporte.Size = new System.Drawing.Size(942, 57);
            this.ogisticaLblDescripcionTipoTransporte.TabIndex = 7;
            this.ogisticaLblDescripcionTipoTransporte.Text = "Registra y administra los diferentes tipos de transporte utilizados en las operac" +
    "iones logísticas, facilitando la clasificación de los vehículos según sus caract" +
    "erísticas.";
            // 
            // FrmMantenimientoTipoTransporte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(246)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(1144, 741);
            this.Controls.Add(this.ogisticaLblDescripcionTipoTransporte);
            this.Controls.Add(this.LogisticaLblTituloTipoTransporte);
            this.Controls.Add(this.LogisticaNavTipoTransporte);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FrmMantenimientoTipoTransporte";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5015-Tipos de Transporte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CapaVista_Navegador.Navegador LogisticaNavTipoTransporte;
        private System.Windows.Forms.Label LogisticaLblTituloTipoTransporte;
        private System.Windows.Forms.Label ogisticaLblDescripcionTipoTransporte;
    }
}