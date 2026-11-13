using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Asistencia que puede generar hora extra: el empleado llegó
 *  (Presente o Tardanza) y salió después del fin de su jornada.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsAsistenciaHoraExtra
    {
        public int IdAsistenciaEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaAsistenciaEmpleado { get; set; }
        public TimeSpan? HoraEntradaAsistenciaEmpleado { get; set; }
        public TimeSpan HoraSalidaAsistenciaEmpleado { get; set; }
        public string NombreJornada { get; set; }
        public TimeSpan HoraInicioJornada { get; set; }
        public TimeSpan HoraFinJornada { get; set; }
        public decimal SalarioEmpleado { get; set; }
        public int? IdHoraExtra { get; set; }
    }
}
