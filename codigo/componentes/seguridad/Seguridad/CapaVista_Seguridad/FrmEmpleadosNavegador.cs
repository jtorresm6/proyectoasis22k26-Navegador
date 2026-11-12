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
    public partial class FrmEmpleadosNavegador : Form
    {
        ClsModeloEmpleado datos = new ClsModeloEmpleado();
        ClsModeloMantenimientoApp datos2 = new ClsModeloMantenimientoApp();
        public FrmEmpleadosNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsMantenimientoEmpleado", datos.SeguridadMetObtenerTodos());
            fuentes.Add("RpReporteMantenimientoAplicacion", datos2.SeguridadMetObtenerTodos());
            navegador1.NavegadorMetConfigurar("tblempleado", 4, 4, fuentes);
        }

        
    }
}
