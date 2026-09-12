using System;
using System.Data;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a login y permisos
    public class ctrlUsuario
    {
        private usuarios usuarios = new usuarios();

        public bool AutenticarUsuario(string usuario, string clave, out string mensaje, out DataRow datosUsuario)
        {
            mensaje = string.Empty;
            datosUsuario = null;

            if (string.IsNullOrWhiteSpace(usuario))
            {
                mensaje = "Debe ingresar el usuario.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(clave))
            {
                mensaje = "Debe ingresar la contraseña.";
                return false;
            }

            DataTable dt = usuarios.ValidarUsuario(usuario.Trim(), clave.Trim());

            if (dt.Rows.Count == 0)
            {
                mensaje = "Usuario o contraseña incorrectos.";
                return false;
            }

            datosUsuario = dt.Rows[0];
            return true;
        }

        // Se deja igual que antes, pendiente de conectar con la tabla real de permisos
        public bool GuardarRelacionUsuarioPermiso(int idUsuario, int idAplicacion, int idModulo, int idPermiso)
        {
            bool existeApp = usuarios.ExisteAplicacion(idAplicacion);
            bool existeMod = usuarios.ExisteModulo(idModulo);

            if (existeApp && existeMod)
                return usuarios.GuardarUsuarioPermisoBD(idUsuario, idAplicacion, idModulo, idPermiso);

            return false;
        }
    }
}