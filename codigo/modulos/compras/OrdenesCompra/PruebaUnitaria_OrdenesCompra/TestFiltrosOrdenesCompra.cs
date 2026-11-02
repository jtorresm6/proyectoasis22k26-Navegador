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
 *  Verifica búsqueda y filtros sobre las órdenes ya consultadas.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestFiltrosOrdenesCompra
    {
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompra _Controlador;
        private DataTable _Resultado;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompra(_Repositorio);
        }

        [TestCleanup]
        public void ComprasProcLiberarPrueba()
        {
            _Resultado?.Dispose();
            _Repositorio.Dispose();
        }

        [TestMethod]
        public void Test_FiltrarPorCodigo()
        {
            // Arrange
            string Texto = " oc-003 ";

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, Texto, "Todos", null, null);

            // Assert
            Assert.AreEqual(1, _Resultado.Rows.Count);
            Assert.AreEqual(103, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
        }

        [TestMethod]
        public void Test_FiltrarProveedorSinAcentos()
        {
            // Arrange
            string Texto = "jose carnicos";

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, Texto, "Todos", null, null);

            // Assert
            Assert.AreEqual(2, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_FiltrarPorNit()
        {
            // Arrange
            string Texto = "NIT-4";

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, Texto, "Todos", null, null);

            // Assert
            Assert.AreEqual(2, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_FiltrarPorEstado()
        {
            // Arrange
            string Estado = "Recibida";

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "", Estado, null, null);

            // Assert
            Assert.AreEqual(1, _Resultado.Rows.Count);
            Assert.AreEqual(105, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
        }

        [TestMethod]
        public void Test_FiltrarFechasIncluyeLimites()
        {
            // Arrange
            DateTime Inicio = new DateTime(2026, 10, 5);
            DateTime Fin = new DateTime(2026, 10, 15);

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "", "Todos", Inicio, Fin);

            // Assert
            Assert.AreEqual(3, _Resultado.Rows.Count);
            Assert.AreEqual(102, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
            Assert.AreEqual(104, Convert.ToInt32(_Resultado.Rows[2]["idOrdenCompra"]));
        }

        [TestMethod]
        public void Test_FiltrarCombinaTextoEstadoYFechas()
        {
            // Arrange
            DateTime Fecha = new DateTime(2026, 10, 10);

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "jose", "Rechazada", Fecha, Fecha);

            // Assert
            Assert.AreEqual(1, _Resultado.Rows.Count);
            Assert.AreEqual(103, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
        }

        [TestMethod]
        public void Test_FiltrarSinCoincidencias()
        {
            // Arrange
            string Texto = "NO-EXISTE";

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, Texto, "Todos", null, null);

            // Assert
            Assert.AreEqual(0, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_FiltrarNoModificaDatosOriginales()
        {
            // Arrange
            string CodigoOriginal = Convert.ToString(_Repositorio.Datos.Rows[0]["codigoOrdenCompra"]);

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "OC-001", "Todos", null, null);
            _Resultado.Rows[0]["codigoOrdenCompra"] = "OTRO-CODIGO";

            // Assert
            Assert.AreEqual(CodigoOriginal, _Repositorio.Datos.Rows[0]["codigoOrdenCompra"]);
        }

        [TestMethod]
        public void Test_FiltrarOmiteFilasEliminadas()
        {
            // Arrange
            _Repositorio.Datos.Rows.Find(101).Delete();

            // Act
            _Resultado = _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "", "Todos", null, null);

            // Assert
            Assert.AreEqual(4, _Resultado.Rows.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FiltrarRechazaRangoIncompleto()
        {
            // Arrange
            DateTime Inicio = new DateTime(2026, 10, 1);

            // Act
            _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "", "Todos", Inicio, null);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_FiltrarRechazaRangoInvertido()
        {
            // Arrange
            DateTime Inicio = new DateTime(2026, 10, 31);
            DateTime Fin = new DateTime(2026, 10, 1);

            // Act
            _Controlador.ComprasFuncFiltrar(_Repositorio.Datos, "", "Todos", Inicio, Fin);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_FiltrarRechazaTablaNula()
        {
            // Arrange
            DataTable Datos = null;

            // Act
            _Controlador.ComprasFuncFiltrar(Datos, "", "Todos", null, null);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }
    }
}
