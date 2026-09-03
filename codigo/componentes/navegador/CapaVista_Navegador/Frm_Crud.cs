using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador_Navegador;

namespace CapaVista_Navegador
{
    public partial class Frm_Crud : Form
    {
        public Frm_Crud()
        {
            InitializeComponent();
        }
        string nombreTabla = "tbl_empleados";
        Controlador controlador = new Controlador();

        public void actualizarDataGridView()
        {
            DataTable dtVista = controlador.llenarDgv(nombreTabla);
            Dgv_datos.DataSource = dtVista;
        }

        private void Btn_Consultar_Click(object sender, EventArgs e)
        {
            actualizarDataGridView();
        }
    }
}
