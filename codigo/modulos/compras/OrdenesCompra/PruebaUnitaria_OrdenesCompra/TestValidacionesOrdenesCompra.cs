using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using CapaControlador_OrdenesCompra;
using PruebaUnitaria_OrdenesCompra.Apoyo;

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestValidacionesOrdenesCompra
    {
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompra _Controlador;
        private DateTime _Emision;
        private DateTime _Vencimiento;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompra(_Repositorio);
            _Emision = new DateTime(2026, 10, 1);
            _Vencimiento = new DateTime(2026, 10, 31);
        }

        [TestCleanup]
        public void ComprasProcLiberarPrueba()
        {
            _Repositorio.Dispose();
        }

        [TestMethod]
        public void Test_CalcularVencimientoRespetaDiasYAnioBisiesto()
        {
            Assert.AreEqual(new DateTime(2028, 3, 29), _Controlador.ComprasFuncCalcularVencimiento(new DateTime(2028, 2, 28, 15, 30, 0), 30));
            Assert.AreEqual(_Emision.Date, _Controlador.ComprasFuncCalcularVencimiento(_Emision.AddHours(9), 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CalcularVencimientoRechazaPlazoNegativo()
        {
            _Controlador.ComprasFuncCalcularVencimiento(_Emision, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_ObtenerTipoCambioRechazaMonedaInvalida()
        {
            _Controlador.ComprasFuncObtenerTipoCambio(0L, _Emision);
        }

        [TestMethod]
        public void Test_CrearSinCodigoConImpuesto()
        {
            int Id = _Controlador.ComprasFuncCrear(3, 2, 1, _Emision, _Vencimiento,
                1M, 100M, 10M, " Insumos ", 10.8M);
            Assert.AreEqual(900, Id);
            Assert.AreEqual(10.8M, _Repositorio.ImpuestoCreado);
            Assert.AreEqual("Insumos", _Repositorio.ComentarioCreado);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaImpuestoNegativo()
        {
            _Controlador.ComprasFuncCrear(3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 0M, "", -1M);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaImpuestoConCincoDecimales()
        {
            _Controlador.ComprasFuncCrear(3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 0M, "", 0.00001M);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaTotalFueraDeRango()
        {
            _Controlador.ComprasFuncCrear(3, 2, 1, _Emision, _Vencimiento, 1M, 99999999.9999M, 0M, "", 1M);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_ActualizarVencimientoRechazaOrdenBloqueada()
        {
            _Repositorio.Datos.Rows.Find(101)["bloqueadoOrdenCompra"] = true;
            _Controlador.ComprasProcActualizarVencimiento(101, _Vencimiento);
        }

        [TestMethod]
        public void Test_ObtenerMapeaImpuestoYBloqueo()
        {
            DataRow Fila = _Repositorio.Datos.Rows.Find(101);
            Fila["impuestoOrdenCompra"] = 12M;
            Fila["bloqueadoOrdenCompra"] = true;
            var Orden = _Controlador.ComprasFuncObtener(101);
            Assert.AreEqual(12M, Orden.Impuesto);
            Assert.IsTrue(Orden.Bloqueado);
            Assert.AreEqual("101", Orden.NumeroOrden);
        }

        [TestMethod]
        public void Test_CrearDevuelvePkYNormalizaTextos()
        {
            string Codigo = " 000-OC-001 ";
            string Comentario = " Insumos ";
            int Resultado = _Controlador.ComprasFuncCrear(Codigo, 3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, Comentario);
            Assert.AreEqual(900, Resultado);
            Assert.AreEqual(string.Empty, _Repositorio.CodigoCreado);
            Assert.AreEqual("Insumos", _Repositorio.ComentarioCreado);
        }

        [TestMethod]
        public void Test_CrearEliminaHoraDeLasFechas()
        {
            DateTime Emision = _Emision.AddHours(9);
            DateTime Vencimiento = _Vencimiento.AddHours(18);
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, Emision, Vencimiento, 1M, 100M, 10M, "Insumos");
            Assert.AreEqual(_Emision, _Repositorio.EmisionCreada);
            Assert.AreEqual(_Vencimiento, _Repositorio.VencimientoCreado);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaCodigoVacio()
        {
            string Codigo = "   ";
            _Controlador.ComprasFuncCrear(Codigo, 3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaCodigoMayorDeTreinta()
        {
            string Codigo = new string('A', 31);
            _Controlador.ComprasFuncCrear(Codigo, 3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, "Insumos");
        }

        [TestMethod]
        public void Test_CrearAceptaCodigoDeTreinta()
        {
            string Codigo = new string('A', 30);
            _Controlador.ComprasFuncCrear(Codigo, 3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, "Insumos");
            Assert.AreEqual(string.Empty, _Repositorio.CodigoCreado);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaProveedorInvalido()
        {
            int IdProveedor = 0;
            _Controlador.ComprasFuncCrear("OC-001", IdProveedor, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaVencimientoAnterior()
        {
            DateTime Vencimiento = _Emision.AddDays(-1);
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, Vencimiento, 1M, 100M, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaTipoCambioCero()
        {
            decimal TipoCambio = 0M;
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, _Vencimiento, TipoCambio, 100M, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaTipoCambioConSieteDecimales()
        {
            decimal TipoCambio = 1.0000001M;
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, _Vencimiento, TipoCambio, 100M, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaMontoConCincoDecimales()
        {
            decimal Subtotal = 100.00001M;
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, _Vencimiento, 1M, Subtotal, 10M, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_CrearRechazaDescuentoMayorQueSubtotal()
        {
            decimal Subtotal = 100M;
            decimal Descuento = 101M;
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, _Vencimiento, 1M, Subtotal, Descuento, "Insumos");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_CrearRechazaComentarioExcesivo()
        {
            string Comentario = new string('A', 501);
            _Controlador.ComprasFuncCrear("OC-001", 3, 2, 1, _Emision, _Vencimiento, 1M, 100M, 10M, Comentario);
        }

        [TestMethod]
        public void Test_ActualizarVencimientoDeOrdenPendiente()
        {
            int IdOrden = 101;
            DateTime Fecha = new DateTime(2026, 11, 15, 8, 0, 0);
            _Controlador.ComprasProcActualizarVencimiento(IdOrden, Fecha);
            Assert.AreEqual(Fecha.Date, _Repositorio.VencimientoActualizado);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_ActualizarVencimientoRechazaOrdenRecibida()
        {
            int IdOrden = 105;
            DateTime Fecha = new DateTime(2026, 12, 15);
            _Controlador.ComprasProcActualizarVencimiento(IdOrden, Fecha);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ActualizarVencimientoRechazaFechaAnterior()
        {
            int IdOrden = 101;
            DateTime Fecha = new DateTime(2026, 9, 30);
            _Controlador.ComprasProcActualizarVencimiento(IdOrden, Fecha);
        }
    }
}
