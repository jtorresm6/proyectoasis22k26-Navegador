/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander Chiquito Paredes
 * Carné : 0901-23-3747
 * Fecha : 02/11/2026
 * ==================================================================
 * Propósito :
 *  Pruebas de la ventana 9102 de Horas Extra: cálculo del pago,
 *  duración sugerida y reglas del controlador. Ninguna necesita la
 *  BD ni sesión iniciada.
 * ===================================================================
*/

using System.Reflection;
using CapaControlador_Asistencia.Modelos_de_Controladores;
using CapaControlador_Seguridad;
using CapaModelo_Asistencia.Entidades;
using CapaVista_Asistencia.Formularios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MSTestHorasExtra
{
    [TestClass]
    public class TestHorasExtra
    {
        private static ClsModeloHoraExtra RrhhMetHoraExtraValida()
        {
            return new ClsModeloHoraExtra
            {
                IdEmpleado = 2,
                IdAsistenciaEmpleado = 10,
                IdPercepcionDeduccion = 1,
                Duracion = 90,
                ValorHora = 18.75m,
                PorcentajeRecargo = 0.50m,
                EstadoHoraExtra = "Pendiente",
                Observaciones = ""
            };
        }

        // ---------- CÁLCULOS

        [DataTestMethod]
        [DataRow(4500, 18.75)]
        [DataRow(3600, 15.00)]
        [DataRow(0, 0)]
        [DataRow(-100, 0)]
        public void RrhhMetCalcularValorHora_Salario_DevuelveSalarioEntre30Entre8(double Salario, double Esperado)
        {
            // Act
            decimal Resultado = ClsModeloHoraExtra.RrhhMetCalcularValorHora((decimal)Salario);

            // Assert
            Assert.AreEqual((decimal)Esperado, Resultado);
        }

        [DataTestMethod]
        [DataRow(90, 18.75, 0.50, 42.19)]
        [DataRow(60, 20.00, 0.50, 30.00)]
        [DataRow(120, 10.00, 0.00, 20.00)]
        [DataRow(30, 18.75, 1.00, 18.75)]
        public void RrhhMetCalcularTotal_DuracionValorYRecargo_CalculaElPago(int Minutos, double ValorHora, double Recargo, double Esperado)
        {
            // Act
            decimal Resultado = ClsModeloHoraExtra.RrhhMetCalcularTotal(Minutos, (decimal)ValorHora, (decimal)Recargo);

            // Assert
            Assert.AreEqual((decimal)Esperado, Resultado);
        }

        [DataTestMethod]
        [DataRow(0, 18.75)]
        [DataRow(90, 0)]
        public void RrhhMetCalcularTotal_SinDuracionOSinValor_DevuelveCero(int Minutos, double ValorHora)
        {
            // Act
            decimal Resultado = ClsModeloHoraExtra.RrhhMetCalcularTotal(Minutos, (decimal)ValorHora, 0.50m);

            // Assert
            Assert.AreEqual(0m, Resultado);
        }

        [DataTestMethod]
        [DataRow(18, 30, 17, 0, 90)]
        [DataRow(17, 0, 17, 0, 0)]
        [DataRow(16, 0, 17, 0, 0)]
        [DataRow(23, 59, 8, 0, 360)]
        public void RrhhMetDuracionSugerida_SalidaYFinDeJornada_DevuelveMinutosConTope(int HoraSalida, int MinSalida,
                                                                                      int HoraFin, int MinFin, int Esperado)
        {
            // Act
            int Resultado = ClsModeloHoraExtra.RrhhMetDuracionSugerida(new TimeSpan(HoraSalida, MinSalida, 0),
                                                                     new TimeSpan(HoraFin, MinFin, 0));

            // Assert
            Assert.AreEqual(Esperado, Resultado);
        }

        // ---------- CONTROLADOR

        [DataTestMethod]
        [DataRow(EstadoEntidad.Added)]
        [DataRow(EstadoEntidad.Modified)]
        public void RrhhMetGrabarCambios_SinEmpleado_PideSeleccionarEmpleado(EstadoEntidad Estado)
        {
            // Arrange
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.IdEmpleado = 0;
            Controlador.Estado = Estado;

            // Act
            string Resultado = Controlador.RrhhMetGrabarCambios();

            // Assert
            Assert.AreEqual("Debe seleccionar un empleado.", Resultado);
            Assert.IsFalse(Controlador.Grabado);
        }

        [TestMethod]
        public void RrhhMetValidarDatos_SinAsistencia_PideLaAsistencia()
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.IdAsistenciaEmpleado = 0;

            Assert.AreEqual("Seleccione la asistencia de donde sale la hora extra.", Controlador.RrhhMetValidarDatos());
        }

        [TestMethod]
        public void RrhhMetValidarDatos_SinConcepto_PideElConcepto()
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.IdPercepcionDeduccion = 0;

            Assert.AreEqual("Seleccione el concepto de pago.", Controlador.RrhhMetValidarDatos());
        }

        [DataTestMethod]
        [DataRow(0, "La duración debe ser mayor a 0 minutos.")]
        [DataRow(-5, "La duración debe ser mayor a 0 minutos.")]
        [DataRow(361, "La duración no puede pasar de 360 minutos.")]
        public void RrhhMetValidarDatos_DuracionFueraDeRango_NoSeAcepta(int Minutos, string Mensaje)
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.Duracion = Minutos;

            Assert.AreEqual(Mensaje, Controlador.RrhhMetValidarDatos());
        }

        [TestMethod]
        public void RrhhMetValidarDatos_EmpleadoSinSalario_AvisaQueNoSePuedeCalcular()
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.ValorHora = 0;

            Assert.AreEqual("El empleado no tiene salario registrado, no se puede calcular el valor de la hora.",
                            Controlador.RrhhMetValidarDatos());
        }

        [DataTestMethod]
        [DataRow(-0.10)]
        [DataRow(10.00)]
        public void RrhhMetValidarDatos_RecargoFueraDeRango_NoSeAcepta(double Recargo)
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.PorcentajeRecargo = (decimal)Recargo;

            Assert.AreEqual("El recargo debe estar entre 0 % y 999 %.", Controlador.RrhhMetValidarDatos());
        }

        [DataTestMethod]
        [DataRow("Pagada")]
        [DataRow("")]
        [DataRow("Otro")]
        public void RrhhMetValidarDatos_EstadoNoPermitido_PideEstadoValido(string Estado)
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.EstadoHoraExtra = Estado;

            Assert.AreEqual("Seleccione un estado válido.", Controlador.RrhhMetValidarDatos());
        }

        [TestMethod]
        public void RrhhMetValidarDatos_RechazadaSinMotivo_PideElMotivo()
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.EstadoHoraExtra = "Rechazada";
            Controlador.Observaciones = "  ";

            Assert.AreEqual("Escriba en observaciones el motivo del rechazo.", Controlador.RrhhMetValidarDatos());
        }

        [DataTestMethod]
        [DataRow("Pendiente", "")]
        [DataRow("Aprobada", "")]
        [DataRow("Rechazada", "No se autorizó")]
        public void RrhhMetValidarDatos_DatosCorrectos_NoDevuelveError(string Estado, string Observaciones)
        {
            var Controlador = RrhhMetHoraExtraValida();
            Controlador.EstadoHoraExtra = Estado;
            Controlador.Observaciones = Observaciones;

            Assert.IsNull(Controlador.RrhhMetValidarDatos());
        }

        // ---------- CONTROLADOR: propiedades ----------

        [TestMethod]
        public void Constructor_ValoresIniciales_RecargoDe50YPendiente()
        {
            // Arrange y Act
            var Controlador = new ClsModeloHoraExtra();

            // Assert
            Assert.AreEqual(0.50m, Controlador.PorcentajeRecargo);
            Assert.AreEqual("Pendiente", Controlador.EstadoHoraExtra);
            Assert.IsFalse(Controlador.Bloqueada);
            Assert.AreEqual(9102, ClsModeloHoraExtra.IdAplicacion);
            Assert.AreEqual(360, ClsModeloHoraExtra.DuracionMaxima);
        }

        [TestMethod]
        public void Bloqueada_EstadoPagada_NoSePuedeCambiar()
        {
            // Arrange y Act
            var Controlador = new ClsModeloHoraExtra { EstadoHoraExtra = "Pagada" };

            // Assert
            Assert.IsTrue(Controlador.Bloqueada);
        }

        // ---------- ENTIDAD ----------

        [TestMethod]
        public void Entidad_AlAsignarValores_LosConserva()
        {
            // Arrange y Act
            var Entidad = new ClsHoraExtra
            {
                IdHoraExtra = 4,
                IdEmpleado = 2,
                IdAsistenciaEmpleado = 10,
                DuracionHoraExtra = 90,
                ValorHoraOrdinariaHoraExtra = 18.75m,
                PorcentajeRecargoHoraExtra = 0.50m,
                TotalPagoHoraExtra = 42.19m,
                EstadoHoraExtra = "PENDIENTE"
            };

            // Assert
            Assert.AreEqual(4, Entidad.IdHoraExtra);
            Assert.AreEqual(90, Entidad.DuracionHoraExtra);
            Assert.AreEqual(42.19m, Entidad.TotalPagoHoraExtra);
            Assert.IsNull(Entidad.IdNominaEncabezado);
            Assert.IsNull(Entidad.IdUsuarioAprueba);
        }

        // ---------- FORMULARIO

        [DataTestMethod]
        [DataRow(90, "1:30")]
        [DataRow(45, "0:45")]
        [DataRow(360, "6:00")]
        public void RrhhMetTextoDuracion_Minutos_DevuelveHorasYMinutos(int Minutos, string Esperado)
        {
            // Arrange
            MethodInfo Metodo = typeof(FrmHorasExtra).GetMethod("RrhhMetTextoDuracion",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Act
            object Resultado = Metodo.Invoke(null, new object[] { Minutos });

            // Assert
            Assert.AreEqual(Esperado, Resultado);
        }
    }
}
