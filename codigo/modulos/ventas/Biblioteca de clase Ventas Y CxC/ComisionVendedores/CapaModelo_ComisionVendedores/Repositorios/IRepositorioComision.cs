// Autor: Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
// Define las operaciones de persistencia que utiliza el controlador.

using System;
using System.Collections.Generic;
using CapaModelo_ComisionVendedores.Entidades;

namespace CapaModelo_ComisionVendedores.Contratos
{
    public interface IRepositorioComision
    {
        List<ClsVentaVendedorBase> VentasMetObtenerVentasPorMetodo(string Metodo, DateTime FechaInicio, DateTime FechaFinExclusiva);
        List<ClsProductoComision> VentasMetConsultarPorcentajes(string Metodo);
        int VentasMetActualizarPorcentaje(string Metodo, int IdClasificacion, decimal Porcentaje);
        int VentasMetGuardarComisiones(List<ClsVentaVendedorBase> Comisiones, DateTime FechaInicio, DateTime FechaFin, string Metodo);
        List<ClsFacturaComision> VentasMetObtenerFacturas();
        List<ClsVendedorComision> VentasMetObtenerVendedores();
        ClsFacturaComision VentasMetObtenerFactura(int IdFactura);
        ClsVendedorComision VentasMetObtenerVendedor(int IdVendedor);
        List<ClsDetalleProductoComision> VentasMetObtenerDetallesFactura(int IdFactura, string Metodo);
        int VentasMetAsignarVendedorFactura(int IdFactura, int IdVendedor);
        int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, string Metodo);
        int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones);
        int VentasMetGuardarComisionFactura(int IdFactura, int IdVendedor, string Metodo);
        int VentasMetGuardarComisionFacturaPorProducto(int IdFactura, int IdVendedor, List<ClsDetalleProductoComision> Selecciones);
        void VentasMetSincronizarAbonos(int? IdFactura);
        List<ClsFacturaPago> VentasMetObtenerFacturasConSaldo();
        List<ClsFormaPagoVenta> VentasMetObtenerFormasPagoVenta();
        decimal VentasMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario);
        List<ClsComisionNomina> VentasMetObtenerComisionesNomina();
    }
}
