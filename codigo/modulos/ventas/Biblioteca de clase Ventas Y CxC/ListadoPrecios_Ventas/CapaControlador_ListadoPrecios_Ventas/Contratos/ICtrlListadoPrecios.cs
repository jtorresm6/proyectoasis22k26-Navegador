using System.Collections.Generic;
using CapaModelo_ListadoPrecios_Ventas.Entidades;

namespace CapaControlador_ListadoPrecios_Ventas.Contratos
{
    public interface ICtrlListadoPrecios
    {
        List<ClsPrecioCliente> VentasFuncConsultarListado(
            string tipoCliente, string marca, string linea,
            string categoria, string proveedor, string producto);

        List<string> VentasFuncObtenerProveedores();

        decimal VentasFuncCalcularPrecioFinal(
            decimal precioBase, decimal valor, string tipoModificacion);

        bool VentasFuncGuardar(
            string tipoCliente, string producto, decimal porcentaje,
            decimal precioBase, string tipoModificacion, decimal valor,
            string alcance, out string mensaje);

        bool VentasFuncGuardarVarios(
            IList<ClsPrecioCliente> registros, out string mensaje);
    }
}
