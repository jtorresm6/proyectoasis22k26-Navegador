using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Navegador
{
    public class Sentencias
    {
        conexionBD conn = new conexionBD();
        public OdbcDataAdapter llenarTbl(string nombreTabla)
        {
            string sSQL = "SELECT * FROM " + nombreTabla + " ;";
            OdbcDataAdapter daSentencias = new OdbcDataAdapter(sSQL, conn.conexion());
            return daSentencias;
        }

        public void ejecutarQuery(string sSQL)
        {
            try
            {
                OdbcConnection connection = conn.conexion();
                OdbcCommand cmd = new OdbcCommand(sSQL, connection);
                cmd.ExecuteNonQuery();
                conn.desconexion(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Sentencias.ejecutarQuery: " + ex.Message);
                throw;
            }
        }
    }
}
