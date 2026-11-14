using System.Reflection;
using CapaControlador_CierreNominas;
using CapaControlador_CierreNominas.Objetos_de_valor;
using CapaModelo_CierreNominas.Contratos;
using CapaModelo_CierreNominas.Entidades;
using CapaModelo_CierreNominas.Repositorios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * ==================================================================
 * Área: Recursos Humanos
 * Autores: Lourdes Isabel Melendez Pineda
 * Fecha o ultima edicion: 02/11/2026
 * ==================================================================
 * Propósito : Pruebas unitarias del cierre de nómina. Cubren las reglas
 * del controlador (validaciones y verificación por empleado),
 * los objetos de valor, las entidades, el contrato y el
 * repositorio del Modelo.
 * No necesitan base de datos ni inicio de sesión.
 * ===================================================================
 */


namespace MSTestCierreNominas
{
    [TestClass]
    public class TestCierreNominas
    {
        // ---------- AYUDAS ----------

        private static ClsPrevioCierreNomina RrhhMetPrevioCorrecto()
        {
            return new ClsPrevioCierreNomina
            {
                IdNominaEncabezado = 1,
                IdEmpleado = 1,
                CodigoEmpleado = "EMP-001",
                NombreEmpleado = "Isabel Meléndez",
                SalarioBaseNominaEncabezado = 5000m,
                TotalPercepcionNominaEncabezado = 5250m,
                TotalDeduccionNominaEncabezado = 241.50m,
                TotalNetoNominaEncabezado = 5008.50m,
                DetallePercepcion = 5250m,
                DetalleDeduccion = 241.50m,
                ConceptosSinCuenta = 0
            };
        }

        private static string RrhhMetVerificar(ClsPrevioCierreNomina Item)
        {
            var Controlador = new ClsModeloCierreNomina();
            MethodInfo Metodo = typeof(ClsModeloCierreNomina).GetMethod("RrhhMetVerificarEmpleado",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)Metodo.Invoke(Controlador, new object[] { Item });
        }

        // ---------- CONTROLADOR: propiedades ----------

        [TestMethod]
        public void Propiedades_AlAsignarValores_LosConservan()
        {
            // Arrange
            var Controlador = new ClsModeloCierreNomina();

            // Act
            Controlador.PeriodoCierreNomina = "2026-10";
            Controlador.FechaInicioCierreNomina = new DateTime(2026, 10, 1);
            Controlador.FechaFinalCierreNomina = new DateTime(2026, 10, 31);

            // Assert
            Assert.AreEqual("2026-10", Controlador.PeriodoCierreNomina);
            Assert.AreEqual(new DateTime(2026, 10, 1), Controlador.FechaInicioCierreNomina);
            Assert.AreEqual(new DateTime(2026, 10, 31), Controlador.FechaFinalCierreNomina);
        }

        [TestMethod]
        public void Propiedades_DeSoloLectura_NacenSinValor()
        {
            // Arrange y Act
            var Controlador = new ClsModeloCierreNomina();

            // Assert
            Assert.AreEqual(0, Controlador.IdCierreNomina);
            Assert.AreEqual(0m, Controlador.TotalDebeCierreNomina);
            Assert.AreEqual(0m, Controlador.TotalHaberCierreNomina);
            Assert.IsNull(Controlador.CodigoPolizaCierreNomina);
            Assert.IsNull(Controlador.EstadoPolizaCierreNomina);
        }

        // ---------- CONTROLADOR: verificación por empleado ----------

        [TestMethod]
        public void RrhhMetVerificarEmpleado_ConDatosCorrectos_NoDevuelveProblema()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void RrhhMetVerificarEmpleado_ConceptoSinCuenta_AvisaQueFaltaCuenta()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();
            Item.ConceptosSinCuenta = 1;

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.AreEqual("Concepto sin cuenta contable", Resultado);
        }

