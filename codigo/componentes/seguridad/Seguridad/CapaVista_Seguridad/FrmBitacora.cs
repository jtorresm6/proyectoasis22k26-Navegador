using AplicacionPerfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ventana_Bitacora_Seguridad
{
    public partial class FrmBitacora : Form
    {
        public FrmBitacora()
        {
            InitializeComponent();
        }

        private void btnBuscarAccion_Click(object sender, EventArgs e)
        {

            AsignacionPerfiles form = new AsignacionPerfiles();
            form.Show();

        }
    }
}
