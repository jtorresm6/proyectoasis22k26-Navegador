using System;
using System.Collections.Generic;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;

namespace CapaControlador_ComisionVendedores.Contratos
{
    public interface IControladorComision
    {
        List<ClsComisionVendedorResultado> VentasFuncCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin);

        int VentasFuncGuardar(
            List<ClsComisionVendedorResultado> Resultados,
            DateTime FechaInicio,
            DateTime FechaFin,
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo);

        List<ClsProductoPorcentaje> VentasFuncConsultarPorcentajes(EnumMetodoComision Metodo);
        int VentasFuncActualizarPorcentaje(EnumMetodoComision Metodo, List<int> IdsProducto, decimal Porcentaje);
        List<ClsFacturaComisionVista> VentasFuncObtenerFacturas();
        List<ClsVendedorComisionVista> VentasFuncObtenerVendedores();
        List<ClsComisionFacturaDetalle> VentasFuncCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
    }
}
