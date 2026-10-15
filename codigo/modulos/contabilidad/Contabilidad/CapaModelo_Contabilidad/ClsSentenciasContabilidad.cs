/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Clase base que hereda de la conexión y ejecuta las sentencias SQL
 *  del módulo: instrucciones de escritura y consultas que devuelven
 *  una tabla, con o sin parámetros.
 * ===================================================================
*/

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Contabilidad
{
    public abstract class ClsSentenciasContabilidad : ClsConexionContabilidad
    {
        public int ContabilidadFuncEjecutarNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            using (OdbcConnection ConexionActiva = ContabilidadFuncObtenerConexion())
            {
                ConexionActiva.Open();
                using (OdbcCommand Comando = new OdbcCommand())
                {
                    Comando.Connection = ConexionActiva;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = ComandoTipo;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    return Comando.ExecuteNonQuery();
                }
            }
        }

        public DataTable ContabilidadFuncEjecutarConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            return ContabilidadFuncEjecutarConsulta(ComandoTexto, ComandoTipo, new List<OdbcParameter>());
        }

        public DataTable ContabilidadFuncEjecutarConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            DataTable TablaDatos = new DataTable();
            using (OdbcConnection ConexionActiva = ContabilidadFuncObtenerConexion())
            {
                ConexionActiva.Open();
                using (OdbcCommand Comando = new OdbcCommand())
                {
                    Comando.Connection = ConexionActiva;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = ComandoTipo;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    using (OdbcDataReader LectorDatos = Comando.ExecuteReader())
                    {
                        TablaDatos.Load(LectorDatos);
                    }
                }
            }
            return TablaDatos;
        }
    }
}
