// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Muestra la confirmación o el resultado de una operación.

using System.Windows.Forms;

namespace CapaVista_ComisionVendedores_Ventas
{
    internal sealed partial class FrmOperacionComision : Form
    {
        public FrmOperacionComision()
        {
            InitializeComponent();
        }

        internal FrmOperacionComision(string Titulo, string Mensaje, bool Confirmar, string Accion) : this()
        {
            VentasLblMensaje.Text = Mensaje;
            VentasLblTitulo.Text = Titulo;
            VentasBtnConfirmar.AccessibleDescription = Accion;
            VentasBtnConfirmar.Visible = Confirmar;
            VentasBtnCancelar.Visible = Confirmar;
            VentasBtnCerrar.Visible = !Confirmar;
            AcceptButton = Confirmar ? VentasBtnConfirmar : VentasBtnCerrar;
            CancelButton = Confirmar ? VentasBtnCancelar : null;
        }
    }
}
