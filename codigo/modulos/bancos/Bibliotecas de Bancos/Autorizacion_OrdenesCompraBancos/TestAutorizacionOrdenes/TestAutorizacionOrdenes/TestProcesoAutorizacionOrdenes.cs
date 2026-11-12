using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_AutorizacionOrdenes;
using CapaControlador_AutorizacionOrdenes.Modelos_de_controladores;
using CapaModelo_AutorizacionOrdenes.Entidades;
using CapaModelo_AutorizacionOrdenes.Repositorios;

namespace TestAutorizacionOrdenes
{
    [TestClass]
    public class TestControladorAutorizacionOrdenes
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioCuentaDisponible_Agregar_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioCuentaDisponible();
            //Act: intentar agregar un registro
            Repositorio.BancosMetAgregar(new ClsCuentaDisponible());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioCuentaDisponible_Editar_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioCuentaDisponible();
            //Act: intentar editar un registro
            Repositorio.BancosMetEditar(new ClsCuentaDisponible());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioCuentaDisponible_Remover_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioCuentaDisponible();
            //Act: intentar remover un registro
            Repositorio.BancosMetRemover(new ClsCuentaDisponible());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioOrdenCompra_Agregar_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioOrdenCompra();
            //Act: intentar agregar un registro
            Repositorio.BancosMetAgregar(new ClsOrdenCompra());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioOrdenCompra_Editar_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioOrdenCompra();
            //Act: intentar editar un registro
            Repositorio.BancosMetEditar(new ClsOrdenCompra());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioOrdenCompra_Remover_NoSoportado()
        {
            //Arrange: repositorio de solo lectura
            var Repositorio = new ClsRepositorioOrdenCompra();
            //Act: intentar remover un registro
            Repositorio.BancosMetRemover(new ClsOrdenCompra());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioAutorizacionOrdenCompra_Editar_NoSoportado()
        {
            //Arrange: repositorio de autorizaciones, que no se editan ni se eliminan
            var Repositorio = new ClsRepositorioAutorizacionOrdenCompra();
            //Act: intentar editar una autorización
            Repositorio.BancosMetEditar(new ClsAutorizacionOrdenCompra());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Test_RepositorioAutorizacionOrdenCompra_Remover_NoSoportado()
        {
            //Arrange: repositorio de autorizaciones, que no se editan ni se eliminan
            var Repositorio = new ClsRepositorioAutorizacionOrdenCompra();
            //Act: intentar eliminar una autorización
            Repositorio.BancosMetRemover(new ClsAutorizacionOrdenCompra());
            //Assert: debe lanzar NotSupportedException
        }

        [TestMethod]
        public void Test_EntidadAutorizacionOrdenCompra_ListasIniciales_EstanVacias()
        {
            //Arrange: entidad recién creada
            var Entidad = new ClsAutorizacionOrdenCompra();
            //Act: leer la cantidad de evidencias y detalles
            int Evidencias = Entidad.Evidencias.Count;
            int Detalles = Entidad.Detalles.Count;
            //Assert: ambas listas deben estar vacías
            Assert.AreEqual(0, Evidencias);
            Assert.AreEqual(0, Detalles);
        }

        [TestMethod]
        public void Test_EntidadAutorizacionOrdenCompra_Autorizada_GuardaLosValores()
        {
            //Arrange: autorización número 10 de la orden 5, aprobada por el usuario 2
            var Entidad = new ClsAutorizacionOrdenCompra();
            Entidad.IdAutorizacionOrden = 10;
            Entidad.IdOrdenCompra = 5;
            Entidad.EstadoAutorizacionOrden = "AUTORIZADA";
            Entidad.IdUsuarioAutoriza = 2;
            //Act: leer los valores guardados
            long IdAutorizacion = Entidad.IdAutorizacionOrden;
            long IdOrden = Entidad.IdOrdenCompra;
            string Estado = Entidad.EstadoAutorizacionOrden;
            int? IdUsuario = Entidad.IdUsuarioAutoriza;
            //Assert: deben ser los mismos que se asignaron
            Assert.AreEqual(10, IdAutorizacion);
            Assert.AreEqual(5, IdOrden);
            Assert.AreEqual("AUTORIZADA", Estado);
            Assert.AreEqual((int?)2, IdUsuario);
        }

        [TestMethod]
        public void Test_EntidadAutorizacionOrdenCompra_Rechazada_GuardaElMotivo()
        {
            //Arrange: autorización rechazada con motivo y dirección IP
            var Entidad = new ClsAutorizacionOrdenCompra();
            Entidad.EstadoAutorizacionOrden = "RECHAZADA";
            Entidad.MotivoRechazoAutorizacionOrden = "Falta autorización";
            Entidad.Ip = "127.0.0.1";
            //Act: leer el motivo y la IP
            string Motivo = Entidad.MotivoRechazoAutorizacionOrden;
            string Ip = Entidad.Ip;
            //Assert: deben ser los asignados
            Assert.AreEqual("Falta autorización", Motivo);
            Assert.AreEqual("127.0.0.1", Ip);
        }

        [TestMethod]
        public void Test_EntidadAutorizacionOrdenCompra_SinUsuario_IniciaNulo()
        {
            //Arrange: entidad recién creada
            var Entidad = new ClsAutorizacionOrdenCompra();
            //Act: leer el usuario que autoriza
            int? IdUsuario = Entidad.IdUsuarioAutoriza;
            //Assert: debe ser nulo
            Assert.IsNull(IdUsuario);
        }

        [TestMethod]
        public void Test_EntidadEvidenciaFondos_GuardaLosValores()
        {
            //Arrange: evidencia de la cuenta 3 con saldo de 1500.50 y tipo de cambio de 7.65
            var Entidad = new ClsEvidenciaFondos();
            Entidad.IdCuenta = 3;
            Entidad.SaldoDisponibleDisponibilidadFondos = 1500.50m;
            Entidad.TipoCambioDisponibilidadFondos = 7.65m;
            //Act: leer los valores guardados
            long IdCuenta = Entidad.IdCuenta;
            decimal Saldo = Entidad.SaldoDisponibleDisponibilidadFondos;
            decimal TipoCambio = Entidad.TipoCambioDisponibilidadFondos;
            //Assert: deben ser los asignados
            Assert.AreEqual(3, IdCuenta);
            Assert.AreEqual(1500.50m, Saldo);
            Assert.AreEqual(7.65m, TipoCambio);
        }

        [TestMethod]
        public void Test_EntidadEvidenciaFondos_IdTipoCambio_IniciaNulo()
        {
            //Arrange: evidencia recién creada
            var Entidad = new ClsEvidenciaFondos();
            //Act: leer el identificador del tipo de cambio
            long? IdTipoCambio = Entidad.IdTipoCambio;
            //Assert: debe ser nulo
            Assert.IsNull(IdTipoCambio);
        }

        [TestMethod]
        public void Test_EntidadAutorizacionDetalle_GuardaLosValores()
        {
            //Arrange: detalle de la cuenta 3 con monto de 750.25 reservado
            var Entidad = new ClsAutorizacionDetalle();
            Entidad.IdCuenta = 3;
            Entidad.MontoAsignadoPagoOrden = 750.25m;
            Entidad.EstadoPagoOrden = "RESERVADA";
            //Act: leer los valores guardados
            long IdCuenta = Entidad.IdCuenta;
            decimal Monto = Entidad.MontoAsignadoPagoOrden;
            string Estado = Entidad.EstadoPagoOrden;
            //Assert: deben ser los asignados
            Assert.AreEqual(3, IdCuenta);
            Assert.AreEqual(750.25m, Monto);
            Assert.AreEqual("RESERVADA", Estado);
        }

        [TestMethod]
        public void Test_EntidadAutorizacionDetalle_CamposOpcionales_IniciaNulos()
        {
            //Arrange: detalle recién creado
            var Entidad = new ClsAutorizacionDetalle();
            //Act: leer la fecha de reserva y la orden de pago
            DateTime? FechaReserva = Entidad.FechaReservaPagoOrden;
            long? IdOrdenPago = Entidad.IdOrdenPago;
            //Assert: ambos deben ser nulos
            Assert.IsNull(FechaReserva);
            Assert.IsNull(IdOrdenPago);
        }

        [TestMethod]
        public void Test_EntidadCuentaDisponible_GuardaLosValores()
        {
            //Arrange: cuenta 8 de Banrural con saldo de 20000 y reservado de 45000.50
            var Entidad = new ClsCuentaDisponible();
            Entidad.IdCuenta = 8;
            Entidad.NombreBanco = "Banrural";
            Entidad.SaldoDisponible = 20000m;
            Entidad.MontoComprometido = 45000.50m;
            //Act: leer los valores guardados
            long IdCuenta = Entidad.IdCuenta;
            string NombreBanco = Entidad.NombreBanco;
            decimal Saldo = Entidad.SaldoDisponible;
            decimal Comprometido = Entidad.MontoComprometido;
            //Assert: deben ser los asignados
            Assert.AreEqual(8, IdCuenta);
            Assert.AreEqual("Banrural", NombreBanco);
            Assert.AreEqual(20000m, Saldo);
            Assert.AreEqual(45000.50m, Comprometido);
        }

        [TestMethod]
        public void Test_EntidadCuentaDisponible_MontoComprometido_IniciaEnCero()
        {
            //Arrange: cuenta recién creada
            var Entidad = new ClsCuentaDisponible();
            //Act: leer el monto comprometido
            decimal Comprometido = Entidad.MontoComprometido;
            //Assert: debe ser 0
            Assert.AreEqual(0m, Comprometido);
        }

        [TestMethod]
        public void Test_EntidadOrdenCompra_GuardaLosValores()
        {
            //Arrange: orden OC-15 pendiente por 100000
            var Entidad = new ClsOrdenCompra();
            Entidad.IdOrdenCompra = 15;
            Entidad.ReferenciaOrdenCompra = "OC-15";
            Entidad.ImporteOrdenCompra = 100000m;
            Entidad.EstadoOrdenCompra = "Pendiente";
            //Act: leer los valores guardados
            long IdOrden = Entidad.IdOrdenCompra;
            string Referencia = Entidad.ReferenciaOrdenCompra;
            decimal Importe = Entidad.ImporteOrdenCompra;
            string Estado = Entidad.EstadoOrdenCompra;
            //Assert: deben ser los asignados
            Assert.AreEqual(15, IdOrden);
            Assert.AreEqual("OC-15", Referencia);
            Assert.AreEqual(100000m, Importe);
            Assert.AreEqual("Pendiente", Estado);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_ListasIniciales_EstanVacias()
        {
            //Arrange: modelo recién creado
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            //Act: leer la cantidad de evidencias y detalles
            int Evidencias = Modelo.Evidencias.Count;
            int Detalles = Modelo.Detalles.Count;
            //Assert: ambas listas deben estar vacías
            Assert.AreEqual(0, Evidencias);
            Assert.AreEqual(0, Detalles);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_Propiedades_GuardanElValor()
        {
            //Arrange: rechazo de la autorización 10 por falta de fondos desde 127.0.0.1
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            Modelo.IdAutorizacionOrden = 10;
            Modelo.EstadoAutorizacionOrden = "RECHAZADA";
            Modelo.MotivoRechazoAutorizacionOrden = "Sin fondos";
            Modelo.Ip = "127.0.0.1";
            //Act: leer los valores guardados
            long IdAutorizacion = Modelo.IdAutorizacionOrden;
            string Estado = Modelo.EstadoAutorizacionOrden;
            string Motivo = Modelo.MotivoRechazoAutorizacionOrden;
            string Ip = Modelo.Ip;
            //Assert: deben ser los asignados
            Assert.AreEqual(10, IdAutorizacion);
            Assert.AreEqual("RECHAZADA", Estado);
            Assert.AreEqual("Sin fondos", Motivo);
            Assert.AreEqual("127.0.0.1", Ip);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_FechasDeRegistro_IniciaPorDefecto()
        {
            //Arrange: modelo recién creado
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            //Act: leer las fechas de registro
            DateTime Creado = Modelo.CreatedAt;
            DateTime Actualizado = Modelo.UpdatedAt;
            //Assert: ambas deben ser la fecha por defecto
            Assert.AreEqual(default(DateTime), Creado);
            Assert.AreEqual(default(DateTime), Actualizado);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AsignarListas_GuardaLasListas()
        {
            //Arrange: una evidencia y dos detalles
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Evidencias = new List<ClsEvidenciaFondos> { new ClsEvidenciaFondos() };
            var Detalles = new List<ClsAutorizacionDetalle> { new ClsAutorizacionDetalle(), new ClsAutorizacionDetalle() };
            //Act: asignar las listas al modelo
            Modelo.Evidencias = Evidencias;
            Modelo.Detalles = Detalles;
            //Assert: el modelo debe tener 1 evidencia y 2 detalles
            Assert.AreEqual(1, Modelo.Evidencias.Count);
            Assert.AreEqual(2, Modelo.Detalles.Count);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_LimpiarCuentas_VaciaLasListas()
        {
            //Arrange: modelo con una cuenta de 500 agregada
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(1, 1, 1000m, 1m, null);
            Modelo.BancosMetAgregarCuenta(Cuenta, 1, 500m);
            //Act: limpiar las cuentas
            Modelo.BancosMetLimpiarCuentas();
            //Assert: las evidencias y los detalles deben quedar vacíos
            Assert.AreEqual(0, Modelo.Evidencias.Count);
            Assert.AreEqual(0, Modelo.Detalles.Count);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_LimpiarCuentas_SinCuentas_NoFalla()
        {
            //Arrange: modelo sin cuentas agregadas
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            //Act: limpiar las cuentas
            Modelo.BancosMetLimpiarCuentas();
            //Assert: las listas deben seguir vacías
            Assert.AreEqual(0, Modelo.Evidencias.Count);
            Assert.AreEqual(0, Modelo.Detalles.Count);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_MismaMoneda_UsaTipoCambioUno()
        {
            //Arrange: cuenta en quetzales con tipo de cambio de 7.65 para una orden en quetzales
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(1, 1, 1000m, 7.65m, 3);
            //Act: agregar la cuenta con un monto de 500
            Modelo.BancosMetAgregarCuenta(Cuenta, 1, 500m);
            var Evidencia = Modelo.Evidencias[0];
            //Assert: como la moneda es la misma, el tipo de cambio debe ser 1 y sin identificador
            Assert.AreEqual(1m, Evidencia.TipoCambioDisponibilidadFondos);
            Assert.IsNull(Evidencia.IdTipoCambio);
            Assert.AreEqual(1, Evidencia.IdMonedaOrigen);
            Assert.AreEqual(1, Evidencia.IdMonedaDestino);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_MonedaDistinta_UsaTipoCambioDeLaCuenta()
        {
            //Arrange: cuenta en quetzales con tipo de cambio de 7.65 para una orden en dólares
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(1, 1, 1000m, 7.65m, 3);
            //Act: agregar la cuenta con un monto de 500
            Modelo.BancosMetAgregarCuenta(Cuenta, 2, 500m);
            var Evidencia = Modelo.Evidencias[0];
            //Assert: debe usar el tipo de cambio de la cuenta y sus monedas de origen y destino
            Assert.AreEqual(7.65m, Evidencia.TipoCambioDisponibilidadFondos);
            Assert.AreEqual((long?)3, Evidencia.IdTipoCambio);
            Assert.AreEqual(1, Evidencia.IdMonedaOrigen);
            Assert.AreEqual(2, Evidencia.IdMonedaDestino);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_GuardaLaEvidencia()
        {
            //Arrange: cuenta 7 con saldo de 2500 en quetzales
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(7, 1, 2500m, 1m, null);
            //Act: agregar la cuenta con un monto de 500
            Modelo.BancosMetAgregarCuenta(Cuenta, 1, 500m);
            var Evidencia = Modelo.Evidencias[0];
            //Assert: la evidencia debe guardar la cuenta, el saldo, la fecha de hoy y la fuente
            Assert.AreEqual(7, Evidencia.IdCuenta);
            Assert.AreEqual(2500m, Evidencia.SaldoDisponibleDisponibilidadFondos);
            Assert.AreEqual(DateTime.Today, Evidencia.FechaCorteDisponibilidadFondos);
            Assert.AreEqual("tblDisponibilidadDiaria", Evidencia.FuenteDisponibilidadFondos);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_GuardaElDetalleConElMonto()
        {
            //Arrange: cuenta 7 con saldo de 2500 en quetzales
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(7, 1, 2500m, 1m, null);
            //Act: agregar la cuenta con un monto de 500.75
            Modelo.BancosMetAgregarCuenta(Cuenta, 1, 500.75m);
            var Detalle = Modelo.Detalles[0];
            //Assert: el detalle debe guardar la cuenta y el monto exacto
            Assert.AreEqual(7, Detalle.IdCuenta);
            Assert.AreEqual(500.75m, Detalle.MontoAsignadoPagoOrden);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_MontoConCentavos_ConservaLosCentavos()
        {
            //Arrange: cuenta con saldo de 5000 y un monto de 1234.56
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(2, 2, 5000m, 7.65m, 1);
            //Act: agregar la cuenta con el monto de 1234.56
            Modelo.BancosMetAgregarCuenta(Cuenta, 2, 1234.56m);
            //Assert: el detalle debe conservar los centavos
            Assert.AreEqual(1234.56m, Modelo.Detalles[0].MontoAsignadoPagoOrden);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_MontoCero_GuardaCero()
        {
            //Arrange: cuenta con saldo de 1000
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var Cuenta = BancosMetCrearCuenta(1, 1, 1000m, 1m, null);
            //Act: agregar la cuenta con un monto de 0
            Modelo.BancosMetAgregarCuenta(Cuenta, 1, 0m);
            //Assert: el detalle debe guardar 0
            Assert.AreEqual(0m, Modelo.Detalles[0].MontoAsignadoPagoOrden);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_VariasCuentas_AgregaUnaPorCuenta()
        {
            //Arrange: dos cuentas en quetzales de 70000 y 30000
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var PrimeraCuenta = BancosMetCrearCuenta(1, 1, 70000m, 1m, null);
            var SegundaCuenta = BancosMetCrearCuenta(2, 1, 30000m, 1m, null);
            //Act: agregar las dos cuentas con todo su saldo
            Modelo.BancosMetAgregarCuenta(PrimeraCuenta, 1, 70000m);
            Modelo.BancosMetAgregarCuenta(SegundaCuenta, 1, 30000m);
            //Assert: debe haber 2 evidencias y 2 detalles, en el orden agregado
            Assert.AreEqual(2, Modelo.Evidencias.Count);
            Assert.AreEqual(2, Modelo.Detalles.Count);
            Assert.AreEqual(1, Modelo.Detalles[0].IdCuenta);
            Assert.AreEqual(2, Modelo.Detalles[1].IdCuenta);
        }

        [TestMethod]
        public void Test_ModeloAutorizacion_AgregarCuenta_DespuesDeLimpiar_SoloQuedaLaNueva()
        {
            //Arrange: una cuenta agregada y luego limpiada
            var Modelo = new ClsModeloAutorizacionOrdenCompra();
            var PrimeraCuenta = BancosMetCrearCuenta(1, 1, 1000m, 1m, null);
            var SegundaCuenta = BancosMetCrearCuenta(2, 1, 2000m, 1m, null);
            Modelo.BancosMetAgregarCuenta(PrimeraCuenta, 1, 500m);
            Modelo.BancosMetLimpiarCuentas();
            //Act: agregar una segunda cuenta con un monto de 800
            Modelo.BancosMetAgregarCuenta(SegundaCuenta, 1, 800m);
            //Assert: solo debe quedar la segunda cuenta
            Assert.AreEqual(1, Modelo.Detalles.Count);
            Assert.AreEqual(2, Modelo.Detalles[0].IdCuenta);
            Assert.AreEqual(800m, Modelo.Detalles[0].MontoAsignadoPagoOrden);
        }

        [TestMethod]
        public void Test_ModeloCuentaDisponible_Propiedades_GuardanElValor()
        {
            //Arrange: cuenta 4 de Banrural con saldo de 9000, tipo de cambio 2 y reservado de 45000
            var Cuenta = new ClsModeloCuentaDisponible();
            BancosMetAsignarPropiedad(Cuenta, "IdCuenta", 4L);
            BancosMetAsignarPropiedad(Cuenta, "NombreBanco", "Banrural");
            BancosMetAsignarPropiedad(Cuenta, "SaldoDisponible", 9000m);
            BancosMetAsignarPropiedad(Cuenta, "IdTipoCambio", 2L);
            BancosMetAsignarPropiedad(Cuenta, "MontoComprometido", 45000m);
            //Act: leer los valores guardados
            long IdCuenta = Cuenta.IdCuenta;
            string NombreBanco = Cuenta.NombreBanco;
            decimal Saldo = Cuenta.SaldoDisponible;
            long? IdTipoCambio = Cuenta.IdTipoCambio;
            decimal Comprometido = Cuenta.MontoComprometido;
            //Assert: deben ser los asignados
            Assert.AreEqual(4, IdCuenta);
            Assert.AreEqual("Banrural", NombreBanco);
            Assert.AreEqual(9000m, Saldo);
            Assert.AreEqual((long?)2, IdTipoCambio);
            Assert.AreEqual(45000m, Comprometido);
        }

        [TestMethod]
        public void Test_ModeloCuentaDisponible_NombreCuentaCompleto_UneBancoYCuenta()
        {
            //Arrange: cuenta 123456 de Banrural
            var Cuenta = new ClsModeloCuentaDisponible();
            BancosMetAsignarPropiedad(Cuenta, "NombreBanco", "Banrural");
            BancosMetAsignarPropiedad(Cuenta, "NumeroCuenta", "123456");
            //Act: leer el nombre completo de la cuenta
            string NombreCompleto = Cuenta.NombreCuentaCompleto;
            //Assert: debe ser Banrural - 123456
            Assert.AreEqual("Banrural - 123456", NombreCompleto);
        }

        [TestMethod]
        public void Test_ModeloCuentaDisponible_BuscarPorId_EncuentraLaCuenta()
        {
            //Arrange: lista con las cuentas 1 y 2, y se busca la 2
            var Modelo = new ClsModeloCuentaDisponible();
            var Lista = new List<ClsModeloCuentaDisponible> { BancosMetCrearCuenta(1, 1, 1000m, 1m, null), BancosMetCrearCuenta(2, 1, 2000m, 1m, null) };
            BancosMetAsignarCampo(Modelo, "_ListaCuentaDisponible", Lista);
            //Act: buscar la cuenta 2
            var Resultado = Modelo.BancosMetBuscarPorId(2);
            //Assert: debe devolver la cuenta 2 con saldo de 2000
            Assert.IsNotNull(Resultado);
            Assert.AreEqual(2, Resultado.IdCuenta);
            Assert.AreEqual(2000m, Resultado.SaldoDisponible);
        }

        [TestMethod]
        public void Test_ModeloCuentaDisponible_BuscarPorId_NoExiste_DevuelveNulo()
        {
            //Arrange: lista con solo la cuenta 1, y se busca la 99
            var Modelo = new ClsModeloCuentaDisponible();
            var Lista = new List<ClsModeloCuentaDisponible> { BancosMetCrearCuenta(1, 1, 1000m, 1m, null) };
            BancosMetAsignarCampo(Modelo, "_ListaCuentaDisponible", Lista);
            //Act: buscar la cuenta 99
            var Resultado = Modelo.BancosMetBuscarPorId(99);
            //Assert: debe devolver nulo
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ModeloCuentaDisponible_BuscarPorId_ListaVacia_DevuelveNulo()
        {
            //Arrange: lista de cuentas vacía
            var Modelo = new ClsModeloCuentaDisponible();
            var Lista = new List<ClsModeloCuentaDisponible>();
            BancosMetAsignarCampo(Modelo, "_ListaCuentaDisponible", Lista);
            //Act: buscar la cuenta 1
            var Resultado = Modelo.BancosMetBuscarPorId(1);
            //Assert: debe devolver nulo
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ModeloOrdenCompra_Propiedades_GuardanElValor()
        {
            //Arrange: orden OC-15 pendiente por 100000
            var Orden = new ClsModeloOrdenCompra();
            BancosMetAsignarPropiedad(Orden, "IdOrdenCompra", 15L);
            BancosMetAsignarPropiedad(Orden, "ReferenciaOrdenCompra", "OC-15");
            BancosMetAsignarPropiedad(Orden, "ImporteOrdenCompra", 100000m);
            BancosMetAsignarPropiedad(Orden, "EstadoOrdenCompra", "PENDIENTE");
            //Act: leer los valores guardados
            long IdOrden = Orden.IdOrdenCompra;
            string Referencia = Orden.ReferenciaOrdenCompra;
            decimal Importe = Orden.ImporteOrdenCompra;
            string Estado = Orden.EstadoOrdenCompra;
            //Assert: deben ser los asignados
            Assert.AreEqual(15, IdOrden);
            Assert.AreEqual("OC-15", Referencia);
            Assert.AreEqual(100000m, Importe);
            Assert.AreEqual("PENDIENTE", Estado);
        }

        [TestMethod]
        public void Test_ModeloOrdenCompra_BuscarPorId_EncuentraLaOrden()
        {
            //Arrange: lista con las órdenes 10 y 20, y se busca la 20
            var Modelo = new ClsModeloOrdenCompra();
            var Lista = new List<ClsModeloOrdenCompra> { BancosMetCrearOrden(10), BancosMetCrearOrden(20) };
            BancosMetAsignarCampo(Modelo, "_ListaOrdenCompra", Lista);
            //Act: buscar la orden 20
            var Resultado = Modelo.BancosMetBuscarPorId(20);
            //Assert: debe devolver la orden 20
            Assert.IsNotNull(Resultado);
            Assert.AreEqual(20, Resultado.IdOrdenCompra);
        }

        [TestMethod]
        public void Test_ModeloOrdenCompra_BuscarPorId_NoExiste_DevuelveNulo()
        {
            //Arrange: lista con solo la orden 10, y se busca la 99
            var Modelo = new ClsModeloOrdenCompra();
            var Lista = new List<ClsModeloOrdenCompra> { BancosMetCrearOrden(10) };
            BancosMetAsignarCampo(Modelo, "_ListaOrdenCompra", Lista);
            //Act: buscar la orden 99
            var Resultado = Modelo.BancosMetBuscarPorId(99);
            //Assert: debe devolver nulo
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ModeloOrdenCompra_BuscarPorId_ListaVacia_DevuelveNulo()
        {
            //Arrange: lista de órdenes vacía
            var Modelo = new ClsModeloOrdenCompra();
            var Lista = new List<ClsModeloOrdenCompra>();
            BancosMetAsignarCampo(Modelo, "_ListaOrdenCompra", Lista);
            //Act: buscar la orden 1
            var Resultado = Modelo.BancosMetBuscarPorId(1);
            //Assert: debe devolver nulo
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_EstadoEntidad_Valores_SonLosEsperados()
        {
            //Arrange: estados Added, Modified y Deleted
            EstadoEntidad Agregado = EstadoEntidad.Added;
            EstadoEntidad Modificado = EstadoEntidad.Modified;
            EstadoEntidad Eliminado = EstadoEntidad.Deleted;
            //Act: convertir cada estado a número
            int ValorAgregado = (int)Agregado;
            int ValorModificado = (int)Modificado;
            int ValorEliminado = (int)Eliminado;
            //Assert: deben ser 0, 1 y 2
            Assert.AreEqual(0, ValorAgregado);
            Assert.AreEqual(1, ValorModificado);
            Assert.AreEqual(2, ValorEliminado);
        }

        private void BancosMetAsignarPropiedad(object Objeto, string Nombre, object Valor)
        {
            Objeto.GetType().GetProperty(Nombre).GetSetMethod(true).Invoke(Objeto, new object[] { Valor });
        }

        private void BancosMetAsignarCampo(object Objeto, string Nombre, object Valor)
        {
            Objeto.GetType().GetField(Nombre, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Objeto, Valor);
        }

        private ClsModeloCuentaDisponible BancosMetCrearCuenta(long IdCuenta, long IdMoneda, decimal Saldo, decimal TipoCambio, long? IdTipoCambio)
        {
            var Cuenta = new ClsModeloCuentaDisponible();
            BancosMetAsignarPropiedad(Cuenta, "IdCuenta", IdCuenta);
            BancosMetAsignarPropiedad(Cuenta, "IdMoneda", IdMoneda);
            BancosMetAsignarPropiedad(Cuenta, "SaldoDisponible", Saldo);
            BancosMetAsignarPropiedad(Cuenta, "TipoCambio", TipoCambio);
            BancosMetAsignarPropiedad(Cuenta, "IdTipoCambio", IdTipoCambio);
            return Cuenta;
        }

        private ClsModeloOrdenCompra BancosMetCrearOrden(long IdOrden)
        {
            var Orden = new ClsModeloOrdenCompra();
            BancosMetAsignarPropiedad(Orden, "IdOrdenCompra", IdOrden);
            return Orden;
        }
    }
}