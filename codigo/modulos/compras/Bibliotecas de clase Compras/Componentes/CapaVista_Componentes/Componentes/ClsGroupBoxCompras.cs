using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Agrupa controles y aplica la presentación visual común de las secciones de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public partial class ClsGroupBoxCompras : GroupBox
    {

        public ClsGroupBoxCompras()
        {
            Font = new Font("Tahoma",10F,FontStyle.Bold,GraphicsUnit.Point);
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            Margin = new Padding(3);
        }

    }
}
