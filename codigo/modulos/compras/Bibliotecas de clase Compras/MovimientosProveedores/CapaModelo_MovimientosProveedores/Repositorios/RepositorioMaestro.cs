using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_MovimientosProveedores.Repositorios
{
    public abstract class RepositorioMaestro : Repositorio
    {
        protected DataTable EjecucionConsulta(string texto, CommandType tipo, params OdbcParameter[] parametros)
        {
            using (var conexion = ObtenerConexion())
            using (var comando = new OdbcCommand(texto, conexion) { CommandType = tipo, CommandTimeout = 20 })
            {
                comando.Parameters.AddRange(parametros);
                conexion.Open();
                using (var lector = comando.ExecuteReader())
                {
                    var tabla = new DataTable();
                    tabla.Load(lector);
                    return tabla;
                }
            }
        }

        protected int EjecucionNonQuery(string texto, List<OdbcParameter> parametros, CommandType tipo)
        {
            using (var conexion = ObtenerConexion())
            using (var comando = new OdbcCommand(texto, conexion) { CommandType = tipo, CommandTimeout = 20 })
            {
                comando.Parameters.AddRange(parametros.ToArray());
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }
    }
}
