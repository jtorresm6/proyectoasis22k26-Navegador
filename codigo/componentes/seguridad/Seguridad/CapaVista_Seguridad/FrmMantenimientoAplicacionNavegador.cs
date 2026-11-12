
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Seguridad
{
    public partial class FrmMantenimientoAplicacionNavegador : Form
    {

        public FrmMantenimientoAplicacionNavegador()
        {
            Dictionary<string, object> fuentes = new Dictionary<string, object>();

            InitializeComponent();
            navegador1.NavegadorMetConfigurar("tblaplicacion", 4, 7);
        }
    }
}
