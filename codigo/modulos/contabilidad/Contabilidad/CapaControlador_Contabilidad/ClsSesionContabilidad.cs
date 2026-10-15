/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Sesión propia de Contabilidad: usuario, empleado y roles de quien
 *  inició sesión en el módulo mientras la aplicación está abierta.
 * Reglas especificas:
 *  El control Navegador (DLL compartida) lee la sesión desde
 *  ClsSesionSeguridad y no se puede modificar. Por eso, al iniciar
 *  y cerrar sesión, esta clase refleja los mismos datos allí; es el
 *  único punto de contacto que queda con Seguridad.
 * ===================================================================
*/

using System.Collections.Generic;
using System.Linq;
using CapaControlador_Seguridad.Objetos_de_valor;

namespace CapaControlador_Contabilidad
{
    public static class ClsSesionContabilidad
    {
        public static int IdUsuario { get; private set; }
        public static string NombreUsuario { get; private set; }
        public static string NombreEmpleado { get; private set; }
        public static Dictionary<int, string> Roles { get; private set; } = new Dictionary<int, string>();

        public static bool HaySesionActiva
        {
            get { return IdUsuario > 0; }
        }

        public static List<int> IdsRoles
        {
            get { return Roles.Keys.ToList(); }
        }

        public static string ContabilidadFuncRolesComoTexto()
        {
            return string.Join(", ", Roles.Values);
        }

        public static void ContabilidadMetIniciarSesion(int IdUsuarioNuevo, string NombreUsuarioNuevo,
            string NombreEmpleadoNuevo, Dictionary<int, string> RolesNuevos)
        {
            IdUsuario = IdUsuarioNuevo;
            NombreUsuario = NombreUsuarioNuevo;
            NombreEmpleado = NombreEmpleadoNuevo;
            Roles = RolesNuevos ?? new Dictionary<int, string>();

            // Reflejo para el Navegador (ver nota del encabezado).
            List<ClsRolInfo> RolesNavegador = new List<ClsRolInfo>();
            foreach (KeyValuePair<int, string> Rol in Roles)
                RolesNavegador.Add(new ClsRolInfo { IdRol = Rol.Key, NombreRol = Rol.Value });
            ClsSesionSeguridad.SeguridadMetIniciarSesion(IdUsuario, NombreUsuario, NombreEmpleado, RolesNavegador);
        }

        public static void ContabilidadMetCerrarSesion()
        {
            IdUsuario = 0;
            NombreUsuario = null;
            NombreEmpleado = null;
            Roles = new Dictionary<int, string>();
            ClsSesionSeguridad.SeguridadMetCerrarSesion();
        }
    }
}
