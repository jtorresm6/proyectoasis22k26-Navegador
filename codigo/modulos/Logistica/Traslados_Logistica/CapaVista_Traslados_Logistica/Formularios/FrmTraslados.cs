// Autor: José Xavier Bolaños Tenas, carné 0901-23-2128.
// Propósito: gestionar la captura, navegación y consulta de traslados logísticos.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_Traslados_Logistica.Formularios
{
    public partial class FrmTraslados : Form
    {
        // Inicializa los controles del formulario de traslados.
        public FrmTraslados()
        {
            InitializeComponent();
        }

        // Abre el formulario con el detalle del traslado.
        private void LogisticaMetVerDetalleTraslados(object sender, EventArgs e)
        {
            FrmDetalleTraslados frmDetalleTraslados = new FrmDetalleTraslados();
            frmDetalleTraslados.ShowDialog();
        }
    }
}
