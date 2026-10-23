using System.Drawing;
using System.Windows.Forms;

namespace Ventas.Estandarizacion
{
    /// <summary>Dialogos modales de Ventas con los estados visuales definidos por EST-10.</summary>
    public static class ClsDialogosVentas
    {
        private static readonly Color _ColorPrincipal = Color.FromArgb(59, 29, 74);
        private static readonly Color _ColorFondo = Color.FromArgb(248, 245, 249);
        private static readonly Color _ColorExito = Color.FromArgb(46, 125, 50);
        private static readonly Color _ColorAdvertencia = Color.FromArgb(201, 162, 74);
        private static readonly Color _ColorError = Color.FromArgb(179, 33, 33);

        public static DialogResult VentasMetMostrarExito(IWin32Window Propietario, string Mensaje, string Titulo = "Ventas")
        {
            return VentasMetMostrar(Propietario, Mensaje, Titulo, "✓", _ColorExito, MessageBoxButtons.OK);
        }

        public static DialogResult VentasMetMostrarAdvertencia(IWin32Window Propietario, string Mensaje, string Titulo)
        {
            return VentasMetMostrar(Propietario, Mensaje, Titulo, "!", _ColorAdvertencia, MessageBoxButtons.OK);
        }

        public static DialogResult VentasMetMostrarError(IWin32Window Propietario, string Mensaje, string Titulo)
        {
            return VentasMetMostrar(Propietario, Mensaje, Titulo, "×", _ColorError, MessageBoxButtons.OK);
        }

        public static DialogResult VentasMetConfirmar(IWin32Window Propietario, string Mensaje, string Titulo)
        {
            return VentasMetMostrar(Propietario, Mensaje, Titulo, "!", _ColorAdvertencia, MessageBoxButtons.YesNo);
        }

        private static DialogResult VentasMetMostrar(IWin32Window Propietario, string Mensaje, string Titulo, string Simbolo, Color ColorEstado, MessageBoxButtons Botones)
        {
            using (Form Ventana = new Form())
            using (Panel Barra = new Panel())
            using (Label EtiquetaSimbolo = new Label())
            using (Label EtiquetaMensaje = new Label())
            using (Button BotonPrincipal = new Button())
            using (Button BotonSecundario = new Button())
            {
                Ventana.Text = Titulo ?? "Ventas";
                Ventana.ClientSize = new Size(520, 205);
                Ventana.BackColor = _ColorFondo;
                Ventana.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                Ventana.FormBorderStyle = FormBorderStyle.FixedDialog;
                Ventana.MaximizeBox = false;
                Ventana.MinimizeBox = false;
                Ventana.ShowIcon = false;
                Ventana.ShowInTaskbar = false;
                Ventana.StartPosition = Propietario == null ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
                Ventana.AutoScaleMode = AutoScaleMode.Font;

                Barra.BackColor = _ColorPrincipal;
                Barra.Dock = DockStyle.Top;
                Barra.Height = 6;

                EtiquetaSimbolo.AutoSize = false;
                EtiquetaSimbolo.Bounds = new Rectangle(28, 47, 56, 64);
                EtiquetaSimbolo.Font = new Font("Segoe UI Symbol", 34F, FontStyle.Bold);
                EtiquetaSimbolo.ForeColor = ColorEstado;
                EtiquetaSimbolo.Text = Simbolo;
                EtiquetaSimbolo.TextAlign = ContentAlignment.MiddleCenter;

                EtiquetaMensaje.AutoEllipsis = true;
                EtiquetaMensaje.Bounds = new Rectangle(100, 45, 390, 82);
                EtiquetaMensaje.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
                EtiquetaMensaje.ForeColor = _ColorPrincipal;
                EtiquetaMensaje.Text = Mensaje ?? string.Empty;
                EtiquetaMensaje.TextAlign = ContentAlignment.MiddleLeft;

                BotonPrincipal.BackColor = _ColorPrincipal;
                BotonPrincipal.FlatStyle = FlatStyle.Flat;
                BotonPrincipal.FlatAppearance.BorderSize = 0;
                BotonPrincipal.ForeColor = Color.White;
                BotonPrincipal.Size = new Size(112, 38);
                BotonPrincipal.Location = new Point(Botones == MessageBoxButtons.YesNo ? 270 : 370, 146);
                BotonPrincipal.Text = Botones == MessageBoxButtons.YesNo ? "Sí" : "Aceptar";
                BotonPrincipal.DialogResult = Botones == MessageBoxButtons.YesNo ? DialogResult.Yes : DialogResult.OK;

                if (Botones == MessageBoxButtons.YesNo)
                {
                    BotonSecundario.BackColor = _ColorFondo;
                    BotonSecundario.FlatStyle = FlatStyle.Flat;
                    BotonSecundario.FlatAppearance.BorderColor = _ColorPrincipal;
                    BotonSecundario.ForeColor = _ColorPrincipal;
                    BotonSecundario.Size = new Size(112, 38);
                    BotonSecundario.Location = new Point(390, 146);
                    BotonSecundario.Text = "No";
                    BotonSecundario.DialogResult = DialogResult.No;
                    Ventana.Controls.Add(BotonSecundario);
                    Ventana.CancelButton = BotonSecundario;
                }
                else
                {
                    Ventana.CancelButton = BotonPrincipal;
                }

                Ventana.Controls.Add(Barra);
                Ventana.Controls.Add(EtiquetaSimbolo);
                Ventana.Controls.Add(EtiquetaMensaje);
                Ventana.Controls.Add(BotonPrincipal);
                Ventana.AcceptButton = BotonPrincipal;
                return Propietario == null ? Ventana.ShowDialog() : Ventana.ShowDialog(Propietario);
            }
        }
    }
}