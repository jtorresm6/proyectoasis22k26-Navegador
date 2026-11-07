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
 *  Ventana 9201 - Permisos y Vacaciones. Menú de mantenimientos
 *  relacionados: desde aquí se abre FrmPermisos o FrmVacaciones.
 * ===================================================================
*/

namespace CapaVista_PermisosVacaciones.Formularios
{
    public partial class FrmPermisosVacaciones : Form
    {
        public FrmPermisosVacaciones()
        {
            InitializeComponent();
        }

        private void RrhhBtnAbrirPermisos_Click(object sender, EventArgs e)
        {
            FrmPermisos RrhhFrmPermisos = new FrmPermisos();
            RrhhFrmPermisos.ShowDialog(this);
        }

        private void RrhhBtnAbrirVacaciones_Click(object sender, EventArgs e)
        {
            FrmVacaciones RrhhFrmVacaciones = new FrmVacaciones();
            RrhhFrmVacaciones.ShowDialog(this);
        }
    }
}
