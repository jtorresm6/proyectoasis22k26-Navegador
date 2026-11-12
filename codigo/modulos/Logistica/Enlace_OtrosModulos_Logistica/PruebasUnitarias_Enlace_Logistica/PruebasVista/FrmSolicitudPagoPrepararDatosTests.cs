
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;
using CapaModelo_Enlace_Logistica.Entidades;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoPrepararDatosTests
    {
        // ==========================================
        // EJECUTAR PRUEBAS WINFORMS EN HILO STA
        // ==========================================

        private void EjecutarEnSTA(Action accion)
        {
            Exception error = null;

            Thread hilo = new Thread(() =>
            {
                try
                {
                    accion();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            });

            hilo.SetApartmentState(ApartmentState.STA);
            hilo.Start();
            hilo.Join();

            if (error != null)
            {
                ExceptionDispatchInfo.Capture(error).Throw();
            }
        }

        // ==========================================
        // ASIGNAR CONTROLES PRIVADOS DEL FORMULARIO
        // ==========================================

        private void AsignarControl(
            Frm_Solicitud_Pago formulario,
            string nombre,
            object control)
        {
            FieldInfo campo =
                typeof(Frm_Solicitud_Pago).GetField(
                    nombre,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);

            Assert.IsNotNull(
                campo,
                "No se encontró el control: " + nombre);

            campo.SetValue(formulario, control);
        }

        // ==========================================
        // CREAR FORMULARIO SIMULADO SIN CONSULTAR MYSQL
        // ==========================================

        private Frm_Solicitud_Pago CrearFormularioSimulado()
        {
            // No ejecuta el constructor normal del formulario.
            // Solo se utilizará con el método que lee controles.
            Frm_Solicitud_Pago formulario =
                (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));

            AsignarControl(
                formulario,
                "LogisticaTxtNumeroGuia",
                new TextBox { Text = " GUIA-D26-0007 " });

            AsignarControl(
                formulario,
                "LogisticaTxtFechaServicio",
                new TextBox { Text = "17/10/2026" });

            AsignarControl(
                formulario,
                "LogisticaTxtNombreTransportista",
                new TextBox { Text = "Transportista DEMO26 07" });

            AsignarControl(
                formulario,
                "LogisticaTxtNitTransportista",
                new TextBox { Text = "1234567-8" });

            AsignarControl(
                formulario,
                "LogisticaTxtDireccionTransportista",
                new TextBox { Text = "Ciudad de Guatemala" });

            ComboBox banco = new ComboBox();
            banco.Items.Add("Banco de prueba");
            banco.SelectedIndex = 0;

            AsignarControl(
                formulario,
                "LogisticaCboBancoBeneficiario",
                banco);

            ComboBox tipoCuenta = new ComboBox();
            tipoCuenta.Items.Add("Monetaria");
            tipoCuenta.SelectedIndex = 0;

            AsignarControl(
                formulario,
                "LogisticaCboTipoCuenta",
                tipoCuenta);

            AsignarControl(
                formulario,
                "LogisticaTxtNumeroCuenta",
                new TextBox { Text = " 1234567890 " });

            AsignarControl(
                formulario,
                "LogisticaRdoDolares",
                new RadioButton { Checked = false });

            AsignarControl(
                formulario,
                "LogisticaTxtConceptoPago",
                new TextBox
                {
                    Text = " Pago por servicio de transporte "
                });

            DataGridView detalle = new DataGridView();
            detalle.AllowUserToAddRows = false;

            detalle.Columns.Add("Numero", "No.");
            detalle.Columns.Add("Descripcion", "Descripción");
            detalle.Columns.Add("Cantidad", "Cantidad");
            detalle.Columns.Add("Costo", "Costo unitario");
            detalle.Columns.Add("Monto", "Monto");

            detalle.Rows.Add(
                "1",
                "Transporte externo",
                "2",
                "250",
                "500");

            AsignarControl(
                formulario,
                "LogisticaDgvDetalleServicio",
                detalle);

            return formulario;
        }

        // ==========================================
        // CREAR SERVICIO QUEMADO
        // ==========================================

        private ClsServicioTransporte CrearServicioValido()
        {
            return new ClsServicioTransporte
            {
                IdServicio = 910007,
                NumeroGuia = "GUIA-D26-0007",
                FechaServicio = new DateTime(2026, 10, 17),
                NombreTransportista = "Transportista DEMO26 07",
                NitTransportista = "1234567-8",
                DireccionTransportista = "Ciudad de Guatemala",
                TipoServicio = "Transporte externo",
                Cantidad = 1m,
                CostoUnitario = 405m,
                MontoTotal = 405m,
                Moneda = "GTQ"
            };
        }

        // ==========================================
        // EJECUTAR METODO PRIVADO
        // ==========================================

        private ClsSolicitudPagoVistaPrevia PrepararDatos(
            Frm_Solicitud_Pago formulario,
            ClsServicioTransporte servicio,
            decimal monto)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "PrepararDatosVistaPrevia",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró PrepararDatosVistaPrevia.");

            try
            {
                return (ClsSolicitudPagoVistaPrevia)
                    metodo.Invoke(
                        formulario,
                        new object[] { servicio, monto });
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(
                        ex.InnerException).Throw();
                }

                throw;
            }
        }

        // ==========================================
        // PRUEBA 1: DATOS GENERALES
        // ==========================================

        [TestMethod]
        public void PrepararDatos_ServicioValido_ConservaDatos()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual(910007, resultado.IdServicio);
                Assert.AreEqual(
                    "GUIA-D26-0007",
                    resultado.NumeroGuia);

                Assert.AreEqual(
                    "17/10/2026",
                    resultado.FechaServicio);

                Assert.AreEqual(
                    "Transportista DEMO26 07",
                    resultado.NombreTransportista);

                Assert.AreEqual(
                    "1234567-8",
                    resultado.NitTransportista);

                Assert.AreEqual(
                    "Ciudad de Guatemala",
                    resultado.DireccionTransportista);
            });
        }

        // ==========================================
        // PRUEBA 2: DATOS BANCARIOS
        // ==========================================

        [TestMethod]
        public void PrepararDatos_BancoSeleccionado_ConservaCuenta()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual(
                    "Banco de prueba",
                    resultado.BancoBeneficiario);

                Assert.AreEqual(
                    "Monetaria",
                    resultado.TipoCuenta);

                Assert.AreEqual(
                    "1234567890",
                    resultado.NumeroCuenta);
            });
        }

        // ==========================================
        // PRUEBA 3: BANCO PENDIENTE
        // ==========================================

        [TestMethod]
        public void PrepararDatos_SinBanco_DevuelvePendiente()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox banco = new ComboBox();
                ComboBox tipoCuenta = new ComboBox();

                AsignarControl(
                    formulario,
                    "LogisticaCboBancoBeneficiario",
                    banco);

                AsignarControl(
                    formulario,
                    "LogisticaCboTipoCuenta",
                    tipoCuenta);

                AsignarControl(
                    formulario,
                    "LogisticaTxtNumeroCuenta",
                    new TextBox { Text = "" });

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual(
                    "Pendiente",
                    resultado.BancoBeneficiario);

                Assert.AreEqual(
                    "Pendiente",
                    resultado.TipoCuenta);

                Assert.AreEqual(
                    "Pendiente",
                    resultado.NumeroCuenta);
            });
        }

        // ==========================================
        // PRUEBA 4: MONEDA GTQ
        // ==========================================

        [TestMethod]
        public void PrepararDatos_Quetzales_AsignaGTQ()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual("GTQ", resultado.Moneda);
                Assert.AreEqual(500m, resultado.MontoTotal);
            });
        }

        // ==========================================
        // PRUEBA 5: MONEDA USD
        // ==========================================

        [TestMethod]
        public void PrepararDatos_Dolares_AsignaUSD()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                AsignarControl(
                    formulario,
                    "LogisticaRdoDolares",
                    new RadioButton { Checked = true });

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual("USD", resultado.Moneda);
            });
        }

        // ==========================================
        // PRUEBA 6: DETALLE DEL DATAGRIDVIEW
        // ==========================================

        [TestMethod]
        public void PrepararDatos_DetalleExistente_UsaDatosTabla()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual(
                    "Transporte externo",
                    resultado.DescripcionServicio);

                Assert.AreEqual(2m, resultado.Cantidad);
                Assert.AreEqual(250m, resultado.CostoUnitario);
                Assert.AreEqual(500m, resultado.MontoTotal);
            });
        }

        // ==========================================
        // PRUEBA 7: DETALLE SIN FILAS
        // ==========================================

        [TestMethod]
        public void PrepararDatos_SinFilas_UsaDatosServicio()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                DataGridView detalle = new DataGridView();
                detalle.AllowUserToAddRows = false;

                for (int i = 0; i < 5; i++)
                {
                    detalle.Columns.Add(
                        "Columna" + i,
                        "Columna " + i);
                }

                AsignarControl(
                    formulario,
                    "LogisticaDgvDetalleServicio",
                    detalle);

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 405m);

                // ASSERT
                Assert.AreEqual(
                    "Transporte externo",
                    resultado.DescripcionServicio);

                Assert.AreEqual(1m, resultado.Cantidad);
                Assert.AreEqual(405m, resultado.CostoUnitario);
                Assert.AreEqual(405m, resultado.MontoTotal);
            });
        }

        // ==========================================
        // PRUEBA 8: CONCEPTO DE PAGO
        // ==========================================

        [TestMethod]
        public void PrepararDatos_ConceptoConEspacios_EliminaEspacios()
        {
            EjecutarEnSTA(() =>
            {
                // ARRANGE
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                // ACT
                ClsSolicitudPagoVistaPrevia resultado =
                    PrepararDatos(formulario, servicio, 500m);

                // ASSERT
                Assert.AreEqual(
                    "Pago por servicio de transporte",
                    resultado.ConceptoPago);
            });
        }
    }
}
