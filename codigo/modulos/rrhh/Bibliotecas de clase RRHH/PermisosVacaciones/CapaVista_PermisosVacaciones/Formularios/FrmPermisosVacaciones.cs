using System;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_PermisosVacaciones.Clases;
using CapaVista_Seguridad.Ayudas;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9201 - Permisos y Vacaciones. Menú de mantenimientos
 *  relacionados: desde aquí se abre FrmPermisos o FrmVacaciones.
 *  Cada opción se deshabilita si el usuario no tiene acceso a ella.
 * ===================================================================
*/

namespace CapaVista_PermisosVacaciones.Formularios
{
    public partial class FrmPermisosVacaciones : Form
    {
        private readonly ClsControladorPermisosVacaciones _Controlador = new ClsControladorPermisosVacaciones();

        public FrmPermisosVacaciones()
        {
            InitializeComponent();
            Load += FrmPermisosVacaciones_Load;
        }

        private void FrmPermisosVacaciones_Load(object sender, EventArgs e)
        {
            RrhhMetAplicarAcceso(RrhhBtnAbrirPermisos, _Controlador.RrhhFuncIdAplicacionPermisos());
            RrhhMetAplicarAcceso(RrhhBtnAbrirVacaciones, _Controlador.RrhhFuncIdAplicacionVacaciones());
        }

        private void RrhhMetAplicarAcceso(Button Boton, int IdAplicacion)
        {
            if (ClsSeguridadFormHelper.SeguridadMetTieneAcceso(_Controlador.RrhhFuncIdModulo(), IdAplicacion)) return;
            Boton.Enabled = false;
            Boton.BackColor = Color.FromArgb(200, 200, 200);
        }

        private void RrhhBtnAbrirPermisos_Click(object sender, EventArgs e)
        {
            using (var RrhhFrmPermisos = new FrmPermisos())
            {
                RrhhFrmPermisos.ShowDialog(this);
            }
        }

        private void RrhhBtnAbrirVacaciones_Click(object sender, EventArgs e)
        {
            using (var RrhhFrmVacaciones = new FrmVacaciones())
            {
                RrhhFrmVacaciones.ShowDialog(this);
            }
        }
    }
}
