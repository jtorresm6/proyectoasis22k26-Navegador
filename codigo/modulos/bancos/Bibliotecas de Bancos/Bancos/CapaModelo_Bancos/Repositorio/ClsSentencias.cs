using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Bancos.Repositorio
{
    public class ClsSentencias : ClsConexion
    {
        private DataTable _TablaDatos;
        public int BancosMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            using (var ConexionActiva = BancosMetObtenerConexion())
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
        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            _TablaDatos = new DataTable();
            using (var ConexionActiva = BancosMetObtenerConexion())
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
        public DataTable BancosMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            _TablaDatos = new DataTable();
            using (var ConexionActiva = BancosMetObtenerConexion())
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
