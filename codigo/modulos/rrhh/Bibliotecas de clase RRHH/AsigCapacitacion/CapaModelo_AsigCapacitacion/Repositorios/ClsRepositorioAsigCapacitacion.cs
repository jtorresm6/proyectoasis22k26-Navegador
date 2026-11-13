using CapaModelo_AsigCapacitacion.Contratos;
using CapaModelo_AsigCapacitacion.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * ==================================================================
 * Área : RRHH
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 31/10/2026
 * ==================================================================
 * Propósito :
 *  El formulario del módulo de Asignacion Capacitacion nos 
 *  permite registrar, consultar, modificar y eliminar los 
 *  datos ingresados al sistema, tambien cuenta con Reportes y Ayudas.
 * ===================================================================
*/

namespace CapaModelo_AsigCapacitacion.Repositorios
{
    public class ClsRepositorioAsigCapacitacion : ClsSentencias, IRepositorioAsigCapacitacion
    {
        private string _SelectAll;
        private string _Insert;
        private string _Update;
        private string _Delete;

        public ClsRepositorioAsigCapacitacion()
        {
            _SelectAll = @"
                SELECT a.idAsignacionCapacitacion,
                       a.idEmpleado,
                       CONCAT(e.nombresEmpleado, ' ', e.apellidosEmpleado) AS nombreEmpleado,
                       a.idCapacitacion,
                       c.nombreCapacitacion,
                       i.nombreInstructor,
                       a.fechaAsignacionCapacitacion
                FROM tblasignacioncapacitacion a
                INNER JOIN tblempleado e ON a.idEmpleado = e.idEmpleado
                INNER JOIN tblcapacitacion c ON a.idCapacitacion = c.idCapacitacion
                LEFT JOIN tblinstructor i ON c.idInstructor = i.idInstructor
                ORDER BY a.fechaAsignacionCapacitacion, a.idAsignacionCapacitacion";

            _Insert = "INSERT INTO tblasignacioncapacitacion (idEmpleado, idCapacitacion, fechaAsignacionCapacitacion) VALUES (?, ?, ?)";
            _Update = "UPDATE tblasignacioncapacitacion SET idEmpleado=?, idCapacitacion=?, fechaAsignacionCapacitacion=? WHERE idAsignacionCapacitacion=?";
            _Delete = "DELETE FROM tblasignacioncapacitacion WHERE idAsignacionCapacitacion=?";
        }

        public int RrhhMetAgregar(ClsAsigCapacitacion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));
            Parametros.Add(new OdbcParameter("p_idCapacitacion", Entidad.IdCapacitacion));
            Parametros.Add(new OdbcParameter("p_fecha", Entidad.FechaAsignacionCapacitacion));

            return RrhhMetEjecucionNonQuery(_Insert, Parametros, CommandType.Text);
        }

        public int RrhhMetEditar(ClsAsigCapacitacion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idEmpleado", Entidad.IdEmpleado));
            Parametros.Add(new OdbcParameter("p_idCapacitacion", Entidad.IdCapacitacion));
            Parametros.Add(new OdbcParameter("p_fecha", Entidad.FechaAsignacionCapacitacion));
            Parametros.Add(new OdbcParameter("p_idAsignacion", Entidad.IdAsignacionCapacitacion));

            return RrhhMetEjecucionNonQuery(_Update, Parametros, CommandType.Text);
        }

        public int RrhhMetRemover(ClsAsigCapacitacion Entidad)
        {
            var Parametros = new List<OdbcParameter>();
            Parametros.Add(new OdbcParameter("p_idAsignacion", Entidad.IdAsignacionCapacitacion));

            return RrhhMetEjecucionNonQuery(_Delete, Parametros, CommandType.Text);
        }

        public IEnumerable<ClsAsigCapacitacion> RrhhMetObtenerTodos()
        {
            var ListaAsignaciones = new List<ClsAsigCapacitacion>();
            var TablaDatos = RrhhMetEjecucionConsulta(_SelectAll, CommandType.Text);
            foreach (DataRow Fila in TablaDatos.Rows)
            {
                var Asignacion = new ClsAsigCapacitacion();
                Asignacion.IdAsignacionCapacitacion = Convert.ToInt32(Fila[0]);
                Asignacion.IdEmpleado = Convert.ToInt32(Fila[1]);
                Asignacion.NombreEmpleado = Fila[2].ToString();
                Asignacion.IdCapacitacion = Convert.ToInt32(Fila[3]);
                Asignacion.NombreCapacitacion = Fila[4].ToString();
                Asignacion.NombreInstructor = Fila[5] == DBNull.Value ? "Sin instructor" : Fila[5].ToString();
                Asignacion.FechaAsignacionCapacitacion = Convert.ToDateTime(Fila[6]);
                ListaAsignaciones.Add(Asignacion);
            }
            TablaDatos.Clear();
            TablaDatos = null;
            return ListaAsignaciones;
        }

        public DataTable RrhhMetObtenerEmpleados()
        {
            return RrhhMetEjecucionConsulta(
                @"SELECT idEmpleado,
                         CONCAT(nombresEmpleado, ' ', apellidosEmpleado) AS nombreEmpleado
                  FROM tblempleado
                  WHERE is_active = 1
                  ORDER BY nombresEmpleado, apellidosEmpleado",
                CommandType.Text);
        }

        public DataTable RrhhMetObtenerCapacitaciones()
        {
            return RrhhMetEjecucionConsulta(
                @"SELECT idCapacitacion, nombreCapacitacion
                  FROM tblcapacitacion
                  WHERE estadoCapacitacion = 1
                  ORDER BY nombreCapacitacion",
                CommandType.Text);
        }
    }
}