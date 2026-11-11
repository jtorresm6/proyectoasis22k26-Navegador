using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista_Componentes.Componentes
{
    internal sealed class ClsFondoEntradaCompras : IDisposable
    {
        private readonly Control _Vista;
        private readonly Action _Actualizar;
        private readonly List<Control> _Contenedores = new List<Control>();

        internal ClsFondoEntradaCompras(Control Vista, Action Actualizar)
        {
            _Vista = Vista;
            _Actualizar = Actualizar;
            _Vista.ParentChanged += Reconectar;
            _Vista.Disposed += Liberar;
            Reconectar(null, EventArgs.Empty);
        }

        internal static Color ObtenerColor(Control Vista)
        {
            while (Vista != null)
            {
                if (Vista.BackColor.A == 255) return Vista.BackColor;
                Vista = Vista.Parent;
            }
            return ClsTemaCompras.FondoGeneral;
        }

        private void Reconectar(object Remitente, EventArgs Evento)
        {
            Desconectar();
            for (Control Padre = _Vista.Parent; Padre != null; Padre = Padre.Parent)
            {
                _Contenedores.Add(Padre);
                Padre.BackColorChanged += Actualizar;
                Padre.ParentChanged += Reconectar;
            }
            _Actualizar();
        }

        private void Actualizar(object Remitente, EventArgs Evento)
        {
            if (!_Vista.IsDisposed) _Actualizar();
        }

        private void Desconectar()
        {
            foreach (Control Padre in _Contenedores)
            {
                Padre.BackColorChanged -= Actualizar;
                Padre.ParentChanged -= Reconectar;
            }
            _Contenedores.Clear();
        }

        private void Liberar(object Remitente, EventArgs Evento)
        {
            Dispose();
        }

        public void Dispose()
        {
            Desconectar();
            _Vista.ParentChanged -= Reconectar;
            _Vista.Disposed -= Liberar;
        }
    }
}
