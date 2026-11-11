using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_OrdenesCompra.Entidades;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Consulta órdenes de compra, detalles y catálogos relacionados.
 * ===================================================================
*/

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
            return ComprasFuncConsultar("SELECT d.ordenDetalleOrdenCompra, d.idProducto, p.skuProducto, p.nombreProducto, d.cantidadDetalleOrdenCompra, "
                + "costoUnitarioDetalleOrdenCompra, descuentoPorcentajeDetalleOrdenCompra, "
                + "ROUND(cantidadDetalleOrdenCompra * costoUnitarioDetalleOrdenCompra * "
                + "(1 - descuentoPorcentajeDetalleOrdenCompra / 100), 4) AS subtotalDetalleOrdenCompra FROM tblDetalleOrdenCompra d LEFT JOIN tblProducto p ON p.idProducto = d.idProducto WHERE d.idOrdenCompra = ? ORDER BY d.ordenDetalleOrdenCompra", Id);
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
            string Clave = ComprasFuncColumnaCatalogo("tblMoneda", "Pk_Moneda", "idMoneda");
            string Estado = ComprasFuncColumnaCatalogo("tblMoneda", "is_active", "activoMoneda");
            return ComprasFuncConsultar(string.Format("SELECT {0} AS idMoneda, codigoMoneda, nombreMoneda, simboloMoneda "
                + "FROM tblMoneda WHERE {1} = 1 ORDER BY codigoMoneda", Clave, Estado));
        }

        internal DataTable ComprasFuncListarPlazos()
        {
            return ComprasFuncConsultar("SELECT idPlazo, nombrePlazo, diasPlazo FROM tblPlazo ORDER BY diasPlazo, nombrePlazo");
        }

        internal DataTable ComprasFuncObtenerTipoCambio(long IdMoneda, DateTime Fecha)
        {
            if (IdMoneda <= 0) throw new ArgumentOutOfRangeException(nameof(IdMoneda));
            string ClaveMoneda = ComprasFuncColumnaCatalogo("tblMoneda", "Pk_Moneda", "idMoneda");
            string Estado = ComprasFuncColumnaCatalogo("tblMoneda", "is_active", "activoMoneda");
            string ClaveCambio = ComprasFuncColumnaCatalogo("tblTipoCambio", "Pk_TipoCambio", "idTipoCambio");
            return ComprasFuncEjecucionConsulta(string.Format("SELECT t.{0} AS idTipoCambio, t.valorTipoCambio, t.fechaTipoCambio "
                + "FROM tblTipoCambio t INNER JOIN tblMoneda d ON d.{1} = t.Fk_Moneda_Destino_Codigo "
                + "WHERE t.Fk_Moneda_Origen_Codigo = ? AND d.codigoMoneda = 'GTQ' AND d.{2} = 1 "
                + "AND t.fechaTipoCambio <= ? ORDER BY t.fechaTipoCambio DESC, t.versionTipoCambio DESC, t.{0} DESC LIMIT 1", ClaveCambio, ClaveMoneda, Estado),
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

        private string ComprasFuncColumnaCatalogo(string Tabla, string Preferida, string Anterior)
        {
            using (var Columnas = ComprasFuncEjecucionConsulta(
                "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() "
                + "AND LOWER(TABLE_NAME) = LOWER(?) AND COLUMN_NAME IN (?, ?)", CommandType.Text,
                new List<OdbcParameter>
                {
                    new OdbcParameter("Tabla", OdbcType.VarChar, 64) { Value = Tabla },
                    new OdbcParameter("Preferida", OdbcType.VarChar, 64) { Value = Preferida },
                    new OdbcParameter("Anterior", OdbcType.VarChar, 64) { Value = Anterior }
                }))
            {
                bool TieneAnterior = false;
                foreach (DataRow Fila in Columnas.Rows)
                {
                    string Nombre = Convert.ToString(Fila[0]);
                    if (string.Equals(Nombre, Preferida, StringComparison.OrdinalIgnoreCase)) return Preferida;
                    if (string.Equals(Nombre, Anterior, StringComparison.OrdinalIgnoreCase)) TieneAnterior = true;
                }
                if (TieneAnterior) return Anterior;
                throw new InvalidOperationException("La tabla " + Tabla + " no contiene la columna " + Preferida + " ni " + Anterior + ". Actualiza el esquema de Compras.");
            }
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
