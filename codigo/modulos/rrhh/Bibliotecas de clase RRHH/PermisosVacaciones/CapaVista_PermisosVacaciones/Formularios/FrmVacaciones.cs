/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 23/10/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9203 - Vacaciones. Mantenimiento de tblVacacionEmpleado con el Navegador.
 * ===================================================================
*/

using System.Windows.Forms;
using CapaControlador_PermisosVacaciones.Clases;

namespace CapaVista_PermisosVacaciones.Formularios
{
    public partial class FrmVacaciones : Form
    {
        private readonly ClsControladorPermisosVacaciones _Controlador = new ClsControladorPermisosVacaciones();

        public FrmVacaciones()
        {
            InitializeComponent();
            RrhhNavVacaciones.NavegadorMetConfigurar(
                _Controlador.RrhhFuncTablaVacaciones(),
                _Controlador.RrhhFuncIdModulo(),
                _Controlador.RrhhFuncIdAplicacionVacaciones());
        }
    }
}
