/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Valida las credenciales del inicio de sesión de Contabilidad: busca
 *  el usuario activo, verifica la contraseña contra su hash BCrypt y,
 *  si coincide, carga el usuario y sus roles en la sesión del módulo.
 * Reglas especificas:
 *  Ante usuario inexistente o contraseña incorrecta devuelve false sin
 *  distinguir el motivo; los errores de base de datos se propagan.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Contabilidad;

namespace CapaControlador_Contabilidad
{
    public class ClsControladorLoginContabilidad
    {
        private readonly ClsRepositorioUsuariosContabilidad _Repositorio = new ClsRepositorioUsuariosContabilidad();

        public bool ContabilidadFuncIniciarSesion(string NombreUsuario, string Contrasena)
        {
            if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrEmpty(Contrasena)) return false;

            DataTable Usuario = _Repositorio.ContabilidadFuncObtenerUsuario(NombreUsuario.Trim());
            if (Usuario.Rows.Count == 0) return false;

            DataRow Fila = Usuario.Rows[0];
            if (!BCrypt.Net.BCrypt.Verify(Contrasena, Convert.ToString(Fila["contrasenaUsuario"]))) return false;

            int IdUsuario = Convert.ToInt32(Fila["idUsuario"]);
            Dictionary<int, string> Roles = new Dictionary<int, string>();
            foreach (DataRow FilaRol in _Repositorio.ContabilidadFuncObtenerRoles(IdUsuario).Rows)
                Roles[Convert.ToInt32(FilaRol["idRol"])] = Convert.ToString(FilaRol["nombreRol"]);

            ClsSesionContabilidad.ContabilidadMetIniciarSesion(IdUsuario,
                Convert.ToString(Fila["nombreUsuario"]), Convert.ToString(Fila["nombreEmpleado"]), Roles);
            return true;
        }

        public void ContabilidadMetCerrarSesion()
        {
            ClsSesionContabilidad.ContabilidadMetCerrarSesion();
        }
    }
}
