namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrLabel
    {
        private System.ComponentModel.IContainer _Componentes = null;

        protected override void Dispose(bool LiberarRecursos)
        {
            if (LiberarRecursos && ComprasLblAsterisco != null && ComprasLblAsterisco.Parent == null)
                ComprasLblAsterisco.Dispose();
            if (LiberarRecursos && (_Componentes != null))
            {
                _Componentes.Dispose();
            }
            base.Dispose(LiberarRecursos);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.ComprasTlpMain = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.ComprasLblTexto = new ClsEtiquetaCompras();
            this.ComprasLblAsterisco = new System.Windows.Forms.Label();
            this.ComprasTlpMain.SuspendLayout();
            this.SuspendLayout();
            this.ComprasTlpMain.ColumnCount = 2;
            this.ComprasTlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ComprasTlpMain.Controls.Add(this.ComprasLblTexto, 0, 0);
            this.ComprasTlpMain.Controls.Add(this.ComprasLblAsterisco, 1, 0);
            this.ComprasTlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpMain.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComprasTlpMain.ForeColor = ClsTemaCompras.PrincipalNavegador;
            this.ComprasTlpMain.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpMain.Name = "ComprasTlpMain";
            this.ComprasTlpMain.RowCount = 1;
            this.ComprasTlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpMain.Size = new System.Drawing.Size(208, 20);
            this.ComprasTlpMain.TabIndex = 0;
            this.ComprasLblTexto.AutoSize = true;
            this.ComprasLblTexto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblTexto.Location = new System.Drawing.Point(3, 0);
            this.ComprasLblTexto.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.ComprasLblTexto.Name = "ComprasLblTexto";
            this.ComprasLblTexto.Size = new System.Drawing.Size(189, 20);
            this.ComprasLblTexto.TabIndex = 0;
            this.ComprasLblTexto.Text = "Label de Compras";
            this.ComprasLblTexto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ComprasLblAsterisco.AutoSize = true;
            this.ComprasLblAsterisco.Dock = System.Windows.Forms.DockStyle.Left;
            this.ComprasLblAsterisco.ForeColor = System.Drawing.Color.Red;
            this.ComprasLblAsterisco.Location = new System.Drawing.Point(192, 0);
            this.ComprasLblAsterisco.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasLblAsterisco.Name = "ComprasLblAsterisco";
            this.ComprasLblAsterisco.Size = new System.Drawing.Size(16, 20);
            this.ComprasLblAsterisco.TabIndex = 1;
            this.ComprasLblAsterisco.Text = "*";
            this.ComprasLblAsterisco.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = ClsTemaCompras.FondoGeneral;
            this.ForeColor = ClsTemaCompras.PrincipalNavegador;
            this.Controls.Add(this.ComprasTlpMain);
            this.MaximumSize = new System.Drawing.Size(1000, 0);
            this.Name = "ComprasUsrLabel";
            this.Size = new System.Drawing.Size(208, 20);
            this.ComprasTlpMain.ResumeLayout(false);
            this.ComprasTlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras ComprasTlpMain;
        private System.Windows.Forms.Label ComprasLblTexto;
        private System.Windows.Forms.Label ComprasLblAsterisco;
    }
}
