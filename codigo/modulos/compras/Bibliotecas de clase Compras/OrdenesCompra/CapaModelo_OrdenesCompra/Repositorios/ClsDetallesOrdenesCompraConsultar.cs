using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_OrdenesCompra.Entidades;
namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsDetallesOrdenesCompraConsultar : ClsOrdenesCompraRepositorioBase
    {
        internal DataTable ComprasFuncListarDetalles(int Id)
        {
            ComprasProcValidarId(Id);
            return ComprasFuncConsultar("SELECT d.ordenDetalleOrdenCompra, d.idProducto, d.idBodega, b.Fk_Sucursal_Codigo AS idSucursal, "
                + "COALESCE(s.nombreSucursal, 'Sin sucursal') AS nombreSucursal, COALESCE(b.nombreBodega, 'Sin bodega') AS nombreBodega, "
                + "p.skuProducto, p.nombreProducto, "
                + "d.cantidadDetalleOrdenCompra, d.costoUnitarioDetalleOrdenCompra, d.descuentoPorcentajeDetalleOrdenCompra, "
                + "ROUND(d.costoUnitarioDetalleOrdenCompra * d.descuentoPorcentajeDetalleOrdenCompra / 100, 4) AS descuentoImporteDetalleOrdenCompra, "
                + "ROUND(d.cantidadDetalleOrdenCompra * d.costoUnitarioDetalleOrdenCompra * "
                + "(1 - d.descuentoPorcentajeDetalleOrdenCompra / 100), 4) AS subtotalDetalleOrdenCompra "
                + "FROM tblDetalleOrdenCompra d LEFT JOIN tblProducto p ON p.idProducto = d.idProducto "
                + "LEFT JOIN tblBodega b ON b.idBodega = d.idBodega LEFT JOIN tblSucursal s ON s.idSucursal = b.Fk_Sucursal_Codigo "
                + "WHERE d.idOrdenCompra = ? ORDER BY d.ordenDetalleOrdenCompra", Id);
        }
    }
}
