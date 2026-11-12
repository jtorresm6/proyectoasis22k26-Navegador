/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Autor  : Andre De Jesus Gonzalez Camey
 * Carné  : 9959-23-1224 -- 9959-23-3117
 * Fecha  : 08/11/2026
 * ================================================================
 * Propósito :
 *  Proporciona la conexión de base de datos usada por el módulo
 *  de cierre bancario.
 * ================================================================
 */
using System;
using System.Data.Odbc;

namespace CapaModelo_CierreBancario.Repositorio
{
    public abstract class ClsConexion
    {
        protected string _ConnectionString;

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
                Console.WriteLine("No se pudo cerrar la conexión a la base de datos.");
            }
        }
    }
}
