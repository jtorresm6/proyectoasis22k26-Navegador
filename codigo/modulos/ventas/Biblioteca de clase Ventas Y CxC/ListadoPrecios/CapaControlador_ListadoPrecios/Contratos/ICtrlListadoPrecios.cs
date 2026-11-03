using System.Collections.Generic;
using CapaModelo_ListadoPrecios.Entidades;
using CapaControlador_ListadoPrecios;

namespace CapaControlador_ListadoPrecios.Contratos
{

    namespace CapaControlador_ListadoPrecios
    {
        public interface ICtrlListadoPrecios
        {
            List<ClsPrecioCliente> VentasFuncConsultarListado(string tipoCliente, string marca, string linea, string categoria, string proveedor, string producto);
            decimal VentasFuncCalcularPrecioFinal(decimal precioBase, decimal valor, string tipoModificacion);
            bool VentasFuncGuardar(string tipoCliente, string producto, decimal porcentaje, decimal precioBase, string tipoModificacion, decimal valor, string alcance, out string mensaje);
        }
    }
}
