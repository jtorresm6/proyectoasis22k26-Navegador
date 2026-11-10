using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Traslados_Logistica.Contratos;
using CapaModelo_Traslados_Logistica.Entidades;

/*
 * ==================================================================
 * Área : Logística
 * Autor : José Xavier Bolaños Tenas
 * Carné : 0901-23-2128
 * Fecha : 05/11/2026
 * ==================================================================
 * Propósito :
 * Aqui se encuentran las consultas utilizadas para registrar,
 * consultar y actualizar los traslados entre bodegas.
 * ===================================================================
 */

namespace CapaModelo_Traslados_Logistica.Repositorios
{
    public class ClsRepositorioTraslado : ClsSentencias, IRepositorioTraslado
    {
        private const string _EstadosTrasladoActivo = "'SOLICITADO', 'EN_TRANSITO'";
        private readonly ClsRepositorioInventarioTraslado _Inventario;
        private readonly ClsRepositorioViajeTraslado _Viaje;

        private readonly string _ConsultaTraslados =
            "SELECT traslado.codigoTraslado AS CodigoTraslado, " +
            "traslado.fechaSolicitudTraslado AS FechaSolicitud, " +
            "origen.nombreBodega AS BodegaOrigen, " +
            "traslado.fechaSalidaTraslado AS FechaSalida, " +
            "destino.nombreBodega AS BodegaDestino, " +
            "traslado.fechaLlegadaTraslado AS FechaLlegada, " +
            "traslado.Fk_Bodega_Origen_Codigo AS CodigoBodegaOrigen, " +
            "traslado.Fk_Bodega_Destino_Codigo AS CodigoBodegaDestino, " +
            "traslado.Fk_Transporte_Codigo AS CodigoTransporte, " +
            "COALESCE(transporte.nroPlacaTransporte, 'Sin asignar') AS Transporte, " +
            "transporte.Fk_Tipo_Transporte_Codigo AS CodigoTipoTransporte, " +
            "COALESCE(tipoTransporte.nombreTipoTransporte, '') AS TipoTransporte, " +
            "COALESCE(conductor.idConductor, 0) AS CodigoConductor, " +
            "COALESCE(conductor.nombreCompletoConductor, " +
            "(SELECT conductorExterno.nombreConductorTransporte " +
            "FROM tblServicioTransporteExterno servicioExterno " +
            "INNER JOIN tblConductorTransporte conductorExterno ON " +
            "conductorExterno.Fk_Servicio_Transporte_Externo_Codigo = " +
            "servicioExterno.idServicioTransporteExterno " +
            "WHERE servicioExterno.Fk_Traslado_Codigo = traslado.idTraslado " +
            "ORDER BY servicioExterno.idServicioTransporteExterno DESC LIMIT 1), " +
            "'Sin asignar') AS Conductor, " +
            "traslado.observacionesTraslado AS Observaciones, " +
            "traslado.estadoTraslado AS Estado " +
            "FROM tblTraslado traslado " +
            "INNER JOIN tblBodega origen ON origen.idBodega = traslado.Fk_Bodega_Origen_Codigo " +
            "INNER JOIN tblBodega destino ON destino.idBodega = traslado.Fk_Bodega_Destino_Codigo " +
            "LEFT JOIN tblTransporte transporte ON transporte.idTransporte = traslado.Fk_Transporte_Codigo " +
            "LEFT JOIN tblTipoTransporte tipoTransporte ON tipoTransporte.idTipoTransporte = " +
            "transporte.Fk_Tipo_Transporte_Codigo " +
            "LEFT JOIN tblConductor conductor ON conductor.idConductor = (" +
            "SELECT viaje.Fk_Conductor_Codigo FROM tblEncabezadoViaje viaje " +
            "WHERE viaje.Fk_Transporte_Codigo = traslado.Fk_Transporte_Codigo " +
            "AND traslado.fechaSalidaTraslado IS NOT NULL " +
            "ORDER BY ABS(TIMESTAMPDIFF(SECOND, viaje.horaSalidaEncabezadoViaje, " +
            "traslado.fechaSalidaTraslado)), viaje.idEncabezadoViaje DESC LIMIT 1) " +
            "ORDER BY traslado.idTraslado";

        public ClsRepositorioTraslado()
        {
            _Inventario = new ClsRepositorioInventarioTraslado();
            _Viaje = new ClsRepositorioViajeTraslado();
        }

        public DataTable LogisticaMetObtenerTraslados()
        {
            return LogisticaMetEjecutarConsulta(_ConsultaTraslados);
        }

        public DataTable LogisticaMetObtenerBodegas()
        {
            const string Consulta =
                "SELECT idBodega AS CodigoBodega, nombreBodega AS NombreBodega " +
                "FROM tblBodega ORDER BY nombreBodega";
            return LogisticaMetEjecutarConsulta(Consulta);
        }

        public DataTable LogisticaMetObtenerTiposTransporte()
        {
            const string Consulta =
                "SELECT idTipoTransporte AS CodigoTipoTransporte, " +
                "nombreTipoTransporte AS NombreTipoTransporte " +
                "FROM tblTipoTransporte ORDER BY nombreTipoTransporte";
            return LogisticaMetEjecutarConsulta(Consulta);
        }

