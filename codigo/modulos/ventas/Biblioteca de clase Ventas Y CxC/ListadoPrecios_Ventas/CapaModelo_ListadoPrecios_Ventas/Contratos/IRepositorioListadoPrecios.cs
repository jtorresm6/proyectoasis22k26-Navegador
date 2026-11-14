using System.Collections.Generic;
using CapaModelo_ListadoPrecios_Ventas.Entidades;

namespace CapaModelo_ListadoPrecios_Ventas.Contratos
{
    /// <summary>Define las operaciones de persistencia del Listado de Precios.</summary>
    public interface IRepositorioListadoPrecios
    {
        List<ClsPrecioCliente> VentasFuncObtenerListado();
        bool VentasFuncGuardar(ClsPrecioCliente Registro, out string Mensaje);
    }
}
