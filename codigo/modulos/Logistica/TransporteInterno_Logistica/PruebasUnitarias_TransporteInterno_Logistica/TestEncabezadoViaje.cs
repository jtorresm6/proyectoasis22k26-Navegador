using CapaControlador_TransporteInterno_Logistica.Modelos_de_Controladores;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias_TransporteInterno_Logistica
{
    [TestClass]
    public class TestEncabezadoViaje
    {
        // =====================================================
        // DATOS VÁLIDOS PARA LAS PRUEBAS
        // =====================================================

        private ClsDatosEncabezadoViaje CrearDatosValidos()
        {
            return new ClsDatosEncabezadoViaje
            {
                IdEncabezadoViaje = 1,

                FechaDocumentoEncabezadoViaje =
                    new DateTime(2026, 11, 8),

                HoraSalidaEncabezadoViaje =
                    new DateTime(2026, 11, 8, 8, 0, 0),

                HoraLlegadaEncabezadoViaje =
                    new DateTime(2026, 11, 8, 12, 0, 0),

                RequiereCadenaFrioEncabezadoViaje = true,

                EstadoViajeEncabezadoViaje = "EN_CURSO",

                PesoTotalEncabezadoViaje = 150.000m,

                Fk_Transporte_Codigo = 1,

                Fk_Conductor_Codigo = 1
            };
        }

        // =====================================================
        // PRUEBA 1: DATOS VÁLIDOS
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_DatosValidos()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            // Act.
            ClsValidacionesEncabezadoViaje
                .LogisticaProcValidar(Datos);

            // Assert.
            // La prueba pasa si no ocurre una excepción.
        }

        // =====================================================
        // PRUEBA 2: TRANSPORTE NO SELECCIONADO
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_SinTransporte()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.Fk_Transporte_Codigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 3: CONDUCTOR NO SELECCIONADO
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_SinConductor()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.Fk_Conductor_Codigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 4: ESTADO DEL VIAJE VACÍO
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_SinEstado()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.EstadoViajeEncabezadoViaje = "";

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 5: FECHA DE SALIDA INVÁLIDA
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_SalidaInvalida()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.HoraSalidaEncabezadoViaje =
                DateTime.MinValue;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 6: LLEGADA ANTERIOR A SALIDA
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_LlegadaAnterior()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.HoraLlegadaEncabezadoViaje =
                new DateTime(2026, 11, 8, 7, 0, 0);

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 7: FECHA DEL DOCUMENTO INVÁLIDA
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_FechaDocumentoInvalida()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.FechaDocumentoEncabezadoViaje =
                DateTime.MinValue;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 8: DATOS NULOS
        // =====================================================

        [TestMethod]
        public void Test_Encabezado_DatosNulos()
        {
            // Arrange.
            ClsDatosEncabezadoViaje Datos = null;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesEncabezadoViaje
                    .LogisticaProcValidar(Datos));
        }
    }
}
