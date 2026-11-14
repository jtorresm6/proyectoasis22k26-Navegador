using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_OrdenesCompra.Entidades;
namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsEncabezadosOrdenesCompraConsultar : ClsOrdenesCompraRepositorioBase
    {
        private const string _Consulta = "SELECT o.idOrdenCompra, CAST(o.idOrdenCompra AS CHAR) AS codigoOrdenCompra, o.idProveedor, o.idPlazo, o.idMoneda, "
            + "o.fechaEmisionOrdenCompra, o.fechaVencimientoOrdenCompra, o.tipoCambioOrdenCompra, "
            + "o.subtotalOrdenCompra, o.descuentoOrdenCompra, o.impuestoOrdenCompra, o.bloqueadoOrdenCompra, o.totalOrdenCompra, "
            + "o.comentarioOrdenCompra, o.estadoOrdenCompra, p.nombreProveedor, p.nitProveedor, "
            + "p.telefonoProveedor, p.correoProveedor, p.direccionProveedor, p.estadoProveedor, "
            + "z.nombrePlazo, z.diasPlazo FROM tblEncabezadoOrdenCompra o "
            + "INNER JOIN tblProveedor p ON p.idProveedor = o.idProveedor "
            + "INNER JOIN tblPlazo z ON z.idPlazo = o.idPlazo ";

        internal DataTable ComprasFuncListar()
        {
            return ComprasFuncConsultar(_Consulta + "ORDER BY o.fechaEmisionOrdenCompra DESC, o.idOrdenCompra DESC");
        }

        internal DataTable ComprasFuncListar(string Estado)
        {
            return ComprasFuncEjecucionConsulta(_Consulta + "WHERE o.estadoOrdenCompra = ? "
                + "ORDER BY o.fechaEmisionOrdenCompra DESC, o.idOrdenCompra DESC", CommandType.Text,
                new List<OdbcParameter> { new OdbcParameter("Estado", OdbcType.VarChar, 30) { Value = Estado } });
        }

        internal ClsOrdenesCompraEntidad ComprasFuncObtener(int Id)
        {
            ComprasProcValidarId(Id);
            using (DataTable Datos = ComprasFuncConsultar(_Consulta + "WHERE o.idOrdenCompra = ?", Id))
            {
                return Datos.Rows.Count == 0 ? null : new ClsOrdenesCompraEntidad(Datos.Rows[0]);
            }
        }
    }
}
