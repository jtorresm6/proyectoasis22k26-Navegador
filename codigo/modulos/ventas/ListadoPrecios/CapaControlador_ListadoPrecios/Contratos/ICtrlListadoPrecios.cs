using System.Collections.Generic;

using CapaControlador_ListadoPrecios;

namespace CapaControlador_ListadoPrecios.Contratos
{
    /// <summary>Contrato de operaciones disponibles para el Listado de Precios.</summary>
    public interface ICtrlListadoPrecios
    {
        List<ClsPrecioCliente> ListadoPreciosMetConsultarListado();
        decimal ListadoPreciosMetCalcularPrecioFinal(decimal PrecioBase, decimal PorcentajeDescuento);
        bool ListadoPreciosMetGuardar(string TipoCliente, string TipoProducto, decimal PorcentajeDescuento, decimal PrecioBase, out string Mensaje);
    }
}
