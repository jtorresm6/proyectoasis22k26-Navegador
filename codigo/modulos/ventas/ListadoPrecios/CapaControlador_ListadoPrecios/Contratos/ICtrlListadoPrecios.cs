using System.Collections.Generic;

using CapaControlador_ListadoPrecios;

namespace CapaControlador_ListadoPrecios.Contratos
{
    /// <summary>Contrato de operaciones disponibles para el Listado de Precios.</summary>
    public interface ICtrlListadoPrecios
    {
        List<ClsPrecioCliente> VentasFuncConsultarListado();
        decimal VentasFuncCalcularPrecioFinal(decimal PrecioBase, decimal Descuento);
        bool VentasFuncGuardar(string TipoCliente, string TipoProducto, decimal Porcentaje, decimal PrecioBase, out string Mensaje);
    }
}
