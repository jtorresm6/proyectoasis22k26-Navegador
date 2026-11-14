using System;

namespace CapaModelo_ListadoPrecios_Ventas.Entidades
{
    /// <summary>Entidad que representa el precio asignado a un tipo de cliente y un producto.</summary>
    public class ClsPrecioCliente
    {
        // Llaves reales de la base de datos: tbltipocliente, tblproducto y tblproveedor.
        public int IdListaPrecio { get; set; }
        public int IdTipoCliente { get; set; }
        public int IdProducto { get; set; }
        // 0 cuando el producto todavía no tiene proveedor asignado.
        public int IdProveedor { get; set; }
        public string TipoCliente { get; set; }
        public string Marca { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string Proveedor { get; set; }
        public string Producto { get; set; }
        public string TipoModificacion { get; set; }
        public decimal Valor { get; set; }
        public string AlcanceModificacion { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}
