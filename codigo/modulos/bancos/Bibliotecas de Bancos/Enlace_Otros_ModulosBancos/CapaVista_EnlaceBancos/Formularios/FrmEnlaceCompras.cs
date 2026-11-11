using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceCompras : Form
    {
        public FrmEnlaceCompras() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceCompras(ClsControladorEnlaceB Controlador)
        {
            InitializeComponent();
            _BancarioConsulta.BancarioMetConfigurar(ModuloEnlace.Compras, Controlador);
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_BancarioConsulta.Ocupado) Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
