using System.Collections.Generic;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Agrupa los datos solicitados para registrar un traslado.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Entidades
{
    public class ClsTrasladoSolicitud
    {
        public ClsTrasladoSolicitud()
        {
            Detalles = new List<ClsProductoTrasladoDetalle>();
        }

        public string CodigoTraslado { get; set; }
        public string Estado { get; set; }
        public int BodegaOrigen { get; set; }
        public int BodegaDestino { get; set; }
        public int CodigoTipoTransporte { get; set; }
        public int CodigoTransporte { get; set; }
        public int CodigoConductor { get; set; }
        public string Observaciones { get; set; }
        public IList<ClsProductoTrasladoDetalle> Detalles { get; private set; }
    }
}
