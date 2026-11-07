
namespace CapaVista_Inventario_Logistica.Formularios
{
    partial class FrmMantenimientoProducto
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
                    typeof(FrmMantenimientoProducto));

            this.LogisticaPnlProducto =
                new System.Windows.Forms.Panel();

            this.LogisticaBtnLinea =
                new System.Windows.Forms.Button();

            this.LogisticaBtnMarca =
                new System.Windows.Forms.Button();

            this.LogisticaBtnCategoria =
                new System.Windows.Forms.Button();

            this.LogisticaNavProducto =
                new CapaVista_Navegador.Navegador();

            this.LogisticaLblDescripcionProducto =
                new System.Windows.Forms.Label();

            this.LogisticaLblTituloProducto =
                new System.Windows.Forms.Label();

            this.LogisticaPnlProducto.SuspendLayout();
            this.SuspendLayout();

            //
            // LogisticaPnlProducto
            //
            this.LogisticaPnlProducto.BackgroundImage =
                global::CapaVista_Inventario_Logistica
                .Properties.Resources.FondoLogica2;

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaBtnLinea);

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaBtnMarca);

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaBtnCategoria);

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaNavProducto);

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaLblDescripcionProducto);

            this.LogisticaPnlProducto.Controls.Add(
                this.LogisticaLblTituloProducto);

            this.LogisticaPnlProducto.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.LogisticaPnlProducto.Font =
                new System.Drawing.Font("Segoe UI", 8.25F);

            this.LogisticaPnlProducto.Location =
                new System.Drawing.Point(0, 0);

            this.LogisticaPnlProducto.Name =
                "LogisticaPnlProducto";

            this.LogisticaPnlProducto.Size =
                new System.Drawing.Size(1144, 741);

            this.LogisticaPnlProducto.TabIndex = 0;

            //
            // LogisticaBtnLinea
            //
            this.LogisticaBtnLinea.BackColor =
                System.Drawing.Color.FromArgb(242, 216, 197);

            this.LogisticaBtnLinea.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.LogisticaBtnLinea.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(210, 99, 46);

            this.LogisticaBtnLinea.FlatAppearance.BorderSize = 2;

            this.LogisticaBtnLinea.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.LogisticaBtnLinea.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    12.75F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.LogisticaBtnLinea.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaBtnLinea.Location =
                new System.Drawing.Point(1020, 30);

            this.LogisticaBtnLinea.Name =
                "LogisticaBtnLinea";

            this.LogisticaBtnLinea.Size =
                new System.Drawing.Size(95, 52);

            this.LogisticaBtnLinea.TabIndex = 5;

            this.LogisticaBtnLinea.Text = "Línea";

            this.LogisticaBtnLinea.UseVisualStyleBackColor = false;

            this.LogisticaBtnLinea.Click +=
                new System.EventHandler(
                    this.LogisticaBtnLinea_Click);

            //
            // LogisticaBtnMarca
            //
            this.LogisticaBtnMarca.BackColor =
                System.Drawing.Color.FromArgb(242, 216, 197);

            this.LogisticaBtnMarca.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.LogisticaBtnMarca.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(210, 99, 46);

            this.LogisticaBtnMarca.FlatAppearance.BorderSize = 2;

            this.LogisticaBtnMarca.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.LogisticaBtnMarca.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    12.75F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.LogisticaBtnMarca.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaBtnMarca.Location =
                new System.Drawing.Point(909, 30);

            this.LogisticaBtnMarca.Name =
                "LogisticaBtnMarca";

            this.LogisticaBtnMarca.Size =
                new System.Drawing.Size(95, 52);

            this.LogisticaBtnMarca.TabIndex = 4;

            this.LogisticaBtnMarca.Text = "Marca";

            this.LogisticaBtnMarca.UseVisualStyleBackColor = false;

            this.LogisticaBtnMarca.Click +=
                new System.EventHandler(
                    this.LogisticaBtnMarca_Click);

            //
            // LogisticaBtnCategoria
            //
            this.LogisticaBtnCategoria.BackColor =
                System.Drawing.Color.FromArgb(242, 216, 197);

            this.LogisticaBtnCategoria.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.LogisticaBtnCategoria.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(210, 99, 46);

            this.LogisticaBtnCategoria.FlatAppearance.BorderSize = 2;

            this.LogisticaBtnCategoria.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.LogisticaBtnCategoria.Font =
                new System.Drawing.Font(
                    "Tahoma",
                    12.75F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0)));

            this.LogisticaBtnCategoria.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaBtnCategoria.Location =
                new System.Drawing.Point(795, 30);

            this.LogisticaBtnCategoria.Name =
                "LogisticaBtnCategoria";

            this.LogisticaBtnCategoria.Size =
                new System.Drawing.Size(95, 52);

            this.LogisticaBtnCategoria.TabIndex = 3;

            this.LogisticaBtnCategoria.Text = "Categoría";

            this.LogisticaBtnCategoria.UseVisualStyleBackColor = false;

            this.LogisticaBtnCategoria.Click +=
                new System.EventHandler(
                    this.LogisticaBtnCategoria_Click);

            //
            // LogisticaNavProducto
            //
            this.LogisticaNavProducto.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaNavProducto.Location =
                new System.Drawing.Point(37, 127);

            this.LogisticaNavProducto.Margin =
                new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.LogisticaNavProducto.Name =
                "LogisticaNavProducto";

            this.LogisticaNavProducto.Size =
                new System.Drawing.Size(1078, 90);

            this.LogisticaNavProducto.TabIndex = 2;

            //
            // LogisticaLblDescripcionProducto
            //
            this.LogisticaLblDescripcionProducto.AutoSize = true;

            this.LogisticaLblDescripcionProducto.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblDescripcionProducto.Font =
                new System.Drawing.Font("Tahoma", 12.75F);

            this.LogisticaLblDescripcionProducto.ForeColor =
                System.Drawing.Color.Black;

            this.LogisticaLblDescripcionProducto.Location =
                new System.Drawing.Point(33, 83);

            this.LogisticaLblDescripcionProducto.Name =
                "LogisticaLblDescripcionProducto";

            this.LogisticaLblDescripcionProducto.Size =
                new System.Drawing.Size(435, 42);

            this.LogisticaLblDescripcionProducto.TabIndex = 1;

            this.LogisticaLblDescripcionProducto.Text =
                "Administra el catálogo de productos, sus características, \r\n" +
                "precios y clasificación comercial.";

            //
            // LogisticaLblTituloProducto
            //
            this.LogisticaLblTituloProducto.AutoSize = true;

            this.LogisticaLblTituloProducto.BackColor =
                System.Drawing.Color.Transparent;

            this.LogisticaLblTituloProducto.Font =
                new System.Drawing.Font(
                    "Lucida Fax",
                    20.25F,
                    System.Drawing.FontStyle.Bold);

            this.LogisticaLblTituloProducto.ForeColor =
                System.Drawing.Color.FromArgb(62, 138, 132);

            this.LogisticaLblTituloProducto.Location =
                new System.Drawing.Point(31, 30);

            this.LogisticaLblTituloProducto.Name =
                "LogisticaLblTituloProducto";

            this.LogisticaLblTituloProducto.Size =
                new System.Drawing.Size(145, 32);

            this.LogisticaLblTituloProducto.TabIndex = 0;

            this.LogisticaLblTituloProducto.Text = "Producto";

            //
            // FrmMantenimientoProducto
            //
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(6F, 13F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(251, 246, 242);

            this.ClientSize =
                new System.Drawing.Size(1144, 741);

            this.Controls.Add(this.LogisticaPnlProducto);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Icon =
                ((System.Drawing.Icon)(
                    resources.GetObject("$this.Icon")));

            this.MaximizeBox = false;

            this.Name = "FrmMantenimientoProducto";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text = "5006 - Producto";

            this.LogisticaPnlProducto.ResumeLayout(false);
            this.LogisticaPnlProducto.PerformLayout();

            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel
            LogisticaPnlProducto;

        private CapaVista_Navegador.Navegador
            LogisticaNavProducto;

        private System.Windows.Forms.Label
            LogisticaLblDescripcionProducto;

        private System.Windows.Forms.Label
            LogisticaLblTituloProducto;

        private System.Windows.Forms.Button
            LogisticaBtnLinea;

        private System.Windows.Forms.Button
            LogisticaBtnMarca;

        private System.Windows.Forms.Button
            LogisticaBtnCategoria;
    }
}
