
using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaVista_Enlace_Logistica.Formas;

namespace PruebasUnitarias_Enlace_Logistica.PruebasVista
{
    [TestClass]
    public class FrmSolicitudPagoVistaPreviaPendientesTests
    {
        [TestMethod]
        public void VistaPrevia_MetodoPrivado_Existe()
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "MostrarVistaPrevia",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(metodo);

            Assert.AreEqual(
                typeof(void),
                metodo.ReturnType);

            Assert.AreEqual(
                0,
                metodo.GetParameters().Length);
        }

        [TestMethod]
        public void BotonVistaPrevia_Evento_Existe()
        {
            MethodInfo metodo =
                typeof(Frm_Solicitud_Pago).GetMethod(
                    "LogisticaBtnVistaPrevia_Click",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.IsNotNull(metodo);

            ParameterInfo[] parametros =
                metodo.GetParameters();

            Assert.AreEqual(2, parametros.Length);

            Assert.AreEqual(
                typeof(object),
                parametros[0].ParameterType);

            Assert.AreEqual(
                typeof(EventArgs),
                parametros[1].ParameterType);
        }
    }
}

