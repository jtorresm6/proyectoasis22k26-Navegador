using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_OrdenesCompra.Presentacion
{
    internal sealed class ClsOrdenesCompraComunEdicion : IMessageFilter, IDisposable
    {
        private readonly Dictionary<Control, bool> _SoloLecturaOriginal = new Dictionary<Control, bool>();
        private readonly Dictionary<Control, bool> _TabOriginal = new Dictionary<Control, bool>();
        private readonly HashSet<Control> _Bloqueados = new HashSet<Control>();

        internal ClsOrdenesCompraComunEdicion(Control Contenedor)
        {
            Registrar(Contenedor);
            Application.AddMessageFilter(this);
        }

        private void Registrar(Control Contenedor)
        {
            foreach (Control Campo in Contenedor.Controls)
            {
                var Texto = Campo as ComprasUsrTextBox;
                if (Texto != null) _SoloLecturaOriginal[Campo] = Texto.ReadOnly;
                if (Texto != null || Campo is ComprasUsrComboBox || Campo is DateTimePicker || Campo is CheckBox)
                    _TabOriginal[Campo] = Campo.TabStop;
                else Registrar(Campo);
            }
        }

        internal bool PuedeEditar(Control Campo) => !_Bloqueados.Contains(Campo);

        internal void Establecer(Control Campo, bool Editable)
        {
            Campo.Enabled = true;
            if (Editable) _Bloqueados.Remove(Campo);
            else _Bloqueados.Add(Campo);
            var Texto = Campo as ComprasUsrTextBox;
            if (Texto != null)
            {
                if (!_SoloLecturaOriginal.ContainsKey(Campo)) _SoloLecturaOriginal[Campo] = Texto.ReadOnly;
                Texto.ReadOnly = _SoloLecturaOriginal[Campo] || !Editable;
            }
            if (_TabOriginal.ContainsKey(Campo)) Campo.TabStop = _TabOriginal[Campo] && Editable;
        }

        internal void Aplicar(Control Contenedor, bool Editable)
        {
            Contenedor.Enabled = true;
            foreach (Control Campo in Contenedor.Controls)
            {
                if (Campo is ComprasUsrTextBox || Campo is ComprasUsrComboBox || Campo is DateTimePicker || Campo is CheckBox)
                    Establecer(Campo, Editable);
                else if (Campo is Button) Campo.Enabled = Editable;
                else Aplicar(Campo, Editable);
            }
        }

        public bool PreFilterMessage(ref Message Mensaje)
        {
            if (Mensaje.Msg == 0x020A || Mensaje.Msg == 0x020E) return false;
            Control Campo = Control.FromChildHandle(Mensaje.HWnd);
            while (Campo != null && !_Bloqueados.Contains(Campo)) Campo = Campo.Parent;
            if (Campo == null || Campo is ComprasUsrTextBox) return false;
            int Codigo = Mensaje.Msg;
            if (Codigo == 0x0100 || Codigo == 0x0101 || Codigo == 0x0104 || Codigo == 0x0105)
            {
                Keys Tecla = (Keys)Mensaje.WParam.ToInt32();
                if (Tecla == Keys.Tab || (Control.ModifierKeys & Keys.Control) != 0 && (Tecla == Keys.C || Tecla == Keys.A)) return false;
                return true;
            }
            if (Codigo == 0x0102 || Codigo == 0x0106) return true;
            return Codigo >= 0x0201 && Codigo <= 0x020E || Codigo == 0x007B
                || Codigo == 0x0300 || Codigo == 0x0302 || Codigo == 0x0303;
        }

        public void Dispose() => Application.RemoveMessageFilter(this);
    }
}
