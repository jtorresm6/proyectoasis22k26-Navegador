
/*
 * ============================================================
 * MÓDULO: LOGÍSTICA
 * PRUEBAS UNITARIAS: CIERRE DE INVENTARIO
 *
 * DESCRIPCIÓN:
 * Verifica las reglas de negocio del cierre mensual.
 *
 * FRAMEWORK: MSTest - .NET Framework
 * ============================================================
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaModelo_CierreInventario_Logistica.Validaciones;

namespace PruebasUnitarias_CierreInventario_Logistica
{
    [TestClass]
    public class UnitTest1
    {
        // =====================================================
        // PRUEBA 1: INVENTARIO CORRECTAMENTE CUADRADO
        // =====================================================

        [TestMethod]
        public void Prueba01_InventarioCuadrado()
        {
            bool resultado =
                ClsValidacionCierreInventario.ValidarCuadratura(
                    100, 20, 5, 115);

            Assert.IsTrue(resultado);
        }

        // =====================================================
        // PRUEBA 2: INVENTARIO CON DIFERENCIAS
        // =====================================================

        [TestMethod]
        public void Prueba02_InventarioConDiferencias()
        {
            bool resultado =
                ClsValidacionCierreInventario.ValidarCuadratura(
                    100, 20, 5, 110);

            Assert.IsFalse(resultado);
        }

        // =====================================================
        // PRUEBA 3: INVENTARIO CON CANTIDADES NEGATIVAS
        // =====================================================

        [TestMethod]
        public void Prueba03_InventarioNegativo()
        {
            bool resultado =
                ClsValidacionCierreInventario.ValidarCuadratura(
                    100, -20, 5, 75);

            Assert.IsFalse(resultado);
        }

        // =====================================================
        // PRUEBA 4: CÁLCULO DEL SALDO FINAL
        // =====================================================

        [TestMethod]
        public void Prueba04_CalcularSaldoFinal()
        {
            long resultado =
                ClsValidacionCierreInventario.CalcularSaldo(
                    100, 20, 5);

            Assert.AreEqual(115L, resultado);
        }

        // =====================================================
        // PRUEBA 5: PERÍODO FINALIZADO
        // =====================================================

        [TestMethod]
        public void Prueba05_PeriodoFinalizado()
        {
            DateTime fechaActual =
                new DateTime(2026, 11, 10);

            bool resultado =
                ClsValidacionCierreInventario.ValidarPeriodoFinalizado(
                    2026, 10, fechaActual);

            Assert.IsTrue(resultado);
        }

        // =====================================================
        // PRUEBA 6: PERÍODO SIN FINALIZAR
        // =====================================================

        [TestMethod]
        public void Prueba06_PeriodoNoFinalizado()
        {
            DateTime fechaActual =
                new DateTime(2026, 11, 10);

            bool resultado =
                ClsValidacionCierreInventario.ValidarPeriodoFinalizado(
                    2026, 11, fechaActual);

            Assert.IsFalse(resultado);
        }
    }
}
