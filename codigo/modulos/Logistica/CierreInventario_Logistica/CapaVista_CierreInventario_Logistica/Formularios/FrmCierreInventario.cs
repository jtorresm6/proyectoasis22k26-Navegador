/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA
 * FORMULARIO: FrmCierreInventario
 *
 * DESCRIPCIÓN:
 * Formulario que abordad la interfaz para las consultas del cierre de inventario
 *
 * RESPONSABLE: Nills Berducido Gómez
 * CARNÉ: 0901 23 14275
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_CierreInventario_Logistica.Formularios
{
    public partial class FrmCierreInventario : Form
    {
        public FrmCierreInventario()
        {
            InitializeComponent();
        }


        private void LogisticaBtnConsultarCierre_Click_1(object sender, EventArgs e)
        {
            FrmConsultaCierres frmConsulta = new FrmConsultaCierres();
            frmConsulta.ShowDialog();

        }
    }
}
