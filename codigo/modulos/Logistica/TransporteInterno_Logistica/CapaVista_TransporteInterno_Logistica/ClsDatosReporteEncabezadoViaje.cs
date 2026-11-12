using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaVista_TransporteInterno_Logistica
{
    public class ClsDatosReporteEncabezadoViaje
    {
         public string CodigoViaje { get; set; }

        public DateTime FechaDocumento { get; set; }

        public DateTime FechaSalida { get; set; }

        public DateTime? FechaLlegada { get; set; }

        public string Transporte { get; set; }

        public string Conductor { get; set; }

        public string EstadoViaje { get; set; }

        public decimal PesoTotal { get; set; }
    }
}
