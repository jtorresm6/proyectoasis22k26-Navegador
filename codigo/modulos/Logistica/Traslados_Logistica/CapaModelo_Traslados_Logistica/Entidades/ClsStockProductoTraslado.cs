/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Representa las existencias de un producto en una bodega.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Entidades
{
    public class ClsStockProductoTraslado
    {
        public int IdStock { get; set; }
        public int CodigoProducto { get; set; }
        public int CodigoSucursal { get; set; }
        public int StockActual { get; set; }
        public int Apartados { get; set; }
        public bool EstadoVenta { get; set; }
        public bool EstadoCompra { get; set; }

    }
}
