using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Traslados_Logistica.Contratos;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Coordina reservas, movimientos de inventario y viajes.
 * ===================================================================
 */

namespace CapaControlador_Traslados_Logistica
{
    public partial class ClsModeloTraslado
    {
        private static void LogisticaMetCambiarEstadoTransporte(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoTransporte, string EstadoNuevo)
        {
            if (CodigoTransporte <= 0)
            {
                return;
            }

            if (UnidadTrabajo.LogisticaMetCambiarEstadoTransporte(
                CodigoTransporte, EstadoNuevo) == 0)
            {
                throw new InvalidOperationException(
                    "Debe existir un transporte y el estado '" + EstadoNuevo +
                    "' en la base de datos para actualizar su disponibilidad.");
            }
        }

        private static void LogisticaMetActualizarViajeActivo(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoTransporte,
            string Estado, bool RegistrarLlegada)
        {
            if (CodigoTransporte > 0)
            {
                UnidadTrabajo.LogisticaMetActualizarViajeActivo(
                    CodigoTransporte, Estado, RegistrarLlegada);
            }
        }

        private static int LogisticaMetObtenerTipoMovimiento(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, char Signo)
        {
            DataTable TiposMovimiento = UnidadTrabajo.LogisticaMetObtenerTiposMovimiento();
            int CodigoTipoMovimiento = 0;
            foreach (DataRow Tipo in TiposMovimiento.Rows)
            {
                if (Convert.ToString(Tipo["Signo"]) == Signo.ToString() &&
                    Convert.ToString(Tipo["Nombre"]).IndexOf(
                        "traslado", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    int CodigoCandidato = Convert.ToInt32(Tipo["CodigoTipoMovimiento"]);
                    if (CodigoTipoMovimiento == 0 || CodigoCandidato < CodigoTipoMovimiento)
                    {
                        CodigoTipoMovimiento = CodigoCandidato;
                    }
                }
            }

            if (CodigoTipoMovimiento == 0)
            {
                throw new InvalidOperationException(
                    "Debe existir un tipo de movimiento de traslado con signo '" + Signo + "'.");
            }

            return CodigoTipoMovimiento;
        }

        private static object LogisticaMetObtenerLoteMovimientoSalida(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoTraslado, int CodigoProducto)
        {
            DataTable Lotes = UnidadTrabajo.LogisticaMetObtenerLotesMovimientosTraslado(
                CodigoTraslado, CodigoProducto);
            foreach (DataRow Lote in Lotes.Rows)
            {
                if (Convert.ToString(Lote["Signo"]) == "-" &&
                    !string.Equals(Convert.ToString(Lote["EstadoMovimiento"]),
                        "ANULADO", StringComparison.OrdinalIgnoreCase))
                {
                    return Lote.IsNull("CodigoLote") ? DBNull.Value : Lote["CodigoLote"];
                }
            }

            return DBNull.Value;
        }

        private static object LogisticaMetObtenerLoteProducto(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoProducto)
        {
            DataTable Lotes = UnidadTrabajo.LogisticaMetObtenerLotesProducto(CodigoProducto);
            DataRow LoteSeleccionado = null;
            foreach (DataRow Lote in Lotes.Rows)
            {
                if (LoteSeleccionado == null ||
                    Convert.ToDateTime(Lote["FechaVencimiento"]) <
                    Convert.ToDateTime(LoteSeleccionado["FechaVencimiento"]) ||
                    (Convert.ToDateTime(Lote["FechaVencimiento"]) ==
                    Convert.ToDateTime(LoteSeleccionado["FechaVencimiento"]) &&
                    Convert.ToInt64(Lote["CodigoLote"]) <
                    Convert.ToInt64(LoteSeleccionado["CodigoLote"])))
                {
                    LoteSeleccionado = Lote;
                }
            }

            return LoteSeleccionado == null ? DBNull.Value : LoteSeleccionado["CodigoLote"];
        }

        private static int LogisticaMetObtenerSucursalBodega(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoBodega)
        {
            int CodigoSucursal = UnidadTrabajo.LogisticaMetObtenerSucursalBodega(CodigoBodega);
            if (CodigoSucursal <= 0)
            {
                throw new InvalidOperationException(
                    "No fue posible obtener la sucursal de la bodega de destino.");
            }

            return CodigoSucursal;
        }

        private static void LogisticaMetActualizarStock(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int IdStock, int StockAnterior,
            int StockActual, int Apartados, int Cargos, int Abonos)
        {
            if (UnidadTrabajo.LogisticaMetActualizarStock(
                IdStock, StockAnterior, StockActual, Apartados, Cargos, Abonos) != 1)
            {
                throw new InvalidOperationException("No fue posible actualizar el stock del traslado.");
            }
        }

        private static void LogisticaMetActualizarApartados(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int IdStock, int Apartados, int Virtual)
        {
            if (UnidadTrabajo.LogisticaMetActualizarApartados(IdStock, Apartados, Virtual) != 1)
            {
                throw new InvalidOperationException(
                    "No fue posible actualizar las reservas del stock del traslado.");
            }
        }

        private void LogisticaMetSincronizarReservas(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int BodegaAnterior, int BodegaNueva,
            IDictionary<int, int> DetallesAnteriores,
            IList<ClsProductoTrasladoDetalle> DetallesNuevos)
        {
            Dictionary<int, ClsStockProductoTraslado> StocksAnteriores =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(BodegaAnterior);
            if (BodegaAnterior != BodegaNueva)
            {
                LogisticaMetActualizarReservas(UnidadTrabajo, StocksAnteriores,
                    LogisticaMetCrearDetalles(DetallesAnteriores), false);
                Dictionary<int, ClsStockProductoTraslado> StocksNuevos =
                    UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(BodegaNueva);
                LogisticaMetValidarDisponibilidad(DetallesNuevos, StocksNuevos, null);
                LogisticaMetActualizarReservas(UnidadTrabajo, StocksNuevos, DetallesNuevos, true);
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
            LogisticaMetActualizarReservas(UnidadTrabajo, StocksAnteriores, Liberaciones, false);
            LogisticaMetActualizarReservas(UnidadTrabajo, StocksAnteriores, Deltas, true);
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

        private void LogisticaMetActualizarReservas(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo,
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

                int ApartadosNuevos = Reservar
                    ? Stock.Apartados + Detalle.Cantidad
                    : Math.Max(0, Stock.Apartados - Detalle.Cantidad);
                if (Reservar && ApartadosNuevos > Stock.StockActual)
                {
                    throw new InvalidOperationException(
                        "No hay existencias suficientes para reservar el producto " +
                        Detalle.SKU + ".");
                }

                LogisticaMetActualizarApartados(UnidadTrabajo, Stock.IdStock,
                    ApartadosNuevos, Stock.StockActual - ApartadosNuevos);
                Stock.Apartados = ApartadosNuevos;
            }
        }

        private void LogisticaMetRegistrarSalida(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Traslado,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            Dictionary<int, ClsStockProductoTraslado> Stocks =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Traslado.BodegaOrigen);
            LogisticaMetValidarDisponibilidad(Detalles, Stocks,
                LogisticaMetCrearCantidades(Detalles));
            int TipoMovimiento = LogisticaMetObtenerTipoMovimiento(UnidadTrabajo, '-');
            int CodigoMovimiento = UnidadTrabajo.LogisticaMetInsertarMovimiento(
                TipoMovimiento, Traslado, "Salida por traslado " + Traslado.CodigoTraslado);

            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock = Stocks[Detalle.CodigoProducto];
                int ActualNuevo = Stock.StockActual - Detalle.Cantidad;
                int ApartadosNuevos = Math.Max(0, Stock.Apartados - Detalle.Cantidad);
                LogisticaMetActualizarStock(UnidadTrabajo,
                    Stock.IdStock, Stock.StockActual, ActualNuevo, ApartadosNuevos,
                    0, Detalle.Cantidad);
                UnidadTrabajo.LogisticaMetActualizarSaldo(
                    Traslado.BodegaOrigen, Detalle.CodigoProducto, ActualNuevo);
                object Lote = LogisticaMetObtenerLoteProducto(
                    UnidadTrabajo, Detalle.CodigoProducto);
                UnidadTrabajo.LogisticaMetInsertarDetalleMovimiento(
                    CodigoMovimiento, Detalle, Lote);
                Stock.StockActual = ActualNuevo;
                Stock.Apartados = ApartadosNuevos;
            }
        }

