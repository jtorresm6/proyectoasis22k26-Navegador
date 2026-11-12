
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoMetodosTests
    {
        // ==========================================
        // PREPARAR INSTANCIA SIN EJECUTAR CONSTRUCTOR
        // ==========================================

        private Frm_Solicitud_Pago CrearFormularioSinInicializar()
        {
            // Se utiliza exclusivamente para invocar
            // métodos privados que no dependen de la UI.
            return (Frm_Solicitud_Pago)
                FormatterServices.GetUninitializedObject(
                    typeof(Frm_Solicitud_Pago));
        }

        // ==========================================
        // INVOCAR OBTENER TEXTO RESUMEN
        // ==========================================

        private string EjecutarObtenerTextoResumen(string texto)
        {
            Frm_Solicitud_Pago formulario =
                CrearFormularioSinInicializar();

            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "ObtenerTextoResumen",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró ObtenerTextoResumen.");

            object resultado = metodo.Invoke(
                formulario,
                new object[] { texto });

            return (string)resultado;
        }

        // ==========================================
        // INVOCAR OBTENER DECIMAL VISTA PREVIA
        // ==========================================

        private bool EjecutarObtenerDecimal(
            string texto,
            out decimal valor)
        {
            Frm_Solicitud_Pago formulario =
                CrearFormularioSinInicializar();

            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "ObtenerDecimalVistaPrevia",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(
                metodo,
                "No se encontró ObtenerDecimalVistaPrevia.");

            object[] parametros = { texto, 0m };

            bool resultado = (bool)metodo.Invoke(
                formulario,
                parametros);

            valor = (decimal)parametros[1];

            return resultado;
        }

        // ==========================================
        // PRUEBAS OBTENER TEXTO RESUMEN
        // ==========================================

        [TestMethod]
        public void ObtenerTextoResumen_TextoValido_DevuelveTexto()
        {
            // ARRANGE
            string texto = "Transportista DEMO26 07";

            // ACT
            string resultado =
                EjecutarObtenerTextoResumen(texto);

            // ASSERT
            Assert.AreEqual(
                "Transportista DEMO26 07",
                resultado);
        }

        [TestMethod]
        public void ObtenerTextoResumen_ConEspacios_EliminaEspacios()
        {
            // ARRANGE
            string texto = "   Banco de prueba   ";

            // ACT
            string resultado =
                EjecutarObtenerTextoResumen(texto);

            // ASSERT
            Assert.AreEqual("Banco de prueba", resultado);
        }

        [TestMethod]
        public void ObtenerTextoResumen_Vacio_DevuelveGuion()
        {
            // ARRANGE
            string texto = string.Empty;

            // ACT
            string resultado =
                EjecutarObtenerTextoResumen(texto);

            // ASSERT
            Assert.AreEqual("-", resultado);
        }

        [TestMethod]
        public void ObtenerTextoResumen_Nulo_DevuelveGuion()
        {
            // ARRANGE
            string texto = null;

            // ACT
            string resultado =
                EjecutarObtenerTextoResumen(texto);

            // ASSERT
            Assert.AreEqual("-", resultado);
        }

        [TestMethod]
        public void ObtenerTextoResumen_SoloEspacios_DevuelveGuion()
        {
            // ARRANGE
            string texto = "    ";

            // ACT
            string resultado =
                EjecutarObtenerTextoResumen(texto);

            // ASSERT
            Assert.AreEqual("-", resultado);
        }

        // ==========================================
        // PRUEBAS OBTENER DECIMAL VISTA PREVIA
        // ==========================================

        [TestMethod]
        public void ObtenerDecimal_EnteroValido_DevuelveValor()
        {
            // ARRANGE
            string texto = "405";
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsTrue(resultado);
            Assert.AreEqual(405m, valor);
        }

        [TestMethod]
        public void ObtenerDecimal_DecimalValido_DevuelveValor()
        {
            // ARRANGE
            string texto = "405.75";
            decimal valor;

            CultureInfo culturaAnterior =
                CultureInfo.CurrentCulture;

            try
            {
                // Cultura fija para evitar depender
                // de la configuración regional del equipo.
                CultureInfo.CurrentCulture =
                    CultureInfo.GetCultureInfo("en-US");

                // ACT
                bool resultado =
                    EjecutarObtenerDecimal(texto, out valor);

                // ASSERT
                Assert.IsTrue(resultado);
                Assert.AreEqual(405.75m, valor);
            }
            finally
            {
                CultureInfo.CurrentCulture = culturaAnterior;
            }
        }

        [TestMethod]
        public void ObtenerDecimal_TextoInvalido_DevuelveFalse()
        {
            // ARRANGE
            string texto = "abc";
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsFalse(resultado);
            Assert.AreEqual(0m, valor);
        }

        [TestMethod]
        public void ObtenerDecimal_TextoVacio_DevuelveFalse()
        {
            // ARRANGE
            string texto = string.Empty;
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsFalse(resultado);
            Assert.AreEqual(0m, valor);
        }

        [TestMethod]
        public void ObtenerDecimal_Nulo_DevuelveFalse()
        {
            // ARRANGE
            string texto = null;
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsFalse(resultado);
            Assert.AreEqual(0m, valor);
        }

        [TestMethod]
        public void ObtenerDecimal_NumeroNegativo_DevuelveValor()
        {
            // ARRANGE
            string texto = "-405";
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsTrue(resultado);
            Assert.AreEqual(-405m, valor);
        }

        [TestMethod]
        public void ObtenerDecimal_Espacios_DevuelveFalse()
        {
            // ARRANGE
            string texto = "   ";
            decimal valor;

            // ACT
            bool resultado =
                EjecutarObtenerDecimal(texto, out valor);

            // ASSERT
            Assert.IsFalse(resultado);
            Assert.AreEqual(0m, valor);
        }
    }
}

