using CapaModelo_AsigCapacitacion.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace CapaModelo_AsigCapacitacion.Contratos
{
    public interface IRepositorioAsigCapacitacion
    {
        int RrhhMetAgregar(ClsAsigCapacitacion Entidad);
        int RrhhMetEditar(ClsAsigCapacitacion Entidad);
        int RrhhMetRemover(ClsAsigCapacitacion Entidad);
        IEnumerable<ClsAsigCapacitacion> RrhhMetObtenerTodos();
        DataTable RrhhMetObtenerEmpleados();
        DataTable RrhhMetObtenerCapacitaciones();
    }
}
