using System;
using System.Collections.Generic;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Contratos
{
    public interface IRepositorioComision
    {
        List<ClsVentaVendedorBase> ComisionVendedoresMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva);
        List<ClsProductoComision> ComisionVendedoresMetConsultarPorcentajes(string Metodo);
        int ComisionVendedoresMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje);
        int ComisionVendedoresMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo);
        List<ClsFacturaComision> ComisionVendedoresMetObtenerFacturas();
        List<ClsVendedorComision> ComisionVendedoresMetObtenerVendedores();
        ClsFacturaComision ComisionVendedoresMetObtenerFactura(int IdFactura);
        ClsVendedorComision ComisionVendedoresMetObtenerVendedor(int IdVendedor);
        List<ClsDetalleProductoComision> ComisionVendedoresMetObtenerDetallesFactura(int IdFactura, string Metodo);
        int ComisionVendedoresMetAsignarVendedorFactura(int IdFactura, int IdVendedor);
        int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo);
        int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones);
        int ComisionVendedoresMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo);
        int ComisionVendedoresMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones);
        void ComisionVendedoresMetSincronizarAbonos(int? IdFactura);
        List<ClsFacturaPago> ComisionVendedoresMetObtenerFacturasConSaldo();
        List<ClsFormaPagoVenta> ComisionVendedoresMetObtenerFormasPagoVenta();
        decimal ComisionVendedoresMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario);
    }
}
