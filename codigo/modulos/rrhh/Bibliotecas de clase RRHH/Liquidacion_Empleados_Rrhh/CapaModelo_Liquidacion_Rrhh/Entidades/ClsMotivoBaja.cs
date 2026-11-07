using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Entidades
{
    public class ClsMotivoBaja
    {
        public int IdMotivoBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public bool AplicaIndenmizacionMotivoBaja{ get; set; }
        
        public bool EstadoMotivoBaja{ get; set; }
    }
}
