using System.Data.Odbc;
using CapaModelo_Componentes.Repositorios;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Proporciona acceso a la conexión ODBC compartida de Compras y
 *  permite liberar los recursos de conexión utilizados por pagos.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores
{
    public abstract class ClsConexionCompras : ClsRepositorioMaestroCompras
    {
        protected OdbcConnection ComprasMetObtenerConexion()
        {
            return ComprasFuncObtenerConexion();
        }

        public void ComprasMetDesconexion(OdbcConnection ConexionOdbc)
        {
            ConexionOdbc?.Dispose();
        }
    }
}
