using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Centraliza la ejecución de consultas, sentencias y resultados
 *  escalares mediante la infraestructura de acceso a datos de Compras.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores
{
    public abstract class ClsSentenciasCompras : ClsConexionCompras
    {
        public int ComprasMetEjecucionNonQuery(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            return ComprasFuncEjecucionNonQuery(ComandoTexto, Parametros, ComandoTipo);
        }

        
        
        public object ComprasMetEjecucionEscalar(string ComandoTexto, List<OdbcParameter> Parametros, CommandType ComandoTipo)
        {
            return ComprasFuncEjecucionEscalar(ComandoTexto, Parametros, ComandoTipo);
        }

        public DataTable ComprasMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo)
        {
            return ComprasFuncEjecucionConsulta(ComandoTexto, ComandoTipo);
        }

        public DataTable ComprasMetEjecucionConsulta(string ComandoTexto, CommandType ComandoTipo, List<OdbcParameter> Parametros)
        {
            return ComprasFuncEjecucionConsulta(ComandoTexto, ComandoTipo, Parametros);
        }
    }
}
