using System;
using System.Data;
using System.Linq;
using CapaControlador_Pedidos;
using CapaModelo_Pedidos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPedidos
{
    [TestClass]
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Comprueba las reglas y acciones de Pedidos.
    public class TestPedidos
    {
        private static DataRow VentasFuncPedido(int Id = 0, int Cotizacion = 0)
        {
            var Tabla = new DataTable();
            foreach (string Campo in new[] { "Id", "CotizacionId", "ClienteId", "VendedorId", "MonedaId" })
                Tabla.Columns.Add(Campo, typeof(int));
            foreach (string Campo in new[] { "Fecha", "Entrega", "Vencimiento" })
                Tabla.Columns.Add(Campo, typeof(DateTime));
            Tabla.Columns.Add("Estado");
            Tabla.Columns.Add("NuevoEstado");
            Tabla.Columns.Add("Numero");
            return Tabla.Rows.Add(Id, Cotizacion == 0 ? (object)DBNull.Value : Cotizacion,
                2, 7, 1, new DateTime(2026, 11, 1), new DateTime(2026, 11, 3),
                new DateTime(2026, 11, 8), "PENDIENTE", "PENDIENTE", "");
        }

        private static DataTable VentasFuncLineas()
        {
                    var Lineas = CapaControlador_Pedidos.ClsPedidoControlador.VentasFuncLineas();
            Lineas.Rows.Add(3, "SKU", "Producto", 2m, 10m, 1m, 0m);
            return Lineas;
        }

        [TestMethod]
        public void Guardar_Independiente_GuardaNumero()
        {
            //Arrange
            var Falso = new RepositorioFalso();
            var Acciones = new ClsPedidoAcciones(0, new ClsPedidoControlador(Falso)) { Moneda = 1 };
            Acciones.VentasMetLinea(VentasFuncLineas().Rows[0]);
            //Act
            string Numero = Acciones.VentasFuncGuardar(2, 7, false,
                new DateTime(2026, 11, 1), new DateTime(2026, 11, 3),
                new DateTime(2026, 11, 8), "");
            //Assert
            Assert.AreEqual("PED-00000012", Numero);
            Assert.AreEqual(1, Falso.Guardados);
            Assert.AreEqual(19m, Acciones.Lineas.Rows[0]["Total"]);
        }

        [TestMethod]
        public void Guardar_DesdeCotizacion_CargaYGuardaLineas()
        {
            //Arrange
            var Falso = new RepositorioFalso { Cotizaciones = new DataTable() };
            foreach (string Campo in new[] { "CotizacionId", "Nombre", "VendedorId", "MonedaId",
                "ProductoId", "Codigo", "Descripcion", "Cantidad", "Precio", "Descuento" })
                Falso.Cotizaciones.Columns.Add(Campo);
            Falso.Cotizaciones.Rows.Add(5, "COT-5", 7, 1, 3, "SKU", "Producto", 2, 10, 1);
            var Acciones = new ClsPedidoAcciones(0, new ClsPedidoControlador(Falso));
            //Act
            DataTable Opciones = Acciones.VentasFuncCotizaciones(2, new DateTime(2026, 11, 1));
            int Vendedor = Acciones.VentasFuncCotizacion(5);
            string Numero = Acciones.VentasFuncGuardar(2, Vendedor, true,
                new DateTime(2026, 11, 1), new DateTime(2026, 11, 3),
                new DateTime(2026, 11, 8), "");
            //Assert
            Assert.AreEqual(1, Opciones.Rows.Count);
            Assert.AreEqual(19m, Acciones.Lineas.Rows[0]["Total"]);
            Assert.AreEqual("PED-00000012", Numero);
            Assert.AreEqual(5, Falso.CotizacionConsultada);
        }

        [TestMethod]
        public void Guardar_CotizacionVencida_Rechaza()
        {
            //Arrange
            var Falso = new RepositorioFalso { CotizacionVigente = false };
            var Controlador = new ClsPedidoControlador(Falso);
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(VentasFuncPedido(0, 5), VentasFuncLineas());
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
            Assert.AreEqual(0, Falso.Guardados);
        }

        [TestMethod]
        public void Guardar_SinProductos_Rechaza()
        {
            //Arrange
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            DataTable Lineas = CapaControlador_Pedidos.ClsPedidoControlador.VentasFuncLineas();
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(VentasFuncPedido(), Lineas);
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Guardar_SinCliente_Rechaza()
        {
            //Arrange
            DataRow Pedido = VentasFuncPedido();
            Pedido["ClienteId"] = 0;
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(Pedido, VentasFuncLineas());
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Guardar_DescuentoExcesivo_Rechaza()
        {
            //Arrange
            DataTable Lineas = VentasFuncLineas();
            Lineas.Rows[0]["Descuento"] = 30m;
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(VentasFuncPedido(), Lineas);
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Guardar_CantidadFraccionaria_Rechaza()
        {
            //Arrange
            DataTable Lineas = VentasFuncLineas();
            Lineas.Rows[0]["Cantidad"] = 1.5m;
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            //Assert
            Assert.ThrowsException<ArgumentException>(() => Controlador.VentasFuncGuardar(VentasFuncPedido(), Lineas));
        }

        [TestMethod]
        public void Guardar_ReferenciaMayorDe30_Rechaza()
        {
            //Arrange
            DataRow Pedido = VentasFuncPedido(8);
            Pedido["Numero"] = new string('P', 31);
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(Pedido, VentasFuncLineas());
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Guardar_EntregaAnterior_Rechaza()
        {
            //Arrange
            DataRow Pedido = VentasFuncPedido();
            Pedido["Entrega"] = new DateTime(2026, 10, 31);
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardar(Pedido, VentasFuncLineas());
            //Assert
            Assert.ThrowsException<ArgumentException>(Operacion);
        }

        [TestMethod]
        public void Guardar_Existente_ConservaId()
        {
            //Arrange
            var Controlador = new ClsPedidoControlador(new RepositorioFalso());
            //Act
            int Id = Controlador.VentasFuncGuardar(VentasFuncPedido(8), VentasFuncLineas());
            //Assert
            Assert.AreEqual(8, Id);
        }

        [TestMethod]
        public void Anular_PedidoValido_EnviaAlModelo()
        {
            //Arrange
            var Falso = new RepositorioFalso();
            var Acciones = new ClsPedidoAcciones(8, new ClsPedidoControlador(Falso));
            //Act
            Acciones.VentasMetEstado("ANULADO");
            //Assert
            Assert.AreEqual("ANULADO", Falso.Estado);
            Assert.AreEqual(8, Falso.IdEstado);
        }

        [TestMethod]
        public void Guardar_Existente_PermiteCambiarEstado()
        {
            //Arrange
            var Falso = new RepositorioFalso();
            DataRow Pedido = VentasFuncPedido(8);
            Pedido["NuevoEstado"] = "COMPLETADO";
            //Act
            new ClsPedidoControlador(Falso).VentasFuncGuardar(Pedido, VentasFuncLineas());
            //Assert
            Assert.AreEqual("COMPLETADO", Falso.NuevoEstado);
        }

        [TestMethod]
        public void Guardar_ExistenteAnulado_RechazaEdicion()
        {
            //Arrange
            DataRow Pedido = VentasFuncPedido(8);
            Pedido["Estado"] = "ANULADO";
            Pedido["NuevoEstado"] = "PENDIENTE";
            //Act
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() =>
                new ClsPedidoControlador(new RepositorioFalso()).VentasFuncGuardar(Pedido, VentasFuncLineas()));
        }

        [TestMethod]
        public void Reporte_RespetaEstadoSeleccionado()
        {
            //Arrange
            var Falso = new RepositorioFalso();
            //Act
            new ClsPedidoControlador(Falso).VentasFuncReporte(
                new DateTime(2026, 11, 1), new DateTime(2026, 11, 30), "ANULADO");
            //Assert
            Assert.AreEqual("ANULADO", Falso.EstadoReporte);
        }

        [TestMethod]
        public void Reporte_RechazaEstadoDesconocido()
        {
            //Arrange
            //Act
            //Assert
            Assert.ThrowsException<ArgumentException>(() =>
                new ClsPedidoControlador(new RepositorioFalso()).VentasFuncReporte(
                    new DateTime(2026, 11, 1), new DateTime(2026, 11, 30), "OTRO"));
        }

        [TestMethod]
        public void VentasMetCalculaTotalesConDescuento()
        {
            //Arrange
            var Acciones = new ClsPedidoAcciones(0, new ClsPedidoControlador(new RepositorioFalso()));
            DataTable Productos = ClsPedidoControlador.VentasFuncLineas();
            DataRow Producto = Productos.Rows.Add(1, "P01", "Producto", 1m, 12.50m, 0m, 12.50m);

            //Act
            Acciones.VentasMetProducto(Producto, 3m, 2.25m);

            //Assert
            CollectionAssert.AreEqual(new[] { 37.50m, 35.25m }, Acciones.VentasFuncTotales());
        }

        [TestMethod]
        public void VentasMetRechazaFechasInvertidasYEstadoInvalido()
        {
            //Arrange
            var Repositorio = new RepositorioFalso();
            var Controlador = new ClsPedidoControlador(Repositorio);
            var Fecha = new DateTime(2026, 11, 6);

            //Act
            //Assert
            Assert.ThrowsException<ArgumentException>(() =>
                Controlador.VentasFuncReporte(Fecha, Fecha.AddDays(-1), "ANULADO"));
            Assert.ThrowsException<ArgumentException>(() => Controlador.VentasMetEstado(5, "OTRO"));
            Assert.IsNull(Repositorio.EstadoReporte);
            Assert.IsNull(Repositorio.Estado);
        }

        [TestMethod]
        public void VentasMetConsultaContactoDePedidoEnBaseDePruebas()
        {
            //Arrange
            int IdPedido;
            if (!int.TryParse(Environment.GetEnvironmentVariable("VENTAS_TEST_PEDIDO_ID"), out IdPedido) ||
                IdPedido <= 0)
                Assert.Inconclusive("Configura VENTAS_TEST_PEDIDO_ID con un pedido de prueba cuyo cliente tenga contacto.");

            var Controlador = new ClsPedidoControlador();
            //Act
            DataTable Pedido = Controlador.VentasFuncPedido(IdPedido);
            //Assert
            Assert.IsTrue(Pedido.Rows.Count > 0, "El pedido de prueba no existe.");
            DataRow Datos = Pedido.Rows[0];
            string Contacto = Convert.ToString(Datos["contactoCliente"]);
            Assert.IsFalse(string.IsNullOrWhiteSpace(Contacto), "El cliente de prueba no tiene contacto.");

            DataRow[] Clientes = Controlador.VentasFuncCatalogo("Clientes").Select("Id = " + Datos["ClienteId"]);
            if (Clientes.Length > 0)
                Assert.AreEqual(Contacto, Convert.ToString(Clientes[0]["contactoCliente"]));

            int IdCliente = Convert.ToInt32(Datos["ClienteId"]);
            DateTime Fecha = Convert.ToDateTime(Datos["Fecha"]);
            string Estado = Convert.ToString(Datos["Estado"]);
            Assert.IsFalse(string.IsNullOrWhiteSpace(Controlador.VentasFuncMoneda(Convert.ToInt32(Datos["MonedaId"]))));
            Assert.IsTrue(Controlador.VentasFuncCatalogo("Vendedores").Columns.Contains("Id"));
            Assert.IsTrue(Controlador.VentasFuncListar().Select("Id = " + IdPedido).Length > 0);

            DataTable Reporte = Controlador.VentasFuncReporte(Fecha, Fecha, Estado);
            Assert.IsTrue(Reporte.Rows.Cast<DataRow>().Any(Fila => Equals(Fila["Numero"], Datos["Numero"])));

            DataTable Listas = Controlador.VentasFuncListas(IdCliente, Fecha);
            Assert.IsTrue(Listas.Columns.Contains("TipoClienteId"));
            foreach (DataRow Lista in Listas.Rows)
                Assert.IsTrue(Controlador.VentasFuncProductos(Convert.ToInt32(Lista["Id"])).Columns.Contains("Precio"));

            Assert.IsTrue(Controlador.VentasFuncCotizaciones(IdCliente, Fecha).Columns.Contains("CotizacionId"));
        }
    }

    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Simula la persistencia para verificar el controlador.
    internal sealed class RepositorioFalso : ClsPedidoRepositorio
    {
        internal int Guardados, CotizacionConsultada, IdEstado;
        internal bool CotizacionVigente = true;
        internal string Estado, NuevoEstado, EstadoReporte;
        internal DataTable Cotizaciones = new DataTable();

        public override DataTable VentasFuncCatalogo(string Nombre) { return new DataTable(); }
        public override DataTable VentasFuncListas(int Cliente, DateTime Fecha) { return new DataTable(); }
        public override DataTable VentasFuncCotizaciones(int Cliente, DateTime Fecha) { return Cotizaciones; }
        public override bool VentasFuncCotVigente(int Cotizacion, int Cliente, DateTime Fecha)
        {
            CotizacionConsultada = Cotizacion;
            return CotizacionVigente;
        }
        public override DataTable VentasFuncProductos(int Lista) { return new DataTable(); }
        public override string VentasFuncMoneda(int Id) { return "GTQ"; }
        public override DataTable VentasFuncListar() { return new DataTable(); }
        public override DataTable VentasFuncReporte(DateTime Desde, DateTime Hasta, string Estado)
        {
            EstadoReporte = Estado;
            return new DataTable();
        }
        public override DataTable VentasFuncPedido(int Id) { return new DataTable(); }
        public override int VentasFuncGuardar(DataRow Pedido, DataTable Lineas)
        {
            Guardados++;
            NuevoEstado = Convert.ToString(Pedido["NuevoEstado"]);
            if (Convert.ToInt32(Pedido["Id"]) > 0) return Convert.ToInt32(Pedido["Id"]);
            Pedido["Numero"] = "PED-00000012";
            return 12;
        }
        public override void VentasMetEstado(int Id, string NuevoEstado) { IdEstado = Id; Estado = NuevoEstado; }
    }
}
