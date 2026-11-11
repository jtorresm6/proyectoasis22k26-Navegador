/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito :
 *  Clase base abstracta que sabe conectarse a la base de datos por
 *  ODBC usando el DSN EmbutidosS.A. De aquí heredan los repositorios.
 * ===================================================================
*/

using System;
using System.Data.Odbc;

namespace CapaModelo_Contabilidad.Repositorios
{
    public abstract class ClsRepositorioContabilidad
    {
        protected readonly string _CadenaConexion;

        protected ClsRepositorioContabilidad()
        {
            _CadenaConexion = "Dsn=EmbutidosS.A";
        }

        protected OdbcConnection ContabilidadFuncObtenerConexion()
        {
            return new OdbcConnection(_CadenaConexion);
        }

        public void ContabilidadMetDesconexion(OdbcConnection ConexionOdbc)
        {
            try
            {
                ConexionOdbc.Close();
            }
            catch (OdbcException)
            {
                Console.WriteLine("No se desconectó");
            }
        }
    }
}
