
namespace CapaVista_Inventario_Logistica.Formularios
{
    partial class FrmMantenimientoDetalleMovimiento
    {
        /// <summary>
        /// Variable requerida por el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberar los recursos utilizados.
        /// </summary>
        /// <param name="disposing">
        /// Indica si se deben liberar los recursos.
        /// </param>
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
        /// Método requerido para admitir el diseñador.
        /// No modificar mediante el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(
                    typeof(FrmMantenimientoDetalleMovimiento));

            this.LogisticaPnlDetalleMovimiento =
                new System.Windows.Forms.Panel();

            this.LogisticaNavDetalleMovimiento =
                new CapaVista_Navegador.Navegador();

            this.LogisticaLblDescripcionDetalleMovimiento =
                new System.Windows.Forms.Label();

            this.LogisticaLblDetalleMovimiento =
                new System.Windows.Forms.Label();

            this.LogisticaPnlDetalleMovimiento.SuspendLayout();
            this.SuspendLayout();

            //
            // LogisticaPnlDetalleMovimiento
            //
            this.LogisticaPnlDetalleMovimiento.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.FondoLogica2;

            this.LogisticaPnlDetalleMovimiento.Controls.Add(
                this.LogisticaNavDetalleMovimiento);

            this.LogisticaPnlDetalleMovimiento.Controls.Add(
                this.LogisticaLblDescripcionDetalleMovimiento);

            this.LogisticaPnlDetalleMovimiento.Controls.Add(
                this.LogisticaLblDetalleMovimiento);

            this.LogisticaPnlDetalleMovimiento.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.LogisticaPnlDetalleMovimiento.Location =
                new System.Drawing.Point(0, 0);

            this.LogisticaPnlDetalleMovimiento.Name =
                "LogisticaPnlDetalleMovimiento";

            this.LogisticaPnlDetalleMovimiento.Size =
                new System.Drawing.Size(1144, 741);

            this.LogisticaPnlDetalleMovimiento.TabIndex = 0;

            //
            // LogisticaNavDetalleMovimiento
            //
            this.LogisticaNavDetalleMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaNavDetalleMovimiento.Location =
                new System.Drawing.Point(42, 130);

            this.LogisticaNavDetalleMovimiento.Margin =
                new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.LogisticaNavDetalleMovimiento.Name =
                "LogisticaNavDetalleMovimiento";

            this.LogisticaNavDetalleMovimiento.Size =
                new System.Drawing.Size(1078, 90);

            this.LogisticaNavDetalleMovimiento.TabIndex = 2;

            //
            // LogisticaLblDescripcionDetalleMovimiento
            //
            this.LogisticaLblDescripcionDetalleMovimiento.AutoSize = true;

            this.LogisticaLblDescripcionDetalleMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDescripcionDetalleMovimiento.Font =
                new System.Drawing.Font("Tahoma", 12.75F);

            this.LogisticaLblDescripcionDetalleMovimiento.ForeColor =
                System.Drawing.Color.Black;

            this.LogisticaLblDescripcionDetalleMovimiento.Location =
                new System.Drawing.Point(38, 66);

            this.LogisticaLblDescripcionDetalleMovimiento.Name =
                "LogisticaLblDescripcionDetalleMovimiento";

            this.LogisticaLblDescripcionDetalleMovimiento.Size =
                new System.Drawing.Size(495, 42);

            this.LogisticaLblDescripcionDetalleMovimiento.TabIndex = 1;

            this.LogisticaLblDescripcionDetalleMovimiento.Text =
                "Permite agregar productos, especificar lotes, registrar cantidades\r\n" +
                "y establecer los precios de compra de cada movimiento.";

            //
            // LogisticaLblDetalleMovimiento
            //
            this.LogisticaLblDetalleMovimiento.AutoSize = true;

            this.LogisticaLblDetalleMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDetalleMovimiento.Font =
                new System.Drawing.Font(
                    "Lucida Fax",
                    20.25F,
                    System.Drawing.FontStyle.Bold);

            this.LogisticaLblDetalleMovimiento.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaLblDetalleMovimiento.Location =
                new System.Drawing.Point(35, 27);

            this.LogisticaLblDetalleMovimiento.Name =
                "LogisticaLblDetalleMovimiento";

            this.LogisticaLblDetalleMovimiento.Size =
                new System.Drawing.Size(341, 32);

            this.LogisticaLblDetalleMovimiento.TabIndex = 0;

            this.LogisticaLblDetalleMovimiento.Text =
                "Detalle de Movimiento";

            //
            // FrmMantenimientoDetalleMovimiento
            //
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(6F, 13F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.ClientSize =
                new System.Drawing.Size(1144, 741);

            this.Controls.Add(
                this.LogisticaPnlDetalleMovimiento);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Icon =
                ((System.Drawing.Icon)(
                    resources.GetObject("$this.Icon")));

            this.MaximizeBox = false;

            this.Name =
                "FrmMantenimientoDetalleMovimiento";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "5007 - Detalle de Movimiento";

            this.LogisticaPnlDetalleMovimiento.ResumeLayout(false);
            this.LogisticaPnlDetalleMovimiento.PerformLayout();

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel
            LogisticaPnlDetalleMovimiento;

        private CapaVista_Navegador.Navegador
            LogisticaNavDetalleMovimiento;

        private System.Windows.Forms.Label
            LogisticaLblDescripcionDetalleMovimiento;

        private System.Windows.Forms.Label
            LogisticaLblDetalleMovimiento;
    }
}
