using CapaModelo_Asistencia.Contratos;
using CapaModelo_Asistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Repositorio de tblHoraExtra para la ventana 9102. Registra,
 *  modifica, elimina y consulta horas extra, y trae las asistencias
 *  que pueden generar hora extra y los conceptos de percepción.
 *  Una hora extra que Nómina ya tomó (idNominaEncabezado con valor)
 *  no se modifica ni se elimina desde aquí.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Repositorios
{
    public class ClsRepositorioHoraExtra : ClsSentencias, IRepositorioHoraExtra
    {
        private string _SelectAll;
        private string _SelectAsistencias;
        private string _SelectConceptos;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioHoraExtra()
        {
            _SelectAll = @"
                SELECT h.idHoraExtra,
                       h.idPercepcionDeduccion,
                       pd.nombrePercepcionDeduccion,
                       h.idEmpleado,
                       e.codigoEmpleado,
                       CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
                       h.idAsistenciaEmpleado,
                       a.fechaAsistenciaEmpleado,
                       TIME_FORMAT(j.horaInicioJornada, '%H:%i:%s') AS horaInicio,
                       TIME_FORMAT(j.horaFinJornada, '%H:%i:%s') AS horaFin,
                       TIME_FORMAT(a.horaSalidaAsistenciaEmpleado, '%H:%i:%s') AS horaSalida,
                       h.duracionHoraExtra,
                       h.valorHoraOrdinariaHoraExtra,
                       h.porcentajeRecargoHoraExtra,
                       h.totalPagoHoraExtra,
                       h.estadoHoraExtra,
                       h.idUsuarioAprueba,
                       u.nombreUsuario,
                       h.fechaAprobacionHoraExtra,
                       h.idNominaEncabezado,
                       h.observacionHoraExtra
                FROM tblhoraextra h
                INNER JOIN tblempleado e ON h.idEmpleado = e.idEmpleado
                INNER JOIN tblasistenciaempleado a ON h.idAsistenciaEmpleado = a.idAsistenciaEmpleado
                INNER JOIN tblpercepciondeduccion pd ON h.idPercepcionDeduccion = pd.idPercepcionDeduccion
                LEFT JOIN tbljornada j ON e.idJornada = j.idJornada
                LEFT JOIN tblusuario u ON h.idUsuarioAprueba = u.idUsuario
                WHERE h.is_active = 1
                ORDER BY a.fechaAsistenciaEmpleado DESC, nombreEmpleado";

            // Presente o Tardanza, con salida después del fin de la jornada y antes de la baja si la hay
            _SelectAsistencias = @"
                SELECT a.idAsistenciaEmpleado,
                       a.idEmpleado,
                       a.fechaAsistenciaEmpleado,
                       TIME_FORMAT(a.horaEntradaAsistenciaEmpleado, '%H:%i:%s') AS horaEntrada,
                       TIME_FORMAT(a.horaSalidaAsistenciaEmpleado, '%H:%i:%s') AS horaSalida,
                       j.nombreJornada,
                       TIME_FORMAT(j.horaInicioJornada, '%H:%i:%s') AS horaInicio,
                       TIME_FORMAT(j.horaFinJornada, '%H:%i:%s') AS horaFin,
                       e.salarioEmpleado,
                       h.idHoraExtra
                FROM tblasistenciaempleado a
                INNER JOIN tblempleado e ON a.idEmpleado = e.idEmpleado
                INNER JOIN tbljornada j ON e.idJornada = j.idJornada
                LEFT JOIN tblhoraextra h ON h.idAsistenciaEmpleado = a.idAsistenciaEmpleado
                LEFT JOIN (SELECT idEmpleado, MIN(fechaBajaEmpleado) AS fechaBaja
                           FROM tblbajaempleado
                           WHERE is_active = 1
                           GROUP BY idEmpleado) b ON b.idEmpleado = a.idEmpleado
                WHERE a.estadoAsistenciaEmpleado IN ('PRESENTE', 'TARDANZA')
                  AND a.horaSalidaAsistenciaEmpleado > j.horaFinJornada
                  AND (b.fechaBaja IS NULL OR a.fechaAsistenciaEmpleado < b.fechaBaja)
                ORDER BY a.fechaAsistenciaEmpleado DESC";

            // efectoPercepcionDeduccion = 1 es percepción, igual que en Nómina
            _SelectConceptos = @"
                SELECT idPercepcionDeduccion, nombrePercepcionDeduccion
                FROM tblpercepciondeduccion
                WHERE is_active = 1 AND efectoPercepcionDeduccion = 1
                ORDER BY nombrePercepcionDeduccion";

            _Insert = @"INSERT INTO tblhoraextra (idPercepcionDeduccion, idEmpleado, idAsistenciaEmpleado, duracionHoraExtra,
                            valorHoraOrdinariaHoraExtra, porcentajeRecargoHoraExtra, totalPagoHoraExtra, estadoHoraExtra,
                            idUsuarioAprueba, fechaAprobacionHoraExtra, observacionHoraExtra)
                        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            _Update = @"UPDATE tblhoraextra SET idPercepcionDeduccion=?, idEmpleado=?, idAsistenciaEmpleado=?, duracionHoraExtra=?,
                            valorHoraOrdinariaHoraExtra=?, porcentajeRecargoHoraExtra=?, totalPagoHoraExtra=?, estadoHoraExtra=?,
                            idUsuarioAprueba=?, fechaAprobacionHoraExtra=?, observacionHoraExtra=?
                        WHERE idHoraExtra=? AND idNominaEncabezado IS NULL";
            _Delete = "DELETE FROM tblhoraextra WHERE idHoraExtra=? AND idNominaEncabezado IS NULL";
        }

        public int RrhhMetAgregar(ClsHoraExtra Entidad)
        {
            return RrhhMetEjecucionInsert(_Insert, RrhhMetParametrosDatos(Entidad));
        }

        // Devuelve las filas afectadas; 0 quiere decir que Nómina ya la tomó
        public int RrhhMetEditar(ClsHoraExtra Entidad)
        {
            var Parametros = RrhhMetParametrosDatos(Entidad);
            Parametros.Add(new OdbcParameter("p_idHoraExtra", Entidad.IdHoraExtra));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsHoraExtra Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idHoraExtra", Entidad.IdHoraExtra));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        private static List<OdbcParameter> RrhhMetParametrosDatos(ClsHoraExtra Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idPercepcion", Entidad.IdPercepcionDeduccion));
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));
            Parametros.Add(new OdbcParameter("p_idAsistencia", Entidad.IdAsistenciaEmpleado));
            Parametros.Add(new OdbcParameter("p_duracion", Entidad.DuracionHoraExtra));
            Parametros.Add(new OdbcParameter("p_valorHora", OdbcType.Decimal) { Value = Entidad.ValorHoraOrdinariaHoraExtra });
            Parametros.Add(new OdbcParameter("p_recargo", OdbcType.Decimal) { Value = Entidad.PorcentajeRecargoHoraExtra });
            Parametros.Add(new OdbcParameter("p_total", OdbcType.Decimal) { Value = Entidad.TotalPagoHoraExtra });
            Parametros.Add(new OdbcParameter("p_estado", OdbcType.VarChar) { Value = Entidad.EstadoHoraExtra });
            Parametros.Add(new OdbcParameter("p_idUsuario", OdbcType.Int)
            {
                Value = Entidad.IdUsuarioAprueba.HasValue ? (object)Entidad.IdUsuarioAprueba.Value : DBNull.Value
            });
            Parametros.Add(new OdbcParameter("p_fechaAprobacion", OdbcType.DateTime)
            {
                Value = Entidad.FechaAprobacionHoraExtra.HasValue ? (object)Entidad.FechaAprobacionHoraExtra.Value : DBNull.Value
            });
            Parametros.Add(new OdbcParameter("p_observacion", OdbcType.VarChar)
            {
                Value = string.IsNullOrWhiteSpace(Entidad.ObservacionHoraExtra) ? (object)DBNull.Value : Entidad.ObservacionHoraExtra
            });
            return Parametros;
        }

        public IEnumerable<ClsHoraExtra> RrhhMetObtenerTodos()
        {
            var ListaHoras = new List<ClsHoraExtra>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Hora = new ClsHoraExtra();
                Hora.IdHoraExtra = Convert.ToInt32(Fila[0]);
                Hora.IdPercepcionDeduccion = Convert.ToInt32(Fila[1]);
                Hora.NombrePercepcionDeduccion = Fila[2].ToString();
                Hora.IdEmpleado = Convert.ToInt32(Fila[3]);
                Hora.CodigoEmpleado = Fila[4].ToString();
                Hora.NombreEmpleado = Fila[5].ToString();
                Hora.IdAsistenciaEmpleado = Convert.ToInt32(Fila[6]);
                Hora.FechaAsistenciaEmpleado = Convert.ToDateTime(Fila[7]);
                Hora.HoraInicioJornada = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[8]);
                Hora.HoraFinJornada = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[9]);
                Hora.HoraSalidaAsistenciaEmpleado = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[10]);
                Hora.DuracionHoraExtra = Convert.ToInt32(Fila[11]);
                Hora.ValorHoraOrdinariaHoraExtra = Convert.ToDecimal(Fila[12]);
                Hora.PorcentajeRecargoHoraExtra = Convert.ToDecimal(Fila[13]);
                Hora.TotalPagoHoraExtra = Convert.ToDecimal(Fila[14]);
                Hora.EstadoHoraExtra = Fila[15].ToString();
                Hora.IdUsuarioAprueba = Fila[16] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila[16]);
                Hora.NombreUsuarioAprueba = Fila[17] == DBNull.Value ? "" : Fila[17].ToString();
                Hora.FechaAprobacionHoraExtra = Fila[18] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Fila[18]);
                Hora.IdNominaEncabezado = Fila[19] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila[19]);
                Hora.ObservacionHoraExtra = Fila[20] == DBNull.Value ? "" : Fila[20].ToString();
                ListaHoras.Add(Hora);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaHoras;
        }

        public ClsHoraExtra RrhhMetBuscarPorId(int IdHoraExtra)
        {
            foreach (ClsHoraExtra Hora in RrhhMetObtenerTodos())
            {
                if (Hora.IdHoraExtra == IdHoraExtra) return Hora;
            }
            return null;
        }

        public IEnumerable<ClsAsistenciaHoraExtra> RrhhMetObtenerAsistenciasConExtra()
        {
            var ListaAsistencias = new List<ClsAsistenciaHoraExtra>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAsistencias, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Asistencia = new ClsAsistenciaHoraExtra();
                Asistencia.IdAsistenciaEmpleado = Convert.ToInt32(Fila[0]);
                Asistencia.IdEmpleado = Convert.ToInt32(Fila[1]);
                Asistencia.FechaAsistenciaEmpleado = Convert.ToDateTime(Fila[2]);
                Asistencia.HoraEntradaAsistenciaEmpleado = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[3]);
                Asistencia.HoraSalidaAsistenciaEmpleado = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[4]) ?? TimeSpan.Zero;
                Asistencia.NombreJornada = Fila[5].ToString();
                Asistencia.HoraInicioJornada = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[6]) ?? TimeSpan.Zero;
                Asistencia.HoraFinJornada = ClsRepositorioAsistencia.RrhhMetLeerHora(Fila[7]) ?? TimeSpan.Zero;
                Asistencia.SalarioEmpleado = Fila[8] == DBNull.Value ? 0m : Convert.ToDecimal(Fila[8]);
                Asistencia.IdHoraExtra = Fila[9] == DBNull.Value ? (int?)null : Convert.ToInt32(Fila[9]);
                ListaAsistencias.Add(Asistencia);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaAsistencias;
        }

        public DataTable RrhhMetObtenerConceptos()
        {
            return RrhhMetEjecucionConsulta(_SelectConceptos, CommandType.Text);
        }
    }
}
