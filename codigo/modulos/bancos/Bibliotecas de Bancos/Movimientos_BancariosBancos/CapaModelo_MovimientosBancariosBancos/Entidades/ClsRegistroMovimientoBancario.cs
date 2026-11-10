/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_MovimientosBancarios.Entidades
{
    public class ClsRegistroMovimientoBancario
    {
        public long IdConceptoBancario { get; set; }
        public long IdMetodoPago { get; set; }
        public long IdTipoTransaccion { get; set; } 
        public long IdCuentaEmpresa { get; set; }
        public long? IdCuentaTercero { get; set; }
        public long? IdBeneficiario { get; set; }
        public string TipoBeneficiario { get; set; }
        public string Concepto { get; set; }
        public string Naturaleza { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteGTQ { get; set; }
        public decimal TipoCambioAplicado { get; set; }
        public DateTime FechaOperacion { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public string ReferenciaMovimiento { get; set; }
    }
}
