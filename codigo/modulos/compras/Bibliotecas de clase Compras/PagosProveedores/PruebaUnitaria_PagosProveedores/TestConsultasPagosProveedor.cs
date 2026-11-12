using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CapaControlador_PagosProveedores;
using CapaModelo_PagosProveedores.Entidades;
using PruebaUnitaria_PagosProveedores.Apoyo;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Diana Mishel Loeiza Ramírez
 * Carné : 9959-23-3457
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica la consulta de pagos por id, el listado y los catálogos.
 * ===================================================================
*/

namespace PruebaUnitaria_PagosProveedores
{
    [TestClass]
    public class TestConsultasPagosProveedor
    {
        private ClsRepositorioPagosProveedorPrueba _Repositorio;
        private ClsModeloPagoProveedor _Controlador;

        [TestInitialize]
        public void ComprasProcPrepararPrueba()
        {
            _Repositorio = new ClsRepositorioPagosProveedorPrueba();
            var Encabezado = new ClsEncabezadoPagoProveedor
            {
                IdPagoProveedor = 4,
                IdProveedor = 2,
                NombreProveedor = "Proveedor Uno",
                FechaPagoProveedor = new DateTime(2026, 10, 5),
                MontoPagoProveedor = 250M,
                IdMetodoPago = 3,
                NombreMetodoPago = "Transferencia",
                IdMoneda = 1,
                ReferenciaPagoProveedor = "REF-9",
                EstadoPagoProveedor = "Pagado"
            };
            Encabezado.Detalles.Add(new ClsDetallePagoProveedor
            {
                SerieDocumento = "A",
                NumeroDocumento = "10",
                IdConcepto = 1,
                NombreConcepto = "Factura",
                MontoAplicado = 250M
            });
            _Repositorio.Datos.Add(Encabezado);
            _Controlador = new ClsModeloPagoProveedor(_Repositorio);
        }

        [TestMethod]
        public void Test_ObtenerPorIdMapeaEncabezado()
        {
            // Arrange
            int IdPago = 4;

            // Act
            var Pago = _Controlador.ComprasMetObtenerPorId(IdPago);

            // Assert
            Assert.IsNotNull(Pago);
            Assert.AreEqual(4, Pago.IdPagoProveedor);
            Assert.AreEqual("PG-0004", Pago.NoPagoProveedor);
            Assert.AreEqual("Proveedor Uno", Pago.NombreProveedor);
            Assert.AreEqual("Transferencia", Pago.NombreMetodoPago);
            Assert.AreEqual(250M, Pago.MontoPagoProveedor);
            Assert.AreEqual("Pagado", Pago.EstadoPagoProveedor);
        }

        [TestMethod]
        public void Test_ObtenerPorIdMapeaDetalles()
        {
            // Arrange
            int IdPago = 4;

            // Act
            var Pago = _Controlador.ComprasMetObtenerPorId(IdPago);

            // Assert
            Assert.AreEqual(1, Pago.Detalles.Count);
            Assert.AreEqual("A", Pago.Detalles[0].SerieDocumento);
            Assert.AreEqual(250M, Pago.TotalAplicado);
        }

        [TestMethod]
        public void Test_ObtenerPorIdInexistente()
        {
            // Arrange
            int IdPago = 999;

            // Act
            var Pago = _Controlador.ComprasMetObtenerPorId(IdPago);

            // Assert
            Assert.IsNull(Pago);
        }

        [TestMethod]
        public void Test_ObtenerTodosMapeaCadaRegistro()
        {
            // Arrange / Act
            var Lista = _Controlador.ComprasMetObtenerTodos();

            // Assert
            Assert.AreEqual(1, Lista.Count);
            Assert.AreEqual("Proveedor Uno", Lista[0].NombreProveedor);
        }

        [TestMethod]
        public void Test_CatalogosDevuelvenLoDelRepositorio()
        {
            // Arrange / Act / Assert
            Assert.AreSame(_Repositorio.Proveedores, _Controlador.ComprasMetObtenerProveedores());
            Assert.AreSame(_Repositorio.MetodosPago, _Controlador.ComprasMetObtenerMetodosPago());
            Assert.AreSame(_Repositorio.Monedas, _Controlador.ComprasMetObtenerMonedas());
            Assert.AreSame(_Repositorio.CuentasPendientes, _Controlador.ComprasMetObtenerCuentasPendientes(2));
        }
    }
}