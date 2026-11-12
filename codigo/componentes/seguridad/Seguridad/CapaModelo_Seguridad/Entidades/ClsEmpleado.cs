using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * ==================================================================
 * Área : Seguridad
 * Autor : Carlos David Calderón Ramirez
 * Carné : 9959-23-848
 * Fecha : 22/09/2026
 * ==================================================================
 * Propósito :
 * Aqui en esta clase se encuentran los get y set necesarios y 
 * utilizados en la vista del formulario
 * ===================================================================
*/

namespace CapaModelo_Seguridad.Entidades
{
    public class ClsEmpleado
    {
        public int IdEmpleado { get; set; }
        public string CodigoEmpleado { get; set; }
        public string DpiEmpleado { get; set; }
        public string NitEmpleado { get; set; }
        public string NombresEmpleado { get; set; }
        public string ApellidosEmpleado { get; set; }
        public string PuestoEmpleado { get; set; }
        public string GeneroEmpleado { get; set; }
        public DateTime FechaNacimientoEmpleado { get; set; }
        public DateTime FechaContratacionEmpleado { get; set; }
        public string TelefonoEmpleado { get; set; }
        public string CorreoEmpleado { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}