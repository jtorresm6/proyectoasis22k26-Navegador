
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
    public class FrmSolicitudPagoBotonesTests
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

        private void AsignarCampo(
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
                "No existe el campo: " + nombre);

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
                "No existe el control: " + nombre);

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
                "No existe el método: " + nombre);

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
                AsignarCampo(formulario, nombre, new TextBox());
            }

            AsignarCampo(
                formulario,
                "LogisticaCboServicioTransporte",
                new ComboBox());

            AsignarCampo(
                formulario,
                "LogisticaCboBancoBeneficiario",
                new ComboBox());

            AsignarCampo(
                formulario,
                "LogisticaCboTipoCuenta",
                new ComboBox());

            AsignarCampo(
                formulario,
                "LogisticaRdoQuetzales",
                new RadioButton());

            AsignarCampo(
                formulario,
                "LogisticaRdoDolares",
                new RadioButton());

            AsignarCampo(
                formulario,
                "LogisticaLblMonedaMonto",
                new Label());

            DataGridView detalle = new DataGridView();
            detalle.AllowUserToAddRows = false;

            detalle.Columns.Add("Descripcion", "Descripción");
            detalle.Columns.Add("Cantidad", "Cantidad");
            detalle.Columns.Add("Costo", "Costo");
            detalle.Columns.Add("Total", "Total");

            AsignarCampo(
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
                AsignarCampo(formulario, nombre, new Label());
            }

            return formulario;
        }

        private void ColocarDatosDePrueba(
            Frm_Solicitud_Pago formulario)
        {
            ObtenerControl<TextBox>(
                formulario,
                "LogisticaTxtNumeroServicio").Text = "910007";

            ObtenerControl<TextBox>(
                formulario,
                "LogisticaTxtNumeroGuia").Text =
                    "GUIA-D26-0007";

            ObtenerControl<TextBox>(
                formulario,
                "LogisticaTxtMontoPagar").Text = "500.00";

            ObtenerControl<TextBox>(
                formulario,
                "LogisticaTxtConceptoPago").Text =
                    "Pago de transporte";

            ObtenerControl<TextBox>(
                formulario,
                "LogisticaTxtNumeroCuenta").Text =
                    "1234567890";

            ObtenerControl<RadioButton>(
                formulario,
                "LogisticaRdoDolares").Checked = true;
        }

        // ==========================================
        // PRUEBA 1: PRIMER BOTÓN LIMPIAR
        // ==========================================

        [TestMethod]
        public void LogisticaBtnLimpiar_Click_LimpiaFormulario()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ColocarDatosDePrueba(formulario);

                EjecutarMetodo(
                    formulario,
                    "LogisticaBtnLimpiar_Click",
                    null,
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

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtConceptoPago").Text);
            });
        }

        // ==========================================
        // PRUEBA 2: SEGUNDO BOTÓN LIMPIAR
        // ==========================================

        [TestMethod]
        public void LogisticaBtnLimpiar_Click_1_LimpiaFormulario()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ColocarDatosDePrueba(formulario);

                EjecutarMetodo(
                    formulario,
                    "LogisticaBtnLimpiar_Click_1",
                    null,
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

                Assert.AreEqual(
                    "",
                    ObtenerControl<TextBox>(
                        formulario,
                        "LogisticaTxtNumeroCuenta").Text);
            });
        }

        // ==========================================
        // PRUEBA 3: RESTABLECER MONEDA
        // ==========================================

        [TestMethod]
        public void LogisticaBtnLimpiar_Click_1_RestableceQuetzales()
        {
            EjecutarEnSTA(() =>
            {
                Frm_Solicitud_Pago formulario =
                    CrearFormularioSimulado();

                ColocarDatosDePrueba(formulario);

                EjecutarMetodo(
                    formulario,
                    "LogisticaBtnLimpiar_Click_1",
                    null,
                    EventArgs.Empty);

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
    }
}
