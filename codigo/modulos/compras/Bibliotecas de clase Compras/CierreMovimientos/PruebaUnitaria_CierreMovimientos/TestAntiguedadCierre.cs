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
 *  Verifica la clasificación de los documentos pendientes por rango de
 *  antigüedad (límites de cada rango, agrupación, nombres para el reporte
 *  y casos de error).
 *  Cada prueba sigue el patrón Arrange / Act / Assert.
 * ===================================================================
*/

namespace PruebaUnitaria_CierreMovimientos
{
    [TestClass]
    public class TestAntiguedadCierre
    {
        private static readonly DateTime _Corte = new DateTime(2026, 10, 31);

        private static List<ClsRangoAntiguedad> ComprasFuncRangos()
        {
            return new List<ClsRangoAntiguedad>
            {
                new ClsRangoAntiguedad { IdRango = 1, Nombre = "Por vencer", DiasDesde = null, DiasHasta = 0 },
                new ClsRangoAntiguedad { IdRango = 2, Nombre = "1 a 30 días", DiasDesde = 1, DiasHasta = 30 },
                new ClsRangoAntiguedad { IdRango = 3, Nombre = "31 a 60 días", DiasDesde = 31, DiasHasta = 60 },
                new ClsRangoAntiguedad { IdRango = 4, Nombre = "61 a 90 días", DiasDesde = 61, DiasHasta = 90 },
                new ClsRangoAntiguedad { IdRango = 5, Nombre = "Más de 90 días", DiasDesde = 91, DiasHasta = null }
            };
        }

        private static ClsDocumentoPendiente ComprasFuncDocumento(int IdProveedor, int DiasVencidos, decimal Saldo)
        {
            return new ClsDocumentoPendiente
            {
                IdProveedor = IdProveedor,
                NombreProveedor = "Proveedor " + IdProveedor,
                FechaVencimiento = _Corte.AddDays(-DiasVencidos),
                SaldoLocal = Saldo
            };
        }

        private static int ComprasFuncRangoDe(int DiasVencidos)
        {
            return ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(
                new[] { ComprasFuncDocumento(1, DiasVencidos, 100m) }, ComprasFuncRangos(), _Corte).Single().IdRango;
        }

        [TestMethod]
        public void ComprasProcRespetaLimitesDeCadaRango()
        {
            // Arrange
            int[] DiasVencidos = { -15, 0, 1, 30, 31, 90, 91, 400 };
            int[] RangosEsperados = { 1, 1, 2, 2, 3, 4, 5, 5 };

            // Act
            int[] RangosObtenidos = DiasVencidos.Select(ComprasFuncRangoDe).ToArray();

            // Assert
            CollectionAssert.AreEqual(RangosEsperados, RangosObtenidos);
        }

        [TestMethod]
        public void ComprasProcAgrupaPorProveedorYRango()
        {
            // Arrange
            var Documentos = new[]
            {
                ComprasFuncDocumento(1, 10, 9000m),
                ComprasFuncDocumento(1, 20, 350m),
                ComprasFuncDocumento(1, -5, 22500m),
                ComprasFuncDocumento(2, 45, 1200.50m)
            };

            // Act
            List<ClsAntiguedadCierre> Antiguedad =
                ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, ComprasFuncRangos(), _Corte);
            ClsAntiguedadCierre UnoATreinta = Antiguedad.Single(Fila => Fila.IdProveedor == 1 && Fila.IdRango == 2);

            // Assert
            Assert.AreEqual(3, Antiguedad.Count);
            Assert.AreEqual(9350m, UnoATreinta.Monto);
            Assert.AreEqual(2, UnoATreinta.CantidadDocumentos);
            Assert.AreEqual(1200.50m, Antiguedad.Single(Fila => Fila.IdProveedor == 2).Monto);
            Assert.AreEqual(Documentos.Sum(Documento => Documento.SaldoLocal), Antiguedad.Sum(Fila => Fila.Monto));
        }

        [TestMethod]
        public void ComprasProcConservaNombresParaElReporte()
        {
            // Arrange
            var Documentos = new[] { ComprasFuncDocumento(7, 45, 500m) };

            // Act
            ClsAntiguedadCierre Fila = ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(
                Documentos, ComprasFuncRangos(), _Corte).Single();

            // Assert
            Assert.AreEqual("Proveedor 7", Fila.NombreProveedor);
            Assert.AreEqual("31 a 60 días", Fila.NombreRango);
        }

        [TestMethod]
        public void ComprasProcIgnoraDocumentosSinSaldo()
        {
            // Arrange
            var Documentos = new[] { ComprasFuncDocumento(1, 10, 0m), ComprasFuncDocumento(1, 10, -50m) };

            // Act
            List<ClsAntiguedadCierre> Antiguedad =
                ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, ComprasFuncRangos(), _Corte);

            // Assert
            Assert.AreEqual(0, Antiguedad.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcSinRangosLanzaExcepcion()
        {
            // Arrange
            var Documentos = new[] { ComprasFuncDocumento(1, 10, 100m) };
            var SinRangos = new List<ClsRangoAntiguedad>();

            // Act
            ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, SinRangos, _Corte);

            // Assert: lo verifica ExpectedException (InvalidOperationException).
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcRangosConHuecoLanzaExcepcion()
        {
            // Arrange
            List<ClsRangoAntiguedad> Rangos = ComprasFuncRangos().Where(Rango => Rango.IdRango != 3).ToList();
            var Documentos = new[] { ComprasFuncDocumento(1, 45, 100m) };

            // Act
            ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, Rangos, _Corte);

            // Assert: lo verifica ExpectedException (InvalidOperationException).
        }
    }
}
