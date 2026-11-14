using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ==================================================================
 * Área : Bancos
 * Autor : Andy Alfonso Garcia Lopez
 * Carné : 9959-23-1494
 * Fecha : 28/10/2026
 * ==================================================================
 * Propósito :
 *  Enumeración que define el estado de una entidad (Added, Deleted, Modified)
 *  para indicar a la capa modelo si se trata de una inserción, eliminación o
 *  modificación.
 * ===================================================================
*/

namespace CapaControlador_AutorizacionOrdenes
{
    public enum EstadoEntidad
    {
        Added,
        Modified,
        Deleted
    }
}
