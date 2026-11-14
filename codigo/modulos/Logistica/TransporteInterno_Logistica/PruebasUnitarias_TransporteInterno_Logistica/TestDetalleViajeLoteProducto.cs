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
    public class TestDetalleViajeLoteProducto
    {
        // =====================================================
        // DATOS VÁLIDOS PARA LAS PRUEBAS
        // =====================================================

        private ClsDatosDetalleViajeLoteProducto CrearDatosValidos()
        {
            return new ClsDatosDetalleViajeLoteProducto
            {
                IdDetalleViajeLoteProducto = 1,

                FkDetalleViajeCodigo = 1,

                FkLoteProductoCodigo = 1,

                CantidadCajasDetalleViajeLoteProducto = 10,

                PesoKgDetalleViajeLoteProducto = 125.500m
            };
        }

        // =====================================================
        // PRUEBA 1: DATOS VÁLIDOS
        // =====================================================

        [TestMethod]
        public void Test_Lote_DatosValidos()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            // Act.
            ClsValidacionesDetalleViajeLoteProducto
                .LogisticaProcValidar(Datos);

            // Assert.
            // La prueba pasa si no ocurre una excepción.
        }

        // =====================================================
        // PRUEBA 2: DETALLE DE VIAJE NO SELECCIONADO
        // =====================================================

        [TestMethod]
        public void Test_Lote_SinDetalleViaje()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.FkDetalleViajeCodigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 3: LOTE DE PRODUCTO NO SELECCIONADO
        // =====================================================

        [TestMethod]
        public void Test_Lote_SinLoteProducto()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.FkLoteProductoCodigo = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 4: CANTIDAD DE CAJAS IGUAL A CERO
        // =====================================================

        [TestMethod]
        public void Test_Lote_CantidadCero()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.CantidadCajasDetalleViajeLoteProducto = 0;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 5: CANTIDAD DE CAJAS NEGATIVA
        // =====================================================

        [TestMethod]
        public void Test_Lote_CantidadNegativa()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.CantidadCajasDetalleViajeLoteProducto = -5;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 6: PESO NEGATIVO
        // =====================================================

        [TestMethod]
        public void Test_Lote_PesoNegativo()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.PesoKgDetalleViajeLoteProducto = -20.500m;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }

        // =====================================================
        // PRUEBA 7: PESO IGUAL A CERO
        // =====================================================

        [TestMethod]
        public void Test_Lote_PesoCero()
        {
            // Arrange.
            var Datos = CrearDatosValidos();

            Datos.PesoKgDetalleViajeLoteProducto = 0;

            // Act.
            ClsValidacionesDetalleViajeLoteProducto
                .LogisticaProcValidar(Datos);

            // Assert.
            // El peso cero está permitido por las reglas actuales.
        }

        // =====================================================
        // PRUEBA 8: DATOS NULOS
        // =====================================================

        [TestMethod]
        public void Test_Lote_DatosNulos()
        {
            // Arrange.
            ClsDatosDetalleViajeLoteProducto Datos = null;

            // Act y Assert.
            Assert.ThrowsException<ValidationException>(
                () => ClsValidacionesDetalleViajeLoteProducto
                    .LogisticaProcValidar(Datos));
        }
    }
}
