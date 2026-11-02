using CapaModelo_Nomina.Contratos;
using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioPercepcionDeduccion : ClsSentencias, IRepositorioPercepcionDeduccion
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioPercepcionDeduccion()
        {
            _SelectAll = "SELECT idPercepcionDeduccion, nombrePercepcionDeduccion, periodoPercepcionDeduccion, " +
                         "efectoPercepcionDeduccion, tipoPercepcionDeduccion, valorTipoPercepcionDeduccion, " +
                         "aplicacionPercepcionDeduccion, is_active, created_at, updated_at " +
                         "FROM tblPercepcionDeduccion";
            _Insert = "INSERT INTO tblPercepcionDeduccion (nombrePercepcionDeduccion, periodoPercepcionDeduccion, " +
                      "efectoPercepcionDeduccion, tipoPercepcionDeduccion, valorTipoPercepcionDeduccion, " +
                      "aplicacionPercepcionDeduccion, is_active) VALUES (?, ?, ?, ?, ?, ?, ?)";
            _Update = "UPDATE tblPercepcionDeduccion SET nombrePercepcionDeduccion=?, periodoPercepcionDeduccion=?, " +
                      "efectoPercepcionDeduccion=?, tipoPercepcionDeduccion=?, valorTipoPercepcionDeduccion=?, " +
                      "aplicacionPercepcionDeduccion=?, is_active=? WHERE idPercepcionDeduccion=?";
            _Delete = "DELETE FROM tblPercepcionDeduccion WHERE idPercepcionDeduccion=?";
        }

        public int RrhhMetAgregar(ClsPercepcionDeduccion Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int RrhhMetEditar(ClsPercepcionDeduccion Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", Entidad.IdPercepcionDeduccion));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsPercepcionDeduccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", Entidad.IdPercepcionDeduccion));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsPercepcionDeduccion> RrhhMetObtenerTodos()
        {
            var ListaPercepcionesDeducciones = new List<ClsPercepcionDeduccion>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsPercepcionDeduccion();
                Item.IdPercepcionDeduccion = Convert.ToInt32(Fila["idPercepcionDeduccion"]);
                Item.NombrePercepcionDeduccion = Fila["nombrePercepcionDeduccion"].ToString();
                Item.PeriodoPercepcionDeduccion = Fila["periodoPercepcionDeduccion"].ToString();
                Item.EfectoPercepcionDeduccion = Convert.ToBoolean(Fila["efectoPercepcionDeduccion"]);
                Item.TipoPercepcionDeduccion = Fila["tipoPercepcionDeduccion"].ToString();
                Item.ValorTipoPercepcionDeduccion = Convert.ToDecimal(Fila["valorTipoPercepcionDeduccion"]);
                Item.AplicacionPercepcionDeduccion = Fila["aplicacionPercepcionDeduccion"].ToString();
                Item.IsActive = Convert.ToBoolean(Fila["is_active"]);
                Item.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Item.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaPercepcionesDeducciones.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaPercepcionesDeducciones;
        }

        public int RrhhMetContarPorNombre(string NombrePercepcionDeduccion, int IdExcluir)
        {
            string Sql = "SELECT COUNT(*) FROM tblPercepcionDeduccion " +
                         "WHERE nombrePercepcionDeduccion = ? AND idPercepcionDeduccion <> ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_nombrePercepcionDeduccion", NombrePercepcionDeduccion));
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", IdExcluir));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        // Parametros comunes de INSERT y UPDATE (mismo orden que el SQL, ODBC es posicional)
        private List<OdbcParameter> RrhhMetCrearParametros(ClsPercepcionDeduccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_nombrePercepcionDeduccion", OdbcType.VarChar) { Value = Entidad.NombrePercepcionDeduccion });
            Parametros.Add(new OdbcParameter("P_periodoPercepcionDeduccion", OdbcType.VarChar) { Value = Entidad.PeriodoPercepcionDeduccion });
            Parametros.Add(new OdbcParameter("P_efectoPercepcionDeduccion", OdbcType.Int) { Value = Entidad.EfectoPercepcionDeduccion ? 1 : 0 });
            Parametros.Add(new OdbcParameter("P_tipoPercepcionDeduccion", OdbcType.VarChar) { Value = Entidad.TipoPercepcionDeduccion });
            Parametros.Add(new OdbcParameter("P_valorTipoPercepcionDeduccion", OdbcType.VarChar)
            {
                Value = Entidad.ValorTipoPercepcionDeduccion.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture)
            });
            Parametros.Add(new OdbcParameter("P_aplicacionPercepcionDeduccion", OdbcType.VarChar) { Value = Entidad.AplicacionPercepcionDeduccion });
            Parametros.Add(new OdbcParameter("P_is_active", OdbcType.Int) { Value = Entidad.IsActive ? 1 : 0 });
            return Parametros;
        }
    }
}
