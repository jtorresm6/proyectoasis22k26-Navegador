using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_CierreBancario;

namespace PruebasBancarios
{
    [TestClass]
    public class PruebasControladorCierreBancario
    {
        // BancosMetConsultar

        [TestMethod]
        public void Consultar_FechaInicialFormatoInvalido_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetConsultar("2024-01-01", "31/12/2024", out DataTable _);

            // Assert
            Assert.AreEqual("Fecha inicial no es válida. Use el formato dd/MM/aaaa.", resultado);
        }

        [TestMethod]
        public void Consultar_FechaFinalFormatoInvalido_RetornaMensajeError()
        {
            var ctrl = new ClsControladorCierreBancario();

            string resultado = ctrl.BancosMetConsultar("01/01/2024", "2024-12-31", out DataTable _);

            Assert.AreEqual("Fecha final no es válida. Use el formato dd/MM/aaaa.", resultado);
        }

        [TestMethod]
        public void Consultar_FechaInicialMayorQueFinal_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetConsultar("31/12/2024", "01/01/2024", out DataTable _);

            // Assert
            Assert.AreEqual("La fecha inicial no puede ser mayor que la fecha final.", resultado);
        }

        [TestMethod]
        public void Consultar_FechaInicialFormatoInvalido_ResultadoEsNulo()
        {
            var ctrl = new ClsControladorCierreBancario();

            ctrl.BancosMetConsultar("INVALIDA", "31/12/2024", out DataTable resultado);

            Assert.IsNull(resultado);
        }

        // BancosMetValidarCierre

        [TestMethod]
        public void ValidarCierre_FechaInicialFormatoInvalido_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetValidarCierre(
                "01-01-2024", "31/12/2024",
                out bool _, out bool _, out bool _);

            // Assert
            Assert.AreEqual("Fecha inicial no es válida.", resultado);
        }

        [TestMethod]
        public void ValidarCierre_FechaFinalFormatoInvalido_RetornaMensajeError()
        {
            var ctrl = new ClsControladorCierreBancario();

            string resultado = ctrl.BancosMetValidarCierre(
                "01/01/2024", "2024-12-31",
                out bool _, out bool _, out bool _);

            Assert.AreEqual("Fecha final no es válida.", resultado);
        }

        [TestMethod]
        public void ValidarCierre_FechasInvalidas_BoolsSalenEnFalso()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            ctrl.BancosMetValidarCierre(
                "INVALIDA", "31/12/2024",
                out bool periodoValido,
                out bool conciliacionesVerificadas,
                out bool saldosVerificados);

            // Assert
            Assert.IsFalse(periodoValido);
            Assert.IsFalse(conciliacionesVerificadas);
            Assert.IsFalse(saldosVerificados);
        }

        // BancosMetCerrarPeriodo

        [TestMethod]
        public void CerrarPeriodo_ResponsableNulo_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetCerrarPeriodo("01/01/2024", "31/01/2024", null);

            // Assert
            Assert.AreEqual("Debe indicar el responsable del cierre.", resultado);
        }

        [TestMethod]
        public void CerrarPeriodo_ResponsableVacio_RetornaMensajeError()
        {
            var ctrl = new ClsControladorCierreBancario();

            string resultado = ctrl.BancosMetCerrarPeriodo("01/01/2024", "31/01/2024", "");

            Assert.AreEqual("Debe indicar el responsable del cierre.", resultado);
        }

        [TestMethod]
        public void CerrarPeriodo_ResponsableSoloEspacios_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetCerrarPeriodo("01/01/2024", "31/01/2024", "   ");

            // Assert
            Assert.AreEqual("Debe indicar el responsable del cierre.", resultado);
        }

        [TestMethod]
        public void CerrarPeriodo_FechaInicialFormatoInvalido_RetornaMensajeError()
        {
            // Arrange
            var ctrl = new ClsControladorCierreBancario();

            // Act
            string resultado = ctrl.BancosMetCerrarPeriodo("2024/01/01", "31/01/2024", "Andre");

            // Assert
            Assert.AreEqual("Fecha inicial no es válida.", resultado);
        }

        [TestMethod]
        public void CerrarPeriodo_FechaFinalFormatoInvalido_RetornaMensajeError()
        {
            var ctrl = new ClsControladorCierreBancario();

            string resultado = ctrl.BancosMetCerrarPeriodo("01/01/2024", "2024/01/31", "Andre");

            Assert.AreEqual("Fecha final no es válida.", resultado);
        }
    }
}
