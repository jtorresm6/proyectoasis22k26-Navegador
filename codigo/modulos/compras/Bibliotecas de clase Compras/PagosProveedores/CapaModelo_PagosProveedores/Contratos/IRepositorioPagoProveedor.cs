using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_PagosProveedores.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Propósito :
 *  Contrato del repositorio de pagos a proveedores (encabezado + detalle).

 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Contratos
{
    public interface IRepositorioPagoProveedor
    {
        int ComprasMetAgregar(ClsEncabezadoPagoProveedor Pago);
        void ComprasMetEditar(ClsEncabezadoPagoProveedor Pago);
        void ComprasMetRemover(ClsEncabezadoPagoProveedor Pago);

        List<ClsEncabezadoPagoProveedor> ComprasMetConsultar(int? IdProveedor, string Estado, DateTime? Desde, DateTime? Hasta);
        ClsEncabezadoPagoProveedor ComprasMetObtenerPorId(int IdPago);

        DataTable ComprasMetObtenerProveedores();
        DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor);
        DataTable ComprasMetObtenerMetodosPago();
        DataTable ComprasMetObtenerMonedas();

        // Busca el tipo de cambio de la moneda a GTQ vigente a la fecha indicada.
        void ComprasMetResolverTipoCambio(long IdMoneda, DateTime Fecha, out long? IdTipoCambio, out decimal Valor);
    }
}