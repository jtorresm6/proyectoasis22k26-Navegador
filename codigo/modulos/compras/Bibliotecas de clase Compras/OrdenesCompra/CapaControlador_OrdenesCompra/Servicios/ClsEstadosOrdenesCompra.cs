using System;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Define los estados válidos de una orden de compra.
 * ===================================================================
*/










namespace CapaControlador_OrdenesCompra.Servicios
{
    internal static class ClsEstadosOrdenesCompra
    {
        private static readonly string[] _Estados = { "Pendiente", "Autorizada", "Rechazada", "Anulada", "Recibida" };

        
        
        
        
        internal static string ComprasFuncNormalizarEstado(string Estado)
        {
            string Valor = (Estado ?? string.Empty).Trim();
            string Canonico = Array.Find(_Estados, Actual => string.Equals(Actual, Valor, StringComparison.OrdinalIgnoreCase));
            if (Canonico == null)
                throw new ArgumentException("Estado no valido. Usa Pendiente, Autorizada, Rechazada, Anulada o Recibida.", nameof(Estado));
            return Canonico;
        }

        
        
        
        
        internal static bool ComprasFuncEsTodos(string Estado)
        {
            return string.IsNullOrWhiteSpace(Estado) || string.Equals(Estado.Trim(), "Todos", StringComparison.OrdinalIgnoreCase);
        }

        
        
        
        
        internal static void ComprasProcValidarFiltro(string Estado)
        {
            if (Array.IndexOf(_Estados, Estado) < 0)
                throw new ArgumentException("Estado no válido.", nameof(Estado));
        }
    }
}
