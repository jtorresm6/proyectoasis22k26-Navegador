using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
 *  Verifica el grabado, la edición y la eliminación según el estado
 *  de la entidad, y el manejo de errores.
 * ===================================================================
*/

namespace PruebaUnitaria_PagosProveedores
{
    [TestClass]
    public class TestEstadosPagosProveedor
    {
        private ClsRepositorioPagosProveedorPrueba _Repositorio;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioPagosProveedorPrueba();
        }

        [TestMethod]
        public void Test_AgregarGrabaYAsignaId()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual("Grabacion exitosa", Mensaje);
            Assert.AreEqual(1, _Repositorio.LlamadasAgregar);
            Assert.AreEqual(1, Pago.IdPagoProveedor);
        }

        [TestMethod]
        public void Test_AgregarCopiaEncabezadoYDetalle()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual(100M, _Repositorio.UltimoGuardado.MontoPagoProveedor);
            Assert.AreEqual(1, _Repositorio.UltimoGuardado.Detalles.Count);
            Assert.AreEqual("A", _Repositorio.UltimoGuardado.Detalles[0].SerieDocumento);
        }

        [TestMethod]
        public void Test_AgregarUsaTipoCambioResuelto()
        {
            // Arrange
            _Repositorio.TipoCambioIdADevolver = 5;
            _Repositorio.TipoCambioValorADevolver = 7.75M;
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual(5L, _Repositorio.UltimoGuardado.IdTipoCambio);
            Assert.AreEqual(7.75M, _Repositorio.UltimoGuardado.TipoCambioPagoProveedor);
        }

        [TestMethod]
        public void Test_AgregarRechazaTipoCambioSinIdDistintoDeUno()
        {
            // Arrange
            _Repositorio.TipoCambioIdADevolver = null;
            _Repositorio.TipoCambioValorADevolver = 7.75M;
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            StringAssert.Contains(Mensaje, "el valor debe ser 1");
            Assert.AreEqual(0, _Repositorio.LlamadasAgregar);
        }

        [TestMethod]
        public void Test_AgregarConDatosInvalidosNoGraba()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.IdProveedor = 0;

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            StringAssert.Contains(Mensaje, "Debe seleccionar un proveedor");
            Assert.AreEqual(0, _Repositorio.LlamadasAgregar);
        }

        [TestMethod]
        public void Test_ModificarActualiza()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.Estado = EnmEstadoEntidadCompras.Modified;
            Pago.IdPagoProveedor = 3;

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual("Actualizacion exitosa", Mensaje);
            Assert.AreEqual(1, _Repositorio.LlamadasEditar);
        }

        [TestMethod]
        public void Test_ModificarPermiteEstadoAnulado()
        {
            // Arrange
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);
            Pago.Estado = EnmEstadoEntidadCompras.Modified;
            Pago.EstadoPagoProveedor = "Anulado";

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual("Actualizacion exitosa", Mensaje);
        }

        [TestMethod]
        public void Test_EliminarNoValida()
        {
            // Arrange
            var Pago = new ClsModeloPagoProveedor(_Repositorio)
            {
                Estado = EnmEstadoEntidadCompras.Deleted,
                IdPagoProveedor = 9
            };

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            Assert.AreEqual("Eliminacion exitosa", Mensaje);
            Assert.AreEqual(1, _Repositorio.LlamadasRemover);
        }

        [TestMethod]
        public void Test_ExcepcionDeRepositorioDevuelveMensajeGenerico()
        {
            // Arrange
            _Repositorio.ExcepcionAlGrabar = new ApplicationException("Mensaje de prueba");
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            StringAssert.Contains(Mensaje, "error inesperado");
        }

        [TestMethod]
        public void Test_ExcepcionInesperadaNoExponeDetalleInterno()
        {
            // Arrange
            _Repositorio.ExcepcionAlGrabar = new InvalidOperationException("detalle interno");
            var Pago = ClsRepositorioPagosProveedorPrueba.CrearPagoValido(_Repositorio);

            // Act
            string Mensaje = Pago.ComprasMetGrabarCambios();

            // Assert
            StringAssert.Contains(Mensaje, "error inesperado");
            Assert.IsFalse(Mensaje.Contains("detalle interno"));
        }
    }
}