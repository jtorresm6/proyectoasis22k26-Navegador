using CapaControlador_ListadoPrecios;
using CapaModelo_ListadoPrecios.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestListadoPrecios
{
    [TestClass]
    public class TestListadoPrecios
    {
        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConDescuento_AplicaPorcentaje()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(200m, 15m, "Descuento");

            // Assert
            Assert.AreEqual(170m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConAumentoPorcentaje_AumentaPrecioBase()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(100m, 12.5m, "AumentoPorcentaje");

            // Assert
            Assert.AreEqual(112.50m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConAumentoBase_SumaValorAlPrecio()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(75m, 10m, "AumentoBase");

            // Assert
            Assert.AreEqual(85m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConCero_MantienePrecioBase()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(125.50m, 0m, "Descuento");

            // Assert
            Assert.AreEqual(125.50m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConDecimales_RedondeaADosDecimales()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act: el redondeo del modelo utiliza MidpointRounding.AwayFromZero.
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(10m, 2.555m, "AumentoBase");

            // Assert
            Assert.AreEqual(12.56m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConAliasDescuento_AplicaPorcentaje()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(80m, 10m, "Descuento (%)");

            // Assert
            Assert.AreEqual(72m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConAliasAumentoPorcentaje_AplicaPorcentaje()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(80m, 10m, "Aumento por porcentaje (%)");

            // Assert
            Assert.AreEqual(88m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConAliasAumentoBase_SumaValor()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act
            decimal Resultado = Controlador.VentasFuncCalcularPrecioFinal(80m, 5m, "Aumento por base");

            // Assert
            Assert.AreEqual(85m, Resultado);
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConPrecioBaseNegativo_LanzaExcepcion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act y Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Controlador.VentasFuncCalcularPrecioFinal(-10m, 5m, "Descuento"));
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConValorNegativo_LanzaExcepcion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act y Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => Controlador.VentasFuncCalcularPrecioFinal(100m, -5m, "Descuento"));
        }

        [TestMethod]
        public void VentasFuncCalcularPrecioFinal_ConTipoNoReconocido_LanzaExcepcion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();

            // Act y Assert
            Assert.ThrowsException<ArgumentException>(
                () => Controlador.VentasFuncCalcularPrecioFinal(100m, 5m, "Modificacion invalida"));
        }

        [TestMethod]
        public void VentasFuncGuardarVarios_SinRegistros_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();
            string Mensaje;

            // Act
            bool Resultado = Controlador.VentasFuncGuardarVarios(
                new List<ClsPrecioCliente>(), out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("No hay productos seleccionados para guardar.", Mensaje);
        }

        [TestMethod]
        public void VentasFuncGuardarVarios_ConListaNula_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();
            string Mensaje;

            // Act
            bool Resultado = Controlador.VentasFuncGuardarVarios(null, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("No hay productos seleccionados para guardar.", Mensaje);
        }

        [TestMethod]
        public void VentasFuncGuardarVarios_ConRegistroNulo_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();
            string Mensaje;
            var Registros = new List<ClsPrecioCliente> { null };

            // Act
            bool Resultado = Controlador.VentasFuncGuardarVarios(Registros, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("Una de las filas no contiene un identificador de producto válido.", Mensaje);
        }

        [TestMethod]
        public void VentasFuncGuardarVarios_ConIdProductoInvalido_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();
            string Mensaje;
            var Registros = new List<ClsPrecioCliente>
            {
                new ClsPrecioCliente { IdProducto = 0, PrecioFinal = 50m }
            };

            // Act
            bool Resultado = Controlador.VentasFuncGuardarVarios(Registros, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("Una de las filas no contiene un identificador de producto válido.", Mensaje);
        }

        [TestMethod]
        public void VentasFuncGuardarVarios_ConPrecioFinalNegativo_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCtrlListadoPrecios();
            string Mensaje;
            var Registros = new List<ClsPrecioCliente>
            {
                new ClsPrecioCliente { IdProducto = 1, PrecioFinal = -1m }
            };

            // Act
            bool Resultado = Controlador.VentasFuncGuardarVarios(Registros, out Mensaje);

            // Assert
            Assert.IsFalse(Resultado);
            Assert.AreEqual("El precio final no puede ser negativo.", Mensaje);
        }
    }
}
