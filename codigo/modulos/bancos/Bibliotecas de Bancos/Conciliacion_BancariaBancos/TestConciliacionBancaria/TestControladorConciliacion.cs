/*
 * ================================================================
 * Área   : Bancos
 * Autor  : Evelyn Sofía Andrade Luna
 * Carné  : 9959-23-1224
 * Fecha  : 12/11/2026
 * ================================================================
 * Propósito :
 *  PRUEBAS UNITARIAS DE CONCILIACION BANCARIA
 * ================================================================
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CapaControlador_ConciliacionBancaria;
using CapaModelo_ConciliacionBancaria.Entidades;
using CapaModelo_ConciliacionBancaria.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConciliacionBancaria
{
    [TestClass]
    public class TestMotorConciliacion
    {
        private static ClsMovimientoLibro Libro(long Id, string Referencia, decimal Importe, int Sentido, int Dia)
        {
            return new ClsMovimientoLibro
            {
                IdMovimiento = Id,
                Referencia = Referencia,
                Concepto = "Movimiento " + Id,
                Importe = Importe,
                Sentido = Sentido,
                FechaOperacion = new DateTime(2026, 2, Dia),
                FechaValor = new DateTime(2026, 2, Dia)
            };
        }

        private static ClsLineaExtracto Banco(long Id, string Referencia, decimal Importe, int Sentido, int Dia)
        {
            return new ClsLineaExtracto
            {
                IdLinea = Id,
                Identificador = "L" + Id,
                Referencia = Referencia,
                Concepto = "Línea " + Id,
                Importe = Importe,
                Sentido = Sentido,
                FechaOperacion = new DateTime(2026, 2, Dia),
                FechaValor = new DateTime(2026, 2, Dia)
            };
        }

        [TestMethod]
        public void Test_EmparejaPorReferenciaEImporteIguales()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "CH-100", 500m, -1, 5) },
                new[] { Banco(10, "ch-100", 500m, -1, 9) },
                null);

            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(ClsValoresConciliacion.ResultadoCoincide, Resultado[0].Resultado);
            Assert.AreEqual(ClsValoresConciliacion.MetodoAutomatica, Resultado[0].Metodo);
            Assert.AreEqual(0m, Resultado[0].Diferencia);
            Assert.AreEqual(1, Resultado[0].Movimiento.IdMovimiento);
            Assert.AreEqual(10, Resultado[0].Linea.IdLinea);
        }

        [TestMethod]
        public void Test_ReferenciaIgualConImporteDistintoGeneraDiferencia()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "TR-7", 1000m, 1, 5) },
                new[] { Banco(10, "TR-7", 950m, 1, 5) },
                null);

            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(ClsValoresConciliacion.ResultadoDiferencia, Resultado[0].Resultado);
            Assert.AreEqual(-50m, Resultado[0].Diferencia);
            Assert.AreEqual(ClsMotorConciliacion.MotivoImporteDistinto, Resultado[0].Motivo);
        }

        [TestMethod]
        public void Test_SinReferenciaEmparejaPorImporteYFechaCercana()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, null, 250m, 1, 10) },
                new[] { Banco(10, null, 250m, 1, 12) },
                null);

            Assert.AreEqual(1, Resultado.Count);
            Assert.AreEqual(ClsValoresConciliacion.ResultadoCoincide, Resultado[0].Resultado);
        }

        [TestMethod]
        public void Test_FechaFueraDeToleranciaNoEmpareja()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, null, 250m, 1, 1) },
                new[] { Banco(10, null, 250m, 1, 1 + ClsMotorConciliacion.ToleranciaDias + 1) },
                null);

            Assert.AreEqual(2, Resultado.Count);
            Assert.IsTrue(Resultado.All(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente));
        }

        [TestMethod]
        public void Test_SentidoDistintoNoEmpareja()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, null, 300m, 1, 5) },
                new[] { Banco(10, null, 300m, -1, 5) },
                null);

            Assert.AreEqual(2, Resultado.Count);
            Assert.IsTrue(Resultado.All(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente));
        }

        [TestMethod]
        public void Test_ReferenciasDistintasNoEmparejanAunqueCoincidaElImporte()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A-1", 300m, 1, 5) },
                new[] { Banco(10, "B-2", 300m, 1, 5) },
                null);

            Assert.AreEqual(2, Resultado.Count);
            Assert.IsTrue(Resultado.All(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente));
        }

        [TestMethod]
        public void Test_CadaLineaDelBancoSeUsaUnaSolaVez()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, null, 100m, 1, 5), Libro(2, null, 100m, 1, 6) },
                new[] { Banco(10, null, 100m, 1, 5) },
                null);

            Assert.AreEqual(2, Resultado.Count);
            Assert.AreEqual(1, Resultado.Count(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoCoincide));
            Assert.AreEqual(1, Resultado.Count(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoPendiente));
            Assert.AreEqual(1, Resultado.First(Detalle => Detalle.Resultado == ClsValoresConciliacion.ResultadoCoincide).Movimiento.IdMovimiento);
        }

        [TestMethod]
        public void Test_PartidasSinParejaQuedanPendientesConSuMotivo()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "X-1", 80m, -1, 5) },
                new[] { Banco(10, "Y-9", 15m, -1, 20) },
                null);

            ClsDetalleConciliacion SoloLibro = Resultado.Single(Detalle => Detalle.Movimiento != null);
            ClsDetalleConciliacion SoloBanco = Resultado.Single(Detalle => Detalle.Linea != null);
            Assert.AreEqual(ClsMotorConciliacion.MotivoMovimientoSinBanco, SoloLibro.Motivo);
            Assert.AreEqual(ClsMotorConciliacion.MotivoLineaSinMovimiento, SoloBanco.Motivo);
        }

        [TestMethod]
        public void Test_ParejasManualesSeConservanYNoSeReutilizan()
        {
            ClsMovimientoLibro MovimientoManual = Libro(1, null, 100m, 1, 5);
            ClsLineaExtracto LineaManual = Banco(10, null, 100m, 1, 28);
            ClsDetalleConciliacion Manual = new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoCoincide,
                Metodo = ClsValoresConciliacion.MetodoManual,
                Movimiento = MovimientoManual,
                Linea = LineaManual
            };

            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { MovimientoManual, Libro(2, null, 100m, 1, 28) },
                new[] { LineaManual, Banco(11, null, 100m, 1, 27) },
                new[] { Manual });

            Assert.AreEqual(2, Resultado.Count);
            Assert.AreEqual(1, Resultado.Count(Detalle => Detalle.Metodo == ClsValoresConciliacion.MetodoManual));
            ClsDetalleConciliacion Automatica = Resultado.Single(Detalle => Detalle.Metodo == ClsValoresConciliacion.MetodoAutomatica);
            Assert.AreEqual(2, Automatica.Movimiento.IdMovimiento);
            Assert.AreEqual(11, Automatica.Linea.IdLinea);
        }

        [TestMethod]
        public void Test_LosPendientesSalenPrimeroEnElOrden()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 10m, 1, 1), Libro(2, "Z", 20m, 1, 9) },
                new[] { Banco(10, "A", 10m, 1, 1) },
                null);

            Assert.AreEqual(ClsValoresConciliacion.ResultadoPendiente, Resultado[0].Resultado);
            Assert.AreEqual(ClsValoresConciliacion.ResultadoCoincide, Resultado[1].Resultado);
        }

        [TestMethod]
        public void Test_ListasVaciasNoGeneranFilas()
        {
            List<ClsDetalleConciliacion> Resultado = ClsMotorConciliacion.BancosFuncEmparejar(null, null, null);

            Assert.AreEqual(0, Resultado.Count);
        }

        [TestMethod]
        public void Test_TotalesSeparanConciliadoYPorConciliar()
        {
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 100m, 1, 1), Libro(2, "B", 40m, -1, 2), Libro(3, "C", 70m, 1, 3) },
                new[] { Banco(10, "A", 100m, 1, 1), Banco(11, "B", 35m, -1, 2), Banco(12, "D", 12m, -1, 4) },
                null);

            ClsTotalesConciliacion Totales = ClsMotorConciliacion.BancosFuncCalcularTotales(Detalles);

            Assert.AreEqual(1, Totales.Coincidencias);
            Assert.AreEqual(1, Totales.Diferencias);
            Assert.AreEqual(2, Totales.Pendientes);
            Assert.AreEqual(0m, Totales.CargosConciliados);
            Assert.AreEqual(100m, Totales.AbonosConciliados);
            Assert.AreEqual(47m, Totales.CargosPorConciliar);
            Assert.AreEqual(70m, Totales.AbonosPorConciliar);
        }

        [TestMethod]
        public void Test_EstadoConciliadaSinPartidasAbiertasYSaldosIguales()
        {
            ClsTotalesConciliacion Totales = new ClsTotalesConciliacion { Coincidencias = 3 };

            Assert.AreEqual(ClsValoresConciliacion.EstadoConciliada, ClsMotorConciliacion.BancosFuncDeterminarEstado(Totales, 0m));
        }

        [TestMethod]
        public void Test_EstadoConDiferenciasSiHayPendientes()
        {
            ClsTotalesConciliacion Totales = new ClsTotalesConciliacion { Coincidencias = 3, Pendientes = 1 };

            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, ClsMotorConciliacion.BancosFuncDeterminarEstado(Totales, 0m));
        }

        [TestMethod]
        public void Test_EstadoConDiferenciasSiHayDiferenciaDeImporte()
        {
            ClsTotalesConciliacion Totales = new ClsTotalesConciliacion { Coincidencias = 3, Diferencias = 1 };

            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, ClsMotorConciliacion.BancosFuncDeterminarEstado(Totales, 0m));
        }

        [TestMethod]
        public void Test_EstadoConDiferenciasSiLosSaldosNoCuadran()
        {
            ClsTotalesConciliacion Totales = new ClsTotalesConciliacion { Coincidencias = 3 };

            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, ClsMotorConciliacion.BancosFuncDeterminarEstado(Totales, 0.01m));
        }

        [TestMethod]
        public void Test_MetodoAutomaticoSinParejasManuales()
        {
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 10m, 1, 1) }, new[] { Banco(10, "A", 10m, 1, 1) }, null);

            Assert.AreEqual(ClsValoresConciliacion.MetodoAutomatica, ClsMotorConciliacion.BancosFuncDeterminarMetodo(Detalles));
        }

        [TestMethod]
        public void Test_MetodoManualSoloConParejasManuales()
        {
            List<ClsDetalleConciliacion> Detalles = new List<ClsDetalleConciliacion>
            {
                new ClsDetalleConciliacion
                {
                    Resultado = ClsValoresConciliacion.ResultadoCoincide,
                    Metodo = ClsValoresConciliacion.MetodoManual,
                    Movimiento = Libro(1, null, 10m, 1, 1),
                    Linea = Banco(10, null, 10m, 1, 1)
                }
            };

            Assert.AreEqual(ClsValoresConciliacion.MetodoManual, ClsMotorConciliacion.BancosFuncDeterminarMetodo(Detalles));
        }

        [TestMethod]
        public void Test_MetodoMixtoConParejasManualesYAutomaticas()
        {
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 10m, 1, 1), Libro(2, "B", 20m, 1, 2) },
                new[] { Banco(10, "A", 10m, 1, 1), Banco(11, "B", 20m, 1, 2) },
                null);
            Detalles[0].Metodo = ClsValoresConciliacion.MetodoManual;

            Assert.AreEqual(ClsValoresConciliacion.MetodoMixta, ClsMotorConciliacion.BancosFuncDeterminarMetodo(Detalles));
        }

        [TestMethod]
        public void Test_TextoDeEstadoParaLaVentana()
        {
            Assert.AreEqual("Conciliada", ClsMotorConciliacion.BancosFuncTextoEstado(ClsValoresConciliacion.EstadoConciliada));
            Assert.AreEqual("Con diferencias", ClsMotorConciliacion.BancosFuncTextoEstado(ClsValoresConciliacion.EstadoConDiferencias));
            Assert.AreEqual("Cerrada", ClsMotorConciliacion.BancosFuncTextoEstado(ClsValoresConciliacion.EstadoCerrada));
            Assert.AreEqual("Abierta", ClsMotorConciliacion.BancosFuncTextoEstado(ClsValoresConciliacion.EstadoAbierta));
        }

        [TestMethod]
        public void Test_TablaMuestraImportesConSignoYOrigen()
        {
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 100m, -1, 1), Libro(2, "B", 60m, 1, 2) },
                new[] { Banco(10, "A", 100m, -1, 1) },
                null);

            DataTable Tabla = ClsTablaConciliacion.BancosFuncConstruir(Detalles, null);

            Assert.AreEqual(2, Tabla.Rows.Count);
            Assert.AreEqual("Pendiente", Tabla.Rows[0]["Estado"]);
            Assert.AreEqual("Solo libro", Tabla.Rows[0]["Origen"]);
            Assert.AreEqual(60m, Tabla.Rows[0]["ImporteLibro"]);
            Assert.AreEqual("Conciliado", Tabla.Rows[1]["Estado"]);
            Assert.AreEqual("Libro y banco", Tabla.Rows[1]["Origen"]);
            Assert.AreEqual(-100m, Tabla.Rows[1]["ImporteLibro"]);
            Assert.AreEqual(-100m, Tabla.Rows[1]["ImporteBanco"]);
            Assert.AreEqual("01/02/2026", Tabla.Rows[1]["Fecha"]);
            Assert.IsFalse(Tabla.Columns.Contains("Cuenta"));
        }

        [TestMethod]
        public void Test_TablaDelReporteIncluyeDatosDelEncabezado()
        {
            ClsConciliacion Encabezado = new ClsConciliacion
            {
                IdConciliacion = 5,
                FechaCorte = new DateTime(2026, 2, 28),
                Estado = ClsValoresConciliacion.EstadoConDiferencias,
                SaldoLibro = 900m,
                SaldoBanco = 950m,
                Diferencia = 50m,
                Banco = "Banco Industrial",
                NumeroCuenta = "001-123",
                NombreCuenta = "Operativa",
                Moneda = "GTQ",
                ReferenciaExtracto = "EXT-02",
                Responsable = "usuario"
            };
            List<ClsDetalleConciliacion> Detalles = ClsMotorConciliacion.BancosFuncEmparejar(
                new[] { Libro(1, "A", 100m, 1, 1) }, new ClsLineaExtracto[0], null);

            DataTable Tabla = ClsTablaConciliacion.BancosFuncConstruir(Detalles, Encabezado);

            Assert.AreEqual("001-123 - Operativa", Tabla.Rows[0]["Cuenta"]);
            Assert.AreEqual("28/02/2026", Tabla.Rows[0]["FechaCorte"]);
            Assert.AreEqual("Con diferencias", Tabla.Rows[0]["EstadoConciliacion"]);
            Assert.AreEqual(50m, Tabla.Rows[0]["DiferenciaSaldos"]);
        }
    }

    [TestClass]
    public class TestControladorConciliacion
    {
        private const string FechaValida = "28/02/2026";

        private static RepositorioFalso RepositorioBase()
        {
            return new RepositorioFalso();
        }

        [TestMethod]
        public void Test_ConstructorRechazaRepositorioNulo()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ClsControladorConciliacion(null));
        }

        [TestMethod]
        public void Test_ConsultarCuentasDevuelveLosRegistros()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            DataTable DatosEsperados = new DataTable();
            Repositorio.Cuentas = DatosEsperados;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarCuentas(out DataTable Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreSame(DatosEsperados, Resultado);
        }

        [TestMethod]
        public void Test_ConsultarCuentasDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.ErrorConsulta = new InvalidOperationException();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarCuentas(out DataTable Resultado);

            Assert.AreEqual("Error inesperado al consultar las cuentas.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ConsultarExtractosRechazaCuentaVacia()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetConsultarExtractos(0, out DataTable Resultado);

            Assert.AreEqual("Seleccione una cuenta bancaria.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ConsultarExtractosDevuelveLosRegistrosDeLaCuenta()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            DataTable DatosEsperados = new DataTable();
            Repositorio.Extractos = DatosEsperados;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarExtractos(7, out DataTable Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreSame(DatosEsperados, Resultado);
            Assert.AreEqual(7, Repositorio.CuentaConsultada);
        }

        [TestMethod]
        public void Test_ObtenerPeriodoExtractoDevuelveFechasYSaldoInicial()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetObtenerPeriodoExtracto(3, out DateTime Inicio, out DateTime Fin, out decimal SaldoInicial);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(new DateTime(2026, 2, 1), Inicio);
            Assert.AreEqual(new DateTime(2026, 2, 28), Fin);
            Assert.AreEqual(1000m, SaldoInicial);
        }

        [TestMethod]
        public void Test_ObtenerPeriodoExtractoRechazaExtractoInexistente()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Extracto = null;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetObtenerPeriodoExtracto(3, out DateTime Inicio, out DateTime Fin, out decimal SaldoInicial);

            Assert.AreEqual("No se encontró el extracto seleccionado.", Error);
        }

        [TestMethod]
        public void Test_CorteRechazaCuentaVacia()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(0, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Seleccione una cuenta bancaria.", Error);
            Assert.IsNull(Resultado);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_CorteRechazaExtractoVacio()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetEjecutarCorte(1, 0, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Seleccione un extracto bancario.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_CorteRechazaFechaInvalida()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, "31/02/2026", "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Fecha de corte no es válida. Use el formato dd/MM/aaaa.", Error);
            Assert.IsNull(Resultado);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_CorteRechazaFormatoDeFechaDistinto()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, "2026-02-28", "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Fecha de corte no es válida. Use el formato dd/MM/aaaa.", Error);
        }

        [TestMethod]
        public void Test_CorteRechazaExtractoDeOtraCuenta()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(99, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("El extracto no pertenece a la cuenta seleccionada.", Error);
            Assert.IsNull(Resultado);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_CorteRechazaFechaFueraDelPeriodoDelExtracto()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, "05/03/2026", "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("La fecha de corte debe estar dentro del período del extracto (01/02/2026 al 28/02/2026).", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_CorteRechazaConciliacionCerrada()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Existente = new ClsConciliacion
            {
                IdConciliacion = 8,
                IdCuenta = 1,
                IdExtracto = 3,
                FechaCorte = new DateTime(2026, 2, 28),
                Estado = ClsValoresConciliacion.EstadoCerrada
            };
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("La conciliación de este corte ya está cerrada y no se puede modificar.", Error);
            Assert.IsNull(Resultado);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_CorteRechazaConciliacionExistenteDeOtroExtracto()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Existente = new ClsConciliacion
            {
                IdConciliacion = 8,
                IdCuenta = 1,
                IdExtracto = 4,
                FechaCorte = new DateTime(2026, 2, 28),
                Estado = ClsValoresConciliacion.EstadoConDiferencias
            };
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Ya existe una conciliación de esta cuenta con esa fecha de corte para otro extracto.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_CorteConciliaCuandoTodoCoincideYLosSaldosCuadran()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Libro.Add(new ClsMovimientoLibro { IdMovimiento = 1, Referencia = "CH-1", Concepto = "Pago", Importe = 200m, Sentido = -1, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) });
            Repositorio.Lineas.Add(new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = "CH-1", Concepto = "Cheque", Importe = 200m, Sentido = -1, FechaOperacion = new DateTime(2026, 2, 6), FechaValor = new DateTime(2026, 2, 6) });
            Repositorio.NetoExtracto = -200m;
            Repositorio.SaldoLibro = 800m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConciliada, Resultado.Estado);
            Assert.AreEqual("Conciliada", Resultado.EstadoTexto);
            Assert.AreEqual(1000m, Resultado.SaldoInicialBanco);
            Assert.AreEqual(800m, Resultado.SaldoBanco);
            Assert.AreEqual(800m, Resultado.SaldoLibro);
            Assert.AreEqual(0m, Resultado.Diferencia);
            Assert.AreEqual(200m, Resultado.CargosConciliados);
            Assert.AreEqual(1, Resultado.Coincidencias);
            Assert.AreEqual(0, Resultado.Pendientes);
            Assert.AreEqual(1, Resultado.Tabla.Rows.Count);
            Assert.AreEqual(1, Repositorio.LlamadasGuardado);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConciliada, Repositorio.EncabezadoGuardado.Estado);
            Assert.AreEqual(ClsValoresConciliacion.MetodoAutomatica, Repositorio.EncabezadoGuardado.Metodo);
            Assert.AreEqual(new DateTime(2026, 2, 28), Repositorio.EncabezadoGuardado.FechaCorte);
            Assert.AreEqual("usuario", Repositorio.EncabezadoGuardado.Responsable);
            Assert.AreEqual(1, Repositorio.DetallesGuardados.Count);
        }

        [TestMethod]
        public void Test_CorteQuedaConDiferenciasSiHayPartidasPendientes()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Libro.Add(new ClsMovimientoLibro { IdMovimiento = 1, Referencia = "CH-9", Concepto = "Cheque en tránsito", Importe = 300m, Sentido = -1, FechaOperacion = new DateTime(2026, 2, 27), FechaValor = new DateTime(2026, 2, 27) });
            Repositorio.Lineas.Add(new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = "COM", Concepto = "Comisión", Importe = 25m, Sentido = -1, FechaOperacion = new DateTime(2026, 2, 20), FechaValor = new DateTime(2026, 2, 20) });
            Repositorio.NetoExtracto = -25m;
            Repositorio.SaldoLibro = 700m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, Resultado.Estado);
            Assert.AreEqual(2, Resultado.Pendientes);
            Assert.AreEqual(975m, Resultado.SaldoBanco);
            Assert.AreEqual(275m, Resultado.Diferencia);
            Assert.AreEqual(325m, Resultado.CargosPorConciliar);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, Repositorio.EncabezadoGuardado.Estado);
        }

        [TestMethod]
        public void Test_CorteConSaldosDistintosNoConciliaAunqueTodoCoincida()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.Libro.Add(new ClsMovimientoLibro { IdMovimiento = 1, Referencia = "D-1", Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) });
            Repositorio.Lineas.Add(new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = "D-1", Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) });
            Repositorio.NetoExtracto = 100m;
            Repositorio.SaldoLibro = 1050m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, Resultado.Estado);
            Assert.AreEqual(50m, Resultado.Diferencia);
        }

        [TestMethod]
        public void Test_CorteConservaLasParejasManualesDeLaConciliacionExistente()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            ClsMovimientoLibro MovimientoManual = new ClsMovimientoLibro { IdMovimiento = 1, Referencia = null, Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) };
            ClsLineaExtracto LineaManual = new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = null, Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 25), FechaValor = new DateTime(2026, 2, 25) };
            Repositorio.Existente = new ClsConciliacion
            {
                IdConciliacion = 8,
                IdCuenta = 1,
                IdExtracto = 3,
                FechaCorte = new DateTime(2026, 2, 28),
                Estado = ClsValoresConciliacion.EstadoConDiferencias
            };
            Repositorio.Detalles.Add(new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoCoincide,
                Metodo = ClsValoresConciliacion.MetodoManual,
                Movimiento = MovimientoManual,
                Linea = LineaManual
            });
            Repositorio.Libro.Add(MovimientoManual);
            Repositorio.Lineas.Add(LineaManual);
            Repositorio.NetoExtracto = 100m;
            Repositorio.SaldoLibro = 1100m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(8, Resultado.IdConciliacion);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConciliada, Resultado.Estado);
            Assert.AreEqual(ClsValoresConciliacion.MetodoManual, Repositorio.EncabezadoGuardado.Metodo);
            Assert.AreEqual(8, Repositorio.EncabezadoGuardado.IdConciliacion);
            Assert.AreEqual(8, Repositorio.ConciliacionUsadaEnCandidatos);
        }

        [TestMethod]
        public void Test_CorteUsaSistemaCuandoNoHayResponsable()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.SaldoLibro = 1000m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "  ", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("SISTEMA", Repositorio.EncabezadoGuardado.Responsable);
        }

        [TestMethod]
        public void Test_CorteDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.ErrorConsulta = new InvalidOperationException();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Error inesperado al realizar el corte de conciliación.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_CorteDevuelveElRechazoDelGuardado()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.ResultadoGuardado = "Error de base de datos al guardar la conciliación.";
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetEjecutarCorte(1, 3, FechaValida, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Error de base de datos al guardar la conciliación.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_DiferenciaDeMarcaCalculaBancoMenosLibro()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            Assert.AreEqual(-5m, Controlador.BancosFuncDiferenciaMarca(-100m, -95m));
            Assert.AreEqual(20m, Controlador.BancosFuncDiferenciaMarca(80m, 100m));
        }

        [TestMethod]
        public void Test_MismoSentidoReconoceCargosYAbonos()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            Assert.IsTrue(Controlador.BancosFuncMismoSentido(-100m, -95m));
            Assert.IsTrue(Controlador.BancosFuncMismoSentido(80m, 100m));
            Assert.IsFalse(Controlador.BancosFuncMismoSentido(-100m, 100m));
            Assert.IsFalse(Controlador.BancosFuncMismoSentido(0m, 100m));
        }

        [TestMethod]
        public void Test_SugeridaExigeMismoSentidoEImporteIgual()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            Assert.IsTrue(Controlador.BancosFuncEsSugerida(-250m, -250m));
            Assert.IsTrue(Controlador.BancosFuncEsSugerida(250m, 250m));
            Assert.IsFalse(Controlador.BancosFuncEsSugerida(-250m, 250m));
            Assert.IsFalse(Controlador.BancosFuncEsSugerida(250m, 249m));
        }

        [TestMethod]
        public void Test_MarcarRechazaSinConciliacion()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetMarcar(0, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Realice primero el corte de conciliación.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_MarcarRechazaSeleccionIncompleta()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetMarcar(8, 1, 0, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Seleccione un movimiento del libro y una línea del estado de cuenta.", Error);
        }

        [TestMethod]
        public void Test_MarcarRechazaPartidasQueNoEstanPendientes()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.Detalles.Clear();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Solo se pueden marcar partidas que estén pendientes.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_MarcarRechazaSentidosDistintos()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, -1);
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("El movimiento y la línea del estado de cuenta deben ser ambos cargos o ambos abonos.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_MarcarExigeConfirmarLaDiferenciaDeImporte()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 90m, 1, 1);
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Los importes no coinciden. Confirme la diferencia para marcar las partidas.", Error);
            Assert.IsNull(Resultado);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_MarcarImportesIgualesConciliaLaPareja()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.NetoExtracto = 100m;
            Repositorio.SaldoLibro = 1100m;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(1, Resultado.Coincidencias);
            Assert.AreEqual(0, Resultado.Pendientes);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConciliada, Resultado.Estado);
            Assert.AreEqual(ClsValoresConciliacion.MetodoManual, Repositorio.EncabezadoGuardado.Metodo);
            Assert.AreEqual("Marcado manualmente.", Repositorio.DetallesGuardados.Single().Motivo);
        }

        [TestMethod]
        public void Test_MarcarConDiferenciaAceptadaRegistraElMotivo()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 90m, 1, 1);
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, true, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(1, Resultado.Diferencias);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, Resultado.Estado);
            ClsDetalleConciliacion Guardado = Repositorio.DetallesGuardados.Single();
            Assert.AreEqual(ClsValoresConciliacion.ResultadoDiferencia, Guardado.Resultado);
            Assert.AreEqual(-10m, Guardado.Diferencia);
            Assert.AreEqual("Importe distinto, marcado manualmente.", Guardado.Motivo);
        }

        [TestMethod]
        public void Test_MarcarRechazaConciliacionCerrada()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.ConciliacionPorId.Estado = ClsValoresConciliacion.EstadoCerrada;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("La conciliación de este corte ya está cerrada y no se puede modificar.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_MarcarDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.ErrorConsulta = new InvalidOperationException();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetMarcar(8, 1, 10, false, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Error inesperado al marcar las partidas.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_DesmarcarRechazaSinConciliacion()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetDesmarcar(0, 1, 10, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Realice primero el corte de conciliación.", Error);
        }

        [TestMethod]
        public void Test_DesmarcarRechazaSeleccionIncompleta()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetDesmarcar(8, 0, 0, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Seleccione una pareja marcada manualmente para desmarcarla.", Error);
        }

        [TestMethod]
        public void Test_DesmarcarRechazaParejasAutomaticas()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.Detalles.Clear();
            Repositorio.Detalles.Add(new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoCoincide,
                Metodo = ClsValoresConciliacion.MetodoAutomatica,
                Movimiento = Repositorio.Libro[0],
                Linea = Repositorio.Lineas[0]
            });
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetDesmarcar(8, 1, 10, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual("Solo se pueden desmarcar las partidas que se marcaron manualmente.", Error);
            Assert.AreEqual(0, Repositorio.LlamadasGuardado);
        }

        [TestMethod]
        public void Test_DesmarcarSeparaLaParejaManual()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.Detalles.Clear();
            Repositorio.Detalles.Add(new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoCoincide,
                Metodo = ClsValoresConciliacion.MetodoManual,
                Movimiento = new ClsMovimientoLibro { IdMovimiento = 1, Referencia = "A", Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) },
                Linea = new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = "Z", Concepto = "Depósito", Importe = 100m, Sentido = 1, FechaOperacion = new DateTime(2026, 2, 25), FechaValor = new DateTime(2026, 2, 25) }
            });
            Repositorio.Libro[0].Referencia = "A";
            Repositorio.Lineas[0].Referencia = "Z";
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetDesmarcar(8, 1, 10, "usuario", out ClsResultadoConciliacion Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(2, Resultado.Pendientes);
            Assert.AreEqual(0, Resultado.Coincidencias);
            Assert.AreEqual(ClsValoresConciliacion.EstadoConDiferencias, Resultado.Estado);
        }

        [TestMethod]
        public void Test_ReporteRechazaConciliacionVacia()
        {
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(RepositorioBase());

            string Error = Controlador.BancosMetConsultarReporte(0, out DataTable Resultado);

            Assert.AreEqual("No hay una conciliación para reportar. Realice primero el corte.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ReporteRechazaConciliacionInexistente()
        {
            RepositorioFalso Repositorio = RepositorioBase();
            Repositorio.ConciliacionPorId = null;
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarReporte(8, out DataTable Resultado);

            Assert.AreEqual("La conciliación seleccionada ya no existe.", Error);
            Assert.IsNull(Resultado);
        }

        [TestMethod]
        public void Test_ReporteDevuelveLasPartidasConElEncabezado()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarReporte(8, out DataTable Resultado);

            Assert.AreEqual(string.Empty, Error);
            Assert.AreEqual(2, Resultado.Rows.Count);
            Assert.IsTrue(Resultado.Columns.Contains("Cuenta"));
            Assert.AreEqual("001-123 - Operativa", Resultado.Rows[0]["Cuenta"]);
        }

        [TestMethod]
        public void Test_ReporteDevuelveErrorSiRepositorioFalla()
        {
            RepositorioFalso Repositorio = RepositorioConPendientes(100m, 100m, 1, 1);
            Repositorio.ErrorConsulta = new InvalidOperationException();
            ClsControladorConciliacion Controlador = new ClsControladorConciliacion(Repositorio);

            string Error = Controlador.BancosMetConsultarReporte(8, out DataTable Resultado);

            Assert.AreEqual("Error inesperado al consultar el reporte.", Error);
            Assert.IsNull(Resultado);
        }

        private static RepositorioFalso RepositorioConPendientes(decimal ImporteLibro, decimal ImporteBanco, int SentidoLibro, int SentidoBanco)
        {
            RepositorioFalso Repositorio = new RepositorioFalso();
            ClsMovimientoLibro Movimiento = new ClsMovimientoLibro { IdMovimiento = 1, Referencia = "A", Concepto = "Depósito", Importe = ImporteLibro, Sentido = SentidoLibro, FechaOperacion = new DateTime(2026, 2, 5), FechaValor = new DateTime(2026, 2, 5) };
            ClsLineaExtracto Linea = new ClsLineaExtracto { IdLinea = 10, Identificador = "A", Referencia = "Z", Concepto = "Depósito", Importe = ImporteBanco, Sentido = SentidoBanco, FechaOperacion = new DateTime(2026, 2, 25), FechaValor = new DateTime(2026, 2, 25) };
            Repositorio.Libro.Add(Movimiento);
            Repositorio.Lineas.Add(Linea);
            Repositorio.ConciliacionPorId = new ClsConciliacion
            {
                IdConciliacion = 8,
                IdCuenta = 1,
                IdExtracto = 3,
                FechaCorte = new DateTime(2026, 2, 28),
                Estado = ClsValoresConciliacion.EstadoConDiferencias,
                Banco = "Banco Industrial",
                NumeroCuenta = "001-123",
                NombreCuenta = "Operativa",
                Moneda = "GTQ",
                ReferenciaExtracto = "EXT-02",
                Responsable = "usuario"
            };
            Repositorio.Detalles.Add(new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoPendiente,
                Metodo = ClsValoresConciliacion.MetodoAutomatica,
                Movimiento = Movimiento
            });
            Repositorio.Detalles.Add(new ClsDetalleConciliacion
            {
                Resultado = ClsValoresConciliacion.ResultadoPendiente,
                Metodo = ClsValoresConciliacion.MetodoAutomatica,
                Linea = Linea
            });
            return Repositorio;
        }
    }

    internal sealed class RepositorioFalso : IClsRepositorioConciliacion
    {
        public DataTable Cuentas { get; set; } = new DataTable();
        public DataTable Extractos { get; set; } = new DataTable();
        public ClsDatosExtracto Extracto { get; set; } = new ClsDatosExtracto
        {
            IdExtracto = 3,
            IdCuenta = 1,
            Referencia = "EXT-02",
            FechaInicio = new DateTime(2026, 2, 1),
            FechaFin = new DateTime(2026, 2, 28),
            SaldoInicial = 1000m,
            SaldoFinal = 1000m
        };
        public decimal SaldoLibro { get; set; } = 1000m;
        public decimal NetoExtracto { get; set; }
        public List<ClsMovimientoLibro> Libro { get; } = new List<ClsMovimientoLibro>();
        public List<ClsLineaExtracto> Lineas { get; } = new List<ClsLineaExtracto>();
        public ClsConciliacion Existente { get; set; }
        public ClsConciliacion ConciliacionPorId { get; set; }
        public List<ClsDetalleConciliacion> Detalles { get; } = new List<ClsDetalleConciliacion>();
        public Exception ErrorConsulta { get; set; }
        public string ResultadoGuardado { get; set; } = string.Empty;
        public long CuentaConsultada { get; private set; }
        public long ConciliacionUsadaEnCandidatos { get; private set; }
        public int LlamadasGuardado { get; private set; }
        public ClsConciliacion EncabezadoGuardado { get; private set; }
        public List<ClsDetalleConciliacion> DetallesGuardados { get; private set; }

        public DataTable BancosMetConsultarCuentas()
        {
            if (ErrorConsulta != null) throw ErrorConsulta;
            return Cuentas;
        }

        public DataTable BancosMetConsultarExtractos(long IdCuenta)
        {
            CuentaConsultada = IdCuenta;
            if (ErrorConsulta != null) throw ErrorConsulta;
            return Extractos;
        }

        public ClsDatosExtracto BancosMetObtenerExtracto(long IdExtracto)
        {
            if (ErrorConsulta != null) throw ErrorConsulta;
            return Extracto;
        }

        public decimal BancosMetObtenerSaldoLibro(long IdCuenta, DateTime FechaCorte)
        {
            return SaldoLibro;
        }

        public decimal BancosMetObtenerNetoExtracto(long IdExtracto, DateTime FechaCorte)
        {
            return NetoExtracto;
        }

        public List<ClsMovimientoLibro> BancosMetConsultarMovimientosLibro(long IdCuenta, DateTime FechaCorte, long IdConciliacion)
        {
            ConciliacionUsadaEnCandidatos = IdConciliacion;
            return Libro;
        }

        public List<ClsLineaExtracto> BancosMetConsultarLineasExtracto(long IdExtracto, DateTime FechaCorte, long IdConciliacion)
        {
            return Lineas;
        }

        public ClsConciliacion BancosMetBuscarConciliacion(long IdCuenta, DateTime FechaCorte)
        {
            if (ErrorConsulta != null) throw ErrorConsulta;
            return Existente;
        }

        public ClsConciliacion BancosMetObtenerConciliacion(long IdConciliacion)
        {
            if (ErrorConsulta != null) throw ErrorConsulta;
            return ConciliacionPorId;
        }

        public List<ClsDetalleConciliacion> BancosMetConsultarDetalles(long IdConciliacion)
        {
            return Detalles;
        }

        public string BancosMetGuardarConciliacion(ClsConciliacion Encabezado, List<ClsDetalleConciliacion> Detalles, out long IdConciliacion)
        {
            LlamadasGuardado++;
            EncabezadoGuardado = Encabezado;
            DetallesGuardados = Detalles;
            IdConciliacion = Encabezado.IdConciliacion > 0 ? Encabezado.IdConciliacion : 1;
            return ResultadoGuardado;
        }
    }
}
