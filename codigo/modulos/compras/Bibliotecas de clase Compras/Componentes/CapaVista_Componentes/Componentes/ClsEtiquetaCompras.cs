using System;
using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Proporciona etiquetas con la tipografía y los colores comunes de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public sealed class ClsEtiquetaCompras : Label
    {
        internal bool ComprasConservarRojo { get; set; }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            if (!ComprasConservarRojo)
            {
                base.OnPaint(Evento);
                return;
            }
            TextFormatFlags Formato = TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix | TextFormatFlags.TextBoxControl;
            if (TextAlign == ContentAlignment.MiddleLeft) Formato |= TextFormatFlags.VerticalCenter;
            Rectangle Area = new Rectangle(Padding.Left, Padding.Top, Math.Max(0, ClientSize.Width - Padding.Horizontal), Math.Max(0, ClientSize.Height - Padding.Vertical));
            TextRenderer.DrawText(Evento.Graphics, Text, Font, Area, Color.Red, Formato);
        }
    }
}
