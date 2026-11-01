using System;
using System.Data.Odbc;

namespace CapaModelo_Empleado.Repositorios
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection RrhhMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        public void RrhhMetDesconexion(OdbcConnection ConexionOdbc)
        {
            try
            {
                ConexionOdbc.Close();
            }
            catch (OdbcException)
            {
                Console.WriteLine("No se desconecto");
            }
        }
    }
}