        public DataTable LogisticaMetObtenerConductoresDisponibles(string CodigoTrasladoExcluir)
        {
            const string Consulta =
                "SELECT conductor.idConductor AS CodigoConductor, " +
                "CONCAT(conductor.codigoConductor, ' - ', conductor.nombreCompletoConductor) " +
                "AS NombreConductor " +
                "FROM tblConductor conductor " +
                "INNER JOIN tblEstado estado ON estado.idEstado = conductor.Fk_Estado_Codigo " +
                "WHERE UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblEncabezadoViaje viaje " +
                "WHERE viaje.Fk_Conductor_Codigo = conductor.idConductor " +
                "AND viaje.estadoViajeEncabezadoViaje = 'EN_CURSO') " +
                "ORDER BY conductor.nombreCompletoConductor";
            return LogisticaMetEjecutarConsulta(Consulta);
        }

        public DataTable LogisticaMetObtenerTransportesDisponibles(int CodigoTipoTransporte)
        {
            return LogisticaMetObtenerTransportesDisponibles(CodigoTipoTransporte, null);
        }

        public DataTable LogisticaMetObtenerTransportesDisponibles(
            int CodigoTipoTransporte, string CodigoTrasladoExcluir)
        {
            string Consulta =
                "SELECT transporte.idTransporte AS CodigoTransporte, " +
                "transporte.nroPlacaTransporte AS PlacaTransporte " +
                "FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "WHERE transporte.Fk_Tipo_Transporte_Codigo = ? " +
                "AND UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblTraslado traslado " +
                "WHERE traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                "AND traslado.estadoTraslado IN (" + _EstadosTrasladoActivo + ")) ";
            var Parametros = new List<OdbcParameter>
            {
                new OdbcParameter("CodigoTipoTransporte", OdbcType.Int) { Value = CodigoTipoTransporte }
            };

            if (!string.IsNullOrWhiteSpace(CodigoTrasladoExcluir))
            {
                Consulta =
                    Consulta.Substring(0, Consulta.IndexOf("AND UPPER", StringComparison.Ordinal)) +
                    "AND (UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                    "OR transporte.idTransporte = (SELECT Fk_Transporte_Codigo FROM tblTraslado " +
                    "WHERE codigoTraslado = ?)) " +
                    "AND NOT EXISTS (SELECT 1 FROM tblTraslado traslado " +
                    "WHERE traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                    "AND traslado.estadoTraslado IN (" + _EstadosTrasladoActivo + ") " +
                    "AND traslado.codigoTraslado <> ?) ";
                Parametros.Add(new OdbcParameter("CodigoTrasladoActual", OdbcType.VarChar, 30)
                {
                    Value = CodigoTrasladoExcluir
                });
                Parametros.Add(new OdbcParameter("CodigoTrasladoExcluir", OdbcType.VarChar, 30)
                {
                    Value = CodigoTrasladoExcluir
                });
            }

            Consulta += "ORDER BY transporte.nroPlacaTransporte";
            return LogisticaMetEjecutarConsulta(Consulta, Parametros.ToArray());
        }

        public DataTable LogisticaMetObtenerProductosDisponibles(int CodigoBodega)
        {
            return LogisticaMetObtenerProductosDisponibles(CodigoBodega, null);
        }

        public DataTable LogisticaMetObtenerProductosDisponibles(
            int CodigoBodega, string CodigoTrasladoExcluir)
        {
            string JoinReserva = string.Empty;
            string CantidadReserva = "0";
            if (!string.IsNullOrWhiteSpace(CodigoTrasladoExcluir))
            {
                JoinReserva =
                    "LEFT JOIN (SELECT detalle.Fk_Producto_Codigo, " +
                    "SUM(detalle.cantidadDetalleTraslado) AS CantidadReservada " +
                    "FROM tblTraslado traslado " +
                    "INNER JOIN tblDetalleTraslado detalle ON detalle.Fk_Traslado_Codigo = traslado.idTraslado " +
                    "WHERE traslado.codigoTraslado = ? " +
                    "GROUP BY detalle.Fk_Producto_Codigo) reservaActual " +
                    "ON reservaActual.Fk_Producto_Codigo = stock.Fk_Producto_Codigo ";
                CantidadReserva = "COALESCE(reservaActual.CantidadReservada, 0)";
            }

            string Consulta =
                "SELECT producto.idProducto AS CodigoProducto, " +
                "producto.skuProducto AS SKU, producto.nombreProducto AS Nombre, " +
                "producto.descripcionProducto AS Descripcion, " +
                "producto.pesoProducto AS PesoUnitario, " +
                "stock.stockActualStock - stock.apartadosStock + " + CantidadReserva +
                " AS ExistenciasDisponibles, " +
                "CONCAT(producto.skuProducto, ' - ', producto.nombreProducto) AS ProductoMostrar " +
                "FROM tblStock stock " +
                "INNER JOIN tblProducto producto ON producto.idProducto = stock.Fk_Producto_Codigo " +
                JoinReserva +
                "WHERE stock.Fk_Bodega_Codigo = ? " +
                "AND stock.stockActualStock - stock.apartadosStock + " + CantidadReserva + " > 0 " +
                "ORDER BY producto.nombreProducto";

            if (string.IsNullOrWhiteSpace(CodigoTrasladoExcluir))
            {
                return LogisticaMetEjecutarConsulta(
                    Consulta,
                    new OdbcParameter("CodigoBodega", OdbcType.Int) { Value = CodigoBodega });
            }

            return LogisticaMetEjecutarConsulta(
                Consulta,
                new OdbcParameter("CodigoTrasladoExcluir", OdbcType.VarChar, 30)
                {
                    Value = CodigoTrasladoExcluir
                },
                new OdbcParameter("CodigoBodega", OdbcType.Int) { Value = CodigoBodega });
        }

