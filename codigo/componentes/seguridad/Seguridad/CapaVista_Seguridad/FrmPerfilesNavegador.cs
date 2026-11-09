
using CapaControlador_Seguridad;
using CapaVista_Navegador;
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
    public partial class FrmPerfilesNavegador : Form
    {

        ClsModeloRoles datos = new ClsModeloRoles();
        public FrmPerfilesNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsMantenimientoPerfiles", datos.SeguridadMetObtenerTodos());
            fuentes.Add("RpReporteMantenimientoPerfil", datos.SeguridadMetObtenerTodos());
            navegador1.NavegadorMetConfigurar("tblRol", 4, 8, fuentes);
        }

        
    }
}
