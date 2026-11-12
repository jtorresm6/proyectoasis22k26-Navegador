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
    public partial class FrmModulosNavegador : Form
    {
        public FrmModulosNavegador()
        {
            InitializeComponent();
            navegador1.NavegadorMetConfigurar("tblmodulo", 4, 6);
        }
    }
}
