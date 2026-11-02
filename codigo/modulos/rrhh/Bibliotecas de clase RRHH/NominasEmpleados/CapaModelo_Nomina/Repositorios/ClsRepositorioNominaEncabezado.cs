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
    public class ClsRepositorioNominaEncabezado : ClsSentencias, IRepositorioNominaEncabezado
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        // ==== AJUSTAR SI TUS COLUMNAS SE LLAMAN DISTINTO ====
        private const string COL_SALARIO_EMPLEADO = "salarioEmpleado";            // tblEmpleado
        private const string COL_DEPARTAMENTO_EMPLEADO = "idDepartamento";        // tblEmpleado
        private const string COL_FECHA_ASISTENCIA = "fechaAsistenciaEmpleado";    // tblAsistenciaEmpleado

        public ClsRepositorioNominaEncabezado()
        {
            _SelectAll = "SELECT idNominaEncabezado, idEmpleado, fechaInicioNominaEncabezado, fechaFinalNominaEncabezado, " +
                         "salarioBaseNominaEncabezado, duracionHoraExtraNominaEncabezado, totalHorasExtraNominaEncabezado, " +
                         "totalPercepcionNominaEncabezado, totalDeduccionNominaEncabezado, totalNetoNominaEncabezado, " +
                         "estadoNminaEncabezado, created_at, updated_at FROM tblNominaEncabezado";
            _Insert = "INSERT INTO tblNominaEncabezado (idEmpleado, fechaInicioNominaEncabezado, fechaFinalNominaEncabezado, " +
                      "salarioBaseNominaEncabezado, duracionHoraExtraNominaEncabezado, totalHorasExtraNominaEncabezado, " +
                      "totalPercepcionNominaEncabezado, totalDeduccionNominaEncabezado, totalNetoNominaEncabezado, " +
                      "estadoNminaEncabezado) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            _Update = "UPDATE tblNominaEncabezado SET estadoNminaEncabezado=? WHERE idNominaEncabezado=?";
            _Delete = "DELETE FROM tblNominaEncabezado WHERE idNominaEncabezado=?";
        }

        // Horas extra que entran a la nomina: aprobadas, activas, sin nomina asignada y dentro del periodo
        private const string FILTRO_HORAS_EXTRA =
            "he.idEmpleado = ? AND he.estadoHoraExtra = 'APROBADA' AND he.is_active = 1 " +
            "AND he.idNominaEncabezado IS NULL AND ae." + COL_FECHA_ASISTENCIA + " BETWEEN ? AND ?";

        // ---------- Contrato generico ----------
        public int RrhhMetAgregar(ClsNominaEncabezado Entidad)
        {
            // Encabezado + asignacion de horas extra en una sola transaccion
            using (var Conexion = RrhhMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        var Parametros = new List<OdbcParameter>();
                        Parametros.Add(RrhhMetParametroEntero("P_idEmpleado", Entidad.IdEmpleado));
                        Parametros.Add(RrhhMetParametroFecha("P_fechaInicio", Entidad.FechaInicioNominaEncabezado));
                        Parametros.Add(RrhhMetParametroFecha("P_fechaFinal", Entidad.FechaFinalNominaEncabezado));
                        Parametros.Add(RrhhMetParametroDecimal("P_salarioBase", Entidad.SalarioBaseNominaEncabezado));
                        Parametros.Add(RrhhMetParametroEntero("P_duracionHoraExtra", Entidad.DuracionHoraExtraNominaEncabezado));
                        Parametros.Add(RrhhMetParametroDecimal("P_totalHorasExtra", Entidad.TotalHorasExtraNominaEncabezado));
                        Parametros.Add(RrhhMetParametroDecimal("P_totalPercepcion", Entidad.TotalPercepcionNominaEncabezado));
                        Parametros.Add(RrhhMetParametroDecimal("P_totalDeduccion", Entidad.TotalDeduccionNominaEncabezado));
                        Parametros.Add(RrhhMetParametroDecimal("P_totalNeto", Entidad.TotalNetoNominaEncabezado));
                        Parametros.Add(RrhhMetParametroTexto("P_estado", Entidad.EstadoNminaEncabezado));

                        int Filas = RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text, Conexion, Transaccion);

                        object IdNuevo = RrhhMetEjecucionEscalar("SELECT LAST_INSERT_ID()", new List<OdbcParameter>(),
                            CommandType.Text, Conexion, Transaccion);
                        Entidad.IdNominaEncabezado = Convert.ToInt32(IdNuevo);

                        // Solo se asignan a la nomina; NO se marcan como pagadas
                        string SqlAsignar = "UPDATE tblHoraExtra he INNER JOIN tblAsistenciaEmpleado ae " +
                                            "ON ae.idAsistenciaEmpleado = he.idAsistenciaEmpleado " +
                                            "SET he.idNominaEncabezado = ? WHERE " + FILTRO_HORAS_EXTRA;
                        var ParametrosHoras = new List<OdbcParameter>();
                        ParametrosHoras.Add(RrhhMetParametroEntero("P_idNominaEncabezado", Entidad.IdNominaEncabezado));
                        ParametrosHoras.Add(RrhhMetParametroEntero("P_idEmpleado", Entidad.IdEmpleado));
                        ParametrosHoras.Add(RrhhMetParametroFecha("P_fechaInicio", Entidad.FechaInicioNominaEncabezado));
                        ParametrosHoras.Add(RrhhMetParametroFecha("P_fechaFinal", Entidad.FechaFinalNominaEncabezado));
                        RrhhMetEjecucionNonQuery(SqlAsignar, ParametrosHoras, CommandType.Text, Conexion, Transaccion);

                        Transaccion.Commit();
                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int RrhhMetEditar(ClsNominaEncabezado Entidad)
        {
            using (var Conexion = RrhhMetObtenerConexion())
            {
                Conexion.Open();
                using (var Transaccion = Conexion.BeginTransaction())
                {
                    try
                    {
                        var Parametros = new List<OdbcParameter>();
                        Parametros.Add(RrhhMetParametroTexto("P_estado", Entidad.EstadoNminaEncabezado));
                        Parametros.Add(RrhhMetParametroEntero("P_idNominaEncabezado", Entidad.IdNominaEncabezado));
                        int Filas = RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text, Conexion, Transaccion);

                        // Una nomina rechazada libera sus horas extra para que puedan entrar en otra nomina
                        if (Entidad.EstadoNminaEncabezado == "Rechazada")
                        {
                            var ParametrosHoras = new List<OdbcParameter>();
                            ParametrosHoras.Add(RrhhMetParametroEntero("P_idNominaEncabezado", Entidad.IdNominaEncabezado));
                            RrhhMetEjecucionNonQuery(
                                "UPDATE tblHoraExtra SET idNominaEncabezado = NULL " +
                                "WHERE idNominaEncabezado = ? AND estadoHoraExtra = 'APROBADA'",
                                ParametrosHoras, CommandType.Text, Conexion, Transaccion);
                        }

                        Transaccion.Commit();
                        return Filas;
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public int RrhhMetRemover(ClsNominaEncabezado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idNominaEncabezado", Entidad.IdNominaEncabezado));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsNominaEncabezado> RrhhMetObtenerTodos()
        {
            var Lista = new List<ClsNominaEncabezado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Item = new ClsNominaEncabezado();
                Item.IdNominaEncabezado = Convert.ToInt32(Fila["idNominaEncabezado"]);
                Item.IdEmpleado = Convert.ToInt32(Fila["idEmpleado"]);
                Item.FechaInicioNominaEncabezado = Convert.ToDateTime(Fila["fechaInicioNominaEncabezado"]);
                Item.FechaFinalNominaEncabezado = Convert.ToDateTime(Fila["fechaFinalNominaEncabezado"]);
                Item.SalarioBaseNominaEncabezado = Convert.ToDecimal(Fila["salarioBaseNominaEncabezado"]);
                Item.DuracionHoraExtraNominaEncabezado = Convert.ToInt32(Fila["duracionHoraExtraNominaEncabezado"]);
                Item.TotalHorasExtraNominaEncabezado = Convert.ToDecimal(Fila["totalHorasExtraNominaEncabezado"]);
                Item.TotalPercepcionNominaEncabezado = Convert.ToDecimal(Fila["totalPercepcionNominaEncabezado"]);
                Item.TotalDeduccionNominaEncabezado = Convert.ToDecimal(Fila["totalDeduccionNominaEncabezado"]);
                Item.TotalNetoNominaEncabezado = Convert.ToDecimal(Fila["totalNetoNominaEncabezado"]);
                Item.EstadoNminaEncabezado = Fila["estadoNminaEncabezado"].ToString();
                Item.CreatedAt = Convert.ToDateTime(Fila["created_at"]);
                Item.UpdatedAt = Convert.ToDateTime(Fila["updated_at"]);
                Lista.Add(Item);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return Lista;
        }

        // ---------- Metodos propios de la tabla ----------
        public bool RrhhMetEsEmpleadoActivo(int IdEmpleado)
        {
            string Sql = "SELECT COUNT(*) FROM tblEmpleado WHERE idEmpleado = ? AND is_active = 1";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado", IdEmpleado));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]) > 0;
        }

        public decimal? RrhhMetObtenerSalarioEmpleado(int IdEmpleado)
        {
            string Sql = "SELECT " + COL_SALARIO_EMPLEADO + " FROM tblEmpleado WHERE idEmpleado = ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado", IdEmpleado));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            if (Tabla.Rows.Count == 0 || Tabla.Rows[0][0] == DBNull.Value)
                return null;
            return Convert.ToDecimal(Tabla.Rows[0][0]);
        }

        public void RrhhMetObtenerResumenHorasExtra(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal,
            out int Duracion, out decimal TotalPago)
        {
            string Sql = "SELECT COALESCE(SUM(he.duracionHoraExtra), 0) AS Duracion, " +
                         "COALESCE(SUM(he.totalPagoHoraExtra), 0) AS TotalPago " +
                         "FROM tblHoraExtra he INNER JOIN tblAsistenciaEmpleado ae " +
                         "ON ae.idAsistenciaEmpleado = he.idAsistenciaEmpleado WHERE " + FILTRO_HORAS_EXTRA;
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado", IdEmpleado));
            Parametros.Add(RrhhMetParametroFecha("P_fechaInicio", FechaInicio));
            Parametros.Add(RrhhMetParametroFecha("P_fechaFinal", FechaFinal));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            Duracion = Convert.ToInt32(Tabla.Rows[0]["Duracion"]);
            TotalPago = Convert.ToDecimal(Tabla.Rows[0]["TotalPago"]);
        }

        public List<ClsPercepcionDeduccion> RrhhMetObtenerConceptosAplicables(int IdEmpleado)
        {
            // 'Todos' siempre aplica; 'Algunos' solo si el empleado (o su departamento) esta asignado
            string Sql = "SELECT pd.idPercepcionDeduccion, pd.nombrePercepcionDeduccion, pd.periodoPercepcionDeduccion, " +
                         "pd.efectoPercepcionDeduccion, pd.tipoPercepcionDeduccion, pd.valorTipoPercepcionDeduccion, " +
                         "pd.aplicacionPercepcionDeduccion, pd.is_active " +
                         "FROM tblPercepcionDeduccion pd WHERE pd.is_active = 1 AND (" +
                         "pd.aplicacionPercepcionDeduccion = 'Todos' OR (pd.aplicacionPercepcionDeduccion = 'Algunos' " +
                         "AND EXISTS (SELECT 1 FROM tblPercepcionDeduccionEmpleado pde " +
                         "WHERE pde.idPercepcionDeduccion = pd.idPercepcionDeduccion AND pde.is_active = 1 " +
                         "AND (pde.idEmpleado = ? OR pde.idDepartamento = " +
                         "(SELECT e." + COL_DEPARTAMENTO_EMPLEADO + " FROM tblEmpleado e WHERE e.idEmpleado = ?)))))";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado1", IdEmpleado));
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado2", IdEmpleado));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);

            var Lista = new List<ClsPercepcionDeduccion>();
            foreach (DataRow Fila in Tabla.Rows)
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
                Lista.Add(Item);
            }
            return Lista;
        }

        public bool RrhhMetObtenerRangoSalario(int IdPercepcionDeduccion, decimal Salario,
            out string Modo, out decimal Valor, out decimal MontoFijoBase)
        {
            string Sql = "SELECT modoValorPercepcionDeduccionRango, valorPercepcionDeduccionRango, " +
                         "montoFijoBasePercepcionDeduccionRango FROM tblPercepcionDeduccionRango " +
                         "WHERE idPercepcionDeduccion = ? AND is_active = 1 " +
                         "AND salarioDesdePercepcionDeduccionRango <= CAST(? AS DECIMAL(12,4)) " +
                         "AND (salarioHastaPercepcionDeduccionRango IS NULL " +
                         "OR salarioHastaPercepcionDeduccionRango >= CAST(? AS DECIMAL(12,4))) " +
                         "ORDER BY salarioDesdePercepcionDeduccionRango DESC LIMIT 1";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idPercepcionDeduccion", IdPercepcionDeduccion));
            Parametros.Add(RrhhMetParametroDecimal("P_salario1", Salario));
            Parametros.Add(RrhhMetParametroDecimal("P_salario2", Salario));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);

            if (Tabla.Rows.Count == 0)
            {
                Modo = null;
                Valor = 0;
                MontoFijoBase = 0;
                return false;
            }
            Modo = Tabla.Rows[0]["modoValorPercepcionDeduccionRango"].ToString();
            Valor = Convert.ToDecimal(Tabla.Rows[0]["valorPercepcionDeduccionRango"]);
            MontoFijoBase = Convert.ToDecimal(Tabla.Rows[0]["montoFijoBasePercepcionDeduccionRango"]);
            return true;
        }

        public int RrhhMetContarTraslapes(int IdEmpleado, DateTime FechaInicio, DateTime FechaFinal, int IdExcluir)
        {
            // Nominas rechazadas no bloquean el periodo
            string Sql = "SELECT COUNT(*) FROM tblNominaEncabezado WHERE idEmpleado = ? " +
                         "AND estadoNminaEncabezado <> 'Rechazada' AND idNominaEncabezado <> ? " +
                         "AND fechaInicioNominaEncabezado <= ? AND fechaFinalNominaEncabezado >= ?";
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(RrhhMetParametroEntero("P_idEmpleado", IdEmpleado));
            Parametros.Add(RrhhMetParametroEntero("P_idExcluir", IdExcluir));
            Parametros.Add(RrhhMetParametroFecha("P_fechaFinal", FechaFinal));
            Parametros.Add(RrhhMetParametroFecha("P_fechaInicio", FechaInicio));
            var Tabla = RrhhMetEjecucionConsulta(Sql, CommandType.Text, Parametros);
            return Convert.ToInt32(Tabla.Rows[0][0]);
        }

        // ---------- Parametros con tipo explicito (el driver ODBC no los infiere bien) ----------
        private OdbcParameter RrhhMetParametroEntero(string Nombre, int Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.Int) { Value = Valor };
        }

        private OdbcParameter RrhhMetParametroTexto(string Nombre, string Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar) { Value = Valor };
        }

        private OdbcParameter RrhhMetParametroDecimal(string Nombre, decimal Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar) { Value = Valor.ToString("0.0000", CultureInfo.InvariantCulture) };
        }

        private OdbcParameter RrhhMetParametroFecha(string Nombre, DateTime Valor)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar) { Value = Valor.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) };
        }

    }
}
