using System.Windows.Forms;
using CapaVista_MovimientosProveedores;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Carlos Andres Arriaza Lara
 * Carné : 0901-23-13862
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Abre el formulario de Movimientos de Proveedores en una ventana modal para realizar pruebas de presentación.
 * ===================================================================
*/

namespace Ejecucion_PruebaMovimientos
{
    public partial class FrmPruebaMovimientos : Form
    {
        private readonly FrmMovimientosProveedores _Movimientos = new FrmMovimientosProveedores();
        public FrmPruebaMovimientos()
        {
            InitializeComponent();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            Shown += (Remitente, Evento) =>
            {
                _Movimientos.ShowDialog(this);
                Close();
            };
            FormClosed += (Remitente, Evento) => _Movimientos.Dispose();
        }
    }
}