        private void LogisticaMetRegistrarEntrada(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Traslado,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            Dictionary<int, ClsStockProductoTraslado> Origen =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Traslado.BodegaOrigen);
            Dictionary<int, ClsStockProductoTraslado> Destino =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Traslado.BodegaDestino);
            int SucursalDestino = LogisticaMetObtenerSucursalBodega(UnidadTrabajo,
                Traslado.BodegaDestino);
            int TipoMovimiento = LogisticaMetObtenerTipoMovimiento(UnidadTrabajo, '+');
            int CodigoMovimiento = UnidadTrabajo.LogisticaMetInsertarMovimiento(
                TipoMovimiento, Traslado, "Entrada por traslado " + Traslado.CodigoTraslado);

            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado StockDestino;
                if (!Destino.TryGetValue(Detalle.CodigoProducto, out StockDestino))
                {
                    ClsStockProductoTraslado StockOrigen;
                    bool EstadoVenta = Origen.TryGetValue(Detalle.CodigoProducto, out StockOrigen)
                        ? StockOrigen.EstadoVenta : true;
                    bool EstadoCompra = StockOrigen == null ? true : StockOrigen.EstadoCompra;
                    int IdStock = UnidadTrabajo.LogisticaMetInsertarStockDestino(
                        SucursalDestino, Traslado.BodegaDestino, Detalle.CodigoProducto,
                        Detalle.Cantidad, EstadoVenta, EstadoCompra);
                    StockDestino = new ClsStockProductoTraslado
                    {
                        IdStock = IdStock,
                        CodigoProducto = Detalle.CodigoProducto,
                        CodigoSucursal = SucursalDestino,
                        StockActual = Detalle.Cantidad,
                        EstadoVenta = EstadoVenta,
                        EstadoCompra = EstadoCompra
                    };
                    Destino.Add(Detalle.CodigoProducto, StockDestino);
                }
                else
                {
                    int ActualNuevo = StockDestino.StockActual + Detalle.Cantidad;
                    LogisticaMetActualizarStock(UnidadTrabajo,
                        StockDestino.IdStock, StockDestino.StockActual, ActualNuevo,
                        StockDestino.Apartados, Detalle.Cantidad, 0);
                    StockDestino.StockActual = ActualNuevo;
                }

