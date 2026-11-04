using System.Data.Odbc;

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
        private readonly string _CadenaConexion;

        protected ClsRepositorioCompras()
        {
            _CadenaConexion = "Dsn=EmbutidosS.A";
        }
        protected OdbcConnection ComprasFuncObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }
    }

}
