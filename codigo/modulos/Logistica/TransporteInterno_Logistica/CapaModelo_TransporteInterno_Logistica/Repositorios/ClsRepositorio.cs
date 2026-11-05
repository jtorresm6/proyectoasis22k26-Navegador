// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorio.cs
// DESCRIPCIÓN: Conexión ODBC centralizada del módulo
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System.Data.Odbc;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public abstract class ClsRepositorio
    {
        private readonly string _CadenaConexion;

        protected ClsRepositorio()
        {
            _CadenaConexion = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection LogisticaMetObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }
    }
}
