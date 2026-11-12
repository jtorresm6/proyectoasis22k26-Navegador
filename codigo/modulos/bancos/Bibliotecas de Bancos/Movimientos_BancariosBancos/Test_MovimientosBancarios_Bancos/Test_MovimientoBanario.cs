/*Oscar Emilio Morales Lemus; 9959-23-3070*/
using CapaModelo_MovimientosBancarios.Entidades;
using CapaModelo_MovimientosBancarios.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test_MovimientosBancarios_Bancos
{
    [TestClass]
    public class Test_MovimientoBanario
    {
        [TestMethod]
        public void Test_Registrar_ReferenciaNula()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.ReferenciaMovimiento = null;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La referencia del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ReferenciaVacia()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.ReferenciaMovimiento = "";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La referencia del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ReferenciaEspacios()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.ReferenciaMovimiento = "   ";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La referencia del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ReferenciaLarga()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.ReferenciaMovimiento = new string('X', 101);

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La referencia del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_NaturalezaNula()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Naturaleza = null;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La naturaleza del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_NaturalezaDesconocida()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Naturaleza = "EXTERNA";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La naturaleza del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_TipoBeneficiarioNulo()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = null;
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));

            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El tipo de beneficiario no es válido.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_TipoBeneficiarioInvalido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "BANCO";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El tipo de beneficiario no es válido.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_IdentificadorConceptoInvalido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdConceptoBancario = 0;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Faltan identificadores obligatorios.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_IdentificadorMetodoPagoInvalido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdMetodoPago = 0;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Faltan identificadores obligatorios.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_IdentificadorTipoTransaccionInvalido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdTipoTransaccion = 0;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Faltan identificadores obligatorios.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_IdentificadorCuentaInvalido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdCuentaEmpresa = 0;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Faltan identificadores obligatorios.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ImporteCero()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 0m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ImporteNegativo()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = -1m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ImporteLimite()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 100000000m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento)); 
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_ImporteCincoDecimales()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 1.12345m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_EquivalenteNoCoincide()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.ImporteGTQ = 200m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Esta versión únicamente permite operaciones GTQ.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_TipoCambioDistinto()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoCambioAplicado = 7.75m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Esta versión únicamente permite operaciones GTQ.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_FechaOperacionFutura()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.FechaOperacion = DateTime.Today.AddDays(1);

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Las fechas no cumplen las reglas de esta versión.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_FechaAplicacionFutura()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.FechaAplicacion = DateTime.Today.AddDays(1);

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Las fechas no cumplen las reglas de esta versión.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_FechaAplicacionAnterior()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.FechaAplicacion = DateTime.Today.AddDays(-1);

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Las fechas no cumplen las reglas de esta versión.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_EMPLEADOSinIdentificador()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "EMPLEADO";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Debe seleccionar un beneficiario y su cuenta.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_PROVEEDORSinIdentificador()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "PROVEEDOR";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Debe seleccionar un beneficiario y su cuenta.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_CLIENTESinIdentificador()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "CLIENTE";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Debe seleccionar un beneficiario y su cuenta.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_EmpleadoSinCuenta()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "EMPLEADO";
            Movimiento.IdBeneficiario = 1;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Debe seleccionar un beneficiario y su cuenta.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_SinTerceroConBeneficiario()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdBeneficiario = 1;
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Este concepto no debe asociarse a un tercero.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_SinTerceroConCuenta()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdCuentaTercero = 1;
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Este concepto no debe asociarse a un tercero.", Error.Message);
        }

        [TestMethod]
        public void Test_Registrar_MovimientoNulo()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            ClsRegistroMovimientoBancario Movimiento = null;
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<ArgumentNullException>(() => Repositorio.BancosMetRegistrarMovimiento(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Agregar_ImporteCero()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 0m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Agregar_ImporteNegativo()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = -25m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("El importe no es válido para DECIMAL(12,4).", Error.Message);
        }

        [TestMethod]
        public void Test_Agregar_NaturalezaInvalida()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Naturaleza = "OTRA";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("La naturaleza del movimiento no es válida.", Error.Message);
        }

        [TestMethod]
        public void Test_Agregar_CuentaInvalida()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.IdCuentaEmpresa = -1;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Faltan identificadores obligatorios.", Error.Message);
        }

        [TestMethod]
        public void Test_Agregar_BeneficiarioObligatorio()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.TipoBeneficiario = "CLIENTE";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Debe seleccionar un beneficiario y su cuenta.", Error.Message);
        }

        [TestMethod]
        public void Test_Agregar_FechaFutura()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.FechaOperacion = DateTime.Today.AddDays(2);

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<InvalidOperationException>(() => Repositorio.BancosMetAgregar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.AreEqual("Las fechas no cumplen las reglas de esta versión.", Error.Message);
        }

        [TestMethod]
        public void Test_Editar_Valido_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetEditar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Editar_ImporteCero_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 0m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetEditar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Editar_NaturalezaInvalida_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Naturaleza = "ERROR";

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetEditar(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Remover_Valido_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetRemover(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Remover_ImporteCero_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Importe = 0m;

            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetRemover(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_Remover_NaturalezaInvalida_NoPermitido()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            var Movimiento = new ClsRegistroMovimientoBancario
            {
                ReferenciaMovimiento = "PRUEBA-001",
                Naturaleza = "INGRESO",
                TipoBeneficiario = "NINGUNO",
                IdCuentaEmpresa = 1,
                IdTipoTransaccion = 1,
                IdConceptoBancario = 1,
                IdMetodoPago = 1,
                Importe = 100m,
                ImporteGTQ = 100m,
                TipoCambioAplicado = 1m,
                FechaOperacion = DateTime.Today,
                FechaAplicacion = DateTime.Today,
                IdBeneficiario = null,
                IdCuentaTercero = null
            };
            Movimiento.Naturaleza = "ERROR";
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetRemover(Movimiento));
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_ObtenerTodos_LanzaExcepcion()
        {
            // Arrange: preparar los datos de prueba.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            // Act: ejecutar el método real.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetObtenerTodos());
            // Assert: comprobar el resultado obtenido.
            Assert.IsNotNull(Error);
        }

        [TestMethod]
        public void Test_ObtenerTodos_IndicaPendiente()
        {
            // Arrange: crear el repositorio.
            var Repositorio = new ClsRepositorioMovimientoBancario();
            // Act: ejecutar la consulta no implementada en la versión revisada.
            var Error = Assert.ThrowsException<NotSupportedException>(() => Repositorio.BancosMetObtenerTodos());
            // Assert: comprobar el mensaje de la excepción.
            StringAssert.Contains(Error.Message, "aún no está implementada");
        }

    }
}
