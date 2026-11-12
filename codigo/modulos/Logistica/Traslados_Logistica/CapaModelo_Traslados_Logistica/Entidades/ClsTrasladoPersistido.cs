/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Contiene los datos actuales de un traslado almacenado.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Entidades
{
    public class ClsTrasladoPersistido
    {
        public int CodigoInterno { get; set; }
        public string CodigoTraslado { get; set; }
        public string Estado { get; set; }
        public int BodegaOrigen { get; set; }
        public int BodegaDestino { get; set; }
        public int CodigoTransporte { get; set; }
        public int CodigoConductor { get; set; }
    }
}
