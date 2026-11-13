using System;
using System.Data;
using CapaControlador_OrdenesCompra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PruebaUnitaria_OrdenesCompra.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 08/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica las consultas y los datos devueltos por el controlador de órdenes.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestConsultasOrdenesCompra
    {
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompraControlador _Controlador;

        [TestInitialize]
        public void Preparar()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompraControlador(_Repositorio);
        }

        [TestCleanup]
        public void Liberar() => _Repositorio.Dispose();

        [DataTestMethod]
        [DataRowAttribute(null)]
        [DataRowAttribute("")]
        [DataRowAttribute("   ")]
        [DataRowAttribute("Todos")]
        [DataRowAttribute(" todos ")]
        public void ListarSinEstadoDevuelveTodas(string estado)
        {
            using (var resultado = _Controlador.ComprasFuncListar(estado))
                Assert.AreEqual(5, resultado.Rows.Count);
        }

        [DataTestMethod]
        [DataRowAttribute(" pendiente ", "Pendiente", 101)]
        [DataRowAttribute("AUTORIZADA", "Autorizada", 102)]
        [DataRowAttribute("rechazada", "Rechazada", 103)]
        [DataRowAttribute("Anulada", "Anulada", 104)]
        [DataRowAttribute("Recibida", "Recibida", 105)]
        public void ListarNormalizaEstado(string entrada, string esperado, int id)
        {
            using (var resultado = _Controlador.ComprasFuncListar(entrada))
            {
                Assert.AreEqual(esperado, _Repositorio.EstadoSolicitado);
                Assert.AreEqual(1, resultado.Rows.Count);
                Assert.AreEqual(id, resultado.Rows[0]["idOrdenCompra"]);
            }
        }

        [TestMethod]
        public void ListarNoComparteFilasConRepositorio()
        {
            using (var resultado = _Controlador.ComprasFuncListar())
                resultado.Rows[0]["nombreProveedor"] = "Modificado";
            Assert.AreNotEqual("Modificado", _Repositorio.Datos.Rows[0]["nombreProveedor"]);
        }

        [TestMethod]
        public void ObtenerMapeaImportesProveedorYBloqueo()
        {
            var fila = _Repositorio.Datos.Rows.Find(101);
            fila["impuestoOrdenCompra"] = 12M;
            fila["totalOrdenCompra"] = 102.1234M;
            fila["bloqueadoOrdenCompra"] = true;
            var orden = _Controlador.ComprasFuncObtener(101);
            Assert.AreEqual("101", orden.NumeroOrden);
            Assert.AreEqual(100.1234M, orden.Subtotal);
            Assert.AreEqual(10M, orden.Descuento);
            Assert.AreEqual(12M, orden.Impuesto);
            Assert.AreEqual(102.1234M, orden.MontoTotal);
            Assert.AreEqual(3, orden.IdProveedor);
            Assert.AreEqual(fila["nombreProveedor"], orden.NombreProveedor);
            Assert.IsTrue(orden.ProveedorActivo);
            Assert.IsTrue(orden.Bloqueado);
        }

        [TestMethod]
        public void ObtenerInexistenteDevuelveNull() => Assert.IsNull(_Controlador.ComprasFuncObtener(999));

        [DataTestMethod]
        [DataRowAttribute(0)]
        [DataRowAttribute(-1)]
        public void ConsultasRechazanIdentificadoresInvalidos(int id)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasFuncObtener(id));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasFuncListarDetalles(id));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasFuncObtenerTipoCambio(id, DateTime.Today));
        }

        [TestMethod]
        public void DetallesInexistentesConservanEsquema()
        {
            using (var resultado = _Controlador.ComprasFuncListarDetalles(999))
            {
                Assert.AreEqual(0, resultado.Rows.Count);
                Assert.IsTrue(resultado.Columns.Contains("idProducto"));
            }
        }

        [DataTestMethod]
        [DataRowAttribute(20L, "7.7")]
        [DataRowAttribute(30L, "8.4")]
        public void TipoCambioUsaMonedaYFechaSinHora(long moneda, string tasa)
        {
            var fecha = new DateTime(2026, 11, 8, 15, 30, 0);
            using (var resultado = _Controlador.ComprasFuncObtenerTipoCambio(moneda, fecha))
            {
                Assert.AreEqual(decimal.Parse(tasa, System.Globalization.CultureInfo.InvariantCulture), resultado.Rows[0]["valorTipoCambio"]);
                Assert.AreEqual(fecha.Date, resultado.Rows[0]["fechaTipoCambio"]);
            }
        }

        [TestMethod]
        public void TipoCambioNoDisponibleDevuelveTablaVacia()
        {
            using (var resultado = _Controlador.ComprasFuncObtenerTipoCambio(40L, new DateTime(2026, 11, 8)))
                Assert.AreEqual(0, resultado.Rows.Count);
        }

        [TestMethod]
        public void ProveedoresSoloIncluyenActivos()
        {
            using (var resultado = _Controlador.ComprasFuncListarProveedores())
                foreach (DataRow fila in resultado.Rows)
                    Assert.IsTrue((bool)fila["estadoProveedor"]);
        }

        [TestMethod]
        public void ConstructorRechazaRepositorioNulo() => Assert.ThrowsException<ArgumentNullException>(() => new ClsOrdenesCompraControlador(null));
    }
}
