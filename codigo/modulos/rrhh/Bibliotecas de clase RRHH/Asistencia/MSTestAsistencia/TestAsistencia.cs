/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas de la ventana 9101 de Asistencia: reglas del controlador
 *  y cálculos de la vista. Ninguna necesita la BD ni sesión iniciada.
 * ===================================================================
*/

using System.Drawing;
using System.Reflection;
using CapaControlador_Asistencia.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using CapaModelo_Asistencia.Entidades;
using CapaVista_Asistencia.Formularios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MSTestAsistencia
{
    [TestClass]
    public class TestAsistencia
    {
        private static ClsModeloAsistencia RrhhMetAsistenciaValida()
        {
            return new ClsModeloAsistencia
            {
                IdEmpleado = 2,
                FechaAsistencia = DateTime.Today,
                HoraEntrada = new TimeSpan(8, 0, 0),
                HoraSalida = new TimeSpan(17, 0, 0),
                EstadoAsistencia = "Presente",
                Observaciones = ""
            };
        }

        // ---------- CONTROLADOR

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void RrhhMetGrabarCambios_SinEmpleado_PideSeleccionarEmpleado(EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.IdEmpleado = 0;
            Controlador.Estado = Estado;

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe seleccionar un empleado.", Resultado);
            Assert.IsFalse(Controlador.Grabado);
        }

        [TestMethod]
        public void RrhhMetGrabarCambios_FechaFutura_NoSeAcepta()
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.FechaAsistencia = DateTime.Today.AddDays(1);
            Controlador.Estado = EstadoEntidad.Added;

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("La fecha no puede ser futura.", Resultado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_SalidaSinEntrada_PideLaEntrada()
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.HoraEntrada = null;

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("Marque primero la hora de entrada.", Resultado);
        }

        [DataTestMethod]
        [DataRow(17, 0)]
        [DataRow(7, 30)]
        public void RrhhMetValidarDatos_SalidaNoDespuesDeEntrada_NoSeAcepta(int Hora, int Minuto)
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.HoraEntrada = new TimeSpan(17, 0, 0);
            Controlador.HoraSalida = new TimeSpan(Hora, Minuto, 0);

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("La hora de salida debe ser después de la hora de entrada.", Resultado);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Vacaciones")]
        [DataRow(null)]
        public void RrhhMetValidarDatos_EstadoInvalido_PideEstadoValido(string Estado)
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.EstadoAsistencia = Estado;

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("Seleccione un estado válido.", Resultado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_TardanzaSinEntrada_NoSeAcepta()
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.HoraEntrada = null;
            Controlador.HoraSalida = null;
            Controlador.EstadoAsistencia = "Tardanza";

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("Para marcar tardanza se necesita la hora de entrada.", Resultado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_AusenteConEntrada_NoSeAcepta()
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.HoraSalida = null;
            Controlador.EstadoAsistencia = "Ausente";

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("Si el empleado está ausente no lleva hora de entrada.", Resultado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_ObservacionesMuyLargas_NoSeAceptan()
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.Observaciones = new string('x', 256);

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.AreEqual("Las observaciones no pueden pasar de 255 caracteres.", Resultado);
        }

        [DataTestMethod]
        [DataRow("Presente")]
        [DataRow("PRESENTE")]
        [DataRow("Tardanza")]
        public void RrhhMetValidarDatos_DatosCorrectos_NoDevuelveError(string Estado)
        {
            // Arrange
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.EstadoAsistencia = Estado;

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_PresenteSinHoras_SeAceptaPorCapacitacion()
        {
            // Arrange: en días de capacitación se marca Presente sin hora de entrada
            var Controlador = RrhhMetAsistenciaValida();
            Controlador.HoraEntrada = null;
            Controlador.HoraSalida = null;

            // Act
            string Resultado = Controlador.RrhhMetValidarDatos();

            // Assert
            Assert.IsNull(Resultado);
        }

        // ---------- CONTROLADOR: propiedades ----------

        [TestMethod]
        public void Propiedades_AlAsignarValores_LosConservan()
        {
            // Arrange
            var Controlador = new ClsModeloAsistencia();

            // Act
            Controlador.IdAsistenciaEmpleado = 1;
            Controlador.IdEmpleado = 2;
            Controlador.NombreEmpleado = "Carlos";
            Controlador.FechaAsistencia = new DateTime(2026, 11, 2);
            Controlador.HoraEntrada = new TimeSpan(8, 2, 0);
            Controlador.HoraSalida = null;
            Controlador.EstadoAsistencia = "Tardanza";

            // Assert
            Assert.AreEqual(1, Controlador.IdAsistenciaEmpleado);
            Assert.AreEqual(2, Controlador.IdEmpleado);
            Assert.AreEqual("Carlos", Controlador.NombreEmpleado);
            Assert.AreEqual(new DateTime(2026, 11, 2), Controlador.FechaAsistencia);
            Assert.AreEqual(new TimeSpan(8, 2, 0), Controlador.HoraEntrada);
            Assert.IsNull(Controlador.HoraSalida);
            Assert.AreEqual("Tardanza", Controlador.EstadoAsistencia);
            Assert.AreEqual(9000, ClsModeloAsistencia.IdModulo);
            Assert.AreEqual(9101, ClsModeloAsistencia.IdAplicacion);
        }

        // ---------- ENTIDAD ----------

        [TestMethod]
        public void Entidad_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Entidad = new ClsAsistenciaEmpleado
            {
                IdAsistenciaEmpleado = 10,
                IdEmpleado = 20,
                FechaAsistenciaEmpleado = new DateTime(2026, 11, 2),
                HoraEntradaAsistenciaEmpleado = new TimeSpan(8, 0, 0),
                EstadoAsistenciaEmpleado = "PRESENTE"
            };

            // Assert
            Assert.AreEqual(10, Entidad.IdAsistenciaEmpleado);
            Assert.AreEqual(20, Entidad.IdEmpleado);
            Assert.AreEqual(new DateTime(2026, 11, 2), Entidad.FechaAsistenciaEmpleado);
            Assert.AreEqual(new TimeSpan(8, 0, 0), Entidad.HoraEntradaAsistenciaEmpleado);
            Assert.IsNull(Entidad.HoraSalidaAsistenciaEmpleado);
            Assert.AreEqual("PRESENTE", Entidad.EstadoAsistenciaEmpleado);
        }

        // ---------- FORMULARIO

        private static object RrhhMetInvocarEstatico(string Metodo, params object[] Parametros)
        {
            MethodInfo Info = typeof(FrmAsistencia).GetMethod(Metodo, BindingFlags.NonPublic | BindingFlags.Static);
            return Info.Invoke(null, Parametros);
        }

        [TestMethod]
        public void RrhhMetHorasTrabajadas_EntradaYSalida_DevuelveHorasYMinutos()
        {
            // Act
            object Resultado = RrhhMetInvocarEstatico("RrhhMetHorasTrabajadas",
                                                      new TimeSpan(8, 15, 0), new TimeSpan(17, 45, 0));

            // Assert
            Assert.AreEqual("9:30", Resultado);
        }

        [TestMethod]
        public void RrhhMetHorasTrabajadas_SinEntrada_DevuelveCero()
        {
            // Act
            object Resultado = RrhhMetInvocarEstatico("RrhhMetHorasTrabajadas", DBNull.Value, DBNull.Value);

            // Assert
            Assert.AreEqual("0:00", Resultado);
        }

        [TestMethod]
        public void RrhhMetHorasTrabajadas_SinSalida_DevuelveGuion()
        {
            // Act
            object Resultado = RrhhMetInvocarEstatico("RrhhMetHorasTrabajadas", new TimeSpan(8, 0, 0), DBNull.Value);

            // Assert
            Assert.AreEqual("-", Resultado);
        }

        [DataTestMethod]
        [DataRow("Presente", 214, 242, 224)]
        [DataRow("Tardanza", 252, 235, 201)]
        [DataRow("Ausente", 248, 215, 218)]
        [DataRow("Permiso", 221, 235, 247)]
        public void RrhhMetColorEstado_CadaEstado_TieneSuColor(string Estado, int R, int G, int B)
        {
            // Act
            var Color = (Color)RrhhMetInvocarEstatico("RrhhMetColorEstado", Estado);

            // Assert
            Assert.AreEqual(System.Drawing.Color.FromArgb(R, G, B).ToArgb(), Color.ToArgb());
        }
    }
}
