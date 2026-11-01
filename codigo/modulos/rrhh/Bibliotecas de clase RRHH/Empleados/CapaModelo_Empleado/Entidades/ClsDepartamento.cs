/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de la tabla tblDepartamento para el reporte de la aplicación 9002.
 * ===================================================================
*/

using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsDepartamento
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public bool EstadoDepartamento { get; set; }
    }
}
