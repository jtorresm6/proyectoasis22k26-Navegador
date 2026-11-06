using CapaControlador_Seguridad;
using CapaControlador_Seguridad.Modelos_de_controladores;
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
        private ClsModeloMantenimientoApp datos = new ClsModeloMantenimientoApp();
        public FrmMantenimientoAplicacionNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsMantenimientoAplicacion", datos.SeguridadMetObtenerTodos());
            fuentes.Add("RpReporteMantenimientoAplicacion", datos.SeguridadMetObtenerTodos());
            navegador1.NavegadorMetConfigurar("tblaplicacion", 4, 7, fuentes);
            this.Load += FrmMantenimientoAplicacionNavegador_Load;

        }
        private void FrmMantenimientoAplicacionNavegador_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(280, 20);
        }
    }
}

