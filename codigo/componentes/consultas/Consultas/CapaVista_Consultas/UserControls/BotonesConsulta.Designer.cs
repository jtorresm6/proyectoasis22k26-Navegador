namespace CapaVista_Consultas
{
    partial class BotonesConsulta
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_Botones = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Limpiar = new System.Windows.Forms.Button();
            this.btn_EjecutarConsulta = new System.Windows.Forms.Button();
            this.btn_Generar = new System.Windows.Forms.Button();
            this.pnl_Botones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_Botones
            // 
            this.pnl_Botones.Controls.Add(this.btn_Limpiar);
            this.pnl_Botones.Controls.Add(this.btn_EjecutarConsulta);
            this.pnl_Botones.Controls.Add(this.btn_Generar);
            this.pnl_Botones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Botones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnl_Botones.Location = new System.Drawing.Point(0, 0);
            this.pnl_Botones.Name = "pnl_Botones";
            this.pnl_Botones.Padding = new System.Windows.Forms.Padding(0, 10, 10, 10);
            this.pnl_Botones.Size = new System.Drawing.Size(1174, 70);
            this.pnl_Botones.TabIndex = 0;
            // 
            // btn_Limpiar
            // 
            this.btn_Limpiar.Location = new System.Drawing.Point(1011, 13);
            this.btn_Limpiar.Name = "btn_Limpiar";
            this.btn_Limpiar.Size = new System.Drawing.Size(150, 40);
            this.btn_Limpiar.TabIndex = 0;
            this.btn_Limpiar.Text = "Limpiar";
            this.btn_Limpiar.UseVisualStyleBackColor = true;
            // 
            // btn_EjecutarConsulta
            // 
            this.btn_EjecutarConsulta.Location = new System.Drawing.Point(855, 13);
            this.btn_EjecutarConsulta.Name = "btn_EjecutarConsulta";
            this.btn_EjecutarConsulta.Size = new System.Drawing.Size(150, 40);
            this.btn_EjecutarConsulta.TabIndex = 1;
            this.btn_EjecutarConsulta.Text = "Ejecutar Consulta";
            this.btn_EjecutarConsulta.UseVisualStyleBackColor = true;
            // 
            // btn_Generar
            // 
            this.btn_Generar.Location = new System.Drawing.Point(699, 13);
            this.btn_Generar.Name = "btn_Generar";
            this.btn_Generar.Size = new System.Drawing.Size(150, 40);
            this.btn_Generar.TabIndex = 2;
            this.btn_Generar.Text = "Generar";
            this.btn_Generar.UseVisualStyleBackColor = true;
            // 
            // BotonesConsulta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.pnl_Botones);
            this.Name = "BotonesConsulta";
            this.Size = new System.Drawing.Size(1174, 70);
            this.Load += new System.EventHandler(this.BotonesConsulta_Load);
            this.pnl_Botones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnl_Botones;
        private System.Windows.Forms.Button btn_Limpiar;
        private System.Windows.Forms.Button btn_EjecutarConsulta;
        private System.Windows.Forms.Button btn_Generar;
    }
}
