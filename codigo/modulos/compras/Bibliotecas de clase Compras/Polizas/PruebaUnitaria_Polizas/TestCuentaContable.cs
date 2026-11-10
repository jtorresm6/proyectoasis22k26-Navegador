using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Polizas;

namespace PruebaUnitaria_Polizas
{
    [TestClass]
    public class TestCuentaContable
    {
        [TestMethod]
        public void ComprasFuncFormatoValido_AceptaCodigoCompleto()
        {
            Assert.IsTrue(ClsCuentaContable.ComprasFuncFormatoValido("1.01.02.001"));
        }

        [TestMethod]
        public void ComprasFuncFormatoValido_RechazaCodigoIncompleto()
        {
            Assert.IsFalse(ClsCuentaContable.ComprasFuncFormatoValido("1.01.02"));
        }

        [TestMethod]
        public void ComprasFuncFormatoValido_RechazaNuloYCaracteresInvalidos()
        {
            Assert.IsFalse(ClsCuentaContable.ComprasFuncFormatoValido(null));
            Assert.IsFalse(ClsCuentaContable.ComprasFuncFormatoValido("1.01.02.ABC"));
        }
    }
}
