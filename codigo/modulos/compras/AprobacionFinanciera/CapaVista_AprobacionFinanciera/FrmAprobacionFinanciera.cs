/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la consulta de órdenes para su revisión en el proceso de aprobación financiera.
 * ===================================================================
*/

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmAprobacionFinanciera : System.Windows.Forms.Form
    {
        public FrmAprobacionFinanciera()
        {
            InitializeComponent();
            ComprasBtnRegresar.Click += (Remitente, Evento) => Close();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
        }


    }
}
