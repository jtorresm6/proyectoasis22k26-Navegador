using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Capacitación asignada a un empleado con sus fechas. Los días que
 *  cubre cuentan como día trabajado en la asistencia.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsCapacitacionAsistencia
    {
        public int IdEmpleado { get; set; }
        public string NombreCapacitacion { get; set; }
        public DateTime FechaInicioCapacitacion { get; set; }
        public DateTime FechaFinCapacitacion { get; set; }
    }
}
