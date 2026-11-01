using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Entidades
{
    public class ClsTipoTransaccion
    {
        public int PK_TipoTransaccion { get; set; }
        public string CodigoTipoTransaccion { get; set; }
        public string NombreTipoTransaccion { get; set; }
        public string NaturalezaTipoTransaccion { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}