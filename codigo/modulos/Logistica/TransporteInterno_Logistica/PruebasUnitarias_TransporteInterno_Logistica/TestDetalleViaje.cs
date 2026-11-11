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
    public class TestDetalleViaje
    {
        // =====================================================
        // DATOS VÁLIDOS PARA LAS PRUEBAS
        // =====================================================

        private ClsDatosDetalleViaje CrearDatosValidos()
        {
            return new ClsDatosDetalleViaje
            {
                IdDetalleViaje = 1,
                FkEncabezadoViajeCodigo = 1,
                OrdenDetalleViaje = 1,
                DescripcionDetalleViaje =
                    "Transporte de productos refrigerados",
                CantidadDetalleViaje = 15.000m,
                PesoDetalleViaje = 150.000m
            };
        }

        // =====================================================
        // PRUEBA 1: DATOS VÁLIDOS
        // =====================================================

        [TestMethod]
        public void Test_Detalle_DatosValidos()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            // Act.
            ClsValidacionesDetalleViaje
                .LogisticaProcValidar(Datos);

            // Assert.
            // La prueba pasa si no ocurre una excepción.
        }

        // =====================================================
        // PRUEBA 2: VIAJE NO SELECCIONADO
        // =====================================================

        [TestMethod]
        public void Test_Detalle_SinViaje()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.FkEncabezadoViajeCodigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 3: ORDEN INVÁLIDO
        // =====================================================

        [TestMethod]
        public void Test_Detalle_OrdenInvalido()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.OrdenDetalleViaje = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 4: DESCRIPCIÓN VACÍA
        // =====================================================

        [TestMethod]
        public void Test_Detalle_SinDescripcion()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.DescripcionDetalleViaje = "";

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 5: DESCRIPCIÓN MAYOR A 255 CARACTERES
        // =====================================================

        [TestMethod]
        public void Test_Detalle_DescripcionExcedeLongitud()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.DescripcionDetalleViaje =
                new string('A', 256);

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 6: CANTIDAD INVÁLIDA
        // =====================================================

        [TestMethod]
        public void Test_Detalle_CantidadInvalida()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.CantidadDetalleViaje = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 7: PESO NEGATIVO
        // =====================================================

        [TestMethod]
        public void Test_Detalle_PesoNegativo()
        {
            // Arrange.
            var Datos = CrearDatosValidos();
            Datos.PesoDetalleViaje = -10.000m;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 8: DATOS NULOS
        // =====================================================

        [TestMethod]
        public void Test_Detalle_DatosNulos()
        {
            // Arrange.
            ClsDatosDetalleViaje Datos = null;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViaje
                    .LogisticaProcValidar(Datos));
        }
    }
}
