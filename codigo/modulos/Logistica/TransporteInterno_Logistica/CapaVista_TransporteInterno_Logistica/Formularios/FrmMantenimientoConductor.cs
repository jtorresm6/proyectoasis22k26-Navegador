// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// FORMULARIO: FrmMantenimientoConductor
// DESCRIPCIÓN:
// Formulario encargado de administrar la información de los conductores
// registrados dentro del submódulo de Transporte Interno.
//
// Permite registrar, consultar, modificar y eliminar los datos de los
// conductores, incluyendo nombres, códigos, números de licencia,
// fechas de vencimiento de documentos y estados operativos.
//
// Integra el componente Navegador para gestionar los registros de la
// tabla tblConductor y facilitar la asignación de conductores a los viajes.
//
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista_TransporteInterno_Logistica.Formularios
{
    public partial class FrmMantenimientoConductor : Form
    {
        public FrmMantenimientoConductor()
        {
            InitializeComponent();

            LogisticaNavConductores.NavegadorMetConfigurar("tblConductor", 4, 6);
        }

        private void FrmMantenimientoConductor_Load(object sender, EventArgs e)
        {

        }
    }
}
