using System.Collections.Generic;
using CapaModelo_ListadoPrecios.Entidades;

namespace CapaModelo_ListadoPrecios.Contratos
{
    /// <summary>Define las operaciones de persistencia del Listado de Precios.</summary>
    public interface IRepositorioListadoPrecios
    {
        List<ClsPrecioCliente> ListadoPreciosMetObtenerListado();
        bool ListadoPreciosMetGuardar(ClsPrecioCliente RegistroPrecio, out string Mensaje);
    }
}
