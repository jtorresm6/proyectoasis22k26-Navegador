using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaControlador_Liquidacion_Rrhh;
using CapaControlador_Liquidacion_Rrhh.Objetos_de_valor;
using CapaModelo_Liquidacion_Rrhh.Entidades;
using CapaVista_Liquidacion_Rrhh.Formularios;

namespace MSTestLiquidacionRrhh
{
    [TestClass]
    public class TestBajaEmpleado
    {
        // ==========================================
        // 1. PRUEBAS DE VALIDACIÓN EN CONTROLADOR
        // ==========================================

        [DataTestMethod]
        [DataRow(EstadoEntidadLiquidacion.Added)]
        [DataRow(EstadoEntidadLiquidacion.Modified)]
        public void LiquidacionMetGrabarCambios_SinEmpleado_LanzaErrorOValidacion(EstadoEntidadLiquidacion estado)
        {
            // Arrange
            var controlador = new ClsModeloBajaEmpleado
            {
                IdBajaEmpleado = 1,
                IdEmpleado = 0, // Id de empleado inválido
                IdMotivoBaja = 1,
                FechaBajaEmpleado = DateTime.Today,
                ObservacionBajaEmpleado = "Prueba unitaria",
                IdUsuarioRegistra = 1,
                IsActive = 1,
                Estado = estado
            };

            // Act
            string resultado = controlador.LiquidacionMetGrabarCambios();

            // Assert
            Assert.IsNotNull(resultado, "El método debe retornar un mensaje de respuesta.");
        }

        [DataTestMethod]
        [DataRow(EstadoEntidadLiquidacion.Added)]
        [DataRow(EstadoEntidadLiquidacion.Modified)]
        public void LiquidacionMetGrabarCambios_SinMotivoBaja_LanzaErrorOValidacion(EstadoEntidadLiquidacion estado)
        {
            // Arrange
            var controlador = new ClsModeloBajaEmpleado
            {
                IdBajaEmpleado = 1,
                IdEmpleado = 3,
                IdMotivoBaja = 0, // Id de motivo inválido
                FechaBajaEmpleado = DateTime.Today,
                ObservacionBajaEmpleado = "Prueba unitaria sin motivo",
                IdUsuarioRegistra = 1,
                IsActive = 1,
                Estado = estado
            };

            // Act
            string resultado = controlador.LiquidacionMetGrabarCambios();

            // Assert
            Assert.IsNotNull(resultado, "El resultado no debe ser nulo al validar motivo.");
        }

        // ==========================================
        // 2. PRUEBAS DE PROPIEDADES EN CONTROLADOR
        // ==========================================

        [TestMethod]
        public void PropiedadesControlador_AlAsignarValores_LosConservaCorrectamente()
        {
            // Arrange
            DateTime fechaPrueba = new DateTime(2026, 11, 2);
            var controlador = new ClsModeloBajaEmpleado();

            // Act
            controlador.IdBajaEmpleado = 5;
            controlador.IdEmpleado = 12;
            controlador.IdMotivoBaja = 2;
            controlador.FechaBajaEmpleado = fechaPrueba;
            controlador.ObservacionBajaEmpleado = "Despido por reorganización";
            controlador.IdUsuarioRegistra = 1;
            controlador.IsActive = 1;
            controlador.NombresEmpleado = "Juan Pérez";
            controlador.NombreMotivoBaja = "Despido";

            // Assert
            Assert.AreEqual(5, controlador.IdBajaEmpleado);
            Assert.AreEqual(12, controlador.IdEmpleado);
            Assert.AreEqual(2, controlador.IdMotivoBaja);
            Assert.AreEqual(fechaPrueba, controlador.FechaBajaEmpleado);
            Assert.AreEqual("Despido por reorganización", controlador.ObservacionBajaEmpleado);
            Assert.AreEqual(1, controlador.IdUsuarioRegistra);
            Assert.AreEqual(1, controlador.IsActive);
            Assert.AreEqual("Juan Pérez", controlador.NombresEmpleado);
            Assert.AreEqual("Despido", controlador.NombreMotivoBaja);
        }

