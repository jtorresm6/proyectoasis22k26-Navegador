using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_TransporteExterno_Logistica.Repositorio;

namespace CapaModelo_TransporteExterno_Logistica
{
    public class ClsRepositorioIngresarProducto : ClsRepositorio
    {
        // Obtener las guías disponibles.
        public DataTable LogisticaFuncObtenerGuias()
        {
            DataTable tabla = new DataTable();

            string sql = @"
                SELECT
                    idServicioTransporteExterno AS idServicio,
                    numeroGuiaServicioTransporteExterno AS numeroGuia
                FROM tblServicioTransporteExterno
                WHERE numeroGuiaServicioTransporteExterno IS NOT NULL
                  AND estadoServicioTransporteExterno NOT IN ('Entregado', 'Cancelado')
                ORDER BY numeroGuiaServicioTransporteExterno";

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            using (OdbcCommand comando = new OdbcCommand(sql, conexion))
            using (OdbcDataAdapter adaptador = new OdbcDataAdapter(comando))
            {
                conexion.Open();
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        // Obtener productos y existencias de la bodega de origen.
        public DataTable LogisticaFuncObtenerProductosDisponibles(int idServicio)
        {
            DataTable tabla = new DataTable();

            string sql = @"
                SELECT
                    p.idProducto AS idProducto,
                    p.nombreProducto AS producto,
                    p.precioCostoProducto AS precioUnitario,

                    COALESCE(
                        st.stockActualStock,
                        st.stockFisicoStock,
                        0
                    ) AS stockFisico,

                    COALESCE(
                        (
                            SELECT SUM(pt.cantidadProductoTransporte)
                            FROM tblProductoTransporte pt
                            WHERE pt.Fk_Servicio_Transporte_Externo_Codigo = ?
                              AND pt.Fk_Producto_Codigo = p.idProducto
                        ), 0
                    ) AS cantidadGuardadaServicio

                FROM tblProducto p

                LEFT JOIN tblServicioTransporteExterno s
                    ON s.idServicioTransporteExterno = ?

                LEFT JOIN tblTraslado t
                    ON t.idTraslado = s.Fk_Traslado_Codigo

                LEFT JOIN tblBodega b
                    ON b.idBodega = t.Fk_Bodega_Origen_Codigo

                LEFT JOIN tblStock st
                    ON st.Fk_Producto_Codigo = p.idProducto
                   AND st.Fk_Bodega_Codigo = b.idBodega
                   AND st.Fk_Sucursal_Codigo = b.Fk_Sucursal_Codigo

                ORDER BY p.nombreProducto";

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            using (OdbcCommand comando = new OdbcCommand(sql, conexion))
            using (OdbcDataAdapter adaptador = new OdbcDataAdapter(comando))
            {
                // ODBC utiliza el orden de los parámetros.
                comando.Parameters.AddWithValue("@servicio1", idServicio);
                comando.Parameters.AddWithValue("@servicio2", idServicio);

                conexion.Open();
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        // Obtener los productos que ya pertenecen al servicio.
        public DataTable LogisticaFuncObtenerProductosDelServicio(int idServicio)
        {
            DataTable tabla = new DataTable();

            string sql = @"
                SELECT
                    pt.idProductoTransporte AS idProductoTransporte,
                    p.idProducto AS idProducto,
                    p.nombreProducto AS producto,
                    pt.cantidadProductoTransporte AS cantidad,
                    pt.unidadMedidaProductoTransporte AS unidad,
                    p.precioCostoProducto AS precioUnitario,

                    (
                        pt.cantidadProductoTransporte *
                        p.precioCostoProducto
                    ) AS precioTotal,

                    COALESCE(
                        st.stockActualStock,
                        st.stockFisicoStock,
                        0
                    ) AS stockFisico,

                    pt.cantidadProductoTransporte AS cantidadGuardadaServicio

                FROM tblProductoTransporte pt

                INNER JOIN tblProducto p
                    ON p.idProducto = pt.Fk_Producto_Codigo

                INNER JOIN tblServicioTransporteExterno s
                    ON s.idServicioTransporteExterno =
                       pt.Fk_Servicio_Transporte_Externo_Codigo

                LEFT JOIN tblTraslado t
                    ON t.idTraslado = s.Fk_Traslado_Codigo

                LEFT JOIN tblBodega b
                    ON b.idBodega = t.Fk_Bodega_Origen_Codigo

                LEFT JOIN tblStock st
                    ON st.Fk_Producto_Codigo = p.idProducto
                   AND st.Fk_Bodega_Codigo = b.idBodega
                   AND st.Fk_Sucursal_Codigo = b.Fk_Sucursal_Codigo

                WHERE pt.Fk_Servicio_Transporte_Externo_Codigo = ?

                ORDER BY p.nombreProducto";

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            using (OdbcCommand comando = new OdbcCommand(sql, conexion))
            using (OdbcDataAdapter adaptador = new OdbcDataAdapter(comando))
            {
                comando.Parameters.AddWithValue("@idServicio", idServicio);

                conexion.Open();
                adaptador.Fill(tabla);
            }

            return tabla;
        }

        // Obtener la bodega y sucursal de origen del servicio.
        private void LogisticaMetObtenerOrigen(
            int idServicio,
            OdbcConnection conexion,
            OdbcTransaction transaccion,
            out int idBodega,
            out int idSucursal)
        {
            idBodega = 0;
            idSucursal = 0;

            string sql = @"
                SELECT
                    b.idBodega,
                    b.Fk_Sucursal_Codigo
                FROM tblServicioTransporteExterno s
                INNER JOIN tblTraslado t
                    ON t.idTraslado = s.Fk_Traslado_Codigo
                INNER JOIN tblBodega b
                    ON b.idBodega = t.Fk_Bodega_Origen_Codigo
                WHERE s.idServicioTransporteExterno = ?";

            using (OdbcCommand comando =
                new OdbcCommand(sql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@servicio", idServicio);

                using (OdbcDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                    {
                        throw new Exception(
                            "El transporte no tiene una bodega de origen asociada.");
                    }

                    idBodega = Convert.ToInt32(lector["idBodega"]);
                    idSucursal = Convert.ToInt32(
                        lector["Fk_Sucursal_Codigo"]);
                }
            }
        }

        // Sincronizar el saldo de tblSaldos con tblStock.
        private void LogisticaMetSincronizarSaldo(
            int idProducto,
            int idBodega,
            decimal saldo,
            OdbcConnection conexion,
            OdbcTransaction transaccion)
        {
            string sqlExiste = @"
                SELECT COUNT(*)
                FROM tblSaldos
                WHERE Fk_Producto_Codigo = ?
                  AND Fk_Bodega_Codigo = ?";

            int existe;

            using (OdbcCommand comando =
                new OdbcCommand(sqlExiste, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@producto", idProducto);
                comando.Parameters.AddWithValue("@bodega", idBodega);

                existe = Convert.ToInt32(comando.ExecuteScalar());
            }

            if (existe > 0)
            {
                string sqlActualizar = @"
                    UPDATE tblSaldos
                    SET saldoActualSaldo = ?
                    WHERE Fk_Producto_Codigo = ?
                      AND Fk_Bodega_Codigo = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(sqlActualizar, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue("@saldo", saldo);
                    comando.Parameters.AddWithValue("@producto", idProducto);
                    comando.Parameters.AddWithValue("@bodega", idBodega);

                    comando.ExecuteNonQuery();
                }
            }
            else
            {
                string sqlInsertar = @"
                    INSERT INTO tblSaldos
                    (
                        Fk_Producto_Codigo,
                        Fk_Bodega_Codigo,
                        saldoActualSaldo
                    )
                    VALUES (?, ?, ?)";

                using (OdbcCommand comando =
                    new OdbcCommand(sqlInsertar, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue("@producto", idProducto);
                    comando.Parameters.AddWithValue("@bodega", idBodega);
                    comando.Parameters.AddWithValue("@saldo", saldo);

                    comando.ExecuteNonQuery();
                }
            }
        }

        // Actualizar inventario según la diferencia de cantidades.
        private void LogisticaMetActualizarInventario(
            int idProducto,
            int idBodega,
            int idSucursal,
            int diferencia,
            OdbcConnection conexion,
            OdbcTransaction transaccion)
        {
            if (diferencia == 0)
            {
                return;
            }

            int filas;

            if (diferencia > 0)
            {
                // Se necesita descontar inventario.
                string sql = @"
                    UPDATE tblStock
                    SET
                        stockFisicoStock =
                            COALESCE(stockFisicoStock, 0) - ?,
                        stockActualStock =
                            COALESCE(stockActualStock, stockFisicoStock, 0) - ?
                    WHERE Fk_Sucursal_Codigo = ?
                      AND Fk_Bodega_Codigo = ?
                      AND Fk_Producto_Codigo = ?
                      AND COALESCE(
                            stockActualStock,
                            stockFisicoStock,
                            0
                          ) >= ?
                      AND COALESCE(stockFisicoStock, 0) >= ?";

                using (OdbcCommand comando =
                    new OdbcCommand(sql, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue("@cantidadFisica", diferencia);
                    comando.Parameters.AddWithValue("@cantidadActual", diferencia);
                    comando.Parameters.AddWithValue("@sucursal", idSucursal);
                    comando.Parameters.AddWithValue("@bodega", idBodega);
                    comando.Parameters.AddWithValue("@producto", idProducto);
                    comando.Parameters.AddWithValue("@existenciaActual", diferencia);
                    comando.Parameters.AddWithValue("@existenciaFisica", diferencia);

                    filas = comando.ExecuteNonQuery();
                }

                if (filas == 0)
                {
                    throw new Exception(
                        "No existe suficiente inventario para uno de los productos.");
                }
            }
            else
            {
                // Se devuelve al inventario la cantidad reducida o eliminada.
                int cantidadReponer = Math.Abs(diferencia);

                string sql = @"
                    UPDATE tblStock
                    SET
                        stockFisicoStock =
                            COALESCE(stockFisicoStock, 0) + ?,
                        stockActualStock =
                            COALESCE(stockActualStock, stockFisicoStock, 0) + ?
                    WHERE Fk_Sucursal_Codigo = ?
                      AND Fk_Bodega_Codigo = ?
                      AND Fk_Producto_Codigo = ?";

                using (OdbcCommand comando =
                    new OdbcCommand(sql, conexion, transaccion))
                {
                    comando.Parameters.AddWithValue("@cantidadFisica", cantidadReponer);
                    comando.Parameters.AddWithValue("@cantidadActual", cantidadReponer);
                    comando.Parameters.AddWithValue("@sucursal", idSucursal);
                    comando.Parameters.AddWithValue("@bodega", idBodega);
                    comando.Parameters.AddWithValue("@producto", idProducto);

                    filas = comando.ExecuteNonQuery();
                }

                if (filas == 0)
                {
                    throw new Exception(
                        "No se encontró el inventario que debe reponerse.");
                }
            }

            // Consultar el saldo actualizado de tblStock.
            string sqlSaldo = @"
                SELECT
                    COALESCE(
                        stockActualStock,
                        stockFisicoStock,
                        0
                    )
                FROM tblStock
                WHERE Fk_Sucursal_Codigo = ?
                  AND Fk_Bodega_Codigo = ?
                  AND Fk_Producto_Codigo = ?";

            decimal saldoActual;

            using (OdbcCommand comando =
                new OdbcCommand(sqlSaldo, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@sucursal", idSucursal);
                comando.Parameters.AddWithValue("@bodega", idBodega);
                comando.Parameters.AddWithValue("@producto", idProducto);

                object resultado = comando.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    throw new Exception(
                        "No fue posible consultar el inventario actualizado.");
                }

                saldoActual = Convert.ToDecimal(resultado);
            }

            // Mantener sincronizada la tabla de saldos.
            LogisticaMetSincronizarSaldo(
                idProducto,
                idBodega,
                saldoActual,
                conexion,
                transaccion);
        }

        // Guardar productos y actualizar inventario.
        public void LogisticaProcGuardarProductos(
            int idServicio,
            DataTable productos)
        {
            if (productos == null)
            {
                throw new Exception("No se recibieron productos para guardar.");
            }

            using (OdbcConnection conexion = LogisticaMetObtenerConexion())
            {
                conexion.Open();

                using (OdbcTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        int idBodegaOrigen;
                        int idSucursalOrigen;

                        LogisticaMetObtenerOrigen(
                            idServicio,
                            conexion,
                            transaccion,
                            out idBodegaOrigen,
                            out idSucursalOrigen);

                        // Consultar cantidades que ya estaban guardadas.
                        Dictionary<int, int> cantidadesAnteriores =
                            new Dictionary<int, int>();

                        string sqlAnteriores = @"
                            SELECT
                                Fk_Producto_Codigo,
                                SUM(cantidadProductoTransporte)
                            FROM tblProductoTransporte
                            WHERE Fk_Servicio_Transporte_Externo_Codigo = ?
                            GROUP BY Fk_Producto_Codigo";

                        using (OdbcCommand comando =
                            new OdbcCommand(sqlAnteriores, conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@servicio", idServicio);

                            using (OdbcDataReader lector = comando.ExecuteReader())
                            {
                                while (lector.Read())
                                {
                                    int idProducto = Convert.ToInt32(lector[0]);
                                    int cantidad = Convert.ToInt32(
                                        Convert.ToDecimal(lector[1]));

                                    cantidadesAnteriores[idProducto] = cantidad;
                                }
                            }
                        }

                        // Obtener las nuevas cantidades.
                        Dictionary<int, int> cantidadesNuevas =
                            new Dictionary<int, int>();

                        foreach (DataRow fila in productos.Rows)
                        {
                            int idProducto = Convert.ToInt32(fila["idProducto"]);
                            int cantidad = Convert.ToInt32(fila["cantidad"]);

                            if (cantidad <= 0)
                            {
                                throw new Exception(
                                    "Las cantidades deben ser mayores que cero.");
                            }

                            if (cantidadesNuevas.ContainsKey(idProducto))
                            {
                                throw new Exception(
                                    "No se puede agregar el mismo producto más de una vez.");
                            }

                            cantidadesNuevas.Add(idProducto, cantidad);
                        }

                        // Reunir productos anteriores y nuevos.
                        HashSet<int> productosAActualizar = new HashSet<int>();

                        foreach (int id in cantidadesAnteriores.Keys)
                        {
                            productosAActualizar.Add(id);
                        }

                        foreach (int id in cantidadesNuevas.Keys)
                        {
                            productosAActualizar.Add(id);
                        }

                        // Actualizar solamente la diferencia de inventario.
                        foreach (int idProducto in productosAActualizar)
                        {
                            int cantidadAnterior = 0;
                            int cantidadNueva = 0;

                            if (cantidadesAnteriores.ContainsKey(idProducto))
                            {
                                cantidadAnterior = cantidadesAnteriores[idProducto];
                            }

                            if (cantidadesNuevas.ContainsKey(idProducto))
                            {
                                cantidadNueva = cantidadesNuevas[idProducto];
                            }

                            int diferencia = cantidadNueva - cantidadAnterior;

                            LogisticaMetActualizarInventario(
                                idProducto,
                                idBodegaOrigen,
                                idSucursalOrigen,
                                diferencia,
                                conexion,
                                transaccion);
                        }

                        // Eliminar los registros anteriores del servicio.
                        string sqlEliminar = @"
                            DELETE FROM tblProductoTransporte
                            WHERE Fk_Servicio_Transporte_Externo_Codigo = ?";

                        using (OdbcCommand comando =
                            new OdbcCommand(sqlEliminar, conexion, transaccion))
                        {
                            comando.Parameters.AddWithValue("@servicio", idServicio);
                            comando.ExecuteNonQuery();
                        }

                        // Insertar la nueva lista de productos.
                        string sqlInsertar = @"
                            INSERT INTO tblProductoTransporte
                            (
                                Fk_Servicio_Transporte_Externo_Codigo,
                                Fk_Producto_Codigo,
                                cantidadProductoTransporte,
                                unidadMedidaProductoTransporte
                            )
                            VALUES (?, ?, ?, ?)";

                        foreach (DataRow fila in productos.Rows)
                        {
                            using (OdbcCommand comando =
                                new OdbcCommand(sqlInsertar, conexion, transaccion))
                            {
                                comando.Parameters.AddWithValue(
                                    "@servicio", idServicio);

                                comando.Parameters.AddWithValue(
                                    "@producto",
                                    Convert.ToInt32(fila["idProducto"]));

                                comando.Parameters.AddWithValue(
                                    "@cantidad",
                                    Convert.ToInt32(fila["cantidad"]));

                                comando.Parameters.AddWithValue(
                                    "@unidad", "Unidad");

                                comando.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}