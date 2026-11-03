using CapaVista_Seguridad.Ayudas;
using System;
using System.Drawing;
using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * Modificado por : Byron Alexander Chiquito Paredes 0901-23-3747 (29/10/2026)
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)

 * ===================================================================
*/

namespace CapaVista_EvaluacionDesempeño.Formularios
{
    public partial class FrmEvaluacionDesempeño : Form
    {
        private const int ID_MODULO = 9000;

        public FrmEvaluacionDesempeño()
        {
            InitializeComponent();
            Load += FrmEvaluacionDesempeño_Load;
        }

        private void FrmEvaluacionDesempeño_Load(object sender, EventArgs e)
        {
            RrhhMetAplicarAcceso(RrhhBtnAbrirRealizarEvaluacion, 9402);
            RrhhMetAplicarAcceso(RrhhBtnAbrirPlanMejora, 9403);
            RrhhMetAplicarAcceso(RrhhBtnAbrirVerEvaluaciones, 9404);
        }

        private void RrhhMetAplicarAcceso(Button Boton, int IdAplicacion)
        {
            if (ClsSeguridadFormHelper.SeguridadMetTieneAcceso(ID_MODULO, IdAplicacion)) return;
            Boton.Enabled = false;
            Boton.BackColor = Color.FromArgb(200, 200, 200);
        }

               private void RrhhBtnAbrirRealizarEvaluacion_Click(object sender, EventArgs e)
        {
            
        }

        private void RrhhBtnAbrirPlanMejora_Click(object sender, EventArgs e)
        {
            using (var RrhhFrmPlan = new FrmPlanMejora())
            {
                RrhhFrmPlan.ShowDialog(this);
            }
        }

        private void RrhhBtnAbrirVerEvaluaciones_Click(object sender, EventArgs e)
        {
            using (var RrhhFrmVer = new FrmVerEvaluaciones())
            {
                RrhhFrmVer.ShowDialog(this);
            }
        }
    }
}