                UnidadTrabajo.LogisticaMetActualizarSaldo(
                    Traslado.BodegaDestino, Detalle.CodigoProducto, StockDestino.StockActual);
                object LoteSalida = LogisticaMetObtenerLoteMovimientoSalida(
                    UnidadTrabajo, Traslado.CodigoInterno, Detalle.CodigoProducto);
                UnidadTrabajo.LogisticaMetInsertarDetalleMovimiento(
                    CodigoMovimiento, Detalle, LoteSalida);
            }
        }

        private void LogisticaMetAnularSalida(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Traslado,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            UnidadTrabajo.LogisticaMetAnularMovimientosSalida(Traslado.CodigoInterno);
            Dictionary<int, ClsStockProductoTraslado> Stocks =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Traslado.BodegaOrigen);
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                ClsStockProductoTraslado Stock;
                if (!Stocks.TryGetValue(Detalle.CodigoProducto, out Stock))
                {
                    throw new InvalidOperationException(
                        "No existe stock del producto " + Detalle.CodigoProducto +
                        " para revertir la salida del traslado.");
                }

                int ActualNuevo = Stock.StockActual + Detalle.Cantidad;
                LogisticaMetActualizarStock(UnidadTrabajo,
                    Stock.IdStock, Stock.StockActual, ActualNuevo, Stock.Apartados,
                    0, -Detalle.Cantidad);
                UnidadTrabajo.LogisticaMetActualizarSaldo(
                    Traslado.BodegaOrigen, Detalle.CodigoProducto, ActualNuevo);
                Stock.StockActual = ActualNuevo;
            }
        }

        private void LogisticaMetRegistrarViajeSalida(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Traslado,
            IList<ClsProductoTrasladoDetalle> Detalles, bool ServicioExterno)
        {
            if (ServicioExterno)
            {
                return;
            }

            if (Traslado.CodigoConductor <= 0 ||
                !LogisticaMetConductorDisponible(UnidadTrabajo, Traslado.CodigoConductor))
            {
                throw new InvalidOperationException(
                    "El conductor seleccionado no está disponible para el viaje.");
            }

            double PesoTotal = 0;
            object RefrigeracionTransporte =
                UnidadTrabajo.LogisticaMetObtenerRefrigeracionTransporte(Traslado.CodigoTransporte);
            bool RequiereCadenaFrio = RefrigeracionTransporte != null &&
                RefrigeracionTransporte != DBNull.Value &&
                Convert.ToBoolean(RefrigeracionTransporte);
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                double PesoDetalle = LogisticaMetCalcularPesoTotal(Detalle);
                PesoTotal += PesoDetalle;
                if (!RequiereCadenaFrio)
                {
                    RequiereCadenaFrio = LogisticaMetProductoRequiereFrio(
                        UnidadTrabajo, Detalle.CodigoProducto);
                }
            }

            long CodigoViaje = UnidadTrabajo.LogisticaMetInsertarEncabezadoViaje(
                RequiereCadenaFrio, PesoTotal,
                Traslado.CodigoTransporte, Traslado.CodigoConductor);
            int Orden = 1;
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                double PesoDetalle = LogisticaMetCalcularPesoTotal(Detalle);
                string Descripcion = string.IsNullOrWhiteSpace(Detalle.Nombre)
                    ? "Producto " + Detalle.CodigoProducto
                    : Detalle.Nombre;
                long CodigoDetalle = UnidadTrabajo.LogisticaMetInsertarDetalleViaje(
                    CodigoViaje, Orden, Descripcion, Detalle.Cantidad, PesoDetalle);
                object Lote = LogisticaMetObtenerLoteProducto(
                    UnidadTrabajo, Detalle.CodigoProducto);
                if (Lote != null && Lote != DBNull.Value)
                {
                    UnidadTrabajo.LogisticaMetInsertarDetalleViajeLote(
                        CodigoDetalle, Convert.ToInt64(Lote), Detalle.Cantidad, PesoDetalle);
                }

                Orden++;
            }
        }

        private static bool LogisticaMetProductoRequiereFrio(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoProducto)
        {
            DataTable RefrigeracionLotes =
                UnidadTrabajo.LogisticaMetObtenerRefrigeracionLotesProducto(CodigoProducto);
            foreach (DataRow Lote in RefrigeracionLotes.Rows)
            {
                if (!Lote.IsNull("RequiereFrio") && Convert.ToBoolean(Lote["RequiereFrio"]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}