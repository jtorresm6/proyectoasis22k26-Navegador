using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Permite desplazar el contenido verticalmente y ajusta su ancho para evitar desplazamiento horizontal.
 * ===================================================================
*/

namespace CapaVista_Componentes.Componentes
{
    public class ClsPanelVerticalCompras : Panel
    {
        private bool _AjustandoAncho;

        public ClsPanelVerticalCompras()
        {
            AutoScroll = true;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            DoubleBuffered = true;
        }

        protected override void OnLayout(LayoutEventArgs Evento)
        {
            if (_AjustandoAncho) return;
            _AjustandoAncho = true;
            try
            {
                base.OnLayout(Evento);
                foreach (Control Contenido in Controls)
                {
                    Contenido.Width = ClientSize.Width;
                }
                HorizontalScroll.Enabled = false;
                HorizontalScroll.Visible = false;
            }
            finally
            {
                _AjustandoAncho = false;
            }
        }
    }
}
