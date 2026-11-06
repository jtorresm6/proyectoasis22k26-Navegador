
using System;
using System.Data.Odbc;

namespace CapaModelo_Enlace_Logistica.Repositorios
{
    public abstract class ClsRepositorioBase
    {
        private readonly string _cadenaConexion;

        protected ClsRepositorioBase()
            : this("Dsn=EmbutidosS.A;")
        {
        }

        protected ClsRepositorioBase(string cadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexion))
            {
                throw new ArgumentException(
                    "La cadena de conexión no puede estar vacía.",
                    nameof(cadenaConexion));
            }

            _cadenaConexion = cadenaConexion;
        }

        protected OdbcConnection ObtenerConexion()
        {
            return new OdbcConnection(_cadenaConexion);
        }
    }
}

