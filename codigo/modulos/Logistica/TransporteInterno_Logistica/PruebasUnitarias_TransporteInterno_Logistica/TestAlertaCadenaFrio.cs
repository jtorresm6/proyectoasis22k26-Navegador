using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;

namespace PruebasUnitarias_TransporteInterno_Logistica
{
    [TestClass]
    public class TestAlertaCadenaFrio
    {
        // =====================================================
        // DATOS VÁLIDOS PARA LAS PRUEBAS
        // =====================================================

        private ClsDatosAlertaCadenaFrio CrearDatosValidos()
        {
            return new ClsDatosAlertaCadenaFrio
            {
                IdAlertaCadenaFrio = 1,

                FkTelemetriaTemperaturaCodigo = 1,

                TipoAlertaCadenaFrio = "Temperatura alta",

                AtendidaAlertaCadenaFrio = false,

                ObservacionCalidadAlertaCadenaFrio =
                    "Se detectó una temperatura fuera " +
                    "del rango establecido."
            };
        }

        // =====================================================
        // PRUEBA 1: DATOS VÁLIDOS
        // =====================================================

        [TestMethod]
        public void Test_Alerta_DatosValidos()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            // Act.
            ClsValidacionesAlertaCadenaFrio
                .LogisticaProcValidar(Datos);

            // Assert.
            // La prueba pasa si no se produce una excepción.
        }

        // =====================================================
        // PRUEBA 2: TELEMETRÍA NO SELECCIONADA
        // =====================================================

        [TestMethod]
        public void Test_Alerta_SinTelemetria()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.FkTelemetriaTemperaturaCodigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 3: TIPO DE ALERTA VACÍO
        // =====================================================

        [TestMethod]
        public void Test_Alerta_SinTipo()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.TipoAlertaCadenaFrio = "";

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 4: OBSERVACIÓN VACÍA
        // =====================================================

        [TestMethod]
        public void Test_Alerta_SinObservacion()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.ObservacionCalidadAlertaCadenaFrio = "";

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 5: TIPO DE ALERTA MUY LARGO
        // =====================================================

        [TestMethod]
        public void Test_Alerta_TipoExcedeLongitud()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.TipoAlertaCadenaFrio =
                new string('A', 31);

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 6: OBSERVACIÓN MUY LARGA
        // =====================================================

        [TestMethod]
        public void Test_Alerta_ObservacionExcedeLongitud()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.ObservacionCalidadAlertaCadenaFrio =
                new string('A', 501);

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 7: DATOS NULOS
        // =====================================================

        [TestMethod]
        public void Test_Alerta_DatosNulos()
        {
            // Arrange.
            ClsDatosAlertaCadenaFrio Datos = null;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesAlertaCadenaFrio
                    .LogisticaProcValidar(Datos));
        }
    }
}
