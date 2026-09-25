using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaVista_Navegador;
using CapaControlador_Seguridad;

namespace CapaVista_Seguridad
{
    
        public partial class FrmEmpleadosNavegador : Form
        {
            public FrmEmpleadosNavegador()
            {
                InitializeComponent();
         
                navegador2.NavegadorMetConfigurar("tblempleado", 4, 4);
            }

        }
    }
   