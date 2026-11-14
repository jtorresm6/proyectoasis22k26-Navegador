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
 * Procesa las operaciones transaccionales de traslados.
 * ===================================================================
 */

namespace CapaControlador_Traslados_Logistica
{
    public partial class ClsModeloTraslado
    {
        public void LogisticaMetGuardarTraslado(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones)
        {
            ClsTrasladoSolicitud Solicitud = LogisticaMetCrearSolicitud(
                CodigoTraslado, BodegaOrigen, BodegaDestino, CodigoTipoTransporte,
                CodigoTransporte, CodigoConductor, Observaciones,
                ClsReglasEstadoTraslado.Solicitado);
            _RepositorioTraslado.LogisticaMetEjecutarUnidadTrabajo(
                UnidadTrabajo => LogisticaMetProcesarGuardarTraslado(UnidadTrabajo, Solicitud));
        }

        public void LogisticaMetActualizarTraslado(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones, string Estado)
        {
            ClsTrasladoSolicitud Solicitud = LogisticaMetCrearSolicitud(
                CodigoTraslado, BodegaOrigen, BodegaDestino, CodigoTipoTransporte,
                CodigoTransporte, CodigoConductor, Observaciones, Estado);
            _RepositorioTraslado.LogisticaMetEjecutarUnidadTrabajo(
                UnidadTrabajo => LogisticaMetProcesarActualizarTraslado(
                    UnidadTrabajo, Solicitud));
        }

        private void LogisticaMetProcesarGuardarTraslado(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoSolicitud Solicitud)
        {
            if (UnidadTrabajo.LogisticaMetObtenerTrasladoBloqueado(
                Solicitud.CodigoTraslado) != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un traslado con el código " + Solicitud.CodigoTraslado + ".");
            }

            if (Solicitud.CodigoTransporte > 0 &&
                !LogisticaMetTransporteDisponible(UnidadTrabajo,
                    Solicitud.CodigoTipoTransporte, Solicitud.CodigoTransporte, null))
            {
                throw new InvalidOperationException(
                    "Seleccione una placa válida, disponible y correspondiente al tipo de transporte.");
            }

            Dictionary<int, ClsStockProductoTraslado> Stocks =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Solicitud.BodegaOrigen);
            LogisticaMetValidarDisponibilidad(Solicitud.Detalles, Stocks, null);
            int CodigoTraslado = UnidadTrabajo.LogisticaMetInsertarTraslado(Solicitud);
            UnidadTrabajo.LogisticaMetInsertarDetalles(CodigoTraslado, Solicitud.Detalles);
            LogisticaMetActualizarReservas(UnidadTrabajo, Stocks, Solicitud.Detalles, true);
            LogisticaMetCambiarEstadoTransporte(UnidadTrabajo,
                Solicitud.CodigoTransporte, "NO DISPONIBLE");
        }

        private void LogisticaMetProcesarActualizarTraslado(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoSolicitud Solicitud)
        {
            ClsTrasladoPersistido Existente = UnidadTrabajo.LogisticaMetObtenerTrasladoBloqueado(
                Solicitud.CodigoTraslado);
            if (Existente == null)
            {
                throw new InvalidOperationException("El traslado seleccionado ya no existe.");
            }

            LogisticaMetValidarTrasladoPersistido(Existente, Solicitud.Estado);
            if (Existente.Estado == ClsReglasEstadoTraslado.EnTransito)
            {
                LogisticaMetProcesarTrasladoEnTransito(UnidadTrabajo, Existente, Solicitud);
                return;
            }

            LogisticaMetProcesarTrasladoSolicitado(UnidadTrabajo, Existente, Solicitud);
        }

        private void LogisticaMetProcesarTrasladoSolicitado(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Existente,
            ClsTrasladoSolicitud Solicitud)
        {
            bool ServicioExterno = UnidadTrabajo.LogisticaMetObtenerCantidadServiciosExternos(
                Existente.CodigoInterno) > 0;
            Dictionary<int, int> DetallesPersistidos =
                UnidadTrabajo.LogisticaMetObtenerDetallesBloqueados(Existente.CodigoInterno);
            Dictionary<int, ClsStockProductoTraslado> Stocks =
                UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Solicitud.BodegaOrigen);
            LogisticaMetValidarDisponibilidad(
                Solicitud.Detalles, Stocks,
                Existente.BodegaOrigen == Solicitud.BodegaOrigen ? DetallesPersistidos : null);

            if (Solicitud.Estado == ClsReglasEstadoTraslado.EnTransito && !ServicioExterno)
            {
                if (Solicitud.CodigoConductor <= 0)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un conductor para despachar el traslado.");
                }

