using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_MovimientosClientes;

namespace TestMovimientosClientes
{
    [TestClass]
    public class TestMovimientosClientes
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(-3)]
        public void ImporteNumerico_ConValorValido_ConservaValor(int Valor)
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            decimal Resultado = Controlador.VentasFuncImporteCelda(Valor);
            // Assert
            Assert.AreEqual((decimal)Valor, Resultado);
        }

        [TestMethod]
        public void ValidarCaptura_Invalida_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetValidarCaptura(false, "Dato requerido");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ValidarCaptura_Valida_Continua()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            bool Aceptada = false;
            // Act
            Controlador.VentasMetValidarCaptura(true, "Dato requerido");
            Aceptada = true;
            // Assert
            Assert.IsTrue(Aceptada);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Documento_Inexistente_NoDevuelveUnaFacturaAjena()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            var Resultado = Controlador.VentasFuncDocumento(0, 0, 0, "NO-EXISTE", true);
            // Assert
            Assert.AreEqual(0, Resultado.Rows.Count);
        }

        [TestMethod]
        public void RegistrarRecibo_MontoCero_RechazaAntesDePersistir()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncRegistrarRecibo(1, 1, 1, "", DateTime.Today, 0m, 1m, "EFECTIVO", "", "", new DataTable());
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void RegistrarRecibo_FormaInvalida_RechazaAntesDePersistir()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncRegistrarRecibo(1, 1, 1, "", DateTime.Today, 1m, 1m, "INVALIDA", "", "", new DataTable());
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void RegistrarDocumento_MontoCero_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncRegistrarDocumento(1, 1, 0, 1, "", 0, DateTime.Today, DateTime.Today, 0m, "", "Prueba");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void EstadoCuenta_FechasInvertidas_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncEstadoCuenta(1, 1, DateTime.Today.AddDays(1), DateTime.Today);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void ModificarDocumento_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetModificarDocumento(0, "FACTURA", 0, "Prueba", DateTime.Today);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void AnularDocumento_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetAnularDocumento(0, "FACTURA", 0, "Prueba");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void ModificarRecibo_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetModificarRecibo(0, 0, "Prueba");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void AnularRecibo_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetAnularRecibo(0, DateTime.Today, 0, "Prueba");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void LeerDocumento_Inexistente_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncLeerDocumento(0, "FACTURA");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void LeerRecibo_Inexistente_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncLeerRecibo(0);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void LeerMovimiento_Inexistente_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncLeerMovimiento(0);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void PrepararDeposito_SinRecibos_Rechaza()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncPrepararDeposito(1, 0, DateTime.Today, "", "1", "1", "1", "1", "", new DataTable());
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Deposito_Inexistente_NoDevuelveUnDocumentoAjeno()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncDeposito("NO-EXISTE-PRUEBA-UNITARIA");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }
    }
}
