/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : daniella jurado
 * Carné : 0901-23-3679
 * Fecha : 23/10/2026
 * Modificado por : Daniella Jurado 0901-23-3679 (02/11/2026)
 * ==================================================================
 * Propósito :
 *  Ventana 9202 - Permisos. Mantenimiento de tblPermisoEmpleado con el Navegador.
 * Reglas especificas:
 *  Se ingresa el empleado, la fecha inicio y fin, el motivo y si es con
 *  goce de sueldo. Los días (totalPermisoEmpleado) los calcula la BD,
 *  por eso ese campo se deja vacío. La fecha fin no puede ser anterior
 *  a la fecha inicio.
 * ===================================================================
*/

using System.Windows.Forms;
using CapaControlador_PermisosVacaciones.Clases;
using CapaVista_PermisosVacaciones.Utilidades;

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
            ClsValidacionFechas.RrhhMetConfigurar(this, RrhhNavPermisos, _Controlador.RrhhFuncFechasPermisos());
        }
    }
}
