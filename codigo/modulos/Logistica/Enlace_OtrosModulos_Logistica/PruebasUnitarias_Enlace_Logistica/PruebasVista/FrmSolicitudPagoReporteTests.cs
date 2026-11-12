
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoReporteTests
    {
        private const string NombreRecurso =
            "CapaVista_Enlace_Logistica.Reportes." +
            "SolicitudPagoTransporte.rdlc";

        private Frm_Solicitud_Pago CrearFormularioSimulado()
        {
            return (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));
        }

        private string ObtenerRuta(
            Frm_Solicitud_Pago formulario)
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "ObtenerRutaReporteSolicitudPago",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró el método del reporte.");

            try
            {
                return (string)metodo.Invoke(
                    formulario,
                    null);
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    System.Runtime.ExceptionServices
                        .ExceptionDispatchInfo
                        .Capture(ex.InnerException)
                        .Throw();
                }

                throw;
            }
        }

        // ==========================================
        // PRUEBA 1: COMPROBAR RECURSO RDLC
        // ==========================================

        [TestMethod]
        public void Reporte_RecursoIncrustado_Existe()
        {
            Assembly ensamblado =
                typeof(Frm_Solicitud_Pago).Assembly;

            string[] recursos =
                ensamblado.GetManifestResourceNames();

            Assert.IsTrue(
                recursos.Contains(NombreRecurso),
                "El archivo RDLC no está incrustado " +
                "con el nombre esperado: " +
                NombreRecurso);
        }

        // ==========================================
        // PRUEBA 2: CREAR COPIA TEMPORAL
        // ==========================================

        [TestMethod]
        public void ObtenerRutaReporte_GeneraArchivoTemporal()
        {
            Frm_Solicitud_Pago formulario =
                CrearFormularioSimulado();

            string ruta = null;

            try
            {
                ruta = ObtenerRuta(formulario);

                Assert.IsFalse(
                    string.IsNullOrWhiteSpace(ruta));

                Assert.IsTrue(
                    File.Exists(ruta),
                    "No se generó el archivo temporal.");

                Assert.AreEqual(
                    ".rdlc",
                    Path.GetExtension(ruta));

                StringAssert.StartsWith(
                    Path.GetFileName(ruta),
                    "SolicitudPagoTransporte_");

                Assert.AreEqual(
                    Path.GetFullPath(Path.GetTempPath())
                        .TrimEnd(
                            Path.DirectorySeparatorChar),
                    Path.GetDirectoryName(
                        Path.GetFullPath(ruta))
                        .TrimEnd(
                            Path.DirectorySeparatorChar));
            }
            finally
            {
                if (!string.IsNullOrEmpty(ruta) &&
                    File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
            }
        }

        // ==========================================
        // PRUEBA 3: VERIFICAR CONTENIDO
        // ==========================================

        [TestMethod]
        public void ObtenerRutaReporte_CopiaContenidoOriginal()
        {
            Frm_Solicitud_Pago formulario =
                CrearFormularioSimulado();

            string ruta = null;

            try
            {
                ruta = ObtenerRuta(formulario);

                byte[] original;

                using (Stream recurso =
                    typeof(Frm_Solicitud_Pago)
                        .Assembly
                        .GetManifestResourceStream(
                            NombreRecurso))
                {
                    Assert.IsNotNull(
                        recurso,
                        "No se encontró el recurso original.");

                    using (MemoryStream memoria =
                        new MemoryStream())
                    {
                        recurso.CopyTo(memoria);
                        original = memoria.ToArray();
                    }
                }

                byte[] copia = File.ReadAllBytes(ruta);

                CollectionAssert.AreEqual(
                    original,
                    copia,
                    "El archivo temporal no coincide " +
                    "con el recurso incrustado.");
            }
            finally
            {
                if (!string.IsNullOrEmpty(ruta) &&
                    File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
            }
        }

        // ==========================================
        // PRUEBA 4: RUTAS DIFERENTES
        // ==========================================

        [TestMethod]
        public void ObtenerRutaReporte_DosLlamadas_GeneranRutasDistintas()
        {
            Frm_Solicitud_Pago formulario =
                CrearFormularioSimulado();

            string primera = null;
            string segunda = null;

            try
            {
                primera = ObtenerRuta(formulario);
                segunda = ObtenerRuta(formulario);

                Assert.AreNotEqual(
                    primera,
                    segunda,
                    "Cada llamada debe generar " +
                    "una ruta temporal diferente.");

                Assert.IsTrue(File.Exists(primera));
                Assert.IsTrue(File.Exists(segunda));
            }
            finally
            {
                if (!string.IsNullOrEmpty(primera) &&
                    File.Exists(primera))
                {
                    File.Delete(primera);
                }

                if (!string.IsNullOrEmpty(segunda) &&
                    File.Exists(segunda))
                {
                    File.Delete(segunda);
                }
            }
        }
    }
}
