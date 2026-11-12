using System;
using System.Windows.Forms;
using CapaControlador_EnlaceB;

namespace CapaVista_EnlaceB.Formularios
{
    public partial class FrmEnlaceOtrosModulos : Form
    {
        private readonly ClsControladorEnlaceB _Controlador;

        public FrmEnlaceOtrosModulos() : this(new ClsControladorEnlaceB()) { }

        public FrmEnlaceOtrosModulos(ClsControladorEnlaceB Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
        }

        private void BancarioMetAbrirLogistica(object Remitente, EventArgs Argumentos)
        {
            using (var Ventana = new FrmEnlaceLogistica(_Controlador))
                Ventana.ShowDialog(this);
        }

        private void BancarioMetAbrirCompras(object Remitente, EventArgs Argumentos)
        {
            using (var Ventana = new FrmEnlaceCompras(_Controlador))
                Ventana.ShowDialog(this);
        }

        private void BancarioMetAbrirVentas(object Remitente, EventArgs Argumentos)
        {
            using (var Ventana = new FrmEnlaceVentas(_Controlador))
                Ventana.ShowDialog(this);
        }

        private void BancarioMetAbrirRecursosHumanos(object Remitente, EventArgs Argumentos)
        {
            using (var Ventana = new FrmEnlaceRecursosHumanos(_Controlador))
                Ventana.ShowDialog(this);
        }

        private void BancarioMetAbrirContabilidad(object Remitente, EventArgs Argumentos)
        {
            using (var Ventana = new FrmEnlaceContabilidad(_Controlador))
                Ventana.ShowDialog(this);
        }

        private void BancarioMetCerrar(object Remitente, EventArgs Argumentos)
        {
            Close();
        }
    }
}
