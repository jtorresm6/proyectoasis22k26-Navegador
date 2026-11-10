using System;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Clase base que hereda de la conexión y ejecuta las
 * sentencias SQL del módulo de traslados, tanto consultas
 * como operaciones de inserción, edición y eliminación.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Repositorios
{
    public abstract class ClsSentencias : ClsConexion
    {
        protected static OdbcParameter LogisticaMetCrearParametro(
            string Nombre, OdbcType Tipo, object Valor)
        {
            return new OdbcParameter(Nombre, Tipo)
            {
                Value = Valor ?? DBNull.Value
            };
        }

        protected static OdbcParameter LogisticaMetCrearParametro(
            string Nombre, OdbcType Tipo, int Tamano, object Valor)
        {
            return new OdbcParameter(Nombre, Tipo, Tamano)
            {
                Value = Valor ?? DBNull.Value
            };
        }

        protected static OdbcParameter LogisticaMetCrearParametroEnteroOpcional(
            string Nombre, int Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Int)
            {
                Value = Valor > 0 ? (object)Valor : DBNull.Value
            };
        }

        protected static int LogisticaMetEjecutarNoConsulta(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Consulta, params OdbcParameter[] Parametros)
        {
            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                if (Parametros != null && Parametros.Length > 0)
                {
                    Comando.Parameters.AddRange(Parametros);
                }

                return Comando.ExecuteNonQuery();
            }
        }

        protected static object LogisticaMetEjecutarEscalar(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            string Consulta, params OdbcParameter[] Parametros)
        {
            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                if (Parametros != null && Parametros.Length > 0)
                {
                    Comando.Parameters.AddRange(Parametros);
                }

                return Comando.ExecuteScalar();
            }
        }

        protected static int LogisticaMetObtenerUltimoId(
            OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            return Convert.ToInt32(
                LogisticaMetEjecutarEscalar(Conexion, Transaccion, "SELECT LAST_INSERT_ID()"));
        }

        protected static long LogisticaMetObtenerUltimoIdLargo(
            OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            return Convert.ToInt64(
                LogisticaMetEjecutarEscalar(Conexion, Transaccion, "SELECT LAST_INSERT_ID()"));
        }
    }
}
