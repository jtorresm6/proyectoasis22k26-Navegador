using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public abstract class ClsSentencias : ClsConexion
    {
        public int RrhhMetEjecucionNonQuery(string ComandoTexto, CommandType ComandoTipo)
        {
            return RrhhMetEjecucionNonQuery(ComandoTexto, new List<OdbcParameter>(), ComandoTipo);
        }

        public int RrhhMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            using (var ConexionActiva = RrhhMetObtenerConexion())
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

        // Con transaccion (conexion abierta y transaccion que maneja quien llama)
        public int RrhhMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo,
            OdbcConnection Conexion, OdbcTransaction Transaccion)
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

        // Devuelve un solo valor (por ejemplo SELECT LAST_INSERT_ID()) dentro de una transaccion
        public object RrhhMetEjecucionEscalar(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo,
            OdbcConnection Conexion, OdbcTransaction Transaccion)
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

        // ---------- Consultas (SELECT) ----------
        public DataTable RrhhMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            return RrhhMetEjecucionConsulta(ComandoTexto, ComandoTipo, new List<OdbcParameter>());
        }

        public DataTable RrhhMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            var TablaDatos = new DataTable();
            using (var ConexionActiva = RrhhMetObtenerConexion())
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
    }
}
