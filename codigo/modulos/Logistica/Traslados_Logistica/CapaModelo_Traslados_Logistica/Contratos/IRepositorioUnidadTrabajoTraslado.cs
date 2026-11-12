using System.Data;
using System.Collections.Generic;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 * Define operaciones de datos ligadas a una transacción de traslado.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Contratos
{
    public interface IRepositorioUnidadTrabajoTraslado
    {
        ClsTrasladoPersistido LogisticaMetObtenerTrasladoBloqueado(string CodigoTraslado);
        int LogisticaMetObtenerCantidadServiciosExternos(int CodigoTraslado);
        DataTable LogisticaMetObtenerTransporte(int CodigoTransporte);
        DataTable LogisticaMetObtenerConductor(int CodigoConductor);
        object LogisticaMetObtenerRefrigeracionTransporte(int CodigoTransporte);
        DataTable LogisticaMetObtenerRefrigeracionLotesProducto(int CodigoProducto);
        Dictionary<int, ClsStockProductoTraslado> LogisticaMetObtenerStocksBloqueados(int CodigoBodega);
        Dictionary<int, int> LogisticaMetObtenerDetallesBloqueados(int CodigoTraslado);

        int LogisticaMetInsertarTraslado(ClsTrasladoSolicitud Solicitud);
        void LogisticaMetInsertarDetalles(int CodigoTraslado,
            IList<ClsProductoTrasladoDetalle> Detalles);
        void LogisticaMetActualizarEncabezado(int CodigoTraslado, ClsTrasladoSolicitud Solicitud);
        void LogisticaMetActualizarObservaciones(int CodigoTraslado, string Observaciones);
        void LogisticaMetActualizarEstado(int CodigoTraslado, string Estado);
        void LogisticaMetActualizarDetalles(int CodigoTraslado,
            IList<ClsProductoTrasladoDetalle> Detalles);
        int LogisticaMetCambiarEstadoTransporte(int CodigoTransporte, string EstadoNuevo);

        DataTable LogisticaMetObtenerTiposMovimiento();
        int LogisticaMetObtenerSucursalBodega(int CodigoBodega);
        DataTable LogisticaMetObtenerLotesProducto(int CodigoProducto);
        DataTable LogisticaMetObtenerLotesMovimientosTraslado(int CodigoTraslado, int CodigoProducto);
        int LogisticaMetInsertarMovimiento(int TipoMovimiento, ClsTrasladoPersistido Traslado,
            string Observaciones);
        void LogisticaMetInsertarDetalleMovimiento(int CodigoMovimiento,
            ClsProductoTrasladoDetalle Detalle, object LotePreferido);
        void LogisticaMetAnularMovimientosSalida(int CodigoTraslado);
        int LogisticaMetInsertarStockDestino(int CodigoSucursal, int CodigoBodega,
            int CodigoProducto, int Cantidad, bool EstadoVenta, bool EstadoCompra);
        void LogisticaMetActualizarSaldo(int CodigoBodega, int CodigoProducto, int SaldoActual);
        int LogisticaMetActualizarStock(int IdStock, int StockAnterior, int StockActual,
            int Apartados, int Cargos, int Abonos);
        int LogisticaMetActualizarApartados(int IdStock, int Apartados, int Virtual);

        long LogisticaMetInsertarEncabezadoViaje(bool RequiereCadenaFrio, double PesoTotal,
            int CodigoTransporte, int CodigoConductor);
        long LogisticaMetInsertarDetalleViaje(long CodigoViaje, int Orden, string Descripcion,
            int Cantidad, double PesoTotal);
        void LogisticaMetInsertarDetalleViajeLote(long CodigoDetalle, long CodigoLote,
            int Cantidad, double PesoTotal);
        void LogisticaMetActualizarViajeActivo(int CodigoTransporte, string Estado,
            bool RegistrarLlegada);
    }
}