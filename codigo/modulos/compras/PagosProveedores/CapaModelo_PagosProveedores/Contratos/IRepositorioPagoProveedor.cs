using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_PagosProveedores.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 20/10/2026
 * ==================================================================
 * Propósito :
 *  Define las consultas de pagos a proveedores y la obtención de
 *  proveedores y cuentas pendientes, además de las operaciones comunes.
 * ===================================================================
*/

namespace CapaModelo_PagosProveedores.Contratos
{

    public interface IRepositorioPagoProveedor : IRepositorioGenerico<ClsPagoProveedor>
    {
        IEnumerable<ClsPagoProveedor> ComprasMetConsultar(int? IdProveedor, string Estado, DateTime? Desde, DateTime? Hasta);
        ClsPagoProveedor ComprasMetObtenerPorId(int IdPago);
        DataTable ComprasMetObtenerProveedores();
        DataTable ComprasMetObtenerCuentasPendientes(int IdProveedor);
    }
}
