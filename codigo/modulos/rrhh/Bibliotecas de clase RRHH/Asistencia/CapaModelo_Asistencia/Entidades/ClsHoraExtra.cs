using System;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Entidad de un registro de tblHoraExtra con los datos del empleado,
 *  la asistencia de donde sale y quién la aprobó.
 * ===================================================================
*/

namespace CapaModelo_Asistencia.Entidades
{
    public class ClsHoraExtra
    {
        public int IdHoraExtra { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public string NombrePercepcionDeduccion { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public int IdAsistenciaEmpleado { get; set; }
        public DateTime FechaAsistenciaEmpleado { get; set; }
        public TimeSpan? HoraInicioJornada { get; set; }
        public TimeSpan? HoraFinJornada { get; set; }
        public TimeSpan? HoraSalidaAsistenciaEmpleado { get; set; }
        public int DuracionHoraExtra { get; set; }
        public decimal ValorHoraOrdinariaHoraExtra { get; set; }
        public decimal PorcentajeRecargoHoraExtra { get; set; }
        public decimal TotalPagoHoraExtra { get; set; }
        public string EstadoHoraExtra { get; set; }
        public int? IdUsuarioAprueba { get; set; }
        public string NombreUsuarioAprueba { get; set; }
        public DateTime? FechaAprobacionHoraExtra { get; set; }
        public int? IdNominaEncabezado { get; set; }
        public string ObservacionHoraExtra { get; set; }
    }
}
