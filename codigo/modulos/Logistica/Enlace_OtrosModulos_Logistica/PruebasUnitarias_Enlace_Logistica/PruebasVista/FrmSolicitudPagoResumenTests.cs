
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
    public class FrmSolicitudPagoResumenTests
    {
        // ==========================================
        // EJECUTAR LAS PRUEBAS EN UN HILO STA
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
        // ASIGNAR CONTROLES PRIVADOS
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
        // CREAR FORMULARIO CON DATOS QUEMADOS
        // ==========================================

        private Frm_Solicitud_Pago CrearFormularioSimulado()
        {
            // No ejecuta el constructor real.
            // Evita consultas a la base de datos.
            Frm_Solicitud_Pago formulario =
                (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));

            AsignarControl(
                formulario,
                "LogisticaTxtNumeroServicio",
                new TextBox { Text = "910007" });

            AsignarControl(
                formulario,
                "LogisticaTxtFechaServicio",
                new TextBox { Text = "17/10/2026" });

            AsignarControl(
                formulario,
                "LogisticaTxtNombreTransportista",
                new TextBox
                {
                    Text = "Transportista DEMO26 07"
                });

            AsignarControl(
                formulario,
                "LogisticaTxtNitTransportista",
                new TextBox { Text = "1234567-8" });

            ComboBox banco = new ComboBox();
            banco.Items.Add("Banco Industrial");
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
                new TextBox { Text = "1234567890" });

            AsignarControl(
                formulario,
                "LogisticaLblMonedaMonto",
                new Label { Text = "GTQ" });

            AsignarControl(
                formulario,
                "LogisticaTxtMontoPagar",
                new TextBox { Text = "405.00" });

            AsignarControl(
                formulario,
                "LogisticaTxtConceptoPago",
                new TextBox
                {
                    Text = "Pago por servicio de transporte"
                });

            // Etiquetas del resumen

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenNumeroServicio",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenFechaServicio",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenTransportista",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenNit",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenBanco",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenTipoCuenta",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenNumeroCuenta",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenMoneda",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenMonto",
                new Label());

            AsignarControl(
                formulario,
                "LogisticaLblTextoResumenConcepto",
                new Label());

            return formulario;
        }

        // ==========================================
        // OBTENER CONTROLES PARA LAS COMPROBACIONES
        // ==========================================

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

            Assert.IsNotNull(
                campo,
                "No se encontró el control: " + nombre);

            T control = campo.GetValue(formulario) as T;

            Assert.IsNotNull(
                control,
                "El control no tiene el tipo esperado: " + nombre);

            return control;
        }

        // ==========================================
        // EJECUTAR EL MÉTODO REAL
        // ==========================================

        private void ActualizarResumen(
            Frm_Solicitud_Pago formulario)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "ActualizarResumenSolicitud",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);

            Assert.IsNotNull(
                metodo,
                "No se encontró ActualizarResumenSolicitud.");

            try
            {
                metodo.Invoke(formulario, null);
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
        // PRUEBA 1: NÚMERO DE SERVICIO
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_NumeroServicio_MuestraNumero()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenNumeroServicio");

                Assert.AreEqual("910007", resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 2: FECHA DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_FechaServicio_MuestraFecha()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenFechaServicio");

                Assert.AreEqual(
                    "17/10/2026",
                    resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 3: TRANSPORTISTA
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_Transportista_MuestraNombre()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenTransportista");

                Assert.AreEqual(
                    "Transportista DEMO26 07",
                    resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 4: NIT
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_Nit_MuestraNit()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenNit");

                Assert.AreEqual(
                    "1234567-8",
                    resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 5: BANCO BENEFICIARIO
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_BancoSeleccionado_MuestraBanco()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenBanco");

                Assert.AreEqual(
                    "Banco Industrial",
                    resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 6: TIPO Y NÚMERO DE CUENTA
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_CuentaBancaria_MuestraDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label tipoCuenta = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenTipoCuenta");

                Label numeroCuenta = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenNumeroCuenta");

                Assert.AreEqual(
                    "Monetaria",
                    tipoCuenta.Text);

                Assert.AreEqual(
                    "1234567890",
                    numeroCuenta.Text);
            });
        }

        // ==========================================
        // PRUEBA 7: MONEDA GTQ
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_MonedaGTQ_MuestraGTQ()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenMoneda");

                Assert.AreEqual("GTQ", resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 8: MONEDA USD
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_MonedaUSD_MuestraUSD()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                Label moneda = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblMonedaMonto");

                moneda.Text = "USD";

                ActualizarResumen(formulario);

                Label resultado = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenMoneda");

                Assert.AreEqual("USD", resultado.Text);
            });
        }

        // ==========================================
        // PRUEBA 9: MONTO Y CONCEPTO
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_MontoYConcepto_MuestraDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ActualizarResumen(formulario);

                Label monto = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenMonto");

                Label concepto = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenConcepto");

                Assert.AreEqual("405.00", monto.Text);

                Assert.AreEqual(
                    "Pago por servicio de transporte",
                    concepto.Text);
            });
        }

        // ==========================================
        // PRUEBA 10: CAMPOS VACÍOS
        // ==========================================

        [TestMethod]
        public void ActualizarResumen_CamposVacios_UsaTextoPredeterminado()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNumeroServicio").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtFechaServicio").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNombreTransportista").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNitTransportista").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNumeroCuenta").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtMontoPagar").Text = "";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtConceptoPago").Text = "";

                ObtenerControl<ComboBox>(
                    formulario,
                    "LogisticaCboBancoBeneficiario")
                    .SelectedIndex = -1;

                ObtenerControl<ComboBox>(
                    formulario,
                    "LogisticaCboTipoCuenta")
                    .SelectedIndex = -1;

                ActualizarResumen(formulario);

                // Obtener el valor predeterminado real
                // usando el método existente del formulario.
                MethodInfo metodoTexto =
                    typeof(Frm_Solicitud_Pago).GetMethod(
                        "ObtenerTextoResumen",
                        BindingFlags.Instance |
                        BindingFlags.Static |
                        BindingFlags.NonPublic |
                        BindingFlags.Public);

                Assert.IsNotNull(metodoTexto);

                string textoPredeterminado =
                    (string)metodoTexto.Invoke(
                        metodoTexto.IsStatic ? null : formulario,
                        new object[] { "" });

                string[] etiquetas =
                {
                    "LogisticaLblTextoResumenNumeroServicio",
                    "LogisticaLblTextoResumenFechaServicio",
                    "LogisticaLblTextoResumenTransportista",
                    "LogisticaLblTextoResumenNit",
                    "LogisticaLblTextoResumenBanco",
                    "LogisticaLblTextoResumenTipoCuenta",
                    "LogisticaLblTextoResumenNumeroCuenta",
                    "LogisticaLblTextoResumenMonto",
                    "LogisticaLblTextoResumenConcepto"
                };

                foreach (string nombre in etiquetas)
                {
                    Label resultado = ObtenerControl<Label>(
                        formulario,
                        nombre);

                    Assert.AreEqual(
                        textoPredeterminado,
                        resultado.Text,
                        "Falló la etiqueta: " + nombre);
                }
            });
        }
    }
}

