namespace CapaModelo_Traslados_Logistica.Entidades
{
    public class ClsProductoTrasladoDetalle
    {
        public int CodigoProducto { get; set; }
        public string SKU { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int ExistenciasDisponibles { get; set; }
        public decimal PesoUnitario { get; set; }

        public decimal PesoTotal
        {
            get { return PesoUnitario * Cantidad; }
        }
    }
}
