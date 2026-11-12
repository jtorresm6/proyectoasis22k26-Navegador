using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;
using CapaModelo_EnlaceB.Entidades;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceLogistica : Form
    {
        public FrmEnlaceLogistica() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceLogistica(ClsControladorEnlaceB Controlador)
        {
            InitializeComponent();
            _BancarioConsulta.BancarioMetConfigurar(ModuloEnlace.Logistica, Controlador);
        }

        protected override void OnFormClosing(FormClosingEventArgs Argumentos)
        {
            if (_BancarioConsulta.Ocupado) Argumentos.Cancel = true;
            base.OnFormClosing(Argumentos);
        }
    }
}
