using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_GeneracionPagos;
using CapaModelo_GeneracionPagos.Entidades;
using CapaModelo_GeneracionPagos.Repositorio;

namespace TestGeneracionPagos
{
    [TestClass]
    public class TestControladorGeneracionPagos
    {
        [TestMethod]
        public void Test_FormatearImporte_Quetzales_AgregaSimboloQ()
        {
            //Arrange: importe de 1500 en quetzales
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(1500m, "GTQ");
            //Assert: debe ser Q 1,500.00
            Assert.AreEqual("Q 1,500.00", Texto);
        }

        [TestMethod]
        public void Test_FormatearImporte_Dolares_AgregaSimboloUS()
        {
            //Arrange: importe de 1200.50 en dólares
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(1200.50m, "USD");
            //Assert: debe ser US$ 1,200.50
            Assert.AreEqual("US$ 1,200.50", Texto);
        }

        [TestMethod]
        public void Test_FormatearImporte_OtraMoneda_AgregaElCodigoDeMoneda()
        {
            //Arrange: importe de 350 en euros
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(350m, "EUR");
            //Assert: debe ser EUR 350.00
            Assert.AreEqual("EUR 350.00", Texto);
        }

        [TestMethod]
        public void Test_FormatearImporte_Cero_MuestraDosDecimales()
        {
            //Arrange: importe de 0 en quetzales
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(0m, "GTQ");
            //Assert: debe ser Q 0.00
            Assert.AreEqual("Q 0.00", Texto);
        }

        [TestMethod]
        public void Test_FormatearImporte_MasDeDosDecimales_RedondeaADos()
        {
            //Arrange: importe de 10.456 en quetzales
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(10.456m, "GTQ");
            //Assert: debe ser Q 10.46
            Assert.AreEqual("Q 10.46", Texto);
        }

        [TestMethod]
        public void Test_FormatearImporte_MilesYMillones_UsaComaComoSeparador()
        {
            //Arrange: importe de 1250000 en quetzales
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: formatear el importe
            string Texto = Controlador.BancosFuncFormatearImporte(1250000m, "GTQ");
            //Assert: debe ser Q 1,250,000.00
            Assert.AreEqual("Q 1,250,000.00", Texto);
        }

        [TestMethod]
        public void Test_TipoCambio_Quetzales_DevuelveUno()
        {
            //Arrange: moneda base GTQ
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: obtener el tipo de cambio
            decimal TipoCambio = Controlador.BancosFuncObtenerTipoCambio("GTQ");
            //Assert: debe ser 1
            Assert.AreEqual(1m, TipoCambio);
        }

        [TestMethod]
        public void Test_Equivalente_1000ATipoCambio775_Devuelve7750()
        {
            //Arrange: monto de 1000 con tipo de cambio de 7.75
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente
            decimal Equivalente = Controlador.BancosFuncCalcularEquivalente(1000m, 7.75m);
            //Assert: debe ser 7750
            Assert.AreEqual(7750m, Equivalente);
        }

        [TestMethod]
        public void Test_Equivalente_TipoCambioUno_DevuelveElMismoMonto()
        {
            //Arrange: monto de 2500 con tipo de cambio de 1
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente
            decimal Equivalente = Controlador.BancosFuncCalcularEquivalente(2500m, 1m);
            //Assert: debe ser 2500
            Assert.AreEqual(2500m, Equivalente);
        }

        [TestMethod]
        public void Test_Equivalente_MontoCero_DevuelveCero()
        {
            //Arrange: monto de 0 con tipo de cambio de 7.75
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente
            decimal Equivalente = Controlador.BancosFuncCalcularEquivalente(0m, 7.75m);
            //Assert: debe ser 0
            Assert.AreEqual(0m, Equivalente);
        }

