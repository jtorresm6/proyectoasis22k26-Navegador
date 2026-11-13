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
 *  Repositorio de tblAsistenciaEmpleado. Registra, modifica, elimina
 *  y consulta asistencias. También trae lo que la ventana 9101 revisa
 *  de otras ventanas de RRHH: empleados con su jornada y baja,
 *  permisos aprobados, vacaciones y capacitaciones asignadas.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Repositorios
{
    public class ClsRepositorioAsistencia : ClsSentencias, IRepositorioAsistencia
    {
        private string _SelectAll;
        private string _SelectEmpleados;
        private string _SelectPermisos;
        private string _SelectCapacitaciones;
        private string _SelectFechaBaja;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioAsistencia()
        {
            _SelectAll = @"
                SELECT a.idAsistenciaEmpleado,
                       a.idEmpleado,
                       e.codigoEmpleado,
                       CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
                       d.nombreDepartamento,
                       p.nombrePuesto,
                       a.fechaAsistenciaEmpleado,
                       TIME_FORMAT(a.horaEntradaAsistenciaEmpleado, '%H:%i:%s') AS horaEntrada,
                       TIME_FORMAT(a.horaSalidaAsistenciaEmpleado, '%H:%i:%s') AS horaSalida,
                       a.estadoAsistenciaEmpleado,
                       a.observacionAsistenciaEmpleado,
                       a.created_at,
                       a.updated_at
                FROM tblasistenciaempleado a
                INNER JOIN tblempleado e ON a.idEmpleado = e.idEmpleado
                LEFT JOIN tbldepartamento d ON e.idDepartamento = d.idDepartamento
                LEFT JOIN tblpuestoempleado pe ON pe.idEmpleado = e.idEmpleado
                      AND pe.fechaFinPuestoEmpleado IS NULL AND pe.is_active = 1
                LEFT JOIN tblpuesto p ON pe.idPuesto = p.idPuesto
                ORDER BY a.fechaAsistenciaEmpleado DESC, nombreEmpleado";

            // Entran también los que ya tienen baja, para poder registrar sus días anteriores a la baja
            _SelectEmpleados = @"
                SELECT e.idEmpleado,
                       e.codigoEmpleado,
                       CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
                       d.nombreDepartamento,
                       p.nombrePuesto,
                       tp.nombreTipoPuesto,
                       j.nombreJornada,
                       TIME_FORMAT(j.horaInicioJornada, '%H:%i:%s') AS horaInicio,
                       TIME_FORMAT(j.horaFinJornada, '%H:%i:%s') AS horaFin,
                       j.minutosToleranciaJornada,
                       e.fechaContratacionEmpleado,
                       b.fechaBaja,
                       e.salarioEmpleado
                FROM tblempleado e
                INNER JOIN tbljornada j ON e.idJornada = j.idJornada
                LEFT JOIN tbldepartamento d ON e.idDepartamento = d.idDepartamento
                LEFT JOIN tblpuestoempleado pe ON pe.idEmpleado = e.idEmpleado
                      AND pe.fechaFinPuestoEmpleado IS NULL AND pe.is_active = 1
                LEFT JOIN tblpuesto p ON pe.idPuesto = p.idPuesto
                LEFT JOIN tbltipopuesto tp ON p.idTipoPuesto = tp.idTipoPuesto
                LEFT JOIN (SELECT idEmpleado, MIN(fechaBajaEmpleado) AS fechaBaja
                           FROM tblbajaempleado
                           WHERE is_active = 1
                           GROUP BY idEmpleado) b ON b.idEmpleado = e.idEmpleado
                WHERE e.is_active = 1 OR b.fechaBaja IS NOT NULL
                ORDER BY e.nombresEmpleado, e.apellidosEmpleado";

            // Las vacaciones se tratan como un permiso por días
            _SelectPermisos = @"
                SELECT pe.idPermisoEmpleado,
                       pe.idEmpleado,
                       tp.nombreTipoPermiso,
                       pe.fechaInicioPermisoEmpleado,
                       pe.fechaFinPermisoEmpleado,
                       pe.motivoPermisoEmpleado,
                       pe.estadoPermisoEmpleado,
                       pe.unidadPermisoEmpleado,
                       TIME_FORMAT(pe.horaInicioPermisoEmpleado, '%H:%i:%s') AS horaInicio,
                       TIME_FORMAT(pe.horaFinPermisoEmpleado, '%H:%i:%s') AS horaFin
                FROM tblpermisoempleado pe
                INNER JOIN tbltipopermiso tp ON pe.idTipoPermiso = tp.idTipoPermiso
                WHERE pe.is_active = 1
                  AND pe.estadoPermisoEmpleado = 'APROBADO'
                UNION ALL
                SELECT v.idVacacionEmpleado,
                       v.idEmpleado,
                       'Vacaciones',
                       v.fechaInicioVacacionEmpleado,
                       v.fechaFinVacacionEmpleado,
                       IFNULL(v.observacionVacacionEmpleado, 'Vacaciones'),
                       v.estadoVacacionEmpleado,
                       'DIAS',
                       NULL,
                       NULL
                FROM tblvacacionempleado v
                WHERE v.is_active = 1
                  AND v.estadoVacacionEmpleado IN ('APROBADO', 'EN CURSO', 'FINALIZADO')";

            _SelectCapacitaciones = @"
                SELECT a.idEmpleado,
                       c.nombreCapacitacion,
                       c.fechaInicioCapacitacion,
                       c.fechaFinCapacitacion
                FROM tblasignacioncapacitacion a
                INNER JOIN tblcapacitacion c ON a.idCapacitacion = c.idCapacitacion
                WHERE c.estadoCapacitacion = 1";

            _SelectFechaBaja = @"SELECT MIN(fechaBajaEmpleado) FROM tblbajaempleado
                                 WHERE idEmpleado = ? AND is_active = 1";

            _Insert = @"INSERT INTO tblasistenciaempleado (idEmpleado, fechaAsistenciaEmpleado, horaEntradaAsistenciaEmpleado,
                            horaSalidaAsistenciaEmpleado, estadoAsistenciaEmpleado, observacionAsistenciaEmpleado)
                        VALUES (?, ?, ?, ?, ?, ?)";
            _Update = @"UPDATE tblasistenciaempleado SET idEmpleado=?, fechaAsistenciaEmpleado=?, horaEntradaAsistenciaEmpleado=?,
                            horaSalidaAsistenciaEmpleado=?, estadoAsistenciaEmpleado=?, observacionAsistenciaEmpleado=?
                        WHERE idAsistenciaEmpleado=?";
            _Delete = "DELETE FROM tblasistenciaempleado WHERE idAsistenciaEmpleado=?";
        }

        public int RrhhMetAgregar(ClsAsistenciaEmpleado Entidad)
        {
            return RrhhMetEjecucionInsert(_Insert, RrhhMetParametrosDatos(Entidad));
        }

        public int RrhhMetEditar(ClsAsistenciaEmpleado Entidad)
        {
            var Parametros = RrhhMetParametrosDatos(Entidad);
            Parametros.Add(new OdbcParameter("p_idAsistencia", Entidad.IdAsistenciaEmpleado));
            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsAsistenciaEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idAsistencia", Entidad.IdAsistenciaEmpleado));
            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        private static List<OdbcParameter> RrhhMetParametrosDatos(ClsAsistenciaEmpleado Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));
            Parametros.Add(new OdbcParameter("p_fecha", OdbcType.Date) { Value = Entidad.FechaAsistenciaEmpleado.Date });
            Parametros.Add(RrhhMetParametroHora("p_entrada", Entidad.HoraEntradaAsistenciaEmpleado));
            Parametros.Add(RrhhMetParametroHora("p_salida", Entidad.HoraSalidaAsistenciaEmpleado));
            Parametros.Add(new OdbcParameter("p_estado", OdbcType.VarChar) { Value = Entidad.EstadoAsistenciaEmpleado });
            Parametros.Add(new OdbcParameter("p_observacion", OdbcType.VarChar)
            {
                Value = string.IsNullOrWhiteSpace(Entidad.ObservacionAsistenciaEmpleado)
                        ? (object)DBNull.Value : Entidad.ObservacionAsistenciaEmpleado
            });
            return Parametros;
        }

        // La hora se manda como texto HH:mm:ss, MySQL la convierte a TIME sin problema
        private static OdbcParameter RrhhMetParametroHora(string Nombre, TimeSpan? Hora)
        {
            return new OdbcParameter(Nombre, OdbcType.VarChar)
            {
                Value = Hora.HasValue ? (object)Hora.Value.ToString(@"hh\:mm\:ss") : DBNull.Value
            };
        }

        public IEnumerable<ClsAsistenciaEmpleado> RrhhMetObtenerTodos()
        {
            var ListaAsistencias = new List<ClsAsistenciaEmpleado>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Asistencia = new ClsAsistenciaEmpleado();
                Asistencia.IdAsistenciaEmpleado = Convert.ToInt32(Fila[0]);
                Asistencia.IdEmpleado = Convert.ToInt32(Fila[1]);
                Asistencia.CodigoEmpleado = Fila[2].ToString();
                Asistencia.NombreEmpleado = Fila[3].ToString();
                Asistencia.DepartamentoEmpleado = Fila[4].ToString();
                Asistencia.PuestoEmpleado = Fila[5].ToString();
                Asistencia.FechaAsistenciaEmpleado = Convert.ToDateTime(Fila[6]);
                Asistencia.HoraEntradaAsistenciaEmpleado = RrhhMetLeerHora(Fila[7]);
                Asistencia.HoraSalidaAsistenciaEmpleado = RrhhMetLeerHora(Fila[8]);
                Asistencia.EstadoAsistenciaEmpleado = Fila[9].ToString();
                Asistencia.ObservacionAsistenciaEmpleado = Fila[10] == DBNull.Value ? "" : Fila[10].ToString();
                Asistencia.CreatedAt = Convert.ToDateTime(Fila[11]);
                Asistencia.UpdatedAt = Convert.ToDateTime(Fila[12]);
                ListaAsistencias.Add(Asistencia);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaAsistencias;
        }

        public IEnumerable<ClsEmpleadoAsistencia> RrhhMetObtenerEmpleados()
        {
            var ListaEmpleados = new List<ClsEmpleadoAsistencia>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectEmpleados, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Empleado = new ClsEmpleadoAsistencia();
                Empleado.IdEmpleado = Convert.ToInt32(Fila[0]);
                Empleado.CodigoEmpleado = Fila[1].ToString();
                Empleado.NombreEmpleado = Fila[2].ToString();
                Empleado.DepartamentoEmpleado = Fila[3].ToString();
                Empleado.PuestoEmpleado = Fila[4].ToString();
                Empleado.TipoPuesto = Fila[5].ToString();
                Empleado.NombreJornada = Fila[6].ToString();
                Empleado.HoraInicioJornada = RrhhMetLeerHora(Fila[7]) ?? TimeSpan.Zero;
                Empleado.HoraFinJornada = RrhhMetLeerHora(Fila[8]) ?? TimeSpan.Zero;
                Empleado.MinutosToleranciaJornada = Fila[9] == DBNull.Value ? 0 : Convert.ToInt32(Fila[9]);
                Empleado.FechaContratacionEmpleado = Convert.ToDateTime(Fila[10]);
                Empleado.FechaBajaEmpleado = Fila[11] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Fila[11]);
                Empleado.SalarioEmpleado = Fila[12] == DBNull.Value ? 0m : Convert.ToDecimal(Fila[12]);
                ListaEmpleados.Add(Empleado);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaEmpleados;
        }

        public IEnumerable<ClsPermisoAsistencia> RrhhMetObtenerPermisosAprobados()
        {
            var ListaPermisos = new List<ClsPermisoAsistencia>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectPermisos, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Permiso = new ClsPermisoAsistencia();
                Permiso.IdPermisoEmpleado = Convert.ToInt32(Fila[0]);
                Permiso.IdEmpleado = Convert.ToInt32(Fila[1]);
                Permiso.NombreTipoPermiso = Fila[2].ToString();
                Permiso.FechaInicioPermisoEmpleado = Convert.ToDateTime(Fila[3]);
                Permiso.FechaFinPermisoEmpleado = Convert.ToDateTime(Fila[4]);
                Permiso.MotivoPermisoEmpleado = Fila[5].ToString();
                Permiso.EstadoPermisoEmpleado = Fila[6].ToString();
                Permiso.UnidadPermisoEmpleado = Fila[7].ToString();
                Permiso.HoraInicioPermisoEmpleado = RrhhMetLeerHora(Fila[8]);
                Permiso.HoraFinPermisoEmpleado = RrhhMetLeerHora(Fila[9]);
                ListaPermisos.Add(Permiso);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaPermisos;
        }

        public IEnumerable<ClsCapacitacionAsistencia> RrhhMetObtenerCapacitaciones()
        {
            var ListaCapacitaciones = new List<ClsCapacitacionAsistencia>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectCapacitaciones, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                ListaCapacitaciones.Add(new ClsCapacitacionAsistencia
                {
                    IdEmpleado = Convert.ToInt32(Fila[0]),
                    NombreCapacitacion = Fila[1].ToString(),
                    FechaInicioCapacitacion = Convert.ToDateTime(Fila[2]),
                    FechaFinCapacitacion = Convert.ToDateTime(Fila[3])
                });
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaCapacitaciones;
        }

        public DateTime? RrhhMetObtenerFechaBaja(int IdEmpleado)
        {
            var Parametros = new List<OdbcParameter> { new OdbcParameter("p_idEmpleado", IdEmpleado) };
            object Valor = RrhhMetEjecucionEscalar(_SelectFechaBaja, Parametros);
            return Valor == null || Valor == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(Valor);
        }

        public static TimeSpan? RrhhMetLeerHora(object Valor)
        {
            if (Valor == null || Valor == DBNull.Value) return null;
            if (Valor is TimeSpan Hora) return Hora;
            if (Valor is DateTime Fecha) return Fecha.TimeOfDay;
            return TimeSpan.TryParse(Valor.ToString(), out TimeSpan Leida) ? Leida : (TimeSpan?)null;
        }
    }
}
