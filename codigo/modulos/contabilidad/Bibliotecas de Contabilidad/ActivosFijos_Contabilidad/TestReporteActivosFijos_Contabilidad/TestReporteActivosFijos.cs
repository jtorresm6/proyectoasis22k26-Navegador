/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Dylan Hernández
 * Carné : 0901-23-519
 * Fecha : 11/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas MSTest del reporte de activos fijos: filtros y fecha de
 *  corte, depreciación acumulada y valor en libros, consulta contra la
 *  base, enlace con el MDI y carga del RDLC.
 * Reglas especificas:
 *  Cada prueba sigue Arrange, Act y Assert. Las de la clase
 *  TestConsultaReporteActivosFijos y la creación de la ventana leen la
 *  base local (DSN EmbutidosS.A) y no escriben nada.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Contabilidad;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaVista_Contabilidad;

namespace TestReporteActivosFijos
{
    // Repositorio falso del reporte
    internal class ClsRepositorioReporteFalso : IRepositorioReporteActivosFijosContabilidad
    {
        // Activos que devuelve
        public List<ClsActivoReporteContabilidad> Activos = new List<ClsActivoReporteContabilidad>();

        // Veces consultado
        public int Consultas;

        // Último módulo pedido
        public string ModuloPedido = null!;

        // Último tipo pedido
        public string TipoPedido = null!;

        // Devuelve los activos
        public List<ClsActivoReporteContabilidad> ContabilidadFuncObtenerActivosAlCorte(DateTime Corte, string Modulo, string Tipo)
        {
            Consultas++;
            ModuloPedido = Modulo;
            TipoPedido = Tipo;
            return Activos;
        }
    }

    // Datos de ejemplo
    internal static class ClsDatosEjemploReporte
    {
        // Crea un activo
        public static ClsActivoReporteContabilidad ContabilidadFuncActivo(string Codigo, string Modulo, string Tipo, string NombreTipo,
            DateTime Adquisicion, DateTime Inicio, decimal Compra, decimal Residual, int Meses)
        {
            return new ClsActivoReporteContabilidad
            {
                Codigo = Codigo, Descripcion = "Activo " + Codigo, Modulo = Modulo, Tipo = Tipo, NombreTipo = NombreTipo,
                Adquisicion = Adquisicion, Inicio = Inicio, Compra = Compra, Residual = Residual, ValorEnLibros = Compra, Meses = Meses
            };
        }

