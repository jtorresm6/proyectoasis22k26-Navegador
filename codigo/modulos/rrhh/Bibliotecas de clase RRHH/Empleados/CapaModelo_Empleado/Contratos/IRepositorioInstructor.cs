using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using CapaModelo_Empleado.Entidades;

namespace CapaModelo_Empleado.Contratos
{
    public interface IRepositorioInstructor
    {
        IEnumerable<ClsInstructor> RrhhMetObtenerTodos();
    }
}
