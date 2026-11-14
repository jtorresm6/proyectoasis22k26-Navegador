// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Define las bases y los métodos de cálculo admitidos por el módulo.

namespace CapaControlador_ComisionVendedores_Ventas.Enumeraciones
{
    // Identifica si el cálculo se basa en lo facturado o en lo cobrado.
    public enum EnumBaseComision
    {
        Ventas,
        Cobrado
    }

    // Identifica la clasificación de la cual se toma el porcentaje.
    public enum EnumMetodoComision
    {
        Vendedor,
        Marca,
        Linea,
        Producto,
        Categoria
    }
}