        // Activos variados
        public static List<ClsActivoReporteContabilidad> ContabilidadFuncActivos()
        {
            return new List<ClsActivoReporteContabilidad>
            {
                ContabilidadFuncActivo("AF-0003", "Ventas", "VEHICULO", "Equipo de transporte", new DateTime(2025, 1, 10), new DateTime(2025, 1, 15), 120000m, 20000m, 60),
                ContabilidadFuncActivo("AF-0001", "Contabilidad", "EQ_COMPUTO", "Equipo electronico de computo", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1), 5000m, 500m, 36),
                ContabilidadFuncActivo("AF-0002", "Contabilidad", "MOBILIARIO", "Mobiliario y equipo de oficina", new DateTime(2026, 3, 2), new DateTime(2026, 3, 20), 9000m, 0m, 60),
                ContabilidadFuncActivo("AF-0004", "Logística", "MAQUINARIA", "Maquinaria y equipo de planta", new DateTime(2023, 6, 1), new DateTime(2023, 7, 1), 250000m, 25000m, 120),
                ContabilidadFuncActivo("AF-0005", "Ventas", "EQ_COMPUTO", "Equipo electronico de computo", new DateTime(2026, 10, 1), new DateTime(2026, 12, 1), 7200m, 0m, 36)
            };
        }
    }

    // Pruebas de filtros y fecha de corte
    [TestClass]
    public class TestFiltrosReporteActivosFijos
    {
        // Corte futuro
        [TestMethod]
        public void Test_ValidarFiltros_CorteFuturo_Error()
        {
            // Arrange
            DateTime Corte = DateTime.Today.AddDays(1);

            // Act
            string Mensaje = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncValidarFiltros(Corte,
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo);

            // Assert
            Assert.AreEqual("La fecha de corte no puede ser posterior a hoy.", Mensaje);
        }

        // Filtros válidos
        [TestMethod]
        public void Test_ValidarFiltros_CorteHoyYAgrupacionValida_SinError()
        {
            // Arrange
            DateTime Corte = DateTime.Today;

            // Act
            string PorModulo = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncValidarFiltros(Corte,
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo);
            string PorVidaUtil = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncValidarFiltros(Corte,
                ClsControladorReporteActivosFijosContabilidad.AgruparVidaUtil);

            // Assert
            Assert.AreEqual(string.Empty, PorModulo);
            Assert.AreEqual(string.Empty, PorVidaUtil);
        }

        // Corte muy antiguo
        [TestMethod]
        public void Test_ValidarFiltros_CorteMuyAntiguo_Error()
        {
            // Arrange
            DateTime Corte = new DateTime(1999, 12, 31);

            // Act
            string Mensaje = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncValidarFiltros(Corte,
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo);

            // Assert
            Assert.AreNotEqual(string.Empty, Mensaje);
        }

        // Agrupación inválida
        [TestMethod]
        public void Test_ValidarFiltros_AgrupacionInvalida_Error()
        {
            // Arrange
            DateTime Corte = DateTime.Today;

            // Act
            string Mensaje = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncValidarFiltros(Corte, "Color");

            // Assert
            Assert.AreEqual("Elija si el reporte se agrupa por módulo o por vida útil.", Mensaje);
        }

        // Filtro inválido sin consulta
        [TestMethod]
        public void Test_DatosReporte_FiltroInvalido_NoConsultaLaBase()
        {
            // Arrange
            ClsRepositorioReporteFalso Repositorio = new ClsRepositorioReporteFalso();
            ClsControladorReporteActivosFijosContabilidad Controlador = new ClsControladorReporteActivosFijosContabilidad(Repositorio);
            string Mensaje;

            // Act
            DataTable Datos = Controlador.ContabilidadFuncDatosReporte(DateTime.Today.AddMonths(1), ClsControladorReporteActivosFijosContabilidad.Todos,
                string.Empty, ClsControladorReporteActivosFijosContabilidad.AgruparModulo, out Mensaje);

            // Assert
            Assert.IsNull(Datos);
            Assert.AreEqual(0, Repositorio.Consultas);
            Assert.AreNotEqual(string.Empty, Mensaje);
        }

        // Opción Todos
        [TestMethod]
        public void Test_DatosReporte_FiltroTodos_ConsultaSinFiltros()
        {
            // Arrange
            ClsRepositorioReporteFalso Repositorio = new ClsRepositorioReporteFalso();
            ClsControladorReporteActivosFijosContabilidad Controlador = new ClsControladorReporteActivosFijosContabilidad(Repositorio);
            string Mensaje;

            // Act
            using (DataTable Datos = Controlador.ContabilidadFuncDatosReporte(DateTime.Today, ClsControladorReporteActivosFijosContabilidad.Todos,
                null!, ClsControladorReporteActivosFijosContabilidad.AgruparModulo, out Mensaje))
            {
                // Assert
                Assert.IsNotNull(Datos);
                Assert.AreEqual(string.Empty, Mensaje);
                Assert.AreEqual(string.Empty, Repositorio.ModuloPedido);
                Assert.AreEqual(string.Empty, Repositorio.TipoPedido);
            }
        }

        // Filtros al repositorio
        [TestMethod]
        public void Test_DatosReporte_FiltroModuloYTipo_SeEnvianAlRepositorio()
        {
            // Arrange
            ClsRepositorioReporteFalso Repositorio = new ClsRepositorioReporteFalso();
            ClsControladorReporteActivosFijosContabilidad Controlador = new ClsControladorReporteActivosFijosContabilidad(Repositorio);
            string Mensaje;

            // Act
            using (DataTable Datos = Controlador.ContabilidadFuncDatosReporte(DateTime.Today, " Ventas ", "VEHICULO",
                ClsControladorReporteActivosFijosContabilidad.AgruparVidaUtil, out Mensaje))
            {
                // Assert
                Assert.IsNotNull(Datos);
                Assert.AreEqual("Ventas", Repositorio.ModuloPedido);
                Assert.AreEqual("VEHICULO", Repositorio.TipoPedido);
            }
        }

        // Adquirido después del corte
        [TestMethod]
        public void Test_ArmarTabla_AdquiridoDespuesDelCorte_NoSale()
        {
            // Arrange
            List<ClsActivoReporteContabilidad> Activos = ClsDatosEjemploReporte.ContabilidadFuncActivos();
            DateTime Corte = new DateTime(2025, 12, 31);

            // Act
            using (DataTable Tabla = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(Activos, Corte,
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo))
            {
                // Assert
                CollectionAssert.AreEquivalent(new[] { "AF-0001", "AF-0003", "AF-0004" },
                    Tabla.Rows.Cast<DataRow>().Select(Fila => (string)Fila["Codigo"]).ToArray());
            }
        }

        // Orden por vida útil
        [TestMethod]
        public void Test_ArmarTabla_AgrupaPorVidaUtil_OrdenadoPorGrupoYCodigo()
        {
            // Arrange
            List<ClsActivoReporteContabilidad> Activos = ClsDatosEjemploReporte.ContabilidadFuncActivos();

            // Act
            using (DataTable Tabla = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(Activos, new DateTime(2026, 10, 31),
                ClsControladorReporteActivosFijosContabilidad.AgruparVidaUtil))
            {
                // Assert
                string[] Grupos = Tabla.Rows.Cast<DataRow>().Select(Fila => (string)Fila["Grupo"]).ToArray();
                CollectionAssert.AreEqual(Grupos.OrderBy(Grupo => Grupo).ToArray(), Grupos);
                Assert.AreEqual("Equipo de transporte", Grupos[0]);
                Assert.AreEqual("Equipo electronico de computo", Grupos[1]);
                Assert.AreEqual("AF-0001", Tabla.Rows[1]["Codigo"]);
                Assert.AreEqual("AF-0005", Tabla.Rows[2]["Codigo"]);
            }
        }
    }

    // Pruebas de depreciación al corte
    [TestClass]
    public class TestDepreciacionReporteActivosFijos
    {
        // Doce cuotas
        [TestMethod]
        public void Test_DepreciacionAlCorte_DoceMeses_DoceCuotas()
        {
            // Arrange
            DateTime Inicio = new DateTime(2025, 1, 15);
            DateTime Corte = new DateTime(2025, 12, 31);
            int Meses;

            // Act
            decimal Acumulada = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncDepreciacionAlCorte(3600m, 0m, 36, Inicio, Corte, out Meses);

            // Assert
            Assert.AreEqual(12, Meses);
            Assert.AreEqual(1200m, Acumulada);
        }

        // Mes de inicio y de corte
        [TestMethod]
        public void Test_DepreciacionAlCorte_IncluyeMesDeInicioYMesDeCorte()
        {
            // Arrange
            DateTime Inicio = new DateTime(2026, 3, 20);
            DateTime Corte = new DateTime(2026, 4, 1);
            int Meses;

            // Act
            decimal Acumulada = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncDepreciacionAlCorte(6000m, 0m, 60, Inicio, Corte, out Meses);

            // Assert
            Assert.AreEqual(2, Meses);
            Assert.AreEqual(200m, Acumulada);
        }

        // Corte antes del inicio
        [TestMethod]
        public void Test_DepreciacionAlCorte_CorteAntesDelInicio_Cero()
        {
            // Arrange
            DateTime Inicio = new DateTime(2026, 3, 20);
            DateTime Corte = new DateTime(2026, 3, 5);
            int Meses;

            // Act
            decimal Acumulada = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncDepreciacionAlCorte(6000m, 0m, 60, Inicio, Corte, out Meses);

            // Assert
            Assert.AreEqual(0, Meses);
            Assert.AreEqual(0m, Acumulada);
        }

        // Cuota redondeada
        [TestMethod]
        public void Test_DepreciacionAlCorte_CuotaRedondeada_NoPasaDelValorDepreciable()
        {
            // Arrange
            DateTime Inicio = new DateTime(2024, 1, 1);
            DateTime Corte = new DateTime(2026, 1, 31);
            int Meses;

            // Act
            decimal Acumulada = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncDepreciacionAlCorte(1000m, 0m, 3, Inicio, Corte, out Meses);

            // Assert
            Assert.AreEqual(1000m, Acumulada);
            Assert.AreEqual(4, Meses);
        }

        // Misma cuota del módulo
        [TestMethod]
        public void Test_DepreciacionAlCorte_CoincideConCuotaDelControladorDelModulo()
        {
            // Arrange
            DateTime Inicio = new DateTime(2025, 6, 10);
            decimal Libros = 15000m;
            for (DateTime Mes = new DateTime(2025, 6, 1); Mes <= new DateTime(2026, 2, 1); Mes = Mes.AddMonths(1))
                Libros -= ClsControladorActivosFijosContabilidad.ContabilidadFuncCuotaMensual(15000m, 1500m, 48, Libros, Inicio, Mes);
            int Meses;

            // Act
            decimal Acumulada = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncDepreciacionAlCorte(15000m, 1500m, 48, Inicio,
                new DateTime(2026, 2, 28), out Meses);

            // Assert
            Assert.AreEqual(15000m - Libros, Acumulada);
            Assert.AreEqual(9, Meses);
        }

        // Activo depreciado
        [TestMethod]
        public void Test_ArmarTabla_TotalmenteDepreciado_LibrosIgualResidualYSeñal()
        {
            // Arrange
            List<ClsActivoReporteContabilidad> Activos = new List<ClsActivoReporteContabilidad>
            {
                ClsDatosEjemploReporte.ContabilidadFuncActivo("AF-0001", "Contabilidad", "EQ_COMPUTO", "Equipo electronico de computo",
                    new DateTime(2020, 1, 1), new DateTime(2020, 1, 1), 5000m, 500m, 36)
            };

            // Act
            using (DataTable Tabla = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(Activos, new DateTime(2026, 10, 31),
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo))
            {
                // Assert
                DataRow Fila = Tabla.Rows[0];
                Assert.AreEqual(4500m, Fila["Acumulada"]);
                Assert.AreEqual(500m, Fila["Libros"]);
                Assert.AreEqual(36, Fila["MesesDepreciados"]);
                Assert.AreEqual(true, Fila["Depreciado"]);
                Assert.AreEqual("Depreciado", Fila["Estado"]);
                Assert.AreEqual(0m, Fila["Cuota"]);
            }
        }

        // Activo en uso
        [TestMethod]
        public void Test_ArmarTabla_EnUso_LibrosYEstadoConMeses()
        {
            // Arrange
            List<ClsActivoReporteContabilidad> Activos = new List<ClsActivoReporteContabilidad>
            {
                ClsDatosEjemploReporte.ContabilidadFuncActivo("AF-0003", "Ventas", "VEHICULO", "Equipo de transporte",
                    new DateTime(2025, 1, 10), new DateTime(2025, 1, 15), 120000m, 20000m, 60)
            };

            // Act
            using (DataTable Tabla = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(Activos, new DateTime(2025, 12, 31),
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo))
            {
                // Assert
                DataRow Fila = Tabla.Rows[0];
                Assert.AreEqual(1666.67m, Fila["Cuota"]);
                Assert.AreEqual(20000.04m, Fila["Acumulada"]);
                Assert.AreEqual(99999.96m, Fila["Libros"]);
                Assert.AreEqual("En uso (12/60)", Fila["Estado"]);
                Assert.AreEqual(false, Fila["Depreciado"]);
            }
        }

        // Activo sin iniciar
        [TestMethod]
        public void Test_ArmarTabla_InicioDespuesDelCorte_SinIniciar()
        {
            // Arrange
            List<ClsActivoReporteContabilidad> Activos = new List<ClsActivoReporteContabilidad>
            {
                ClsDatosEjemploReporte.ContabilidadFuncActivo("AF-0005", "Ventas", "EQ_COMPUTO", "Equipo electronico de computo",
                    new DateTime(2026, 10, 1), new DateTime(2026, 12, 1), 7200m, 0m, 36)
            };

            // Act
            using (DataTable Tabla = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(Activos, new DateTime(2026, 10, 31),
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo))
            {
                // Assert
                DataRow Fila = Tabla.Rows[0];
                Assert.AreEqual("Sin iniciar", Fila["Estado"]);
                Assert.AreEqual(0m, Fila["Acumulada"]);
                Assert.AreEqual(7200m, Fila["Libros"]);
                Assert.AreEqual(200m, Fila["Cuota"]);
            }
        }
    }

    // Pruebas de la consulta contra la base
    [TestClass]
    public class TestConsultaReporteActivosFijos
    {
        // Controlador con la base real
        private ClsControladorReporteActivosFijosContabilidad _Controlador = null!;

        // Crea el controlador
        [TestInitialize]
        public void ContabilidadMetIniciar()
        {
            _Controlador = new ClsControladorReporteActivosFijosContabilidad();
        }

        // Mismos activos que la pantalla
        [TestMethod]
        public void Test_ConsultaBase_Todos_MismosActivosQueLaPantallaDelModulo()
        {
            // Arrange
            string[] Esperados;
            using (DataTable Activos = new ClsControladorActivosFijosContabilidad().ContabilidadFuncActivos())
                Esperados = Activos.Rows.Cast<DataRow>().Where(Fila => (DateTime)Fila["Adquisicion"] <= DateTime.Today)
                    .Select(Fila => (string)Fila["Codigo"]).ToArray();
            string Mensaje;

            // Act
            using (DataTable Datos = _Controlador.ContabilidadFuncDatosReporte(DateTime.Today, ClsControladorReporteActivosFijosContabilidad.Todos,
                string.Empty, ClsControladorReporteActivosFijosContabilidad.AgruparModulo, out Mensaje))
            {
                // Assert
                Assert.AreEqual(string.Empty, Mensaje);
                Assert.IsNotNull(Datos);
                CollectionAssert.AreEquivalent(Esperados, Datos.Rows.Cast<DataRow>().Select(Fila => (string)Fila["Codigo"]).ToArray());
            }
        }

        // Valores cuadran
        [TestMethod]
        public void Test_ConsultaBase_ValoresDelReporte_Cuadran()
        {
            // Arrange
            string Mensaje;

            // Act
            using (DataTable Datos = _Controlador.ContabilidadFuncDatosReporte(DateTime.Today, ClsControladorReporteActivosFijosContabilidad.Todos,
                string.Empty, ClsControladorReporteActivosFijosContabilidad.AgruparVidaUtil, out Mensaje))
            {
                // Assert
                Assert.IsNotNull(Datos, Mensaje);
                foreach (DataRow Fila in Datos.Rows)
                {
                    Assert.AreEqual((decimal)Fila["Compra"], (decimal)Fila["Acumulada"] + (decimal)Fila["Libros"]);
                    Assert.IsTrue((decimal)Fila["Libros"] >= (decimal)Fila["Residual"]);
                    Assert.IsTrue((DateTime)Fila["Adquisicion"] <= DateTime.Today);
                }
            }
        }

        // Filtro por módulo
        [TestMethod]
        public void Test_ConsultaBase_FiltroPorModulo_SoloEseModulo()
        {
            // Arrange
            string[] Modulos = _Controlador.ContabilidadFuncModulos();
            string Modulo = Modulos[1];
            string Mensaje;
            using (DataTable Todos = _Controlador.ContabilidadFuncDatosReporte(DateTime.Today, ClsControladorReporteActivosFijosContabilidad.Todos,
                string.Empty, ClsControladorReporteActivosFijosContabilidad.AgruparModulo, out Mensaje))
                if (Todos != null && Todos.Rows.Count > 0) Modulo = (string)Todos.Rows[0]["Modulo"];

            // Act
            using (DataTable Datos = _Controlador.ContabilidadFuncDatosReporte(DateTime.Today, Modulo, string.Empty,
                ClsControladorReporteActivosFijosContabilidad.AgruparModulo, out Mensaje))
            {
                // Assert
                Assert.AreEqual(ClsControladorReporteActivosFijosContabilidad.Todos, Modulos[0]);
                Assert.IsNotNull(Datos, Mensaje);
                Assert.IsTrue(Datos.Rows.Cast<DataRow>().All(Fila => (string)Fila["Modulo"] == Modulo));
            }
        }

        // Catálogo de tipos
        [TestMethod]
        public void Test_ConsultaBase_CatalogoDeTipos_EmpiezaConTodos()
        {
            // Arrange y Act
            using (DataTable Tipos = _Controlador.ContabilidadFuncTipos())
            {
                // Assert
                Assert.IsTrue(Tipos.Rows.Count > 1);
                Assert.AreEqual(ClsControladorReporteActivosFijosContabilidad.Todos, Tipos.Rows[0]["Nombre"]);
                Assert.AreEqual(string.Empty, Tipos.Rows[0]["Tipo"]);
            }
        }
    }

    // Pruebas de la vista y del RDLC
    [TestClass]
    public class TestVistaReporteActivosFijos
    {
        // Ejecuta en un hilo STA
        private static void ContabilidadMetEnSta(Action Accion)
        {
            Exception? Error = null;
            Thread Hilo = new Thread(() =>
            {
                try { Accion(); }
                catch (Exception Excepcion) { Error = Excepcion; }
            });
            Hilo.SetApartmentState(ApartmentState.STA);
            Hilo.Start();
            Hilo.Join();
            if (Error != null) throw new AssertFailedException(Error.ToString());
        }

        // Busca el visor del reporte
        private static ReportViewer ContabilidadFuncVisor(Form Formulario)
        {
            return (ReportViewer)Formulario.Controls.Find("ContabilidadRpvReporte", true).Single();
        }

        // Ventana para el MDI
        [TestMethod]
        public void Test_CrearReporte_YaNoDevuelveNull()
        {
            ContabilidadMetEnSta(() =>
            {
                // Arrange
                ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;

                // Act
                using (Form Formulario = ClsReporteActivosFijosContabilidad.ContabilidadFuncCrearReporte(Permisos))
                {
                    // Assert
                    Assert.IsNotNull(Formulario);
                    Assert.IsInstanceOfType(Formulario, typeof(FrmReporteActivosFijos));
                    Assert.AreEqual("10003 - ReporteActivosFijos", Formulario.Text);
                    Assert.IsTrue(ContabilidadFuncVisor(Formulario).ShowPrintButton);
                    Assert.IsTrue(ContabilidadFuncVisor(Formulario).ShowExportButton);
                }
            });
        }

        // Sin permiso de imprimir
        [TestMethod]
        public void Test_CrearReporte_SinPermisoDeImprimir_OcultaImprimirYExportar()
        {
            ContabilidadMetEnSta(() =>
            {
                // Arrange
                ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
                Permisos.PuedeImprimir = false;

                // Act
                using (Form Formulario = ClsReporteActivosFijosContabilidad.ContabilidadFuncCrearReporte(Permisos))
                {
                    // Assert
                    Assert.IsFalse(ContabilidadFuncVisor(Formulario).ShowPrintButton);
                    Assert.IsFalse(ContabilidadFuncVisor(Formulario).ShowExportButton);
                }
            });
        }

        // RDLC embebido
        [TestMethod]
        public void Test_Rdlc_EmbebidoConNombreUnico()
        {
            // Arrange
            System.Reflection.Assembly Ensamblado = typeof(FrmReporteActivosFijos).Assembly;

            // Act
            string[] Recursos = Ensamblado.GetManifestResourceNames();

            // Assert
            CollectionAssert.Contains(Recursos, FrmReporteActivosFijos.RecursoReporte);
            Assert.AreEqual("CapaVista_Contabilidad.Reportes.RpReporteActivosFijos.rdlc", FrmReporteActivosFijos.RecursoReporte);
        }

        // PDF con datos
        [TestMethod]
        public void Test_Rdlc_ConDatosDeEjemplo_RenderizaPdf()
        {
            // Arrange
            DateTime Corte = new DateTime(2026, 10, 31);
            LocalReport Reporte = new LocalReport();
            using (DataTable Datos = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncArmarTabla(ClsDatosEjemploReporte.ContabilidadFuncActivos(),
                Corte, ClsControladorReporteActivosFijosContabilidad.AgruparModulo))
            {
                FrmReporteActivosFijos.ContabilidadMetPrepararReporte(Reporte, Datos, FrmReporteActivosFijos.ContabilidadFuncParametros(Corte,
                    ClsControladorReporteActivosFijosContabilidad.Todos, ClsControladorReporteActivosFijosContabilidad.Todos,
                    ClsControladorReporteActivosFijosContabilidad.AgruparModulo));

                // Act
                byte[] Pdf = Reporte.Render("PDF");

                // Assert
                Assert.IsTrue(Pdf.Length > 1000);
                Assert.AreEqual("%PDF", System.Text.Encoding.ASCII.GetString(Pdf, 0, 4));
            }
        }

        // PDF sin filas
        [TestMethod]
        public void Test_Rdlc_SinFilas_RenderizaPdf()
        {
            // Arrange
            DateTime Corte = DateTime.Today;
            LocalReport Reporte = new LocalReport();
            using (DataTable Datos = ClsControladorReporteActivosFijosContabilidad.ContabilidadFuncCrearTabla())
            {
                FrmReporteActivosFijos.ContabilidadMetPrepararReporte(Reporte, Datos, FrmReporteActivosFijos.ContabilidadFuncParametros(Corte,
                    "Ventas", ClsControladorReporteActivosFijosContabilidad.Todos, ClsControladorReporteActivosFijosContabilidad.AgruparVidaUtil));

                // Act
                byte[] Pdf = Reporte.Render("PDF");

                // Assert
                Assert.AreEqual("%PDF", System.Text.Encoding.ASCII.GetString(Pdf, 0, 4));
            }
        }
    }
}
