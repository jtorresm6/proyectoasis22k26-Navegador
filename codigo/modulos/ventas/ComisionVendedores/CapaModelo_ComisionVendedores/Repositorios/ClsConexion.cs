using System.Data.Odbc;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        protected ClsConexion()
        {
            _ConnectionString = CapaModelo_Ventas.ClsConexion.CadenaConexion;
        }

        protected OdbcConnection VentasFuncObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

    }
}
