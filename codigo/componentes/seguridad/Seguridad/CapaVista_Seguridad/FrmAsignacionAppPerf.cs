using CapaVista_Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyecto2k26
{
    public partial class FrmAsignacionAppPerf : Form
    {
        public FrmAsignacionAppPerf()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmAsignacionAplicacionUsuario frmasigusu = new FrmAsignacionAplicacionUsuario();
            this.Hide();
            frmasigusu.ShowDialog();
            this.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // TODO: lógica para buscar y cargar resultados en dataGridView1
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            // TODO: lógica para quitar la fila seleccionada de dataGridView1
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // TODO: lógica para imprimir el contenido de dataGridView1
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
