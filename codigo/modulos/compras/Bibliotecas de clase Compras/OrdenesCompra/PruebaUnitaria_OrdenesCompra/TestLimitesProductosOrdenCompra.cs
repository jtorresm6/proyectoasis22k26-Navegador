using System;
using System.Data;
using System.Globalization;
using CapaControlador_OrdenesCompra;
using CapaVista_OrdenesCompra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PruebaUnitaria_OrdenesCompra.Apoyo;

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestLimitesProductosOrdenCompra
    {
        private static decimal Numero(string Texto) => decimal.Parse(Texto, CultureInfo.InvariantCulture);

        [DataTestMethod]
        [DataRow("1", "99999999.9999", "0")]
        [DataRow("99999999.9999", "1", "0")]
        [DataRow("0.0001", "1", "0")]
        [DataRow("1", "0", "0")]
        [DataRow("2", "49999999.9999", "49999999.9999")]
        public void LineaEnLimiteEsAceptada(string Cantidad, string Precio, string Descuento)
        {
            ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(Numero(Cantidad), Numero(Precio), Numero(Descuento));
        }

        [DataTestMethod]
        [DataRow("0", "1", "0")]
        [DataRow("-1", "1", "0")]
        [DataRow("100000000", "0", "0")]
        [DataRow("0.00001", "1", "0")]
        [DataRow("1", "100000000", "0")]
        [DataRow("1", "-1", "0")]
        [DataRow("1", "0.00001", "0")]
        [DataRow("1", "1", "-1")]
        [DataRow("1", "1", "2")]
        [DataRow("1", "1", "0.00001")]
        [DataRow("2", "50000000", "0")]
        [DataRow("2", "50000000", "50000000")]
        [DataRow("99999999.9999", "99999999.9999", "0")]
        public void LineaFueraDeLimiteEsRechazada(string Cantidad, string Precio, string Descuento)
        {
            Assert.ThrowsException<ArgumentException>(() => ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(
                Numero(Cantidad), Numero(Precio), Numero(Descuento)));
        }

        [TestMethod]
        public void AcumuladoAceptaMaximoYRechazaUnaDiezMilesimaMas()
        {
            ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(1M, 0.9999M, 0M, 99999999M);
            Assert.ThrowsException<ArgumentException>(() => ClsOrdenesCompraControlador.ComprasProcValidarLimitesProducto(1M, 1M, 0M, 99999999M));
        }

        private static void Guardar(ClsProductosOrdenesCompraTabla Productos, decimal Precio, int Cantidad = 1,
            DataRow Editado = null, decimal Cambio = 1M)
        {
            using (var Producto = new DataTable())
            using (var Bodega = new DataTable())
            {
                Producto.Columns.Add("idProducto", typeof(int));
                foreach (string Campo in new[] { "skuProducto", "nombreProducto", "descripcionProducto" }) Producto.Columns.Add(Campo);
                Producto.Columns.Add("pesoProducto", typeof(decimal));
                Bodega.Columns.Add("idBodega", typeof(int));
                Bodega.Columns.Add("Fk_Sucursal_Codigo", typeof(int));
                Bodega.Columns.Add("nombreBodega");
                Productos.Guardar(Editado, Producto.Rows.Add(1, "SKU", "Producto", "Descripcion", 1M),
                    Bodega.Rows.Add(1, 1, "Bodega"), Cantidad, Precio * Cambio, Cambio, Precio, 0M);
                Productos.Recalcular(Cambio);
            }
        }

        [TestMethod]
        public void AgregarImporteExcesivoNoInsertaLinea()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Assert.ThrowsException<ArgumentException>(() => Guardar(Productos, 50000000M, 2));
                Assert.AreEqual(0, Productos.Detalles.Rows.Count);
            }
        }

        [TestMethod]
        public void AgregarAcumuladoExcesivoConservaOrdenAnterior()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Guardar(Productos, 60000000M);
                Assert.ThrowsException<ArgumentException>(() => Guardar(Productos, 40000000M));
                Assert.AreEqual(1, Productos.Detalles.Rows.Count);
                Assert.AreEqual(60000000M, Productos.Bruto);
            }
        }

        [TestMethod]
        public void ModificarExcesivoNoAlteraLineaOriginal()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Guardar(Productos, 100M);
                DataRow Fila = Productos.Detalles.Rows[0];
                Assert.ThrowsException<ArgumentException>(() => Guardar(Productos, 50000000M, 2, Fila));
                Assert.AreEqual(1, Fila["cantidad"]);
                Assert.AreEqual(100M, Fila["precio"]);
                Assert.AreEqual(100M, Fila["total"]);
            }
        }

        [TestMethod]
        public void ModificarAcumuladoExcesivoConservaAmbasLineas()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Guardar(Productos, 60000000M);
                Guardar(Productos, 30000000M);
                DataRow Fila = Productos.Detalles.Rows[1];
                Assert.ThrowsException<ArgumentException>(() => Guardar(Productos, 40000000M, 1, Fila));
                Assert.AreEqual(30000000M, Fila["precio"]);
                Assert.AreEqual(90000000M, Productos.Bruto);
            }
        }

        [TestMethod]
        public void ModificarReemplazaImporteAnteriorSinSumarloDosVeces()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Guardar(Productos, 60000000M);
                Guardar(Productos, 30000000M);
                Guardar(Productos, 39999999.99M, 1, Productos.Detalles.Rows[1]);
                Assert.AreEqual(99999999.99M, Productos.Bruto);
            }
        }

        [TestMethod]
        public void GuardadoRechazaAcumuladoExcesivoAunqueSeEviteLaCaptura()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            {
                Guardar(Productos, 50000000M);
                var Valores = Productos.Detalles.Rows[0].ItemArray;
                Valores[0] = 2;
                Productos.Detalles.Rows.Add(Valores);
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Assert.ThrowsException<ArgumentException>(() => Controlador.ComprasFuncCrearConDetalles(
                    1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Productos.Detalles));
                Assert.AreEqual(0, Repositorio.Creaciones);
            }
        }

        [DataTestMethod]
        [DataRow("cantidad", "100000000")]
        [DataRow("cantidad", "0.00001")]
        [DataRow("precio", "100000000")]
        [DataRow("precio", "1.00001")]
        [DataRow("porcentaje", "100.01")]
        [DataRow("porcentaje", "33.333")]
        [DataRow("idProducto", "0")]
        [DataRow("idBodega", "0")]
        public void CamposInvalidosNoLleganAlRepositorio(string Campo, string Valor)
        {
            using (var Detalles = new DataTable())
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            {
                Detalles.Columns.Add("idProducto", typeof(int));
                Detalles.Columns.Add("idBodega", typeof(int));
                foreach (string Nombre in new[] { "cantidad", "precio", "porcentaje" }) Detalles.Columns.Add(Nombre, typeof(decimal));
                var Fila = Detalles.Rows.Add(1, 1, 1M, 1M, 0M);
                Fila[Campo] = Numero(Valor);
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Assert.ThrowsException<ArgumentException>(() => Controlador.ComprasFuncCrearConDetalles(
                    1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Detalles));
                Assert.AreEqual(0, Repositorio.Creaciones);
            }
        }

        [TestMethod]
        public void FechaFueraDeRangoDeDateNoLlegaAlRepositorio()
        {
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            {
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Assert.ThrowsException<ArgumentException>(() => Controlador.ComprasFuncCrear(
                    1, 1, 1, new DateTime(999, 12, 31), DateTime.Today, 1M, 1M, 0M, ""));
                Assert.AreEqual(0, Repositorio.Creaciones);
                Assert.AreEqual(900, Controlador.ComprasFuncCrear(
                    1, 1, 1, new DateTime(1000, 1, 1), DateTime.MaxValue, 1M, 1M, 0M, ""));
            }
        }

        [DataTestMethod]
        [DataRow("0")]
        [DataRow("1000000")]
        [DataRow("1.0000001")]
        public void TipoCambioFueraDeLimiteNoInsertaLinea(string Cambio)
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Assert.ThrowsException<ArgumentException>(() => Guardar(Productos, 1M, Cambio: Numero(Cambio)));
                Assert.AreEqual(0, Productos.Detalles.Rows.Count);
            }
        }
    }
}

