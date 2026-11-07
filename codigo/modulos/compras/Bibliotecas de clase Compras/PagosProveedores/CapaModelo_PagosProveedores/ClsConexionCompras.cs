using System;
using System.Data.Odbc;



namespace CapaModelo_PagosProveedores
{
    public abstract class ClsConexionCompras
    {
        protected readonly string _ConnectionString;

        public ClsConexionCompras()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection ComprasMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        public void ComprasMetDesconexion(OdbcConnection ConexionOdbc)
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