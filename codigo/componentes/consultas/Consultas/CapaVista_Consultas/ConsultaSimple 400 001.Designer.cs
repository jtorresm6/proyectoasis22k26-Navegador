namespace CapaVista_Consultas
{
    partial class ConsultaSimple_400_001
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
            this.agrupar_Ordenar1 = new CapaVista_Consultas.Agrupar_Ordenar();
            this.tablaSimple1 = new CapaVista_Consultas.TablaSimple();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // agrupar_Ordenar1
            // 
            this.agrupar_Ordenar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.agrupar_Ordenar1.Location = new System.Drawing.Point(15, 14);
            this.agrupar_Ordenar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.agrupar_Ordenar1.Name = "agrupar_Ordenar1";
            this.agrupar_Ordenar1.Size = new System.Drawing.Size(833, 107);
            this.agrupar_Ordenar1.TabIndex = 2;
            // 
            // tablaSimple1
            // 
            this.tablaSimple1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.tablaSimple1.Location = new System.Drawing.Point(15, 126);
            this.tablaSimple1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablaSimple1.Name = "tablaSimple1";
            this.tablaSimple1.Size = new System.Drawing.Size(1052, 314);
            this.tablaSimple1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(816, 39);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 63);
            this.button1.TabIndex = 3;
            this.button1.Text = "Filtrar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(967, 65);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 37);
            this.button2.TabIndex = 4;
            this.button2.Text = "Restablecer";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(980, 457);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 63);
            this.button3.TabIndex = 5;
            this.button3.Text = "Consulta Compleja";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ConsultaSimple_400_001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 529);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.agrupar_Ordenar1);
            this.Controls.Add(this.tablaSimple1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConsultaSimple_400_001";
            this.Text = "ConsultaSimple_400_001";
            this.ResumeLayout(false);

        }

        #endregion

        private Agrupar_Ordenar agrupar_Ordenar1;
        private TablaSimple tablaSimple1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}