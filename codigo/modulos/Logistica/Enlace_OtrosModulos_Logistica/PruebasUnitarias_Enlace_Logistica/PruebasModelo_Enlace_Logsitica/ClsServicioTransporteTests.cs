using CapaModelo_Enlace_Logistica.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasUnitarias_Enlace_Logistica.PruebasModelo_Enlace_Logsitica
{
    [TestClass]
    public class ClsServicioTransporteTests
    {
        [TestMethod]
        public void AsignarDatosServicio_ConservaValores()
        {
            // ARRANGE
            int idEsperado = 910007;
            string guiaEsperada = "GUIA-D26-0007";
            string transportistaEsperado = "Transportista DEMO26 07";

            ClsServicioTransporte servicio =
                new ClsServicioTransporte();

            // ACT
            servicio.IdServicio = idEsperado;
            servicio.NumeroGuia = guiaEsperada;
            servicio.NombreTransportista = transportistaEsperado;

            // ASSERT
            Assert.AreEqual(idEsperado, servicio.IdServicio);
            Assert.AreEqual(guiaEsperada, servicio.NumeroGuia);
            Assert.AreEqual(
                transportistaEsperado,
                servicio.NombreTransportista
            );
        }

        [TestMethod]
        public void AsignarMontos_ConservaPrecisionDecimal()
        {
            // ARRANGE
            ClsServicioTransporte servicio =
                new ClsServicioTransporte();

            // ACT
            servicio.Cantidad = 2.5m;
            servicio.CostoUnitario = 162.25m;
            servicio.MontoTotal = 405.625m;

            // ASSERT
            Assert.AreEqual(2.5m, servicio.Cantidad);
            Assert.AreEqual(162.25m, servicio.CostoUnitario);
            Assert.AreEqual(405.625m, servicio.MontoTotal);
        }

        [TestMethod]
        public void AsignarMoneda_ConservaCodigo()
        {
            // ARRANGE
            ClsServicioTransporte servicio =
                new ClsServicioTransporte();

            // ACT
            servicio.Moneda = "USD";

            // ASSERT
            Assert.AreEqual("USD", servicio.Moneda);
        }

        [TestMethod]
        public void AsignarFechaServicio_ConservaFecha()
        {
            // ARRANGE
            DateTime fechaEsperada =
                new DateTime(2026, 10, 17);

            ClsServicioTransporte servicio =
                new ClsServicioTransporte();

            // ACT
            servicio.FechaServicio = fechaEsperada;

            // ASSERT
            Assert.IsTrue(servicio.FechaServicio.HasValue);
            Assert.AreEqual(
                fechaEsperada,
                servicio.FechaServicio.Value
            );
        }

        [TestMethod]
        public void AsignarDatosTransportista_ConservaInformacion()
        {
            // ARRANGE
            ClsServicioTransporte servicio =
                new ClsServicioTransporte();

            string nitEsperado = "1234567-8";
            string direccionEsperada = "Ciudad de Guatemala";

            // ACT
            servicio.NitTransportista = nitEsperado;
            servicio.DireccionTransportista = direccionEsperada;

            // ASSERT
            Assert.AreEqual(
                nitEsperado,
                servicio.NitTransportista
            );

            Assert.AreEqual(
                direccionEsperada,
                servicio.DireccionTransportista
            );
        }
    }
}
