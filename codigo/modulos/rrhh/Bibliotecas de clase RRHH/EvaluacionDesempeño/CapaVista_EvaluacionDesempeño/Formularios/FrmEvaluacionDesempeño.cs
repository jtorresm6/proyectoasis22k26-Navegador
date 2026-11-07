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
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9401 - Evaluación de Desempeño
 *  Menú de opciones relacionadas, sin eventos ni lógica.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmEvaluacionDesempeño : Form
    {
        public FrmEvaluacionDesempeño()
        {
            InitializeComponent();
        }

        private void RrhhBtnAbrirRealizarEvaluacion_Click(object sender, EventArgs e)
        {
            FrmRealizarEvaluacion RrhhFrmEvaluacion = new FrmRealizarEvaluacion();
            RrhhFrmEvaluacion.ShowDialog(this);
        }

        private void RrhhBtnAbrirPlanMejora_Click(object sender, EventArgs e)
        {
            FrmPlanMejora RrhhFrmPlan = new FrmPlanMejora();
            RrhhFrmPlan.ShowDialog(this);
        }

        private void RrhhBtnAbrirVerEvaluaciones_Click(object sender, EventArgs e)
        {
            FrmVerEvaluaciones RrhhFrmVer = new FrmVerEvaluaciones();
            RrhhFrmVer.ShowDialog(this);
        }
    }
}
