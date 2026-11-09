using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_MovimientosBancarios.Formularios
{
    public partial class FrmMovimientosBancarios : Form
    {
        public FrmMovimientosBancarios()
        {
            InitializeComponent();
            MostrarFormulario(new FrmListaMovimientos());
        }

        private void BancosBtnLista_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new FrmListaMovimientos());
        }
        
        private void BancosBtnMovimientos_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new FrmMovimientos());
        }

        private void BancosBtnCheques_Click(object sender, EventArgs e)
        {
            MostrarFormulario(new FrmCheques());
        }
        private void MostrarFormulario(Form frm)
        {
            BancosScContenedor.Panel2.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            BancosScContenedor.Panel2.Controls.Add(frm);
            frm.Show();
        }
    }
}
