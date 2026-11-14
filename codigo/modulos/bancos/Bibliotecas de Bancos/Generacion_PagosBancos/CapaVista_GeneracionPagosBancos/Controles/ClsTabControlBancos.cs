using System.Drawing;
using System.Windows.Forms;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 05/11/2026
 * ================================================================
 * Propósito :
 *  Control de pestañas con el diseño del módulo Bancos. Se dibuja
 *  dentro del propio control, por eso el aspecto se ve igual en el
 *  diseñador del formulario y al ejecutar la aplicación.
 * ================================================================
 */

namespace CapaVista_GeneracionPagos.Controles
{
    public class ClsTabControlBancos : TabControl
    {
        private static readonly Color _VerdeOscuro = Color.FromArgb(28, 74, 48);
        private static readonly Color _VerdeBorde = Color.FromArgb(46, 138, 85);
        private static readonly Color _VerdeClaro = Color.FromArgb(212, 231, 211);
        private static readonly Color _VerdeSobre = Color.FromArgb(190, 217, 189);
        private static readonly Color _TextoInactivo = Color.FromArgb(50, 70, 58);

        private int _PestanaSobre = -1;

        public ClsTabControlBancos()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(170, 34);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics Lienzo = e.Graphics;
            Color Fondo = Parent != null ? Parent.BackColor : SystemColors.Control;
            Lienzo.Clear(Fondo);

            if (TabCount == 0)
                return;

            int BordeInferior = GetTabRect(0).Bottom;
            var Pagina = new Rectangle(0, BordeInferior, Width - 1, Height - 1 - BordeInferior);
            using (var Pincel = new SolidBrush(Color.White))
                Lienzo.FillRectangle(Pincel, Pagina);
            using (var Lapiz = new Pen(_VerdeBorde))
                Lienzo.DrawRectangle(Lapiz, Pagina);

            for (int Indice = 0; Indice < TabCount; Indice++)
                BancosMetPintarPestana(Lienzo, Indice, BordeInferior);
        }

        private void BancosMetPintarPestana(Graphics Lienzo, int Indice, int BordeInferior)
        {
            Rectangle Area = GetTabRect(Indice);
            bool Activa = Indice == SelectedIndex;
            Color ColorFondo = Activa ? Color.White : Indice == _PestanaSobre ? _VerdeSobre : _VerdeClaro;

            using (var Pincel = new SolidBrush(ColorFondo))
                Lienzo.FillRectangle(Pincel, Area);

            using (var Lapiz = new Pen(_VerdeBorde))
            {
                Lienzo.DrawLine(Lapiz, Area.Left, Area.Top, Area.Right - 1, Area.Top);
                Lienzo.DrawLine(Lapiz, Area.Left, Area.Top, Area.Left, Area.Bottom);
                Lienzo.DrawLine(Lapiz, Area.Right - 1, Area.Top, Area.Right - 1, Area.Bottom);
            }

            if (Activa)
            {
                using (var Pincel = new SolidBrush(Color.White))
                    Lienzo.FillRectangle(Pincel, Area.Left + 1, BordeInferior, Area.Width - 2, 1);
            }

            using (var Fuente = new Font("Segoe UI", 10F, Activa ? FontStyle.Bold : FontStyle.Regular))
            {
                TextRenderer.DrawText(Lienzo, TabPages[Indice].Text, Fuente, Area, Activa ? _VerdeOscuro : _TextoInactivo,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            }
        }

        protected override void OnSelectedIndexChanged(System.EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int Sobre = -1;
            for (int Indice = 0; Indice < TabCount; Indice++)
            {
                if (GetTabRect(Indice).Contains(e.Location))
                {
                    Sobre = Indice;
                    break;
                }
            }

            if (Sobre != _PestanaSobre)
            {
                _PestanaSobre = Sobre;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_PestanaSobre != -1)
            {
                _PestanaSobre = -1;
                Invalidate();
            }
        }
    }
}
