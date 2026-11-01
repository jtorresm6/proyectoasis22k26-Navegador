/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 23/10/2026
 * ==================================================================
 * Propósito :
 *  Ventana 9202 - Permisos. Mantenimiento de tblPermisoEmpleado con el Navegador.
 * ===================================================================
*/

using System.Windows.Forms;
using CapaControlador_PermisosVacaciones.Clases;

namespace CapaVista_PermisosVacaciones.Formularios
{
    public partial class FrmPermisos : Form
    {
        private readonly ClsControladorPermisosVacaciones _Controlador = new ClsControladorPermisosVacaciones();

        public FrmPermisos()
        {
            InitializeComponent();
            RrhhNavPermisos.NavegadorMetConfigurar(
                _Controlador.RrhhFuncTablaPermisos(),
                _Controlador.RrhhFuncIdModulo(),
                _Controlador.RrhhFuncIdAplicacionPermisos());
        }
    }
}
