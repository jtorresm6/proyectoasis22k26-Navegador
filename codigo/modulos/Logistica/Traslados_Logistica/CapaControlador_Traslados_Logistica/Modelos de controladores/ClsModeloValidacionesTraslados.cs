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
 * Contiene las validaciones y reglas del traslado.
 * ===================================================================
 */

namespace CapaControlador_Traslados_Logistica
{
    public partial class ClsModeloTraslado
    {
        private ClsTrasladoSolicitud LogisticaMetCrearSolicitud(
            string CodigoTraslado, int BodegaOrigen, int BodegaDestino,
            int CodigoTipoTransporte, int CodigoTransporte, int CodigoConductor,
            string Observaciones, string Estado)
        {
            if (string.IsNullOrWhiteSpace(CodigoTraslado) || CodigoTraslado.Trim().Length > 30)
            {
                throw new InvalidOperationException(
                    "Ingrese un código de traslado de hasta 30 caracteres.");
            }

            if (BodegaOrigen <= 0 || BodegaDestino <= 0)
            {
                throw new InvalidOperationException("Seleccione las bodegas de origen y destino.");
            }

            if (BodegaOrigen == BodegaDestino)
            {
                throw new InvalidOperationException(
                    "La bodega de origen debe ser diferente a la bodega de destino.");
            }

            if (LogisticaMetRequiereTransporte(Estado) &&
                (CodigoTipoTransporte <= 0 || CodigoTransporte <= 0))
            {
                throw new InvalidOperationException(
                    "Para despachar o recibir el traslado debe asignar un transporte disponible.");
            }

            if (_Detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "Agregue al menos un producto al detalle del traslado.");
            }

            if (Estado != ClsReglasEstadoTraslado.Solicitado &&
                Estado != ClsReglasEstadoTraslado.EnTransito &&
                Estado != ClsReglasEstadoTraslado.Recibido &&
                Estado != ClsReglasEstadoTraslado.Cancelado)
            {
                throw new InvalidOperationException("Seleccione un estado válido para el traslado.");
            }

            if (CodigoTransporte > 0 && CodigoTipoTransporte <= 0)
            {
                throw new InvalidOperationException(
                    "Seleccione el tipo de transporte de la placa asignada.");
            }

            if (Observaciones != null && Observaciones.Length > 500)
            {
                throw new InvalidOperationException(
                    "Las observaciones no pueden superar los 500 caracteres.");
            }

            var Solicitud = new ClsTrasladoSolicitud
            {
                CodigoTraslado = CodigoTraslado.Trim(),
                BodegaOrigen = BodegaOrigen,
                BodegaDestino = BodegaDestino,
                CodigoTipoTransporte = CodigoTipoTransporte,
                CodigoTransporte = CodigoTransporte,
                CodigoConductor = CodigoConductor,
                Observaciones = Observaciones ?? string.Empty,
                Estado = Estado
            };
            foreach (ClsProductoTrasladoDetalle Detalle in _Detalles)
            {
                Solicitud.Detalles.Add(new ClsProductoTrasladoDetalle
                {
                    CodigoProducto = Detalle.CodigoProducto,
                    SKU = Detalle.SKU,
                    Nombre = Detalle.Nombre,
                    Descripcion = Detalle.Descripcion,
                    Cantidad = Detalle.Cantidad,
                    ExistenciasDisponibles = Detalle.ExistenciasDisponibles,
                    PesoUnitario = Detalle.PesoUnitario
                });
            }

            return Solicitud;
        }