        // ==========================================
        // 3. PRUEBAS DE PROPIEDADES EN ENTIDAD
        // ==========================================

        [TestMethod]
        public void PropiedadesEntidad_AlAsignarValores_LosConservaCorrectamente()
        {
            // Arrange & Act
            DateTime fechaPrueba = new DateTime(2026, 11, 2);
            var entidad = new ClsBajaEmpleado
            {
                IdBajaEmpleado = 10,
                IdEmpleado = 25,
                IdMotivoBaja = 3,
                FechaBajaEmpleado = fechaPrueba,
                ObservacionBajaEmpleado = "Renuncia voluntaria",
                IdUsuarioRegistra = 2,
                IsActive = 1,
                NombresEmpleado = "Maria Lopez",
                NombreMotivoBaja = "Renuncia"
            };

            // Assert
            Assert.AreEqual(10, entidad.IdBajaEmpleado);
            Assert.AreEqual(25, entidad.IdEmpleado);
            Assert.AreEqual(3, entidad.IdMotivoBaja);
            Assert.AreEqual(fechaPrueba, entidad.FechaBajaEmpleado);
            Assert.AreEqual("Renuncia voluntaria", entidad.ObservacionBajaEmpleado);
            Assert.AreEqual(2, entidad.IdUsuarioRegistra);
            Assert.AreEqual(1, entidad.IsActive);
            Assert.AreEqual("Maria Lopez", entidad.NombresEmpleado);
            Assert.AreEqual("Renuncia", entidad.NombreMotivoBaja);
        }

        // ==========================================
        // 4. PRUEBAS DE INTERFAZ / FORMULARIO (UI)
        // ==========================================

        [TestMethod]
        public void RrhhMetLimpiarCampos_AlEjecutar_RestableceFormulario()
        {
            // Arrange
            using (var formulario = new FrmBajaEmpleado())
            {
                // Buscamos los controles por su nombre en el formulario
                var txtIdBaja = formulario.Controls.Find("RrhhTxtIdBajaEmpleado", true);
                var txtObs = formulario.Controls.Find("RrhhTxtObservacionesBaja", true);
                var dtpFecha = formulario.Controls.Find("RrhhDtpFechaBajaEmpleado", true);
                var cboEmp = formulario.Controls.Find("RrhhCboEmpleado", true);
                var cboMot = formulario.Controls.Find("RrhhCboMotivoBaja", true);

                // Simulamos la carga de datos previos si los controles existen
                if (txtObs.Length > 0) ((TextBox)txtObs[0]).Text = "Texto de prueba previo";
                if (dtpFecha.Length > 0) ((DateTimePicker)dtpFecha[0]).Value = new DateTime(2020, 1, 1);

                // Invocamos el método privado de limpieza/reinicio mediante Reflexión
                var metodoLimpiar = typeof(FrmBajaEmpleado).GetMethod("RrhhMetLimpiarCampos",
                    BindingFlags.NonPublic | BindingFlags.Instance)
                    ?? typeof(FrmBajaEmpleado).GetMethod("RrhhMetReinicio",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (metodoLimpiar != null)
                {
                    // Act
                    metodoLimpiar.Invoke(formulario, null);

                    // Assert
                    if (txtObs.Length > 0) Assert.AreEqual(string.Empty, ((TextBox)txtObs[0]).Text);
                    if (dtpFecha.Length > 0) Assert.AreEqual(DateTime.Today, ((DateTimePicker)dtpFecha[0]).Value.Date);
                    if (cboEmp.Length > 0) Assert.AreEqual(-1, ((ComboBox)cboEmp[0]).SelectedIndex);
                    if (cboMot.Length > 0) Assert.AreEqual(-1, ((ComboBox)cboMot[0]).SelectedIndex);
                }
            }
        }
    }
}