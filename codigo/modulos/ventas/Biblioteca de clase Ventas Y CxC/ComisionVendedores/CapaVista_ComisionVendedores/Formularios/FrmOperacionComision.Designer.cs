// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Declara los controles del diálogo de operación.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista_ComisionVendedores
{
    partial class FrmOperacionComision
    {
        private Panel VentasPnlEncabezado;
        private Label VentasLblTitulo;
        private Label VentasLblMensaje;
        private Button VentasBtnConfirmar;
        private Button VentasBtnCancelar;
        private Button VentasBtnCerrar;

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmOperacionComision));
            this.VentasPnlEncabezado = new Panel();
            this.VentasLblTitulo = new Label();
            this.VentasLblMensaje = new Label();
            this.VentasBtnConfirmar = new Button();
            this.VentasBtnCancelar = new Button();
            this.VentasBtnCerrar = new Button();
            this.SuspendLayout();
            this.VentasPnlEncabezado.Name = "VentasPnlEncabezado";
            this.VentasPnlEncabezado.Dock = DockStyle.Top;
            this.VentasPnlEncabezado.Height = 58;
            this.VentasPnlEncabezado.BackColor = Color.FromArgb(59, 29, 74);
            this.VentasLblTitulo.Name = "VentasLblTitulo";
            this.VentasLblTitulo.Text = "Confirmar operación";
            this.VentasLblTitulo.Location = new Point(20, 12);
            this.VentasLblTitulo.Size = new Size(540, 34);
            this.VentasLblTitulo.Font = new Font("Lucida Sans Unicode", 16F, FontStyle.Bold | FontStyle.Italic);
            this.VentasLblTitulo.ForeColor = Color.White;
            this.VentasLblMensaje.Name = "VentasLblMensaje";
            this.VentasLblMensaje.Text = "";
            this.VentasLblMensaje.Location = new Point(20, 70);
            this.VentasLblMensaje.Size = new Size(540, 58);
            this.VentasLblMensaje.TextAlign = ContentAlignment.MiddleLeft;
            this.VentasLblMensaje.Font = new Font("Tahoma", 10F);
            this.VentasPnlEncabezado.Controls.Add(this.VentasLblTitulo);
            this.Controls.Add(this.VentasPnlEncabezado);
            this.Controls.Add(this.VentasLblMensaje);
            this.VentasBtnConfirmar.Name = "VentasBtnConfirmar";
            this.VentasBtnConfirmar.Location = new Point(450, 136);
            this.VentasBtnConfirmar.Size = new Size(64, 64);
            this.VentasBtnConfirmar.DialogResult = DialogResult.Yes;
            this.VentasBtnConfirmar.Visible = true;
            this.VentasBtnConfirmar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalGuardarVentas")));
            this.VentasBtnConfirmar.BackgroundImageLayout = ImageLayout.Stretch;
            this.VentasBtnConfirmar.FlatStyle = FlatStyle.Flat;
            this.VentasBtnConfirmar.FlatAppearance.BorderSize = 0;
            this.VentasBtnConfirmar.UseVisualStyleBackColor = false;
            this.VentasBtnConfirmar.Text = "";
            this.VentasBtnConfirmar.AccessibleName = "Confirmar";
            this.Controls.Add(this.VentasBtnConfirmar);
            this.VentasBtnCancelar.Name = "VentasBtnCancelar";
            this.VentasBtnCancelar.Location = new Point(370, 136);
            this.VentasBtnCancelar.Size = new Size(64, 64);
            this.VentasBtnCancelar.DialogResult = DialogResult.No;
            this.VentasBtnCancelar.Visible = true;
            this.VentasBtnCancelar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalCancelarVentas")));
            this.VentasBtnCancelar.BackgroundImageLayout = ImageLayout.Stretch;
            this.VentasBtnCancelar.FlatStyle = FlatStyle.Flat;
            this.VentasBtnCancelar.FlatAppearance.BorderSize = 0;
            this.VentasBtnCancelar.UseVisualStyleBackColor = false;
            this.VentasBtnCancelar.Text = "";
            this.VentasBtnCancelar.AccessibleName = "Cancelar";
            this.Controls.Add(this.VentasBtnCancelar);
            this.VentasBtnCerrar.Name = "VentasBtnCerrar";
            this.VentasBtnCerrar.Location = new Point(450, 136);
            this.VentasBtnCerrar.Size = new Size(64, 64);
            this.VentasBtnCerrar.DialogResult = DialogResult.OK;
            this.VentasBtnCerrar.Visible = false;
            this.VentasBtnCerrar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BotonOriginalSalirVentas")));
            this.VentasBtnCerrar.BackgroundImageLayout = ImageLayout.Stretch;
            this.VentasBtnCerrar.FlatStyle = FlatStyle.Flat;
            this.VentasBtnCerrar.FlatAppearance.BorderSize = 0;
            this.VentasBtnCerrar.UseVisualStyleBackColor = false;
            this.VentasBtnCerrar.Text = "";
            this.VentasBtnCerrar.AccessibleName = "Cerrar";
            this.Controls.Add(this.VentasBtnCerrar);
            this.AcceptButton = this.VentasBtnConfirmar;
            this.CancelButton = this.VentasBtnCancelar;
            this.ClientSize = new Size(580, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(248, 245, 249);
            this.Icon = ((Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmOperacionComision";
            this.Text = "7008 - OperacionComision";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
