// ============================================================================
// MÓDULO: LOGÍSTICA - TRANSPORTE INTERNO
// ARCHIVO: ClsRepositorioMaestro.cs
// DESCRIPCIÓN: Consulta SQL con conexión ODBC y parámetros opcionales
// RESPONSABLE: BRIAN ANDREE DE LA CRUZ SOSA 0901-23-6124
// ============================================================================

using System.Data;
using System.Data.Odbc;

namespace CapaModelo_TransporteInterno_Logistica.Repositorios
{
    public abstract class ClsRepositorioMaestro : ClsRepositorio
    {
        public DataTable LogisticaMetEjecucionConsulta(string sql, params OdbcParameter[] parametros)
        {
            var tabla = new DataTable();
            using (var conexion = LogisticaMetObtenerConexion())
            using (var comando = new OdbcCommand(sql, conexion))
            {
                if (parametros != null && parametros.Length > 0)
                    comando.Parameters.AddRange(parametros);

                conexion.Open();
                using (var lector = comando.ExecuteReader())
                    tabla.Load(lector);
            }
            return tabla;
        }
    }
}
