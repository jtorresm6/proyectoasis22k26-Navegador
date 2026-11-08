using System.Data.Odbc;
using CapaModelo_Seguridad;

namespace CapaModelo_PagosProveedores
{
    public abstract class ClsConexionCompras : ClsConexion
    {
        protected OdbcConnection ComprasMetObtenerConexion()
        {
            return SeguridadMetObtenerConexion();
        }

        public void ComprasMetDesconexion(OdbcConnection ConexionOdbc)
        {
            SeguridadMetDesconexion(ConexionOdbc);
        }
    }
}