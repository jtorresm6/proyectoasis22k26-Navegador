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
 *  Verifica la clasificación de los documentos pendientes por rango de
 *  antigüedad (límites de cada rango, agrupación y casos de error).
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
            Assert.AreEqual(1, ComprasFuncRangoDe(-15));
            Assert.AreEqual(1, ComprasFuncRangoDe(0));
            Assert.AreEqual(2, ComprasFuncRangoDe(1));
            Assert.AreEqual(2, ComprasFuncRangoDe(30));
            Assert.AreEqual(3, ComprasFuncRangoDe(31));
            Assert.AreEqual(4, ComprasFuncRangoDe(90));
            Assert.AreEqual(5, ComprasFuncRangoDe(91));
            Assert.AreEqual(5, ComprasFuncRangoDe(400));
        }

        [TestMethod]
        public void ComprasProcAgrupaPorProveedorYRango()
        {
            var Documentos = new[]
            {
                ComprasFuncDocumento(1, 10, 9000m),
                ComprasFuncDocumento(1, 20, 350m),
                ComprasFuncDocumento(1, -5, 22500m),
                ComprasFuncDocumento(2, 45, 1200.50m)
            };

            List<ClsAntiguedadCierre> Antiguedad =
                ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, ComprasFuncRangos(), _Corte);

            Assert.AreEqual(3, Antiguedad.Count);
            ClsAntiguedadCierre UnoATreinta = Antiguedad.Single(Fila => Fila.IdProveedor == 1 && Fila.IdRango == 2);
            Assert.AreEqual(9350m, UnoATreinta.Monto);
            Assert.AreEqual(2, UnoATreinta.CantidadDocumentos);
            Assert.AreEqual(1200.50m, Antiguedad.Single(Fila => Fila.IdProveedor == 2).Monto);
            Assert.AreEqual(Documentos.Sum(Documento => Documento.SaldoLocal), Antiguedad.Sum(Fila => Fila.Monto));
        }

        [TestMethod]
        public void ComprasProcIgnoraDocumentosSinSaldo()
        {
            var Documentos = new[] { ComprasFuncDocumento(1, 10, 0m), ComprasFuncDocumento(1, 10, -50m) };

            Assert.AreEqual(0, ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(Documentos, ComprasFuncRangos(), _Corte).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcSinRangosLanzaExcepcion()
        {
            ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(
                new[] { ComprasFuncDocumento(1, 10, 100m) }, new List<ClsRangoAntiguedad>(), _Corte);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ComprasProcRangosConHuecoLanzaExcepcion()
        {
            List<ClsRangoAntiguedad> Rangos = ComprasFuncRangos().Where(Rango => Rango.IdRango != 3).ToList();
            ClsReglasCierreMovimientos.ComprasFuncCalcularAntiguedad(new[] { ComprasFuncDocumento(1, 45, 100m) }, Rangos, _Corte);
        }
    }
}
