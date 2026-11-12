/*
 * ==================================================================
 * Área : Rrhh
 * Autor : Cristian David Sipac Ispache
 * Carné : 9959-23-1567
 * Fecha : 06/11/2026
 * ==================================================================
 * Propósito :
 *  Representa la asignación de una percepción o deducción a un
 *  empleado o departamento. Almacena los identificadores del
 *  concepto y su destino, el estado activo de la asignación
 *  y las fechas de creación y actualización.
 * ==================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsPercepcionDeduccionEmpleado
    {
        public int IdPercepcionDeduccionEmpleado { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public int? IdEmpleado { get; set; }        // NULL si se asigna solo a departamento
        public int? IdDepartamento { get; set; }    // NULL si se asigna solo a empleado
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
