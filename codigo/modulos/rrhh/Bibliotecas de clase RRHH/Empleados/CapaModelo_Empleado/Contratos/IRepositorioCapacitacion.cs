using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Contratos
{
    public interface IRepositorioCapacitacion
    {
        IEnumerable<ClsCapacitacion> RrhhMetObtenerTodos();
    }
}