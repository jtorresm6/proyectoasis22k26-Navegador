/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Define los estados utilizados por los traslados entre bodegas.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Entidades
{
    public static class ClsReglasEstadoTraslado
    {
        public const string Solicitado = "SOLICITADO";
        public const string EnTransito = "EN_TRANSITO";
        public const string Recibido = "RECIBIDO";
        public const string Cancelado = "CANCELADO";
    }
}