        [TestMethod]
        public void Test_Equivalente_MontoConCentavos_CalculaConCuatroDecimales()
        {
            //Arrange: monto de 150.50 con tipo de cambio de 7.7512
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente
            decimal Equivalente = Controlador.BancosFuncCalcularEquivalente(150.50m, 7.7512m);
            //Assert: debe ser 1166.5556
            Assert.AreEqual(1166.5556m, Equivalente);
        }

        [TestMethod]
        public void Test_Equivalente_MasDeCuatroDecimales_RedondeaACuatro()
        {
            //Arrange: monto de 1 con tipo de cambio de 1.23456
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente
            decimal Equivalente = Controlador.BancosFuncCalcularEquivalente(1m, 1.23456m);
            //Assert: debe ser 1.2346
            Assert.AreEqual(1.2346m, Equivalente);
        }

        [TestMethod]
        public void Test_Equivalente_Repositorio_CoincideConElControlador()
        {
            //Arrange: monto de 1000 con tipo de cambio de 7.75
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: calcular el equivalente en el repositorio y en el controlador
            decimal DesdeRepositorio = ClsRepositorioGeneracionPagos.BancosFuncCalcularEquivalente(1000m, 7.75m);
            decimal DesdeControlador = Controlador.BancosFuncCalcularEquivalente(1000m, 7.75m);
            //Assert: ambos deben dar lo mismo
            Assert.AreEqual(DesdeRepositorio, DesdeControlador);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_Cheque_DevuelveCHEQUE()
        {
            //Arrange: tipo de pago llamado Cheque
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("Cheque");
            //Assert: debe ser CHEQUE
            Assert.AreEqual("CHEQUE", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_ChequeEnMayusculas_DevuelveCHEQUE()
        {
            //Arrange: tipo de pago llamado CHEQUE DE CAJA
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("CHEQUE DE CAJA");
            //Assert: debe ser CHEQUE
            Assert.AreEqual("CHEQUE", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_Transferencia_DevuelveTRANSFERENCIA()
        {
            //Arrange: tipo de pago llamado Transferencia bancaria
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("Transferencia bancaria");
            //Assert: debe ser TRANSFERENCIA
            Assert.AreEqual("TRANSFERENCIA", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_TransferenciaEnMinusculas_DevuelveTRANSFERENCIA()
        {
            //Arrange: tipo de pago llamado transferencia
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("transferencia");
            //Assert: debe ser TRANSFERENCIA
            Assert.AreEqual("TRANSFERENCIA", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_Efectivo_DevuelveSuPropioNombre()
        {
            //Arrange: tipo de pago llamado Efectivo (ya no hay restricción a cheque o transferencia)
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("Efectivo");
            //Assert: el medio es el nombre del tipo de pago
            Assert.AreEqual("Efectivo", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_NombreConEspacios_SeRecorta()
        {
            //Arrange: tipo de pago con espacios alrededor
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("  Deposito en ventanilla ");
            //Assert: sin espacios sobrantes
            Assert.AreEqual("Deposito en ventanilla", Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_NombreLargo_SeRecortaA30()
        {
            //Arrange: nombre de más de 30 caracteres (largo de la columna medioOrdenPago)
            string Nombre = "Pago por convenio con proveedor extranjero";
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago(Nombre);
            //Assert: cabe en la columna
            Assert.IsTrue(Medio.Length <= 30);
            Assert.IsTrue(Nombre.StartsWith(Medio));
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_TextoVacio_NoTieneMedio()
        {
            //Arrange: tipo de pago sin nombre
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago("");
            //Assert: no debe haber medio (null)
            Assert.IsNull(Medio);
        }

        [TestMethod]
        public void Test_MedioDeTipoPago_Nulo_NoTieneMedio()
        {
            //Arrange: tipo de pago nulo
            //Act: obtener el medio de pago
            string Medio = ClsRepositorioGeneracionPagos.BancosFuncMedioDeTipoPago(null);
            //Assert: no debe haber medio (null)
            Assert.IsNull(Medio);
        }

        [TestMethod]
        public void Test_OrdenPago_OrigenTransporte_SoloInformaLaSolicitud()
        {
            //Arrange: orden de pago con origen en una solicitud de transporte
            var Orden = new ClsOrdenPago { IdSolicitudPagoTransporte = 7 };
            //Act: leer los orígenes de la orden
            //Assert: solo debe venir informada la solicitud de transporte
            Assert.AreEqual(7L, Orden.IdSolicitudPagoTransporte);
            Assert.IsNull(Orden.IdOrdenCompraRecibida);
            Assert.IsNull(Orden.IdDetallePlanillaRecibida);
        }

        [TestMethod]
        public void Test_PrepararOrden_OrigenDesconocido_PideSeleccionarObligacion()
        {
            //Arrange: origen que no existe
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: preparar la orden de pago
            ClsResultadoOperacion Resultado = Controlador.BancosMetPrepararOrden("X", 1, 1, 1, 500m, new DateTime(2026, 11, 5), 1, "usuario", 1);
            //Assert: no debe prepararse y debe mostrar el mensaje
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione una obligación de la lista.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_PrepararOrden_OrigenVacio_PideSeleccionarObligacion()
        {
            //Arrange: origen vacío
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: preparar la orden de pago
            ClsResultadoOperacion Resultado = Controlador.BancosMetPrepararOrden("", 1, 1, 1, 500m, new DateTime(2026, 11, 5), 1, "usuario", 1);
            //Assert: no debe prepararse y debe mostrar el mensaje
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione una obligación de la lista.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_AutorizarOrden_AutorizadorVacio_PideSeleccionarAutorizador()
        {
            //Arrange: autorizador vacío
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: autorizar la orden
            ClsResultadoOperacion Resultado = Controlador.BancosMetAutorizarOrden(1, "", 1);
            //Assert: no debe registrarse y debe pedir el autorizador
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione el autorizador.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_AutorizarOrden_AutorizadorNulo_PideSeleccionarAutorizador()
        {
            //Arrange: autorizador nulo
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: autorizar la orden
            ClsResultadoOperacion Resultado = Controlador.BancosMetAutorizarOrden(1, null, 1);
            //Assert: no debe registrarse y debe pedir el autorizador
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione el autorizador.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_AutorizarOrden_AutorizadorSoloEspacios_PideSeleccionarAutorizador()
        {
            //Arrange: autorizador con solo espacios
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: autorizar la orden
            ClsResultadoOperacion Resultado = Controlador.BancosMetAutorizarOrden(1, "   ", 1);
            //Assert: no debe registrarse y debe pedir el autorizador
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione el autorizador.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_RechazarOrden_AutorizadorVacio_PideSeleccionarAutorizador()
        {
            //Arrange: autorizador vacío
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: rechazar la orden
            ClsResultadoOperacion Resultado = Controlador.BancosMetRechazarOrden(1, "", 1);
            //Assert: no debe registrarse y debe pedir el autorizador
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione el autorizador.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_RechazarOrden_AutorizadorNulo_PideSeleccionarAutorizador()
        {
            //Arrange: autorizador nulo
            var Controlador = new ClsControladorGeneracionPagos();
            //Act: rechazar la orden
            ClsResultadoOperacion Resultado = Controlador.BancosMetRechazarOrden(1, null, 1);
            //Assert: no debe registrarse y debe pedir el autorizador
            Assert.AreEqual(false, Resultado.Exito);
            Assert.AreEqual("Seleccione el autorizador.", Resultado.Mensaje);
        }

        [TestMethod]
        public void Test_ResultadoOperacion_SinValor_ConservaExitoYMensaje()
        {
            //Arrange: resultado exitoso con mensaje y sin valor
            //Act: crear el resultado
            var Resultado = new ClsResultadoOperacion(true, "Listo");
            //Assert: debe conservar el éxito y el mensaje, con el valor vacío
            Assert.AreEqual(true, Resultado.Exito);
            Assert.AreEqual("Listo", Resultado.Mensaje);
            Assert.IsNull(Resultado.Valor);
        }

        [TestMethod]
        public void Test_ResultadoOperacion_ConValor_ConservaElValor()
        {
            //Arrange: resultado exitoso con el número de orden 25
            //Act: crear el resultado
            var Resultado = new ClsResultadoOperacion(true, "Listo", "25");
            //Assert: debe conservar el valor 25
            Assert.AreEqual("25", Resultado.Valor);
        }

        [TestMethod]
        public void Test_ValoresPago_Estados_CoincideConLaVista()
        {
            //Arrange: estados definidos en el modelo y en la vista
            //Act: comparar los valores
            string EstadoVista = ClsValoresPagoVista.EstadoPendiente;
            //Assert: deben ser iguales
            Assert.AreEqual(ClsValoresPago.EstadoPendiente, EstadoVista);
            Assert.AreEqual(ClsValoresPago.EstadoAutorizada, ClsValoresPagoVista.EstadoAutorizada);
            Assert.AreEqual(ClsValoresPago.EstadoRechazada, ClsValoresPagoVista.EstadoRechazada);
            Assert.AreEqual(ClsValoresPago.EstadoEmitida, ClsValoresPagoVista.EstadoEmitida);
            Assert.AreEqual(ClsValoresPago.EstadoPagada, ClsValoresPagoVista.EstadoPagada);
        }

        [TestMethod]
        public void Test_Vencimientos_ObligacionVencida_CuentaComoVencida()
        {
            //Arrange: obligación que venció hace 6 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0001", Hoy.AddDays(-6), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: debe haber 1 vencida y faltar -6 días
            Assert.AreEqual(1, Resumen.Vencidas);
            Assert.AreEqual(-6, Resumen.Avisos[0].Dias);
        }

        [TestMethod]
        public void Test_Vencimientos_VenceHoy_CuentaComoVenceHoy()
        {
            //Arrange: obligación que vence hoy
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0002", Hoy.AddDays(0), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: debe haber 1 que vence hoy
            Assert.AreEqual(1, Resumen.VencenHoy);
            Assert.AreEqual(0, Resumen.Vencidas);
        }

        [TestMethod]
        public void Test_Vencimientos_VenceEnTresDias_CuentaComoPorVencer()
        {
            //Arrange: obligación que vence dentro de 3 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0003", Hoy.AddDays(3), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: debe haber 1 por vencer
            Assert.AreEqual(1, Resumen.PorVencer);
        }

        [TestMethod]
        public void Test_Vencimientos_VenceEnCuatroDias_NoSeAvisa()
        {
            //Arrange: obligación que vence dentro de 4 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0004", Hoy.AddDays(4), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: no debe haber avisos
            Assert.AreEqual(false, Resumen.HayAvisos);
        }

        [TestMethod]
        public void Test_Vencimientos_SinFechaDeVencimiento_NoSeAvisa()
        {
            //Arrange: obligación sin fecha de vencimiento
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            Tabla.Rows.Add("Compras y CxP", "OC-0005", "Proveedor", DBNull.Value, "GTQ", 500m);
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: no debe haber avisos
            Assert.AreEqual(false, Resumen.HayAvisos);
        }

        [TestMethod]
        public void Test_Vencimientos_SinObligaciones_NoHayAvisos()
        {
            //Arrange: tabla de obligaciones vacía
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: no debe haber avisos
            Assert.AreEqual(false, Resumen.HayAvisos);
        }

        [TestMethod]
        public void Test_Vencimientos_ObligacionesMezcladas_CuentaCadaGrupo()
        {
            //Arrange: obligaciones que vencen en -6, -2, 0, 2 y 10 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0010", Hoy.AddDays(10), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0011", Hoy.AddDays(-6), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0012", Hoy.AddDays(2), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0013", Hoy.AddDays(-2), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0014", Hoy.AddDays(0), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: 2 vencidas, 1 hoy, 1 por vencer y 4 avisos
            Assert.AreEqual(2, Resumen.Vencidas);
            Assert.AreEqual(1, Resumen.VencenHoy);
            Assert.AreEqual(1, Resumen.PorVencer);
            Assert.AreEqual(4, Resumen.Avisos.Count);
        }

        [TestMethod]
        public void Test_Vencimientos_Detalle_VaDeLaMasAtrasadaALaMasProxima()
        {
            //Arrange: obligaciones que vencen en 2, -6 y 0 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0020", Hoy.AddDays(2), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0021", Hoy.AddDays(-6), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0022", Hoy.AddDays(0), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: deben salir en orden: -6, 0 y 2
            Assert.AreEqual("OC-0021", Resumen.Avisos[0].Documento);
            Assert.AreEqual("OC-0022", Resumen.Avisos[1].Documento);
            Assert.AreEqual("OC-0020", Resumen.Avisos[2].Documento);
        }

        [TestMethod]
        public void Test_Vencimientos_ImporteEnDolares_SeFormateaConSimbolo()
        {
            //Arrange: obligación de 1800 dólares que vence hoy
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0030", Hoy.AddDays(0), 1800m, "USD");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: el importe debe ser US$ 1,800.00
            Assert.AreEqual("US$ 1,800.00", Resumen.Avisos[0].Importe);
        }

        [TestMethod]
        public void Test_Vencimientos_SoloPorVencer_NoEsUrgente()
        {
            //Arrange: obligación que vence dentro de 2 días
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0040", Hoy.AddDays(2), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: hay avisos pero no son urgentes
            Assert.AreEqual(true, Resumen.HayAvisos);
            Assert.AreEqual(false, Resumen.HayUrgentes);
        }

        [TestMethod]
        public void Test_Vencimientos_VencidaOVenceHoy_EsUrgente()
        {
            //Arrange: obligación vencida y otra que vence hoy
            var Controlador = new ClsControladorGeneracionPagos();
            DateTime Hoy = new DateTime(2026, 11, 5);
            DataTable Tabla = CrearObligaciones();
            AgregarObligacion(Tabla, "OC-0050", Hoy.AddDays(-1), 1000m, "GTQ");
            AgregarObligacion(Tabla, "OC-0051", Hoy.AddDays(0), 1000m, "GTQ");
            //Act: resumir los vencimientos
            ClsResumenVencimientos Resumen = Controlador.BancosFuncResumirVencimientos(Tabla, Hoy);
            //Assert: debe ser urgente
            Assert.AreEqual(true, Resumen.HayUrgentes);
        }

        [TestMethod]
        public void Test_Vencimientos_DiasDeAviso_SonTres()
        {
            //Arrange: días de aviso configurados
            //Act: leer los días de aviso
            int Dias = ClsControladorGeneracionPagos.DiasAvisoVencimiento;
            //Assert: deben ser 3
            Assert.AreEqual(3, Dias);
        }
        private DataTable CrearObligaciones()
        {
            var Tabla = new DataTable();
            Tabla.Columns.Add("Origen", typeof(string));
            Tabla.Columns.Add("Documento", typeof(string));
            Tabla.Columns.Add("Beneficiario", typeof(string));
            Tabla.Columns.Add("Vencimiento", typeof(DateTime));
            Tabla.Columns.Add("Moneda", typeof(string));
            Tabla.Columns.Add("Pendiente", typeof(decimal));
            return Tabla;
        }

        private void AgregarObligacion(DataTable Tabla, string Documento, DateTime Vencimiento, decimal Pendiente, string Moneda)
        {
            Tabla.Rows.Add("Compras y CxP", Documento, "Proveedor", Vencimiento, Moneda, Pendiente);
        }
    }
}