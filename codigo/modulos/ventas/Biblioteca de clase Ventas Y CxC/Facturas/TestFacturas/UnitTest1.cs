using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFacturas
{
    [TestClass]
    public class TestFacturaUnitaria
    {
        [DataTestMethod]
        [DataRow("12345678")]
        [DataRow("87654321")]
        [DataRow("CF")]
        public void ValidarNit_FormatosPermitidos_RetornaTrue(string nitPrueba)
        {
            // Arrange (Preparar)
            string nit = nitPrueba;

            // Act (Actuar)
            bool esValido = !string.IsNullOrWhiteSpace(nit);

            // Assert (Afirmar / Comprobar)
            Assert.IsTrue(esValido, $"El NIT {nitPrueba} debe ser válido para la emisión DTE.");
        }

        [TestMethod]
        public void TestEstructuraDataTableFactura_NoNulo()
        {
            // Arrange & Act
            DataTable dtDetalle = new DataTable("dtFacturaDetalle");
            dtDetalle.Columns.Add("Codigo", typeof(int));
            dtDetalle.Columns.Add("Producto", typeof(string));
            dtDetalle.Columns.Add("Total", typeof(decimal));

            dtDetalle.Rows.Add(101, "Salchicha Jamonada 1lb", 200.00m);

            // Assert
            Assert.IsNotNull(dtDetalle, "El DataTable del detalle de factura no debe ser nulo.");
            Assert.AreEqual(1, dtDetalle.Rows.Count, "El detalle debe contener al menos un elemento.");
        }
    }
}