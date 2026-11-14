using System.Reflection;
using CapaControlador_AsigCapacitacion.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using CapaModelo_AsigCapacitacion;
using CapaModelo_AsigCapacitacion.Entidades;
using CapaVista_AsigCapacitacion.Formularios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;

namespace MSTestAsigCapacitacion
{
    [TestClass]
    public class TestAsigCapacitacion
    {
        // ---------- CONTROLADOR

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void RrhhMetGrabarCambios_SinEmpleado_PideSeleccionarEmpleado(EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloAsigCapacitacion
            {
                IdEmpleado = 0,
                IdCapacitacion = 3,
                FechaAsignacionCapacitacion = DateTime.Today,
                Estado = Estado
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe seleccionar un Empleado.", Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void RrhhMetGrabarCambios_SinCapacitacion_PideSeleccionarCapacitacion(EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloAsigCapacitacion
            {
                IdEmpleado = 5,
                IdCapacitacion = 0,
                FechaAsignacionCapacitacion = DateTime.Today,
                Estado = Estado
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe seleccionar una Capacitación.", Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_ConIdsNegativos_TratadosComoNoSeleccionados()
        {
            // Arrange
            var Controlador = new ClsModeloAsigCapacitacion
            {
                IdEmpleado = -1,
                IdCapacitacion = -1,
                Estado = EstadoEntidad.Added
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert: se valida primero el empleado
            Assert.AreEqual("Debe seleccionar un Empleado.", Resultado);
        }

        // ---------- CONTROLADOR: propiedades ----------

        [TestMethod]
        public void Propiedades_AlAsignarValores_LosConservan()
        {
            // Arrange
            DateTime Fecha = new DateTime(2026, 10, 31);
            var Controlador = new ClsModeloAsigCapacitacion();

            // Act
            Controlador.IdAsignacionCapacitacion = 1;
            Controlador.IdEmpleado = 2;
            Controlador.NombreEmpleado = "Carlos";
            Controlador.IdCapacitacion = 3;
            Controlador.NombreCapacitacion = "Excel";
            Controlador.NombreInstructor = "Ana";
            Controlador.FechaAsignacionCapacitacion = Fecha;

            // Assert
            Assert.AreEqual(1, Controlador.IdAsignacionCapacitacion);
            Assert.AreEqual(2, Controlador.IdEmpleado);
            Assert.AreEqual("Carlos", Controlador.NombreEmpleado);
            Assert.AreEqual(3, Controlador.IdCapacitacion);
            Assert.AreEqual("Excel", Controlador.NombreCapacitacion);
            Assert.AreEqual("Ana", Controlador.NombreInstructor);
            Assert.AreEqual(Fecha, Controlador.FechaAsignacionCapacitacion);
        }

        // ---------- ENTIDAD ----------

        [TestMethod]
        public void Entidad_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Entidad = new ClsAsigCapacitacion
            {
                IdAsignacionCapacitacion = 10,
                IdEmpleado = 20,
                IdCapacitacion = 30,
                NombreEmpleado = "Carlos",
                NombreCapacitacion = "Seguridad",
                NombreInstructor = "Luis",
                FechaAsignacionCapacitacion = new DateTime(2026, 10, 31)
            };

            // Assert
            Assert.AreEqual(10, Entidad.IdAsignacionCapacitacion);
            Assert.AreEqual(20, Entidad.IdEmpleado);
            Assert.AreEqual(30, Entidad.IdCapacitacion);
            Assert.AreEqual("Carlos", Entidad.NombreEmpleado);
            Assert.AreEqual("Seguridad", Entidad.NombreCapacitacion);
            Assert.AreEqual("Luis", Entidad.NombreInstructor);
            Assert.AreEqual(new DateTime(2026, 10, 31), Entidad.FechaAsignacionCapacitacion);
        }

        // ---------- FORMULARIO

        [TestMethod]
        public void RrhhMetReinicio_ConDatosPrevios_LimpiaCamposYRestableceFecha()
        {
            // Arrange
            using (var Formulario = new FrmAsigCapacitacion())
            {
                var Fecha = (DateTimePicker)Formulario.Controls.Find("RrhhDtpFechaAsig", true)[0];
                Fecha.Value = new DateTime(2020, 1, 1);

                var CampoId = typeof(FrmAsigCapacitacion).GetField("_IdAsignacionSeleccionada",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                CampoId.SetValue(Formulario, 7);

                var Metodo = typeof(FrmAsigCapacitacion).GetMethod("RrhhMetReinicio",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                // Act
                Metodo.Invoke(Formulario, null);

                // Assert
                Assert.AreEqual(0, (int)CampoId.GetValue(Formulario));
                Assert.AreEqual(DateTime.Today, Fecha.Value.Date);
                var CboEmp = (ComboBox)Formulario.Controls.Find("RrhhCboEmpleado", true)[0];
                var CboCap = (ComboBox)Formulario.Controls.Find("RrhhCboCapacitacion", true)[0];
                Assert.AreEqual(-1, CboEmp.SelectedIndex);
                Assert.AreEqual(-1, CboCap.SelectedIndex);
            }
        }
    }
}