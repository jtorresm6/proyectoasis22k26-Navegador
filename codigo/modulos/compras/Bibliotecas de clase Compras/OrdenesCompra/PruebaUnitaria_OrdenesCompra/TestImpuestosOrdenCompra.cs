using System;
using System.Data;
using System.Globalization;
using CapaControlador_OrdenesCompra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PruebaUnitaria_OrdenesCompra.Apoyo;

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestImpuestosOrdenCompra
    {
        private static decimal Numero(string Texto) => decimal.Parse(Texto, CultureInfo.InvariantCulture);

        [DataTestMethod]
        [DataRow("90", "12", "10.8")]
        [DataRow("90", "100", "90")]
        [DataRow("1", "999.99", "9.9999")]
        [DataRow("9000000", "999.99", "89999100")]
        [DataRow("0", "99.99", "0")]
        [DataRow("99999999.9999", "0", "0")]
        [DataRow("0.0001", "50", "0.0001")]
        [DataRow("1", "0.01", "0.0001")]
        [DataRow("49999999.9999", "99.99", "49994999.9999")]
        public void ImpuestoRespetaPorcentajeYCuatroDecimales(string Base, string Porcentaje, string Esperado)
        {
            Assert.AreEqual(Numero(Esperado), ClsOrdenesCompraControlador.ComprasFuncCalcularImpuesto(Numero(Base), Numero(Porcentaje)));
        }

        [DataTestMethod]
        [DataRow("100", "-0.01")]
        [DataRow("100", "1000")]
        [DataRow("100", "999.999")]
        [DataRow("100", "12.345")]
        [DataRow("-1", "12")]
        [DataRow("100000000", "0")]
        [DataRow("1.00001", "0")]
        [DataRow("99999999.9999", "0.01")]
        [DataRow("50000000", "100")]
        [DataRow("10000000", "999.99")]
        [DataRow("20000000", "999.99")]
        public void ImportesFueraDeRangoNoSeAceptan(string Base, string Porcentaje)
        {
            Assert.ThrowsException<ArgumentException>(() => ClsOrdenesCompraControlador.ComprasFuncCalcularImpuesto(Numero(Base), Numero(Porcentaje)));
        }

        private static DataTable Detalles(decimal Precio)
        {
            var Datos = new DataTable();
            Datos.Columns.Add("idProducto", typeof(int)); Datos.Columns.Add("idBodega", typeof(int));
            foreach (string Campo in new[] { "cantidad", "precio", "porcentaje" }) Datos.Columns.Add(Campo, typeof(decimal));
            Datos.Rows.Add(1, 1, 1M, Precio, 10M);
            return Datos;
        }

        [TestMethod]
        public void EncabezadoGuardaImpuestoYDescuentoComoImportesAbsolutos()
        {
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            using (var Datos = Detalles(100M))
            {
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Controlador.ComprasFuncCrearConDetalles(1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Datos, 12M);
                Assert.AreEqual(100M, Repositorio.SubtotalCreado);
                Assert.AreEqual(10M, Repositorio.DescuentoCreado);
                Assert.AreEqual(10.8M, Repositorio.ImpuestoCreado);
                Assert.AreEqual(100.8M, Repositorio.SubtotalCreado - Repositorio.DescuentoCreado + Repositorio.ImpuestoCreado);
            }
        }

        [DataTestMethod]
        [DataRow("99999999.9999", "12")]
        [DataRow("20000000", "999.99")]
        public void TotalConImpuestoExcesivoNoLlegaAlRepositorio(string Precio, string Porcentaje)
        {
            using (var Repositorio = new ClsRepositorioOrdenesCompraPrueba())
            using (var Datos = Detalles(Numero(Precio)))
            {
                Datos.Rows[0]["porcentaje"] = 0M;
                var Controlador = new ClsOrdenesCompraControlador(Repositorio);
                Assert.ThrowsException<ArgumentException>(() => Controlador.ComprasFuncCrearConDetalles(
                    1, 1, 1, DateTime.Today, DateTime.Today, 1M, null, "", Datos, Numero(Porcentaje)));
                Assert.AreEqual(0, Repositorio.Creaciones);
            }
        }
    }
}
