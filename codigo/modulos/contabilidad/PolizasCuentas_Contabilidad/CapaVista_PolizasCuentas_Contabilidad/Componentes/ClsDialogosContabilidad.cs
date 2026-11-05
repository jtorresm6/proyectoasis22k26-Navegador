/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: diálogos de confirmación, éxito y error de EST-10 V1.1. */
using System;
using System.Drawing;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal static class ClsDialogosContabilidad
    {
        internal static void ContabilidadMetExito(IWin32Window Propietario, string Mensaje)
        {
            ContabilidadFuncMostrar(Propietario, "Validación completada", Mensaje, "✓", Color.FromArgb(34, 139, 70), false);
        }

        internal static void ContabilidadMetError(IWin32Window Propietario, string Mensaje)
        {
            ContabilidadFuncMostrar(Propietario, "Error", Mensaje, "×", Color.Firebrick, false);
        }

        internal static bool ContabilidadFuncConfirmar(IWin32Window Propietario, string Mensaje, string Accion)
        {
            return ContabilidadFuncMostrar(Propietario, "Confirmar acción", Mensaje, "!", Color.FromArgb(197, 143, 62), true, Accion) == DialogResult.Yes;
        }

        private static DialogResult ContabilidadFuncMostrar(IWin32Window Propietario, string Titulo, string Mensaje,
            string Simbolo, Color ColorEstado, bool Confirmar, string Accion = "Aceptar")
        {
            using (Form Dialogo = new Form())
            {
                Dialogo.Name = "FrmMensajeContabilidad";
                Dialogo.Text = "10008 - MensajeContabilidad";
                Dialogo.Icon = (Propietario as Form)?.Icon;
                Dialogo.ClientSize = new Size(540, 220);
                Dialogo.StartPosition = FormStartPosition.CenterParent;
                Dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                Dialogo.MaximizeBox = false;
                Dialogo.MinimizeBox = true;
                Dialogo.ShowInTaskbar = false;
                Dialogo.BackColor = Color.FromArgb(246, 247, 248);
                Label Icono = new Label { Name = "ContabilidadLblEstado", Text = Simbolo, ForeColor = ColorEstado,
                    Font = new Font("Segoe UI", 32F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(20, 24), Size = new Size(64, 72) };
                Label Texto = new Label { Name = "ContabilidadLblMensaje", Text = Mensaje, ForeColor = Color.FromArgb(35, 50, 61),
                    Font = new Font("Tahoma", 10F), Location = new Point(100, 62), Size = new Size(416, 90) };
                Label Encabezado = new Label { Name = "ContabilidadLblTitulo", Text = Titulo, ForeColor = Color.FromArgb(35, 50, 61),
                    Font = new Font("Lucida Fax", 16F, FontStyle.Bold | FontStyle.Italic), Location = new Point(100, 20), Size = new Size(416, 36) };
                ClsBotonContabilidad Primario = new ClsBotonContabilidad { Name = "ContabilidadBtnAceptar", Text = Accion,
                    DialogResult = Confirmar ? DialogResult.Yes : DialogResult.OK, Location = new Point(314, 164), Size = new Size(202, 40) };
                Dialogo.Controls.AddRange(new Control[] { Icono, Encabezado, Texto, Primario });
                Dialogo.AcceptButton = Primario;
                if (Confirmar)
                {
                    ClsBotonContabilidad Secundario = new ClsBotonContabilidad { Name = "ContabilidadBtnCancelar", Text = "No",
                        Estilo = EstiloBotonContabilidad.Secundario, DialogResult = DialogResult.No,
                        Location = new Point(100, 164), Size = new Size(190, 40) };
                    Dialogo.Controls.Add(Secundario);
                    Dialogo.CancelButton = Secundario;
                    Dialogo.ActiveControl = Secundario;
                    Dialogo.AcceptButton = Secundario;
                }
                else Dialogo.CancelButton = Primario;
                return Dialogo.ShowDialog(Propietario);
            }
        }
    }
}
