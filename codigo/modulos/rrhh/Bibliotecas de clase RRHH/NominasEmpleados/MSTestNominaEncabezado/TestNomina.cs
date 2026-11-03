using CapaControlador_Nomina.Modelos_de_controladores;
using CapaControlador_Nomina.ObjetosdeValor;
using CapaModelo_Nomina.Entidades;
using CapaVista_Nomina.Formularios;
using Microsoft.Testing.Platform.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;



namespace MSTestNominaEncabezado
{
    [TestClass]
    public class TestNomina
    {


        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void RrhhMetGrabarCambios_SinEmpleado_PideSeleccionarEmpleado(int Id)
        {
           
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = Id,
                Estado = EstadoEntidad.Added
            };

            
            string Resultado = Controlador.RrhhMetGrabarCambios();


            Assert.AreEqual("Debe seleccionar un empleado", Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_FechasInvertidas_RechazaPeriodo()
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = 5,
                Estado = EstadoEntidad.Added,
                FechaInicioNominaEncabezado = new DateTime(2026, 10, 31),
                FechaFinalNominaEncabezado = new DateTime(2026, 10, 1)
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "La fecha final no puede ser menor a la fecha inicial",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void RrhhMetCalcularConceptos_SinEmpleado_PideSeleccionarEmpleado(int Id)
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetCalcularConceptos();

            // Assert
            Assert.AreEqual("Debe seleccionar un empleado", Resultado);
        }

        [TestMethod]
        public void RrhhMetCalcularConceptos_FechasInvertidas_RechazaPeriodo()
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = 5,
                FechaInicioNominaEncabezado = new DateTime(2026, 10, 31),
                FechaFinalNominaEncabezado = new DateTime(2026, 10, 1)
            };

            // Act
            string Resultado = Controlador.RrhhMetCalcularConceptos();

            // Assert
            Assert.AreEqual(
                "La fecha final no puede ser menor a la fecha inicial",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void RrhhMetCalcularNomina_SinEmpleado_PideSeleccionarEmpleado(int Id)
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetCalcularNomina();

            // Assert
            Assert.AreEqual("Debe seleccionar un empleado", Resultado);
        }

