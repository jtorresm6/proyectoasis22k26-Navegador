using System;
using System.Data;
using System.Data.Odbc;

namespace CapaModelo_AprobacionFinanciera
{
    public class ClsRepositorioDevoluciones
    {
        private readonly string _CadenaConexion = "Dsn=EmbutidosS.A";

        public DataTable ComprasFuncListarCompras()
        {
            DataTable Datos = new DataTable();

            string Sql = @"
                SELECT 
                    c.idCompra AS 'idCompra', 
                    c.serieFacturaCompra AS 'serieFacturaCompra', 
                    c.numeroFacturaCompra AS 'numeroFacturaCompra', 
                    p.nombreProveedor AS 'Proveedor', 
                    c.fechaCompra AS 'fechaCompra', 
                    c.totalCompra AS 'Total', 
                    c.estadoCompra AS 'estadoCompra' 
                FROM tblEncabezadoCompra c
                INNER JOIN tblProveedor p ON c.idProveedor = p.idProveedor
                ORDER BY c.idCompra DESC;";

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Sql, Conexion))
                    {
                        Adaptador.Fill(Datos);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar las compras en BD: " + Ex.Message);
                }
            }
            return Datos;
        }
        // MÉTODO datos generales (Encabezado) compra específica
        public DataTable ComprasFuncObtenerEncabezadoCompra(int IdCompra)
        {
            DataTable Datos = new DataTable();
            string Sql = @"
                SELECT 
                    c.idCompra,
                    c.idOrdenCompra,
                    c.serieFacturaCompra, 
                    c.numeroFacturaCompra, 
                    c.fechaCompra, 
                    c.totalCompra, 
                    c.estadoCompra 
                FROM tblEncabezadoCompra c
                WHERE c.idCompra = ?;";

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
                    {
                        Comando.Parameters.Add(new OdbcParameter("Id", IdCompra));
                        using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Comando))
                        {
                            Adaptador.Fill(Datos);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar el encabezado de la compra: " + Ex.Message);
                }
            }
            return Datos;
        }

        // MÉTODO Extrae productos (Detalles) compra específica
        public DataTable ComprasFuncObtenerDetallesCompra(int IdCompra)
        {
            DataTable DatosDetalle = new DataTable();
            string Sql = @"
                SELECT 
                    d.ordenDetalleCompra AS 'Linea',
                    d.idProducto AS 'IdProducto',
                    p.nombreProducto AS 'Producto',
                    d.costoUnitarioDetalleCompra AS 'Precio',
                    d.cantidadDetalleCompra AS 'CantidadRecibida', 
                    (d.cantidadDetalleCompra * d.costoUnitarioDetalleCompra) AS 'Subtotal',
                    d.idBodega AS 'IdBodega',
                    d.descuentoPorcentualDetalleCompra AS 'Descuento'
                FROM tblDetalleCompra d
                JOIN tblProducto p ON d.idProducto = p.idProducto
                WHERE d.idCompra = ?;";

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
                    {
                        Comando.Parameters.Add(new OdbcParameter("Id", IdCompra));
                        using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Comando))
                        {
                            Adaptador.Fill(DatosDetalle);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar detalles de la compra: " + Ex.Message);
                }
            }
            return DatosDetalle;
        }
        public int ComprasFuncRegistrarDevolucion(int IdCompra, DateTime FechaDevolucion, string Motivo, string SerieNota, string NumeroNota, decimal TotalDevolucion, DataTable Detalles)
        {
            int IdDevolucionGenerado = 0;

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insertar el Encabezado de la Devolución 
                            string SqlEncabezado = @"INSERT INTO tblDevolucionCompra 
                                (idCompra, fechaDevolucionCompra, motivoDevolucionCompra, serieNotaCreditoDevolucionCompra, numeroNotaCreditoDevolucionCompra, totalDevolucionCompra, estadoDevolucionCompra) 
                                VALUES (?, ?, ?, ?, ?, ?, 'Registrada')";

                            using (OdbcCommand CmdEnc = new OdbcCommand(SqlEncabezado, Conexion, Transaccion))
                            {
                                CmdEnc.Parameters.Add(new OdbcParameter("idCompra", IdCompra));
                                CmdEnc.Parameters.Add(new OdbcParameter("fechaDevolucionCompra", FechaDevolucion.Date));
                                CmdEnc.Parameters.Add(new OdbcParameter("motivoDevolucionCompra", Motivo));

                                // Manejo de nulos si dejan en blanco la serie o el número
                                CmdEnc.Parameters.Add(new OdbcParameter("serie", string.IsNullOrWhiteSpace(SerieNota) ? (object)DBNull.Value : SerieNota.Trim()));
                                CmdEnc.Parameters.Add(new OdbcParameter("numero", string.IsNullOrWhiteSpace(NumeroNota) ? (object)DBNull.Value : NumeroNota.Trim()));

                                CmdEnc.Parameters.Add(new OdbcParameter("totalDevolucionCompra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = TotalDevolucion });
                                CmdEnc.ExecuteNonQuery();

                                CmdEnc.Parameters.Clear();
                                CmdEnc.CommandText = "SELECT LAST_INSERT_ID()";
                                IdDevolucionGenerado = Convert.ToInt32(CmdEnc.ExecuteScalar());
                            }

                            // 2. Insertar los Detalles y Afectar el Stock Logístico
                            if (Detalles != null && Detalles.Rows.Count > 0)
                            {
                                string SqlDetalle = @"INSERT INTO tblDetalleDevolucionCompra 
                                    (idDevolucionCompra, ordenDetalleDevolucionCompra, idProducto, idBodega, cantidadDetalleDevolucionCompra, costoUnitarioDetalleDevolucionCompra, descuentoPorcentajeDetalleDevolucionCompra) 
                                    VALUES (?, ?, ?, ?, ?, ?, 0)";

                                string sqlRestarStock = @"UPDATE tblStock s
                                    INNER JOIN tblBodega b ON s.Fk_Bodega_Codigo = b.idBodega
                                    SET s.stockActualStock = s.stockActualStock - ?, 
                                        s.abonosMesStock = s.abonosMesStock + ? 
                                    WHERE s.Fk_Producto_Codigo = ? AND s.Fk_Bodega_Codigo = ?";

                                using (OdbcCommand CmdDet = new OdbcCommand(SqlDetalle, Conexion, Transaccion))
                                using (OdbcCommand CmdStockDev = new OdbcCommand(sqlRestarStock, Conexion, Transaccion))
                                {
                                    int linea = 1;
                                    foreach (DataRow Fila in Detalles.Rows)
                                    {
                                        if (Fila.RowState == DataRowState.Deleted) continue;

                                        decimal cantidadDevuelta = Convert.ToDecimal(Fila["Cantidad"]);
                                        int idProdDev = Convert.ToInt32(Fila["IdProducto"]);
                                        int idBodDev = Convert.ToInt32(Fila["IdBodega"]);
                                        decimal costoUnitario = Convert.ToDecimal(Fila["Precio"]);

                                        CmdDet.Parameters.Clear();
                                        CmdDet.Parameters.Add(new OdbcParameter("idDevolucionCompra", IdDevolucionGenerado));
                                        CmdDet.Parameters.Add(new OdbcParameter("orden", linea));
                                        CmdDet.Parameters.Add(new OdbcParameter("idProducto", idProdDev));
                                        CmdDet.Parameters.Add(new OdbcParameter("idBodega", idBodDev));
                                        CmdDet.Parameters.Add(new OdbcParameter("cantidad", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = cantidadDevuelta });
                                        CmdDet.Parameters.Add(new OdbcParameter("costo", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = costoUnitario });
                                        CmdDet.ExecuteNonQuery();

                                        CmdStockDev.Parameters.Clear();
                                        CmdStockDev.Parameters.Add(new OdbcParameter("cant1", OdbcType.Decimal) { Value = cantidadDevuelta });
                                        CmdStockDev.Parameters.Add(new OdbcParameter("cant2", OdbcType.Decimal) { Value = cantidadDevuelta });
                                        CmdStockDev.Parameters.Add(new OdbcParameter("prod", idProdDev));
                                        CmdStockDev.Parameters.Add(new OdbcParameter("bod", idBodDev));
                                        CmdStockDev.ExecuteNonQuery();

                                        linea++;
                                    }
                                }
                            }
                            Transaccion.Commit();
                        }
                        catch (Exception)
                        {
                            Transaccion.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error transaccional al guardar devolución: " + Ex.Message);
                }
            }
            return IdDevolucionGenerado;
        }
    }
}