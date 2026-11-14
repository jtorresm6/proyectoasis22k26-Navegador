using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Clase base para conectarse a la base de datos por ODBC
 * usando el DSN EmbutidosS.A. De aquí heredan los
 * repositorios para abrir la conexión.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Repositorios
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection LogisticaMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }
    }
}
