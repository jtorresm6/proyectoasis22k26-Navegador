using VentasListadoPrecios.Models;

namespace VentasListadoPrecios.Controladores;

public sealed class ClsListadoPreciosControlador
{
    private readonly List<ClsProductoVenta> _Productos = new()
    {
        new ClsProductoVenta { Codigo = "EMB-001", Nombre = "Jamón Horneado Especial", UnidadMedida = "Lb", Precio = 45.00m },
        new ClsProductoVenta { Codigo = "EMB-002", Nombre = "Salchicha Alemana", UnidadMedida = "Paq", Precio = 55.00m },
        new ClsProductoVenta { Codigo = "EMB-003", Nombre = "Chorizo Riojano Tradicional", UnidadMedida = "Pieza", Precio = 85.00m },
        new ClsProductoVenta { Codigo = "EMB-004", Nombre = "Salami Madurado", UnidadMedida = "Pieza", Precio = 42.00m }
    };
    private readonly List<ClsItemCarritoVenta> _Carrito = new();

    public IReadOnlyList<ClsProductoVenta> VentasFuncObtenerProductos() => _Productos;
    public IReadOnlyList<ClsItemCarritoVenta> VentasFuncObtenerCarrito() => _Carrito;
    public decimal VentasFuncCalcularTotal() => _Carrito.Sum(Elemento => Elemento.Producto.Precio * Elemento.Cantidad);

    public void VentasMetAgregarProducto(ClsProductoVenta Producto)
    {
        var Elemento = _Carrito.FirstOrDefault(Registro => Registro.Producto.Codigo == Producto.Codigo);
        if (Elemento is null)
        {
            _Carrito.Add(new ClsItemCarritoVenta { Producto = Producto, Cantidad = 1 });
            return;
        }
        Elemento.Cantidad++;
    }

    public void VentasMetCambiarCantidad(ClsItemCarritoVenta Elemento, int Cambio)
    {
        Elemento.Cantidad += Cambio;
        if (Elemento.Cantidad <= 0) _Carrito.Remove(Elemento);
    }

    public void VentasMetConfirmarPedido() => _Carrito.Clear();
}