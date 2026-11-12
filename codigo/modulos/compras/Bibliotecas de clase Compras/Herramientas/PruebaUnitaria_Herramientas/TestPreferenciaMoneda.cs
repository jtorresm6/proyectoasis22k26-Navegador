using System;
using CapaControlador_Herramientas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica la moneda de visualización que usa el cierre de movimientos para
 *  mostrar el equivalente de sus totales.
 *  Las pruebas no consultan la base de datos: usan casos que el controlador
 *  resuelve antes de buscar un tipo de cambio en Bancos.
 *  Cada prueba sigue el patrón Arrange / Act / Assert.
 * ===================================================================
*/

namespace PruebaUnitaria_Herramientas
{
    [TestClass]
    public class TestPreferenciaMoneda
    {
        private static readonly DateTime _Fecha = new DateTime(2026, 10, 31);
        private static readonly ClsMonedaCompras _Quetzal = new ClsMonedaCompras(1, "GTQ", "Quetzal", "Q");
        private static readonly ClsMonedaCompras _Dolar = new ClsMonedaCompras(2, "USD", "Dólar estadounidense", "US$");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComprasProcMonedaNulaLanzaExcepcion()
        {
            // Arrange
            ClsMonedaCompras SinMoneda = null;

            // Act
            ClsPreferenciaMonedaCompras.ComprasProcEstablecerMoneda(SinMoneda);

            // Assert: lo verifica ExpectedException (ArgumentNullException).
        }

        [TestMethod]
        public void ComprasProcDetectaCuandoSeUsaOtraMoneda()
        {
            // Arrange
            ClsPreferenciaMonedaCompras.ComprasProcEstablecerMoneda(_Dolar);

            // Act
            bool ConDolar = ClsPreferenciaMonedaCompras.ComprasFuncUsaOtraMoneda(_Quetzal);
            bool SinMonedaLocal = ClsPreferenciaMonedaCompras.ComprasFuncUsaOtraMoneda(null);

            // Assert
            Assert.IsTrue(ConDolar);
            Assert.IsFalse(SinMonedaLocal);
            Assert.AreEqual("USD", ClsPreferenciaMonedaCompras.MonedaVisualizacion.Codigo);
        }

        [TestMethod]
        public void ComprasProcMonedaLocalMuestraElMontoSinConvertir()
        {
            // Arrange
            ClsPreferenciaMonedaCompras.ComprasProcEstablecerMoneda(_Quetzal);
            var Controlador = new ClsControladorMoneda();

            // Act
            bool UsaOtraMoneda = ClsPreferenciaMonedaCompras.ComprasFuncUsaOtraMoneda(_Quetzal);
            string Texto = ClsPreferenciaMonedaCompras.ComprasFuncFormatearDesdeLocal(2500m, _Fecha, Controlador, _Quetzal);

            // Assert
            Assert.IsFalse(UsaOtraMoneda);
            Assert.AreEqual("Q 2,500.00", Texto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComprasProcFormatearSinControladorLanzaExcepcion()
        {
            // Arrange
            ClsControladorMoneda SinControlador = null;

            // Act
            ClsPreferenciaMonedaCompras.ComprasFuncFormatearDesdeLocal(100m, _Fecha, SinControlador, _Quetzal);

            // Assert: lo verifica ExpectedException (ArgumentNullException).
        }
    }
}
