using CapaVista_Componentes.Componentes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Compras
{
    public partial class FrmMantenimientoConceptoProveedor : Form
    {
        public FrmMantenimientoConceptoProveedor()
        {
            InitializeComponent();
            ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasUsrMantenimiento.NavegadorMetConfigurar("tblConceptoProveedor", 4, 4);
        }
    }
}
