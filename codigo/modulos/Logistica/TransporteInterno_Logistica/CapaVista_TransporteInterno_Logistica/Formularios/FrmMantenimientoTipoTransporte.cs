// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// FORMULARIO: FrmMantenimientoTipoTransporte
// DESCRIPCIÓN:
// Formulario encargado de administrar los diferentes tipos de transporte
// utilizados en las operaciones del módulo de Logística.
//
// Permite registrar, consultar, modificar y eliminar los tipos de transporte,
// facilitando la clasificación de los vehículos según sus características
// y funciones dentro de las operaciones logísticas.
//
// Integra el componente Navegador para gestionar los registros de la
// tabla tblTipoTransporte y mantener actualizada la información del sistema.
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
    public partial class FrmMantenimientoTipoTransporte : Form
    {
        public FrmMantenimientoTipoTransporte()
        {
            InitializeComponent();

            LogisticaNavTipoTransporte.NavegadorMetConfigurar("tblTipoTransporte", 4, 6);
        }
    }
}
