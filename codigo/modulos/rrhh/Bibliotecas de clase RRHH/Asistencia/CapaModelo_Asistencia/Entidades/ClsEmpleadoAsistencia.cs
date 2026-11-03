using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Datos del empleado que necesita la ventana de asistencia:
 *  departamento, puesto vigente, jornada con su tolerancia, fecha de
 *  contratación y, si la tiene, la fecha de baja.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsEmpleadoAsistencia
    {
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string DepartamentoEmpleado { get; set; }
        public string PuestoEmpleado { get; set; }
        public string TipoPuesto { get; set; }
        public string NombreJornada { get; set; }
        public TimeSpan HoraInicioJornada { get; set; }
        public TimeSpan HoraFinJornada { get; set; }
        public int MinutosToleranciaJornada { get; set; }
        public DateTime FechaContratacionEmpleado { get; set; }
        public DateTime? FechaBajaEmpleado { get; set; }
        public decimal SalarioEmpleado { get; set; }
    }
}
