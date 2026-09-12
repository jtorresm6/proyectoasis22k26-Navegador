using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Consultas
{
    public partial class ConsultaSimple_400_001 : Form
    {
        public ConsultaSimple_400_001()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConsultaCompleja_500_001 consultaCompleja_500_001 = new ConsultaCompleja_500_001();
            this.Hide();
            consultaCompleja_500_001.Show();
        }
    }
}