        public DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado)
        {
            const string Consulta =
                "SELECT producto.idProducto AS CodigoProducto, producto.skuProducto AS SKU, " +
                "producto.nombreProducto AS Nombre, producto.descripcionProducto AS Descripcion, " +
                "detalle.cantidadDetalleTraslado AS Cantidad, producto.pesoProducto AS PesoUnitario, " +
                "producto.pesoProducto * detalle.cantidadDetalleTraslado AS PesoTotal, " +
                "producto.precioCostoProducto AS PrecioUnitario, " +
                "producto.precioCostoProducto * detalle.cantidadDetalleTraslado AS PrecioTotal, " +
                "COALESCE(stock.stockActualStock, 0) - COALESCE(stock.apartadosStock, 0) + " +
                "detalle.cantidadDetalleTraslado AS ExistenciasDisponibles " +
                "FROM tblTraslado traslado " +
                "INNER JOIN tblDetalleTraslado detalle ON detalle.Fk_Traslado_Codigo = traslado.idTraslado " +
                "INNER JOIN tblProducto producto ON producto.idProducto = detalle.Fk_Producto_Codigo " +
                "LEFT JOIN tblStock stock ON stock.Fk_Bodega_Codigo = traslado.Fk_Bodega_Origen_Codigo " +
                "AND stock.Fk_Producto_Codigo = detalle.Fk_Producto_Codigo " +
                "WHERE traslado.codigoTraslado = ? " +
                "ORDER BY producto.idProducto";

            return LogisticaMetEjecutarConsulta(
                Consulta,
                new OdbcParameter("CodigoTraslado", OdbcType.VarChar, 30)
                {
                    Value = CodigoTraslado
                });
        }

        public void LogisticaMetGuardarTraslado(ClsTrasladoSolicitud Solicitud)
        {
            if (Solicitud == null)
            {
                throw new ArgumentNullException("Solicitud");
            }

            LogisticaMetValidarSolicitud(Solicitud);
            LogisticaMetValidarDetalles(Solicitud.Detalles);
            LogisticaMetEjecutarTransaccion(Conexion =>
            {
                using (var Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        LogisticaMetValidarCodigoDisponible(
                            Conexion, Transaccion, Solicitud.CodigoTraslado);
                        if (Solicitud.CodigoTransporte > 0)
                        {
                            LogisticaMetValidarTransporte(Conexion, Transaccion, Solicitud);
                        }

                        Dictionary<int, ClsStockProductoTraslado> Stocks =
                            _Inventario.LogisticaMetObtenerStocksBloqueados(
                                Conexion, Transaccion, Solicitud.BodegaOrigen);
                        _Inventario.LogisticaMetValidarDisponibilidad(
                            Solicitud.Detalles, Stocks, null);
                        int CodigoGenerado = LogisticaMetInsertarEncabezado(
                            Conexion, Transaccion, Solicitud);
                        LogisticaMetInsertarDetalles(
                            Conexion, Transaccion, CodigoGenerado, Solicitud.Detalles);
                        _Inventario.LogisticaMetReservar(
                            Conexion, Transaccion, Stocks, Solicitud.Detalles);
                        LogisticaMetCambiarEstadoTransporte(
                            Conexion, Transaccion, Solicitud.CodigoTransporte, "NO DISPONIBLE");
                        Transaccion.Commit();
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            });
        }

        public void LogisticaMetActualizarTraslado(ClsTrasladoSolicitud Solicitud)
        {
            if (Solicitud == null)
            {
                throw new ArgumentNullException("Solicitud");
            }

            LogisticaMetEjecutarTransaccion(Conexion =>
            {
                using (var Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        ClsTrasladoPersistido Existente = LogisticaMetObtenerTrasladoBloqueado(
                            Conexion, Transaccion, Solicitud.CodigoTraslado);
                        if (Existente == null)
                        {
                            throw new InvalidOperationException("El traslado seleccionado ya no existe.");
                        }

                        if (Existente.Estado == ClsReglasEstadoTraslado.Recibido ||
                            Existente.Estado == ClsReglasEstadoTraslado.Cancelado)
                        {
                            throw new InvalidOperationException(
                                "Un traslado recibido o cancelado es histórico y no puede modificarse.");
                        }

                        ClsReglasEstadoTraslado.LogisticaMetValidarCambioEstado(
                            Existente.Estado, Solicitud.Estado);
                        LogisticaMetValidarSolicitud(Solicitud);
                        LogisticaMetValidarDetalles(Solicitud.Detalles);

                        if (Existente.Estado == ClsReglasEstadoTraslado.EnTransito)
                        {
                            LogisticaMetProcesarTrasladoEnTransito(
                                Conexion, Transaccion, Existente, Solicitud);
                        }
                        else
                        {
                            LogisticaMetProcesarTrasladoSolicitado(
                                Conexion, Transaccion, Existente, Solicitud);
                        }

                        Transaccion.Commit();
                    }
                    catch
                    {
                        Transaccion.Rollback();
                        throw;
                    }
                }
            });
        }

