using CapaModelo_CierreMovimientos.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026 - 11/11/2026 (AAA)
 * ==================================================================
 * Propósito :
 *  Verifica el cálculo del saldo final por proveedor, la descripción de los
 *  períodos y su estado. La prueba del validador de períodos cerrados
 *  queda comentada junto con la clase (pendiente para el 100 %).
 *  Cada prueba sigue el patrón Arrange / Act / Assert.
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
            // Arrange
            var Saldo = new ClsSaldoProveedorCierre { SaldoInicial = 5000m, Cargos = 22500m, Abonos = 1200m };

            // Act
            decimal SaldoFinal = Saldo.SaldoFinal;

            // Assert
            Assert.AreEqual(26300m, SaldoFinal);
            Assert.IsTrue(Saldo.TieneMovimiento);
        }

        [TestMethod]
        public void ComprasProcProveedorSinMovimientosSeOmite()
        {
            // Arrange
            var SinMovimientos = new ClsSaldoProveedorCierre();
            var SoloSaldoInicial = new ClsSaldoProveedorCierre { SaldoInicial = -150m };

            // Act
            bool SinMovimientosSeIncluye = SinMovimientos.TieneMovimiento;
            bool SoloSaldoInicialSeIncluye = SoloSaldoInicial.TieneMovimiento;

            // Assert
            Assert.IsFalse(SinMovimientosSeIncluye);
            Assert.IsTrue(SoloSaldoInicialSeIncluye);
        }

        [TestMethod]
        public void ComprasProcDescripcionDelPeriodo()
        {
            // Arrange
            var Octubre = new ClsPeriodoCompras { Anio = 2026, Mes = 10 };
            var Enero = new ClsPeriodoCompras { Anio = 2027, Mes = 1 };

            // Act
            string DescripcionOctubre = Octubre.Descripcion;
            string DescripcionEnero = Enero.Descripcion;

            // Assert
            Assert.AreEqual("Octubre 2026", DescripcionOctubre);
            Assert.AreEqual("Enero 2027", DescripcionEnero);
        }

        [TestMethod]
        public void ComprasProcEstadoCerradoDelPeriodo()
        {
            // Arrange
            var Cerrado = new ClsPeriodoCompras { Estado = "Cerrado" };
            var Abierto = new ClsPeriodoCompras { Estado = "Abierto" };

            // Act
            bool CerradoEstaCerrado = Cerrado.EstaCerrado;
            bool AbiertoEstaCerrado = Abierto.EstaCerrado;

            // Assert
            Assert.IsTrue(CerradoEstaCerrado);
            Assert.IsFalse(AbiertoEstaCerrado);
        }

        // Pendiente para el 100 %: se activa junto con ClsValidadorPeriodoCompras.
        // Requiere agregar "using System;" y "using CapaModelo_CierreMovimientos.Servicios;".
        // [TestMethod]
        // public void ComprasProcValidadorDeFechasCerradas()
        // {
        //     // Arrange
        //     DateTime UltimoCierre = new DateTime(2026, 10, 31);
        //
        //     // Act
        //     bool SinCierres = ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 1, 1), null);
        //     bool MesCerrado = ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 9, 15), UltimoCierre);
        //     bool UltimoDiaConHora = ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 10, 31, 18, 30, 0), UltimoCierre);
        //     bool MesAbierto = ClsValidadorPeriodoCompras.ComprasFuncFechaCerrada(new DateTime(2026, 11, 1), UltimoCierre);
        //
        //     // Assert
        //     Assert.IsFalse(SinCierres);
        //     Assert.IsTrue(MesCerrado);
        //     Assert.IsTrue(UltimoDiaConHora);
        //     Assert.IsFalse(MesAbierto);
        // }
    }
}
