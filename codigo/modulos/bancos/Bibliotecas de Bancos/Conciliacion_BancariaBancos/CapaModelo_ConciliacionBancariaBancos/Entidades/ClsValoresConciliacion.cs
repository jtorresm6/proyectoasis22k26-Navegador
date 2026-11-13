/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  Define los estados, resultados y métodos permitidos en una
 *  conciliación bancaria.
 * ================================================================
 */
namespace CapaModelo_ConciliacionBancaria.Entidades
{
    public static class ClsValoresConciliacion
    {
        public const string EstadoAbierta = "ABIERTA";
        public const string EstadoConciliada = "CONCILIADA";
        public const string EstadoConDiferencias = "CON_DIFERENCIAS";
        public const string EstadoCerrada = "CERRADA";
        public const string ResultadoCoincide = "COINCIDE";
        public const string ResultadoPendiente = "PENDIENTE";
        public const string ResultadoDiferencia = "DIFERENCIA";
        public const string MetodoAutomatica = "AUTOMATICA";
        public const string MetodoManual = "MANUAL";
        public const string MetodoMixta = "MIXTA";
    }
}
