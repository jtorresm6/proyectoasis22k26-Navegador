using CapaModelo_Nomina.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Contratos
{
    public interface IRepositorioPercepcionDeduccionEmpleado : IRepositorioGenerico<ClsPercepcionDeduccionEmpleado>
    {
        int RrhhMetContarPorEmpleado(int IdPercepcionDeduccion, int IdEmpleado, int IdExcluir);

        // Cuenta asignaciones repetidas de la misma percepcion/deduccion al mismo departamento
        int RrhhMetContarPorDepartamento(int IdPercepcionDeduccion, int IdDepartamento, int IdExcluir);
    }
}
