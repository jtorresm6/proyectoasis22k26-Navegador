using System;
using System.Collections.Generic;
using CapaControlador_ComisionVendedores.DTO;
using CapaControlador_ComisionVendedores.Enumeraciones;

namespace CapaControlador_ComisionVendedores.Contratos
{
    public interface IControladorComision
    {
        List<ClsComisionVendedorResultado> ComisionVendedoresMetCalcular(
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo,
            DateTime FechaInicio,
            DateTime FechaFin);

        int ComisionVendedoresMetGuardar(
            List<ClsComisionVendedorResultado> Resultados,
            DateTime FechaInicio,
            DateTime FechaFin,
            EnumMetodoComision Metodo,
            EnumBaseComision BaseCalculo);

        List<ClsProductoPorcentaje> ComisionVendedoresMetConsultarPorcentajes(EnumMetodoComision Metodo);
        int ComisionVendedoresMetActualizarPorcentaje(EnumMetodoComision Metodo, int IdClasificacion, decimal Porcentaje);
        List<ClsFacturaComisionVista> ComisionVendedoresMetObtenerFacturas();
        List<ClsVendedorComisionVista> ComisionVendedoresMetObtenerVendedores();
        int ComisionVendedoresMetAsignarVendedorFactura(int IdFactura, int IdVendedor);
        int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        List<ClsComisionFacturaDetalle> ComisionVendedoresMetCalcularFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        int ComisionVendedoresMetGuardarComisionFactura(int IdFactura, int IdVendedor, EnumMetodoComision Metodo);
        List<ClsComisionFacturaDetalle> ComisionVendedoresMetCalcularFactura(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones);
        int ComisionVendedoresMetAsignarVendedorYGuardarComision(int IdFactura, int IdVendedor, List<ClsComisionFacturaDetalle> Selecciones);
        List<ClsFacturaPagoVista> ComisionVendedoresMetObtenerFacturasConSaldo();
        List<ClsFormaPagoVentaVista> ComisionVendedoresMetObtenerFormasPagoVenta();
        decimal ComisionVendedoresMetRegistrarAbonoFactura(int IdFactura, decimal Monto,
            int IdFormaPago, DateTime Fecha, string Referencia, int IdUsuario);
    }
}
