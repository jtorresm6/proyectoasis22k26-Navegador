/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 09/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Comprobar las reglas y el traslado del cierre.
 */
// Natali Sofía Montenegro Portillo | Carné 0901-23-10017 | Cierre de movimientos | 09/11/2026
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reporting.WinForms;
using CapaControlador_CierreMovimientos;
using CapaVista_CierreMovimientos;

namespace TestCierreMovimientos
{
    [TestClass]
    public class TestCierreMovimientos
    {
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(-3)]
        public void ImporteNumerico_ConValorValido_ConservaValor(int Valor)
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            decimal Resultado = Controlador.VentasFuncImporteCelda(Valor);
            //Assert
            Assert.AreEqual((decimal)Valor, Resultado);
        }

        [TestMethod]
        public void ValidarCaptura_Invalida_RechazaOperacion()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            Action Operacion = () => Controlador.VentasMetValidarCaptura(false, "Dato requerido");
            //Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ValidarCaptura_Valida_Continua()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            bool Aceptada = false;
            //Act
            Controlador.VentasMetValidarCaptura(true, "Dato requerido");
            Aceptada = true;
            //Assert
            Assert.IsTrue(Aceptada);
        }

        [DataTestMethod]
        [DataRow(2024, 2, 29)]
        [DataRow(2026, 2, 28)]
        [DataRow(2026, 12, 31)]
        public void PeriodoMensual_Calendario_CierraEnUltimoDia(int Anio, int Mes, int Dia)
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            var Periodo = Controlador.VentasFuncPeriodo(Anio, Mes, false);
            //Assert
            Assert.AreEqual(new DateTime(Anio, Mes, 1), Periodo[0]);
            Assert.AreEqual(new DateTime(Anio, Mes, Dia), Periodo[1]);
        }

        [TestMethod]
        public void PeriodoAnual_CubreAnioCompleto()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            var Periodo = Controlador.VentasFuncPeriodo(2026, 8, true);
            //Assert
            Assert.AreEqual(new DateTime(2026, 1, 1), Periodo[0]);
            Assert.AreEqual(new DateTime(2026, 12, 31), Periodo[1]);
        }

        [TestMethod]
        public void Traslado_SinEnlace_NoAccedeALaBase()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            Action Operacion = () => Controlador.VentasFuncTrasladar(1, 1, null);
            //Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void Poliza_SinPais_NoSeEnvia()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            var Partida = new DataTable();
            Partida.Columns.Add("Cuenta", typeof(string));
            Partida.Columns.Add("Debe", typeof(decimal));
            Partida.Columns.Add("Haber", typeof(decimal));
            Partida.Columns.Add("Descripcion", typeof(string));
            Partida.Rows.Add("1.01.04.001", 10m, 0m, "Prueba");

            //Act
            var Error = Assert.ThrowsException<InvalidOperationException>(() =>
                Controlador.VentasFuncRegistrarContabilidad("PRUEBA", 1, new DateTime(2026, 10, 31), Partida, 0));
            //Assert
            StringAssert.Contains(Error.Message, "país");
        }

        [TestMethod]
        public void CierreIndependiente_NoReferenciaDllContable()
        {
            //Arrange
            var Controlador = typeof(ClsCierreControlador).Assembly;
            //Act
            var Referencias = Controlador.GetReferencedAssemblies();
            var MetodoPaises = typeof(ClsCierreControlador).GetMethod("VentasFuncPaisesContables", Type.EmptyTypes);
            //Assert
            Assert.IsFalse(Array.Exists(Referencias,
                Referencia => Referencia.Name.IndexOf("Contabilidad", StringComparison.OrdinalIgnoreCase) >= 0));
            Assert.IsNotNull(MetodoPaises);
            Assert.AreEqual(typeof(DataTable), MetodoPaises.ReturnType);
        }

        [TestMethod]
        public void FormularioCierre_SeConstruyeSinContabilidad()
        {
            //Arrange
            Exception Error = null;
            var Hilo = new Thread(() =>
            {
                try
                {
                    using (var Formulario = new FrmCierreMovimientosVentas())
                        Assert.IsFalse(Formulario.IsDisposed);
                }
                catch (Exception Excepcion) { Error = Excepcion; }
            });
            Hilo.SetApartmentState(ApartmentState.STA);
            //Act
            Hilo.Start();
            //Assert
            Assert.IsTrue(Hilo.Join(TimeSpan.FromSeconds(10)), "El formulario tardó demasiado en abrirse.");
            if (Error != null)
                Assert.Fail("El formulario no pudo construirse: " + Error);
        }

        [DataTestMethod]
        [DataRow(false, "CLI-1", "12.50")]
        [DataRow(true, "1.01.04.001", "12.50")]
        public void ReporteCierre_PreparaSaldoYPartida(bool EsPartida, string Referencia, string Importe)
        {
            //Arrange
            var Datos = new DataTable();
            foreach (string Campo in new[] { "Codigo", "Nombre", "Anterior", "Cargos", "Abonos",
                "Actual", "Cuenta", "Descripcion", "Debe", "Haber" })
                Datos.Columns.Add(Campo);
            Datos.Rows.Add("CLI-1", "Cliente", 10m, 5m, 2.5m, 12.5m,
                "1.01.04.001", "Cuenta", 12.5m, 0m);

            var Reporte = new LocalReport();
            var Tipo = Assembly.Load("CapaVista_CierreMovimientos")
                .GetType("CapaVista_CierreMovimientos.FrmReporteCierreMovimientos", true);
            //Act
            Tipo.GetMethod("VentasMetPreparar", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[] { Reporte, Datos, "Cierre", new DateTime(2026, 10, 1),
                    new DateTime(2026, 10, 31), "GTQ", EsPartida });

            //Assert
            var Tabla = Reporte.DataSources[0].Value as DataTable;
            Assert.IsNotNull(Tabla);
            Assert.AreEqual(1, Tabla.Rows.Count);
            Assert.AreEqual(Referencia, Tabla.Rows[0]["Referencia"]);
            Assert.AreEqual(Importe, Tabla.Rows[0][EsPartida ? "Cargos" : "Actual"]);
        }

        [TestMethod]
        public void GuardarBorrador_PeriodoIncompleto_Rechaza()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            Action Operacion = () => Controlador.VentasFuncGuardarBorrador(0, 1, 0, "MENSUAL", new DateTime(2026, 10, 2), new DateTime(2026, 10, 31), "");
            //Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void Confirmar_SinRevision_RechazaSinEscribirEnBase()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            var Error = Assert.ThrowsException<InvalidOperationException>(() =>
                Controlador.VentasFuncConfirmar(1, 1, "MENSUAL", new DateTime(2026, 2, 1),
                    new DateTime(2026, 2, 28), false));
            //Assert
            StringAssert.Contains(Error.Message, "reviso");
        }

        [TestMethod]
        public void Revisar_PeriodoIncompleto_RechazaSinConsultarBase()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            var Error = Assert.ThrowsException<InvalidOperationException>(() =>
                Controlador.VentasFuncRevisar(1, "MENSUAL", new DateTime(2026, 2, 2),
                    new DateTime(2026, 2, 28)));
            bool EsDemo = Controlador.VentasFuncEsDocumentoDemo(null);
            //Assert
            Assert.IsNotNull(Error);
            Assert.IsFalse(EsDemo);
        }

        [TestMethod]
        public void Historial_FormatoInvalido_Rechaza()
        {
            //Arrange
            var Controlador = new ClsCierreControlador();
            //Act
            Action Operacion = () => Controlador.VentasFuncHistorial(1, 0, "octubre");
            //Assert
            Assert.ThrowsException<InvalidOperationException>(Operacion);
        }

        [TestMethod]
        public void ReporteCierre_ConSaldo_GeneraDocumentoPdf()
        {
            //Arrange
            Assembly Vista = Assembly.Load("CapaVista_CierreMovimientos");
            using (var Definicion = Vista.GetManifestResourceStream(
                "CapaVista_CierreMovimientos.Reportes.RpReporteCierreMovimientos.rdlc"))
            {
                Assert.IsNotNull(Definicion, "El reporte debe estar incluido en la vista de Cierre.");
                var Datos = new DataTable();
                foreach (string Columna in new[] { "Referencia", "Detalle", "Anterior", "Cargos", "Abonos", "Actual" })
                    Datos.Columns.Add(Columna, typeof(string));
                Datos.Rows.Add("CLI-1", "Cliente de prueba", "10.00", "5.00", "2.00", "13.00");

                var Reporte = new LocalReport();
                Reporte.LoadReportDefinition(Definicion);
                Reporte.DataSources.Add(new ReportDataSource("DsCierre", Datos));
                Reporte.SetParameters(new[]
                {
                    new ReportParameter("FechaDesde", "01/10/2026"),
                    new ReportParameter("FechaHasta", "31/10/2026"),
                    new ReportParameter("Estado", "Cierre de prueba | GTQ")
                });
                //Act
                byte[] Pdf = Reporte.Render("PDF");
                //Assert
                Assert.IsTrue(Pdf.Length > 1000, "El reporte debe contener una página imprimible.");
            }
        }
    }
}
