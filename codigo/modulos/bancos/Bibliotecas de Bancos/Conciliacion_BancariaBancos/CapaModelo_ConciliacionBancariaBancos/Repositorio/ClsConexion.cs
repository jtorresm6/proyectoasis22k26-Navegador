/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Proporciona la conexión de base de datos usada por la conciliación
 *  bancaria.
 * ================================================================
 */
using System.Data.Odbc;

namespace CapaModelo_ConciliacionBancaria.Repositorio
{
    public abstract class ClsConexion
    {
        protected string _ConnectionString;

        protected ClsConexion()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection BancosMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }
    }
}
