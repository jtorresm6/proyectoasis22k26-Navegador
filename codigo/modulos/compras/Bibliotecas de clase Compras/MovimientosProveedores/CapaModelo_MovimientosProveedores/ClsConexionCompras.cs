using System.Data.Odbc;
using CapaModelo_MovimientosProveedores.Repositorios;

namespace CapaModelo_MovimientosProveedores
{
    public abstract class ClsConexionCompras : RepositorioMaestro
    {
        protected OdbcConnection ComprasMetObtenerConexion() { return ObtenerConexion(); }
    }
}
