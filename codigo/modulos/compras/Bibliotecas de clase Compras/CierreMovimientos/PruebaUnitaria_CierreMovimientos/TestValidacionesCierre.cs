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
 * Fecha : 04/11/2026
 * ==================================================================
 * Propósito :
 *  Verifica las validaciones previas del cierre: qué casos bloquean el cierre
 *  y cuáles solo generan advertencias.
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
            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(ComprasFuncIndicadoresLimpios());

            Assert.IsTrue(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones));
            Assert.AreEqual(0, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Bloqueante));
            Assert.AreEqual(0, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Advertencia));
        }

        [TestMethod]
        public void ComprasProcPagosProgramadosBloquean()
        {
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.PagosProgramados = 2;
            Indicadores.MontoPagosProgramados = 6000m;

            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(Indicadores);

            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones));
            ClsValidacionCierre Pagos = Validaciones.Single(Validacion => Validacion.Origen == "3.5 Pagos a proveedores");
            Assert.AreEqual(ComprasEnumEstadoValidacion.Bloqueante, Pagos.Estado);
            StringAssert.Contains(Pagos.Validacion, "2 pago(s)");
        }

        [TestMethod]
        public void ComprasProcPeriodoAnteriorAbiertoBloquea()
        {
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.PeriodosAbiertosAnteriores = 1;

            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(ComprasFuncEvaluar(Indicadores)));
        }

        [TestMethod]
        public void ComprasProcMesSinTerminarBloquea()
        {
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();

            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(Indicadores, Hoy: new DateTime(2026, 10, 31))));
            Assert.IsTrue(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(Indicadores, Hoy: new DateTime(2026, 11, 1))));
        }

        [TestMethod]
        public void ComprasProcPeriodoCerradoBloquea()
        {
            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(ComprasFuncIndicadoresLimpios(), ComprasFuncOctubre("Cerrado"))));
        }

        [TestMethod]
        public void ComprasProcSinRangosDeAntiguedadBloquea()
        {
            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(
                ComprasFuncEvaluar(new ClsIndicadoresCierre { RangosAntiguedadActivos = 0 })));
        }

        [TestMethod]
        public void ComprasProcAdvertenciasNoBloquean()
        {
            ClsIndicadoresCierre Indicadores = ComprasFuncIndicadoresLimpios();
            Indicadores.OrdenesPendientes = 2;
            Indicadores.ComprasSinCuentaPorPagar = 1;
            Indicadores.DocumentosMonedaExtranjera = 3;

            List<ClsValidacionCierre> Validaciones = ComprasFuncEvaluar(Indicadores, Proveedores: 0, HaySesion: false);

            Assert.IsTrue(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(Validaciones));
            Assert.AreEqual(5, ComprasFuncContar(Validaciones, ComprasEnumEstadoValidacion.Advertencia));
        }

        [TestMethod]
        public void ComprasProcSinValidacionesNoPermiteCerrar()
        {
            Assert.IsFalse(ClsReglasCierreMovimientos.ComprasFuncPuedeCerrar(null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ComprasProcPeriodoNuloLanzaExcepcion()
        {
            ClsReglasCierreMovimientos.ComprasFuncEvaluarValidaciones(null, ComprasFuncIndicadoresLimpios(), 0, true, _Hoy);
        }
    }
}
