using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaControlador_Nomina.ObjetosdeValor
{
    public class ClsConceptoCalculado
    {

        public const string NombreConceptoComision = "Comisión por Ventas";

        public int IdPercepcionDeduccion { get; set; }
        public string NombreConcepto { get; set; }
        public bool EsPercepcion { get; set; }
        public decimal Monto { get; set; }
    }
}
