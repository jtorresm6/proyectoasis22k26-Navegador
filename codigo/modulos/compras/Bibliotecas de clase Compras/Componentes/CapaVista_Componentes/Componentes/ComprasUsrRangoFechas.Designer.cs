namespace CapaVista_Componentes.Componentes
{
    partial class ComprasUsrRangoFechas
    {
        private System.ComponentModel.IContainer _Components = null;

        protected override void Dispose(bool Disposing)
        {
            if (Disposing && _Components != null) _Components.Dispose();
            base.Dispose(Disposing);
        }

        private void InitializeComponent()
        {
            this.ComprasTlpRango = new System.Windows.Forms.TableLayoutPanel();
            this.ComprasLblFechaInicio = new ComprasUsrLabel();
            this.ComprasLblFechaFin = new ComprasUsrLabel();
            this.ComprasDtpFechaInicio = new ClsDatePickerCompras();
            this.ComprasDtpFechaFin = new ClsDatePickerCompras();
            this.ComprasTlpRango.SuspendLayout();
            this.SuspendLayout();
            this.ComprasTlpRango.Name = "ComprasTlpRango";
            this.ComprasTlpRango.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasTlpRango.Margin = new System.Windows.Forms.Padding(0);
            this.ComprasTlpRango.ColumnCount = 2;
            this.ComprasTlpRango.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpRango.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ComprasTlpRango.RowCount = 2;
            this.ComprasTlpRango.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.ComprasTlpRango.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ComprasTlpRango.Controls.Add(this.ComprasLblFechaInicio, 0, 0);
            this.ComprasTlpRango.Controls.Add(this.ComprasLblFechaFin, 1, 0);
            this.ComprasTlpRango.Controls.Add(this.ComprasDtpFechaInicio, 0, 1);
            this.ComprasTlpRango.Controls.Add(this.ComprasDtpFechaFin, 1, 1);
            this.ComprasLblFechaInicio.Name = "ComprasLblFechaInicio";
            this.ComprasLblFechaInicio.Texto = "Desde";
            this.ComprasLblFechaInicio.MostrarAsterisco = false;
            this.ComprasLblFechaInicio.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFechaInicio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaInicio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasLblFechaFin.Name = "ComprasLblFechaFin";
            this.ComprasLblFechaFin.Texto = "Hasta";
            this.ComprasLblFechaFin.MostrarAsterisco = false;
            this.ComprasLblFechaFin.AlineacionTexto = System.Drawing.ContentAlignment.MiddleLeft;
            this.ComprasLblFechaFin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComprasLblFechaFin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ComprasDtpFechaInicio.Name = "ComprasDtpFechaInicio";
            this.ComprasDtpFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComprasDtpFechaInicio.TabIndex = 0;
            this.ComprasDtpFechaFin.Name = "ComprasDtpFechaFin";
            this.ComprasDtpFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.ComprasDtpFechaFin.TabIndex = 1;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = ClsTemaCompras.FondoGeneral;
            this.ForeColor = ClsTemaCompras.PrincipalNavegador;
            this.Controls.Add(this.ComprasTlpRango);
            this.Name = "ComprasUsrRangoFechas";
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(400, 80);
            this.Size = new System.Drawing.Size(440, 80);
            this.ComprasTlpRango.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel ComprasTlpRango;
        private ComprasUsrLabel ComprasLblFechaInicio;
        private ComprasUsrLabel ComprasLblFechaFin;
        private ClsDatePickerCompras ComprasDtpFechaInicio;
        private ClsDatePickerCompras ComprasDtpFechaFin;
    }
}
