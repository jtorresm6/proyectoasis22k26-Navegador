using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;

namespace CapaModelo_MovimientosProveedores
{
    /// <summary>Reutiliza la conexión ODBC compartida de Compras.</summary>
    public abstract class ClsConexionCompras : ClsRepositorioMaestroCompras
    {
        protected OdbcConnection ComprasMetObtenerConexion()
        {
            return ComprasFuncObtenerConexion();
        }
    }
}
