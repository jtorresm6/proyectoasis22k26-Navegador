/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Panel reutilizable con esquinas redondeadas, relleno y borde de la
 *  paleta de Contabilidad. Sirve como tarjeta para agrupar campos.
 * ===================================================================
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    internal class ClsPanelRedondeadoContabilidad : Panel
    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    {
        private int _Radio = 18;
        private Color _ColorBorde = Color.FromArgb(212, 222, 228);
        private Color _ColorRelleno = Color.White;

        public ClsPanelRedondeadoContabilidad()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        [Category("Contabilidad"), DefaultValue(18)]
        public int Radio
        {
            get { return _Radio; }
            set { _Radio = Math.Max(0, value); Invalidate(); }
        }

        [Category("Contabilidad")]
        public Color ColorBorde
        {
            get { return _ColorBorde; }
            set { _ColorBorde = value; Invalidate(); }
        }

        [Category("Contabilidad")]
        public Color ColorRelleno
        {
            get { return _ColorRelleno; }
            set { _ColorRelleno = value; Invalidate(); }
        }

        private bool ShouldSerializeColorBorde() { return _ColorBorde != Color.FromArgb(212, 222, 228); }
        private bool ShouldSerializeColorRelleno() { return _ColorRelleno != Color.White; }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            base.OnPaintBackground(Evento);
            Graphics Lienzo = Evento.Graphics;
            Lienzo.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle Area = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath Ruta = ContabilidadFuncRuta(Area, _Radio))
            using (SolidBrush Relleno = new SolidBrush(_ColorRelleno))
            using (Pen Borde = new Pen(_ColorBorde, 2))
            {
                Lienzo.FillPath(Relleno, Ruta);
                Lienzo.DrawPath(Borde, Ruta);
            }
        }

        private static GraphicsPath ContabilidadFuncRuta(Rectangle Rectangulo, int Radio)
        {
            GraphicsPath Ruta = new GraphicsPath();
            if (Radio <= 0) { Ruta.AddRectangle(Rectangulo); return Ruta; }
            int Diametro = Radio * 2;
            Ruta.AddArc(Rectangulo.X, Rectangulo.Y, Diametro, Diametro, 180, 90);
            Ruta.AddArc(Rectangulo.Right - Diametro, Rectangulo.Y, Diametro, Diametro, 270, 90);
            Ruta.AddArc(Rectangulo.Right - Diametro, Rectangulo.Bottom - Diametro, Diametro, Diametro, 0, 90);
            Ruta.AddArc(Rectangulo.X, Rectangulo.Bottom - Diametro, Diametro, Diametro, 90, 90);
            Ruta.CloseFigure();
            return Ruta;
        }
    }
}
