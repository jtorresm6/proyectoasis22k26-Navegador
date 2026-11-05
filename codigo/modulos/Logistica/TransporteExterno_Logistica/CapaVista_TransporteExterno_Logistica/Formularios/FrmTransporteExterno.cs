using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmTransporteExterno : Form
    {
        public FrmTransporteExterno()
        {
            InitializeComponent();
        }

        private void LogisticaBtnRutaSeguimiento_Click(object sender, EventArgs e)
        {
            FrmRutaYSeguimiento frmRutaSeguimiento = new FrmRutaYSeguimiento();
            frmRutaSeguimiento.ShowDialog();
        }

        private void LogisticaBtnProductosCarga_Click(object sender, EventArgs e)
        {
            FrmProductosATrasladar frmProductoTrasladado = new FrmProductosATrasladar();
            frmProductoTrasladado.ShowDialog();
        }

        private void LogisticaBtnCostosPagos_Click(object sender, EventArgs e)
        {
            FrmCostosYPagosDelServicio costosYPagosDelServicio = new FrmCostosYPagosDelServicio();
            costosYPagosDelServicio.ShowDialog();
        }

        private void LogisticaBtnIncidenciasEHistorial_Click(object sender, EventArgs e)
        {
            FrmIncidenciasEHistorial frmIncidenciasEHistorial = new FrmIncidenciasEHistorial();
            frmIncidenciasEHistorial.ShowDialog();
        }

        private void LogisticaBtnListadoTransporteExterno_Click(object sender, EventArgs e)
        {
            FrmListadoTransporteExterno frmListadoTransporteExterno = new FrmListadoTransporteExterno();
            frmListadoTransporteExterno.ShowDialog();
        }

        private void LogisticaBtnRegistroTransporte_Click_1(object sender, EventArgs e)
        {
            FrmRegistroTransporteExterno frmTransporteExterno = new FrmRegistroTransporteExterno();
            frmTransporteExterno.ShowDialog();
        }
    }
}