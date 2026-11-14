using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CapaVista_Componentes.Componentes;

namespace CapaVista_OrdenesCompra.Presentacion
{
    internal sealed class ClsProductosOrdenesCompraPaginacion : NativeWindow, IDisposable
    {
        private readonly DataGridView _Tabla;
        private readonly ComprasUsrDataGridView _Control;
        private bool _Actualizando;
        private bool _Liberado;
        private bool _Pendiente;

        internal ClsProductosOrdenesCompraPaginacion(ComprasUsrDataGridView Control)
        {
            _Control = Control;
            DataGridView Tabla = Control.Tabla;
            _Tabla = Tabla;
            Tabla.HandleCreated += Crear;
            Tabla.HandleDestroyed += Destruir;
            Tabla.SizeChanged += Cambiar;
            Tabla.DataBindingComplete += Enlazar;
            Tabla.RowsAdded += Agregar;
            Tabla.RowsRemoved += Quitar;
            Tabla.RowHeightChanged += Fila;
            Tabla.RowStateChanged += EstadoFila;
            Tabla.RowHeadersWidthChanged += Cambiar;
            Tabla.ColumnWidthChanged += Columna;
            Tabla.ColumnStateChanged += EstadoColumna;
            Tabla.ColumnAdded += Columna;
            Tabla.ColumnRemoved += Columna;
            Tabla.ColumnHeadersHeightChanged += Cambiar;
            Tabla.Disposed += Liberar;
            if (Tabla.IsHandleCreated) AssignHandle(Tabla.Handle);
            Actualizar();
        }

        private void Crear(object Remitente, EventArgs Evento) { AssignHandle(_Tabla.Handle); Programar(); }
        private void Destruir(object Remitente, EventArgs Evento) { _Pendiente = false; if (Handle != IntPtr.Zero) ReleaseHandle(); }
        private void Cambiar(object Remitente, EventArgs Evento) => Programar();
        private void Enlazar(object Remitente, DataGridViewBindingCompleteEventArgs Evento) => Programar();
        private void Agregar(object Remitente, DataGridViewRowsAddedEventArgs Evento) => Programar();
        private void Quitar(object Remitente, DataGridViewRowsRemovedEventArgs Evento) => Programar();
        private void Fila(object Remitente, DataGridViewRowEventArgs Evento) => Programar();
        private void Columna(object Remitente, DataGridViewColumnEventArgs Evento) => Programar();
        private void EstadoFila(object Remitente, DataGridViewRowStateChangedEventArgs Evento) => Programar();
        private void EstadoColumna(object Remitente, DataGridViewColumnStateChangedEventArgs Evento) => Programar();
        private void Liberar(object Remitente, EventArgs Evento) => Dispose();

        private void Programar()
        {
            if (_Liberado || _Pendiente || !_Tabla.IsHandleCreated || _Tabla.IsDisposed) return;
            _Pendiente = true;
            _Tabla.BeginInvoke(new Action(() =>
            {
                _Pendiente = false;
                Actualizar();
            }));
        }

        internal void Actualizar()
        {
            if (_Liberado || _Actualizando || _Tabla.IsDisposed) return;
            _Actualizando = true;
            try
            {
                _Control.AutoScroll = false;
                if (_Tabla.ScrollBars != ScrollBars.None) _Tabla.ScrollBars = ScrollBars.None;
                foreach (Control Hijo in _Tabla.Controls)
                    if (Hijo is ScrollBar) Hijo.Visible = false;
                int Alto = Math.Max(0, _Tabla.ClientSize.Height - (_Tabla.ColumnHeadersVisible ? _Tabla.ColumnHeadersHeight : 0));
                int AltoFila = Math.Max(28, _Tabla.RowTemplate.Height);
                int Capacidad = Math.Max(1, Alto / AltoFila);
                if (!_Control.PaginacionHabilitada) _Control.PaginacionHabilitada = true;
                if (_Control.RegistrosPorPagina != Capacidad) _Control.RegistrosPorPagina = Capacidad;
                if (_Tabla.HorizontalScrollingOffset != 0) _Tabla.HorizontalScrollingOffset = 0;
            }
            finally { _Actualizando = false; }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr Ventana, int Codigo, IntPtr Parametro, IntPtr Datos);

        protected override void WndProc(ref Message Mensaje)
        {
            if (Mensaje.Msg == 0x020A || Mensaje.Msg == 0x020E)
            {
                Actualizar();
                for (Control Padre = _Control.Parent; Padre != null; Padre = Padre.Parent)
                {
                    var Panel = Padre as ScrollableControl;
                    if (Panel == null || !Panel.AutoScroll || Panel.IsDisposed) continue;
                    Mensaje.Result = SendMessage(Panel.Handle, Mensaje.Msg, Mensaje.WParam, Mensaje.LParam);
                    return;
                }
                Mensaje.Result = IntPtr.Zero;
                return;
            }
            if (Mensaje.Msg == 0x0114 || Mensaje.Msg == 0x0115)
            {
                Mensaje.Result = IntPtr.Zero;
                return;
            }
            base.WndProc(ref Mensaje);
        }

        public void Dispose()
        {
            if (_Liberado) return;
            _Liberado = true;
            _Tabla.HandleCreated -= Crear; _Tabla.HandleDestroyed -= Destruir; _Tabla.SizeChanged -= Cambiar;
            _Tabla.DataBindingComplete -= Enlazar; _Tabla.RowsAdded -= Agregar; _Tabla.RowsRemoved -= Quitar;
            _Tabla.RowStateChanged -= EstadoFila; _Tabla.RowHeadersWidthChanged -= Cambiar;
            _Tabla.RowHeightChanged -= Fila; _Tabla.ColumnWidthChanged -= Columna; _Tabla.ColumnStateChanged -= EstadoColumna;
            _Tabla.ColumnAdded -= Columna; _Tabla.ColumnRemoved -= Columna; _Tabla.ColumnHeadersHeightChanged -= Cambiar;
            _Tabla.Disposed -= Liberar;
            if (Handle != IntPtr.Zero) ReleaseHandle();
        }
    }
}
