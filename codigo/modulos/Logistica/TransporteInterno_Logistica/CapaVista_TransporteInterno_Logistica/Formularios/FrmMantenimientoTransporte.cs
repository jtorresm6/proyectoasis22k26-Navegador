// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// FORMULARIO: FrmMantenimientoTransporte
// DESCRIPCIÓN:
// Formulario encargado de administrar la información de los vehículos
// utilizados en las operaciones de Transporte Interno.
//
// Permite registrar, consultar, modificar y eliminar los datos de los
// vehículos, incluyendo placas, marcas, modelos, años de fabricación,
// características de refrigeración, tipos y estados operativos.
//
// Integra el componente Navegador para gestionar los registros de la
// tabla tblTransporte y facilitar el control de los vehículos disponibles.
//
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Windows.Forms;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmMantenimientoTransporte : Form
    {
        // ============================================
        // CONSTRUCTOR PRINCIPAL
        // ============================================

        public FrmMantenimientoTransporte()
        {
            InitializeComponent();

            LogisticaNavTransporteInterno.NavegadorMetConfigurar("tblTransporte", 4, 6);
        }

        // ============================================
        // MÉTODO PARA ABRIR FORMULARIOS
        // ============================================

        private void LogisticaMetAbrirFormulario(Form Formulario)
        {
            using (Formulario)
            {
                Formulario.StartPosition =
                    FormStartPosition.CenterParent;

                Formulario.ShowDialog(this);
            }
        }

        // ============================================
        // CONSULTA DE TRANSPORTE
        // ============================================

        private void LogisticaBtnConsultaTransporte_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new FrmMantenimientoEstadoTransporte());
        }

        // ============================================
        // CONTROL DE TRANSPORTE
        // ============================================

        private void LogisticaBtnControlTransporte_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new FrmMantenimientoTipoTransporte());
        }

        // ============================================
        // GESTIÓN DE VIAJES
        // ============================================

        private void LogisticaBtnGestionViajes_Click(
            object sender, EventArgs e)
        {
            LogisticaMetAbrirFormulario(
                new FrmMantenimientoConductor());
        }
    }
}
