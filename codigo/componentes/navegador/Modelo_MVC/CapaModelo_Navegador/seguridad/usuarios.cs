using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Navegador
{
    // Todo lo relacionado a usuarios: login y permisos
    public class usuarios
    {
        conexionBD conn = new conexionBD();

        public DataTable ValidarUsuario(string usuario, string clave)
        {
            string sSQL = "SELECT id_usuario, nombre_usuario, id_rol FROM tbl_usuarios " +
                          "WHERE nombre_usuario = ? AND contrasena = ? AND estado_usuario = 1";

            OdbcConnection conexion = conn.conexion();
            DataTable dt = new DataTable();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@clave", clave);

                    using (OdbcDataAdapter da = new OdbcDataAdapter(comando))
                        da.Fill(dt);
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dt;
        }

        // Placeholder mientras se define el modulo real de aplicaciones/modulos/permisos
        public bool ExisteAplicacion(int idAplicacion)
        {
            return idAplicacion > 0;
        }

        public bool ExisteModulo(int idModulo)
        {
            return idModulo > 0;
        }

        public bool GuardarUsuarioPermisoBD(int idUsuario, int idAplicacion, int idModulo, int idPermiso)
        {
            return true;
        }
    }
}