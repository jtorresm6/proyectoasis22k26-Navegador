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
 *  Aplica el estilo común de Compras a los controles de selección de una opción.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public class ClsRadioButtonCompras : RadioButton
    {


        public ClsRadioButtonCompras()
        {
            Font = new Font("Tahoma",9.0F,FontStyle.Regular,GraphicsUnit.Point);

            ForeColor = ClsTemaCompras.PrincipalNavegador;
            BackColor = Color.Transparent;
            AutoSize = true;
            Cursor = Cursors.Hand;
            UseVisualStyleBackColor = true;
            Margin = new Padding(3);
        }

    }
}
