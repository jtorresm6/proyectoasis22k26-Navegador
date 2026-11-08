using System;
using System.Globalization;
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
 *  Verifica las reglas de validación para el registro de órdenes de compra.
 * ===================================================================
*/

namespace PruebaUnitaria_OrdenesCompra
{
    [TestClass]
    public class TestValidacionesOrdenesCompra
    {
        private static readonly DateTime Emision = new DateTime(2026, 10, 1);
        private ClsRepositorioOrdenesCompraPrueba _Repositorio;
        private ClsOrdenesCompra _Controlador;

        [TestInitialize]
        public void Preparar()
        {
            _Repositorio = new ClsRepositorioOrdenesCompraPrueba();
            _Controlador = new ClsOrdenesCompra(_Repositorio);
        }

        [TestCleanup]
        public void Liberar() => _Repositorio.Dispose();

        [DataTestMethod]
        [DataRowAttribute(2028, 2, 28, 30, 2028, 3, 29)]
        [DataRowAttribute(2026, 12, 31, 1, 2027, 1, 1)]
        [DataRowAttribute(2026, 10, 1, 0, 2026, 10, 1)]
        public void CalcularVencimientoRespetaCalendario(int anio, int mes, int dia, int plazo, int anioFin, int mesFin, int diaFin)
            => Assert.AreEqual(new DateTime(anioFin, mesFin, diaFin), _Controlador.ComprasFuncCalcularVencimiento(new DateTime(anio, mes, dia, 15, 30, 0), plazo));

        [TestMethod]
        public void RechazarPlazoNegativo() => Assert.ThrowsException<ArgumentOutOfRangeException>(() => _Controlador.ComprasFuncCalcularVencimiento(Emision, -1));

        [TestMethod]
        public void CrearNormalizaComentarioFechasYDevuelveId()
        {
            int id = _Controlador.ComprasFuncCrear(3, 2, 1, Emision.AddHours(9), Emision.AddDays(30).AddHours(18), 1M, 100M, 10M, " Insumos ", 10.8M);
            Assert.AreEqual(900, id);
            Assert.AreEqual(1, _Repositorio.Creaciones);
            Assert.AreEqual(10.8M, _Repositorio.ImpuestoCreado);
            Assert.AreEqual("Insumos", _Repositorio.ComentarioCreado);
            Assert.AreEqual(Emision, _Repositorio.EmisionCreada);
            Assert.AreEqual(Emision.AddDays(30), _Repositorio.VencimientoCreado);
        }

        [DataTestMethod]
        [DataRowAttribute(null)]
        [DataRowAttribute("")]
        [DataRowAttribute("   ")]
        public void ComentarioOpcionalSeGuardaVacio(string comentario)
        {
            Crear(comentario: comentario);
            Assert.AreEqual(string.Empty, _Repositorio.ComentarioCreado);
        }

        [DataTestMethod]
        [DataRowAttribute(0, 2, 1)]
        [DataRowAttribute(-1, 2, 1)]
        [DataRowAttribute(3, 0, 1)]
        [DataRowAttribute(3, -1, 1)]
        [DataRowAttribute(3, 2, 0)]
        [DataRowAttribute(3, 2, -1)]
        public void IdentificadoresInvalidosNoGuardan(int proveedor, int plazo, int moneda)
        {
            Assert.ThrowsException<ArgumentException>(() => Crear(proveedor: proveedor, plazo: plazo, moneda: moneda));
            Assert.AreEqual(0, _Repositorio.Creaciones);
        }

