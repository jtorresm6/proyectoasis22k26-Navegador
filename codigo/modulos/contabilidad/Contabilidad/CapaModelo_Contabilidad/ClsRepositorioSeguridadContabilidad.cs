/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 14/10/2026
 * ==================================================================
 * Propósito :
 *  Consulta el registro de aplicaciones de Contabilidad y los permisos
 *  de los roles sobre ellas, con la conexión propia del módulo.
 *  Solo realiza lecturas.
 * ===================================================================
*/

using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace CapaModelo_Contabilidad
{
    public class ClsRepositorioSeguridadContabilidad : ClsSentenciasContabilidad
    {
        public DataTable ContabilidadFuncObtenerAplicaciones()
        {
            return ContabilidadFuncEjecutarConsulta(
                "SELECT a.idModulo, a.idAplicacion, a.nombreAplicacion " +
                "FROM tblaplicacion a INNER JOIN tblmodulo m ON m.idModulo=a.idModulo " +
                "WHERE m.nombreModulo=? AND m.is_active=1 AND a.is_active=1",
                CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Modulo", "Contabilidad") });
        }

        // Un usuario tiene acceso si alguno de sus roles tiene asignada la aplicación.
        public bool ContabilidadFuncTieneAcceso(List<int> IdsRoles, int IdModulo, int IdAplicacion)
        {
            if (IdsRoles == null || IdsRoles.Count == 0) return false;

            StringBuilder Marcadores = new StringBuilder();
            List<OdbcParameter> Parametros = new List<OdbcParameter>();
            foreach (int IdRol in IdsRoles)
            {
                if (Marcadores.Length > 0) Marcadores.Append(",");
                Marcadores.Append("?");
                Parametros.Add(new OdbcParameter("Rol" + Parametros.Count, IdRol));
            }
            Parametros.Add(new OdbcParameter("Modulo", IdModulo));
            Parametros.Add(new OdbcParameter("Aplicacion", IdAplicacion));

            DataTable Tabla = ContabilidadFuncEjecutarConsulta(
                "SELECT COUNT(*) FROM tblrolmoduloaplicacion WHERE idRol IN (" + Marcadores +
                ") AND idModulo=? AND idAplicacion=?", CommandType.Text, Parametros);
            return Tabla.Rows.Count > 0 && System.Convert.ToInt64(Tabla.Rows[0][0]) > 0;
        }
    }
}
