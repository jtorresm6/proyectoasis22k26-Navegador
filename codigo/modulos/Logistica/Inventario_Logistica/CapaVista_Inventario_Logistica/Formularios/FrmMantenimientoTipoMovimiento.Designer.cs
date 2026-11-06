
namespace CapaVista_Inventario_Logistica.Formularios
{
    partial class FrmMantenimientoTipoMovimiento
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
                    typeof(FrmMantenimientoTipoMovimiento));

            this.LogisticaPnlTipoMovimiento =
                new System.Windows.Forms.Panel();

            this.LogisticaNavTipoMovimiento =
                new CapaVista_Navegador.Navegador();

            this.LogisticaLblDescripcionTipoMovimiento =
                new System.Windows.Forms.Label();

            this.LogisiticaLblTituloTipoMovimiento =
                new System.Windows.Forms.Label();

            this.LogisticaPnlTipoMovimiento.SuspendLayout();
            this.SuspendLayout();

            //
            // LogisticaPnlTipoMovimiento
            //
            this.LogisticaPnlTipoMovimiento.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.FondoLogica2;

            this.LogisticaPnlTipoMovimiento.Controls.Add(
                this.LogisticaNavTipoMovimiento);

            this.LogisticaPnlTipoMovimiento.Controls.Add(
                this.LogisticaLblDescripcionTipoMovimiento);

            this.LogisticaPnlTipoMovimiento.Controls.Add(
                this.LogisiticaLblTituloTipoMovimiento);

            this.LogisticaPnlTipoMovimiento.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.LogisticaPnlTipoMovimiento.Location =
                new System.Drawing.Point(0, 0);

            this.LogisticaPnlTipoMovimiento.Name =
                "LogisticaPnlTipoMovimiento";

            this.LogisticaPnlTipoMovimiento.Size =
                new System.Drawing.Size(1144, 741);

            this.LogisticaPnlTipoMovimiento.TabIndex = 0;

            //
            // LogisticaNavTipoMovimiento
            //
            this.LogisticaNavTipoMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaNavTipoMovimiento.Location =
                new System.Drawing.Point(30, 136);

            this.LogisticaNavTipoMovimiento.Margin =
                new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.LogisticaNavTipoMovimiento.Name =
                "LogisticaNavTipoMovimiento";

            this.LogisticaNavTipoMovimiento.Size =
                new System.Drawing.Size(1078, 90);

            this.LogisticaNavTipoMovimiento.TabIndex = 2;

            //
            // LogisticaLblDescripcionTipoMovimiento
            //
            this.LogisticaLblDescripcionTipoMovimiento.AutoSize = true;

            this.LogisticaLblDescripcionTipoMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDescripcionTipoMovimiento.Font =
                new System.Drawing.Font("Tahoma", 12.75F);

            this.LogisticaLblDescripcionTipoMovimiento.ForeColor =
                System.Drawing.Color.Black;

            this.LogisticaLblDescripcionTipoMovimiento.Location =
                new System.Drawing.Point(43, 92);

            this.LogisticaLblDescripcionTipoMovimiento.Name =
                "LogisticaLblDescripcionTipoMovimiento";

            this.LogisticaLblDescripcionTipoMovimiento.Size =
                new System.Drawing.Size(430, 42);

            this.LogisticaLblDescripcionTipoMovimiento.TabIndex = 1;

            this.LogisticaLblDescripcionTipoMovimiento.Text =
                "Configura los tipos de movimiento de inventario y define \r\n" +
                "su efecto de entrada o salida.";

            //
            // LogisiticaLblTituloTipoMovimiento
            //
            this.LogisiticaLblTituloTipoMovimiento.AutoSize = true;

            this.LogisiticaLblTituloTipoMovimiento.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisiticaLblTituloTipoMovimiento.Font =
                new System.Drawing.Font(
                    "Lucida Fax",
                    20.25F,
                    System.Drawing.FontStyle.Bold);

            this.LogisiticaLblTituloTipoMovimiento.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisiticaLblTituloTipoMovimiento.Location =
                new System.Drawing.Point(41, 42);

            this.LogisiticaLblTituloTipoMovimiento.Name =
                "LogisiticaLblTituloTipoMovimiento";

            this.LogisiticaLblTituloTipoMovimiento.Size =
                new System.Drawing.Size(306, 32);

            this.LogisiticaLblTituloTipoMovimiento.TabIndex = 0;

            this.LogisiticaLblTituloTipoMovimiento.Text =
                "Tipo de Movimiento";

            //
            // FrmMantenimientoTipoMovimiento
            //
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(6F, 13F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.ClientSize =
                new System.Drawing.Size(1144, 741);

            this.Controls.Add(this.LogisticaPnlTipoMovimiento);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Icon =
                ((System.Drawing.Icon)(
                    resources.GetObject("$this.Icon")));

            this.MaximizeBox = false;

            this.Name =
                "FrmMantenimientoTipoMovimiento";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "5008 - Tipo de Movimiento";

            this.LogisticaPnlTipoMovimiento.ResumeLayout(false);
            this.LogisticaPnlTipoMovimiento.PerformLayout();

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel
            LogisticaPnlTipoMovimiento;

        private CapaVista_Navegador.Navegador
            LogisticaNavTipoMovimiento;

        private System.Windows.Forms.Label
            LogisticaLblDescripcionTipoMovimiento;

        private System.Windows.Forms.Label
            LogisiticaLblTituloTipoMovimiento;
    }
}
