/* Área: Contabilidad. Autor: Gabriel André Guillén Pocón. Carné: 0901-23-1998.
 * Fecha: 14/10/2026. Propósito: asterisco obligatorio, borde rojo y error bajo el campo (EST-10 §7). */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    internal sealed class ClsValidacionContabilidad : IDisposable
    {
        private readonly Dictionary<Control, Label> _Errores = new Dictionary<Control, Label>();
        private readonly Dictionary<Control, Control[]> _Bordes = new Dictionary<Control, Control[]>();

        internal void ContabilidadMetObligatorio(Control Campo)
        {
            Label Etiqueta = Campo.Parent.Controls.OfType<Label>()
                .Where(Elemento => Elemento.Bottom <= Campo.Top && Math.Abs(Elemento.Left - Campo.Left) <= 2)
                .OrderByDescending(Elemento => Elemento.Bottom).FirstOrDefault();
            if (Etiqueta == null) return;
            string Nombre = "ContabilidadLblObligatorio" + Campo.Name;
            if (Campo.Parent.Controls.ContainsKey(Nombre)) return;
            int AnchoTexto = TextRenderer.MeasureText(Etiqueta.Text, Etiqueta.Font).Width;
            Etiqueta.AutoSize = false;
            Etiqueta.Width = AnchoTexto + 10;
            Label Asterisco = new Label { Name = Nombre, Text = "*", AutoSize = true, ForeColor = Color.Firebrick,
                BackColor = Color.Transparent, Font = new Font("Tahoma", 10F, FontStyle.Bold),
                Location = new Point(Etiqueta.Right + 2, Etiqueta.Top) };
            Campo.Parent.Controls.Add(Asterisco);
            Asterisco.BringToFront();
        }

        internal bool ContabilidadFuncValidar(Control Campo, string Mensaje)
        {
            Label Error;
            if (!_Errores.TryGetValue(Campo, out Error))
            {
                Error = new Label { Name = "ContabilidadLblError" + Campo.Name, ForeColor = Color.Firebrick,
                    BackColor = Color.Transparent, Font = new Font("Tahoma", 8F), AutoEllipsis = true,
                    Location = new Point(Campo.Left, Campo.Bottom + 3), Size = new Size(Campo.Width, 18), Visible = false };
                Campo.Parent.Controls.Add(Error);
                _Errores.Add(Campo, Error);
                Rectangle[] Lineas =
                {
                    new Rectangle(Campo.Left - 2, Campo.Top - 2, Campo.Width + 4, 2),
                    new Rectangle(Campo.Left - 2, Campo.Bottom, Campo.Width + 4, 2),
                    new Rectangle(Campo.Left - 2, Campo.Top, 2, Campo.Height),
                    new Rectangle(Campo.Right, Campo.Top, 2, Campo.Height)
                };
                Control[] Bordes = new Control[Lineas.Length];
                for (int Indice = 0; Indice < Lineas.Length; Indice++)
                {
                    Bordes[Indice] = new Label { Name = "ContabilidadLblBorde" + Campo.Name + Indice,
                        BackColor = Color.Firebrick, Bounds = Lineas[Indice], Visible = false };
                    Campo.Parent.Controls.Add(Bordes[Indice]);
                    Bordes[Indice].BringToFront();
                }
                _Bordes.Add(Campo, Bordes);
                if (Campo is DateTimePicker) ((DateTimePicker)Campo).ValueChanged += ContabilidadMetCorregir;
                else if (Campo is NumericUpDown) ((NumericUpDown)Campo).ValueChanged += ContabilidadMetCorregir;
                else if (Campo is ComboBox) ((ComboBox)Campo).SelectedIndexChanged += ContabilidadMetCorregir;
                else Campo.TextChanged += ContabilidadMetCorregir;
            }
            Error.Text = Mensaje ?? string.Empty;
            Error.Visible = !string.IsNullOrEmpty(Mensaje);
            foreach (Control Borde in _Bordes[Campo]) Borde.Visible = !string.IsNullOrEmpty(Mensaje);
            Error.BringToFront();
            Campo.Parent.Invalidate();
            return string.IsNullOrEmpty(Mensaje);
        }

        private void ContabilidadMetCorregir(object Remitente, EventArgs Argumentos)
        {
            Control Campo = (Control)Remitente;
            _Errores[Campo].Visible = false;
            foreach (Control Borde in _Bordes[Campo]) Borde.Visible = false;
            Campo.Parent.Invalidate();
        }

        internal void ContabilidadMetLimpiar()
        {
            foreach (Label Error in _Errores.Values) Error.Visible = false;
            foreach (Control[] Bordes in _Bordes.Values)
                foreach (Control Borde in Bordes) Borde.Visible = false;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<Control, Label> Entrada in _Errores)
            {
                Control Campo = Entrada.Key;
                if (Campo is DateTimePicker) ((DateTimePicker)Campo).ValueChanged -= ContabilidadMetCorregir;
                else if (Campo is NumericUpDown) ((NumericUpDown)Campo).ValueChanged -= ContabilidadMetCorregir;
                else if (Campo is ComboBox) ((ComboBox)Campo).SelectedIndexChanged -= ContabilidadMetCorregir;
                else Campo.TextChanged -= ContabilidadMetCorregir;
                Entrada.Value.Dispose();
                foreach (Control Borde in _Bordes[Campo]) Borde.Dispose();
            }
            _Errores.Clear();
            _Bordes.Clear();
        }
    }
}
