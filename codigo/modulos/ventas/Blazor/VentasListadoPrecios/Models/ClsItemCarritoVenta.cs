namespace VentasListadoPrecios.Models;

public sealed class ClsItemCarritoVenta
{
    public ClsProductoVenta Producto { get; set; } = new();
    public int Cantidad { get; set; }
}