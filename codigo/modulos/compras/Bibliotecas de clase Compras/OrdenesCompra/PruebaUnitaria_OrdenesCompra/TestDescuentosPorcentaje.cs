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
    public class TestDescuentosPorcentaje
    {
        [DataTestMethod]
        [DataRow("0", "9")]
        [DataRow("99.99", "0.0009")]
        [DataRow("33.33", "6.0003")]
        [DataRow("0.01", "8.9991")]
        public void TotalDeLineaCoincideConCalculoDeBaseDatos(string Porcentaje, string Total)
        {
            Assert.AreEqual(decimal.Parse(Total, CultureInfo.InvariantCulture), ClsOrdenesCompraControlador.ComprasFuncCalcularTotalProducto(
                3M, 3M, decimal.Parse(Porcentaje, CultureInfo.InvariantCulture)));
        }

        [DataTestMethod]
        [DataRow("-0.01")]
        [DataRow("100")]
        [DataRow("100.01")]
        [DataRow("33.333")]
        [DataRow("0.001")]
        public void PorcentajeFueraDeRangoOPrecisionNoSeAcepta(string Valor)
        {
            Assert.ThrowsException<ArgumentException>(() => ClsOrdenesCompraControlador.ComprasFuncCalcularDescuento(
                3M, decimal.Parse(Valor, CultureInfo.InvariantCulture)));
        }

        private static void Agregar(ClsProductosOrdenesCompraTabla Productos, decimal Porcentaje, DataRow Editado = null, string NombreSucursal = null)
        {
            using (var Producto = new DataTable())
            using (var Bodega = new DataTable())
            {
                Producto.Columns.Add("idProducto", typeof(int));
                foreach (string Campo in new[] { "skuProducto", "nombreProducto", "descripcionProducto" }) Producto.Columns.Add(Campo);
                Producto.Columns.Add("pesoProducto", typeof(decimal));
                Bodega.Columns.Add("idBodega", typeof(int)); Bodega.Columns.Add("Fk_Sucursal_Codigo", typeof(int)); Bodega.Columns.Add("nombreBodega");
                Productos.Guardar(Editado, Producto.Rows.Add(1, "SKU", "Producto", "Descripcion", 1M),
                    Bodega.Rows.Add(1, 1, "Bodega"), 3, 3.33M, 1M, 3.33M, Porcentaje, NombreSucursal);
                Productos.Recalcular(1M);
            }
        }

        [TestMethod]
        public void CapturaYGuardadoConservanLosMismosImportesSinReconstruirPorcentaje()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            {
                Agregar(Productos, 0.01M);
                Assert.AreEqual(9.9890M, Productos.Detalles.Rows[0]["total"]);
                Assert.AreEqual(0.0010M, Productos.Descuento);
                Assert.AreEqual(Productos.Descuento, Productos.Detalles.Rows[0]["descuentoTotal"]);
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Controlador.ComprasFuncCrearConDetalles(1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Productos.Detalles);
                Assert.AreEqual(Productos.Bruto, Repositorio.SubtotalCreado);
                Assert.AreEqual(Productos.Descuento, Repositorio.DescuentoCreado);
                Assert.AreEqual(0.01M, Repositorio.DetallesCreados.Rows[0]["porcentaje"]);
                Assert.IsFalse(Productos.Detalles.Columns.Contains("tipoDescuento"));
                Assert.IsFalse(Productos.Detalles.Columns.Contains("valorDescuento"));
            }
        }

        [TestMethod]
        public void NombreDeSucursalSeConservaSinUsarIdentificadorComoNombre()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Agregar(Productos, 10M);
                Assert.AreEqual(string.Empty, Productos.Detalles.Rows[0]["sucursal"]);
                Agregar(Productos, 10M, Productos.Detalles.Rows[0], "Sucursal Central");
                Assert.AreEqual("Sucursal Central", Productos.Detalles.Rows[0]["sucursal"]);
            }
        }

        [TestMethod]
        public void EditarYConvertirMonedaConservanPorcentaje()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Agregar(Productos, 10M);
                Agregar(Productos, 33.33M, Productos.Detalles.Rows[0]);
                Productos.Recalcular(2M);
                Assert.AreEqual(33.33M, Productos.Detalles.Rows[0]["porcentaje"]);
                Assert.AreEqual(ClsOrdenesCompraControlador.ComprasFuncCalcularTotalProducto(3M, 1.67M, 33.33M), Productos.Detalles.Rows[0]["total"]);
                Productos.Recalcular(1M);
                Assert.AreEqual(6.6603M, Productos.Detalles.Rows[0]["total"]);
            }
        }

        [TestMethod]
        public void PorcentajeConTresDecimalesNoModificaLineaExistente()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            {
                Agregar(Productos, 10M);
                Assert.ThrowsException<ArgumentException>(() => Agregar(Productos, 33.333M, Productos.Detalles.Rows[0]));
                Assert.AreEqual(10M, Productos.Detalles.Rows[0]["porcentaje"]);
                Assert.AreEqual(1, Productos.Detalles.Rows.Count);
            }
        }

        [TestMethod]
        public void GuardadoRechazaMayorPrecisionAunqueLlegueMetadataAnterior()
        {
            using (var Productos = new ClsProductosOrdenesCompraTabla())
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            {
                Agregar(Productos, 10M);
                Productos.Detalles.Columns.Add("tipoDescuento", typeof(string));
                Productos.Detalles.Columns.Add("valorDescuento", typeof(decimal));
                Productos.Detalles.Rows[0]["porcentaje"] = 33.333M;
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Assert.ThrowsException<ArgumentException>(() => Controlador.ComprasFuncCrearConDetalles(
                    1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Productos.Detalles));
                Assert.AreEqual(0, Repositorio.Creaciones);
            }
        }
    }
}
