using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_OrdenesCompra.Entidades;
namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsOrdenesCompraConsultarCatalogos : ClsOrdenesCompraRepositorioBase
    {
        internal DataTable ComprasFuncListarSucursales()
        {
            return ComprasFuncConsultar("SELECT idSucursal, nombreSucursal FROM tblSucursal ORDER BY nombreSucursal");
        }

        internal DataTable ComprasFuncListarBodegas()
        {
            return ComprasFuncConsultar("SELECT b.idBodega, b.nombreBodega, b.Fk_Sucursal_Codigo, b.paisBodega, s.nombreSucursal "
                + "FROM tblBodega b INNER JOIN tblSucursal s ON s.idSucursal = b.Fk_Sucursal_Codigo "
                + "ORDER BY s.nombreSucursal, b.nombreBodega");
        }

        internal DataTable ComprasFuncListarProductos()
        {
            return ComprasFuncConsultar("SELECT idProducto, idProveedorProductos, skuProducto, nombreProducto, descripcionProducto, pesoProducto, precioCostoProducto FROM tblProducto ORDER BY nombreProducto");
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
    }
}
