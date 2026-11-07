namespace CapaVista_EvaluacionDesempeño.Formularios
{
    partial class FrmRealizarEvaluacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRealizarEvaluacion));
            this.RrhhNavRealizarEvaluacion = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // RrhhNavRealizarEvaluacion
            // 
            this.RrhhNavRealizarEvaluacion.Location = new System.Drawing.Point(12, 92);
            this.RrhhNavRealizarEvaluacion.Name = "RrhhNavRealizarEvaluacion";
            this.RrhhNavRealizarEvaluacion.Size = new System.Drawing.Size(1435, 107);
            this.RrhhNavRealizarEvaluacion.TabIndex = 0;
            // 
            // FrmRealizarEvaluacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1462, 640);
            this.Controls.Add(this.RrhhNavRealizarEvaluacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmRealizarEvaluacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9402 – RealizarEvaluacion";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavRealizarEvaluacion;
    }
}