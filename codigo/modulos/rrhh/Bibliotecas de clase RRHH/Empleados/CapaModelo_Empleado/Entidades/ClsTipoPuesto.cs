/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Entidad de la tabla tblTipoPuesto para el reporte de la aplicación 9003.
 * ===================================================================
*/

using System;

namespace CapaModelo_Empleado.Entidades
{
    public class ClsTipoPuesto
    {
        public int IdTipoPuesto { get; set; }
        public string NombreTipoPuesto { get; set; }
        public bool EstadoTipoPuesto { get; set; }
    }
}
