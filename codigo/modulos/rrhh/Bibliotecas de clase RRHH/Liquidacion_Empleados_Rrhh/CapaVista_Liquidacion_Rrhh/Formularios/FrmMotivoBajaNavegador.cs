using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaVista_Liquidacion_Rrhh.frmReportes;

namespace CapaVista_Liquidacion_Rrhh.Formularios
{
    public partial class FrmMotivoBajaNavegador : Form
    {
        public FrmMotivoBajaNavegador()
        {
            InitializeComponent();
            RrhhNavMotivoBaja.NavegadorMetConfigurar("tblMotivoBaja", 9000, 9702);
        }

        private void RrhhBtnReporteLiqui_Click(object sender, EventArgs e)
        {
            FrmReporteMotivoBaja reporteMotivoBaja = new FrmReporteMotivoBaja();
            reporteMotivoBaja.Show();
        }
    }
}
