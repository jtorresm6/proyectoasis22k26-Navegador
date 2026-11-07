using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using CapaControlador_OrdenesCompra;
using PruebaUnitaria_OrdenesCompra.Apoyo;

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestConsultasOrdenesCompra
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
        public void Test_ListarTodasLasOrdenes()
        {
            int CantidadEsperada = 5;
            _Resultado = _Controlador.ComprasFuncListar();
            Assert.AreEqual(CantidadEsperada, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_ListarOrdenesPendientes()
        {
            string Estado = "Pendiente";
            _Resultado = _Controlador.ComprasFuncListar(Estado);
            Assert.AreEqual(1, _Resultado.Rows.Count);
            Assert.AreEqual(101, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
        }

        [TestMethod]
        public void Test_ListarTodosLosEstados()
        {
            string Estado = "Todos";
            _Resultado = _Controlador.ComprasFuncListar(Estado);
            Assert.AreEqual(5, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_ListarSinEstado()
        {
            string Estado = null;
            _Resultado = _Controlador.ComprasFuncListar(Estado);
            Assert.AreEqual(5, _Resultado.Rows.Count);
        }

        [TestMethod]
        public void Test_ListarNormalizaEstado()
        {
            string Estado = " autorizada ";
            _Resultado = _Controlador.ComprasFuncListar(Estado);
            Assert.AreEqual("Autorizada", _Repositorio.EstadoSolicitado);
            Assert.AreEqual(102, Convert.ToInt32(_Resultado.Rows[0]["idOrdenCompra"]));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ListarRechazaEstadoDesconocido()
        {
            string Estado = "Aprobada";
            _Controlador.ComprasFuncListar(Estado);
        }

        [TestMethod]
        public void Test_ObtenerOrdenPorPk()
        {
            int IdOrden = 102;
            var Resultado = _Controlador.ComprasFuncObtener(IdOrden);
            Assert.AreEqual(IdOrden, Resultado.IdOrdenCompra);
            Assert.AreEqual("102", Resultado.CodigoOrdenCompra);
            Assert.AreEqual(90.1234M, Resultado.MontoTotal);
        }

        [TestMethod]
        public void Test_ObtenerOrdenInexistente()
        {
            int IdOrden = 999;
            var Resultado = _Controlador.ComprasFuncObtener(IdOrden);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ObtenerRechazaPkInvalido()
        {
            int IdOrden = 0;
            _Controlador.ComprasFuncObtener(IdOrden);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ListarDetallesRechazaPkInvalido()
        {
            int IdOrden = -1;
            _Controlador.ComprasFuncListarDetalles(IdOrden);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ConstructorRechazaRepositorioNulo()
        {
            CapaModelo_OrdenesCompra.Contratos.IRepositorioOrdenesCompra Repositorio = null;
            new ClsOrdenesCompra(Repositorio);
        }
    }
}
