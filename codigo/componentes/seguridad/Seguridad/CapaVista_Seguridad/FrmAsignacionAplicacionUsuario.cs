using proyecto2k26;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ventana_Bitacora_Seguridad;

namespace CapaVista_Seguridad
{
    public partial class FrmAsignacionAplicacionUsuario : Form
    {
        public FrmAsignacionAplicacionUsuario()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmBitacora frmbita = new FrmBitacora();
            this.Hide();
            frmbita.ShowDialog();
            this.Show();
        }
    }
}
