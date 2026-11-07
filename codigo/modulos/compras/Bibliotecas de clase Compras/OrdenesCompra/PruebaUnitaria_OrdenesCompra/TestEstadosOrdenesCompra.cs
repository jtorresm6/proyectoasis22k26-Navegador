using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using CapaControlador_OrdenesCompra;
using PruebaUnitaria_OrdenesCompra.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diego Fernando Santizo Samayoa
 * Carné : 0901-22-15950
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Verifica aprobación, anulación y validación de estados del controlador.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestEstadosOrdenesCompra
    {
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompra _Controlador;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompra(_Repositorio);
        }

        [TestCleanup]
        public void ComprasProcLiberarPrueba()
        {
            _Repositorio.Dispose();
        }

        [TestMethod]
        public void Test_AprobarOrdenPendiente()
        {
            // Arrange
            int IdOrden = 101;
            string Estado = "Autorizada";

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, Estado);

            // Assert
            Assert.AreEqual("Autorizada", _Controlador.ComprasFuncObtener(IdOrden).Estado);
        }

        [TestMethod]
        public void Test_CambiarEstadoNormalizaTexto()
        {
            // Arrange
            int IdOrden = 101;
            string Estado = " autorizada ";

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, Estado);

            // Assert
            Assert.AreEqual("Autorizada", _Controlador.ComprasFuncObtener(IdOrden).Estado);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CambiarEstadoRechazaTodos()
        {
            // Arrange
            int IdOrden = 101;
            string Estado = "Todos";

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, Estado);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CambiarEstadoRechazaNombreDesconocido()
        {
            // Arrange
            int IdOrden = 101;
            string Estado = "Aprobada";

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, Estado);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CambiarEstadoRechazaPkInvalido()
        {
            // Arrange
            int IdOrden = 0;

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, "Autorizada");

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_CambiarEstadoPropagaOrdenInexistente()
        {
            // Arrange
            int IdOrden = 999;

            // Act
            _Controlador.ComprasProcCambiarEstado(IdOrden, "Autorizada");

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        public void Test_AnularDevuelveOrdenActualizada()
        {
            // Arrange
            int IdOrden = 101;

            // Act
            var Resultado = _Controlador.ComprasFuncAnular(IdOrden);

            // Assert
            Assert.AreEqual(IdOrden, Resultado.IdOrdenCompra);
            Assert.AreEqual("Anulada", Resultado.Estado);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_AnularDetectaOperacionNoAplicada()
        {
            // Arrange
            int IdOrden = 101;
            _Repositorio.OmitirAnulacion = true;

            // Act
            _Controlador.ComprasFuncAnular(IdOrden);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }
    }
}
