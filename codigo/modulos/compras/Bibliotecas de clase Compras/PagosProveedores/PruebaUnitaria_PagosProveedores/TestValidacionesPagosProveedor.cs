using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CapaControlador_PagosProveedores;
using PruebaUnitaria_PagosProveedores.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica las validaciones del encabezado y el detalle del pago.
 * ===================================================================
*/

namespace PruebaUnitaria_PagosProveedores
{
    [TestClass]
    public class TestValidacionesPagosProveedor
    {
        private ClsRepositorioPagosProveedorPrueba _Repositorio;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioPagosProveedorPrueba();
        }

        private static List<string> ComprasFuncValidar(ClsModeloPagoProveedor Pago)
        {
            var Errores = new List<ValidationResult>();
            Validator.TryValidateObject(Pago, new ValidationContext(Pago), Errores, true);
            return Errores.Select(Error => Error.ErrorMessage).ToList();
        }

        [TestMethod]
        public void Test_PagoValidoNoGeneraErrores()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            Assert.AreEqual(0, Errores.Count);
        }

        [TestMethod]
        public void Test_MontoCeroGeneraError()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.MontoPagoProveedor = 0M;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "El monto debe ser mayor a cero");
        }

        [TestMethod]
        public void Test_MontoExcedeMaximo()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.MontoPagoProveedor = 100000000M;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "El monto excede el máximo permitido");
        }

        [TestMethod]
        public void Test_ProveedorNoSeleccionado()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.IdProveedor = 0;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Debe seleccionar un proveedor");
        }

        [TestMethod]
        public void Test_MetodoPagoNoSeleccionado()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.IdMetodoPago = 0;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Debe seleccionar un método de pago");
        }

        [TestMethod]
        public void Test_MonedaNoSeleccionada()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.IdMoneda = 0;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Debe seleccionar una moneda");
        }

        [TestMethod]
        public void Test_ReferenciaMayorDeTreinta()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.ReferenciaPagoProveedor = new string('X', 31);

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "La Referencia no puede exceder 30 caracteres");
        }

        [TestMethod]
        public void Test_ReferenciaDeTreintaEsValida()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.ReferenciaPagoProveedor = new string('X', 30);

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            Assert.AreEqual(0, Errores.Count);
        }

        [TestMethod]
        public void Test_SinDetallesGeneraError()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.Detalles = null;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Debe aplicar el pago al menos a un documento por pagar");
        }

        [TestMethod]
        public void Test_DocumentoRepetidoGeneraError()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.MontoPagoProveedor = 200M;
            Pago.Detalles.Add(ClsRepositorioPagosProveedorPrueba.CrearDetalle("A", "1", 100M));

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Un mismo documento no puede aparecer dos veces en el pago");
        }

        [TestMethod]
        public void Test_MontoAplicadoCeroGeneraError()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.Detalles[0].MontoAplicado = 0M;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            Assert.IsTrue(Errores.Any(Mensaje => Mensaje.Contains("debe ser mayor a cero") && Mensaje.Contains("A-1")));
        }

        [TestMethod]
        public void Test_MontoDistintoDeLaSumaAplicada()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.MontoPagoProveedor = 150M;

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores,
                "El monto del pago debe ser igual a la suma de los montos aplicados a los documentos");
        }

        [TestMethod]
        public void Test_PagoNuevoConEstadoRechazado()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.EstadoPagoProveedor = "Rechazado";

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "Un pago nuevo solo puede quedar Pagado o Programado");
        }

        [TestMethod]
        public void Test_EstadoInvalido()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.EstadoPagoProveedor = "Cualquiera";

            // Act
            var Errores = ComprasFuncValidar(Pago);

            // Assert
            CollectionAssert.Contains(Errores, "El Estado debe ser Pagado, Programado, Rechazado o Anulado");
        }

        [TestMethod]
        public void Test_TotalAplicadoSumaLosDetalles()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.Detalles.Add(ClsRepositorioPagosProveedorPrueba.CrearDetalle("B", "2", 50M));

            // Act
            decimal Total = Pago.TotalAplicado;

            // Assert
            Assert.AreEqual(150M, Total);
        }

        [TestMethod]
        public void Test_NumeroPagoTieneFormatoPG()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.IdPagoProveedor = 7;

            // Act
            string Numero = Pago.NoPagoProveedor;

            // Assert
            Assert.AreEqual("PG-0007", Numero);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ConstructorRechazaRepositorioNulo()
        {
            // Arrange
            CapaModelo_PagosProveedores.Contratos.IRepositorioPagoProveedor Repositorio = null;

            // Act
            new ClsModeloPagoProveedor(Repositorio);

            // Assert: ExpectedException comprueba el tipo de error esperado.
        }
    }
}