        private void LogisticaMetProcesarTrasladoSolicitado(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Existente, ClsTrasladoSolicitud Solicitud)
        {
            bool ServicioExterno = LogisticaMetTieneServicioExterno(
                Conexion, Transaccion, Existente.CodigoInterno);
            Dictionary<int, ClsStockProductoTraslado> Stocks =
                _Inventario.LogisticaMetObtenerStocksBloqueados(
                    Conexion, Transaccion, Solicitud.BodegaOrigen);
            Dictionary<int, int> DetallesPersistidos =
                LogisticaMetObtenerDetallesBloqueados(Conexion, Transaccion, Existente.CodigoInterno);
            _Inventario.LogisticaMetValidarDisponibilidad(
                Solicitud.Detalles, Stocks, DetallesPersistidos);

            if (Solicitud.Estado == ClsReglasEstadoTraslado.EnTransito && !ServicioExterno)
            {
                if (Solicitud.CodigoConductor <= 0)
                {
                    throw new InvalidOperationException(
                        "Debe seleccionar un conductor para despachar el traslado.");
                }

                if (Solicitud.CodigoConductor != Existente.CodigoConductor)
                {
                    LogisticaMetValidarConductorActualizacion(
                        Conexion, Transaccion, Solicitud);
                }
            }

            if (Solicitud.CodigoTransporte > 0 &&
                Solicitud.CodigoTransporte != Existente.CodigoTransporte)
            {
                LogisticaMetValidarTransporteActualizacion(Conexion, Transaccion, Solicitud);
            }

            bool CambiaronDetalles = !LogisticaMetDetallesIguales(
                DetallesPersistidos, Solicitud.Detalles);
            if (CambiaronDetalles || Existente.BodegaOrigen != Solicitud.BodegaOrigen)
            {
                _Inventario.LogisticaMetSincronizarReservas(
                    Conexion, Transaccion, Existente.BodegaOrigen, Solicitud.BodegaOrigen,
                    DetallesPersistidos, Solicitud.Detalles);
            }

            LogisticaMetActualizarEncabezado(Conexion, Transaccion, Existente.CodigoInterno, Solicitud);
            if (CambiaronDetalles)
            {
                LogisticaMetActualizarDetalles(
                    Conexion, Transaccion, Existente.CodigoInterno, Solicitud.Detalles);
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
                    _Inventario.LogisticaMetObtenerStocksBloqueados(
                        Conexion, Transaccion, Existente.BodegaOrigen);
                _Inventario.LogisticaMetLiberarReserva(
                    Conexion, Transaccion, StocksOrigen, Solicitud.Detalles);
                LogisticaMetCambiarEstadoTransporte(
                    Conexion, Transaccion, TransporteAnterior, "DISPONIBLE");
                if (Solicitud.CodigoTransporte != TransporteAnterior)
                {
                    LogisticaMetCambiarEstadoTransporte(
                        Conexion, Transaccion, Solicitud.CodigoTransporte, "DISPONIBLE");
                }

                return;
            }

            if (Solicitud.CodigoTransporte != TransporteAnterior)
            {
                LogisticaMetCambiarEstadoTransporte(
                    Conexion, Transaccion, TransporteAnterior, "DISPONIBLE");
                LogisticaMetCambiarEstadoTransporte(
                    Conexion, Transaccion, Solicitud.CodigoTransporte, "NO DISPONIBLE");
            }

            if (Solicitud.Estado == ClsReglasEstadoTraslado.EnTransito)
            {
                if (Existente.CodigoTransporte <= 0)
                {
                    throw new InvalidOperationException(
                        "Para despachar el traslado debe asignar un transporte.");
                }

                _Inventario.LogisticaMetRegistrarSalida(
                    Conexion, Transaccion, Existente, Solicitud.Detalles);
                _Viaje.LogisticaMetRegistrarSalida(
                    Conexion, Transaccion, Existente, Solicitud.Detalles);
            }
        }

