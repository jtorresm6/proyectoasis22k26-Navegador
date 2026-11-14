
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
    public class FrmSolicitudPagoLimpiarTests
    {
        // ==========================================
        // 1. EJECUTAR EN HILO STA
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
        // 2. ASIGNAR CONTROLES PRIVADOS
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
        // 3. OBTENER CONTROLES PRIVADOS
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
        // 4. CREAR FORMULARIO CON DATOS QUEMADOS
        // ==========================================

        private Frm_Solicitud_Pago CrearFormularioSimulado()
        {
            
            Frm_Solicitud_Pago formulario =
                (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));

            ComboBox servicios = new ComboBox();
            servicios.Items.Add("GUIA-D26-0007");
            servicios.Items.Add("GUIA-D26-0008");
            servicios.SelectedIndex = 0;

            AsignarControl(
                formulario,
                "LogisticaCboServicioTransporte",
                servicios);

            AsignarControl(
                formulario,
                "LogisticaTxtNumeroServicio",
                new TextBox { Text = "910007" });

            AsignarControl(
                formulario,
                "LogisticaTxtNumeroGuia",
                new TextBox { Text = "GUIA-D26-0007" });

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

            DataGridView detalle = new DataGridView();
            detalle.AllowUserToAddRows = false;

            detalle.Columns.Add("Numero", "No.");
            detalle.Columns.Add("Descripcion", "Descripción");
            detalle.Columns.Add("Cantidad", "Cantidad");
            detalle.Columns.Add("CostoUnitario", "Costo unitario");
            detalle.Columns.Add("Monto", "Monto");

            detalle.Rows.Add(
                "1",
                "Transporte externo",
                "2.0000",
                "250.00",
                "500.00");

            AsignarControl(
                formulario,
                "LogisticaDgvDetalleServicio",
                detalle);

            AsignarControl(
                formulario,
                "LogisticaTxtMontoPagar",
                new TextBox { Text = "500.00" });

            AsignarControl(
                formulario,
                "LogisticaTxtConceptoPago",
                new TextBox
                {
                    Text = "Pago por servicio de transporte"
                });

            AsignarControl(
                formulario,
                "LogisticaRdoQuetzales",
                new RadioButton { Checked = false });

            AsignarControl(
                formulario,
                "LogisticaRdoDolares",
                new RadioButton { Checked = true });

            AsignarControl(
                formulario,
                "LogisticaLblMonedaMonto",
                new Label { Text = "USD" });

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
                    new Label { Text = "Valor anterior" });
            }

            return formulario;
        }

        // ==========================================
        // 5. EJECUTAR MÉTODOS PRIVADOS
        // ==========================================

        private void EjecutarMetodo(
            Frm_Solicitud_Pago formulario,
            string nombreMetodo)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    nombreMetodo,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró el método: " + nombreMetodo);

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
        // PRUEBA 1: DESELECCIONAR SERVICIO
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_DeseleccionaServicio()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                ComboBox servicios = ObtenerControl<ComboBox>(
                    formulario,
                    "LogisticaCboServicioTransporte");

                Assert.AreEqual(-1, servicios.SelectedIndex);
            });
        }

        // ==========================================
        // PRUEBA 2: CONSERVAR GUÍAS DISPONIBLES
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_ConservaServiciosDisponibles()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                ComboBox servicios = ObtenerControl<ComboBox>(
                    formulario,
                    "LogisticaCboServicioTransporte");

                Assert.AreEqual(2, servicios.Items.Count);

                Assert.AreEqual(
                    "GUIA-D26-0007",
                    servicios.Items[0].ToString());
            });
        }

        // ==========================================
        // PRUEBA 3: LIMPIAR DATOS DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_LimpiaDatosServicio()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                string[] campos =
                {
                    "LogisticaTxtNumeroServicio",
                    "LogisticaTxtNumeroGuia",
                    "LogisticaTxtFechaServicio"
                };

                foreach (string nombre in campos)
                {
                    Assert.AreEqual(
                        "",
                        ObtenerControl<TextBox>(
                            formulario,
                            nombre).Text,
                        "No se limpió: " + nombre);
                }
            });
        }

        // ==========================================
        // PRUEBA 4: LIMPIAR TRANSPORTISTA
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_LimpiaTransportista()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                string[] campos =
                {
                    "LogisticaTxtNombreTransportista",
                    "LogisticaTxtNitTransportista",
                    "LogisticaTxtDireccionTransportista"
                };

                foreach (string nombre in campos)
                {
                    Assert.AreEqual(
                        "",
                        ObtenerControl<TextBox>(
                            formulario,
                            nombre).Text,
                        "No se limpió: " + nombre);
                }
            });
        }

        // ==========================================
        // PRUEBA 5: LIMPIAR DATOS BANCARIOS
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_LimpiaDatosBancarios()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                Assert.AreEqual(
                    -1,
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboBancoBeneficiario")
                        .SelectedIndex);

                Assert.AreEqual(
                    -1,
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboTipoCuenta")
                        .SelectedIndex);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroCuenta").Text);
            });
        }

        // ==========================================
        // PRUEBA 6: LIMPIAR DETALLE
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_LimpiaDetalle()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                DataGridView detalle =
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio");

                Assert.AreEqual(0, detalle.Rows.Count);
                Assert.AreEqual(5, detalle.Columns.Count);
            });
        }

        // ==========================================
        // PRUEBA 7: LIMPIAR PAGO
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_LimpiaMontoYConcepto()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtMontoPagar").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtConceptoPago").Text);
            });
        }

        // ==========================================
        // PRUEBA 8: RESTABLECER MONEDA
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_RestableceMonedaGTQ()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                Assert.IsTrue(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoQuetzales").Checked);

                Assert.IsFalse(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoDolares").Checked);

                Assert.AreEqual(
                    "GTQ",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);
            });
        }

        // ==========================================
        // PRUEBA 9: ACTUALIZAR RESUMEN
        // ==========================================

        [TestMethod]
        public void LimpiarFormulario_ActualizaResumen()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarFormulario");

                Label moneda = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenMoneda");

                Assert.AreEqual("GTQ", moneda.Text);

                Label numeroServicio = ObtenerControl<Label>(
                    formulario,
                    "LogisticaLblTextoResumenNumeroServicio");

                Assert.AreNotEqual(
                    "910007",
                    numeroServicio.Text);
            });
        }

        // ==========================================
        // PRUEBA 10: LIMPIAR DATOS SIN CAMBIAR BANCO
        // ==========================================

        [TestMethod]
        public void LimpiarDatosServicio_ConservaBanco()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarDatosServicio");

                Assert.AreEqual(
                    0,
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboBancoBeneficiario")
                        .SelectedIndex);

                Assert.AreEqual(
                    "1234567890",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroCuenta").Text);
            });
        }

        // ==========================================
        // PRUEBA 11: LIMPIAR DATOS DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void LimpiarDatosServicio_LimpiaCamposYDetalle()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarDatosServicio");

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNombreTransportista").Text);

                Assert.AreEqual(
                    0,
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio").Rows.Count);
            });
        }

        // ==========================================
        // PRUEBA 12: LIMPIAR DATOS CONSERVA MONEDA
        // ==========================================

        [TestMethod]
        public void LimpiarDatosServicio_ConservaMonedaActual()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                EjecutarMetodo(
                    formulario,
                    "LimpiarDatosServicio");

                Assert.IsTrue(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoDolares").Checked);

                Assert.AreEqual(
                    "USD",
                    ObtenerControl<Label>(
                        formulario,
                        "LogisticaLblMonedaMonto").Text);
            });
        }
    }
}
