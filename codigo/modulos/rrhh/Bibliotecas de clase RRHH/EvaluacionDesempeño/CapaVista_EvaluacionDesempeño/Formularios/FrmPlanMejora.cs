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
 *  Ventana 9403 - Plan de Mejora (solo diseño).
 *  Menú de opciones relacionadas, sin eventos ni lógica.
 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmPlanMejora : Form
    {
        public FrmPlanMejora()
        {
            InitializeComponent();
        }

        private void RrhhBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
