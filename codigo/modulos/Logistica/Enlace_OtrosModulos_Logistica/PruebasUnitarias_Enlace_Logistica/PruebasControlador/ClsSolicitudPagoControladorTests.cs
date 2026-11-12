
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CapaContralador_Enlace_Logistica;
using CapaModelo_Enlace_Logistica.Contratos;
using CapaModelo_Enlace_Logistica.Entidades;

namespace PruebasUnitarias_Enlace_Logistica.PruebasControlador
{
    [TestClass]
    public class ClsSolicitudPagoControladorTests
    {
        // ==========================================
        // REPOSITORIO SIMULADO CON DATOS QUEMADOS
        // ==========================================

        private class RepositorioSimulado :
            ClsIRepositorioSolicitudPago
        {
            public IEnumerable<ClsServicioTransporte> Servicios
            { get; set; }

            public bool LanzarError { get; set; }

            public int NumeroConsultas { get; private set; }

            public IEnumerable<ClsServicioTransporte>
                ObtenerServiciosDisponibles()
            {
                NumeroConsultas++;

                if (LanzarError)
                {
                    throw new InvalidOperationException(
                        "Error simulado del repositorio.");
                }

                return Servicios;
            }
        }

        // ==========================================
        // DATOS DE PRUEBA
        // ==========================================

        private ClsServicioTransporte CrearServicioValido()
        {
            return new ClsServicioTransporte
            {
                IdServicio = 910007,
                NumeroGuia = "GUIA-D26-0007",
                NombreTransportista = "Transportista DEMO26 07",
                TipoServicio = "Transporte externo",
                Cantidad = 1m,
                CostoUnitario = 405m,
                MontoTotal = 405m,
                Moneda = "GTQ",
                TipoPago = "Transferencia",
                FechaServicio = new DateTime(2026, 10, 17)
            };
        }

        private ClsSolicitudPagoControlador
            CrearControladorSimulado()
        {
            RepositorioSimulado repositorio =
                new RepositorioSimulado
                {
                    Servicios = new List<ClsServicioTransporte>()
                };

            return new ClsSolicitudPagoControlador(repositorio);
        }

        // ==========================================
        // PRUEBAS DEL CONSTRUCTOR
        // ==========================================

        [TestMethod]
        public void Constructor_RepositorioValido_CreaControlador()
        {
            // ARRANGE
            RepositorioSimulado repositorio =
                new RepositorioSimulado
                {
                    Servicios = new List<ClsServicioTransporte>()
                };

            // ACT
            ClsSolicitudPagoControlador controlador =
                new ClsSolicitudPagoControlador(repositorio);

            // ASSERT
            Assert.IsNotNull(controlador);
        }

        [TestMethod]
        public void Constructor_RepositorioNulo_LanzaExcepcion()
        {
            // ARRANGE
            ClsIRepositorioSolicitudPago repositorio = null;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                new ClsSolicitudPagoControlador(repositorio);
            }
            catch (ArgumentNullException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        // ==========================================
        // PRUEBAS DE OBTENER SERVICIOS
        // ==========================================

        [TestMethod]
        public void ObtenerServicios_ConDatos_DevuelveDosServicios()
        {
            // ARRANGE
            ClsServicioTransporte servicio1 =
                CrearServicioValido();

            ClsServicioTransporte servicio2 =
                CrearServicioValido();

            servicio2.IdServicio = 910008;
            servicio2.NumeroGuia = "GUIA-D26-0008";
            servicio2.MontoTotal = 650m;

            RepositorioSimulado repositorio =
                new RepositorioSimulado
                {
                    Servicios = new List<ClsServicioTransporte>
                    {
                        servicio1,
                        servicio2
                    }
                };

            ClsSolicitudPagoControlador controlador =
                new ClsSolicitudPagoControlador(repositorio);

            // ACT
            List<ClsServicioTransporte> resultado =
                controlador.ObtenerServiciosDisponibles();

            // ASSERT
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual(910007, resultado[0].IdServicio);
            Assert.AreEqual(910008, resultado[1].IdServicio);
            Assert.AreEqual(1, repositorio.NumeroConsultas);
        }

        [TestMethod]
        public void ObtenerServicios_SinDatos_DevuelveListaVacia()
        {
            // ARRANGE
            RepositorioSimulado repositorio =
                new RepositorioSimulado
                {
                    Servicios = new List<ClsServicioTransporte>()
                };

            ClsSolicitudPagoControlador controlador =
                new ClsSolicitudPagoControlador(repositorio);

            // ACT
            List<ClsServicioTransporte> resultado =
                controlador.ObtenerServiciosDisponibles();

            // ASSERT
            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            Assert.AreEqual(1, repositorio.NumeroConsultas);
        }

        [TestMethod]
        public void ObtenerServicios_ErrorRepositorio_PropagaExcepcion()
        {
            // ARRANGE
            RepositorioSimulado repositorio =
                new RepositorioSimulado
                {
                    LanzarError = true
                };

            ClsSolicitudPagoControlador controlador =
                new ClsSolicitudPagoControlador(repositorio);

            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ObtenerServiciosDisponibles();
            }
            catch (InvalidOperationException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
            Assert.AreEqual(1, repositorio.NumeroConsultas);
        }

        // ==========================================
        // PRUEBAS DE VALIDAR SERVICIO
        // ==========================================

        [TestMethod]
        public void ValidarServicio_DatosValidosGTQ_NoLanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            // ACT
            controlador.ValidarServicio(servicio);

            // ASSERT
            Assert.IsTrue(servicio.IdServicio > 0);
            Assert.IsTrue(servicio.Cantidad > 0);
            Assert.IsTrue(servicio.MontoTotal > 0);
            Assert.AreEqual("GTQ", servicio.Moneda);
        }

        [TestMethod]
        public void ValidarServicio_DatosValidosUSD_NoLanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Moneda = "USD";

            // ACT
            controlador.ValidarServicio(servicio);

            // ASSERT
            Assert.AreEqual("USD", servicio.Moneda);
        }

        [TestMethod]
        public void ValidarServicio_ServicioNulo_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio = null;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_IdCero_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.IdServicio = 0;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_IdNegativo_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.IdServicio = -1;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_CantidadCero_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Cantidad = 0m;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_CantidadNegativa_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Cantidad = -5m;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_MontoCero_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.MontoTotal = 0m;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_MontoNegativo_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.MontoTotal = -405m;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_MonedaInvalida_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Moneda = "EUR";
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_MonedaVacia_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Moneda = "";
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }

        [TestMethod]
        public void ValidarServicio_MonedaNula_LanzaExcepcion()
        {
            // ARRANGE
            ClsSolicitudPagoControlador controlador =
                CrearControladorSimulado();

            ClsServicioTransporte servicio =
                CrearServicioValido();

            servicio.Moneda = null;
            bool excepcionDetectada = false;

            // ACT
            try
            {
                controlador.ValidarServicio(servicio);
            }
            catch (ArgumentException)
            {
                excepcionDetectada = true;
            }

            // ASSERT
            Assert.IsTrue(excepcionDetectada);
        }
    }
}
