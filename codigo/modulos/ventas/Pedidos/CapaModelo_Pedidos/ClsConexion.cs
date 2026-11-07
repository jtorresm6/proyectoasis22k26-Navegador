using System.Data.Odbc;

namespace CapaModelo_Pedidos
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = CapaModelo_Ventas.ClsConexion.CadenaConexion;
        }

        protected OdbcConnection VentasFuncObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }
    }
}
