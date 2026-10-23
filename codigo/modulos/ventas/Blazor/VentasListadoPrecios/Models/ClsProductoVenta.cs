namespace VentasListadoPrecios.Models;

public sealed class ClsProductoVenta
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}