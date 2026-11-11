using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceRecursosHumanos : Form
    {
        public FrmEnlaceRecursosHumanos() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceRecursosHumanos(ClsControladorEnlaceB Controlador)
        {
            InitializeComponent();
            _BancarioConsulta.BancarioMetConfigurar(ModuloEnlace.RecursosHumanos, Controlador);
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_BancarioConsulta.Ocupado) Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
