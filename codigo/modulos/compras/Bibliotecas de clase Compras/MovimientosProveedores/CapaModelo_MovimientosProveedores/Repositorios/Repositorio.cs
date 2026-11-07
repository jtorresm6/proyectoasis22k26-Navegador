using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    public abstract class Repositorio : ClsRepositorioCompras
    {
        protected OdbcConnection ObtenerConexion() { return ComprasFuncObtenerConexion(); }
    }
}
