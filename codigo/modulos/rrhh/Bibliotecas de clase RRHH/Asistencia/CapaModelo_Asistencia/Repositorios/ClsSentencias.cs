using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta las consultas y los cambios de la vista de Asistencia.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Repositorios
{
    public class ClsSentencias : ClsConexion
    {
        public DataTable RrhhMetEjecucionConsulta(string Consulta, CommandType Tipo)
        {
            var Tabla = new DataTable();
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                Comando.CommandType = Tipo;
                Conexion.Open();
                using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Comando))
                {
                    Adaptador.Fill(Tabla);
                }
            }
            return Tabla;
        }

        public int RrhhMetEjecucionNonQuery(string Consulta, List<OdbcParameter> Parametros, CommandType Tipo)
        {
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                Comando.CommandType = Tipo;
                if (Parametros != null)
                {
                    foreach (OdbcParameter Parametro in Parametros)
                    {
                        Comando.Parameters.Add(Parametro);
                    }
                }

                Conexion.Open();
                return Comando.ExecuteNonQuery();
            }
        }

        public object RrhhMetEjecucionEscalar(string Consulta, List<OdbcParameter> Parametros)
        {
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                if (Parametros != null)
                {
                    foreach (OdbcParameter Parametro in Parametros)
                    {
                        Comando.Parameters.Add(Parametro);
                    }
                }
                Conexion.Open();
                return Comando.ExecuteScalar();
            }
        }

        // Inserta y devuelve el id generado, en la misma conexión para que LAST_INSERT_ID sea el correcto
        public int RrhhMetEjecucionInsert(string Consulta, List<OdbcParameter> Parametros)
        {
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            {
                Conexion.Open();
                using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
                {
                    if (Parametros != null)
                    {
                        foreach (OdbcParameter Parametro in Parametros)
                        {
                            Comando.Parameters.Add(Parametro);
                        }
                    }
                    Comando.ExecuteNonQuery();
                }
                using (OdbcCommand Comando = new OdbcCommand("SELECT LAST_INSERT_ID()", Conexion))
                {
                    return System.Convert.ToInt32(Comando.ExecuteScalar());
                }
            }
        }
    }
}
