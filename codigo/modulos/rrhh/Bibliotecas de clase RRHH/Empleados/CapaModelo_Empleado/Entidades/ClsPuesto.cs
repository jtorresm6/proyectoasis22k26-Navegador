/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de la tabla tblPuesto para el reporte de la aplicación 9004.
 * ===================================================================
*/

using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsPuesto
    {
        public int IdPuesto { get; set; }
        public int IdTipoPuesto { get; set; }
        public string NombreTipoPuesto { get; set; }
        public string NombrePuesto { get; set; }
        public string DescripcionPuesto { get; set; }
        public bool EstadoPuesto { get; set; }
    }
}
