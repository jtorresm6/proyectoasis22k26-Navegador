using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Cotizaciones;

namespace TestCotizaciones
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
            var Controlador = new ClsCotizacionesControlador();
            var Monedas = Controlador.VentasFuncCatalogo("Monedas");
            Assert.IsTrue(Monedas.Rows.Count > 0, "Se requiere una moneda cargada para consultar.");
            int Moneda = Convert.ToInt32(Monedas.Rows[0][0]);
            var Clientes = Controlador.VentasFuncCatalogo("Clientes");
            Assert.IsTrue(Clientes.Rows.Count > 0, "Se requiere un cliente cargado para consultar.");
            int Cliente = Convert.ToInt32(Clientes.Rows[0][0]);
            // Act
            var Lista = Controlador.VentasFuncListaCliente(Cliente, Moneda, DateTime.Today);
            var Productos = Controlador.VentasFuncProductos(Cliente, Moneda, DateTime.Today);
            var Cotizaciones = Controlador.VentasFuncListar("");
            // Assert
            Assert.IsNotNull(Lista);
            Assert.IsNotNull(Productos);
            Assert.IsTrue(Cotizaciones.Columns.Count > 0);
        }
    }
}
