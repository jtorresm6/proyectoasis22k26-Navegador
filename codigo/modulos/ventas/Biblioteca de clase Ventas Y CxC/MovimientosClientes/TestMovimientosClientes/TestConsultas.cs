using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_MovimientosClientes;

namespace TestMovimientosClientes
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
            var Controlador = new ClsMovimientosControlador();
            var Monedas = Controlador.VentasFuncCatalogo("Monedas");
            Assert.IsTrue(Monedas.Rows.Count > 0, "Se requiere una moneda cargada para consultar.");
            int Moneda = Convert.ToInt32(Monedas.Rows[0][0]);
            var Clientes = Controlador.VentasFuncCatalogo("Clientes");
            Assert.IsTrue(Clientes.Rows.Count > 0, "Se requiere un cliente cargado para consultar.");
            int Cliente = Convert.ToInt32(Clientes.Rows[0][0]);
            // Act
            var Cuenta = Controlador.VentasFuncEstadoCuenta(Cliente, Moneda, new DateTime(2000, 1, 1), DateTime.Today);
            var Pendientes = Controlador.VentasFuncPendientes(Cliente, Moneda);
            var Documentos = Controlador.VentasFuncListarDocumentos(Cliente, Moneda);
            var Recibos = Controlador.VentasFuncListarRecibos(Cliente, Moneda);
            var Consulta = Controlador.VentasFuncConsultarRecibos("", "");
            var Aplicaciones = Controlador.VentasFuncAplicaciones(0);
            var Disponibles = Controlador.VentasFuncRecibosDisponibles(Moneda);
            // Assert
            Assert.AreEqual(2, Cuenta.Tables.Count);
            Assert.IsTrue(Cuenta.Tables[1].Columns.Contains("Saldo"));
            Assert.IsTrue(Pendientes.Columns.Count > 0);
            Assert.IsTrue(Documentos.Columns.Count > 0);
            Assert.IsTrue(Recibos.Columns.Count > 0);
            Assert.IsTrue(Consulta.Columns.Count > 0);
            Assert.IsTrue(Aplicaciones.Columns.Count > 0);
            Assert.IsTrue(Disponibles.Columns.Count > 0);
        }

        [TestMethod]
        [TestCategory("Integracion")]
        public void CatalogosBancosYDocumentos_DevuelvenCamposParaCaptura()
        {
            // Arrange
            var Controlador = new ClsMovimientosControlador();
            // Act
            var Tipos = Controlador.VentasFuncCatalogo("TiposDocumentoCxC");
            var Bancos = Controlador.VentasFuncCatalogo("BancosDeposito");
            var Cuentas = Controlador.VentasFuncCatalogo("CuentasDeposito");
            var Conceptos = Controlador.VentasFuncCatalogo("ConceptosDeposito");
            // Assert
            Assert.AreEqual(5, Tipos.Rows.Count);
            Assert.AreEqual("CARGO", Tipos.Select("Id = 0")[0]["Efecto"]);
            Assert.AreEqual("ABONO", Tipos.Select("Id = 1")[0]["Efecto"]);
            Assert.IsTrue(Bancos.Columns.Contains("Id"));
            Assert.IsTrue(Conceptos.Columns.Contains("Nombre"));
            Assert.IsTrue(Cuentas.Columns.Contains("Banco"));
            Assert.IsTrue(Cuentas.Columns.Contains("Moneda"));
        }
    }
}
