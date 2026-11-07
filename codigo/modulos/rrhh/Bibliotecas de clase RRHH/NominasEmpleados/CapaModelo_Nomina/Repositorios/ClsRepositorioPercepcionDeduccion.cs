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
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_nombre", Entidad.NombrePercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_periodo", Entidad.PeriodoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_efecto", Entidad.EfectoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_tipo", Entidad.TipoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_valor", Entidad.ValorTipoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_aplicacion", Entidad.AplicacionPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_is_active", Entidad.IsActive));
            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int RrhhMetEditar(ClsPercepcionDeduccion Entidad)
        {
            
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_nombre", Entidad.NombrePercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_periodo", Entidad.PeriodoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_efecto", Entidad.EfectoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_tipo", Entidad.TipoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_valor", Entidad.ValorTipoPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_aplicacion", Entidad.AplicacionPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_is_active", Entidad.IsActive));
            Parametros.Add(new OdbcParameter("p_id", Entidad.IdPercepcionDeduccion));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsPercepcionDeduccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_id", Entidad.IdPercepcionDeduccion));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsPercepcionDeduccion> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsPercepcionDeduccion>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsPercepcionDeduccion();
                Item.IdPercepcionDeduccion = Convert.ToInt32(Fila[0]);
                Item.NombrePercepcionDeduccion = Fila[1].ToString();
                Item.PeriodoPercepcionDeduccion = Fila[2].ToString();
                Item.EfectoPercepcionDeduccion = Convert.ToBoolean(Fila[3]);
                Item.TipoPercepcionDeduccion = Fila[4].ToString();
                Item.ValorTipoPercepcionDeduccion = Convert.ToDecimal(Fila[5]);
                Item.AplicacionPercepcionDeduccion = Fila[6].ToString();
                Item.IsActive = Convert.ToBoolean(Fila[7]);
                Item.CreatedAt = Convert.ToDateTime(Fila[8]);
                Item.UpdatedAt = Convert.ToDateTime(Fila[9]);
                Lista.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }
    }
}