        private static bool LogisticaMetRequiereTransporte(string Estado)
        {
            return string.Equals(Estado, ClsReglasEstadoTraslado.EnTransito,
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(Estado, ClsReglasEstadoTraslado.Recibido,
                    StringComparison.OrdinalIgnoreCase);
        }

        private static double LogisticaMetCalcularPesoTotal(ClsProductoTrasladoDetalle Detalle)
        {
            return Detalle.PesoUnitario * Detalle.Cantidad;
        }

        private static bool LogisticaMetEsEstadoActivo(string Estado)
        {
            return string.Equals(Estado, ClsReglasEstadoTraslado.Solicitado,
                    StringComparison.OrdinalIgnoreCase) ||
                string.Equals(Estado, ClsReglasEstadoTraslado.EnTransito,
                    StringComparison.OrdinalIgnoreCase);
        }

        private static bool LogisticaMetConductorDisponible(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoConductor)
        {
            DataTable DatosConductor = UnidadTrabajo.LogisticaMetObtenerConductor(CodigoConductor);
            if (DatosConductor.Rows.Count == 0)
            {
                return false;
            }

            foreach (DataRow Conductor in DatosConductor.Rows)
            {
                if (!string.Equals(Convert.ToString(Conductor["EstadoConductor"]),
                    "DISPONIBLE", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Convert.ToString(Conductor["EstadoViaje"]),
                        "EN_CURSO", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool LogisticaMetTransporteDisponible(
            IRepositorioUnidadTrabajoTraslado UnidadTrabajo, int CodigoTipoTransporte,
            int CodigoTransporte, string CodigoTrasladoExcluir)
        {
            DataTable Asignaciones = UnidadTrabajo.LogisticaMetObtenerTransporte(CodigoTransporte);
            if (Asignaciones.Rows.Count == 0)
            {
                return false;
            }

            bool TipoCorrecto = false;
            bool Disponible = false;
            bool AsignadoAlTraslado = false;
            bool TieneOtroTrasladoActivo = false;
            foreach (DataRow Asignacion in Asignaciones.Rows)
            {
                TipoCorrecto = Convert.ToInt32(Asignacion["CodigoTipoTransporte"]) ==
                    CodigoTipoTransporte;
                Disponible = string.Equals(Convert.ToString(Asignacion["EstadoTransporte"]),
                    "DISPONIBLE", StringComparison.OrdinalIgnoreCase);
                string CodigoAsignado = Convert.ToString(Asignacion["CodigoTrasladoAsignado"]);
                if (!string.IsNullOrWhiteSpace(CodigoTrasladoExcluir) &&
                    string.Equals(CodigoAsignado, CodigoTrasladoExcluir,
                        StringComparison.OrdinalIgnoreCase))
                {
                    AsignadoAlTraslado = true;
                }
                else if (LogisticaMetEsEstadoActivo(
                    Convert.ToString(Asignacion["EstadoTrasladoAsignado"])))
                {
                    TieneOtroTrasladoActivo = true;
                }
            }

            return TipoCorrecto && (Disponible || AsignadoAlTraslado) &&
                !TieneOtroTrasladoActivo;
        }

        private static void LogisticaMetValidarDisponibilidad(
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

                int Disponible = Stock.StockActual - Stock.Apartados + ReservaActual;
                if (Disponible < Detalle.Cantidad)
                {
                    throw new InvalidOperationException(
                        "Las existencias disponibles del producto " + Detalle.SKU +
                        " cambiaron. Disponibles: " + Math.Max(0, Disponible) + ".");
                }
            }
        }

        private static int LogisticaMetCalcularApartados(
            ClsStockProductoTraslado Stock, ClsProductoTrasladoDetalle Detalle, bool Reservar)
        {
            int NuevoApartado = Reservar
                ? Stock.Apartados + Detalle.Cantidad
                : Math.Max(0, Stock.Apartados - Detalle.Cantidad);
            if (Reservar && NuevoApartado > Stock.StockActual)
            {
                throw new InvalidOperationException(
                    "No hay existencias suficientes para reservar el producto " +
                    Detalle.SKU + ".");
            }

            return NuevoApartado;
        }

        private static void LogisticaMetValidarTrasladoPersistido(
            ClsTrasladoPersistido Existente, string EstadoNuevo)
        {
            if (Existente.Estado == ClsReglasEstadoTraslado.Recibido ||
                Existente.Estado == ClsReglasEstadoTraslado.Cancelado)
            {
                throw new InvalidOperationException(
                    "Un traslado recibido o cancelado es histórico y no puede modificarse.");
            }

            if (!LogisticaMetPuedeCambiarEstado(Existente.Estado, EstadoNuevo))
            {
                throw new InvalidOperationException(
                    "La transición de estado de " + Existente.Estado + " a " + EstadoNuevo +
                    " no está permitida.");
            }
        }

        private static bool LogisticaMetPuedeCambiarEstado(string EstadoActual, string EstadoNuevo)
        {
            if (string.Equals(EstadoActual, EstadoNuevo, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(EstadoActual, ClsReglasEstadoTraslado.Solicitado,
                StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(EstadoNuevo, ClsReglasEstadoTraslado.EnTransito,
                        StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(EstadoNuevo, ClsReglasEstadoTraslado.Cancelado,
                        StringComparison.OrdinalIgnoreCase);
            }

            return string.Equals(EstadoActual, ClsReglasEstadoTraslado.EnTransito,
                    StringComparison.OrdinalIgnoreCase) &&
                (string.Equals(EstadoNuevo, ClsReglasEstadoTraslado.Recibido,
                        StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(EstadoNuevo, ClsReglasEstadoTraslado.Cancelado,
                        StringComparison.OrdinalIgnoreCase));
        }

        private static bool LogisticaMetDetallesIguales(
            IDictionary<int, int> DetallesPersistidos,
            IList<ClsProductoTrasladoDetalle> DetallesSolicitados)
        {
            if (DetallesPersistidos.Count != DetallesSolicitados.Count)
            {
                return false;
            }

            foreach (ClsProductoTrasladoDetalle Detalle in DetallesSolicitados)
            {
                int CantidadPersistida;
                if (!DetallesPersistidos.TryGetValue(Detalle.CodigoProducto, out CantidadPersistida) ||
                    CantidadPersistida != Detalle.Cantidad)
                {
                    return false;
                }
            }

            return true;
        }

        private static void LogisticaMetValidarDatosEnTransito(
            ClsTrasladoPersistido Existente, ClsTrasladoSolicitud Solicitud,
            bool ServicioExterno, IDictionary<int, int> DetallesPersistidos)
        {
            if (Existente.BodegaOrigen != Solicitud.BodegaOrigen ||
                Existente.BodegaDestino != Solicitud.BodegaDestino ||
                Existente.CodigoTransporte != Solicitud.CodigoTransporte ||
                (!ServicioExterno && Existente.CodigoConductor != Solicitud.CodigoConductor))
            {
                throw new InvalidOperationException(
                    "No se pueden cambiar bodegas, transporte ni conductor cuando el traslado está en tránsito.");
            }

            if (!LogisticaMetDetallesIguales(DetallesPersistidos, Solicitud.Detalles))
            {
                throw new InvalidOperationException(
                    "No se puede cambiar el detalle de un traslado que está en tránsito.");
            }
        }
    }
}