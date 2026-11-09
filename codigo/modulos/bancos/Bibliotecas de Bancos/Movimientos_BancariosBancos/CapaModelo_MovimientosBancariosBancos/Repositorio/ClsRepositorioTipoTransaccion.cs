/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using CapaModelo_MovimientosBancarios.Contratos;
using CapaModelo_MovimientosBancarios.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Repositorio
{
    public class ClsRepositorioTipoTransaccion : ClsSentencias, IRepositorioTipoTransaccion
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioTipoTransaccion()
        {
            _SelectAll = "SELECT * FROM tblTipoTransaccion";
            _Insert = "INSERT INTO tblTipoTransaccion VALUES (DEFAULT, ?, ?, ?, DEFAULT, DEFAULT, DEFAULT)";
            _Update = "UPDATE tblTipoTransaccion SET codigoTipoTransaccion=?, nombreTipoTransaccion=?, naturalezaTipoTransaccion=? WHERE idTipoTransaccion=?";
            _Delete = "DELETE FROM tblTipoTransaccion WHERE idTipoTransaccion=?";
        }

        public int BancosMetAgregar(ClsTipoTransaccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoTipoTransaccion", Entidad.CodigoTipoTransaccion));
            Parametros.Add(new OdbcParameter("p_nombreTipoTransaccion", Entidad.NombreTipoTransaccion));
            Parametros.Add(new OdbcParameter("p_naturalezaTipoTransaccion", Entidad.NaturalezaTipoTransaccion));

            return BancosMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int BancosMetEditar(ClsTipoTransaccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_codigoTipoTransaccion", Entidad.CodigoTipoTransaccion));
            Parametros.Add(new OdbcParameter("p_nombreTipoTransaccion", Entidad.NombreTipoTransaccion));
            Parametros.Add(new OdbcParameter("p_naturalezaTipoTransaccion", Entidad.NaturalezaTipoTransaccion));
            Parametros.Add(new OdbcParameter("p_idTipoTransaccion", Entidad.PK_TipoTransaccion));

            return BancosMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int BancosMetRemover(ClsTipoTransaccion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idTipoTransaccion", Entidad.PK_TipoTransaccion));

            return BancosMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsTipoTransaccion> BancosMetObtenerTodos()
        {
            var ListaTipoTransaccion = new List<ClsTipoTransaccion>();
            var TablaDatos = BancosMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var TipoTransaccion = new ClsTipoTransaccion();
                TipoTransaccion.PK_TipoTransaccion = Convert.ToInt32(Fila[0]);
                TipoTransaccion.CodigoTipoTransaccion = Fila[1].ToString();
                TipoTransaccion.NombreTipoTransaccion = Fila[2].ToString();
                TipoTransaccion.NaturalezaTipoTransaccion = Fila[3].ToString();
                TipoTransaccion.IsActive = Convert.ToBoolean(Fila[4]);
                TipoTransaccion.CreatedAt = Convert.ToDateTime(Fila[5]);
                TipoTransaccion.UpdatedAt = Convert.ToDateTime(Fila[6]);
                ListaTipoTransaccion.Add(TipoTransaccion);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaTipoTransaccion;
        }
    }
}