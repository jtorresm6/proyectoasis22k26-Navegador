using System.Windows.Forms;

namespace CapaVista_Mantenimientos
{
    public partial class FrmMantenimientosVentas : Form
    {
        public FrmMantenimientosVentas()
        {
            InitializeComponent();
            var AreaTrabajo = Screen.FromControl(this).WorkingArea;
            AutoScrollMinSize = ClientSize;
            AutoScroll = true;
            MinimumSize = System.Drawing.Size.Empty;
            Size = new System.Drawing.Size(System.Math.Min(Width, System.Math.Max(1, AreaTrabajo.Width - 32)),
                System.Math.Min(Height, System.Math.Max(1, AreaTrabajo.Height - 32)));
            VentasUsrNavegador.NavegadorMetConfigurar("tblcliente", 4, 4);
        }
    }
}