        private void LogisticaMetProcesarTrasladoEnTransito(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Existente, ClsTrasladoSolicitud Solicitud)
        {
            LogisticaMetValidarDatosEnTransito(Conexion, Transaccion, Existente, Solicitud);
            LogisticaMetActualizarObservaciones(
                Conexion, Transaccion, Existente.CodigoInterno, Solicitud.Observaciones);

            if (Solicitud.Estado == ClsReglasEstadoTraslado.Recibido)
            {
                _Inventario.LogisticaMetRegistrarEntrada(
                    Conexion, Transaccion, Existente, Solicitud.Detalles);
                _Viaje.LogisticaMetRegistrarLlegada(
                    Conexion, Transaccion, Existente.CodigoTransporte);
                LogisticaMetActualizarEstado(
                    Conexion, Transaccion, Existente.CodigoInterno, Solicitud.Estado);
                if (Existente.CodigoTransporte > 0)
                {
                    LogisticaMetCambiarEstadoTransporte(
                        Conexion, Transaccion, Existente.CodigoTransporte, "DISPONIBLE");
                }
            }
            else if (Solicitud.Estado == ClsReglasEstadoTraslado.Cancelado)
            {
                _Inventario.LogisticaMetAnularSalida(
                    Conexion, Transaccion, Existente, Solicitud.Detalles);
                _Viaje.LogisticaMetCancelarViaje(
                    Conexion, Transaccion, Existente.CodigoTransporte);
                LogisticaMetEjecutarNoConsulta(
                    Conexion, Transaccion,
                    "UPDATE tblTraslado SET estadoTraslado = ? WHERE idTraslado = ?",
                    LogisticaMetCrearParametro(
                        "Estado", OdbcType.VarChar, 20, ClsReglasEstadoTraslado.Cancelado),
                    LogisticaMetCrearParametro("Codigo", OdbcType.Int, Existente.CodigoInterno));
                if (Existente.CodigoTransporte > 0)
                {
                    LogisticaMetCambiarEstadoTransporte(
                        Conexion, Transaccion, Existente.CodigoTransporte, "DISPONIBLE");
                }
            }
        }

        private void LogisticaMetEjecutarTransaccion(Action<OdbcConnection> Accion)
        {
            try
            {
                using (var Conexion = LogisticaMetObtenerConexion())
                {
                    Conexion.Open();
                    Accion(Conexion);
                }
            }
            catch (OdbcException Error)
            {
                throw new DataException("No fue posible persistir el traslado en la base de datos.", Error);
            }
        }

        private DataTable LogisticaMetEjecutarConsulta(
            string Consulta, params OdbcParameter[] Parametros)
        {
            var Tabla = new DataTable();
            try
            {
                using (var Conexion = LogisticaMetObtenerConexion())
                using (var Comando = new OdbcCommand(Consulta, Conexion))
                using (var Adaptador = new OdbcDataAdapter(Comando))
                {
                    Comando.Parameters.AddRange(Parametros);
                    Adaptador.Fill(Tabla);
                }
            }
            catch (OdbcException Error)
            {
                throw new DataException("No fue posible consultar los datos del traslado.", Error);
            }

            return Tabla;
        }

