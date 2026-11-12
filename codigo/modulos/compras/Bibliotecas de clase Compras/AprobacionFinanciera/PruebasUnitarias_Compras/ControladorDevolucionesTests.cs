using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_AprobacionFinanciera;

namespace PruebasUnitarias_Compras
{
    [TestClass]
    public class ControladorDevolucionesTests
    {
        [TestMethod]
        public void RegistrarDevolucion_TotalCero_LanzaExcepcionArgumentException()
        {
            // Arrange
            var controlador = new ClsControladorDevoluciones();
            DataTable detallesFalsos = new DataTable();
            detallesFalsos.Columns.Add("IdProducto");
            detallesFalsos.Columns.Add("IdBodega");
            detallesFalsos.Rows.Add(1, 1);

            decimal totalCero = 0m;

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                controlador.ComprasFuncRegistrarDevolucion(
                    IdCompra: 501,
                    FechaDevolucion: DateTime.Now,
                    Motivo: "Prueba total cero",
                    SerieNota: "A",
                    NumeroNota: "123",
                    TotalDevolucion: totalCero,
                    Detalles: detallesFalsos
                );
            });
        }

        [TestMethod]
        public void RegistrarDevolucion_SinProductos_LanzaExcepcionArgumentException()
        {
            // Arrange
            var controlador = new ClsControladorDevoluciones();
            DataTable detallesVacios = new DataTable();
            detallesVacios.Columns.Add("IdProducto");
            detallesVacios.Columns.Add("IdBodega");

            decimal totalValido = 500m;

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                controlador.ComprasFuncRegistrarDevolucion(
                    IdCompra: 501,
                    FechaDevolucion: DateTime.Now,
                    Motivo: "Prueba sin productos",
                    SerieNota: "A",
                    NumeroNota: "123",
                    TotalDevolucion: totalValido,
                    Detalles: detallesVacios
                );
            });
        }

        [TestMethod]
        public void RegistrarDevolucion_DetallesNulos_LanzaExcepcionArgumentException()
        {
            // Arrange
            var controlador = new ClsControladorDevoluciones();
            DataTable detallesNulos = null;
            decimal totalValido = 500m;

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                controlador.ComprasFuncRegistrarDevolucion(
                    IdCompra: 501,
                    FechaDevolucion: DateTime.Now,
                    Motivo: "Prueba detalles nulos",
                    SerieNota: "A",
                    NumeroNota: "123",
                    TotalDevolucion: totalValido,
                    Detalles: detallesNulos
                );
            });
        }
    }
}