using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_OrdenesCompra.Entidades;

namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsConsultasOrdenesCompra : ClsRepositorioBaseOrdenesCompra
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

        internal ClsOrdenCompra ComprasFuncObtener(int Id)
        {
            ComprasProcValidarId(Id);
            using (DataTable Datos = ComprasFuncConsultar(_Consulta + "WHERE o.idOrdenCompra = ?", Id))
            {
                return Datos.Rows.Count == 0 ? null : new ClsOrdenCompra(Datos.Rows[0]);
            }
        }

        internal DataTable ComprasFuncListarDetalles(int Id)
        {
            ComprasProcValidarId(Id);
            return ComprasFuncConsultar("SELECT ordenDetalleOrdenCompra, idProducto, cantidadDetalleOrdenCompra, "
                + "costoUnitarioDetalleOrdenCompra, descuentoPorcentajeDetalleOrdenCompra, "
                + "ROUND(cantidadDetalleOrdenCompra * costoUnitarioDetalleOrdenCompra * "
                + "(1 - descuentoPorcentajeDetalleOrdenCompra / 100), 4) AS subtotalDetalleOrdenCompra FROM tblDetalleOrdenCompra WHERE idOrdenCompra = ? ORDER BY ordenDetalleOrdenCompra", Id);
        }

        internal DataTable ComprasFuncListarSucursales()
        {
            return ComprasFuncConsultar("SELECT idSucursal, nombreSucursal FROM tblSucursal ORDER BY nombreSucursal");
        }

        internal DataTable ComprasFuncListarBodegas()
        {
            return ComprasFuncConsultar("SELECT idBodega, nombreBodega, Fk_Sucursal_Codigo, paisBodega FROM tblBodega ORDER BY nombreBodega");
        }

        internal DataTable ComprasFuncListarProductos()
        {
            return ComprasFuncConsultar("SELECT idProducto, skuProducto, nombreProducto, descripcionProducto, pesoProducto, precioCostoProducto FROM tblProducto ORDER BY nombreProducto");
        }

        internal DataTable ComprasFuncListarMonedas()
        {
            return ComprasFuncConsultar("SELECT idMoneda, codigoMoneda, nombreMoneda, simboloMoneda "
                + "FROM tblMoneda WHERE activoMoneda = 1 ORDER BY codigoMoneda");
        }

        internal DataTable ComprasFuncListarPlazos()
        {
            return ComprasFuncConsultar("SELECT idPlazo, nombrePlazo, diasPlazo FROM tblPlazo ORDER BY diasPlazo, nombrePlazo");
        }

        internal DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, DateTime Fecha)
        {
            if (IdMoneda <= 0) throw new ArgumentOutOfRangeException(nameof(IdMoneda));
            return ComprasFuncEjecucionConsulta("SELECT t.idTipoCambio, t.valorTipoCambio, t.fechaTipoCambio "
                + "FROM tblTipoCambio t INNER JOIN tblMoneda d ON d.idMoneda = t.Fk_Moneda_Destino_Codigo "
                + "WHERE t.Fk_Moneda_Origen_Codigo = ? AND d.codigoMoneda = 'GTQ' AND d.activoMoneda = 1 "
                + "AND t.fechaTipoCambio <= ? ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC, t.idTipoCambio DESC LIMIT 1",
                CommandType.Text, new List<OdbcParameter>
                {
                    new OdbcParameter("Moneda", OdbcType.BigInt) { Value = IdMoneda },
                    new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date }
                });
        }

        internal DataTable ComprasFuncListarProveedores()
        {
            return ComprasFuncConsultar("SELECT idProveedor, nombreProveedor, nitProveedor, telefonoProveedor, "
                + "correoProveedor, direccionProveedor, estadoProveedor, saldoActualProveedor, idPlazo FROM tblProveedor "
                + "WHERE estadoProveedor = 1 ORDER BY nombreProveedor");
        }

        private DataTable ComprasFuncConsultar(string Sql, int? Id = null)
        {
            var Parametros = new List<OdbcParameter>();
            if (Id.HasValue)
                Parametros.Add(new OdbcParameter("Id", OdbcType.Int) { Value = Id.Value });
            return ComprasFuncEjecucionConsulta(Sql, CommandType.Text, Parametros);
        }

        private static void ComprasProcValidarId(int Id)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id));
        }
    }
}
