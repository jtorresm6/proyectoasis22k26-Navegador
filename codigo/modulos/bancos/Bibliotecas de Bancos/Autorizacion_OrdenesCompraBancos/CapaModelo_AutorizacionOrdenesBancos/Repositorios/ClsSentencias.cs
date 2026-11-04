using CapaModelo_AutorizacionOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  La clase ClsSentencias centraliza la ejecución de comandos SQL por ODBC
 *  (consultas, comandos sin retorno y escalares, con o sin transacción) para
 *  que los repositorios hereden de ella.
 * ===================================================================
*/

namespace CapaModelo_AutorizacionOrdenes.Repositorios
{
    public abstract class ClsSentencias : ClsConexion
    {
        private DataTable _TablaDatos;
        public int BancosMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            using (var ConexionActiva = BancosMetObtenerConexion())
            {
                ConexionActiva.Open();
                using (var Comando = new OdbcCommand())
                {
                    Comando.Connection = ConexionActiva;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = ComandoTipo;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    return Comando.ExecuteNonQuery();
                }
            }
        }
        // PARA NAVEGADOR
        public int BancosMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            using (var Comando = new OdbcCommand())
            {
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
                Comando.CommandText = ComandoTexto;
                Comando.CommandType = ComandoTipo;
                Comando.Parameters.AddRange(Parametros.ToArray());
                return Comando.ExecuteNonQuery();
            }
        }
        // PARA NAVEGADOR

        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            _TablaDatos = new DataTable();
            using (var ConexionActiva = BancosMetObtenerConexion())
            {
                ConexionActiva.Open();
                using (var Comando = new OdbcCommand())
                {
                    Comando.Connection = ConexionActiva;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = ComandoTipo;
                    using (var LectorDatos = Comando.ExecuteReader())
                        _TablaDatos.Load(LectorDatos);
                }
                return _TablaDatos;
            }
        }
        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            _TablaDatos = new DataTable();
            using (var ConexionActiva = BancosMetObtenerConexion())
            {
                ConexionActiva.Open();
                using (var Comando = new OdbcCommand())
                {
                    Comando.Connection = ConexionActiva;
                    Comando.CommandText = ComandoTexto;
                    Comando.CommandType = ComandoTipo;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    using (var LectorDatos = Comando.ExecuteReader())
                        _TablaDatos.Load(LectorDatos);
                }
                return _TablaDatos;
            }
        }

        public object BancosMetEjecucionEscalar(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            using (var Comando = new OdbcCommand())
            {
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
                Comando.CommandText = ComandoTexto;
                Comando.CommandType = ComandoTipo;
                Comando.Parameters.AddRange(Parametros.ToArray());
                return Comando.ExecuteScalar();
            }
        }
    }
}

