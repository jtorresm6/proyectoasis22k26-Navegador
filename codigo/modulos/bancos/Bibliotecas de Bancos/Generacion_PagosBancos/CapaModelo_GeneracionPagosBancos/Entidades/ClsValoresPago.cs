/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 04/11/2026
 * ================================================================
 * Propósito :
 *  Valores fijos del proceso Generación de Pagos (estados de la
 *  orden, medios de pago, orígenes de la obligación y decisiones de
 *  aprobación) para no repetirlos como texto en cada capa.
 * ================================================================
 */

namespace CapaModelo_GeneracionPagos.Entidades
{
    public static class ClsValoresPago
    {
        public const string EstadoPendiente = "PENDIENTE";
        public const string EstadoAutorizada = "AUTORIZADA";
        public const string EstadoRechazada = "RECHAZADA";
        public const string EstadoEmitida = "EMITIDA";
        public const string EstadoPagada = "PAGADA";

        public const string MedioCheque = "CHEQUE";
        public const string MedioTransferencia = "TRANSFERENCIA";

        public const string OrigenCompra = "C";
        public const string OrigenPlanilla = "R";
        public const string OrigenLogistica = "L";

        public const string DecisionAprobada = "APROBADA";
        public const string DecisionRechazada = "RECHAZADA";
    }
}
