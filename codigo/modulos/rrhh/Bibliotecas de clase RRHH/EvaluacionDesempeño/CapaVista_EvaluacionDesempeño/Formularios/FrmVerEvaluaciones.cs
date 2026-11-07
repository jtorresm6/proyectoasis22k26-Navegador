using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : (pendiente)
 * Carné : (pendiente)
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Prototipo NO funcional de la ventana 9404 - VerEvaluaciones.
 *  Consulta del colaborador: resumen de su evaluación, calificación
 *  por criterio, retroalimentación, plan de mejora e historial.
 *  Solo contiene el diseño de la pantalla (sin datos de ejemplo,
 *  eventos, validaciones ni acceso a datos). La lógica se conectará
 *  después a CapaControlador_EvaluacionDesempeño.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmVerEvaluaciones : Form
    {
        public FrmVerEvaluaciones()
        {
            InitializeComponent();
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
