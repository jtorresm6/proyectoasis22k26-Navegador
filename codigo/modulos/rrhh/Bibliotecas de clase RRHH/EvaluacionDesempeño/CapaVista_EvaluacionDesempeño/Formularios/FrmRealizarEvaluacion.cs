/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 23/10/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9402 - RealizarEvaluacion. Mantenimiento de
 *  tblEvaluacionDesempeno con el Navegador.
 * ===================================================================
*/

using System.Windows.Forms;

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmRealizarEvaluacion : Form
    {
        public FrmRealizarEvaluacion()
        {
            InitializeComponent();
            RrhhNavRealizarEvaluacion.NavegadorMetConfigurar("tblEvaluacionDesempeno", 9000, 9402);
        }
    }
}
