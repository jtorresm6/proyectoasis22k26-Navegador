using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaModelo_AprobacionFinanciera;

namespace PruebasUnitarias_Compras
{
    [TestClass]
    public class RepositoriosLecturaTests
    {
        [TestMethod]
        public void Aprobacion_ListarPendientes_RetornaDataTableSinErrores()
        {
            // Arrange
            var repositorio = new ClsRepositorioAprobacion();

            // Act
            DataTable resultado = repositorio.ComprasFuncListarPendientes();

            // Assert
            // Verificamos que devuelva un objeto válido y que contenga las columnas correctas
            Assert.IsNotNull(resultado, "El repositorio devolvió un null en lugar de un DataTable.");
            Assert.IsTrue(resultado.Columns.Contains("Documento"), "La consulta SQL no trajo la columna 'Documento'.");
        }

        [TestMethod]
        public void Devoluciones_ListarCompras_RetornaDataTableSinErrores()
        {
            // Arrange
            var repositorio = new ClsRepositorioDevoluciones();

            // Act
            DataTable resultado = repositorio.ComprasFuncListarCompras();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Columns.Contains("Proveedor"), "El INNER JOIN con tblProveedor falló o no trajo el nombre.");
        }

        [TestMethod]
        public void Devoluciones_ObtenerEncabezadoCompraInexistente_RetornaTablaVaciaPeroEstructurada()
        {
            // Arrange
            var repositorio = new ClsRepositorioDevoluciones();
            int idCompraFalsa = 9999999; // Un ID que sabemos que no existe

            // Act
            DataTable resultado = repositorio.ComprasFuncObtenerEncabezadoCompra(idCompraFalsa);

            // Assert
            // Al consultar algo que no existe, no debe crashear, solo debe devolver 0 filas
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Rows.Count, "Se esperaban 0 filas para una compra inexistente.");
            Assert.IsTrue(resultado.Columns.Contains("estadoCompra"), "Error en la estructura de columnas SQL.");
        }

        [TestMethod]
        public void Devoluciones_ObtenerDetallesCompraInexistente_RetornaTablaVaciaPeroEstructurada()
        {
            // Arrange
            var repositorio = new ClsRepositorioDevoluciones();
            int idCompraFalsa = 9999999;

            // Act
            DataTable resultado = repositorio.ComprasFuncObtenerDetallesCompra(idCompraFalsa);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Rows.Count);
            Assert.IsTrue(resultado.Columns.Contains("Subtotal"), "La matemática SQL del Subtotal no se generó correctamente.");
        }
    }
}