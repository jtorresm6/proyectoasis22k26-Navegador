/*
 * ============================================================================
 * MÓDULO: LOGÍSTICA - INVENTARIO
 * ARCHIVO: ClsProducto.cs
 * REALIZADO POR: VELVETH SARAÍ CHAVEZ MEJÍA
 * CARNÉ: 0901 23 6269
 * DESCRIPCIÓN: Representa un registro de tblProducto y los nombres de sus catálogos asociados.
 * FECHA DE REALIZACIÓN: 05/11/2026
 * ============================================================================
 */

namespace CapaModelo_Inventario_Logistica.Entidades
{
    public class ClsProducto
    {
        public int IdProducto { get; set; }
        public string SkuProducto { get; set; }
        public string ClasificacionProducto { get; set; }
        public string NombreProducto { get; set; }
        public int IdProveedorProductos { get; set; }
        public int FkMarcaCodigo { get; set; }
        public int FkCategoriaCodigo { get; set; }
        public int FkLineaCodigo { get; set; }
        public string DescripcionProducto { get; set; }
        public byte[] ImagenProducto { get; set; }
        public string PaisProducto { get; set; }
        public decimal PrecioCostoProducto { get; set; }
        public decimal PrecioVentaProducto { get; set; }
        public decimal PorcentajeComisionProducto { get; set; }
        public int StockMinimoProducto { get; set; }
        public int StockMaximoProducto { get; set; }
        public decimal PesoProducto { get; set; }

        // Campos adicionales, recuperados con JOIN para las consultas.
        public string NombreCategoria { get; set; }
        public string NombreMarca { get; set; }
        public string NombreLinea { get; set; }
        public string NombreProveedor { get; set; }
    }
}
