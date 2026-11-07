using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_OrdenesCompra.Repositorios
{
    internal sealed class ClsComandosOrdenesCompra : ClsRepositorioBaseOrdenesCompra
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
                var Orden = new ClsConsultasOrdenesCompra().ComprasFuncObtener(Id);
                if (Orden == null || Orden.Estado != "Pendiente" || Orden.Bloqueado || Orden.FechaVencimiento != Fecha.Date)
                    throw new InvalidOperationException("No se pudo actualizar el vencimiento de la orden pendiente.");
            }
        }

        internal int ComprasFuncCrear(int IdProveedor, int IdPlazo, int IdMoneda, DateTime Emision,
            DateTime Vencimiento, decimal TipoCambio, decimal Subtotal, decimal Descuento, string Comentario, decimal Impuesto = 0M)
        {
            const string Sql = "INSERT INTO tblEncabezadoOrdenCompra (idProveedor, idPlazo, idMoneda, "
                + "fechaEmisionOrdenCompra, fechaVencimientoOrdenCompra, tipoCambioOrdenCompra, "
                + "subtotalOrdenCompra, descuentoOrdenCompra, impuestoOrdenCompra, totalOrdenCompra, comentarioOrdenCompra) "
                + "SELECT p.idProveedor, z.idPlazo, ?, ?, ?, ?, ?, ?, ?, ?, ? FROM tblProveedor p "
                + "CROSS JOIN tblPlazo z WHERE p.idProveedor = ? AND p.estadoProveedor = 1 AND z.idPlazo = ?";
            return ComprasFuncEjecutar((Conexion, Transaccion) =>
            {
                var Parametros = new List<OdbcParameter>
                {
                    new OdbcParameter("Moneda", OdbcType.Int) { Value = IdMoneda },
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
                    return Convert.ToInt32(Comando.ExecuteScalar());
                }
            }, true);
        }

        private static OdbcParameter ComprasFuncParametroDecimal(string Nombre, decimal Valor, byte Escala)
        {
            return new OdbcParameter(Nombre, OdbcType.Decimal) { Precision = 12, Scale = Escala, Value = Valor };
        }

        private static void ComprasProcValidarId(int Id)
        {
            if (Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(Id));
        }
    }
}
