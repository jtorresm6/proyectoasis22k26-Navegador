
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
    public class FrmSolicitudPagoSeleccionServicioTests
    {
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

        private void AsignarControl(
            Frm_Solicitud_Pago formulario,
            string nombre,
            object valor)
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

            campo.SetValue(formulario, valor);
        }

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

        private void EjecutarMetodo(
            Frm_Solicitud_Pago formulario,
            string nombre,
            params object[] parametros)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    nombre,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic |
                    BindingFlags.Public);

            Assert.IsNotNull(
                metodo,
                "No se encontró el método: " + nombre);

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

        // PRUEBA 1: SERVICIO VÁLIDO

        [TestMethod]
        public void SeleccionarServicio_Valido_CargaDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox combo =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboServicioTransporte");

                ClsServicioTransporte servicio =
                    CrearServicioValido();

                combo.Items.Add(servicio);
                combo.SelectedIndex = 0;

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                Assert.AreEqual(
                    "910007",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);

                Assert.AreEqual(
                    "GUIA-D26-0007",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroGuia").Text);

                Assert.AreEqual(
                    "500.00",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtMontoPagar").Text);
            });
        }

        // PRUEBA 2: SIN SERVICIO SELECCIONADO

        [TestMethod]
        public void SeleccionarServicio_SinSeleccion_LimpiaDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox combo =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboServicioTransporte");

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNumeroServicio").Text = "910007";

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtMontoPagar").Text = "500.00";

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtMontoPagar").Text);
            });
        }

        // PRUEBA 3: CONSERVAR DATOS BANCARIOS

        [TestMethod]
        public void SeleccionarServicio_SinSeleccion_ConservaBanco()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox banco =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboBancoBeneficiario");

                banco.Items.Add("Banco Industrial");
                banco.SelectedIndex = 0;

                ObtenerControl<TextBox>(
                    formulario,
                    "LogisticaTxtNumeroCuenta").Text = "1234567890";

                ComboBox combo =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboServicioTransporte");

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                Assert.AreEqual(0, banco.SelectedIndex);

                Assert.AreEqual(
                    "1234567890",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroCuenta").Text);
            });
        }

        // PRUEBA 4: ACTUALIZAR DETALLE

        [TestMethod]
        public void SeleccionarServicio_Valido_CargaDetalle()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox combo =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboServicioTransporte");

                combo.Items.Add(CrearServicioValido());
                combo.SelectedIndex = 0;

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                DataGridView detalle =
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio");

                Assert.AreEqual(1, detalle.Rows.Count);

                Assert.AreEqual(
                    "Transporte externo",
                    Convert.ToString(
                        detalle.Rows[0].Cells[1].Value));
            });
        }

        // PRUEBA 5: CAMBIAR ENTRE SERVICIOS

        [TestMethod]
        public void SeleccionarServicio_CambiarSeleccion_ActualizaDatos()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ComboBox combo =
                    ObtenerControl<ComboBox>(
                        formulario,
                        "LogisticaCboServicioTransporte");

                ClsServicioTransporte primero =
                    CrearServicioValido();

                ClsServicioTransporte segundo =
                    CrearServicioValido();

                segundo.IdServicio = 910008;
                segundo.NumeroGuia = "GUIA-D26-0008";
                segundo.TipoServicio = "Servicio refrigerado";
                segundo.MontoTotal = 750m;

                combo.Items.Add(primero);
                combo.Items.Add(segundo);

                combo.SelectedIndex = 0;

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                combo.SelectedIndex = 1;

                EjecutarMetodo(
                    formulario,
                    "LogisticaCboServicioTransporte_SelectionChangeCommitted",
                    combo,
                    EventArgs.Empty);

                Assert.AreEqual(
                    "910008",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroServicio").Text);

                Assert.AreEqual(
                    "GUIA-D26-0008",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroGuia").Text);

                Assert.AreEqual(
                    1,
                    ObtenerControl<DataGridView>(
                        formulario,
                        "LogisticaDgvDetalleServicio").Rows.Count);
            });
        }
    }
}
