using System.Data.Odbc;

namespace CapaModelo_CierreInventario_Logistica.Repositorio
{
    public class ClsConexion
    {
        public OdbcConnection AbrirConexion()
        {
            OdbcConnection conexion = new OdbcConnection("DSN=EmbutidosS.A;");
            conexion.Open();
            return conexion;
        }
    }
}
