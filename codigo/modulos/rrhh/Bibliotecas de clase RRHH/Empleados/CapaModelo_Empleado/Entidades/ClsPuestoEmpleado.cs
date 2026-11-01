/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de la tabla tblPuestoEmpleado para el reporte de la aplicación 9006.
 * ===================================================================
*/

using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsPuestoEmpleado
    {
        public int IdPuestoEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public int IdPuesto { get; set; }
        public string NombrePuesto { get; set; }
        public DateTime FechaAsignacionPuestoEmpleado { get; set; }
        public DateTime? FechaFinPuestoEmpleado { get; set; }
        public bool EstadoPuestoEmpleado { get; set; }
    }
}
