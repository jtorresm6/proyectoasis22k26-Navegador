using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_Compras
{
    public sealed class ClsPanelInformacionCompras : ClsPanelCompras
    {
        private float _OpacidadFondo = 0.9F;
        private int _RadioBorde = 16;

        public ClsPanelInformacionCompras()
        {
            BackColor = Color.Transparent;
            ForeColor = ClsTemaCompras.FondoGeneral;
        }

        [Category("Compras"), DefaultValue(0.9F)]
        public float OpacidadFondo
        {
            get { return _OpacidadFondo; }
            set { _OpacidadFondo = Math.Max(0F, Math.Min(1F, value)); Invalidate(true); }
        }

        [Category("Compras"), DefaultValue(16)]
        public int RadioBorde
        {
            get { return _RadioBorde; }
            set { _RadioBorde = Math.Max(0, value); Invalidate(true); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (Width < 3 || Height < 3) return;
            var area = new RectangleF(1F, 1F, Width - 2F, Height - 2F);
            float diametro = Math.Min(RadioBorde * 2F, Math.Min(area.Width, area.Height));
            using (var borde = new GraphicsPath())
            using (var fondo = new SolidBrush(Color.FromArgb((int)Math.Round(255 * OpacidadFondo),
                ClsTemaCompras.PrincipalNavegador)))
            using (var linea = new Pen(ClsTemaCompras.AcentoNavegador, 1F))
            {
                if (diametro <= 0) borde.AddRectangle(area);
                else
                {
                    borde.AddArc(area.Left, area.Top, diametro, diametro, 180, 90);
                    borde.AddArc(area.Right - diametro, area.Top, diametro, diametro, 270, 90);
                    borde.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90);
                    borde.AddArc(area.Left, area.Bottom - diametro, diametro, diametro, 90, 90);
                    borde.CloseFigure();
                }
                var estado = e.Graphics.Save();
                try
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(fondo, borde);
                    e.Graphics.DrawPath(linea, borde);
                }
                finally { e.Graphics.Restore(estado); }
            }
        }
    }
}
