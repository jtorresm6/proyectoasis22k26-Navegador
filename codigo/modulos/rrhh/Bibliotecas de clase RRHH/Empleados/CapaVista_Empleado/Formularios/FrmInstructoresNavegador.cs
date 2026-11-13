using CapaControlador_Empleado.Modelos_de_Controladores;
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
    public partial class FrmInstructoresNavegador : Form
    {
        ClsModeloInstructor datos = new ClsModeloInstructor();
        //ClsModeloMantenimientoApp datos2 = new ClsModeloMantenimientoApp();
        public FrmInstructoresNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsInstructor", datos.InstructorMetObtenerParaReporte());
            fuentes.Add("RpInstructor", datos.InstructorMetObtenerParaReporte());
            RrhhNavInstructores.NavegadorMetConfigurar("tblInstructor", 9000, 9303, fuentes);
        }

      
    }
}
