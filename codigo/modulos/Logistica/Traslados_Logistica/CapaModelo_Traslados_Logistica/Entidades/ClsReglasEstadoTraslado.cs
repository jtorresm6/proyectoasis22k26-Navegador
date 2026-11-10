using System;

namespace CapaModelo_Traslados_Logistica.Entidades
{
    public static class ClsReglasEstadoTraslado
    {
        public const string Solicitado = "SOLICITADO";
        public const string EnTransito = "EN_TRANSITO";
        public const string Recibido = "RECIBIDO";
        public const string Cancelado = "CANCELADO";

        public static bool LogisticaMetPuedeCambiarEstado(string EstadoActual, string EstadoNuevo)
        {
            if (string.Equals(EstadoActual, EstadoNuevo, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(EstadoActual, Solicitado, StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(EstadoNuevo, EnTransito, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(EstadoNuevo, Cancelado, StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(EstadoActual, EnTransito, StringComparison.OrdinalIgnoreCase) &&
                (string.Equals(EstadoNuevo, Recibido, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(EstadoNuevo, Cancelado, StringComparison.OrdinalIgnoreCase));
        }

        public static void LogisticaMetValidarCambioEstado(string EstadoActual, string EstadoNuevo)
        {
            if (!LogisticaMetPuedeCambiarEstado(EstadoActual, EstadoNuevo))
            {
                throw new InvalidOperationException(
                    "La transición de estado de " + EstadoActual + " a " + EstadoNuevo +
                    " no está permitida.");
            }
        }

        public static bool LogisticaMetRequiereTransporte(string Estado)
        {
            return string.Equals(Estado, EnTransito, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(Estado, Recibido, StringComparison.OrdinalIgnoreCase);
        }
    }
}
