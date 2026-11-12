using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmOperacionCompletada : Form
    {
        public FrmOperacionCompletada()
        {
            InitializeComponent();
        }

        internal FrmOperacionCompletada(string Mensaje) : this()
        {
            _BancarioLblMensaje.Text = Mensaje;
        }
    }
}

