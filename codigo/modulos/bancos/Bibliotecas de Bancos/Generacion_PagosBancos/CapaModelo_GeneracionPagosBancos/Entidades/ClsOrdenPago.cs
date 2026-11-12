using System;
using System.Collections.Generic;
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
 *  La entidad ClsOrdenPago representa los datos con los que se
 *  prepara una orden de pago (tabla tblOrdenPago). Exactamente uno
 *  de los tres orígenes (compra recibida, detalle de planilla o
 *  solicitud de pago de transporte) debe venir informado.
 * ================================================================
 */

namespace CapaModelo_GeneracionPagos.Entidades
{
    public class ClsOrdenPago
    {
        public long IdOrdenPago { get; set; }
        public string ReferenciaOrdenPago { get; set; }
        public string BeneficiarioOrdenPago { get; set; }
        public decimal ImporteOrdenPago { get; set; }
        public string MedioOrdenPago { get; set; }
        public string EstadoOrdenPago { get; set; }
        public DateTime FechaProgramadaOrdenPago { get; set; }
        public string IdentificadorSolicitanteOrdenPago { get; set; }
        public long IdCuentaBancaria { get; set; }
        public long IdMoneda { get; set; }
        public long? IdOrdenCompraRecibida { get; set; }
        public long? IdDetallePlanillaRecibida { get; set; }
        public long? IdSolicitudPagoTransporte { get; set; }
        public int? IdUsuario { get; set; }
        public string Ip { get; set; }
    }
}
