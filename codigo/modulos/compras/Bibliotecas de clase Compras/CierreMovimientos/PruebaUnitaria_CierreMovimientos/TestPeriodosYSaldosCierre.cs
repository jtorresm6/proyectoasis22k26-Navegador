using System;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_CierreMovimientos.Servicios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica el cálculo del saldo final por proveedor, la descripción de los
 *  períodos y la regla del validador de períodos cerrados.
 * ===================================================================
*/

namespace PruebaUnitaria_CierreMovimientos
{
    [TestClass]
    public class TestPeriodosYSaldosCierre
    {
        [TestMethod]
        public void ComprasProcSaldoFinalCuadra()
        {
            var Saldo = new ClsSaldoProveedorCierre { SaldoInicial = 5000m, Cargos = 22500m, Abonos = 1200m };

            Assert.AreEqual(26300m, Saldo.SaldoFinal);
            Assert.IsTrue(Saldo.TieneMovimiento);
        }

        [TestMethod]
        public void ComprasProcProveedorSinMovimientosSeOmite()
        {
            Assert.IsFalse(new ClsSaldoProveedorCierre().TieneMovimiento);
            Assert.IsTrue(new ClsSaldoProveedorCierre { SaldoInicial = -150m }.TieneMovimiento);
        }

        [TestMethod]
        public void ComprasProcDescripcionDelPeriodo()
        {
            Assert.AreEqual("Octubre 2026", new ClsPeriodoCompras { Anio = 2026, Mes = 10 }.Descripcion);
            Assert.AreEqual("Enero 2027", new ClsPeriodoCompras { Anio = 2027, Mes = 1 }.Descripcion);
        }

        [TestMethod]
        public void ComprasProcEstadoCerradoDelPeriodo()
        {
            Assert.IsTrue(new ClsPeriodoCompras { Estado = "Cerrado" }.EstaCerrado);
            Assert.IsFalse(new ClsPeriodoCompras { Estado = "Abierto" }.EstaCerrado);
        }

        [TestMethod]
        public void ComprasProcValidadorDeFechasCerradas()
        {
            DateTime UltimoCierre = new DateTime(2026, 10, 31);

            Assert.IsFalse(ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 1, 1), null));
            Assert.IsTrue(ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 9, 15), UltimoCierre));
            Assert.IsTrue(ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 10, 31, 18, 30, 0), UltimoCierre));
            Assert.IsFalse(ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 11, 1), UltimoCierre));
        }
    }
}
