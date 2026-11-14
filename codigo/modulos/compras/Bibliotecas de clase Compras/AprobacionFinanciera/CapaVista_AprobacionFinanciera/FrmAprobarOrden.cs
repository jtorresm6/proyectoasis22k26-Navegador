using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la confirmación de aprobación de una orden de compra y permite cancelar la operación.
 * ===================================================================
*/

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmAprobarOrden : Form
    {
        public FrmAprobarOrden()
        {
            InitializeComponent();
            ComprasBtnNoAprobar.Click += (Remitente, Evento) => Close();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
        }
    }
}
