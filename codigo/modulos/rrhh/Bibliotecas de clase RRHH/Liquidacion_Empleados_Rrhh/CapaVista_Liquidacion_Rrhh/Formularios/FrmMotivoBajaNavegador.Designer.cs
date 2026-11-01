namespace CapaVista_Liquidacion_Rrhh.Formularios
{
    partial class FrmMotivoBajaNavegador
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMotivoBajaNavegador));
            this.RrhhNavMotivoBaja = new CapaVista_Navegador.Navegador();
            this.SuspendLayout();
            // 
            // RrhhNavMotivoBaja
            // 
            this.RrhhNavMotivoBaja.Location = new System.Drawing.Point(11, 108);
            this.RrhhNavMotivoBaja.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RrhhNavMotivoBaja.Name = "RrhhNavMotivoBaja";
            this.RrhhNavMotivoBaja.Size = new System.Drawing.Size(1437, 111);
            this.RrhhNavMotivoBaja.TabIndex = 0;
            // 
            // FrmMotivoBajaNavegador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1449, 769);
            this.Controls.Add(this.RrhhNavMotivoBaja);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMotivoBajaNavegador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "9702 – MotivoBaja";
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Navegador.Navegador RrhhNavMotivoBaja;
    }
}