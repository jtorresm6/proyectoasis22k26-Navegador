using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_CierreMovimientos;

namespace TestCierreMovimientos
{
    [TestClass]
    public class TestConsultas
    {
        [TestMethod]
        [TestCategory("Integracion")]
        public void Consultas_EmbutidosSA_DevuelvenEstructurasEsperadas()
        {
            // Arrange
            Environment.SetEnvironmentVariable("EmbutidosS.A", "Dsn=EmbutidosS.A;Connection Timeout=5");
            var Controlador = new ClsCierreControlador();
            var Monedas = Controlador.VentasFuncCatalogo("Monedas");
            Assert.IsTrue(Monedas.Rows.Count > 0, "Se requiere una moneda cargada para consultar.");
            int Moneda = Convert.ToInt32(Monedas.Rows[0][0]);
            var Periodo = Controlador.VentasFuncPeriodo(DateTime.Today.Year, DateTime.Today.Month, false);
            // Act
            var Lista = Controlador.VentasFuncListar(Moneda);
            var Revision = Controlador.VentasFuncRevisar(Moneda, "MENSUAL", Periodo[0], Periodo[1]);
            var Estado = Controlador.VentasFuncEstadoPeriodo(Moneda, "MENSUAL", Periodo[0], Periodo[1]);
            var Historial = Controlador.VentasFuncHistorial(Moneda, 0, "");
            var Partida = Controlador.VentasFuncPartida(0);
            var Traslado = Controlador.VentasFuncEstadoTraslado(0);
            // Assert
            Assert.IsTrue(Lista.Columns.Count > 0);
            Assert.IsTrue(Revision.Columns.Count > 0);
            Assert.IsTrue(Estado.Columns.Count > 0);
            Assert.IsTrue(Historial.Columns.Count > 0);
            Assert.IsTrue(Partida.Columns.Contains("Debe"));
            Assert.IsTrue(Traslado.Columns.Count > 0);
        }
    }
}
