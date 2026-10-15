/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta el usuario activo y sus roles para el inicio de sesión de
 *  Contabilidad. Solo realiza lecturas sobre las tablas de usuarios
 *  que comparte el sistema; la contraseña se verifica en el
 *  Controlador.
 * ===================================================================
*/

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Contabilidad
{
    public class ClsRepositorioUsuariosContabilidad : ClsSentenciasContabilidad
    {
        private const string _Login =
            "SELECT u.idUsuario, u.nombreUsuario, u.contrasenaUsuario, " +
            "CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado " +
            "FROM tblusuario u INNER JOIN tblempleado e ON u.idEmpleado = e.idEmpleado " +
            "WHERE u.nombreUsuario=? AND u.is_active=1";

        private const string _RolesPorUsuario =
            "SELECT ur.idRol, r.nombreRol FROM tblusuariorol ur " +
            "INNER JOIN tblrol r ON ur.idRol = r.idRol WHERE ur.idUsuario=?";

        public DataTable ContabilidadFuncObtenerUsuario(string NombreUsuario)
        {
            return ContabilidadFuncEjecutarConsulta(_Login, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Usuario", NombreUsuario) });
        }

        public DataTable ContabilidadFuncObtenerRoles(int IdUsuario)
        {
            return ContabilidadFuncEjecutarConsulta(_RolesPorUsuario, CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Usuario", IdUsuario) });
        }
    }
}
