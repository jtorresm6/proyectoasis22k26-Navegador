using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Entidades
{
    public class ClsConceptoBancario
    {   
        public long idConceptoBancario { get; set; }
        public string CodigoConceptoBancario { get; set; }
        public string NombreConceptoBancario { get; set; }
        public string DescripcionConceptoBancario { get; set; }
        public string NaturalezaConceptoBancario { get; set; }
        public string ClaseOperacionConceptoBancario { get; set; }
        public string TipoTerceroConceptoBancario { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
