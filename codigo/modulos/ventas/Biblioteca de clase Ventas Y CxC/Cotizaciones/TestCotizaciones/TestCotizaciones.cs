using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Cotizaciones;

namespace TestCotizaciones
{
    [TestClass]
    public class TestCotizaciones
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(-3)]
        public void ImporteNumerico_ConValorValido_ConservaValor(int Valor)
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            decimal Resultado = Controlador.VentasFuncImporteCelda(Valor);
            // Assert
            Assert.AreEqual((decimal)Valor, Resultado);
        }

        [TestMethod]
        public void ValidarCaptura_Invalida_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetValidarCaptura(false, "Dato requerido");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ValidarCaptura_Valida_Continua()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            bool Aceptada = false;
            // Act
            Controlador.VentasMetValidarCaptura(true, "Dato requerido");
            Aceptada = true;
            // Assert
            Assert.IsTrue(Aceptada);
        }

        [DataTestMethod]
        [DataRow("BORRADOR", true)]
        [DataRow("EMITIDA", false)]
        [DataRow("ANULADA", false)]
        public void PuedeEditar_ConsultaGuardada_RespetaEstado(string Estado, bool Esperado)
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            bool Resultado = Controlador.VentasFuncPuedeEditar(1, Estado, DateTime.Today.AddDays(1));
            // Assert
            Assert.AreEqual(Esperado, Resultado);
        }

        [TestMethod]
        public void CalcularLinea_ConDescuento_CalculaBaseImpuestoYTotal()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            var Productos = new DataTable();
            foreach (string Campo in new[]
            {
                "Id",
                "Descripcion",
                "Tipo",
                "Precio",
                "Tasa"
            }

            )
                Productos.Columns.Add(Campo, typeof(object));
            DataRow Producto = Productos.Rows.Add(1, "Jamon", "B", 100m, 12m);
            // Act
            var Linea = Controlador.VentasFuncCalcularLinea(Producto, 2m, 20m);
            // Assert
            Assert.AreEqual(200m, Linea[7]);
            Assert.AreEqual(21.60m, Linea[8]);
            Assert.AreEqual(201.60m, Linea[9]);
        }

        [TestMethod]
        public void CalcularLinea_SinProducto_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncCalcularLinea(null, 1m, 0m);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ValidarEdicion_Emitida_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetValidarEdicion(1, "EMITIDA", DateTime.Today.AddDays(1));
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void Guardar_SinLineas_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncGuardar(0, "", 1, 1, 1, 1, DateTime.Today, DateTime.Today.AddDays(1), 1m, "BORRADOR", new DataTable());
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Consultar_Inexistente_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncConsultar("NO-EXISTE-PRUEBA-UNITARIA");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Convertir_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncConvertir(0, false, 1, 0);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Anular_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCotizacionesControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetAnular(0, 0);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }
    }
}
