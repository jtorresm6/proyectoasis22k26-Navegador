using System;
using System.Collections.Generic;
using CapaControlador_ComisionVendedores_Ventas.DTO;
using CapaControlador_ComisionVendedores_Ventas.Enumeraciones;

namespace CapaControlador_ComisionVendedores_Ventas.Contratos
{
    public interface IControladorComision
    {
        List<ClsComisionVendedorResultado> VentasMetCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin);

        int VentasMetGuardar(
            List<ClsComisionVendedorResultado> Resultados,
            DateTime FechaInicio,
            DateTime FechaFin,
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo);

        List<ClsProductoPorcentaje> VentasMetConsultarPorcentajes(EnumMetodoComision Metodo);
        int VentasMetActualizarPorcentaje(EnumMetodoComision Metodo, int IdClasificacion, decimal Porcentaje);
        List<ClsFacturaComisionVista> VentasMetObtenerFacturas();
        List<ClsVendedorComisionVista> VentasMetObtenerVendedores();
        int VentasMetAsignarVendedorFactura(int IdFactura, int IdVendedor);
        int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        List<ClsComisionFacturaDetalle> VentasMetCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        int VentasMetGuardarComisionFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        List<ClsComisionFacturaDetalle> VentasMetCalcularFactura(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones);
        int VentasMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones);
        List<ClsFacturaPagoVista> VentasMetObtenerFacturasConSaldo();
        List<ClsFormaPagoVentaVista> VentasMetObtenerFormasPagoVenta();
        decimal VentasMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario);
        List<ClsComisionNominaVista> VentasMetObtenerComisionesNomina();
    }
}
