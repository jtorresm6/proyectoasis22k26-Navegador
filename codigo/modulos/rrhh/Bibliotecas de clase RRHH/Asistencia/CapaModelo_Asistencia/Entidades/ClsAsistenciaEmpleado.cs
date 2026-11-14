using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Entidad de un registro de tblAsistenciaEmpleado junto con los
 *  datos del empleado que se muestran en la lista de asistencias.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsAsistenciaEmpleado
    {
        public int IdAsistenciaEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string DepartamentoEmpleado { get; set; }
        public string PuestoEmpleado { get; set; }
        public DateTime FechaAsistenciaEmpleado { get; set; }
        public TimeSpan? HoraEntradaAsistenciaEmpleado { get; set; }
        public TimeSpan? HoraSalidaAsistenciaEmpleado { get; set; }
        public string EstadoAsistenciaEmpleado { get; set; }
        public string ObservacionAsistenciaEmpleado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
