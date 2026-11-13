using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;
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
 *  Organiza controles en filas y columnas y administra su presentación y estado seleccionado.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public class ClsTableLayoutPanelCompras : TableLayoutPanel
    {
        private int _ActualizacionesVisuales;
        private IntPtr _VentanaOculta;
        private Image _ImagenNormal, _ImagenHover, _ImagenSeleccionado, _FondoOriginal;
        private bool _Seleccionado, _MouseDentro, _CambiandoFondo;
        private Color _ColorTextoSeleccionado = ClsTemaCompras.AcentoNavegador;
        private readonly Dictionary<Control, Color> _ColoresTexto = new Dictionary<Control, Color>();

        [Category("Compras"), Description("Color del texto de la segunda columna cuando el panel está seleccionado.")]
        public Color ColorTextoSeleccionado
        {
            get { return _ColorTextoSeleccionado; }
            set { _ColorTextoSeleccionado = value; ComprasProcActualizarColorTexto(); }
        }

        public bool ShouldSerializeColorTextoSeleccionado()
        {
            return _ColorTextoSeleccionado != ClsTemaCompras.AcentoNavegador;
        }

        public void ResetColorTextoSeleccionado()
        {
            ColorTextoSeleccionado = ClsTemaCompras.AcentoNavegador;
        }

        [Category("Compras"), DefaultValue(null), Description("Fondo normal; si está vacío utiliza BackgroundImage.")]
        public Image ImagenNormal
        {
            get { return _ImagenNormal; }
            set { _ImagenNormal = value; ComprasProcActualizarFondo(); }
        }

        [Category("Compras"), DefaultValue(null), Description("Fondo al pasar el cursor; si está vacío utiliza el normal.")]
        public Image ImagenHover
        {
            get { return _ImagenHover; }
            set { _ImagenHover = value; ComprasProcActualizarFondo(); }
        }

        [Category("Compras"), DefaultValue(null), Description("Fondo seleccionado, con prioridad sobre hover; si está vacío utiliza el normal.")]
        public Image ImagenSeleccionado
        {
            get { return _ImagenSeleccionado; }
            set { _ImagenSeleccionado = value; ComprasProcActualizarFondo(); }
        }

        [Category("Compras"), DefaultValue(false), Description("Indica la opción seleccionada del navegador.")]
        public bool Seleccionado
        {
            get { return _Seleccionado; }
            set
            {
                _Seleccionado = value;
                ComprasProcActualizarFondo();
                ComprasProcActualizarColorTexto();
                Invalidate();
            }
        }

        private void ComprasProcActualizarColorTexto()
        {
            if (_Seleccionado)
            {
                foreach (Control Elemento in Controls)
                {
                    if (GetColumn(Elemento) != 1) continue;
                    if (!_ColoresTexto.ContainsKey(Elemento)) _ColoresTexto.Add(Elemento, Elemento.ForeColor);
                    Elemento.ForeColor = _ColorTextoSeleccionado;
                }
            }
            else
            {
                foreach (KeyValuePair<Control, Color> Original in _ColoresTexto)
                    if (!Original.Key.IsDisposed) Original.Key.ForeColor = Original.Value;
                _ColoresTexto.Clear();
            }
        }

        public void ComprasProcActualizarHover(bool Dentro)
        {
            if (_MouseDentro == Dentro) return;
            _MouseDentro = Dentro;
            ComprasProcActualizarFondo();
        }

        protected override void OnBackgroundImageChanged(EventArgs Evento)
        {
            if (!_CambiandoFondo) _FondoOriginal = base.BackgroundImage;
            base.OnBackgroundImageChanged(Evento);
            if (!_CambiandoFondo) ComprasProcActualizarFondo();
        }

        private void ComprasProcActualizarFondo()
        {
            Image Normal = _ImagenNormal ?? _FondoOriginal;
            Image Fondo = _Seleccionado ? (_ImagenSeleccionado ?? Normal)
                : _MouseDentro ? (_ImagenHover ?? Normal) : Normal;
            if (ReferenceEquals(base.BackgroundImage, Fondo)) return;
            _CambiandoFondo = true;
            try { base.BackgroundImage = Fondo; }
            finally { _CambiandoFondo = false; }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr Ventana, int Mensaje, IntPtr Parametro, IntPtr Datos);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr Ventana);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RedrawWindow(IntPtr Ventana, IntPtr Rectangulo, IntPtr Region, uint Opciones);

        public ClsTableLayoutPanelCompras()
        {
            BackColor = ClsTemaCompras.FondoGeneral;
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs Evento)
        {
            base.OnPaint(Evento);
            if (!_Seleccionado) return;
            using (var Pincel = new SolidBrush(ClsTemaCompras.AcentoNavegador))
                Evento.Graphics.FillRectangle(Pincel, 0, 0, 4, Height);
        }

        internal void ComprasProcOcultarActualizacion()
        {
            if (_ActualizacionesVisuales++ != 0 || !IsHandleCreated || !IsWindowVisible(Handle)) return;
            _VentanaOculta = Handle;
            SendMessage(_VentanaOculta, 0x000B, IntPtr.Zero, IntPtr.Zero);
        }

        internal void ComprasProcRestaurarActualizacion()
        {
            if (--_ActualizacionesVisuales != 0) return;
            IntPtr Ventana = _VentanaOculta;
            _VentanaOculta = IntPtr.Zero;
            if (Ventana == IntPtr.Zero || IsDisposed || Disposing || !IsHandleCreated || Handle != Ventana) return;
            SendMessage(Ventana, 0x000B, new IntPtr(1), IntPtr.Zero);
            RedrawWindow(Ventana, IntPtr.Zero, IntPtr.Zero, 0x0485);
        }
    }
}