                if (Solicitud.CodigoConductor != Existente.CodigoConductor &&
                    !LogisticaMetConductorDisponible(UnidadTrabajo, Solicitud.CodigoConductor))
                {
                    throw new InvalidOperationException(
                        "El conductor seleccionado ya no está disponible.");
                }
            }

            if (Solicitud.CodigoTransporte > 0 &&
                Solicitud.CodigoTransporte != Existente.CodigoTransporte &&
                !LogisticaMetTransporteDisponible(UnidadTrabajo,
                    Solicitud.CodigoTipoTransporte, Solicitud.CodigoTransporte,
                    Solicitud.CodigoTraslado))
            {
                throw new InvalidOperationException(
                    "El transporte seleccionado ya no está disponible o no corresponde al tipo elegido.");
            }

            bool CambiaronDetalles = !LogisticaMetDetallesIguales(
                DetallesPersistidos, Solicitud.Detalles);
            if (CambiaronDetalles || Existente.BodegaOrigen != Solicitud.BodegaOrigen)
            {
                LogisticaMetSincronizarReservas(
                    UnidadTrabajo, Existente.BodegaOrigen, Solicitud.BodegaOrigen,
                    DetallesPersistidos, Solicitud.Detalles);
            }

            UnidadTrabajo.LogisticaMetActualizarEncabezado(Existente.CodigoInterno, Solicitud);
            if (CambiaronDetalles)
            {
                UnidadTrabajo.LogisticaMetActualizarDetalles(
                    Existente.CodigoInterno, Solicitud.Detalles);
            }

            int TransporteAnterior = Existente.CodigoTransporte;
            Existente.BodegaOrigen = Solicitud.BodegaOrigen;
            Existente.BodegaDestino = Solicitud.BodegaDestino;
            Existente.CodigoTransporte = Solicitud.CodigoTransporte;
            Existente.CodigoConductor = ServicioExterno ||
                Solicitud.Estado == ClsReglasEstadoTraslado.Solicitado
                    ? 0
                    : Solicitud.CodigoConductor;
            Existente.CodigoTraslado = Solicitud.CodigoTraslado;

            if (Solicitud.Estado == ClsReglasEstadoTraslado.Cancelado)
            {
                Dictionary<int, ClsStockProductoTraslado> StocksOrigen =
                    UnidadTrabajo.LogisticaMetObtenerStocksBloqueados(Existente.BodegaOrigen);
                LogisticaMetActualizarReservas(
                    UnidadTrabajo, StocksOrigen, Solicitud.Detalles, false);
                LogisticaMetCambiarEstadoTransporte(
                    UnidadTrabajo, TransporteAnterior, "DISPONIBLE");
                if (Solicitud.CodigoTransporte != TransporteAnterior)
                {
                    LogisticaMetCambiarEstadoTransporte(UnidadTrabajo,
                        Solicitud.CodigoTransporte, "DISPONIBLE");
                }

                return;
            }

            if (Solicitud.CodigoTransporte != TransporteAnterior)
            {
                LogisticaMetCambiarEstadoTransporte(
                    UnidadTrabajo, TransporteAnterior, "DISPONIBLE");
                LogisticaMetCambiarEstadoTransporte(UnidadTrabajo,
                    Solicitud.CodigoTransporte, "NO DISPONIBLE");
            }

            if (Solicitud.Estado == ClsReglasEstadoTraslado.EnTransito)
            {
                LogisticaMetRegistrarSalida(UnidadTrabajo, Existente, Solicitud.Detalles);
                LogisticaMetRegistrarViajeSalida(UnidadTrabajo, Existente, Solicitud.Detalles,
                    ServicioExterno);
            }
        }

        private void LogisticaMetProcesarTrasladoEnTransito(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, ClsTrasladoPersistido Existente,
            ClsTrasladoSolicitud Solicitud)
        {
            bool ServicioExterno = UnidadTrabajo.LogisticaMetObtenerCantidadServiciosExternos(
                Existente.CodigoInterno) > 0;
            Dictionary<int, int> DetallesPersistidos =
                UnidadTrabajo.LogisticaMetObtenerDetallesBloqueados(Existente.CodigoInterno);
            LogisticaMetValidarDatosEnTransito(
                Existente, Solicitud, ServicioExterno, DetallesPersistidos);
            UnidadTrabajo.LogisticaMetActualizarObservaciones(
                Existente.CodigoInterno, Solicitud.Observaciones);

            if (Solicitud.Estado == ClsReglasEstadoTraslado.Recibido)
            {
                LogisticaMetRegistrarEntrada(UnidadTrabajo, Existente, Solicitud.Detalles);
                LogisticaMetActualizarViajeActivo(UnidadTrabajo,
                    Existente.CodigoTransporte, "FINALIZADO", true);
                UnidadTrabajo.LogisticaMetActualizarEstado(
                    Existente.CodigoInterno, Solicitud.Estado);
                LogisticaMetCambiarEstadoTransporte(UnidadTrabajo,
                    Existente.CodigoTransporte, "DISPONIBLE");
            }
            else if (Solicitud.Estado == ClsReglasEstadoTraslado.Cancelado)
            {
                LogisticaMetAnularSalida(UnidadTrabajo, Existente, Solicitud.Detalles);
                LogisticaMetActualizarViajeActivo(UnidadTrabajo,
                    Existente.CodigoTransporte, "CANCELADO", false);
                UnidadTrabajo.LogisticaMetActualizarEstado(
                    Existente.CodigoInterno, ClsReglasEstadoTraslado.Cancelado);
                LogisticaMetCambiarEstadoTransporte(UnidadTrabajo,
                    Existente.CodigoTransporte, "DISPONIBLE");
            }
        }
    }
}