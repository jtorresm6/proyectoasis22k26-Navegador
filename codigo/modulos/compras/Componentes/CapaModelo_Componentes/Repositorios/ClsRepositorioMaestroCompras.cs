using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Ejecuta consultas parametrizadas, operaciones escalares y transacciones de acceso a datos.
 * ===================================================================
*/

namespace CapaModelo_Componentes.Repositorios
{
    public abstract class ClsRepositorioMaestroCompras : ClsRepositorioCompras
    {
        protected DataTable ComprasFuncEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo,
            List<OdbcParameter> Parametros = null)
        {
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, ComandoTexto, ComandoTipo, Parametros))
                using (var Lector = Comando.ExecuteReader())
                {
                    var Datos = new DataTable();
                    Datos.Load(Lector);
                    return Datos;
                }
            }, false);
        }

        protected int ComprasFuncEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros,
            CommandType ComandoTipo)
        {
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, ComandoTexto, ComandoTipo, Parametros))
                    return Comando.ExecuteNonQuery();
            }, false);
        }

        protected object ComprasFuncEjecucionEscalar(string ComandoTexto, List<OdbcParameter> Parametros,
            CommandType ComandoTipo)
        {
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, ComandoTexto, ComandoTipo, Parametros))
                    return Comando.ExecuteScalar();
            }, false);
        }

        protected Resultado ComprasFuncEjecutar<Resultado>(Func<OdbcConnection, OdbcTransaction, Resultado> Operacion,
            bool UsarTransaccion)
        {
            if (Operacion == null) throw new ArgumentNullException(nameof(Operacion));
            try
            {
                using (var Conexion = ComprasFuncObtenerConexion())
                {
                    Conexion.Open();
                    if (!UsarTransaccion) return Operacion(Conexion, null);
                    using (var Transaccion = Conexion.BeginTransaction())
                    {
                        Resultado Valor = Operacion(Conexion, Transaccion);
                        Transaccion.Commit();
                        return Valor;
                    }
                }
            }
            catch (OdbcException Error)
            {
                throw ClsExcepcionDatosCompras.ComprasFuncTraducir(Error);
            }
        }

        protected static OdbcCommand ComprasFuncCrearComando(OdbcConnection Conexion, OdbcTransaction Transaccion,
            string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            var Comando = new OdbcCommand(ComandoTexto, Conexion, Transaccion)
            {
                CommandType = ComandoTipo,
                CommandTimeout = 20
            };
            if (Parametros != null) Comando.Parameters.AddRange(Parametros.ToArray());
            return Comando;
        }
    }
}
