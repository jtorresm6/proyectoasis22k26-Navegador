namespace CapaVista_Consultas
{
    partial class ConsultaCompleja_500_001
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
            this.tablaCompleja1 = new CapaVista_Consultas.TablaCompleja();
            this.condicionesLogica1 = new CapaVista_Consultas.CondicionesLogica();
            this.consultasReutilizables1 = new CapaVista_Consultas.ConsultasReutilizables();
            this.condicionesComparacion1 = new CapaVista_Consultas.CondicionesComparacion();
            this.agrupar_Ordenar1 = new CapaVista_Consultas.Agrupar_Ordenar();
            this.botonesConsulta1 = new CapaVista_Consultas.BotonesConsulta();
            this.SuspendLayout();
            // 
            // tablaCompleja1
            // 
            this.tablaCompleja1.Location = new System.Drawing.Point(0, 0);
            this.tablaCompleja1.Name = "tablaCompleja1";
            this.tablaCompleja1.Size = new System.Drawing.Size(794, 440);
            this.tablaCompleja1.TabIndex = 4;
            // 
            // condicionesLogica1
            // 
            this.condicionesLogica1.Location = new System.Drawing.Point(4, 466);
            this.condicionesLogica1.Name = "condicionesLogica1";
            this.condicionesLogica1.Size = new System.Drawing.Size(459, 224);
            this.condicionesLogica1.TabIndex = 3;
            // 
            // consultasReutilizables1
            // 
            this.consultasReutilizables1.Location = new System.Drawing.Point(811, 0);
            this.consultasReutilizables1.Name = "consultasReutilizables1";
            this.consultasReutilizables1.Size = new System.Drawing.Size(431, 267);
            this.consultasReutilizables1.TabIndex = 2;
            // 
            // condicionesComparacion1
            // 
            this.condicionesComparacion1.Location = new System.Drawing.Point(469, 466);
            this.condicionesComparacion1.Name = "condicionesComparacion1";
            this.condicionesComparacion1.Size = new System.Drawing.Size(391, 224);
            this.condicionesComparacion1.TabIndex = 1;
            // 
            // agrupar_Ordenar1
            // 
            this.agrupar_Ordenar1.Location = new System.Drawing.Point(12, 696);
            this.agrupar_Ordenar1.Name = "agrupar_Ordenar1";
            this.agrupar_Ordenar1.Size = new System.Drawing.Size(799, 84);
            this.agrupar_Ordenar1.TabIndex = 0;
            // 
            // botonesConsulta1
            // 
            this.botonesConsulta1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.botonesConsulta1.Location = new System.Drawing.Point(0, 782);
            this.botonesConsulta1.Name = "botonesConsulta1";
            this.botonesConsulta1.Size = new System.Drawing.Size(1254, 70);
            this.botonesConsulta1.TabIndex = 5;
            // 
            // ConsultaCompleja_500_001
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 852);
            this.Controls.Add(this.botonesConsulta1);
            this.Controls.Add(this.tablaCompleja1);
            this.Controls.Add(this.condicionesLogica1);
            this.Controls.Add(this.consultasReutilizables1);
            this.Controls.Add(this.condicionesComparacion1);
            this.Controls.Add(this.agrupar_Ordenar1);
            this.MinimumSize = new System.Drawing.Size(1020, 740);
            this.Name = "ConsultaCompleja_500_001";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consulta Compleja";
            this.ResumeLayout(false);

        }

        #endregion

        private Agrupar_Ordenar agrupar_Ordenar1;
        private CondicionesComparacion condicionesComparacion1;
        private ConsultasReutilizables consultasReutilizables1;
        private CondicionesLogica condicionesLogica1;
        private TablaCompleja tablaCompleja1;
        private BotonesConsulta botonesConsulta1;
    }
}