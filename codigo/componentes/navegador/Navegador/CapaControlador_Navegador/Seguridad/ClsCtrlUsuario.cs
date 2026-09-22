using System;
using System.Data;
using CapaModelo_Navegador;

// Jose Javier Torres - 0901-23-1091 16/09/2026

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a login y permisos
    public class ClsCtrlUsuario
    {
        private ClsUsuarios _Usuarios = new ClsUsuarios();

        public bool NavegadorFuncAutenticarUsuario(string Usuario, string Clave, out string Mensaje, out DataRow DatosUsuario)
        {
            Mensaje = string.Empty;
            DatosUsuario = null;

            if (string.IsNullOrWhiteSpace(Usuario))
            {
                Mensaje = "Debe ingresar el Usuario.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Clave))
            {
                Mensaje = "Debe ingresar la contraseña.";
                return false;
            }

            DataTable TablaDatos = _Usuarios.NavegadorFuncValidarUsuario(Usuario.Trim(), Clave.Trim());

            if (TablaDatos.Rows.Count == 0)
            {
                Mensaje = "Usuario o contraseña incorrectos.";
                return false;
            }

            DatosUsuario = TablaDatos.Rows[0];
            return true;
        }

        // Se deja igual que antes, pendiente de conectar con la tabla real de permisos
        public bool NavegadorFuncGuardarRelacionUsuarioPermiso(int IdUsuario, int IdAplicacion, int IdModulo, int IdPermiso)
        {
            //Inicio del cambio Dylan Rene Hernandez Recinos 0901-23-519
            // Valida que el Id de Aplicación y el Id de Módulo sean mayores a cero
            if (IdAplicacion <= 0 || IdModulo <= 0)
                return false;

            return _Usuarios.NavegadorFuncGuardarUsuarioPermisoBD(IdUsuario, IdAplicacion, IdModulo, IdPermiso);
        }
    }
}
// Jose Javier Torres - 0901-23-1091 16/09/2026
