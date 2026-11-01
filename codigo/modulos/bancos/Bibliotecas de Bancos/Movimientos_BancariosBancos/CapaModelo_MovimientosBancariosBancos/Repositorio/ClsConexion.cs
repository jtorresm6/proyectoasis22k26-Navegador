/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Oscar Emilio Morales Lemus
 * Carné  : 9959-23-3070
 * Fecha  : 23/10/2026
 * ================================================================
 * Propósito :
 *  Clase base abstracta de conexión para el módulo Movimientos
 *  Bancarios. Centraliza la cadena de conexión ODBC al DSN
 *  EmbutidosS.A y provee métodos para obtener y cerrar la conexión
 *  a la base de datos, siendo heredada por los repositorios del
 *  módulo.
 * ================================================================
 */

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public abstract class ClsConexion
    {
        protected readonly string _ConnectionString;

        public ClsConexion()
        {
            _ConnectionString = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection BancosMetObtenerConexion()
        {
            return new OdbcConnection(_ConnectionString);
        }

        public void BancosMetDesconexion(OdbcConnection ConexionOdbc)
        {
            try
            {
                ConexionOdbc.Close();
            }
            catch (OdbcException)
            {
                Console.WriteLine("No se pudo cerrar la conexión a la base de datos :(");
            }
        }
    }
}
