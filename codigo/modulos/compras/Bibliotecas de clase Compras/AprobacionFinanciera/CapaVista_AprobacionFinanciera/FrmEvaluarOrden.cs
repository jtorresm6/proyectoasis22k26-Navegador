/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta la evaluación de una orden de compra y las opciones del proceso de aprobación o rechazo.
 * ===================================================================
*/

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmEvaluarOrden : System.Windows.Forms.Form
    {
        public FrmEvaluarOrden()
        {
            InitializeComponent();
            ComprasBtnRegresar.Click += (Remitente, Evento) => Close();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
        }
    }
}
