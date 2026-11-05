
/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
 * FORMULARIO: FrmTipoTraslado
 *
 * RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA
 * CARNÉ: 0901-23-6124
 * ============================================================================
 */

using System;
using System.Windows.Forms;
using CapaVista_TransporteExterno_Logistica.Formularios;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmTipoTraslado : Form
    {
        // ----------------------------------------------
        // Constructor principal
        // ----------------------------------------------

        public FrmTipoTraslado()
        {
            InitializeComponent();
        }

        // Procedimiento para abrir formularios secundarios.
        public void LogisticaMetAbrirFormulario(Form Formulario)
        {
            using (Formulario)
            {
                Formulario.StartPosition =
                    FormStartPosition.CenterParent;

                Formulario.ShowDialog(this);
            }
        }

        // ============================================
        // EVENTOS DE BOTONES
        // ============================================

        private void LogisticaBtnTransporteInterno_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new FrmMantenimientoTransporte());
        }

        private void LogisticaBtntTransporteExterno_Click(object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(new CapaVista_TransporteExterno_Logistica.Formularios.FrmTransporteExterno());
        }
    }
}
