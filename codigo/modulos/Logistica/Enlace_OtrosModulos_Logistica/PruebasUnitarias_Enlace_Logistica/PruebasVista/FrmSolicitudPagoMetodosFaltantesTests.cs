using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;
using CapaContralador_Enlace_Logistica;
using CapaModelo_Enlace_Logistica.Contratos;
using CapaModelo_Enlace_Logistica.Entidades;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoMetodosFaltantesTests
    {
        private class RepositorioSimulado : ClsIRepositorioSolicitudPago
        {
            public IEnumerable<ClsServicioTransporte> Servicios { get; set; }
            public int NumeroConsultas { get; private set; }

            public IEnumerable<ClsServicioTransporte> ObtenerServiciosDisponibles()
            {
                NumeroConsultas++;
                return Servicios;
            }
        }

        private static void EjecutarEnSTA(Action accion)
        {
            Exception error = null;
            Thread hilo = new Thread(() =>
            {
                try { accion(); }
                catch (Exception ex) { error = ex; }
            });
            hilo.SetApartmentState(ApartmentState.STA);
            hilo.Start();
            hilo.Join();
            if (error != null) ExceptionDispatchInfo.Capture(error).Throw();
        }

        private static FieldInfo Campo(string nombre)
        {
            FieldInfo campo = typeof(Frm_Solicitud_Pago).GetField(
                nombre, BindingFlags.Instance | BindingFlags.NonPublic |
                BindingFlags.Public);
            Assert.IsNotNull(campo, "No existe el campo: " + nombre);
            return campo;
        }

        private static void AsignarCampo(Frm_Solicitud_Pago formulario,
            string nombre, object valor)
        {
            Campo(nombre).SetValue(formulario, valor);
        }

        private static T ObtenerCampo<T>(Frm_Solicitud_Pago formulario,
            string nombre)
        {
            object valor = Campo(nombre).GetValue(formulario);
            Assert.IsNotNull(valor, "El campo es nulo: " + nombre);
            return (T)valor;
        }

        private static void EjecutarMetodo(Frm_Solicitud_Pago formulario,
            string nombre)
        {
            MethodInfo metodo = typeof(Frm_Solicitud_Pago).GetMethod(
                nombre, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(metodo, "No existe el método: " + nombre);
            try { metodo.Invoke(formulario, null); }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        private static List<ClsServicioTransporte> CrearServiciosSimulados()
        {
            return new List<ClsServicioTransporte>
            {
                new ClsServicioTransporte
                {
                    IdServicio = 910007,
                    NumeroGuia = "GUIA-D26-0007",
                    NombreTransportista = "Transportista DEMO26 07",
                    TipoServicio = "Transporte externo",
                    Cantidad = 2m,
                    CostoUnitario = 250m,
                    MontoTotal = 500m,
                    Moneda = "GTQ",
                    FechaServicio = new DateTime(2026, 10, 17)
                },
                new ClsServicioTransporte
                {
                    IdServicio = 910008,
                    NumeroGuia = "GUIA-D26-0008",
                    NombreTransportista = "Transportista DEMO26 08",
                    TipoServicio = "Servicio refrigerado",
                    Cantidad = 3m,
                    CostoUnitario = 300m,
                    MontoTotal = 900m,
                    Moneda = "USD",
                    FechaServicio = new DateTime(2026, 10, 18)
                }
            };
        }

        private static Frm_Solicitud_Pago CrearFormularioSimulado(
            RepositorioSimulado repositorio)
        {
            Frm_Solicitud_Pago formulario = (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(typeof(Frm_Solicitud_Pago));

            AsignarCampo(formulario, "controladorSolicitudPago",
                new ClsSolicitudPagoControlador(repositorio));
            AsignarCampo(formulario, "serviciosDisponibles",
                new List<ClsServicioTransporte>());

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
                AsignarCampo(formulario, nombre, new TextBox());

            // Un ComboBox aislado necesita un BindingContext para resolver
            // el DataSource sin ejecutar el constructor real del formulario.
            ComboBox comboServicios = new ComboBox();
            comboServicios.BindingContext = new BindingContext();
            AsignarCampo(formulario, "LogisticaCboServicioTransporte",
                comboServicios);

            AsignarCampo(formulario, "LogisticaCboBancoBeneficiario",
                new ComboBox());
            AsignarCampo(formulario, "LogisticaCboTipoCuenta",
                new ComboBox());
            AsignarCampo(formulario, "LogisticaRdoQuetzales",
                new RadioButton());
            AsignarCampo(formulario, "LogisticaRdoDolares",
                new RadioButton());
            AsignarCampo(formulario, "LogisticaLblMonedaMonto",
                new Label());

            DataGridView detalle = new DataGridView();
            detalle.AllowUserToAddRows = false;
            detalle.Columns.Add("Numero", "Numero");
            detalle.Columns.Add("Descripcion", "Descripcion");
            detalle.Columns.Add("Cantidad", "Cantidad");
            detalle.Columns.Add("CostoUnitario", "Costo unitario");
            detalle.Columns.Add("Monto", "Monto");
            AsignarCampo(formulario, "LogisticaDgvDetalleServicio", detalle);

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
                AsignarCampo(formulario, nombre, new Label());

            return formulario;
        }

        // Valida la lista realmente asignada por el método de producción.
        // Items.Count depende también del ciclo de enlace visual de WinForms.
        private static List<ClsServicioTransporte> ServiciosEnCombo(
            Frm_Solicitud_Pago formulario)
        {
            ComboBox combo = ObtenerCampo<ComboBox>(formulario,
                "LogisticaCboServicioTransporte");
            Assert.IsNotNull(combo.DataSource,
                "CargarServiciosTransporte no asignó DataSource.");
            List<ClsServicioTransporte> lista =
                combo.DataSource as List<ClsServicioTransporte>;
            Assert.IsNotNull(lista,
                "El DataSource no es List<ClsServicioTransporte>.");
            return lista;
        }

        [TestMethod]
        public void CargarServicios_DosServicios_CargaCombo()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = CrearServiciosSimulados() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                EjecutarMetodo(form, "CargarServiciosTransporte");

                List<ClsServicioTransporte> lista = ServiciosEnCombo(form);
                Assert.AreEqual(2, lista.Count);
                Assert.AreEqual("GUIA-D26-0007", lista[0].NumeroGuia);
                Assert.AreEqual("GUIA-D26-0008", lista[1].NumeroGuia);
                Assert.AreEqual(1, repo.NumeroConsultas);
            });
        }

        [TestMethod]
        public void CargarServicios_ListaVacia_NoAgregaItems()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = new List<ClsServicioTransporte>() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                EjecutarMetodo(form, "CargarServiciosTransporte");

                Assert.AreEqual(0, ServiciosEnCombo(form).Count);
                ComboBox combo = ObtenerCampo<ComboBox>(form,
                    "LogisticaCboServicioTransporte");
                Assert.AreEqual(-1, combo.SelectedIndex);
                Assert.AreEqual(1, repo.NumeroConsultas);
            });
        }

        [TestMethod]
        public void CargarServicios_ConfiguraDisplayYValueMember()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = CrearServiciosSimulados() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                EjecutarMetodo(form, "CargarServiciosTransporte");

                ComboBox combo = ObtenerCampo<ComboBox>(form,
                    "LogisticaCboServicioTransporte");
                Assert.AreEqual("NumeroGuia", combo.DisplayMember);
                Assert.AreEqual("IdServicio", combo.ValueMember);
                Assert.AreEqual(-1, combo.SelectedIndex);
                Assert.AreEqual(2, ServiciosEnCombo(form).Count);
            });
        }

        [TestMethod]
        public void CargarServicios_Recargar_ReemplazaLista()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = CrearServiciosSimulados() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                EjecutarMetodo(form, "CargarServiciosTransporte");
                Assert.AreEqual(2, ServiciosEnCombo(form).Count);

                repo.Servicios = new List<ClsServicioTransporte>
                {
                    new ClsServicioTransporte
                    {
                        IdServicio = 910009,
                        NumeroGuia = "GUIA-D26-0009",
                        Moneda = "GTQ",
                        Cantidad = 1m,
                        MontoTotal = 100m
                    }
                };
                EjecutarMetodo(form, "CargarServiciosTransporte");

                List<ClsServicioTransporte> lista = ServiciosEnCombo(form);
                Assert.AreEqual(1, lista.Count);
                Assert.AreEqual("GUIA-D26-0009", lista[0].NumeroGuia);
                Assert.AreEqual(2, repo.NumeroConsultas);
            });
        }

        [TestMethod]
        public void InicializarFormulario_LimpiaCamposYCargaServicios()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = CrearServiciosSimulados() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                ObtenerCampo<TextBox>(form,
                    "LogisticaTxtNumeroServicio").Text = "910007";
                ObtenerCampo<TextBox>(form,
                    "LogisticaTxtMontoPagar").Text = "500.00";
                ObtenerCampo<TextBox>(form,
                    "LogisticaTxtConceptoPago").Text = "Pago anterior";

                EjecutarMetodo(form, "InicializarFormulario");

                Assert.AreEqual("", ObtenerCampo<TextBox>(form,
                    "LogisticaTxtNumeroServicio").Text);
                Assert.AreEqual("", ObtenerCampo<TextBox>(form,
                    "LogisticaTxtMontoPagar").Text);
                Assert.AreEqual("", ObtenerCampo<TextBox>(form,
                    "LogisticaTxtConceptoPago").Text);
                Assert.AreEqual(2, ServiciosEnCombo(form).Count);
                Assert.AreEqual(1, repo.NumeroConsultas);
            });
        }

        [TestMethod]
        public void InicializarFormulario_RestableceMonedaGTQ()
        {
            EjecutarEnSTA(() =>
            {
                RepositorioSimulado repo = new RepositorioSimulado
                { Servicios = CrearServiciosSimulados() };
                Frm_Solicitud_Pago form = CrearFormularioSimulado(repo);
                ObtenerCampo<RadioButton>(form,
                    "LogisticaRdoDolares").Checked = true;

                EjecutarMetodo(form, "InicializarFormulario");

                Assert.IsTrue(ObtenerCampo<RadioButton>(form,
                    "LogisticaRdoQuetzales").Checked);
                Assert.IsFalse(ObtenerCampo<RadioButton>(form,
                    "LogisticaRdoDolares").Checked);
                Assert.AreEqual("GTQ", ObtenerCampo<Label>(form,
                    "LogisticaLblMonedaMonto").Text);
                Assert.AreEqual(1, repo.NumeroConsultas);
            });
        }
    }
}

