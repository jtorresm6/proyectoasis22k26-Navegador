using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Empleado.Repositorios
{
    public class ClsSentencias : ClsConexion
    {
        public DataTable RrhhMetEjecucionConsulta(string Consulta, CommandType Tipo)
        {
            var Tabla = new DataTable();
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                Comando.CommandType = Tipo;
                Conexion.Open();
                using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Comando))
                {
                    Adaptador.Fill(Tabla);
                }
            }
            return Tabla;
        }
    }
}