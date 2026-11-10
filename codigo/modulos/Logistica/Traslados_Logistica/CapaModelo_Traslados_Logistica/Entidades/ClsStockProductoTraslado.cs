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

        public int Disponible
        {
            get { return StockActual - Apartados; }
        }
    }
}
