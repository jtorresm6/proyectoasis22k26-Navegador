using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Entidades
{
    public class ClsBajaEmpleado
    {
        public int IdBajaEmpleado { get; set; }
        public int IdEmpleado { get; set; }
        public int IdMotivoBaja { get; set; }
        public DateTime FechaBajaEmpleado { get; set; }
        public string ObservacionBajaEmpleado { get; set; }
        public int IdUsuarioRegistra { get; set; }
        public int IsActive { get; set; }

        public string NombresEmpleado { get; set; }
        public string NombreMotivoBaja { get; set; }
    }
}
