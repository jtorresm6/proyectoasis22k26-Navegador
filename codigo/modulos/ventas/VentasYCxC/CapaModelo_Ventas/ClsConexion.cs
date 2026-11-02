using System;
using System.Data.Odbc;
using System.Configuration;


namespace CapaModelo_Ventas
{
    public abstract class ClsConexion
    {
        // Unica configuracion de conexion para el MDI y los modelos de Ventas.
        public static string CadenaConexion
        {
            get
            {
                string Cadena = Environment.GetEnvironmentVariable("VENTAS_ODBC");
                if (string.IsNullOrWhiteSpace(Cadena)) Cadena = ConfigurationManager.AppSettings["VentasConexion"];
                return string.IsNullOrWhiteSpace(Cadena) ? "Dsn=EmbutidosS.A;Connection Timeout=5" : Cadena;
            }
        }

        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = CadenaConexion;
        }

        protected OdbcConnection VentasFuncObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        public void VentasMetDesconectar(OdbcConnection ConexionOdbc)
        {
            try
            {
                ConexionOdbc.Close();
            }
            catch (OdbcException)
            {
                Console.WriteLine("No se desconecto");
            }
        }
    }
}