        [TestMethod]
        public void RrhhMetVerificarEmpleado_NetoDistintoDePercepcionMenosDeduccion_AvisaNetoNoCuadra()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();
            Item.TotalNetoNominaEncabezado = 5000m;

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.AreEqual("Neto no cuadra", Resultado);
        }

        [TestMethod]
        public void RrhhMetVerificarEmpleado_DetalleDistintoDelTotalDePercepcion_AvisaDetalleNoCoincide()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();
            Item.DetallePercepcion = 5000m;

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.AreEqual("Detalle no coincide", Resultado);
        }

        [TestMethod]
        public void RrhhMetVerificarEmpleado_DetalleDistintoDelTotalDeDeduccion_AvisaDetalleNoCoincide()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();
            Item.DetalleDeduccion = 200m;

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.AreEqual("Detalle no coincide", Resultado);
        }

        [TestMethod]
        public void RrhhMetVerificarEmpleado_ConVariosProblemas_PriorizaLaCuentaFaltante()
        {
            // Arrange
            ClsPrevioCierreNomina Item = RrhhMetPrevioCorrecto();
            Item.ConceptosSinCuenta = 2;
            Item.TotalNetoNominaEncabezado = 1m;
            Item.DetallePercepcion = 1m;

            // Act
            string Resultado = RrhhMetVerificar(Item);

            // Assert
            Assert.AreEqual("Concepto sin cuenta contable", Resultado);
        }

        // ---------- OBJETOS DE VALOR ----------

        [TestMethod]
        public void ClsPeriodoNominaValor_Texto_MuestraMesYAnioConMayuscula()
        {
            // Arrange
            var Periodo = new ClsPeriodoNominaValor { FechaFinalNomina = new DateTime(2026, 10, 31) };

            // Act
            string Texto = Periodo.Texto;

            // Assert
            Assert.AreEqual("Octubre 2026", Texto);
        }

        [TestMethod]
        public void ClsPeriodoNominaValor_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Periodo = new ClsPeriodoNominaValor
            {
                PeriodoNomina = "2026-10",
                FechaInicioNomina = new DateTime(2026, 10, 1),
                FechaFinalNomina = new DateTime(2026, 10, 31),
                CantidadEmpleadoNomina = 3
            };

            // Assert
            Assert.AreEqual("2026-10", Periodo.PeriodoNomina);
            Assert.AreEqual(new DateTime(2026, 10, 1), Periodo.FechaInicioNomina);
            Assert.AreEqual(new DateTime(2026, 10, 31), Periodo.FechaFinalNomina);
            Assert.AreEqual(3, Periodo.CantidadEmpleadoNomina);
        }

        [TestMethod]
        public void ClsResumenCierreNominaValor_AlCrearse_NaceBloqueadoYConListaVacia()
        {
            // Arrange y Act
            var Resumen = new ClsResumenCierreNominaValor();

            // Assert
            Assert.IsNotNull(Resumen.Filas);
            Assert.AreEqual(0, Resumen.Filas.Count);
            Assert.IsFalse(Resumen.PuedeCerrar);
            Assert.IsFalse(Resumen.CuadreCorrecto);
        }

        [TestMethod]
        public void ClsFilaCierreNominaValor_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Fila = new ClsFilaCierreNominaValor
            {
                IdNominaEncabezado = 4,
                Codigo = "EMP-001",
                Empleado = "Isabel Meléndez",
                Percepciones = 5250m,
                Deducciones = 241.50m,
                Verificacion = "Correcto",
                Verificado = true
            };

            // Assert
            Assert.AreEqual(4, Fila.IdNominaEncabezado);
            Assert.AreEqual("EMP-001", Fila.Codigo);
            Assert.AreEqual("Isabel Meléndez", Fila.Empleado);
            Assert.AreEqual(5250m, Fila.Percepciones);
            Assert.AreEqual(241.50m, Fila.Deducciones);
            Assert.AreEqual("Correcto", Fila.Verificacion);
            Assert.IsTrue(Fila.Verificado);
        }

        [TestMethod]
        public void ClsConfiguracionCierreNomina_Valores_SonLosDelCierre()
        {
            // Assert
            Assert.AreEqual("MOD-RRHH", ClsConfiguracionCierreNomina.CodigoModuloContable);
            Assert.AreEqual(1, ClsConfiguracionCierreNomina.IdPais);
            Assert.AreEqual("NOM-", ClsConfiguracionCierreNomina.PrefijoDocumentoOrigen);
            Assert.AreEqual("2.01.02.001", ClsConfiguracionCierreNomina.CodigoCuentaSueldosPorPagar);
        }

        [TestMethod]
        public void ClsConfiguracionCierreNomina_DocumentoOrigen_NoSuperaLosTreintaCaracteres()
        {
            // Arrange
            string Documento = ClsConfiguracionCierreNomina.PrefijoDocumentoOrigen + "2026-10";

            // Assert: tblPolizaEncabezado.documentoOrigen es VARCHAR(30)
            Assert.IsTrue(Documento.Length <= 30);
            Assert.AreEqual("NOM-2026-10", Documento);
        }

        // ---------- ENTIDADES ----------

        [TestMethod]
        public void ClsCierreNomina_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Entidad = new ClsCierreNomina
            {
                IdCierreNomina = 1,
                PeriodoCierreNomina = "2026-10",
                FechaInicioCierreNomina = new DateTime(2026, 10, 1),
                FechaFinalCierreNomina = new DateTime(2026, 10, 31),
                CantidadEmpleadoCierreNomina = 3,
                TotalPercepcionCierreNomina = 17250m,
                TotalDeduccionCierreNomina = 946.95m,
                TotalNetoCierreNomina = 16303.05m,
                TotalPatronalCierreNomina = 0m,
                TotalDebeCierreNomina = 17250m,
                TotalHaberCierreNomina = 17250m,
                DocumentoOrigenCierreNomina = "NOM-2026-10",
                CodigoPolizaCierreNomina = "POL-2026-10-0005",
                FechaPolizaCierreNomina = new DateTime(2026, 10, 31),
                EstadoPolizaCierreNomina = "VALIDADA",
                IdUsuarioCierra = 1
            };

            // Assert
            Assert.AreEqual(1, Entidad.IdCierreNomina);
            Assert.AreEqual("2026-10", Entidad.PeriodoCierreNomina);
            Assert.AreEqual(3, Entidad.CantidadEmpleadoCierreNomina);
            Assert.AreEqual(17250m, Entidad.TotalPercepcionCierreNomina);
            Assert.AreEqual(946.95m, Entidad.TotalDeduccionCierreNomina);
            Assert.AreEqual(16303.05m, Entidad.TotalNetoCierreNomina);
            Assert.AreEqual(Entidad.TotalDebeCierreNomina, Entidad.TotalHaberCierreNomina);
            Assert.AreEqual("NOM-2026-10", Entidad.DocumentoOrigenCierreNomina);
            Assert.AreEqual("POL-2026-10-0005", Entidad.CodigoPolizaCierreNomina);
            Assert.AreEqual("VALIDADA", Entidad.EstadoPolizaCierreNomina);
            Assert.AreEqual(1, Entidad.IdUsuarioCierra);
        }

        [TestMethod]
        public void ClsCierreNominaEmpleado_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Entidad = new ClsCierreNominaEmpleado
            {
                IdCierreNominaEmpleado = 1,
                IdCierreNomina = 2,
                IdNominaEncabezado = 3,
                IdEmpleado = 4,
                CodigoEmpleado = "EMP-001",
                NombreEmpleado = "Isabel Meléndez",
                SalarioBaseCierreNominaEmpleado = 5000m,
                TotalPercepcionCierreNominaEmpleado = 5250m,
                TotalDeduccionCierreNominaEmpleado = 241.50m,
                TotalNetoCierreNominaEmpleado = 5008.50m,
                AcumuladoPercepcionAnioCierreNominaEmpleado = 5250m,
                AcumuladoDeduccionAnioCierreNominaEmpleado = 241.50m
            };

            // Assert
            Assert.AreEqual(2, Entidad.IdCierreNomina);
            Assert.AreEqual(3, Entidad.IdNominaEncabezado);
            Assert.AreEqual(4, Entidad.IdEmpleado);
            Assert.AreEqual("EMP-001", Entidad.CodigoEmpleado);
            Assert.AreEqual(5000m, Entidad.SalarioBaseCierreNominaEmpleado);
            Assert.AreEqual(Entidad.TotalPercepcionCierreNominaEmpleado - Entidad.TotalDeduccionCierreNominaEmpleado,
                Entidad.TotalNetoCierreNominaEmpleado);
            Assert.AreEqual(5250m, Entidad.AcumuladoPercepcionAnioCierreNominaEmpleado);
            Assert.AreEqual(241.50m, Entidad.AcumuladoDeduccionAnioCierreNominaEmpleado);
        }

        [TestMethod]
        public void ClsNominaAbiertaYLineaPoliza_AlAsignarValores_LosConservan()
        {
            // Arrange y Act
            var Nomina = new ClsNominaAbierta
            {
                PeriodoNomina = "2026-10",
                FechaInicioNomina = new DateTime(2026, 10, 1),
                FechaFinalNomina = new DateTime(2026, 10, 31),
                CantidadEmpleadoNomina = 3
            };
            var Linea = new ClsLineaPolizaNomina
            {
                IdCuenta = 207,
                CodigoCuenta = "5.04.01.001",
                NombreCuenta = "Sueldos Normales",
                EsPercepcion = true,
                MontoTotal = 16500m
            };

            // Assert
            Assert.AreEqual("2026-10", Nomina.PeriodoNomina);
            Assert.AreEqual(3, Nomina.CantidadEmpleadoNomina);
            Assert.AreEqual(207, Linea.IdCuenta);
            Assert.AreEqual("5.04.01.001", Linea.CodigoCuenta);
            Assert.IsTrue(Linea.EsPercepcion);
            Assert.AreEqual(16500m, Linea.MontoTotal);
        }

        // ---------- CONTRATO ----------

        [TestMethod]
        public void IRepositorioCierreNomina_LaImplementaElRepositorio()
        {
            // Assert
            Assert.IsTrue(typeof(IRepositorioCierreNomina).IsAssignableFrom(typeof(ClsRepositorioCierreNomina)));
        }

        [DataTestMethod]
        [DataRow("RrhhMetObtenerNominasAbiertas")]
        [DataRow("RrhhMetObtenerPrevio")]
        [DataRow("RrhhMetObtenerLineasPoliza")]
        [DataRow("RrhhMetObtenerDetalleCierre")]
        [DataRow("RrhhMetNominaCerrada")]
        public void IRepositorioCierreNomina_TieneLasConsultasPropiasDelCierre(string NombreMetodo)
        {
            // Act
            MethodInfo Metodo = typeof(IRepositorioCierreNomina).GetMethod(NombreMetodo);

            // Assert
            Assert.IsNotNull(Metodo);
        }

        // ---------- REPOSITORIO ----------

        [TestMethod]
        public void RrhhMetEditar_ElCierreEsIrreversible_LanzaNotSupportedException()
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();

            // Act y Assert
            NotSupportedException Excepcion = Assert.ThrowsException<NotSupportedException>(
                () => Repositorio.RrhhMetEditar(new ClsCierreNomina()));
            Assert.AreEqual("El cierre de nómina es irreversible y no se puede editar.", Excepcion.Message);
        }

        [TestMethod]
        public void RrhhMetRemover_ElCierreEsIrreversible_LanzaNotSupportedException()
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();

            // Act y Assert
            NotSupportedException Excepcion = Assert.ThrowsException<NotSupportedException>(
                () => Repositorio.RrhhMetRemover(new ClsCierreNomina()));
            Assert.AreEqual("El cierre de nómina es irreversible y no se puede eliminar.", Excepcion.Message);
        }

        [DataTestMethod]
        [DataRow("_Insert", 15)]
        [DataRow("_InsertEmpleado", 9)]
        [DataRow("_SelectEmpleadosPorCerrar", 4)]
        [DataRow("_SelectPrevio", 2)]
        [DataRow("_SelectLineasPoliza", 2)]
        [DataRow("_SelectDetalleCierre", 1)]
        [DataRow("_SelectNominaCerrada", 1)]
        [DataRow("_SelectNominasAbiertas", 0)]
        [DataRow("_SelectAll", 0)]
        public void Sql_CantidadDeParametros_CoincideConLosQueSeEnvian(string NombreCampo, int Esperados)
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();
            FieldInfo Campo = typeof(ClsRepositorioCierreNomina).GetField(NombreCampo,
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            string Sql = (string)Campo.GetValue(Repositorio);

            // Assert
            Assert.AreEqual(Esperados, Sql.Count(c => c == '?'));
        }

        [DataTestMethod]
        [DataRow("_SelectNominasAbiertas")]
        [DataRow("_SelectPrevio")]
        [DataRow("_SelectLineasPoliza")]
        [DataRow("_SelectEmpleadosPorCerrar")]
        public void Sql_NominasCerrables_SonLasAprobadasYPagadas(string NombreCampo)
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();
            FieldInfo Campo = typeof(ClsRepositorioCierreNomina).GetField(NombreCampo,
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            string Sql = (string)Campo.GetValue(Repositorio);

            // Assert
            StringAssert.Contains(Sql, "IN ('Aprobada', 'Pagada')");
            StringAssert.Contains(Sql, "NOT EXISTS");
        }

        [DataTestMethod]
        [DataRow("_SelectPrevio")]
        [DataRow("_SelectLineasPoliza")]
        [DataRow("_SelectEmpleadosPorCerrar")]
        public void Sql_FiltroDePeriodo_EsPorRangoDelMes(string NombreCampo)
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();
            FieldInfo Campo = typeof(ClsRepositorioCierreNomina).GetField(NombreCampo,
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            string Sql = (string)Campo.GetValue(Repositorio);

            // Assert
            StringAssert.Contains(Sql, "ne.fechaFinalNominaEncabezado >= ?");
            StringAssert.Contains(Sql, "ne.fechaFinalNominaEncabezado <= ?");
        }

        [TestMethod]
        public void Sql_NominasAbiertas_AgrupaPorMes()
        {
            // Arrange
            var Repositorio = new ClsRepositorioCierreNomina();
            FieldInfo Campo = typeof(ClsRepositorioCierreNomina).GetField("_SelectNominasAbiertas",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            string Sql = (string)Campo.GetValue(Repositorio);

            // Assert
            StringAssert.Contains(Sql, "GROUP BY YEAR(ne.fechaFinalNominaEncabezado), MONTH(ne.fechaFinalNominaEncabezado)");
        }
    }
}