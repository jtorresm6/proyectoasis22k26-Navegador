using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 03/11/2026
 * ================================================================
 * Propósito :
 *  Objeto de valor con lo que la pestaña Registrar necesita mostrar:
 *  el movimiento bancario (ya registrado o por registrar) y las
 *  líneas de la póliza bancaria.
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public class ClsDatosRegistro
    {
        public bool Registrado { get; set; }
        public string Referencia { get; set; }
        public decimal Importe { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal ImporteGTQ { get; set; }
        public string EstadoPoliza { get; set; }
        public string Advertencia { get; set; }
        public DataTable Poliza { get; set; }
    }
}
