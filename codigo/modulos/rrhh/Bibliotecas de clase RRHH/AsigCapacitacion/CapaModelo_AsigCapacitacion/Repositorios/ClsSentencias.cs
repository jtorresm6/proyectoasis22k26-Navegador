using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * ==================================================================
 * Área : RRHH
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 31/10/2026
 * ==================================================================
 * Propósito :
 *  El formulario del módulo de Asignacion Capacitacion nos 
 *  permite registrar, consultar, modificar y eliminar los 
 *  datos ingresados al sistema, tambien cuenta con Reportes y Ayudas.
 * ===================================================================
*/
namespace CapaModelo_AsigCapacitacion.Repositorios
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

        public int RrhhMetEjecucionNonQuery(string Consulta, List<OdbcParameter> Parametros, CommandType Tipo)
        {
            using (OdbcConnection Conexion = RrhhMetObtenerConexion())
            using (OdbcCommand Comando = new OdbcCommand(Consulta, Conexion))
            {
                Comando.CommandType = Tipo;

                if (Parametros != null)
                {
                    foreach (OdbcParameter Parametro in Parametros)
                    {
                        Comando.Parameters.Add(Parametro);
                    }
                }

                Conexion.Open();
                return Comando.ExecuteNonQuery();
            }
        }
    }
}