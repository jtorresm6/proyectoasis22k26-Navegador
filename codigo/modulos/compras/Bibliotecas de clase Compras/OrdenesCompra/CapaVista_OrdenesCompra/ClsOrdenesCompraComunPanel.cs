using System.Drawing;
using System.Windows.Forms;
using CapaVista_Componentes.Componentes;

namespace CapaVista_OrdenesCompra.Presentacion
{
    public sealed class ClsOrdenesCompraComunPanel : ClsPanelVerticalCompras
    {
        protected override Point ScrollToControl(Control ControlActivo) => DisplayRectangle.Location;
    }
}
