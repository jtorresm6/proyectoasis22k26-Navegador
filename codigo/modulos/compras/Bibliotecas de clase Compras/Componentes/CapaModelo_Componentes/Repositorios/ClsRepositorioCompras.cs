using System.Data.Odbc;
using CapaModelo_Seguridad;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Proporciona la conexión ODBC compartida que utilizan los repositorios de Compras.
 * ===================================================================
*/

namespace CapaModelo_Componentes.Repositorios
{
    public abstract class ClsRepositorioCompras
    {
        private readonly ConexionSeguridad _Conexion = new ConexionSeguridad();

        protected OdbcConnection ComprasFuncObtenerConexion()
        {
            return _Conexion.ObtenerConexion();
        }

        private sealed class ConexionSeguridad : ClsConexion
        {
            internal OdbcConnection ObtenerConexion()
            {
                return SeguridadMetObtenerConexion();
            }
        }
    }

}
