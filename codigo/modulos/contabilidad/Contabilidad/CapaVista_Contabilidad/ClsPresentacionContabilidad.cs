/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026
 * Propósito: reglas de presentación comunes de las ventanas del módulo (EST-10 V1.1).
 * Esta clase pertenece a la Vista; no contiene operaciones de negocio ni datos.
 */
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista_Contabilidad
{
    internal static class ClsPresentacionContabilidad
    {
        private static readonly Icon _IconoContabilidad = (Icon)new ComponentResourceManager(
            typeof(FrmMDIContabilidad)).GetObject("$this.Icon");

        // Ventana hija de tamaño fijo, centrada sobre el MDI y con el ícono del módulo.
        internal static void ContabilidadMetPrepararVentana(Form Formulario, string Titulo, Size AreaCliente)
        {
            Formulario.AutoScaleMode = AutoScaleMode.None;
            Formulario.ClientSize = AreaCliente;
            string NombreVentana = Formulario.GetType().Name.Substring(3);
            int Codigo = Formulario is FrmPolizasLocales ? 10001 : Formulario is FrmPresupuestos ? 10002
                : Formulario is FrmActivosFijos ? 10003 : Formulario is FrmCierreContable ? 10004 : 10005;
            Formulario.Text = Codigo + " - " + NombreVentana;
            Formulario.Icon = _IconoContabilidad;
            Formulario.MaximizeBox = false;
            Formulario.MinimizeBox = true;
            Formulario.FormBorderStyle = FormBorderStyle.FixedSingle;
            Formulario.StartPosition = FormStartPosition.CenterParent;
            Formulario.BackColor = Color.FromArgb(246, 247, 248);
            Formulario.Font = new Font("Segoe UI", 10F);
            ContabilidadMetAplicarTipografia(Formulario);
        }

        internal static void ContabilidadMetAplicarTipografia(Control Contenedor)
        {
            foreach (Control Elemento in Contenedor.Controls)
            {
                if (Elemento is Label && !Elemento.Font.FontFamily.Name.StartsWith("Lucida"))
                    Elemento.Font = new Font("Tahoma", Elemento.Font.Size, Elemento.Font.Style);
                if (Elemento is TextBox || Elemento is ComboBox || Elemento is NumericUpDown || Elemento is DateTimePicker)
                    Elemento.Font = new Font("Segoe UI", Elemento.Font.Size, FontStyle.Regular);
                ContabilidadMetAplicarTipografia(Elemento);
            }
        }
    }
}
