using CapaControlador_Empleado.Modelos_de_Controladores;
using CapaVista_Navegador;
using CapaControlador_Empleado;
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
    public partial class FrmCapacitacionesNavegador : Form
    {
        ClsModeloCapacitacion datos = new ClsModeloCapacitacion();
        //ClsModeloMantenimientoApp datos2 = new ClsModeloMantenimientoApp();
        public FrmCapacitacionesNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsCapacitacion", datos.CapacitacionMetObtenerParaReporte());
            fuentes.Add("RpCapacitacion", datos.CapacitacionMetObtenerParaReporte());
            RrhhNavCapacitaciones.NavegadorMetConfigurar("tblCapacitacion", 9000, 9302, fuentes);
        }



    }
     
}
