using System.Data;
using System.Data.Odbc;


// Jose Javier Torres - 0901-23-1091 16/09/2026

namespace CapaModelo_Navegador
{
    // Todo lo relacionado a usuarios: login y permisos
    public class ClsUsuarios
    {
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        public DataTable NavegadorFuncValidarUsuario(string Usuario, string Clave)
        {
            string ConsultaSQL = "SELECT id_usuario, nombre_usuario, id_rol FROM tbl_usuarios " +
                          "WHERE nombre_usuario = ? AND contrasena = ? AND estado_usuario = 1";

            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@Usuario", Usuario);
                    Comando.Parameters.AddWithValue("@clave", Clave);

                    using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(Comando))
                        AdaptadorDatos.Fill(TablaDatos);
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }

            return TablaDatos;
        }
        public bool NavegadorFuncGuardarUsuarioPermisoBD(int IdUsuario, int IdAplicacion, int IdModulo, int IdPermiso)
        {
            return true;
        }
    }
}

// Jose Javier Torres - 0901-23-1091 16/09/2026
