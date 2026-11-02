namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrMDITab
    {
        private System.ComponentModel.IContainer _Componentes = null;
        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Componentes != null) _Componentes.Dispose();
            base.Dispose(Disposing);
        }
        private void InitializeComponent()
        {
            this.ComprasTlpOpcion = new CapaVista_Componentes.Componentes.ClsTableLayoutPanelCompras();
            this.SuspendLayout();
            // 
            // ComprasTlpOpcion
            // 
            this.ComprasTlpOpcion.BackColor = CapaVista_Componentes.Componentes.ClsTemaCompras.FondoGeneral;
            this.ComprasTlpOpcion.ColumnCount = 2;
            this.ComprasTlpOpcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.ComprasTlpOpcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOpcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpOpcion.ForeColor = CapaVista_Componentes.Componentes.ClsTemaCompras.PrincipalNavegador;
            this.ComprasTlpOpcion.Location = new System.Drawing.Point(0, 0);
            this.ComprasTlpOpcion.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpOpcion.Name = "ComprasTlpOpcion";
            this.ComprasTlpOpcion.RowCount = 1;
            this.ComprasTlpOpcion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpOpcion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.ComprasTlpOpcion.Size = new System.Drawing.Size(252, 82);
            this.ComprasTlpOpcion.TabIndex = 0;
            // 
            // ComprasUsrMDITab
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.ComprasTlpOpcion);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ComprasUsrMDITab";
            this.Size = new System.Drawing.Size(252, 82);
            this.ResumeLayout(false);

        }

        private ClsTableLayoutPanelCompras ComprasTlpOpcion;
    }
}
