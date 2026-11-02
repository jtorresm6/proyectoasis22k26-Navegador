using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsPercepcionDeduccionRango
    {
        public int IdPercepcionDeduccionRango { get; set; }
        public int IdPercepcionDeduccion { get; set; }
        public decimal SalarioDesdePercepcionDeduccionRango { get; set; }
        public decimal? SalarioHastaPercepcionDeduccionRango { get; set; }   // NULL = sin limite superior
        public string ModoValorPercepcionDeduccionRango { get; set; }        // ENUM('%','Monto')
        public decimal ValorPercepcionDeduccionRango { get; set; }
        public decimal MontoFijoBasePercepcionDeduccionRango { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
