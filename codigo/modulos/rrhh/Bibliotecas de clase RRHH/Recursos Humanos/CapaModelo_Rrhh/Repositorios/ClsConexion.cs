using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Modulo: RRHH
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 09/10/2026
 * ==================================================================
 * Propósito : Clase base para conectarse a la base de datos por ODBC
 * usando el DSN EmbutidosS.A. De aquí heredan los
 * repositorios para abrir y cerrar la conexión.
 * ===================================================================
 */

namespace CapaModelo_Rrhh.Repositorios
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
