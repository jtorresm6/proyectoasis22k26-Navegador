using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsNominaDetalle
    {
        public int IdNominaDetalle { get; set; }
        public int IdNominaEncabezado { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public decimal MontoNominaDetalle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
