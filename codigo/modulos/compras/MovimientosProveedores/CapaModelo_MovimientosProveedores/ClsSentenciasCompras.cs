using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_MovimientosProveedores
{
    /// <summary>Centraliza las consultas parametrizadas mediante los componentes de Compras.</summary>
    public abstract class ClsSentenciasCompras : ClsConexionCompras
    {
        protected DataTable ComprasMetEjecucionConsulta(string Sql, params OdbcParameter[] Parametros)
        {
            return ComprasFuncEjecucionConsulta(Sql, CommandType.Text, new List<OdbcParameter>(Parametros));
        }
    }
}
