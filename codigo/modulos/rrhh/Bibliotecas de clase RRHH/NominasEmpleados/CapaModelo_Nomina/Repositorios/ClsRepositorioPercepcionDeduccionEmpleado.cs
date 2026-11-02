using CapaModelo_Nomina.Entidades;
using CapaModelo_Nomina.Contratos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Repositorios
{
    public class ClsRepositorioPercepcionDeduccionEmpleado : ClsSentencias, IRepositorioPercepcionDeduccionEmpleado
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioPercepcionDeduccionEmpleado()
        {
            _SelectAll = "SELECT idPercepcionDeduccionEmpleado, idPercepcionDeduccion, idEmpleado, idDepartamento, " +
                         "is_active, created_at, updated_at " +
                         "FROM tblPercepcionDeduccionEmpleado";
            _Insert = "INSERT INTO tblPercepcionDeduccionEmpleado (idPercepcionDeduccion, idEmpleado, idDepartamento, " +
                      "is_active) VALUES (?, ?, ?, ?)";
            _Update = "UPDATE tblPercepcionDeduccionEmpleado SET idPercepcionDeduccion=?, idEmpleado=?, " +
                      "idDepartamento=?, is_active=? WHERE idPercepcionDeduccionEmpleado=?";
            _Delete = "DELETE FROM tblPercepcionDeduccionEmpleado WHERE idPercepcionDeduccionEmpleado=?";
        }

        public int RrhhMetAgregar(ClsPercepcionDeduccionEmpleado Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int RrhhMetEditar(ClsPercepcionDeduccionEmpleado Entidad)
        {
            var Parametros = RrhhMetCrearParametros(Entidad);
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionEmpleado", Entidad.IdPercepcionDeduccionEmpleado));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsPercepcionDeduccionEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionEmpleado", Entidad.IdPercepcionDeduccionEmpleado));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsPercepcionDeduccionEmpleado> RrhhMetObtenerTodos()
        {
            var ListaAsignaciones = new List<ClsPercepcionDeduccionEmpleado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsPercepcionDeduccionEmpleado();
                Item.IdPercepcionDeduccionEmpleado = Convert.ToInt32(Fila["idPercepcionDeduccionEmpleado"]);
                Item.IdPercepcionDeduccion = Convert.ToInt32(Fila["idPercepcionDeduccion"]);
                // idEmpleado e idDepartamento aceptan NULL en la BD
                Item.IdEmpleado = Fila["idEmpleado"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["idEmpleado"]);
                Item.IdDepartamento = Fila["idDepartamento"] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila["idDepartamento"]);
                Item.IsActive = Convert.ToBoolean(Fila["is_active"]);
                Item.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Item.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                ListaAsignaciones.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaAsignaciones;
        }

        public int RrhhMetContarPorEmpleado(int IdPercepcionDeduccion, int IdEmpleado, int IdExcluir)
        {
            string Sql = "SELECT COUNT(*) FROM tblPercepcionDeduccionEmpleado " +
                         "WHERE idPercepcionDeduccion = ? AND idEmpleado = ? AND idPercepcionDeduccionEmpleado <> ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", IdPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("P_idEmpleado", IdEmpleado));
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionEmpleado", IdExcluir));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        public int RrhhMetContarPorDepartamento(int IdPercepcionDeduccion, int IdDepartamento, int IdExcluir)
        {
            string Sql = "SELECT COUNT(*) FROM tblPercepcionDeduccionEmpleado " +
                         "WHERE idPercepcionDeduccion = ? AND idDepartamento = ? AND idPercepcionDeduccionEmpleado <> ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", IdPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("P_idDepartamento", IdDepartamento));
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccionEmpleado", IdExcluir));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        // Parametros comunes de INSERT y UPDATE (mismo orden que el SQL, ODBC es posicional)
        private List<OdbcParameter> RrhhMetCrearParametros(ClsPercepcionDeduccionEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_idPercepcionDeduccion", OdbcType.Int) { Value = Entidad.IdPercepcionDeduccion });
            // Si no hay empleado o departamento se envia NULL a la BD
            Parametros.Add(new OdbcParameter("P_idEmpleado", OdbcType.Int) { Value = (object)Entidad.IdEmpleado ?? DBNull.Value });
            Parametros.Add(new OdbcParameter("P_idDepartamento", OdbcType.Int) { Value = (object)Entidad.IdDepartamento ?? DBNull.Value });
            Parametros.Add(new OdbcParameter("P_is_active", OdbcType.Int) { Value = Entidad.IsActive ? 1 : 0 });
            return Parametros;
        }
    }
}
