using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades_Navegador
{
    // Guarda toda la info de una columna en un solo objeto, la usan las 3 capas
    public class columnaInfo
    {
        public string Nombre { get; set; }
        public string TipoDato { get; set; }
        public string TipoNet { get; set; }
        public string TipoColumnaTexto { get; set; }
        public long Longitud { get; set; }
        public long TamanoColumna { get; set; }
        public bool Nullable { get; set; }
        public bool EsPK { get; set; }
        public bool EsFK { get; set; }
        public bool EsAutoincremento { get; set; }
        public string TablaFK { get; set; }
        public string ColumnaFK { get; set; }
    }
}