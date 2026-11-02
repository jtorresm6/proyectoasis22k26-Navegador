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
// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
using CapaVista_Contabilidad.Properties;
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal static class ClsPresentacionContabilidad
    {
        private static readonly Icon _IconoContabilidad = Properties.Resources.Icono_Contabilidad;

        private static readonly System.Collections.Generic.Dictionary<string, int> _Codigos = new System.Collections.Generic.Dictionary<string, int>
        {
            { "FrmEstadosFinancieros", 10005 }
        };

        internal static void ContabilidadMetPrepararVentana(Form Formulario, string Titulo, Size AreaCliente)
        {
            Formulario.AutoScaleMode = AutoScaleMode.None;
            Formulario.ClientSize = AreaCliente;
            string NombreVentana = Formulario.GetType().Name.Substring(3);
            int Codigo = _Codigos[Formulario.GetType().Name];
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