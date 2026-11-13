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
        public string PeriodoPercepcionDeduccion { get; set; }      // ENUM('MENSUAL','ANUAL')
        public bool EfectoPercepcionDeduccion { get; set; }
        public string TipoPercepcionDeduccion { get; set; }         // ENUM('Tabla','%','Monto')
        public decimal ValorTipoPercepcionDeduccion { get; set; }
        public string AplicacionPercepcionDeduccion { get; set; }   // ENUM('Todos','Algunos')
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
