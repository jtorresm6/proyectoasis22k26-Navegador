using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_PermisosVacaciones.Clases;
using CapaModelo_PermisosVacaciones.Clases;
using CapaVista_PermisosVacaciones.Formularios;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias de Permisos y Vacaciones: configuración del
 *  Navegador, conteo de días hábiles, validaciones de vacaciones que
 *  no necesitan la base de datos y lectura del empleado del combo.
 * ===================================================================
*/

namespace MSTestPermisosVacaciones
{
    [TestClass]
    public class TestPermisosVacaciones
    {
        // ==========================================
        // 1. PRUEBAS DE CONFIGURACIÓN EN CONTROLADOR
        // ==========================================

        [TestMethod]
        public void Controlador_Configuracion_DevuelveModuloAplicacionesYTablas()
        {
            // Arrange
            var Controlador = new ClsControladorPermisosVacaciones();

            // Act & Assert
            Assert.AreEqual(9000, Controlador.RrhhFuncIdModulo());
            Assert.AreEqual(9202, Controlador.RrhhFuncIdAplicacionPermisos());
            Assert.AreEqual(9203, Controlador.RrhhFuncIdAplicacionVacaciones());
            Assert.AreEqual("tblPermisoEmpleado", Controlador.RrhhFuncTablaPermisos());
            Assert.AreEqual("tblVacacionEmpleado", Controlador.RrhhFuncTablaVacaciones());
        }

        [TestMethod]
        public void Modelo_ColumnasDeFecha_DevuelveInicioYFin()
        {
            // Arrange
            var Sentencias = new ClsSentenciasPermisosVacaciones();

            // Act
            string[] Permisos = Sentencias.RrhhFuncFechasPermisos();
            string[] Vacaciones = Sentencias.RrhhFuncFechasVacaciones();

            // Assert
            CollectionAssert.AreEqual(new[] { "fechaInicioPermisoEmpleado", "fechaFinPermisoEmpleado" }, Permisos);
            CollectionAssert.AreEqual(new[] { "fechaInicioVacacionEmpleado", "fechaFinVacacionEmpleado" }, Vacaciones);
        }

        // ==========================================
        // 2. PRUEBAS DE DÍAS HÁBILES (LUNES A VIERNES)
        // ==========================================

        [DataTestMethod]
        [DataRow("2026-11-02", "2026-11-06", 5)]   // lunes a viernes
        [DataRow("2026-11-02", "2026-11-08", 5)]   // semana completa: sábado y domingo no cuentan
        [DataRow("2026-11-02", "2026-11-13", 10)]  // dos semanas
        [DataRow("2026-11-07", "2026-11-08", 0)]   // solo fin de semana
        [DataRow("2026-11-04", "2026-11-04", 1)]   // un solo día hábil
        [DataRow("2026-11-06", "2026-11-02", 0)]   // fecha fin anterior a la de inicio
        public void RrhhFuncDiasHabiles_SegunRango_CuentaLunesAViernes(string Inicio, string Fin, int Esperado)
        {
            // Arrange
            var Controlador = new ClsControladorPermisosVacaciones();

            // Act
            int Dias = Controlador.RrhhFuncDiasHabiles(DateTime.Parse(Inicio), DateTime.Parse(Fin));

            // Assert
            Assert.AreEqual(Esperado, Dias);
        }

        // ==========================================
        // 3. PRUEBAS DE VALIDACIÓN DE VACACIONES
        // ==========================================

        [TestMethod]
        public void RrhhFuncValidarVacacion_FinAntesDeInicio_DevuelveMensaje()
        {
            // Arrange
            var Controlador = new ClsControladorPermisosVacaciones();

            // Act
            string Mensaje = Controlador.RrhhFuncValidarVacacion(1, new DateTime(2026, 11, 10), new DateTime(2026, 11, 2), 0);

            // Assert
            Assert.AreEqual("La fecha fin no puede ser anterior a la fecha inicio.", Mensaje);
        }

        [TestMethod]
        public void RrhhFuncValidarVacacion_SoloFinDeSemana_DevuelveMensaje()
        {
            // Arrange
            var Controlador = new ClsControladorPermisosVacaciones();

            // Act
            string Mensaje = Controlador.RrhhFuncValidarVacacion(1, new DateTime(2026, 11, 7), new DateTime(2026, 11, 8), 0);

            // Assert
            Assert.AreEqual("El rango elegido no tiene días hábiles (lunes a viernes).", Mensaje);
        }

        // ==========================================
        // 4. PRUEBAS DE INTERFAZ / FORMULARIO (UI)
        // ==========================================

        [DataTestMethod]
        [DataRow("5 - Isabel Meléndez", 5)]
        [DataRow("12", 12)]
        [DataRow("", 0)]
        [DataRow(null, 0)]
        public void RrhhFuncIdOpcion_TextoDelCombo_DevuelveIdEmpleado(string? Opcion, int Esperado)
        {
            // Arrange
            MethodInfo? Metodo = typeof(FrmVacaciones).GetMethod("RrhhFuncIdOpcion",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(Metodo, "El formulario debe tener el método RrhhFuncIdOpcion.");

            // Act
            int Id = (int)Metodo.Invoke(null, new object?[] { Opcion })!;

            // Assert
            Assert.AreEqual(Esperado, Id);
        }
    }
}
