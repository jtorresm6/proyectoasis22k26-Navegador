using System;
using System.Collections.Generic;
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

        public void LogisticaMetValidarDisponibilidad(
            IList<ClsProductoTrasladoDetalle> Detalles,
            IDictionary<int, ClsStockProductoTraslado> Stocks,
            IDictionary<int, int> ReservaPropia)
        {
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock;
                if (!Stocks.TryGetValue(Detalle.CodigoProducto, out Stock))
                {
                    throw new InvalidOperationException(
                        "No existe stock del producto " + Detalle.SKU + " en la bodega de origen.");
                }

                int ReservaActual = 0;
                if (ReservaPropia != null)
                {
                    ReservaPropia.TryGetValue(Detalle.CodigoProducto, out ReservaActual);
                }

                int Disponible = Stock.Disponible + ReservaActual;
                if (Disponible < Detalle.Cantidad)
                {
                    throw new InvalidOperationException(
                        "Las existencias disponibles del producto " + Detalle.SKU +
                        " cambiaron. Disponibles: " + Math.Max(0, Disponible) + ".");
                }
            }
        }

        public void LogisticaMetReservar(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            IDictionary<int, ClsStockProductoTraslado> Stocks,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            LogisticaMetAjustarApartados(Conexion, Transaccion, Stocks, Detalles, true);
        }

        public void LogisticaMetLiberarReserva(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            IDictionary<int, ClsStockProductoTraslado> Stocks,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            LogisticaMetAjustarApartados(Conexion, Transaccion, Stocks, Detalles, false);
        }

        public void LogisticaMetSincronizarReservas(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int BodegaAnterior, int BodegaNueva,
            IDictionary<int, int> DetallesAnteriores,
            IList<ClsProductoTrasladoDetalle> DetallesNuevos)
        {
            Dictionary<int, ClsStockProductoTraslado> StocksAnteriores =
                LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, BodegaAnterior);
            if (BodegaAnterior != BodegaNueva)
            {
                LogisticaMetLiberarReserva(
                    Conexion, Transaccion, StocksAnteriores,
                    LogisticaMetCrearDetalles(DetallesAnteriores));
                Dictionary<int, ClsStockProductoTraslado> StocksNuevos =
                    LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, BodegaNueva);
                LogisticaMetValidarDisponibilidad(DetallesNuevos, StocksNuevos, null);
                LogisticaMetReservar(Conexion, Transaccion, StocksNuevos, DetallesNuevos);
                return;
            }

            var Deltas = new List<ClsProductoTrasladoDetalle>();
            var Liberaciones = new List<ClsProductoTrasladoDetalle>();
            var ProductosNuevos = new HashSet<int>();

            foreach (ClsProductoTrasladoDetalle Detalle in DetallesNuevos)
            {
                ProductosNuevos.Add(Detalle.CodigoProducto);
                int CantidadAnterior;
                DetallesAnteriores.TryGetValue(Detalle.CodigoProducto, out CantidadAnterior);
                int Delta = Detalle.Cantidad - CantidadAnterior;
                if (Delta > 0)
                {
                    Deltas.Add(new ClsProductoTrasladoDetalle
                    {
                        CodigoProducto = Detalle.CodigoProducto,
                        SKU = Detalle.SKU,
                        Cantidad = Delta
                    });
                }
                else if (Delta < 0)
                {
                    Liberaciones.Add(new ClsProductoTrasladoDetalle
                    {
                        CodigoProducto = Detalle.CodigoProducto,
                        Cantidad = -Delta
                    });
                }
            }

            foreach (KeyValuePair<int, int> Anterior in DetallesAnteriores)
            {
                if (!ProductosNuevos.Contains(Anterior.Key))
                {
                    Liberaciones.Add(new ClsProductoTrasladoDetalle
                    {
                        CodigoProducto = Anterior.Key,
                        Cantidad = Anterior.Value
                    });
                }
            }

            LogisticaMetValidarDisponibilidad(Deltas, StocksAnteriores, null);
            if (Liberaciones.Count > 0)
            {
                LogisticaMetLiberarReserva(Conexion, Transaccion, StocksAnteriores, Liberaciones);
            }

            if (Deltas.Count > 0)
            {
                LogisticaMetReservar(Conexion, Transaccion, StocksAnteriores, Deltas);
            }
        }

        public void LogisticaMetRegistrarSalida(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Traslado, IList<ClsProductoTrasladoDetalle> Detalles)
        {
            Dictionary<int, ClsStockProductoTraslado> Stocks =
                LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, Traslado.BodegaOrigen);
            LogisticaMetValidarDisponibilidad(Detalles, Stocks, LogisticaMetCrearCantidades(Detalles));

            int TipoSalida = LogisticaMetObtenerTipoMovimiento(Conexion, Transaccion, '-');
            int CodigoMovimiento = LogisticaMetInsertarMovimiento(
                Conexion, Transaccion, TipoSalida, Traslado, "Salida por traslado " + Traslado.CodigoTraslado);

            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock = Stocks[Detalle.CodigoProducto];
                int NuevoActual = Stock.StockActual - Detalle.Cantidad;
                int NuevoApartado = Math.Max(0, Stock.Apartados - Detalle.Cantidad);
                LogisticaMetActualizarStock(
                    Conexion, Transaccion, Stock.IdStock, Stock.StockActual,
                    NuevoActual, NuevoApartado, 0, Detalle.Cantidad);
                Stock.StockActual = NuevoActual;
                Stock.Apartados = NuevoApartado;
                LogisticaMetSincronizarSaldo(
                    Conexion, Transaccion, Traslado.BodegaOrigen,
                    Detalle.CodigoProducto, NuevoActual);
                LogisticaMetInsertarDetalleMovimiento(
                    Conexion, Transaccion, CodigoMovimiento, Detalle);
            }
        }

        public void LogisticaMetRegistrarEntrada(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Traslado, IList<ClsProductoTrasladoDetalle> Detalles)
        {
            Dictionary<int, ClsStockProductoTraslado> Origen =
                LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, Traslado.BodegaOrigen);
            Dictionary<int, ClsStockProductoTraslado> Destino =
                LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, Traslado.BodegaDestino);
            int SucursalDestino = LogisticaMetObtenerSucursalBodega(
                Conexion, Transaccion, Traslado.BodegaDestino);
            int TipoEntrada = LogisticaMetObtenerTipoMovimiento(Conexion, Transaccion, '+');
            int CodigoMovimiento = LogisticaMetInsertarMovimiento(
                Conexion, Transaccion, TipoEntrada, Traslado, "Entrada por traslado " + Traslado.CodigoTraslado);

            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado StockDestino;
                if (!Destino.TryGetValue(Detalle.CodigoProducto, out StockDestino))
                {
                    ClsStockProductoTraslado StockOrigen;
                    bool EstadoVenta = true;
                    bool EstadoCompra = true;
                    if (Origen.TryGetValue(Detalle.CodigoProducto, out StockOrigen))
                    {
                        EstadoVenta = StockOrigen.EstadoVenta;
                        EstadoCompra = StockOrigen.EstadoCompra;
                    }

                    int IdStock = LogisticaMetInsertarStockDestino(
                        Conexion, Transaccion, SucursalDestino, Traslado.BodegaDestino,
                        Detalle.CodigoProducto, Detalle.Cantidad, EstadoVenta, EstadoCompra);
                    StockDestino = new ClsStockProductoTraslado
                    {
                        IdStock = IdStock,
                        CodigoProducto = Detalle.CodigoProducto,
                        CodigoSucursal = SucursalDestino,
                        StockActual = Detalle.Cantidad,
                        Apartados = 0,
                        EstadoVenta = EstadoVenta,
                        EstadoCompra = EstadoCompra
                    };
                    Destino.Add(Detalle.CodigoProducto, StockDestino);
                }
                else
                {
                    int NuevoActual = StockDestino.StockActual + Detalle.Cantidad;
                    LogisticaMetActualizarStock(
                        Conexion, Transaccion, StockDestino.IdStock, StockDestino.StockActual,
                        NuevoActual, StockDestino.Apartados, Detalle.Cantidad, 0);
                    StockDestino.StockActual = NuevoActual;
                }

                LogisticaMetSincronizarSaldo(
                    Conexion, Transaccion, Traslado.BodegaDestino,
                    Detalle.CodigoProducto, StockDestino.StockActual);
                object LoteSalida = LogisticaMetObtenerLoteMovimientoSalida(
                    Conexion, Transaccion, Traslado.CodigoInterno, Detalle.CodigoProducto);
                LogisticaMetInsertarDetalleMovimiento(
                    Conexion, Transaccion, CodigoMovimiento, Detalle, LoteSalida);
            }
        }

        public void LogisticaMetAnularSalida(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Traslado, IList<ClsProductoTrasladoDetalle> Detalles)
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
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, Traslado.CodigoInterno));

            Dictionary<int, ClsStockProductoTraslado> Stocks =
                LogisticaMetObtenerStocksBloqueados(Conexion, Transaccion, Traslado.BodegaOrigen);
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock;
                if (!Stocks.TryGetValue(Detalle.CodigoProducto, out Stock))
                {
                    throw new InvalidOperationException(
                        "No existe stock del producto " + Detalle.CodigoProducto +
                        " para revertir la salida del traslado.");
                }

                int NuevoActual = Stock.StockActual + Detalle.Cantidad;
                LogisticaMetActualizarStock(
                    Conexion, Transaccion, Stock.IdStock, Stock.StockActual,
                    NuevoActual, Stock.Apartados, 0, -Detalle.Cantidad);
                Stock.StockActual = NuevoActual;
                LogisticaMetSincronizarSaldo(
                    Conexion, Transaccion, Traslado.BodegaOrigen,
                    Detalle.CodigoProducto, NuevoActual);
            }
        }

        private void LogisticaMetAjustarApartados(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            IDictionary<int, ClsStockProductoTraslado> Stocks,
            IList<ClsProductoTrasladoDetalle> Detalles, bool Reservar)
        {
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock;
                if (!Stocks.TryGetValue(Detalle.CodigoProducto, out Stock))
                {
                    throw new InvalidOperationException(
                        "No existe stock del producto " + Detalle.CodigoProducto +
                        " en la bodega de origen.");
                }

                int NuevoApartado = Reservar
                    ? Stock.Apartados + Detalle.Cantidad
                    : Math.Max(0, Stock.Apartados - Detalle.Cantidad);
                if (Reservar && NuevoApartado > Stock.StockActual)
                {
                    throw new InvalidOperationException(
                        "No hay existencias suficientes para reservar el producto " +
                        Detalle.SKU + ".");
                }

                int NuevoVirtual = Stock.StockActual - NuevoApartado;
                LogisticaMetEjecutarNoConsulta(
                    Conexion, Transaccion,
                    "UPDATE tblStock SET apartadosStock = ?, stockVirtualStock = ? WHERE idStock = ?",
                    LogisticaMetCrearParametro("Apartados", OdbcType.Int, NuevoApartado),
                    LogisticaMetCrearParametro("Virtual", OdbcType.Int, NuevoVirtual),
                    LogisticaMetCrearParametro("IdStock", OdbcType.Int, Stock.IdStock));
                Stock.Apartados = NuevoApartado;
            }
        }

        private void LogisticaMetActualizarStock(
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
            if (LogisticaMetEjecutarNoConsulta(
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
                LogisticaMetCrearParametro("IdStock", OdbcType.Int, IdStock)) != 1)
            {
                throw new InvalidOperationException("No fue posible actualizar el stock del traslado.");
            }
        }

        private int LogisticaMetInsertarStockDestino(
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

        private void LogisticaMetSincronizarSaldo(
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

        private int LogisticaMetInsertarMovimiento(
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

        private void LogisticaMetInsertarDetalleMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoMovimiento, ClsProductoTrasladoDetalle Detalle)
        {
            LogisticaMetInsertarDetalleMovimiento(
                Conexion, Transaccion, CodigoMovimiento, Detalle, null);
        }

        private void LogisticaMetInsertarDetalleMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoMovimiento, ClsProductoTrasladoDetalle Detalle, object LotePreferido)
        {
            decimal Costo = LogisticaMetObtenerCostoProducto(
                Conexion, Transaccion, Detalle.CodigoProducto);
            object Lote = LotePreferido == null || LotePreferido == DBNull.Value
                ? LogisticaMetObtenerLoteProducto(Conexion, Transaccion, Detalle.CodigoProducto)
                : LotePreferido;
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

        private int LogisticaMetObtenerTipoMovimiento(
            OdbcConnection Conexion, OdbcTransaction Transaccion, char Signo)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT idTipoMovimiento FROM tblTipoMovimiento " +
                "WHERE signoTipoMovimiento = ? AND nombreTipoMovimiento LIKE '%traslado%' " +
                "ORDER BY idTipoMovimiento LIMIT 1",
                LogisticaMetCrearParametro("Signo", OdbcType.Char, Signo.ToString()));
            if (Resultado == null)
            {
                throw new InvalidOperationException(
                    "Debe existir un tipo de movimiento de traslado con signo '" + Signo + "'.");
            }

            return Convert.ToInt32(Resultado);
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

        private object LogisticaMetObtenerLoteMovimientoSalida(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTraslado, int CodigoProducto)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT detalle.Fk_Lote_Producto_Codigo " +
                "FROM tblMovimientoInventario movimiento " +
                "INNER JOIN tblTipoMovimiento tipo ON tipo.idTipoMovimiento = " +
                "movimiento.Fk_Tipo_Movimiento_Codigo " +
                "INNER JOIN tblDetalleMovimiento detalle ON " +
                "detalle.Fk_Movimiento_Inventario_Codigo = movimiento.idMovimientoInventario " +
                "WHERE movimiento.Fk_Traslado_Codigo = ? " +
                "AND tipo.signoTipoMovimiento = '-' " +
                "AND movimiento.estadoMovimientoInventario <> 'ANULADO' " +
                "AND detalle.Fk_Producto_Codigo = ? " +
                "ORDER BY movimiento.idMovimientoInventario DESC LIMIT 1",
                LogisticaMetCrearParametro("Traslado", OdbcType.Int, CodigoTraslado),
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto));
            return Resultado ?? DBNull.Value;
        }

        public object LogisticaMetObtenerLoteProducto(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoProducto)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT idLoteProducto FROM tblLoteProducto WHERE Fk_Producto_Codigo = ? " +
                "ORDER BY fechaVencimientoLoteProducto, idLoteProducto LIMIT 1",
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto));
            return Resultado ?? DBNull.Value;
        }

        public bool LogisticaMetProductoRequiereFrio(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoProducto)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT 1 FROM tblLoteProducto WHERE Fk_Producto_Codigo = ? " +
                "AND requiereFrioLoteProducto = 1 LIMIT 1",
                LogisticaMetCrearParametro("Producto", OdbcType.Int, CodigoProducto));
            return Resultado != null;
        }

        private int LogisticaMetObtenerSucursalBodega(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoBodega)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT Fk_Sucursal_Codigo FROM tblBodega WHERE idBodega = ?",
                LogisticaMetCrearParametro("Bodega", OdbcType.Int, CodigoBodega));
            if (Resultado == null)
            {
                throw new InvalidOperationException(
                    "No fue posible obtener la sucursal de la bodega de destino.");
            }

            return Convert.ToInt32(Resultado);
        }

        private static IList<ClsProductoTrasladoDetalle> LogisticaMetCrearDetalles(
            IDictionary<int, int> Cantidades)
        {
            var Detalles = new List<ClsProductoTrasladoDetalle>();
            foreach (KeyValuePair<int, int> Par in Cantidades)
            {
                Detalles.Add(new ClsProductoTrasladoDetalle
                {
                    CodigoProducto = Par.Key,
                    Cantidad = Par.Value
                });
            }

            return Detalles;
        }

        private static Dictionary<int, int> LogisticaMetCrearCantidades(
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            var Cantidades = new Dictionary<int, int>();
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                Cantidades[Detalle.CodigoProducto] = Detalle.Cantidad;
            }

            return Cantidades;
        }
    }
}
