/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Representa un producto y sus cantidades dentro de un traslado.
 * ===================================================================
 */

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
        public double PesoUnitario { get; set; }

    }
}
