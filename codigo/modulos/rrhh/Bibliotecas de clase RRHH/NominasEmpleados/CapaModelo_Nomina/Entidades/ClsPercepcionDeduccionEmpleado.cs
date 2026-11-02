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
