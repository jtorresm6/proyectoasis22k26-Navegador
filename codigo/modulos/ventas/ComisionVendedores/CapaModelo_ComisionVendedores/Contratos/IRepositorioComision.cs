using System;
using System.Collections.Generic;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Contratos
{
    public interface IRepositorioComision
    {
        List<ClsVentaVendedorBase> VentasFuncObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva);
        List<ClsProductoComision> VentasFuncConsultarPorcentajes(string Metodo);
        int VentasFuncActualizarPorcentaje(string Metodo, List<int> IdsProducto, decimal Porcentaje);
        int VentasFuncGuardarComisiones(List<ClsComisionCalculada> Comisiones);
        List<ClsFacturaComision> VentasFuncObtenerFacturas();
        List<ClsVendedorComision> VentasFuncObtenerVendedores();
        ClsFacturaComision VentasFuncObtenerFactura(int IdFactura);
        ClsVendedorComision VentasFuncObtenerVendedor(int IdVendedor, DateTime Fecha);
        List<ClsDetalleProductoComision> VentasFuncObtenerDetallesFactura(int IdFactura, string Metodo);
    }
}
