using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Clase base abstracta que centraliza la ejecución de comandos
 *  ODBC (consultas, comandos sin retorno y escalares, con o sin
 *  transacción) para que los repositorios de Generación de Pagos
 *  hereden de ella.
 * ================================================================
 */

namespace CapaModelo_GeneracionPagos.Repositorio
{
    public abstract class ClsSentencias : ClsConexion
    {
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

        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            var TablaDatos = new DataTable();
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
                        TablaDatos.Load(LectorDatos);
                }
            }
            return TablaDatos;
        }

        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros, OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            var TablaDatos = new DataTable();
            using (var Comando = new OdbcCommand())
            {
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;
                Comando.CommandText = ComandoTexto;
                Comando.CommandType = ComandoTipo;
                Comando.Parameters.AddRange(Parametros.ToArray());
                using (var LectorDatos = Comando.ExecuteReader())
                    TablaDatos.Load(LectorDatos);
            }
            return TablaDatos;
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
