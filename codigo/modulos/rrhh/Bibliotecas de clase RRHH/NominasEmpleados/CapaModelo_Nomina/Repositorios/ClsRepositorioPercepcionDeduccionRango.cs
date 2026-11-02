using CapaModelo_Nomina.Contratos;
using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioPercepcionDeduccionRango : ClsSentencias, iRepositorioPercepcionDeduccionRango
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioPercepcionDeduccionRango()
        {
            _SelectAll = "SELECT idPercepcionDeduccionRango, idPercepcionDeduccion, salarioDesdePercepcionDeduccionRango, " +
                         "salarioHastaPercepcionDeduccionRango, modoValorPercepcionDeduccionRango, " +
                         "valorPercepcionDeduccionRango, montoFijoBasePercepcionDeduccionRango, " +
                         "is_active, created_at, updated_at " +
                         "FROM tblPercepcionDeduccionRango";
            _Insert = "INSERT INTO tblPercepcionDeduccionRango (idPercepcionDeduccion, salarioDesdePercepcionDeduccionRango, " +
                      "salarioHastaPercepcionDeduccionRango, modoValorPercepcionDeduccionRango, " +
                      "valorPercepcionDeduccionRango, montoFijoBasePercepcionDeduccionRango, is_active) " +
                      "VALUES (?, ?, ?, ?, ?, ?, ?)";
            _Update = "UPDATE tblPercepcionDeduccionRango SET idPercepcionDeduccion=?, salarioDesdePercepcionDeduccionRango=?, " +
                      "salarioHastaPercepcionDeduccionRango=?, modoValorPercepcionDeduccionRango=?, " +
                      "valorPercepcionDeduccionRango=?, montoFijoBasePercepcionDeduccionRango=?, is_active=? " +
                      "WHERE idPercepcionDeduccionRango=?";
            _Delete = "DELETE FROM tblPercepcionDeduccionRango WHERE idPercepcionDeduccionRango=?";
        }

        public int RrhhMetAgregar(ClsPercepcionDeduccionRango Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int RrhhMetEditar(ClsPercepcionDeduccionRango Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionRango", Entidad.IdPercepcionDeduccionRango));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsPercepcionDeduccionRango Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionRango", Entidad.IdPercepcionDeduccionRango));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsPercepcionDeduccionRango> RrhhMetObtenerTodos()
        {
            var ListaRangos = new List<ClsPercepcionDeduccionRango>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsPercepcionDeduccionRango();
                Item.IdPercepcionDeduccionRango = Convert.ToInt32(Fila["idPercepcionDeduccionRango"]);
                Item.IdPercepcionDeduccion = Convert.ToInt32(Fila["idPercepcionDeduccion"]);
                Item.SalarioDesdePercepcionDeduccionRango = Convert.ToDecimal(Fila["salarioDesdePercepcionDeduccionRango"]);
                // salarioHasta acepta NULL en la BD (sin limite superior)
                Item.SalarioHastaPercepcionDeduccionRango = Fila["salarioHastaPercepcionDeduccionRango"] == DBNull.Value
                    ? (decimal?)null
                    : Convert.ToDecimal(Fila["salarioHastaPercepcionDeduccionRango"]);
                Item.ModoValorPercepcionDeduccionRango = Fila["modoValorPercepcionDeduccionRango"].ToString();
                Item.ValorPercepcionDeduccionRango = Convert.ToDecimal(Fila["valorPercepcionDeduccionRango"]);
                Item.MontoFijoBasePercepcionDeduccionRango = Convert.ToDecimal(Fila["montoFijoBasePercepcionDeduccionRango"]);
                Item.IsActive = Convert.ToBoolean(Fila["is_active"]);
                Item.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Item.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaRangos.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaRangos;
        }

        public int RrhhMetContarPorDesde(int IdPercepcionDeduccion, decimal SalarioDesde, int IdExcluir)
        {
            string Sql = "SELECT COUNT(*) FROM tblPercepcionDeduccionRango " +
                         "WHERE idPercepcionDeduccion = ? " +
                         "AND salarioDesdePercepcionDeduccionRango = CAST(? AS DECIMAL(12,4)) " +
                         "AND idPercepcionDeduccionRango <> ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", IdPercepcionDeduccion));
            Parametros.Add(RrhhMetParametroDecimal("P_salarioDesde", SalarioDesde));
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionRango", IdExcluir));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        // Parametros comunes de INSERT y UPDATE (mismo orden que el SQL, ODBC es posicional)
        private List<OdbcParameter> RrhhMetCrearParametros(ClsPercepcionDeduccionRango Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", OdbcType.Int) { Value = Entidad.IdPercepcionDeduccion });
            Parametros.Add(RrhhMetParametroDecimal("P_salarioDesdePercepcionDeduccionRango", Entidad.SalarioDesdePercepcionDeduccionRango));
            Parametros.Add(RrhhMetParametroDecimal("P_salarioHastaPercepcionDeduccionRango", Entidad.SalarioHastaPercepcionDeduccionRango));
            Parametros.Add(new OdbcParameter("P_modoValorPercepcionDeduccionRango", OdbcType.VarChar) { Value = Entidad.ModoValorPercepcionDeduccionRango });
            Parametros.Add(RrhhMetParametroDecimal("P_valorPercepcionDeduccionRango", Entidad.ValorPercepcionDeduccionRango));
            Parametros.Add(RrhhMetParametroDecimal("P_montoFijoBasePercepcionDeduccionRango", Entidad.MontoFijoBasePercepcionDeduccionRango));
            Parametros.Add(new OdbcParameter("P_is_active", OdbcType.Int) { Value = Entidad.IsActive ? 1 : 0 });
            return Parametros;
        }

        // Decimal como texto con punto (InvariantCulture) para evitar problemas de coma/punto con el driver; NULL si no hay valor
        private OdbcParameter RrhhMetParametroDecimal(string Nombre, decimal? Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar)
            {
                Value = Valor.HasValue
                    ? (object)Valor.Value.ToString("0.0000", CultureInfo.InvariantCulture)
                    : DBNull.Value
            };
        }
    }
}
