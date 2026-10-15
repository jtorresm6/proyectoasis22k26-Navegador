/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Clase base para conectarse a la base de datos por ODBC usando el
 *  DSN EmbutidosS.A, igual que la conexión de Seguridad pero propia
 *  de Contabilidad: no depende de CapaModelo_Seguridad. De aquí
 *  heredan los repositorios del módulo.
 * ===================================================================
*/

using System;
using System.Data.Odbc;

namespace CapaModelo_Contabilidad
{
    public abstract class ClsConexionContabilidad
    {
        protected readonly string _CadenaConexion;

        protected ClsConexionContabilidad()
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
