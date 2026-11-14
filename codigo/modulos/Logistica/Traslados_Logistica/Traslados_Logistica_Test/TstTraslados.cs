using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using CapaControlador_Traslados_Logistica;
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
 * Verifica las operaciones del controlador de traslados.
 * ===================================================================
 */

namespace Traslados_Logistica_Test
{
    [TestClass]
    public class TstTraslados
    {
        [TestMethod]
        public void GuardarTraslado_ConDatosValidos_PersisteSolicitudYDetalle()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 12);

            // Act
            Controlador.LogisticaMetGuardarTraslado(
                " QA-TR-001 ", 1, 2, 1, 5, 7, "Embutidos refrigerados");

            // Assert
            Assert.IsNotNull(Repositorio.SolicitudGuardada);
            Assert.AreEqual("QA-TR-001", Repositorio.SolicitudGuardada.CodigoTraslado);
            Assert.AreEqual(ClsReglasEstadoTraslado.Solicitado, Repositorio.SolicitudGuardada.Estado);
            Assert.AreEqual(1, Repositorio.SolicitudGuardada.Detalles.Count);
            Assert.AreEqual(12, Repositorio.SolicitudGuardada.Detalles[0].Cantidad);
            Assert.AreEqual(1, Repositorio.SolicitudGuardada.BodegaOrigen);
            Assert.AreEqual(2, Repositorio.SolicitudGuardada.BodegaDestino);
        }

        [TestMethod]
        public void GuardarTraslado_SolicitadoSinConductor_PermiteCrearSolicitud()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 1);

            // Act
            Controlador.LogisticaMetGuardarTraslado(
                "QA-TR-SIN-CONDUCTOR", 1, 2, 0, 0, 0, "Pendiente de despacho");

            // Assert
            Assert.IsNotNull(Repositorio.SolicitudGuardada);
            Assert.AreEqual(0, Repositorio.SolicitudGuardada.CodigoConductor);
            Assert.AreEqual(ClsReglasEstadoTraslado.Solicitado, Repositorio.SolicitudGuardada.Estado);
        }

        [TestMethod]
        public void ActualizarTraslado_DespachoSinConductor_RechazaSolicitud()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 1);

            // Act
            System.InvalidOperationException Error = Assert.ThrowsException<System.InvalidOperationException>(
                () => Controlador.LogisticaMetActualizarTraslado(
                    "QA-TR-ACT-SIN-CONDUCTOR", 1, 2, 1, 5, 0, "",
                    ClsReglasEstadoTraslado.EnTransito));

            // Assert
            Assert.IsNotNull(Error);
            Assert.IsNull(Repositorio.SolicitudActualizada);
        }

        [TestMethod]
        public void ActualizarTraslado_ConDatosValidos_PersisteCambiosDeDespacho()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 8);

            // Act
            Controlador.LogisticaMetActualizarTraslado(
                "QA-TR-003", 1, 2, 1, 5, 7, "Despacho refrigerado",
                ClsReglasEstadoTraslado.EnTransito);

            // Assert
            Assert.IsNotNull(Repositorio.SolicitudActualizada);
            Assert.AreEqual(ClsReglasEstadoTraslado.EnTransito, Repositorio.SolicitudActualizada.Estado);
            Assert.AreEqual(1, Repositorio.SolicitudActualizada.CodigoTipoTransporte);
            Assert.AreEqual(5, Repositorio.SolicitudActualizada.CodigoTransporte);
            Assert.AreEqual("Despacho refrigerado", Repositorio.SolicitudActualizada.Observaciones);
            Assert.AreEqual(8, Repositorio.SolicitudActualizada.Detalles[0].Cantidad);
        }

        [TestMethod]
        public void ActualizarTraslado_AlCancelar_PersisteCambiosYProductos()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 4);

            // Act
            Controlador.LogisticaMetActualizarTraslado(
                "QA-TR-002", 1, 2, 0, 0, 7, "Pedido cancelado antes del despacho",
                ClsReglasEstadoTraslado.Cancelado);

            // Assert
            Assert.IsNotNull(Repositorio.SolicitudActualizada);
            Assert.AreEqual("QA-TR-002", Repositorio.SolicitudActualizada.CodigoTraslado);
            Assert.AreEqual(ClsReglasEstadoTraslado.Cancelado, Repositorio.SolicitudActualizada.Estado);
            Assert.AreEqual("Pedido cancelado antes del despacho",
                Repositorio.SolicitudActualizada.Observaciones);
            Assert.AreEqual(1, Repositorio.SolicitudActualizada.Detalles.Count);
            Assert.AreEqual(4, Repositorio.SolicitudActualizada.Detalles[0].Cantidad);
        }

        [TestMethod]
        public void EliminarProducto_ProductoEnDetalle_LoRetiraDelDetalle()
        {
            // Arrange
            var Controlador = new ClsModeloTraslado(new RepositorioTrasladoFalso());
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 3);

            // Act
            Controlador.LogisticaMetEliminarProducto(101);

            // Assert
            Assert.AreEqual(0, Controlador.LogisticaMetObtenerDetalle().Rows.Count);
        }

        [TestMethod]
        public void ObtenerTransportesDisponibles_FiltraEstadoTipoYOcupacionEnControlador()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso
            {
                Transportes = CrearTablaTransportes()
            };
            var Controlador = new ClsModeloTraslado(Repositorio);

            // Act
            DataTable Resultado = Controlador.LogisticaMetObtenerTransportesDisponibles(1, "TR-EDIT");

            // Assert
            Assert.AreEqual(2, Resultado.Rows.Count);
            Assert.AreEqual(5, Resultado.Rows[0]["CodigoTransporte"]);
            Assert.AreEqual(7, Resultado.Rows[1]["CodigoTransporte"]);
        }

        [TestMethod]
        public void ObtenerProductosDisponibles_CalculaStockYFiltraEnControlador()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso
            {
                Productos = CrearTablaProductosConStock()
            };
            var Controlador = new ClsModeloTraslado(Repositorio);

            // Act
            DataTable Resultado = Controlador.LogisticaMetObtenerProductosDisponibles(1, null);

            // Assert
            Assert.AreEqual(2, Resultado.Rows.Count);
            Assert.AreEqual(9, Resultado.Rows[0]["ExistenciasDisponibles"]);
            Assert.AreEqual(1, Resultado.Rows[1]["ExistenciasDisponibles"]);
        }

        [TestMethod]
        public void FiltrarTraslados_CombinaCodigoFechaYEstado()
        {
            // Arrange
            var Repositorio = new RepositorioTrasladoFalso
            {
                Traslados = CrearTablaTraslados()
            };
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetCargarTraslados();

            // Act
            DataTable Resultado = Controlador.LogisticaMetFiltrarTraslados(
                "TR-10", new System.DateTime(2026, 11, 8), "SOLICITADO");

            // Assert
            Assert.AreEqual(1, Resultado.Rows.Count);
            Assert.AreEqual("TR-100", Resultado.Rows[0]["CodigoTraslado"]);
            Assert.AreEqual("TR-100", Controlador.LogisticaMetIrAInicio()["CodigoTraslado"]);
        }

        private static DataTable CrearTablaTraslados()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoTraslado", typeof(string));
            Tabla.Columns.Add("FechaSolicitud", typeof(System.DateTime));
            Tabla.Columns.Add("Estado", typeof(string));
            Tabla.Rows.Add("TR-100", new System.DateTime(2026, 11, 8, 9, 0, 0), "SOLICITADO");
            Tabla.Rows.Add("TR-101", new System.DateTime(2026, 11, 8, 9, 0, 0), "EN_TRANSITO");
            Tabla.Rows.Add("TR-102", new System.DateTime(2026, 11, 7, 9, 0, 0), "SOLICITADO");
            return Tabla;
        }

        private static DataTable CrearTablaTransportes()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoTransporte", typeof(int));
            Tabla.Columns.Add("PlacaTransporte", typeof(string));
            Tabla.Columns.Add("CodigoTipoTransporte", typeof(int));
            Tabla.Columns.Add("EstadoTransporte", typeof(string));
            Tabla.Columns.Add("CodigoTrasladoAsignado", typeof(string));
            Tabla.Columns.Add("EstadoTrasladoAsignado", typeof(string));
            Tabla.Rows.Add(5, "A-100", 1, "DISPONIBLE", System.DBNull.Value, System.DBNull.Value);
            Tabla.Rows.Add(6, "B-200", 1, "NO DISPONIBLE", "TR-OTHER", "EN_TRANSITO");
            Tabla.Rows.Add(7, "C-300", 1, "NO DISPONIBLE", "TR-EDIT", "SOLICITADO");
            Tabla.Rows.Add(8, "D-400", 2, "DISPONIBLE", System.DBNull.Value, System.DBNull.Value);
            return Tabla;
        }

        private static DataTable CrearTablaProductosConStock()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("CodigoProducto", typeof(int));
            Tabla.Columns.Add("SKU", typeof(string));
            Tabla.Columns.Add("Nombre", typeof(string));
            Tabla.Columns.Add("Descripcion", typeof(string));
            Tabla.Columns.Add("PesoUnitario", typeof(double));
            Tabla.Columns.Add("StockActual", typeof(int));
            Tabla.Columns.Add("CantidadApartada", typeof(int));
            Tabla.Columns.Add("CantidadReservada", typeof(int));
            Tabla.Rows.Add(101, "EMB-CH-001", "Chorizo", "Paquete", 1.0, 10, 3, 2);
            Tabla.Rows.Add(102, "EMB-CH-002", "Longaniza", "Paquete", 0.8, 3, 2, 0);
            Tabla.Rows.Add(103, "EMB-CH-003", "Salami", "Paquete", 0.5, 1, 2, 0);
            return Tabla;
        }

        private static DataRow CrearProducto()
        {
            var Productos = new DataTable();
            Productos.Columns.Add("CodigoProducto", typeof(int));
            Productos.Columns.Add("SKU", typeof(string));
            Productos.Columns.Add("Nombre", typeof(string));
            Productos.Columns.Add("Descripcion", typeof(string));
            Productos.Columns.Add("PesoUnitario", typeof(double));
            Productos.Columns.Add("ExistenciasDisponibles", typeof(int));
            Productos.Rows.Add(101, "EMB-CH-001", "Chorizo parrillero", "Paquete de 1 kg", 1.0, 50);
            return Productos.Rows[0];
        }

        #region Fake necesario para probar los métodos del controlador sin depender de ODBC
        private sealed class RepositorioTrasladoFalso : IRepositorioTraslado
        {
            public ClsTrasladoSolicitud SolicitudGuardada { get; private set; }
            public ClsTrasladoSolicitud SolicitudActualizada { get; private set; }
            public DataTable Traslados { get; set; }
            public DataTable Transportes { get; set; }
            public DataTable Productos { get; set; }

            public DataTable LogisticaMetObtenerTraslados()
            {
                return Traslados ?? new DataTable();
            }

            public DataTable LogisticaMetObtenerBodegas()
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerTiposTransporte()
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerConductores()
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerTransportes()
            {
                return Transportes ?? new DataTable();
            }

            public DataTable LogisticaMetObtenerStockProductos(
                int CodigoBodega, string CodigoTrasladoReserva)
            {
                return Productos ?? new DataTable();
            }

            public DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado)
            {
                return new DataTable();
            }

            public void LogisticaMetEjecutarUnidadTrabajo(
                System.Action<IRepositorioUnidadTrabajoTraslado> Operacion)
            {
                Operacion(new UnidadTrabajoTrasladoFalsa(this));
            }

            private sealed class UnidadTrabajoTrasladoFalsa : IRepositorioUnidadTrabajoTraslado
            {
                private readonly RepositorioTrasladoFalso _Repositorio;
                private readonly Dictionary<int, ClsStockProductoTraslado> _Stocks =
                    new Dictionary<int, ClsStockProductoTraslado>
                {
                    { 101, new ClsStockProductoTraslado
                        {
                            IdStock = 1,
                            CodigoProducto = 101,
                            CodigoSucursal = 1,
                            StockActual = 50,
                            EstadoVenta = true,
                            EstadoCompra = true
                        }
                    }
                };

                public UnidadTrabajoTrasladoFalsa(RepositorioTrasladoFalso Repositorio)
                {
                    _Repositorio = Repositorio;
                }

                public ClsTrasladoPersistido LogisticaMetObtenerTrasladoBloqueado(string CodigoTraslado)
                {
                    if (CodigoTraslado == "QA-TR-001" ||
                        CodigoTraslado == "QA-TR-SIN-CONDUCTOR")
                    {
                        return null;
                    }

                    return new ClsTrasladoPersistido
                    {
                        CodigoInterno = 10,
                        CodigoTraslado = CodigoTraslado,
                        Estado = ClsReglasEstadoTraslado.Solicitado,
                        BodegaOrigen = 1,
                        BodegaDestino = 2
                    };
                }

                public int LogisticaMetObtenerCantidadServiciosExternos(int CodigoTraslado) { return 0; }

                public DataTable LogisticaMetObtenerTransporte(int CodigoTransporte)
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("CodigoTransporte", typeof(int));
                    Datos.Columns.Add("CodigoTipoTransporte", typeof(int));
                    Datos.Columns.Add("EstadoTransporte", typeof(string));
                    Datos.Columns.Add("CodigoTrasladoAsignado", typeof(string));
                    Datos.Columns.Add("EstadoTrasladoAsignado", typeof(string));
                    Datos.Rows.Add(CodigoTransporte, 1, "DISPONIBLE", System.DBNull.Value,
                        System.DBNull.Value);
                    return Datos;
                }

                public DataTable LogisticaMetObtenerConductor(int CodigoConductor)
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("CodigoConductor", typeof(int));
                    Datos.Columns.Add("EstadoConductor", typeof(string));
                    Datos.Columns.Add("EstadoViaje", typeof(string));
                    Datos.Rows.Add(CodigoConductor, "DISPONIBLE", System.DBNull.Value);
                    return Datos;
                }

                public object LogisticaMetObtenerRefrigeracionTransporte(int CodigoTransporte)
                {
                    return false;
                }

                public DataTable LogisticaMetObtenerRefrigeracionLotesProducto(int CodigoProducto)
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("RequiereFrio", typeof(bool));
                    return Datos;
                }

                public Dictionary<int, ClsStockProductoTraslado> LogisticaMetObtenerStocksBloqueados(
                    int CodigoBodega) { return _Stocks; }

                public Dictionary<int, int> LogisticaMetObtenerDetallesBloqueados(int CodigoTraslado)
                {
                    return new Dictionary<int, int>();
                }

                public int LogisticaMetInsertarTraslado(ClsTrasladoSolicitud Solicitud)
                {
                    _Repositorio.SolicitudGuardada = Solicitud;
                    return 11;
                }

                public void LogisticaMetInsertarDetalles(int CodigoTraslado,
                    IList<ClsProductoTrasladoDetalle> Detalles) { }

                public void LogisticaMetActualizarEncabezado(int CodigoTraslado,
                    ClsTrasladoSolicitud Solicitud)
                {
                    _Repositorio.SolicitudActualizada = Solicitud;
                }

                public void LogisticaMetActualizarObservaciones(int CodigoTraslado, string Observaciones) { }
                public void LogisticaMetActualizarEstado(int CodigoTraslado, string Estado) { }
                public void LogisticaMetActualizarDetalles(int CodigoTraslado,
                    IList<ClsProductoTrasladoDetalle> Detalles) { }
                public int LogisticaMetCambiarEstadoTransporte(int CodigoTransporte, string EstadoNuevo)
                {
                    return 1;
                }
                public DataTable LogisticaMetObtenerTiposMovimiento()
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("CodigoTipoMovimiento", typeof(int));
                    Datos.Columns.Add("Signo", typeof(string));
                    Datos.Columns.Add("Nombre", typeof(string));
                    Datos.Rows.Add(1, "-", "Salida por traslado");
                    Datos.Rows.Add(2, "+", "Entrada por traslado");
                    return Datos;
                }

                public int LogisticaMetObtenerSucursalBodega(int CodigoBodega) { return 1; }
                public DataTable LogisticaMetObtenerLotesProducto(int CodigoProducto)
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("CodigoLote", typeof(long));
                    Datos.Columns.Add("FechaVencimiento", typeof(System.DateTime));
                    return Datos;
                }

                public DataTable LogisticaMetObtenerLotesMovimientosTraslado(
                    int CodigoTraslado, int CodigoProducto)
                {
                    var Datos = new DataTable();
                    Datos.Columns.Add("CodigoMovimiento", typeof(int));
                    Datos.Columns.Add("Signo", typeof(string));
                    Datos.Columns.Add("EstadoMovimiento", typeof(string));
                    Datos.Columns.Add("CodigoLote", typeof(long));
                    return Datos;
                }
                public int LogisticaMetInsertarMovimiento(int TipoMovimiento,
                    ClsTrasladoPersistido Traslado, string Observaciones) { return 1; }
                public void LogisticaMetInsertarDetalleMovimiento(int CodigoMovimiento,
                    ClsProductoTrasladoDetalle Detalle, object LotePreferido) { }
                public void LogisticaMetAnularMovimientosSalida(int CodigoTraslado) { }
                public int LogisticaMetInsertarStockDestino(int CodigoSucursal, int CodigoBodega,
                    int CodigoProducto, int Cantidad, bool EstadoVenta, bool EstadoCompra) { return 2; }
                public void LogisticaMetActualizarSaldo(int CodigoBodega,
                    int CodigoProducto, int SaldoActual) { }

                public int LogisticaMetActualizarStock(int IdStock, int StockAnterior,
                    int StockActual, int Apartados, int Cargos, int Abonos)
                {
                    foreach (ClsStockProductoTraslado Stock in _Stocks.Values)
                    {
                        if (Stock.IdStock == IdStock)
                        {
                            Stock.StockActual = StockActual;
                            Stock.Apartados = Apartados;
                            return 1;
                        }
                    }

                    return 0;
                }

                public int LogisticaMetActualizarApartados(int IdStock, int Apartados, int Virtual)
                {
                    foreach (ClsStockProductoTraslado Stock in _Stocks.Values)
                    {
                        if (Stock.IdStock == IdStock)
                        {
                            Stock.Apartados = Apartados;
                            return 1;
                        }
                    }

                    return 0;
                }

                public long LogisticaMetInsertarEncabezadoViaje(bool RequiereCadenaFrio,
                    double PesoTotal, int CodigoTransporte, int CodigoConductor) { return 1; }
                public long LogisticaMetInsertarDetalleViaje(long CodigoViaje, int Orden,
                    string Descripcion, int Cantidad, double PesoTotal) { return Orden; }
                public void LogisticaMetInsertarDetalleViajeLote(long CodigoDetalle,
                    long CodigoLote, int Cantidad, double PesoTotal) { }
                public void LogisticaMetActualizarViajeActivo(int CodigoTransporte,
                    string Estado, bool RegistrarLlegada) { }
            }
        }
        #endregion
    }
}
