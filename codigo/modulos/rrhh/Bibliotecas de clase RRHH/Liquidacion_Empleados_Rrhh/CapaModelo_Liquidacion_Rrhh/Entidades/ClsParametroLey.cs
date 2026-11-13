using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Liquidacion_Rrhh.Entidades
{
    public class ClsParametroLey
    {
        public int IdParametroLey { get; set; }
        public int IdEmpresa { get; set; }
        public string CodigoParametroLey { get; set; }
        public string DescripcionParametroLey { get; set; }
        public decimal ValorParametroLey { get; set; }
        public DateTime VigenteDesdeParametroLey { get; set; }
        public DateTime VigenteHastaParametroLey { get; set; }
        public bool IsActive { get; set; }

        // Propiedad opcional para mostrar el nombre de la empresa en reportes si se hace JOIN
        public string NombreEmpresa { get; set; }
    }
}