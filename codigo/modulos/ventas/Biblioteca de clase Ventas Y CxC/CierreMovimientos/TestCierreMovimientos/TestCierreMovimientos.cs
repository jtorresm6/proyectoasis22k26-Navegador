using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_CierreMovimientos;

namespace TestCierreMovimientos
{
    [TestClass]
    public class TestCierreMovimientos
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(-3)]
        public void ImporteNumerico_ConValorValido_ConservaValor(int Valor)
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            decimal Resultado = Controlador.VentasFuncImporteCelda(Valor);
            // Assert
            Assert.AreEqual((decimal)Valor, Resultado);
        }

        [TestMethod]
        public void ValidarCaptura_Invalida_RechazaOperacion()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetValidarCaptura(false, "Dato requerido");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ValidarCaptura_Valida_Continua()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            bool Aceptada = false;
            // Act
            Controlador.VentasMetValidarCaptura(true, "Dato requerido");
            Aceptada = true;
            // Assert
            Assert.IsTrue(Aceptada);
        }

        [DataTestMethod]
        [DataRow(2024, 2, 29)]
        [DataRow(2026, 2, 28)]
        [DataRow(2026, 12, 31)]
        public void PeriodoMensual_Calendario_CierraEnUltimoDia(int Anio, int Mes, int Dia)
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            var Periodo = Controlador.VentasFuncPeriodo(Anio, Mes, false);
            // Assert
            Assert.AreEqual(new DateTime(Anio, Mes, 1), Periodo[0]);
            Assert.AreEqual(new DateTime(Anio, Mes, Dia), Periodo[1]);
        }

        [TestMethod]
        public void PeriodoAnual_CubreAnioCompleto()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            var Periodo = Controlador.VentasFuncPeriodo(2026, 8, true);
            // Assert
            Assert.AreEqual(new DateTime(2026, 1, 1), Periodo[0]);
            Assert.AreEqual(new DateTime(2026, 12, 31), Periodo[1]);
        }

        [TestMethod]
        public void Traslado_SinEnlace_NoAccedeALaBase()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncTrasladar(1, 1, null);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void GuardarBorrador_PeriodoIncompleto_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncGuardarBorrador(0, 1, 0, "MENSUAL", new DateTime(2026, 10, 2), new DateTime(2026, 10, 31), "");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void Historial_FormatoInvalido_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncHistorial(1, 0, "octubre");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void EliminarBorrador_SinUsuario_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasMetEliminarBorrador(0, 0, "Prueba");
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void Confirmar_SinRevision_Rechaza()
        {
            // Arrange
            var Controlador = new ClsCierreControlador();
            // Act
            Action Operacion = () => Controlador.VentasFuncConfirmar(1, 0, "MENSUAL", new DateTime(2026, 10, 1), new DateTime(2026, 10, 31), false);
            // Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }
    }
}
