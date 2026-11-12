using CapaModelo_GeneracionPagos.Entidades;

/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 04/11/2026
 * ================================================================
 * Propósito :
 *  Expone a la vista los valores fijos del proceso de pago (estados,
 *  medios y orígenes) sin que la vista tenga que referenciar la capa
 *  modelo. Los valores se toman de ClsValoresPago.
 * ================================================================
 */

namespace CapaControlador_GeneracionPagos
{
    public static class ClsValoresPagoVista
    {
        public const string EstadoPendiente = ClsValoresPago.EstadoPendiente;
        public const string EstadoAutorizada = ClsValoresPago.EstadoAutorizada;
        public const string EstadoRechazada = ClsValoresPago.EstadoRechazada;
        public const string EstadoEmitida = ClsValoresPago.EstadoEmitida;
        public const string EstadoPagada = ClsValoresPago.EstadoPagada;

        public const string MedioCheque = ClsValoresPago.MedioCheque;

        public const string OrigenPlanilla = ClsValoresPago.OrigenPlanilla;

        public const string DecisionAprobada = ClsValoresPago.DecisionAprobada;
    }
}