        [TestMethod]
        public void RrhhMetCalcularNomina_FechasInvertidas_RechazaPeriodo()
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                IdEmpleado = 5,
                FechaInicioNominaEncabezado = new DateTime(2026, 10, 31),
                FechaFinalNominaEncabezado = new DateTime(2026, 10, 1)
            };

            // Act
            string Resultado = Controlador.RrhhMetCalcularNomina();

            // Assert
            Assert.AreEqual(
                "La fecha final no puede ser menor a la fecha inicial",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Modified, 0)]
        [DataRow(EstadoEntidad.Modified, -1)]
        [DataRow(EstadoEntidad.Deleted, 0)]
        [DataRow(EstadoEntidad.Deleted, -1)]
        public void RrhhMetGrabarCambios_SinIdNomina_RechazaOperacion(
            EstadoEntidad Estado, int Id)
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                Estado = Estado,
                IdNominaEncabezado = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe indicar una nómina válida para esta operación",
                Resultado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_EliminarNomina_IndicaQueNoEstaHabilitado()
        {
            // Arrange
            var Controlador = new ClsModeloNominaEncabezado
            {
                Estado = EstadoEntidad.Deleted,
                IdNominaEncabezado = 7
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "La eliminación de nóminas se habilitará cuando se defina el manejo del detalle",
                Resultado);
        }

        // ---------- CONTROLADOR: DETALLE ----------

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void RrhhMetGenerarDetalle_SinNomina_PideSeleccionarNomina(int Id)
        {
            // Arrange
            var Controlador = new ClsModeloNominaDetalle();

            // Act
            string Resultado = Controlador.RrhhMetGenerarDetalle(Id);

            // Assert
            Assert.AreEqual("Debe seleccionar una nómina", Resultado);
        }

        // ---------- CONTROLADOR: PERCEPCIONES Y DEDUCCIONES ----------

        [DataTestMethod]
        [DataRow(EstadoEntidad.Modified, 0)]
        [DataRow(EstadoEntidad.Modified, -1)]
        [DataRow(EstadoEntidad.Deleted, 0)]
        [DataRow(EstadoEntidad.Deleted, -1)]
        public void PercepcionDeduccion_SinId_RechazaModificarOEliminar(
            EstadoEntidad Estado, int Id)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccion
            {
                Estado = Estado,
                IdPercepcionDeduccion = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe indicar una percepción/deducción válida para esta operación",
                Resultado);
        }

        // ---------- CONTROLADOR: ASIGNACIÓN DE CONCEPTOS ----------

        [DataTestMethod]
        [DataRow(EstadoEntidad.Modified, 0)]
        [DataRow(EstadoEntidad.Modified, -1)]
        [DataRow(EstadoEntidad.Deleted, 0)]
        [DataRow(EstadoEntidad.Deleted, -1)]
        public void Asignacion_SinId_RechazaModificarOEliminar(
            EstadoEntidad Estado, int Id)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionEmpleado
            {
                Estado = Estado,
                IdPercepcionDeduccionEmpleado = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe indicar una asignación válida para esta operación",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void Asignacion_SinConcepto_PideSeleccionarConcepto(
            EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionEmpleado
            {
                Estado = Estado,
                IdPercepcionDeduccionEmpleado = 1,
                IdPercepcionDeduccion = 0
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe seleccionar una percepción/deducción",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void Asignacion_SinDestino_PideEmpleadoODepartamento(
            EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionEmpleado
            {
                Estado = Estado,
                IdPercepcionDeduccionEmpleado = 1,
                IdPercepcionDeduccion = 2
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe asignar la percepción/deducción a un empleado o a un departamento",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void Asignacion_AmbosDestinos_RechazaAsignacion(
            EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionEmpleado
            {
                Estado = Estado,
                IdPercepcionDeduccionEmpleado = 1,
                IdPercepcionDeduccion = 2,
                IdEmpleado = 3,
                IdDepartamento = 4
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "La percepción/deducción se asigna a un empleado o a un departamento, no a ambos",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Asignacion_IdsNoPositivos_SeNormalizanANull(int Id)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionEmpleado
            {
                Estado = EstadoEntidad.Added,
                IdPercepcionDeduccion = 2,
                IdEmpleado = Id,
                IdDepartamento = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.IsNull(Controlador.IdEmpleado);
            Assert.IsNull(Controlador.IdDepartamento);

            Assert.AreEqual(
                "Debe asignar la percepción/deducción a un empleado o a un departamento",
                Resultado);
        }

        // ---------- CONTROLADOR: RANGOS ----------

        [DataTestMethod]
        [DataRow(EstadoEntidad.Modified, 0)]
        [DataRow(EstadoEntidad.Modified, -1)]
        [DataRow(EstadoEntidad.Deleted, 0)]
        [DataRow(EstadoEntidad.Deleted, -1)]
        public void Rango_SinId_RechazaModificarOEliminar(
            EstadoEntidad Estado, int Id)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionRango
            {
                Estado = Estado,
                IdPercepcionDeduccionRango = Id
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe indicar un rango válido para esta operación",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void Rango_SinConcepto_PideSeleccionarConcepto(
            EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionRango
            {
                Estado = Estado,
                IdPercepcionDeduccionRango = 1,
                IdPercepcionDeduccion = 0
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Debe seleccionar una percepción/deducción",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added, 999)]
        [DataRow(EstadoEntidad.Added, 1000)]
        [DataRow(EstadoEntidad.Modified, 999)]
        [DataRow(EstadoEntidad.Modified, 1000)]
        public void Rango_LimiteFinalMenorOIgual_RechazaIntervalo(
            EstadoEntidad Estado, int Hasta)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionRango
            {
                Estado = Estado,
                IdPercepcionDeduccionRango = 1,
                IdPercepcionDeduccion = 2,
                SalarioDesdePercepcionDeduccionRango = 1000m,
                SalarioHastaPercepcionDeduccionRango = Hasta
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "El salario final debe ser mayor al salario inicial",
                Resultado);
        }

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void Rango_PorcentajeMayorACien_RechazaValor(
            EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = new ClsModeloPercepcionDeduccionRango
            {
                Estado = Estado,
                IdPercepcionDeduccionRango = 1,
                IdPercepcionDeduccion = 2,
                SalarioDesdePercepcionDeduccionRango = 0m,
                SalarioHastaPercepcionDeduccionRango = null,
                ModoValorPercepcionDeduccionRango = "%",
                ValorPercepcionDeduccionRango = 101m
            };

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual(
                "Cuando el modo es %, el valor no puede ser mayor a 100",
                Resultado);
        }

        // ---------- ENTIDAD ----------

        [TestMethod]
        public void EntidadEncabezado_AlAsignarValores_LosConserva()
        {
            // Arrange
            DateTime Fecha = new DateTime(2026, 10, 1);

            // Act
            var Entidad = new ClsNominaEncabezado
            {
                IdNominaEncabezado = 1,
                IdEmpleado = 5,
                FechaInicioNominaEncabezado = Fecha,
                FechaFinalNominaEncabezado = Fecha.AddDays(30),
                SalarioBaseNominaEncabezado = 4000m,
                DuracionHoraExtraNominaEncabezado = 120,
                TotalHorasExtraNominaEncabezado = 100m,
                TotalPercepcionNominaEncabezado = 250m,
                TotalDeduccionNominaEncabezado = 193.20m,
                TotalNetoNominaEncabezado = 4156.80m,
                EstadoNminaEncabezado = "Aprobada",
                CreatedAt = Fecha,
                UpdatedAt = Fecha
            };

            // Assert
            Assert.AreEqual(1, Entidad.IdNominaEncabezado);
            Assert.AreEqual(5, Entidad.IdEmpleado);
            Assert.AreEqual(Fecha, Entidad.FechaInicioNominaEncabezado);
            Assert.AreEqual(
                Fecha.AddDays(30),
                Entidad.FechaFinalNominaEncabezado);

            Assert.AreEqual(4000m, Entidad.SalarioBaseNominaEncabezado);
            Assert.AreEqual(120, Entidad.DuracionHoraExtraNominaEncabezado);
            Assert.AreEqual(100m, Entidad.TotalHorasExtraNominaEncabezado);
            Assert.AreEqual(250m, Entidad.TotalPercepcionNominaEncabezado);
            Assert.AreEqual(193.20m, Entidad.TotalDeduccionNominaEncabezado);
            Assert.AreEqual(4156.80m, Entidad.TotalNetoNominaEncabezado);
            Assert.AreEqual("Aprobada", Entidad.EstadoNminaEncabezado);
            Assert.AreEqual(Fecha, Entidad.CreatedAt);
            Assert.AreEqual(Fecha, Entidad.UpdatedAt);
        }

        // ---------- FORMULARIO ----------

        [TestMethod]
        [TestCategory("Formulario")]
        public void RrhhMetLimpiarMontos_ConDatosPrevios_RestableceMontos()
        {
            EjecutarEnSta(() =>
            {
                using (var Formulario = new FrmNominaEncabezado())
                {
                    // Arrange
                    string[] Nombres =
                    {
                        "RrhhTxtSalarioBase",
                        "RrhhTxtHorasExtra",
                        "RrhhTxtTotalPercepciones",
                        "RrhhTxtTotalDeducciones",
                        "RrhhTxtTotalNeto"
                    };

                    foreach (string Nombre in Nombres)
                    {
                        ObtenerControl<TextBox>(
                            Formulario, Nombre).Text = "999.99";
                    }

                    var Metodo = ObtenerMetodo("RrhhMetLimpiarMontos");

                    // Act
                    Metodo.Invoke(Formulario, null);

                    // Assert
                    foreach (string Nombre in Nombres)
                    {
                        Assert.AreEqual(
                            "0.00",
                            ObtenerControl<TextBox>(
                                Formulario, Nombre).Text,
                            Nombre);
                    }
                }
            });
        }

        [TestMethod]
        [TestCategory("Formulario")]
        public void RrhhMetConfigurarControles_Montos_SonSoloLectura()
        {
            EjecutarEnSta(() =>
            {
                // Arrange
                using (var Formulario = new FrmNominaEncabezado())
                {
                    var Metodo = ObtenerMetodo(
                        "RrhhMetConfigurarControles");

                    // Act
                    Metodo.Invoke(Formulario, null);

                    // Assert
                    string[] Nombres =
                    {
                        "RrhhTxtDepartamento",
                        "RrhhTxtSalarioBase",
                        "RrhhTxtHorasExtra",
                        "RrhhTxtTotalPercepciones",
                        "RrhhTxtTotalDeducciones",
                        "RrhhTxtTotalNeto"
                    };

                    foreach (string Nombre in Nombres)
                    {
                        Assert.IsTrue(
                            ObtenerControl<TextBox>(
                                Formulario, Nombre).ReadOnly,
                            Nombre);
                    }

                    var Estados = ObtenerControl<ComboBox>(
                        Formulario, "RrhhCboEstadoNomina");

                    Assert.AreEqual(
                        ComboBoxStyle.DropDownList,
                        Estados.DropDownStyle);

                    var Opciones = new List<object>();

                    foreach (object Item in Estados.Items)
                    {
                        Opciones.Add(Item);
                    }

                    CollectionAssert.AreEqual(
                        new object[] { "Aprobada", "Rechazada", "Pagada" },
                        Opciones);
                }
            });
        }

        [TestMethod]
        [TestCategory("Formulario")]
        public void RrhhMetConfigurarGrid_Lista_ImpideEdicionManual()
        {
            EjecutarEnSta(() =>
            {
                // Arrange
                using (var Formulario = new FrmNominaEncabezado())
                {
                    var Metodo = ObtenerMetodo("RrhhMetConfigurarGrid");

                    // Act
                    Metodo.Invoke(Formulario, null);

                    // Assert
                    var Grid = ObtenerControl<DataGridView>(
                        Formulario,
                        "RrhhDgvListaNominaEncabezado");

                    Assert.IsTrue(Grid.ReadOnly);
                    Assert.IsFalse(Grid.AllowUserToAddRows);
                    Assert.IsFalse(Grid.AllowUserToDeleteRows);
                    Assert.IsFalse(Grid.MultiSelect);
                    Assert.IsFalse(Grid.AutoGenerateColumns);

                    Assert.AreEqual(
                        DataGridViewSelectionMode.FullRowSelect,
                        Grid.SelectionMode);

                    Assert.AreEqual(12, Grid.Columns.Count);

                    Assert.AreEqual(
                        "TotalNetoNominaEncabezado",
                        Grid.Columns[
                            "TotalNetoNominaEncabezado"].DataPropertyName);
                }
            });
        }

        // ---------- AUXILIARES ----------

        private static MethodInfo ObtenerMetodo(string Nombre)
        {
            var Metodo = typeof(FrmNominaEncabezado).GetMethod(
                Nombre,
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(
                Metodo,
                "No existe el método " + Nombre);

            return Metodo;
        }

        private static T ObtenerControl<T>(
            Form Formulario, string Nombre) where T : Control
        {
            var Controles = Formulario.Controls.Find(Nombre, true);

            Assert.AreEqual(
                1,
                Controles.Length,
                "No se encontró un único control " + Nombre);

            Assert.IsInstanceOfType(Controles[0], typeof(T));

            return (T)Controles[0];
        }

        private static void EjecutarEnSta(Action Accion)
        {
            Exception Error = null;

            var Hilo = new Thread(() =>
            {
                try
                {
                    Accion();
                }
                catch (Exception Ex)
                {
                    Error = Ex;
                }
            });

            Hilo.IsBackground = true;
            Hilo.SetApartmentState(ApartmentState.STA);
            Hilo.Start();

            if (!Hilo.Join(TimeSpan.FromSeconds(30)))
            {
                Assert.Fail(
                    "La prueba del formulario superó 30 segundos.");
            }

            if (Error != null)
            {
                ExceptionDispatchInfo.Capture(Error).Throw();
            }
        }
    }

}
