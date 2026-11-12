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

        public void LogisticaMetEjecutarUnidadTrabajo(
            Action<IRepositorioUnidadTrabajoTraslado> Operacion)
        {
            LogisticaMetEjecutarTransaccion(Conexion =>
            {
                using (var Transaccion = Conexion.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        Operacion(new UnidadTrabajoTraslado(
                            this, Conexion, Transaccion, _Inventario, _Viaje));
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

        public DataTable LogisticaMetObtenerConductores()
        {
            const string Consulta =
                "SELECT conductor.idConductor AS CodigoConductor, " +
                "CONCAT(conductor.codigoConductor, ' - ', conductor.nombreCompletoConductor) " +
                "AS NombreConductor, UPPER(TRIM(estado.tipoEstado)) AS EstadoConductor, " +
                "UPPER(TRIM(viaje.estadoViajeEncabezadoViaje)) AS EstadoViaje " +
                "FROM tblConductor conductor " +
                "INNER JOIN tblEstado estado ON estado.idEstado = conductor.Fk_Estado_Codigo " +
                "LEFT JOIN tblEncabezadoViaje viaje ON " +
                "viaje.Fk_Conductor_Codigo = conductor.idConductor " +
                "ORDER BY conductor.nombreCompletoConductor";
            return LogisticaMetEjecutarConsulta(Consulta);
        }

        public DataTable LogisticaMetObtenerTransportes()
        {
            const string Consulta =
                "SELECT transporte.idTransporte AS CodigoTransporte, " +
                "transporte.nroPlacaTransporte AS PlacaTransporte, " +
                "transporte.Fk_Tipo_Transporte_Codigo AS CodigoTipoTransporte, " +
                "UPPER(TRIM(estado.tipoEstado)) AS EstadoTransporte, " +
                "traslado.codigoTraslado AS CodigoTrasladoAsignado, " +
                "UPPER(TRIM(traslado.estadoTraslado)) AS EstadoTrasladoAsignado " +
                "FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "LEFT JOIN tblTraslado traslado ON " +
                "traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                "ORDER BY transporte.nroPlacaTransporte";
            return LogisticaMetEjecutarConsulta(Consulta);
        }

        public DataTable LogisticaMetObtenerStockProductos(
            int CodigoBodega, string CodigoTrasladoReserva)
        {
            string JoinReserva = string.Empty;
            string CantidadReserva = "0";
            if (!string.IsNullOrWhiteSpace(CodigoTrasladoReserva))
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
                "stock.stockActualStock AS StockActual, " +
                "stock.apartadosStock AS CantidadApartada, " +
                CantidadReserva + " AS CantidadReservada " +
                "FROM tblStock stock " +
                "INNER JOIN tblProducto producto ON producto.idProducto = stock.Fk_Producto_Codigo " +
                JoinReserva +
                "WHERE stock.Fk_Bodega_Codigo = ? " +
                "ORDER BY producto.nombreProducto";

            if (string.IsNullOrWhiteSpace(CodigoTrasladoReserva))
            {
                return LogisticaMetEjecutarConsulta(
                    Consulta,
                    new OdbcParameter("CodigoBodega", OdbcType.Int) { Value = CodigoBodega });
            }

            return LogisticaMetEjecutarConsulta(
                Consulta,
                new OdbcParameter("CodigoTrasladoReserva", OdbcType.VarChar, 30)
                {
                    Value = CodigoTrasladoReserva
                },
                new OdbcParameter("CodigoBodega", OdbcType.Int) { Value = CodigoBodega });
        }

        public DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado)
        {
            const string Consulta =
                "SELECT producto.idProducto AS CodigoProducto, producto.skuProducto AS SKU, " +
                "producto.nombreProducto AS Nombre, producto.descripcionProducto AS Descripcion, " +
                "detalle.cantidadDetalleTraslado AS Cantidad, producto.pesoProducto AS PesoUnitario, " +
                "producto.precioCostoProducto AS PrecioUnitario, " +
                "COALESCE(stock.stockActualStock, 0) AS StockActual, " +
                "COALESCE(stock.apartadosStock, 0) AS CantidadApartada " +
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

        private int LogisticaMetObtenerCantidadServiciosExternos(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTraslado)
        {
            object Resultado = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT COUNT(*) FROM tblServicioTransporteExterno " +
                "WHERE Fk_Traslado_Codigo = ?",
                LogisticaMetCrearParametro("Traslado", OdbcType.Int, CodigoTraslado));
            return Convert.ToInt32(Resultado);
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

        private DataTable LogisticaMetObtenerTransporte(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoTransporte)
        {
            const string Consulta =
                "SELECT transporte.idTransporte AS CodigoTransporte, " +
                "transporte.Fk_Tipo_Transporte_Codigo AS CodigoTipoTransporte, " +
                "UPPER(TRIM(estado.tipoEstado)) AS EstadoTransporte, " +
                "traslado.codigoTraslado AS CodigoTrasladoAsignado, " +
                "UPPER(TRIM(traslado.estadoTraslado)) AS EstadoTrasladoAsignado " +
                "FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "LEFT JOIN tblTraslado traslado ON traslado.Fk_Transporte_Codigo = transporte.idTransporte " +
                "WHERE transporte.idTransporte = ? FOR UPDATE";
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoTransporte", OdbcType.Int).Value = CodigoTransporte;
                using (var Lector = Comando.ExecuteReader())
                {
                    Tabla.Load(Lector);
                }
            }

            return Tabla;
        }

        private DataTable LogisticaMetObtenerConductor(
            OdbcConnection Conexion, OdbcTransaction Transaccion, int CodigoConductor)
        {
            const string Consulta =
                "SELECT conductor.idConductor AS CodigoConductor, " +
                "UPPER(TRIM(estado.tipoEstado)) AS EstadoConductor, " +
                "UPPER(TRIM(viaje.estadoViajeEncabezadoViaje)) AS EstadoViaje " +
                "FROM tblConductor conductor " +
                "INNER JOIN tblEstado estado ON estado.idEstado = conductor.Fk_Estado_Codigo " +
                "LEFT JOIN tblEncabezadoViaje viaje ON viaje.Fk_Conductor_Codigo = conductor.idConductor " +
                "WHERE conductor.idConductor = ? FOR UPDATE";
            var Tabla = new DataTable();
            using (var Comando = new OdbcCommand(Consulta, Conexion, Transaccion))
            {
                Comando.Parameters.Add("CodigoConductor", OdbcType.Int).Value = CodigoConductor;
                using (var Lector = Comando.ExecuteReader())
                {
                    Tabla.Load(Lector);
                }
            }

            return Tabla;
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
                "fechaLlegadaTraslado = CASE WHEN ? = 'RECIBIDO' " +
                "THEN CURRENT_TIMESTAMP ELSE fechaLlegadaTraslado END WHERE idTraslado = ?",
                LogisticaMetCrearParametro("Estado", OdbcType.VarChar, 20, Estado),
                LogisticaMetCrearParametro("EstadoLlegada", OdbcType.VarChar, 20, Estado),
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

        private int LogisticaMetCambiarEstadoTransporte(
            OdbcConnection Conexion, OdbcTransaction Transaccion,
            int CodigoTransporte, string EstadoNuevo)
        {
            LogisticaMetEjecutarNoConsulta(
                Conexion, Transaccion,
                "UPDATE tblTransporte SET Fk_Estado_Codigo = COALESCE(" +
                "(SELECT idEstado FROM tblEstado WHERE UPPER(TRIM(tipoEstado)) = ? LIMIT 1), " +
                "Fk_Estado_Codigo) WHERE idTransporte = ?",
                LogisticaMetCrearParametro("TipoEstado", OdbcType.VarChar, 100, EstadoNuevo),
                LogisticaMetCrearParametro("CodigoTransporte", OdbcType.Int, CodigoTransporte));
            object Coincidencias = LogisticaMetEjecutarEscalar(
                Conexion, Transaccion,
                "SELECT COUNT(*) FROM tblTransporte transporte " +
                "INNER JOIN tblEstado estado ON estado.idEstado = transporte.Fk_Estado_Codigo " +
                "WHERE transporte.idTransporte = ? AND UPPER(TRIM(estado.tipoEstado)) = ?",
                LogisticaMetCrearParametro("CodigoTransporte", OdbcType.Int, CodigoTransporte),
                LogisticaMetCrearParametro("TipoEstado", OdbcType.VarChar, 100, EstadoNuevo));
            return Convert.ToInt32(Coincidencias);
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

        private sealed class UnidadTrabajoTraslado : IRepositorioUnidadTrabajoTraslado
        {
            private readonly ClsRepositorioTraslado _Repositorio;
            private readonly OdbcConnection _Conexion;
            private readonly OdbcTransaction _Transaccion;
            private readonly ClsRepositorioInventarioTraslado _Inventario;
            private readonly ClsRepositorioViajeTraslado _Viaje;

            public UnidadTrabajoTraslado(ClsRepositorioTraslado Repositorio,
                OdbcConnection Conexion, OdbcTransaction Transaccion,
                ClsRepositorioInventarioTraslado Inventario,
                ClsRepositorioViajeTraslado Viaje)
            {
                _Repositorio = Repositorio;
                _Conexion = Conexion;
                _Transaccion = Transaccion;
                _Inventario = Inventario;
                _Viaje = Viaje;
            }

            public ClsTrasladoPersistido LogisticaMetObtenerTrasladoBloqueado(string CodigoTraslado)
            {
                return _Repositorio.LogisticaMetObtenerTrasladoBloqueado(
                    _Conexion, _Transaccion, CodigoTraslado);
            }

            public int LogisticaMetObtenerCantidadServiciosExternos(int CodigoTraslado)
            {
                return _Repositorio.LogisticaMetObtenerCantidadServiciosExternos(
                    _Conexion, _Transaccion, CodigoTraslado);
            }

            public DataTable LogisticaMetObtenerTransporte(int CodigoTransporte)
            {
                return _Repositorio.LogisticaMetObtenerTransporte(
                    _Conexion, _Transaccion, CodigoTransporte);
            }

            public DataTable LogisticaMetObtenerConductor(int CodigoConductor)
            {
                return _Repositorio.LogisticaMetObtenerConductor(
                    _Conexion, _Transaccion, CodigoConductor);
            }

            public object LogisticaMetObtenerRefrigeracionTransporte(int CodigoTransporte)
            {
                return _Viaje.LogisticaMetObtenerRefrigeracionTransporte(
                    _Conexion, _Transaccion, CodigoTransporte);
            }

            public DataTable LogisticaMetObtenerRefrigeracionLotesProducto(int CodigoProducto)
            {
                return _Inventario.LogisticaMetObtenerRefrigeracionLotesProducto(
                    _Conexion, _Transaccion, CodigoProducto);
            }

            public Dictionary<int, ClsStockProductoTraslado> LogisticaMetObtenerStocksBloqueados(
                int CodigoBodega)
            {
                return _Inventario.LogisticaMetObtenerStocksBloqueados(
                    _Conexion, _Transaccion, CodigoBodega);
            }

            public Dictionary<int, int> LogisticaMetObtenerDetallesBloqueados(int CodigoTraslado)
            {
                return _Repositorio.LogisticaMetObtenerDetallesBloqueados(
                    _Conexion, _Transaccion, CodigoTraslado);
            }

            public int LogisticaMetInsertarTraslado(ClsTrasladoSolicitud Solicitud)
            {
                return _Repositorio.LogisticaMetInsertarEncabezado(
                    _Conexion, _Transaccion, Solicitud);
            }

            public void LogisticaMetInsertarDetalles(int CodigoTraslado,
                IList<ClsProductoTrasladoDetalle> Detalles)
            {
                _Repositorio.LogisticaMetInsertarDetalles(
                    _Conexion, _Transaccion, CodigoTraslado, Detalles);
            }

            public void LogisticaMetActualizarEncabezado(int CodigoTraslado,
                ClsTrasladoSolicitud Solicitud)
            {
                _Repositorio.LogisticaMetActualizarEncabezado(
                    _Conexion, _Transaccion, CodigoTraslado, Solicitud);
            }

            public void LogisticaMetActualizarObservaciones(int CodigoTraslado, string Observaciones)
            {
                _Repositorio.LogisticaMetActualizarObservaciones(
                    _Conexion, _Transaccion, CodigoTraslado, Observaciones);
            }

            public void LogisticaMetActualizarEstado(int CodigoTraslado, string Estado)
            {
                _Repositorio.LogisticaMetActualizarEstado(
                    _Conexion, _Transaccion, CodigoTraslado, Estado);
            }

            public void LogisticaMetActualizarDetalles(int CodigoTraslado,
                IList<ClsProductoTrasladoDetalle> Detalles)
            {
                _Repositorio.LogisticaMetActualizarDetalles(
                    _Conexion, _Transaccion, CodigoTraslado, Detalles);
            }

            public int LogisticaMetCambiarEstadoTransporte(int CodigoTransporte, string EstadoNuevo)
            {
                return _Repositorio.LogisticaMetCambiarEstadoTransporte(
                    _Conexion, _Transaccion, CodigoTransporte, EstadoNuevo);
            }

            public DataTable LogisticaMetObtenerTiposMovimiento()
            {
                return _Inventario.LogisticaMetObtenerTiposMovimiento(
                    _Conexion, _Transaccion);
            }

            public int LogisticaMetObtenerSucursalBodega(int CodigoBodega)
            {
                return _Inventario.LogisticaMetObtenerSucursalBodega(
                    _Conexion, _Transaccion, CodigoBodega);
            }

            public DataTable LogisticaMetObtenerLotesProducto(int CodigoProducto)
            {
                return _Inventario.LogisticaMetObtenerLotesProducto(
                    _Conexion, _Transaccion, CodigoProducto);
            }

            public DataTable LogisticaMetObtenerLotesMovimientosTraslado(
                int CodigoTraslado, int CodigoProducto)
            {
                return _Inventario.LogisticaMetObtenerLotesMovimientosTraslado(
                    _Conexion, _Transaccion, CodigoTraslado, CodigoProducto);
            }

            public int LogisticaMetInsertarMovimiento(int TipoMovimiento,
                ClsTrasladoPersistido Traslado, string Observaciones)
            {
                return _Inventario.LogisticaMetInsertarMovimiento(
                    _Conexion, _Transaccion, TipoMovimiento, Traslado, Observaciones);
            }

            public void LogisticaMetInsertarDetalleMovimiento(int CodigoMovimiento,
                ClsProductoTrasladoDetalle Detalle, object LotePreferido)
            {
                _Inventario.LogisticaMetInsertarDetalleMovimiento(
                    _Conexion, _Transaccion, CodigoMovimiento, Detalle, LotePreferido);
            }

            public void LogisticaMetAnularMovimientosSalida(int CodigoTraslado)
            {
                _Inventario.LogisticaMetAnularMovimientosSalida(
                    _Conexion, _Transaccion, CodigoTraslado);
            }

            public int LogisticaMetInsertarStockDestino(int CodigoSucursal, int CodigoBodega,
                int CodigoProducto, int Cantidad, bool EstadoVenta, bool EstadoCompra)
            {
                return _Inventario.LogisticaMetInsertarStockDestino(
                    _Conexion, _Transaccion, CodigoSucursal, CodigoBodega, CodigoProducto,
                    Cantidad, EstadoVenta, EstadoCompra);
            }

            public void LogisticaMetActualizarSaldo(int CodigoBodega, int CodigoProducto, int SaldoActual)
            {
                _Inventario.LogisticaMetSincronizarSaldo(
                    _Conexion, _Transaccion, CodigoBodega, CodigoProducto, SaldoActual);
            }

            public int LogisticaMetActualizarStock(int IdStock, int StockAnterior, int StockActual,
                int Apartados, int Cargos, int Abonos)
            {
                return _Inventario.LogisticaMetActualizarStock(
                    _Conexion, _Transaccion, IdStock, StockAnterior, StockActual,
                    Apartados, Cargos, Abonos);
            }

            public int LogisticaMetActualizarApartados(int IdStock, int Apartados, int Virtual)
            {
                return _Inventario.LogisticaMetActualizarApartados(
                    _Conexion, _Transaccion, IdStock, Apartados, Virtual);
            }

            public long LogisticaMetInsertarEncabezadoViaje(bool RequiereCadenaFrio,
                double PesoTotal, int CodigoTransporte, int CodigoConductor)
            {
                return _Viaje.LogisticaMetInsertarEncabezadoViaje(
                    _Conexion, _Transaccion, RequiereCadenaFrio, PesoTotal,
                    CodigoTransporte, CodigoConductor);
            }

            public long LogisticaMetInsertarDetalleViaje(long CodigoViaje, int Orden,
                string Descripcion, int Cantidad, double PesoTotal)
            {
                return _Viaje.LogisticaMetInsertarDetalleViaje(
                    _Conexion, _Transaccion, CodigoViaje, Orden,
                    Descripcion, Cantidad, PesoTotal);
            }

            public void LogisticaMetInsertarDetalleViajeLote(long CodigoDetalle,
                long CodigoLote, int Cantidad, double PesoTotal)
            {
                _Viaje.LogisticaMetInsertarDetalleViajeLote(
                    _Conexion, _Transaccion, CodigoDetalle, CodigoLote, Cantidad, PesoTotal);
            }

            public void LogisticaMetActualizarViajeActivo(int CodigoTransporte,
                string Estado, bool RegistrarLlegada)
            {
                _Viaje.LogisticaMetActualizarViajeActivo(
                    _Conexion, _Transaccion, CodigoTransporte, Estado, RegistrarLlegada);
            }
        }
    }
}
