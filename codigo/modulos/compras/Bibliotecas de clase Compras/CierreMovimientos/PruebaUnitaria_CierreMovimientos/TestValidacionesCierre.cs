using System;
using System.Collections.Generic;
using System.Linq;
using CapaModelo_CierreMovimientos.Entidades;
using CapaModelo_CierreMovimientos.Servicios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/*
 * ==================================================================
 * Área : Compras
 * Autor : José Pablo Cano Cóbar
 * Carné : 0901-23-1727
 * Fecha : 04/11/2026 - 11/11/2026 (AAA)
 * ==================================================================
 * Propósito :
 *  Verifica las validaciones previas del cierre: qué casos bloquean el cierre
 *  y cuáles solo generan advertencias.
 *  Cada prueba sigue el patrón Arrange / Act / Assert.
 * ===================================================================
*/

namespace PruebaUnitaria_CierreMovimientos
{
    [TestClass]
    public class TestValidacionesCierre
    {
        private static readonly DateTime _Hoy = new DateTime(2026, 11, 4);

        private static ClsPeriodoCompras ComprasFuncOctubre(string Estado = "Abierto")
        {
            return new ClsPeriodoCompras
            {
                IdPeriodo = 1, Anio = 2026, Mes = 10, Estado = Estado,
                FechaInicio = new DateTime(2026, 10, 1), FechaFin = new DateTime(2026, 10, 31)
            };
        }

        private static ClsIndicadoresCierre ComprasFuncIndicadoresLimpios()
        {
            return new ClsIndicadoresCierre { RangosAntiguedadActivos = 5 };
        }

        private static List<ClsValidacionCierre> ComprasFuncEvaluar(ClsIndicadoresCierre Indicadores,
            ClsPeriodoCompras Periodo = null, int Proveedores = 3, bool HaySesion = true, DateTime? Hoy = null)
        {
            return ClsReglasCierreMovimientos.ComprasFuncEvaluarValidaciones(Periodo ?? ComprasFuncOctubre(),
                Indicadores, Proveedores, HaySesion, Hoy ?? _Hoy);
        }

        private static int ComprasFuncContar(IEnumerable<ClsValidacionCierre> Validaciones, ComprasEnumEstadoValidacion Estado)
        {
            return Validaciones.Count(Validacion => Validacion.Estado == Estado);
        }

        [TestMethod]
        public void ComprasProcSinPendientesPermiteCerrar()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();

            // Act
            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(Indicadores);
            bool PuedeCerrar = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones);

            // Assert
            Assert.IsTrue(PuedeCerrar);
            Assert.AreEqual(0, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Bloqueante));
            Assert.AreEqual(0, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Advertencia));
        }

        [TestMethod]
        public void ComprasProcPagosProgramadosBloquean()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.PagosProgramados = 2;
            Indicadores.MontoPagosProgramados = 6000m;

            // Act
            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(Indicadores);
            ClsValidacionCierre Pagos = Validaciones.Single(Validacion => Validacion.Origen == "3.5 Pagos a proveedores");

            // Assert
            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones));
            Assert.AreEqual(ComprasEnumEstadoValidacion.Bloqueante, Pagos.Estado);
            StringAssert.Contains(Pagos.Validacion, "2 pago(s)");
        }

        [TestMethod]
        public void ComprasProcPeriodoAnteriorAbiertoBloquea()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.PeriodosAbiertosAnteriores = 1;

            // Act
            bool PuedeCerrar = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(ComprasFuncEvaluar(Indicadores));

            // Assert
            Assert.IsFalse(PuedeCerrar);
        }

        [TestMethod]
        public void ComprasProcMesSinTerminarBloquea()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();

            // Act
            bool UltimoDiaDelMes = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(Indicadores, Hoy: new DateTime(2026, 10, 31)));
            bool DiaSiguiente = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(Indicadores, Hoy: new DateTime(2026, 11, 1)));

            // Assert
            Assert.IsFalse(UltimoDiaDelMes);
            Assert.IsTrue(DiaSiguiente);
        }

        [TestMethod]
        public void ComprasProcPeriodoCerradoBloquea()
        {
            // Arrange
            ClsPeriodoCompras Cerrado = ComprasFuncOctubre("Cerrado");

            // Act
            bool PuedeCerrar = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(ComprasFuncIndicadoresLimpios(), Cerrado));

            // Assert
            Assert.IsFalse(PuedeCerrar);
        }

        [TestMethod]
        public void ComprasProcSinRangosDeAntiguedadBloquea()
        {
            // Arrange
            var Indicadores = new ClsIndicadoresCierre { RangosAntiguedadActivos = 0 };

            // Act
            bool PuedeCerrar = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(ComprasFuncEvaluar(Indicadores));

            // Assert
            Assert.IsFalse(PuedeCerrar);
        }

        [TestMethod]
        public void ComprasProcAdvertenciasNoBloquean()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.OrdenesPendientes = 2;
            Indicadores.ComprasSinCuentaPorPagar = 1;
            Indicadores.DocumentosMonedaExtranjera = 3;

            // Act
            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(Indicadores, Proveedores: 0, HaySesion: false);

            // Assert
            Assert.IsTrue(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones));
            Assert.AreEqual(5, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Advertencia));
        }

        [TestMethod]
        public void ComprasProcSinValidacionesNoPermiteCerrar()
        {
            // Arrange
            List<ClsValidacionCierre> SinValidaciones = null;

            // Act
            bool PuedeCerrar = ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(SinValidaciones);

            // Assert
            Assert.IsFalse(PuedeCerrar);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComprasProcPeriodoNuloLanzaExcepcion()
        {
            // Arrange
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();

            // Act
            ClsReglasCierreMovimientos.ComprasFuncEvaluarValidaciones(null, Indicadores, 0, true, _Hoy);

            // Assert: lo verifica ExpectedException (ArgumentNullException).
        }
    }
}
