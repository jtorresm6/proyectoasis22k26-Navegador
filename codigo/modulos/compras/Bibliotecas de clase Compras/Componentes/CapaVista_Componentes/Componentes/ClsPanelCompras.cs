using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Proporciona paneles con el estilo visual compartido de los formularios de Compras.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public class ClsPanelCompras : Panel
    {

        public ClsPanelCompras()
        {
            BackColor = ClsTemaCompras.FondoGeneral;
            ForeColor = ClsTemaCompras.PrincipalNavegador;
            DoubleBuffered = true;
            ResizeRedraw = true;
        }

    }
}
