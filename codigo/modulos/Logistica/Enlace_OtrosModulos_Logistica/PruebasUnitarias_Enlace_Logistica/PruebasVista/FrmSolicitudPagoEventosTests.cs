
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoEventosTests
    {
        // Ejecutar pruebas WinForms en un hilo STA.
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

        // Asignar un control privado del formulario.
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
                "No se encontró el campo: " + nombre);

            campo.SetValue(formulario, control);
        }

        // Obtener un control privado del formulario.
        private T ObtenerControl<T>(
            Frm_Solicitud_Pago formulario,
            string nombre) where T : Control
        {
            FieldInfo campo =
                typeof(Frm_Solicitud_Pago).GetField(
                    nombre,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);

            Assert.IsNotNull(campo);

            T control = campo.GetValue(formulario) as T;

            Assert.IsNotNull(
                control,
                "Control no encontrado: " + nombre);

            return control;
        }

        // Invocar métodos privados reales.
        private void EjecutarMetodo(
            Frm_Solicitud_Pago formulario,
            string nombreMetodo,
            params object[] parametros)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    nombreMetodo,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);

            Assert.IsNotNull(
                metodo,
                "No se encontró el método: " + nombreMetodo);

            try
            {
                metodo.Invoke(formulario, parametros);
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

        // Crear formulario sin constructor ni base de datos.
        private Frm_Solicitud_Pago CrearFormularioSimulado()
        {
            Frm_Solicitud_Pago formulario =
                (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));

            string[] cajas =
            {
                "LogisticaTxtNumeroServicio",
                "LogisticaTxtNumeroGuia",
                "LogisticaTxtFechaServicio",
                "LogisticaTxtNombreTransportista",
                "LogisticaTxtNitTransportista",
                "LogisticaTxtDireccionTransportista",
                "LogisticaTxtNumeroCuenta",
                "LogisticaTxtMontoPagar",
                "LogisticaTxtConceptoPago"
            };

            foreach (string nombre in cajas)
            {
                AsignarControl(
                    formulario,
                    nombre,
                    new TextBox());
            }

            AsignarControl(
                formulario,
                "LogisticaCboServicioTransporte",
                new ComboBox());

            AsignarControl(
                formulario,
                "LogisticaCboBancoBeneficiario",
                new ComboBox());

            AsignarControl(
                formulario,
                "LogisticaCboTipoCuenta",
                new ComboBox());

            AsignarControl(
                formulario,
                "LogisticaRdoQuetzales",
                new RadioButton());

            AsignarControl(
                formulario,
                "LogisticaRdoDolares",
                new RadioButton());

            AsignarControl(
                formulario,
                "LogisticaLblMonedaMonto",
                new Label { Text = "GTQ" });

            string[] etiquetas =
            {
                "LogisticaLblTextoResumenNumeroServicio",
                "LogisticaLblTextoResumenFechaServicio",
                "LogisticaLblTextoResumenTransportista",
                "LogisticaLblTextoResumenNit",
                "LogisticaLblTextoResumenBanco",
                "LogisticaLblTextoResumenTipoCuenta",
                "LogisticaLblTextoResumenNumeroCuenta",
                "LogisticaLblTextoResumenMoneda",
                "LogisticaLblTextoResumenMonto",
                "LogisticaLblTextoResumenConcepto"
            };

            foreach (string nombre in etiquetas)
            {
                AsignarControl(
                    formulario,
                    nombre,
                    new Label());
            }

            return formulario;
        }

        // ==========================================
        // PRUEBA 1: ACTUALIZAR MONEDA A GTQ
        // ==========================================

        [TestMethod]
        public void ActualizarMoneda_Quetzales_MuestraGTQ()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ObtenerControl<RadioButton>(
                    formulario,
                    "LogisticaRdoQuetzales").Checked = true;

                ObtenerControl<RadioButton>(
                    formulario,
                    "LogisticaRdoDolares").Checked = false;

                EjecutarMetodo(
                    formulario,
                    "ActualizarMoneda");

                Assert.AreEqual(
                    "GTQ",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);
            });
        }

        // ==========================================
        // PRUEBA 2: ACTUALIZAR MONEDA A USD
        // ==========================================

        [TestMethod]
        public void ActualizarMoneda_Dolares_MuestraUSD()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ObtenerControl<RadioButton>(
                    formulario,
                    "LogisticaRdoDolares").Checked = true;

                EjecutarMetodo(
                    formulario,
                    "ActualizarMoneda");

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);
            });
        }

        // ==========================================
        // PRUEBA 3: EVENTO CAMBIO DE MONEDA
        // ==========================================

        [TestMethod]
        public void Moneda_CheckedChanged_ActualizaMonedaYResumen()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ObtenerControl<RadioButton>(
                    formulario,
                    "LogisticaRdoDolares").Checked = true;

                EjecutarMetodo(
                    formulario,
                    "Moneda_CheckedChanged",
                    null,
                    EventArgs.Empty);

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblTextoResumenMoneda").Text);
            });
        }

        // ==========================================
        // PRUEBA 4: EVENTO ACTUALIZAR RESUMEN
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_Evento_ActualizaMonto()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtMontoPagar").Text = "750.50";

                EjecutarMetodo(
                    formulario,
                    "ActualizarResumen_Evento",
                    null,
                    EventArgs.Empty);

                Assert.AreEqual(
                    "750.50",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblTextoResumenMonto").Text);
            });
        }

        // ==========================================
        // PRUEBA 5: CONFIGURAR EVENTOS
        // ==========================================

        [TestMethod]
        public void ConfigurarEventos_CambioMonto_ActualizaResumen()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "ConfigurarEventos");

                TextBox monto =
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtMontoPagar");

                monto.Text = "980.25";

                Assert.AreEqual(
                    "980.25",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblTextoResumenMonto").Text);
            });
        }

        // ==========================================
        // PRUEBA 6: EVENTOS DE MONEDA CONFIGURADOS
        // ==========================================

        [TestMethod]
        public void ConfigurarEventos_CambioDolares_ActualizaMoneda()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "ConfigurarEventos");

                RadioButton dolares =
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoDolares");

                dolares.Checked = true;

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblTextoResumenMoneda").Text);
            });
        }
    }
}

