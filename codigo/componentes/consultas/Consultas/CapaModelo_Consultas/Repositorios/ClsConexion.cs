using System;
using System.Data.Odbc;

namespace CapaModelo_Consultas
{
    // Inicio de código de "Diego Fernando Santizo Samayoa" - carné: "0901-22-15950" - Fecha: "20/09/26"
    internal class ClsConexion
    {
        public OdbcConnection ConsultasFuncConexion()
        {
            OdbcConnection Conn = new OdbcConnection("Dsn=EmbutidosS.A");
            try
            {
                Conn.Open();
            }
            catch (OdbcException Ex)
            {
                Console.WriteLine("Conexion fallida. Error: " + Ex.Message);
            }
            return Conn;
        }

        public void ConsultasProcDesconexion(OdbcConnection conn)
        {
            try
            {
                conn.Close();
            }
            catch (OdbcException Ex)
            {
                Console.WriteLine("Error al cerrar la conexión. Error: " + Ex.Message);
            }
        }
    }
    // Fin de código de "Diego Fernando Santizo Samayoa" - carné: "0901-22-15950" - Fecha: "20/09/26"
}
