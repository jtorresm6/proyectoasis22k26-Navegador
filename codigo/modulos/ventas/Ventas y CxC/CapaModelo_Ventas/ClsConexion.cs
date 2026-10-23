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
                return string.IsNullOrWhiteSpace(Cadena) ? "Dsn=Ventasbd;Connection Timeout=5" : Cadena;
            }
        }

        protected readonly string _CadenaConexionLocal;

        public ClsConexion()
        {
            _CadenaConexionLocal = CadenaConexion;
        }

        protected OdbcConnection VentasMetObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexionLocal);
        }

        public void VentasMetDesconexion(OdbcConnection ConexionOdbc)
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
