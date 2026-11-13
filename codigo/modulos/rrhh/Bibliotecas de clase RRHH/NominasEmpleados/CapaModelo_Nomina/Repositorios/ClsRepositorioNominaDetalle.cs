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
    public class ClsRepositorioNominaDetalle : ClsSentencias, IRepositorioNominaDetalle
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioNominaDetalle()
        {
            _SelectAll = "SELECT idNominaDetalle, idNominaEncabezado, idPercepcionDeduccion, montoNominaDetalle, " +
                         "created_at, updated_at FROM tblNominaDetalle";
            _Insert = "INSERT INTO tblNominaDetalle (idNominaEncabezado, idPercepcionDeduccion, montoNominaDetalle) VALUES (?, ?, ?)";
            _Update = "UPDATE tblNominaDetalle SET montoNominaDetalle=? WHERE idNominaDetalle=?";
            _Delete = "DELETE FROM tblNominaDetalle WHERE idNominaDetalle=?";
        }

        public int RrhhMetAgregar(ClsNominaDetalle Entidad)
        {
            return RrhhMetEjecucionNonQuery(_Insert, RrhhMetCrearParametrosInsert(Entidad), CommandType.Text);
        }

        public int RrhhMetEditar(ClsNominaDetalle Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroDecimal("P_montoNominaDetalle", Entidad.MontoNominaDetalle));
            Parametros.Add(RrhhMetParametroEntero("P_idNominaDetalle", Entidad.IdNominaDetalle));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsNominaDetalle Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idNominaDetalle", Entidad.IdNominaDetalle));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsNominaDetalle> RrhhMetObtenerTodos()
        {
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            return RrhhMetConvertirTabla(TablaDatos);
        }

        public int RrhhMetAgregarLote(List<ClsNominaDetalle> Lineas)
        {
            using (var Conexion = RrhhMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        int Total = 0;
                        foreach (ClsNominaDetalle Linea in Lineas)
                        {
                            Total += RrhhMetEjecucionNonQuery(_Insert, RrhhMetCrearParametrosInsert(Linea),
                                CommandType.Text, Conexion, Transaccion);
                        }
                        Transaccion.Commit();
                        return Total;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public IEnumerable<ClsNominaDetalle> RrhhMetObtenerPorNomina(int IdNominaEncabezado)
        {
            string Sql = _SelectAll + " WHERE idNominaEncabezado = ? ORDER BY idNominaDetalle";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idNominaEncabezado", IdNominaEncabezado));
            var TablaDatos = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return RrhhMetConvertirTabla(TablaDatos);
        }

        public int RrhhMetContarPorNomina(int IdNominaEncabezado)
        {
            string Sql = "SELECT COUNT(*) FROM tblNominaDetalle WHERE idNominaEncabezado = ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idNominaEncabezado", IdNominaEncabezado));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        public List<int> RrhhMetObtenerNominasConDetalle()
        {
            var Lista = new List<int>();
            var Tabla = RrhhMetEjecucionConsulta("SELECT DISTINCT idNominaEncabezado FROM tblNominaDetalle", CommandType.Text);
            foreach (DataRow Fila in Tabla.Rows)
                Lista.Add(Convert.ToInt32(Fila[0]));
            return Lista;
        }

        public int? RrhhMetObtenerIdConceptoPorNombre(string NombreConcepto)
        {
            string Sql = "SELECT idPercepcionDeduccion FROM tblPercepcionDeduccion WHERE nombrePercepcionDeduccion = ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("P_nombre", OdbcType.VarChar) { Value = NombreConcepto });
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            if (Tabla.Rows.Count == 0)
                return null;
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        private List<ClsNominaDetalle> RrhhMetConvertirTabla(DataTable TablaDatos)
        {
            var Lista = new List<ClsNominaDetalle>();
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsNominaDetalle();
                Item.IdNominaDetalle = Convert.ToInt32(Fila["idNominaDetalle"]);
                Item.IdNominaEncabezado = Convert.ToInt32(Fila["idNominaEncabezado"]);
                Item.IdPercepcionDeduccion = Convert.ToInt32(Fila["idPercepcionDeduccion"]);
                Item.MontoNominaDetalle = Convert.ToDecimal(Fila["montoNominaDetalle"]);
                Item.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Item.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                Lista.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }

        private List<OdbcParameter> RrhhMetCrearParametrosInsert(ClsNominaDetalle Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idNominaEncabezado", Entidad.IdNominaEncabezado));
            Parametros.Add(RrhhMetParametroEntero("P_idPercepcionDeduccion", Entidad.IdPercepcionDeduccion));
            Parametros.Add(RrhhMetParametroDecimal("P_montoNominaDetalle", Entidad.MontoNominaDetalle));
            return Parametros;
        }

        private OdbcParameter RrhhMetParametroEntero(string Nombre, int Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Int) { Value = Valor };
        }

        private OdbcParameter RrhhMetParametroDecimal(string Nombre, decimal Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar) { Value = Valor.ToString("0.0000", CultureInfo.InvariantCulture) };
        }
    }
}
