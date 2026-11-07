using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Empleado.Formularios
{
    public partial class FrmEmpleados : Form
    {
        public FrmEmpleados()
        {
            InitializeComponent();
        }

        private void RrhhBtnDepartamentos_Click(object sender, EventArgs e)
        {
            new FrmDepartamentosNavegador().ShowDialog(this);
        }

        private void RrhhBtnTipP_Click(object sender, EventArgs e)
        {
            new FrmTipoPuestosNavegador().ShowDialog(this);
        }

        private void RrhhBtnPuestos_Click(object sender, EventArgs e)
        {
            new FrmPuestosNavegador().ShowDialog(this);
        }

        private void RrhhBtnJornadas_Click(object sender, EventArgs e)
        {
            new FrmJornadasNavegador().ShowDialog(this);
        }

        private void RrhhBtnAsigP_Click(object sender, EventArgs e)
        {
            new FrmAsigPuestosNavegador().ShowDialog(this);
        }
    }
}
