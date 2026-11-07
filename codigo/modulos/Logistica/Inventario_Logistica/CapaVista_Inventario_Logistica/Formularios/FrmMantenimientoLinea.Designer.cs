
namespace CapaVista_Inventario_Logistica.Formularios
{
    partial class FrmMantenimientoLinea
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
                    typeof(FrmMantenimientoLinea));

            this.LogisticaPnlLinea =
                new System.Windows.Forms.Panel();

            this.LogisticaNavLinea =
                new CapaVista_Navegador.Navegador();

            this.LogisticaLblDescripcionLinea =
                new System.Windows.Forms.Label();

            this.LogisticaLblTituloLinea =
                new System.Windows.Forms.Label();

            this.LogisticaPnlLinea.SuspendLayout();
            this.SuspendLayout();

            //
            // LogisticaPnlLinea
            //
            this.LogisticaPnlLinea.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.LogisticaPnlLinea.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.FondoLogica2;

            this.LogisticaPnlLinea.Controls.Add(
                this.LogisticaNavLinea);

            this.LogisticaPnlLinea.Controls.Add(
                this.LogisticaLblDescripcionLinea);

            this.LogisticaPnlLinea.Controls.Add(
                this.LogisticaLblTituloLinea);

            this.LogisticaPnlLinea.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.LogisticaPnlLinea.Location =
                new System.Drawing.Point(0, 0);

            this.LogisticaPnlLinea.Name =
                "LogisticaPnlLinea";

            this.LogisticaPnlLinea.Size =
                new System.Drawing.Size(1144, 741);

            this.LogisticaPnlLinea.TabIndex = 0;

            //
            // LogisticaNavLinea
            //
            this.LogisticaNavLinea.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaNavLinea.Location =
                new System.Drawing.Point(39, 102);

            this.LogisticaNavLinea.Margin =
                new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.LogisticaNavLinea.Name =
                "LogisticaNavLinea";

            this.LogisticaNavLinea.Size =
                new System.Drawing.Size(1078, 90);

            this.LogisticaNavLinea.TabIndex = 2;

            //
            // LogisticaLblDescripcionLinea
            //
            this.LogisticaLblDescripcionLinea.AutoSize = true;

            this.LogisticaLblDescripcionLinea.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDescripcionLinea.Font =
                new System.Drawing.Font("Tahoma", 12.75F);

            this.LogisticaLblDescripcionLinea.ForeColor =
                System.Drawing.Color.Black;

            this.LogisticaLblDescripcionLinea.Location =
                new System.Drawing.Point(35, 69);

            this.LogisticaLblDescripcionLinea.Name =
                "LogisticaLblDescripcionLinea";

            this.LogisticaLblDescripcionLinea.Size =
                new System.Drawing.Size(498, 21);

            this.LogisticaLblDescripcionLinea.TabIndex = 1;

            this.LogisticaLblDescripcionLinea.Text =
                "Administra las líneas de productos y sus porcentajes de comisión.";

            //
            // LogisticaLblTituloLinea
            //
            this.LogisticaLblTituloLinea.AutoSize = true;

            this.LogisticaLblTituloLinea.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblTituloLinea.Font =
                new System.Drawing.Font(
                    "Lucida Fax",
                    20.25F,
                    System.Drawing.FontStyle.Bold);

            this.LogisticaLblTituloLinea.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaLblTituloLinea.Location =
                new System.Drawing.Point(33, 22);

            this.LogisticaLblTituloLinea.Name =
                "LogisticaLblTituloLinea";

            this.LogisticaLblTituloLinea.Size =
                new System.Drawing.Size(91, 32);

            this.LogisticaLblTituloLinea.TabIndex = 0;

            this.LogisticaLblTituloLinea.Text = "Línea";

            //
            // FrmMantenimientoLinea
            //
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(6F, 13F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.ClientSize =
                new System.Drawing.Size(1144, 741);

            this.Controls.Add(this.LogisticaPnlLinea);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Icon =
                ((System.Drawing.Icon)(
                    resources.GetObject("$this.Icon")));

            this.MaximizeBox = false;

            this.Name = "FrmMantenimientoLinea";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "5004 - Línea";

            this.LogisticaPnlLinea.ResumeLayout(false);
            this.LogisticaPnlLinea.PerformLayout();

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel
            LogisticaPnlLinea;

        private System.Windows.Forms.Label
            LogisticaLblDescripcionLinea;

        private System.Windows.Forms.Label
            LogisticaLblTituloLinea;

        private CapaVista_Navegador.Navegador
            LogisticaNavLinea;
    }
}
