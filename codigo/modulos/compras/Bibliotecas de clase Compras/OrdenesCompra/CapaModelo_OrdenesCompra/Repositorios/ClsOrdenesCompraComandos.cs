using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Guarda órdenes de compra y ejecuta cambios de estado y vencimiento.
 * ===================================================================
*/

namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsOrdenesCompraComandos : ClsOrdenesCompraRepositorioBase
    {

        internal void ComprasProcCambiarEstado(int Id, string Estado)
        {
            ComprasProcValidarId(Id);
            ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                var Parametros = new List<OdbcParameter> { new OdbcParameter("Id", OdbcType.Int) { Value = Id } };
                using (var Consulta = ComprasFuncCrearComando(Conexion, Transaccion,
                    "SELECT estadoOrdenCompra, bloqueadoOrdenCompra FROM tblEncabezadoOrdenCompra WHERE idOrdenCompra = ? FOR UPDATE", CommandType.Text, Parametros))
                {
                    using (var Fila = Consulta.ExecuteReader())
                    {
                        if (!Fila.Read())
                            throw new InvalidOperationException("La orden de compra no existe.");
                        if (Convert.ToBoolean(Fila[1]))
                            throw new InvalidOperationException("La orden de compra est\u00e1 bloqueada.");
                        string Actual = Convert.ToString(Fila[0]);
                        if (Actual == Estado)
                            return 0;
                        if (Estado == "Anulada" && Actual == "Recibida")
                            throw new InvalidOperationException("No se puede anular una orden recibida.");
                    }
                }
                Parametros = new List<OdbcParameter>
                {
                    new OdbcParameter("Estado", OdbcType.VarChar, 30) { Value = Estado },
                    new OdbcParameter("Id", OdbcType.Int) { Value = Id }
                };
                string Sql = "UPDATE tblEncabezadoOrdenCompra SET estadoOrdenCompra = ? WHERE idOrdenCompra = ? AND bloqueadoOrdenCompra = 0";
                if (Estado == "Anulada")
                    Sql += " AND NOT EXISTS (SELECT 1 FROM tblEncabezadoCompra c WHERE c.idOrdenCompra = tblEncabezadoOrdenCompra.idOrdenCompra)";
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, Sql, CommandType.Text, Parametros))
                {
                    int Afectadas = Comando.ExecuteNonQuery();
                    if (Afectadas != 1)
                        throw new InvalidOperationException("No se pudo cambiar el estado. La orden puede tener una compra registrada.");
                    return Afectadas;
                }
            }, true);
        }

        internal void ComprasProcAnular(int Id)
        {
            ComprasProcValidarId(Id);
            int Afectadas = ComprasFuncEjecucionNonQuery("UPDATE tblEncabezadoOrdenCompra o SET estadoOrdenCompra = 'Anulada' "
                + "WHERE o.idOrdenCompra = ? AND o.bloqueadoOrdenCompra = 0 AND o.estadoOrdenCompra IN ('Pendiente', 'Autorizada', 'Rechazada') "
                + "AND NOT EXISTS (SELECT 1 FROM tblEncabezadoCompra c WHERE c.idOrdenCompra = o.idOrdenCompra)",
                new List<OdbcParameter> { new OdbcParameter("Id", OdbcType.Int) { Value = Id } }, CommandType.Text);
            if (Afectadas != 1)
                throw new InvalidOperationException("La orden no existe, ya está anulada o tiene una compra registrada.");
        }

        internal void ComprasProcActualizarVencimiento(int Id, DateTime Fecha)
        {
            ComprasProcValidarId(Id);
            int Afectadas = ComprasFuncEjecucionNonQuery("UPDATE tblEncabezadoOrdenCompra SET fechaVencimientoOrdenCompra = ? "
                + "WHERE idOrdenCompra = ? AND fechaEmisionOrdenCompra <= ? AND estadoOrdenCompra = 'Pendiente' AND bloqueadoOrdenCompra = 0",
                new List<OdbcParameter>
                {
                    new OdbcParameter("Fecha", OdbcType.Date) { Value = Fecha.Date },
                    new OdbcParameter("Id", OdbcType.Int) { Value = Id },
                    new OdbcParameter("Emision", OdbcType.Date) { Value = Fecha.Date }
                }, CommandType.Text);
            if (Afectadas == 0)
            {
                var Orden = new ClsEncabezadosOrdenesCompraConsultar().ComprasFuncObtener(Id);
                if (Orden == null || Orden.Estado != "Pendiente" || Orden.Bloqueado || Orden.FechaVencimiento != Fecha.Date)
                    throw new InvalidOperationException("No se pudo actualizar el vencimiento de la orden pendiente.");
            }
        }

        internal int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M,
            DataTable Detalles = null, long? IdTipoCambio = null)
        {
            const string Sql = "INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, idMoneda, idTipoCambio, "
                + "fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, tipoCambioOrdenCompra, "
                + "subtotalOrdenCompra, descuentoOrdenCompra, impuestoOrdenCompra, totalOrdenCompra, comentarioOrdenCompra) "
                + "SELECT p.idProveedor, z.idPlazo, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? FROM tblProveedor p "
                + "CROSS JOIN tblPlazo z WHERE p.idProveedor = ? AND p.estadoProveedor = 1 AND z.idPlazo = ?";
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                var Parametros = new List<OdbcParameter>
                {
                    new OdbcParameter("Moneda", OdbcType.Int) { Value = IdMoneda },
                    new OdbcParameter("IdTipoCambio", OdbcType.BigInt) { Value = (object)IdTipoCambio ?? DBNull.Value },
                    new OdbcParameter("Emision", OdbcType.Date) { Value = Emision.Date },
                    new OdbcParameter("Vencimiento", OdbcType.Date) { Value = Vencimiento.Date },
                    ComprasFuncParametroDecimal("TipoCambio", TipoCambio, 6),
                    ComprasFuncParametroDecimal("Subtotal", Subtotal, 4),
                    ComprasFuncParametroDecimal("Descuento", Descuento, 4),
                    ComprasFuncParametroDecimal("Impuesto", Impuesto, 4),
                    ComprasFuncParametroDecimal("Total", Subtotal - Descuento + Impuesto, 4),
                    new OdbcParameter("Comentario", OdbcType.VarChar, 500) { Value = (object)Comentario ?? DBNull.Value },
                    new OdbcParameter("Proveedor", OdbcType.Int) { Value = IdProveedor },
                    new OdbcParameter("Plazo", OdbcType.Int) { Value = IdPlazo }
                };
                using (var Comando = ComprasFuncCrearComando(Conexion, Transaccion, Sql, CommandType.Text, Parametros))
                {
                    if (Comando.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("El proveedor está inactivo o el plazo no existe.");
                    Comando.Parameters.Clear();
                    Comando.CommandText = "SELECT LAST_INSERT_ID()";
                    int Id = Convert.ToInt32(Comando.ExecuteScalar());
                    if (Id <= 0) throw new InvalidOperationException("No se obtuvo el número de la orden.");
                    if (Detalles != null)
                    {
                        int Linea = 0;
                        foreach (DataRow Detalle in Detalles.Rows)
                        {
                            Comando.Parameters.Clear();
                            Comando.CommandText = "INSERT INTO tblDetalleOrdenCompra (idOrdenCompra, ordenDetalleOrdenCompra, idProducto, idBodega, "
                                + "cantidadDetalleOrdenCompra, costoUnitarioDetalleOrdenCompra, descuentoPorcentajeDetalleOrdenCompra) SELECT ?, ?, ?, ?, ?, ?, ? "
                                + "FROM tblProducto WHERE idProducto = ? AND idProveedorProductos = ?";
                            Comando.Parameters.Add(new OdbcParameter("Orden", OdbcType.Int) { Value = Id });
                            Comando.Parameters.Add(new OdbcParameter("Linea", OdbcType.Int) { Value = ++Linea });
                            Comando.Parameters.Add(new OdbcParameter("Producto", OdbcType.Int) { Value = Detalle["idProducto"] });
                            Comando.Parameters.Add(new OdbcParameter("Bodega", OdbcType.Int) { Value = Detalle["idBodega"] });
                            Comando.Parameters.Add(ComprasFuncParametroDecimal("Cantidad", Convert.ToDecimal(Detalle["cantidad"]), 4));
                            Comando.Parameters.Add(ComprasFuncParametroDecimal("Costo", Convert.ToDecimal(Detalle["precio"]), 4));
                            Comando.Parameters.Add(ComprasFuncParametroDecimal("Porcentaje", Convert.ToDecimal(Detalle["porcentaje"]), 2));
                            Comando.Parameters.Add(new OdbcParameter("ProductoProveedor", OdbcType.Int) { Value = Detalle["idProducto"] });
                            Comando.Parameters.Add(new OdbcParameter("ProveedorProducto", OdbcType.Int) { Value = IdProveedor });
                            if (Comando.ExecuteNonQuery() != 1)
                                throw new InvalidOperationException("No se pudo guardar el producto: no existe o no pertenece al proveedor de la orden.");
                        }
                    }
                    return Id;
                }
            }, true);
        }

        private static OdbcParameter ComprasFuncParametroDecimal(string Nombre, decimal Valor, byte Escala)
        {
            return new OdbcParameter(Nombre, OdbcType.Decimal) { Precision = 12, Scale = Escala, Value = Valor };
        }


    }
}
