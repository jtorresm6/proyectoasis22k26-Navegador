using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CapaControlador_TipoCambio;
using CapaModelo_TipoCambio.Entidades;
using CapaModelo_TipoCambio.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestTipoCambioBancos
{
    [TestClass]
    public class PruebasTipoCambio
    {
        [TestMethod]
        public void CrearPar_UsdAGtq_ConTasaQuemada_ConservaTasaDirecta()
        {
            // Arrange: 1 USD = Q7.75 y GTQ usa la tasa de identidad 1.
            var usd = CrearTasa("USD", 7.75m);
            var gtq = CrearTasa("GTQ", 1m);
            MethodInfo metodo = typeof(ClsRepositorioTipoCambio).GetMethod(
                "CrearPar", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(metodo, "No se encontró el cálculo interno de pares.");

            // Act
            var resultado = (ClsTipoCambio)metodo.Invoke(null, new object[] { usd, gtq });

            // Assert
            Assert.AreEqual("USD", resultado.MonedaOrigen);
            Assert.AreEqual("GTQ", resultado.MonedaDestino);
            Assert.AreEqual(7.75m, resultado.Tasa);
            Assert.AreEqual("7.7500", resultado.TasaTexto);
            Assert.AreEqual("DIRECTA", resultado.Metodo);
        }

        [TestMethod]
        public void CrearPar_ConTasasQuemadas_CalculaConversionCruzada()
        {
            // Arrange: 1 USD = Q7.75 y 1 EUR = Q8.50.
            var usd = CrearTasa("USD", 7.75m);
            var eur = CrearTasa("EUR", 8.50m);
            MethodInfo metodo = typeof(ClsRepositorioTipoCambio).GetMethod(
                "CrearPar", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(metodo, "No se encontró el cálculo interno de pares.");

            // Act
            var resultado = (ClsTipoCambio)metodo.Invoke(null, new object[] { usd, eur });

            // Assert
            Assert.AreEqual("USD", resultado.MonedaOrigen);
            Assert.AreEqual("EUR", resultado.MonedaDestino);
            Assert.AreEqual(7.75m / 8.50m, resultado.Tasa);
            Assert.AreEqual("0.9118", resultado.TasaTexto);
            Assert.AreEqual("CRUZADA (promedio)", resultado.Metodo);
        }

        [TestMethod]
        public void CrearIdentidad_ConMonedaQuemada_DevuelveTasaUno()
        {
            // Arrange
            var moneda = new ClsMoneda
            {
                IdBanguat = 2,
                Codigo = "USD",
                Nombre = "Dólar estadounidense"
            };
            DateTime fecha = new DateTime(2026, 10, 31);
            MethodInfo metodo = typeof(ClsRepositorioTipoCambio).GetMethod(
                "CrearIdentidad", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(metodo, "No se encontró el cálculo interno de identidad.");

            // Act
            var resultado = (ClsTipoCambio)metodo.Invoke(null, new object[] { fecha, moneda });

            // Assert
            Assert.AreEqual(1m, resultado.Tasa);
            Assert.AreEqual("1.0000", resultado.TasaTexto);
            Assert.AreEqual("USD", resultado.MonedaOrigen);
            Assert.AreEqual("USD", resultado.MonedaDestino);
            Assert.AreEqual("IDENTIDAD", resultado.Metodo);
        }

        [TestMethod]
        public async Task ConvertirAsync_ImporteNegativo_RechazaAntesDeConsultarServicio()
        {
            // Arrange
            var repositorio = new ClsRepositorioTipoCambio();

            // Act y Assert
            var error = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() =>
                repositorio.BancosMetConvertirAsync(
                    -1m, DateTime.Today, "USD", "GTQ", CancellationToken.None));
            StringAssert.Contains(error.Message, "no puede ser negativo");
        }

        [TestMethod]
        public async Task ObtenerHistoricoAsync_RangoInvertido_RechazaAntesDeConsultarServicio()
        {
            // Arrange
            var repositorio = new ClsRepositorioTipoCambio();
            DateTime desde = new DateTime(2026, 10, 31);
            DateTime hasta = new DateTime(2026, 10, 30);

            // Act y Assert
            var error = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                repositorio.BancosMetObtenerHistoricoAsync(
                    desde, hasta, "USD", "GTQ", CancellationToken.None));
            StringAssert.Contains(error.Message, "fecha inicial");
        }

        [TestMethod]
        public async Task ObtenerTasasActualesAsync_FechaFutura_RechazaAntesDeConsultarServicio()
        {
            // Arrange
            var repositorio = new ClsRepositorioTipoCambio();

            // Act y Assert
            var error = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                repositorio.BancosMetObtenerTasasActualesAsync(
                    DateTime.Today.AddDays(1), true, false, CancellationToken.None));
            StringAssert.Contains(error.Message, "fechas futuras");
        }

        [TestMethod]
        public void Controlador_RepositorioNulo_RechazaLaDependencia()
        {
            // Act y Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                new ClsControladorTipoCambio(null));
        }

        [TestMethod]
        public void EntidadesTipoCambio_ConDatosQuemados_ConservanSusValores()
        {
            // Arrange y Act
            DateTime fecha = new DateTime(2026, 10, 31);
            var tasa = new ClsTipoCambio
            {
                FechaTasa = fecha,
                CodigoMoneda = "USD",
                NombreMoneda = "Dólar estadounidense",
                MonedaOrigen = "USD",
                MonedaDestino = "GTQ",
                Tasa = 7.75m,
                TasaTexto = "7.7500",
                Fuente = "Dato quemado",
                Estado = "Disponible",
                Metodo = "PRUEBA"
            };
            var conversion = new ClsResultadoConversion
            {
                Resultado = 775m,
                Tasa = tasa.Tasa,
                Fecha = fecha,
                MonedaOrigen = tasa.MonedaOrigen,
                MonedaDestino = tasa.MonedaDestino,
                Fuente = tasa.Fuente,
                Metodo = tasa.Metodo
            };

            // Assert
            Assert.AreEqual(7.75m, tasa.Tasa);
            Assert.AreEqual("7.7500", tasa.TasaTexto);
            Assert.AreEqual(775m, conversion.Resultado);
            Assert.AreEqual("USD", conversion.MonedaOrigen);
            Assert.AreEqual("GTQ", conversion.MonedaDestino);
        }

        private static ClsTipoCambio CrearTasa(string codigo, decimal tasa)
        {
            return new ClsTipoCambio
            {
                FechaTasa = new DateTime(2026, 10, 31),
                CodigoMoneda = codigo,
                NombreMoneda = codigo,
                MonedaOrigen = codigo,
                MonedaDestino = "GTQ",
                Tasa = tasa,
                TasaTexto = tasa.ToString("F4", System.Globalization.CultureInfo.InvariantCulture),
                Fuente = "Dato quemado",
                Estado = "Disponible",
                Metodo = "DIRECTA"
            };
        }
    }
}
