using System;
using System.Data;
using System.Data.Odbc;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Pedro José Gómez Villalobos
 * Carné : 0901-23-4868
 * Propósito :
 *  Capa Modelo para gestionar la comprobación de órdenes de compra.
 *  Maneja la extracción de datos y transacciones hacia tblCompra.
 * ===================================================================
*/

namespace CapaModelo_AprobacionFinanciera
{
    public class ClsRepositorioAprobacion
    {
        private readonly string _CadenaConexion = "Dsn=EmbutidosS.A";

        // MÉTODO Muestra las órdenes pendientes
        public DataTable ComprasFuncListarPendientes()
        {
            DataTable Datos = new DataTable();

            // Órdenes de Compra ya 'Autorizada'
            string Sql = @"SELECT 
                    idOrdenCompra AS 'ID Orden', 
                    CONCAT('OC-', idOrdenCompra) AS 'Documento', 
                    fechaEmisionOrdenCompra AS 'Emisión', 
                    totalOrdenCompra AS 'Total Esperado', 
                    estadoOrdenCompra AS 'Estado' 
                   FROM tblEncabezadoOrdenCompra 
                   WHERE estadoOrdenCompra = 'Autorizada';";

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
                    throw new Exception("Error al consultar las órdenes en BD: " + Ex.Message);
                }
            }
            return Datos;
        }

        public DataTable ComprasFuncObtenerDetalleOrden(int IdOrden)
        {
            DataTable DatosDetalle = new DataTable();

            // detalles de la orden
            string Sql = @"
        SELECT 
            d.ordenDetalleOrdenCompra AS 'Linea',
            d.idProducto AS 'IdProducto',
            p.nombreProducto AS 'Producto',
            d.costoUnitarioDetalleOrdenCompra AS 'Precio',
            d.cantidadDetalleOrdenCompra AS 'CantidadPedida',
            d.cantidadDetalleOrdenCompra AS 'CantidadRecibida', 
            (d.cantidadDetalleOrdenCompra * d.costoUnitarioDetalleOrdenCompra) AS 'Subtotal',
            d.idBodega AS 'IdBodega',
            d.descuentoPorcentajeDetalleOrdenCompra AS 'Descuento'
        FROM tblDetalleOrdenCompra d
        JOIN tblProducto p ON d.idProducto = p.idProducto
        WHERE d.idOrdenCompra = ?;";

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcCommand Comando = new OdbcCommand(Sql, Conexion))
                    {
                        Comando.Parameters.Add(new OdbcParameter("Id", IdOrden));
                        using (OdbcDataAdapter Adaptador = new OdbcDataAdapter(Comando))
                        {
                            Adaptador.Fill(DatosDetalle);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al consultar detalles de la orden: " + Ex.Message);
                }
            }
            return DatosDetalle;
        }

        // MÉTODO GUARDAR RECEPCIÓN ACTUALIZADO CON PARÁMETROS DE ESTADO Y MÉTODO DE VALUACIÓN
        public int ComprasFuncRegistrarComprobacion(int IdOrden, string Serie, string Numero, DateTime Recepcion, DateTime Emision, decimal MontoOrden, decimal MontoFactura, string EstadoCompra, string MetodoValuacion, DataTable Detalles)
        {
            int IdCompraGenerado = 0;

            using (OdbcConnection Conexion = new OdbcConnection(_CadenaConexion))
            {
                try
                {
                    Conexion.Open();
                    using (OdbcTransaction Transaccion = Conexion.BeginTransaction())
                    {
                        try
                        {
                            // Heredar Orden de Compra
                            int idProveedor = 0;
                            long idMoneda = 0;
                            long? idTipoCambio = null;
                            decimal tipoCambio = 1;

                            string sqlLeerOrden = "SELECT idProveedor, idMoneda, idTipoCambio, tipoCambioOrdenCompra FROM tblEncabezadoOrdenCompra WHERE idOrdenCompra = ?";
                            using (OdbcCommand CmdLeer = new OdbcCommand(sqlLeerOrden, Conexion, Transaccion))
                            {
                                CmdLeer.Parameters.Add(new OdbcParameter("idOrden", IdOrden));
                                using (OdbcDataReader Lector = CmdLeer.ExecuteReader())
                                {
                                    if (Lector.Read())
                                    {
                                        idProveedor = Convert.ToInt32(Lector["idProveedor"]);
                                        idMoneda = Convert.ToInt64(Lector["idMoneda"]);
                                        if (Lector["idTipoCambio"] != DBNull.Value)
                                            idTipoCambio = Convert.ToInt64(Lector["idTipoCambio"]);
                                        tipoCambio = Convert.ToDecimal(Lector["tipoCambioOrdenCompra"]);
                                    }
                                }
                            }

                            // tblEncabezadoCompra: INSERT con estado dinámico mediante parámetro
                            string SqlEncabezado = @"INSERT INTO tblEncabezadoCompra 
                                (idOrdenCompra, idProveedor, serieFacturaCompra, numeroFacturaCompra, fechaCompra, 
                                 idMetodoPago, idMoneda, idTipoCambio, tipoCambioCompra, subtotalCompra, 
                                 descuentoCompra, impuestoCompra, totalCompra, estadoCompra) 
                                VALUES (?, ?, ?, ?, ?, 1, ?, ?, ?, ?, 0, 0, ?, ?)";

                            using (OdbcCommand CmdEnc = new OdbcCommand(SqlEncabezado, Conexion, Transaccion))
                            {
                                CmdEnc.Parameters.Add(new OdbcParameter("idOrdenCompra", IdOrden));
                                CmdEnc.Parameters.Add(new OdbcParameter("idProveedor", idProveedor));
                                CmdEnc.Parameters.Add(new OdbcParameter("serieFacturaCompra", Serie));
                                CmdEnc.Parameters.Add(new OdbcParameter("numeroFacturaCompra", Numero));
                                CmdEnc.Parameters.Add(new OdbcParameter("fechaCompra", Recepcion.Date));
                                CmdEnc.Parameters.Add(new OdbcParameter("idMoneda", idMoneda));
                                CmdEnc.Parameters.Add(new OdbcParameter("idTipoCambio", (object)idTipoCambio ?? DBNull.Value));
                                CmdEnc.Parameters.Add(new OdbcParameter("tipoCambioCompra", OdbcType.Decimal) { Precision = 12, Scale = 6, Value = tipoCambio });
                                CmdEnc.Parameters.Add(new OdbcParameter("subtotalCompra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = MontoFactura });
                                CmdEnc.Parameters.Add(new OdbcParameter("totalCompra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = MontoFactura });
                                CmdEnc.Parameters.Add(new OdbcParameter("estado", EstadoCompra));

                                CmdEnc.ExecuteNonQuery();

                                CmdEnc.Parameters.Clear();
                                CmdEnc.CommandText = "SELECT LAST_INSERT_ID()";
                                IdCompraGenerado = Convert.ToInt32(CmdEnc.ExecuteScalar());
                            }

                            // C. Insertar en tblDetalleCompra y afectar Logística (Stock)
                            if (Detalles != null && Detalles.Rows.Count > 0)
                            {
                                string SqlDetalle = @"INSERT INTO tblDetalleCompra 
                                    (idCompra, ordenDetalleCompra, idProducto, idBodega, cantidadDetalleCompra, costoUnitarioDetalleCompra, descuentoPorcentualDetalleCompra) 
                                    VALUES (?, ?, ?, ?, ?, ?, ?)";

                                using (OdbcCommand CmdDet = new OdbcCommand(SqlDetalle, Conexion, Transaccion))
                                {
                                    foreach (DataRow Fila in Detalles.Rows)
                                    {
                                        if (Fila.RowState == DataRowState.Deleted) continue;

                                        decimal cantidadRecibida = Convert.ToDecimal(Fila["CantidadRecibida"]);
                                        int idProd = Convert.ToInt32(Fila["IdProducto"]);
                                        int idBod = Convert.ToInt32(Fila["IdBodega"]);

                                        CmdDet.Parameters.Clear();
                                        CmdDet.Parameters.Add(new OdbcParameter("idCompra", IdCompraGenerado));
                                        CmdDet.Parameters.Add(new OdbcParameter("ordenDetalleCompra", Convert.ToInt32(Fila["Linea"])));
                                        CmdDet.Parameters.Add(new OdbcParameter("idProducto", idProd));
                                        CmdDet.Parameters.Add(new OdbcParameter("idBodega", idBod));
                                        CmdDet.Parameters.Add(new OdbcParameter("cantidadDetalleCompra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = cantidadRecibida });
                                        CmdDet.Parameters.Add(new OdbcParameter("costoUnitarioDetalleCompra", OdbcType.Decimal) { Precision = 12, Scale = 4, Value = Convert.ToDecimal(Fila["Precio"]) });
                                        CmdDet.Parameters.Add(new OdbcParameter("descuento", OdbcType.Decimal) { Precision = 3, Scale = 2, Value = Convert.ToDecimal(Fila["Descuento"]) });

                                        CmdDet.ExecuteNonQuery();

                                        // REGLA LOGÍSTICA: Solo sumar existencias si la compra fue Registrada (Aprobada) y no Anulada.
                                        if (EstadoCompra == "Registrada")
                                        {
                                            string sqlActualizarStock = @"UPDATE tblStock s
                                                INNER JOIN tblBodega b ON s.Fk_Bodega_Codigo = b.idBodega
                                                SET s.stockActualStock = s.stockActualStock + ?, 
                                                    s.cargosMesStock = s.cargosMesStock + ? 
                                                WHERE s.Fk_Producto_Codigo = ? AND s.Fk_Bodega_Codigo = ?";

                                            using (OdbcCommand CmdStock = new OdbcCommand(sqlActualizarStock, Conexion, Transaccion))
                                            {
                                                CmdStock.Parameters.Add(new OdbcParameter("cant1", OdbcType.Decimal) { Value = cantidadRecibida });
                                                CmdStock.Parameters.Add(new OdbcParameter("cant2", OdbcType.Decimal) { Value = cantidadRecibida });
                                                CmdStock.Parameters.Add(new OdbcParameter("prod", idProd));
                                                CmdStock.Parameters.Add(new OdbcParameter("bod", idBod));
                                                CmdStock.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }

                            // Actualizar a 'Recibida' en el encabezado de Orden de Compra (solo para cambiar su estado)
                            string SqlActualizar = "UPDATE tblEncabezadoOrdenCompra SET estadoOrdenCompra = 'Recibida' WHERE idOrdenCompra = ?";
                            using (OdbcCommand CmdAct = new OdbcCommand(SqlActualizar, Conexion, Transaccion))
                            {
                                CmdAct.Parameters.Add(new OdbcParameter("IdOrden", IdOrden));
                                CmdAct.ExecuteNonQuery();
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
                    throw new Exception("Error transaccional en BD: " + Ex.Message);
                }
            }
            return IdCompraGenerado;
        }
    }
}