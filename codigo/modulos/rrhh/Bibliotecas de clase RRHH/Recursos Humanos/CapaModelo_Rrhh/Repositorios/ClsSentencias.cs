using CapaModelo_Rrhh.Repositorios;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área: Recuros Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 9/10/2026
 * ==================================================================
 * Propósito : Clase base que hereda de la conexión y ejecuta las
 * sentencias SQL del sistema, ya sea para insertar,
 * editar o eliminar o para hacer
 * consultas que devuelven una tabla de resultados, con
 * o sin parámetros.
 * ===================================================================
 */

namespace CapaModelo_Rrhh
{
    public abstract class ClsSentencias : ClsConexion
    {
        private DataTable _TablaDatos;
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
        // PARA NAVEGADOR
        public int RrhhMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo, OdbcConnection Conexion, OdbcTransaction Transaccion)
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

        public DataTable RrhhMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            _TablaDatos = new DataTable();
            using (var ConexionActiva = RrhhMetObtenerConexion())
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

        public DataTable RrhhMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            _TablaDatos = new DataTable();
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
                        _TablaDatos.Load(LectorDatos);
                }
                return _TablaDatos;
            }
        }
    }
}