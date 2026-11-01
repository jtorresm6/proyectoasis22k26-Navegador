/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón
 * Carné: 0901-23-1998
 * Fecha: 10/10/2026
 * Propósito: presentar los accesos del Controlador en el menú del MDI.
 * Reproduce la matriz de gris del Navegador, cuyo método es privado.
 * Conserva los originales y libera únicamente las imágenes que crea.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CapaVista_Contabilidad
{
    public sealed class ClsPermisosMenuContabilidad : IDisposable
    {
        private readonly Dictionary<Button, Image> _Originales = new Dictionary<Button, Image>();
        private readonly Dictionary<Button, Image> _Grises = new Dictionary<Button, Image>();

        public void ContabilidadMetAplicar(Button Boton, bool TieneAcceso)
        {
            if (!_Originales.ContainsKey(Boton)) _Originales.Add(Boton, Boton.BackgroundImage);
            if (!TieneAcceso && !_Grises.ContainsKey(Boton) && _Originales[Boton] != null)
                _Grises.Add(Boton, ContabilidadFuncConvertirGris(_Originales[Boton]));
            Boton.Enabled = TieneAcceso;
            Boton.BackgroundImage = TieneAcceso || !_Grises.ContainsKey(Boton) ? _Originales[Boton] : _Grises[Boton];
            Boton.Invalidate();
        }

        public static void ContabilidadMetRestringir(Control Boton, bool Permitido)
        {
            if (Permitido) return;
            Boton.Enabled = false;
            Boton.EnabledChanged += (Remitente, Evento) => { if (Boton.Enabled) Boton.Enabled = false; };
        }

        private static Image ContabilidadFuncConvertirGris(Image Imagen)
        {
            Bitmap Gris = new Bitmap(Imagen.Width, Imagen.Height);
            using (Graphics Lienzo = Graphics.FromImage(Gris))
            using (ImageAttributes Atributos = new ImageAttributes())
            {
                Atributos.SetColorMatrix(new ColorMatrix(new float[][]
                {
                    new float[] { .3f, .3f, .3f, 0, 0 },
                    new float[] { .59f, .59f, .59f, 0, 0 },
                    new float[] { .11f, .11f, .11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                }));
                Lienzo.DrawImage(Imagen, new Rectangle(0, 0, Gris.Width, Gris.Height),
                    0, 0, Imagen.Width, Imagen.Height, GraphicsUnit.Pixel, Atributos);
            }
            return Gris;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<Button, Image> Original in _Originales)
                if (!Original.Key.IsDisposed) Original.Key.BackgroundImage = Original.Value;
            foreach (Image Gris in _Grises.Values) Gris.Dispose();
            _Grises.Clear();
            _Originales.Clear();
        }
    }
}
