using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Permiso aprobado o vacaciones de un empleado. Los que son por días
 *  dejan la asistencia en Permiso; los que son por horas solo quitan
 *  la tardanza o la salida antes de tiempo que cubren.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsPermisoAsistencia
    {
        public int IdPermisoEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public string NombreTipoPermiso { get; set; }
        public DateTime FechaInicioPermisoEmpleado { get; set; }
        public DateTime FechaFinPermisoEmpleado { get; set; }
        public string MotivoPermisoEmpleado { get; set; }
        public string EstadoPermisoEmpleado { get; set; }
        public string UnidadPermisoEmpleado { get; set; }
        public TimeSpan? HoraInicioPermisoEmpleado { get; set; }
        public TimeSpan? HoraFinPermisoEmpleado { get; set; }
    }
}
