/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Botón reutilizable del módulo: esquinas redondeadas, ícono opcional
 *  a la izquierda del texto y cuatro estilos con la paleta de
 *  Contabilidad. Se arrastra desde la caja de herramientas igual que
 *  los controles del componente de Consultas.
 * ===================================================================
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

// Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
namespace CapaVista_Contabilidad.Componentes
// Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
{
    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    internal enum EstiloBotonContabilidad
    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    {
        Principal,
        Secundario,
        Acento,
        Dorado
    }

    // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    internal class ClsBotonContabilidad : Button
    // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
    {
        private EstiloBotonContabilidad _Estilo = EstiloBotonContabilidad.Principal;
        private Image _Icono;
        private int _Radio = 10;
        private bool _Sobre;
        private bool _Presionado;

        public ClsBotonContabilidad()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Size = new Size(150, 40);
        }

        [Category("Contabilidad"), DefaultValue(EstiloBotonContabilidad.Principal)]
        public EstiloBotonContabilidad Estilo
        {
            get { return _Estilo; }
            set { _Estilo = value; Invalidate(); }
        }

        [Category("Contabilidad"), DefaultValue(null)]
        public Image Icono
        {
            get { return _Icono; }
            set { _Icono = value; Invalidate(); }
        }

        [Category("Contabilidad"), DefaultValue(10)]
        public int Radio
        {
            get { return _Radio; }
            set { _Radio = Math.Max(0, value); Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs Evento) { _Sobre = true; Invalidate(); base.OnMouseEnter(Evento); }
        protected override void OnMouseLeave(EventArgs Evento) { _Sobre = false; _Presionado = false; Invalidate(); base.OnMouseLeave(Evento); }
        protected override void OnMouseDown(MouseEventArgs Evento) { _Presionado = true; Invalidate(); base.OnMouseDown(Evento); }
        protected override void OnMouseUp(MouseEventArgs Evento) { _Presionado = false; Invalidate(); base.OnMouseUp(Evento); }
        protected override void OnEnabledChanged(EventArgs Evento) { Invalidate(); base.OnEnabledChanged(Evento); }

        private Color ContabilidadFuncColorRelleno()
        {
            if (!Enabled) return Color.FromArgb(230, 235, 239);
            Color Base, Sobre, Presion;
            switch (_Estilo)
            {
                case EstiloBotonContabilidad.Secundario:
                    Base = Color.FromArgb(212, 222, 228); Sobre = Color.FromArgb(195, 209, 218); Presion = Color.FromArgb(176, 193, 205); break;
                case EstiloBotonContabilidad.Acento:
                    Base = Color.FromArgb(70, 104, 125); Sobre = Color.FromArgb(88, 123, 145); Presion = Color.FromArgb(52, 77, 93); break;
                case EstiloBotonContabilidad.Dorado:
                    Base = Color.FromArgb(197, 143, 62); Sobre = Color.FromArgb(214, 160, 80); Presion = Color.FromArgb(169, 120, 50); break;
                default:
                    Base = Color.FromArgb(35, 50, 61); Sobre = Color.FromArgb(52, 77, 93); Presion = Color.FromArgb(28, 40, 49); break;
            }
            return _Presionado ? Presion : (_Sobre ? Sobre : Base);
        }

        private Color ContabilidadFuncColorTexto()
        {
            if (!Enabled) return Color.FromArgb(154, 169, 180);
            return _Estilo == EstiloBotonContabilidad.Secundario ? Color.FromArgb(35, 50, 61) : Color.White;
        }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            Graphics Lienzo = Evento.Graphics;
            Lienzo.SmoothingMode = SmoothingMode.AntiAlias;
            Lienzo.Clear(ContabilidadFuncColorFondo());

            Rectangle Area = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath Ruta = ContabilidadFuncRuta(Area, _Radio))
            using (SolidBrush Relleno = new SolidBrush(ContabilidadFuncColorRelleno()))
                Lienzo.FillPath(Relleno, Ruta);

            Color ColorTexto = ContabilidadFuncColorTexto();
            SizeF Medida = Lienzo.MeasureString(Text, Font);
            int TamanoIcono = _Icono == null ? 0 : Math.Min(22, Height - 16);
            int Separacion = (_Icono != null && Text.Length > 0) ? 8 : 0;
            float Total = TamanoIcono + Separacion + Medida.Width;
            float X = (Width - Total) / 2f;

            if (_Icono != null)
            {
                Lienzo.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle Destino = new Rectangle((int)X, (Height - TamanoIcono) / 2, TamanoIcono, TamanoIcono);
                if (Enabled) Lienzo.DrawImage(_Icono, Destino);
                else
                {
                    using (ImageAttributes Atributos = new ImageAttributes())
                    {
                        Atributos.SetColorMatrix(new ColorMatrix(new float[][]
                        {
                            new float[] { 0, 0, 0, 0, 0 },
                            new float[] { 0, 0, 0, 0, 0 },
                            new float[] { 0, 0, 0, 0, 0 },
                            new float[] { 0, 0, 0, 1, 0 },
                            new float[] { 154 / 255f, 169 / 255f, 180 / 255f, 0, 1 }
                        }));
                        Lienzo.DrawImage(_Icono, Destino, 0, 0, _Icono.Width, _Icono.Height, GraphicsUnit.Pixel, Atributos);
                    }
                }
                X += TamanoIcono + Separacion;
            }
            using (SolidBrush Pincel = new SolidBrush(ColorTexto))
                Lienzo.DrawString(Text, Font, Pincel, X, (Height - Medida.Height) / 2f);
        }

        private Color ContabilidadFuncColorFondo()
        {
            for (Control Padre = Parent; Padre != null; Padre = Padre.Parent)
            {
                ClsPanelRedondeadoContabilidad Tarjeta = Padre as ClsPanelRedondeadoContabilidad;
                if (Tarjeta != null) return Tarjeta.ColorRelleno;
                if (Padre.BackColor.A == 255) return Padre.BackColor;
            }
            return SystemColors.Control;
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
