/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Repositorio maestro abstracto: ejecuta las sentencias SQL de todo
 *  el módulo. Un método para insert, update y delete y otro para
 *  consultas que devuelven una tabla, con o sin parámetros.
 * ===================================================================
*/

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Contabilidad.Repositorios
{
    public abstract class ClsRepositorioMaestroContabilidad : ClsRepositorioContabilidad
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
