using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using CapaControlador_Traslados_Logistica;
using CapaModelo_Traslados_Logistica.Contratos;
using CapaModelo_Traslados_Logistica.Entidades;

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
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 1);

            Controlador.LogisticaMetGuardarTraslado(
                "QA-TR-SIN-CONDUCTOR", 1, 2, 0, 0, 0, "Pendiente de despacho");

            Assert.IsNotNull(Repositorio.SolicitudGuardada);
            Assert.AreEqual(0, Repositorio.SolicitudGuardada.CodigoConductor);
            Assert.AreEqual(ClsReglasEstadoTraslado.Solicitado, Repositorio.SolicitudGuardada.Estado);
        }

        [TestMethod]
        public void ActualizarTraslado_DespachoSinConductor_DelegaValidacionAlRepositorio()
        {
            var Repositorio = new RepositorioTrasladoFalso();
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetAgregarProducto(CrearProducto(), 1);

            Controlador.LogisticaMetActualizarTraslado(
                "QA-TR-SIN-CONDUCTOR", 1, 2, 1, 5, 0, "",
                ClsReglasEstadoTraslado.EnTransito);

            Assert.IsNotNull(Repositorio.SolicitudActualizada);
            Assert.AreEqual(0, Repositorio.SolicitudActualizada.CodigoConductor);
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
        public void FiltrarTraslados_CombinaCodigoFechaYEstado()
        {
            var Repositorio = new RepositorioTrasladoFalso
            {
                Traslados = CrearTablaTraslados()
            };
            var Controlador = new ClsModeloTraslado(Repositorio);
            Controlador.LogisticaMetCargarTraslados();

            DataTable Resultado = Controlador.LogisticaMetFiltrarTraslados(
                "TR-10", new System.DateTime(2026, 11, 8), "SOLICITADO");

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

        private static DataRow CrearProducto()
        {
            var Productos = new DataTable();
            Productos.Columns.Add("CodigoProducto", typeof(int));
            Productos.Columns.Add("SKU", typeof(string));
            Productos.Columns.Add("Nombre", typeof(string));
            Productos.Columns.Add("Descripcion", typeof(string));
            Productos.Columns.Add("PesoUnitario", typeof(decimal));
            Productos.Columns.Add("ExistenciasDisponibles", typeof(int));
            Productos.Rows.Add(101, "EMB-CH-001", "Chorizo parrillero", "Paquete de 1 kg", 1.0m, 50);
            return Productos.Rows[0];
        }

        private sealed class RepositorioTrasladoFalso : IRepositorioTraslado
        {
            public ClsTrasladoSolicitud SolicitudGuardada { get; private set; }
            public ClsTrasladoSolicitud SolicitudActualizada { get; private set; }
            public DataTable Traslados { get; set; }

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

            public DataTable LogisticaMetObtenerConductoresDisponibles(string CodigoTrasladoExcluir)
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerTransportesDisponibles(int CodigoTipoTransporte)
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerTransportesDisponibles(
                int CodigoTipoTransporte, string CodigoTrasladoExcluir)
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerProductosDisponibles(int CodigoBodega)
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerProductosDisponibles(
                int CodigoBodega, string CodigoTrasladoExcluir)
            {
                return new DataTable();
            }

            public DataTable LogisticaMetObtenerDetalleTraslado(string CodigoTraslado)
            {
                return new DataTable();
            }

            public void LogisticaMetGuardarTraslado(ClsTrasladoSolicitud Solicitud)
            {
                SolicitudGuardada = Solicitud;
            }

            public void LogisticaMetActualizarTraslado(ClsTrasladoSolicitud Solicitud)
            {
                SolicitudActualizada = Solicitud;
            }
        }
    }
}
