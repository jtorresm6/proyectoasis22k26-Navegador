using System;
using System.Collections.Generic;
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

namespace CapaModelo_AsigCapacitacion.Entidades
{
    public class ClsAsigCapacitacion
    {
        public int IdAsignacionCapacitacion { get; set; }
        public int IdEmpleado { get; set; }
        public int IdCapacitacion { get; set; }
        public string NombreEmpleado { get; set; }
        public string NombreCapacitacion { get; set; }
        public string NombreInstructor { get; set; }

        public DateTime FechaAsignacionCapacitacion { get; set; }
    }
}
