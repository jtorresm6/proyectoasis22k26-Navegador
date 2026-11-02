using CapaModelo_Liquidacion_Rrhh.Contratos;
using CapaModelo_Liquidacion_Rrhh.Entidades;
using CapaModelo_Liquidacion_Rrhh.Repositorios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_Liquidacion_Rrhh.Repositorios
{
    public class ClsRepositorioBajaEmpleado : ClsSentencias, IRepositorioBajaEmpleado
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioBajaEmpleado()
        {
            _SelectAll = "SELECT idBajaEmpleado, idEmpleado, idMotivoBaja, fechaBajaEmpleado, observacionBajaEmpleado, idUsuarioRegistra, is_active FROM tblBajaEmpleado";

            _Insert = "INSERT INTO tblBajaEmpleado (idEmpleado, idMotivoBaja, fechaBajaEmpleado, observacionBajaEmpleado, idUsuarioRegistra, is_active) VALUES (?,?,?,?,?,?)";

            _Update = "UPDATE tblBajaEmpleado SET idEmpleado=?, idMotivoBaja=?, fechaBajaEmpleado=?, observacionBajaEmpleado=?, idUsuarioRegistra=?, is_active=? WHERE idBajaEmpleado=?";

            _Delete = "DELETE FROM tblBajaEmpleado WHERE idBajaEmpleado=?";
        }

        public int LiquidacionMetAgregar(ClsBajaEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado),
                new OdbcParameter("p_idMotivoBaja", Entidad.IdMotivoBaja),
                new OdbcParameter("p_fechaBajaEmpleado", Entidad.FechaBajaEmpleado),
                new OdbcParameter("p_observacionBajaEmpleado", (object)Entidad.ObservacionBajaEmpleado ?? DBNull.Value),
                new OdbcParameter("p_idUsuarioRegistra", Entidad.IdUsuarioRegistra),
                new OdbcParameter("p_is_active", Entidad.IsActive)
            };

            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int LiquidacionMetEditar(ClsBajaEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado),
                new OdbcParameter("p_idMotivoBaja", Entidad.IdMotivoBaja),
                new OdbcParameter("p_fechaBajaEmpleado", Entidad.FechaBajaEmpleado),
                new OdbcParameter("p_observacionBajaEmpleado", (object)Entidad.ObservacionBajaEmpleado ?? DBNull.Value),
                new OdbcParameter("p_idUsuarioRegistra", Entidad.IdUsuarioRegistra),
                new OdbcParameter("p_is_active", Entidad.IsActive),
                new OdbcParameter("p_idBajaEmpleado", Entidad.IdBajaEmpleado)
            };

            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int LiquidacionMetRemover(ClsBajaEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("p_idBajaEmpleado", Entidad.IdBajaEmpleado)
            };

            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsBajaEmpleado> LiquidacionMetObtenerTodos()
        {
            var ListaBajas = new List<ClsBajaEmpleado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);

            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Baja = new ClsBajaEmpleado
                {
                    IdBajaEmpleado = Convert.ToInt32(Fila[0]),
                    IdEmpleado = Convert.ToInt32(Fila[1]),
                    IdMotivoBaja = Convert.ToInt32(Fila[2]),
                    FechaBajaEmpleado = Convert.ToDateTime(Fila[3]),
                    ObservacionBajaEmpleado = Fila[4].ToString(),
                    IdUsuarioRegistra = Convert.ToInt32(Fila[5]),
                    IsActive = Convert.ToInt32(Fila[6])
                };
                ListaBajas.Add(Baja);
            }

            TablaDatos.Clear();
            return ListaBajas;
        }

        public DataTable LiquidacionMetObtenerEmpleados()
        {
            return RrhhMetEjecucionConsulta("SELECT idEmpleado, CONCAT(nombresEmpleado, ' ', apellidosEmpleado) AS nombreEmpleado FROM tblempleado", CommandType.Text);
        }

        public DataTable LiquidacionMetObtenerMotivosBaja()
        {
            return RrhhMetEjecucionConsulta("SELECT idMotivoBaja, nombreMotivoBaja FROM tblMotivoBaja", CommandType.Text);
        }
    }
}
