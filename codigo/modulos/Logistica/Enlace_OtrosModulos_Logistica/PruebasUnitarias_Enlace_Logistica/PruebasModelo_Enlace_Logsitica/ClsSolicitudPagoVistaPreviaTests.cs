
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaModelo_Enlace_Logistica.Entidades;

namespace PruebasUnitarias_Enlace_Logistica.PruebasModelo
{
    [TestClass]
    public class ClsSolicitudPagoVistaPreviaTests
    {
        // ==========================================
        // DATOS QUEMADOS PARA LAS PRUEBAS
        // ==========================================

        private ClsSolicitudPagoVistaPrevia CrearSolicitudValida()
        {
            return new ClsSolicitudPagoVistaPrevia
            {
                IdServicio = 910007,
                NumeroGuia = "GUIA-D26-0007",
                FechaServicio = "17/10/2026",
                NombreTransportista = "Transportista DEMO26 07",
                NitTransportista = "1234567-8",
                DireccionTransportista = "Ciudad de Guatemala",
                BancoBeneficiario = "Banco de prueba",
                TipoCuenta = "Monetaria",
                NumeroCuenta = "1234567890",
                Moneda = "GTQ",
                MontoTotal = 405.00m,
                ConceptoPago = "Pago por servicio de transporte",
                DescripcionServicio = "Servicio DEMO26 07",
                Cantidad = 1m,
                CostoUnitario = 405.00m
            };
        }

        // ==========================================
        // PRUEBA 1: CREAR LA ENTIDAD
        // ==========================================

        [TestMethod]
        public void CrearSolicitud_InstanciaCorrectamente()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud = null;

            // ACT
            solicitud = new ClsSolicitudPagoVistaPrevia();

            // ASSERT
            Assert.IsNotNull(solicitud);
        }

        // ==========================================
        // PRUEBA 2: DATOS DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void AsignarDatosServicio_ConservaValores()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            int id = solicitud.IdServicio;
            string guia = solicitud.NumeroGuia;
            string fecha = solicitud.FechaServicio;

            // ASSERT
            Assert.AreEqual(910007, id);
            Assert.AreEqual("GUIA-D26-0007", guia);
            Assert.AreEqual("17/10/2026", fecha);
        }

        // ==========================================
        // PRUEBA 3: DATOS DEL TRANSPORTISTA
        // ==========================================

        [TestMethod]
        public void AsignarDatosTransportista_ConservaValores()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            string nombre = solicitud.NombreTransportista;
            string nit = solicitud.NitTransportista;
            string direccion = solicitud.DireccionTransportista;

            // ASSERT
            Assert.AreEqual(
                "Transportista DEMO26 07", nombre);

            Assert.AreEqual("1234567-8", nit);

            Assert.AreEqual(
                "Ciudad de Guatemala", direccion);
        }

        // ==========================================
        // PRUEBA 4: INFORMACION BANCARIA
        // ==========================================

        [TestMethod]
        public void AsignarDatosBancarios_ConservaValores()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            string banco = solicitud.BancoBeneficiario;
            string tipoCuenta = solicitud.TipoCuenta;
            string numeroCuenta = solicitud.NumeroCuenta;

            // ASSERT
            Assert.AreEqual("Banco de prueba", banco);
            Assert.AreEqual("Monetaria", tipoCuenta);
            Assert.AreEqual("1234567890", numeroCuenta);
        }

        // ==========================================
        // PRUEBA 5: MONEDA GTQ
        // ==========================================

        [TestMethod]
        public void AsignarMonedaGTQ_ConservaValor()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.Moneda = "GTQ";

            // ASSERT
            Assert.AreEqual("GTQ", solicitud.Moneda);
        }

        // ==========================================
        // PRUEBA 6: MONEDA USD
        // ==========================================

        [TestMethod]
        public void AsignarMonedaUSD_ConservaValor()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.Moneda = "USD";

            // ASSERT
            Assert.AreEqual("USD", solicitud.Moneda);
        }

        // ==========================================
        // PRUEBA 7: MONTO TOTAL
        // ==========================================

        [TestMethod]
        public void AsignarMontoTotal_ConservaPrecisionDecimal()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            decimal montoEsperado = 405.75m;

            // ACT
            solicitud.MontoTotal = montoEsperado;

            // ASSERT
            Assert.AreEqual(
                montoEsperado,
                solicitud.MontoTotal);
        }

        // ==========================================
        // PRUEBA 8: DETALLE DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void AsignarDetalleServicio_ConservaValores()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.DescripcionServicio =
                "Transporte externo";

            solicitud.Cantidad = 2m;
            solicitud.CostoUnitario = 250.50m;

            // ASSERT
            Assert.AreEqual(
                "Transporte externo",
                solicitud.DescripcionServicio);

            Assert.AreEqual(2m, solicitud.Cantidad);

            Assert.AreEqual(
                250.50m,
                solicitud.CostoUnitario);
        }

        // ==========================================
        // PRUEBA 9: CONCEPTO DE PAGO
        // ==========================================

        [TestMethod]
        public void AsignarConceptoPago_ConservaTexto()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            string conceptoEsperado =
                "Pago por transporte de mercadería";

            // ACT
            solicitud.ConceptoPago = conceptoEsperado;

            // ASSERT
            Assert.AreEqual(
                conceptoEsperado,
                solicitud.ConceptoPago);
        }

        // ==========================================
        // PRUEBA 10: DATOS BANCARIOS PENDIENTES
        // ==========================================

        [TestMethod]
        public void AsignarDatosBancariosPendientes_ConservaTexto()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.BancoBeneficiario = "Pendiente";
            solicitud.TipoCuenta = "Pendiente";
            solicitud.NumeroCuenta = "Pendiente";

            // ASSERT
            Assert.AreEqual(
                "Pendiente",
                solicitud.BancoBeneficiario);

            Assert.AreEqual(
                "Pendiente",
                solicitud.TipoCuenta);

            Assert.AreEqual(
                "Pendiente",
                solicitud.NumeroCuenta);
        }

        // ==========================================
        // PRUEBA 11: MONTO CERO
        // ==========================================

        [TestMethod]
        public void AsignarMontoCero_ConservaValor()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.MontoTotal = 0m;

            // ASSERT
            Assert.AreEqual(0m, solicitud.MontoTotal);
        }

        // ==========================================
        // PRUEBA 12: TEXTO VACIO
        // ==========================================

        [TestMethod]
        public void AsignarConceptoVacio_ConservaTextoVacio()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            // ACT
            solicitud.ConceptoPago = string.Empty;

            // ASSERT
            Assert.AreEqual(
                string.Empty,
                solicitud.ConceptoPago);
        }

        // ==========================================
        // PRUEBA 13: VALORES DECIMALES
        // ==========================================

        [TestMethod]
        public void AsignarCantidadYCosto_ConservaDecimales()
        {
            // ARRANGE
            ClsSolicitudPagoVistaPrevia solicitud =
                CrearSolicitudValida();

            decimal cantidadEsperada = 1.5m;
            decimal costoEsperado = 125.75m;

            // ACT
            solicitud.Cantidad = cantidadEsperada;
            solicitud.CostoUnitario = costoEsperado;

            // ASSERT
            Assert.AreEqual(
                cantidadEsperada,
                solicitud.Cantidad);

            Assert.AreEqual(
                costoEsperado,
                solicitud.CostoUnitario);
        }
    }
}

