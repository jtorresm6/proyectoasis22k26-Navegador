using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Nomina.Entidades
{
    public class ClsPercepcionDeduccion
    {
        public int IdPercepcionDeduccion { get; set; }
        public string NombrePercepcionDeduccion { get; set; }
        public string PeriodoPercepcionDeduccion { get; set; }     
        public bool EfectoPercepcionDeduccion { get; set; }
        public string TipoPercepcionDeduccion { get; set; }         
        public decimal ValorTipoPercepcionDeduccion { get; set; }
        public string AplicacionPercepcionDeduccion { get; set; }  
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
