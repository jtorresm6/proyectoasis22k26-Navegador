
namespace CapaVista_Inventario_Logistica.Formularios
{
    partial class FrmMantenimientoSucursales
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
                    typeof(FrmMantenimientoSucursales));

            this.LogisticaPnlSucursales =
                new System.Windows.Forms.Panel();

            this.LogisticaBtnBodega =
                new System.Windows.Forms.Button();

            this.LogisticaNavSucursales =
                new CapaVista_Navegador.Navegador();

            this.LogisticaLblDescripcionSucursales =
                new System.Windows.Forms.Label();

            this.LogisticaLblTituloSucursales =
                new System.Windows.Forms.Label();

            this.LogisticaPnlSucursales.SuspendLayout();
            this.SuspendLayout();

            //
            // LogisticaPnlSucursales
            //
            this.LogisticaPnlSucursales.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.LogisticaPnlSucursales.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.FondoLogica2;

            this.LogisticaPnlSucursales.Controls.Add(
                this.LogisticaBtnBodega);

            this.LogisticaPnlSucursales.Controls.Add(
                this.LogisticaNavSucursales);

            this.LogisticaPnlSucursales.Controls.Add(
                this.LogisticaLblDescripcionSucursales);

            this.LogisticaPnlSucursales.Controls.Add(
                this.LogisticaLblTituloSucursales);

            this.LogisticaPnlSucursales.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.LogisticaPnlSucursales.Location =
                new System.Drawing.Point(0, 0);

            this.LogisticaPnlSucursales.Name =
                "LogisticaPnlSucursales";

            this.LogisticaPnlSucursales.Size =
                new System.Drawing.Size(1144, 741);

            this.LogisticaPnlSucursales.TabIndex = 0;

            //
            // LogisticaBtnBodega
            //
            this.LogisticaBtnBodega.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaBtnBodega.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.btn_bodega;

            this.LogisticaBtnBodega.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.LogisticaBtnBodega.FlatAppearance.BorderSize = 0;

            this.LogisticaBtnBodega.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.LogisticaBtnBodega.Location =
                new System.Drawing.Point(1044, 23);

            this.LogisticaBtnBodega.Name =
                "LogisticaBtnBodega";

            this.LogisticaBtnBodega.Size =
                new System.Drawing.Size(60, 66);

            this.LogisticaBtnBodega.TabIndex = 3;

            this.LogisticaBtnBodega.UseVisualStyleBackColor = false;

            this.LogisticaBtnBodega.Click +=
                new System.EventHandler(
                    this.LogisticaBtnBodega_Click);

            //
            // LogisticaNavSucursales
            //
            this.LogisticaNavSucursales.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaNavSucursales.Location =
                new System.Drawing.Point(37, 87);

            this.LogisticaNavSucursales.Margin =
                new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.LogisticaNavSucursales.Name =
                "LogisticaNavSucursales";

            this.LogisticaNavSucursales.Size =
                new System.Drawing.Size(1073, 99);

            this.LogisticaNavSucursales.TabIndex = 2;

            //
            // LogisticaLblDescripcionSucursales
            //
            this.LogisticaLblDescripcionSucursales.AutoSize = true;

            this.LogisticaLblDescripcionSucursales.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDescripcionSucursales.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    12.75F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.LogisticaLblDescripcionSucursales.ForeColor =
                System.Drawing.Color.Black;

            this.LogisticaLblDescripcionSucursales.Location =
                new System.Drawing.Point(21, 64);

            this.LogisticaLblDescripcionSucursales.Name =
                "LogisticaLblDescripcionSucursales";

            this.LogisticaLblDescripcionSucursales.Size =
                new System.Drawing.Size(563, 21);

            this.LogisticaLblDescripcionSucursales.TabIndex = 1;

            this.LogisticaLblDescripcionSucursales.Text =
                "Administra las sucursales y gestiona su ubicación, país y estado operativo.\r\n";

            //
            // LogisticaLblTituloSucursales
            //
            this.LogisticaLblTituloSucursales.AutoSize = true;

            this.LogisticaLblTituloSucursales.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblTituloSucursales.Font =
                new System.Drawing.Font(
                    "Lucida Fax",
                    20.25F,
                    System.Drawing.FontStyle.Bold,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.LogisticaLblTituloSucursales.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaLblTituloSucursales.Location =
                new System.Drawing.Point(19, 23);

            this.LogisticaLblTituloSucursales.Name =
                "LogisticaLblTituloSucursales";

            this.LogisticaLblTituloSucursales.Size =
                new System.Drawing.Size(167, 32);

            this.LogisticaLblTituloSucursales.TabIndex = 0;

            this.LogisticaLblTituloSucursales.Text =
                "Sucursales";

            //
            // FrmMantenimientoSucursales
            //
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(6F, 13F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize =
                new System.Drawing.Size(1144, 741);

            this.Controls.Add(this.LogisticaPnlSucursales);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Icon =
                ((System.Drawing.Icon)(
                    resources.GetObject("$this.Icon")));

            this.MaximizeBox = false;

            this.Name = "FrmMantenimientoSucursales";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "5001 -  Sucursales";

            this.LogisticaPnlSucursales.ResumeLayout(false);
            this.LogisticaPnlSucursales.PerformLayout();

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel
            LogisticaPnlSucursales;

        private System.Windows.Forms.Label
            LogisticaLblDescripcionSucursales;

        private System.Windows.Forms.Label
            LogisticaLblTituloSucursales;

        private CapaVista_Navegador.Navegador
            LogisticaNavSucursales;

        private System.Windows.Forms.Button
            LogisticaBtnBodega;
    }
}