        private ClsTrasladoPersistido LogisticaMetObtenerTrasladoBloqueado(
            OdbcConnection Conexion, OdbcTransaction Transaccion, string CodigoTraslado)
        {
            const string Consulta =
                "SELECT idTraslado, estadoTraslado, Fk_Bodega_Origen_Codigo, " +
                "Fk_Bodega_Destino_Codigo, COALESCE(Fk_Transporte_Codigo, 0), " +
                "COALESCE((SELECT viaje.Fk_Conductor_Codigo FROM tblEncabezadoViaje viaje " +
                "WHERE viaje.Fk_Transporte_Codigo = tblTraslado.Fk_Transporte_Codigo " +
                "AND tblTraslado.fechaSalidaTraslado IS NOT NULL " +
                "ORDER BY ABS(TIMESTAMPDIFF(SECOND, viaje.horaSalidaEncabezadoViaje, " +
                "tblTraslado.fechaSalidaTraslado)), viaje.idEncabezadoViaje DESC LIMIT 1), 0), " +
                "codigoTraslado " +
                "FROM tblTraslado WHERE codigoTraslado = ? FOR UPDATE";

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoTraslado", OdbcType.VarChar, 30).Value = CodigoTraslado;
                using (var Lector = Comando.ExecuteReader())
                {
                    if (!Lector.Read())
                    {
                        return null;
                    }

                    return new ClsTrasladoPersistido
                    {
                        CodigoInterno = Convert.ToInt32(Lector[0]),
                        Estado = Convert.ToString(Lector[1]),
                        BodegaOrigen = Convert.ToInt32(Lector[2]),
                        BodegaDestino = Convert.ToInt32(Lector[3]),
                        CodigoTransporte = Convert.ToInt32(Lector[4]),
                        CodigoConductor = Convert.ToInt32(Lector[5]),
                        CodigoTraslado = Convert.ToString(Lector[6])
                    };
                }
            }
        }

        private void LogisticaMetValidarDatosEnTransito(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            ClsTrasladoPersistido Existente, ClsTrasladoSolicitud Solicitud)
        {
            bool ServicioExterno = LogisticaMetTieneServicioExterno(
                Conexion, Transaccion, Existente.CodigoInterno);
            if (Existente.BodegaOrigen != Solicitud.BodegaOrigen ||
                Existente.BodegaDestino != Solicitud.BodegaDestino ||
                Existente.CodigoTransporte != Solicitud.CodigoTransporte ||
                (!ServicioExterno && Existente.CodigoConductor != Solicitud.CodigoConductor))
            {
                throw new InvalidOperationException(
                    "No se pueden cambiar bodegas, transporte ni conductor cuando el traslado está en tránsito.");
            }

            Dictionary<int, int> DetallesPersistidos =
                LogisticaMetObtenerDetallesBloqueados(Conexion, Transaccion, Existente.CodigoInterno);
            if (!LogisticaMetDetallesIguales(DetallesPersistidos, Solicitud.Detalles))
            {
                throw new InvalidOperationException(
                    "No se puede cambiar el detalle de un traslado que está en tránsito.");
            }
        }

        private bool LogisticaMetTieneServicioExterno(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT 1 FROM tblServicioTransporteExterno " +
                "WHERE Fk_Traslado_Codigo = ? LIMIT 1",
                LogisticaMetCrearParametro("Traslado", OdbcType.Int, CodigoTraslado));
            return Resultado != null;
        }

        private bool LogisticaMetDetallesIguales(
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

        private Dictionary<int, int> LogisticaMetObtenerDetallesBloqueados(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado)
        {
            const string Consulta =
                "SELECT Fk_Producto_Codigo, cantidadDetalleTraslado " +
                "FROM tblDetalleTraslado WHERE Fk_Traslado_Codigo = ? " +
                "ORDER BY Fk_Producto_Codigo FOR UPDATE";
            var Detalles = new Dictionary<int, int>();

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoTraslado", OdbcType.Int).Value = CodigoTraslado;
                using (var Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        Detalles.Add(Convert.ToInt32(Lector[0]), Convert.ToInt32(Lector[1]));
                    }
                }
            }

            return Detalles;
        }

        private void LogisticaMetValidarSolicitud(ClsTrasladoSolicitud Solicitud)
        {
            if (string.IsNullOrWhiteSpace(Solicitud.CodigoTraslado) ||
                Solicitud.CodigoTraslado.Trim().Length > 30)
            {
                throw new InvalidOperationException(
                    "El código del traslado debe tener hasta 30 caracteres.");
            }

            if (Solicitud.BodegaOrigen <= 0 || Solicitud.BodegaDestino <= 0 ||
                Solicitud.BodegaOrigen == Solicitud.BodegaDestino)
            {
                throw new InvalidOperationException(
                    "Seleccione bodegas de origen y destino diferentes.");
            }

            if (ClsReglasEstadoTraslado.LogisticaMetRequiereTransporte(Solicitud.Estado) &&
                (Solicitud.CodigoTransporte <= 0 || Solicitud.CodigoTipoTransporte <= 0))
            {
                throw new InvalidOperationException(
                    "Para despachar o recibir el traslado debe asignar un transporte válido.");
            }

            if (Solicitud.CodigoTransporte > 0 && Solicitud.CodigoTipoTransporte <= 0)
            {
                throw new InvalidOperationException(
                    "Seleccione el tipo de transporte de la placa asignada.");
            }

            if (Solicitud.Observaciones != null && Solicitud.Observaciones.Length > 500)
            {
                throw new InvalidOperationException(
                    "Las observaciones no pueden superar los 500 caracteres.");
            }

            if (Solicitud.Estado != ClsReglasEstadoTraslado.Solicitado &&
                Solicitud.Estado != ClsReglasEstadoTraslado.EnTransito &&
                Solicitud.Estado != ClsReglasEstadoTraslado.Recibido &&
                Solicitud.Estado != ClsReglasEstadoTraslado.Cancelado)
            {
                throw new InvalidOperationException("Seleccione un estado válido para el traslado.");
            }
        }

        private void LogisticaMetValidarDetalles(IList<ClsProductoTrasladoDetalle> Detalles)
        {
            if (Detalles == null || Detalles.Count == 0)
            {
                throw new InvalidOperationException(
                    "El traslado debe contener al menos un producto.");
            }

            var Productos = new HashSet<int>();
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                if (Detalle == null || Detalle.CodigoProducto <= 0 || Detalle.Cantidad <= 0)
                {
                    throw new InvalidOperationException(
                        "Cada producto debe tener un código válido y una cantidad mayor que cero.");
                }

                if (!Productos.Add(Detalle.CodigoProducto))
                {
                    throw new InvalidOperationException(
                        "Un producto no puede repetirse en el detalle del traslado.");
                }
            }
        }

        private void LogisticaMetValidarCodigoDisponible(
            OdbcConnection Conexion, OdbcTransaction Transaccion, string CodigoTraslado)
        {
            object Existe = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT 1 FROM tblTraslado WHERE codigoTraslado = ?",
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.VarChar, 30, CodigoTraslado));
            if (Existe != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un traslado con el código " + CodigoTraslado + ".");
            }
        }

        private void LogisticaMetValidarTransporteActualizacion(
            OdbcConnection Conexion, OdbcTransaction Transaccion, ClsTrasladoSolicitud Solicitud)
        {
            const string Consulta =
                "SELECT transporte.idTransporte FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "WHERE transporte.idTransporte = ? " +
                "AND transporte.Fk_Tipo_Transporte_Codigo = ? " +
                "AND UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblTraslado traslado " +
                "WHERE traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                "AND traslado.estadoTraslado IN (" + _EstadosTrasladoActivo + ") " +
                "AND traslado.codigoTraslado <> ?) FOR UPDATE";

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoTransporte", OdbcType.Int).Value =
                    Solicitud.CodigoTransporte;
                Comando.Parameters.Add("CodigoTipoTransporte", OdbcType.Int).Value =
                    Solicitud.CodigoTipoTransporte;
                Comando.Parameters.Add("CodigoTrasladoExcluir", OdbcType.VarChar, 30).Value =
                    Solicitud.CodigoTraslado;
                if (Comando.ExecuteScalar() == null)
                {
                    throw new InvalidOperationException(
                        "El transporte seleccionado ya no está disponible o no corresponde al tipo elegido.");
                }
            }
        }

        private void LogisticaMetActualizarEncabezado(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado,
            ClsTrasladoSolicitud Solicitud)
        {
            const string Consulta =
                "UPDATE tblTraslado SET Fk_Bodega_Origen_Codigo = ?, " +
                "Fk_Bodega_Destino_Codigo = ?, Fk_Transporte_Codigo = ?, " +
                "observacionesTraslado = ?, estadoTraslado = ?, " +
                "fechaSalidaTraslado = CASE WHEN ? = 'EN_TRANSITO' " +
                "THEN COALESCE(fechaSalidaTraslado, CURRENT_TIMESTAMP) ELSE fechaSalidaTraslado END, " +
                "fechaLlegadaTraslado = CASE WHEN ? = 'RECIBIDO' " +
                "THEN CURRENT_TIMESTAMP ELSE fechaLlegadaTraslado END " +
                "WHERE idTraslado = ?";

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("BodegaOrigen", OdbcType.Int).Value = Solicitud.BodegaOrigen;
                Comando.Parameters.Add("BodegaDestino", OdbcType.Int).Value = Solicitud.BodegaDestino;
                Comando.Parameters.Add(LogisticaMetCrearParametroEnteroOpcional(
                    "CodigoTransporte", Solicitud.CodigoTransporte));
                Comando.Parameters.Add("Observaciones", OdbcType.VarChar, 500).Value =
                    Solicitud.Observaciones ?? string.Empty;
                Comando.Parameters.Add("Estado", OdbcType.VarChar, 20).Value = Solicitud.Estado;
                Comando.Parameters.Add("EstadoSalida", OdbcType.VarChar, 20).Value = Solicitud.Estado;
                Comando.Parameters.Add("EstadoLlegada", OdbcType.VarChar, 20).Value = Solicitud.Estado;
                Comando.Parameters.Add("CodigoTraslado", OdbcType.Int).Value = CodigoTraslado;
                Comando.ExecuteNonQuery();
            }
        }

        private void LogisticaMetActualizarObservaciones(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado,
            string Observaciones)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblTraslado SET observacionesTraslado = ? WHERE idTraslado = ?",
                LogisticaMetCrearParametro("Observaciones", OdbcType.VarChar, 500, Observaciones ?? string.Empty),
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, CodigoTraslado));
        }

        private void LogisticaMetActualizarEstado(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado, string Estado)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblTraslado SET estadoTraslado = ?, " +
                "fechaLlegadaTraslado = CURRENT_TIMESTAMP WHERE idTraslado = ?",
                LogisticaMetCrearParametro("Estado", OdbcType.VarChar, 20, Estado),
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, CodigoTraslado));
        }

        private void LogisticaMetActualizarDetalles(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "DELETE FROM tblDetalleTraslado WHERE Fk_Traslado_Codigo = ?",
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, CodigoTraslado));
            LogisticaMetInsertarDetalles(Conexion, Transaccion, CodigoTraslado, Detalles);
        }

        private void LogisticaMetCambiarEstadoTransporte(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTransporte, string EstadoNuevo)
        {
            if (CodigoTransporte <= 0)
            {
                return;
            }

            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT idEstado FROM tblEstado WHERE UPPER(TRIM(tipoEstado)) = ?",
                LogisticaMetCrearParametro("TipoEstado", OdbcType.VarChar, 100, EstadoNuevo));
            if (Resultado == null)
            {
                throw new InvalidOperationException(
                    "Debe existir el estado '" + EstadoNuevo +
                    "' en tblEstado para actualizar la disponibilidad del transporte.");
            }

            int CodigoEstado = Convert.ToInt32(Resultado);
            int Actualizados = LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblTransporte SET Fk_Estado_Codigo = ? WHERE idTransporte = ?",
                LogisticaMetCrearParametro("CodigoEstado", OdbcType.Int, CodigoEstado),
                LogisticaMetCrearParametro("CodigoTransporte", OdbcType.Int, CodigoTransporte));
            if (Actualizados == 0)
            {
                object YaActualizado = LogisticaMetEjecutarEscalar(
                    Conexion, Transaccion,
                    "SELECT COUNT(*) FROM tblTransporte WHERE idTransporte = ? AND Fk_Estado_Codigo = ?",
                    LogisticaMetCrearParametro("CodigoTransporte", OdbcType.Int, CodigoTransporte),
                    LogisticaMetCrearParametro("CodigoEstado", OdbcType.Int, CodigoEstado));
                if (Convert.ToInt32(YaActualizado) == 0)
                {
                    throw new InvalidOperationException(
                        "No se pudo actualizar el estado del transporte asignado.");
                }
            }
        }

        private void LogisticaMetValidarConductorActualizacion(
            OdbcConnection Conexion, OdbcTransaction Transaccion, ClsTrasladoSolicitud Solicitud)
        {
            const string Consulta =
                "SELECT conductor.idConductor FROM tblConductor conductor " +
                "INNER JOIN tblEstado estado ON estado.idEstado = conductor.Fk_Estado_Codigo " +
                "WHERE conductor.idConductor = ? " +
                "AND UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblEncabezadoViaje viaje " +
                "WHERE viaje.Fk_Conductor_Codigo = conductor.idConductor " +
                "AND viaje.estadoViajeEncabezadoViaje = 'EN_CURSO') " +
                "FOR UPDATE";

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoConductor", OdbcType.Int).Value =
                    Solicitud.CodigoConductor;
                if (Comando.ExecuteScalar() == null)
                {
                    throw new InvalidOperationException(
                        "El conductor seleccionado ya no está disponible.");
                }
            }
        }

        private void LogisticaMetValidarTransporte(
            OdbcConnection Conexion, OdbcTransaction Transaccion, ClsTrasladoSolicitud Solicitud)
        {
            const string Consulta =
                "SELECT transporte.idTransporte FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "WHERE transporte.idTransporte = ? " +
                "AND transporte.Fk_Tipo_Transporte_Codigo = ? " +
                "AND UPPER(TRIM(estado.tipoEstado)) = 'DISPONIBLE' " +
                "AND NOT EXISTS (SELECT 1 FROM tblTraslado traslado " +
                "WHERE traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                "AND traslado.estadoTraslado IN ('SOLICITADO', 'EN_TRANSITO')) " +
                "FOR UPDATE";

            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoTransporte", OdbcType.Int).Value = Solicitud.CodigoTransporte;
                Comando.Parameters.Add("CodigoTipoTransporte", OdbcType.Int).Value = Solicitud.CodigoTipoTransporte;
                if (Comando.ExecuteScalar() == null)
                {
                    throw new InvalidOperationException(
                        "Seleccione una placa válida, disponible y correspondiente al tipo de transporte.");
                }
            }
        }

        private int LogisticaMetInsertarEncabezado(
            OdbcConnection Conexion, OdbcTransaction Transaccion, ClsTrasladoSolicitud Solicitud)
        {
            const string Consulta =
                "INSERT INTO tblTraslado " +
                "(codigoTraslado, Fk_Bodega_Origen_Codigo, Fk_Bodega_Destino_Codigo, " +
                "Fk_Transporte_Codigo, fechaSolicitudTraslado, " +
                "estadoTraslado, observacionesTraslado) " +
                "VALUES (?, ?, ?, ?, CURRENT_TIMESTAMP, 'SOLICITADO', ?)";
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion, Consulta,
                LogisticaMetCrearParametro("CodigoTraslado", OdbcType.VarChar, 30, Solicitud.CodigoTraslado),
                LogisticaMetCrearParametro("BodegaOrigen", OdbcType.Int, Solicitud.BodegaOrigen),
                LogisticaMetCrearParametro("BodegaDestino", OdbcType.Int, Solicitud.BodegaDestino),
                LogisticaMetCrearParametroEnteroOpcional("CodigoTransporte", Solicitud.CodigoTransporte),
                LogisticaMetCrearParametro(
                    "Observaciones", OdbcType.VarChar, 500, Solicitud.Observaciones ?? string.Empty));
            return LogisticaMetObtenerUltimoId(Conexion, Transaccion);
        }

        private void LogisticaMetInsertarDetalles(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado,
            IList<ClsProductoTrasladoDetalle> Detalles)
        {
            const string Consulta =
                "INSERT INTO tblDetalleTraslado " +
                "(Fk_Traslado_Codigo, Fk_Producto_Codigo, cantidadDetalleTraslado) VALUES (?, ?, ?)";
            foreach (ClsProductoTrasladoDetalle Detalle in Detalles)
            {
                LogisticaMetEjecutarNoConsulta(
                    Conexion, Transaccion, Consulta,
                    LogisticaMetCrearParametro("CodigoTraslado", OdbcType.Int, CodigoTraslado),
                    LogisticaMetCrearParametro("CodigoProducto", OdbcType.Int, Detalle.CodigoProducto),
                    LogisticaMetCrearParametro("Cantidad", OdbcType.Int, Detalle.Cantidad));
            }
        }
    }
}
