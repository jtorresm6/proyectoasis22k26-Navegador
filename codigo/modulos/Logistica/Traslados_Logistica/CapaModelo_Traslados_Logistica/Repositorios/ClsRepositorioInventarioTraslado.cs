using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Repositorio encargado de reservar, despachar y recibir el
 * inventario asociado a un traslado entre bodegas.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Repositorios
{
    public class ClsRepositorioInventarioTraslado : ClsSentencias
    {
        public Dictionary<int, ClsStockProductoTraslado> LogisticaMetObtenerStocksBloqueados(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoBodega)
        {
            const string Consulta =
                "SELECT idStock, Fk_Producto_Codigo, Fk_Sucursal_Codigo, " +
                "stockActualStock, apartadosStock, estadoVentaStock, estadoCompraStock " +
                "FROM tblStock WHERE Fk_Bodega_Codigo = ? ORDER BY Fk_Producto_Codigo FOR UPDATE";
            var Stocks = new Dictionary<int, ClsStockProductoTraslado>();

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoBodega", OdbcType.Int).Value = CodigoBodega;
                using (var Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        var Stock = new ClsStockProductoTraslado
                        {
                            IdStock = Convert.ToInt32(Lector[0]),
                            CodigoProducto = Convert.ToInt32(Lector[1]),
                            CodigoSucursal = Convert.ToInt32(Lector[2]),
                            StockActual = Convert.ToInt32(Lector[3]),
                            Apartados = Convert.ToInt32(Lector[4]),
                            EstadoVenta = Convert.ToBoolean(Lector[5]),
                            EstadoCompra = Convert.ToBoolean(Lector[6])
                        };
                        Stocks.Add(Stock.CodigoProducto, Stock);
                    }
                }
            }

            return Stocks;
        }

        public int LogisticaMetActualizarApartados(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int IdStock, int Apartados, int Virtual)
        {
            return LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblStock SET apartadosStock = ?, stockVirtualStock = ? WHERE idStock = ?",
                LogisticaMetCrearParametro("Apartados", OdbcType.Int, Apartados),
                LogisticaMetCrearParametro("Virtual", OdbcType.Int, Virtual),
                LogisticaMetCrearParametro("IdStock", OdbcType.Int, IdStock));
        }

        public void LogisticaMetAnularMovimientosSalida(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblMovimientoInventario movimiento " +
                "INNER JOIN tblTipoMovimiento tipo ON tipo.idTipoMovimiento = " +
                "movimiento.Fk_Tipo_Movimiento_Codigo " +
                "SET movimiento.estadoMovimientoInventario = 'ANULADO' " +
                "WHERE movimiento.Fk_Traslado_Codigo = ? " +
                "AND tipo.signoTipoMovimiento = '-' " +
                "AND movimiento.estadoMovimientoInventario = 'PENDIENTE'",
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, CodigoTraslado));
        }

        public int LogisticaMetActualizarStock(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int IdStock,
            int StockAnterior, int StockActual, int Apartados, int Cargos, int Abonos)
        {
            int Virtual = StockActual - Apartados;
            const string Consulta =
                "UPDATE tblStock SET stockAnteriorStock = ?, stockActualStock = ?, " +
                "stockVirtualStock = ?, apartadosStock = ?, " +
                "cargosMesStock = cargosMesStock + ?, " +
                "cargosAcumuladosStock = cargosAcumuladosStock + ?, " +
                "abonosMesStock = CASE WHEN abonosMesStock + ? < 0 THEN 0 ELSE abonosMesStock + ? END, " +
                "abonosAcumuladosStock = CASE WHEN abonosAcumuladosStock + ? < 0 THEN 0 " +
                "ELSE abonosAcumuladosStock + ? END " +
                "WHERE idStock = ?";
            return LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("Anterior", OdbcType.Int, StockAnterior),
                LogisticaMetCrearParametro("Actual", OdbcType.Int, StockActual),
                LogisticaMetCrearParametro("Virtual", OdbcType.Int, Virtual),
                LogisticaMetCrearParametro("Apartados", OdbcType.Int, Apartados),
                LogisticaMetCrearParametro("CargosMes", OdbcType.Int, Cargos),
                LogisticaMetCrearParametro("CargosAcum", OdbcType.Int, Cargos),
                LogisticaMetCrearParametro("AbonosMes", OdbcType.Int, Abonos),
                LogisticaMetCrearParametro("AbonosMes2", OdbcType.Int, Abonos),
                LogisticaMetCrearParametro("AbonosAcum", OdbcType.Int, Abonos),
                LogisticaMetCrearParametro("AbonosAcum2", OdbcType.Int, Abonos),
                LogisticaMetCrearParametro("IdStock", OdbcType.Int, IdStock));
        }

        public int LogisticaMetInsertarStockDestino(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoSucursal, int CodigoBodega, int CodigoProducto, int Cantidad,
            bool EstadoVenta, bool EstadoCompra)
        {
            const string Consulta =
                "INSERT INTO tblStock (Fk_Sucursal_Codigo, Fk_Bodega_Codigo, Fk_Producto_Codigo, " +
                "stockFisicoStock, stockVirtualStock, stockAnteriorStock, stockActualStock, " +
                "cargosMesStock, abonosMesStock, cargosAcumuladosStock, abonosAcumuladosStock, " +
                "apartadosStock, estadoVentaStock, estadoCompraStock) " +
                "VALUES (?, ?, ?, 0, ?, 0, ?, ?, 0, ?, 0, 0, ?, ?)";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("Sucursal", OdbcType.Int, CodigoSucursal),
                LogisticaMetCrearParametro("Bodega", OdbcType.Int, CodigoBodega),
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto),
                LogisticaMetCrearParametro("Virtual", OdbcType.Int, Cantidad),
                LogisticaMetCrearParametro("Actual", OdbcType.Int, Cantidad),
                LogisticaMetCrearParametro("CargosMes", OdbcType.Int, Cantidad),
                LogisticaMetCrearParametro("CargosAcum", OdbcType.Int, Cantidad),
                LogisticaMetCrearParametro("Venta", OdbcType.TinyInt, EstadoVenta ? 1 : 0),
                LogisticaMetCrearParametro("Compra", OdbcType.TinyInt, EstadoCompra ? 1 : 0));
            return LogisticaMetObtenerUltimoId(Conexion, Transaccion);
        }

        public void LogisticaMetSincronizarSaldo(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoBodega, int CodigoProducto, int SaldoActual)
        {
            int Actualizados = LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblSaldos SET saldoActualSaldo = ? " +
                "WHERE Fk_Bodega_Codigo = ? AND Fk_Producto_Codigo = ?",
                LogisticaMetCrearParametro("Saldo", OdbcType.Int, SaldoActual),
                LogisticaMetCrearParametro("Bodega", OdbcType.Int, CodigoBodega),
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto));
            if (Actualizados == 0)
            {
                LogisticaMetEjecutarNoConsulta(
                    Conexion, Transaccion,
                    "INSERT INTO tblSaldos (Fk_Producto_Codigo, Fk_Bodega_Codigo, saldoActualSaldo) " +
                    "VALUES (?, ?, ?)",
                    LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto),
                    LogisticaMetCrearParametro("Bodega", OdbcType.Int, CodigoBodega),
                    LogisticaMetCrearParametro("Saldo", OdbcType.Int, SaldoActual));
            }
        }

        public int LogisticaMetInsertarMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int TipoMovimiento,
            ClsTrasladoPersistido Traslado, string Observaciones)
        {
            const string Consulta =
                "INSERT INTO tblMovimientoInventario " +
                "(Fk_Tipo_Movimiento_Codigo, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, " +
                "Fk_Transporte_Codigo, Fk_Traslado_Codigo, fechaMovimientoInventario, " +
                "observacionesMovimientoInventario, estadoMovimientoInventario) " +
                "VALUES (?, ?, ?, ?, ?, CURRENT_TIMESTAMP, ?, 'PENDIENTE')";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("Tipo", OdbcType.Int, TipoMovimiento),
                LogisticaMetCrearParametro("Origen", OdbcType.Int, Traslado.BodegaOrigen),
                LogisticaMetCrearParametro("Destino", OdbcType.Int, Traslado.BodegaDestino),
                LogisticaMetCrearParametroEnteroOpcional("Transporte", Traslado.CodigoTransporte),
                LogisticaMetCrearParametro("Traslado", OdbcType.Int, Traslado.CodigoInterno),
                LogisticaMetCrearParametro("Observaciones", OdbcType.VarChar, 500, Observaciones));
            return LogisticaMetObtenerUltimoId(Conexion, Transaccion);
        }

        public void LogisticaMetInsertarDetalleMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoMovimiento, ClsProductoTrasladoDetalle Detalle, object LotePreferido)
        {
            decimal Costo = LogisticaMetObtenerCostoProducto(
                Conexion, Transaccion, Detalle.CodigoProducto);
            object Lote = LotePreferido ?? (object)DBNull.Value;
            const string Consulta =
                "INSERT INTO tblDetalleMovimiento " +
                "(Fk_Movimiento_Inventario_Codigo, Fk_Lote_Producto_Codigo, Fk_Producto_Codigo, " +
                "cantidadDetalleMovimiento, precioCostoDetalleMovimiento) " +
                "VALUES (?, ?, ?, ?, ?)";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("Movimiento", OdbcType.Int, CodigoMovimiento),
                new OdbcParameter("Lote", OdbcType.BigInt) { Value = Lote },
                LogisticaMetCrearParametro("Producto", OdbcType.Int, Detalle.CodigoProducto),
                LogisticaMetCrearParametro("Cantidad", OdbcType.Int, Detalle.Cantidad),
                LogisticaMetCrearParametro("Costo", OdbcType.Decimal, Costo));
        }

        public DataTable LogisticaMetObtenerTiposMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion)
        {
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(
                "SELECT idTipoMovimiento AS CodigoTipoMovimiento, " +
                "signoTipoMovimiento AS Signo, nombreTipoMovimiento AS Nombre " +
                "FROM tblTipoMovimiento", Conexion, Transaccion))
            using (var Lector = Comando.ExecuteReader())
            {
                Tabla.Load(Lector);
            }

            return Tabla;
        }

        private decimal LogisticaMetObtenerCostoProducto(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoProducto)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT precioCostoProducto FROM tblProducto WHERE idProducto = ?",
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto));
            return Resultado == null ? 0 : Convert.ToDecimal(Resultado);
        }

        public DataTable LogisticaMetObtenerLotesMovimientosTraslado(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTraslado, int CodigoProducto)
        {
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(
                "SELECT movimiento.idMovimientoInventario AS CodigoMovimiento, " +
                "tipo.signoTipoMovimiento AS Signo, " +
                "UPPER(TRIM(movimiento.estadoMovimientoInventario)) AS EstadoMovimiento, " +
                "detalle.Fk_Lote_Producto_Codigo AS CodigoLote " +
                "FROM tblMovimientoInventario movimiento " +
                "INNER JOIN tblTipoMovimiento tipo ON tipo.idTipoMovimiento = " +
                "movimiento.Fk_Tipo_Movimiento_Codigo " +
                "INNER JOIN tblDetalleMovimiento detalle ON " +
                "detalle.Fk_Movimiento_Inventario_Codigo = movimiento.idMovimientoInventario " +
                "WHERE movimiento.Fk_Traslado_Codigo = ? " +
                "AND detalle.Fk_Producto_Codigo = ? " +
                "ORDER BY movimiento.idMovimientoInventario DESC", Conexion, Transaccion))
            {
                Comando.Parameters.Add("Traslado", OdbcType.Int).Value = CodigoTraslado;
                Comando.Parameters.Add("Producto", OdbcType.Int).Value = CodigoProducto;
                using (var Lector = Comando.ExecuteReader())
                {
                    Tabla.Load(Lector);
                }
            }

            return Tabla;
        }

        public DataTable LogisticaMetObtenerLotesProducto(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoProducto)
        {
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(
                "SELECT idLoteProducto AS CodigoLote, fechaVencimientoLoteProducto AS FechaVencimiento " +
                "FROM tblLoteProducto WHERE Fk_Producto_Codigo = ?", Conexion, Transaccion))
            {
                Comando.Parameters.Add("Producto", OdbcType.Int).Value = CodigoProducto;
                using (var Lector = Comando.ExecuteReader())
                {
                    Tabla.Load(Lector);
                }
            }

            return Tabla;
        }

        public DataTable LogisticaMetObtenerRefrigeracionLotesProducto(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoProducto)
        {
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(
                "SELECT requiereFrioLoteProducto AS RequiereFrio " +
                "FROM tblLoteProducto WHERE Fk_Producto_Codigo = ?", Conexion, Transaccion))
            {
                Comando.Parameters.Add("Producto", OdbcType.Int).Value = CodigoProducto;
                using (var Lector = Comando.ExecuteReader())
                {
                    Tabla.Load(Lector);
                }
            }

            return Tabla;
        }

        public int LogisticaMetObtenerSucursalBodega(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoBodega)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT Fk_Sucursal_Codigo FROM tblBodega WHERE idBodega = ?",
                LogisticaMetCrearParametro("Bodega", OdbcType.Int, CodigoBodega));
            return Resultado == null ? 0 : Convert.ToInt32(Resultado);
        }

    }
}
