using System;
using System.Data.Odbc;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        protected ClsConexion()
        {
            string CadenaConfigurada = Environment.GetEnvironmentVariable("EMBUTIDOS_ODBC_CONNECTION_STRING");
            _ConnectionString = string.IsNullOrWhiteSpace(CadenaConfigurada)
                ? "Dsn=EmbutidosS.A"
                : CadenaConfigurada;
        }

        protected OdbcConnection ComisionVendedoresMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        protected void ComisionVendedoresMetAbrirConexion(OdbcConnection Conexion)
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
                            "No está configurado el DSN ODBC EmbutidosS.A. Configure un DSN de 32 bits " +
                            "con el controlador MySQL ODBC 8.0 Unicode, o defina la variable de entorno " +
                            "EMBUTIDOS_ODBC_CONNECTION_STRING con la conexión a dbsistemaembutidos.", Error);
                }
                throw;
            }
        }

    }
}
