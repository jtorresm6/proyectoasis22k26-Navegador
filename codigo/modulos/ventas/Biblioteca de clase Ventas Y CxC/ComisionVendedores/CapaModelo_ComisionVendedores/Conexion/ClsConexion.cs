// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Abre conexiones ODBC y prepara comandos con sus parámetros en el orden requerido.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_ComisionVendedores.Conexion
{
    public abstract class ClsConexion
    {
        protected readonly string _CadenaConexion;

        protected ClsConexion()
        {
            string CadenaConfigurada = Environment.GetEnvironmentVariable("EMBUTIDOS_ODBC_CONNECTION_STRING");
            _CadenaConexion = string.IsNullOrWhiteSpace(CadenaConfigurada)
                ? "Dsn=EmbutidosS.A"
                : CadenaConfigurada;
        }

        protected OdbcConnection VentasMetObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }

        protected void VentasMetAbrirConexion(OdbcConnection Conexion)
        {
            try
            {
                Conexion.Open();
            }
            catch (OdbcException Error)
            {
                foreach (OdbcError Detalle in Error.Errors)
                {
                    if (Detalle.SQLState == "IM002")
                        throw new System.InvalidOperationException(
                            "No está configurado el DSN ODBC EmbutidosS.A. Configure un DSN de " +
                            (Environment.Is64BitProcess ? "64 bits " : "32 bits ") +
                            "con el controlador MySQL ODBC 8.0 Unicode, o defina la variable de entorno " +
                            "EMBUTIDOS_ODBC_CONNECTION_STRING con la conexión a dbsistemaembutidos.", Error);
                }
                throw;
            }
        }

        protected int VentasMetEjecucionNonQuery(
            string ComandoTexto,
            List<OdbcParameter> Parametros,
            OdbcConnection Conexion,
            OdbcTransaction Transaccion)
        {
            using (OdbcCommand Comando = new OdbcCommand(ComandoTexto, Conexion, Transaccion))
            {
                Comando.CommandType = CommandType.Text;
                Comando.Parameters.AddRange(Parametros.ToArray());
                return Comando.ExecuteNonQuery();
            }
        }

        protected DataTable VentasMetEjecucionConsulta(
            string ComandoTexto,
            List<OdbcParameter> Parametros)
        {
            DataTable TablaDatos = new DataTable();
            using (OdbcConnection ConexionActiva = VentasMetObtenerConexion())
            {
                VentasMetAbrirConexion(ConexionActiva);
                using (OdbcCommand Comando = new OdbcCommand(ComandoTexto, ConexionActiva))
                {
                    Comando.CommandType = CommandType.Text;
                    Comando.Parameters.AddRange(Parametros.ToArray());
                    using (OdbcDataReader LectorDatos = Comando.ExecuteReader())
                        TablaDatos.Load(LectorDatos);
                }
            }
            return TablaDatos;
        }
    }
}
