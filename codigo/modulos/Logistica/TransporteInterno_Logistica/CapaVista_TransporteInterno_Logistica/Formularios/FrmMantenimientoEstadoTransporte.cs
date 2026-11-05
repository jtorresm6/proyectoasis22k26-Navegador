// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// FORMULARIO: FrmMantenimientoEstadoTransporte
// DESCRIPCIÓN:
// Formulario encargado de administrar los estados utilizados dentro del
// submódulo de Transporte Interno.
//
// Permite registrar, consultar, modificar y eliminar los estados asociados
// a los vehículos y conductores, facilitando la identificación de sus
// condiciones operativas y disponibilidad.
//
// Integra el componente Navegador para gestionar los registros de la
// tabla tblEstado y mantener actualizada la información del sistema.
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
    public partial class FrmMantenimientoEstadoTransporte : Form
    {
        public FrmMantenimientoEstadoTransporte()
        {
            InitializeComponent();

            LogisticaNavEstados.NavegadorMetConfigurar("tblEstado", 4, 6);
        }
    }
}
