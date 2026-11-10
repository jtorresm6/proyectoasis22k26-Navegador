/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Mario Taracena
 * Carné : 0901-23-9335
 * Fecha : 30/10/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias del apartado de Presupuestos sobre la Capa
 *  Modelo (entidad y repositorio) y la Capa Vista (las dos ventanas
 *  del mantenimiento).
 * Reglas especificas:
 *  Cada prueba sigue la triple A: Arrange inicializa las variables,
 *  Act ejecuta el método probado y Assert comprueba el resultado.
 *  Las pruebas usan el DSN EmbutidosS.A, pero ninguna escribe datos:
 *  solo consultan. Las dos que llaman a editar y eliminar lo hacen
 *  con un identificador que no existe, así afectan cero filas.
 *  Las ventanas se construyen en un hilo STA, como lo exige Windows
 *  Forms, y se liberan al terminar cada prueba.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows.Forms;
using CapaControlador_Contabilidad;
using CapaModelo_Contabilidad.Contratos;
using CapaModelo_Contabilidad.Entidades;
using CapaModelo_Contabilidad.Repositorios;
using CapaVista_Contabilidad;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas_Contabilidad
{
    [TestClass]
    public class TestPresupuestosContabilidad
    {
        private ClsRepositorioPresupuestosContabilidad _Repositorio;

        [ClassInitialize]
        public static void ContabilidadMetIniciarClase(TestContext Contexto)
        {
            // Si no hay conexión, las pruebas fallan aquí con un mensaje claro y no al abrir una ventana.
            try
            {
                new ClsRepositorioPresupuestosContabilidad().ContabilidadFuncObtenerModulos();
            }
            catch (Exception Excepcion)
            {
                throw new InvalidOperationException(
                    "No hay conexión con el DSN EmbutidosS.A: " + Excepcion.Message, Excepcion);
            }
        }

        [TestInitialize]
        public void ContabilidadMetIniciar()
        {
            _Repositorio = new ClsRepositorioPresupuestosContabilidad();
        }

        //  Apoyos de las pruebas
        // ==================================================================

        // Construye la ventana en un hilo STA, ejecuta las comprobaciones y la libera.
        private static void ContabilidadMetEnVentana<TVentana>(Func<TVentana> Crear, Action<TVentana> Comprobar) where TVentana : Form
        {
            Exception Fallo = null;
            Thread Hilo = new Thread(delegate ()
            {
                TVentana Ventana = null;
                try
                {
                    Ventana = Crear();
                    Comprobar(Ventana);
                }
                catch (Exception Excepcion)
                {
                    Fallo = Excepcion;
                }
                finally
                {
                    if (Ventana != null) Ventana.Dispose();
                }
            });
            Hilo.SetApartmentState(ApartmentState.STA);
            Hilo.Start();
            Hilo.Join();
            if (Fallo != null) ExceptionDispatchInfo.Capture(Fallo).Throw();
        }

        // Busca un control de la ventana por su nombre, aunque esté dentro de un panel.
        private static TControl ContabilidadFuncControl<TControl>(Form Ventana, string Nombre) where TControl : Control
        {
            Control[] Encontrados = Ventana.Controls.Find(Nombre, true);
            Assert.AreEqual(1, Encontrados.Length, "No se encontró el control " + Nombre + ".");
            return (TControl)Encontrados[0];
        }

        // Icono asignado a un boton de Contabilidad.
        private static object ContabilidadFuncIcono(Button Boton)
        {
            return Boton.GetType().GetProperty("Icono").GetValue(Boton);
        }

        // Presupuesto de ejemplo, en memoria, para cargar la ventana de edición.
        private static ClsModeloPresupuestoContabilidad ContabilidadFuncPresupuestoDePrueba()
        {
            return new ClsModeloPresupuestoContabilidad
            {
                IdPresupuesto = 99,
                Nombre = "Presupuesto de prueba",
                FechaInicio = new DateTime(2026, 3, 1),
                FechaCierre = new DateTime(2026, 9, 30),
                Partidas =
                {
                    new ClsModeloPartidaPresupuestoContabilidad { Nombre = "Sueldos", Modulo = "Contabilidad", Cantidad = 1500m },
                    new ClsModeloPartidaPresupuestoContabilidad { Nombre = "Equipo", Modulo = "Bancos", Cantidad = 2500m }
                }
            };
        }

        //  Capa Modelo
        // ==================================================================

        [TestMethod]
        public void Test_EntidadNaceConDetalleVacio()
        {
            //Arrange: inicializar las variables
            ClsPresupuestoContabilidad Entidad;
            //Act: ejecucion del metodo a probar
            Entidad = new ClsPresupuestoContabilidad();
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Entidad.Detalle);
            Assert.AreEqual(0, Entidad.Detalle.Count);
        }

        [TestMethod]
        public void Test_EntidadGuardaSusDatos()
        {
            //Arrange: inicializar las variables
            DateTime Inicio = new DateTime(2026, 1, 1);
            DateTime Cierre = new DateTime(2026, 12, 31);
            //Act: ejecucion del metodo a probar
            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad
            {
                IdPresupuesto = 5, Nombre = "Presupuesto 2026", Total = 4000m, Gastado = 1500m,
                FechaInicio = Inicio, FechaCierre = Cierre
            };
            //Assert: comprobacion de los resultados
            Assert.AreEqual(5, Entidad.IdPresupuesto);
            Assert.AreEqual("Presupuesto 2026", Entidad.Nombre);
            Assert.AreEqual(4000m, Entidad.Total);
            Assert.AreEqual(1500m, Entidad.Gastado);
            Assert.AreEqual(Inicio, Entidad.FechaInicio);
            Assert.AreEqual(Cierre, Entidad.FechaCierre);
        }

        [TestMethod]
        public void Test_EntidadAceptaPresupuestoAbierto()
        {
            //Arrange: inicializar las variables
            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad { FechaInicio = new DateTime(2026, 1, 1) };
            //Act: ejecucion del metodo a probar
            Entidad.FechaCierre = null;
            //Assert: comprobacion de los resultados
            Assert.IsFalse(Entidad.FechaCierre.HasValue);
        }

        [TestMethod]
        public void Test_EntidadGuardaSusPartidas()
        {
            //Arrange: inicializar las variables
            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad();
            //Act: ejecucion del metodo a probar
            Entidad.Detalle.Add(new ClsPresupuestoDetalleContabilidad { Nombre = "Sueldos", Modulo = "Contabilidad", Cantidad = 1500m });
            //Assert: comprobacion de los resultados
            Assert.AreEqual(1, Entidad.Detalle.Count);
            Assert.AreEqual("Sueldos", Entidad.Detalle[0].Nombre);
            Assert.AreEqual("Contabilidad", Entidad.Detalle[0].Modulo);
            Assert.AreEqual(1500m, Entidad.Detalle[0].Cantidad);
        }

        [TestMethod]
        public void Test_RepositorioCumpleElContrato()
        {
            //Arrange: inicializar las variables
            object Repositorio = _Repositorio;
            //Act: ejecucion del metodo a probar
            IRepositorioPresupuestosContabilidad Contrato = Repositorio as IRepositorioPresupuestosContabilidad;
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Contrato);
        }

        [TestMethod]
        public void Test_ObtenerModulos()
        {
            //Arrange: inicializar las variables
            string[] Modulos;
            //Act: ejecucion del metodo a probar
            Modulos = _Repositorio.ContabilidadFuncObtenerModulos();
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Modulos);
            Assert.IsTrue(Modulos.Length > 0, "No hay módulos contables activos en la base de datos.");
        }

        [TestMethod]
        public void Test_ObtenerModulosSinNombresVacios()
        {
            //Arrange: inicializar las variables
            string[] Modulos = _Repositorio.ContabilidadFuncObtenerModulos();
            //Act: ejecucion del metodo a probar
            bool TodosConNombre = Modulos.All(Modulo => !string.IsNullOrWhiteSpace(Modulo));
            //Assert: comprobacion de los resultados
            Assert.IsTrue(TodosConNombre);
        }

        [TestMethod]
        public void Test_ObtenerTodos()
        {
            //Arrange: inicializar las variables
            IEnumerable<ClsPresupuestoContabilidad> Presupuestos;
            //Act: ejecucion del metodo a probar
            Presupuestos = _Repositorio.ContabilidadFuncObtenerTodos();
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Presupuestos);
            Assert.IsTrue(Presupuestos.All(Presupuesto => Presupuesto.IdPresupuesto > 0));
        }

        [TestMethod]
        public void Test_ObtenerTodosArmaElDetalle()
        {
            //Arrange: inicializar las variables
            List<ClsPresupuestoContabilidad> Presupuestos = _Repositorio.ContabilidadFuncObtenerTodos().ToList();
            //Act: ejecucion del metodo a probar
            bool DetalleCompleto = Presupuestos.All(Presupuesto => Presupuesto.Detalle != null
                && Presupuesto.Detalle.All(Partida => !string.IsNullOrWhiteSpace(Partida.Nombre)));
            //Assert: comprobacion de los resultados
            Assert.IsTrue(DetalleCompleto, "Hay partidas sin nombre en la consulta de presupuestos.");
        }

        [TestMethod]
        public void Test_ObtenerTodosNoRepiteIdentificadores()
        {
            //Arrange: inicializar las variables
            List<ClsPresupuestoContabilidad> Presupuestos = _Repositorio.ContabilidadFuncObtenerTodos().ToList();
            //Act: ejecucion del metodo a probar
            int Distintos = Presupuestos.Select(Presupuesto => Presupuesto.IdPresupuesto).Distinct().Count();
            //Assert: comprobacion de los resultados
            Assert.AreEqual(Presupuestos.Count, Distintos);
        }

        [TestMethod]
        public void Test_ObtenerEjecutado()
        {
            //Arrange: inicializar las variables
            DateTime Desde = new DateTime(2026, 1, 1);
            DateTime Hasta = new DateTime(2026, 12, 31);
            //Act: ejecucion del metodo a probar
            Dictionary<string, decimal> Ejecutado = _Repositorio.ContabilidadFuncObtenerEjecutado(Desde, Hasta);
            //Assert: comprobacion de los resultados
            Assert.IsNotNull(Ejecutado);
            Assert.IsTrue(Ejecutado.Keys.All(Modulo => !string.IsNullOrWhiteSpace(Modulo)));
        }

        [TestMethod]
        public void Test_ObtenerEjecutadoRangoInvertido()
        {
            //Arrange: inicializar las variables
            DateTime Desde = new DateTime(2026, 12, 31);
            DateTime Hasta = new DateTime(2026, 1, 1);
            //Act: ejecucion del metodo a probar
            Dictionary<string, decimal> Ejecutado = _Repositorio.ContabilidadFuncObtenerEjecutado(Desde, Hasta);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0, Ejecutado.Count);
        }

        [TestMethod]
        public void Test_ObtenerEjecutadoSinMovimientos()
        {
            //Arrange: inicializar las variables
            DateTime Desde = new DateTime(1900, 1, 1);
            DateTime Hasta = new DateTime(1900, 12, 31);
            //Act: ejecucion del metodo a probar
            Dictionary<string, decimal> Ejecutado = _Repositorio.ContabilidadFuncObtenerEjecutado(Desde, Hasta);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0, Ejecutado.Count);
        }

        [TestMethod]
        public void Test_EditarPresupuestoInexistente()
        {
            //Arrange: inicializar las variables
            // El identificador no existe: la actualización afecta cero filas y se deshace.
            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad
            {
                IdPresupuesto = int.MaxValue, Nombre = "Presupuesto inexistente", Total = 0m, Gastado = 0m,
                FechaInicio = new DateTime(2026, 1, 1), FechaCierre = new DateTime(2026, 12, 31)
            };
            //Act: ejecucion del metodo a probar
            int Filas = _Repositorio.ContabilidadFuncEditar(Entidad);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0, Filas);
        }

        [TestMethod]
        public void Test_EliminarPresupuestoInexistente()
        {
            //Arrange: inicializar las variables
            // El identificador no existe: la baja lógica afecta cero filas.
            ClsPresupuestoContabilidad Entidad = new ClsPresupuestoContabilidad { IdPresupuesto = int.MaxValue };
            //Act: ejecucion del metodo a probar
            int Filas = _Repositorio.ContabilidadFuncEliminar(Entidad);
            //Assert: comprobacion de los resultados
            Assert.AreEqual(0, Filas);
        }

        //  Capa Vista
        // ==================================================================

        [TestMethod]
        public void Test_EdicionPreparaLaVentana()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, null), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual("10002 - PresupuestoEdicion", Ventana.Text);
                Assert.AreEqual(1000, Ventana.ClientSize.Width);
                Assert.AreEqual(690, Ventana.ClientSize.Height);
                Assert.AreEqual(FormBorderStyle.FixedSingle, Ventana.FormBorderStyle);
                Assert.AreEqual(FormStartPosition.CenterScreen, Ventana.StartPosition);
            });
        }

        [TestMethod]
        public void Test_EdicionNavegaLaTabla()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Ventana.Show();
                DataGridView Tabla = ContabilidadFuncControl<DataGridView>(Ventana, "ContabilidadDgvDetalle");
                ContabilidadFuncControl<Button>(Ventana, "ContabilidadBtnFin").PerformClick();
                Assert.AreEqual(1, Tabla.CurrentRow.Index);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnSiguiente").Enabled);
                ContabilidadFuncControl<Button>(Ventana, "ContabilidadBtnInicio").PerformClick();
                Assert.AreEqual(0, Tabla.CurrentRow.Index);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnAnterior").Enabled);
                ContabilidadFuncControl<Button>(Ventana, "ContabilidadBtnSiguiente").PerformClick();
                Assert.AreEqual(1, Tabla.CurrentRow.Index);
                ContabilidadFuncControl<Button>(Ventana, "ContabilidadBtnAnterior").PerformClick();
                Assert.AreEqual(0, Tabla.CurrentRow.Index);
            });
        }

        [TestMethod]
        public void Test_BotonesConIconoDelCatalogo()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            string[] Edicion = { "ContabilidadBtnAgregar", "ContabilidadBtnQuitar", "ContabilidadBtnCancelar", "ContabilidadBtnGuardar",
                "ContabilidadBtnInicio", "ContabilidadBtnAnterior", "ContabilidadBtnSiguiente", "ContabilidadBtnFin" };
            string[] Consulta = { "ContabilidadBtnNueva", "ContabilidadBtnModificar", "ContabilidadBtnEliminar",
                "ContabilidadBtnInicio", "ContabilidadBtnAnterior", "ContabilidadBtnSiguiente", "ContabilidadBtnFin" };
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, null), Ventana =>
            //Assert: comprobacion de los resultados
            {
                foreach (string Nombre in Edicion)
                    Assert.IsNotNull(ContabilidadFuncIcono(ContabilidadFuncControl<Button>(Ventana, Nombre)), Nombre);
            });
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            {
                foreach (string Nombre in Consulta)
                    Assert.IsNotNull(ContabilidadFuncIcono(ContabilidadFuncControl<Button>(Ventana, Nombre)), Nombre);
            });
        }

        [TestMethod]
        public void Test_EdicionNuevoPresupuesto()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            int Anio = DateTime.Today.Year;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, null), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual(0, Ventana.IdPresupuesto);
                Assert.AreEqual("Nuevo presupuesto", ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblTitulo").Text);
                Assert.AreEqual(string.Empty, ContabilidadFuncControl<TextBox>(Ventana, "ContabilidadTxtNombrePresupuesto").Text);
                Assert.AreEqual(new DateTime(Anio, 1, 1), ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpInicio").Value.Date);
                Assert.AreEqual(new DateTime(Anio, 12, 31), ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpCierre").Value.Date);
                Assert.IsTrue(ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpCierre").Checked);
            });
        }

        [TestMethod]
        public void Test_EdicionNuevoPresupuestoSinPartidas()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, null), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual(0, ContabilidadFuncControl<DataGridView>(Ventana, "ContabilidadDgvDetalle").Rows.Count);
                Assert.IsTrue(ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblResumen").Text.Contains("Partidas: 0"));
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnQuitar").Enabled);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnGuardar").Enabled);
            });
        }

        [TestMethod]
        public void Test_EdicionCargaPresupuestoExistente()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual(99, Ventana.IdPresupuesto);
                Assert.AreEqual("Modificar presupuesto", ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblTitulo").Text);
                Assert.AreEqual("Presupuesto de prueba", ContabilidadFuncControl<TextBox>(Ventana, "ContabilidadTxtNombrePresupuesto").Text);
                Assert.AreEqual(new DateTime(2026, 3, 1), ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpInicio").Value.Date);
                Assert.AreEqual(new DateTime(2026, 9, 30), ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpCierre").Value.Date);
            });
        }

        [TestMethod]
        public void Test_EdicionMuestraLasPartidasEnLaTabla()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                DataGridView Tabla = ContabilidadFuncControl<DataGridView>(Ventana, "ContabilidadDgvDetalle");
                Assert.AreEqual(2, Tabla.Rows.Count);
                Assert.AreEqual(3, Tabla.Columns.Count);
                Assert.AreEqual("Sueldos", Tabla.Rows[0].Cells[0].Value);
                Assert.AreEqual(2500m, Tabla.Rows[1].Cells[2].Value);
            });
        }

        [TestMethod]
        public void Test_EdicionResumeElTotalAsignado()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                string Resumen = ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblResumen").Text;
                Assert.IsTrue(Resumen.Contains("Partidas: 2"), Resumen);
                Assert.IsTrue(Resumen.Contains("4,000.00"), Resumen);
            });
        }

        [TestMethod]
        public void Test_EdicionPresupuestoSinFechaDeCierre()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            Presupuesto.FechaCierre = null;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.IsFalse(ContabilidadFuncControl<DateTimePicker>(Ventana, "ContabilidadDtpCierre").Checked);
            });
        }

        [TestMethod]
        public void Test_EdicionCargaLosModulosDeLaBase()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            int Esperados = _Repositorio.ContabilidadFuncObtenerModulos().Length;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, null), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual(Esperados, ContabilidadFuncControl<ComboBox>(Ventana, "ContabilidadCboModulo").Items.Count);
            });
        }

        [TestMethod]
        public void Test_EdicionSinPermisosBloqueaLosBotones()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Ninguno;
            ClsModeloPresupuestoContabilidad Presupuesto = ContabilidadFuncPresupuestoDePrueba();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestoEdicion(Permisos, Presupuesto), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnAgregar").Enabled);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnQuitar").Enabled);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnGuardar").Enabled);
                Assert.IsTrue(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnCancelar").Enabled);
            });
        }


        [TestMethod]
        public void Test_ConsultaPreparaLaVentana()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual("10002 - Presupuestos", Ventana.Text);
                Assert.AreEqual(1180, Ventana.ClientSize.Width);
                Assert.AreEqual(688, Ventana.ClientSize.Height);
            });
        }

        [TestMethod]
        public void Test_ConsultaUsaPermisosTotalesPorOmision()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.IsTrue(Permisos.PuedeInsertar);
                Assert.IsTrue(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnNueva").Enabled);
            });
        }

        [TestMethod]
        public void Test_ConsultaCargaLaListaDePresupuestos()
        {
            //Arrange: inicializar las variables
            int Esperados = _Repositorio.ContabilidadFuncObtenerTodos().Count();
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.AreEqual(Esperados, ContabilidadFuncControl<ComboBox>(Ventana, "ContabilidadCboPresupuesto").Items.Count);
            });
        }

        [TestMethod]
        public void Test_ConsultaHabilitaSegunLaSeleccion()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                bool HayPresupuesto = ContabilidadFuncControl<ComboBox>(Ventana, "ContabilidadCboPresupuesto").Items.Count > 0;
                Assert.AreEqual(HayPresupuesto, ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnModificar").Enabled);
                Assert.AreEqual(HayPresupuesto, ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnEliminar").Enabled);
            });
        }

        [TestMethod]
        public void Test_ConsultaMuestraVigenciaOAviso()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                bool HayPresupuesto = ContabilidadFuncControl<ComboBox>(Ventana, "ContabilidadCboPresupuesto").Items.Count > 0;
                string Vigencia = ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblVigencia").Text;
                Assert.AreEqual(HayPresupuesto, Vigencia.StartsWith("Vigencia: del "), Vigencia);
                Assert.AreEqual(!HayPresupuesto, Vigencia.StartsWith("No hay presupuestos guardados"), Vigencia);
            });
        }

        [TestMethod]
        public void Test_ConsultaResumeLaEjecucion()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                bool HayPresupuesto = ContabilidadFuncControl<ComboBox>(Ventana, "ContabilidadCboPresupuesto").Items.Count > 0;
                string Resumen = ContabilidadFuncControl<Label>(Ventana, "ContabilidadLblResumen").Text;
                Assert.AreEqual(HayPresupuesto, Resumen.Contains("Asignado:"), Resumen);
                if (HayPresupuesto) Assert.IsTrue(Resumen.Contains("Ejecutado:") && Resumen.Contains("Restante:"), Resumen);
            });
        }

        [TestMethod]
        public void Test_ConsultaArmaLaTablaDeEjecucion()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Total;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                DataGridView Tabla = ContabilidadFuncControl<DataGridView>(Ventana, "ContabilidadDgvEjecucion");
                Assert.AreEqual(5, Tabla.Columns.Count);
                Assert.IsFalse(Tabla.AllowUserToAddRows);
            });
        }

        [TestMethod]
        public void Test_ConsultaSinPermisosBloqueaLosBotones()
        {
            //Arrange: inicializar las variables
            ClsModeloPermisosPantallaContabilidad Permisos = ClsModeloPermisosPantallaContabilidad.Ninguno;
            //Act: ejecucion del metodo a probar
            ContabilidadMetEnVentana(() => new FrmPresupuestos(Permisos), Ventana =>
            //Assert: comprobacion de los resultados
            {
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnNueva").Enabled);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnModificar").Enabled);
                Assert.IsFalse(ContabilidadFuncControl<Control>(Ventana, "ContabilidadBtnEliminar").Enabled);
            });
        }
    }
}
