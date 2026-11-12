
using System;
using System.Globalization;
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
    public class FrmSolicitudPagoMostrarDatosTests
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
        // OBTENER CONTROLES PRIVADOS
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

            Assert.IsNotNull(campo);

            T control = campo.GetValue(formulario) as T;

            Assert.IsNotNull(
                control,
                "No se encontró el control: " + nombre);

            return control;
        }

        // ==========================================
        // CREAR FORMULARIO SIN CONSULTAR MYSQL
        // ==========================================

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
                "LogisticaTxtMontoPagar",
                "LogisticaTxtConceptoPago",
                "LogisticaTxtNumeroCuenta"
            };

            foreach (string nombre in cajas)
            {
                AsignarControl(
                    formulario,
                    nombre,
                    new TextBox());
            }

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

            DataGridView detalle = new DataGridView();
            detalle.AllowUserToAddRows = false;

            detalle.Columns.Add("Numero", "No.");
            detalle.Columns.Add("Descripcion", "Descripción");
            detalle.Columns.Add("Cantidad", "Cantidad");
            detalle.Columns.Add("CostoUnitario", "Costo unitario");
            detalle.Columns.Add("Monto", "Monto");

            AsignarControl(
                formulario,
                "LogisticaDgvDetalleServicio",
                detalle);

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
                Cantidad = 2m,
                CostoUnitario = 250m,
                MontoTotal = 500m,
                Moneda = "GTQ"
            };
        }

        // ==========================================
        // INVOCAR MÉTODO PRIVADO REAL
        // ==========================================

        private void MostrarDatos(
            Frm_Solicitud_Pago formulario,
            ClsServicioTransporte servicio)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "MostrarDatosServicio",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró MostrarDatosServicio.");

            try
            {
                metodo.Invoke(
                    formulario,
                    new object[] { servicio });
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
        public void MostrarDatos_NumeroServicio_CargaId()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.AreEqual(
                    "910007",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);
            });
        }

        // ==========================================
        // PRUEBA 2: NÚMERO DE GUÍA
        // ==========================================

        [TestMethod]
        public void MostrarDatos_NumeroGuia_CargaGuia()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.AreEqual(
                    "GUIA-D26-0007",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroGuia").Text);
            });
        }

        // ==========================================
        // PRUEBA 3: FECHA DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void MostrarDatos_FechaServicio_CargaFecha()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.AreEqual(
                    "17/10/2026",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtFechaServicio").Text);
            });
        }

        // ==========================================
        // PRUEBA 4: DATOS DEL TRANSPORTISTA
        // ==========================================

        [TestMethod]
        public void MostrarDatos_Transportista_CargaDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.AreEqual(
                    "Transportista DEMO26 07",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNombreTransportista").Text);

                Assert.AreEqual(
                    "1234567-8",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNitTransportista").Text);

                Assert.AreEqual(
                    "Ciudad de Guatemala",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtDireccionTransportista").Text);
            });
        }

        // ==========================================
        // PRUEBA 5: CONCEPTO DEL PAGO
        // ==========================================

        [TestMethod]
        public void MostrarDatos_TipoServicio_GeneraConcepto()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.AreEqual(
                    "Pago por servicio de transporte: Transporte externo",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtConceptoPago").Text);
            });
        }

        // ==========================================
        // PRUEBA 6: MONEDA GTQ
        // ==========================================

        [TestMethod]
        public void MostrarDatos_MonedaGTQ_SeleccionaQuetzales()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                MostrarDatos(
                    formulario,
                    CrearServicioValido());

                Assert.IsTrue(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoQuetzales").Checked);

                Assert.IsFalse(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoDolares").Checked);
            });
        }

        // ==========================================
        // PRUEBA 7: MONEDA USD
        // ==========================================

        [TestMethod]
        public void MostrarDatos_MonedaUSD_SeleccionaDolares()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                servicio.Moneda = "usd";

                MostrarDatos(formulario, servicio);

                Assert.IsTrue(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoDolares").Checked);

                Assert.IsFalse(
                    ObtenerControl<RadioButton>(
                        formulario,
                        "LogisticaRdoQuetzales").Checked);
            });
        }

        // ==========================================
        // PRUEBA 8: DETALLE DEL SERVICIO
        // ==========================================

        [TestMethod]
        public void MostrarDatos_ServicioValido_AgregaFilaDetalle()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                MostrarDatos(formulario, servicio);

                DataGridView detalle =
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio");

                Assert.AreEqual(1, detalle.Rows.Count);

                Assert.AreEqual(
                    "1",
                    Convert.ToString(
                        detalle.Rows[0].Cells[0].Value));

                Assert.AreEqual(
                    "Transporte externo",
                    Convert.ToString(
                        detalle.Rows[0].Cells[1].Value));

                Assert.AreEqual(
                    2m,
                    decimal.Parse(
                        Convert.ToString(
                            detalle.Rows[0].Cells[2].Value),
                        NumberStyles.Number,
                        CultureInfo.CurrentCulture));

                Assert.AreEqual(
                    250m,
                    decimal.Parse(
                        Convert.ToString(
                            detalle.Rows[0].Cells[3].Value),
                        NumberStyles.Number,
                        CultureInfo.CurrentCulture));

                Assert.AreEqual(
                    500m,
                    decimal.Parse(
                        Convert.ToString(
                            detalle.Rows[0].Cells[4].Value),
                        NumberStyles.Number,
                        CultureInfo.CurrentCulture));
            });
        }

        // ==========================================
        // PRUEBA 9: VALORES NULOS
        // ==========================================

        [TestMethod]
        public void MostrarDatos_ValoresNulos_NoGeneraTextosInvalidos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                servicio.NumeroGuia = null;
                servicio.FechaServicio = null;
                servicio.NombreTransportista = null;
                servicio.NitTransportista = null;
                servicio.DireccionTransportista = null;
                servicio.TipoServicio = null;

                MostrarDatos(formulario, servicio);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroGuia").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtFechaServicio").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNombreTransportista").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNitTransportista").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtDireccionTransportista").Text);

                Assert.AreEqual(
                    "Pago por servicio de transporte: ",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtConceptoPago").Text);
            });
        }

        // ==========================================
        // PRUEBA 10: CAMBIO DE SERVICIO
        // ==========================================

        [TestMethod]
        public void MostrarDatos_CambiarServicio_ReemplazaDetalle()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ClsServicioTransporte primero =
                    CrearServicioValido();

                ClsServicioTransporte segundo =
                    CrearServicioValido();

                segundo.IdServicio = 910008;
                segundo.TipoServicio = "Servicio refrigerado";
                segundo.Cantidad = 3m;
                segundo.CostoUnitario = 100m;
                segundo.MontoTotal = 300m;

                MostrarDatos(formulario, primero);
                MostrarDatos(formulario, segundo);

                DataGridView detalle =
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio");

                Assert.AreEqual(1, detalle.Rows.Count);

                Assert.AreEqual(
                    "Servicio refrigerado",
                    Convert.ToString(
                        detalle.Rows[0].Cells[1].Value));

                Assert.AreEqual(
                    "910008",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);
            });
        }
    }
}

