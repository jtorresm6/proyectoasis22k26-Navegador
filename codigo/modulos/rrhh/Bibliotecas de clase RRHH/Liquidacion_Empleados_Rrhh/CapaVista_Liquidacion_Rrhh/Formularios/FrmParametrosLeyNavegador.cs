using CapaControlador_Liquidacion_Rrhh.Modelos_de_Controladores;
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

namespace CapaVista_Liquidacion_Rrhh.Formularios
{
    public partial class FrmParametrosLeyNavegador : Form
    {
        ClsModeloParametroLey DatosParametroLey = new ClsModeloParametroLey();
        
        public FrmParametrosLeyNavegador()
        {
            InitializeComponent();
            Dictionary<string, object> fuentes = new Dictionary<string, object>();
            fuentes.Add("DsParametroLey", DatosParametroLey.ParametroLeyMetObtenerParaReporte());
          
            RrhhNavParametroLey.NavegadorMetConfigurar("tblParametroLey", 9000, 9703);
        }
    }
}
