using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la confirmación de rechazo de una orden de compra y permite cancelar la operación.
 * ===================================================================
*/

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmRechazarOrden : Form
    {
        public FrmRechazarOrden()
        {
            InitializeComponent();
            ComprasBtnNoRechazar.Click += (Remitente, Evento) => Close();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
        }
    }
}
