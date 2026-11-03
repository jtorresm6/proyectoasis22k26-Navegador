using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_EvaluacionDesempeño.Controladores;
using CapaControlador_Seguridad;
using CapaModelo_EvaluacionDesempeño.Entidades;
using CapaVista_EvaluacionDesempeño.Formularios;

/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas unitarias de Evaluación de Desempeño: validaciones del
 *  controlador de Plan de Mejora, cálculos de las entidades de Ver
 *  Evaluaciones y validación de notas de Realizar Evaluación.
 *  Ninguna prueba necesita la base de datos.
 * ===================================================================
*/

namespace MSTestEvaluacionDesempeno
{
    [TestClass]
    public class TestEvaluacionDesempeno
    {
        // ==========================================
        // 1. PRUEBAS DE VALIDACIÓN EN CONTROLADOR
        // ==========================================

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void RrhhMetGrabarCambios_SinNombre_DevuelveValidacion(EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPlanMejora
            {
                IdPlanMejora = 1,
                NombrePlanMejora = "   ",
                DescripcionPlanMejora = "Descripción de prueba",
                Estado = Estado
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe ingresar el Nombre del Plan.", Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_NombreMayorA100_DevuelveValidacion()
        {
            // Arrange
            var Controlador = new ClsModeloPlanMejora
            {
                NombrePlanMejora = new string('A', 101),
                DescripcionPlanMejora = "Descripción de prueba",
                Estado = EstadoEntidad.Added
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("El Nombre del Plan no puede tener más de 100 caracteres.", Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_SinDescripcion_DevuelveValidacion()
        {
            // Arrange
            var Controlador = new ClsModeloPlanMejora
            {
                NombrePlanMejora = "Plan de prueba",
                DescripcionPlanMejora = "",
                Estado = EstadoEntidad.Added
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe ingresar la Descripción.", Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_EmpleadoSinEvaluacion_DevuelveValidacion()
        {
            // Arrange
            var Controlador = new ClsModeloPlanMejora
            {
                NombrePlanMejora = "Plan de prueba",
                DescripcionPlanMejora = "Descripción de prueba",
                IdEmpleado = 3,
                IdEvaluacionDesempeno = null,
                Estado = EstadoEntidad.Added
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe seleccionar la Evaluación del empleado a la que se relaciona el plan.", Resultado);
        }

        // ==========================================
        // 2. PRUEBAS DE PROPIEDADES EN CONTROLADOR
        // ==========================================

        [TestMethod]
        public void PropiedadesControlador_AlAsignarValores_LosConservaCorrectamente()
        {
            // Arrange
            var Controlador = new ClsModeloPlanMejora();

            // Act
            Controlador.IdPlanMejora = 4;
            Controlador.NombrePlanMejora = "Gestión del tiempo";
            Controlador.DescripcionPlanMejora = "Mejorar la puntualidad";
            Controlador.IdEmpleado = 2;
            Controlador.NombreEmpleado = "Carlos Ramírez";
            Controlador.IdEvaluacionDesempeno = 7;
            Controlador.FechaEvaluacion = "02/11/2026";

            // Assert
            Assert.AreEqual(4, Controlador.IdPlanMejora);
            Assert.AreEqual("Gestión del tiempo", Controlador.NombrePlanMejora);
            Assert.AreEqual("Mejorar la puntualidad", Controlador.DescripcionPlanMejora);
            Assert.AreEqual(2, Controlador.IdEmpleado);
            Assert.AreEqual("Carlos Ramírez", Controlador.NombreEmpleado);
            Assert.AreEqual(7, Controlador.IdEvaluacionDesempeno);
            Assert.AreEqual("02/11/2026", Controlador.FechaEvaluacion);
        }

        // ==========================================
        // 3. PRUEBAS DE CÁLCULOS EN ENTIDADES
        // ==========================================

        [DataTestMethod]
        [DataRow(4.65, "93%", "Excelente")]
        [DataRow(4.00, "80%", "Excelente")]
        [DataRow(2.65, "53%", "Satisfactorio")]
        [DataRow(2.50, "50%", "Satisfactorio")]
        [DataRow(1.65, "33%", "Insuficiente")]
        public void ResultadoEvaluacion_SegunPuntaje_CalculaPorcentajeYNivel(double Puntaje, string Porcentaje, string Nivel)
        {
            // Arrange & Act
            var Evaluacion = new ClsResultadoEvaluacion { PuntajeEvaluacionDesempeno = (decimal)Puntaje };

            // Assert
            Assert.AreEqual(Porcentaje, Evaluacion.PorcentajeTexto);
            Assert.AreEqual(Nivel, Evaluacion.NombreNivelDesempeno);
        }

        [TestMethod]
        public void DetalleEvaluacion_AlAsignarPesoYNota_CalculaPonderado()
        {
            // Arrange & Act
            var Aspecto = new ClsDetalleEvaluacion
            {
                NombreCriterioEvaluacion = "Calidad del trabajo",
                PesoDetalleEvaluacionDesempeno = 0.25m,
                NotaDetalleEvaluacionDesempeno = 4
            };

            // Assert
            Assert.AreEqual(1.00m, Aspecto.PonderadoDetalleEvaluacionDesempeno);
        }

        [TestMethod]
        public void ResultadoEvaluacion_DescripcionEvaluacion_MuestraFechaYEvaluador()
        {
            // Arrange & Act
            var Evaluacion = new ClsResultadoEvaluacion
            {
                FechaEvaluacionDesempeno = new DateTime(2026, 11, 2),
                NombreEvaluador = "María López"
            };

            // Assert
            Assert.AreEqual("02/11/2026 - María López", Evaluacion.DescripcionEvaluacion);
        }

        [TestMethod]
        public void PropiedadesEntidadPlan_AlAsignarValores_LosConservaCorrectamente()
        {
            // Arrange & Act
            var Entidad = new ClsPlanMejora
            {
                IdPlanMejora = 3,
                NombrePlanMejora = "Fortalecimiento del trabajo en equipo",
                DescripcionPlanMejora = "Mejorar la comunicación",
                IdEvaluacionDesempeno = 9,
                IdEmpleado = 1,
                NombreEmpleado = "Isabel Meléndez",
                FechaEvaluacion = "30/10/2026"
            };

            // Assert
            Assert.AreEqual(3, Entidad.IdPlanMejora);
            Assert.AreEqual("Fortalecimiento del trabajo en equipo", Entidad.NombrePlanMejora);
            Assert.AreEqual("Mejorar la comunicación", Entidad.DescripcionPlanMejora);
            Assert.AreEqual(9, Entidad.IdEvaluacionDesempeno);
            Assert.AreEqual(1, Entidad.IdEmpleado);
            Assert.AreEqual("Isabel Meléndez", Entidad.NombreEmpleado);
            Assert.AreEqual("30/10/2026", Entidad.FechaEvaluacion);
        }

        // ==========================================
        // 4. PRUEBAS DE INTERFAZ / FORMULARIO (UI)
        // ==========================================

        // Solo notas válidas o vacías: una nota inválida abre un mensaje emergente.
        [DataTestMethod]
        [DataRow("1")]
        [DataRow("3")]
        [DataRow("5")]
        [DataRow("")]
        public void RrhhMetValidarNota_NotaValida_NoCancelaElCampo(string Nota)
        {
            // Arrange
            using (var Formulario = new FrmRealizarEvaluacion())
            using (var Campo = new TextBox { Text = Nota })
            {
                var Evento = new CancelEventArgs();
                MethodInfo Metodo = typeof(FrmRealizarEvaluacion).GetMethod("RrhhMetValidarNota",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(Metodo, "El formulario debe tener el método RrhhMetValidarNota.");

                // Act
                Metodo.Invoke(Formulario, new object[] { Campo, Evento });

                // Assert
                Assert.IsFalse(Evento.Cancel);
            }
        }
    }
}
