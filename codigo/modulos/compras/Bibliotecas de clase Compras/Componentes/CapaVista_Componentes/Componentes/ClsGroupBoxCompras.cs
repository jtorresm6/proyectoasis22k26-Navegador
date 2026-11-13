using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
 *  Agrupa controles y aplica la presentación visual común de las secciones de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public partial class ClsGroupBoxCompras : GroupBox
    {
        private static readonly ComponentResourceManager _Recursos =
            new ComponentResourceManager(typeof(ClsGroupBoxCompras));
        private string _ImagenFondo = "Ninguna";
        private bool _MostrarBorde = true;

        [Category("Compras")]
        [DefaultValue("Ninguna")]
        [Description("Imagen del recurso del GroupBox que se muestra estirada como fondo. Ninguna conserva el fondo sin imagen.")]
        [TypeConverter(typeof(ClsImagenFondoGroupBoxComprasConverter))]
        [RefreshProperties(RefreshProperties.All)]
        public string ImagenFondo
        {
            get => _ImagenFondo;
            set
            {
                string Nombre = string.IsNullOrWhiteSpace(value) ? "Ninguna" : value;
                Image Imagen = null;
                if (Nombre != "Ninguna")
                {
                    Imagen = _Recursos.GetObject(Nombre, CultureInfo.InvariantCulture) as Image;
                    if (Imagen == null)
                        throw new ArgumentException("La imagen no existe en los recursos del GroupBox.", nameof(value));
                }
                _ImagenFondo = Nombre;
                BackgroundImage = Imagen;
                if (Imagen != null) BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        [Category("Compras")]
        [DefaultValue(true)]
        [Description("Muestra el borde del GroupBox. Al ocultarlo conserva el titulo y el fondo.")]
        public bool MostrarBorde
        {
            get => _MostrarBorde;
            set
            {
                if (_MostrarBorde == value) return;
                _MostrarBorde = value;
                SetStyle(ControlStyles.UserPaint, !value || FlatStyle != FlatStyle.System);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            if (_MostrarBorde)
            {
                base.OnPaint(Evento);
                return;
            }
            TextFormatFlags Formato = TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
            if (!ShowKeyboardCues) Formato |= TextFormatFlags.HidePrefix;
            if (RightToLeft == RightToLeft.Yes)
                Formato |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            TextRenderer.DrawText(Evento.Graphics, Text, Font,
                new Rectangle(8, 0, Math.Max(0, Width - 16), Font.Height),
                Enabled ? ForeColor : SystemColors.GrayText, Formato);
        }

        public bool ShouldSerializeBackgroundImage()
        {
            return _ImagenFondo == "Ninguna" && BackgroundImage != null;
        }

        internal static string[] ObtenerImagenesFondo()
        {
            var Nombres = new List<string>();
            foreach (DictionaryEntry Recurso in _Recursos.GetResourceSet(CultureInfo.InvariantCulture, true, true))
            {
                if (Recurso.Value is Image) Nombres.Add((string)Recurso.Key);
            }
            Nombres.Sort(StringComparer.OrdinalIgnoreCase);
            Nombres.Insert(0, "Ninguna");
            return Nombres.ToArray();
        }

        public ClsGroupBoxCompras()
        {
            Font = new Font("Tahoma",10F,FontStyle.Bold,GraphicsUnit.Point);
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            Margin = new Padding(3);
        }

    }
    public sealed class ClsImagenFondoGroupBoxComprasConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext Contexto) => true;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext Contexto) => true;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext Contexto)
        {
            return new StandardValuesCollection(ClsGroupBoxCompras.ObtenerImagenesFondo());
        }
    }
}
