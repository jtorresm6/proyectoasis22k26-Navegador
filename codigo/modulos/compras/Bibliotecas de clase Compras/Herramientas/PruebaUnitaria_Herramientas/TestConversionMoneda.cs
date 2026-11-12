using System;
using System.Collections.Generic;
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
 *  Verifica las reglas del conversor de monedas de Herramientas: lectura del
 *  monto escrito por el usuario, conversión en la misma moneda, formato de
 *  montos y tasas, y elección de la moneda local y la extranjera sugerida.
 *  Las pruebas no consultan la base de datos: usan casos que el controlador
 *  resuelve antes de buscar un tipo de cambio en Bancos.
 *  Cada prueba sigue el patrón Arrange / Act / Assert.
 * ===================================================================
*/

namespace PruebaUnitaria_Herramientas
{
    [TestClass]
    public class TestConversionMoneda
    {
        private static readonly DateTime _Fecha = new DateTime(2026, 11, 6);
        private static readonly ClsMonedaCompras _Quetzal = new ClsMonedaCompras(1, "GTQ", "Quetzal", "Q");
        private static readonly ClsMonedaCompras _Dolar = new ClsMonedaCompras(2, "USD", "Dólar estadounidense", "US$");
        private static readonly ClsMonedaCompras _Euro = new ClsMonedaCompras(3, "EUR", "Euro", "EUR");

        [TestMethod]
        public void ComprasProcMismaMonedaNoCambiaElMonto()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();

            // Act
            ClsResultadoConversion Resultado = Controlador.ComprasFuncConvertir(1234.56m, _Quetzal, _Quetzal, _Fecha);

            // Assert
            Assert.IsTrue(Resultado.Exitoso);
            Assert.AreEqual(1234.56m, Resultado.MontoConvertido);
            Assert.AreEqual(1m, Resultado.TipoCambio);
            Assert.AreEqual("Misma moneda", Resultado.Metodo);
        }

        [TestMethod]
        public void ComprasProcAceptaMontosConSeparadorDeMiles()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();

            // Act
            ClsResultadoConversion ConComa = Controlador.ComprasFuncConvertir("1,234.50", _Dolar, _Dolar, _Fecha);
            ClsResultadoConversion ConPunto = Controlador.ComprasFuncConvertir("1.234,50", _Dolar, _Dolar, _Fecha);

            // Assert
            Assert.IsTrue(ConComa.Exitoso);
            Assert.AreEqual(1234.50m, ConComa.MontoOrigen);
            Assert.IsTrue(ConPunto.Exitoso);
            Assert.AreEqual(1234.50m, ConPunto.MontoOrigen);
        }

        [TestMethod]
        public void ComprasProcRechazaMontosInvalidos()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();
            string[] Invalidos = { "", "abc", "0", "-5", "12.345", "100000000" };

            // Act
            var Resultados = new List<ClsResultadoConversion>();
            foreach (string Monto in Invalidos)
                Resultados.Add(Controlador.ComprasFuncConvertir(Monto, _Quetzal, _Dolar, _Fecha));

            // Assert
            foreach (ClsResultadoConversion Resultado in Resultados)
            {
                Assert.IsFalse(Resultado.Exitoso);
                StringAssert.Contains(Resultado.Mensaje, "monto numérico");
            }
        }

        [TestMethod]
        public void ComprasProcExigeMonedaDeOrigenYDestino()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();

            // Act
            ClsResultadoConversion Resultado = Controlador.ComprasFuncConvertir("100", null, _Dolar, _Fecha);

            // Assert
            Assert.IsFalse(Resultado.Exitoso);
            StringAssert.Contains(Resultado.Mensaje, "Seleccione la moneda");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcHistorialConLaMismaMonedaLanzaExcepcion()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();

            // Act
            Controlador.ComprasFuncObtenerHistorial(_Dolar, _Dolar, _Fecha.AddDays(-30), _Fecha);

            // Assert: lo verifica ExpectedException (InvalidOperationException).
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcHistorialConFechasInvertidasLanzaExcepcion()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();

            // Act
            Controlador.ComprasFuncObtenerHistorial(_Dolar, _Quetzal, _Fecha, _Fecha.AddDays(-30));

            // Assert: lo verifica ExpectedException (InvalidOperationException).
        }

        [TestMethod]
        public void ComprasProcFormateaMontosYTasas()
        {
            // Arrange
            decimal Monto = 1234567.891m;
            decimal Tasa = 7.75m;

            // Act
            string MontoFormateado = ClsControladorMoneda.ComprasFuncFormatearMonto(Monto, _Dolar);
            string TasaFormateada = ClsControladorMoneda.ComprasFuncFormatearTasa(Tasa);

            // Assert
            Assert.AreEqual("US$ 1,234,567.89", MontoFormateado);
            Assert.AreEqual("7.7500", TasaFormateada);
        }

        [TestMethod]
        public void ComprasProcEligeMonedaLocalYExtranjeraSugerida()
        {
            // Arrange
            var Controlador = new ClsControladorMoneda();
            var Monedas = new List<ClsMonedaCompras> { _Euro, _Quetzal, _Dolar };
            var SinDolar = new List<ClsMonedaCompras> { _Quetzal, _Euro };

            // Act
            ClsMonedaCompras Local = Controlador.ComprasFuncObtenerMonedaLocal(Monedas);
            ClsMonedaCompras Sugerida = Controlador.ComprasFuncObtenerMonedaExtranjeraSugerida(Monedas);
            ClsMonedaCompras SugeridaSinDolar = Controlador.ComprasFuncObtenerMonedaExtranjeraSugerida(SinDolar);

            // Assert
            Assert.AreEqual("GTQ", Local.Codigo);
            Assert.AreEqual("USD", Sugerida.Codigo);
            Assert.AreEqual("EUR", SugeridaSinDolar.Codigo);
        }
    }
}
