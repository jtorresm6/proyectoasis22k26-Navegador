/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de la tabla tblJornada para el reporte de la aplicación 9005.
 * ===================================================================
*/

using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsJornada
    {
        public int IdJornada { get; set; }
        public string NombreJornada { get; set; }
        public TimeSpan HoraInicioJornada { get; set; }
        public TimeSpan HoraFinJornada { get; set; }
        public int MinutosToleranciaJornada { get; set; }
        public bool EstadoJornada { get; set; }
    }
}
