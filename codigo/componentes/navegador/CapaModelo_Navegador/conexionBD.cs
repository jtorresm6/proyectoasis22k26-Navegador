using System;
using System.Data.Odbc;

namespace CapaModelo_Navegador
{
    public class cls_ConexionBD
    {
        private const string Cns_DSN = "Dsn=BD_ProyectoNominas";

        public OdbcConnection met_ObtenerConexion()
        {
            try
            {
                OdbcConnection conn = new OdbcConnection(Cns_DSN);
                conn.Open();
                return conn;
            }
            catch (OdbcException ex)
            {
                throw new Exception("Error al conectar con la base de datos. Verifique el DSN.", ex);
            }
        }

        public void met_CerrarConexion(OdbcConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    conn.Close();
                }
                catch (OdbcException ex)
                {
                    throw new Exception("Error al cerrar la conexión con la base de datos.", ex);
                }
            }
        }
    }
}