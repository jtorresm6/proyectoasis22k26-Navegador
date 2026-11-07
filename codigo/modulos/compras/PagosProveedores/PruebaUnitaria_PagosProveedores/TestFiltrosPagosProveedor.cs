using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CapaControlador_PagosProveedores;
using PruebaUnitaria_PagosProveedores.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica los filtros de proveedor, estado y fechas en la consulta.
 * ===================================================================
*/

namespace PruebaUnitaria_PagosProveedores
{
    [TestClass]
    public class TestFiltrosPagosProveedor
    {
        private ClsRepositorioPagosProveedorPrueba _Repositorio;
        private ClsModeloPagoProveedor _Controlador;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioPagosProveedorPrueba();
            _Repositorio.Datos.Add(ClsRepositorioPagosProveedorPrueba.CrearEncabezado(1, 1, "Pagado", new DateTime(2026, 10, 1)));
            _Repositorio.Datos.Add(ClsRepositorioPagosProveedorPrueba.CrearEncabezado(2, 1, "Programado", new DateTime(2026, 10, 15)));
            _Repositorio.Datos.Add(ClsRepositorioPagosProveedorPrueba.CrearEncabezado(3, 2, "Pagado", new DateTime(2026, 10, 20)));
            _Controlador = new ClsModeloPagoProveedor(_Repositorio);
        }

        [TestMethod]
        public void Test_SinFiltrosDevuelveTodos()
        {
            // Arrange / Act
            var Lista = _Controlador.ComprasMetObtenerTodos();

            // Assert
            Assert.AreEqual(3, Lista.Count);
        }

        [TestMethod]
        public void Test_FiltrarPorProveedor()
        {
            // Arrange
            int IdProveedor = 1;

            // Act
            var Lista = _Controlador.ComprasMetObtenerTodos(IdProveedor, null, null, null);

            // Assert
            Assert.AreEqual(2, Lista.Count);
        }

        [TestMethod]
        public void Test_ProveedorCeroSeIgnora()
        {
            // Arrange / Act
            _Controlador.ComprasMetObtenerTodos(0, null, null, null);

            // Assert
            Assert.IsNull(_Repositorio.UltimoFiltroProveedor);
        }

        [TestMethod]
        public void Test_ProveedorNegativoSeIgnora()
        {
            // Arrange / Act
            _Controlador.ComprasMetObtenerTodos(-5, null, null, null);

            // Assert
            Assert.IsNull(_Repositorio.UltimoFiltroProveedor);
        }

        [TestMethod]
        public void Test_EstadoTodosSeIgnora()
        {
            // Arrange / Act
            var Lista = _Controlador.ComprasMetObtenerTodos(null, "Todos", null, null);

            // Assert
            Assert.AreEqual(3, Lista.Count);
            Assert.IsNull(_Repositorio.UltimoFiltroEstado);
        }

        [TestMethod]
        public void Test_EstadoEnBlancoSeIgnora()
        {
            // Arrange / Act
            _Controlador.ComprasMetObtenerTodos(null, "   ", null, null);

            // Assert
            Assert.IsNull(_Repositorio.UltimoFiltroEstado);
        }

        [TestMethod]
        public void Test_FiltrarPorEstado()
        {
            // Arrange
            string Estado = "Pagado";

            // Act
            var Lista = _Controlador.ComprasMetObtenerTodos(null, Estado, null, null);

            // Assert
            Assert.AreEqual(2, Lista.Count);
        }

        [TestMethod]
        public void Test_FiltrarPorRangoDeFechas()
        {
            // Arrange
            DateTime Desde = new DateTime(2026, 10, 10);
            DateTime Hasta = new DateTime(2026, 10, 18);

            // Act
            var Lista = _Controlador.ComprasMetObtenerTodos(null, null, Desde, Hasta);

            // Assert
            Assert.AreEqual(1, Lista.Count);
            Assert.AreEqual(2, Lista[0].IdPagoProveedor);
        }

        [TestMethod]
        public void Test_FiltrarCombinaProveedorYEstado()
        {
            // Arrange / Act
            var Lista = _Controlador.ComprasMetObtenerTodos(1, "Pagado", null, null);

            // Assert
            Assert.AreEqual(1, Lista.Count);
            Assert.AreEqual(1, Lista[0].IdPagoProveedor);
        }

        [TestMethod]
        public void Test_FiltrarSinCoincidencias()
        {
            // Arrange
            int IdProveedor = 99;

            // Act
            var Lista = _Controlador.ComprasMetObtenerTodos(IdProveedor, null, null, null);

            // Assert
            Assert.AreEqual(0, Lista.Count);
        }
    }
}