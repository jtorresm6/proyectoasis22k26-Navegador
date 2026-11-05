using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_ComisionVendedores.Repositorios
{
    public abstract class ClsSentencias : ClsConexion
    {
        protected int VentasFuncEjecucionNonQuery(
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

        protected DataTable VentasFuncEjecucionConsulta(
            string ComandoTexto,
            List<OdbcParameter> Parametros)
        {
            DataTable TablaDatos = new DataTable();
            using (OdbcConnection ConexionActiva = VentasFuncObtenerConexion())
            {
                ConexionActiva.Open();
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
