using System;
using System.Collections.Generic;
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
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection RrhhMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        public void RrhhMetDesconexion(OdbcConnection ConexionOdbc)
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
