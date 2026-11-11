using CapaVista_Ventas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Windows.Forms;

namespace TestVentas
{
    [TestClass]
    public class TestVentas
    {
        [TestMethod]
        public void VentasMetActualizarSesion_ConDatos_MuestraUsuarioYRol()
        {
            // Arrange: preparar el formulario y los datos de sesión.
            using (var Formulario = new FrmMdiVentas())
            {
                string Usuario = "Javier";
                string Rol = "Administrador";

                // Act: ejecutar el método que se está probando.
                Formulario.VentasMetActualizarSesion(Usuario, Rol);

                // Assert: comprobar los textos esperados.
                Assert.AreEqual("Usuario: Javier", VentasFuncTexto(Formulario, "VentasLblUsuario"));
                Assert.AreEqual("Rol: Administrador", VentasFuncTexto(Formulario, "VentasLblRol"));
            }
        }

        [DataTestMethod]
        [DataRow(null, null, "Usuario: Sin sesión", "Rol: Sin asignar")]
        [DataRow("", "", "Usuario: Sin sesión", "Rol: Sin asignar")]
        [DataRow("   ", "   ", "Usuario: Sin sesión", "Rol: Sin asignar")]
        [DataRow("Javier", null, "Usuario: Javier", "Rol: Sin asignar")]
        [DataRow(null, "Vendedor", "Usuario: Sin sesión", "Rol: Vendedor")]
        public void VentasMetActualizarSesion_ConDatosFaltantes_MuestraValoresPredeterminados(
            string Usuario, string Rol, string UsuarioEsperado, string RolEsperado)
        {
            // Arrange: cada fila proporciona un caso de datos faltantes.
            using (var Formulario = new FrmMdiVentas())
            {
                // Act: actualizar la sesión con ese caso.
                Formulario.VentasMetActualizarSesion(Usuario, Rol);

                // Assert: comprobar cada campo de forma independiente.
                Assert.AreEqual(UsuarioEsperado, VentasFuncTexto(Formulario, "VentasLblUsuario"));
                Assert.AreEqual(RolEsperado, VentasFuncTexto(Formulario, "VentasLblRol"));
            }
        }

        [TestMethod]
        public void VentasMetActualizarSesion_ConSesionAnterior_ReemplazaLosDatos()
        {
            // Arrange: iniciar con otro usuario.
            using (var Formulario = new FrmMdiVentas("Isabel", "Administrador"))
            {
                // Act: cambiar la sesión existente.
                Formulario.VentasMetActualizarSesion("Javier", "Vendedor");

                // Assert: los textos corresponden a la nueva sesión.
                Assert.AreEqual("Usuario: Javier", VentasFuncTexto(Formulario, "VentasLblUsuario"));
                Assert.AreEqual("Rol: Vendedor", VentasFuncTexto(Formulario, "VentasLblRol"));
            }
        }

        [TestMethod]
        public void VentasMetActualizarIndicadores_ConImportes_MuestraQuetzalesYContadores()
        {
            // Arrange: preparar importes y cantidades diferentes para cada indicador.
            using (var Formulario = new FrmMdiVentas())
            {
                decimal VentasDia = 1234.567m;
                decimal SaldoCxC = 9876.543m;
                decimal PagosDia = 250.5m;
                int Clientes = 1234;
                int Facturas = 27;
                int Pedidos = 8;

                // Act: actualizar los seis indicadores.
                Formulario.VentasMetActualizarIndicadores(VentasDia, SaldoCxC, Clientes, Facturas, PagosDia, Pedidos);

                // Assert: comprobar moneda, decimales y posición de cada valor.
                Assert.AreEqual("Q 1,234.57", VentasFuncTexto(Formulario, "VentasLblValorKpi1"));
                Assert.AreEqual("Q 9,876.54", VentasFuncTexto(Formulario, "VentasLblValorKpi2"));
                Assert.AreEqual("1,234", VentasFuncTexto(Formulario, "VentasLblValorKpi3"));
                Assert.AreEqual("27", VentasFuncTexto(Formulario, "VentasLblValorKpi4"));
                Assert.AreEqual("Q 250.50", VentasFuncTexto(Formulario, "VentasLblValorKpi5"));
                Assert.AreEqual("8", VentasFuncTexto(Formulario, "VentasLblValorKpi6"));
            }
        }

        [TestMethod]
        public void VentasMetActualizarIndicadores_ConCeros_ReemplazaLosValoresAnteriores()
        {
            // Arrange: preparar un formulario que ya muestra datos.
            using (var Formulario = new FrmMdiVentas())
            {
                Formulario.VentasMetActualizarIndicadores(100m, 200m, 3, 4, 50m, 6);

                // Act: actualizar con un período sin operaciones.
                Formulario.VentasMetActualizarIndicadores(0m, 0m, 0, 0, 0m, 0);

                // Assert: no deben quedar valores del período anterior.
                Assert.AreEqual("Q 0.00", VentasFuncTexto(Formulario, "VentasLblValorKpi1"));
                Assert.AreEqual("Q 0.00", VentasFuncTexto(Formulario, "VentasLblValorKpi2"));
                Assert.AreEqual("0", VentasFuncTexto(Formulario, "VentasLblValorKpi3"));
                Assert.AreEqual("0", VentasFuncTexto(Formulario, "VentasLblValorKpi4"));
                Assert.AreEqual("Q 0.00", VentasFuncTexto(Formulario, "VentasLblValorKpi5"));
                Assert.AreEqual("0", VentasFuncTexto(Formulario, "VentasLblValorKpi6"));
            }
        }

        [TestMethod]
        public void VentasMetActualizarIndicadores_ConOtraCultura_ConservaFormatoGuatemalteco()
        {
            // Arrange: simular una configuración regional que usa coma decimal.
            CultureInfo CulturaAnterior = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
                using (var Formulario = new FrmMdiVentas())
                {
                    // Act: el método debe utilizar es-GT, independientemente del equipo.
                    Formulario.VentasMetActualizarIndicadores(1234.5m, 2000.75m, 1000, 2, 30.25m, 4);

                    // Assert: comprobar separadores y símbolo de quetzales.
                    Assert.AreEqual("Q 1,234.50", VentasFuncTexto(Formulario, "VentasLblValorKpi1"));
                    Assert.AreEqual("Q 2,000.75", VentasFuncTexto(Formulario, "VentasLblValorKpi2"));
                    Assert.AreEqual("1,000", VentasFuncTexto(Formulario, "VentasLblValorKpi3"));
                    Assert.AreEqual("Q 30.25", VentasFuncTexto(Formulario, "VentasLblValorKpi5"));
                }
            }
            finally
            {
                CultureInfo.CurrentCulture = CulturaAnterior;
            }
        }

        private static string VentasFuncTexto(Form Formulario, string NombreControl)
        {
            return Formulario.Controls.Find(NombreControl, true)[0].Text;
        }
    }
}
