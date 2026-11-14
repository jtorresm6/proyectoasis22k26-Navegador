using System;
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
 *  Verifica los estados y las transiciones de las órdenes de compra.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestEstadosOrdenesCompra
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
        [DataRowAttribute(" autorizada ", "Autorizada")]
        [DataRowAttribute("RECHAZADA", "Rechazada")]
        [DataRowAttribute("anulada", "Anulada")]
        [DataRowAttribute("Recibida", "Recibida")]
        [DataRowAttribute("Pendiente", "Pendiente")]
        public void CambiarEstadoNormalizaTexto(string entrada, string esperado)
        {
            _Controlador.ComprasProcCambiarEstado(101, entrada);
            Assert.AreEqual(esperado, _Controlador.ComprasFuncObtener(101).Estado);
            Assert.AreEqual("Autorizada", _Controlador.ComprasFuncObtener(102).Estado);
        }

        [DataTestMethod]
        [DataRowAttribute(null)]
        [DataRowAttribute("")]
        [DataRowAttribute("   ")]
        [DataRowAttribute("Todos")]
        [DataRowAttribute("Aprobada")]
        public void CambiarEstadoInvalidoNoModificaOrden(string estado)
        {
            Assert.ThrowsException<ArgumentException>(() => _Controlador.ComprasProcCambiarEstado(101, estado));
            Assert.AreEqual("Pendiente", _Controlador.ComprasFuncObtener(101).Estado);
        }

        [DataTestMethod]
        [DataRowAttribute(0)]
        [DataRowAttribute(-1)]
        public void CambiarEstadoYAnularRechazanIdInvalido(int id)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasProcCambiarEstado(id, "Autorizada"));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasFuncAnular(id));
        }

        [TestMethod]
        public void CambiarEstadoRechazaOrdenInexistente() => Assert.ThrowsException<InvalidOperationException>(() => _Controlador.ComprasProcCambiarEstado(999, "Autorizada"));

        [TestMethod]
        public void AnularDevuelveOrdenActualizada()
        {
            var resultado = _Controlador.ComprasFuncAnular(101);
            Assert.AreEqual(101, resultado.IdOrdenCompra);
            Assert.AreEqual("Anulada", resultado.Estado);
        }

        [TestMethod]
        public void AnularDetectaOperacionNoAplicada()
        {
            _Repositorio.OmitirAnulacion = true;
            Assert.ThrowsException<InvalidOperationException>(() => _Controlador.ComprasFuncAnular(101));
            Assert.AreEqual("Pendiente", _Controlador.ComprasFuncObtener(101).Estado);
        }

        [TestMethod]
        public void AnularRechazaOrdenInexistente() => Assert.ThrowsException<InvalidOperationException>(() => _Controlador.ComprasFuncAnular(999));
    }
}
