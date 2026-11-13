using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsNominaEncabezado
    {
        public int IdNominaEncabezado { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaInicioNominaEncabezado { get; set; }
        public DateTime FechaFinalNominaEncabezado { get; set; }
        public decimal SalarioBaseNominaEncabezado { get; set; }
        public int DuracionHoraExtraNominaEncabezado { get; set; }
        public decimal TotalHorasExtraNominaEncabezado { get; set; }
        public decimal TotalPercepcionNominaEncabezado { get; set; }
        public decimal TotalDeduccionNominaEncabezado { get; set; }
        public decimal TotalNetoNominaEncabezado { get; set; }
        public string EstadoNminaEncabezado { get; set; }   // ENUM('Aprobada','Rechazada','Pagada')
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
