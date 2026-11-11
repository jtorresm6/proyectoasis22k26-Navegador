using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;
using System.ComponentModel.DataAnnotations;

namespace PruebasUnitarias_TransporteInterno_Logistica
{
    [TestClass]
    public class TestTelemetriaTemperatura
    {
        // Datos válidos compartidos por las pruebas.
        private ClsDatosTelemetriaTemperatura CrearDatosValidos()
        {
            return new ClsDatosTelemetriaTemperatura
            {
                FkEncabezadoViajeCodigo = 1,
                FechaHoraTelemetriaTemperatura =
                    new DateTime(2026, 11, 8, 10, 0, 0),
                TemperaturaCelsiusTelemetriaTemperatura = 4.50m,
                HumedadRelativaTelemetriaTemperatura = 0.6500m,
                PuertaAbiertaTelemetriaTemperatura = false,
                UbicacionGpsTelemetriaTemperatura =
                    "Bodega Central"
            };
        }

        [TestMethod]
        public void Test_Telemetria_DatosValidos()
        {
            // Arrange
            var Datos = CrearDatosValidos();

            // Act
            ClsValidacionesTelemetria.LogisticaProcValidar(Datos);

            // Assert
            // Si no se lanza excepción, la prueba pasa.
        }

        [TestMethod]
        public void Test_Telemetria_TemperaturaInvalida()
        {
            // Arrange
            var Datos = CrearDatosValidos();
            Datos.TemperaturaCelsiusTelemetriaTemperatura = 1000m;

            // Act y Assert
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesTelemetria
                    .LogisticaProcValidar(Datos));
        }

        [TestMethod]
        public void Test_Telemetria_HumedadInvalida()
        {
            // Arrange
            var Datos = CrearDatosValidos();
            Datos.HumedadRelativaTelemetriaTemperatura = 1.5m;

            // Act y Assert
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesTelemetria
                    .LogisticaProcValidar(Datos));
        }

        [TestMethod]
        public void Test_Telemetria_SinViaje()
        {
            // Arrange
            var Datos = CrearDatosValidos();
            Datos.FkEncabezadoViajeCodigo = 0;

            // Act y Assert
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesTelemetria
                    .LogisticaProcValidar(Datos));
        }

        [TestMethod]
        public void Test_Telemetria_SinUbicacion()
        {
            // Arrange
            var Datos = CrearDatosValidos();
            Datos.UbicacionGpsTelemetriaTemperatura = "";

            // Act y Assert
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesTelemetria
                    .LogisticaProcValidar(Datos));
        }
    }
}