        [DataTestMethod]
        [DataRowAttribute("-1", "100", "10", "0")]
        [DataRowAttribute("1000000", "100", "10", "0")]
        [DataRowAttribute("1.0000001", "100", "10", "0")]
        [DataRowAttribute("1", "-1", "0", "0")]
        [DataRowAttribute("1", "100000000", "0", "0")]
        [DataRowAttribute("1", "100.00001", "0", "0")]
        [DataRowAttribute("1", "100", "-1", "0")]
        [DataRowAttribute("1", "100", "101", "0")]
        [DataRowAttribute("1", "100", "0.00001", "0")]
        [DataRowAttribute("1", "100", "0", "-1")]
        [DataRowAttribute("1", "100", "0", "0.00001")]
        [DataRowAttribute("1", "99999999.9999", "0", "1")]
        public void ImportesFueraDeRangoOPrecisionNoGuardan(string cambio, string subtotal, string descuento, string impuesto)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Crear(cambio: Decimal(cambio), subtotal: Decimal(subtotal), descuento: Decimal(descuento), impuesto: Decimal(impuesto)));
            Assert.AreEqual(0, _Repositorio.Creaciones);
        }

        [DataTestMethod]
        [DataRowAttribute("0.000001", "0", "0", "0")]
        [DataRowAttribute("999999.999999", "99999999.9999", "0", "0")]
        [DataRowAttribute("1", "100", "100", "0")]
        [DataRowAttribute("1", "0", "0", "99999999.9999")]
        public void ImportesEnLimitesSeGuardan(string cambio, string subtotal, string descuento, string impuesto)
        {
            Assert.AreEqual(900, Crear(cambio: Decimal(cambio), subtotal: Decimal(subtotal), descuento: Decimal(descuento), impuesto: Decimal(impuesto)));
            Assert.AreEqual(1, _Repositorio.Creaciones);
        }

        [TestMethod]
        public void TipoCambioCeroNoGuarda()
        {
            Assert.ThrowsException<ArgumentException>(() => Crear(cambio: 0M));
            Assert.AreEqual(0, _Repositorio.Creaciones);
        }

        [TestMethod]
        public void VencimientoAnteriorNoGuarda()
        {
            Assert.ThrowsException<ArgumentException>(() => Crear(vencimiento: Emision.AddDays(-1)));
            Assert.AreEqual(0, _Repositorio.Creaciones);
        }

        [TestMethod]
        public void ComentarioAcepta500YRechaza501Caracteres()
        {
            Crear(comentario: new string('A', 500));
            Assert.AreEqual(500, _Repositorio.ComentarioCreado.Length);
            Assert.ThrowsException<ArgumentException>(() => Crear(comentario: new string('A', 501)));
            Assert.AreEqual(1, _Repositorio.Creaciones);
        }

        [DataTestMethod]
        [DataRowAttribute(null)]
        [DataRowAttribute("")]
        [DataRowAttribute("   ")]
        [DataRowAttribute("1234567890123456789012345678901")]
        public void CodigoLegacyInvalidoNoGuarda(string codigo)
        {
            Assert.ThrowsException<ArgumentException>(() => _Controlador.ComprasFuncCrear(codigo, 3, 2, 1, Emision, Emision, 1M, 100M, 10M, ""));
            Assert.AreEqual(0, _Repositorio.Creaciones);
        }

        [DataTestMethod]
        [DataRowAttribute(" OC-001 ")]
        [DataRowAttribute("123456789012345678901234567890")]
        public void CodigoLegacyValidoUsaIdGenerado(string codigo)
        {
            Assert.AreEqual(900, _Controlador.ComprasFuncCrear(codigo, 3, 2, 1, Emision, Emision, 1M, 100M, 10M, ""));
            Assert.AreEqual(string.Empty, _Repositorio.CodigoCreado);
        }

        [TestMethod]
        public void ActualizarVencimientoDePendienteEliminaHora()
        {
            var fecha = Emision.AddDays(45).AddHours(9);
            _Controlador.ComprasProcActualizarVencimiento(101, fecha);
            Assert.AreEqual(fecha.Date, _Repositorio.VencimientoActualizado);
        }

        [DataTestMethod]
        [DataRowAttribute(102)]
        [DataRowAttribute(103)]
        [DataRowAttribute(104)]
        [DataRowAttribute(105)]
        [DataRowAttribute(999)]
        public void ActualizarVencimientoRechazaOrdenNoEditable(int id)
        {
            Assert.ThrowsException<InvalidOperationException>(() => _Controlador.ComprasProcActualizarVencimiento(id, Emision.AddDays(60)));
            Assert.AreEqual(default(DateTime), _Repositorio.VencimientoActualizado);
        }

        [TestMethod]
        public void ActualizarVencimientoRechazaBloqueada()
        {
            _Repositorio.Datos.Rows.Find(101)["bloqueadoOrdenCompra"] = true;
            Assert.ThrowsException<InvalidOperationException>(() => _Controlador.ComprasProcActualizarVencimiento(101, Emision));
            Assert.AreEqual(default(DateTime), _Repositorio.VencimientoActualizado);
        }

        [TestMethod]
        public void ActualizarVencimientoRechazaFechaAnterior()
        {
            Assert.ThrowsException<ArgumentException>(() => _Controlador.ComprasProcActualizarVencimiento(101, Emision.AddDays(-1)));
            Assert.AreEqual(default(DateTime), _Repositorio.VencimientoActualizado);
        }

        private int Crear(int proveedor = 3, int plazo = 2, int moneda = 1, decimal cambio = 1M,
            decimal subtotal = 100M, decimal descuento = 10M, decimal impuesto = 0M, string comentario = "Insumos", DateTime? vencimiento = null)
            => _Controlador.ComprasFuncCrear(proveedor, plazo, moneda, Emision, vencimiento ?? Emision.AddDays(30), cambio, subtotal, descuento, comentario, impuesto);

        private static decimal Decimal(string valor) => decimal.Parse(valor, CultureInfo.InvariantCulture);
    }
}
