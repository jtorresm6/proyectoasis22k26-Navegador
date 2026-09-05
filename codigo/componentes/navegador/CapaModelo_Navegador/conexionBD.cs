using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Navegador
{
    class conexionBD
    {
        public OdbcConnection conexion()
        {
            OdbcConnection conn = new OdbcConnection("Dsn=Umg_taller");

            try
            {
                conn.Open();
            }
            catch (OdbcException)
            {
                Console.WriteLine("Error al conectar a la base de datos");
            }

            return conn;
        }

        public void desconexion(OdbcConnection conn)
        {
            try
            {
                if (conn != null && conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            catch (OdbcException)
            {
                Console.WriteLine("Error al desconectar de la base de datos");
            }
        }
    }

}